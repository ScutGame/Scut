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
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1413_佣兵可服用丹药列表接口
    /// </summary>
    public class Action1413 : BaseAction
    {
        private int generalID = 0;
        private int medicineType = 0;
        private List<UserItemInfo> itemArray = new List<UserItemInfo>();

        public Action1413(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1413, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(itemArray.Count);
            foreach (UserItemInfo item in itemArray)
            {
                ItemBaseInfo ItemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(ItemInfo == null ? 0 : ItemInfo.ItemID);
                dsItem.PushIntoStack(ItemInfo == null ? string.Empty : ItemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(ItemInfo == null ? LanguageManager.GetLang().shortInt : ItemInfo.MedicineType);
                dsItem.PushIntoStack(ItemInfo == null ? LanguageManager.GetLang().shortInt : ItemInfo.MedicineLv);
                dsItem.PushIntoStack(ItemInfo == null ? string.Empty : ItemInfo.HeadID.ToNotNullString());

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("MedicineType", ref medicineType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(Uid);

            itemArray = package.ItemPackage.FindAll(
                u =>
                {
                    var helper = new UserItemHelper(u);
                    return !u.IsRemove && u.ItemType == ItemType.YaoJi &&
                           helper.DemandLv <= ContextUser.UserLv &&
                           helper.MedicineType == medicineType &&
                           (u.ItemStatus == ItemStatus.BeiBao || u.ItemStatus == ItemStatus.CangKu);
                });
            itemArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.ItemID.CompareTo(y.ItemID);
            });
            return true;
        }
    }
}