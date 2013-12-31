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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 魂技类
    /// </summary>
    public class GeneralEscalateHelper
    {
        public static GameDataCacheSet<UserAbility> _cacheSetAbility = new GameDataCacheSet<UserAbility>();
        public static GameDataCacheSet<UserFunction> _cacheSetUserFun = new GameDataCacheSet<UserFunction>();
        /// <summary>
        /// 玩家荣誉值升级
        /// </summary>
        /// <param name="user"></param>
        /// <param name="honourNum"></param>
        public static void AddUserLv(GameUser user, int honourNum)
        {
            short generalMaxLv = GameConfigSet.CurrMaxLv.ToShort();
            if (user.UserLv >= generalMaxLv)
            {
                CheckFun(user);
                return;
            }
            short rankLv = user.UserLv;
            user.HonourNum = MathUtils.Addition(user.HonourNum, honourNum);
            short nextLv = MathUtils.Addition(user.UserLv, 1.ToShort());
            while (nextLv <= generalMaxLv)
            {
                GeneralEscalateInfo generalEscalate = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(nextLv, GeneralType.YongHu);
                if (generalEscalate != null && user.HonourNum >= generalEscalate.UpExperience)
                {
                    user.UserLv = nextLv;
                    user.IsLv = true;
                    UserHelper.OpenMagic(user.UserID, user.UserLv);
                    user.HonourNum = MathUtils.Subtraction(user.HonourNum, generalEscalate.UpExperience);

                    if (generalEscalate.FunctionEnum != null && generalEscalate.FunctionEnum != "")
                    {
                        var feArray = generalEscalate.FunctionEnum.Split(',');
                        foreach (var fun in feArray)
                        {
                            var userFun = _cacheSetUserFun.FindKey(user.UserID, fun);
                            if (userFun == null)
                            {
                                userFun = new UserFunction();
                                userFun.UserID = user.UserID;
                                userFun.FunEnum = fun.ToEnum<FunctionEnum>();
                                userFun.CreateDate = DateTime.Now;
                                _cacheSetUserFun.Add(userFun);
                                _cacheSetUserFun.Update();
                                user.OpenFun.Add(userFun);
                            }
                        }

                    }
                    FestivalHelper.GetUpgradeGiveGift(user.UserID, nextLv);
                    nextLv = MathUtils.Addition(user.UserLv, 1.ToShort());
                }
                else
                {
                    break;
                }
            }
            if (user.UserLv > rankLv)
            {
                Ranking<UserRank> ranking = RankingFactory.Get<UserRank>(CombatRanking.RankingKey);
                UserRank rankInfo;
                int rankID;
                if (ranking.TryGetRankNo(m => m.UserID == user.UserID, out rankID))
                {
                    rankInfo = ranking.Find(s => s.UserID == user.UserID);
                    if (rankInfo != null)
                    {
                        rankInfo.UserLv = user.UserLv;
                        rankInfo.TotalCombatNum = user.CombatNum;
                        rankInfo.ObtainNum = user.ObtainNum;
                    }
                }
            }
            CheckFun(user);
        }

        public static void CheckFun(GameUser user)
        {
            var generalEscalate = new ConfigCacheSet<GeneralEscalateInfo>().FindAll(s => s.GeneralLv <= user.UserLv && s.GeneralType == GeneralType.YongHu && s.FunctionEnum != "0" && s.FunctionEnum != "");
            foreach (var generalEscalateInfo in generalEscalate)
            {
                var feArray = generalEscalateInfo.FunctionEnum.Split(',');
                foreach (var fun in feArray)
                {
                    var userFun = _cacheSetUserFun.FindKey(user.UserID, fun);
                    if (userFun == null)
                    {
                        userFun = new UserFunction();
                        userFun.UserID = user.UserID;
                        userFun.FunEnum = fun.ToEnum<FunctionEnum>();
                        userFun.CreateDate = DateTime.Now;
                        _cacheSetUserFun.Add(userFun);
                        _cacheSetUserFun.Update();
                       
                    }
                }


            }
        }
    }
}