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
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets.WebSocket
{
    /// <summary>
    /// 
    /// </summary>
    public enum HandshakeResult
    {
        /// <summary>
        /// 
        /// </summary>
        Wait,
        /// <summary>
        /// 
        /// </summary>
        Success,
        /// <summary>
        /// 
        /// </summary>
        Close,
    }

    /// <summary>
    /// Handshake processor
    /// min version http://tools.ietf.org/html/draft-ietf-hybi-thewebsocketprotocol-00
    /// </summary>
    public abstract class BaseHandshakeProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly int _version;
        private static readonly byte[] HandshakeEndBytes = Encoding.UTF8.GetBytes("\r\n\r\n");

        /// <summary>
        /// init
        /// </summary>
        /// <param name="version"></param>
        /// <param name="encoding"></param>
        protected BaseHandshakeProcessor(int version, Encoding encoding)
        {
            _version = version;
            Encoding = encoding;
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected RequestHandler Handler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Encoding Encoding { get; set; }

        internal HandshakeResult Send(DataToken dataToken)
        {
            string handshakeData = CreateHandshakeData(dataToken);
            try
            {
                Handler.SendMessage(dataToken.Socket, handshakeData, Encoding, result => { });
                return HandshakeResult.Success;
            }
            catch (Exception)
            {
                return HandshakeResult.Close;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract string CreateHandshakeData(DataToken dataToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handshakeData"></param>
        /// <returns></returns>
        protected abstract bool CheckSignKey(HandshakeData handshakeData);

        /// <summary>
        /// Receive handshake
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <param name="dataToken"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal HandshakeResult Receive(SocketAsyncEventArgs ioEventArgs, DataToken dataToken, byte[] data)
        {
            if (dataToken.byteArrayForHandshake == null)
            {
                dataToken.byteArrayForHandshake = new List<byte>(data);
            }
            else
            {
                dataToken.byteArrayForHandshake.AddRange(data);
            }
            var buffer = dataToken.byteArrayForHandshake.ToArray();
            int headLength = MathUtils.IndexOf(buffer, HandshakeEndBytes);
            if (headLength < 0)
            {
                //data not complate, wait receive
                return HandshakeResult.Wait;
            }
            headLength += HandshakeEndBytes.Length;
            byte[] headBytes = new byte[headLength];
            Buffer.BlockCopy(buffer, 0, headBytes, 0, headBytes.Length);

            string message = Encoding.GetString(headBytes);
            HandshakeData handshakeData = dataToken.Socket.Handshake;
            string error;
            if (TryParseHandshake(message, handshakeData, out error))
            {
                if (handshakeData.ParamItems.ContainsKey(HandshakeHeadKeys.SecKey1) &&
                    handshakeData.ParamItems.ContainsKey(HandshakeHeadKeys.SecKey2))
                {
                    int remainBytesNum = buffer.Length - headLength;
                    if (!handshakeData.IsClient && remainBytesNum == 8)
                    {
                        byte[] secKey3Bytes = new byte[remainBytesNum];
                        Buffer.BlockCopy(buffer, headBytes.Length, secKey3Bytes, 0, secKey3Bytes.Length);
                        handshakeData.ParamItems[HandshakeHeadKeys.SecKey3] = secKey3Bytes;
                    }
                    else if (handshakeData.IsClient && remainBytesNum == 16)
                    {
                        byte[] secKey3Bytes = new byte[remainBytesNum];
                        Buffer.BlockCopy(buffer, headBytes.Length, secKey3Bytes, 0, secKey3Bytes.Length);
                        handshakeData.ParamItems[HandshakeHeadKeys.SecAccept] = secKey3Bytes;
                    }
                    else
                    {
                        //data not complate, wait receive
                        return HandshakeResult.Wait;
                    }
                }
                if (!handshakeData.IsClient)
                {
                    bool result = ResponseHandshake(dataToken.Socket, handshakeData);
                    if (!result)
                    {
                        TraceLog.ReleaseWriteDebug("Client {0} handshake fail, message:\r\n{2}", dataToken.Socket.RemoteEndPoint, message);
                        return HandshakeResult.Close;
                    }
                    dataToken.byteArrayForHandshake = null;
                    dataToken.Socket.Handshake.Handshaked = true;
                    return HandshakeResult.Success;
                }
                if (CheckSignKey(handshakeData))
                {
                    dataToken.byteArrayForHandshake = null;
                    dataToken.Socket.Handshake.Handshaked = true;
                    return HandshakeResult.Success;
                }
                return HandshakeResult.Close;
            }
            TraceLog.WriteWarn("Client {0} handshake {1}error, detail\r\n{2}", dataToken.Socket.RemoteEndPoint, error, message);
            return HandshakeResult.Close;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handshakeData"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        protected virtual bool TryParseHandshake(string message, HandshakeData handshakeData, out string error)
        {
            using (var reader = new StringReader(message))
            {
                error = string.Empty;
                string headData = reader.ReadLine() ?? "";
                var headParams = headData.Split(' ');
                if (headParams.Length < 3 ||
                    (headParams[0] != HandshakeHeadKeys.Method && headParams[0] != HandshakeHeadKeys.HttpVersion))
                {
                    return false;
                }
                bool isResponse = headParams[0] == HandshakeHeadKeys.HttpVersion;
                if (!isResponse)
                {
                    if (Handler.IsSecurity) handshakeData.UriSchema = "wss";
                    handshakeData.Method = headParams[0];
                    handshakeData.UrlPath = headParams[1];
                    handshakeData.HttpVersion = headParams[2];
                }

                string paramStr;
                while (!string.IsNullOrEmpty(paramStr = reader.ReadLine()))
                {
                    //ex: Upgrade: WebSocket
                    var paramArr = paramStr.Split(':');
                    if (paramArr.Length < 2)
                    {
                        continue;
                    }
                    string key = paramArr[0].Trim();
                    //value includ spance char
                    string value = string.Join("", paramArr, 1, paramArr.Length - 1);
                    if (value.Length > 1) value = value.Substring(1); //pre char is spance
                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }
                    if (string.Equals(HandshakeHeadKeys.Host, key))
                    {
                        handshakeData.Host = value;
                        continue;
                    }
                    if (string.Equals(HandshakeHeadKeys.SecVersion, key))
                    {
                        handshakeData.WebSocketVersion = value.ToInt();
                        continue;
                    }
                    if (string.Equals(HandshakeHeadKeys.SecProtocol, key))
                    {
                        handshakeData.Protocol = GetFirstProtocol(value);
                        continue;
                    }
                    if (string.Equals(HandshakeHeadKeys.Cookie, key))
                    {
                        ParseCookies(handshakeData, value);
                        continue;
                    }
                    handshakeData.ParamItems[key] = value;
                }

                if (handshakeData.ParamItems.ContainsKey(HandshakeHeadKeys.Upgrade) &&
                    handshakeData.ParamItems.ContainsKey(HandshakeHeadKeys.Connection))
                {
                    return true;
                }
                error = "not support websocket ";
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handshake"></param>
        /// <param name="cookieStr"></param>
        /// <returns></returns>
        protected void ParseCookies(HandshakeData handshake, string cookieStr)
        {
            if (handshake == null) return;
            if (handshake.Cookies == null)
            {
                handshake.Cookies = new Dictionary<string, string>();
            }
            var array = cookieStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in array)
            {
                var kvs = item.Split('=');
                if (kvs.Length == 2 && !string.IsNullOrEmpty(kvs[0]))
                {
                    handshake.Cookies[Uri.UnescapeDataString(kvs[0])] = Uri.UnescapeDataString(kvs[1].Trim());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        protected string ToCookiesString(Dictionary<string, string> cookies)
        {
            if (cookies == null) return string.Empty;
            StringBuilder result = new StringBuilder();
            foreach (var cookie in cookies)
            {
                result.AppendFormat("{0}={1};", Uri.EscapeDataString(cookie.Key), Uri.EscapeDataString(cookie.Value));
            }
            return result.ToString().TrimEnd(';');
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="handshakeData"></param>
        protected abstract bool ResponseHandshake(ExSocket socket, HandshakeData handshakeData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        protected virtual string GetFirstProtocol(string protocol)
        {
            if (string.IsNullOrEmpty(protocol))
            {
                return string.Empty;
            }
            var arrNames = protocol.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return arrNames.Length > 0 ? arrNames[0] : string.Empty;
        }

    }
}
