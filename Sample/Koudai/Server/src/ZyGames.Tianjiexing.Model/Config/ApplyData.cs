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
using System.Runtime.Serialization;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model
{
    /// <summary>
    /// 报名数据
    /// </summary>
    [Serializable, ProtoContract]
    public class ApplyData
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [ProtoMember(1)]
        public string NickName
        {
            get;
            set;
        }
        /// <summary>
        /// 战力
        /// </summary>
        [ProtoMember(2)]
        public int TotalCombatNum
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        [ProtoMember(3)]
        public int CombatType
        {
            get;
            set;
        }
        /// <summary>
        /// 所在服名称
        /// </summary>
        [ProtoMember(4)]
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 活动ID
        /// </summary>
        [ProtoMember(5)]
        public int FastID
        {
            get;
            set;
        }
        /// <summary>
        /// 名次
        /// </summary>
        [ProtoMember(6)]
        public int Rank
        {
            get;
            set;
        }
        /// <summary>
        /// 等级
        /// </summary>
        [ProtoMember(7)]
        public int UserLv
        {
            get;
            set;
        }
        /// <summary>
        /// 声望
        /// </summary>
        [ProtoMember(8)]
        public int ObtainNum
        {
            get;
            set;
        }
    }
}