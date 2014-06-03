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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using IronPython.Modules;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Net;
using ZyGames.Framework.RPC.IO;
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
        /// <param name="userFactory"></param>
        public static void Request(Func<int, BaseUser> userFactory)
        {
            Request(GameEnvironment.Setting.ActionTypeName, userFactory);
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
            HttpGameResponse response = new HttpGameResponse(HttpContext.Current.Response);
            Request(typeName, httpGet, response, userFactory);
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="actionGetter"></param>
        /// <param name="response"></param>
        /// <param name="userFactory"></param>
        public static void Request(ActionGetter actionGetter, BaseGameResponse response, Func<int, BaseUser> userFactory)
        {
            Request(GameEnvironment.Setting.ActionTypeName, actionGetter, response, userFactory);
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="response"></param>
        /// <param name="userFactory"></param>
        /// <param name="actionGetter"></param>
        public static void Request(string typeName, ActionGetter actionGetter, BaseGameResponse response, Func<int, BaseUser> userFactory)
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
                    Process(action, actionGetter, response, userFactory);
                    if (action != null)
                    {
                        return;
                    }
                }
                else
                {
                    errorInfo = Language.Instance.SignError;
                    TraceLog.WriteError("Action request {3} error:{2},rl:{0},param:{1}", isRL, actionGetter.ToParamString(), errorInfo, tempName);
                }
            }
            catch (Exception ex)
            {
                errorInfo = Language.Instance.ServerBusy;
                TraceLog.WriteError("Action request {0} error:{1}\r\nparam:{2}", tempName, ex, actionGetter.ToParamString());
            }
            response.WriteError(actionGetter, Language.Instance.ErrorCode, errorInfo);
        }

        /// <summary>
        /// 请求脚本处理
        /// </summary>
        /// <param name="userFactory">创建user对象工厂</param>
        public static void RequestScript(Func<int, BaseUser> userFactory = null)
        {
            HttpGet httpGet = new HttpGet(HttpContext.Current.Request);
            BaseGameResponse response = new HttpGameResponse(HttpContext.Current.Response);
            RequestScript(httpGet, response, userFactory);
        }

        /// <summary>
        /// 请求脚本处理
        /// </summary>
        /// <param name="actionGetter">请求参数对象</param>
        /// <param name="response">字节输出处理</param>
        /// <param name="userFactory">创建user对象工厂,可为Null</param>
        public static void RequestScript(ActionGetter actionGetter, BaseGameResponse response, Func<int, BaseUser> userFactory)
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
                        Process(baseStruct, actionGetter, response, userFactory);
                        return;
                    }
                }
                else
                {
                    errorInfo = Language.Instance.SignError;
                    TraceLog.WriteError("Action request error:{2},rl:{0},param:{1}", isRl, actionGetter.ToParamString(), errorInfo);
                }
            }
            catch (Exception ex)
            {
                errorInfo = Language.Instance.ServerBusy;
                TraceLog.WriteError("Action request error:{0}\r\nparam:{1}", ex, actionGetter.ToParamString());
            }
            response.WriteError(actionGetter, Language.Instance.ErrorCode, errorInfo);
        }

        /// <summary>
        /// 获取Action处理的输出字节流
        /// </summary>
        /// <returns></returns>
        public static byte[] GetActionResponse(IActionDispatcher actionDispatcher, int actionId, BaseUser baseUser, string urlParam, out ActionGetter actionGetter)
        {
            int userId = baseUser != null ? baseUser.GetUserId() : 0;
            GameSession session = GameSession.Get(userId);
            string sessionId = session != null ? session.SessionId : "";
            string param = string.Format("MsgId={0}&St={1}&Sid={2}&Uid={3}&ActionID={4}{5}",
                0,
                "st",
                sessionId,
                userId,
                actionId,
                urlParam);

            RequestPackage requestPackage = new RequestPackage(0, sessionId, actionId, userId);
            requestPackage.UrlParam = param;
            requestPackage.IsUrlParam = true;
            requestPackage.Session = session;
            requestPackage.ReceiveTime = DateTime.Now;
            actionGetter = new HttpGet(requestPackage);
            BaseStruct baseStruct = FindRoute(GameEnvironment.Setting.ActionTypeName, actionGetter, actionId);
            SocketGameResponse response = new SocketGameResponse();
            response.WriteErrorCallback += actionDispatcher.ResponseError;
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
                        baseStruct.WriteResponse(response);
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
        /// <param name="actionGetter"></param>
        /// <param name="response"></param>
        /// <param name="userFactory"></param>
        public static void Process(BaseStruct baseStruct, ActionGetter actionGetter, BaseGameResponse response, Func<int, BaseUser> userFactory)
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
                        baseStruct.WriteResponse(response);
                    }
                    else
                    {
                        baseStruct.WriteErrorAction(response);
                    }
                }
            }
        }


        /// <summary>
        /// 将指定的Action结果广播给指定范围的玩家
        /// </summary>
        /// <typeparam name="T">BaseUser对象</typeparam>
        /// <param name="actionId">指定的Action</param>
        /// <param name="userList">指定范围的玩家</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="successHandle">成功回调</param>
        public static void BroadcastAction<T>(int actionId, List<T> userList, Parameters parameters, Action<T> successHandle) where T : BaseUser
        {
            StringBuilder shareParam = new StringBuilder();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    shareParam.AppendFormat("&{0}={1}", parameter.Key, parameter.Value);
                }
            }
            IActionDispatcher actionDispatcher = new ActionDispatcher();
            ActionGetter actionParam;
            byte[] sendData = GetActionResponse(actionDispatcher, actionId, null, shareParam.ToString(), out actionParam);
            foreach (var user in userList)
            {
                if (user == default(T))
                {
                    continue;
                }
                try
                {
                    GameSession session = GameSession.Get(user.GetUserId());
                    if (session != null)
                    {
                        if (session.SendAsync(sendData, 0, sendData.Length))
                        {
                            if (successHandle != null) successHandle(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("BroadcastAction  action:{0} userId:{1} error:{2}", actionId, user.PersonalId, ex);
                }
            }
        }

        /// <summary>
        /// 给指定范围的每个玩家发送指定的Action结果
        /// </summary>
        /// <typeparam name="T">BaseUser对象</typeparam>
        /// <param name="userList">指定范围的玩家</param>
        /// <param name="actionId">指定的Action</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="successHandle">成功回调</param>
        public static void SendAsyncAction<T>(List<T> userList, int actionId, Parameters parameters, Action<ActionGetter> successHandle) where T : BaseUser
        {
            StringBuilder shareParam = new StringBuilder();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    shareParam.AppendFormat("&{0}={1}", parameter.Key, parameter.Value);
                }
            }
            IActionDispatcher actionDispatcher = new ActionDispatcher();
            foreach (var user in userList)
            {
                if (user == default(T))
                {
                    continue;
                }
                try
                {
                    var session = GameSession.Get(user.GetUserId());
                    ActionGetter actionParam;
                    byte[] sendData = GetActionResponse(actionDispatcher, actionId, user, shareParam.ToString(), out actionParam);
                    if (session != null &&
                        session.SendAsync(sendData, 0, sendData.Length) &&
                        actionParam != null)
                    {
                        successHandle(actionParam);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("SendToClient action:{0} userId:{1} error:{2}", actionId, user.PersonalId, ex);
                }
            }
        }


        internal static BaseStruct FindScriptRoute(ActionGetter actionGetter, int actionID)
        {
            string scriptTypeName = string.Format(GameEnvironment.Setting.ScriptTypeName, actionID);
            string scriptCode = "";

            if (!ScriptEngines.SettupInfo.DisablePython) //By Seamoon 在Python禁用的情况下，就没有必要再加载了
            {
                scriptCode = string.Format("action.action{0}", actionID);
                dynamic scriptScope = ScriptEngines.ExecutePython(scriptCode);
                if (scriptScope != null)
                {
                    bool ignoreAuthorize = _ignoreAuthorizeSet.Contains(actionID);
                    return new ScriptAction(ScriptType.Python, actionID, actionGetter, scriptScope, ignoreAuthorize);
                }
            }
            if (!ScriptEngines.SettupInfo.DisableLua)
            {
                scriptCode = string.Format("Action{0}", actionID);
                dynamic scriptScope = ScriptEngines.ExecuteLua("GetTable", scriptCode, actionID);
                if (scriptScope != null)
                {
                    bool ignoreAuthorize = _ignoreAuthorizeSet.Contains(actionID);
                    return new LuaAction(actionID, actionGetter, scriptScope, ignoreAuthorize);
                }
            }

            scriptCode = string.Format("action.action{0}", actionID);
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