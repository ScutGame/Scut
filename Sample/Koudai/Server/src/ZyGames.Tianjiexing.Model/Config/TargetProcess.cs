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
using ProtoBuf;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 目标被攻击过程
    /// </summary>
    [Serializable, ProtoContract]
    public class TargetProcess
    {
        public TargetProcess()
        {
            DamageStatusList = new List<AbilityEffectStatus>();
            TrumpStatusList = new List<SkillInfo>();
        }
        /// <summary>
        /// 目标佣兵
        /// </summary>
        [ProtoMember(1)]
        public int GeneralID
        {
            get;
            set;
        }
        /// <summary>
        /// 生命
        /// </summary>
        [ProtoMember(2)]
        public int LiveNum
        {
            get;
            set;
        }
        /// <summary>
        /// 当前气势
        /// </summary>
        [ProtoMember(3)]
        public short Momentum
        {
            get;
            set;
        }
        /// <summary>
        /// 佣兵站的位置
        /// </summary>
        [ProtoMember(4)]
        public short Position
        {
            get;
            set;
        }
        /// <summary>
        /// 阵形角色
        /// </summary>
        [ProtoMember(5)]
        public EmbattleRole Role
        {
            get;
            set;
        }
        /// <summary>
        /// 伤害值
        /// </summary>
        [ProtoMember(6)]
        public int DamageNum
        {
            get;
            set;
        }
        /// <summary>
        /// 是否闪避
        /// </summary>
        [ProtoMember(7)]
        public bool IsShanBi
        {
            get;
            set;
        }
        /// <summary>
        /// 是否爆击
        /// </summary>
        [ProtoMember(8)]
        public bool IsBaoji
        {
            get;
            set;
        }
        /// <summary>
        /// 是否格挡
        /// </summary>
        [ProtoMember(9)]
        public bool IsGeDang
        {
            get;
            set;
        }
        /// <summary>
        /// 是否反击
        /// </summary>
        [ProtoMember(10)]
        public bool IsBack
        {
            get;
            set;
        }

        [ProtoMember(11)]
        public bool IsMove
        {
            get;
            set;
        }
        /// <summary>
        /// 反击伤害
        /// </summary>
        [ProtoMember(12)]
        public int BackDamageNum
        {
            get;
            set;
        }
        /// <summary>
        /// 佣兵状态
        /// </summary>
        [ProtoMember(13)]
        public BattleStatus TargetStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 中招持续效果
        /// </summary>
        [ProtoMember(14)]
        public List<AbilityEffectStatus> DamageStatusList
        {
            get;
            set;
        }

        /// <summary>
        /// 法宝中招效果
        /// </summary>
        [ProtoMember(15)]
        public List<SkillInfo> TrumpStatusList
        {
            get;
            set;
        }
    }
}