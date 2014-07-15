using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Lang;
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
            int actionid;
            if (!int.TryParse(param["actionid"], out actionid)) { return false; }
            int msgid;
            if (!int.TryParse(param["msgid"], out msgid)) { return false; }
            int userId;
            int.TryParse(param["uid"], out userId);

            string sessionId = param.ContainsKey("sid") ? param["sid"] : "";
            package = new RequestPackage(msgid, sessionId, actionid, userId)
            {
                ProxySid = proxySid,
                IsProxyRequest = param.ContainsKey("isproxy"),
                RouteName = routeName,
                IsUrlParam = true,
                UrlParam = paramStr
            };

            return true;
        }

        public virtual bool TryDecodePackage(HttpListenerContext context, out RequestPackage package)
        {
            package = null;
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
            package = new RequestPackage(msgid, sessionId, actionid, userId)
            {
                ProxySid = proxySid,
                IsProxyRequest = param.ContainsKey("isproxy"),
                RouteName = routeName,
                IsUrlParam = true,
                UrlParam = data
            };

            return true;
        }

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
            
            var session = GameSession.Get(sessionId)
                ?? GameSession.CreateNew(Guid.NewGuid(), context.Request);

            package = new RequestPackage(msgId, sessionId, actionId, userId)
            {
                IsUrlParam = true,
                UrlParam = d,
                Session = session
            };
            return true;
        }


        public virtual ActionGetter GetActionGetter(RequestPackage package)
        {
            return new HttpGet(package);
        }

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
