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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Event;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// Boss奖励配置
    /// </summary>
    //[Serializable, ProtoContract]
    public class BossActivePrize : EntityChangeEvent
    {
        protected BossActivePrize()
            : base(false)
        {
        }

        /// <summary>
        /// 声望奖励比率
        /// </summary>
        [ProtoMember(1)]
        public int ObtainRate { get; set; }
        /// <summary>
        /// 最大声望奖励
        /// </summary>
        [ProtoMember(2)]
        public int MaxObtain { get; set; }
        /// <summary>
        /// 金币奖励比率
        /// </summary>
        [ProtoMember(3)]
        public int CoinRate { get; set; }
        /// <summary>
        /// 最大金币奖励
        /// </summary>
        [ProtoMember(4)]
        public int MaxCoin { get; set; }
        /// <summary>
        /// 击杀Boss时初始等级
        /// </summary>
        [ProtoMember(5)]
        public int KillBossLv { get; set; }
        /// <summary>
        /// 击杀Boss奖励比率
        /// </summary>
        [ProtoMember(6)]
        public int KillBossRate { get; set; }

        /// <summary>
        /// 排名前几可获得声望
        /// </summary>
        [ProtoMember(7)]
        public int TopObtain { get; set; }
        /// <summary>
        /// 排名前几奖励声望
        /// </summary>
        [ProtoMember(8)]
        public int TopObtainNum { get; set; }


        /// <summary>
        /// 第一名
        /// </summary>
        [ProtoMember(9)]
        public int[] Rank1 { get; set; }
        /// <summary>
        /// 第二名
        /// </summary>
        [ProtoMember(10)]
        public int[] Rank2 { get; set; }
        /// <summary>
        /// 第三名
        /// </summary>
        [ProtoMember(11)]
        public int[] Rank3 { get; set; }

        /// <summary>
        /// 物品配置
        /// </summary>
        [ProtoMember(12)]
        public CacheList<CacheList<BossItem>> Items { get; set; }

        /// <summary>
        /// 击杀奖励
        /// </summary>
        [ProtoMember(13)]
        public int[] KillReward { get; set; }

        /// <summary>
        /// 排名第二阶梯可获得声望
        /// </summary>
        [ProtoMember(14)]
        public int AfterObtain { get; set; }

        /// <summary>
        /// 排名第二阶梯可获得声望
        /// </summary>
        [ProtoMember(15)]
        public int AfterObtainNum { get; set; }

        /// <summary>
        /// 第四名
        /// </summary>
        [ProtoMember(16)]
        public int[] Rank4 { get; set; }

        /// <summary>
        /// 第五名
        /// </summary>
        [ProtoMember(17)]
        public int[] Rank5 { get; set; }
    }

    [Serializable, ProtoContract]
    public class BossItem
    {
        /// <summary>
        /// 玩家等级
        /// </summary>
        [ProtoMember(1)]
        public short UserLv { get; set; }
        /// <summary>
        /// 物品类型
        /// </summary>
        [ProtoMember(2)]
        public ItemType Type { get; set; }
        /// <summary>
        /// 物品等级
        /// </summary>
        [ProtoMember(3)]
        public short ItemLv { get; set; }
        /// <summary>
        /// 物品品质
        /// </summary>
        [ProtoMember(4)]
        public short Quality { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [ProtoMember(5)]
        public int Num { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [ProtoMember(6)]
        public int ItemId { get; set; }
    }
}