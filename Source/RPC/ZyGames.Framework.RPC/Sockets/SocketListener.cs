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
        private SocketSessionPool _sessionPool;
        private CacheListener _sessioinListen;
        private readonly AutoResetEvent[] _resetEvent;
        private SocketAsyncEventArgs _acceptEventArg;
        private BufferManager _bufferManager;
        private SocketAsyncEventArgsProxy _saeaProxy;

        /// <summary>
        /// 
        /// </summary>
        public SocketListener(SocketSettings settings)
        {
            _settings = settings;
            _sessioinListen = new CacheListener("__SOCKET_SESSION_POOL", 5, OnRemoveSession);
            _resetEvent = new AutoResetEvent[1];
            _resetEvent[0] = new AutoResetEvent(false);

            _sessionPool = new SocketSessionPool();
            _readWritePool = new SocketAsyncPool(_settings.NumOfSaeaForRecSend);
            int numSize = settings.NumOfSaeaForRecSend * settings.BufferSize;
            _bufferManager = new BufferManager(numSize, settings.BufferSize);
            _saeaProxy = new SocketAsyncEventArgsProxy(settings.BufferSize);
            _saeaProxy.ReceiveCompleted += OnReceiveCompleted;
            _saeaProxy.SendCompleted += OnSendCompleted;
            _saeaProxy.SendingHandle += OnSocketSending;
            _saeaProxy.ClosedHandle += e => OnSocketClosing(null, e);

            Init();
        }

        private void Init()
        {
            _bufferManager.InitBuffer();
            SocketAsyncEventArgs saea;
            for (int i = 0; i < _settings.NumOfSaeaForRecSend; i++)
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
        public event Action<object, EndPoint> SocketClosing;

        /// <summary>
        /// 
        /// </summary>
        public event Action<string, byte[]> ReceiveCompleted;
        /// <summary>
        /// 
        /// </summary>
        public event Action<string, byte[]> ReceiveTimeout;

        /// <summary>
        /// 
        /// </summary>
        public event Action<string, string, int, int> SocketSending;

        private void OnReceiveCompleted(SocketSession token, byte[] data)
        {
            if (_settings.EnableReceiveTimeout)
            {
                token.UserData = data;
                token.EnableTimeout(OnReceiveTimeout, token, _settings.ReceiveTimeout);
            }
            token.Refresh();
            if (ReceiveCompleted != null)
            {
                ReceiveCompleted(token.RemoteAddress, data);
            }
        }

        private void OnReceiveTimeout(object state)
        {
            try
            {
                SocketSession token = state as SocketSession;
                if (token != null)
                {
                    ReceiveTimeout.BeginInvoke(token.RemoteAddress, token.UserData as byte[], ReceiveTimeoutEnd, token);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnReceiveTimeout error:{0}", ex);
            }
        }

        private void ReceiveTimeoutEnd(IAsyncResult ar)
        {
            try
            {
                SocketSession token = ar as SocketSession;
                if (token != null && token.EnableTimeoutState)
                {
                    token.UserData = null;
                    token.CloseTimeout();
                }
            }
            catch { }
        }

        private void OnSocketSending(SocketAsyncEventArgs e)
        {
            if (SocketSending != null)
            {
                var session = e.UserToken as SocketSession;
                SocketSending.BeginInvoke(session.LocalAddress, session.RemoteAddress, session.SendPacket.MessageByteDone, session.SendPacket.RemainingByteCount, null, null);
            }
        }

        private void OnSendCompleted(SocketAsyncEventArgs e)
        {
            try
            {
                SocketSession token = e.UserToken as SocketSession;
                if (token != null && token.EnableTimeoutState)
                {
                    token.UserData = null;
                    token.CloseTimeout();
                }
            }
            catch { }
            _readWritePool.Push(e);
        }

        private void OnSocketClosing(object sender, SocketAsyncEventArgs e)
        {
            var session = e.UserToken as SocketSession;
            if (session != null)
            {
                session.Clear();
            }
            _sessionPool.Remove(e);
            _readWritePool.Push(e);
            if (SocketClosing != null)
            {
                try
                {
                    var remoteEndPoint = e.AcceptSocket.RemoteEndPoint;
                    SocketClosing.BeginInvoke(sender, remoteEndPoint, null, null);
                }
                catch { }
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
                            var socketArgs = er.Current.Value;
                            var session = socketArgs.UserToken as SocketSession;
                            if (session == null || session.AccessTime > DateTime.Now.AddSeconds(-_settings.ContinuedTimeout))
                            {
                                continue;
                            }
                            if (socketArgs.BytesTransferred == 0 && socketArgs.SocketError != SocketError.Success)
                            {
                                OnSocketClosing(null, socketArgs);
                                TraceLog.ReleaseWrite("Clear disposed socket connection {0}", session.RemoteAddress);
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

                _acceptEventArg = new SocketAsyncEventArgs();
                _acceptEventArg.Completed += OnAcceptCompleted;
                StartAccept(_acceptEventArg);
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
        public bool CheckConnected(string remoteAddress)
        {
            var client = _sessionPool.Find(remoteAddress);
            return client != null && client.AcceptSocket.Connected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteAddress"></param>
        /// <param name="buffer"></param>
        public bool PushSend(string remoteAddress, byte[] buffer)
        {
            SocketAsyncEventArgs saeaClient = _sessionPool.Find(remoteAddress);
            if (saeaClient == null ||
                saeaClient.AcceptSocket == null ||
                !saeaClient.AcceptSocket.Connected)
            {
                return false;
            }
            var e = _readWritePool.Pop();
            e.AcceptSocket = saeaClient.AcceptSocket;
            if (saeaClient.UserToken is SocketSession)
            {
                ((SocketSession)saeaClient.UserToken).Refresh();
            }
            SocketSession token = (SocketSession)e.UserToken;
            token.LocalEndPoint = saeaClient.AcceptSocket.LocalEndPoint;
            token.RemoteEndPoint = saeaClient.AcceptSocket.RemoteEndPoint;
            token.Refresh();

            return TryPushSend(e, buffer);
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
            //OnSocketClosing(buffer, e);
            return false;
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
                SocketSession token = (SocketSession)receiveEventArgs.UserToken;
                token.LocalEndPoint = acceptEventArg.AcceptSocket.LocalEndPoint;
                token.RemoteEndPoint = acceptEventArg.AcceptSocket.RemoteEndPoint;

#if DEBUG
                Console.WriteLine("{0}>>{1} connect success", DateTime.Now.ToLongTimeString(), token.RemoteAddress);
#endif
                _sessionPool.Put(receiveEventArgs);
                _saeaProxy.StartReceive(receiveEventArgs);

            }
            catch (SocketException ex)
            {
                SocketSession token = acceptEventArg.UserToken as SocketSession;
                TraceLog.WriteError("Listener Error when processing data received from {0}:\r\n{1}", token.RemoteAddress, ex);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Listener error:{0}", ex.ToString());
            }

            if (ConnectCompleted != null)
            {
                ConnectCompleted(acceptEventArg);
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
                _acceptEventArg = null;
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
