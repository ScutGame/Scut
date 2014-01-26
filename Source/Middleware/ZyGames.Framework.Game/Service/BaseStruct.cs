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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Framework.Game.Service
{
	/// <summary>
	/// Base struct.
	/// </summary>
    public abstract class BaseStruct : GameStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public static string PublishType = ConfigUtils.GetSetting("PublishType", "Release");

        /// <summary>
        /// 本次登录SessionID句柄
        /// </summary>
        protected string Sid;
        /// <summary>
        /// 提交Action用户唯一ID
        /// </summary>
        protected string Uid;

        private int _userId;
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            get
            {
                return _userId;
            }
            set { _userId = value; }
        }
        /// <summary>
        /// User创建工厂
        /// </summary>
        public Func<int, BaseUser> UserFactory { get; set; }

        /// <summary>
        /// 当前游戏上下文对象
        /// </summary>
        public GameContext Current { get; private set; }

        /// <summary>
        /// url分解器
        /// </summary>
        protected HttpGet httpGet;

        /// <summary>
        /// 是否是压力测试
        /// </summary>
        protected bool IsRunLoader
        {
            get
            {
                return CheckRunloader(httpGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool IsRealse
        {
            get
            {
                return PublishType.Equals("Release", StringComparison.CurrentCultureIgnoreCase);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpGet"></param>
        /// <returns></returns>
        public static bool CheckRunloader(HttpGet httpGet)
        {
            return !IsRealse && httpGet.ContainsKey("rl");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aActionId"></param>
        /// <param name="httpGet"></param>
        protected BaseStruct(short aActionId, HttpGet httpGet) :
            base(aActionId)
        {
            actionId = aActionId;
            this.httpGet = httpGet;
        }

        /// <summary>
        /// 子类实现
        /// </summary>
        protected virtual void InitChildAction()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoInit()
        {
            Uid = "0";
            httpGet.GetString("uid", ref Uid);
            httpGet.GetInt("uid", ref _userId);

            if (!IsPush)
            {
                httpGet.GetInt("MsgId", ref MsgId);
            }
            string st = httpGet.GetStringValue("St");
            if (!string.IsNullOrEmpty(st))
            {
                St = st;
            }
            InitContext(httpGet.SessionId, actionId, UserId);
            InitAction();
            InitChildAction();
        }

        private void InitContext(string ssid, int actionId, int userId)
        {
            Current = GameContext.GetInstance(ssid, actionId, userId);
            if (UserFactory != null)
            {
                Current.User = UserFactory(userId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILocking RequestLock()
        {
            ILocking strategy = null;
            strategy = Current.MonitorLock.Lock();
            if (strategy == null || !strategy.TryEnterLock())
            {
                ErrorCode = Language.Instance.ErrorCode;
                if (!IsRealse) ErrorInfo = Language.Instance.ServerBusy;
                TraceLog.WriteError("Action-{0} Uid:{1} locked timeout.", actionId, UserId);
            }
            return strategy;
        }

        /// <summary>
        /// 子类实现Action处理
        /// </summary>
        /// <returns></returns>
        public abstract bool TakeAction();

        /// <summary>
        /// 处理结束执行
        /// </summary>
        public virtual void TakeActionAffter(bool state)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckAction()
        {
            return true;
        }

        /// <summary>
        /// 执行Action处理
        /// </summary>
        /// <returns></returns>
        public override bool DoAction()
        {
            bool result;
            try
            {
                if (!CheckAction())
                {
                    return false;
                }
                result = TakeAction();
                if(Current.UserId == 0 && UserId > 0)
                {
                    Current.SetValue(UserId);
                }
                TakeActionAffter(result);
            }
            catch (Exception ex)
            {
                SaveLog(ex);
                ErrorCode = Language.Instance.ErrorCode;
                if (!IsRealse)
                {
                    ErrorInfo = Language.Instance.ServerBusy;
                }
                return false;
            }
            return result;
        }

        /// <summary>
        /// 读取Url参数
        /// </summary>
        /// <returns></returns>
        public override bool ReadUrlElement()
        {
            //调整加锁位置
            bool result = false;
            httpGet.GetString("Sid", ref Sid);
            if (GetUrlElement())
            {
                if (ValidateElement())
                {
                    result = true;
                }
                else if (ErrorCode == 0)
                {
                    ErrorCode = Language.Instance.ValidateCode;
                    if (!IsRealse)
                    {
                        ErrorInfo = Language.Instance.ValidateError;
                    }
                }
            }
            else
            {
                if (ErrorCode == 0)
                {
                    ErrorCode = Language.Instance.ErrorCode;
                    if (!IsRealse)
                    {
                        ErrorInfo = Language.Instance.UrlElement + httpGet.ErrorMsg;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorInfo"></param>
        public void DebugTips(string errorInfo)
        {
            DebugTips(errorInfo, Language.Instance.ErrorCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <param name="errorCode"></param>
        public void DebugTips(string errorInfo, int errorCode)
        {
            ErrorCode = errorCode;
            if (!IsRealse)
            {
                ErrorInfo = errorInfo;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorInfo"></param>
        public void Tips(string errorInfo)
        {
            Tips(errorInfo, Language.Instance.ErrorCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <param name="errorCode"></param>
        public void Tips(string errorInfo, int errorCode)
        {
            ErrorCode = errorCode;
            ErrorInfo = errorInfo;
        }

        /// <summary>
        /// 较验参数
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateElement()
        {
            return true;
        }

        /// <summary>
        /// 是否此请求忽略UID参数
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsIgnoreUid()
        {
            return false;
        }


        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="actionStat"></param>
        /// <param name="cont"></param>
        protected override void SaveActionLogToDB(LogActionStat actionStat, string cont)
        {
            try
            {
                ActionCount.ActionVisit(actionId, actionStat);

            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }
        }
    }
}