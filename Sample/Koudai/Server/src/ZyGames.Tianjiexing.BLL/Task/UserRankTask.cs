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
using System.Data;
using System.Configuration;
using System.Linq;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Timing;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Task
{
    public class UserRankTask : BaseTask
    {
        private const int OrderCount = 50;
        public UserRankTask()
            : base(60000)
        {
            TaskName = "UserRankTask";
            Timing = "00:00:00";
        }

        protected override void DoExecute(object obj)
        {
            RankLv();
            RankObtion();
            RankCoin();
            RankCombatNum();
            new BaseLog().SaveLog("排行榜刷新");
        }


        /// <summary>
        /// 等级排行榜
        /// </summary>
        public static void RankLv()
        {
            CacheRank cacheRank = new CacheRank();
            cacheRank.Load(int.MaxValue);
            List<UserRank> ranksArray = cacheRank.GetUserRankList();
            ranksArray.QuickSort((x, y) =>
            {
                int result;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                result = (int)y.UserLv.CompareTo(x.UserLv);
                if (result == 0)
                {
                    UserGeneral generalx = UserGeneral.GetMainGeneral(x.UserID);
                    UserGeneral generaly = UserGeneral.GetMainGeneral(y.UserID);
                    if (generalx != null && generaly != null)
                    {
                        result =
                            generaly.CurrExperience.CompareTo(
                                generalx.CurrExperience);
                    }
                }
                return result;
            });
        }

        /// <summary>
        /// 声望排行榜
        /// </summary>
        public static void RankObtion()
        {
            string noticeContent = string.Empty;
            CacheRank cacheRank = new CacheRank();
            var ranksArray = cacheRank.GetUserRankList();
            ranksArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return y.ObtainNum.CompareTo(x.ObtainNum);
            });
        }

        /// <summary>
        /// 财富排行榜
        /// </summary>
        public static void RankCoin()
        {
            string noticeContent = string.Empty;
            CacheRank cacheRank = new CacheRank();
            var ranksArray = cacheRank.GetUserRankList();
            ranksArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return y.GameCoin.CompareTo(x.GameCoin);
            });
        }

        /// <summary>
        /// 战力排行榜
        /// </summary>
        public static void RankCombatNum()
        {
            CacheRank cacheRank = new CacheRank();
            var ranksArray = cacheRank.GetUserRankList();
            ranksArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return y.TotalCombatNum.CompareTo(x.TotalCombatNum);
            });
        }
    }
}