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
    public delegate void SocketClosingHandle(string remoteAddress);

    /// <summary>
    /// Socke监听器
    /// </summary>
    public class SocketListener : IDisposable
    {
        private Socket _listenSocket;
        private SocketAsyncPool readWritePool;
        private readonly int _socketTimeout;
        private CacheListener _sessioinListen;
        private AutoResetEvent[] resetEvent;

        /// <summary>
        /// 连接关闭事件
        /// </summary>
        public event SocketClosingHandle OnClosing;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readWritePool"></param>
        /// <param name="connectTimeout">Socket连接超时（秒）</param>
        public SocketListener(SocketAsyncPool readWritePool, int connectTimeout)
        {
            this.readWritePool = readWritePool;
            _socketTimeout = connectTimeout;
            _sessioinListen = new CacheListener("__SOCKET_SESSION_POOL", 5, OnRemoveSession);

            resetEvent = new System.Threading.AutoResetEvent[1];
            resetEvent[0] = new System.Threading.AutoResetEvent(false);
        }

        private void OnRemoveSession(string key, object value, CacheItemRemovedReason reason)
        {
            try
            {
                if (reason == CacheItemRemovedReason.Expired)
                {
                    var list = SocketSessionPool.Current.ToList();
                    foreach (var socketArgs in list)
                    {
                        var session = socketArgs.UserToken as SocketSession;
                        if (session == null || session.AccessTime > DateTime.Now.AddSeconds(-_socketTimeout))
                        {
                            continue;
                        }
                        if (socketArgs.BytesTransferred == 0 && socketArgs.SocketError != SocketError.Success)
                        {
                            TraceLog.ReleaseWrite("Clear expired socket connection {0}", session.RemoteAddress);
                            session.OnClosed();
                            SocketSessionPool.Current.Remove(socketArgs);
                            readWritePool.ReleaseSAEAToPush(socketArgs);
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

        public void Start()
        {
            resetEvent[0].Set();

        }

        public void Stop()
        {
            resetEvent[0].Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public bool Listen(int port, int backlog)
        {
            string host = Environment.MachineName;
            return Listen(host, port, backlog);
        }

        /// <summary>
        /// 监听连接
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public bool Listen(string host, int port, int backlog)
        {
            IPHostEntry hostEntry = null;
            try
            {
                hostEntry = Dns.GetHostEntry(host);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Host:\"{0}\" {1}", host, ex.Message));
            }
            IPAddress[] addressList = hostEntry.AddressList;
            if (addressList.Length > 0)
            {
                IPAddress address = null;
                //优先使用IP4
                foreach (var ipAddress in addressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        address = ipAddress;
                        break;
                    }
                }
                if (address == null)
                {
                    address = addressList[addressList.Length - 1];
                }
                IPEndPoint localEndPoint = new IPEndPoint(address, port);
                return Listen(localEndPoint, backlog);
            }
            return false;
        }

        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="localEndPoint"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public bool Listen(IPEndPoint localEndPoint, int backlog)
        {
            Start();
            _sessioinListen.Start();
            _listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _listenSocket.ReceiveBufferSize = readWritePool.BufferSize;
                _listenSocket.SendBufferSize = readWritePool.BufferSize;

                if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    // Set dual-mode (IPv4 & IPv6) for the socket listener.
                    _listenSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                    _listenSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
                }
                else
                {
                    _listenSocket.Bind(localEndPoint);
                }

                // Start the server.
                _listenSocket.Listen(backlog);
                TraceLog.ReleaseWrite("The socket-server {0}:{1} is listenning", localEndPoint.Address, localEndPoint.Port);
                // Post accepts on the listening socket.);

                var acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += OnAcceptCompleted;
                StartAccept(acceptEventArg);
                return true;
            }
            catch (SocketException sex)
            {
                TraceLog.WriteError("The socket-server {0}:{1} listen error:{2}-{3}",
                                    localEndPoint.Address, localEndPoint.Port, sex.SocketErrorCode, sex);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("The socket-server {0}:{1} error:{2}",
                                    localEndPoint.Address, localEndPoint.Port, ex);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="successHandle"></param>
        public void NotifyAll(byte[] buffer, Action<SocketSession> successHandle)
        {
            Notify(m => true, buffer, successHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="math"></param>
        /// <param name="buffer"></param>
        /// <param name="successHandle"></param>
        public void Notify(Predicate<SocketSession> math, byte[] buffer, Action<SocketSession> successHandle)
        {
            Notify(math, buffer, successHandle, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="math"></param>
        /// <param name="buffer"></param>
        /// <param name="successHandle"></param>
        /// <param name="faildHandle"></param>
        /// <param name="errorHandle"></param>
        public void Notify(Predicate<SocketSession> math, byte[] buffer, Action<SocketSession> successHandle, Action<SocketSession> faildHandle, Action<Exception> errorHandle = null)
        {
            var clientList = SocketSessionPool.Current.ToList();
            foreach (var handle in clientList)
            {
                try
                {
                    if (handle == null) continue;

                    var session = handle.UserToken as SocketSession;
                    if (session != null && session.Connection != null && session.Connection.Connected && math(session))
                    {
                        if (handle.BytesTransferred > 0 && handle.SocketError == SocketError.Success)
                        {
                            session.PutInSendQueue(buffer);
                            //readWritePool.StartNewSend(handle);
                            session.StartSend();
                            if (successHandle != null)
                            {
                                successHandle(session);
                            }
                        }
                        else
                        {
                            session.OnClosed();
                            SocketSessionPool.Current.Remove(handle);
                            readWritePool.ReleaseSAEAToPush(handle);
                            if (faildHandle != null)
                            {
                                faildHandle(session);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (errorHandle != null)
                    {
                        errorHandle(ex);
                    }
                    TraceLog.WriteError("Socket notify:{0}", ex);
                }
            }
        }


        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            _sessioinListen.Stop();
            if (_listenSocket != null)
            {
                _listenSocket.Close();
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
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArg)
        {
            Socket clientSocket = acceptEventArg.AcceptSocket;
            if (!clientSocket.Connected)
            {
                return;
            }
            try
            {
                WaitHandle.WaitAll(resetEvent);
                resetEvent[0].Set();
                SocketAsyncEventArgs readEventArgs = readWritePool.Pop();
                if (readEventArgs != null)
                {
                    readEventArgs.AcceptSocket = acceptEventArg.AcceptSocket;
                    int revOffset = readEventArgs.Offset;
                    int sendOffset = revOffset + readWritePool.BufferSize;
                    SocketSession session = new SocketSession(readEventArgs, revOffset, sendOffset, readWritePool.BufferSize);
                    session.InitAsyncEventSend(readWritePool.Buffers);
                    session.OnClosing += OnClosing;
                    readEventArgs.UserToken = session;

#if DEBUG
                    Console.WriteLine("{0}>>connect:{1},saeapos:{2},offset:{3}", DateTime.Now.ToLongTimeString(),
                        readEventArgs.AcceptSocket.RemoteEndPoint, revOffset, sendOffset);
#endif
                    SocketSessionPool.Current.Add(readEventArgs);
                    readWritePool.StartReceive(readEventArgs);
                }
                else
                {
                    TraceLog.WriteError("There are no more available socket connection to allocate, lastsize:{0},usesize:{1},max:{2}", readWritePool.Capacity, readWritePool.UseCapacity, readWritePool.MaxCapacity);
                }
            }
            catch (SocketException ex)
            {
                SocketSession token = acceptEventArg.UserToken as SocketSession;
                TraceLog.WriteError("Listener Error when processing data received from {0}:\r\n{1}", token.Connection.RemoteEndPoint, ex.ToString());
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
                _sessioinListen = null;
                //清理托管对象
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
            if (_listenSocket != null)
            {
                try
                {
                    IDisposable disposable = _listenSocket;
                    disposable.Dispose();
                    _listenSocket = null;
                }
                catch (Exception)
                {
                }
            }
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
