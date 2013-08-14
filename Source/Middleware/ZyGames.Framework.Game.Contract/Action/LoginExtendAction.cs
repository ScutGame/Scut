using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;

namespace ZyGames.Framework.Game.Contract.Action
{
    /// <summary>
    /// 提供扩展渠道登录
    /// </summary>
    public abstract class LoginExtendAction : LoginAction
    {
        protected string RefeshToken;
        protected string Scope;
        protected string QihooUserID;
        protected string AccessToken360;

        protected LoginExtendAction(short actionId, HttpGet httpGet)
            : base(actionId, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(Sid);
            PushIntoStack(Uid);
            PushIntoStack(UserType);
            PushIntoStack(MathUtils.Now.ToString("yyyy-MM-dd HH:mm"));
            PushIntoStack(GuideId);
            PushIntoStack(PassportId);
            PushIntoStack(AccessToken360);
            PushIntoStack(RefeshToken);
            PushIntoStack(QihooUserID);
            PushIntoStack(Scope);
        }

        protected override void SetParameter(ILogin login)
        {
            AbstractLogin baseLogin = login as AbstractLogin;
            if (baseLogin != null)
            {
                //AccessToken = baseLogin.AccessToken;
                RefeshToken = baseLogin.RefeshToken;
                QihooUserID = baseLogin.QihooUserID;
                Scope = baseLogin.Scope;
                AccessToken360 = baseLogin.AccessToken;
            }
        }

       
    }
}
