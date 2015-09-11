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
    public class HandshakeItems : Dictionary<string, object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            object obj;
            if (TryGetValue(key, out obj))
            {
                value = (T)obj;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Web socket handshake data
    /// </summary>
    public class HandshakeData
    {
        /// <summary>
        /// init
        /// </summary>
        public HandshakeData()
        {
            UriSchema = "ws";
            ParamItems = new HandshakeItems();
        }

        ///// <summary>
        ///// more then sec-websocket-accept
        ///// </summary>
        //public object WebSocketSignKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsClient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Handshaked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            get { return string.Format(HandshakeHeadKeys.RespUrl, UriSchema, Host, UrlPath); }
        }

        /// <summary>
        /// ws or wss
        /// </summary>
        public string UriSchema { get; set; }

        /// <summary>
        /// Http request method, GET
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Http request url path
        /// </summary>
        public string UrlPath { get; set; }

        /// <summary>
        /// Client http verion, HTTP/1.1
        /// </summary>
        public string HttpVersion { get; set; }

        /// <summary>
        /// client host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Client WebSocket Version
        /// </summary>
        public int WebSocketVersion { get; set; }

        /// <summary>
        /// Client request param items.
        /// </summary>
        public HandshakeItems ParamItems { get; set; }

        /// <summary>
        /// Client cookies.
        /// </summary>
        public Dictionary<string, string> Cookies { get; set; }

        ///// <summary>
        ///// versioin 00 secKey3
        ///// </summary>
        //internal byte[] SecKey3 { get; set; }

        /// <summary>
        /// Sec protocol
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QueueString
        {
            get
            {
                int index = UrlPath.IndexOf('?');
                return index > -1 ? UrlPath.Substring(index) : UrlPath;
            }
        }
    }
}
