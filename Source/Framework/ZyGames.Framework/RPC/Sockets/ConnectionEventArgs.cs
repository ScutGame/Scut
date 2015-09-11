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
using ZyGames.Framework.RPC.Sockets.WebSocket;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// Connection event handler.
    /// </summary>
    public delegate void ConnectionEventHandler(ISocket socket, ConnectionEventArgs e);

    /// <summary>
    /// Connection event arguments.
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the socket.
        /// </summary>
        /// <value>The socket.</value>
        public ExSocket Socket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataMeaage Meaage { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data
        {
            get { return Meaage != null ? Meaage.Data : null; }
            set
            {
                if (Meaage == null)
                {
                    Meaage = new DataMeaage() { Data = value };
                }
                else
                {
                    Meaage.Data = value;
                }
            }
        }
    }
}