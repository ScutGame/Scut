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
using System.Text;
using System.Web;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 参数获取者
    /// </summary>
    public class ParamGeter : IDisposable
    {
        private const string SignName = "sign";
        private readonly string _signKey;
        private string _requestParam;
        private Dictionary<string, string> _paramSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="signKey"></param>
        public ParamGeter(string param, string signKey = "")
        {
            _signKey = signKey;
            _paramSet = new Dictionary<string, string>();
            InitData(param);
        }

        private void InitData(string param)
        {
            param = HttpUtility.UrlDecode(param, Encoding.UTF8) ?? "";
            int index = param.LastIndexOf(string.Format("&{0}=", SignName), StringComparison.OrdinalIgnoreCase);
            if (index != -1)
            {
                _requestParam = param.Substring(0, index);
            }
            string[] splitresult = param.Split(new char[] { '&' });
            foreach (string result in splitresult)
            {
                string[] equalsplitresult = result.Split(new char[] { '=' });
                if (equalsplitresult.Length == 2)
                {
                    string key = equalsplitresult[0].ToLower();
                    string value = HttpUtility.UrlDecode(equalsplitresult[1], Encoding.UTF8);
                    if (_paramSet.ContainsKey(key))
                    {
                        _paramSet[key] = value;
                    }
                    else
                    {
                        _paramSet.Add(key, value);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckSign()
        {
            if (_paramSet.ContainsKey(SignName))
            {
                if (_requestParam != null)
                {
                    string key = SignUtils.EncodeSign(_requestParam, _signKey);
                    if (!string.IsNullOrEmpty(key) && key == _paramSet[SignName])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            name = name.ToLower();
            return _paramSet.ContainsKey(name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name)
        {
            name = name.ToLower();
            if (_paramSet.ContainsKey(name))
            {
                return _paramSet[name];
            }
            return null;
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
        /// 释放
        /// </summary>
        public void Dispose()
        {
            DoDispose(true);
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                _paramSet = null;
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }
    }
}