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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Ex socket.
    /// </summary>
    public class ExSocket
    {
        private Socket socket;
        private IPEndPoint remoteEndPoint;
        private ConcurrentQueue<byte[]> sendQueue;
        private int isInSending;
        internal DateTime LastAccessTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.RPC.Sockets.ExSocket"/> class.
        /// </summary>
        /// <param name="socket">Socket.</param>
        public ExSocket(Socket socket)
        {
            HashCode = Guid.NewGuid();
            this.socket = socket;
            this.remoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            sendQueue = new ConcurrentQueue<byte[]>();
        }

        /// <summary>
        /// HashCode
        /// </summary>
        public Guid HashCode { get; private set; }

        /// <summary>
        /// Gets the work socket.
        /// </summary>
        /// <value>The work socket.</value>
        public Socket WorkSocket { get { return socket; } }
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
    }
}