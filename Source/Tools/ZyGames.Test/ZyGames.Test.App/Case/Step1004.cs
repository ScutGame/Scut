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
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Test.App.Case
{
    /// <summary>
    /// 登录
    /// </summary>
    public class Step1004 : CaseStep
    {
        protected override void SetUrlElement()
        {
            _session.Context.PassportId = "Z" + (_session.Setting.PassprotId + _session.Id);
            string pwd = EncodePassword(_session.Setting.UserPwd);
            SetRequestParam("MobileType", 1);
            SetRequestParam("Pid", _session.Context.PassportId);
            SetRequestParam("Pwd", pwd);
            SetRequestParam("DeviceID", "0a-0s-0f-04");
            SetRequestParam("GameType", _session.Setting.GameId);
            SetRequestParam("ServerID", _session.Setting.ServerId);
            SetRequestParam("ScreenX", "500");
            SetRequestParam("ScreenY", "400");
            SetRequestParam("RetailID", "0000");
            SetRequestParam("RetailUser", "");
            SetRequestParam("ClientAppVersion", "1");

        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
            _session.Context.SessionId = reader.ReadString();
            _session.Context.UserId = reader.ReadString().ToInt();
            int UserType = reader.ReadInt();
            string LoginTime = reader.ReadString();
            int GuideID = reader.ReadInt();
            if (GuideID == 1005)
            {
                SetChildStep("1005");
            }
            return true;
        }

    }
}