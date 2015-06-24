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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZyGames.Framework.Common
{
    /// <summary>
    /// Http mothod
    /// </summary>
    public abstract class HttpUtils
    {
        /// <summary>
        /// The content type xml.
        /// </summary>
        public const string XmlContentType = "application/xml";
        /// <summary>
        /// stream
        /// </summary>
        public const string StreamContentType = "application/octet-stream";
        /// <summary>
        /// Get request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string GetString(string url, Encoding encoding, string contentType = "")
        {
            using (var resp = Get(url, contentType, null, string.Empty, null))
            using (var sr = new StreamReader(resp.GetResponseStream(), encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Get request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Stream Get(string url, string contentType = "")
        {
            using (var resp = Get(url, contentType, null, string.Empty, null))
            {
                return resp.GetResponseStream();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string url, Encoding encoding, string contentType = "")
        {
            using (var resp = await GetAsync(url, contentType, null, string.Empty, null))
            using (var sr = new StreamReader(resp.GetResponseStream(), encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        /// <param name="userAgent"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static Task<WebResponse> GetAsync(string url, string contentType, int? timeout, string userAgent, CookieCollection cookies)
        {
            var request = DoGetRequest(url, contentType, timeout, userAgent, cookies);
            return request.GetResponseAsync();
        }

        /// <summary>  
        /// Get request  
        /// </summary>  
        /// <param name="url">URL</param>
        /// <param name="contentType"></param>
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse Get(string url, string contentType, int? timeout, string userAgent, CookieCollection cookies)
        {
            var request = DoGetRequest(url, contentType, timeout, userAgent, cookies);
            return request.GetResponse() as HttpWebResponse;
        }

        private static HttpWebRequest DoGetRequest(string url, string contentType, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            if (!string.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request;
        }

        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string PostString(string url, string parameters, Encoding encoding, string contentType = "")
        {
            using (var resp = Post(url, parameters, null, string.Empty, encoding, contentType, null))
            using (var sr = new StreamReader(resp.GetResponseStream(), encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<string> PostStringAsync(string url, string parameters, Encoding encoding, string contentType = "")
        {
            using (var resp = await PostAsync(url, parameters, null, String.Empty, encoding, contentType, null))
            using (var sr = new StreamReader(resp.GetResponseStream(), encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string BuildPostParams(IDictionary<string, string> parameters)
        {
            StringBuilder buffer = new StringBuilder();
            string val;
            foreach (string key in parameters.Keys)
            {
                val = HttpUtility.UrlEncode(parameters[key]);
                buffer.AppendFormat("&{0}={1}", key, val);
            }
            return buffer.ToString().TrimStart('&');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Stream Post(string url, string parameters, Encoding encoding, string contentType)
        {
            using (var resp = Post(url, parameters, null, string.Empty, encoding, contentType, null))
            {
                return resp.GetResponseStream();
            }
        }

        /// <summary>  
        /// Post request 
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数</param>
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="encoding">发送HTTP请求时所用的编码</param>  
        /// <param name="contentType"></param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse Post(string url, string parameters, int? timeout, string userAgent, Encoding encoding, string contentType, CookieCollection cookies)
        {
            return Post(url, encoding.GetBytes(parameters), timeout, userAgent, contentType, cookies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="inputBytes"></param>
        /// <param name="timeout"></param>
        /// <param name="userAgent"></param>
        /// <param name="contentType"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static HttpWebResponse Post(string url, byte[] inputBytes, int? timeout, string userAgent, string contentType, CookieCollection cookies)
        {
            var request = DoPostRequest(url, inputBytes, timeout, userAgent, contentType, cookies);
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="userAgent"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static Task<WebResponse> PostAsync(string url, string parameters, int? timeout, string userAgent, Encoding encoding, string contentType, CookieCollection cookies)
        {
            return PostAsync(url, encoding.GetBytes(parameters), timeout, userAgent, contentType, cookies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="inputBytes"></param>
        /// <param name="timeout"></param>
        /// <param name="userAgent"></param>
        /// <param name="contentType"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static Task<WebResponse> PostAsync(string url, byte[] inputBytes, int? timeout, string userAgent, string contentType, CookieCollection cookies)
        {
            var request = DoPostRequest(url, inputBytes, timeout, userAgent, contentType, cookies);
            return request.GetResponseAsync();
        }

        private static HttpWebRequest DoPostRequest(string url, byte[] inputBytes, int? timeout, string userAgent, string contentType, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            if (!string.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (inputBytes != null && inputBytes.Length > 0)
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(inputBytes, 0, inputBytes.Length);
                }
            }
            return request;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true;
        }
    }
}
