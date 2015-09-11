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

using ZyGames.Framework.Game.Context;
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
        protected RegisterAction(short aActionId, ActionGetter httpGet)
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
            if (actionGetter.GetString("UserName", ref UserName) &&
                actionGetter.GetByte("Sex", ref Sex) &&
                actionGetter.GetString("HeadID", ref HeadID) &&
                actionGetter.GetString("RetailID", ref RetailID) &&
                actionGetter.GetString("Pid", ref Pid, 1, int.MaxValue) &&
                actionGetter.GetEnum("MobileType", ref MobileType)
                )
            {
                UserName = UserName.Trim();
                actionGetter.GetWord("ScreenX", ref ScreenX);
                actionGetter.GetWord("ScreenY", ref ScreenY);
                actionGetter.GetWord("ClientAppVersion", ref ReqAppVersion);
                actionGetter.GetString("DeviceID", ref DeviceID);
                actionGetter.GetInt("GameID", ref GameID);
                actionGetter.GetInt("ServerID", ref ServerID);
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
            if (Current.UserId <= 0)
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.UrlElement;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool TakeAction()
        {
            IUser user;
            if (CreateUserRole(out user) && Current != null && user != null)
            {
                Current.Bind(user);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 处理结束执行
        /// </summary>
        /// <param name="state">If set to <c>true</c> state.</param>
        public override void TakeActionAffter(bool state)
        {
        }

        /// <summary>
        /// Gets the action parameter.
        /// </summary>
        /// <returns><c>true</c>, if action parameter was gotten, <c>false</c> otherwise.</returns>
        protected abstract bool GetActionParam();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool CreateUserRole(out IUser user);

    }
}