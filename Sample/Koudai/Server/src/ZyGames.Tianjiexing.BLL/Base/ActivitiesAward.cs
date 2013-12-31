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
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;



namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// (节日)活动奖励
    /// </summary>
    public class ActivitiesAward
    {
        /// <summary>
        /// 父亲节奖励
        /// </summary>
        public static void FathersDay(GameUser user)
        {
            short energyNum = 30;
            int obtainNum = 50;
            int gameGoin = 20000;
            string fatherTime = "2012-06-17 00:00:00";
            DateTime fatherDate = DateTime.Parse(fatherTime);
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
            if (dailyRestrain != null && dailyRestrain.Funtion13 < 1)
            {
                if (DateTime.Now.Date == fatherDate.Date)
                {
                    dailyRestrain.Funtion13 = 1;
                    //dailyRestrain.Update();
                    user.EnergyNum = MathUtils.Addition(user.EnergyNum, energyNum, short.MaxValue);
                    user.ObtainNum = MathUtils.Addition(user.ObtainNum, obtainNum, int.MaxValue);
                    user.GameCoin = MathUtils.Addition(user.GameCoin, gameGoin, int.MaxValue);
                    //user.Update();
                    string content = string.Format(LanguageManager.GetLang().St_FathersDay,
                                                   energyNum, obtainNum,
                                                   gameGoin);
                    new TjxChatService().SystemSendWhisper(user, content);
                }
            }
        }

        /// <summary>
        /// 七夕、端午节奖励
        /// </summary>
        public static void DragonBoatFestival(string userID, int festivalID)
        {
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(userID, festivalID);
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            var chatService = new TjxChatService();
            if (festival != null)
            {
                CacheList<PrizeInfo> rewardsArray = festival.Reward;
                if (NoviceHelper.IsFestivalOpen(festivalID))
                {
                    if (restrain != null && restrain.RestrainNum <= festival.RestrainNum)
                    {
                        restrain.RestrainNum = MathUtils.Addition(restrain.RestrainNum, 1, int.MaxValue);
                        restrain.RefreashDate = DateTime.Now;
                        //restrain.Update();
                        foreach (PrizeInfo reward in rewardsArray)
                        {
                            GameUserReward(userID, reward.Type, reward.ItemID, reward.Num);
                        }
                        if (userInfo != null && festival.FestivalExtend != null)
                        {
                            chatService.SystemSendWhisper(userInfo, festival.FestivalExtend.Desc);
                        }
                    }
                    else if (restrain == null)
                    {
                        foreach (PrizeInfo reward in rewardsArray)
                        {
                            GameUserReward(userID, reward.Type, reward.ItemID, reward.Num);
                        }
                        if (userInfo != null && festival.FestivalExtend != null)
                        {
                            chatService.SystemSendWhisper(userInfo, festival.FestivalExtend.Desc);
                        }
                        restrain = new FestivalRestrain
                                       {
                                           UserID = userID,
                                           FestivalID = festivalID,
                                           RefreashDate = DateTime.Now,
                                           RestrainNum = 1,
                                       };
                        new GameDataCacheSet<FestivalRestrain>().Add(restrain);
                    }
                }
            }
        }

        /// <summary>
        /// 活动奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="reward"></param>
        /// <param name="num"></param>
        public static void GameUserReward(string userID, RewardType reward, int itemID, int num)
        {
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (userInfo != null)
            {
                if (reward == RewardType.GameGoin)
                {
                    userInfo.GameCoin = MathUtils.Addition(userInfo.GameCoin, num, int.MaxValue);
                    //userInfo.Update();
                }
                else if (reward == RewardType.Obtion)
                {
                    userInfo.ObtainNum = MathUtils.Addition(userInfo.ObtainNum, num, int.MaxValue);
                    //userInfo.Update();
                }
                else if (reward == RewardType.EnergyNum)
                {
                    userInfo.EnergyNum = MathUtils.Addition(userInfo.EnergyNum, (short)num, short.MaxValue);
                    //userInfo.Update();
                }
                else if (reward == RewardType.ExpNum)
                {
                    userInfo.ExpNum = MathUtils.Addition(userInfo.ExpNum, num, int.MaxValue);
                    //userInfo.Update();
                }
                else if (reward == RewardType.Gold)
                {
                    userInfo.ItemGold = MathUtils.Addition(userInfo.ItemGold, num, int.MaxValue);
                    //userInfo.Update();
                }
                else if (reward == RewardType.Experience)
                {
                    UserHelper.UserGeneralExp(userID, num);
                }
                else if (reward == RewardType.Item)
                {
                    UserItemHelper.AddUserItem(userID, itemID, num);
                }
                else if (reward == RewardType.CrystalType)
                {
                    CrystalQualityType qualityType = CrystalQualityType.PurPle;
                    GetUserCrystal(userInfo, qualityType);
                }
                else if (reward == RewardType.CrystalId)
                {
                    UserHelper.CrystalAppend(userID, itemID, 2);
                }
            }
        }

        /// <summary>
        /// 活动奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="reward"></param>
        /// <param name="num"></param>
        public static string GameUserRewardNocite(string userID, RewardType reward, int itemID, int num, CrystalQualityType qualityType)
        {
            string content = string.Empty;
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (userInfo != null)
            {
                if (reward == RewardType.GameGoin)
                {
                    userInfo.GameCoin = MathUtils.Addition(userInfo.GameCoin, num, int.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().St_SummerThreeGameCoinNotice, num);
                }
                else if (reward == RewardType.Obtion)
                {
                    userInfo.ObtainNum = MathUtils.Addition(userInfo.ObtainNum, num, int.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().St_SummerThreeObtionNotice, num);
                }
                else if (reward == RewardType.EnergyNum)
                {
                    userInfo.EnergyNum = MathUtils.Addition(userInfo.EnergyNum, (short)num, short.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().St_SummerThreeEnergyNotice, num);
                }
                else if (reward == RewardType.ExpNum)
                {
                    userInfo.ExpNum = MathUtils.Addition(userInfo.ExpNum, num, int.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().St_SummerThreeExpNumNotice, num);
                }
                else if (reward == RewardType.Gold)
                {
                    userInfo.ItemGold = MathUtils.Addition(userInfo.ItemGold, num, int.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().St_SummerThreeGoldNotice, num);
                }
                else if (reward == RewardType.Experience)
                {
                    UserHelper.UserGeneralExp(userID, num);
                    content = string.Format(LanguageManager.GetLang().St_SummerThreeExperienceNotice, num);
                }
                else if (reward == RewardType.Item)
                {
                    UserItemHelper.AddUserItem(userID, itemID, num);
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
                    if (itemInfo != null)
                    {
                        content = string.Format(LanguageManager.GetLang().St_SummerThreeItemNotice, num, itemInfo.ItemName);
                    }
                }
                else if (reward == RewardType.CrystalType)
                {
                    GetUserCrystal(userInfo, qualityType);
                }
                else if (reward == RewardType.CrystalId)
                {
                    UserHelper.CrystalAppend(userID, itemID, 2);
                    CrystalInfo info = new ConfigCacheSet<CrystalInfo>().FindKey(itemID);
                    if (info != null)
                    {
                        content = string.Format(LanguageManager.GetLang().St_SummerCrystalNotice, info.CrystalName);
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 奖励命运水晶
        /// </summary>
        public static void GetUserCrystal(GameUser user, CrystalQualityType qualityType)
        {
            List<CrystalInfo> crystalArray2 = new ConfigCacheSet<CrystalInfo>().FindAll(m => m.CrystalQuality == qualityType);
            if (crystalArray2.Count > 0)
            {
                int randomNum = RandomUtils.GetRandom(0, crystalArray2.Count);
                CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(crystalArray2[randomNum].CrystalID);
                UserHelper.CrystalAppend(user.UserID, crystal.CrystalID, 2);
                string chatcontent = string.Empty;
                if (crystal.CrystalQuality == CrystalQualityType.Orange)
                {
                    chatcontent = LanguageManager.GetLang().St1305_GainQualityNotice;
                }
                else
                {
                    chatcontent = LanguageManager.GetLang().St1305_HighQualityNotice;
                }
                string content = string.Format(chatcontent,
                    user.NickName,
                    CrystalHelper.GetQualityName(crystal.CrystalQuality),
                    crystal.CrystalName
                    );
                new TjxChatService().SystemSend(ChatType.World, content);
            }
        }

        /// <summary>
        /// 假日狂欢活动--竞技场幸运数字七
        /// </summary>
        /// <param name="userID"></param>
        public static void HolidayFestival(string userID)
        {
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            int festivalID = 0;// 1004;
            FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(userID, festivalID);
            if (restrain != null)
            {
                if (restrain.RefreashDate.Date != DateTime.Now.Date)
                {
                    restrain.RestrainNum = 0;
                    restrain.RefreashDate = DateTime.Now;
                }
            }
            var chatService = new TjxChatService();
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                List<PrizeInfo> rewardsArray = festival.Reward.ToList();
                if (NoviceHelper.IsFestivalOpen(festivalID))
                {
                    if (restrain != null && restrain.RestrainNum <= festival.RestrainNum)
                    {
                        restrain.RestrainNum = MathUtils.Addition(restrain.RestrainNum, 1, int.MaxValue);
                        restrain.RefreashDate = DateTime.Now;
                        //restrain.Update();
                        foreach (PrizeInfo reward in rewardsArray)
                        {
                            GameUserReward(userID, reward.Type, reward.ItemID, reward.Num);
                        }
                        if (userInfo != null)
                        {
                            chatService.SystemSendWhisper(userInfo, LanguageManager.GetLang().St_HolidayFestival);
                        }
                    }
                    else if (restrain == null)
                    {
                        foreach (PrizeInfo reward in rewardsArray)
                        {
                            GameUserReward(userID, reward.Type, reward.ItemID, reward.Num);
                        }
                        if (userInfo != null)
                        {
                            chatService.SystemSendWhisper(userInfo, LanguageManager.GetLang().St_HolidayFestival);
                        }
                        restrain = new FestivalRestrain
                                       {
                                           UserID = userID,
                                           FestivalID = festivalID,
                                           RefreashDate = DateTime.Now,
                                           RestrainNum = 1,
                                       };
                        new GameDataCacheSet<FestivalRestrain>().Add(restrain);
                    }
                }
            }
        }

        /// <summary>
        /// 假日狂欢活动通关获得金币
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        public static void GetHolidayFestivalReward(string userID, int plotID)
        {
            var chatService = new TjxChatService();
            int festivalID = 0;// 1003;
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
                if (plotInfo != null)
                {
                    TimePriod priod = festival.TimePriod;
                    if (NoviceHelper.IsFestivalOpen(festivalID))
                    {
                        DateTime priodStart = DateTime.Parse(DateTime.Now.ToString("d") + " " + priod.Start.ToString("T"));
                        DateTime priodEnd = DateTime.Parse(DateTime.Now.ToString("d") + " " + priod.End.ToString("T"));
                        if (priodStart <= DateTime.Now && DateTime.Now < priodEnd)
                        {
                            if (RandomUtils.IsHit(plotInfo.FestivalProbability))
                            {
                                List<FestivalReward> rewardsArray = plotInfo.FestivalReward.ToList();
                                double[] probability = new double[rewardsArray.Count];
                                for (int i = 0; i < rewardsArray.Count; i++)
                                {
                                    probability[i] = rewardsArray[i].Probability;
                                }
                                int index2 = RandomUtils.GetHitIndex(probability);
                                int itemID = rewardsArray[index2].Item;
                                UserItemHelper.AddUserItem(userID, itemID, 1);
                                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
                                if (userInfo != null && itemInfo != null)
                                {
                                    string content = string.Empty;
                                    if (itemID == 5008)
                                    {
                                        content = string.Format(LanguageManager.GetLang().St_HolidayFestivalGift, itemInfo.ItemName);
                                        chatService.SystemSendWhisper(userInfo, content);
                                    }
                                    else if (itemID == 5009)
                                    {
                                        content = string.Format(LanguageManager.GetLang().St_HolidayFestivalGift, itemInfo.ItemName);
                                        chatService.SystemSendWhisper(userInfo, content);
                                    }
                                    else if (itemID == 5010)
                                    {
                                        content = string.Format(LanguageManager.GetLang().St_HolidayFestivalGift, itemInfo.ItemName);
                                        chatService.SystemSendWhisper(userInfo, content);
                                    }
                                    else if (itemID == 5011)
                                    {
                                        content = string.Format(LanguageManager.GetLang().St_HolidayFestivalGoinGift, userInfo.NickName);
                                        new TjxChatService().SystemSend(ChatType.World, content);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 通用活动奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="reward"></param>
        /// <param name="num"></param>
        public static string GameUserUniversalNocite(string userID, RewardType reward, int itemID, int num, string systemContent)
        {
            string content = string.Empty;
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (userInfo != null)
            {
                if (reward == RewardType.GameGoin)
                {
                    userInfo.GameCoin = MathUtils.Addition(userInfo.GameCoin, num, int.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, string.Format(LanguageManager.GetLang().St_GameCoin, num));
                }
                else if (reward == RewardType.Obtion)
                {
                    userInfo.ObtainNum = MathUtils.Addition(userInfo.ObtainNum, num, int.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, string.Format(LanguageManager.GetLang().St_ObtionNum, num));
                }
                else if (reward == RewardType.EnergyNum)
                {
                    userInfo.EnergyNum = MathUtils.Addition(userInfo.EnergyNum, (short)num, short.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, string.Format(LanguageManager.GetLang().St_EnergyNum, num));
                }
                else if (reward == RewardType.ExpNum)
                {
                    userInfo.ExpNum = MathUtils.Addition(userInfo.ExpNum, num, int.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, string.Format(LanguageManager.GetLang().St_ExpNum, num));
                }
                else if (reward == RewardType.Gold)
                {
                    userInfo.ItemGold = MathUtils.Addition(userInfo.ItemGold, num, int.MaxValue);
                    //userInfo.Update();
                    content = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, string.Format(LanguageManager.GetLang().St_GiftGoldNum, num));
                }
                else if (reward == RewardType.Experience)
                {
                    UserHelper.UserGeneralExp(userID, num);
                    content = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, string.Format(LanguageManager.GetLang().St_Experience, num));
                }
                else if (reward == RewardType.Item)
                {
                    UserItemHelper.AddUserItem(userID, itemID, num);
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
                    if (itemInfo != null)
                    {
                        string itemcontent = string.Format(LanguageManager.GetLang().St_Item, itemInfo.ItemName, 1, num);
                        content = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, itemcontent);
                    }
                }
                else if (reward == RewardType.CrystalType)
                {
                    CrystalQualityType qualityType = CrystalQualityType.PurPle;
                    content = UserUserUniversalCrystal(userInfo, qualityType, systemContent);
                    //GetUserCrystal(userInfo, qualityType);
                }
                else if (reward == RewardType.CrystalId)
                {
                    var packageCrystal = UserCrystalPackage.Get(userID);
                    UserCrystalInfo userCrystal = new UserCrystalInfo()
                    {
                        UserCrystalID = Guid.NewGuid().ToString(),
                        CrystalID = itemID,
                        CrystalLv = 1,
                        CurrExprience = 0,
                        GeneralID = 0,
                        IsSale = 2,
                        CreateDate = DateTime.Now
                    };
                    packageCrystal.SaveCrystal(userCrystal);
                    CrystalInfo info = new ConfigCacheSet<CrystalInfo>().FindKey(itemID);
                    if (info != null)
                    {
                        content = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, string.Format(LanguageManager.GetLang().St_Crystal, info.CrystalQuality, info.CrystalName));
                    }
                }
            }
            return content;
        }


        /// <summary>
        /// 奖励命运水晶
        /// </summary>
        public static string UserUserUniversalCrystal(GameUser user, CrystalQualityType qualityType, string systemContent)
        {
            string crystalContent = string.Empty;
            List<CrystalInfo> crystalArray2 = new ConfigCacheSet<CrystalInfo>().FindAll(m => m.CrystalQuality == qualityType);
            if (crystalArray2.Count > 0)
            {
                int randomNum = RandomUtils.GetRandom(0, crystalArray2.Count);
                CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(crystalArray2[randomNum].CrystalID);
                var packageCrystal = UserCrystalPackage.Get(user.UserID);
                UserCrystalInfo userCrystal = new UserCrystalInfo()
                {
                    UserCrystalID = Guid.NewGuid().ToString(),
                    CrystalID = crystal.CrystalID,
                    CrystalLv = 1,
                    CurrExprience = 0,
                    GeneralID = 0,
                    IsSale = 2,
                    CreateDate = DateTime.Now,
                };
                packageCrystal.SaveCrystal(userCrystal);

                crystalContent = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, systemContent, string.Format(LanguageManager.GetLang().St_Crystal, qualityType, crystal.CrystalName));
            }
            return crystalContent;
        }

    }
}