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
    public class MemberLog : JsonEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [ProtoMember(1)]
        public string UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 1：贡献 2：上香
        /// </summary>
        [ProtoMember(2)]
        public Int16 LogType
        {
            get;
            set;
        }

        /// <summary>
        /// 上香ID
        /// </summary>
        [ProtoMember(3)]
        public Int32 IdolID
        {
            get;
            set;
        }

        /// <summary>
        /// 获得声望
        /// </summary>
        [ProtoMember(4)]
        public Int32 GainObtion
        {
            get;
            set;
        }

        /// <summary>
        /// 贡献经验
        /// </summary>
        [ProtoMember(5)]
        public Int32 Experience
        {
            get;
            set;
        }

        /// <summary>
        /// 获得灵气
        /// </summary>
        [ProtoMember(6)]
        public Int32 GainAura
        {
            get;
            set;
        }

        /// <summary>
        /// 贡献时间
        /// </summary>
        [ProtoMember(7)]
        public DateTime InsertDate
        {
            get;
            set;
        }
    }
}