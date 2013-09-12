using System;
using System.Collections.Concurrent;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 客户端连接
    /// </summary>
    public class SocketAsyncClient
    {
        private Socket _client;
        private AutoResetEvent[] _waitHandle;
        private SocketAsyncEventArgsProxy _saeaProxy;
        private BufferManager _bufferManager;
        private SocketAsyncPool _readWritePool;
        private IPEndPoint _remoteEndPoint;
        private const int OpsToPreAlloc = 2;
        private EndPoint _localEndPoint;
        private SocketAsyncEventArgs _saeaReceive;
        //private SocketSessionPool _sessionPool;

        /// <summary>
        /// 
        /// </summary>
        public SocketAsyncClient(IPEndPoint remoteEndPoint, int maxConnection, int bufferSize = 1024)
            : this(remoteEndPoint, maxConnection, bufferSize, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public SocketAsyncClient(IPEndPoint remoteEndPoint, int maxConnection, int bufferSize, AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            _remoteEndPoint = remoteEndPoint;
            _client = new Socket(addressFamily, socketType, protocolType);
            _waitHandle = new[] { new AutoResetEvent(false) };

            int numOfSaeaForRecSend = maxConnection * OpsToPreAlloc;
            _readWritePool = new SocketAsyncPool(numOfSaeaForRecSend);
            int numSize = numOfSaeaForRecSend * bufferSize;
            _bufferManager = new BufferManager(numSize, bufferSize);
            _saeaProxy = new SocketAsyncEventArgsProxy(bufferSize);
            _saeaProxy.ReceiveCompleted += OnReceiveCompleted;
            _saeaProxy.SendCompleted += OnSendCompleted;
            _saeaProxy.ClosedHandle += OnSocketClosing;

            _bufferManager.InitBuffer();
            SocketAsyncEventArgs saea;
            for (int i = 0; i < numOfSaeaForRecSend; i++)
            {
                saea = _saeaProxy.CreateNewSaea();
                _bufferManager.SetBuffer(saea);
                saea.UserToken = new SocketSession(saea.Offset);
                _readWritePool.Push(saea);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event Action<SocketAsyncEventArgs> ConnectCompleted;
        /// <summary>
        /// 
        /// </summary>
        public event Action<EndPoint> SocketClosing;
        /// <summary>
        /// 
        /// </summary>
        public event Action<string, int> SendCompleted;

        /// <summary>
        /// 
        /// </summary>
        public event Action<SocketAsyncClient, string, byte[]> ReceiveCompleted;

        private void OnReceiveCompleted(SocketSession token, byte[] data)
        {
            token.Refresh();
            if (ReceiveCompleted != null)
            {
                ReceiveCompleted(this, token.RemoteAddress, data);
            }
        }

        private void OnSocketClosing(SocketAsyncEventArgs e)
        {
            var session = e.UserToken as SocketSession;
            if (session != null)
            {
                session.Clear();
            }
            _readWritePool.Push(e);
            if (SocketClosing != null)
            {
                SocketClosing.BeginInvoke(e.AcceptSocket.RemoteEndPoint, null, null);
            }
            _saeaProxy.Disconnect(e);
        }

        /// <summary>
        /// 
        /// </summary>
        public string LocalAddress
        {
            get
            {
                return _localEndPoint.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress
        {
            get
            {
                return _remoteEndPoint.ToString();
            }
        }

        /// <summary>
        /// 算定义数据
        /// </summary>
        public object UserToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Connected { get { return _client.Connected; } }

        /// <summary>
        /// 
        /// </summary>
        public void WaitAll(int msTimeout = 0)
        {
            if (msTimeout == 0)
            {
                WaitHandle.WaitAll(_waitHandle);
            }
            else
            {
                WaitHandle.WaitAll(_waitHandle, msTimeout);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopWait()
        {
            _waitHandle[0].Set();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Connect(int msTimeout = 0)
        {
            ConnectAsync(AddressFamily.InterNetwork);
            WaitAll(msTimeout);
            return Connected;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ConnectAsync()
        {
            ConnectAsync(AddressFamily.InterNetwork);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressFamily"></param>
        public void ConnectAsync(AddressFamily addressFamily)
        {
            if (_saeaProxy.ReceiveCompleted == null)
            {
                throw new Exception("Not set ReceiveCompleted event.");
            }
            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            connectArgs.AcceptSocket = _client;
            connectArgs.RemoteEndPoint = _remoteEndPoint;
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);
            connectArgs.UserToken = _client;
            if (!_client.ConnectAsync(connectArgs))
            {

            }
        }

        private void OnConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success)
                {
                    _localEndPoint = _client.LocalEndPoint;
                    _saeaReceive = _readWritePool.Pop();
                    _saeaReceive.AcceptSocket = _client;
                    SocketSession token = (SocketSession)_saeaReceive.UserToken;
                    token.LocalEndPoint = _localEndPoint;
                    token.RemoteEndPoint = _remoteEndPoint;
                    token.Refresh();
                    _saeaProxy.StartReceive(_saeaReceive);

                    if (ConnectCompleted != null)
                    {
                        ConnectCompleted(e);
                    }
                    StopWait();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnConnectCompleted:{0}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public bool PushSend(byte[] data)
        {
            if (_client == null || !_client.Connected)
            {
                return false;
            }
            var e = _readWritePool.Pop();
            e.AcceptSocket = _client;
            SocketSession token = (SocketSession)e.UserToken;
            token.LocalEndPoint = _client.LocalEndPoint;
            token.RemoteEndPoint = _client.RemoteEndPoint;
            token.Refresh();
            return TryPushSend(e, data);
        }

        private bool TryPushSend(SocketAsyncEventArgs e, byte[] buffer)
        {
            var session = e.UserToken as SocketSession;
            session.SendPacket.InsertByteArray(buffer);
            if (e.SocketError == SocketError.Success)
            {
                _saeaProxy.DoStartSend(e);
                return true;
            }
            OnSocketClosing(e);
            return false;
        }

        private void OnSendCompleted(SocketAsyncEventArgs e)
        {
            if (SendCompleted != null)
            {
                string address = "";
                try
                {
                    address = e.AcceptSocket.RemoteEndPoint.ToString();
                }
                catch { }
                SendCompleted.BeginInvoke(address, e.BytesTransferred, null, null);
            }
            _readWritePool.Push(e);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            _bufferManager = null;
            _readWritePool.Dispose();
            _readWritePool = null;
            _saeaReceive.Dispose();
            _saeaReceive = null;
            _saeaProxy = null;
            try
            {
                _client.Shutdown(SocketShutdown.Both);
            }
            catch { }
            _client.Close();
            //清理托管对象
            GC.SuppressFinalize(this);
        }

    }
}
