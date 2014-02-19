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
using System.Collections.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model.Enum;
using System;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1444_佣兵升级卡片列表接口
    /// </summary>
    public class Action1444 : BaseAction
    {
        private int pageIndex;
        private int pageSize;
        private int pageCount;
        private int itemNum;
        private List<UserItemInfo> useritemList = new List<UserItemInfo>();

        public Action1444(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1444, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(pageCount);
            this.PushIntoStack(useritemList.Count);
            foreach (var item in useritemList)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                itemNum = UserItemHelper.CheckItemNum(ContextUser.UserID, item.ItemID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.UserItemID.ToNotNullString());
                dsItem.PushIntoStack(item.ItemID);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(itemNum);
                dsItem.PushIntoStack(itemInfo == null ? 0 : itemInfo.EffectNum);
                
                this.PushIntoStack(dsItem);
            }

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
            var package = UserItemPackage.Get(ContextUser.UserID);
            if (package != null)
            {
                var useritemArray = package.ItemPackage.FindAll(s => s.ItemType == ItemType.DaoJu && s.PropType == 14);
                useritemList = useritemArray.GetPaging(pageIndex, pageSize, out pageCount);
            }
            return true;
        }
    }
}