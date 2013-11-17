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
using System.Text;

namespace ZyGames.Tianjiexing.Model
{
    public enum ServerEnvKey
    {
        /// <summary>
        /// 竞技场连胜第一
        /// </summary>
        SportFirstUser = 1000,
        SportFirstWinNum,
        /// <summary>
        /// 竞技场排名奖励
        /// </summary>
        JingJiChangReward,
        /// <summary>
        /// 时间boss被杀时间
        /// </summary>
        BossKillDate,
        /// <summary>
        /// 活动基本时间
        /// </summary>
        FastBaseTime = 20001,
        /// <summary>
        /// 活动开始时间
        /// </summary>
        FastBeginTime = 20002,
        /// <summary>
        /// 活动ID
        /// </summary>
        FastID = 20003,
        /// <summary>
        /// 当前阶段
        /// </summary>
        FastStage = 20004,
        /// <summary>
        /// 当前轮次
        /// </summary>
        FastStageRound = 20005,
        /// <summary>
        /// 活动时间
        /// </summary>
        FastDate = 20006,
        /// <summary>
        /// 活动间隔时间
        /// </summary>
        FastIntervalTimeServer = 20007,
        /// <summary>
        /// 活动间隔时间
        /// </summary>
        FastIntervalTimeFianl = 20008,
        /// <summary>
        /// 活动奖励发放时间
        /// </summary>
        FastPrizeTime = 20009,
        /// <summary>
        /// 数据同步时间
        /// </summary>
        FastSyncDateTime = 20010,
        /// <summary>
        /// 参加服
        /// </summary>
        FastCombatServer = 20011,
        /// <summary>
        /// 参加服名称
        /// </summary>
        FastCombatServerName = 20012,
        /// <summary>
        /// 天榜冠军
        /// </summary>
        FastChiomption = 20013,
        /// <summary>
        /// 是否下发奖励
        /// </summary>
        FastPrizeGetTime = 20014,
        /// <summary>
        /// 人榜冠军
        /// </summary>
        FastChimption = 20015,

        /// <summary>
        /// 每天同步数据是否成功
        /// </summary>
        FastSyncResult = 20016,
        /// <summary>
        /// 每天同步数据发的时间
        /// </summary>
        FastEveryDaySync = 20017,
        /// <summary>
        /// 玩家宝藏
        /// </summary>
        UserTreasure = 12003,

        /// <summary>
        /// 公会城市争斗战开始时间
        /// </summary>
        FightStartDate = 64010,

        /// <summary>
        /// 第几届
        /// </summary>
        FirstSessions = 64011,

        /// <summary>
        /// 当前阶段
        /// </summary>
        FirstStage = 64012,
        /// <summary>
        /// 当前轮次
        /// </summary>
        FirstStageRound = 64013,

        /// <summary>
        /// 争夺战是否下发奖励
        /// </summary>
        IsGuildPrize = 64014,

        /// <summary>
        /// 争夺战奖励下发时间
        /// </summary>
        GuildPrizeTime = 64015,

        /// <summary>
        /// 冠军
        /// </summary>
        Champion = 64016,

        /// <summary>
        /// 本届公会战结束时间
        /// </summary>
        GuildEndTime = 64017,
        /// <summary>
        /// 击杀boss玩家
        /// </summary>
        KillBossUserID = 64018,

        /// <summary>
        /// 上期boss伤害排行
        /// </summary>
        FirstHalfBoss = 64019,
    }
}