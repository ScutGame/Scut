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
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// 
    /// </summary>
    public class HandshakeHeadKeys
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Method = "GET";

        /// <summary>
        /// 
        /// </summary>
        public const string HttpVersion = "HTTP/1.1";
        /// <summary>
        /// 
        /// </summary>
        public const string Host = "Host";
        /// <summary>
        /// 
        /// </summary>
        public const string Connection = "Connection";
        /// <summary>
        /// 
        /// </summary>
        public const string Cookie = "Cookie";
        /// <summary>
        /// 
        /// </summary>
        public const string Upgrade = "Upgrade";
        /// <summary>
        /// 
        /// </summary>
        public const string Origin = "Origin";

        /// <summary>
        /// 
        /// </summary>
        public const string SecSignKey = "Sec-WebSocket-SignKey";

        /// <summary>
        /// 
        /// </summary>
        public const string SecAccept = "Sec-WebSocket-Accept";
        /// <summary>
        /// 
        /// </summary>
        public const string SecKey = "Sec-WebSocket-Key";
        /// <summary>
        /// 
        /// </summary>
        public const string SecKey1 = "Sec-WebSocket-Key1";
        /// <summary>
        /// 
        /// </summary>
        public const string SecKey2 = "Sec-WebSocket-Key2";
        /// <summary>
        /// 
        /// </summary>
        public const string SecKey3 = "Sec-WebSocket-Key3";
        /// <summary>
        /// 
        /// </summary>
        public const string SecVersion = "Sec-WebSocket-Version";
        /// <summary>
        /// 
        /// </summary>
        public const string SecProtocol = "Sec-WebSocket-Protocol";
        /// <summary>
        /// 
        /// </summary>
        public const string SecExtensions = "Sec-WebSocket-Extensions";
        /// <summary>
        /// 
        /// </summary>
        public const string Protocol = "WebSocket-Protocol";
        /// <summary>
        /// 
        /// </summary>
        public const string RespHead_00 = "HTTP/1.1 101 WebSocket Protocol Handshake";
        /// <summary>
        /// 
        /// </summary>
        public const string RespHead_10 = "HTTP/1.1 101 Switching Protocols";
        /// <summary>
        /// 
        /// </summary>
        public const string RespUpgrade = Upgrade + ": websocket";
        /// <summary>
        /// 
        /// </summary>
        public const string RespUpgrade00 = Upgrade + ": WebSocket";
        /// <summary>
        /// 
        /// </summary>
        public const string RespConnection = Connection + ": Upgrade";
        /// <summary>
        /// 
        /// </summary>
        public const string RespOriginLine = "Sec-WebSocket-Origin: {0}";
        /// <summary>
        /// 
        /// </summary>
        public const string RespUrl = "{0}://{1}{2}";
        /// <summary>
        /// 
        /// </summary>
        public const string SecLocation = "Sec-WebSocket-Location: " + RespUrl;
        /// <summary>
        /// 
        /// </summary>
        public const string RespProtocol = SecProtocol + ": {0}";
        /// <summary>
        /// 
        /// </summary>
        public const string RespAccept = SecAccept + ": {0}";
    }
}
