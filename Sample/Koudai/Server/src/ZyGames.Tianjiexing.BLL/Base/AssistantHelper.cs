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
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class AssistantHelper
    {
        /// <summary>
        /// 小助手列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<HelperInfo> UserHelperList(GameUser user)
        {
            List<HelperInfo> helperInfosList = new List<HelperInfo>();
            var functionArray = new GameDataCacheSet<UserFunction>().FindAll(user.UserID);
            foreach (UserFunction function in functionArray)
            {
                if (function.FunEnum == FunctionEnum.Goujili)
                {
                    helperInfosList.Add(GetDailyEnergy(user, RestrainType.GouMaiJingLi));
                }
                if (function.FunEnum == FunctionEnum.Jingkuandong)
                {
                    helperInfosList.Add(GetDailyEnergy(user, RestrainType.WaJinKuang));
                }
                if (function.FunEnum == FunctionEnum.Gonghui)
                {
                    helperInfosList.Add(GetDailyPrayer(user));
                }
                if (function.FunEnum == FunctionEnum.Mingyunshuijing)
                {
                    helperInfosList.Add(GetDailyEnergy(user, RestrainType.MianFeiLieMing));
                }
                if (function.FunEnum == FunctionEnum.Jingjichang)
                {
                    helperInfosList.Add(GetDailyEnergy(user, RestrainType.JingJiChangTiaoZhan));
                }
                if (function.FunEnum == FunctionEnum.Meiritanxian)
                {
                    helperInfosList.Add(GetDailyExplore(user));
                }
                if (function.FunEnum == FunctionEnum.Zhongzhijiyangshu)
                {
                    helperInfosList.Add(GetDailyPlant(user));
                }
            }
            return helperInfosList;
        }

        /// <summary>
        /// 精力，金矿洞，免费猎命，竞技场挑战次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static HelperInfo GetDailyEnergy(GameUser user, RestrainType restrainType)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
            int maxNum = VipHelper.GetVipUseNum(user.VipLv, restrainType);
            int currPayNum = 0;
            if (dailyRestrain != null && DateTime.Now.Date == dailyRestrain.RefreshDate.Date)
            {
                if (restrainType == RestrainType.GouMaiJingLi)
                {
                    currPayNum = MathUtils.Subtraction(maxNum, dailyRestrain.Funtion4, 0);
                }
                else if (restrainType == RestrainType.WaJinKuang)
                {
                    currPayNum = MathUtils.Subtraction(maxNum, dailyRestrain.Funtion3, 0);
                }
                else if (restrainType == RestrainType.MianFeiLieMing)
                {
                    currPayNum = MathUtils.Subtraction(maxNum, dailyRestrain.Funtion2, 0);
                }
                else if (restrainType == RestrainType.JingJiChangTiaoZhan)
                {
                    UserChallengeNum userChallenge = new GameDataCacheSet<UserChallengeNum>().FindKey(user.UserID);
                    if (userChallenge != null && DateTime.Now.Date == userChallenge.InsertDate.Date)
                    {
                        //当日总挑战次数
                        currPayNum = MathUtils.Addition(maxNum, userChallenge.ChallengeNum, int.MaxValue);
                        currPayNum = MathUtils.Subtraction(currPayNum, dailyRestrain.Funtion9, 0);
                    }
                    else
                    {
                        currPayNum = MathUtils.Subtraction(maxNum, dailyRestrain.Funtion9, 0);
                    }
                }

            }
            else
            {
                currPayNum = maxNum;
            }
            //if (restrainType == RestrainType.JingJiChangTiaoZhan)
            //{
            //    UserChallengeNum userChallenge = new GameDataCacheSet<UserChallengeNum>().FindKey(user.UserID);
            //    if (userChallenge != null && DateTime.Now.Date == userChallenge.InsertDate.Date)
            //    {
            //        currPayNum = currPayNum.Addition(userChallenge.ChallengeNum, int.MaxValue); //当日总挑战次数
            //        currPayNum = maxNum.Subtraction(currPayNum, 0);
            //    }
            //}
            return new HelperInfo() { Type = restrainType, SurplusNum = currPayNum, TotalNum = maxNum };
        }

        /// <summary>
        /// 公会祈祷
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static HelperInfo GetDailyPrayer(GameUser user)
        {
            int currPayNum = 1;
            int maxNum = 0;
            if (!string.IsNullOrEmpty(user.MercenariesID))
            {
                maxNum = 1;
                GuildMemberLog memberLog = new ShareCacheStruct<GuildMemberLog>().FindKey(user.MercenariesID);
                if (memberLog != null)
                {
                    List<MemberLog> guildArray = memberLog.GetLog(m => m.LogType == 2 && m.UserID.Equals(user.UserID));

                    foreach (MemberLog guildLog in guildArray)
                    {
                        if (DateTime.Now.Date == guildLog.InsertDate.Date)
                        {
                            currPayNum = MathUtils.Subtraction(currPayNum, 1, 0);
                            break;
                        }
                    }
                }
            }
            else
            {
                currPayNum = 0;
                maxNum = 0;
            }

            return new HelperInfo() { Type = RestrainType.Prayer, SurplusNum = currPayNum, TotalNum = maxNum };
        }

        /// <summary>
        /// 每日探险次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static HelperInfo GetDailyExplore(GameUser user)
        {
            int currPayNum = 0;
            int maxNum = 10;
            UserExpedition userExp = new GameDataCacheSet<UserExpedition>().FindKey(user.UserID);

            if (userExp != null && DateTime.Now.Date == userExp.InsertDate.Date)
            {
                currPayNum = MathUtils.Subtraction(maxNum, userExp.ExpeditionNum, 0);
            }
            else
            {
                currPayNum = maxNum;
            }
            return new HelperInfo() { Type = RestrainType.Explore, SurplusNum = currPayNum, TotalNum = maxNum };
        }

        /// <summary>
        /// 每日种植
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static HelperInfo GetDailyPlant(GameUser user)
        {
            int currPayNum = 0;
            int maxNum = 0;
            var landArray = new GameDataCacheSet<UserLand>().FindAll(user.UserID, u => u.IsGain == 1 || u.DoRefresh() > 0);
            UserPlant plant = new GameDataCacheSet<UserPlant>().FindKey(user.UserID);
            if (plant != null)
            {
                maxNum = plant.LandNum;
                currPayNum = MathUtils.Subtraction(maxNum, landArray.Count, 0);
            }
            return new HelperInfo() { Type = RestrainType.Plant, SurplusNum = currPayNum, TotalNum = maxNum };
        }
    }
}