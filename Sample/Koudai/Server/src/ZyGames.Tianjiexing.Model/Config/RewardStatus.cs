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

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 玩家佣兵魂技
    /// </summary>
    [Serializable, ProtoContract]
    public class RewardStatus : EntityChangeEvent
    {
        public RewardStatus()
            : base(false)
        {

        }

        private Int32 _sjtid;

        /// <summary>
        /// 可领取奖励层数的ID
        /// </summary>
        [ProtoMember(1)]
        public Int32 SJTID
        {
            get { return _sjtid; }
            set
            {
                _sjtid = value;
                NotifyByModify();
            }
        }

        private Int32 _isReceive;
        /// <summary>
        /// 是否已领取0：否1：是
        /// </summary>
        [ProtoMember(2)]
        public Int32 IsReceive
        {
            get { return _isReceive; }
            set { _isReceive = value; NotifyByModify(); }
        }

        private int _StarNum;

        /// <summary>
        /// 星星分数
        /// </summary>
        [ProtoMember(3)]
        public Int32 StarNum
        {
            get { return _StarNum; }
            set { _StarNum = value; NotifyByModify(); }
        }
        private int _experienceNum;
        /// <summary>
        /// 当前经验
        /// </summary>
        [ProtoMember(4)]
        public Int32 ExperienceNum
        {
            get { return _experienceNum; }
            set { _experienceNum = value; NotifyByModify(); }
        }

        private Int32 _RewardType;

        /// <summary>
        /// 奖励类型0：不可重复领取1：可以重复领取
        /// </summary>
        [ProtoMember(5)]
        public Int32 RewardType
        {
            get { return _RewardType; }
            set
            {
                _RewardType = value;
                NotifyByModify();
            }
        }

        
    }
}