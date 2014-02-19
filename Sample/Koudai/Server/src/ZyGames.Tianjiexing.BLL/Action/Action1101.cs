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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

using ZyGames.Tianjiexing.Lang;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 背包列表
    /// </summary>
    public class Action1101 : BaseAction
    {
        private int pageIndex = 0;
        private int pageSize = 0;
        private int pageCount = 0;
        private int _packType = 0;
        private int _gridNum;
        private int _itemCount = 0;
        private List<UserItemInfo> mlist = new List<UserItemInfo>();
        public Action1101(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1101, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(_itemCount);
            PushIntoStack(_gridNum);
            PushIntoStack(ContextUser.GameCoin);
            PushIntoStack(ContextUser.GoldNum);
            PushIntoStack(mlist.Count);
            foreach (UserItemInfo item in mlist)
            {
                DataStruct ds = new DataStruct();
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                ds.PushIntoStack(item.UserItemID.ToNotNullString());
                ds.PushIntoStack(itemInfo == null ? 0 : itemInfo.ItemID);
                ds.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.ItemType);
                ds.PushIntoStack(item.Num);
                ds.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                ds.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : itemInfo.PropType);
                ds.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                ds.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemDesc.ToNotNullString());
                ds.PushIntoStack(itemInfo == null ? 0 : itemInfo.SalePrice);
                ds.PushIntoStack(itemInfo == null ? 0 : itemInfo.IsUse ? 1 : 0);
                ds.PushIntoStack(itemInfo == null ? 0 : itemInfo.IsCostly ? 1 : 0);
                ds.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.MaxHeadID);
                ds.PushIntoStack(itemInfo == null ? 0 : itemInfo.PropType != 15 ? 0 : ItemUseHelper.ItemNum(ContextUser.UserID, itemInfo.EffectNum) <= 0 ? 0 : 1);
                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                && httpGet.GetInt("PageSize", ref pageSize)
                && httpGet.GetInt("PackType", ref _packType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cacheSetPack = new GameDataCacheSet<UserPack>();
            var userPack = cacheSetPack.FindKey(UserId.ToString());
            var pack = userPack != null && userPack.PackTypeList != null ? userPack.PackTypeList.Find(s => s.BackpackType == BackpackType.BeiBao) : null;
            _gridNum = pack != null ? pack.Position : 0;
            //int sellDate = (ConfigEnvSet.GetInt("UserItemSold.IntervalDate") * 60 * 60);

            var package = UserItemPackage.Get(Uid);
            var sellItem = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.Sell && m.ItemType.ToInt() == _packType);
            _itemCount = package != null && package.ItemPackage != null
                            ? package.ItemPackage.FindAll(s => s.ItemType != ItemType.ZhuangBei).Count
                            : 0;
            foreach (var item in sellItem)
            {
                UserItemHelper.AddItemLog(ContextUser.UserID, item.ItemID, item.Num, item.ItemLv, 4, item.UserItemID);
                package.RemoveItem(item);
            }

            var itemSortArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.BeiBao && m.ItemType.ToInt() == _packType);
            itemSortArray.QuickSort((x, y) =>
            {
                int result = 0;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;

                result = (y.CanUse).CompareTo(x.CanUse);
                if (result == 0)
                {
                    result = ((int)x.ItemType).CompareTo((int)y.ItemType);
                    if (result == 0)
                    {
                        result = x.ItemID.CompareTo(y.ItemID);
                        if (result == 0)
                        {
                            if (x.ItemType == ItemType.ZhuangBei)
                            {
                                UserItemHelper h1 = new UserItemHelper(y);
                                UserItemHelper h2 = new UserItemHelper(x);
                                result = ((int)h2.QualityType).CompareTo((int)h1.QualityType);
                                if (result == 0)
                                {
                                    result = ((int)y.ItemLv).CompareTo(x.ItemLv);
                                }
                            }
                            else
                            {
                                result = ((int)y.ItemLv).CompareTo(x.ItemLv);
                            }
                        }
                    }

                }
                return result;
            });

            mlist = itemSortArray.GetPaging(pageIndex, pageSize, out pageCount);
            return true;
        }
    }
}