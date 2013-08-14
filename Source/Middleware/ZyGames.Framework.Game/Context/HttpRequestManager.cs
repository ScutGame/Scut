using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Context
{
    public static class HttpRequestManager
    {
        private static int httpRequestTimeOut = 10000; // 10秒超时

        #region GetPostData Post数据并获取返回数据
        /// <summary>
        /// GetPostData Post数据并获取返回数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="MethodType">请求类型</param>
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
