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
using System.Text;
using System.Net;
using System.Web;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using NLog;


namespace ProxyServer
{
    class HttpProxy
    {
        private static readonly Logger Logger = LogManager.GetLogger("GameHttpProxy");
        private static string gameName = Util.GetAppSetting<string>("GameName","default");
        private static int gamePort = Util.GetAppSetting<int>("GamePort", 8088);
        private static string errmsg = Util.GetAppSetting<string>("ErrMsg");
        private static int proxyCheckPeriod = Util.GetAppSetting<int>("ProxyCheckPeriod", 60000);
        private static int httpProxyTimeout = Util.GetAppSetting<int>("HttpProxyTimeout", 120000);
        private static string gameHost = Util.GetAppSetting<string>("GameHost", "http://127.0.0.1");
        private HttpListener listener;
        private GSConnectionManager gsConnectionManager;
        private ConcurrentDictionary<Guid, HttpClientConnection> pool = new ConcurrentDictionary<Guid, HttpClientConnection>();
        private Timer timer;

        public HttpProxy(GSConnectionManager gsConnectionManager)
        {
            this.gsConnectionManager = gsConnectionManager;

            listener = new HttpListener();
            var hosts = gameHost.Split(',');
            foreach (var h in hosts)
            {
                listener.Prefixes.Add(string.Format("{2}:{0}/{1}/", gamePort, gameName, h));
            }
            listener.Start();
            Logger.Info("Http listent is started,The port:{0}.", gamePort);
            listener.BeginGetContext(ListenerCallback, listener);
            timer = new Timer(Check, null, proxyCheckPeriod, proxyCheckPeriod);
        }

        private void Check(object state)
        {
            ConfigurationManager.RefreshSection("appSettings");
            errmsg = Util.GetAppSetting<string>("ErrMsg");
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Http并发数：{0}", pool.Count);
            }
        }

        public void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);
            listener.BeginGetContext(ListenerCallback, listener);

            var ssid = Guid.NewGuid();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string data = "";
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (string.Compare(request.HttpMethod, "get", true) == 0)
                {
                    data = request.RawUrl.Substring(8);
                    data = HttpUtility.UrlDecode(data);
                }
            }
            else
            {
                data = request.QueryString["d"];
            }

            if (string.IsNullOrEmpty(data))
            {
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    data = reader.ReadToEnd();
                    data = HttpUtility.ParseQueryString(data)["d"];
                }
            }

            int gameId, serverId, statuscode;
            var requestParam = RequestParse.Parse(request.RemoteEndPoint.Address.ToString(), request.RawUrl, data,out gameId, out serverId, out statuscode);
            if (statuscode != (int)HttpStatusCode.OK)
            {
                response.StatusCode = statuscode;
                response.Close();
                return;
            }

            requestParam["UserHostAddress"] = request.RemoteEndPoint.Address.ToString();
            requestParam["ssid"] = ssid.ToString("N");
            requestParam["http"] = "1";

            var clientConnection = new HttpClientConnection { Context = context, SSID = ssid, Param = requestParam };
            clientConnection.TimeoutTimer = new Timer(TimeoutSendback, clientConnection, httpProxyTimeout, Timeout.Infinite);
            byte[] paramData = Encoding.ASCII.GetBytes(RequestParse.ToQueryString(requestParam));
            pool[ssid] = clientConnection;

            try
            {
                gsConnectionManager.Send(gameId,serverId, paramData);
            }
            catch (Exception ex)
            {
                Logger.Error("无法连接游服", ex);
                var responseData = RequestParse.CtorErrMsg(errmsg, requestParam);
                SendDataBack(ssid, responseData, 0, responseData.Length);
            }
        }

        private void TimeoutSendback(object state)
        {
            HttpClientConnection clientConnection = (HttpClientConnection)state;
            NameValueCollection requestParam = clientConnection.Param;
            Logger.Error("超时无法连接游服");
            var responseData = RequestParse.CtorErrMsg(errmsg, requestParam);
            SendDataBack(clientConnection.SSID, responseData, 0, responseData.Length);
        }

        public void FlushConnected()
        {
            foreach (var kv in pool)
            {
                var responseData = RequestParse.CtorErrMsg(errmsg, kv.Value.Param);
                SendDataBack(kv.Value.SSID, responseData, 0, responseData.Length);
            }
        }

        public bool SendDataBack(Guid ssid, byte[] data, int offset, int count)
        {
            try
            {
                HttpClientConnection clientConnection;
                if (!pool.TryRemove(ssid, out clientConnection)) return false;
                clientConnection.TimeoutTimer.Dispose();

                var response = clientConnection.Context.Response;
                response.ContentType = "application/octet-stream";
                if (data[offset] == 0x1f && data[offset + 1] == 0x8b && data[offset + 2] == 0x08 && data[offset + 3] == 0x00)
                {
                    response.AddHeader("Content-Encoding", "gzip");
                }
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.ContentLength64 = count;
                Stream output = response.OutputStream;
                output.Write(data, offset, count);
                output.Close();
            }
            catch
            {

            }

            return true;
        }

        /// <summary>
        /// 获取指定Url的内容
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="encoding">用于对字符串进行编码的 Encoding，默认使用Encoding.Default</param>
        /// <returns></returns>
        public static string GetHttpUrl(string url, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.Default;
            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding;
                var result = client.DownloadString(url);
                return result;
            }
        }
    }

    class HttpClientConnection
    {
        public Guid SSID;
        public HttpListenerContext Context;
        public Timer TimeoutTimer;
        public NameValueCollection Param;
    }
}