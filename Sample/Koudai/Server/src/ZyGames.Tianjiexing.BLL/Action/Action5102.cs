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
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 5102_竞技英雄列表接口
    /// </summary>
    public class Action5102 : BaseAction
    {
        private IList<UserRank> userRankArray = new List<UserRank>();

        public Action5102(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5102, httpGet)
        {

        }

        public override void BuildPacket()
        {
            string sportsName = string.Empty;
            var userGeneralCacheSet = new GameDataCacheSet<UserGeneral>();
            PushIntoStack(userRankArray.Count);
            foreach (UserRank rank in userRankArray)
            {
                sportsName = UserHelper.SportTitleName(rank.ObtainNum);
                short trend = SportTrend(rank.UserID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(rank.RankId);
                dsItem.PushIntoStack(rank.UserID.ToNotNullString());
                dsItem.PushIntoStack(rank.NickName.ToNotNullString());
                dsItem.PushIntoStack(rank.UserLv);
                dsItem.PushIntoStack(rank.TotalCombatNum > 0 ? rank.TotalCombatNum : UserHelper.GetTotalCombatNum(rank.UserID));
                dsItem.PushIntoStack(trend);
                dsItem.PushIntoStack(sportsName);
                dsItem.PushIntoStack(rank.VictoryNum);

                var embattleList = EmbattleHelper.CurrEmbattle(rank.UserID, true);
                dsItem.PushIntoStack(embattleList.Count);
                foreach (var item in embattleList)
                {
                    var userGenearl = userGeneralCacheSet.FindKey(item.UserID, item.GeneralID);
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(item.GeneralID);
                    dsItem1.PushIntoStack(userGenearl == null ? string.Empty : userGenearl.GeneralName.ToNotNullString());
                    dsItem1.PushIntoStack(userGenearl == null ? string.Empty : userGenearl.HeadID.ToNotNullString());
                    dsItem1.PushIntoStack(userGenearl == null ? 0 : userGenearl.GeneralQuality.ToInt());
                    dsItem1.PushIntoStack(userGenearl == null ? 0 : userGenearl.GeneralLv.ToInt());
                    dsItem.PushIntoStack(dsItem1);
                }

                SportsRewardInfo sportsInfo = new ConfigCacheSet<SportsRewardInfo>().FindKey(rank.RankId);
                var embatList = EmbattleHelper.CurrEmbattle(rank.UserID, false);
                dsItem.PushIntoStack(embatList.Count);
                dsItem.PushIntoStack(embattleList.Count);
                dsItem.PushIntoStack(sportsInfo == null ? 0 : sportsInfo.RewardObtian);
                PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            Ranking<UserRank> ranking = RankingFactory.Get<UserRank>(CombatRanking.RankingKey);

            int count = 20;
            int pagecount = 0;
            userRankArray = ranking.GetRange(1, count, out pagecount);
            return true;
        }

        /// <summary>
        /// 趋势1:上升,2:下降,3:不变化
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static short SportTrend(string userID)
        {
            short trend = 3;
            GameUser gameUser = UserCacheGlobal.CheckLoadUser(userID);
            if (gameUser == null) return trend;

            IList<SportsCombat> userSportsCombats = gameUser.GetSportsCombat();
            if (userSportsCombats.Count == 0) return trend;

            SportsCombat nextCombat = userSportsCombats[userSportsCombats.Count - 1];
            if (DateTime.Now < nextCombat.CombatDate.AddSeconds(3600))
            {
                trend = nextCombat.RankStatus == 0 ? (short)3 : (nextCombat.RankStatus == 2 ? (short)2 : (short)1);
            }
            return trend;
        }
    }
}