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
            var stream = Get(url, contentType);
            return new StreamReader(stream, encoding).ReadToEnd();
        }

        /// <summary>
        /// Get request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Stream Get(string url, string contentType = "")
        {
            return Get(url, contentType, null, string.Empty, null).GetResponseStream();
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
            return request.GetResponse() as HttpWebResponse;
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
            return new StreamReader(Post(url, parameters, encoding, contentType), encoding).ReadToEnd();
        }

        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Stream Post(string url, IDictionary<string, string> parameters, Encoding encoding, string contentType = "")
        {
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            foreach (string key in parameters.Keys)
            {
                if (i > 0)
                {
                    buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", key, parameters[key]);
                }
                i++;
            }
            return Post(url, buffer.ToString(), encoding, contentType);
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
            return Post(url, parameters, null, string.Empty, encoding, contentType, null).GetResponseStream();
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
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
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
            if (!string.IsNullOrEmpty(parameters))
            {
                if (parameters.Length > 0 && parameters.StartsWith("?"))
                {
                    parameters = parameters.Substring(1);
                }
                byte[] data = encoding.GetBytes(parameters);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true;
        }
    }
}
