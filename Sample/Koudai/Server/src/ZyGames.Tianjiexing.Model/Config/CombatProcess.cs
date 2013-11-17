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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 战斗过程
    /// </summary>
    [Serializable, ProtoContract]
    public class CombatProcess
    {
        public CombatProcess()
        {
            this.TargetList = new CacheList<TargetProcess>();
            this.DamageStatusList = new CacheList<AbilityEffectStatus>();
            TrumpStatusList = new List<SkillInfo>();
        }

        /// <summary>
        /// 佣兵ID
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

        [ProtoMember(3)]
        public bool IsMove
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public string PercentList { get; set; }


        /// <summary>
        /// 存放1=命中,2=爆击,3=格挡概率
        /// </summary>
        public void AddPercent(int type, decimal percent)
        {
            PercentList += "," + type + "=" + percent;
        }

        /// <summary>
        /// 气势
        /// </summary>
        [ProtoMember(5)]
        public short Momentum
        {
            get;
            set;
        }
        /// <summary>
        /// 佣兵站的位置
        /// </summary>
        [ProtoMember(6)]
        public short Position
        {
            get;
            set;
        }

        /// <summary>
        /// 阵形角色
        /// </summary>
        [ProtoMember(7)]
        public EmbattleRole Role
        {
            get;
            set;
        }

        /// <summary>
        /// 攻击目标
        /// </summary>
        [ProtoMember(8)]
        public AttackTaget AttackTaget
        {
            get;
            set;
        }
        /// <summary>
        /// 攻击类型
        /// </summary>
        [ProtoMember(9)]
        public AttackType AttackType
        {
            get;
            set;
        }
        /// <summary>
        /// 攻击单位
        /// </summary>
        [ProtoMember(10)]
        public AttackUnit AttackUnit
        {
            get;
            set;
        }
        /// <summary>
        /// 伤害类型
        /// </summary>
        [ProtoMember(11)]
        public AbilityProperty AbilityProperty
        {
            get;
            set;
        }

        /// <summary>
        /// 佣兵状态
        /// </summary>
        [ProtoMember(12)]
        public BattleStatus AttStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 反击伤害值
        /// </summary>
        [ProtoMember(13)]
        public int DamageNum
        {
            get;
            set;
        }
        /// <summary>
        /// 释放魂技佣兵光效
        /// </summary>
        [ProtoMember(14)]
        public string AttEffectID
        {
            get;
            set;
        }
        /// <summary>
        /// 目标佣兵光效
        /// </summary>
        [ProtoMember(15)]
        public string TargetEffectID
        {
            get;
            set;
        }
        /// <summary>
        /// 中招持续效果
        /// </summary>
        [ProtoMember(16)]
        public CacheList<AbilityEffectStatus> DamageStatusList
        {
            get;
            set;
        }

        [ProtoMember(17)]
        public CacheList<TargetProcess> TargetList
        {
            get;
            set;
        }

        /// <summary>
        /// 法宝中招效果
        /// </summary>
        [ProtoMember(18)]
        public List<SkillInfo> TrumpStatusList
        {
            get;
            set;
        }

        /// <summary>
        /// 文字光效图片
        /// </summary>
        [ProtoMember(19)]
        public string FntHeadID
        {
            get;
            set;
        }
        /// <summary>
        /// 魂技ID
        /// </summary>
        [ProtoMember(20)]
        public int AbilityID
        {
            get;
            set;
        }
    }
}