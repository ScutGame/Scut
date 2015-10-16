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
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ISocket
    {
        /// <summary>
        /// not proccess buildpack
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="callback"></param>
        protected internal abstract Task<bool> SendAsync(ExSocket socket, byte[] buffer, Action<SocketAsyncResult> callback);

        /// <summary>
        /// has trigger CloseHandshake method
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        protected internal abstract void Closing(SocketAsyncEventArgs ioEventArgs, sbyte opCode, string reason);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public abstract Task PostSend(ExSocket socket, byte[] data, int offset, int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public abstract Task PostSend(ExSocket socket, sbyte opCode, byte[] data, int offset, int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public abstract Task PostSend(ExSocket socket, sbyte opCode, byte[] data, int offset, int count, Action<SocketAsyncResult> callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public abstract void Ping(ExSocket socket);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public abstract void Pong(ExSocket socket);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="reason"></param>
        public abstract void CloseHandshake(ExSocket socket, string reason);
    }
}
