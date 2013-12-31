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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.ConfigModel;
using ZyGames.Tianjiexing.Model.DataModel;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class DialHelper
    {
        /// <summary>
        /// 检查每日抽奖次数
        /// </summary>
        /// <param name="userID"></param>
        public static void CheckDialNum(string userID)
        {
            UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(userID);
            if (userDial != null && userDial.RefreshDate.Date != DateTime.Now.Date)
            {
                userDial.DialNum = 0;
                userDial.RefreshDate = DateTime.Now;
            }
            if (userDial == null)
            {
                var cacheSet = new GameDataCacheSet<UserDial>();
                userDial = new UserDial();
                userDial.UserID = userID;
                userDial.HeadID = string.Empty;
                userDial.ReturnRatio = 0;
                userDial.DialNum = 0;
                cacheSet.Add(userDial);
            }
        }

        ///// <summary>
        ///// 当日是否有免费次数
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public static bool IsDialFree(string userID)
        //{
        //    UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(userID);
        //    int freeNum = GameConfigSet.FreeSweepstakes;
        //    if (userDial != null && userDial.RefreshDate.Date == DateTime.Now.Date && userDial.DialNum >= freeNum)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// 奖励物品名称
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        public static string PrizeItemName(TreasureInfo treasure)
        {
            string name = string.Empty;
            switch (treasure.Type)
            {
                case RewardType.GameGoin:
                    name = string.Format(LanguageManager.GetLang().St_GameCoin, (int)treasure.Num);
                    break;
                case RewardType.Obtion:
                    name = string.Format(LanguageManager.GetLang().St_ObtionNum, (int)treasure.Num);
                    break;
                case RewardType.ExpNum:
                    name = string.Format(LanguageManager.GetLang().St_ExpNum, (int)treasure.Num);
                    break;
                case RewardType.EnergyNum:
                    name = string.Format(LanguageManager.GetLang().St_EnergyNum, (int)treasure.Num);
                    break;
                case RewardType.Experience:
                    name = string.Format(LanguageManager.GetLang().St_Experience, (int)treasure.Num);
                    break;
                case RewardType.Gold:
                    name = string.Format(LanguageManager.GetLang().St_GiftGoldNum, (int)treasure.Num);
                    break;
                case RewardType.CrystalId:
                    CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(treasure.ItemID);
                    name = crystal == null ? string.Empty : crystal.CrystalName;
                    break;
                case RewardType.Item:
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(treasure.ItemID);
                    name = itemInfo == null ? string.Empty : itemInfo.ItemName;
                    break;
                case RewardType.Spare:
                    SparePartInfo sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(treasure.ItemID);
                    name = sparePartInfo == null ? string.Empty : sparePartInfo.Name;
                    break;
                case RewardType.Enchant:
                    EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(treasure.ItemID);
                    name = enchantInfo == null ? string.Empty : enchantInfo.EnchantName;
                    break;
                case RewardType.Again:
                    name = LanguageManager.GetLang().St12004_RewardSweepstakes;
                    break;
                case RewardType.Mood:
                    name = LanguageManager.GetLang().St12004_DidNotAnyReward;
                    break;
                case RewardType.Recharge:
                    name = string.Format(LanguageManager.GetLang().St12004_RechargeReturn, GetTransformData(treasure.Num));
                    break;
            }
            return name;
        }

        ///// <summary>
        ///// 获取大转盘列表ID
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public static int DialGroupID(string userID)
        //{
        //    int groupID = 0;
        //    int postion = 0;
        //    var dialFreeList = new ConfigCacheSet<DialInfo>().FindAll(m => m.DialType == 1);
        //    var dialGoldList = new ConfigCacheSet<DialInfo>().FindAll(m => m.DialType == 2);
        //    if (dialFreeList.Count == 0 || dialGoldList.Count == 0)
        //    {
        //        return groupID;
        //    }
        //    UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(userID);
        //    if (userDial != null)
        //    {
        //        int freeNum = GameConfigSet.FreeSweepstakes;
        //        if (userDial.RefreshDate.Date == DateTime.Now.Date)
        //        {
        //            freeNum = MathUtils.Subtraction(freeNum, userDial.DialNum);
        //            if (userDial.GroupID > 0)
        //            {
        //                groupID = userDial.GroupID;
        //            }
        //            else if (freeNum > 0 && userDial.GroupID == 0)
        //            {
        //                postion = RandomUtils.GetRandom(0, dialFreeList.Count);
        //                var dialpostion = dialFreeList[postion];
        //                if (dialpostion != null)
        //                {
        //                    groupID = dialpostion.GroupID;
        //                }
        //            }
        //            else
        //            {
        //                postion = RandomUtils.GetRandom(0, dialGoldList.Count);
        //                var dialpostion = dialGoldList[postion];
        //                if (dialpostion != null)
        //                {
        //                    groupID = dialpostion.GroupID;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            userDial.RefreshDate = DateTime.Now;
        //            postion = RandomUtils.GetRandom(0, dialFreeList.Count);
        //            var dialpostion = dialFreeList[postion];
        //            if (dialpostion != null)
        //            {
        //                groupID = dialpostion.GroupID;
        //            }
        //        }
        //        userDial.GroupID = groupID;
        //    }
        //    else
        //    {
        //        var cacheSet = new GameDataCacheSet<UserDial>();
        //        postion = RandomUtils.GetRandom(0, dialFreeList.Count);
        //        var dialpostion = dialFreeList[postion];
        //        if (dialpostion != null)
        //        {
        //            groupID = dialpostion.GroupID;
        //        }
        //        userDial = new UserDial();
        //        userDial.UserID = userID;
        //        userDial.HeadID = string.Empty;
        //        userDial.ReturnRatio = 0;
        //        userDial.DialNum = 0;
        //        cacheSet.Add(userDial);
        //    }
        //    userDial.GroupID = groupID;
        //    userDial.RefreshDate = DateTime.Now;
        //    return groupID;
        //}

        ///// <summary>
        ///// 奖励物品
        ///// </summary>
        ///// <param name="prize"></param>
        ///// <returns></returns>
        //public static void GainTreasureInfo(string userID, DialInfo dialInfo, int count)
        //{
        //    var treasure = new TreasureInfo();
        //    treasure.UserID = userID;
        //    treasure.Type = dialInfo.RewardType;
        //    treasure.ItemID = dialInfo.ItemID;
        //    treasure.Num = dialInfo.ItemNum;
        //    treasure.ItemLv = 1;
        //    UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(userID);
        //    if (userDial == null)
        //    {
        //        userDial = new UserDial();
        //    }
        //    userDial.GroupID = 0;
        //    userDial.RefreshDate = DateTime.Now;
        //    userDial.Treasure.Add(treasure);
        //    if (count == 0)
        //    {
        //        userDial.PrizeInfo = treasure;
        //        userDial.HeadID = dialInfo.HeadID;
        //    }
        //    if (GameConfigSet.Treasure > 0 && userDial.Treasure.Count > GameConfigSet.Treasure)
        //    {
        //        for (int i = 0; i < userDial.Treasure.Count - GameConfigSet.Treasure; i++)
        //        {
        //            userDial.Treasure.Remove(userDial.Treasure[i]);
        //        }
        //    }

        //    if (dialInfo.IsShow)
        //    {
        //        var cacheSet = new ShareCacheStruct<ServerEnvSet>();
        //        var serverEnvSet = cacheSet.FindKey(ServerEnvKey.UserTreasure);
        //        if (serverEnvSet == null)
        //        {
        //            serverEnvSet = new ServerEnvSet();
        //            serverEnvSet.EnvKey = ServerEnvKey.UserTreasure;
        //            cacheSet.Add(serverEnvSet);
        //        }
        //        var treasureInfoList = ServerEnvSetTreasure(treasure);
        //        serverEnvSet.EnvValue = JsonUtils.Serialize(treasureInfoList);
        //    }

        //    if (dialInfo.IsBroadcast && !string.IsNullOrEmpty(dialInfo.BroadContent))
        //    {
        //        GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
        //        if (user != null)
        //        {
        //            TjxChatService chatService = new TjxChatService();
        //            chatService.SystemSend(ChatType.World, string.Format(dialInfo.BroadContent, user.NickName));
        //        }

        //    }
        //    if (dialInfo.RewardType == RewardType.Again || dialInfo.RewardType == RewardType.Mood || dialInfo.RewardType == RewardType.Recharge)
        //    {
        //        if (dialInfo.RewardType == RewardType.Recharge)
        //        {
        //            userDial.ReturnRatio = treasure.Num;
        //        }
        //        return;
        //    }
        //    var cachePrize = new ShareCacheStruct<UserTakePrize>();
        //    var takePrize = GetUserTake(treasure, userID);
        //    cachePrize.Add(takePrize);
        //}

        private static List<TreasureInfo> ServerEnvSetTreasure(TreasureInfo treasure)
        {
            List<TreasureInfo> treasureList = new List<TreasureInfo>();
            var cacheSet = new ShareCacheStruct<ServerEnvSet>();
            var serverEnvSet = cacheSet.FindKey(ServerEnvKey.UserTreasure);
            if (serverEnvSet != null && !string.IsNullOrEmpty(serverEnvSet.EnvValue))
            {
                treasureList = JsonUtils.Deserialize<List<TreasureInfo>>(serverEnvSet.EnvValue);
            }
            treasureList.Add(treasure);
            if (GameConfigSet.UserTreasureNum > 0 && treasureList.Count > GameConfigSet.UserTreasureNum)
            {
                for (int i = 0; i < treasureList.Count - GameConfigSet.UserTreasureNum; i++)
                {
                    treasureList.Remove(treasureList[i]);
                }
            }
            return treasureList;
        }

        ///// <summary>
        ///// 奖励配置
        ///// </summary>
        ///// <param name="user"></param>
        //public static UserTakePrize GetUserTake(TreasureInfo prize, string userID)
        //{
        //    UserTakePrize userPrize = new UserTakePrize();
        //    userPrize.CreateDate = DateTime.Now;
        //    userPrize.ID = Guid.NewGuid().ToString();
        //    userPrize.UserID = Convert.ToInt32(userID);
        //    userPrize.MailContent = LanguageManager.GetLang().St12004_YouWheelOfFortune.Substring(0, 9);
        //    userPrize.IsTasked = false;
        //    userPrize.TaskDate = MathUtils.SqlMinDate;
        //    userPrize.ItemPackage = string.Empty;
        //    userPrize.SparePackage = string.Empty;
        //    userPrize.CrystalPackage = string.Empty;
        //    userPrize.OpUserID = 10000;
        //    switch (prize.Type)
        //    {
        //        case RewardType.GameGoin:
        //            userPrize.GameCoin = (int)prize.Num;
        //            userPrize.MailContent += string.Format(LanguageManager.GetLang().St_GameCoin, userPrize.GameCoin) + ",";
        //            break;
        //        case RewardType.Obtion:
        //            userPrize.ObtainNum = (int)prize.Num;
        //            userPrize.MailContent += string.Format(LanguageManager.GetLang().St_ObtionNum, userPrize.ObtainNum) + ",";
        //            break;
        //        case RewardType.ExpNum:
        //            userPrize.ExpNum = (int)prize.Num;
        //            userPrize.MailContent += string.Format(LanguageManager.GetLang().St_ExpNum, userPrize.ExpNum) + ",";
        //            break;
        //        case RewardType.EnergyNum:
        //            userPrize.EnergyNum = (int)prize.Num;
        //            userPrize.MailContent += string.Format(LanguageManager.GetLang().St_EnergyNum, userPrize.EnergyNum) + ",";
        //            break;
        //        case RewardType.Experience:
        //            break;
        //        case RewardType.Gold:
        //            userPrize.Gold = (int)prize.Num;
        //            userPrize.MailContent += string.Format(LanguageManager.GetLang().St_GiftGoldNum, userPrize.Gold) + ",";
        //            break;
        //        case RewardType.Item:
        //            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prize.ItemID);
        //            if (itemInfo != null)
        //            {
        //                userPrize.ItemPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.ItemLv, (int)prize.Num);
        //                userPrize.MailContent += string.Format("{0}*{1}", itemInfo.ItemName, (int)prize.Num) + ",";
        //            }
        //            break;
        //        case RewardType.CrystalId:
        //            CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(prize.ItemID);
        //            if (crystal != null)
        //            {
        //                userPrize.CrystalPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.ItemLv, (int)prize.Num);
        //                userPrize.MailContent += string.Format("{0}*{1}", crystal.CrystalName, (int)prize.Num) + ",";
        //            }
        //            break;
        //        case RewardType.Spare:
        //            SparePartInfo spare = new ConfigCacheSet<SparePartInfo>().FindKey(prize.ItemID);
        //            if (spare != null)
        //            {
        //                userPrize.SparePackage = string.Format("{0}={1}={2}", prize.ItemID, prize.ItemLv, (int)prize.Num);
        //                userPrize.MailContent += string.Format("{0}*{1}", spare.Name, (int)prize.Num) + ",";
        //            }
        //            break;
        //        case RewardType.Enchant:
        //            EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(prize.ItemID);
        //            if (enchantInfo != null)
        //            {
        //                userPrize.EnchantPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.ItemLv, (int)prize.Num);
        //                userPrize.MailContent += string.Format("{0}*{1}", enchantInfo.EnchantName, (int)prize.Num) + ",";
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //    userPrize.MailContent = userPrize.MailContent.TrimEnd(',');
        //    return userPrize;
        //}

        /// <summary>
        /// 充值返还
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="goldNum"></param>
        /// <returns></returns>
        public static void ReturnRatioGold(string userID, int goldNum)
        {
            UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(userID);
            if (userDial != null && userDial.ReturnRatio > 0)
            {
                int giftGold = MathUtils.RoundCustom(goldNum * userDial.ReturnRatio);
                var cachePrize = new ShareCacheStruct<UserTakePrize>();
                UserTakePrize userPrize = new UserTakePrize();
                userPrize.CreateDate = DateTime.Now;
                userPrize.ID = Guid.NewGuid().ToString();
                userPrize.UserID = Convert.ToInt32(userID);
                userPrize.MailContent = string.Format(LanguageManager.GetLang().St12004_RechargeReturnGoldNum, GetTransformData(userDial.ReturnRatio), giftGold);
                userPrize.IsTasked = false;
                userPrize.TaskDate = MathUtils.SqlMinDate;
                userPrize.ItemPackage = string.Empty;
                userPrize.SparePackage = string.Empty;
                userPrize.CrystalPackage = string.Empty;
                userPrize.EnchantPackage = string.Empty;
                userPrize.OpUserID = 10000;
                userPrize.GameCoin = 0;
                userPrize.ObtainNum = 0;
                userPrize.ExpNum = 0;
                userPrize.EnergyNum = 0;
                userPrize.Gold = giftGold;
                userDial.ReturnRatio = 0;
                cachePrize.Add(userPrize);
            }
        }


        ///// <summary>
        ///// 大转盘抽奖获得物品与位置
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="ops"></param>
        ///// <returns></returns>
        //public static short DialPrizePostion(string userID, int ops)
        //{
        //    UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(userID);
        //    if (userDial == null)
        //    {
        //        return 0;
        //    }
        //    int postion = 0;
        //    int groupId = DialGroupID(userID);
        //    var dialList = new ConfigCacheSet<DialInfo>().FindAll(m => m.GroupID == groupId);
        //    double[] dialDoubleList = new double[dialList.Count];
        //    for (int i = 0; i < dialList.Count; i++)
        //    {
        //        dialDoubleList[i] = (double)dialList[i].Probability;
        //    }
        //    if (ops == 1 || ops == 3)
        //    {
        //        postion = (short)RandomUtils.GetHitIndex(dialDoubleList);
        //        if (dialList.Count > postion)
        //        {
        //            DialInfo dialInfo = dialList[postion];
        //            if (dialInfo != null)
        //            {
        //                GainTreasureInfo(userID, dialInfo, 0);
        //                if (dialInfo.RewardType == RewardType.Again && ops == 3)
        //                {
        //                    userDial.DialNum = MathUtils.Subtraction(userDial.DialNum, (short)1);
        //                }
        //                else if (dialInfo.RewardType != RewardType.Again && ops == 1)
        //                {
        //                    userDial.DialNum = MathUtils.Addition(userDial.DialNum, (short)1);
        //                }
        //            }
        //        }
        //    }
        //    else if (ops == 5)
        //    {
        //        int index = 0;
        //        for (int i = 0; i < 5; i++)
        //        {
        //            index = (short)RandomUtils.GetHitIndex(dialDoubleList);
        //            if (dialList.Count > index)
        //            {
        //                DialInfo dialInfo = dialList[index];
        //                if (dialInfo != null)
        //                {
        //                    GainTreasureInfo(userID, dialInfo, i);
        //                    if (dialInfo.RewardType == RewardType.Again)
        //                    {
        //                        userDial.DialNum = MathUtils.Subtraction(userDial.DialNum, (short)1);
        //                    }
        //                }
        //            }
        //            if (i == 0)
        //            {
        //                postion = index;
        //            }
        //        }
        //    }
        //    return (short)postion;
        //}

        /// <summary>
        /// 数据加%
        /// </summary>
        /// <returns></returns>
        public static string GetTransformData(decimal dataNum)
        {
            string dateStr = string.Empty;
            if (dataNum > 0)
            {
                double str = (double)dataNum * 100;
                dateStr = MathUtils.RoundCustom(str, 2) + "%";
            }
            return dateStr;
        }

        /// <summary>
        /// 获取大转盘列表ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int ChestDialGroupID(string userID, int itemID)
        {
            int groupID = 0;
            var dialFreeList = new ConfigCacheSet<DialInfo>().FindAll(m => m.DialType == itemID.ToShort());
            if (dialFreeList.Count == 0)
            {
                return groupID;
            }
            UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(userID);
            if (userDial == null)
            {
                userDial = new UserDial();
                userDial.UserID = userID;
                var cacheSet = new GameDataCacheSet<UserDial>();
                cacheSet.Add(userDial);
            }
            int postion = RandomUtils.GetRandom(0, dialFreeList.Count);
            var dialpostion = dialFreeList[postion];
            if (dialpostion != null)
            {
                groupID = dialpostion.GroupID;
                userDial.GroupID = groupID;
            }
            userDial.HeadID = string.Empty;
            userDial.ReturnRatio = 0;
            userDial.DialNum = 0;
            userDial.RefreshDate = DateTime.Now;
            return groupID;
        }

        /// <summary>
        /// 奖励物品时是否背包已满
        /// </summary>
        /// <param name="user"></param>
        /// <param name="prizeContent"></param>
        /// <returns></returns>
        public static bool IsPrizeItemName(GameUser user, out string prizeContent)
        {
            prizeContent = string.Empty;
            if (!CrystalHelper.CheckAllowCrystall(user))
            {
                prizeContent = LanguageManager.GetLang().St1305_FateBackpackFull;
                return false;
            }

            if (UserItemHelper.CheckItemOut(user, ItemStatus.BeiBao))
            {
                prizeContent = LanguageManager.GetLang().St1606_GridNumNotEnough;
                return false;
            }

            if (UserHelper.IsSpareGridNum(user, 0))
            {
                prizeContent = LanguageManager.GetLang().St1213_GridNumFull;
                return false;
            }

            if (EnchantHelper.IsEnchantPackage(user.UserID))
            {
                prizeContent = LanguageManager.GetLang().St1259_EnchantGridNumFull;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 宝箱抽奖获得物品与位置
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static short ChestDialPrizePostion(GameUser user)
        {
            UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(user.UserID);
            if (userDial == null)
            {
                return 0;
            }
            short postion = 0;
            int groupId = userDial.GroupID; //UserItemHelper.GetUserItemInfoID(user.UserID, userDial.UserItemID);
            var dialList = new ConfigCacheSet<DialInfo>().FindAll(m => m.GroupID == groupId);
            double[] dialDoubleList = new double[dialList.Count];
            for (int i = 0; i < dialList.Count; i++)
            {
                dialDoubleList[i] = (double)dialList[i].Probability;
            }
            postion = (short)RandomUtils.GetHitIndex(dialDoubleList);
            if (dialList.Count > postion)
            {
                DialInfo dialInfo = dialList[postion];
                if (dialInfo != null)
                {
                    ChestGainTreasureInfo(user, dialInfo, 0);
                }
            }
            return postion;
        }

        /// <summary>
        /// 奖励物品
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        public static void ChestGainTreasureInfo(GameUser user, DialInfo dialInfo, int count)
        {
            var treasure = new TreasureInfo();
            treasure.UserID = user.UserID;
            treasure.Type = dialInfo.RewardType;
            treasure.ItemID = dialInfo.ItemID;
            treasure.Num = dialInfo.ItemNum;
            treasure.ItemLv = 1;
            UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(user.UserID);
            if (userDial == null)
            {
                userDial = new UserDial();
            }
            userDial.GroupID = 0;
            userDial.RefreshDate = DateTime.Now;
            userDial.Treasure.Add(treasure);
            if (count == 0)
            {
                userDial.PrizeInfo = treasure;
                userDial.HeadID = dialInfo.HeadID;
            }
            if (GameConfigSet.Treasure > 0 && userDial.Treasure.Count > GameConfigSet.Treasure)
            {
                for (int i = 0; i < userDial.Treasure.Count - GameConfigSet.Treasure; i++)
                {
                    userDial.Treasure.Remove(userDial.Treasure[i]);
                }
            }

            if (dialInfo.IsShow)
            {
                var cacheSet = new ShareCacheStruct<ServerEnvSet>();
                var serverEnvSet = cacheSet.FindKey(ServerEnvKey.UserTreasure);
                if (serverEnvSet == null)
                {
                    serverEnvSet = new ServerEnvSet();
                    serverEnvSet.EnvKey = ServerEnvKey.UserTreasure;
                    cacheSet.Add(serverEnvSet);
                }
                var treasureInfoList = ServerEnvSetTreasure(treasure);
                serverEnvSet.EnvValue = JsonUtils.Serialize(treasureInfoList);
            }

            if (dialInfo.IsBroadcast && !string.IsNullOrEmpty(dialInfo.BroadContent))
            {
                TjxChatService chatService = new TjxChatService();
                chatService.SystemSend(ChatType.World, string.Format(dialInfo.BroadContent, user.NickName));

            }
            if (dialInfo.RewardType == RewardType.Again || dialInfo.RewardType == RewardType.Mood || dialInfo.RewardType == RewardType.Recharge)
            {
                if (dialInfo.RewardType == RewardType.Recharge)
                {
                    userDial.ReturnRatio = treasure.Num;
                }
                return;
            }
            ChestGetUserTake(treasure, user);
        }

        /// <summary>
        /// 奖励配置
        /// </summary>
        /// <param name="user"></param>
        public static void ChestGetUserTake(TreasureInfo prize, GameUser user)
        {
            switch (prize.Type)
            {
                case RewardType.GameGoin:
                    user.GameCoin = MathUtils.Addition(user.GameCoin, prize.Num.ToInt());
                    break;
                case RewardType.Obtion:
                    user.ObtainNum = MathUtils.Addition(user.ObtainNum, prize.Num.ToInt());
                    break;
                case RewardType.ExpNum:
                    user.ExpNum = MathUtils.Addition(user.ExpNum, prize.Num.ToInt());
                    break;
                case RewardType.EnergyNum:
                    user.EnergyNum = MathUtils.Addition(user.EnergyNum, prize.Num.ToShort());
                    break;
                case RewardType.Experience:
                    break;
                case RewardType.Gold:
                    user.GiftGold = MathUtils.Addition(user.GiftGold, prize.Num.ToInt());
                    break;
                case RewardType.Item:
                    if (!UserAbilityHelper.UseUserItem(user.UserID, prize.ItemID))
                    {
                        UserItemHelper.AddUserItem(user.UserID, prize.ItemID, prize.Num.ToInt());
                    }
                    break;
                case RewardType.CrystalId:
                    short crystalLv = prize.ItemLv > 0 ? prize.ItemLv : 1.ToShort();
                    CrystalHelper.AppendCrystal(user.UserID, prize.ItemID, crystalLv);
                    break;
                case RewardType.Spare:
                    AppendSpare(user, prize.ItemID);
                    break;
                case RewardType.Enchant:
                    short enchantLv = prize.ItemLv > 0 ? prize.ItemLv : 1.ToShort();
                    EnchantHelper.AppendEnchat(user.UserID, prize.ItemID, enchantLv);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获得灵件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="spareID"></param>
        public static void AppendSpare(GameUser user, int spareID)
        {
            SparePartInfo partInfo = new ConfigCacheSet<SparePartInfo>().FindKey(spareID);
            UserSparePart sparePart = UserSparePart.GetRandom(spareID);
            if (partInfo == null || sparePart == null || !SparePartInfo.IsExist(spareID))
            {
                return;
            }
            if (!UserHelper.AddSparePart(user, sparePart))
            {
                var chatService = new TjxChatService();
                //掉落灵件
                chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().St4303_SparePartFalling, partInfo.Name));
            }
        }
    }
}