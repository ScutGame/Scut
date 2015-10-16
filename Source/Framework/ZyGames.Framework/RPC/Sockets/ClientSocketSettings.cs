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

using System.Collections.Generic;
using System.Net;

namespace ZyGames.Framework.RPC.Sockets
{
	/// <summary>
	/// Client socket settings.
	/// </summary>
    public class ClientSocketSettings
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.RPC.Sockets.ClientSocketSettings"/> class.
		/// </summary>
		/// <param name="bufferSize">Buffer size.</param>
		/// <param name="remoteEndPoint">Remote end point.</param>
        public ClientSocketSettings(int bufferSize, IPEndPoint remoteEndPoint)
        {
            this.BufferSize = bufferSize;
            this.RemoteEndPoint = remoteEndPoint;
        }
		/// <summary>
		/// Gets the size of the buffer.
		/// </summary>
		/// <value>The size of the buffer.</value>
        public int BufferSize { get; private set; }
        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>The remote end point.</value>
		public IPEndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// 
        /// </summary>
	    public string UrlPath { get; set; }

	    /// <summary>
        /// 
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Extensions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 
        /// </summary>
	    public string SecWebSocketExtensions { get; set; }

	    /// <summary>
        /// Client cookies.
        /// </summary>
        public Dictionary<string, string> Cookies { get; set; }

    }
}