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

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// 
    /// </summary>
    public class WebSocketRequestHandler : RequestHandler
    {
        /// <summary>
        /// init
        /// </summary>
        public WebSocketRequestHandler(bool isSecurity = false, bool isMask = false)
            : this(new Rfc6455HandshakeProcessor(), new Rfc6455MessageProcessor() { IsMask = isMask })
        {
            IsSecurity = isSecurity;
        }


        /// <summary>
        /// init
        /// </summary>
        public WebSocketRequestHandler(BaseHandshakeProcessor handshake, BaseMessageProcessor messageProcessor)
            : base(messageProcessor)
        {
            Handshake = handshake;
        }


        internal override void Bind(ISocket appServer)
        {
            if (Handshake != null)
            {
                Handshake.Handler = this;
            }
            base.Bind(appServer);
        }

        /// <summary>
        /// 
        /// </summary>
        protected BaseHandshakeProcessor Handshake { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <returns></returns>
        public override bool SendHandshake(SocketAsyncEventArgs ioEventArgs)
        {
            try
            {
                var dataToken = ioEventArgs.UserToken as DataToken;
                if (dataToken == null) return false;
                if (Handshake != null)
                {
                    dataToken.Socket.Handshake = new HandshakeData() { IsClient = true};
                    HandshakeResult result = Handshake.Send(dataToken);
                    if (result == HandshakeResult.Close)
                    {
                        AppServer.Closing(ioEventArgs, OpCode.Close, "send handshake fial");
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SendHandshake error:{0}", ex);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <param name="messages"></param>
        /// <param name="hasHandshaked"></param>
        /// <returns>true: is continue receive</returns>
        public override bool TryReceiveMessage(SocketAsyncEventArgs ioEventArgs, out List<DataMeaage> messages, out bool hasHandshaked)
        {
            messages = new List<DataMeaage>();
            hasHandshaked = false;
            try
            {
                var dataToken = ioEventArgs.UserToken as DataToken;
                if (dataToken == null) return false;
                if (Handshake == null) return true;

                byte[] buffer = new byte[ioEventArgs.BytesTransferred];
                Buffer.BlockCopy(ioEventArgs.Buffer, dataToken.DataOffset, buffer, 0, buffer.Length);

                if (dataToken.Socket.Handshake == null)
                {
                    dataToken.Socket.Handshake = new HandshakeData();
                }
                if (!dataToken.Socket.Handshake.Handshaked)
                {
                    HandshakeResult result = Handshake.Receive(ioEventArgs, dataToken, buffer);
                    if (result == HandshakeResult.Success)
                    {
                        hasHandshaked = true;
                    }
                    else if (result == HandshakeResult.Close)
                    {
                        AppServer.Closing(ioEventArgs, OpCode.Close, "receive handshake fail");
                        return false;
                    }
                    return true;
                }
                if (MessageProcessor != null)
                {
                    MessageProcessor.TryReadMeaage(dataToken, buffer, out messages);
                }
                if (dataToken.HeadFrame != null && !dataToken.HeadFrame.CheckRSV)
                {
                    AppServer.Closing(ioEventArgs, OpCode.Close, "receive data RSV error");
                    return false;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("TryReceiveMessage error:{0}", ex);
            }
            return true;
        }

    }
}
