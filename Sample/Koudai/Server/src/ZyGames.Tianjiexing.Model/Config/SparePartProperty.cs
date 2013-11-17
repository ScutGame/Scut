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
    /// 灵件属性
    /// </summary>
    [Serializable, ProtoContract]
    public class SparePartProperty : JsonEntity
    {
        public SparePartProperty()
        {
            NumRange = new double[0];
            Rate = new double[0];
        }

        [ProtoMember(1)]
        public AbilityType AbilityType { get; set; }

        [ProtoMember(2)]
        public double[] NumRange { get; set; }

        [ProtoMember(3)]
        public double[] Rate { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [ProtoMember(4)]
        public double DefNum { get; set; }

        [ProtoMember(5)]
        public double Num { get; set; }

        [ProtoMember(6)]
        public bool IsEnable { get; set; }

        /// <summary>
        /// 索引从0开始
        /// </summary>
        [ProtoMember(7)]
        public short ValueIndex { get; set; }

        public double HitMinValue
        {
            get
            {
                return ValueIndex < NumRange.Length ? NumRange[ValueIndex] : 0;
            }
        }

        public double HitMaxValue
        {
            get { return ValueIndex < NumRange.Length - 1 ? NumRange[ValueIndex + 1] : 0; }
        }
    }
}