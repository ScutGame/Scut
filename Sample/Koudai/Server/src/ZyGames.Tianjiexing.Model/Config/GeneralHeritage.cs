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
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Event;


namespace ZyGames.Tianjiexing.Model.Config
{

    [Serializable, ProtoContract]
    public class GeneralHeritage : EntityChangeEvent
    {
        public GeneralHeritage()
            : base(false)
        {
        }

        private HeritageType _type;

        /// <summary>
        ///  传承类型
        /// </summary>
        [ProtoMember(1)]
        public HeritageType Type
        {
            get { return _type; }
            set { _type = value;
                NotifyByModify(); }
        }

        private int _generalID;

        /// <summary>
        /// 佣兵ID
        /// </summary>
        [ProtoMember(2)]
        public int GeneralID
        {
            get { return _generalID; }
            set { _generalID = value; NotifyByModify(); }
        }

        private short _generalLv;

        /// <summary>
        /// 佣兵等级
        /// </summary>
        [ProtoMember(4)]
        public short GeneralLv
        {
            get { return _generalLv; }
            set { _generalLv = value; NotifyByModify(); }
        }

        private short _powerNum;

        /// <summary>
        /// 力量
        /// </summary>
        [ProtoMember(5)]
        public short PowerNum
        {
            get { return _powerNum; }
            set { _powerNum = value; NotifyByModify(); }
        }

        private short _soulNum;

        /// <summary>
        /// 魂力
        /// </summary>
        [ProtoMember(6)]
        public short SoulNum
        {
            get { return _soulNum; }
            set { _soulNum = value; NotifyByModify(); }
        }

        private short _intellectNum;

        /// <summary>
        /// 智力
        /// </summary>
        [ProtoMember(7)]
        public short IntellectNum
        {
            get { return _intellectNum; }
            set { _intellectNum = value; NotifyByModify(); }
        }

        private int _opsType;

        /// <summary>
        /// 操作类型 1：免费传承 2：晶石传承 3：至尊传承
        /// </summary>
        [ProtoMember(8)]
        public int opsType
        {
            get { return _opsType; }
            set { _opsType = value; NotifyByModify(); }
        }
    }
}