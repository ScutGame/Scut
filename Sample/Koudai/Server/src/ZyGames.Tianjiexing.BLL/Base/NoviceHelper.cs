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
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Common;


namespace ZyGames.Tianjiexing.BLL.Base
{

    /// <summary>
    /// 活动
    /// </summary>
    public class NoviceHelper
    {
        private static List<KeyValuePair<string, string>> _onlinePrizeConfig;
        private static string[] _loginPrizeConfig = ConfigEnvSet.GetString("Novice.LoginPrize").Split(',');

        static NoviceHelper()
        {
            _onlinePrizeConfig = ConfigSetConvert.ParseKeyValue(ConfigEnvSet.GetString("Novice.OnlinePrize"));
        }

        /// <summary>
        /// 活动是否开启
        /// </summary>
        /// <param name="festivalID"></param>
        /// <returns></returns>
        public static bool IsFestivalOpen(int festivalID)
        {
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                DateTime startDate = festival.StartDate;
                DateTime endDate = new DateTime();
                if (festival.EndDate != MathUtils.SqlMinDate)
                {
                    endDate = festival.EndDate;
                }
                else
                {
                    endDate = festival.StartDate.AddHours(festival.ContinuedTime);
                }
                if (festival.IsStop && startDate <= DateTime.Now && DateTime.Now < endDate)
                {
                    return true;
                }
            }
            return false;
        }

        private static void CheckFestivalRestrain(string userid, int festivalID)
        {
            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
            FestivalRestrain restrain = cacheSet.FindKey(userid, festivalID);
            if (restrain != null)
            {
                if (restrain.RefreashDate.Date != DateTime.Now.Date)
                {

                    if (restrain.RefreashDate.Date != DateTime.Now.Date)
                    {
                        restrain.RefreashDate = DateTime.Now;
                        restrain.RestrainNum = 0;
                        //restrain.Update();
                    }

                }
            }
            else
            {
                restrain = new FestivalRestrain()
                {
                    UserID = userid,
                    FestivalID = festivalID,
                    RestrainNum = 0,
                    RefreashDate = DateTime.Now
                };
                cacheSet.Add(restrain);
            }
        }

