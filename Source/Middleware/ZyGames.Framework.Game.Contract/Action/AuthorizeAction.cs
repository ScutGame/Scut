using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Com.Generic;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
    /// <summary>
    /// 授权访问的Action
    /// </summary>
    public abstract class AuthorizeAction : BaseStruct
    {
        protected AuthorizeAction(short actionID, HttpGet httpGet)
            : base(actionID, httpGet)
        {
        }

        /// <summary>
        /// 开启支付通知
        /// </summary>
        public PaymentNotify EnablePayNotify
        {
            get;
            protected set;
        }

        public override bool CheckAction()
        {
            bool result = false;

            if (!GameEnvironment.IsRunning)
            {
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                ErrorInfo = LanguageHelper.GetLang().ServerLoading;
                return false;
            }
            if (IgnoreActionId)
            {
                return true;
            }
            BaseUser gameUser;
            LoginStatus status = CheckUser(Sid, UserId, out gameUser);

            if (IsRunLoader)
            {
                status = LoginStatus.Success;
            }
            switch (status)
            {
                case LoginStatus.NoLogin:
                    ErrorCode = LanguageHelper.GetLang().TimeoutCode;
                    ErrorInfo = LanguageHelper.GetLang().AcountNoLogin;
                    result = false;
                    break;
                case LoginStatus.Logined:
                    ErrorCode = LanguageHelper.GetLang().TimeoutCode;
                    ErrorInfo = LanguageHelper.GetLang().AcountLogined;
                    result = false;
                    break;
                case LoginStatus.Success:
                    if (Current != null)
                    {
                        Current.User = gameUser;
                    }
                    result = true;
                    break;
                default:
                    break;
            }
            if (gameUser != null && gameUser.IsFengJinStatus)
            {
                ErrorCode = LanguageHelper.GetLang().TimeoutCode;
                ErrorInfo = LanguageHelper.GetLang().AcountIsLocked;
                result = false;
            }
            if (result && IsRefresh)
            {
                DoRefresh(actionId, gameUser);
            }
            return result;
        }

        protected virtual bool IsRefresh
        {
            get { return true; }
        }
        /// <summary>
        /// 不检查的ActionID
        /// </summary>
        protected abstract bool IgnoreActionId
        {
            get;
        }

        protected LoginStatus CheckUser(string sessionID, int userId, out BaseUser gameUser)
        {
            gameUser = null;
            if (UserFactory != null)
            {
                gameUser = UserFactory(userId);
                if (gameUser != null)
                {
                    string currSid = gameUser.GetSessionId();
                    if (!string.IsNullOrEmpty(currSid))
                    {
                        return currSid == sessionID ? LoginStatus.Success : LoginStatus.Logined;
                    }
                }
            }
            return LoginStatus.NoLogin;
        }

        protected void DoRefresh(int actionId, BaseUser gameUser)
        {
            if (EnablePayNotify != null)
            {
                EnablePayNotify.Notify(gameUser);
            }
            if (gameUser != null)
            {
                gameUser.RefleshOnlineDate();
            }
        }
    }
}
