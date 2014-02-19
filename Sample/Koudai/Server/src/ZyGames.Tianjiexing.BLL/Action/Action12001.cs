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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.ConfigModel;
using ZyGames.Tianjiexing.Model.DataModel;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 12001_幸运转盘界面接口
    /// </summary>
    public class Action12001 : BaseAction
    {
        private string userItemID;
        private short isFree = 0;
        private int freeNum;
        private string itemHead;
        private string itemContent;
        private int postion = 0;
        private List<DialInfo> dialList = new List<DialInfo>();

        public Action12001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action12001, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack((short)isFree);
            this.PushIntoStack(freeNum);
            this.PushIntoStack(itemHead.ToNotNullString());
            this.PushIntoStack(itemContent.ToNotNullString());
            this.PushIntoStack(dialList.Count);
            foreach (var dial in dialList)
            {
                postion = MathUtils.Addition(postion, 1);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(postion);
                dsItem.PushIntoStack(dial.HeadID.ToNotNullString());
                dsItem.PushIntoStack(dial.ItemDesc.ToNotNullString());
                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref userItemID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            DialHelper.CheckDialNum(ContextUser.UserID);
            //freeNum = GameConfigSet.FreeSweepstakes;
            UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(ContextUser.UserID);
            if (userDial == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            //string content = LanguageManager.GetLang().St_SummerThreeGameCoinNotice.Substring(0, 5);
            //itemHead = userDial.HeadID;
            //if (userDial.PrizeInfo != null && !string.IsNullOrEmpty(userDial.PrizeInfo.UserID))
            //{
            //    if (userDial.PrizeInfo.Type == RewardType.Again || userDial.PrizeInfo.Type == RewardType.Recharge || userDial.PrizeInfo.Type == RewardType.Mood)
            //    {
            //        itemContent = DialHelper.PrizeItemName(userDial.PrizeInfo);
            //    }
            //    else
            //    {
            //        itemContent = content + DialHelper.PrizeItemName(userDial.PrizeInfo);
            //    }
            //}
            //if (userDial.RefreshDate.Date == DateTime.Now.Date)
            //{
            //    freeNum = MathUtils.Subtraction(freeNum, userDial.DialNum);
            //    if (userDial.DialNum >= freeNum)
            //    {
            //        isFree = 1;
            //    }
            //}
            //}
         
            int itemID = UserItemHelper.GetUserItemInfoID(ContextUser.UserID, userItemID);
            var itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (itemInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            int groupID = DialHelper.ChestDialGroupID(ContextUser.UserID, itemID);
            freeNum = UserItemHelper.UserItemNum(ContextUser.UserID, itemID);
            dialList = new ConfigCacheSet<DialInfo>().FindAll(m => m.GroupID == groupID);
            dialList.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.ID.CompareTo(y.ID);
            });
            userDial.UserItemID = userItemID;
            return true;
        }
    }
}