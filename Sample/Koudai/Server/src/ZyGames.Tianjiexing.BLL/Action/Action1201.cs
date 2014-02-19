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


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1201_佣兵装备列表接口
    /// </summary>
    public class Action1201 : BaseAction
    {
        private int _coldTime = 0;
        private short isSynthesis = 0;
        private string queueID = string.Empty;
        private List<UserGeneral> _userGeneralArray = new List<UserGeneral>();

        public Action1201(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1201, httpGet)
        {

        }

        public override bool TakeAction()
        {
            string queID = string.Empty;
            int maxEquNum = ConfigEnvSet.GetInt("UserQueue.EquStrengMaxNum");
            _userGeneralArray = new GameDataCacheSet<UserGeneral>().FindAll( ContextUser.UserID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong );
            List<UserQueue> userQueueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID,m=>m.QueueType== QueueType.EquipmentStrong);
            if (userQueueArray.Count > 0 && userQueueArray.Count == ContextUser.QueueNum)
            {
                int minEquNum = userQueueArray[0].StrengNum;
                int totalcoldTime = userQueueArray[0].DoRefresh();
                queID = userQueueArray[0].QueueID;
                foreach (UserQueue queue in userQueueArray)
                {
                    if (totalcoldTime > queue.DoRefresh())
                    {
                        queID = queue.QueueID;
                        totalcoldTime = queue.DoRefresh();
                    }

                    if (totalcoldTime > 0 && !queue.IsSuspend && minEquNum >= queue.StrengNum && userQueueArray.Count >= ContextUser.QueueNum)
                    {
                        minEquNum = queue.StrengNum;
                        if (minEquNum == maxEquNum)
                        {

                            _coldTime = totalcoldTime;
                            if (_coldTime == 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            _coldTime = 0;
                        }
                    }
                    else
                    {
                        _coldTime = 0;
                    }
                }
            }
            queueID = queID;
            return true;
        }

        public override void BuildPacket()
        {
            PushIntoStack(_coldTime);
            PushIntoStack(_userGeneralArray.Count);
            foreach (UserGeneral general in _userGeneralArray)
            {
                UserGeneral general1 = general;
                List<UserItemInfo> userItemArray = UserItemHelper.GetItems(Uid).FindAll(u => u.GeneralID.Equals(general1.GeneralID) && u.ItemStatus == ItemStatus.YongBing);
                userItemArray.QuickSort((x, y) =>
                {
                    if (x == null && y == null) return 0;
                    if (x != null && y == null) return 1;
                    if (x == null) return -1;
                    return new ConfigCacheSet<ItemBaseInfo>().FindKey(x.ItemID).EquParts.CompareTo(new ConfigCacheSet<ItemBaseInfo>().FindKey(y.ItemID).EquParts);
                });

                DataStruct ds = new DataStruct();
                ds.PushIntoStack(general == null ? 0 : general.GeneralID);
                ds.PushIntoStack(general == null ? string.Empty : general.GeneralName.ToNotNullString());
                // List<UserItem> itemList = userItemDict.ContainsKey(general.UserID) ? userItemDict[general.UserID] : new List<UserItem>();

                ds.PushIntoStack(userItemArray.Count);
                foreach (UserItemInfo userItem in userItemArray)
                {
                    int equCodeTime = ConfigEnvSet.GetInt("UserItem.EquColdTime");
                    short isStrong = 0;
                    int strongMoney = new UserItemHelper(userItem).StrongMoney;
                    if (strongMoney > ContextUser.GameCoin)
                    {
                        isStrong = 1;
                    }
                    else if (userItem.ItemLv >= general.GeneralLv)
                    {
                        isStrong = 2;
                    }
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                    List<ItemSynthesisInfo> itemSynthesisInfosArray =
                        new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m=>m.SynthesisID== userItem.ItemID);
                    if (itemSynthesisInfosArray.Count > 0 && itemInfo.DemandLv <= ContextUser.UserLv)
                    {
                        isSynthesis = 1;
                    }
                    else
                    {
                        isSynthesis = 2;
                    }

                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(userItem.UserItemID);
                    dsItem.PushIntoStack(itemInfo == null ? 0 : itemInfo.ItemID);
                    dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                    dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                    dsItem.PushIntoStack(userItem.ItemLv);
                    dsItem.PushIntoStack(itemInfo == null ? LanguageManager.GetLang().shortInt : (short)itemInfo.QualityType);
                    dsItem.PushIntoStack(strongMoney);
                    dsItem.PushIntoStack(equCodeTime);
                    dsItem.PushIntoStack(isStrong);
                    dsItem.PushIntoStack(isSynthesis);

                    if (itemInfo != null)
                    {
                        List<ItemEquAttrInfo> itemEquArray = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(e => e.ItemID.Equals(itemInfo.ItemID));
                        dsItem.PushIntoStack(itemEquArray.Count);
                        foreach (ItemEquAttrInfo equ in itemEquArray)
                        {
                            DataStruct dsDetail = new DataStruct();
                            dsDetail.PushIntoStack((int)equ.AttributeID);
                            int baseNum;
                            {
                                baseNum = MathUtils.Addition(equ.BaseNum, userItem.ItemLv * equ.IncreaseNum, int.MaxValue);
                            }
                            dsDetail.PushIntoStack(baseNum);
                            dsItem.PushIntoStack(dsDetail);
                        }

                        dsItem.PushIntoStack(itemEquArray.Count);
                        foreach (ItemEquAttrInfo equ in itemEquArray)
                        {
                            DataStruct dsDetail = new DataStruct();
                            dsDetail.PushIntoStack((int)equ.AttributeID);
                            int baseNum;
                            {
                                baseNum = MathUtils.Addition(equ.BaseNum, (int)(MathUtils.Addition(userItem.ItemLv, (short)1, short.MaxValue)) * equ.IncreaseNum, int.MaxValue);
                            }
                            dsDetail.PushIntoStack(baseNum);
                            dsItem.PushIntoStack(dsDetail);
                        }
                    }

                    ds.PushIntoStack(dsItem);
                }

                PushIntoStack(ds);
            }
            PushIntoStack(queueID);
        }

        public override bool GetUrlElement()
        {
            return true;
        }
    }
}