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
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Com;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5101_竞技场列表接口
    /// </summary>
    public class Action5101 : BaseAction
    {
        private List<UserRank> _userRankArray = new List<UserRank>();
        private List<SportsCombat> _userCombatArray = new List<SportsCombat>();

        private string sportsName = string.Empty;
        private int victoryNum;
        private int receiveDate;
        private int codeTime;
        private int sportNum;
        private SportsRewardInfo sportsReward;
        private int rankID = 0;
        private int sportsIntegral = 0;
        private IList<UserRank> userRankArray = new List<UserRank>();

        public Action5101(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5101, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(ObjectExtend.ToNotNullString(ContextUser.NickName));
            PushIntoStack(sportsName.ToNotNullString());
            PushIntoStack(ContextUser.ObtainNum);
            PushIntoStack(rankID);
            PushIntoStack(victoryNum);
            PushIntoStack(sportsReward == null ? 0 : sportsReward.RewardGoin);
            PushIntoStack(sportsReward == null ? 0 : sportsReward.RewardObtian);
            PushIntoStack(receiveDate);
            PushIntoStack(sportNum);
            PushIntoStack(codeTime);
            PushIntoStack(_userRankArray.Count);
            var userGeneralCacheSet = new GameDataCacheSet<UserGeneral>();
            foreach (UserRank user in _userRankArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(user.UserID.ToNotNullString());
                dsItem.PushIntoStack(user.NickName.ToNotNullString());
                dsItem.PushIntoStack(user.HeadID.ToNotNullString());
                dsItem.PushIntoStack(user.RankId);
                dsItem.PushIntoStack(user.UserLv);

                var embattleList = EmbattleHelper.CurrEmbattle(user.UserID, true);
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

                SportsRewardInfo sportsInfo = new ConfigCacheSet<SportsRewardInfo>().FindKey(user.RankId);
                var embatList = EmbattleHelper.CurrEmbattle(user.UserID, false);
                dsItem.PushIntoStack(embatList.Count);
                dsItem.PushIntoStack(embattleList.Count);
                dsItem.PushIntoStack(sportsInfo == null ? 0 : sportsInfo.RewardObtian);
                PushIntoStack(dsItem);
            }

            int index = 0;
            PushIntoStack(_userCombatArray.Count);
            foreach (var usercombat in _userCombatArray)
            {
                short iswin = usercombat.IsWin ? (short)1 : (short)2;
                string userName = string.Empty;
                string toUserName = string.Empty;
                if (usercombat.IsSelf)
                {
                    userName = LanguageManager.GetLang().St5101_JingJiChangMingCheng;
                    toUserName = usercombat.ToUserName;
                }
                else
                {
                    userName = usercombat.ToUserName;
                    toUserName = LanguageManager.GetLang().St5101_JingJiChangMingCheng;
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(MathUtils.ToNotNullString(usercombat.CombatDate.ToString("t")));
                dsItem.PushIntoStack(userName.ToNotNullString());
                dsItem.PushIntoStack(toUserName.ToNotNullString());
                dsItem.PushIntoStack(iswin);
                dsItem.PushIntoStack(usercombat.RankStatus.ToInt());
                dsItem.PushIntoStack(usercombat.TopID);
                dsItem.PushIntoStack(index.ToNotNullString());
                PushIntoStack(dsItem);

                index++;
            }


            string spoName = string.Empty;
            PushIntoStack(userRankArray.Count);
            foreach (UserRank rank in userRankArray)
            {
                spoName = UserHelper.SportTitleName(rank.ObtainNum);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(rank.RankId);
                dsItem.PushIntoStack(rank.UserID.ToNotNullString());
                dsItem.PushIntoStack(rank.NickName.ToNotNullString());
                dsItem.PushIntoStack(rank.UserLv);
                dsItem.PushIntoStack(rank.TotalCombatNum > 0 ? rank.TotalCombatNum : UserHelper.GetTotalCombatNum(rank.UserID));
                dsItem.PushIntoStack(spoName);
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

            PushIntoStack(sportsIntegral);
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            UserStatus status = ContextUser.UserStatus;
            if (status == UserStatus.FengJin)
            {
                return false;
            }
            UserGeneral general = UserGeneral.GetMainGeneral(ContextUser.UserID);
            Ranking<UserRank> ranking = RankingFactory.Get<UserRank>(CombatRanking.RankingKey);
            UserRank rankInfo;
            if (ranking.TryGetRankNo(m => m.UserID == ContextUser.UserID, out rankID))
            {
                rankInfo = ranking.Find(s => s.UserID == ContextUser.UserID);
                //ContextUser.RankID = rankID;
            }
            else
            {
                rankInfo = new UserRank()
                               {
                                   UserID = ContextUser.UserID,
                                   HeadID = general.HeadID,
                                   GameCoin = ContextUser.GameCoin,
                                   NickName = ContextUser.NickName,
                                   ObtainNum = ContextUser.ObtainNum,
                                   UserLv = ContextUser.UserLv,
                                   RankId = int.MaxValue,
                                   RankDate = DateTime.Now,
                                   SportsIntegral = 0
                               };
                ranking.TryAppend(rankInfo);
            }

            sportsName = UserHelper.SportTitleName(ContextUser.ObtainNum);

            sportNum = GetChallGeNum(ContextUser.UserID);
            sportsIntegral = ContextUser.SportsIntegral.ToInt();
            sportsReward = new ConfigCacheSet<SportsRewardInfo>().FindKey(rankInfo.RankId);
            receiveDate = (int)(RankingHelper.GetNextReceiveDate() - DateTime.Now).TotalSeconds;

            CombatRanking combatrank = (CombatRanking)ranking;
            _userRankArray = combatrank.GetRanking(ContextUser);
            _userRankArray.Add(rankInfo);
            _userRankArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.RankId.CompareTo(y.RankId);
            });

            _userCombatArray = ContextUser.GetSportsCombat();
            _userCombatArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return y.CombatDate.CompareTo(x.CombatDate);
            });

            if (rankInfo != null && rankID > 0)
            {
                victoryNum = rankInfo.VictoryNum;
            }

            //NoviceHelper.SportVictoryNum(ContextUser, 2001, victoryNum); //竞技场奖励
            if (victoryNum >= 7)
            {
                ActivitiesAward.HolidayFestival(ContextUser.UserID);
            }
            rankID = ContextUser.RankID;




            int count = 10;
            int pagecount = 0;
            userRankArray = ranking.GetRange(1, count, out pagecount);
            return true;
        }

        /// <summary>
        /// 剩余挑战次数
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int GetChallGeNum(string userID)
        {
            int currNum = 0;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.RefreshDate.Date == DateTime.Now.Date)
            {
                currNum = dailyRestrain.Funtion9;
            }

            int InitialNum = VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.JingJiChangTiaoZhan); //当日挑战次数
            UserChallengeNum userChallenge = new GameDataCacheSet<UserChallengeNum>().FindKey(userID);
            if (userChallenge != null && DateTime.Now.Date == userChallenge.InsertDate.Date)
            {
                InitialNum = MathUtils.Addition(InitialNum, userChallenge.ChallengeNum, int.MaxValue); //当日总挑战次数
            }
            return MathUtils.Subtraction(InitialNum, currNum, 0);
        }
    }
}