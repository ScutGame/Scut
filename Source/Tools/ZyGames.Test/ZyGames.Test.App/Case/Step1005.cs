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

using ZyGames.Framework.RPC.IO;

namespace ZyGames.Test.App.Case
{
    /// <summary>
    /// 创角
    /// </summary>
    public class Step1005 : CaseStep
    {
        protected override void SetUrlElement()
        {
            int number = _session.Setting.PassprotId + _session.Id;
            var pid = "Z" + number;
            SetRequestParam("UserName", "NName" + number);
            SetRequestParam("Sex", 1);
            SetRequestParam("HeadID", 0);
            SetRequestParam("RetailID", "0000");
            SetRequestParam("Pid", pid);
            SetRequestParam("MobileType", 1);
            SetRequestParam("DeviceID", "");
            SetRequestParam("ScreenX", 100);
            SetRequestParam("ScreenY", 100);
            SetRequestParam("ClientAppVersion", 1);
            SetRequestParam("ProfessionType", 1);
            SetRequestParam("PointX", 100);
            SetRequestParam("PointY", 100);

        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
            _session.Context.UserId = reader.ReadInt();
            return true;
        }

    }
}