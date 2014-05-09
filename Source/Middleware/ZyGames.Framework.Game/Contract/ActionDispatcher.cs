using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        bool TryDecodePackage(ConnectionEventArgs e, out RequestPackage package);

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
    class ActionDispatcher : IActionDispatcher
    {

        /// <summary>
        /// Decode request package
        /// </summary>
        /// <param name="e"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public bool TryDecodePackage(ConnectionEventArgs e, out RequestPackage package)
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

        public ActionGetter GetActionGetter(RequestPackage package)
        {
            return new HttpGet(package);
        }

        public void ResponseError(BaseGameResponse response, ActionGetter actionGetter, int errorCode, string errorInfo)
        {
            MessageHead head = new MessageHead(actionGetter.GetMsgId(), actionGetter.GetActionId(), errorCode, errorInfo);
            MessageStructure sb = new MessageStructure();
            sb.WriteBuffer(head);
            response.BinaryWrite(sb.PopBuffer());
        }

    }
}
