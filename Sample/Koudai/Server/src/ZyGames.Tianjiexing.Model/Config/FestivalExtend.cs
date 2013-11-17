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

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 活动扩展字段
    /// </summary>
    [Serializable, ProtoContract]
    public class FestivalExtend : EntityChangeEvent
    {
        public FestivalExtend()
            : base(false)
        {
            Recruit = new List<RecruitFestival>();
        }

        /// <summary>
        /// 开始倍数
        /// </summary>
        [ProtoMember(1)]
        public decimal Multiple { get; set; }

        /// <summary>
        /// 结束倍数
        /// </summary>
        [ProtoMember(2)]
        public decimal EndMultiple { get; set; }

        /// <summary>
        /// 开启七夕翅膀关键字
        /// </summary>
        [ProtoMember(3)]
        public string KeyWord { get; set; }

        /// <summary>
        /// 活动提示语
        /// </summary>
        [ProtoMember(4)]
        public string Desc { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [ProtoMember(5)]
        public bool IsDisplay { get; set; }

        /// <summary>
        /// 原来的倍数、充值返利百分比
        /// </summary>
        [ProtoMember(6)]
        public double MinusNum { get; set; }

        /// <summary>
        /// 佣兵招募送灵魂活动
        /// </summary>
        public List<RecruitFestival> Recruit { get; set; }

        /// <summary>
        /// 考古副本ID
        /// </summary>
        public int PlotID { get; set; }
    }
}