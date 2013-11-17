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
    /// 圣吉塔奖励
    /// </summary>
    [Serializable, ProtoContract]
    public class SJTRewar : EntityChangeEvent
    {
        public SJTRewar()
            : base(false)
        {
        }

        private SJTRewarType _sjtRewarType;

        /// <summary>
        /// 圣吉塔奖励类型
        /// </summary>
        [ProtoMember(1)]
        public SJTRewarType SJTRewarType
        {
            get { return _sjtRewarType; }
            set
            {
                _sjtRewarType = value;
                NotifyByModify();
            }
        }

        private Int32 _ItemId;
        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(2)]
        public Int32 ItemId
        {
            get { return _ItemId; }
            set { _ItemId = value; NotifyByModify(); }
        }

        private int _num;

        /// <summary>
        /// 数量
        /// </summary>
        [ProtoMember(3)]
        public Int32 Num
        {
            get { return _num; }
            set { _num = value; NotifyByModify(); }
        }
        private int _starNum;
        /// <summary>
        /// 星星分数
        /// </summary>
        [ProtoMember(4)]
        public Int32 StarNum
        {
            get { return _starNum; }
            set { _starNum = value; NotifyByModify(); }
        }

        
    }
}