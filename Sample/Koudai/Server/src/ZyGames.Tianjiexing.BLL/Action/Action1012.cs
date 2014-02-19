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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1012_界面消息通知接口
    /// </summary>
    public class Action1012 : BaseAction
    {
        private enum MessageState
        {
            /// <summary>
            /// 聊天
            /// </summary>
            Chat = 1,
            /// <summary>
            /// 在线精力奖励
            /// </summary>
            TakeOnline,
            /// <summary>
            /// 前3天登录奖励
            /// </summary>
            TakeLogin,
            /// <summary>
            /// 前10天阅历声望
            /// </summary>
            TakeDailyExp,
            /// <summary>
            /// 系统广播
            /// </summary>
            Broadcast,
            /// <summary>
            /// 玩家修炼状态
            /// </summary>
            XiuLian,
            /// <summary>
            /// 竞技场奖励
            /// </summary>
            JingJiChangReward,

            /// <summary>
            /// 领取俸禄
            /// </summary>
            LingQuFengLv,

            /// <summary>
            /// 每日领取精力
            /// </summary>
            SurplusEnergy,

            /// <summary>
            /// 活动开启通知
            /// </summary>
            Huodong,

            /// <summary>
            /// 活动精力
            /// </summary>
            FestivalEnergy,

            /// <summary>
            /// 七夕翅膀
            /// </summary>
            Wing,
            /// <summary>
            /// 补尝奖励领取
            /// </summary>
            TaskPrize,
            /// <summary>
            /// 公会晨练
            /// </summary>
            GuildExercise,
            /// <summary>
            /// 15 公会城市争斗战
            /// </summary>
            GuildFight,
        }
        private List<MessageState> _statusList = new List<MessageState>();
        private int stakecount = 0;

        public Action1012(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1012, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_statusList.Count);
            foreach (var msgType in _statusList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)msgType);

                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            UserFunction userFunction = new GameDataCacheSet<UserFunction>().FindKey(ContextUser.UserID, FunctionEnum.Xiulian);
            if (userFunction != null)
            {
                UserHelper.XiuLianGianExperience(ContextUser.UserID); //修炼完成后更改修炼状态

                int inerDate = (int)(DateTime.Now - ContextUser.OnlineDate).TotalSeconds;
                if (inerDate > 300 && ContextUser.UserStatus != UserStatus.XiuLian)
                {
                    int totalTime = 0;
                    GeneralPracticeInfo generalpractice = new ConfigCacheSet<GeneralPracticeInfo>().FindKey(ContextUser.UserLv);
                    if (VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.XiuLianYanChangErShiSiXiaoShi))
                    {
                        totalTime = ConfigEnvSet.GetInt("User.XiuLianDate");
                    }
                    else
                    {
                        if (generalpractice != null)
                        {
                            totalTime = (generalpractice.MaxHour * 60 * 60);
                        }
                    }

                    if (inerDate > 300 && ContextUser.UserStatus != UserStatus.XiuLian && ContextUser.UserStatus != UserStatus.FightCombat)
                    {
                        ContextUser.UserStatus = UserStatus.XiuLian;
                        //ContextUser.Update();

                        List<UserQueue> userQueueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.XiuLian);
                        if (userQueueArray.Count > 0)
                        {
                            UserQueue userQueue = userQueueArray[0];
                            userQueue.TotalColdTime = totalTime;
                            userQueue.ColdTime = totalTime;
                            userQueue.Timing = DateTime.Now;
                            userQueue.IsSuspend = false;
                            userQueue.StrengNum = 0;
                            //userQueue.Update();
                        }
                        _statusList.Add(MessageState.XiuLian);
                    }
                }
            }

            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);

            if (new TjxChatService(ContextUser).HasMessage(ContextUser.ChatVesion))
            {
                _statusList.Add(MessageState.Chat);
            }

            if (NoviceHelper.CheckOnlinePrize(Uid))
            {
                _statusList.Add(MessageState.TakeOnline);
            }

            if (NoviceHelper.CheckLoginPrize(Uid))
            {
                _statusList.Add(MessageState.TakeLogin);
            }
            if (new TjxBroadcastService(ContextUser).HasMessage())
            {
                _statusList.Add(MessageState.Broadcast);
            }

            if (NoviceHelper.CheckDailyExpPrize(Uid))
            {
                _statusList.Add(MessageState.TakeDailyExp);
            }

            if (RankingHelper.IsGainSportsReward(ContextUser.UserID))
            {
                _statusList.Add(MessageState.JingJiChangReward);
            }

            UserFunction countryFunction = new GameDataCacheSet<UserFunction>().FindKey(ContextUser.UserID, FunctionEnum.Country);
            if (countryFunction != null && dailyRestrain != null && dailyRestrain.Funtion8 < VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.LingQuFengLu))
            {
                _statusList.Add(MessageState.LingQuFengLv);
            }

            if (NoviceHelper.DailyEnergy(ContextUser.UserID))
            {
                _statusList.Add(MessageState.SurplusEnergy);
            }
            if (new GameActiveCenter(Uid).HasActive())
            {
                _statusList.Add(MessageState.Huodong);
            }
            if (NoviceHelper.IsAugustSecondWeekEnergy(ContextUser))
            {
                _statusList.Add(MessageState.FestivalEnergy);
            }
            if (NoviceHelper.IsWingFestivalInfo(ContextUser.UserID))
            {
                _statusList.Add(MessageState.Wing);
            }

            if (GuildFightBroadcast(ContextUser.UserID, stakecount, ContextUser.MercenariesID))
            {
                _statusList.Add(MessageState.GuildFight);
            }
            else
            {
                stakecount = 0;
            }

            var userPrizeList = new ShareCacheStruct<UserTakePrize>().FindAll(m => !m.IsTasked && m.UserID == Uid.ToInt());
            if (userPrizeList.Count > 0)
            {
                _statusList.Add(MessageState.TaskPrize);
            }
            return true;
        }

        public static bool GuildFightBroadcast(string userID, int fightcount, string guildiD)
        {
            DateTime endDate;
            DateTime begintime = GuildFightCombat.FightCombatStartDate(out endDate);
            DateTime applyEnd = begintime.AddMinutes(-GameConfigSet.BattleBroadcast);
            if (DateTime.Now > applyEnd && DateTime.Now < begintime && fightcount <= 3)
            {
                FightCombat combat = new FightCombat();
                var fightList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == combat.FastID && s.Stage > 0);
                var serverFight = new ShareCacheStruct<ServerFight>().FindKey(combat.FastID, guildiD);
                if (serverFight != null)
                {
                    var cityfightList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == combat.FastID && s.CityID == serverFight.CityID);
                    if (cityfightList.Count == 1)
                    {
                        return false;
                    }
                }
                foreach (var fight in fightList)
                {
                    var member = new ShareCacheStruct<GuildMember>().FindKey(fight.GuildID, userID);
                    if (member != null)
                    {
                        TraceLog.ReleaseWriteDebug("公会城市争斗战参战成员{0}", userID);
                        fightcount++;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}