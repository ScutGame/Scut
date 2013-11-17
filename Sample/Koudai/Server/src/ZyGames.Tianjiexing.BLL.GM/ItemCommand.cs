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
using ZyGames.Framework;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.GM
{
    /// <summary>
    /// 物品命令
    /// </summary>
    public class ItemCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int itemID = args.Length > 0 ? args[0].Trim().ToInt() : 0;
            int itemNum = args.Length > 1 ? args[1].Trim().ToInt() : 1;
            UserItemHelper.AddUserItem(UserID, itemID, itemNum);
            //AddUserItem(UserID, itemID, itemNum);
        }

        private void AddUserItem(string userID, int itemID, int itemNum)
        {
            if (itemID > 0 && itemNum > 0)
            {
                UserTakePrize userPrizeLog = new UserTakePrize
                {
                    ID = Guid.NewGuid().ToString(),
                    UserID = userID.ToInt(),
                    ItemPackage = string.Format("{0}={1}={2}", itemID, 1, itemNum),
                    MailContent = string.Format("GM发送物品"),
                    IsTasked = false,
                    TaskDate = MathUtils.SqlMinDate,
                    OpUserID = 10000,
                    CreateDate = DateTime.Now

                };
                var sender = DataSyncManager.GetDataSender();
                sender.Send(userPrizeLog);
            }
            else
            {
                throw new Exception("无此物品");
            }
        }
    }
}