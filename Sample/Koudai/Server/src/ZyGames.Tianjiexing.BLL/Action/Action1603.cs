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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1603_装备、丹药合成接口
    /// </summary>
    public class Action1603 : BaseAction
    {
        private string _userItemID = string.Empty;
        private string _userEquID = string.Empty;
        private int _ops = 0;

        public Action1603(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1603, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref _ops))
            {
                httpGet.GetString("UserItemID", ref _userItemID);
                httpGet.GetString("UserEquID", ref _userEquID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            short opType = 0;
            UserItemInfo synthesisUserItem = null;
            var package = UserItemPackage.Get(Uid);
            UserItemInfo userItem = null;
            if (!string.IsNullOrEmpty(_userItemID))
            {
                var userItemList = package.ItemPackage.FindAll(m => !m.IsRemove && m.UserItemID.Equals(_userItemID));
                foreach (var item in userItemList)
                {
                    if (item.ItemStatus == ItemStatus.Sell)
                    {
                        package.RemoveItem(userItem);
                        continue;
                    }
                    else if (item.SoldDate > MathUtils.SqlMinDate)
                    {
                        item.SoldDate = MathUtils.SqlMinDate;
                        //package.Update();
                        userItem = item;
                    }
                    else
                    {
                        userItem = item;
                    }
                }
                if (userItem == null) return false;
                ItemBaseInfo itemBaseOpType = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                if (itemBaseOpType == null) return false;
                if (itemBaseOpType.ItemType == ItemType.TuZhi)
                {
                    opType = 1;
                }
                else
                {
                    opType = 2;
                }
            }
            else if (!string.IsNullOrEmpty(_userEquID))
            {
                opType = 1;
                userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(_userEquID));
            }
            if (userItem == null)
            {
                return false;
            }
            int genlv = ContextUser.UserLv;
            CacheList<SynthesisInfo> synthesisInfoList = new CacheList<SynthesisInfo>();
            short itemLv = 0;
            List<ItemSynthesisInfo> itemSynthesisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.SynthesisID == userItem.ItemID); //图纸ID
            if (itemSynthesisArray.Count > 0)
            {
                ItemSynthesisInfo synthesisInfo = itemSynthesisArray[0];
                if (synthesisInfo == null) return false;
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(synthesisInfo.ItemID);
                if (itemInfo != null && itemInfo.ItemType == ItemType.ZhuangBei && itemInfo.DemandLv > genlv)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_LevelNotEnough;
                    return false;
                }
                List<ItemSynthesisInfo> synthesisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.ItemID == synthesisInfo.ItemID);//合成物品的材料数组
                if (synthesisArray.Count == 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().LoadDataError;
                    return false;
                }
                int useGold = GetSytnthesisGold(ContextUser.UserID, synthesisInfo.ItemID);
                if (_ops == 1)
                {
                    #region 普通合成
                    if (!GetMaterialsNum(ContextUser.UserID, synthesisInfo.ItemID) && ContextUser.VipLv < 4)
                    {
                        ErrorCode = 3;
                        ErrorInfo = LanguageManager.GetLang().St1603_MaterialsNotEnough;
                        return false;
                    }
                    if (!GetMaterialsNum(ContextUser.UserID, synthesisInfo.ItemID) && ContextUser.VipLv >= 4)
                    {
                        ErrorCode = 2;
                        ErrorInfo = LanguageManager.GetLang().St1603_MaterialsNotEnough + "，" + string.Format(LanguageManager.GetLang().St1603_SynthesisEnergyNum, useGold);
                        return false;
                    }
                    foreach (ItemSynthesisInfo synthesis in synthesisArray)
                    {
                        ItemBaseInfo itemsInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(synthesis.SynthesisID);
                        if (itemInfo == null) return false;
                        if (!string.IsNullOrEmpty(_userEquID) && synthesis.SynthesisID == userItem.ItemID && itemsInfo != null && itemInfo.ItemType == ItemType.ZhuangBei)
                        {
                            if (userItem.ItemStatus == ItemStatus.YongBing)
                            {
                                //if (UserHelper.IsItemEquSpare(ContextUser, userItem.UserItemID))
                                //{
                                //    ErrorCode = LanguageManager.GetLang().ErrorCode;
                                //    ErrorInfo = LanguageManager.GetLang().St_ItemEquIndexOfSpare;
                                //    return false;
                                //}
                                if (IsLevelNotEnough(ContextUser, userItem.GeneralID, synthesisInfo.ItemID))
                                {
                                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                                    ErrorInfo = LanguageManager.GetLang().St_LevelNotEnough;
                                    return false;
                                }
                                UserItemHelper.AddItemLog(ContextUser.UserID, userItem.ItemID, userItem.Num, userItem.ItemLv, 9, userItem.UserItemID);
                                synthesisInfoList.Add(new SynthesisInfo() { DemandID = userItem.ItemID, Num = 1 });
                                //package.SyncCache(() =>
                                //{
                                //    userItem.IsRemove = true;
                                //    package.ItemPackage.Remove(userItem);
                                //    package.DelayChange();
                                //});
                            }
                        }
                        else
                        {
                            if (itemsInfo != null && itemsInfo.ItemType == ItemType.ZhuangBei)
                            {
                                synthesisUserItem = GetUserItemInfo(ContextUser.UserID, itemsInfo.ItemID);
                                if (IsLevelNotEnough(ContextUser, synthesisUserItem.GeneralID, synthesisInfo.ItemID))
                                {
                                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                                    ErrorInfo = LanguageManager.GetLang().St_LevelNotEnough;
                                    return false;
                                }
                                //synthesisUserItem = GetUserItemInfo(ContextUser.UserID, itemsInfo.ItemID);
                            }
                            else
                            {
                                synthesisInfoList.Add(new SynthesisInfo() { DemandID = synthesis.SynthesisID, Num = synthesis.DemandNum });
                                UserItemHelper.UseUserItem(ContextUser.UserID, synthesis.SynthesisID, synthesis.DemandNum);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(_userEquID))
                    {
                        itemLv = MathUtils.Subtraction(userItem.ItemLv, (short)5, (short)1);
                        UpdateUserItem(synthesisInfo.ItemID, userItem);
                        UserLogHelper.AppenItemSynthesisLog(ContextUser.UserID, opType, synthesisInfo.ItemID, 1, synthesisInfoList, null, 0, itemLv, userItem.ItemLv);
                    }
                    else
                    {
                        if (synthesisUserItem != null)
                        {
                            itemLv = MathUtils.Subtraction(synthesisUserItem.ItemLv, (short)5, (short)1);
                            UpdateUserItem(synthesisInfo.ItemID, synthesisUserItem);
                            UserLogHelper.AppenItemSynthesisLog(ContextUser.UserID, opType, synthesisInfo.ItemID, 1, synthesisInfoList, null, 0, itemLv, userItem.ItemLv);
                        }
                        else
                        {
                            UserItemHelper.AddUserItem(ContextUser.UserID, synthesisInfo.ItemID, synthesisArray[0].SynthesisNum);
                            UserLogHelper.AppenItemSynthesisLog(ContextUser.UserID, opType, synthesisInfo.ItemID, 1, synthesisInfoList, null, 0, itemLv, userItem.ItemLv);
                        }
                    }
                    #endregion
                }
                else if (_ops == 2)
                {
                    #region 晶石合成
                    if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.JuanZouZhiJieWanCheng))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                        return false;
                    }
                    ErrorCode = 1;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St1603_SynthesisEnergyNum, useGold);
                    return false;
                    #endregion
                }
                else if (_ops == 3)
                {
                    #region 确认晶石合成
                    if (ContextUser.GoldNum < useGold)
                    {
                        ErrorCode = 1;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                    //原因：配置材料不使用晶石合成时提示
                    string userItemName = UseritemGoldName(ContextUser.UserID, synthesisArray);
                    if (!string.IsNullOrEmpty(userItemName))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = string.Format(LanguageManager.GetLang().St_ItemIsGold, userItemName);
                        return false;
                    }

                    foreach (ItemSynthesisInfo synthesis in synthesisArray)
                    {
                        int curNum = 0;
                        ItemBaseInfo itemsInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(synthesis.SynthesisID);
                        if (!string.IsNullOrEmpty(_userEquID) && synthesis.SynthesisID == userItem.ItemID)
                        {
                            if (userItem.ItemStatus == ItemStatus.YongBing)
                            {
                                if (IsLevelNotEnough(ContextUser, userItem.GeneralID, synthesisInfo.ItemID))
                                {
                                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                                    ErrorInfo = LanguageManager.GetLang().St_LevelNotEnough;
                                    return false;
                                }

                                UserItemHelper.AddItemLog(ContextUser.UserID, userItem.ItemID, userItem.Num, userItem.ItemLv, 9, userItem.UserItemID);
                                synthesisInfoList.Add(new SynthesisInfo() { DemandID = userItem.ItemID, Num = 1 });
                            }
                        }
                        else
                        {
                            if (itemsInfo != null && itemsInfo.ItemType == ItemType.ZhuangBei)
                            {
                                synthesisUserItem = GetUserItemInfo(ContextUser.UserID, itemsInfo.ItemID);
                                if (IsLevelNotEnough(ContextUser, synthesisUserItem.GeneralID, synthesisInfo.ItemID))
                                {
                                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                                    ErrorInfo = LanguageManager.GetLang().St_LevelNotEnough;
                                    return false;
                                }
                            }
                            else
                            {
                                var itemArray = UserItemHelper.GetItems(Uid).FindAll(u => u.ItemID.Equals(synthesis.SynthesisID) && (u.ItemStatus == ItemStatus.BeiBao || u.ItemStatus == ItemStatus.CangKu));
                                if (itemArray.Count > 0)
                                {
                                    foreach (var item in itemArray)
                                    {
                                        curNum += item.Num;
                                    }
                                    if (synthesis.DemandNum < curNum)
                                    {
                                        UserItemHelper.UseUserItem(ContextUser.UserID, synthesis.SynthesisID, synthesis.DemandNum);
                                        synthesisInfoList.Add(new SynthesisInfo() { DemandID = synthesis.SynthesisID, Num = synthesis.DemandNum });
                                    }
                                    else
                                    {
                                        UserItemHelper.UseUserItem(ContextUser.UserID, synthesis.SynthesisID, curNum);
                                        synthesisInfoList.Add(new SynthesisInfo() { DemandID = synthesis.SynthesisID, Num = curNum });
                                    }
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(_userEquID))
                    {
                        itemLv = MathUtils.Subtraction(userItem.ItemLv, (short)5, (short)1);
                        UpdateUserItem(synthesisInfo.ItemID, userItem);
                        UserLogHelper.AppenItemSynthesisLog(ContextUser.UserID, opType, synthesisInfo.ItemID, 1, synthesisInfoList, null, 0, itemLv, userItem.ItemLv);
                    }
                    else
                    {
                        if (synthesisUserItem != null)
                        {
                            itemLv = MathUtils.Subtraction(synthesisUserItem.ItemLv, (short)5, (short)1);
                            UpdateUserItem(synthesisInfo.ItemID, synthesisUserItem);
                            UserLogHelper.AppenItemSynthesisLog(ContextUser.UserID, opType, synthesisInfo.ItemID, 1, synthesisInfoList, null, 0, itemLv, userItem.ItemLv);
                        }
                        else
                        {
                            UserItemHelper.AddUserItem(ContextUser.UserID, synthesisInfo.ItemID, synthesisArray[0].SynthesisNum);
                            UserLogHelper.AppenItemSynthesisLog(ContextUser.UserID, opType, synthesisInfo.ItemID, 1, synthesisInfoList, null, 0, itemLv, userItem.ItemLv);
                        }
                    }

                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold, int.MaxValue);
                    // ContextUser.Update();

                    #endregion
                }
            }
            return true;
        }

        private void AppenUserItem(ItemSynthesisInfo synthesisInfo, int ugeneralID, ItemStatus istatus, short itemLv)
        {
            //原因：合成黄色装备，物品存放为0
            if (istatus != ItemStatus.YongBing)
            {
                istatus = ItemStatus.BeiBao;
            }
            var package = UserItemPackage.Get(Uid);

            string uitemID = Guid.NewGuid().ToString();
            UserItemInfo userItem = new UserItemInfo()
            {
                UserItemID = uitemID,
                ItemID = synthesisInfo.ItemID,
                ItemLv = MathUtils.Subtraction(itemLv, (short)5, (short)1),
                ItemStatus = istatus,
                ItemType = ItemType.ZhuangBei,
                GeneralID = ugeneralID,
                Num = 1
            };
            package.SaveItem(userItem);
            //package.DelayChange();
            UserItemHelper.AddItemLog(ContextUser.UserID, synthesisInfo.ItemID, 1, MathUtils.Subtraction(itemLv, (short)5, (short)1), 5, uitemID);
        }

        public static int GetSytnthesisGold(string userID, int itemID)
        {
            int useGold = 0;
            List<ItemSynthesisInfo> synthesisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.ItemID == itemID); //.FindAll(u => u.ItemID.Equals(itemID));//合成物品的材料数组)
            foreach (ItemSynthesisInfo synthesis in synthesisArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(synthesis.SynthesisID);
                if (itemInfo == null)
                {
                    return 0;
                }
                if (!itemInfo.IsGold)
                {
                    useGold = MathUtils.Addition(useGold, 0);
                    continue;
                }
                var itemArray = UserItemHelper.GetItems(userID).FindAll(u => u.ItemID.Equals(itemInfo.ItemID) && (u.ItemStatus == ItemStatus.BeiBao || u.ItemStatus == ItemStatus.CangKu));
                if (itemArray.Count > 0)
                {
                    int curNum = 0;
                    foreach (var item in itemArray)
                    {
                        curNum = MathUtils.Addition(curNum, item.Num, int.MaxValue);
                    }
                    if (synthesis.DemandNum <= curNum)
                    {
                        //useGold = useGold;
                    }
                    else
                    {
                        useGold = MathUtils.Addition(useGold, (MathUtils.Subtraction(synthesis.DemandNum, curNum, 0) * itemInfo.DemandEnergy), int.MaxValue);
                    }

                }
                else
                {
                    useGold = MathUtils.Addition(useGold, (synthesis.DemandNum * itemInfo.DemandEnergy), int.MaxValue);
                }
            }
            return useGold;
        }

        public static bool GetMaterialsNum(string userID, int itemID)
        {
            bool isNotEnough = true;
            List<ItemSynthesisInfo> synthesisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.ItemID == itemID);//合成物品的材料数组
            foreach (ItemSynthesisInfo itemSyn in synthesisArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemSyn.SynthesisID);
                if (itemInfo == null)
                {
                    return false;
                }
                // List<UserItem> itemsList = new List<UserItem>(UserItem.FindAll(UserItem.Index_UserID, u => u.ItemID == itemInfo.ItemID, userID));
                //UserItem[] itemArray = itemsList.FindAll(m => m.ItemStatus != ItemStatus.Sell&&m.);

                var itemArray = UserItemHelper.GetItems(userID).FindAll(u => u.ItemID == itemInfo.ItemID && u.ItemStatus != ItemStatus.Sell && new UserItemHelper(u).GeneralStatus(userID) != GeneralStatus.LiDui);

                if (itemArray.Count > 0)
                {
                    int curNum = 0;
                    foreach (var item in itemArray)
                    {
                        curNum += item.Num;
                    }
                    if (itemSyn.DemandNum <= curNum)
                    {
                        continue;
                    }
                    else
                    {
                        isNotEnough = false;
                    }
                }
                else
                {
                    isNotEnough = false;
                }
            }
            return isNotEnough;
        }

        /// <summary>
        /// 判断是否合成装备
        /// </summary>
        /// <param name="uItemID"></param>
        /// <returns></returns>
        public static bool IsSynthesisEqu(string userId, string uItemID)
        {
            bool isEqu = false;
            var package = UserItemPackage.Get(userId);
            UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(uItemID));
            if (userItem != null)
            {
                ItemBaseInfo itemBaseInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                if (itemBaseInfo != null && itemBaseInfo.ItemType == ItemType.ZhuangBei)
                {
                    isEqu = true;
                }
            }
            return isEqu;
        }

        /// <summary>
        /// 是否有装备
        /// </summary>
        /// <returns></returns>
        public static bool IsHaveEqu(string userID, int itemID)
        {
            bool isequ = false;
            List<ItemSynthesisInfo> synthesisArray = new ConfigCacheSet<ItemSynthesisInfo>().FindAll(u => u.ItemID.Equals(itemID));//合成物品的材料数组
            foreach (ItemSynthesisInfo synthesis in synthesisArray)
            {
                var itemArray = UserItemHelper.GetItems(userID).FindAll(u => u.ItemID.Equals(synthesis.SynthesisID) && u.ItemStatus != ItemStatus.Sell && u.ItemType == ItemType.ZhuangBei && new UserItemHelper(u).GeneralStatus(userID) != GeneralStatus.LiDui);
                if (itemArray.Count > 0)
                {
                    isequ = true;
                    break;
                }
            }
            return isequ;
        }

        /// <summary>
        /// 合成装备需求等级不足
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static bool IsLevelNotEnough(GameUser user, int generalID, int itemID)
        {
            short generalLv = 0;
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (itemInfo == null)
            {
                return false;
            }
            if (generalID == 0)
            {
                generalLv = user.UserLv;// LanguageManager.GetLang().GameUserGeneralID;
            }
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(user.UserID, generalID);
            if (general != null)
            {
                generalLv = general.GeneralLv;
            }
            if (itemInfo.DemandLv > generalLv)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 不可用晶石合成的物品名称
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static string UseritemGoldName(string userID, List<ItemSynthesisInfo> synthesisArray)
        {
            string itemName = string.Empty;
            foreach (ItemSynthesisInfo synthesis in synthesisArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(synthesis.SynthesisID);
                if (itemInfo != null && !itemInfo.IsGold)
                {
                    int itemNum = SpecialItemNum(userID, synthesis.SynthesisID);
                    if (itemNum < synthesis.DemandNum)
                    {
                        itemName = itemInfo.ItemName;
                    }
                }
            }
            return itemName;
        }

        /// <summary>
        /// 玩家剩余的材料
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static int SpecialItemNum(string userID, int itemID)
        {
            int itemNum = 0;
            var package = UserItemPackage.Get(userID);
            UserItemInfo[] itemInfosArray = package.ItemPackage.FindAll(m => m.ItemID.Equals(itemID)).ToArray();
            foreach (UserItemInfo itemInfo in itemInfosArray)
            {
                itemNum += itemInfo.Num;
            }
            return itemNum;
        }

        /// <summary>
        /// 被合成的装备
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static UserItemInfo GetUserItemInfo(string userID, int itemID)
        {
            UserItemInfo userItemInfo = null;
            var userItemsArray = UserItemHelper.GetItems(userID).FindAll(u => u.ItemID == itemID && (u.ItemStatus == ItemStatus.BeiBao || u.ItemStatus == ItemStatus.CangKu));
            if (userItemsArray.Count == 0)
            {
                userItemsArray = UserItemHelper.GetItems(userID).FindAll(u => u.ItemID == itemID && u.ItemStatus == ItemStatus.YongBing && new UserItemHelper(u).GeneralStatus(userID) != GeneralStatus.LiDui);
            }
            if (userItemsArray.Count > 0)
            {
                userItemInfo = userItemsArray[0];
            }
            return userItemInfo;
        }

        /// <summary>
        /// 合成后装备
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="info"></param>
        private void UpdateUserItem(int itemID, UserItemInfo info)
        {
            //原因：合成黄色装备，物品存放为0
            info.ItemID = itemID;
            info.ItemLv = MathUtils.Subtraction(info.ItemLv, (short)5, (short)1);
            var package = UserItemPackage.Get(Uid);
            info.ItemType = ItemType.ZhuangBei;
            package.SaveItem(info);
            //package.DelayChange();
            UserItemHelper.AddItemLog(ContextUser.UserID, itemID, 1, info.ItemLv, 5, info.UserItemID);
        }
    }
}