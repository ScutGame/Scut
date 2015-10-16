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
using System.Text;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class PackageReader
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static byte[] EnterChar = Encoding.UTF8.GetBytes("\r\n\r\n");
        private static readonly string PrefixRouteChar = "route:";
        private static readonly string PrefixParamChar = "?d=";
        //private readonly static byte[] RouteHeadBytes = Encoding.UTF8.GetBytes(PrefixRouteChar);
        //private readonly static byte[] UrlHeadBytes = Encoding.UTF8.GetBytes(PrefixParamChar);
        private Dictionary<string, string> _params = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        private Encoding _encoding;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        public PackageReader(byte[] data, Encoding encoding)
        {
            _encoding = encoding;
            ParseData(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramString">decode data</param>
        /// <param name="inputStream"></param>
        /// <param name="encoding"></param>
        public PackageReader(string paramString, Stream inputStream, Encoding encoding)
        {
            _encoding = encoding;
            ParseData(paramString, inputStream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name]
        {
            get { return _params[name]; }
            set { _params[name] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RawParam { get; set; }

        /// <summary>
        /// Binary data, use '\r\n\r\n' split.
        /// </summary>
        public byte[] InputStream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Params
        {
            get { return _params; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsKey(string name)
        {
            return _params.ContainsKey(name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetParam(string name, out string value)
        {
            value = string.Empty;
            if (_params.ContainsKey(name))
            {
                value = this[name];
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetParam(string name, out int value)
        {
            value = 0;
            string str;
            if (TryGetParam(name, out str))
            {
                value = str.ToInt();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetParam(string name, out Guid value)
        {
            value = Guid.Empty;
            string str;
            if (TryGetParam(name, out str) && Guid.TryParse(str, out value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramString"></param>
        protected virtual void ParseParamString(string paramString)
        {
            string str = paramString;
            int index = paramString.IndexOf(PrefixParamChar, StringComparison.OrdinalIgnoreCase);
            if (index != -1)
            {
                if (paramString.StartsWith(PrefixRouteChar, StringComparison.OrdinalIgnoreCase))
                {
                    //ex: "route:xxx?d="
                    RouteName = paramString.Substring(PrefixRouteChar.Length, index - PrefixRouteChar.Length);
                }
                str = paramString.Substring(index + PrefixParamChar.Length);
                str = HttpUtility.UrlDecode(str) ?? "";

            }
            if (!str.Contains("="))
            {
                TraceLog.ReleaseWriteDebug("Parse request error:{0}", paramString);
                return;
            }

            var nvc = HttpUtility.ParseQueryString(str);
            foreach (var key in nvc.AllKeys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                var val = nvc[key];

                if (PrefixRouteChar.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                {
                    RouteName = val;
                    continue;
                }
                this[key] = val;
            }
        }

        private void ParseData(byte[] data)
        {
            var paramBytes = SplitBuffer(data);
            RawParam = _encoding.GetString(paramBytes);

            ParseParamString(RawParam);
        }

        private byte[] SplitBuffer(byte[] data)
        {
            int paramIndex = MathUtils.IndexOf(data, EnterChar);
            byte[] paramBytes;
            if (paramIndex >= 0)
            {
                paramBytes = new byte[paramIndex];
                Buffer.BlockCopy(data, 0, paramBytes, 0, paramBytes.Length);
                InputStream = new byte[data.Length - paramIndex - EnterChar.Length];
                Buffer.BlockCopy(data, paramIndex + EnterChar.Length, InputStream, 0, InputStream.Length);
            }
            else
            {
                paramBytes = data;
            }
            return paramBytes;
        }

        private void ParseData(string paramString, Stream inputStream)
        {
            if (!string.IsNullOrEmpty(paramString))
            {
                RawParam = paramString;
                ParseParamString(paramString);
            }
            byte[] data = new BufferReader(inputStream).Data;
            var paramBytes = SplitBuffer(data);
            string str = _encoding.GetString(paramBytes);
            if (!string.IsNullOrEmpty(str))
            {
                if (str.StartsWith("d=")) str = "?" + str;
                if (string.IsNullOrEmpty(RawParam)) RawParam = str;
                ParseParamString(str);
            }
        }

    }
}