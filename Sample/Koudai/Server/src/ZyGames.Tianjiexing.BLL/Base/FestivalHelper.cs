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

using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.DataModel;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 节日活动
    /// </summary>
    public class FestivalHelper
    {
        private const string strUserID = "0";
        /// <summary>
        /// 获取当前所有活动
        /// </summary>
        /// <returns></returns>
        public static List<FestivalInfo> GetInfo()
        {
            List<FestivalInfo> festList = new List<FestivalInfo>();
            var list = new ShareCacheStruct<FestivalInfo>().FindAll();
            foreach (FestivalInfo info in list)
            {
                if (info.StartDate < DateTime.Now)
                    continue;
                if (info.StartDate.AddHours(info.ContinuedTime) < DateTime.Now)
                    continue;
                if (info.EndDate < DateTime.Now)
                    continue;
                festList.Add(info);
            }
            return festList;
        }

        /// <summary>
        /// 获取当前所有活动
        /// </summary>
        /// <param name="type">活动类型</param>
        /// <returns></returns>
        public static FestivalInfo GetInfo(FestivalType type)
        {
            var list = new ShareCacheStruct<FestivalInfo>().FindAll();
            foreach (FestivalInfo info in list)
            {
                if (!info.IsStop)
                    continue;
                if (info.StartDate > DateTime.Now)
                    continue;
                //if (info.StartDate.AddHours(info.ContinuedTime) < DateTime.Now)
                //    continue;
                if (info.EndDate < DateTime.Now)
                    continue;
                if (info.FestivalType == type)
                    return info;
            }
            return null;
        }

        /// <summary>
        /// 处理活动
        /// </summary>
        /// <param name="user"></param>
        public static void DoFestival(GameUser user)
        {
            var list = new ShareCacheStruct<FestivalInfo>().FindAll();
            foreach (FestivalInfo info in list)
            {
                if (!info.IsStop)
                    continue;
                if (info.StartDate > DateTime.Now)
                    continue;
                //if (info.StartDate.AddHours(info.ContinuedTime) < DateTime.Now)
                //    continue;
                if (info.EndDate < DateTime.Now)
                    continue;
                switch (info.FestivalType)
                {
                    case FestivalType.PayCount:
                        FestivalPayCount(info, user);
                        break;
                    case FestivalType.VIPFestival:
                        GetVipFestival(user.UserID);
                        break;
                    case FestivalType.LoginGiveGift:
                        GetLoginGainReward(user.UserID, FestivalType.LoginGiveGift);
                        break;
                    case FestivalType.Default:
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 累计充值活动
        /// </summary>
        public static void FestivalPayCount(FestivalInfo info, GameUser user)
        {
            UserRecharge recharge = new GameDataCacheSet<UserRecharge>().FindKey(user.UserID);
            if (recharge == null)
                return;
            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
            FestivalRestrain fRest = cacheSet.FindKey(user.UserID, info.FestivalID);
            if (fRest != null && fRest.RestrainNum >= 1 && fRest.RefreashDate == info.StartDate)
                return;
            if (recharge.FestivalDate >= info.StartDate
                && recharge.FestivalCount >= info.RestrainNum)
            {
                CacheList<PrizeInfo> prizeList = PrizeHelper.GetPrizeInfo(info.Reward);
                foreach (PrizeInfo prize in prizeList)
                {
                    UserTakePrize userTask = PrizeHelper.GetUserTake(prize, user.UserID, info);
                    new ShareCacheStruct<UserTakePrize>().Add(userTask);
                }
                if (fRest == null)
                {
                    fRest = new FestivalRestrain();
                    fRest.FestivalID = info.FestivalID;
                    fRest.RefreashDate = info.StartDate;
                    fRest.RestrainNum = 1;
                    fRest.UserID = user.UserID;
                    cacheSet.Add(fRest);
                }
                else
                {
                    fRest.RefreashDate = info.StartDate;
                    fRest.RestrainNum = 1;
                }
            }
        }

        /// <summary>
        /// 获得拉新卡
        /// </summary>
        /// <param name="user"></param>
        public static void NewHandCardFestival(GameUser user)
        {
            short userLv = (short)ConfigEnvSet.GetInt("UserCard.ActivationCardLv"); //获得新手卡等级
            if (user.UserLv >= userLv && (user.UserExtend == null || string.IsNullOrEmpty(user.UserExtend.CardUserID)))
            {
                GameUserExtend userExtend = new GameUserExtend();
                if (user.UserExtend != null)
                {
                    userExtend = user.UserExtend;
                }
                userExtend.CardUserID = NewHandCardID(user.UserID);
                user.UserExtend = userExtend;
                int gamecoin = ConfigEnvSet.GetInt("UserCard.GainGameCoin");
                int handNum = 0;
                DailyRestrainSet restrainSet = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.NewHand);
                if (restrainSet != null)
                {
                    handNum = restrainSet.MaxNum;
                }
                string content = string.Format(LanguageManager.GetLang().GainNewCard, userLv, gamecoin, user.UserExtend.CardUserID, handNum);
                GainNewCardPackage(user.UserID, gamecoin, string.Empty, content);
            }
        }

        /// <summary>
        /// 新手玩家达到等级获得奖励
        /// </summary>
        /// <param name="user"></param>
        public static void ActivationNewCard(GameUser user)
        {
            if (user.UserExtend != null && !string.IsNullOrEmpty(user.CardID))
            {
                string[] handLv = ConfigEnvSet.GetString("UserCard.GetPackageLv").Split(',');
                for (int i = 0; i < handLv.Length; i++)
                {
                    short newHandLv = Convert.ToInt16(handLv[i]);
                    if (IsReward(user.UserID, user.UserLv, newHandLv))
                    {
                        int cardUserID = user.CardID.Substring(1).ToInt();
                        AddCardReward(user.UserID, strUserID, newHandLv);
                        AddCardReward(cardUserID.ToString(), user.UserID, newHandLv);
                        string item = "5030=1=1";
                        string content = string.Format(LanguageManager.GetLang().NewHandPackage, newHandLv);
                        GainNewCardPackage(user.UserID, 0, item, content);
                        GainNewCardPackage(cardUserID.ToString(), 0, item, content);
                    }
                }
            }
        }

        public static void AddCardReward(string userID, string cardUserID, short userlv)
        {
            var cacheSet = new GameDataCacheSet<UserCardReward>();
            UserCardReward cardReward = new UserCardReward();
            cardReward.UserID = userID;
            cardReward.CardUserID = cardUserID;
            cardReward.UserLv = userlv;
            cardReward.CreateDate = DateTime.Now;
            cacheSet.Add(cardReward);
            cacheSet.Update();
        }

        /// <summary>
        /// 是否可领取奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userLv"></param>
        /// <returns></returns>
        public static bool IsReward(string userID, short userLv, short newhandLv)
        {
            var cacheSet = new GameDataCacheSet<UserCardReward>();
            if (userLv >= newhandLv)
            {
                UserCardReward card = cacheSet.FindKey(userID, strUserID, newhandLv);
                if (card == null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得奖励物品
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="gamecoin"></param>
        /// <param name="item"></param>
        /// <param name="content"></param>
        public static void GainNewCardPackage(string userID, int gamecoin, string item, string content)
        {
            UserTakePrize userPrize = new UserTakePrize();
            userPrize.ID = Guid.NewGuid().ToString();
            userPrize.UserID = Convert.ToInt32(userID);
            userPrize.ObtainNum = 0;
            userPrize.EnergyNum = 0;
            userPrize.GameCoin = gamecoin;
            userPrize.Gold = 0;
            userPrize.ExpNum = 0;
            userPrize.VipLv = 0;
            userPrize.GainBlessing = 0;
            userPrize.ItemPackage = item;
            userPrize.SparePackage = string.Empty;
            userPrize.CrystalPackage = string.Empty;
            userPrize.EnchantPackage = string.Empty;
            userPrize.MailContent = content;
            userPrize.IsTasked = false;
            userPrize.TaskDate = MathUtils.SqlMinDate;
            userPrize.OpUserID = 10000;
            userPrize.CreateDate = DateTime.Now;
            new ShareCacheStruct<UserTakePrize>().Add(userPrize);
        }


        /// <summary>
        /// 拉新卡号
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string NewHandCardID(string userID)
        {
            string str = "C" + userID.PadLeft(10, '0');
            return str;
        }

        /// <summary>
        /// 累计消费活动
        /// </summary>
        public static void TriggerFestivalConsume(string userID, int consumeNum, FestivalType festivalType)
        {
            FestivalInfo fest = GetInfo(festivalType);
            if (fest == null)
            {
                return;
            }
            var cacheSet = new GameDataCacheSet<UserConsume>();
            UserConsume userConsume = cacheSet.FindKey(userID);
            if (userConsume == null)
            {
                userConsume = new UserConsume();
                userConsume.UserID = userID;
                userConsume.GameCoin = 0;
                userConsume.GoldNum = 0;
                userConsume.EnergyNum = 0;
                cacheSet.Add(userConsume);
                cacheSet.Update();
                userConsume = new GameDataCacheSet<UserConsume>().FindKey(userID);
            }
            //晶石
            if (festivalType == FestivalType.SparConsumption)
            {
                if (userConsume.GoldDate < fest.StartDate)
                {
                    userConsume.GoldDate = fest.StartDate;
                    userConsume.GoldNum = consumeNum;
                }
                else
                {
                    userConsume.GoldNum = MathUtils.Addition(userConsume.GoldNum, consumeNum);
                }
            }

            //金币
            if (festivalType == FestivalType.GameCoin)
            {
                if (userConsume.CoinDate < fest.StartDate)
                {
                    userConsume.CoinDate = fest.StartDate;
                    userConsume.GameCoin = consumeNum;
                }
                else
                {
                    userConsume.GameCoin = MathUtils.Addition(userConsume.GameCoin, consumeNum);
                }
            }

            //精力
            if (festivalType == FestivalType.Energy)
            {
                if (userConsume.EnergyDate < fest.StartDate)
                {
                    userConsume.EnergyDate = fest.StartDate;
                    userConsume.EnergyNum = consumeNum;
                }
                else
                {
                    userConsume.EnergyNum = MathUtils.Addition(userConsume.EnergyNum, consumeNum);
                }
            }
            FestivalInfo[] festivalInfosArray =
              new ShareCacheStruct<FestivalInfo>().FindAll(m => m.FestivalType == festivalType).ToArray();
            foreach (FestivalInfo festivalInfo in festivalInfosArray)
            {
                FestivalConsumeCount(festivalInfo, userID);
            }
        }

        /// <summary>
        /// 累计消费活动奖励(晶石，金币，精力)
        /// </summary>
        private static void FestivalConsumeCount(FestivalInfo info, string userID)
        {
            UserConsume consume = new GameDataCacheSet<UserConsume>().FindKey(userID);
            if (consume == null)
            {
                return;
            }
            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
            FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
            if (fRest != null && fRest.RestrainNum >= 1 && fRest.RefreashDate == info.StartDate)
                return;
            if ((info.FestivalType == FestivalType.SparConsumption && consume.GoldDate >= info.StartDate && consume.GoldNum >= info.RestrainNum) ||
                (info.FestivalType == FestivalType.GameCoin && consume.CoinDate >= info.StartDate && consume.GameCoin >= info.RestrainNum) ||
                (info.FestivalType == FestivalType.Energy && consume.EnergyDate >= info.StartDate && consume.EnergyNum >= info.RestrainNum))
            {
                CacheList<PrizeInfo> prizeList = PrizeHelper.GetPrizeInfo(info.Reward);
                foreach (PrizeInfo prize in prizeList)
                {
                    UserTakePrize userTask = PrizeHelper.GetUserTake(prize, userID, info);
                    new ShareCacheStruct<UserTakePrize>().Add(userTask);
                }
                if (fRest == null)
                {
                    fRest = new FestivalRestrain();
                    fRest.FestivalID = info.FestivalID;
                    fRest.RefreashDate = info.StartDate;
                    fRest.RestrainNum = 1;
                    fRest.UserID = userID;
                    cacheSet.Add(fRest);
                }
                else
                {
                    fRest.RefreashDate = info.StartDate;
                    fRest.RestrainNum = 1;
                    //fRest.Update();
                }
            }
        }

        /// <summary>
        /// 购买精力限制次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="festivalType"></param>
        /// <param name="typeID"></param>
        public static void PurchasedEnergy(string userID)
        {
            FestivalInfo info = GetInfo(FestivalType.PurchasedEnergy);
            if (info != null)
            {
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest == null)
                {
                    fRest = new FestivalRestrain();
                    fRest.FestivalID = info.FestivalID;
                    fRest.RefreashDate = DateTime.Now;
                    fRest.RestrainNum = 1;
                    fRest.UserID = userID;
                    cacheSet.Add(fRest);
                }
                else
                {
                    if (DateTime.Now.Date == fRest.RefreashDate.Date)
                    {
                        fRest.RestrainNum = MathUtils.Addition(fRest.RestrainNum, 1);
                    }
                    else
                    {
                        fRest.RestrainNum = 1;
                    }
                    fRest.RefreashDate = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 购买精力，庄园加成
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static double SurplusPurchased(string userID, FestivalType festivalType)
        {
            double addNum = 1;
            FestivalInfo info = GetInfo(festivalType);
            if (info != null && info.FestivalExtend != null && info.FestivalExtend.MinusNum > 0)
            {
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest == null || fRest.RefreashDate < info.StartDate ||
                    (fRest.RefreashDate >= info.StartDate && fRest.RefreashDate <= info.EndDate))
                {
                    addNum = info.FestivalExtend.MinusNum;
                }
            }
            return addNum;
        }

        /// <summary>
        /// 活动剩余次数
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int SurplusEnergyNum(string userID)
        {
            int surplusNum = 0;
            FestivalInfo info = GetInfo(FestivalType.PurchasedEnergy);
            if (info != null && info.FestivalExtend != null && info.FestivalExtend.MinusNum > 0)
            {
                surplusNum = info.RestrainNum;
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest != null && fRest.RefreashDate.Date == DateTime.Now.Date)
                {
                    surplusNum = MathUtils.Subtraction(surplusNum, fRest.RestrainNum, 0);
                }
            }
            return surplusNum;
        }


        /// <summary>
        ///龟兔赛跑活动奖励
        /// </summary>
        public static double TortoiseHare(string userID)
        {
            double energyPro = 1;
            FestivalInfo info = GetInfo(FestivalType.TortoiseHare);
            if (info == null)
            {
                return energyPro;
            }
            List<TaskConfigInfo> taskConfigInfosList = new List<TaskConfigInfo>(info.TaskConfig);
            foreach (TaskConfigInfo taskConfigInfo in taskConfigInfosList)
            {
                if (taskConfigInfo.StratDate.ToDateTime() < DateTime.Now && DateTime.Now < taskConfigInfo.EndData.ToDateTime())
                {
                    energyPro = taskConfigInfo.MinusNum;
                    break;
                }
            }
            return energyPro;
        }

        /// <summary>
        ///vip活动礼包
        /// </summary>
        public static void GetVipFestival(string userID)
        {
            FestivalInfo info = GetInfo(FestivalType.VIPFestival);
            if (info != null && info.FestivalExtend != null)
            {
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest == null || fRest.RefreashDate < info.StartDate ||
                    (fRest.RestrainNum < info.RestrainNum && fRest.RefreashDate >= info.StartDate &&
                     fRest.RefreashDate <= info.EndDate))
                {
                    CacheList<PrizeInfo> prizeList = PrizeHelper.GetPrizeInfo(info.Reward);
                    foreach (PrizeInfo prize in prizeList)
                    {
                        UserTakePrize userTask = PrizeHelper.GetUserTake(prize, userID, info);
                        new ShareCacheStruct<UserTakePrize>().Add(userTask);
                    }
                    if (fRest == null)
                    {
                        fRest = new FestivalRestrain();
                        fRest.FestivalID = info.FestivalID;
                        fRest.RefreashDate = DateTime.Now;
                        fRest.RestrainNum = 1;
                        fRest.UserID = userID;
                        cacheSet.Add(fRest);
                    }
                    else
                    {
                        fRest.RestrainNum = MathUtils.Addition(fRest.RestrainNum, 1);
                        fRest.RefreashDate = DateTime.Now;
                    }
                }
            }
        }

        /// <summary>
        /// 增加活动未领取活动
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="festivalID"></param>
        public static void AppendFestivalRestrain(string userID, int festivalID, int reNum)
        {
            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
            FestivalRestrain fRest = new FestivalRestrain();
            fRest.UserID = userID;
            fRest.FestivalID = festivalID;
            fRest.RefreashDate = DateTime.Now;
            fRest.RestrainNum = reNum;
            fRest.IsReceive = false;
            cacheSet.Add(fRest);
        }

        /// <summary>
        /// 登入送好礼
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="type"></param>
        public static void GetLoginGainReward(string userID, FestivalType type)
        {
            FestivalInfo info = GetInfo(type);
            if (info != null && info.FestivalExtend != null)
            {
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest == null)
                {
                    AppendFestivalRestrain(userID, info.FestivalID, 1);
                }
                else
                {
                    var prizeList = info.Reward.FindAll(s => s.RefreshDate.Date == DateTime.Now.Date);
                    if (fRest.RefreashDate.Date != DateTime.Now.Date && prizeList.Count > 0)
                    {
                        fRest.RefreashDate = DateTime.Now;
                        fRest.IsReceive = false;
                    }
                }
            }
        }

        /// <summary>
        /// 根据类型获取活动奖励 首充奖励、登入送好礼
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="type"></param>
        public static void GetFirstReward(string userID, FestivalType type)
        {
            FestivalInfo info = GetInfo(type);
            if (info != null && info.FestivalExtend != null)
            {
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest == null)
                {
                    AppendFestivalRestrain(userID, info.FestivalID, 1);
                    //if (PrizeHelper.GetUserMail(userID, info))
                    //{
                    //}
                }
            }
        }

        /// <summary>
        /// 升级送好礼
        /// </summary>
        /// <param name="userID"></param>
        public static void GetUpgradeGiveGift(string userID, short userLv)
        {
            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
            var festivalList = new ShareCacheStruct<FestivalInfo>().FindAll(s => s.FestivalType == FestivalType.UpgradeGiveGift);
            foreach (var info in festivalList)
            {
                if (!info.IsStop)
                    continue;
                if (info.StartDate > DateTime.Now)
                    continue;
                if (info.EndDate < DateTime.Now)
                    continue;
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest == null && info.RestrainNum <= userLv)
                {
                    AppendFestivalRestrain(userID, info.FestivalID, info.RestrainNum);
                }

            }
        }

        /// <summary>
        /// 充值获得奖励 首次充值晶石翻倍、充值返利
        /// </summary>
        /// <param name="user"></param>
        /// <param name="payNum"></param>
        /// <param name="type"></param>
        public static void GetPayReward(GameUser user, int payNum, FestivalType type)
        {
            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
            var festivalList = new ShareCacheStruct<FestivalInfo>().FindAll(s => s.FestivalType == type);
            foreach (var info in festivalList)
            {
                if (!info.IsStop)
                    continue;
                if (info.StartDate > DateTime.Now)
                    continue;
                if (info.EndDate < DateTime.Now)
                    continue;
                if (info.FestivalType == type)
                {
                    if (info.FestivalExtend != null)
                    {
                        FestivalRestrain fRest = cacheSet.FindKey(user.UserID, info.FestivalID);
                        UserRecharge userRecharge = new GameDataCacheSet<UserRecharge>().FindKey(user.UserID);
                        if (type == FestivalType.PayReward)
                        {
                            if (fRest == null && userRecharge != null && userRecharge.TotalGoldNum >= info.RestrainNum)
                            {
                                AppendFestivalRestrain(user.UserID, info.FestivalID, info.RestrainNum);
                            }
                        }
                        else
                        {
                            if (fRest == null && payNum >= info.RestrainNum)
                            {
                                AppendFestivalRestrain(user.UserID, info.FestivalID, payNum);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 商城折扣
        /// </summary>
        /// <returns></returns>
        public static double StoreDiscount()
        {
            FestivalInfo info = GetInfo(FestivalType.StoreDiscount);
            if (info != null && info.FestivalExtend != null)
            {
                return info.FestivalExtend.MinusNum;
            }
            return 1;
        }

        /// <summary>
        /// 副本倍数掉落活动
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int DuplicateDropDouble(string userID)
        {
            int multiple = 1;
            FestivalInfo info = GetInfo(FestivalType.DuplicateDropDouble);
            if (info != null && info.FestivalExtend != null && info.FestivalExtend.MinusNum > 0)
            {
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest == null || fRest.RefreashDate.Date != DateTime.Now.Date || fRest.RestrainNum < info.RestrainNum)
                {
                    multiple = info.FestivalExtend.MinusNum.ToInt();
                }
            }
            return multiple;
        }

        /// <summary>
        /// 副本倍数掉落活动限制次数
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static void DuplicateDropDoubleRestrain(string userID)
        {
            int multiple = 1;
            FestivalInfo info = GetInfo(FestivalType.DuplicateDropDouble);
            if (info != null && info.FestivalExtend != null)
            {
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest == null)
                {
                    fRest = new FestivalRestrain();
                    fRest.UserID = userID;
                    fRest.FestivalID = info.FestivalID;
                    fRest.RefreashDate = DateTime.Now;
                    fRest.RestrainNum = 1;
                    cacheSet.Add(fRest);
                }
                else
                {
                    if (fRest.RefreashDate.Date != DateTime.Now.Date)
                    {
                        fRest.RestrainNum = 0;
                        fRest.RefreashDate = DateTime.Now;
                    }
                    fRest.RestrainNum = MathUtils.Addition(fRest.RestrainNum, 1);
                }
            }
        }

        /// <summary>
        /// 招募佣兵送魂魄
        /// </summary>
        /// <param name="userID"></param>
        public static string RecruitGeneral(string userID, RecruitType rtype)
        {
            string festivalContent = string.Empty;
            FestivalInfo info = GetInfo(FestivalType.RecruitGeneral);
            if (info != null && info.FestivalExtend != null && info.FestivalExtend.Recruit.Count > 0)
            {
                var recruitFest = info.FestivalExtend.Recruit.Find(s => s.Type == rtype);
                if (recruitFest != null)
                {
                    var generalList = new ConfigCacheSet<GeneralInfo>().FindAll(s => s.GeneralQuality == recruitFest.GeneralQuality);
                    if (generalList.Count > 0)
                    {
                        int index1 = RandomUtils.GetRandom(0, generalList.Count);
                        if (index1 > -1)
                        {
                            GeneralInfo generalInfo = generalList[index1];
                            if (generalInfo != null)
                            {
                                GeneralHelper.UpdateUserGeneral(userID, generalInfo, GeneralType.Soul, recruitFest.Num);
                                if (!string.IsNullOrEmpty(info.FestivalExtend.Desc))
                                {
                                    festivalContent = string.Format(info.FestivalExtend.Desc, generalInfo.GeneralName, recruitFest.Num);
                                }
                                return festivalContent;
                            }
                        }
                    }
                    else if (recruitFest.GeneralID > 0)
                    {
                        GeneralInfo generalInfo =
                            new ConfigCacheSet<GeneralInfo>().FindKey(recruitFest.GeneralID);
                        if (generalInfo != null)
                        {
                            GeneralHelper.UpdateUserGeneral(userID, generalInfo, GeneralType.Soul, recruitFest.Num);
                            if (!string.IsNullOrEmpty(info.FestivalExtend.Desc))
                            {
                                festivalContent = string.Format(info.FestivalExtend.Desc, generalInfo.GeneralName, recruitFest.Num);
                            }
                            return festivalContent;
                        }
                    }
                }
            }
            return festivalContent;
        }

        /// <summary>
        /// 累计消费送礼
        /// </summary>
        public static void PayAccumulation(string userID, int consumeNum)
        {
            FestivalInfo fest = GetInfo(FestivalType.PayAccumulation);
            if (fest == null)
            {
                return;
            }
            var cacheSet = new GameDataCacheSet<UserConsume>();
            UserConsume userConsume = cacheSet.FindKey(userID);
            if (userConsume == null)
            {
                userConsume = new UserConsume();
                userConsume.UserID = userID;
                userConsume.GameCoin = 0;
                userConsume.GoldNum = 0;
                userConsume.EnergyNum = 0;
                cacheSet.Add(userConsume);
                cacheSet.Update();
                userConsume = new GameDataCacheSet<UserConsume>().FindKey(userID);
            }
            if (userConsume.GoldDate < fest.StartDate)
            {
                userConsume.GoldDate = fest.StartDate;
                userConsume.GoldNum = consumeNum;
            }
            else
            {
                userConsume.GoldNum = MathUtils.Addition(userConsume.GoldNum, consumeNum);
            }
            var cacheSetRestrain = new GameDataCacheSet<FestivalRestrain>();
            var festivalList = new ShareCacheStruct<FestivalInfo>().FindAll(s => s.FestivalType == FestivalType.PayAccumulation);
            foreach (var info in festivalList)
            {
                if (!info.IsStop)
                    continue;
                if (info.StartDate > DateTime.Now)
                    continue;
                if (info.EndDate < DateTime.Now)
                    continue;
                if (info.FestivalType == FestivalType.PayAccumulation)
                {
                    FestivalRestrain fRest = cacheSetRestrain.FindKey(userID, info.FestivalID);
                    if (fRest != null && userConsume.GoldNum >= info.RestrainNum)
                    {
                        AppendFestivalRestrain(userID, fest.FestivalID, fest.RestrainNum);
                    }
                }
            }
        }

        /// <summary>
        /// 活动有效次数
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int FestivalSurplusNum(string userID, int festID)
        {
            int surplusNum = 0;
            FestivalInfo info = new ShareCacheStruct<FestivalInfo>().FindKey(festID);
            if (info != null && info.RestrainNum > 0)
            {
                surplusNum = info.RestrainNum;
                if (info.FestivalType == FestivalType.UpgradeGiveGift)
                {
                    return surplusNum;
                }
                var cacheSet = new GameDataCacheSet<FestivalRestrain>();
                FestivalRestrain fRest = cacheSet.FindKey(userID, info.FestivalID);
                if (fRest != null && fRest.RefreashDate.Date == DateTime.Now.Date)
                {
                    surplusNum = MathUtils.Subtraction(surplusNum, fRest.RestrainNum, 0);
                }
            }
            return surplusNum;
        }

        /// <summary>
        /// 活动奖励是否可领取
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static short FestivalIsReceive(GameUser user, int festID)
        {
            short isReceive = 0;
            FestivalInfo info = new ShareCacheStruct<FestivalInfo>().FindKey(festID);
            if (info != null)
            {
                FestivalRestrain fRest = new GameDataCacheSet<FestivalRestrain>().FindKey(user.UserID, info.FestivalID);
                if (info.FestivalType == FestivalType.UpgradeGiveGift && user.UserLv >= info.RestrainNum)
                {
                    if (fRest == null)
                    {
                        return 0;
                    }
                    else if (!fRest.IsReceive)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                if (info.FestivalType == FestivalType.DuplicateDropDouble)
                {
                    int num = FestivalSurplusNum(user.UserID, festID);
                    if (fRest == null || fRest.RefreashDate.Date != DateTime.Now.Date || info.RestrainNum > num)
                    {
                        isReceive = 1;
                    }
                }
                else if (info.FestivalType == FestivalType.StoreDiscount)
                {
                    isReceive = 1;
                }
                else
                {
                    if (fRest != null && !fRest.IsReceive)
                    {
                        isReceive = 1;
                    }
                }
                if (fRest != null && fRest.IsReceive)
                {
                    isReceive = 2;
                }
            }
            return isReceive;
        }

        /// <summary>
        /// 日常活动领取
        /// </summary>
        /// <param name="user"></param>
        /// <param name="festivalID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool UseFestivalRestrain(GameUser user, int festivalID, out string content)
        {
            content = string.Empty;
            FestivalInfo info = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            var cacheSet = new GameDataCacheSet<FestivalRestrain>();
            FestivalRestrain restrain = cacheSet.FindKey(user.UserID, festivalID);
            short isReceive = FestivalIsReceive(user, festivalID);
            if (info != null && isReceive == 1)
            {
                if (info.FestivalType == FestivalType.UpgradeGiveGift && user.UserLv < info.RestrainNum)
                {
                    content = LanguageManager.GetLang().St_LevelNotEnough;
                    return false;
                }
                if (info.FestivalType == FestivalType.FirstReward
                    || info.FestivalType == FestivalType.UpgradeGiveGift || info.FestivalType == FestivalType.PayAccumulation)
                {
                    content = PrizeHelper.PrizeContent(user, info.Reward.ToList());
                    if (!string.IsNullOrEmpty(content))
                    {
                        return false;
                    }
                    content = PrizeHelper.GetPrizeUserTake(user, info.Reward.ToList());
                    if (!string.IsNullOrEmpty(content))
                    {
                        if (info.FestivalExtend != null && !string.IsNullOrEmpty(info.FestivalExtend.Desc))
                        {
                            content = string.Format(info.FestivalExtend.Desc, content);
                        }
                        return true;
                    }
                }
                if (restrain != null && (info.FestivalType == FestivalType.PayReward || info.FestivalType == FestivalType.FirstPayDoubleSpar))
                {
                    if (info.FestivalExtend != null)
                    {
                        int payNum = (info.RestrainNum * info.FestivalExtend.MinusNum).ToInt();
                        user.GiftGold = MathUtils.Addition(user.GiftGold, payNum);
                        if (!string.IsNullOrEmpty(info.FestivalExtend.Desc))
                        {
                            content = string.Format(info.FestivalExtend.Desc, payNum);
                        }
                        return true;
                    }
                }
                if (info.FestivalType == FestivalType.LoginGiveGift)
                {
                    List<PrizeInfo> prizeList = info.Reward.FindAll(s => s.RefreshDate.Date == DateTime.Now.Date);
                    content = PrizeHelper.GetPrizeUserTake(user, prizeList);
                    if (!string.IsNullOrEmpty(content))
                    {
                        if (info.FestivalExtend != null && !string.IsNullOrEmpty(info.FestivalExtend.Desc))
                        {
                            content = string.Format(info.FestivalExtend.Desc, content);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 当前类型的活动是否都已经完成
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="ftype"></param>
        /// <returns></returns>
        public static bool FestivalCompleted(string userID, FestivalType ftype)
        {
            var list = new ShareCacheStruct<FestivalInfo>().FindAll(s => s.FestivalType == ftype);
            foreach (var festival in list)
            {
                var fRest = new GameDataCacheSet<FestivalRestrain>().FindKey(userID, festival.FestivalID);
                if (fRest != null && fRest.IsReceive)
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    }
}