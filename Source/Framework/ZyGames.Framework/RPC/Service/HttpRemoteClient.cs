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
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.RPC.Service
{
    /// <summary>
    /// Remote client for http
    /// </summary>
    public class HttpRemoteClient : RemoteClient
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private readonly string _url;
        private readonly Encoding _encoding;
        private readonly int? _timeout;
        private CookieCollection _cookies;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public HttpRemoteClient(string url)
            : this(url, Encoding.UTF8, null)
        {
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="timeout">allow null</param>
        /// <param name="hasCookies">use cookies</param>
        public HttpRemoteClient(string url, Encoding encoding, int? timeout, bool hasCookies = false)
        {
            _url = url;
            _encoding = encoding;
            _timeout = timeout;
            if (hasCookies)
            {
                _cookies = new CookieCollection();
            };
            UserAgent = DefaultUserAgent;
            LocalAddress = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// use http post method request.
        /// </summary>
        public bool IsPostMethod { get; set; }

        /// <summary>
        /// Get cookies
        /// </summary>
        public CookieCollection Cookies { get { return _cookies; } }
        /// <summary>
        /// Send
        /// </summary>
        /// <param name="data"></param>
        public override async Task Send(string data)
        {
            var param = _encoding.GetBytes(data);
            await Send(param);
        }

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="data"></param>
        public override async Task Send(byte[] data)
        {
            using (var response = await GetResponse(data))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    RemoteEventArgs e = new RemoteEventArgs();
                    if (stream == null)
                    {
                        OnCallback(e);
                        return;
                    }
                    e.Data = ReadStream(stream, _encoding);
                    OnCallback(e);
                }
            }
        }

        public override void Close()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async Task<WebResponse> GetResponse(byte[] data)
        {
            string url = _url;

            if (!IsPostMethod)
            {
                string param = _encoding.GetString(data).TrimStart('?');
                url = url + ("?" + param);
            }
            HttpWebRequest client = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                client = WebRequest.Create(url) as HttpWebRequest;
                if (client != null) client.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                client = WebRequest.Create(url) as HttpWebRequest;
            }
            if (client == null)
            {
                throw new Exception(string.Format("Create http \"{0}\" request is error.", url));
            }
            if (!string.IsNullOrEmpty(UserAgent))
            {
                client.UserAgent = UserAgent;
            }
            if (_timeout.HasValue)
            {
                client.Timeout = _timeout.Value;
            }
            if (_cookies != null)
            {
                client.CookieContainer = new CookieContainer();
                client.CookieContainer.Add(_cookies);
            }

            if (IsPostMethod)
            {
                client.Method = "POST";
                using (Stream stream = await client.GetRequestStreamAsync())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            else
            {
                client.Method = "GET";
            }
            return await client.GetResponseAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected virtual bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
