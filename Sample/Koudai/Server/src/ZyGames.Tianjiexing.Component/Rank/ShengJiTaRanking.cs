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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.Component
{
    public delegate bool FilterGetter(GameUser gameUser);

    /// <summary>
    /// 圣吉塔玩家等级排行榜
    /// </summary>
    public class ShengJiTaRanking : Ranking<UserRank>
    {
        public const string RankingKey = "ShengJiTaRanking";
        private const int listcount = 60;

        public ShengJiTaRanking()
            : base(RankingKey, listcount)
        {
        }

        protected override int ComparerTo(UserRank x, UserRank y)
        {
            int result = 0;
            //if (x == null && y == null)
            //{
            //    return 0;
            //}
            //if (x != null && y == null)
            //{
            //    return 1;
            //}
            //if (x == null)
            //{
            //    return -1;
            //}
            //result = y.ScoreStar.CompareTo(x.ScoreStar);
            //if (result == 0)
            //{
            //    result = y.MaxTierNum.CompareTo(y.MaxTierNum);
            //    if(result == 0)
            //    {
            //        result = y.EndTime.Second.CompareTo(x.EndTime.Second);
            //    }
            //}
            return result;
        }

        private static void Rank(List<UserRank> rankingShengJiTa, List<UserRank> List)    //获取排行玩家的前20条玩家       
        {
            int index = 1;
            foreach (var item in List)
            {
                if (index > 20)
                {
                    break;
                }
                item.SJTRankId = index;
                rankingShengJiTa.Add(item);
                index++;
            }
        }
        private static void Sort(List<UserRank> List)        //对数据进行排序
        {
            List.QuickSort((x, y) =>
              {
                  int result = 0;
                  if (x == null && y == null) return 0;
                  if (x != null && y == null) return 1;
                  if (x == null) return -1;
                  result = ((int)y.ScoreStar).CompareTo((int)x.ScoreStar);
                  if (result == 0)
                  {
                      result = ((int)y.MaxTierNum).CompareTo((int)x.MaxTierNum);
                      if (result == 0)
                          result = (x.EndTime.Second).CompareTo(y.EndTime.Second);
                  }
                  return result;
              });
        }
        private static void RankUser(UserRank rankingUser, GameUser gameUser, UserShengJiTa user)        //数据添加到对象
        {
            rankingUser.UserID = user.UserID.ToString();
            rankingUser.NickName = gameUser.NickName;
            rankingUser.UserLv = gameUser.UserLv;
            rankingUser.MaxTierNum = user.MaxTierNum;
            rankingUser.ScoreStar = user.ScoreStar;
            rankingUser.HaveRankNum = user.HaveRankNum;
        }

        protected override IList<UserRank> GetCacheList()
        {
            // 圣吉塔排行
            UserShengJiTaHelper.UserShengJiTaRank();

            List<UserRank> rankingShengJiTa = new List<UserRank>();
            List<UserRank> List1 = new List<UserRank>();
            List<UserRank> List2 = new List<UserRank>();
            List<UserRank> List3 = new List<UserRank>();
            int LowMinLv = 10;
            int LowMaxLv = 29;
            int MidMinLv = 30;
            int MidMaxLv = 54;
            int highMinLv = 55;
            new GameDataCacheSet<UserShengJiTa>().Foreach((personalId, key, userShengJiTa) =>
            {

                if (userShengJiTa.ScoreStar > 0)
                {
                    var gameUser = new GameDataCacheSet<GameUser>().FindKey(userShengJiTa.UserID.ToString());
                    //获取圣吉塔10-29的玩家
                    DateTime nowTime = DateTime.Now;
                    if (userShengJiTa.HaveRankNum == 0) //第一次进入排行榜
                        userShengJiTa.HaveRankNum = 1;

                    if (userShengJiTa.HaveRankNum > 0 && (nowTime - userShengJiTa.RankTime).Days > 1)     //隔天未进入排行榜
                    {
                        userShengJiTa.HaveRankNum = 0;
                        userShengJiTa.IsYesterday = 0;
                    }
                    if (userShengJiTa.HaveRankNum > 0 && (nowTime - userShengJiTa.RankTime).Days == 1)     //隔天进入排行榜
                    {
                        userShengJiTa.HaveRankNum += 1;
                        userShengJiTa.IsYesterday = 1;
                    }
                    userShengJiTa.RankTime = nowTime;                                          //玩家进入排行榜的时间


                    if (nowTime.Date == userShengJiTa.EndTime.Date)
                    {
                        UserRank rankingUser = new UserRank();
                        if (gameUser.UserLv >= LowMinLv && gameUser.UserLv <= LowMaxLv)
                        {
                            RankUser(rankingUser, gameUser, userShengJiTa);
                            rankingUser.SJTRankType = SJTRankType.Bronze;
                            List1.Add(rankingUser);
                        }
                        if (gameUser.UserLv >= MidMinLv && gameUser.UserLv <= MidMaxLv)
                        {
                            RankUser(rankingUser, gameUser, userShengJiTa);
                            rankingUser.SJTRankType = SJTRankType.Silver;
                            List2.Add(rankingUser);
                        }
                        if (gameUser.UserLv >= highMinLv)
                        {
                            RankUser(rankingUser, gameUser, userShengJiTa);
                            rankingUser.SJTRankType = SJTRankType.Gold;
                            List3.Add(rankingUser);
                        }
                    }
                    else
                    {
                        userShengJiTa.IsTierNum = 0;
                        userShengJiTa.IsTierStar = 0;
                        userShengJiTa.BattleRount = 0;
                        userShengJiTa.MaxTierNum = 0;
                        userShengJiTa.ScoreStar = 0;
                        userShengJiTa.RoundPoor = 0;
                        userShengJiTa.LifeNum = 0;
                        userShengJiTa.WuLiNum = 0;
                        userShengJiTa.FunJiNum = 0;
                        userShengJiTa.MofaNum = 0;
                        userShengJiTa.FiveTierRewardList.Clear();
                        
                    }
                }
                return true;
            });
            Sort(List1);
            Rank(rankingShengJiTa, List1);
            Sort(List2);
            Rank(rankingShengJiTa, List2);
            Sort(List3);
            Rank(rankingShengJiTa, List3);
            return rankingShengJiTa;
        }
    }
}