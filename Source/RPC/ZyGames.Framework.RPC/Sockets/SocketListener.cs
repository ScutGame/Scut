using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Caching;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socke监听器
    /// </summary>
    public class SocketListener : IDisposable
    {
        private SocketSettings _settings;
        private Socket _listenSocket;
        private SocketAsyncPool _readWritePool;
        private SocketAsyncPool _acceptPool;
        private SocketSessionPool _sessionPool;
        private CacheListener _sessioinListen;
        private BufferManager _bufferManager;
        private SocketAsyncEventArgsProxy _saeaProxy;
        private AutoResetEvent[] _resetEvent;

        /// <summary>
        /// 
        /// </summary>
        public SocketListener(SocketSettings settings)
        {
            _settings = settings;
            _sessioinListen = new CacheListener("__SOCKET_SESSION_POOL", 10, OnRemoveSession);
            _resetEvent = new AutoResetEvent[1];
            _resetEvent[0] = new AutoResetEvent(false);

            _sessionPool = new SocketSessionPool();
            _readWritePool = new SocketAsyncPool(_settings.NumOfSaeaForRecSend);
            _acceptPool = new SocketAsyncPool(_settings.MaxConnection);
            int numSize = settings.NumOfSaeaForRecSend * settings.BufferSize;
            _bufferManager = new BufferManager(numSize, settings.BufferSize);
            _saeaProxy = new SocketAsyncEventArgsProxy(settings.BufferSize);
            _saeaProxy.ReceiveCompleted += OnReceiveCompleted;
            _saeaProxy.SendCompleted += OnSendCompleted;
            _saeaProxy.ClosedHandle += OnSocketClosing;
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _settings.MaxConnection; i++)
            {
                _acceptPool.Push(CreateAcceptEventArgs());
            }
            _bufferManager.InitBuffer();
            SocketAsyncEventArgs saea;
            for (int i = 0; i < _settings.NumOfSaeaForRecSend; i++)
            {
                saea = _saeaProxy.CreateNewSaea();
                _bufferManager.SetBuffer(saea);
                saea.UserToken = new DataToken(saea.Offset);
                _readWritePool.Push(saea);
            }
        }

        private SocketAsyncEventArgs CreateAcceptEventArgs()
        {
            SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            return acceptEventArg;
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


        private void OnReceiveCompleted(SocketProcessEventArgs e)
        {
            if (ReceiveCompleted != null)
            {
                ReceiveCompleted(e);
            }
        }

        private void OnSendCompleted(SocketAsyncEventArgs e)
        {
            _readWritePool.Push(e);
            if (SendCompleted != null)
            {
                SendCompleted(new SocketProcessEventArgs() { Socket = ((DataToken)e.UserToken).Socket });
            }
        }

        private void OnSocketClosing(SocketAsyncEventArgs e)
        {
            var token = e.UserToken as DataToken;
            _sessionPool.Remove(token.Socket);
            _readWritePool.Push(e);
            if (SocketClosing != null)
            {
                SocketClosing.BeginInvoke(new SocketProcessEventArgs() { Socket = token.Socket }, null, null);
            }
            _saeaProxy.CloseConnect(e);
        }

        private void OnRemoveSession(string key, object value, CacheItemRemovedReason reason)
        {
            try
            {
                if (reason == CacheItemRemovedReason.Expired)
                {
                    var er = _sessionPool.GetEnumerator();
                    while (er.MoveNext())
                    {
                        try
                        {
                            var socketObject = er.Current.Value;
                            if (DateTime.Now.Subtract(socketObject.LastAccessTime).TotalSeconds > _settings.ContinuedTimeout)
                            {
                                socketObject.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            TraceLog.WriteError("SocketSessionPool disposed SAEA object error:{0}", e);
                        }
                    }
                    TraceLog.ReleaseWrite("Clear expired socket connection end...");
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SocketSessionPool expired error:{0}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SocketObject GetSession(Guid sessionId)
        {
            return _sessionPool.Find(sessionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Guid, SocketObject>> GetSessionAll()
        {
            return _sessionPool.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _resetEvent[0].Set();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            _resetEvent[0].Reset();
        }

        /// <summary>
        /// 监听
        /// </summary>
        /// <returns></returns>
        public bool Listen()
        {
            Start();
            _sessioinListen.Start();
            _listenSocket = new Socket(_settings.LocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _listenSocket.ReceiveBufferSize = _settings.BufferSize;
                _listenSocket.SendBufferSize = _settings.BufferSize;

                if (_settings.LocalEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    // Set dual-mode (IPv4 & IPv6) for the socket listener.
                    _listenSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                    _listenSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, _settings.LocalEndPoint.Port));
                }
                else
                {
                    _listenSocket.Bind(_settings.LocalEndPoint);
                }

                // Start the server.
                _listenSocket.Listen(_settings.Backlog);
                TraceLog.ReleaseWrite("The socket-server {0}:{1} is listenning", _settings.LocalEndPoint.Address, _settings.LocalEndPoint.Port);
                // Post accepts on the listening socket.);

                SocketAsyncEventArgs acceptEventArg;
                if (!_acceptPool.TryPop(out acceptEventArg))
                {
                    acceptEventArg = CreateAcceptEventArgs();
                }
                StartAccept(acceptEventArg);
                return true;
            }
            catch (SocketException sex)
            {
                TraceLog.WriteError("The socket-server {0}:{1} listen error:{2}-{3}",
                                    _settings.LocalEndPoint.Address, _settings.LocalEndPoint.Port, sex.SocketErrorCode, sex);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("The socket-server {0}:{1} error:{2}",
                                    _settings.LocalEndPoint.Address, _settings.LocalEndPoint.Port, ex);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckConnected(Guid sessionId)
        {
            SocketObject socketObject = _sessionPool.Find(sessionId);
            return socketObject != null && socketObject.Connection.Connected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketObject"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void PushSend(SocketObject socketObject, PacketData data)
        {
            PushSend(socketObject, data.ToByte());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketObject"></param>
        /// <param name="data"></param>
        public void PushSend(SocketObject socketObject, byte[] data)
        {
            var e = _readWritePool.Pop();
            e.AcceptSocket = socketObject.Connection;
            DataToken token = (DataToken)e.UserToken;
            token.Socket = socketObject;
            _saeaProxy.Send(e, data);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            _sessioinListen.Stop();
            if (_listenSocket != null)
            {
                try
                {
                    _listenSocket.Disconnect(true);
                }
                catch { }
            }
        }

        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (!_listenSocket.AcceptAsync(acceptEventArg))
            {
                ProcessAccept(acceptEventArg);
            }
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                ProcessAccept(e);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnAccept error:{0}", ex);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArg)
        {
            try
            {
                WaitHandle.WaitAll(_resetEvent);
                _resetEvent[0].Set();
                Socket clientSocket = acceptEventArg.AcceptSocket;
                if (clientSocket == null)
                {
                    acceptEventArg.AcceptSocket = null;
                    StartAccept(acceptEventArg);
                    return;
                }
                SocketAsyncEventArgs receiveEventArgs = _readWritePool.Pop();
                receiveEventArgs.AcceptSocket = acceptEventArg.AcceptSocket;
                DataToken token = (DataToken)receiveEventArgs.UserToken;
                SocketObject socketObject = new SocketObject(Guid.NewGuid(), acceptEventArg.AcceptSocket);
                socketObject.Init();
                socketObject.sessionPool = _sessionPool;
                socketObject.LastAccessTime = DateTime.Now;
                token.Socket = socketObject;

#if DEBUG
                Console.WriteLine("{0}>>{1} connect success", DateTime.Now.ToLongTimeString(), token.RemoteAddress);
#endif
                _sessionPool.Put(socketObject);
                _saeaProxy.StartReceive(receiveEventArgs);
                _acceptPool.Push(acceptEventArg);

                if (ConnectCompleted != null)
                {
                    ConnectCompleted.BeginInvoke(new SocketProcessEventArgs() { Socket = socketObject }, null, null);
                }
            }
            catch (SocketException ex)
            {
                DataToken token = acceptEventArg.UserToken as DataToken;
                TraceLog.WriteError("Listener Error when processing data received from {0}:\r\n{1}", acceptEventArg.RemoteEndPoint, ex);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Listener error:{0}", ex.ToString());
            }

            // Accept the next connection request.
            acceptEventArg.AcceptSocket = null;
            StartAccept(acceptEventArg);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            DoDispose(true);
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                _sessioinListen.Stop();
                _acceptPool.Dispose();
                _acceptPool = null;
                _bufferManager = null;
                _readWritePool.Dispose();
                _readWritePool = null;
                _sessionPool = null;
                _saeaProxy = null;
                _settings = null;
                if (_listenSocket != null)
                {
                    try
                    {
                        _listenSocket.Close();
                    }
                    catch { }
                }
                //清理托管对象
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }
        /// <summary>
        /// 
        /// </summary>
        ~SocketListener()
        {
            DoDispose(false);
        }
    }
}
