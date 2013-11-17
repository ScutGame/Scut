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
    /// 每日探险答题奖励
    /// [{"Answer":1,"RewardType": 2,"RewardNum": 10},{"Answer":1,"RewardType": 4,"RewardNum": 5},{"Answer":2,"RewardType": 3,"RewardNum": 50},{"Answer":2,"RewardType": 1,"RewardNum": 100}]
    /// </summary>
    [Serializable, ProtoContract]
    public class RewardShow : JsonEntity
    {
        /// <summary>
        /// 答案
        /// </summary>
        [ProtoMember(1)]
        public Int32 Answer
        {
            get;
            set;
        }

        /// <summary>
        /// 奖励类型
        /// </summary>
        [ProtoMember(2)]
        public RewardType RewardType
        {
            get;
            set;
        }

        /// <summary>
        /// 奖励数值
        /// </summary>
        [ProtoMember(3)]
        public Int32 RewardNum
        {
            get;
            set;
        }

    }
}