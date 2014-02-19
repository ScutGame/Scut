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
    /// 1209_未穿戴部位装备列表
    /// </summary>
    public class Action1209 : BaseAction
    {
        private int _equParts;
        private int _generalID;
        private List<UserItemInfo> _userItemArray = new List<UserItemInfo>();

        public Action1209(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1209, httpGet)
        {

        }

        public override void BuildPacket()
        {
            var cacheSetItemEqu = new ConfigCacheSet<ItemEquAttrInfo>();
            PushIntoStack(_userItemArray.Count);
            foreach (UserItemInfo item in _userItemArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                var general = new GameDataCacheSet<UserGeneral>().FindKey(Uid, item.GeneralID);
                var itemEquList = cacheSetItemEqu.FindAll(e => e.ItemID.Equals(item.ItemID));
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.UserItemID.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(item.ItemLv);
                dsItem.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
                dsItem.PushIntoStack(general != null ? general.GeneralName : string.Empty);
                PushIntoStack(dsItem);
                dsItem.PushIntoStack(itemEquList.Count);
                foreach (var itemEquAttrInfo in itemEquList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(itemEquAttrInfo.AttributeID.ToInt());
                    int strengNum = 0;
                    strengNum = MathUtils.Addition(itemEquAttrInfo.BaseNum, (itemEquAttrInfo.IncreaseNum * item.ItemLv), int.MaxValue);
                    //    dsItem1.PushIntoStack(itemEqu.BaseNum);
                    dsItem1.PushIntoStack(strengNum);
                    dsItem.PushIntoStack(dsItem1);
                }
               


            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("EquParts", ref _equParts)
                && httpGet.GetInt("GeneralID", ref _generalID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(Uid, _generalID);
            if (general != null)
            {
                var package = UserItemPackage.Get(Uid);
                _userItemArray = package.ItemPackage.FindAll(
                    u =>
                    {
                        var itemInfo = new UserItemHelper(u);
                        return !u.IsRemove && itemInfo.EquPartsID == _equParts &&
                               itemInfo.DemandLv <= general.GeneralLv &&
                              //u.ItemLv <= general.GeneralLv &&
                              u.ItemStatus.ToEnum<ItemStatus>() != ItemStatus.YongBing &&    //点击装备或者更换装备的时候,在筛选时去掉已经装备
                               itemInfo.CheckCareer(general.CareerID);
                    });
                _userItemArray.QuickSort((a, b) => a.CompareTo(b));
            }
            //var package = UserItemPackage.Get(Uid);
            //_userItemArray = package.ItemPackage.FindAll(s=>s.ItemType==ItemType.ZhuangBei);
            return true;
        }
    }
}