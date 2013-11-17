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
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model.DataModel
{
    [Serializable, ProtoContract]
    public class ServerCombatInfo
    {
        /// <summary>
        /// 基准时间
        /// </summary>
        public DateTime BaseTime
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20001, Convert.ToDateTime("2012-10-1")).ToDateTime(); }
        }
        /// <summary>
        /// 当前活动开始时间
        /// </summary>
        public DateTime BeginTime
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20002, Convert.ToDateTime("2012-10-1")).ToDateTime(); }
            set { ServerEnvSet.Set((ServerEnvKey)20002, value); }
        }
        /// <summary>
        /// 当前活动ID
        /// </summary>
        public int FastID
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20003, 0).ToInt(); }
            set { ServerEnvSet.Set((ServerEnvKey)20003, value); }
        }
        /// <summary>
        /// 当前阶段
        /// </summary>
        public ServerCombatStage CurStage
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20004, 0).ToEnum<ServerCombatStage>(); }
            set { ServerEnvSet.Set((ServerEnvKey)20004, value); }
        }

        /// <summary>
        /// 当前轮次
        /// </summary>
        public int Round
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20005, 0).ToInt(); }
            set { ServerEnvSet.Set((ServerEnvKey)20005, value); }
        }

        /// <summary>
        /// 战斗开始时间
        /// </summary>
        public static DateTime CombatBeginTime
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20006, "12:00").ToDateTime(); }
        }

        /// <summary>
        /// 最终奖励发放时间
        /// </summary>
        public static DateTime PutPrizeTime
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20009, "18:00").ToDateTime(); }
        }

        /// <summary>
        /// 参加服
        /// </summary>
        public string[] TakeServer
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20011, string.Empty).Split(','); }
        }

        public string[] TakeServerName
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20012, string.Empty).Split(','); }
        }

        /// <summary>
        /// 天榜冠军
        /// </summary>
        public string Champion1
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20013, string.Empty); }
            set { ServerEnvSet.Set((ServerEnvKey)20013, value); }
        }

        /// <summary>
        /// 人榜冠军
        /// </summary>
        public string Champion2
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20015, string.Empty); }
            set { ServerEnvSet.Set((ServerEnvKey)20015, value); }
        }

        /// <summary>
        /// 是否下发奖励
        /// </summary>
        public int IsPrize
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20014, 0).ToInt(); }
            set { ServerEnvSet.Set((ServerEnvKey)20014, value); }
        }

        public string[] FastSyncResult
        {
            get { return ServerEnvSet.Get((ServerEnvKey)20016, string.Empty).Split(','); }
            set
            {
                //string[] strArr = value as string[];
                ServerEnvSet.Set((ServerEnvKey)20016, string.Join(",", value as string[]));
            }
        }

        /// <summary>
        /// 间隔时间
        /// </summary>
        public int interval
        {
            get
            {
                switch (CurStage)
                {
                    case ServerCombatStage.Close:
                    case ServerCombatStage.Apply:
                        return 0;
                    case ServerCombatStage.serverkonckout:
                    case ServerCombatStage.finalskonckout:
                        return ServerEnvSet.Get((ServerEnvKey)20007, 10).ToInt();
                    case ServerCombatStage.finals_32:
                    case ServerCombatStage.finals_16:
                    case ServerCombatStage.quarter_final:
                    case ServerCombatStage.semi_final:
                    case ServerCombatStage.final:
                        return ServerEnvSet.Get((ServerEnvKey)20008, 60).ToInt();
                    default:
                        return 0;
                }
            }

        }
        /// <summary>
        /// 剩下人数
        /// </summary>
        public int Lenght
        {
            get
            {
                switch (CurStage)
                {
                    case ServerCombatStage.Close:
                    case ServerCombatStage.Apply:
                        return int.MaxValue;
                    case ServerCombatStage.serverkonckout:
                        return 16;
                    case ServerCombatStage.finalskonckout:
                        return 32;
                    case ServerCombatStage.finals_32:
                        return 16;
                    case ServerCombatStage.finals_16:
                        return 8;
                    case ServerCombatStage.quarter_final:
                        return 4;
                    case ServerCombatStage.semi_final:
                        return 2;
                    case ServerCombatStage.final:
                        return 1;
                    default:
                        return 0;
                }
            }
        }
    }
}