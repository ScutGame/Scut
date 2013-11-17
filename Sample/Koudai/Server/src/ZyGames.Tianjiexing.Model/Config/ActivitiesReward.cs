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
    /// 节日活动奖励
    /// </summary>
    [Serializable, ProtoContract]
    public class ActivitiesReward
    {
        /// <summary>
        /// 收集类型
        /// </summary>
        [ProtoMember(1)]
        public int Gather { get; set; }

        /// <summary>
        /// 奖励类型
        /// </summary>
        [ProtoMember(2)]
        public RewardType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 奖励礼包ID
        /// </summary>
        [ProtoMember(3)]
        public int Item
        {
            get;
            set;
        }

        /// <summary>
        /// 奖励物品ID
        /// </summary>
        [ProtoMember(4)]
        public int ItemID { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        [ProtoMember(5)]
        public int Num
        {
            get;
            set;
        }

        /// <summary>
        /// 活动时间
        /// </summary>
        [ProtoMember(6)]
        public DateTime Date { get; set; }

        /// <summary>
        /// 水晶品质
        /// </summary>
        [ProtoMember(7)]
        public CrystalQualityType CrystalType { get; set; }

        /// <summary>
        /// 命运水晶ID
        /// </summary>
        [ProtoMember(8)]
        public int CrystalID { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [ProtoMember(9)]
        public int UserLv { get; set; }
    }
}