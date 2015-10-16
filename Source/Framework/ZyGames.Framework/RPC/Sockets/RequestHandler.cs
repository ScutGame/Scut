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
using System.Net.Sockets;
using System.Text;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public RequestHandler(BaseMessageProcessor messageProcessor)
        {
            MessageProcessor = messageProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appServer"></param>
        internal virtual void Bind(ISocket appServer)
        {
            AppServer = appServer;
        }

        /// <summary>
        /// 
        /// </summary>
        public ISocket AppServer { get; private set; }


        /// <summary>
        /// websocket schema is wss, need use sub protocol
        /// </summary>
        public bool IsSecurity { get; set; }


        /// <summary>
        /// 
        /// </summary>
        internal protected BaseMessageProcessor MessageProcessor { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <param name="encoding"></param>
        /// <param name="callback"></param>
        public void SendMessage(ExSocket socket, string message, Encoding encoding, Action<SocketAsyncResult> callback)
        {
            byte[] buffer = encoding.GetBytes(message);
            SendMessage(socket, buffer, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="callback"></param>
        public void SendMessage(ExSocket socket, byte[] buffer, Action<SocketAsyncResult> callback)
        {
            AppServer.SendAsync(socket, buffer, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool SendHandshake(SocketAsyncEventArgs ioEventArgs)
        {
            //not handshake data
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <param name="messages"></param>
        /// <param name="hasHandshaked"></param>
        /// <returns></returns>
        public virtual bool TryReceiveMessage(SocketAsyncEventArgs ioEventArgs, out List<DataMeaage> messages, out bool hasHandshaked)
        {
            messages = new List<DataMeaage>();
            hasHandshaked = false;
            try
            {
                var dataToken = ioEventArgs.UserToken as DataToken;
                if (dataToken == null) return false;

                byte[] buffer = new byte[ioEventArgs.BytesTransferred];
                Buffer.BlockCopy(ioEventArgs.Buffer, dataToken.DataOffset, buffer, 0, buffer.Length);

                if (MessageProcessor != null)
                {
                    if (!MessageProcessor.TryReadMeaage(dataToken, buffer, out messages))
                    {
                        AppServer.Closing(ioEventArgs, OpCode.Close, "read data fail");
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("TryReceiveMessage error:{0}", ex);
            }
            return true;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        public virtual void SendCloseHandshake(ExSocket socket, sbyte opCode, string reason)
        {
            if (MessageProcessor != null)
            {
                byte[] data = MessageProcessor.CloseMessage(socket, opCode, reason);
                if (data != null) AppServer.SendAsync(socket, data, result => { });
            }
        }
    }
}
