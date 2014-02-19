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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1202_装备详情接口
    /// </summary>
    public class Action1202 : BaseAction
    {
        private string _userItemId = string.Empty;
        private int _coldTime;
        private short _isStrong;
        private ItemBaseInfo _itemInfo;
        private List<ItemEquAttrInfo> _itemEquArray = new List<ItemEquAttrInfo>();
        private UserItemInfo _userItem;
        private short _demandLv;
        private int _strongMoney;
        private string _toUserId = string.Empty;
        private int _stengBaseNum;
        private int _tenTimesStrongMoney = 0;
        public Action1202(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1202, httpGet)
        {

        }

        public override bool TakeAction()
        {
            int maxEquNum = ConfigEnvSet.GetInt("UserQueue.EquStrengMaxNum");
            UserItemPackage package;
            if (!string.IsNullOrEmpty(_toUserId))
            {
                package = UserItemPackage.Get(_toUserId);
            }
            else
            {
                package = UserItemPackage.Get(ContextUser.UserID);
            }

             _userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(_userItemId)) ?? new UserItemInfo();

            _itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(_userItem.ItemID);
            if (_itemInfo == null)
            {
                SaveDebuLog(string.Format("玩家{0}物品ID={1}[{2}]不存在！", Uid, _userItem.UserItemID, _userItem.ItemID));
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1107_UserItemNotEnough;
                return false;
            }

            if (_userItem.ItemLv > _itemInfo.DemandLv)
            {
                _demandLv = _userItem.ItemLv;
            }
            else
            {
                _demandLv = _itemInfo.DemandLv;
            }

            _strongMoney = new UserItemHelper(_userItem, 1).StrongMoney;
            _tenTimesStrongMoney = new UserItemHelper(_userItem, 10).StrongMoney;  // 强化 10 次用钱
            _itemEquArray = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(m => m.ItemID == _userItem.ItemID);

            if (_userItem.ItemLv >= ContextUser.UserLv || _strongMoney > ContextUser.GameCoin)
            {
                _isStrong = 1;
            }
            List<UserQueue> userQueueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.EquipmentStrong);
            userQueueArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return y.Timing.CompareTo(x.Timing);
            });
            if (userQueueArray.Count == ContextUser.QueueNum)
            {
                DateTime minDateTime = DateTime.MinValue;
                foreach (UserQueue queue in userQueueArray)
                {
                    if (queue.DoRefresh() > 0 && !queue.IsSuspend && minDateTime < queue.Timing && queue.StrengNum >= maxEquNum)
                    {
                        _coldTime = queue.DoRefresh();
                    }
                }
            }
            UserHelper.SparePartPropertyList(Uid, _userItemId); //灵件属性
            return true;
        }

        public override void BuildPacket()
        {
            PushIntoStack(_itemInfo == null ? string.Empty : _itemInfo.ItemName.ToNotNullString());
            PushIntoStack(_itemInfo == null ? string.Empty : _itemInfo.HeadID.ToNotNullString());
            PushIntoStack(_itemInfo == null ? LanguageManager.GetLang().shortInt : (short)_itemInfo.QualityType);
            PushIntoStack(_userItem == null ? LanguageManager.GetLang().shortInt : _userItem.ItemLv);
            PushIntoStack(_demandLv);
            PushIntoStack(_userItem == null ? 0 : _strongMoney);
            PushIntoStack(_coldTime);
            PushIntoStack(_isStrong);

            string[] careerArray = _itemInfo == null ? new string[0] : _itemInfo.CareerRange.Split(',');
            PushIntoStack(careerArray.Length);
            foreach (string career in careerArray)
            {
                DataStruct ds = new DataStruct();
                CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(career);
                ds.PushIntoStack(careerInfo == null ? 0 : careerInfo.CareerID);
                ds.PushIntoStack(careerInfo == null ? string.Empty : careerInfo.CareerName.ToNotNullString());
                PushIntoStack(ds);
            }
            //当前属性
            PushIntoStack(_itemEquArray.Count);
            foreach (ItemEquAttrInfo equ in _itemEquArray)
            {
                int strengNum = 0;
                if (_userItem != null) strengNum = MathUtils.Addition(equ.BaseNum, (equ.IncreaseNum * _userItem.ItemLv), int.MaxValue);
                DataStruct ds = new DataStruct();
                ds.PushIntoStack((int)equ.AttributeID);
                ds.PushIntoStack(strengNum);

                PushIntoStack(ds);
            }

            //下级别装备属性
            PushIntoStack(_itemEquArray.Count);
            foreach (ItemEquAttrInfo equ in _itemEquArray)
            {
                DataStruct dsDetail = new DataStruct();
                dsDetail.PushIntoStack((int)equ.AttributeID);
                if (_userItem != null)
                {
                    _stengBaseNum = MathUtils.Addition(equ.BaseNum, MathUtils.Addition(_userItem.ItemLv, (short)1, short.MaxValue) * equ.IncreaseNum, int.MaxValue);
                }
                dsDetail.PushIntoStack(_stengBaseNum);
                PushIntoStack(dsDetail);
            }

            PushIntoStack(_itemInfo == null ? string.Empty : _itemInfo.ItemDesc);
            //PushIntoStack(PropertyArray.Length);
            //foreach (SparePartProperty item in PropertyArray)
            //{
            //    DataStruct dsItem = new DataStruct();
            //    dsItem.PushIntoStack((int)item.AbilityType);
            //    dsItem.PushIntoStack(UserHelper.PropertyAbility(item.Num));

            //    this.PushIntoStack(dsItem);
            //}

            PushIntoStack(UserHelper.StrongEquPayPrice(Uid, _userItemId));
            PushIntoStack(_userItemId.ToNotNullString());
            PushIntoStack(_itemInfo == null ? string.Empty : _itemInfo.MaxHeadID);
            PushIntoStack(_userItem.ItemLv >= (GameConfigSet.CurrMaxLv * 3) ? 1 : 0);
            PushIntoStack(_userItem == null ? 0 : _tenTimesStrongMoney);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref _userItemId))
            {
                httpGet.GetString("ToUserID", ref _toUserId);
                return true;
            }
            return false;
        }
    }
}