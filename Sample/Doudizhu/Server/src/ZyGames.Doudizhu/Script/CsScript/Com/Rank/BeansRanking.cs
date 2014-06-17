using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Rank;

namespace ZyGames.Doudizhu.Bll
{
    /// <summary>
    /// 金豆排行
    /// </summary>
    public class BeansRanking : Ranking<UserRank>
    {
        public const string RankingKey = "BeansRanking";
        private const int listcount = 100;

        public BeansRanking()
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
            result = y.GameCoin.CompareTo(x.GameCoin);
            return result;
        }

        protected override IList<UserRank> GetCacheList()
        {
            List<UserRank> rankingUserlv = new List<UserRank>();
            try
            {
                new GameDataCacheSet<GameUser>().Foreach((personalId, key, user) =>
                {
                    if (user.LoginDate.Date.AddDays(7) > DateTime.Now.Date)
                    {
                        UserRank rankingUser = new UserRank();
                        rankingUser.UserID = user.UserId;
                        rankingUser.NickName = user.NickName;
                        rankingUser.GameCoin = user.GameCoin;
                        rankingUserlv.Add(rankingUser);
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("BeansRanking error:{0}", ex);
            }
            return rankingUserlv;
        }
    }
}
