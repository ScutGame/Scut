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
    public abstract class LoginAction : BaseStruct
    {
        protected MobileType MobileType;
        protected string PassportId = string.Empty;
        protected string Password = string.Empty;
        protected string DeviceID = string.Empty;
        protected byte Sex;
        protected string NickName = string.Empty;
        protected string HeadID = string.Empty;
        protected Int16 ScreenX;
        protected Int16 ScreenY;
        protected string RetailID = string.Empty;
        protected int UserType;
        protected int ServerID;
        protected int GameType;
        protected readonly LoginProxy LoginProxy;
        protected int GuideId
        {
            get;
            set;
        }

        protected LoginAction(short actionId, HttpGet httpGet)
            : base(actionId, httpGet)
        {
            LoginProxy = new LoginProxy(httpGet);
        }

        public override void BuildPacket()
        {
            PushIntoStack(Sid);
            PushIntoStack(Uid);
            PushIntoStack(UserType);
            PushIntoStack(MathUtils.Now.ToString("yyyy-MM-dd HH:mm"));
            PushIntoStack(GuideId);
            PushIntoStack(PassportId);
        }

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

        public override bool CheckAction()
        {
            if (!GameEnvironment.IsRunning)
            {
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                ErrorInfo = LanguageHelper.GetLang().ServerLoading;
                return false;
            }
            return true;
        }

        public override bool TakeAction()
        {
            ILogin login = LoginProxy.GetLogin();
            if (login != null && login.CheckLogin())
            {
                Uid = login.UserID;
                Sid = httpGet.SessionId;//string.Format("{0}|{1}|{2}", login.SessionID, GameType, ServerID);
                UserId = Uid.ToInt();
                PassportId = login.PassportID;
                UserType = SnsManager.GetUserType(PassportId);
                SetParameter(login);
                InitContext(actionId, UserId);
                using (RequestLock())
                {
                    if (!GetError() && DoSuccess(UserId))
                    {
                        if (UserFactory != null)
                        {
                            var user = UserFactory(UserId);
                            if (user != null)
                            {
                                user.RemoteAddress = httpGet.RemoteAddress;
                                user.SocketSid = httpGet.SessionId;
                                httpGet.LoginSuccessCallback(UserId);
                            }
                        }
                        return true;
                    }
                }
            }
            else
            {
                Uid = string.Empty;
                Sid = string.Empty;
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                ErrorInfo = LanguageHelper.GetLang().PasswordError;
            }
            return false;
        }

        protected virtual void SetParameter(ILogin login)
        {
        }


        protected override bool IsIgnoreUid()
        {
            return true;
        }

        protected abstract bool DoSuccess(int userId);
    }
}