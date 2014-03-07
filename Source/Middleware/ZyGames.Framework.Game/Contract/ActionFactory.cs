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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
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
            Request(GameEnvironment.Setting.ActionTypeName, httpGet, response, userFactory);
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
            int actionID = httpGet.ActionId;
            string tempName = string.Format(typeName, httpGet.ActionId);
            string errorInfo = "";
            try
            {
                bool isRL = BaseStruct.CheckRunloader(httpGet);
                if (isRL || httpGet.CheckSign())
                {
                    BaseStruct action = FindRoute(typeName, httpGet, actionID);
                    Process(action, httpGet, response, userFactory);
                    if (action != null)
                    {
                        return;
                    }
                }
                else
                {
                    errorInfo = "Signature failure";
                    TraceLog.WriteError("Action request {3} error:{2},rl:{0},param:{1}", isRL, httpGet.ParamString, errorInfo, tempName);
                }
            }
            catch (Exception ex)
            {
                errorInfo = "Unknown error";
                TraceLog.WriteError("Action request {0} error:{1}\r\nparam:{2}", tempName, ex, httpGet.ParamString);
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
            int actionID = httpGet.ActionId;
            string errorInfo = "";
            try
            {
                bool isRl = BaseStruct.CheckRunloader(httpGet);
                if (isRl || httpGet.CheckSign())
                {
                    BaseStruct baseStruct = FindScriptRoute(httpGet, actionID);
                    if (baseStruct != null)
                    {
                        Process(baseStruct, httpGet, response, userFactory);
                        return;
                    }
                }
                else
                {
                    errorInfo = "Sign Error";
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
            response.BinaryWrite(sb.PopBuffer());
        }

        /// <summary>
        /// 获取Action处理的输出字节流
        /// </summary>
        /// <returns></returns>
        public static byte[] GetActionResponse(int actionId, BaseUser baseUser, string parameters, out HttpGet httpGet)
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
                parameters);
            httpGet = new HttpGet(param, session);
            BaseStruct baseStruct = FindRoute(GameEnvironment.Setting.ActionTypeName, httpGet, actionId);
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
            HttpGet httpGet;
            byte[] sendData = GetActionResponse(actionId, null, shareParam.ToString(), out httpGet);
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
        public static void SendAsyncAction<T>(List<T> userList, int actionId, Parameters parameters, Action<HttpGet> successHandle) where T : BaseUser
        {
            StringBuilder shareParam = new StringBuilder();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    shareParam.AppendFormat("&{0}={1}", parameter.Key, parameter.Value);
                }
            }
            foreach (var user in userList)
            {
                if (user == default(T))
                {
                    continue;
                }
                try
                {
                    var session = GameSession.Get(user.GetUserId());
                    HttpGet httpGet;
                    byte[] sendData = GetActionResponse(actionId, user, shareParam.ToString(), out httpGet);
                    if (session != null &&
                        session.SendAsync(sendData, 0, sendData.Length) &&
                        httpGet != null)
                    {
                        successHandle(httpGet);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("SendToClient action:{0} userId:{1} error:{2}", actionId, user.PersonalId, ex);
                }
            }
        }


        internal static BaseStruct FindScriptRoute(HttpGet httpGet, int actionID)
        {
            string scriptTypeName = string.Format(GameEnvironment.Setting.ScriptTypeName, actionID);
            string scriptCode = "";

            if (!ScriptEngines.DisablePython) //By Seamoon 在Python禁用的情况下，就没有必要再加载了
            {
                scriptCode = string.Format("{0}/action/action{1}.py", ScriptEngines.PythonDirName, actionID);
                dynamic scriptScope = ScriptEngines.Execute(scriptCode, null);
                if (scriptScope != null)
                {
                    bool ignoreAuthorize = _ignoreAuthorizeSet.Contains(actionID);
                    return new ScriptAction((short)actionID, httpGet, scriptScope, ignoreAuthorize);
                }
            }

            scriptCode = string.Format("{0}/action/action{1}.cs", ScriptEngines.CSharpDirName, actionID);
            BaseStruct baseStruct = ScriptEngines.Execute(scriptCode, scriptTypeName, httpGet);
            if (baseStruct != null) return baseStruct;
            return null;
        }

        internal static BaseStruct FindRoute(string typeExpression, HttpGet httpGet, int actionID)
        {
            BaseStruct baseStruct = FindScriptRoute(httpGet, actionID);
            if (baseStruct != null)
            {
                return baseStruct;
            }
            //在没有找到对应的处理脚本的情况，转而尝试从已编译好的库中找
            string typeName = string.Format(typeExpression, actionID);
            Type actionType = Type.GetType(typeName);
            if (actionType != null)
            {
                try
                {
                    return actionType.CreateInstance<BaseStruct>(new object[] { httpGet });
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("The {0} action init error", actionID), ex);
                }
            }
            throw new NotSupportedException(string.Format("Not found {0} action Interface.", actionID));
        }
    }
}