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
using System.Runtime.Serialization;
using ProtoBuf;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 释放魂技效果状态
    /// </summary>
    [Serializable, ProtoContract]
    public class AbilityEffectStatus
    {
        [ProtoMember(1)]
        public AbilityType AbilityType
        {
            get;
            set;
        }
        /// <summary>
        /// 加点数，计算技能伤害使用
        /// </summary>
        [ProtoMember(2)]
        public int BaseNum
        {
            get;
            set;
        }

        /// <summary>
        /// 释放命中概率
        /// </summary>
        [ProtoMember(3)]
        public decimal HitPercent
        {
            get;
            set;
        }
        /// <summary>
        /// 是否增益效果
        /// </summary>
        [ProtoMember(4)]
        public bool IsIncrease
        {
            get;
            set;
        }
        /// <summary>
        /// 加成效果，计算技能伤害使用, 有修正概率
        /// </summary>
        [ProtoMember(5)]
        public decimal IncreaseNum
        {
            get;
            set;
        }

        /// <summary>
        /// 总回合数
        /// </summary>
        [ProtoMember(6)]
        public int TotalBoutNum
        {
            get;
            set;
        }

        /// <summary>
        /// 当前回合数
        /// </summary>
        [ProtoMember(7)]
        public int BoutNum
        {
            get;
            set;
        }
        /// <summary>
        /// 每回合持续伤害
        /// </summary>
        [ProtoMember(8)]
        public int DamageNum
        {
            get;
            set;
        }

        /// <summary>
        /// 标记是否移除
        /// </summary>
        [ProtoMember(9)]
        public bool IsRemove
        {
            get;
            set;
        }

        /// <summary>
        /// 是否静默状态
        /// </summary>
        public bool IsSilentStatus
        {
            get
            {
                if (this.AbilityType == AbilityType.Yunxuan ||
                    this.AbilityType == AbilityType.Hunshui ||
                    this.AbilityType == AbilityType.Bingdong ||
                    this.AbilityType == AbilityType.Mishi ||
                    this.AbilityType == AbilityType.Dingshen)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否持续受伤状态
        /// </summary>
        public bool IsInjuredStatus
        {
            get
            {
                if (this.AbilityType == AbilityType.Zhongdu ||
                    this.AbilityType == AbilityType.Chuxie )
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 是否增益状态
        /// </summary>
        public bool IsZengyi
        {
            get { return AbilityType >= AbilityType.WuLiGongJi && AbilityType <= AbilityType.BiSha; }
        }

        public AbilityEffectStatus Copy()
        {
           return new AbilityEffectStatus
           {
               AbilityType = AbilityType,
               BaseNum = BaseNum,
               IsIncrease = IsIncrease,
               IncreaseNum = IncreaseNum,
               BoutNum = BoutNum,

               TotalBoutNum = TotalBoutNum,
               DamageNum = DamageNum,
               IsRemove = IsRemove,
               HitPercent = HitPercent
           };
        }
    }
}