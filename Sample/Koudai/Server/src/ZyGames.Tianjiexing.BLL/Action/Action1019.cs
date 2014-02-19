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
using System.Web.Caching;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.SyncThreading;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Com;
using ZyGames.Tianjiexing.Component;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1019_排行榜列表接口
    /// </summary>
    public class Action1019 : BaseAction
    {
        private RankType rankType;
        private int pageIndex = 0;
        private int pageSize = 0;
        private int pageCount = 0;
        private int baseNum;
        private IList<UserRank> userRankArray;
        private int currRankID = 0;

        public Action1019(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1019, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(userRankArray.Count);
            foreach (UserRank userInfo in userRankArray)
            {
                if (rankType == RankType.UserLv)
                {
                    baseNum = userInfo.UserLv;
                }
                else if (rankType == RankType.Obtion)
                {
                    baseNum = userInfo.ObtainNum;
                }
                else if (rankType == RankType.GameCoin)
                {
                    baseNum = userInfo.GameCoin;
                }
                else if (rankType == RankType.ZhanLi)
                {
                    baseNum = userInfo.TotalCombatNum;
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(userInfo.RankId);
                dsItem.PushIntoStack(userInfo.UserID.ToNotNullString());
                dsItem.PushIntoStack(userInfo.NickName.ToNotNullString());
                dsItem.PushIntoStack(baseNum);
                dsItem.PushIntoStack((short)userInfo.CountryID);
                dsItem.PushIntoStack((short)userInfo.VipLv);
                PushIntoStack(dsItem);
            }
            PushIntoStack(currRankID);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("RankType", ref rankType)
                 && httpGet.GetInt("PageIndex", ref pageIndex)
                 && httpGet.GetInt("PageSize", ref pageSize))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
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
            userRankArray = rankList.GetRange(pageIndex, pageSize, out pageCount);
            if (rankList.TryGetRankNo(m => m.UserID == ContextUser.UserID, out currRankID))
            {

            }
            return true;
        }
    }
}