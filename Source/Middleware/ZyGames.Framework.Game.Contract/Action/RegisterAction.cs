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
    public abstract class RegisterAction : BaseStruct
    {
        protected string UserName;
        protected byte Sex;
        protected string HeadID;
        protected string RetailID;
        protected string Pid;
        protected MobileType MobileType;
        protected short ScreenX;
        protected short ScreenY;
        protected short ReqAppVersion;
        protected int GameID;
        protected int ServerID;
        protected string DeviceID;
        public int GuideId { get; set; }

        protected RegisterAction(short aActionId, HttpGet httpGet)
            : base(aActionId, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(GuideId);
        }

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

        public override bool CheckAction()
        {
            if (!GameEnvironment.IsRunning)
            {
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                ErrorInfo = LanguageHelper.GetLang().ServerLoading;
                return false;
            }
            if (UserId <= 0)
            {
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                ErrorInfo = LanguageHelper.GetLang().UrlElement;
                return false;
            }

            return true;
        }
        
        protected abstract bool GetActionParam();

    }
}