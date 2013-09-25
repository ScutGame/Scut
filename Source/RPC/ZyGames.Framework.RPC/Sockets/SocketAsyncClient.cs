using System;
using System.Collections.Concurrent;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 客户端连接
    /// </summary>
    public class SocketAsyncClient
    {
        private Socket _client;
        private SocketAsyncEventArgsProxy _saeaProxy;
        private BufferManager _bufferManager;
        private SocketAsyncPool _readWritePool;
        private IPEndPoint _remoteEndPoint;
        private const int OpsToPreAlloc = 2;
        private SocketAsyncEventArgs _saeaReceive;
        private SocketAsyncEventArgs _saeaSend;
        private SocketObject socketObject;

        /// <summary>
        /// 
        /// </summary>
        public SocketAsyncClient(IPEndPoint remoteEndPoint, int bufferSize = 1024)
            : this(remoteEndPoint, bufferSize, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public SocketAsyncClient(IPEndPoint remoteEndPoint, int bufferSize, AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            _remoteEndPoint = remoteEndPoint;
            _client = new Socket(addressFamily, socketType, protocolType);
            socketObject = new SocketObject(Guid.NewGuid(), _client);
            int maxConnection = 1;
            int numOfSaeaForRecSend = maxConnection * OpsToPreAlloc;
            _readWritePool = new SocketAsyncPool(numOfSaeaForRecSend);
            int numSize = numOfSaeaForRecSend * bufferSize;
            _bufferManager = new BufferManager(numSize, bufferSize);
            _bufferManager.InitBuffer();

            _saeaProxy = new SocketAsyncEventArgsProxy(bufferSize);
            _saeaProxy.ReceiveCompleted += OnReceiveCompleted;
            _saeaProxy.SendCompleted += OnSendCompleted;
            _saeaProxy.ClosedHandle += OnSocketClosing;

            SocketAsyncEventArgs saea;
            for (int i = 0; i < numOfSaeaForRecSend; i++)
            {
                saea = _saeaProxy.CreateNewSaea();
                _bufferManager.SetBuffer(saea);
                saea.UserToken = new DataToken(saea.Offset);
                _readWritePool.Push(saea);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event SocketProcessEvent ConnectCompleted;
        /// <summary>
        /// 
        /// </summary>
        public event SocketProcessEvent SocketClosing;
        /// <summary>
        /// 
        /// </summary>
        public event SocketProcessEvent SendCompleted;

        /// <summary>
        /// 
        /// </summary>
        public event SocketProcessEvent ReceiveCompleted;

        /// <summary>
        /// 
        /// </summary>
        public SocketObject Socket
        {
            get { return socketObject; }
        }

        private void OnReceiveCompleted(SocketProcessEventArgs e)
        {
            if (ReceiveCompleted != null)
            {
                ReceiveCompleted(e);
            }
        }

        private void OnSendCompleted(SocketAsyncEventArgs e)
        {
            //_readWritePool.Push(e);
            if (SendCompleted != null)
            {
                SendCompleted(new SocketProcessEventArgs() { Socket = ((DataToken)e.UserToken).Socket });
            }
        }

        private void OnSocketClosing(SocketAsyncEventArgs e)
        {
            var token = e.UserToken as DataToken;
            _readWritePool.Push(e);
            if (SocketClosing != null)
            {
                SocketClosing.BeginInvoke(new SocketProcessEventArgs() { Socket = token.Socket }, null, null);
            }
            _saeaProxy.Disconnect(e);

        }

        /// <summary>
        /// 
        /// </summary>
        public bool Connected
        {
            get
            {
                return _client.Connected;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Connect()
        {
            _client.Connect(_remoteEndPoint);
            StartReceive();
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
                    StartReceive();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnConnectCompleted:{0}", ex);
            }
        }

        private void StartReceive()
        {
            socketObject.Init();
            socketObject.LastAccessTime = DateTime.Now;
            if (ConnectCompleted != null)
            {
                ConnectCompleted.BeginInvoke(new SocketProcessEventArgs() { Socket = socketObject }, null, null);
            }
            _saeaReceive = _readWritePool.Pop();
            _saeaReceive.AcceptSocket = socketObject.Connection;
            ((DataToken)_saeaReceive.UserToken).Socket = socketObject;

            _saeaSend = _readWritePool.Pop();
            _saeaSend.AcceptSocket = socketObject.Connection;
            ((DataToken)_saeaSend.UserToken).Socket = socketObject;

            _saeaProxy.StartReceive(_saeaReceive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void PushSend(PacketData data)
        {
            PushSend(data.ToByte());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void PushSend(byte[] data)
        {
            PushSend(data, 0, data.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void PushSend(byte[] data, int offset, int count)
        {
            byte[] buffer = new byte[count];
            Buffer.BlockCopy(data, offset, buffer, 0, count);
            _saeaProxy.Send(_saeaSend, data);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            ((DataToken)_saeaReceive.UserToken).Socket.Close();
            _readWritePool.Dispose();
            _saeaReceive.Dispose();
            _saeaSend.Dispose();
            //清理托管对象
            GC.SuppressFinalize(this);
        }

    }
}
