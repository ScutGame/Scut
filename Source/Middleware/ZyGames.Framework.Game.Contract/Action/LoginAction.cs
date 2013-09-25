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
                Sid = string.Format("{0}|{1}|{2}", login.SessionID, GameType, ServerID);
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
