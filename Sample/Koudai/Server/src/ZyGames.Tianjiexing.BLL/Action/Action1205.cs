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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1205_佣兵装备列表接口
    /// </summary>
    public class Action1205 : BaseAction
    {
        private EquParts equParts;
        private List<UserItemInfo> userItemList = new List<UserItemInfo>();


        public Action1205(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1205, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(userItemList.Count);
            foreach (var item in userItemList)
            {
                List<ItemEquAttrInfo> itemEquList = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(e => e.ItemID.Equals(item.ItemID));
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, item.GeneralID);
                int strongMoney = new UserItemHelper(item).StrongMoney;
                int isStrong = 0;
                if (strongMoney > ContextUser.GameCoin)
                {
                    isStrong = 1;
                }
                else if (item.ItemLv > ContextUser.UserLv)
                {
                    isStrong = 2;
                }

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.UserItemID.ToNotNullString());
                dsItem.PushIntoStack(item.ItemID);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(item.ItemLv);
                dsItem.PushIntoStack(itemInfo == null ? (short)0 : itemInfo.QualityType.ToShort());
                dsItem.PushIntoStack(strongMoney);
                dsItem.PushIntoStack((short)isStrong);
                dsItem.PushIntoStack(UserHelper.StrongEquPayPrice(Uid, item.UserItemID));
                dsItem.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemDesc.ToNotNullString());
                dsItem.PushIntoStack(itemEquList.Count);
 
                foreach (var itemEqu in itemEquList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(itemEqu.AttributeID.ToInt());
                    int strengNum = 0;
                    if (item != null) strengNum = MathUtils.Addition(itemEqu.BaseNum, (itemEqu.IncreaseNum * item.ItemLv), int.MaxValue);
                //    dsItem1.PushIntoStack(itemEqu.BaseNum);
                    dsItem1.PushIntoStack(strengNum);
                    dsItem.PushIntoStack(dsItem1);
                }
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                httpGet.GetEnum("EquParts", ref equParts);
                return true;
            }
        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(ContextUser.UserID);
            if (package == null)
            {
                return false;
            }
            if (equParts == EquParts.Whole)
            {
                userItemList = package.ItemPackage.FindAll(s => !s.IsRemove && s.ItemType == ItemType.ZhuangBei);
            }
            else
            {
                //userItemList = package.ItemPackage.FindAll(s => !s.IsRemove && s.ItemType == ItemType.ZhuangBei && s.Equparts == equParts);
                if (equParts == EquParts.WuQi || equParts == EquParts.HunQi)
                {
                    userItemList = package.ItemPackage.FindAll(s => !s.IsRemove && s.ItemType == ItemType.ZhuangBei && s.Equparts == equParts);
                }
                else
                {
                    userItemList = package.ItemPackage.FindAll(s => !s.IsRemove && s.ItemType == ItemType.ZhuangBei && s.Equparts != EquParts.WuQi && s.Equparts != EquParts.HunQi);
                }
            }

            // 增加排序
            userItemList.QuickSort((x, y) =>
            {
                int result = 0;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;

                // 1、品质高低 2、是否上阵
                result = y.ItemQuality.CompareTo(x.ItemQuality);
                if (result == 0)
                {
                    result = y.ItemStatus.CompareTo(x.ItemStatus);
                    if (result == 0)
                    {
                        result = x.ItemStatus.CompareTo(y.ItemStatus);
                        if (result == 0)
                        {
                            result = x.ItemID.CompareTo(y.ItemID);

                        }
                    }
                }
                
                return result;
            });

            return true;
        }
    }
}