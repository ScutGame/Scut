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
using System.Web;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Component
{
    /// <summary>
    /// 玩家战力排行榜
    /// </summary>
    public class CombatNumRanking : Ranking<UserRank>
    {
        public const string RankingKey = "CombatNumRanking";
        private const int listcount = 50;

        public CombatNumRanking()
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
            result = y.TotalCombatNum.CompareTo(x.TotalCombatNum);
            return result;
        }

        protected override IList<UserRank> GetCacheList()
        {
            List<UserRank> rankingUserlv = new List<UserRank>();

            new GameDataCacheSet<GameUser>().Foreach((personalId, key, user) =>
            {
                UserRank rankingUser = new UserRank();
                rankingUser.UserID = user.UserID;
                rankingUser.NickName = user.NickName;
                rankingUser.TotalCombatNum = user.CombatNum;
                rankingUser.CountryID = user.CountryID;
                rankingUser.VipLv = (short)user.VipLv;
                rankingUserlv.Add(rankingUser);
                return true;
            });

            return rankingUserlv;
        }

    }
}