        /// <summary>
        /// 七夕节活动
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        public static void GetTanabataReward(string userID, int plotID)
        {
            int festivalID = 0; //1015;
            if (IsFestivalOpen(festivalID))
            {
                CheckFestivalRestrain(userID, festivalID);
                FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
                FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(userID, festivalID);
                if (restrain != null && restrain.RefreashDate.Date == DateTime.Now.Date && restrain.RestrainNum < festival.RestrainNum)
                {
                    if (TanabataFestival(userID, plotID))
                    {
                        if (TanabataFestival(userID, plotID))
                        {
                            restrain.RestrainNum = MathUtils.Addition(restrain.RestrainNum, 1, int.MaxValue);
                            //restrain.Update();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 七夕节活动
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        private static bool TanabataFestival(string userID, int plotID)
        {
            var chatService = new TjxChatService();
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
            if (plotInfo != null)
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
                        if (itemID == 5023 || itemID == 5024)
                        {
                            content = string.Format(LanguageManager.GetLang().St_Tanabata, itemInfo.ItemName);
                            chatService.SystemSendWhisper(userInfo, content);
                            return true;
                        }
                        else if (itemID == 5022)
                        {
                            content = string.Format(LanguageManager.GetLang().St_UserNameTanabata, userInfo.NickName, itemInfo.ItemName);
                            chatService.SystemSend(ChatType.World, content);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 七夕活动奖励
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GianTanabataReward(GameUser user, UserItemInfo item)
        {
            string content = string.Empty;
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
            if (itemInfo != null)
            {
                List<PrizeInfo> prizeInfosArray = itemInfo.ItemPack.ToList();

                double[] probability = new double[prizeInfosArray.Count];
                for (int i = 0; i < prizeInfosArray.Count; i++)
                {
                    probability[i] = (double)prizeInfosArray[i].Probability;
                }
                int index2 = RandomUtils.GetHitIndex(probability);
                PrizeInfo prizeInfo = prizeInfosArray[index2];
                if (prizeInfo.Type == RewardType.Item && prizeInfo.ItemList != null)
                {
                    content = PrizeItemList(user.UserID, prizeInfo.ItemList, prizeInfo.CrystalType.ToEnum<CrystalQualityType>());
                }
                else
                {
                    content = ActivitiesAward.GameUserRewardNocite(user.UserID, prizeInfo.Type, prizeInfo.ItemID, prizeInfo.Num, prizeInfo.CrystalType);
                }
                UserItemHelper.UseUserItem(user.UserID, item.ItemID, 1);
            }
            return content;
        }

        /// <summary>
        /// Festival活动
        /// </summary>
        /// <param name="contextUser"></param>
        public static void CheckFestival(GameUser contextUser)
        {
            //ActivitiesAward.FathersDay(contextUser);//父亲节奖励
            SevenDaysReceive(contextUser);
            //ActivitiesAward.DragonBoatFestival(contextUser.UserID, 1002); //端午节奖励
            //ActivitiesAward.DragonBoatFestival(contextUser.UserID, 1016); //七夕节奖励
            UserHelper.OpenMagic(contextUser.UserID, contextUser.UserLv);
            UserHelper.ChechDailyRestrain(contextUser.UserID);
            if (contextUser.UserExtend != null && contextUser.UserExtend.DailyInfo != null && contextUser.UserExtend.DailyInfo.MoreNum >= 5)
            {
                GianComradesPack(contextUser);
            }
            GianLoginReward(contextUser);
            AugustSecondWeek(contextUser);
            LoginDaysReceive(contextUser);
            //FestivalHelper.NewHandCardFestival(contextUser); //拉新卡活动
            FestivalHelper.ActivationNewCard(contextUser);
        }


        public static bool CheckOnlinePrize(string user)
        {
            NoviceRestrain noviceRestrain = new GameDataCacheSet<NoviceRestrain>().FindKey(user);
            if (noviceRestrain != null)
            {
                if (noviceRestrain.EnergyNum < _onlinePrizeConfig.Count)
                {
                    KeyValuePair<string, string> keyValuePair = _onlinePrizeConfig[noviceRestrain.EnergyNum];
                    int onlineMinutes = keyValuePair.Key.ToInt();
                    return (noviceRestrain.EnergyDate == DateTime.MinValue ||
                            DateTime.Now > noviceRestrain.EnergyDate.AddMinutes(onlineMinutes));
                }
                return false;
            }
            return true;
        }



        /// <summary>
        /// 在线奖励
        /// </summary>
        /// <param name="user"></param>
        /// <param name="takeNum"></param>
        /// <param name="coldeTime"></param>
        public static bool OnlinePrize(GameUser user, out int takeNum, out int coldeTime, out short onlineEnergy)
        {
            takeNum = 0;
            coldeTime = 0;
            onlineEnergy = 0;
            var cacheSet = new GameDataCacheSet<NoviceRestrain>();
            NoviceRestrain noviceRestrain = cacheSet.FindKey(user.UserID);
            if (noviceRestrain == null)
            {
                noviceRestrain = new NoviceRestrain { UserID = user.UserID };
                cacheSet.Add(noviceRestrain);
            }
            if (noviceRestrain.EnergyNum < _onlinePrizeConfig.Count)
            {
                takeNum = _onlinePrizeConfig.Count - noviceRestrain.EnergyNum;
                KeyValuePair<string, string> keyValuePair = _onlinePrizeConfig[noviceRestrain.EnergyNum];
                int onlineMinutes = keyValuePair.Key.ToInt();
                onlineEnergy = keyValuePair.Value.ToShort();
                if (noviceRestrain.EnergyDate == DateTime.MinValue ||
                    DateTime.Now > noviceRestrain.EnergyDate.AddMinutes(onlineMinutes))
                {
                    user.EnergyNum = MathUtils.Addition(user.EnergyNum, onlineEnergy, short.MaxValue);

                    noviceRestrain.EnergyNum += 1;
                    noviceRestrain.EnergyDate = DateTime.Now;
                    //if (new GameDataCacheSet<NoviceRestrain>().FindKey(user.UserID) == null)
                    //{
                    //    noviceRestrain.Append();
                    //}
                    //else
                    //{
                    //    noviceRestrain.Update();
                    //}
                    coldeTime = 0;
                    return true;
                }
                coldeTime = (int)Math.Floor((noviceRestrain.EnergyDate.AddMinutes(onlineMinutes) - DateTime.Now).TotalSeconds);
            }
            return false;
        }

        public static bool CheckLoginPrize(string user)
        {
            NoviceRestrain noviceRestrain = new GameDataCacheSet<NoviceRestrain>().FindKey(user);
            return noviceRestrain == null ||
                (noviceRestrain.GoldNum < _loginPrizeConfig.Length &&
                DateTime.Now.Date >= noviceRestrain.ReceiveDate.AddDays(1).Date);
        }

        /// <summary>
        /// 登录奖励
        /// </summary>
        /// <param name="user"></param>
        /// <param name="giftGold"></param>
        /// <returns></returns>
        public static bool LoginPrize(GameUser user, out int giftGold)
        {
            giftGold = 0;
            int giftCoin = ConfigEnvSet.GetInt("Novice.LoginPrizeGameCoin");
            NoviceRestrain noviceRestrain = new GameDataCacheSet<NoviceRestrain>().FindKey(user.UserID);
            if (noviceRestrain == null)
            {
                noviceRestrain = new NoviceRestrain { UserID = user.UserID };
            }
            if (noviceRestrain.GoldNum < _loginPrizeConfig.Length)
            {
                if (DateTime.Now.Date >= noviceRestrain.ReceiveDate.AddDays(1).Date)
                {
                    giftGold = _loginPrizeConfig[noviceRestrain.GoldNum].ToInt();
                    noviceRestrain.GoldNum += 1;
                    noviceRestrain.ReceiveDate = DateTime.Now;

                    if (DateTime.Now.Date >= noviceRestrain.ReceiveDate.Date)
                    {
                        user.GameCoin = MathUtils.Addition(user.GameCoin, giftCoin, int.MaxValue);
                        user.ExtGold = MathUtils.Addition(user.ExtGold, giftGold, int.MaxValue);
                        //user.Update();
                    }
                    //if (new GameDataCacheSet<NoviceRestrain>().FindKey(user.UserID) == null)
                    //{
                    //    noviceRestrain.Append();
                    //}
                    //else
                    //{
                    //    noviceRestrain.Update();
                    //}

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 新手礼包
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="noviceID"></param>
        /// <returns></returns>
        public static List<NoviceInfo> GetNoviceList(string userID, int noviceID)
        {
            List<NoviceInfo> noviceList = new List<NoviceInfo>();
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            NoviceRestrain restrain = new GameDataCacheSet<NoviceRestrain>().FindKey(userID);
            if (restrain != null && userInfo != null && restrain.EnergyNum == 0)
            {
                if (noviceID == 1000 && (DateTime.Now - userInfo.LoginTime).TotalSeconds >= 300)
                {
                    noviceList.Add(new NoviceInfo { ID = noviceID });
                }
            }
            else if (restrain != null && userInfo != null && restrain.EnergyNum == 1)
            {
                if (noviceID == 1001 && (DateTime.Now - userInfo.LoginTime).TotalSeconds >= 600)
                {
                    noviceList.Add(new NoviceInfo { ID = noviceID });
                }
            }
            else if (restrain != null && userInfo != null && restrain.EnergyNum == 2)
            {
                if (noviceID == 1002 && (DateTime.Now - userInfo.LoginTime).TotalSeconds >= 900)
                {
                    noviceList.Add(new NoviceInfo { ID = noviceID });
                }
            }
            else if (restrain != null && userInfo != null && restrain.EnergyNum == 3)
            {
                if (noviceID == 1003 && (DateTime.Now - userInfo.LoginTime).TotalSeconds >= 1800)
                {
                    noviceList.Add(new NoviceInfo { ID = noviceID });
                }
            }
            else if (restrain != null && userInfo != null && restrain.GoldNum == 0 && DateTime.Now.Date < userInfo.CreateDate.AddDays(4).Date)
            {
                if (noviceID == 1004 && (restrain.ReceiveDate.ToString() == ""))
                {
                    noviceList.Add(new NoviceInfo { ID = noviceID });
                }
            }
            else if (restrain != null && userInfo != null && restrain.GoldNum == 1 && DateTime.Now.Date < userInfo.CreateDate.AddDays(4).Date)
            {
                if (noviceID == 1004 && DateTime.Now.Date == restrain.ReceiveDate.AddDays(1).Date)
                {
                    noviceList.Add(new NoviceInfo { ID = noviceID });
                }
            }
            else if (restrain != null && userInfo != null && restrain.GoldNum == 2 && DateTime.Now.Date < userInfo.CreateDate.AddDays(4).Date)
            {
                if (noviceID == 1004 && DateTime.Now.Date == restrain.ReceiveDate.AddDays(1).Date)
                {
                    noviceList.Add(new NoviceInfo { ID = noviceID });
                }
            }
            return noviceList;
        }


        /// <summary>
        /// 每天奖励阅历和声望
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckDailyExpPrize(string user)
        {
            NoviceRestrain noviceRestrain = new GameDataCacheSet<NoviceRestrain>().FindKey(user);
            return false;
            //||(noviceRestrain.ExpObtain < _dailyExpPrizeConfig.Length &&
            //DateTime.Now.Date >= noviceRestrain.ExpObtainDate.AddDays(1).Date);
        }

        /// <summary>
        /// 每天奖励阅历和声望
        /// </summary>
        /// <param name="user"></param>
        /// <param name="expNum"></param>
        /// <returns></returns>
        public static bool DailyExpPrize(GameUser user, out int expNum)
        {
            expNum = 0;
            //NoviceRestrain noviceRestrain = new GameDataCacheSet<NoviceRestrain>().FindKey(user.UserID);
            //if (noviceRestrain == null)
            //{
            //    noviceRestrain = new NoviceRestrain { UserID = user.UserID };
            //}
            //if (noviceRestrain.ExpObtain < _dailyExpPrizeConfig.Length)
            //{
            //    if (DateTime.Now.Date >= noviceRestrain.ExpObtainDate.AddDays(1).Date)
            //    {
            //        expNum = _dailyExpPrizeConfig[noviceRestrain.ExpObtain].ToInt();
            //        user.ExpNum = user.ExpNum.Addition(expNum, int.MaxValue);
            //        user.ObtainNum = user.ObtainNum.Addition(expNum, int.MaxValue);
            //        noviceRestrain.ExpObtain += 1;
            //        noviceRestrain.ExpObtainDate = DateTime.Now;
            //        GameUser.DoSyncUserCache(user.UserID, () =>
            //        {
            //            user.Update();
            //            if (new GameDataCacheSet<NoviceRestrain>().FindKey(user.UserID) == null)
            //            {
            //                noviceRestrain.Append();
            //            }
            //            else
            //            {
            //                noviceRestrain.Update();
            //            }
            //        });
            //        return true;
            //    }
            //}
            return false;
        }

        /// <summary>
        /// 是否领取精力
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool DailyEnergy(string userID)
        {
            UserDailyRestrain userDailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (userDailyRestrain != null)
            {
                if (userDailyRestrain.Funtion15.Date != DateTime.Now.Date)
                {
                    DateTime dailyEnergyTime = ConfigEnvSet.GetString("DailyEnergyTime").ToDateTime(DateTime.MinValue);
                    if (DateTime.Now > dailyEnergyTime)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public static void SevenDaysReceive(GameUser user)
        {
            int festivalID = 0; //1007;
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                if (IsFestivalOpen(festivalID))
                {
                    var itemArray = UserItemHelper.GetItems(user.UserID).FindAll(m => m.ItemStatus == ItemStatus.BeiBao);
                    UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
                    List<PrizeInfo> rewardsArray = festival.Reward.ToList();
                    foreach (PrizeInfo reward in rewardsArray)
                    {
                        if (dailyRestrain != null && dailyRestrain.UserExtend != null && dailyRestrain.UserExtend.Funtion17.Date != reward.RefreshDate.Date && DateTime.Now.Date == reward.RefreshDate.Date)
                        {
                            if (itemArray.Count < user.GridNum)
                            {
                                ActivitiesAward.GameUserRewardNocite(user.UserID, reward.Type, reward.ItemID, reward.Num, reward.CrystalType);

                                if (itemArray.Count < user.GridNum)
                                {
                                    dailyRestrain.UserExtend.UpdateNotify(obj =>
                                        {
                                            dailyRestrain.UserExtend.Funtion17 = DateTime.Now;
                                            return true;
                                        });
                                    //dailyRestrain.Update();
                                }

                            }
                            break;
                        }
                    }
                }
            }
        }





        public static void SummerSecondNotice(int type)
        {
            string content = string.Empty;
            if (type == 1 || type == 2 || type == 3)
            {
                content = string.Format(LanguageManager.GetLang().St_SummerSecondNotice1);
            }
            else if (type == 4)
            {
                content = string.Format(LanguageManager.GetLang().St_SummerSecondNotice2);
            }
            else if (type == 5 || type == 6)
            {
                content = string.Format(LanguageManager.GetLang().St_SummerSecondNotice3);
            }
            new TjxChatService().SystemSend(ChatType.World, content);
        }

        public static bool IsCollect(GameUser user, TaskConfigInfo[] taskConfigInfosArray)
        {
            bool isNotEnough = true;
            int num = 0;
            foreach (TaskConfigInfo info in taskConfigInfosArray)
            {
                List<PrizeItemInfo> prizeItemList = new List<PrizeItemInfo>(user.UserExtend.ItemList);
                foreach (PrizeItemInfo itemInfo in prizeItemList)
                {
                    if (itemInfo.ItemID == info.item)
                    {
                        num = MathUtils.Addition(num, itemInfo.Num, int.MaxValue);
                    }
                }
                if (num < info.Num)
                {
                    isNotEnough = false;
                    break;
                }
            }
            return isNotEnough;
        }

        /// <summary>
        /// 物品掉落倍数活动
        /// </summary>
        /// <param name="sss"></param>
        public static decimal FestivalMultiple(decimal probability)
        {
            decimal multiple = probability;
            decimal multiplePro1 = 0;
            decimal multiplePro2 = 0;
            int festID = 0;//1008;
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festID);
            if (festival != null)
            {
                if (IsFestivalOpen(festID))
                {
                    if (festival.FestivalExtend != null)
                    {
                        multiplePro1 = probability * festival.FestivalExtend.Multiple;
                        multiplePro2 = probability * festival.FestivalExtend.EndMultiple;
                        double[] hitIndex = { (double)multiplePro1, (double)multiplePro2 };
                        int index = RandomUtils.GetHitIndex(hitIndex);
                        multiple = (decimal)hitIndex[index];
                    }
                }
            }
            return multiple;
        }

        /// <summary>
        /// 点击第五个命运水晶获得额外
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsGianCrystalPack(GameUser user)
        {
            int festID = 0;//1009;
            var chatService = new TjxChatService();
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festID);
            if (festival != null)
            {
                if (IsFestivalOpen(festID))
                {
                    int itemid = 5020;
                    var package = UserItemPackage.Get(user.UserID);
                    var userItemsArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemStatus == ItemStatus.BeiBao);
                    if (userItemsArray.Count < user.GridNum)
                    {
                        UserItemHelper.AddUserItem(user.UserID, itemid, 1);
                        chatService.SystemSendWhisper(user, LanguageManager.GetLang().St1305_GainCrystalBackpack);
                        return true;
                    }
                    return false;
                }
                return true;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CrystalNumFull(GameUser user, UserItemInfo item)
        {
            string content = string.Empty;
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
            if (itemInfo != null)
            {
                List<PrizeInfo> prizeInfosArray = itemInfo.ItemPack.ToList();
                double[] probability = new double[prizeInfosArray.Count];
                for (int i = 0; i < prizeInfosArray.Count; i++)
                {
                    probability[i] = (double)prizeInfosArray[i].Probability;
                }
                int index2 = RandomUtils.GetHitIndex(probability);
                PrizeInfo prizeInfo = prizeInfosArray[index2];
                content = ActivitiesAward.GameUserRewardNocite(user.UserID, prizeInfo.Type, prizeInfo.ItemID, prizeInfo.Num, prizeInfo.CrystalType);
                //ActivitiesAward.GameUserReward(user.UserID, prizeInfo.Type, prizeInfo.ItemID, prizeInfo.Num);
                UserItemHelper.UseUserItem(user.UserID, item.ItemID, 1);
            }
            return content;
        }

        /// <summary>
        /// 开启多人副本活动
        /// </summary>
        /// <param name="userID"></param>
        public static void GetFunctionEnum(string userID)
        {
            var cacheSet = new GameDataCacheSet<UserFunction>();
            UserFunction function = cacheSet.FindKey(userID, FunctionEnum.MorePlotCoin);
            if (function == null)
            {
                function = new UserFunction
                               {
                                   UserID = userID,
                                   FunEnum = FunctionEnum.MorePlotCoin,
                                   CreateDate = DateTime.Now
                               };
                cacheSet.Add(function);
            }
            function = cacheSet.FindKey(userID, FunctionEnum.MorePlotEnergy);
            if (function == null)
            {
                function = new UserFunction
                {
                    UserID = userID,
                    FunEnum = FunctionEnum.MorePlotEnergy,
                    CreateDate = DateTime.Now
                };

                cacheSet.Add(function);
            }
        }

        /// <summary>
        /// 暑期第四弹 --多人副本，尽享战友礼包！
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void GianComradesPack(GameUser user)
        {
            int festID = 0;// 1010;
            var chatService = new TjxChatService();
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festID);
            if (festival != null)
            {
                if (IsFestivalOpen(festID))
                {
                    List<PrizeInfo> rewardsArray = festival.Reward.ToList();

                    UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
                    if (dailyRestrain != null && dailyRestrain.UserExtend != null)
                    {
                        if (dailyRestrain.UserExtend.Comrades.Date != DateTime.Now.Date)
                        {
                            if (!UserHelper.IsBeiBaoFull(user))
                            {
                                foreach (PrizeInfo reward in rewardsArray)
                                {
                                    ItemBaseInfo iteminfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(reward.ItemID);
                                    if (iteminfo != null)
                                    {
                                        ActivitiesAward.GameUserReward(user.UserID, reward.Type, reward.ItemID, reward.Num);
                                        chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().St_SummerComradesItemNotice, iteminfo.ItemName));
                                    }
                                }
                                dailyRestrain.UserExtend.UpdateNotify(obj =>
                                {
                                    dailyRestrain.UserExtend.Comrades = DateTime.Now;
                                    return true;
                                });
                                //dailyRestrain.Update();
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 暑期第四弹 --多人副本战友礼包奖励！
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GianComradesPackReward(GameUser user, UserItemInfo item)
        {
            string content = string.Empty;
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
            if (itemInfo != null)
            {
                List<PrizeInfo> prizeInfosArray = itemInfo.ItemPack.ToList();
                List<PrizeInfo> prizeInfosList = new List<PrizeInfo>();
                if (user.UserLv >= 40)
                {
                    prizeInfosList = new List<PrizeInfo>(prizeInfosArray).FindAll(m => m.UserLv == 40);
                }
                else
                {
                    prizeInfosList = new List<PrizeInfo>(prizeInfosArray).FindAll(m => m.UserLv == 0);
                }

                double[] probability = new double[prizeInfosList.Count];
                for (int i = 0; i < prizeInfosList.Count; i++)
                {
                    probability[i] = (double)prizeInfosList[i].Probability;
                }
                int index2 = RandomUtils.GetHitIndex(probability);
                PrizeInfo prizeInfo = prizeInfosList[index2];
                if (prizeInfo.Type == RewardType.Item)
                {
                    content = PrizeItemList(user.UserID, prizeInfo.ItemList, prizeInfo.CrystalType.ToEnum<CrystalQualityType>());
                }
                else
                {
                    content = ActivitiesAward.GameUserRewardNocite(user.UserID, prizeInfo.Type, prizeInfo.ItemID, prizeInfo.Num, prizeInfo.CrystalType);
                }
                UserItemHelper.UseUserItem(user.UserID, item.ItemID, 1);
            }
            return content;
        }

        /// <summary>
        /// 某品质某等级的随机装备
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemList"></param>
        public static string PrizeItemList(string userID, ItemRandom itemList, CrystalQualityType crystalType)
        {
            string content = string.Empty;
            QualityType qualityType = (QualityType)Enum.Parse(typeof(QualityType), itemList.Quality.ToString());
            List<ItemBaseInfo> itemInfoArray = new ConfigCacheSet<ItemBaseInfo>().FindAll(m =>
                                                                  m.QualityType == qualityType &&
                                                                  m.DemandLv == itemList.ItemLv && m.ItemType == itemList.Type);

            int randomNum = RandomUtils.GetRandom(0, itemInfoArray.Count);
            ItemBaseInfo itemInfo = itemInfoArray[randomNum];
            if (itemInfo != null)
            {
                content = ActivitiesAward.GameUserRewardNocite(userID, RewardType.Item, itemInfo.ItemID, 1, crystalType);
            }
            return content;
        }


        /// <summary>
        /// 暑期第四弹 --登陆游戏获得奖励
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void GianLoginReward(GameUser user)
        {
            int festID = 0;// 1011;
            var chatService = new TjxChatService();
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festID);
            if (festival != null)
            {
                if (IsFestivalOpen(festID))
                {
                    string level = string.Empty;
                    List<PrizeInfo> rewardsList = new List<PrizeInfo>(festival.Reward).FindAll(m => m.UserLv <= user.UserLv);
                    int rid = 0;
                    int itemID = 0;
                    string content = string.Empty;

                    UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
                    if (dailyRestrain != null && dailyRestrain.UserExtend != null)
                    {
                        level = dailyRestrain.UserExtend.Leveling;
                    }
                    foreach (PrizeInfo reward in rewardsList)
                    {
                        rid = MathUtils.Addition(rid, 1, int.MaxValue);
                        if (!IsReceive(user.UserID, reward.UserLv.ToString(), rid))
                        {
                            if (reward.Type == RewardType.GameGoin)
                            {
                                content = content + string.Format(LanguageManager.GetLang().St_GameCoin, reward.Num) + "，";
                            }
                            if (reward.Type == RewardType.Obtion)
                            {
                                content = content + string.Format(LanguageManager.GetLang().St_ObtionNum + reward.Num) + "，";
                            }
                            if (reward.Type == RewardType.CrystalId)
                            {
                                if (UserHelper.IsCrystalBeiBaoFull(user))
                                {
                                    break;
                                }
                                CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(reward.ItemID);
                                if (crystal != null)
                                {
                                    content = content + crystal.CrystalName + "，";
                                }
                            }
                            else if (reward.Type == RewardType.Item)
                            {
                                if (UserHelper.IsBeiBaoFull(user))
                                {
                                    break;
                                }
                                itemID = reward.ItemID;
                                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
                                if (itemInfo != null)
                                {
                                    content = content + itemInfo.ItemName + "，";
                                }
                            }
                            ActivitiesAward.GameUserReward(user.UserID, reward.Type, reward.ItemID, reward.Num);
                            level = level + reward.UserLv + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(content))
                    {
                        chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().St_SummerLeveling, content));
                    }
                    if (dailyRestrain != null && !string.IsNullOrEmpty(level))
                    {
                        if (!string.IsNullOrEmpty(level))
                        {
                            if (dailyRestrain.UserExtend == null)
                            {
                                dailyRestrain.UserExtend = new DailyUserExtend();
                            }
                            dailyRestrain.UserExtend.UpdateNotify(obj =>
                                {
                                    dailyRestrain.UserExtend.Leveling = level;
                                    return true;
                                });
                            //dailyRestrain.Update();
                        }
                    }
                }
            }
        }

        private static bool IsReceive(string userID, string receiveID, int rid)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            string[] leveling = new string[0];
            if (dailyRestrain != null && dailyRestrain.UserExtend != null)
            {
                if (!string.IsNullOrEmpty(dailyRestrain.UserExtend.Leveling))
                {
                    int lid = 0;
                    leveling = dailyRestrain.UserExtend.Leveling.Split(',');
                    foreach (string s in leveling)
                    {
                        lid = MathUtils.Addition(lid, 1, int.MaxValue);
                        if (lid == rid && s == receiveID)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 八月第二周活动
        /// </summary>
        /// <param name="user"></param>
        public static void AugustSecondWeek(GameUser user)
        {
            int festivalID = 0;// 1014;
            int itemID = 0;
            var chatService = new TjxChatService();
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                if (IsFestivalOpen(festivalID))
                {
                    string systemContent = string.Empty;
                    FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(user.UserID, festivalID);
                    List<PrizeInfo> prizeInfosArray = festival.Reward.ToList();
                    foreach (PrizeInfo reward in prizeInfosArray)
                    {
                        if (DateTime.Now.Date == reward.RefreshDate.Date)
                        {
                            if (reward.Type == RewardType.Item && UserHelper.IsBeiBaoFull(user)) break;
                            if (reward.Type == RewardType.CrystalId && CrystalHelper.IsCrystalNumFull(user)) break;
                            if (reward.Type == RewardType.CrystalId)
                            {
                                itemID = reward.ItemID;
                            }
                            else
                            {
                                itemID = reward.ItemID;
                            }
                            if (restrain != null && restrain.RefreashDate.Date != DateTime.Now.Date)
                            {
                                if (restrain.RefreashDate.Date != DateTime.Now.Date)
                                {
                                    restrain.RestrainNum = 1;
                                    restrain.RefreashDate = DateTime.Now;
                                    //restrain.Update();
                                }

                                systemContent = ActivitiesAward.GameUserUniversalNocite(user.UserID, reward.Type, itemID, reward.Num, LanguageManager.GetLang().St_AugustSecondWeek);
                                break;
                            }
                            else if (restrain == null)
                            {
                                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                                restrain = new FestivalRestrain()
                                        {
                                            UserID = user.UserID,
                                            FestivalID = 1014,
                                            RestrainNum = 1,
                                            RefreashDate = DateTime.Now,
                                        };
                                cacheSet.Add(restrain);

                                systemContent = ActivitiesAward.GameUserUniversalNocite(user.UserID, reward.Type, itemID, reward.Num, LanguageManager.GetLang().St_AugustSecondWeek);
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(systemContent))
                    {
                        chatService.SystemSendWhisper(user, systemContent);
                    }
                }
            }
        }

        /// <summary>
        /// 八月第二周活动 是否领完或开启
        /// </summary>
        /// <param name="user"></param>
        public static bool IsAugustSecondWeekEnergy(GameUser user)
        {
            int festivalID = 0;// 1013;
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                if (IsFestivalOpen(festivalID))
                {
                    FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(user.UserID, festivalID);
                    if (restrain != null)
                    {
                        if (restrain.RefreashDate.Date != DateTime.Now.Date)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 八月第二周活动
        /// </summary>
        /// <param name="user"></param>
        public static int AugustSecondWeekEnergy(GameUser user)
        {
            int energyNum = 0;
            if (IsAugustSecondWeekEnergy(user))
            {
                int festivalID = 0;// 1013;
                FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
                if (festival != null)
                {
                    short userLv = 0;
                    if (user.UserLv >= 40)
                    {
                        userLv = 40;
                    }
                    FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(user.UserID, festivalID);
                    PrizeInfo prizeInfo = new List<PrizeInfo>(festival.Reward).Find(m => m.UserLv == userLv);
                    if (prizeInfo != null)
                    {
                        energyNum = prizeInfo.Num;

                        user.EnergyNum = MathUtils.Addition(user.EnergyNum, (short)prizeInfo.Num, short.MaxValue);
                        //user.Update();

                        if (restrain != null)
                        {

                            restrain.RestrainNum = user.UserLv;
                            restrain.RefreashDate = DateTime.Now;
                            //restrain.Update();

                        }
                        else
                        {

                            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                            restrain = new FestivalRestrain()
                            {
                                UserID = user.UserID,
                                FestivalID = festivalID,
                                RestrainNum = user.UserLv,
                                RefreashDate = DateTime.Now,
                            };

                            cacheSet.Add(restrain);

                        }
                    }
                }
            }
            return energyNum;
        }

        /// <summary>
        /// 限时活动列表
        /// </summary>
        /// <returns></returns>
        public static List<FestivalInfo> LimitFestivalList()
        {
            List<FestivalInfo> festivalList = new List<FestivalInfo>();
            var festivalInfosArray = new ShareCacheStruct<FestivalInfo>().FindAll(m => m.FestivalID != 1012);
            foreach (FestivalInfo info in festivalInfosArray)
            {
                if (IsFestivalOpen(info.FestivalID))
                {
                    if (info.FestivalType != FestivalType.RetailLogin
                        || (info.FestivalType == FestivalType.RetailLogin && info.FestivalExtend != null && info.FestivalExtend.IsDisplay))
                    {
                        festivalList.Add(info);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return festivalList;
        }

        /// <summary>
        /// 七夕充值礼包
        /// </summary>
        /// <returns></returns>
        public static string TanabataRecharge(GameUser user, UserItemInfo item)
        {
            string content = string.Empty;
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
            if (itemInfo != null)
            {
                List<PrizeInfo> prizeInfosArray = itemInfo.ItemPack.ToList();
                double[] probability = new double[prizeInfosArray.Count];
                for (int i = 0; i < prizeInfosArray.Count; i++)
                {
                    probability[i] = (double)prizeInfosArray[i].Probability;
                }
                int index2 = RandomUtils.GetHitIndex(probability);
                PrizeInfo prizeInfo = prizeInfosArray[index2];
                content = ActivitiesAward.GameUserRewardNocite(user.UserID, prizeInfo.Type, prizeInfo.ItemID,
                                                               prizeInfo.Num, prizeInfo.CrystalType);
                UserItemHelper.UseUserItem(user.UserID, item.ItemID, 1);
            }
            return content;
        }

        /// <summary>
        /// 是否开启七夕翅膀活动
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool IsWingFestivalInfo(string userID)
        {
            int festivalID = 0;// 1017;
            FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(userID, festivalID);
            if (restrain != null && IsFestivalOpen(festivalID))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 触发七夕翅膀活动功能
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="chatContent"></param>
        public static void WingFestival(string userid, string chatContent)
        {
            int festivalID = 0;// 1017;
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                if (festival.FestivalExtend != null)
                {
                    if (chatContent.IndexOf(festival.FestivalExtend.KeyWord) != -1)
                    {
                        FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(userid);
                        if (restrain == null)
                        {
                            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                            restrain = new FestivalRestrain()
                            {
                                UserID = userid,
                                FestivalID = festivalID,
                                RefreashDate = DateTime.Now,
                                RestrainNum = 1
                            };
                            cacheSet.Add(restrain);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 中元节变身活动功能
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="chatContent"></param>
        public static void WingZhongYuanFestival(GameUser user, string chatContent)
        {
            int festivalID = 0;// 1019;
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null && festival.FestivalExtend != null)
            {
                if (chatContent.IndexOf(festival.FestivalExtend.KeyWord) != -1)
                {
                    FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(user.UserID);
                    if (restrain == null)
                    {
                        List<PrizeInfo> prizeInfosArray = festival.Reward.ToList();
                        double[] probability = new double[prizeInfosArray.Count];
                        for (int i = 0; i < prizeInfosArray.Count; i++)
                        {
                            probability[i] = (double)prizeInfosArray[i].Probability;
                        }
                        int index2 = RandomUtils.GetHitIndex(probability);
                        PrizeInfo prizeInfo = prizeInfosArray[index2];
                        if (prizeInfo != null)
                        {
                            var cacheSet = new GameDataCacheSet<UserProps>();
                            UserProps props = cacheSet.FindKey(user.UserID, prizeInfo.ItemID);
                            if (props != null && !IsFestivalOpen(festivalID))
                            {
                                cacheSet.Delete(props);
                            }
                            else if (props == null && IsFestivalOpen(festivalID))
                            {
                                props = new UserProps(user.UserID, prizeInfo.ItemID)
                                            {
                                                ChangeTime = festival.StartDate,
                                                SurplusNum = 0
                                            };

                                cacheSet.Add(props);

                                restrain = new FestivalRestrain()
                                {
                                    UserID = user.UserID,
                                    FestivalID = festivalID,
                                    RefreashDate = DateTime.Now,
                                    RestrainNum = 1
                                };
                                new GameDataCacheSet<FestivalRestrain>().Add(restrain);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 竞技场竞技活动
        /// </summary>
        /// <param name="user"></param>
        /// <param name="winNum"></param>
        /// <param name="victoryNum"></param>
        public static void SportCombatFestival(GameUser user, int winNum, int victoryNum)
        {
            //处理活动
            int festivalID = 0;// 1005;
            if (IsFestivalOpen(festivalID))
            {
                if (winNum == 3 || winNum == 5 || winNum == 7 || winNum == 9 || winNum == 12 || winNum == 20)
                {
                    UserHelper.WinsReward(user, victoryNum);
                }
            }
        }

        /// <summary>
        /// 竞技场连胜活动
        /// </summary>
        public static void SportVictoryNum(GameUser user, int festivalID, int victoryNum)
        {
            if (IsFestivalOpen(festivalID))
            {
                var chatService = new TjxChatService();
                FestivalStop(user.UserID, festivalID);
                FestivalRestrain festivalRestrain = new GameDataCacheSet<FestivalRestrain>().FindKey(user.UserID, festivalID);
                int restrainNm = 0;
                if (festivalRestrain != null)
                {
                    restrainNm = festivalRestrain.RestrainNum;
                }
                FestivalInfo festivalInfo = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
                if (festivalInfo != null && festivalInfo.Reward.Count > 0)
                {
                    List<PrizeInfo> prizeInfosList = new List<PrizeInfo>(festivalInfo.Reward);
                    List<PrizeInfo> prizeInfosArray =
                        prizeInfosList.FindAll(m => m.WinNum <= victoryNum && m.WinNum > restrainNm);
                    if (prizeInfosArray.Count > 0)
                    {
                        foreach (PrizeInfo info in prizeInfosArray)
                        {
                            chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().SportVictoryReward, user.NickName, info.WinNum, SportVictoryReward(user, info)));
                        }
                        if (festivalRestrain == null)
                        {
                            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                            festivalRestrain = new FestivalRestrain()
                                                   {
                                                       UserID = user.UserID,
                                                       FestivalID = festivalID,
                                                       RefreashDate = DateTime.Now,
                                                       RestrainNum = victoryNum
                                                   };
                            cacheSet.Add(festivalRestrain);
                        }
                        else
                        {
                            festivalRestrain.RestrainNum = victoryNum;
                            festivalRestrain.RefreashDate = DateTime.Now;
                            //festivalRestrain.Update();

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 端午节通关获得物品
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        public static void GetFestivalReward(string userID, int plotID)
        {
            int festivalID = 0;// 1001;
            var chatService = new TjxChatService();
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
                if (plotInfo != null)
                {
                    if (IsFestivalOpen(festivalID))
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
                                if (itemID == 5005)
                                {
                                    content = string.Format(LanguageManager.GetLang().St_DragonBoatPuTongZongzi, itemInfo.ItemName);
                                    chatService.SystemSendWhisper(userInfo, content);
                                }
                                else if (itemID == 5006)
                                {
                                    content = string.Format(LanguageManager.GetLang().St_DragonBoatPuTongZongzi, itemInfo.ItemName);
                                    chatService.SystemSendWhisper(userInfo, content);
                                }
                                else if (itemID == 5007)
                                {
                                    content = string.Format(LanguageManager.GetLang().St_DragonBoatZongzi, userInfo.NickName);
                                    chatService.SystemSend(ChatType.World, content);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 竞技场奖励
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="prizeInfo"></param>
        public static string SportVictoryReward(GameUser userInfo, PrizeInfo prizeInfo)
        {
            string content = string.Empty;
            if (prizeInfo.Type == RewardType.GameGoin)
            {
                userInfo.GameCoin = MathUtils.Addition(userInfo.GameCoin, prizeInfo.Num, int.MaxValue);
                //userInfo.Update();
                content = string.Format(LanguageManager.GetLang().St_GameCoin, prizeInfo.Num) + "，";
            }
            else if (prizeInfo.Type == RewardType.Obtion)
            {
                userInfo.ObtainNum = MathUtils.Addition(userInfo.ObtainNum, prizeInfo.Num, int.MaxValue);
                //userInfo.Update();
                content = content + string.Format(LanguageManager.GetLang().St_ObtionNum, prizeInfo.Num) + "，";
            }
            else if (prizeInfo.Type == RewardType.EnergyNum)
            {
                userInfo.EnergyNum = MathUtils.Addition(userInfo.EnergyNum, (short)prizeInfo.Num, short.MaxValue);
                //userInfo.Update();
                content = content + string.Format(LanguageManager.GetLang().St_EnergyNum, prizeInfo.Num) + "，";
            }
            else if (prizeInfo.Type == RewardType.ExpNum)
            {
                userInfo.ExpNum = MathUtils.Addition(userInfo.ExpNum, prizeInfo.Num, int.MaxValue);
                //userInfo.Update();
                content = content + string.Format(LanguageManager.GetLang().St_ExpNum, prizeInfo.Num) + "，";
            }
            else if (prizeInfo.Type == RewardType.Gold)
            {
                userInfo.ItemGold = MathUtils.Addition(userInfo.ItemGold, prizeInfo.Num, int.MaxValue);
                //userInfo.Update();
                content = content + string.Format(LanguageManager.GetLang().St_GiftGoldNum, prizeInfo.Num) + "，";
            }
            else if (prizeInfo.Type == RewardType.Experience)
            {
                UserHelper.UserGeneralExp(userInfo.UserID, prizeInfo.Num);
                content = content + string.Format(LanguageManager.GetLang().St_Experience, prizeInfo.Num) + "，";
            }
            else if (prizeInfo.Type == RewardType.Item)
            {
                UserItemHelper.AddUserItem(userInfo.UserID, prizeInfo.ItemID, prizeInfo.Num);
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prizeInfo.ItemID);
                if (itemInfo != null)
                {
                    content = content + string.Format(LanguageManager.GetLang().St_Item, itemInfo.ItemName,prizeInfo.UserLv, prizeInfo.Num) + "，";
                }
            }
            else if (prizeInfo.Type == RewardType.CrystalType)
            {
                List<CrystalInfo> crystalArray2 = new ConfigCacheSet<CrystalInfo>().FindAll(m => m.CrystalQuality == prizeInfo.CrystalType);
                if (crystalArray2.Count > 0)
                {
                    int randomNum = RandomUtils.GetRandom(0, crystalArray2.Count);
                    CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(crystalArray2[randomNum].CrystalID);
                    if (crystal != null)
                    {
                        UserHelper.CrystalAppend(userInfo.UserID, crystal.CrystalID, 2);
                        content = content + string.Format(LanguageManager.GetLang().St_Crystal, CrystalHelper.GetQualityName(prizeInfo.CrystalType), crystal.CrystalName) + "，";
                    }
                }
            }
            else if (prizeInfo.Type == RewardType.CrystalId)
            {
                UserHelper.CrystalAppend(userInfo.UserID, prizeInfo.ItemID, 2);
                CrystalInfo info = new ConfigCacheSet<CrystalInfo>().FindKey(prizeInfo.ItemID);
                if (info != null)
                {
                    content = content + string.Format(LanguageManager.GetLang().St_Crystal, CrystalHelper.GetQualityName(info.CrystalQuality), info.CrystalName) + "，";
                }
            }
            return content.Substring(0, content.Length - 1);
        }

        /// <summary>
        /// 是否本次活动时间
        /// </summary>
        /// <param name="festivalID"></param>
        /// <returns></returns>
        public static void FestivalStop(string userID, int festivalID)
        {
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                DateTime startDate = festival.StartDate;
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain festivalRestrain = cacheSet.FindKey(userID, festivalID);
                if (festivalRestrain != null && festivalRestrain.RefreashDate < startDate)
                {
                    cacheSet.Delete(festivalRestrain);
                }
            }
        }

        /// <summary>
        /// 当天活动时间次数
        /// </summary>
        /// <param name="festivalID"></param>
        /// <returns></returns>
        public static void FestivalRestrainNum(string userID, int festivalID)
        {
            FestivalInfo festival = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festival != null)
            {
                FestivalRestrain festivalRestrain = new GameDataCacheSet<FestivalRestrain>().FindKey(userID, festivalID);
                if (festivalRestrain != null && festivalRestrain.RefreashDate.Date != DateTime.Now.Date)
                {
                    if (festivalRestrain.UserExtend != null && festivalRestrain.UserExtend.Count > 0)
                    {
                        festivalRestrain.UserExtend.Clear();
                    }
                    festivalRestrain.RestrainNum = 0;
                    festivalRestrain.RefreashDate = DateTime.Now;
                }
            }
        }



        /// <summary>
        /// 登录领好礼类型活动
        /// </summary>
        /// <param name="user"></param>
        public static void LoginDaysReceive(GameUser user)
        {
            var festivalInfosArray = new ShareCacheStruct<FestivalInfo>().FindAll(m => m.FestivalType == FestivalType.LoginReceive);
            foreach (FestivalInfo info in festivalInfosArray)
            {
                if (IsFestivalOpen(info.FestivalID))
                {
                    var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                    FestivalRestrain fRest = cacheSet.FindKey(user.UserID, info.FestivalID);
                    if (fRest == null || fRest.RefreashDate < info.StartDate ||
                        (fRest.RefreashDate.Date != DateTime.Now.Date && fRest.RefreashDate >= info.StartDate && fRest.RefreashDate <= info.EndDate))
                    {
                        List<PrizeInfo> prizeList = PrizeHelper.GetPrizeInfo(info.Reward).ToList();
                        foreach (PrizeInfo prize in prizeList)
                        {
                            if (prize.RefreshDate.Date == DateTime.Now.Date)
                            {
                                UserTakePrize userTask = PrizeHelper.GetUserTake(prize, user.UserID, info);
                                new ShareCacheStruct<UserTakePrize>().Add(userTask);
                            }
                        }
                        if (fRest == null)
                        {
                            fRest = new FestivalRestrain();
                            fRest.FestivalID = info.FestivalID;
                            fRest.RefreashDate = DateTime.Now;
                            fRest.RestrainNum = 1;
                            fRest.UserID = user.UserID;
                            cacheSet.Add(fRest);
                        }
                        else
                        {
                            fRest.RestrainNum = 1;
                            fRest.RefreashDate = DateTime.Now;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 普通副本类型活动(获得遗失的月饼配方)
        /// </summary>
        /// <param name="user"></param>
        public static void NormalCountReceive(GameUser user)
        {
            int dailyNum = 0;
            var chatService = new TjxChatService();
            var festivalInfosArray = new ShareCacheStruct<FestivalInfo>().FindAll(m => m.FestivalType == FestivalType.NormalCount);
            foreach (FestivalInfo info in festivalInfosArray)
            {
                if (IsFestivalOpen(info.FestivalID))
                {
                    FestivalRestrainNum(user.UserID, info.FestivalID);
                    FestivalRestrain festivalRestrain = new GameDataCacheSet<FestivalRestrain>().FindKey(user.UserID, info.FestivalID);
                    if (festivalRestrain != null && festivalRestrain.RefreashDate.Date == DateTime.Now.Date)
                    {
                        dailyNum = festivalRestrain.RestrainNum;
                    }
                    if (info.RestrainNum > dailyNum && info.TaskConfig != null)
                    {
                        List<TaskConfigInfo> configInfos = info.TaskConfig.ToList();
                        foreach (TaskConfigInfo configInfo in configInfos)
                        {
                            if (RandomUtils.IsHit(configInfo.probability))
                            {
                                UserItemHelper.AddUserItem(user.UserID, configInfo.item, configInfo.Num);
                                if (info.FestivalExtend != null)
                                {
                                    chatService.SystemSend(ChatType.World, string.Format(info.FestivalExtend.Desc, user.NickName));
                                }
                                if (festivalRestrain != null)
                                {
                                    festivalRestrain.RestrainNum = MathUtils.Addition(festivalRestrain.RestrainNum, 1);
                                    //festivalRestrain.Update();
                                }
                                else
                                {
                                    var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                                    festivalRestrain = new FestivalRestrain()
                                    {
                                        FestivalID = info.FestivalID,
                                        UserID = user.UserID,
                                        RestrainNum = 1,
                                        RefreashDate = DateTime.Now
                                    };
                                    cacheSet.Add(festivalRestrain);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 副本通关活动掉落物品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plotID"></param>
        public static void PlotFestivalList(GameUser user, int plotID)
        {
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
            GetTanabataReward(user.UserID, plotID);
            GetFestivalReward(user.UserID, plotID);
            ActivitiesAward.GetHolidayFestivalReward(user.UserID, plotID); //假日狂欢活动通关获得金币
            if (plotInfo != null)
            {
                if (plotInfo.PlotType == PlotType.Elite)
                {
                    //ElitePlotFestivalList(user);
                }
                else if (plotInfo.PlotType == PlotType.Normal)
                {
                    NormalCountReceive(user);
                }

            }
        }

        /// <summary>
        /// 通关精英副本获得奖励
        /// </summary>
        /// <param name="user"></param>
        public static void ElitePlotFestivalList(GameUser user)
        {
            var festivalInfosArray = new ShareCacheStruct<FestivalInfo>().FindAll(m => m.FestivalType == FestivalType.EliteCount);
            var chatService = new TjxChatService();
            foreach (FestivalInfo info in festivalInfosArray)
            {
                if (IsFestivalOpen(info.FestivalID))
                {
                    if (info.TaskConfig != null)
                    {
                        FestivalRestrainNum(user.UserID, info.FestivalID);
                        int maxNum = 0;
                        var configInfos = info.TaskConfig;
                        foreach (TaskConfigInfo configInfo in configInfos)
                        {
                            maxNum = MathUtils.Addition(maxNum, 1);
                            var plotInfosArray = new ConfigCacheSet<PlotInfo>().FindAll(m => configInfo.CityID.IndexOf(m.CityID.ToString()) >= 0 && m.PlotType == configInfo.PlotType);
                            if (IsEliteClearance(user.UserID, info.FestivalID, plotInfosArray, maxNum))
                            {
                                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(configInfo.item);
                                if (itemInfo != null)
                                {
                                    UserItemHelper.AddUserItem(user.UserID, configInfo.item, configInfo.Num);
                                    GetFestivalRestrain(info, user, maxNum, info.FestivalID);
                                    chatService.SystemSend(ChatType.World, string.Format(configInfo.Desc, user.NickName, itemInfo.ItemName));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 活动限制次数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="user"></param>
        /// <param name="maxNum"></param>
        /// <param name="festivalRestrain"></param>
        private static void GetFestivalRestrain(FestivalInfo info, GameUser user, int maxNum, int festivalID)
        {
            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
            FestivalRestrain festivalRestrain = cacheSet.FindKey(user.UserID, festivalID);
            if (festivalRestrain == null)
            {
                festivalRestrain = new FestivalRestrain()
                {
                    FestivalID = info.FestivalID,
                    UserID = user.UserID,
                    RestrainNum = 1,
                    RefreashDate = DateTime.Now
                };
                cacheSet.Add(festivalRestrain);
                festivalRestrain = cacheSet.FindKey(user.UserID, festivalID);
            }

            RestrainExtend extend = new RestrainExtend();
            extend.ID = maxNum;
            extend.Num = 1;
            if (festivalRestrain.UserExtend == null)
            {
                festivalRestrain.UserExtend = new CacheList<RestrainExtend>();
            }
            festivalRestrain.UserExtend.Add(extend);
            festivalRestrain.RestrainNum = MathUtils.Addition(festivalRestrain.RestrainNum, 1);
            //festivalRestrain.Update();

        }

        /// <summary>
        /// 是否满足通关精英副本
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="festivalID"></param>
        /// <param name="plotInfosArray"></param>
        /// <returns></returns>
        public static bool IsEliteClearance(string userID, int festivalID, List<PlotInfo> plotInfosArray, int number)
        {
            int plotNum = 0;
            FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(userID, festivalID);
            if (restrain != null && restrain.RefreashDate.Date == DateTime.Now.Date && restrain.UserExtend != null && restrain.UserExtend.Count > 0)
            {
                RestrainExtend[] extendList = restrain.UserExtend.FindAll(m => m.ID == number).ToArray();
                plotNum = extendList.Length > 0 ? extendList.Length : 0;
            }
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.RefreshDate.Date == DateTime.Now.Date && dailyRestrain.FunPlot != null && dailyRestrain.FunPlot.Count > 0)
            {
                List<FunPlot> funPlotsList = dailyRestrain.FunPlot.ToList();
                foreach (PlotInfo info in plotInfosArray)
                {
                    FunPlot plot = funPlotsList.Find(m => m.PlotID == info.PlotID);
                    if (plot != null && plot.Num > plotNum)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 渠道登录领好礼类型活动
        /// </summary>
        /// <param name="user"></param>
        public static void RetailLoginDaysReceive(GameUser user)
        {
            var chatService = new TjxChatService();
            var festivalInfosArray = new ShareCacheStruct<FestivalInfo>().FindAll(m => m.FestivalType == FestivalType.RetailLogin);
            foreach (FestivalInfo info in festivalInfosArray)
            {
                bool isChange = false;
                if (IsFestivalOpen(info.FestivalID))
                {
                    FestivalRestrain restrain = new GameDataCacheSet<FestivalRestrain>().FindKey(user.UserID, info.FestivalID);
                    var rewardsArray = info.Reward;
                    foreach (PrizeInfo reward in rewardsArray)
                    {
                        if (reward.RetailID == user.RetailID && restrain == null)
                        {
                            ActivitiesAward.GameUserRewardNocite(user.UserID, reward.Type, reward.ItemID, reward.Num, reward.CrystalType);
                            isChange = true;
                        }
                    }
                    if (isChange && info.TaskConfig.Count > 0)
                    {
                        foreach (TaskConfigInfo configInfo in info.TaskConfig)
                        {
                            chatService.SystemSendWhisper(user, configInfo.Desc);
                            if (restrain == null)
                            {
                                restrain = new FestivalRestrain()
                                {
                                    UserID = user.UserID,
                                    FestivalID = info.FestivalID,
                                    RefreashDate = DateTime.Now,
                                    RestrainNum = 1,
                                };
                                new GameDataCacheSet<FestivalRestrain>().Add(restrain);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 老友礼包
        /// </summary>
        /// <param name="user"></param>
        public static void OldFriendPack(string userID, DateTime loginTime)
        {
            int loginDate = ConfigEnvSet.GetInt("LoginIntervalDate") > 0 ? ConfigEnvSet.GetInt("LoginIntervalDate") : 10;
            if ((DateTime.Now - loginTime).TotalDays >= loginDate && loginTime > MathUtils.SqlMinDate)
            {
                UserTakePrize userPrize = new UserTakePrize();
                userPrize.ID = Guid.NewGuid().ToString();
                userPrize.UserID = Convert.ToInt32(userID);
                userPrize.ObtainNum = 0;
                userPrize.EnergyNum = 0;
                userPrize.GameCoin = 0;
                userPrize.Gold = 0;
                userPrize.ExpNum = 0;
                userPrize.VipLv = 0;
                userPrize.GainBlessing = 0;
                userPrize.ItemPackage = "5025=1=1";
                userPrize.SparePackage = string.Empty;
                userPrize.CrystalPackage = string.Empty;
                userPrize.EnchantPackage = string.Empty;
                userPrize.MailContent = LanguageManager.GetLang().OldFriendPack;
                userPrize.IsTasked = false;
                userPrize.TaskDate = MathUtils.SqlMinDate;
                userPrize.OpUserID = 10000;
                userPrize.CreateDate = DateTime.Now;
                new ShareCacheStruct<UserTakePrize>().Add(userPrize);
            }
        }
    }
}