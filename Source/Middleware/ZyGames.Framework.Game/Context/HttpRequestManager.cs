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
using System.Text;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Context
{
	/// <summary>
	/// Http request manager.
	/// </summary>
    public static class HttpRequestManager
    {
        #region GetPostData Post数据并获取返回数据
        /// <summary>
        /// GetPostData Post数据并获取返回数据
        /// </summary>
        /// <param name="url">url</param>
		/// <param name="methodType">请求类型</param>
        /// <returns></returns>
        private static string GetPostData(string url, string methodType)
        {
            string result = string.Empty;
            int againRequestCount = 3;
            for (int i = 0; i < againRequestCount; i++)
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = methodType;
                    WebResponse resp = req.GetResponse();
                    Stream stream = resp.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                    return result;
                }
                catch (Exception ex)
                {
                    new BaseLog().SaveLog(ex);
                }
            }
            return result;
        }
        #endregion


        /// <summary>
        /// Http Post 获取返回字符串
        /// </summary>
        /// <param name="url">请求地址URL</param>
        /// <param name="methodType">请求类型</param>
        /// <returns>返回字符串</returns>
        public static string PostStringData(string url, string methodType)
        {

            return GetPostData(url, methodType);
        }
        /// <summary>
        /// Http Get 获取返回字符串
        /// </summary>
        /// <param name="url">请求地址URL</param>
        /// <param name="methodType">请求类型</param>
        /// <returns>返回字符串</returns>
        public static string GetStringData(string url, string methodType)
        {

            return GetPostData(url, methodType);
        }

    }
}