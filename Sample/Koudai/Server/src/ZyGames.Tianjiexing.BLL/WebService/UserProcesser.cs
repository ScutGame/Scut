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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.WebService
{
    public class UserProcesser : BaseDataProcesser
    {
        public override void Process(JsonParameter[] paramList)
        {
            JsonParameterList parameters = JsonParameter.Convert(paramList);
            string op = parameters["op"];
            string userID = parameters["UserID"];
            GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(userID);

            OAOperationLog oAOperationLog = new OAOperationLog();
            oAOperationLog.ID = Guid.NewGuid().ToString();
            oAOperationLog.UserID = userID;
            oAOperationLog.OpUserID = Convert.ToInt32(parameters["OpUserID"]);
            oAOperationLog.CreateDate = DateTime.Now;
            if (parameters["EndDate"].Length > 0)
            {
                oAOperationLog.EndDate = Convert.ToDateTime(parameters["EndDate"]);
            }

            oAOperationLog.Reason = parameters["Reason"];
            if (gameUser == null)
            {
                return;
            }
            if (op == "disableId")
            {
                gameUser.UserStatus = UserStatus.FengJin;
                oAOperationLog.OpType = 1;
            }
            else if (op == "enableId")
            {
                gameUser.UserStatus = UserStatus.Normal;
                oAOperationLog.OpType = 2;
            }
            else if (op == "disableMsg")
            {
                gameUser.MsgState = false;
                oAOperationLog.OpType = 3;
            }
            else if (op == "enableMsg")
            {
                gameUser.MsgState = true;
                oAOperationLog.OpType = 4;
            }
            else
            {
                return;
            }
            //gameUser.Update();
            var sender = DataSyncManager.GetDataSender();
            sender.Send(oAOperationLog);
        }
    }
}