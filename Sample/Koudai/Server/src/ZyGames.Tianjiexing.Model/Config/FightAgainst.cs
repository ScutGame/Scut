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
using System.Text;
using ProtoBuf;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 对阵数据
    /// </summary>
    [Serializable, ProtoContract]
    public class FightAgainst
    {
     
        [ProtoMember(1)]
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 阶段
        /// </summary>
        [ProtoMember(2)]
        public int Stage
        {
            get;
            set;
        }
        /// <summary>
        /// 编号
        /// </summary>
        [ProtoMember(3)]
        public int No
        {
            get;
            set;
        }
        /// <summary>
        /// A公会ID
        /// </summary>
        [ProtoMember(4)]
        public string AGuildID
        {
            get;
            set;
        }
        /// <summary>
        /// A公会名称
        /// </summary>
        [ProtoMember(5)]
        public int AGuildName
        {
            get;
            set;
        }
        /// <summary>
        /// B公会ID
        /// </summary>
        [ProtoMember(6)]
        public string BGuildID
        {
            get;
            set;
        }
        /// <summary>
        /// B公会名称
        /// </summary>
        [ProtoMember(7)]
        public int BGuildName
        {
            get;
            set;
        }
        /// <summary>
        /// 状态 0：未开始 1：A赢 2：B赢
        /// </summary>
        [ProtoMember(18)]
        public int Result
        {
            get;
            set;
        }
    }
}