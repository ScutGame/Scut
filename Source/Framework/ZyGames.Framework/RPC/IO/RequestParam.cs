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
using System.Collections;
using System.Text;
using System.Web;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class RequestParam
    {
        /// <summary>
        /// Get or set signKey
        /// </summary>
        public static string SignKey { get; set; }

        class myCultureComparer : IEqualityComparer
        {
            public CaseInsensitiveComparer myComparer;

            public myCultureComparer()
            {
                myComparer = CaseInsensitiveComparer.DefaultInvariant;
            }

            public new bool Equals(object x, object y)
            {
                return myComparer.Compare(x, y) == 0;
            }

            public int GetHashCode(object obj)
            {
                return obj.ToString().ToLower().GetHashCode();
            }
        }

        private Hashtable _params;
        private string _content;
        private Encoding _encoding;
        /// <summary>
        /// sign param name
        /// </summary>
        protected string signName = "sign";

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.RPC.IO.RequestParam"/> class.
        /// </summary>
        public RequestParam()
        {
            _params = new Hashtable(3, (float).8, new myCultureComparer());
            _encoding = Encoding.UTF8;
        }
        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get { return _content; }
        }
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _params.Count; }
        }
        /// <summary>
        /// Sets the chat set.
        /// </summary>
        /// <param name="encoding">Encoding.</param>
        public void SetChatSet(Encoding encoding)
        {
            _encoding = encoding;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                return _params.ContainsKey(name) ? _params[name] : null;
            }
            set
            {
                if (_params.ContainsKey(name))
                {
                    _params[name] = value;
                }
                else
                {
                    _params.Add(name, value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, object value)
        {
            this[name] = value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            _params.Remove(name);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _params.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contain(string name)
        {
            return _params.ContainsKey(name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Get(string name)
        {
            return this[name];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name)
        {
            return Get(name).ToNotNullString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetInt(string name)
        {
            return GetString(name).ToInt();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual void SetContent(string content)
        {
            _content = content;
            string[] splitresult = _content.Split(new char[] { '&' });
            foreach (string result in splitresult)
            {
                string[] equalsplitresult = result.Split(new char[] { '=' });
                if (equalsplitresult.Length != 2) continue;

                string key = equalsplitresult[0].ToLower();
                string value = HttpUtility.UrlDecode(equalsplitresult[1], _encoding);
                _params.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckSign()
        {
            StringBuilder sb = new StringBuilder();
            ArrayList keys = new ArrayList(_params.Keys);
            keys.Sort();
            string sign = "";
            foreach (string key in keys)
            {
                string value = GetString(key);
                if (!String.Equals(signName, key, StringComparison.OrdinalIgnoreCase))
                {
                    sb.Append(key + "=" + value + "&");
                }
                else
                {
                    sign = value;
                }
            }
            string checkSign = EncryptSign(sb.ToString());
            return Equals(sign, checkSign);
        }

        /// <summary>
        /// Tos the post string.
        /// </summary>
        /// <returns>The post string.</returns>
        public string ToPostString()
        {
            StringBuilder sb = new StringBuilder();
            ArrayList keys = new ArrayList(_params.Keys);
            keys.Sort();
            foreach (string key in keys)
            {
                string value = GetString(key);
                if (FilterSignValue(value)
                       && !String.Equals(signName, key, StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendFormat("{0}={1}&", key, HttpUtility.UrlEncode(value, _encoding));
                }
            }
            string paramStr = sb.ToString().TrimEnd('&');
            string sign = EncryptSign(paramStr);
            paramStr += string.Format("&{0}={1}", signName, sign);
            return paramStr;
        }

        /// <summary>
        /// 过滤签名参数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool FilterSignValue(string value)
        {
            return true;
        }


        /// <summary>
        /// 签名加密
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        protected virtual string EncryptSign(string sign)
        {
            return SignUtils.EncodeSign(sign, SignKey);
        }
    }
}