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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Config;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TipException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorInfo"></param>
        public TipException(string errorInfo)
            : base(errorInfo)
        {

        }
    }
    /// <summary>
    /// Base struct.
    /// </summary>
    public abstract class BaseStruct : GameStruct
    {
        /// <summary>
        /// 兼容子类变量名
        /// </summary>
        protected ActionGetter httpGet
        {
            get { return actionGetter; }
        }
        /// <summary>
        /// 是否是压力测试
        /// </summary>
        protected bool IsRunLoader
        {
            get
            {
                return CheckRunloader(actionGetter);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool IsRealse
        {
            get
            {
                var section = ConfigManager.Configger.GetFirstOrAddConfig<AppServerSection>();
                return section.PublishType.Equals("Release", StringComparison.OrdinalIgnoreCase);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionGetter"></param>
        /// <returns></returns>
        public static bool CheckRunloader(ActionGetter actionGetter)
        {
            return !IsRealse && actionGetter.IsRunloader();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aActionId"></param>
        /// <param name="actionGetter"></param>
        protected BaseStruct(int aActionId, ActionGetter actionGetter) :
            base(aActionId)
        {
            actionId = aActionId;
            this.actionGetter = actionGetter;
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
            if (!IsPush)
            {
                MsgId = actionGetter.GetMsgId();
            }
            string st = actionGetter.GetSt();
            if (!string.IsNullOrEmpty(st))
            {
                St = st;
            }
            Sid = actionGetter.SessionId;
            Current = actionGetter.Session;
            InitAction();
            InitChildAction();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="errorTarget"></param>
        /// <param name="waitTimeOutNum"></param>
        /// <param name="isWriteInfo"></param>
        public void WriteLockTimeoutAction(BaseGameResponse response, object errorTarget, long waitTimeOutNum, bool isWriteInfo = true)
        {
            ErrorCode = Language.Instance.LockTimeoutCode;
            if (isWriteInfo && !IsRealse)
            {
                ErrorInfo = Language.Instance.RequestTimeout;
                TraceLog.WriteError("Request action-{0} locked timeout[{3}].\r\nLocked target:{1}\r\nUrl:{2}",
                    actionId, errorTarget, actionGetter.ToString(), waitTimeOutNum);
            }
            else
            {
                TraceLog.WriteDebug("Request action-{0} locked timeout[{3}].\r\nLocked target:{1}\r\nUrl:{2}",
                    actionId, errorTarget, actionGetter.ToString(), waitTimeOutNum);
            }

            WriteErrorAction(response);
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
                TakeActionAffter(result);
            }
            catch (TipException tip)
            {
                Tips(tip.Message);
                return false;
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
                        ErrorInfo = Language.Instance.UrlElement;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// get url parameter
        /// </summary>
        /// <returns></returns>
        public override bool GetUrlElement()
        {
            return true;
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
        /// 刷新时间缀
        /// </summary>
        protected virtual void RefleshSt()
        {
            St = MathUtils.UnixEpochTimeSpan.TotalSeconds.ToCeilingInt().ToString();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        protected virtual string DecodePassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password)) return password;
                return new DESAlgorithmNew().DecodePwd(password, GameEnvironment.Setting.ClientDesDeKey);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Decode password:\"{0}\" error:{1}", password, ex);
            }
            return password;
        }

    }
}