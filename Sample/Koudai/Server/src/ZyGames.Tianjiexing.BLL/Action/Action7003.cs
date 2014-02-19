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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

using ZyGames.Tianjiexing.Lang;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 7003_背包售出物品列表接口
    /// </summary>
    public class Action7003 : BaseAction
    {
        private int pageIndex = 0;
        private int pageSize = 0;
        private int pageCount = 0;
        private List<UserItemInfo> userItemArray = new List<UserItemInfo>();

        public Action7003(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7003, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(pageCount);
            this.PushIntoStack(userItemArray.Count);
            foreach (UserItemInfo item in userItemArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                DataStruct dsItem = new DataStruct();
                int mallPrice = 0;
                if (itemInfo != null && itemInfo.ItemType == ItemType.ZhuangBei)
                {
                    mallPrice = UserHelper.StrongEquPayPrice(Uid, item.UserItemID);
                }
                else if (itemInfo != null)
                {
                    mallPrice = ((itemInfo.SalePrice) * item.Num / 4);
                }

                dsItem.PushIntoStack(item.UserItemID.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(mallPrice);

                PushIntoStack(dsItem);
            }
            PushIntoStack(ContextUser.GameCoin);
            PushIntoStack(ContextUser.GoldNum);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                 && httpGet.GetInt("PageSize", ref pageSize))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            int intervalDate = ConfigEnvSet.GetInt("UserItemSold.IntervalDate");
            int totalIntervalDate = (intervalDate * 60 * 60);

            var package = UserItemPackage.Get(Uid);
            var sellItem = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.Sell);
            foreach (var item in sellItem)
            {
                if ((DateTime.Now - item.SoldDate).TotalSeconds > totalIntervalDate)
                {
                    UserItemHelper.AddItemLog(ContextUser.UserID, item.ItemID, item.Num, item.ItemLv, 4, item.UserItemID);

                    package.RemoveItem(item);
                }
            }

            var tempList = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.Sell);
            userItemArray = tempList.GetPaging(pageIndex, pageSize, out pageCount);
            return true;
        }
    }
}