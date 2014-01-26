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
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;

namespace ZyGames.Framework.Game.Contract.Action
{
	/// <summary>
	/// Login action.
	/// </summary>
    public abstract class LoginAction : BaseStruct
    {
		/// <summary>
		/// The type of the mobile.
		/// </summary>
        protected MobileType MobileType;
		/// <summary>
		/// The passport identifier.
		/// </summary>
        protected string PassportId = string.Empty;
		/// <summary>
		/// The password.
		/// </summary>
        protected string Password = string.Empty;
		/// <summary>
		/// The device I.
		/// </summary>
        protected string DeviceID = string.Empty;
		/// <summary>
		/// The sex.
		/// </summary>
        protected byte Sex;
		/// <summary>
		/// The name of the nick.
		/// </summary>
        protected string NickName = string.Empty;
		/// <summary>
		/// The head I.
		/// </summary>
        protected string HeadID = string.Empty;
		/// <summary>
		/// The screen x.
		/// </summary>
        protected Int16 ScreenX;
		/// <summary>
		/// The screen y.
		/// </summary>
        protected Int16 ScreenY;
		/// <summary>
		/// The retail I.
		/// </summary>
        protected string RetailID = string.Empty;
		/// <summary>
		/// The type of the user.
		/// </summary>
        protected int UserType;
		/// <summary>
		/// The server I.
		/// </summary>
        protected int ServerID;
		/// <summary>
		/// The type of the game.
		/// </summary>
        protected int GameType;
		/// <summary>
		/// The login proxy.
		/// </summary>
        protected readonly LoginProxy LoginProxy;
		/// <summary>
		/// Gets or sets the guide identifier.
		/// </summary>
		/// <value>The guide identifier.</value>
        protected int GuideId
        {
            get;
            set;
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Action.LoginAction"/> class.
		/// </summary>
		/// <param name="actionId">Action identifier.</param>
		/// <param name="httpGet">Http get.</param>
        protected LoginAction(short actionId, HttpGet httpGet)
            : base(actionId, httpGet)
        {
            LoginProxy = new LoginProxy(httpGet);
        }
		/// <summary>
		/// 创建返回协议内容输出栈
		/// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(Sid);
            PushIntoStack(Uid);
            PushIntoStack(UserType);
            PushIntoStack(MathUtils.Now.ToString("yyyy-MM-dd HH:mm"));
            PushIntoStack(GuideId);
            PushIntoStack(PassportId);
        }
		/// <summary>
		/// 接收用户请求的参数，并根据相应类进行检测
		/// </summary>
		/// <returns></returns>
        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("MobileType", ref MobileType) &&
                httpGet.GetString("Pid", ref PassportId) &&
                httpGet.GetString("Pwd", ref Password) &&
                httpGet.GetString("RetailID", ref RetailID))
            {
                httpGet.GetInt("GameType", ref GameType);
                httpGet.GetInt("ServerID", ref ServerID);
                httpGet.GetString("DeviceID", ref DeviceID);
                httpGet.GetByte("Sex", ref Sex);
                httpGet.GetString("NickName", ref NickName);
                httpGet.GetString("HeadID", ref HeadID);
                httpGet.GetWord("ScreenX", ref ScreenX);
                httpGet.GetWord("ScreenY", ref ScreenY);
                return true;
            }
            return false;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override bool CheckAction()
        {
            if (!GameEnvironment.IsRunning)
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.ServerLoading;
                return false;
            }
            return true;
        }
		/// <summary>
		/// 子类实现Action处理
		/// </summary>
		/// <returns></returns>
        public override bool TakeAction()
        {
            ILogin login = LoginProxy.GetLogin();
            if (login != null && login.CheckLogin())
            {
                Uid = login.UserID;
                Sid = Current.SessionId;//string.Format("{0}|{1}|{2}", login.SessionID, GameType, ServerID);
                UserId = Uid.ToInt();
                PassportId = login.PassportID;
                UserType = SnsManager.GetUserType(PassportId);
                SetParameter(login);
                if (!GetError() && DoSuccess(UserId))
                {
                    if (UserFactory != null)
                    {
                        var user = UserFactory(UserId);
                        if (user != null)
                        {
                            Current.User = user;
                            user.RemoteAddress = httpGet.RemoteAddress;
                            user.SocketSid = Current.SessionId;
                            httpGet.LoginSuccessCallback(UserId);
                        }
                    }
                    return true;
                }
            }
            else
            {
                Uid = string.Empty;
                Sid = string.Empty;
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.PasswordError;
            }
            return false;
        }
		/// <summary>
		/// Sets the parameter.
		/// </summary>
		/// <param name="login">Login.</param>
        protected virtual void SetParameter(ILogin login)
        {
        }

		/// <summary>
		/// 是否此请求忽略UID参数
		/// </summary>
		/// <returns></returns>
        protected override bool IsIgnoreUid()
        {
            return true;
        }
		/// <summary>
		/// Dos the success.
		/// </summary>
		/// <returns><c>true</c>, if success was done, <c>false</c> otherwise.</returns>
		/// <param name="userId">User identifier.</param>
        protected abstract bool DoSuccess(int userId);
    }
}