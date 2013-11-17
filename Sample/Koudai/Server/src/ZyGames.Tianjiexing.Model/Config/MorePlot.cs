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

namespace ZyGames.Tianjiexing.Model
{
    /// <summary>
    /// 可创建多人副本列表
    /// </summary>
    [Serializable, ProtoContract]
    public class MorePlot
    {
        [ProtoMember(1)]
        public int PlotID
        {
            get;
            set;
        }
        /// <summary>
        /// 副本
        /// </summary>
        [ProtoMember(2)]
        public string PlotName
        {
            get;
            set;
        }

        /// <summary>
        /// 声望
        /// </summary>
        [ProtoMember(3)]
        public int ObtainNum
        {
            get;
            set;
        }

        /// <summary>
        /// 阅历
        /// </summary>
        [ProtoMember(4)]
        public int ExpNum
        {
            get;
            set;
        }

        /// <summary>
        /// 经验
        /// </summary>
        [ProtoMember(5)]
        public int Experience
        {
            get;
            set;
        }

        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(6)]
        public int ItemId
        {
            get;
            set;
        }
        /// <summary>
        /// 物品名称
        /// </summary>
        [ProtoMember(7)]
        public string ItemName
        {
            get;
            set;
        }
        /// <summary>
        /// 物品数量
        /// </summary>
        [ProtoMember(8)]
        public int ItemNum
        {
            get;
            set;
        }
    }
}