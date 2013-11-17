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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    public class FightCombat
    {
        /// <summary>
        /// 第几届公会争斗战
        /// </summary>
        public int FastID
        {
            get { return ServerEnvSet.GetInt((ServerEnvKey)64011, 1); }
            set { ServerEnvSet.Set((ServerEnvKey)64011, value); }
        }

        /// <summary>
        /// 当前阶段
        /// </summary>
        public int CurStage
        {
            get { return ServerEnvSet.GetInt(ServerEnvKey.FirstStage, 0); }
            set { ServerEnvSet.Set(ServerEnvKey.FirstStage, value); }
        }

        /// <summary>
        /// 当前轮次
        /// </summary>
        public int Round
        {
            get { return ServerEnvSet.GetInt((ServerEnvKey)64013, 1); }
            set { ServerEnvSet.Set((ServerEnvKey)64013, value); }
        }

        /// <summary>
        /// 是否下发奖励
        /// </summary>
        public int IsGuildPrize
        {
            get { return ServerEnvSet.Get((ServerEnvKey)64014, 0).ToInt(); }
            set { ServerEnvSet.Set((ServerEnvKey)64014, value); }
        }

        /// <summary>
        /// 最终奖励发放时间
        /// </summary>
        public static DateTime GuildPrizeTime
        {
            get { return ServerEnvSet.Get(ServerEnvKey.GuildPrizeTime, MathUtils.SqlMinDate).ToDateTime(); }
            set { ServerEnvSet.Set(ServerEnvKey.GuildPrizeTime, value); }
        }

        /// <summary>
        /// 冠军
        /// </summary>
        public string Champion
        {
            get { return ServerEnvSet.Get((ServerEnvKey)64016, string.Empty); }
            set { ServerEnvSet.Set((ServerEnvKey)64016, value); }
        }

        /// <summary>
        /// 公会战结束时间
        /// </summary>
        public DateTime GuildEndTime
        {
            get { return ServerEnvSet.Get(ServerEnvKey.GuildEndTime, DateTime.Now).ToDateTime(); }
            set { ServerEnvSet.Set(ServerEnvKey.GuildEndTime, value); }
        }

        /// <summary>
        /// 公会战开始时间
        /// </summary>
        public DateTime GuildStartTime
        {
            get { return ServerEnvSet.Get(ServerEnvKey.FightStartDate, DateTime.Now).ToDateTime(); }
            set { ServerEnvSet.Set(ServerEnvKey.FightStartDate, value); }
        }

        /// <summary>
        /// 剩下人数
        /// </summary>
        public int Lenght
        {
            get
            {
                switch (CurStage.ToEnum<FightStage>())
                {
                    case FightStage.Close:
                    case FightStage.Apply:
                    case FightStage.Ready:
                    case FightStage.Wait:
                        return 8;
                    case FightStage.quarter_final:
                    case FightStage.semi_Wait:
                        return 4;
                    case FightStage.semi_final:
                    case FightStage.final_Wait:
                        return 2;
                    case FightStage.final:
                        return 1;
                    default:
                        return 0;
                }
            }
        }
    }
}