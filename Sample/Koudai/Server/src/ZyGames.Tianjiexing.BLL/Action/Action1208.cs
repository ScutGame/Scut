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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1208_未穿戴装备列表接口
    /// </summary>
    public class Action1208 : BaseAction
    {
        private int baseNum = 0;
        private int stengBaseNum = 0;

        private int coldTime = 0;
        private List<UserItemInfo> userItemArray = new List<UserItemInfo>();


        public Action1208(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1208, httpGet)
        {

        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(Uid);
            userItemArray = package.ItemPackage.FindAll(u => u.ItemType == ItemType.ZhuangBei && u.GeneralID <= 0 && u.ItemStatus.ToEnum<ItemStatus>() != ItemStatus.YongBing); //点击装备或者更换装备的时候,在筛选时去掉已经装备
            userItemArray.QuickSort((x, y) =>
            {
                int result = 0;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                // 1、品质高低
                result = y.ItemQuality.CompareTo(x.ItemQuality);
                if (result == 0)
                {
                    result = x.ItemID.CompareTo(y.ItemID);
                }
                //return new ConfigCacheSet<ItemBaseInfo>().FindKey(x.ItemID).EquParts.CompareTo(new ConfigCacheSet<ItemBaseInfo>().FindKey(y.ItemID).EquParts);
                return result;
            });
            coldTime = ConfigEnvSet.GetInt("UserItem.EquColdTime");
            return true;
        }

        public override void BuildPacket()
        {
            PushIntoStack(userItemArray.Count);
            foreach (var item in userItemArray)
            {
                short isStrong = 0;
                int strongMoney = new UserItemHelper(item).StrongMoney;
                if (item.ItemLv >= ContextUser.UserLv || strongMoney > ContextUser.GameCoin)
                {
                    isStrong = 1;
                }
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                DataStruct dsItem = new DataStruct();

                dsItem.PushIntoStack(item == null ? string.Empty : item.UserItemID.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? 0 : itemInfo.ItemID);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(item == null ? LanguageManager.GetLang().shortInt : item.ItemLv);
                dsItem.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
                dsItem.PushIntoStack(item == null ? 0 : strongMoney);
                dsItem.PushIntoStack(coldTime);
                dsItem.PushIntoStack(isStrong);
                dsItem.PushIntoStack(UserHelper.StrongEquPayPrice(Uid, item.UserItemID));

                List<ItemEquAttrInfo> itemEquArray = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(e => e.ItemID.Equals(item.ItemID));
                //当前等级装备属性
                dsItem.PushIntoStack(itemEquArray.Count);
                foreach (ItemEquAttrInfo equ in itemEquArray)
                {
                    DataStruct dsDetail = new DataStruct();
                    dsDetail.PushIntoStack((int)equ.AttributeID);
                    if (equ != null && item != null)
                    {
                        baseNum = MathUtils.Addition(equ.BaseNum, item.ItemLv * equ.IncreaseNum, int.MaxValue);
                    }
                    dsDetail.PushIntoStack(baseNum);
                    dsItem.PushIntoStack(dsDetail);
                }

                //下级别装备属性
                dsItem.PushIntoStack(itemEquArray.Count);
                foreach (ItemEquAttrInfo equ in itemEquArray)
                {
                    DataStruct dsDetail = new DataStruct();
                    dsDetail.PushIntoStack((int)equ.AttributeID);
                    if (equ != null && item != null)
                    {
                        stengBaseNum = MathUtils.Addition(equ.BaseNum, (int)(MathUtils.Addition(item.ItemLv, (short)1, short.MaxValue)) * equ.IncreaseNum, int.MaxValue);
                    }
                    dsDetail.PushIntoStack(stengBaseNum);
                    dsItem.PushIntoStack(dsDetail);
                }



                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            return true;
        }
    }
}