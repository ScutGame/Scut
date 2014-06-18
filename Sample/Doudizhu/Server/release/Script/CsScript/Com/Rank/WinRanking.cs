using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Rank;

namespace ZyGames.Doudizhu.Bll
{
    public class WinRanking : Ranking<UserRank>
    {
        public const string RankingKey = "WinRanking";
        private const int listcount = 100;
        private int totalofficeNum = ConfigEnvSet.GetInt("Ranking.OfficeNumber", 500);

        public WinRanking()
            : base(RankingKey, listcount)
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
            result = y.Wining.CompareTo(x.Wining);
            if (result == 0)
            {
                result = y.WinNum.CompareTo(x.WinNum);
            }
            return result;
        }

        protected override IList<UserRank> GetCacheList()
        {
            List<UserRank> rankingUserlv = new List<UserRank>();

            new GameDataCacheSet<GameUser>().Foreach((personalId, key, user) =>
            {
                int totalNum = MathUtils.Addition(user.WinNum, user.FailNum);
                if (totalNum >= totalofficeNum && user.LoginDate.Date.AddDays(7) > DateTime.Now.Date)
                {
                    UserRank rankingUser = new UserRank();
                    rankingUser.UserID = user.UserId;
                    rankingUser.NickName = user.NickName;
                    rankingUser.WinNum = user.WinNum;
                    rankingUser.FailNum = user.FailNum;

                    rankingUser.Wining = MathUtils.RoundCustom((decimal)user.WinNum / totalNum, 2);
                    rankingUserlv.Add(rankingUser);
                }
                return true;
            });
            return rankingUserlv;
        }
    }
}
