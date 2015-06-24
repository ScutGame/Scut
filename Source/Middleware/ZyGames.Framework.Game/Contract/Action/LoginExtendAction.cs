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
		/// <summary>
		/// The refesh token.
		/// </summary>
        protected string RefeshToken;
		/// <summary>
		/// The scope.
		/// </summary>
        protected string Scope;
		/// <summary>
		/// The qihoo user I.
		/// </summary>
        protected string QihooUserID;
		/// <summary>
		/// The access token360.
		/// </summary>
        protected string AccessToken360;
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Action.LoginExtendAction"/> class.
		/// </summary>
		/// <param name="actionId">Action identifier.</param>
		/// <param name="httpGet">Http get.</param>
        protected LoginExtendAction(short actionId, ActionGetter httpGet)
            : base(actionId, httpGet)
        {
        }
		/// <summary>
		/// 创建返回协议内容输出栈
		/// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(Current.SessionId);
            PushIntoStack(Current.UserId.ToNotNullString());
            PushIntoStack(UserType);
            PushIntoStack(MathUtils.Now.ToString("yyyy-MM-dd HH:mm"));
            PushIntoStack(GuideId);
            PushIntoStack(PassportId);
            PushIntoStack(AccessToken360);
            PushIntoStack(RefeshToken);
            PushIntoStack(QihooUserID);
            PushIntoStack(Scope);
        }
		/// <summary>
		/// Sets the parameter.
		/// </summary>
		/// <param name="login">Login.</param>
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