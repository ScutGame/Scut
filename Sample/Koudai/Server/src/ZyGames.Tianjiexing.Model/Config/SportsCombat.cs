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
using System.Runtime.Serialization;
using System.Text;
using ProtoBuf;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 竞技战斗信息
    /// </summary>
    [Serializable, ProtoContract]
    public class SportsCombat
    {
        /// <summary>
        /// 挑战谁
        /// </summary>
        [ProtoMember(1)]
        public string ToUser
        {
            get;
            set;
        }
        /// <summary>
        /// 挑战谁
        /// </summary>
        [ProtoMember(2)]
        public string ToUserName
        {
            get;
            set;
        }
        /// <summary>
        /// 排名ID
        /// </summary>
        [ProtoMember(3)]
        public int TopID
        {
            get;
            set;
        }

        /// <summary>
        /// 发启人是否是自己
        /// </summary>
        [ProtoMember(4)]
        public bool IsSelf { get; set; }

        /// <summary>
        /// 0：不变 1：上升 2：下降
        /// </summary>
        [ProtoMember(5)]
        public short RankStatus { get; set; }

        /// <summary>
        /// 挑战次数
        /// </summary>
        [ProtoMember(6)]
        public int SportsNum
        {
            get;
            set;
        }

        /// <summary>
        /// 连胜次数
        /// </summary>
        [ProtoMember(7)]
        public int WinNum
        {
            get;
            set;
        }

        /// <summary>
        /// 战斗结果
        /// </summary>
        [ProtoMember(8)]
        public bool IsWin
        {
            get;
            set;
        }

        /// <summary>
        /// 奖励金币
        /// </summary>
        [ProtoMember(9)]
        public int RewardGoin
        {
            get;
            set;
        }

        /// <summary>
        /// 奖励声望
        /// </summary>
        [ProtoMember(10)]
        public int RewardObtian
        {
            get;
            set;
        }

        /// <summary>
        /// 战斗过程
        /// </summary>
        [ProtoMember(11)]
        public CombatProcessContainer CombatProcess
        {
            get;
            set;
        }

        /// <summary>
        /// 挑战时间
        /// </summary>
        [ProtoMember(12)]
        public DateTime CombatDate
        {
            get;
            set;
        }
    }
}