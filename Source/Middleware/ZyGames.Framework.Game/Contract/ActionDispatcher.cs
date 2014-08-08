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
using System.Net;
using System.Text;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// Action分发器接口
    /// </summary>
    public interface IActionDispatcher
    {
        /// <summary>
        /// decode package for socket
        /// </summary>
        /// <param name="e"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        bool TryDecodePackage(ConnectionEventArgs e, out RequestPackage package);

        /// <summary>
        /// decode package for http
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        bool TryDecodePackage(HttpListenerContext context, out RequestPackage package);

        /// <summary>
        /// decode package for http
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        bool TryDecodePackage(HttpContext context, out RequestPackage package);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        ActionGetter GetActionGetter(RequestPackage package);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="actionGetter"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        void ResponseError(BaseGameResponse response, ActionGetter actionGetter, int errorCode, string errorInfo);
    }

    /// <summary>
    /// Action分发器
    /// </summary>
    public class ScutActionDispatcher : IActionDispatcher
    {

        /// <summary>
        /// Decode request package
        /// </summary>
        /// <param name="e"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public virtual bool TryDecodePackage(ConnectionEventArgs e, out RequestPackage package)
        {
            package = null;
            string paramStr = Encoding.ASCII.GetString(e.Data);
            int index = paramStr.IndexOf("?d=", StringComparison.CurrentCultureIgnoreCase);
            string routeName = string.Empty;
            if (index != -1)
            {
                if (paramStr.StartsWith("route:", StringComparison.CurrentCultureIgnoreCase))
                {
                    routeName = paramStr.Substring(6, index - 6);
                }
                paramStr = paramStr.Substring(index, paramStr.Length - index);
                paramStr = HttpUtility.ParseQueryString(paramStr)["d"];
            }

            var nvc = HttpUtility.ParseQueryString(paramStr);
            var param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var key in nvc.AllKeys)
            {
                param[key] = nvc[key];
            }
            if (param.ContainsKey("route"))
            {
                routeName = param["route"];
            }
            //if (!param.ContainsKey("ssid")) { Interlocked.Increment(ref errorDropNum); return; }
            if (!param.ContainsKey("actionid")) { return false; }
            if (!param.ContainsKey("msgid")) { return false; }

            //sessionId of proxy server
            Guid proxySid;
            if (!param.ContainsKey("ssid") || !Guid.TryParse(param["ssid"], out proxySid))
            {
                proxySid = Guid.Empty;
            }
            int msgid;
            if (!int.TryParse(param["msgid"], out msgid)) { return false; }
            int actionid;
            if (!int.TryParse(param["actionid"], out actionid)) { return false; }
            int userId;
            int.TryParse(param["uid"], out userId);

            string sessionId = param.ContainsKey("sid") ? param["sid"] : "";
            string proxyId = param.ContainsKey("proxyId") ? param["proxyId"] : "";
            package = new RequestPackage(msgid, sessionId, actionid, userId)
            {
                ProxySid = proxySid,
                ProxyId = proxyId,
                IsProxyRequest = param.ContainsKey("isproxy"),
                RouteName = routeName,
                IsUrlParam = true,
                UrlParam = paramStr
            };

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public virtual bool TryDecodePackage(HttpListenerContext context, out RequestPackage package)
        {
            package = null;
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string data = "";
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (String.Compare(request.HttpMethod, "get", StringComparison.OrdinalIgnoreCase) == 0)
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

            int statuscode = 0;
            Dictionary<string, string> param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (data != null)
            {
                var nvc = HttpUtility.ParseQueryString(data);
                foreach (var key in nvc.AllKeys)
                {
                    param[key] = nvc[key];
                }
            }
            statuscode = CheckHttpParam(param);

            if (statuscode != (int)HttpStatusCode.OK)
            {
                response.StatusCode = statuscode;
                response.Close();
                return false;
            }
            //sessionId of proxy server
            Guid proxySid;
            if (!param.ContainsKey("ssid") || !Guid.TryParse(param["ssid"], out proxySid))
            {
                proxySid = Guid.Empty;
            }
            int actionid;
            if (!int.TryParse(param["actionid"], out actionid)) { return false; }
            int msgid;
            if (!int.TryParse(param["msgid"], out msgid)) { return false; }
            int userId;
            int.TryParse(param["uid"], out userId);
            string routeName = string.Empty;
            if (param.ContainsKey("route"))
            {
                routeName = param["route"];
            }

            string sessionId = param.ContainsKey("sid") ? param["sid"] : "";
            string proxyId = param.ContainsKey("proxyId") ? param["proxyId"] : "";
            package = new RequestPackage(msgid, sessionId, actionid, userId)
            {
                ProxySid = proxySid,
                ProxyId = proxyId,
                IsProxyRequest = param.ContainsKey("isproxy"),
                RouteName = routeName,
                IsUrlParam = true,
                UrlParam = data
            };

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public virtual bool TryDecodePackage(HttpContext context, out RequestPackage package)
        {
            package = null;
            if (context == null)
            {
                return false;
            }
            var d = context.Request["d"] ?? "";
            var nvc = HttpUtility.ParseQueryString(d);
            Dictionary<string, string> param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var key in nvc.AllKeys)
            {
                param[key] = nvc[key];
            }

            string sessionId = param.ContainsKey("sid") ? param["sid"] : "";
            int msgId = (param.ContainsKey("MsgId") ? param["MsgId"] : "0").ToInt();
            int actionId = (param.ContainsKey("actionId") ? param["actionId"] : "0").ToInt();
            int userId = (param.ContainsKey("uid") ? param["uid"] : "0").ToInt();

            Guid proxySid;
            if (!param.ContainsKey("ssid") || !Guid.TryParse(param["ssid"], out proxySid))
            {
                proxySid = Guid.Empty;
            }
            string proxyId = param.ContainsKey("proxyId") ? param["proxyId"] : "";


            package = new RequestPackage(msgId, sessionId, actionId, userId)
            {
                ProxySid = proxySid,
                ProxyId = proxyId,
                IsProxyRequest = param.ContainsKey("isproxy"),
                IsUrlParam = true,
                UrlParam = d,
            };
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public virtual ActionGetter GetActionGetter(RequestPackage package)
        {
            return new HttpGet(package);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="actionGetter"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        public virtual void ResponseError(BaseGameResponse response, ActionGetter actionGetter, int errorCode, string errorInfo)
        {
            MessageHead head = new MessageHead(actionGetter.GetMsgId(), actionGetter.GetActionId(), errorCode, errorInfo);
            MessageStructure sb = new MessageStructure();
            sb.WriteBuffer(head);
            response.BinaryWrite(sb.PopBuffer());
        }

        private int CheckHttpParam(Dictionary<string, string> param)
        {
            if (!param.ContainsKey("actionid"))
            {
                return (int)HttpStatusCode.BadRequest;
            }
            if (!param.ContainsKey("msgid"))
            {
                return (int)HttpStatusCode.BadRequest;
            }
            return (int)HttpStatusCode.OK;
        }

    }
}
