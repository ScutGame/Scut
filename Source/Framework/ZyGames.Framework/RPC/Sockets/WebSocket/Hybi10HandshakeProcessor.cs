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
using System.Security.Cryptography;
using System.Text;

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// version http://tools.ietf.org/html/draft-ietf-hybi-thewebsocketprotocol-10
    /// </summary>
    public class Hybi10HandshakeProcessor : Hybi00HandshakeProcessor
    {
        private const string ServerKey = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        /// <summary>
        /// init
        /// </summary>
        /// <param name="version"></param>
        /// <param name="encoding"></param>
        public Hybi10HandshakeProcessor(int version, Encoding encoding)
            : base(version, encoding)
        {
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="encoding"></param>
        public Hybi10HandshakeProcessor(Encoding encoding)
            : base(8, encoding)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Hybi10HandshakeProcessor()
            : this(Encoding.UTF8)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string CreateHandshakeData(DataToken dataToken)
        {
            ClientWebSocket webSocket = Handler.AppServer as ClientWebSocket;
            if (webSocket == null)
            {
                throw new Exception("ISocket is not WebSocket client");
            }
            string host = webSocket.Settings.RemoteEndPoint.ToString();
            string urlPath = webSocket.Settings.UrlPath;
            string origin = webSocket.Settings.Origin;
            origin = !string.IsNullOrEmpty(origin) ? origin : host;
            string protocol = webSocket.Settings.Protocol;
            string extensions = webSocket.Settings.Extensions;
            string cookie = ToCookiesString(webSocket.Settings.Cookies);

            string secKey = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
            dataToken.Socket.Handshake.ParamItems[HandshakeHeadKeys.SecSignKey] = GenreateKey(secKey);
            dataToken.Socket.Handshake.UriSchema = webSocket.Settings.Scheme;
            dataToken.Socket.Handshake.Host = host;
            dataToken.Socket.Handshake.UrlPath = urlPath;
            dataToken.Socket.Handshake.Protocol = protocol;
            dataToken.Socket.Handshake.HttpVersion = HandshakeHeadKeys.HttpVersion;
            dataToken.Socket.Handshake.Method = HandshakeHeadKeys.Method;
            dataToken.Socket.Handshake.WebSocketVersion = _version;
            dataToken.Socket.Handshake.ParamItems[HandshakeHeadKeys.Origin] = origin;
            ParseCookies(dataToken.Socket.Handshake, cookie);

            StringBuilder result = new StringBuilder();
            result.AppendLine(string.Format("{0} {1} {2}", HandshakeHeadKeys.Method, urlPath, HandshakeHeadKeys.HttpVersion));
            result.AppendLine(HandshakeHeadKeys.RespUpgrade);
            result.AppendLine(HandshakeHeadKeys.RespConnection);
            result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.Host, host));
            result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.Origin, origin));
            result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.SecKey, secKey));
            if (!string.IsNullOrEmpty(protocol))
            {
                result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.SecProtocol, protocol));
            }
            if (!string.IsNullOrEmpty(extensions))
            {
                dataToken.Socket.Handshake.ParamItems[HandshakeHeadKeys.SecExtensions] = extensions;
                result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.SecExtensions, extensions));
            }
            result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.SecVersion, _version));
            if (!string.IsNullOrEmpty(cookie))
            {
                result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.Cookie, cookie));
            }

            result.AppendLine();
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handshakeData"></param>
        /// <returns></returns>
        protected override bool CheckSignKey(HandshakeData handshakeData)
        {
            if (handshakeData.WebSocketVersion < _version)
            {
                return base.CheckSignKey(handshakeData);
            }
            string signKey;
            string accecpKey;
            if (handshakeData.ParamItems.TryGet(HandshakeHeadKeys.SecAccept, out accecpKey) &&
                handshakeData.ParamItems.TryGet(HandshakeHeadKeys.SecSignKey, out signKey))
            {
                return string.Equals(signKey, accecpKey);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="handshakeData"></param>
        /// <returns></returns>
        protected override bool ResponseHandshake(ExSocket socket, HandshakeData handshakeData)
        {
            if (handshakeData.WebSocketVersion < _version)
            {
                return base.ResponseHandshake(socket, handshakeData);
            }

            string secKeyAccept = GenreateKey(handshakeData);
            StringBuilder response = new StringBuilder();
            response.AppendLine(HandshakeHeadKeys.RespHead_10);
            response.AppendLine(HandshakeHeadKeys.RespUpgrade);
            response.AppendLine(HandshakeHeadKeys.RespConnection);
            response.AppendLine(string.Format(HandshakeHeadKeys.RespAccept, secKeyAccept));

            if (!string.IsNullOrEmpty(handshakeData.Protocol))
            {
                response.AppendLine(string.Format(HandshakeHeadKeys.RespProtocol, handshakeData.Protocol));
            }
            response.AppendLine();
            Handler.SendMessage(socket, response.ToString(), Encoding, result => { });
            return true;
        }

        private string GenreateKey(HandshakeData handshakeData)
        {
            string key;
            if (handshakeData.ParamItems.TryGet(HandshakeHeadKeys.SecKey, out key))
            {
                return GenreateKey(key);
            }
            return string.Empty;
        }

        private static string GenreateKey(string key)
        {
            try
            {
                byte[] encryptionString = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(key + ServerKey));
                return Convert.ToBase64String(encryptionString);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
