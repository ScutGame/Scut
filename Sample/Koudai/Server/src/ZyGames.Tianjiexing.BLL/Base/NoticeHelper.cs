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
using System.Web;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Tianjiexing.BLL.Action;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using System.Web.Caching;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Com;
using ZyGames.Tianjiexing.Component;


namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 公告
    /// </summary>
    public class NoticeHelper
    {
        private const int OrderCount = 10;
        /// <summary>
        /// 排行榜公告
        /// </summary>
        public static void RankNotice(GameUser user)
        {
            RankLvNotice(user);
            RankObtionNotice(user);
            RankCoinNotice(user);
            RankCombatNumNotice(user);
        }

        public static Ranking<UserRank> GetRankType(RankType rankType)
        {
            Ranking<UserRank> rankList;
            if (rankType == RankType.UserLv)
            {
                rankList = RankingFactory.Get<UserRank>(UserLvRanking.RankingKey);
            }
            else if (rankType == RankType.Obtion)
            {
                rankList = RankingFactory.Get<UserRank>(ObtainRanking.RankingKey);
            }
            else if (rankType == RankType.GameCoin)
            {
                rankList = RankingFactory.Get<UserRank>(GameCoinRanking.RankingKey);
            }
            else if (rankType == RankType.ZhanLi)
            {
                rankList = RankingFactory.Get<UserRank>(CombatNumRanking.RankingKey);
            }
            else
            {
                throw new ArgumentNullException("rankType", string.Format("rankType:{0}", rankType));
            }
            return rankList;
        }

        /// <summary>
        /// 等级第一公告
        /// </summary>

        public static void RankLvNotice(GameUser user)
        {
            var rankList = GetRankType(RankType.UserLv);
            int rankNo = rankList.GetRankNo(m => m.UserID == user.UserID);
            if (rankNo == 1)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_LvTopTenNotice, user.NickName));
            }
        }

        /// <summary>
        /// 声望公告
        /// </summary>
        public static void RankObtionNotice(GameUser user)
        {
            var rankList = GetRankType(RankType.Obtion);
            int rankNo = rankList.GetRankNo(m => m.UserID == user.UserID);
            if (rankNo == 1)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_ObtionNumNotice, user.NickName));
            }
            else if (rankNo > 1 && rankNo < 4)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_ObtionTopThreeNotice, user.NickName));
            }
        }

        /// <summary>
        /// 财富公告
        /// </summary>
        public static void RankCoinNotice(GameUser user)
        {
            var rankList = GetRankType(RankType.GameCoin);
            int rankNo = rankList.GetRankNo(m => m.UserID == user.UserID);
            if (rankNo == 1)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_GameCoinTopOneNotice, user.NickName));
            }
            else if (rankNo > 1 && rankNo < 4)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_GameCoinThreeNotice, user.NickName));
            }
            else if (rankNo > 3 && rankNo < 11)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_GameCoinTopTenNotice, user.NickName));
            }
        }

        /// <summary>
        /// 战力公告
        /// </summary>
        public static void RankCombatNumNotice(GameUser user)
        {
            var rankList = GetRankType(RankType.ZhanLi);
            int rankNo = rankList.GetRankNo(m => m.UserID == user.UserID);
            if (rankNo == 1)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_CombatNumTopOneNotice, user.NickName));
            }
            else if (rankNo > 1 && rankNo < 4)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_CombatNumTopThreeNotice, user.NickName));
            }
            else if (rankNo > 3 && rankNo < 11)
            {
                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St_CombatNumTopTenNotice, user.NickName));
            }           
        }
    }
}