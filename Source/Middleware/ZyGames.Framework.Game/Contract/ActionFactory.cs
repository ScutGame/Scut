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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using ServiceStack.Common.Extensions;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Net;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// Action管理
    /// </summary>
    public static class ActionFactory
    {
        private static HashSet<int> _ignoreAuthorizeSet = new HashSet<int>();

        /// <summary>
        /// The error code.
        /// </summary>
        public static int ErrorCode = 10000;

        /// <summary>
        /// 设置忽略认证Action
        /// </summary>
        /// <param name="actionIds"></param>
        public static void SetActionIgnoreAuthorize(params int[] actionIds)
        {
            foreach (var actionId in actionIds)
            {
                _ignoreAuthorizeSet.Add(actionId);
            }
        }

        internal static bool IsIgnoreAction(int actionId)
        {
            return _ignoreAuthorizeSet.Contains(actionId);
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        public static void Request()
        {
            Request(GameEnvironment.Setting.ActionTypeName);
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="typeName"></param>
        public static void Request(string typeName)
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("HttpContext is not supported.");
            }
            HttpGet httpGet = new HttpGet(HttpContext.Current.Request);
            HttpGameResponse response = new HttpGameResponse(HttpContext.Current.Response);
            Request(typeName, httpGet, response);
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="actionGetter"></param>
        /// <param name="response"></param>
        public static void Request(ActionGetter actionGetter, BaseGameResponse response)
        {
            Request(GameEnvironment.Setting.ActionTypeName, actionGetter, response);
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="response"></param>
        /// <param name="actionGetter"></param>
        public static void Request(string typeName, ActionGetter actionGetter, BaseGameResponse response)
        {
            var actionId = actionGetter.GetActionId().ToInt();
            string tempName = string.Format(typeName, actionId);
            string errorInfo = "";
            try
            {
                bool isRL = BaseStruct.CheckRunloader(actionGetter);
                if (isRL || actionGetter.CheckSign())
                {
                    BaseStruct action = FindRoute(typeName, actionGetter, actionId);
                    Process(action, actionGetter, response);
                    if (action != null)
                    {
                        return;
                    }
                }
                else
                {
                    errorInfo = Language.Instance.SignError;
                    TraceLog.WriteError("Action request {3} error:{2},rl:{0},param:{1}", isRL, actionGetter.ToString(), errorInfo, tempName);
                }
            }
            catch (Exception ex)
            {
                errorInfo = Language.Instance.ServerBusy;
                TraceLog.WriteError("Action request {0} error:{1}\r\nparam:{2}", tempName, ex, actionGetter.ToString());
            }
            response.WriteError(actionGetter, Language.Instance.ErrorCode, errorInfo);
        }

        /// <summary>
        /// 请求脚本处理
        /// </summary>
        public static void RequestScript()
        {
            HttpGet httpGet = new HttpGet(HttpContext.Current.Request);
            BaseGameResponse response = new HttpGameResponse(HttpContext.Current.Response);
            RequestScript(httpGet, response);
        }

        /// <summary>
        /// 请求脚本处理
        /// </summary>
        /// <param name="actionGetter">请求参数对象</param>
        /// <param name="response">字节输出处理</param>
        public static void RequestScript(ActionGetter actionGetter, BaseGameResponse response)
        {
            int actionId = actionGetter.GetActionId();
            string errorInfo = "";
            try
            {
                bool isRl = BaseStruct.CheckRunloader(actionGetter);
                if (isRl || actionGetter.CheckSign())
                {
                    BaseStruct baseStruct = FindScriptRoute(actionGetter, actionId);
                    if (baseStruct != null)
                    {
                        Process(baseStruct, actionGetter, response);
                        return;
                    }
                }
                else
                {
                    errorInfo = Language.Instance.SignError;
                    TraceLog.WriteError("Action request error:{2},rl:{0},param:{1}", isRl, actionGetter.ToString(), errorInfo);
                }
            }
            catch (Exception ex)
            {
                errorInfo = Language.Instance.ServerBusy;
                TraceLog.WriteError("Action request error:{0}\r\nparam:{1}", ex, actionGetter.ToString());
            }
            response.WriteError(actionGetter, Language.Instance.ErrorCode, errorInfo);
        }

        /// <summary>
        /// 获取Action处理的输出字节流
        /// </summary>
        /// <param name="actionDispatcher"></param>
        /// <param name="actionId"></param>
        /// <param name="session"></param>
        /// <param name="parameters"></param>
        /// <param name="opCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] GetActionResponse(IActionDispatcher actionDispatcher, int actionId, GameSession session, Parameters parameters, sbyte opCode, object message = null)
        {
            var requestPackage = GetResponsePackage(actionId, session, parameters, opCode, message);
            return GetActionResponse(actionDispatcher, actionId, session, requestPackage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="session"></param>
        /// <param name="parameters"></param>
        /// <param name="opCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static RequestPackage GetResponsePackage(int actionId, GameSession session, Parameters parameters, sbyte opCode, object message)
        {
            int userId = session != null ? session.UserId : 0;
            string sessionId = session != null ? session.SessionId : "";

            var paramList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            //init head
            paramList["MsgId"] = "0";
            paramList["St"] = "st";
            paramList["Sid"] = sessionId;
            paramList["Uid"] = userId.ToString();
            paramList["ActionID"] = actionId.ToString();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    paramList[parameter.Key] = parameter.Value.ToString();
                }
            }
            RequestPackage requestPackage = new RequestPackage(0, sessionId, actionId, userId);
            requestPackage.Params = paramList;
            requestPackage.IsUrlParam = true;
            requestPackage.OpCode = opCode;
            requestPackage.Message = message;
            requestPackage.Bind(session);
            return requestPackage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDispatcher"></param>
        /// <param name="actionId"></param>
        /// <param name="session"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public static byte[] GetActionResponse(IActionDispatcher actionDispatcher, int actionId, GameSession session, RequestPackage package)
        {
            package.Bind(session);
            var actionGetter = new HttpGet(package, session);
            return ProcessActionResponse(actionDispatcher, actionId, actionGetter);
        }

        /// <summary>
        /// 获取Action处理的输出字节流
        /// </summary>
        /// <returns></returns>
        private static byte[] ProcessActionResponse(IActionDispatcher actionDispatcher, int actionId, ActionGetter actionGetter)
        {
            BaseStruct baseStruct = FindRoute(GameEnvironment.Setting.ActionTypeName, actionGetter, actionId);
            SocketGameResponse response = new SocketGameResponse();
            response.WriteErrorCallback += actionDispatcher.ResponseError;
            baseStruct.SetPush();
            baseStruct.DoInit();
            object errorTarget;
            long waitTimeOutNum;
            if (actionGetter.Session.EnterLock(actionId, actionGetter.ToString(), out errorTarget, out waitTimeOutNum))
            {
                try
                {
                    if (!baseStruct.GetError() &&
                        baseStruct.ReadUrlElement() &&
                        baseStruct.DoAction() &&
                        !baseStruct.GetError())
                    {
                        baseStruct.WriteResponse(response);
                    }
                    else
                    {
                        baseStruct.WriteErrorAction(response);
                    }
                }
                finally
                {
                    actionGetter.Session.ExitLock(actionId);
                }
            }
            else
            {
                baseStruct.WriteLockTimeoutAction(response, errorTarget, waitTimeOutNum, false);
            }
            return response.ReadByte();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseStruct"></param>
        /// <param name="actionGetter"></param>
        /// <param name="response"></param>
        public static void Process(BaseStruct baseStruct, ActionGetter actionGetter, BaseGameResponse response)
        {
            int actionId = actionGetter.GetActionId();
            baseStruct.DoInit();
            object errorTarget;
            long waitTimeOutNum;
            if (actionGetter.Session.EnterLock(actionId, actionGetter.ToString(), out errorTarget, out waitTimeOutNum))
            {
                try
                {
                    if (!baseStruct.GetError() &&
                        baseStruct.ReadUrlElement() &&
                        baseStruct.DoAction() &&
                        !baseStruct.GetError())
                    {
                        baseStruct.WriteResponse(response);
                    }
                    else
                    {
                        baseStruct.WriteErrorAction(response);
                    }
                }
                finally
                {
                    actionGetter.Session.ExitLock(actionId);
                }
            }
            else
            {
                //小于3次超时不显示提示
                baseStruct.WriteLockTimeoutAction(response, errorTarget, waitTimeOutNum, waitTimeOutNum > 3);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionId"></param>
        /// <param name="userList"></param>
        /// <param name="parameter"></param>
        /// <param name="complateHandle"></param>
        /// <param name="opCode"></param>
        /// <param name="onlineInterval"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task BroadcastAsyncAction<T>(int actionId, List<T> userList, object parameter, Action<SocketAsyncResult> complateHandle, sbyte opCode = OpCode.Binary, int onlineInterval = 0)
            where T : IUser
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                await BroadcastAction(actionId, userList, parameter, (u, s, result) =>
                {
                    if (complateHandle != null) complateHandle(result);
                }, opCode, onlineInterval);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionId"></param>
        /// <param name="userList"></param>
        /// <param name="parameter"></param>
        /// <param name="complateHandle"></param>
        /// <param name="opCode"></param>
        /// <param name="onlineInterval"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task BroadcastAction<T>(int actionId, List<T> userList, object parameter, Action<SocketAsyncResult> complateHandle, sbyte opCode = OpCode.Binary, int onlineInterval = 0)
         where T : IUser
        {
            await BroadcastAction(actionId, userList, parameter, (u, s, result) =>
            {
                if (complateHandle != null) complateHandle(result);
            }, opCode, onlineInterval);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionId"></param>
        /// <param name="userList"></param>
        /// <param name="parameter">Allow Parameters or Custom object as paramter</param>
        /// <param name="complateHandle"></param>
        /// <param name="opCode"></param>
        /// <param name="onlineInterval"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task BroadcastAction<T>(int actionId, List<T> userList, object parameter, Action<T, GameSession, SocketAsyncResult> complateHandle, sbyte opCode, int onlineInterval) where T : IUser
        {
            List<GameSession> sessionList = new List<GameSession>();
            GameSession session;
            foreach (var user in userList)
            {
                session = GameSession.Get(user.GetUserId());
                if (session == null)
                {
                    complateHandle(user, null, new SocketAsyncResult(null) { Result = ResultCode.Close });
                }
                sessionList.Add(session);
            }
            if (sessionList.Count == 0) return;

            RequestPackage package = parameter is Parameters
                ? GetResponsePackage(actionId, sessionList[0], parameter as Parameters, opCode, null)
                : GetResponsePackage(actionId, sessionList[0], null, opCode, parameter);
            await BroadcastAction(actionId, sessionList, package, (s, result) =>
            {
                if (complateHandle != null)
                {
                    complateHandle((T)s.User, s, result);
                }
            }, onlineInterval);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="sessionList"></param>
        /// <param name="package"></param>
        /// <param name="complateHandle"></param>
        /// <param name="onlineInterval"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task BroadcastAction(int actionId, List<GameSession> sessionList, RequestPackage package, Action<GameSession, SocketAsyncResult> complateHandle, int onlineInterval)
        {
            try
            {
                if (sessionList.Count == 0) return;
                if (sessionList.Exists(s => Equals(s, null)))
                {
                    throw new ArgumentNullException("Session is a null value.");
                }

                IActionDispatcher actionDispatcher = new ScutActionDispatcher();
                byte[] sendBuffer = GetActionResponse(actionDispatcher, actionId, sessionList[0], package);

                foreach (var session in sessionList)
                {
                    GameSession temp = session;
                    try
                    {
                        if (onlineInterval <= 0 || session.LastActivityTime > MathUtils.Now.AddSeconds(-onlineInterval))
                        {
                            await session.SendAsync(package.OpCode, sendBuffer, 0, sendBuffer.Length, result =>
                            {
                                if (complateHandle != null)
                                {
                                    complateHandle(temp, result);
                                }
                            });
                        }
                        else
                        {
                            if (complateHandle != null)
                            {
                                complateHandle(temp, new SocketAsyncResult(sendBuffer) { Result = ResultCode.Close });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (complateHandle != null)
                        {
                            complateHandle(temp, new SocketAsyncResult(sendBuffer) { Result = ResultCode.Error, Error = ex });
                        }
                        TraceLog.WriteError("BroadcastAction  action:{0} userId:{1} error:{2}", actionId, session.UserId, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("BroadcastAction  action:{0} error:{1}", actionId, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userList"></param>
        /// <param name="actionId"></param>
        /// <param name="parameter"></param>
        /// <param name="complateHandle"></param>
        /// <param name="opCode"></param>
        /// <param name="onlineInterval"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task SendAsyncAction<T>(List<T> userList, int actionId, object parameter, Action<SocketAsyncResult> complateHandle, sbyte opCode = OpCode.Binary, int onlineInterval = 0)
            where T : IUser
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                await SendAction(userList, actionId, parameter, (u, s, result) =>
                {
                    if (complateHandle != null) complateHandle(result);
                }, opCode, onlineInterval);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userList"></param>
        /// <param name="actionId"></param>
        /// <param name="parameter"></param>
        /// <param name="complateHandle"></param>
        /// <param name="opCode"></param>
        /// <param name="onlineInterval"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task SendAction<T>(List<T> userList, int actionId, object parameter, Action<SocketAsyncResult> complateHandle, sbyte opCode = OpCode.Binary, int onlineInterval = 0)
            where T : IUser
        {
            await SendAction(userList, actionId, parameter, (u, s, result) =>
            {
                if (complateHandle != null) complateHandle(result);
            }, opCode, onlineInterval);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userList"></param>
        /// <param name="actionId"></param>
        /// <param name="parameter">Allow Parameters or Custom object as paramter</param>
        /// <param name="complateHandle"></param>
        /// <param name="opCode"></param>
        /// <param name="onlineInterval"></param>
        public static async System.Threading.Tasks.Task SendAction<T>(List<T> userList, int actionId, object parameter, Action<T, GameSession, SocketAsyncResult> complateHandle, sbyte opCode, int onlineInterval) where T : IUser
        {
            foreach (var user in userList)
            {
                T temp = user;
                GameSession session = GameSession.Get(user.GetUserId());
                if (session != null)
                {
                    RequestPackage package = parameter is Parameters
                        ? GetResponsePackage(actionId, session, parameter as Parameters, opCode, null)
                        : GetResponsePackage(actionId, session, null, opCode, parameter);
                    await SendAction(session, actionId, package, (s, result) =>
                    {
                        if (complateHandle != null)
                        {
                            complateHandle(temp, session, result);
                        }
                    }, onlineInterval);
                }
                else
                {
                    if (complateHandle != null)
                    {
                        complateHandle(temp, null, new SocketAsyncResult(null) { Result = ResultCode.Close });
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionList"></param>
        /// <param name="actionId"></param>
        /// <param name="parameter">Allow Parameters or Custom object as paramter</param>
        /// <param name="complateHandle"></param>
        /// <param name="opCode"></param>
        /// <param name="onlineInterval"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task SendAction(List<GameSession> sessionList, int actionId, object parameter, Action<GameSession, SocketAsyncResult> complateHandle, sbyte opCode, int onlineInterval)
        {
            foreach (var session in sessionList)
            {
                GameSession temp = session;
                if (session != null)
                {
                    RequestPackage package = parameter is Parameters
                        ? GetResponsePackage(actionId, session, parameter as Parameters, opCode, null)
                        : GetResponsePackage(actionId, session, null, opCode, parameter);
                    await SendAction(session, actionId, package, (s, result) =>
                    {
                        if (complateHandle != null)
                        {
                            complateHandle(temp, result);
                        }
                    }, onlineInterval);
                }
                else
                {
                    if (complateHandle != null)
                    {
                        complateHandle(temp, new SocketAsyncResult(null) { Result = ResultCode.Close });
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="actionId"></param>
        /// <param name="package"></param>
        /// <param name="complateHandle"></param>
        /// <param name="onlineInterval"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task SendAction(GameSession session, int actionId, RequestPackage package, Action<GameSession, SocketAsyncResult> complateHandle, int onlineInterval)
        {
            IActionDispatcher actionDispatcher = new ScutActionDispatcher();
            GameSession temp = session;
            byte[] sendBuffer = null;
            try
            {
                sendBuffer = GetActionResponse(actionDispatcher, actionId, session, package);
                if ((onlineInterval <= 0 || session.LastActivityTime > MathUtils.Now.AddSeconds(-onlineInterval)))
                {
                    await session.SendAsync(package.OpCode, sendBuffer, 0, sendBuffer.Length, result =>
                    {
                        if (complateHandle != null)
                        {
                            complateHandle(temp, result);
                        }
                    });
                }
                else
                {
                    if (complateHandle != null)
                    {
                        complateHandle(temp, new SocketAsyncResult(sendBuffer) { Result = ResultCode.Close });
                    }
                }
            }
            catch (Exception ex)
            {
                if (complateHandle != null)
                {
                    complateHandle(temp, new SocketAsyncResult(sendBuffer) { Result = ResultCode.Error, Error = ex });
                }
                TraceLog.WriteError("SendToClient action:{0} userId:{1} error:{2}", actionId, session.UserId, ex);
            }
        }


        internal static BaseStruct FindScriptRoute(ActionGetter actionGetter, int actionId)
        {
            string scriptTypeName = string.Format(GameEnvironment.Setting.ScriptTypeName, actionId);
            string scriptCode = "";

            if (!ScriptEngines.SettupInfo.DisablePython) //By Seamoon 在Python禁用的情况下，就没有必要再加载了
            {
                scriptCode = string.Format("action.action{0}", actionId);
                dynamic scriptScope = ScriptEngines.ExecutePython(scriptCode);
                if (scriptScope != null)
                {
                    bool ignoreAuthorize = _ignoreAuthorizeSet.Contains(actionId);
                    if (actionGetter.Session.IsWebSocket)
                    {
                        return new JsonScriptAction(ScriptType.Python, actionId, actionGetter, scriptScope, ignoreAuthorize);
                    }
                    return new ScriptAction(ScriptType.Python, actionId, actionGetter, scriptScope, ignoreAuthorize);
                }
            }
            if (!ScriptEngines.SettupInfo.DisableLua)
            {
                scriptCode = string.Format("Action{0}", actionId);
                dynamic scriptScope = ScriptEngines.ExecuteLua("GetTable", scriptCode, actionId);
                if (scriptScope != null)
                {
                    bool ignoreAuthorize = _ignoreAuthorizeSet.Contains(actionId);
                    if (actionGetter.Session.IsWebSocket)
                    {
                        return new JsonLuaAction(actionId, actionGetter, scriptScope, ignoreAuthorize);
                    }
                    return new LuaAction(actionId, actionGetter, scriptScope, ignoreAuthorize);
                }
            }

            scriptCode = string.Format("action.action{0}", actionId);
            BaseStruct baseStruct = ScriptEngines.Execute(scriptCode, scriptTypeName, actionGetter);
            if (baseStruct != null) return baseStruct;
            return null;
        }

        internal static BaseStruct FindRoute(string typeExpression, ActionGetter actionGetter, int actionId)
        {
            BaseStruct baseStruct = FindScriptRoute(actionGetter, actionId);
            if (baseStruct != null)
            {
                return baseStruct;
            }
            //在没有找到对应的处理脚本的情况，转而尝试从已编译好的库中找
            string typeName = string.Format(typeExpression, actionId);
            Type actionType = Type.GetType(typeName);
            if (actionType != null)
            {
                try
                {
                    return actionType.CreateInstance<BaseStruct>(new object[] { actionGetter });
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("The {0} action init error", actionId), ex);
                }
            }
            throw new NotSupportedException(string.Format("Not found script action{0}, please check script path setting.", actionId));
        }
    }
}