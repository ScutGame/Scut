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
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 奖励配置信息
    /// </summary>
    [Serializable, ProtoContract]
    public class PrizeInfo : JsonEntity
    {
        /// <summary>
        /// 奖励类型 1：随机奖励,2：概率奖励,3:全部奖励
        /// </summary>
        [ProtoMember(1)]
        public int Reward { get; set; }

        /// <summary>
        /// 奖励类型
        /// </summary>
        [ProtoMember(2)]
        public RewardType Type { get; set; }

        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(3)]
        public int ItemID { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [ProtoMember(4)]
        public int Num { get; set; }

        /// <summary>
        /// 概率
        /// </summary>
        [ProtoMember(5)]
        public decimal Probability { get; set; }

        /// <summary>
        /// 水晶品质
        /// </summary>
        [ProtoMember(7)]
        public CrystalQualityType CrystalType { get; set; }

        /// <summary>
        /// 玩家等级
        /// </summary>
        [ProtoMember(8)]
        public short UserLv { get; set; }

        /// <summary>
        /// 随机物品
        /// </summary>
        [ProtoMember(9)]
        public ItemRandom ItemList { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        [ProtoMember(10)]
        public DateTime RefreshDate { get; set; }

        /// <summary>
        /// 竞技场胜利次数
        /// </summary>
        [ProtoMember(11)]
        public int WinNum { get; set; }

        /// <summary>
        /// 战力加成数值
        /// </summary>
        [ProtoMember(12)]
        public double EffectNum { get; set; }

        /// <summary>
        /// 获得奖励特殊提示语
        /// </summary>
        [ProtoMember(13)]
        public string Desc { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        [ProtoMember(14)]
        public string RetailID { get; set; }

        /// <summary>
        /// 需求物品ID
        /// </summary>
        [ProtoMember(15)]
        public int DemandItem { get; set; }

        /// <summary>
        /// 需求物品数量
        /// </summary>
        [ProtoMember(16)]
        public int DemandNum { get; set; }

         /// <summary>
        /// 系数
        /// </summary>
        [ProtoMember(17)]
        public decimal Coefficient { get; set; }

        /// <summary>
        /// 佣兵品质
        /// </summary>
        [ProtoMember(18)]
        public GeneralQuality GeneralQuality { get; set; }

        /// <summary>
        /// 技能品质
        /// </summary>
        [ProtoMember(19)]
        public int AbilityQuality { get; set; }

        /// <summary>
        /// 是否乘玩家等级
        /// </summary>
        [ProtoMember(20)]
        public int IsMultiplyUserLv { get; set; }

        /// <summary>
        /// 头像ID
        /// </summary>
        [ProtoMember(21)]
        public string HeadID { get; set; }

    }
}