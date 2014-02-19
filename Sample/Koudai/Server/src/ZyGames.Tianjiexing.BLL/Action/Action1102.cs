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
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 背包物品详情接口
    /// </summary>
    public class Action1102 : BaseAction
    {
        private string _userItemID = string.Empty;
        private ItemBaseInfo _itemInfo;
        private UserItemInfo _useritem;
        private List<ItemEquAttrInfo> _itemEquArray = new List<ItemEquAttrInfo>();
        private List<ItemSynthesisInfo> _itemSynthsisArray = new List<ItemSynthesisInfo>();
        private string[] _careerList = new string[0];
        private int _salePrice;
        private short demandLv = 0;
        private SparePartProperty[] PropertyArray = new SparePartProperty[0];
        private string _toUserID = string.Empty;
        private short isShow = 0;
        private string packUser = string.Empty;

        public Action1102(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1102, httpGet)
        {

        }

        public override bool TakeAction()
        {
            if (!string.IsNullOrEmpty(_toUserID))
            {
                packUser = _toUserID;
            }
            else
            {
                packUser = ContextUser.UserID;
            }
            var package = UserItemPackage.Get(packUser);
            _useritem = package.ItemPackage.Find(m => string.Equals(m.UserItemID, _userItemID)) ?? new UserItemInfo();
            if (_useritem == null || _useritem.ItemID == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1107_UserItemNotEnough;
                return false;
            }

            _itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(_useritem.ItemID);
            if (_itemInfo == null)
            {
                ErrorInfo = LanguageManager.GetLang().St1107_UserItemNotEnough;
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }

            if (_useritem.ItemLv > _itemInfo.DemandLv)
            {
                demandLv = _useritem.ItemLv;
            }
            else
            {
                demandLv = _itemInfo.DemandLv;
            }
            if (_itemInfo.ItemType == ItemType.ZhuangBei)
            {
                _salePrice = UserHelper.StrongEquPayPrice(packUser, _userItemID);
            }
            else
            {
                if (isShow > 0)
                {
                    _salePrice = (_itemInfo.SalePrice / 4);

                }
                else
                {
                    _salePrice = ((_itemInfo.SalePrice / 4) * _useritem.Num);
                }
            }

            _itemEquArray = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(m => m.ItemID == _useritem.ItemID);
            List<ItemSynthesisInfo> synthsisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.SynthesisID == _useritem.ItemID);
            if (synthsisArray.Count > 0)
            {
                var synthsis = synthsisArray[0];
                if (_itemInfo.ItemType == ItemType.TuZhi || _itemInfo.ItemType == ItemType.TuZhi)
                {
                    _itemSynthsisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.ItemID == synthsis.ItemID && m.SynthesisID != _useritem.ItemID);
                }
            }

            if (_itemInfo.CareerRange != null && _itemInfo.CareerRange.Trim() != "0")
            {
                _careerList = _itemInfo.CareerRange.Split(',');
            }
            PropertyArray = UserHelper.SparePartPropertyList(packUser, _userItemID); //灵件属性
            return true;
        }

        public override void BuildPacket()
        {
            PushIntoStack(_itemInfo.HeadID.ToNotNullString());
            PushIntoStack(_itemInfo.ItemName.ToNotNullString());
            PushIntoStack((short)_useritem.ItemType);
            PushIntoStack((short)_itemInfo.QualityType);
            PushIntoStack((short)_itemInfo.QualityType);
            PushIntoStack(_useritem.ItemLv);
            PushIntoStack(demandLv);

            PushIntoStack(_itemEquArray.Count);
            foreach (ItemEquAttrInfo equ in _itemEquArray)
            {
                int strengNum = 0;
                if (equ != null) strengNum = MathUtils.Addition(equ.BaseNum, (equ.IncreaseNum * _useritem.ItemLv), int.MaxValue);
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(equ == null ? (short)0 : (short)equ.AttributeID);
                ds.PushIntoStack(equ == null ? 0 : strengNum);
                PushIntoStack(ds);
            }

            PushIntoStack(_careerList.Length);
            foreach (string career in _careerList)
            {
                DataStruct ds = new DataStruct();
                CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(career);
                ds.PushIntoStack(careerInfo == null ? (short)0 : (short)careerInfo.CareerID);
                ds.PushIntoStack(careerInfo == null ? string.Empty : careerInfo.CareerName.ToNotNullString());
                PushIntoStack(ds);
            }

            PushIntoStack(_salePrice);
            PushIntoStack(_itemInfo == null ? string.Empty : _itemInfo.ItemDesc.ToNotNullString());

            PushIntoStack(_itemSynthsisArray.Count);
            foreach (ItemSynthesisInfo synthesis in _itemSynthsisArray)
            {
                int maxNum = GetItemMaxNum(synthesis);
                ItemBaseInfo tempItem = new ConfigCacheSet<ItemBaseInfo>().FindKey(synthesis.SynthesisID);

                DataStruct ds = new DataStruct();
                ds.PushIntoStack(synthesis.SynthesisID);
                ds.PushIntoStack(tempItem != null ? tempItem.ItemName.ToNotNullString() : string.Empty);
                ds.PushIntoStack((short)synthesis.DemandNum);
                ds.PushIntoStack((short)maxNum);

                PushIntoStack(ds);
            }
            PushIntoStack(PropertyArray.Length);
            foreach (SparePartProperty item in PropertyArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((int)item.AbilityType);
                dsItem.PushIntoStack(UserHelper.PropertyAbility(item.Num));

                PushIntoStack(dsItem);
            }
        }

        private int GetItemMaxNum(ItemSynthesisInfo synthesis)
        {

            if (synthesis == null) return 0;
            var package = UserItemPackage.Get(packUser);
            var userItemArray = package.ItemPackage.FindAll(
                u => !u.IsRemove && u.ItemID.Equals(synthesis.SynthesisID) &&
                u.ItemStatus != ItemStatus.Sell &&
                new UserItemHelper(u).GeneralStatus(packUser) != GeneralStatus.LiDui);

            int maxNum = 0;
            int sumNum = 0;
            foreach (var userItem in userItemArray)
            {
                sumNum += userItem.Num;
            }
            if (sumNum >= synthesis.DemandNum)
            {
                maxNum = synthesis.DemandNum;
            }
            else
            {
                maxNum = sumNum;
            }
            return maxNum;
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref _userItemID))
            {
                httpGet.GetString("ToUserID", ref _toUserID);
                httpGet.GetWord("IsShow", ref isShow);
                return true;
            }
            return false;
        }
    }
}