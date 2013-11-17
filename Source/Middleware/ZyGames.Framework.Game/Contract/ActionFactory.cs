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
using System.Net;
using System.Net.Sockets;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// Action管理
    /// </summary>
    public static class ActionFactory
    {
		/// <summary>
		/// The error code.
		/// </summary>
        public static int ErrorCode = 10000;

        internal class ActionConfig
        {
            private static ActionConfig instance;

            static ActionConfig()
            {
                instance = new ActionConfig();
                instance.TypeName = ConfigUtils.GetSetting("Game.Action.TypeName");
                if (string.IsNullOrEmpty(instance.TypeName))
                {
                    string assemblyName = ConfigUtils.GetSetting("Game.Action.AssemblyName");
                    if (!string.IsNullOrEmpty(assemblyName))
                    {
                        instance.TypeName = assemblyName + ".Action.Action{0}," + assemblyName;
                    }
                }
                instance.IpAddress = ConfigUtils.GetSetting("Game.IpAddress");
                if (string.IsNullOrEmpty(instance.IpAddress))
                {
                    instance.IpAddress = GetLocalIp();
                }
                instance.Port = ConfigUtils.GetSetting("Game.Port", 9101);
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
        /// 获取本地IP
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
        /// 请求处理
        /// </summary>
        /// <param name="userFactory"></param>
        public static void Request(Func<int, BaseUser> userFactory)
        {
            Request(ActionConfig.Current.TypeName, userFactory);
        }

        /// <summary>
        /// 请求处理
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
        /// 请求处理
        /// </summary>
        /// <param name="httpGet"></param>
        /// <param name="response"></param>
        /// <param name="userFactory"></param>
        public static void Request(HttpGet httpGet, IGameResponse response, Func<int, BaseUser> userFactory)
        {
            Request(ActionConfig.Current.TypeName, httpGet, response, userFactory);
        }

        /// <summary>
        /// 请求处理
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
					TraceLog.WriteError("Action request {3} error:{2},rl:{0},param:{1}", isRL, httpGet.ParamString, errorInfo, typeName);
                }
            }
            catch (Exception ex)
            {
				TraceLog.WriteError("Action request {0} error:{1}\r\nparam:{2}", typeName, ex, httpGet.ParamString);
            }
            RequestError(response, actionID, errorInfo);
        }

        /// <summary>
        /// 请求脚本处理
        /// </summary>
        /// <param name="userFactory">创建user对象工厂</param>
        public static void RequestScript(Func<int, BaseUser> userFactory = null)
        {
            HttpGet httpGet = new HttpGet(HttpContext.Current.Request);
            IGameResponse response = new HttpGameResponse(HttpContext.Current.Response);
            RequestScript(httpGet, response, userFactory);
        }

        /// <summary>
        /// 请求脚本处理
        /// </summary>
        /// <param name="httpGet">请求参数对象</param>
        /// <param name="response">字节输出处理</param>
        /// <param name="userFactory">创建user对象工厂,可为Null</param>
        public static void RequestScript(HttpGet httpGet, IGameResponse response, Func<int, BaseUser> userFactory)
        {
            int actionID = httpGet.GetInt("actionId");
            string errorInfo = "";
            try
            {
                bool isRl = BaseStruct.CheckRunloader(httpGet);
                if (isRl || httpGet.CheckSign())
                {
                    ScriptRoute scriptRoute = new ScriptRoute(actionID);
                    RouteItem routeItem;
                    var pythonManager = PythonScriptManager.Current;
                    if (pythonManager.TryGetAction(actionID, out routeItem))
                    {
                        if (scriptRoute.TryLoadAction(routeItem.ScriptPath))
                        {
                            var baseStruct = new ScriptAction((short)actionID, httpGet, scriptRoute, routeItem.IgnoreAuthorize);
                            Process(baseStruct, httpGet, response, userFactory);
                            return;
                        }
                    }
                }
                else
                {
                    errorInfo = "签名验证失败";
                    TraceLog.WriteError("Action request error:{2},rl:{0},param:{1}", isRl, httpGet.ParamString, errorInfo);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Action request error:{0}\r\nparam:{1}", ex, httpGet.ParamString);
            }
            RequestError(response, actionID, errorInfo);
        }
        /// <summary>
        /// 出错处理
        /// </summary>
        /// <param name="response"></param>
        /// <param name="actionID"></param>
        /// <param name="errorInfo"></param>
        public static void RequestError(IGameResponse response, int actionID, string errorInfo)
        {
            RequestError(response, actionID, ErrorCode, errorInfo);
        }

        /// <summary>
        /// 出错处理
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
            using (ILocking locking = baseStruct.RequestLock())
            {
                if (locking.IsLocked)
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
            using (ILocking locking = baseStruct.RequestLock())
            {
                if (locking.IsLocked)
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

            Type actionType = Type.GetType(typeName);
            if (actionType != null)
            {
				try
				{
					return actionType.CreateInstance<BaseStruct>(new object[] { httpGet });
					//return (BaseStruct)Activator.CreateInstance(actionType, new object[] { httpGet });
				}
				catch(Exception ex) 
				{
					throw ex;
				}
            }

            throw new NullReferenceException(string.Format("未找到Action处理对象的类型:{0}!", typeName));
        }
    }
}