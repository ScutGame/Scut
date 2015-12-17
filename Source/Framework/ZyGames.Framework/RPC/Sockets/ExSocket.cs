/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZyGames.Framework.RPC.Sockets.WebSocket;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Ex socket.
    /// </summary>
    public class ExSocket
    {
        private Socket socket;
        private IPEndPoint remoteEndPoint;
        private ConcurrentQueue<SocketAsyncResult> sendQueue;
        private int isInSending;
        internal DateTime LastAccessTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.RPC.Sockets.ExSocket"/> class.
        /// </summary>
        /// <param name="socket">Socket.</param>
        public ExSocket(Socket socket)
        {
            HashCode = Guid.NewGuid();
            sendQueue = new ConcurrentQueue<SocketAsyncResult>();
            this.socket = socket;
            InitData();
        }

        private void InitData()
        {
            try
            {
                remoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// HashCode
        /// </summary>
        public Guid HashCode { get; private set; }

        /// <summary>
        /// Is closed flag.
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Is connected of socket
        /// </summary>
        public bool Connected { get { return socket.Connected; } }

        /// <summary>
        /// Gets the work socket.
        /// </summary>
        /// <value>The work socket.</value>
        internal Socket WorkSocket { get { return socket; } }
        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>The remote end point.</value>
        public EndPoint RemoteEndPoint { get { return remoteEndPoint; } }
        /// <summary>
        /// Gets the length of the queue.
        /// </summary>
        /// <value>The length of the queue.</value>
        public int QueueLength { get { return sendQueue.Count; } }

        /// <summary>
        /// Web socket handshake data
        /// </summary>
        public HandshakeData Handshake { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsWebSocket { get { return Handshake != null; } }

        /// <summary>
        /// re-connection use.
        /// </summary>
        /// <param name="key"></param>
        public void Reset(Guid key)
        {
            HashCode = key;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            try
            {
                WorkSocket.Shutdown(SocketShutdown.Both);
            }
            catch { }
            WorkSocket.Close();
        }

        internal bool DirectSendOrEnqueue(byte[] data, Action<SocketAsyncResult> callback)
        {
            lock (socket)
            {
                sendQueue.Enqueue(new SocketAsyncResult(data) { Socket = this, ResultCallback = callback });
                return Interlocked.CompareExchange(ref isInSending, 1, 0) == 0;
            }
        }

        internal bool TryDequeueOrReset(out SocketAsyncResult result)
        {
            lock(socket)
            {
                if (sendQueue.TryDequeue(out result)) return true;
                else Interlocked.Exchange(ref isInSending, 0);
                return false;
            }
        }
        //internal bool TrySetSendFlag()
        //{
        //    return Interlocked.CompareExchange(ref isInSending, 1, 0) == 0;
        //}
        internal void ResetSendFlag()
        {
            Interlocked.Exchange(ref isInSending, 0);
        }
    }
}