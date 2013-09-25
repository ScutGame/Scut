using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Web.Caching;
using System.Xml;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Net;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.RPC.Wcf;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public static class ActionFactory
    {
        internal class ActionConfig
        {
            private static ActionConfig instance;

            static ActionConfig()
            {
                instance = new ActionConfig();
                instance.TypeName = ConfigUtils.GetSetting("Game.Action.TypeName");
                instance.IpAddress = ConfigUtils.GetSetting("Game.IpAddress");
                if (string.IsNullOrEmpty(instance.IpAddress))
                {
                    instance.IpAddress = GetLocalIp();
                }
                instance.Port = ConfigUtils.GetSetting("Game.Port").ToInt();
            }

            public static ActionConfig Current
            {
                get { return instance; }
            }

            public string IpAddress
            {
                get;
                private set;
            }

            public int Port
            {
                get;
                private set;
            }

            public string TypeName
            {
                get;
                private set;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            string localIp = "";
            IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
            foreach (var ipAddress in addressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ipAddress.ToString();
                    break;
                }
            }
            return localIp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userFactory"></param>
        public static void Request(Func<int, BaseUser> userFactory)
        {
            Request(ActionConfig.Current.TypeName, userFactory);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpGet"></param>
        /// <param name="response"></param>
        /// <param name="userFactory"></param>
        public static void Request(HttpGet httpGet, IGameResponse response, Func<int, BaseUser> userFactory)
        {
            Request(ActionConfig.Current.TypeName, httpGet, response, userFactory);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="userFactory"></param>
        public static void Request(string typeName, Func<int, BaseUser> userFactory)
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("HttpContext is not supported.");
            }
            HttpGet httpGet = new HttpGet(HttpContext.Current.Request);
            IGameResponse response = new HttpGameResponse(HttpContext.Current.Response);
            Request(typeName, httpGet, response, userFactory);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="response"></param>
        /// <param name="userFactory"></param>
        /// <param name="httpGet"></param>
        public static void Request(string typeName, HttpGet httpGet, IGameResponse response, Func<int, BaseUser> userFactory)
        {
            int actionID = 0;
            string errorInfo = "";
            try
            {
                bool isRL = BaseStruct.CheckRunloader(httpGet);
                if (isRL || httpGet.CheckSign())
                {
                    if (httpGet.GetInt("ActionID", ref actionID))
                    {
                        BaseStruct action = FindRoute(typeName, httpGet, actionID);
                        Process(action, httpGet, response, userFactory);
                        if (action != null)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    errorInfo = "签名验证失败";
                    TraceLog.WriteError("Action request error:{2},rl:{0},param:{1}", isRL, httpGet.ParamString, errorInfo);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Action request error:{0}\r\nparam:{1}", ex, httpGet.ParamString);
            }
            RequestError(response, actionID, 10000, errorInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="actionID"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        public static void RequestError(IGameResponse response, int actionID, int errorCode, string errorInfo)
        {
            MessageHead head = new MessageHead(actionID, errorCode, errorInfo);
            MessageStructure sb = new MessageStructure();
            sb.WriteBuffer(head);
            response.BinaryWrite(sb.ReadBuffer());
        }

        /// <summary>
        /// 获取Action处理的输出字节流
        /// </summary>
        /// <returns></returns>
        public static byte[] GetActionResponse(int actionId, BaseUser baseUser, string parameters, out HttpGet httpGet)
        {
            string serverHost = string.Format("{0}:{1}", ActionConfig.Current.IpAddress, ActionConfig.Current.Port);
            string param = string.Format("MsgId={0}&St={1}&Sid={2}&Uid={3}&ActionID={4}{5}",
                0,
                "st",
                baseUser.GetSessionId(),
                baseUser.GetUserId(),
                actionId,
                parameters);
            httpGet = new HttpGet(param, baseUser.SocketSid, baseUser.RemoteAddress);
            BaseStruct baseStruct = FindRoute(ActionConfig.Current.TypeName, httpGet, actionId);
            SocketGameResponse response = new SocketGameResponse();
            baseStruct.UserFactory = uid => { return baseUser; };
            baseStruct.SetPush();
            baseStruct.DoInit();
            using (baseStruct.RequestLock())
            {
                if (!baseStruct.GetError() &&
                    baseStruct.ReadUrlElement() &&
                    baseStruct.DoAction() &&
                    !baseStruct.GetError())
                {
                    baseStruct.BuildPacket();
                    baseStruct.WriteAction(response);
                }
                else
                {
                    baseStruct.WriteErrorAction(response);
                }
            }
            return response.ReadByte();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseStruct"></param>
        /// <param name="httpGet"></param>
        /// <param name="response"></param>
        /// <param name="userFactory"></param>
        public static void Process(BaseStruct baseStruct, HttpGet httpGet, IGameResponse response, Func<int, BaseUser> userFactory)
        {
            baseStruct.UserFactory = userFactory;
            baseStruct.DoInit();
            using (baseStruct.RequestLock())
            {
                if (!baseStruct.GetError() &&
                    baseStruct.ReadUrlElement() &&
                    baseStruct.DoAction() &&
                    !baseStruct.GetError())
                {
                    baseStruct.BuildPacket();
                    baseStruct.WriteAction(response);
                }
                else
                {
                    baseStruct.WriteErrorAction(response);
                }
            }
        }

        internal static BaseStruct FindRoute(string typeExpression, HttpGet httpGet, int actionID)
        {
            string typeName = string.Format(typeExpression, actionID);
            ScriptRoute scriptRoute = new ScriptRoute(actionID);
            RouteItem routeItem;
            var pythonManager = PythonScriptManager.Current;
            if (pythonManager.TryGetAction(actionID, out routeItem))
            {
                if (scriptRoute.TryLoadAction(routeItem.ScriptPath))
                {
                    return new ScriptAction((short)actionID, httpGet, scriptRoute, routeItem.IgnoreAuthorize);
                }
                //中间件路由配置
                if (!string.IsNullOrEmpty(routeItem.TypeName))
                {
                    typeName = routeItem.TypeName;
                }
            }

            Type actionType = null;
            try
            {
                actionType = Type.GetType(typeName);
            }
            catch { }
            if (actionType != null)
            {
                return (BaseStruct)Activator.CreateInstance(actionType, new object[] { httpGet });
            }

            throw new NullReferenceException(string.Format("未找到Action处理对象的类型:{0}!", typeName));
        }
    }
}
