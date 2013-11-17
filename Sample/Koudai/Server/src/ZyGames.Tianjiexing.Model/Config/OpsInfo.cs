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
    [Serializable, ProtoContract]
    public class OpsInfo
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        [ProtoMember(1)]
        public int Type { get; set; }

        /// <summary>
        /// Vip等级
        /// </summary>
        [ProtoMember(2)]
        public short VipLv { get; set; }

        /// <summary>
        /// 所需晶石
        /// </summary>
        [ProtoMember(3)]
        public int UseGold { get; set; }

        /// <summary>
        /// 传承百分比
        /// </summary>
        [ProtoMember(4)]
        public double Num { get; set; }

        /// <summary>
        /// 传承消耗的物品ID
        /// </summary>
        [ProtoMember(5)]
        public int ItemID { get; set; }

        /// <summary>
        /// 传承消耗的物品数量
        /// </summary>
        [ProtoMember(6)]
        public int ItemNum { get; set; }
    }
}