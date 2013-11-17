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
using ProtoBuf;
using ZyGames.Framework.Event;


namespace ZyGames.Tianjiexing.Model
{
    [Serializable, ProtoContract]
    public class Karma : EntityChangeEvent
    {
        public Karma()
            : base(false)
        {
        }

        private KarmaType _karmaType;

        /// <summary>
        /// 缘分类型
        /// </summary>
        [ProtoMember(1)]
        public KarmaType KarmaType
        {
            get { return _karmaType; }
            set
            {
                _karmaType = value;
                NotifyByModify();
            }
        }

        private String _valueId;

        /// <summary>
        /// 对应类型ID
        /// </summary>
        [ProtoMember(2)]
        public String ValueID
        {
            get { return _valueId; }
            set { _valueId = value; NotifyByModify(); }
        }

        private AbilityType _abilityType;

        /// <summary>
        /// 缘分属性类型
        /// </summary>
        [ProtoMember(3)]
        public AbilityType AbilityType
        {
            get { return _abilityType; }
            set { _abilityType = value; NotifyByModify(); }
        }
        private Decimal _attValue;

        /// <summary>
        /// 属性值
        /// </summary>
        [ProtoMember(4)]
        public Decimal AttValue
        {
            get { return _attValue; }
            set { _attValue = value; NotifyByModify(); }
        }

        private Int32 _attValueType;

        /// <summary>
        /// 属性值类型1:固定值2：百分比
        /// </summary>
        [ProtoMember(5)]
        public Int32 AttValueType
        {
            get { return _attValueType; }
            set { _attValueType = value; NotifyByModify(); }
        }
        private String _karmaName;
        /// <summary>
        /// 缘分名称
        /// </summary>
        [ProtoMember(6)]
        public String KarmaName
        {
            get { return _karmaName; }
            set { _karmaName = value; NotifyByModify(); }
        }
        private String _karmaDesc;
        /// <summary>
        /// 缘分描述
        /// </summary>
        [ProtoMember(7)]
        public String KarmaDesc
        {
            get { return _karmaDesc; }
            set { _karmaDesc = value; NotifyByModify(); }
        }

        private Int32 _isActive;
        /// <summary>
        /// 是否激活
        /// </summary>
        [ProtoMember(8)]
        public Int32 IsActive
        {
            get { return _isActive; }
            set { _isActive = value; NotifyByModify(); }
        }
    }
}