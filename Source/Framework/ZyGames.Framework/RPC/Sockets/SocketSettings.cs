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

using System.Net;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Socket settings.
    /// </summary>
    public class SocketSettings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxConnections"></param>
        /// <param name="backlog"></param>
        /// <param name="maxAcceptOps"></param>
        /// <param name="bufferSize"></param>
        /// <param name="port"></param>
        public SocketSettings(int maxConnections, int backlog, int maxAcceptOps, int bufferSize, int port)
            : this(maxConnections, backlog, maxAcceptOps, bufferSize, new IPEndPoint(IPAddress.Any, port), 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.RPC.Sockets.SocketSettings"/> class.
        /// </summary>
        /// <param name="maxConnections">Max connections.</param>
        /// <param name="backlog">Backlog.</param>
        /// <param name="maxAcceptOps">Max accept ops.</param>
        /// <param name="bufferSize">Buffer size.</param>
        /// <param name="localEndPoint">Local end point.</param>
        /// <param name="expireInterval">Expire interval.</param>
        /// <param name="expireTime">Expire time.</param>
        public SocketSettings(int maxConnections, int backlog, int maxAcceptOps, int bufferSize, IPEndPoint localEndPoint, int expireInterval, int expireTime)
        {
            this.MaxConnections = maxConnections;
            this.NumOfSaeaForRecSend = 2 * maxConnections;
            this.Backlog = backlog;
            this.MaxAcceptOps = maxAcceptOps;
            this.BufferSize = bufferSize;
            this.LocalEndPoint = localEndPoint;
            this.ExpireInterval = expireInterval;
            this.ExpireTime = expireTime;
        }
        /// <summary>
        /// Gets the max connections.
        /// </summary>
        /// <value>The max connections.</value>
        public int MaxConnections { get; private set; }
        /// <summary>
        /// Gets the number of saea for rec send.
        /// </summary>
        /// <value>The number of saea for rec send.</value>
        public int NumOfSaeaForRecSend { get; private set; }
        /// <summary>
        /// Gets the backlog.
        /// </summary>
        /// <value>The backlog.</value>
        public int Backlog { get; private set; }
        /// <summary>
        /// Gets the max accept ops.
        /// </summary>
        /// <value>The max accept ops.</value>
        public int MaxAcceptOps { get; private set; }
        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        /// <value>The size of the buffer.</value>
        public int BufferSize { get; private set; }
        /// <summary>
        /// Gets the local end point.
        /// </summary>
        /// <value>The local end point.</value>
        public IPEndPoint LocalEndPoint { get; private set; }
        /// <summary>
        /// Gets the expire interval.
        /// </summary>
        /// <value>The expire interval.</value>
        public int ExpireInterval { get; private set; }
        /// <summary>
        /// Gets the expire time.
        /// </summary>
        /// <value>The expire time.</value>
        public int ExpireTime { get; private set; }
    }
}