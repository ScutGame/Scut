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
    /// 收集活动配置
    /// </summary>
    [Serializable, ProtoContract]
    public class TaskConfigInfo : JsonEntity
    {
        /// <summary>
        /// 类型 1:副本概率 2:通关副本获得物品
        /// </summary>
        [ProtoMember(1)]
        public int Gather { get; set; }

        /// <summary>
        /// 副本掉落概率
        /// </summary>
        [ProtoMember(2)]
        public decimal probability { get; set; }

        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(3)]
        public int item { get; set; }

        /// <summary>
        /// 物品数量
        /// </summary>
        [ProtoMember(4)]
        public int Num { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [ProtoMember(5)]
        public string CityID { get; set; }

        /// <summary>
        /// 副本类型
        /// </summary>
        [ProtoMember(6)]
        public PlotType PlotType { get; set; }

        /// <summary>
        /// 活动提示时间
        /// </summary>
        [ProtoMember(7)]
        public DateTime Date { get; set; }

        /// <summary>
        /// 活动提示
        /// </summary>
        [ProtoMember(8)]
        public string Desc { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        [ProtoMember(9)]
        public string  StratDate { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        [ProtoMember(10)]
        public string EndData { get; set; }

        /// <summary>
        /// 原来的倍数
        /// </summary>
        [ProtoMember(11)]
        public double MinusNum { get; set; }
    }
}