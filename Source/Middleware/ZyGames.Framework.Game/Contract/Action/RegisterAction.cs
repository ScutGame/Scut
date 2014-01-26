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
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
	/// <summary>
	/// Register action.
	/// </summary>
    public abstract class RegisterAction : BaseStruct
    {
		/// <summary>
		/// The name of the user.
		/// </summary>
        protected string UserName;
		/// <summary>
		/// The sex.
		/// </summary>
        protected byte Sex;
		/// <summary>
		/// The head I.
		/// </summary>
        protected string HeadID;
		/// <summary>
		/// The retail I.
		/// </summary>
        protected string RetailID;
		/// <summary>
		/// The pid.
		/// </summary>
        protected string Pid;
		/// <summary>
		/// The type of the mobile.
		/// </summary>
        protected MobileType MobileType;
		/// <summary>
		/// The screen x.
		/// </summary>
        protected short ScreenX;
		/// <summary>
		/// The screen y.
		/// </summary>
        protected short ScreenY;
		/// <summary>
		/// The req app version.
		/// </summary>
        protected short ReqAppVersion;
		/// <summary>
		/// The game I.
		/// </summary>
        protected int GameID;
		/// <summary>
		/// The server I.
		/// </summary>
        protected int ServerID;
		/// <summary>
		/// The device I.
		/// </summary>
        protected string DeviceID;
		/// <summary>
		/// Gets or sets the guide identifier.
		/// </summary>
		/// <value>The guide identifier.</value>
        public int GuideId { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Action.RegisterAction"/> class.
		/// </summary>
		/// <param name="aActionId">A action identifier.</param>
		/// <param name="httpGet">Http get.</param>
        protected RegisterAction(short aActionId, HttpGet httpGet)
            : base(aActionId, httpGet)
        {
        }
		/// <summary>
		/// 创建返回协议内容输出栈
		/// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(GuideId);
        }
		/// <summary>
		/// 接收用户请求的参数，并根据相应类进行检测
		/// </summary>
		/// <returns></returns>
        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserName", ref UserName) &&
                httpGet.GetByte("Sex", ref Sex) &&
                httpGet.GetString("HeadID", ref HeadID) &&
                httpGet.GetString("RetailID", ref RetailID) &&
                httpGet.GetString("Pid", ref Pid, 1, int.MaxValue) &&
                httpGet.GetEnum("MobileType", ref MobileType)
                )
            {
                UserName = UserName.Trim();
                httpGet.GetWord("ScreenX", ref ScreenX);
                httpGet.GetWord("ScreenY", ref ScreenY);
                httpGet.GetWord("ClientAppVersion", ref ReqAppVersion);
                httpGet.GetString("DeviceID", ref DeviceID);
                httpGet.GetInt("GameID", ref GameID);
                httpGet.GetInt("ServerID", ref ServerID);
                return GetActionParam();
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
            if (UserId <= 0)
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.UrlElement;
                return false;
            }

            return true;
        }
		/// <summary>
		/// 处理结束执行
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
        public override void TakeActionAffter(bool state)
        {
            if(state)
            {
                var user = UserFactory(UserId);
                if (user != null)
                {
                    user.RemoteAddress = httpGet.RemoteAddress;
                    user.SocketSid = Current.SessionId;
                    httpGet.LoginSuccessCallback(UserId);
                }
            }
        }
        /// <summary>
        /// Gets the action parameter.
        /// </summary>
        /// <returns><c>true</c>, if action parameter was gotten, <c>false</c> otherwise.</returns>
        protected abstract bool GetActionParam();

    }
}