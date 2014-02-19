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
    public class Action1503 : BaseAction
    {
        private int codeTime = 0;
        private int magicID = 0;
        private MagicInfo magicInfo = null;
        private int sumMagicLv = 0;
        private int experience = 0;
        private int guideID = 0;
        private List<UserMagic> userMagicArray = new List<UserMagic>();
        private List<MagicLvInfo> magicLvInfoArray = new List<MagicLvInfo>();
        private int ops = 0;

        public Action1503(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1503, httpGet)
        {

        }

        public override bool TakeAction()
        {
            //List<UserQueue> queueList = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.MagicStrong);
            //if (queueList.Count > 0)
            //{
            //    if (queueList[0].StrengNum >= 2 && queueList[0].DoRefresh() > 0)
            //    {
            //        ErrorCode = LanguageManager.GetLang().ErrorCode;
            //        ErrorInfo = LanguageManager.GetLang().St1503_MagicColding;
            //        return false;
            //    }
            //}
            magicInfo = new ConfigCacheSet<MagicInfo>().FindKey(magicID);
            if (magicInfo == null)
            {
                return false;
            }
            int maxMagicLv = ConfigEnvSet.GetInt("Queue.MaxLength");

            UserMagic userMagic = new GameDataCacheSet<UserMagic>().FindKey(ContextUser.UserID, magicID);
            if (userMagic == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1503_MagicIDNotEnough;
                return false;
            }
            sumMagicLv = MathUtils.Addition(userMagic.MagicLv, (short)1, (short)100);
            MagicLvInfo magicLvInfo = new ConfigCacheSet<MagicLvInfo>().FindKey(magicID, sumMagicLv);
            if (magicLvInfo == null || userMagic.MagicLv == maxMagicLv)
            {
                guideID = 1;
                //等级已达到最高
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1503_MaxMagicLv;
                return false;
            }
            if (ContextUser.UserLv < magicLvInfo.EscalateMinLv)
            {
                //超出用户等级
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1503_MagicLevel;
                return false;
            }


            experience = magicLvInfo.ExpNum;
            //codeTime = magicLvInfo.ColdTime;
            if (experience > ContextUser.ExpNum)
            {
                guideID = 3;
                //阅历不足
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1503_UpgradeExpNum);
                return false;
            }
            int useGold = 0;
            if (ops == 1)
            {
                if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.EquXiaoChuLengQueShiJian))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                    return false;
                }
                useGold = (codeTime / 60);
                if (ContextUser.GoldNum < useGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold, int.MaxValue);
            }
            else
            {
                //放入队列中
                // List<UserQueue> queueList = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType ==QueueType.MagicStrong);
                //todo
                //if (queueList.Count > 0)
                //{
                //    UserQueue queue = queueList[0];
                //    if (queue.DoRefresh() > 0 && queue.StrengNum < 2)
                //    {
                //        queue.TotalColdTime = MathUtils.Addition(queue.TotalColdTime, codeTime, int.MaxValue);
                //        queue.ColdTime = MathUtils.Addition(queue.ColdTime, codeTime, int.MaxValue);
                //        queue.IsSuspend = false;
                //        queue.StrengNum = MathUtils.Addition(queue.StrengNum, 1, int.MaxValue);
                //        //queue.Update();
                //    }
                //    else
                //    {
                //        queue.TotalColdTime = codeTime;
                //        queue.Timing = DateTime.Now;
                //        queue.ColdTime = codeTime;
                //        queue.IsSuspend = false;
                //        queue.StrengNum = 1;
                //        //queue.Update();
                //    }
                //}
                //else
                //{
                //    UserQueue userQueue = new UserQueue()
                //    {
                //        QueueID = Guid.NewGuid().ToString(),
                //        UserID = ContextUser.UserID,
                //        QueueType = QueueType.MagicStrong,
                //        QueueName = QueueType.MagicStrong.ToString(),
                //        TotalColdTime = codeTime,
                //        Timing = DateTime.Now,
                //        ColdTime = codeTime,
                //        StrengNum = 1,
                //        IsSuspend = false
                //    };
                //    new GameDataCacheSet<UserQueue>().Add(userQueue);
                //}
            }

            if (userMagic != null)
            {
                if (userMagic.MagicType == MagicType.MoFaZhen)
                {
                    MagicLvInfo mLvInfo = new ConfigCacheSet<MagicLvInfo>().FindKey(userMagic.MagicID, userMagic.MagicLv);
                    string[] mGridRange = mLvInfo.GridRange.Split(',');
                    foreach (string gridRange in mGridRange)
                    {
                        UserEmbattle userEmbattle = new GameDataCacheSet<UserEmbattle>().FindKey(ContextUser.UserID, userMagic.MagicID, gridRange.ToShort());
                        if (userEmbattle == null)
                        {
                            UserEmbattle embattle = new UserEmbattle()
                            {
                                UserID = ContextUser.UserID,
                                MagicID = userMagic.MagicID,
                                Position = gridRange.ToShort(),
                                GeneralID = 0
                            };
                            new GameDataCacheSet<UserEmbattle>().Add(embattle);
                        }
                    }
                }

                if (experience < ContextUser.ExpNum)
                {
                    ContextUser.ExpNum = MathUtils.Subtraction(ContextUser.ExpNum, experience, 0);
                    userMagic.MagicLv = MathUtils.Addition(userMagic.MagicLv, (short)1, (short)maxMagicLv);
                }
                UserLogHelper.AppenStrongLog(ContextUser.UserID, 1, null, magicID, 2, (short)userMagic.MagicLv, useGold, 0);
            }

            List<UserGeneral> userGeneralsArray = new GameDataCacheSet<UserGeneral>().FindAll(ContextUser.UserID, s => s.GeneralStatus == GeneralStatus.DuiWuZhong && s.GeneralType != GeneralType.Soul);
            foreach (UserGeneral general in userGeneralsArray)
            {
                general.RefreshMaxLife();
            }
            //日常任务-魔术升级
            TaskHelper.TriggerDailyTask(Uid, 4004);
            return true;
        }

        public override void BuildPacket()
        {
            PushIntoStack(codeTime);
            PushIntoStack(guideID);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("MagicID", ref magicID))
            {
                httpGet.GetInt("Ops", ref ops);
                return true;
            }
            return false;
        }
    }
}