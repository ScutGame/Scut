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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1204_装备强化接口
    /// </summary>
    public class Action1204 : BaseAction
    {
        private string userItemID = string.Empty;
        private int queueTime = 0;
        private UserItemInfo userItem = null;
        private ItemBaseInfo itemInfo = null;
        private short isStrong = 0;
        private List<ItemEquAttrInfo> itemEquArray = new List<ItemEquAttrInfo>();
        private int ops = 0;
        private int strongMoney;
        private short strongLv = 0;
        private const int StrongOnce = 1;  // 强化 1 次
        private const int StrongTenTimes = 10;  // 强化 10 次
        public Action1204(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1204, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(userItem == null ? 0 : userItem.ItemID);
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
            PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
            PushIntoStack(userItem == null ? LanguageManager.GetLang().shortInt : userItem.ItemLv);
            PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
            PushIntoStack(userItem == null ? 0 : strongMoney);
            PushIntoStack(queueTime);
            PushIntoStack(isStrong);
            PushIntoStack(itemEquArray.Count);
            foreach (ItemEquAttrInfo equ in itemEquArray)
            {
                DataStruct dsItem = new DataStruct();
                int baseNum = 0;
                if (userItem != null)
                {
                    baseNum = MathUtils.Addition(equ.BaseNum, userItem.ItemLv * equ.IncreaseNum, int.MaxValue);
                }
                dsItem.PushIntoStack((int)equ.AttributeID);
                dsItem.PushIntoStack(baseNum);

                PushIntoStack(dsItem);
            }
            PushIntoStack(itemEquArray.Count);
            foreach (ItemEquAttrInfo equ in itemEquArray)
            {
                DataStruct dsItem = new DataStruct();
                int baseNum = 0;
                if (userItem != null)
                {
                    baseNum = MathUtils.Addition(equ.BaseNum, (int)(MathUtils.Addition(userItem.ItemLv, (short)1, short.MaxValue)) * equ.IncreaseNum, int.MaxValue);
                }
                dsItem.PushIntoStack((int)equ.AttributeID);
                dsItem.PushIntoStack(baseNum);
                PushIntoStack(dsItem);
            }
            PushIntoStack(userItemID.ToNotNullString());
            PushIntoStack(strongLv);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref userItemID) && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            //int maxEquNum = ConfigEnvSet.GetInt("UserQueue.EquStrengMaxNum");
            //int coldTime = ConfigEnvSet.GetInt("UserItem.EquColdTime");

            //铜钱不足
            var package = UserItemPackage.Get(Uid);
            userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID)) ?? new UserItemInfo();
            if (ops == StrongOnce)
            {
                // 强化 1 次用钱
                strongMoney = new UserItemHelper(userItem, 1).StrongMoney;
            }
            else if (ops == StrongTenTimes)
            {
                // 强化 10 次用钱
                strongMoney = new UserItemHelper(userItem, 10).StrongMoney;
            }
            short strongMaxLv = (ContextUser.UserLv * 3).ToShort(); //MathUtils.Addition(ContextUser.UserLv, 1.ToShort()); //强化最高等级
            if (ContextUser.GameCoin < strongMoney)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                return false;
            }

            //UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, userItem.GeneralID);
            //if (userGeneral != null && userItem.ItemLv >= strongMaxLv)
            //{
            //    ErrorCode = LanguageManager.GetLang().ErrorCode;
            //    ErrorInfo = LanguageManager.GetLang().St1204_EquGeneralMaxLv;
            //    return false;
            //}

            if (userItem.ItemLv >= strongMaxLv)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1204_EquMaxLv;
                return false;
            }

            itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
            if (itemInfo != null)
            {
                itemEquArray = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(m => m.ItemID == itemInfo.ItemID);
            }

            if (strongMoney > ContextUser.GameCoin)
            {
                isStrong = 1;
            }
            else if (userItem.ItemLv >= strongMaxLv)
            {
                isStrong = 2;
            }


            UpdateUserItem(ContextUser, userItem, strongMoney);
            //日常任务-强化
            TaskHelper.TriggerDailyTask(Uid, 4001);

            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, userItem.GeneralID);
            if (general != null)
            {
                general.RefreshMaxLife();
            }
            return true;
        }

        /// <summary>
        /// 强化
        /// </summary>
        private void UpdateUserItem(GameUser user, UserItemInfo userItem, int strongMoney)
        {
            if (user.GameCoin >= strongMoney)
            {
                user.GameCoin = MathUtils.Subtraction(user.GameCoin, strongMoney, 0);
                var package = UserItemPackage.Get(user.UserID);
                strongLv = 0;
                VipLvInfo vipLvInfo = new ConfigCacheSet<VipLvInfo>().FindKey(user.VipLv);
                for (int i = 0; i < ops; i++)
                {
                    EquStreng streng = null;
                    if (vipLvInfo != null && vipLvInfo.EquStreng != null)
                    {
                        streng = vipLvInfo.EquStreng;
                    }
                    if (streng != null && RandomUtils.IsHit(streng.probability))
                    {
                        strongLv += (short)RandomUtils.GetRandom(streng.start, streng.end);
                    }
                    else
                    {
                        strongLv += 1;
                    }
                }
                userItem.ItemLv = MathUtils.Addition(userItem.ItemLv, strongLv);
                package.SaveItem(userItem);
                UserLogHelper.AppenStrongLog(user.UserID, 1, userItem.UserItemID, userItem.ItemID, 2, userItem.ItemLv, 0, userItem.GeneralID);
            }
        }
    }
}