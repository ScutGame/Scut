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
using ZyGames.Framework.Event;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 玩家佣兵魂技
    /// </summary>
    [Serializable, ProtoContract]
    public class LairTreasure : EntityChangeEvent
    {
        public LairTreasure()
            : base(false)
        {
        }

        private LairRewardType _LairRewardType;

        /// <summary>
        /// 取宝类型
        /// </summary>
        [ProtoMember(1)]
        public LairRewardType LairRewardType
        {
            get { return _LairRewardType; }
            set
            {
                _LairRewardType = value;
                NotifyByModify();
            }
        }

        private LairQualityType _LairQualityType;
        /// <summary>
        /// 品质
        /// </summary>
        [ProtoMember(2)]
        public LairQualityType LairQualityType
        {
            get { return _LairQualityType; }
            set { _LairQualityType = value; NotifyByModify(); }
        }

        private int _Num;

        /// <summary>
        /// 数量
        /// </summary>
        [ProtoMember(3)]
        public Int32 Num
        {
            get { return _Num; }
            set { _Num = value; NotifyByModify(); }
        }
        private string _HeadID;
        /// <summary>
        /// 当前经验
        /// </summary>
        [ProtoMember(4)]
        public string HeadID
        {
            get { return _HeadID; }
            set { _HeadID = value; NotifyByModify(); }
        }

        private decimal _Probability;

        /// <summary>
        /// 概率
        /// </summary>
        [ProtoMember(5)]
        public decimal Probability
        {
            get { return _Probability; }
            set
            {
                _Probability = value;
                NotifyByModify();
            }
        }

        private int _LairPosition;

        /// <summary>
        /// 位置
        /// </summary>
        [ProtoMember(6)]
        public int LairPosition
        {
            get { return _LairPosition; }
            set
            {
                _LairPosition = value;
                NotifyByModify();
            }
        }
    }
}