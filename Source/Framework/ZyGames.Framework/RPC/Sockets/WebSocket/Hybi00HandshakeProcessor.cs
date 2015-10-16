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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// version http://tools.ietf.org/html/draft-ietf-hybi-thewebsocketprotocol-00
    /// </summary>
    public class Hybi00HandshakeProcessor : BaseHandshakeProcessor
    {
        private const byte spaceChar = 32;
        /// <summary>
        /// chars
        /// </summary>
        private static List<char> charList = new List<char>();
        /// <summary>
        /// numbers
        /// </summary>
        private static List<char> digList = new List<char>();

        static Hybi00HandshakeProcessor()
        {
            //ascii encode
            for (int i = 33; i <= 126; i++)
            {
                char c = (char)i;
                if (char.IsLetter(c))
                {
                    charList.Add(c);
                }
                else if (char.IsDigit(c))
                {
                    digList.Add(c);
                }
            }

        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="version"></param>
        /// <param name="encoding"></param>
        public Hybi00HandshakeProcessor(int version, Encoding encoding)
            : base(version, encoding)
        {
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="encoding"></param>
        public Hybi00HandshakeProcessor(Encoding encoding)
            : base(0, encoding)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToken"></param>
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
            string secKey1 = Encoding.ASCII.GetString(GenerateSecKey());
            string secKey2 = Encoding.ASCII.GetString(GenerateSecKey());
            byte[] secKey3 = GenerateSecKey(8);
            string protocol = webSocket.Settings.Protocol;
            string extensions = webSocket.Settings.Extensions;
            string cookie = ToCookiesString(webSocket.Settings.Cookies);

            dataToken.Socket.Handshake.ParamItems[HandshakeHeadKeys.SecSignKey] = GetResponseSecurityKey(secKey1, secKey2, secKey3);
            dataToken.Socket.Handshake.UriSchema = webSocket.Settings.Scheme;
            dataToken.Socket.Handshake.Host = host;
            dataToken.Socket.Handshake.UrlPath = urlPath;
            dataToken.Socket.Handshake.Protocol = protocol;
            dataToken.Socket.Handshake.HttpVersion = HandshakeHeadKeys.HttpVersion;
            dataToken.Socket.Handshake.Method = HandshakeHeadKeys.Method;
            dataToken.Socket.Handshake.WebSocketVersion = _version;
            dataToken.Socket.Handshake.ParamItems[HandshakeHeadKeys.Origin] = origin;
            dataToken.Socket.Handshake.ParamItems[HandshakeHeadKeys.SecKey1] = secKey1;
            dataToken.Socket.Handshake.ParamItems[HandshakeHeadKeys.SecKey2] = secKey2;
            ParseCookies(dataToken.Socket.Handshake, cookie);

            StringBuilder result = new StringBuilder();
            result.AppendLine(string.Format("{0} {1} {2}", HandshakeHeadKeys.Method, urlPath, HandshakeHeadKeys.HttpVersion));
            result.AppendLine(HandshakeHeadKeys.RespUpgrade00);
            result.AppendLine(HandshakeHeadKeys.RespConnection);
            result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.SecKey1, secKey1));
            result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.SecKey2, secKey2));
            result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.Host, host));
            result.AppendLine(string.Format("{0}: {1}", HandshakeHeadKeys.Origin, !string.IsNullOrEmpty(origin) ? origin : host));

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
            result.Append(Encoding.GetString(secKey3, 0, secKey3.Length));
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handshakeData"></param>
        /// <returns></returns>
        protected override bool CheckSignKey(HandshakeData handshakeData)
        {
            byte[] secAccept;
            byte[] signKey;
            if (handshakeData.ParamItems.TryGet(HandshakeHeadKeys.SecAccept, out secAccept) &&
                handshakeData.ParamItems.TryGet(HandshakeHeadKeys.SecSignKey, out signKey))
            {
                return secAccept.IndexOf(signKey) != -1;
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
            string secKey1;
            string secKey2;
            byte[] secKey3;
            if (handshakeData.ParamItems.TryGet(HandshakeHeadKeys.SecKey1, out secKey1) &&
                handshakeData.ParamItems.TryGet(HandshakeHeadKeys.SecKey2, out secKey2) &&
                handshakeData.ParamItems.TryGet(HandshakeHeadKeys.SecKey3, out secKey3))
            {
                //The minimum version support 
                StringBuilder response = new StringBuilder();
                response.AppendLine(HandshakeHeadKeys.RespHead_00);
                response.AppendLine(HandshakeHeadKeys.RespUpgrade00);
                response.AppendLine(HandshakeHeadKeys.RespConnection);
                string origin;
                if (handshakeData.ParamItems.TryGet(HandshakeHeadKeys.Origin, out origin))
                {
                    response.AppendLine(string.Format(HandshakeHeadKeys.RespOriginLine, origin));
                }
                response.AppendLine(string.Format(HandshakeHeadKeys.SecLocation, handshakeData.UriSchema, handshakeData.Host, handshakeData.UrlPath));
                if (!string.IsNullOrEmpty(handshakeData.Protocol))
                {
                    response.AppendLine(string.Format(HandshakeHeadKeys.RespProtocol, handshakeData.Protocol));
                }
                response.AppendLine();
                Handler.SendMessage(socket, response.ToString(), Encoding, result => { });
                //Encrypt message
                byte[] securityKey = GetResponseSecurityKey(secKey1, secKey2, secKey3);
                Handler.SendMessage(socket, securityKey,  result => { });

                return true;
            }
            return false;
        }

        private const string m_SecurityKeyRegex = "[^0-9]";
        private byte[] GetResponseSecurityKey(string secKey1, string secKey2, byte[] secKey3)
        {
            //Remove all symbols that are not numbers
            string k1 = Regex.Replace(secKey1, m_SecurityKeyRegex, String.Empty);
            string k2 = Regex.Replace(secKey2, m_SecurityKeyRegex, String.Empty);

            //Convert received string to 64 bit integer.
            Int64 intK1 = Int64.Parse(k1);
            Int64 intK2 = Int64.Parse(k2);

            //Dividing on number of spaces
            int k1Spaces = secKey1.Count(c => c == ' ');
            int k2Spaces = secKey2.Count(c => c == ' ');
            int k1FinalNum = (int)(intK1 / k1Spaces);
            int k2FinalNum = (int)(intK2 / k2Spaces);

            //Getting byte parts
            byte[] b1 = BitConverter.GetBytes(k1FinalNum).Reverse().ToArray();
            byte[] b2 = BitConverter.GetBytes(k2FinalNum).Reverse().ToArray();
            //byte[] b3 = Encoding.UTF8.GetBytes(secKey3);
            byte[] b3 = secKey3;

            //Concatenating everything into 1 byte array for hashing.
            byte[] bChallenge = new byte[b1.Length + b2.Length + b3.Length];
            Array.Copy(b1, 0, bChallenge, 0, b1.Length);
            Array.Copy(b2, 0, bChallenge, b1.Length, b2.Length);
            Array.Copy(b3, 0, bChallenge, b1.Length + b2.Length, b3.Length);

            //Hash and return
            byte[] hash = MD5.Create().ComputeHash(bChallenge);
            return hash;
        }


        private byte[] GenerateSecKey()
        {
            return GenerateSecKey(16);
        }

        private byte[] GenerateSecKey(int count)
        {

            int spaceNum = RandomUtils.GetRandom(1, count / 2 + 1);
            int charNum = RandomUtils.GetRandom(3, count - spaceNum);
            int digNum = count - spaceNum - charNum;
            byte[] array = new byte[count];
            int pos = 0;
            for (int i = 0; i < spaceNum; i++)
            {
                array[pos++] = spaceChar;
            }
            for (int j = 0; j < charNum; j++)
            {
                array[pos++] = (byte)charList[RandomUtils.GetRandom(0, charList.Count)];
            }
            for (int k = 0; k < digNum; k++)
            {
                array[pos++] = (byte)digList[RandomUtils.GetRandom(0, digList.Count)];
            }
            return array.RandomSort();
        }
    }
}
