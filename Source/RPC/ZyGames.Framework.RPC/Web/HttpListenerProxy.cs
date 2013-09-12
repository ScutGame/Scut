using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Web
{
    /// <summary>
    /// Http监听代理类
    /// </summary>
    public class HttpListenerProxy : IDisposable
    {
        private readonly HttpSettings _settings;
        private HttpListener _listener;
        private event Action<HttpListenerContext> ReceiveHandle;

        /// <summary>
        /// 
        /// </summary>
        public HttpListenerProxy(HttpSettings settings)
        {
            _settings = settings;
            _listener = new HttpListener();
            ReceiveHandle += OnReceiveCompleted;
        }

        /// <summary>
        /// 
        /// </summary>
        public event Action<HttpConnection, string, byte[]> RequestCompleted;

        /// <summary>
        /// 
        /// </summary>
        public event Action<HttpConnection> RequestTimeout;


        /// <summary>
        /// 
        /// </summary>
        public void Listen()
        {
            var hosts = _settings.HostAddress.Split(',');
            foreach (var h in hosts)
            {
                string appName = _settings.GameAppName;
                if(!appName.EndsWith("/"))
                {
                    appName += "/";
                }
                _listener.Prefixes.Add(string.Format("{0}:{1}/{2}", h, _settings.Port, appName));
            }
            _listener.Start();
            StartAccept();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void PushSend(HttpListenerResponse response, byte[] data, int offset, int count)
        {
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

        private void StartAccept()
        {
            _listener.BeginGetContext(AcceptCompleted, _listener);
        }

        private void AcceptCompleted(IAsyncResult ar)
        {
            HttpListener listener = (HttpListener)ar.AsyncState;
            HttpListenerContext context = listener.EndGetContext(ar);
            ReceiveHandle.BeginInvoke(context, null, null);
            //等待接收下一次请求
            StartAccept();
        }

        private void OnReceiveCompleted(HttpListenerContext context)
        {
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

            if (string.IsNullOrEmpty(data))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Close();
                return;
            }
            string remoteAddress = request.RemoteEndPoint.Address.ToString();
            var requestParam = HttpUtility.ParseQueryString(data);
            //requestParam["UserHostAddress"] = remoteAddress;
            //requestParam["ssid"] = ssid.ToString("N");
            //requestParam["http"] = "1";

            var clientConnection = new HttpConnection { Context = context, SSID = ssid, Param = requestParam };
            clientConnection.TimeoutTimer = new Timer(OnRequestTimeout, clientConnection, _settings.RequestTimeout, Timeout.Infinite);
            byte[] paramData = Encoding.ASCII.GetBytes(data);
            RequestCompleted.BeginInvoke(clientConnection, remoteAddress, paramData, null, null);
        }

        private void OnRequestTimeout(object state)
        {
            try
            {
                HttpConnection clientConnection = (HttpConnection)state;

                if (RequestTimeout != null)
                {
                    RequestTimeout.BeginInvoke(clientConnection, null, null);
                }
                clientConnection.TimeoutTimer.Dispose();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnRequestTimeout error:{0}", ex);
            }
        }

        private static string ToQueryString(NameValueCollection data)
        {
            string value = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            string[] allKeys = data.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string text2 = allKeys[i];
                stringBuilder.Append(value);
                stringBuilder.Append(text2);
                stringBuilder.Append("=");
                stringBuilder.Append(HttpUtility.UrlEncode(data[text2]));
                value = "&";
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _listener.Close();
            _listener = null;
            //清理托管对象
            GC.SuppressFinalize(this);
        }
    }
}