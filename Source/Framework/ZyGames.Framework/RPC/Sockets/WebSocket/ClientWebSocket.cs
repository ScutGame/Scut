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
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// WebSocket client
    /// </summary>
    public class ClientWebSocket : ClientSocket
    {
        static ClientSocketSettings Parse(string url, string[] protocol, string origin)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                if (("ws".Equals(uri.Scheme.ToLower(), StringComparison.Ordinal) ||
                     "wss".Equals(uri.Scheme.ToLower(), StringComparison.Ordinal)))
                {
                    if ("wss".Equals(uri.Scheme.ToLower(), StringComparison.Ordinal) &&
                        (protocol == null || protocol.Length == 0))
                    {
                        throw new ArgumentException("When using \"wss\" schema, it need use sub protocol.");
                    }
                    return new ClientSocketSettings(1024, new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port))
                    {
                        Scheme = uri.Scheme,
                        Protocol = protocol != null ? string.Join(",", protocol) : "",
                        Origin = origin,
                        UrlPath = uri.PathAndQuery
                    };
                }
                throw new ArgumentException("url scheme no support", url);
            }
            throw new ArgumentException("url parse error", url);
        }

        static WebSocketRequestHandler CreateHandler(int version)
        {
            if (version >= 13)
            {
                return new WebSocketRequestHandler(new Rfc6455HandshakeProcessor(version, Encoding.UTF8), new Rfc6455MessageProcessor());
            }
            if (version >= 8)
            {
                return new WebSocketRequestHandler(new Hybi10HandshakeProcessor(version, Encoding.UTF8), new Hybi10MessageProcessor());
            }
            return new WebSocketRequestHandler(new Hybi00HandshakeProcessor(version, Encoding.UTF8), new Hybi00MessageProcessor());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="protocol"></param>
        /// <param name="origin"></param>
        /// <param name="version"></param>
        public ClientWebSocket(string url, string[] protocol = null, string origin = "", int version = 8)
            : this(Parse(url, protocol, origin), CreateHandler(version))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSettings"></param>
        /// <param name="requestHandler"></param>
        protected ClientWebSocket(ClientSocketSettings clientSettings, RequestHandler requestHandler)
            : base(clientSettings, requestHandler)
        {
            DataReceived += ClientWebSocket_DataReceived;
        }

        void ClientWebSocket_DataReceived(ClientSocket sender, SocketEventArgs e)
        {
            try
            {
                if (e.Source.OpCode == OpCode.Ping)
                {
                    DoPing(e);
                }
                else if (e.Source.OpCode == OpCode.Pong)
                {
                    DoPong(e);
                }
                else
                {
                    TriggerMessage(e);
                }

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event SocketEventHandler OnOpened;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void TriggerOpened(SocketEventArgs e)
        {
            SocketEventHandler handler = OnOpened;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event Action<int> OnCloseStatus;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="closeStatus"></param>
        protected void TriggerCloseStatus(int closeStatus)
        {
            Action<int> handler = OnCloseStatus;
            if (handler != null) handler(closeStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        public event SocketEventHandler OnClosed;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void TriggerClosed(SocketEventArgs e)
        {
            SocketEventHandler handler = OnClosed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event SocketEventHandler OnMessage;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void TriggerMessage(SocketEventArgs e)
        {
            SocketEventHandler handler = OnMessage;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event SocketEventHandler OnError;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void TriggerError(SocketEventArgs e)
        {
            SocketEventHandler handler = OnError;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event SocketEventHandler OnPing;

        private void DoPing(SocketEventArgs e)
        {
            SocketEventHandler handler = OnPing;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event SocketEventHandler OnPong;

        private void DoPong(SocketEventArgs e)
        {
            SocketEventHandler handler = OnPong;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void DoOpened(SocketEventArgs e)
        {
            TriggerOpened(e);
            base.DoOpened(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void DoClosed(SocketEventArgs e)
        {
            TriggerClosed(e);
            base.DoClosed(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        protected override void OnClosedStatus(int statusCode)
        {
            TriggerCloseStatus(statusCode);
            base.OnClosedStatus(statusCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="reason"></param>
        public override void CloseHandshake(ExSocket socket, string reason)
        {
            requestHandler.SendCloseHandshake(socket, OpCode.Close, reason);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public async Task Send(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            await Send(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public async Task Send(byte[] data)
        {
            if (connectState != ConnectState.Success)
            {
                TriggerError(new SocketEventArgs() { Socket = Socket, Source = new DataMeaage() { Message = "send fail, connect:" + connectState } });
                return;
            }
            await PostSend(data, 0, data.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Ping()
        {
            Ping(Socket);
        }
    }
}
