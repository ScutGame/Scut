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
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Action.AuthorizeAction"/> class.
		/// </summary>
		/// <param name="actionID">Action I.</param>
		/// <param name="httpGet">Http get.</param>
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
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override bool CheckAction()
        {
            bool result = false;

            if (!GameEnvironment.IsRunning)
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.ServerLoading;
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
                    ErrorCode = Language.Instance.TimeoutCode;
                    ErrorInfo = Language.Instance.AcountNoLogin;
                    result = false;
                    break;
                case LoginStatus.Logined:
                    ErrorCode = Language.Instance.TimeoutCode;
                    ErrorInfo = Language.Instance.AcountLogined;
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
                ErrorCode = Language.Instance.TimeoutCode;
                ErrorInfo = Language.Instance.AcountIsLocked;
                result = false;
            }
            if (result && IsRefresh)
            {
                DoRefresh(actionId, gameUser);
            }
            return result;
        }
		/// <summary>
		/// Gets a value indicating whether this instance is refresh.
		/// </summary>
		/// <value><c>true</c> if this instance is refresh; otherwise, <c>false</c>.</value>
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
		/// <summary>
		/// Checks the user.
		/// </summary>
		/// <returns>The user.</returns>
		/// <param name="sessionID">Session I.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="gameUser">Game user.</param>
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
		/// <summary>
		/// Dos the refresh.
		/// </summary>
		/// <param name="actionId">Action identifier.</param>
		/// <param name="gameUser">Game user.</param>
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