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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1109_背包物品分类列表
    /// </summary>
    public class Action1109 : BaseAction
    {
        private int pageIndex = 0;
        private int pageSize = 0;
        private int pageCount = 0;
        private int itemType = 0;//1装备，2图纸，3道具
        private List<UserItemInfo> mlist = new List<UserItemInfo>();

        public Action1109(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1109, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(mlist.Count);
            foreach (UserItemInfo item in mlist)
            {
                DataStruct ds = new DataStruct();
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                ds.PushIntoStack(item.UserItemID);
                ds.PushIntoStack(itemInfo == null ? 0 : itemInfo.ItemID);
                ds.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.ItemType);
                ds.PushIntoStack(item.Num);
                ds.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                ds.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.PropType);
                ds.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                ds.PushIntoStack(itemInfo == null ? 0 : (int)itemInfo.QualityType);
                ds.PushIntoStack(item.ItemLv);
                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                && httpGet.GetInt("PageSize", ref pageSize)
                && httpGet.GetInt("ItemType", ref itemType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(Uid);
            List<UserItemInfo> itemSortArray = new List<UserItemInfo>();
            if (itemType == 1)
            {
                itemSortArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemType == ItemType.ZhuangBei);
            }
            else if (itemType == 2)
            {
                itemSortArray = package.ItemPackage.FindAll(m => !m.IsRemove && (m.ItemType == ItemType.TuZhi || m.ItemType == ItemType.TuZhi));
            }
            else
            {
                itemSortArray = package.ItemPackage.FindAll(m => !m.IsRemove && (m.ItemType == ItemType.DaoJu
                    || m.ItemType == ItemType.XinShouLiBao || m.ItemType == ItemType.BengDai || m.ItemType == ItemType.CaiLiao ||
                    m.ItemType == ItemType.QiShi || m.ItemType == ItemType.XinShouLiBao || m.ItemType == ItemType.YaoJi || m.ItemType == ItemType.ZuoJi));
            }
            mlist = itemSortArray.GetPaging(pageIndex, pageSize, out pageCount);
            return true;
        }

    }
}