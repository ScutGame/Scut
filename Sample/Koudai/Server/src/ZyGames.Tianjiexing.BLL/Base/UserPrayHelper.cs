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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 背包格子类
    /// </summary>
    public class UserPrayHelper
    {
        public static GameDataCacheSet<UserPray> _cacheSetUserPray = new GameDataCacheSet<UserPray>();
        public static ConfigCacheSet<PrayInfo> _cacheSetPray = new ConfigCacheSet<PrayInfo>();
        public static GameDataCacheSet<GameUser> _cacheSetGameUser = new GameDataCacheSet<GameUser>();
        public static GameDataCacheSet<UserItemPackage> _cacheSetUserItem = new GameDataCacheSet<UserItemPackage>();
        public static GameDataCacheSet<UserPack> _cacheSetUserPack = new GameDataCacheSet<UserPack>();
        /// <summary>
        /// 注册默认添加祈祷
        /// </summary>
        /// <param name="userId"></param>
        public static void AddUserPray(int userId, PrayType prayType)
        {
            var userPray = _cacheSetUserPray.FindKey(userId.ToString());
            if (userPray != null)
            {
                userPray.PrayDate = DateTime.Now.AddDays(-1);
                userPray.PrayNum = 0;
                userPray.IsPray = false;
                userPray.PrayType = prayType;
            }
            else
            {
                userPray = new UserPray(userId);
                userPray.PrayDate = DateTime.Now.AddDays(-1);
                userPray.PrayNum = 0;
                userPray.IsPray = false;
                userPray.PrayType = prayType;
                _cacheSetUserPray.Add(userPray, userId);
            }
        }

        /// <summary>
        /// 奖励配置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status">状态值为1时，下发图片</param>
        public static string GetUserTake(List<PrizeInfo> prizeInfoList, string userID, int status = 0)
        {
            string HeadID = string.Empty;
            int quality = 0;
            List<PrizeInfo> prizeList = new List<PrizeInfo>();
            var prize = prizeInfoList[0];
            string content = string.Empty;
            switch (prize.Reward)
            {
                //随机
                case 1:
                    if (prizeInfoList.Count > 0)
                    {
                        int randomNum = RandomUtils.GetRandom(0, prizeInfoList.Count);
                        prizeList.Add(prizeInfoList[randomNum]);
                    }

                    break;
                //概率
                case 2:
                    int count = prizeInfoList.Count;
                    int[] precent = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        var prize2 = prizeInfoList[i];
                        precent[i] = (prize2.Probability * 1000).ToInt();

                    }
                    int index = RandomUtils.GetHitIndexByTH(precent);
                    prizeList.Add(prizeInfoList[index]);
                    break;
                //全部
                case 3:
                    prizeList.AddRange(prizeInfoList);
                    break;
            }
            prizeList.ForEach(prizeInfo =>
            {
                var cacheSetUser = new GameDataCacheSet<GameUser>();
                var user = cacheSetUser.FindKey(userID);

                if (user != null)
                {
                    switch (prizeInfo.Type)
                    {
                        case RewardType.GameGoin:
                            //content += string.Format(LanguageManager.GetLang().St_GameCoin, prizeInfo.Num) + ",";
                            //user.GameCoin = MathUtils.Addition(user.GameCoin, prizeInfo.Num);
                            HeadID = prizeInfo.HeadID;
                            // 1: 乘玩家等级
                            if (prizeInfo.IsMultiplyUserLv == 1)
                            {
                                int coinNumber = 0;
                                // 获取当前玩家等级
                                if (user.UserLv >= 1 && user.UserLv <= 100)
                                {
                                    coinNumber = prizeInfo.Num * user.UserLv;
                                }

                                else if (user.UserLv < 1)
                                {
                                    coinNumber = prizeInfo.Num * 1;
                                }
                                else
                                {
                                    coinNumber = prizeInfo.Num * 100;
                                }
                                content += string.Format(LanguageManager.GetLang().St_GameCoin, coinNumber) + ",";
                                user.GameCoin = MathUtils.Addition(user.GameCoin, coinNumber);
                            }
                            else
                            {
                                content += string.Format(LanguageManager.GetLang().St_GameCoin, prizeInfo.Num) + ",";
                                user.GameCoin = MathUtils.Addition(user.GameCoin, prizeInfo.Num);
                            }
                            break;
                        case RewardType.Gold:
                            HeadID = prizeInfo.HeadID;
                            content += string.Format(LanguageManager.GetLang().St_GiftGoldNum, prizeInfo.Num) + ",";
                            user.GiftGold = MathUtils.Addition(user.GiftGold, prizeInfo.Num);
                            break;
                        case RewardType.EnergyNum:
                            content += string.Format(LanguageManager.GetLang().St_EnergyNum, prizeInfo.Num) + ",";
                            user.EnergyNum = MathUtils.Addition(user.EnergyNum, prizeInfo.Num).ToShort();
                            break;
                        case RewardType.ExpNum:
                            content += string.Format(LanguageManager.GetLang().St_ExpNum, prizeInfo.Num) + ",";
                            user.ExpNum = MathUtils.Addition(user.ExpNum, prizeInfo.Num);
                            break;
                        case RewardType.Experience:
                            content += string.Format(LanguageManager.GetLang().St_Experience, prizeInfo.Num) + ",";
                            GeneralEscalateHelper.AddUserLv(user, prizeInfo.Num);
                            break;
                        case RewardType.CrystalId:
                            var crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(prizeInfo.ItemID);
                            if (crystalInfo != null)
                            {
                                //content += string.Format(LanguageManager.GetLang().St_Crystal, crystalInfo.CrystalName) + ",";
                                content += crystalInfo.CrystalName + ",";
                                CrystalHelper.AppendCrystal(user.UserID, crystalInfo.CrystalID, prizeInfo.UserLv);
                            }
                            break;

                        case RewardType.Item:
                            var itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prizeInfo.ItemID);
                            if (itemInfo != null)
                            {
                                //content += string.Format(LanguageManager.GetLang().St_Item, itemInfo.ItemName,  prizeInfo.Num) + ",";
                                //UserItemHelper.AddUserItem(user.UserID, prizeInfo.ItemID, prizeInfo.Num, prizeInfo.UserLv);
                                if (status == 1)
                                {
                                    HeadID = prizeInfo.HeadID;
                                    content += string.Format(LanguageManager.GetLang().St_ItemReward, itemInfo.ItemName, prizeInfo.Num) + ",";
                                    UserItemHelper.AddUserItem(user.UserID, prizeInfo.ItemID, prizeInfo.Num, prizeInfo.UserLv);
                                }
                                else
                                {
                                    content += string.Format(LanguageManager.GetLang().St_Item, itemInfo.ItemName, prizeInfo.UserLv, prizeInfo.Num) + ",";
                                    UserItemHelper.AddUserItem(user.UserID, prizeInfo.ItemID, prizeInfo.Num, prizeInfo.UserLv);
                                }
                            }
                            break;

                        case RewardType.MonsterCard:
                            //  获取 JSON 中的怪物记录
                            var monsterCard = prizeInfoList.Find(s => s.Type == RewardType.MonsterCard);

                            var userPlotInfo = new GameDataCacheSet<UserPlotPackage>().FindKey(user.UserID);
                            if (userPlotInfo != null)
                            {
                                List<PlotNPCInfo> plotNpcInfoList = new List<PlotNPCInfo>();
                                var userPlotPackageList = userPlotInfo.PlotPackage.FindAll(x => x.PlotStatus == PlotStatus.Completed);
                                // PlotID
                                if (userPlotPackageList.Count > 0)
                                {
                                    userPlotPackageList.ForEach(userPlot =>
                                                                    {
                                                                        var plotNPCInfo = new ShareCacheStruct<PlotNPCInfo>().Find(x => x.PlotID == userPlot.PlotID);
                                                                        if (plotNPCInfo != null)
                                                                        {
                                                                            plotNpcInfoList.Add(plotNPCInfo);
                                                                        }
                                                                    });
                                    if (plotNpcInfoList.Count > 0)
                                    {

                                        int index = RandomUtils.GetRandom(0, plotNpcInfoList.Count);
                                        int plotNpcID = plotNpcInfoList[index].PlotNpcID;

                                        var plotEmbattleInfo = new ShareCacheStruct<PlotEmbattleInfo>().Find(x => x.PlotNpcID == plotNpcID);

                                        if (plotEmbattleInfo != null)
                                        {
                                            var monsterInfo = new ShareCacheStruct<MonsterInfo>().FindKey(plotEmbattleInfo.MonsterID);
                                            if (monsterInfo != null)
                                            {
                                                var itemMonster = new ShareCacheStruct<ItemBaseInfo>().FindKey(monsterInfo.ItemID);
                                                HeadID = itemMonster.HeadID;

                                                content += string.Format(LanguageManager.GetLang().St_MonsterCard, itemMonster.ItemName,
                                                             prizeInfo.Num) + ",";
                                                UserItemHelper.AddUserItem(user.UserID, monsterInfo.ItemID, prizeInfo.Num, prizeInfo.UserLv);
                                            }
                                        }
                                    }
                                }
                                #region 注释
                                //else
                                //{
                                //    // 副本没有通关默认奖励为第一种金币奖励
                                //    HeadID = prize.HeadID;
                                //    int coinNumber = prize.Num;  // 第一条记录中金币数
                                //    // 获取当前玩家等级
                                //    if (user.UserLv >= 1 && user.UserLv <= 100)
                                //    {
                                //        coinNumber = coinNumber * user.UserLv;
                                //    }

                                //    else if (user.UserLv < 1)
                                //    {
                                //        coinNumber = coinNumber * 1;
                                //    }
                                //    else
                                //    {
                                //        coinNumber = coinNumber * 100;
                                //    }
                                //    content += string.Format(LanguageManager.GetLang().St_GameCoin, coinNumber) + ",";
                                //    user.GameCoin = MathUtils.Addition(user.GameCoin, coinNumber);
                                //}
                                #endregion
                            }

                            break;

                        case RewardType.GeneralSoul:
                            var generalInfoList = new ShareCacheStruct<GeneralInfo>().FindAll(x => x.GeneralQuality == prizeInfo.GeneralQuality);
                            if (generalInfoList != null)
                            {
                                if (generalInfoList.Count > 0)
                                {
                                    int index = RandomUtils.GetRandom(0, generalInfoList.Count);
                                    GeneralInfo generalInfo = generalInfoList[index];
                                    HeadID = generalInfo.HeadID;
                                    quality = generalInfo.GeneralQuality.ToInt();
                                    content += string.Format(LanguageManager.GetLang().St_GeneralSoul,
                                                       generalInfo.GeneralName, prizeInfo.Num) + ",";
                                    UserItemHelper.AddGeneralSoul(user.UserID, generalInfo.SoulID, prizeInfo.Num);
                                }
                            }
                            break;

                        case RewardType.Ability:
                            // 查找绿色技能列表
                            var abilityInfoList = new ShareCacheStruct<AbilityInfo>().FindAll(x => x.AbilityQuality == prizeInfo.AbilityQuality);
                            if (abilityInfoList != null)
                            {
                                if (abilityInfoList.Count > 0)
                                {

                                    int index = RandomUtils.GetRandom(0, abilityInfoList.Count);
                                    AbilityInfo abilityInfo = abilityInfoList[index];
                                    HeadID = abilityInfo.HeadID;
                                    quality = abilityInfo.AbilityQuality;
                                    content += string.Format(LanguageManager.GetLang().St_Ability, abilityInfo.AbilityName, prizeInfo.Num) + ",";
                                    UserAbilityHelper.AddUserAbility(abilityInfo.AbilityID, Convert.ToInt32(user.UserID), 0, 0);
                                }

                            }
                            break;
                    }
                }
                // 状态值为1时下发图片
                if (status == 1)
                {
                    #region  如果奖励为空，则默认奖励金币

                    string[] rewardInfo = content.TrimEnd(',').Split('*');
                    if (String.IsNullOrEmpty(rewardInfo[0]))
                    {
                        // 默认奖励为第一种金币奖励
                        HeadID = prize.HeadID;
                        int coinNumber = prize.Num;  // 第一条记录中金币数
                        // 获取当前玩家等级
                        if (user.UserLv >= 1 && user.UserLv <= 100)
                        {
                            coinNumber = coinNumber * user.UserLv;
                        }

                        else if (user.UserLv < 1)
                        {
                            coinNumber = coinNumber * 1;
                        }
                        else
                        {
                            coinNumber = coinNumber * 100;
                        }
                        content += string.Format(LanguageManager.GetLang().St_GameCoin, coinNumber) + ",";
                        user.GameCoin = MathUtils.Addition(user.GameCoin, coinNumber);
                    }

                    #endregion
                    content = content.TrimEnd(',') + "*" + HeadID + "*" + quality + ",";
                }
            });
            content = content.TrimEnd(',');

            return content;
        }
    }
}