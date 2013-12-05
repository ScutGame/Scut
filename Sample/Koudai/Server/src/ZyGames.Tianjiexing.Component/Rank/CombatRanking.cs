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
using System.Linq;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Data;
using ZyGames.Tianjiexing.Model;
using System.Data;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Tianjiexing.Component
{
    /// <summary>
    /// 竞技场排行
    /// </summary>
    public class CombatRanking : Ranking<UserRank>
    {
        public const string RankingKey = "CombatRanking";
        public List<UserRank> rankList = new List<UserRank>();

        public CombatRanking()
            : base(RankingKey, int.MaxValue)
        {
        }

        protected override int ComparerTo(UserRank x, UserRank y)
        {
            int result = 0;
            if (x == null && y == null)
            {
                return 0;
            }
            if (x != null && y == null)
            {
                return 1;
            }
            if (x == null)
            {
                return -1;
            }
            result = x.RankId.CompareTo(y.RankId);
            if (result == 0)
            {
                result = y.UserLv.CompareTo(x.UserLv);
                if (result == 0)
                {
                    result = x.UserID.CompareTo(y.UserID);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取前5名玩家
        /// </summary>
        /// <returns></returns>
        public List<UserRank> GetRanking(GameUser user)
        {
            List<UserRank> userRankList = new List<UserRank>();
            int currRankId;
            if (TryGetRankNo(m => m.UserID == user.UserID, out currRankId))
            {
                //user.RankID = currRankId;
                int rankIncrice;
                int length = 5;
                if (currRankId < 51) rankIncrice = 1;
                else if (currRankId < 101) rankIncrice = 2;
                else if (currRankId < 501) rankIncrice = 5;
                else rankIncrice = 10;
                int pagesize;
                int pageIndex;
                if (currRankId > 5)
                {
                    pagesize = currRankId;
                    pageIndex = currRankId - (length + 1) * rankIncrice;
                }
                else
                {
                    pagesize = 6;
                    pageIndex = 0;
                }
                int pagecount;
                IList<UserRank> list = this.GetRange(1, pagesize, out pagecount);
                while (pageIndex < pagesize && pageIndex < list.Count)
                {
                    if (list.Count <= pageIndex)
                    {
                        break;
                    }
                    if (list[pageIndex].UserID == user.UserID)
                    {
                        pageIndex = MathUtils.Addition(pageIndex, rankIncrice);
                        continue;
                    }
                    userRankList.Add(list[pageIndex]);
                    pageIndex = MathUtils.Addition(pageIndex, rankIncrice);
                }
            }
            return userRankList;
        }

        protected override IList<UserRank> GetCacheList()
        {
            if (rankList.Count > 0)
            {
                return rankList;
            }
            var dbProvider = DbConnectionProvider.CreateDbProvider(DbConfig.Data);
            string sql = "SELECT UserID,NickName,CombatNum,VictoryNum,RankID,UserLv,ObtainNum,GameCoin,VipLv,CountryID,SportsIntegral,RankDate FROM GameUser where RankID>0 order by RankID";
            using (IDataReader reader = dbProvider.ExecuteReader(CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    UserRank rankInfo = new UserRank();
                    rankInfo.UserID = reader["UserID"].ToString();
                    rankInfo.NickName = reader["NickName"].ToString();
                    rankInfo.UserLv = Convert.ToInt16(reader["UserLv"]);
                    rankInfo.ObtainNum = Convert.ToInt32(reader["ObtainNum"]);
                    rankInfo.TotalCombatNum = Convert.ToInt32(reader["CombatNum"]);
                    rankInfo.VictoryNum = Convert.ToInt32(reader["VictoryNum"]);
                    rankInfo.GameCoin = Convert.ToInt32(reader["GameCoin"]);
                    rankInfo.CountryID = reader["CountryID"].ToEnum<CountryType>();
                    rankInfo.VipLv = Convert.ToInt16(reader["VipLv"]);
                    //rankInfo.HeadID = reader["HeadID"].ToString();
                    rankInfo.RankId = Convert.ToInt32(reader["RankID"]);
                    rankInfo.SportsIntegral = Convert.ToInt32(reader["SportsIntegral"]);
                    rankInfo.RankDate = reader["RankDate"].ToDateTime();
                    rankList.Add(rankInfo);
                }
            }
            return rankList;
        }
        protected override void ChangeRankNo(UserRank item)
        {
            var gameUser = new GameDataCacheSet<GameUser>().FindKey(item.UserID);
            if (gameUser == null)
            {
                return;
            }
            gameUser.RankID = item.RankId;
            gameUser.RankDate = DateTime.Now;
        }
    }
}