using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketObject
    {
        private int isInSending;
        private ConcurrentQueue<byte[]> sendQueue;
        private Socket socket;
        private IPEndPoint localEndPoint;
        private IPEndPoint remoteEndPoint;
        internal DateTime LastAccessTime;
        internal SocketSessionPool sessionPool;
        private AutoResetEvent[] _waitHandle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="connection"></param>
        public SocketObject(Guid sessionId, Socket connection)
        {
            SessionId = sessionId;
            socket = connection;
            _waitHandle = new[] { new AutoResetEvent(false) };
            sendQueue = new ConcurrentQueue<byte[]>();
        }

        internal void Init()
        {
            remoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            localEndPoint = (IPEndPoint)socket.LocalEndPoint;
        }

        internal void Enqueue(byte[] data)
        {
            sendQueue.Enqueue(data);
        }
        internal bool TryDequeue(out byte[] result)
        {
            return sendQueue.TryDequeue(out result);
        }
        internal bool TrySetSendFlag()
        {
            return Interlocked.CompareExchange(ref isInSending, 1, 0) == 0;
        }
        internal void ResetSendFlag()
        {
            Interlocked.Exchange(ref isInSending, 0);
        }

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
        /// <returns></returns>
        public SocketObject GetSession(Guid sessionId)
        {
            return sessionPool.Find(sessionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Guid, SocketObject>> GetSessionAll()
        {
            return sessionPool.GetEnumerator();
        }


        /// <summary>
        /// 
        /// </summary>
        public Guid SessionId { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Socket Connection
        {
            get { return socket; }
        }
        /// <summary>
        /// 
        /// </summary>
        public EndPoint LocalEndPoint
        {
            get { return localEndPoint; }
        }
        /// <summary>
        /// 
        /// </summary>
        public EndPoint RemoteEndPoint
        {
            get { return remoteEndPoint; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(true);
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// 定义数据
        /// </summary>
        public object UserData { get; set; }
    }
}