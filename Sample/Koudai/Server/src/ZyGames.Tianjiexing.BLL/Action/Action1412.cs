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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1412_佣兵修炼获得经验接口
    /// </summary>
    public class Action1412 : BaseAction
    {
        private int experience = 0;
        private int fightingNum = 0;
        private int practiceTime = 0;
        private int currExperience = 0;

        public Action1412(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1412, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(experience);
            PushIntoStack(currExperience);
            this.PushIntoStack(fightingNum);
            this.PushIntoStack(practiceTime);

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            int exp = 0;
            short currMaxLv = ConfigEnvSet.GetInt("User.CurrMaxLv").ToShort(); //玩家最大等级
            UserGeneral userGeneral = UserGeneral.GetMainGeneral(ContextUser.UserID);
            int genLv = 0;
            if (ContextUser.UserLv > currMaxLv)
            {
                genLv = currMaxLv.ToInt();
            }
            else
            {
                genLv = ContextUser.UserLv;
            }
            GeneralPracticeInfo generalpractice = new ConfigCacheSet<GeneralPracticeInfo>().FindKey(genLv);
            var ranking = RankingFactory.Get<UserRank>(CombatNumRanking.RankingKey);
            UserRank rankInfo = ranking.Find(m => m.UserID == ContextUser.UserID);
            if (rankInfo != null && rankInfo.TotalCombatNum > 0)
            {
                fightingNum = rankInfo.TotalCombatNum;
            }
            else
            {
                //fightingNum = UserHelper.GetGameUserCombat(ContextUser.UserID);
            }

            int totalTime = 0;
            if (generalpractice == null)
            {
                return false;
            }
            totalTime = (generalpractice.MaxHour * 60 * 60);
            List<UserQueue> userQueueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.XiuLian);
            if (userQueueArray.Count > 0 && !userQueueArray[0].IsSuspend)
            {
                UserQueue queueInfo = userQueueArray[0];
                TimeSpan ts = DateTime.Now - queueInfo.Timing;
                //practiceTime = (int)ts.TotalSeconds;
                int preExpTime = 0;
                practiceTime = queueInfo.DoRefresh();
                if (practiceTime > 0)
                {
                    if (userQueueArray[0].Timing <= DateTime.Now)
                    {
                        preExpTime = ((int)((DateTime.Now - userQueueArray[0].Timing).TotalSeconds) /
                                      generalpractice.IntervalTime);
                        experience = generalpractice.Exprience * preExpTime;
                        if (experience > 0)
                        {
                            exp = generalpractice.Exprience;
                        }
                    }
                }
                currExperience = MathUtils.Addition(userGeneral.CurrExperience, exp, int.MaxValue);
                queueInfo.StrengNum = MathUtils.Addition(queueInfo.StrengNum, exp, int.MaxValue);
                //queueInfo.Update();

                userGeneral.CurrExperience = currExperience;
                //userGeneral.Update();
            }
            return true;
        }
    }
}