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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1410_佣兵修炼接口
    /// </summary>
    public class Action1411 : BaseAction
    {
        private const int Interval = 1800; //1800; //上线满半小时
        private int ops = 0;
        private int experience = 0;
        private string descript = string.Empty;


        public Action1411(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1411, httpGet)
        {

        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            short currMaxLv = ConfigEnvSet.GetInt("User.CurrMaxLv").ToShort(); //玩家最大等级
            if (ContextUser.UserStatus == UserStatus.SaoDang)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4007_Saodanging;
                return false;
            }
            if (ops == 0 && ContextUser.UserStatus == UserStatus.XiuLian)
            {
                return false;
            }
            int totalTime = 0;
            int genlv = 0;
            if (ContextUser.UserLv > currMaxLv)
            {
                genlv = currMaxLv.ToInt();
            }
            else
            {
                genlv = ContextUser.UserLv;
            }
            GeneralPracticeInfo generalpractice = new ConfigCacheSet<GeneralPracticeInfo>().FindKey(genlv);
            if (generalpractice == null || generalpractice.IntervalTime == 0)
            {
                return false;
            }
            if (VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.XiuLianYanChangErShiSiXiaoShi))
            {
                totalTime = ConfigEnvSet.GetInt("User.XiuLianDate");
            }
            else
            {
                totalTime = (generalpractice.MaxHour * 60 * 60);
            }

            List<UserQueue> userQueueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.XiuLian);
            if (ops == 0)
            {
                this.ErrorCode = 0;
                UserQueue userQueue = null;
                if (userQueueArray.Count > 0)
                {
                    userQueue = userQueueArray[0];
                }
                if (ContextUser.UserStatus != UserStatus.XiuLian)
                {
                    ContextUser.UserStatus = UserStatus.XiuLian;
                    //ContextUser.Update();
                    if (userQueue == null)
                    {
                        userQueue = new UserQueue()
                        {
                            UserID = ContextUser.UserID,
                            QueueID = Guid.NewGuid().ToString(),
                            QueueType = QueueType.XiuLian,
                            QueueName = QueueType.XiuLian.ToString(),
                            TotalColdTime = totalTime,
                            Timing = DateTime.Now,
                            ColdTime = totalTime,
                            IsSuspend = false,
                            StrengNum = 0
                        };
                        new GameDataCacheSet<UserQueue>().Add(userQueue);
                    }
                    else
                    {
                        userQueue.TotalColdTime = totalTime;
                        userQueue.ColdTime = totalTime;
                        userQueue.Timing = DateTime.Now;
                        userQueue.StrengNum = 0;
                        userQueue.DoStart();
                    }
                }
            }
            else if (ops == 1)
            {
                ErrorCode = 1;
                if (ContextUser.UserStatus != UserStatus.Normal && ContextUser.UserStatus != UserStatus.FightCombat)
                {
                    ContextUser.UserStatus = UserStatus.Normal;
                    //ContextUser.Update();
                    if (userQueueArray.Count > 0)
                    {
                        UserQueue userQueue = userQueueArray[0];
                        userQueue.IsSuspend = true;
                        //userQueue.Update();
                    }
                    List<UserMagic> userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(ContextUser.UserID, u => u.IsEnabled && u.MagicType == MagicType.MoFaZhen);
                    if (userMagicArray.Count > 0)
                    {
                        List<UserEmbattle> userEmbattleArray = new GameDataCacheSet<UserEmbattle>().FindAll(ContextUser.UserID, m => m.MagicID == userMagicArray[0].MagicID);
                        int practiceTime = 0; //修炼时间
                        if (userQueueArray.Count > 0)
                        {
                            UserQueue queueInfo = userQueueArray[0];
                            TimeSpan ts = DateTime.Now - queueInfo.Timing;
                            practiceTime = (int)ts.TotalSeconds;
                            if (practiceTime <= totalTime && queueInfo.Timing <= DateTime.Now)
                            {
                                experience = ((practiceTime / generalpractice.IntervalTime) * generalpractice.Exprience);
                            }
                            foreach (UserEmbattle embattle in userEmbattleArray)
                            {
                                UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, embattle.GeneralID);
                                if (general != null)
                                {
                                    general.Experience1 = MathUtils.Addition(general.Experience1, experience, int.MaxValue);
                                    if (general.GeneralLv >= UserHelper._currMaxLv)
                                    {
                                        continue;
                                    }
                                    if (general.GeneralID == LanguageManager.GetLang().GameUserGeneralID)
                                    {
                                        general.CurrExperience = MathUtils.Addition(general.CurrExperience, MathUtils.Subtraction(experience, queueInfo.StrengNum, 0), int.MaxValue);
                                    }
                                    else
                                    {
                                        general.CurrExperience = MathUtils.Addition(general.CurrExperience, experience, int.MaxValue);
                                    }

                                    //general.Update();
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}