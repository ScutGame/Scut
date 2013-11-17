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
using System.Diagnostics;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model.DataModel;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Com;
using ZyGames.Tianjiexing.Component;
using ZyGames.Framework.Game.Com.Rank;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 竞技场帮助类
    /// </summary>
    public class RankingHelper
    {
        /// <summary>
        /// 竞技场每日奖励
        /// </summary>
        /// <param name="user"></param>
        public static void DailySportsRankPrize(GameUser user)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
            if (IsGainSportsReward(user.UserID) && dailyRestrain != null && dailyRestrain.UserExtend != null)
            {
                //var envSet = ServerEnvSet.Get(ServerEnvKey.JingJiChangReward, "");
                SportsRewardInfo sportsInfo = new ConfigCacheSet<SportsRewardInfo>().FindKey(dailyRestrain.UserExtend.UserRankID);
                //if (envSet != null && sportsInfo != null && envSet.ToDateTime().Date > dailyRestrain.Funtion11.Date &&
                // dailyRestrain.UserExtend.UserRankID > 0)
                if (sportsInfo != null && DateTime.Now.Date != dailyRestrain.Funtion11.Date &&
                 dailyRestrain.UserExtend.UserRankID > 0)
                {
                    dailyRestrain.Funtion11 = DateTime.Now;
                    dailyRestrain.UserExtend.UserRankID = 0;

                    user.SportsIntegral = MathUtils.Addition(user.SportsIntegral, sportsInfo.RewardObtian,
                                                             int.MaxValue);
                    user.GameCoin = MathUtils.Addition(user.GameCoin, sportsInfo.RewardGoin, int.MaxValue);
                    string sportContent = string.Format(LanguageManager.GetLang().St5106_JingJiChangRankReward,
                                                        sportsInfo.RewardObtian, sportsInfo.RewardGoin);
                    var chatService = new TjxChatService();
                    chatService.SystemSendWhisper(user, sportContent);
                }
            }
        }

        /// <summary>
        /// 竞技场奖励是否可领取
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool IsGainSportsReward(string userID)
        {
            if (UserHelper.IsOpenFunction(userID, FunctionEnum.Jingjichang))
            {
                int rankID = 0;
                Ranking<UserRank> ranking = RankingFactory.Get<UserRank>(CombatRanking.RankingKey);
                if (ranking.TryGetRankNo(m => m.UserID == userID, out rankID))
                {
                    SportSRewardGain(userID, rankID);
                    SportsRewardInfo sportsInfo = new ConfigCacheSet<SportsRewardInfo>().FindKey(rankID);
                    if (sportsInfo != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 竞技场排名
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="rankID"></param>
        public static int SportSRewardGain(string userID, int rankID)
        {
            if (UserHelper.IsOpenFunction(userID, FunctionEnum.Jingjichang))
            {
                //var envSet = ServerEnvSet.Get(ServerEnvKey.JingJiChangReward, "");
                UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
                if (dailyRestrain == null)
                {
                    return 0;
                }

                if (DateTime.Now.Date != dailyRestrain.Funtion11.Date)
                {
                    if (dailyRestrain.UserExtend == null)
                    {
                        dailyRestrain.UserExtend = new DailyUserExtend();
                    }
                    dailyRestrain.UserExtend.UpdateNotify(obj =>
                    {
                        dailyRestrain.UserExtend.UserRankID = rankID;
                        return true;
                    });
                    return rankID;
                }
                //if (envSet == null || envSet.ToDateTime().Date >= GetNextReceiveDate())
                //{
                //    GetNextReceiveDate();
                //    envSet = ServerEnvSet.Get(ServerEnvKey.JingJiChangReward, "");
                //}
                //if (envSet == null || dailyRestrain == null)
                //{
                //    return 0;
                //}
                //if (envSet.ToDateTime().Date > dailyRestrain.Funtion11.Date)
                //{
                //    if (dailyRestrain.UserExtend == null)
                //    {
                //        dailyRestrain.UserExtend = new DailyUserExtend();
                //    }
                //    dailyRestrain.UserExtend.UpdateNotify(obj =>
                //    {
                //        dailyRestrain.UserExtend.UserRankID = rankID;
                //        return true;
                //    });
                //    return rankID;
                //}
            }
            return 0;
        }

        /// <summary>
        /// 竞技场奖励下次领取时间 
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNextReceiveDate()
        {
            DateTime nextReceiveDate = new DateTime(); //下次领取时间
            DateTime initialDate = ConfigEnvSet.GetString("Sports.ReceiveBonusTime").ToDateTime();
            int intervalDate = ConfigEnvSet.GetInt("Sports.IntervalDate"); //间隔时间 （小时）
            int intervalHours = (int)((DateTime.Now - initialDate).TotalHours);
            int dividedNum = intervalHours / intervalDate;
            int totalHuors = (MathUtils.Addition(dividedNum, 1, int.MaxValue) * intervalDate);
            nextReceiveDate = initialDate.AddHours(totalHuors);
            DateTime nowDate = initialDate.AddHours((dividedNum * intervalDate));

            var date = ServerEnvSet.Get(ServerEnvKey.JingJiChangReward, "");
            if (nowDate != date.ToDateTime().Date)
            {
                ServerEnvSet.Set(ServerEnvKey.JingJiChangReward, nowDate.ToString());
            }
            else
            {
                ServerEnvSet.Set(ServerEnvKey.JingJiChangReward, nextReceiveDate.AddDays(-3).ToString());
            }
            return nextReceiveDate.Date;
        }
    }
}