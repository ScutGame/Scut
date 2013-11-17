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
    public class AttributeValueChance : EntityChangeEvent
    {
        public AttributeValueChance()
            : base(false)
        {
        }

        private String _potenceValue;

        /// <summary>
        /// 力量值
        /// </summary>
        [ProtoMember(1)]
        public String PotenceValue
        {
            get { return _potenceValue; }
            set
            {
                _potenceValue = value;
                NotifyByModify();
            }
        }

        private String _potenceNum;
        /// <summary>
        /// 力量值概率
        /// </summary>
        [ProtoMember(2)]
        public String PotenceNum
        {
            get { return _potenceNum; }
            set { _potenceNum = value; NotifyByModify(); }
        }

        private String _thoughtValue;

        /// <summary>
        /// 魂力值
        /// </summary>
        [ProtoMember(3)]
        public String ThoughtValue
        {
            get { return _thoughtValue; }
            set { _thoughtValue = value; NotifyByModify(); }
        }

        private String _thoughtNum;

        /// <summary>
        /// 魂力值概率
        /// </summary>
        [ProtoMember(4)]
        public String ThoughtNum 
        {
            get { return _thoughtNum; }
            set { _thoughtNum = value; NotifyByModify(); }
        }

        private String _intelligenceValue;

        /// <summary>
        /// 智力值
        /// </summary>
        [ProtoMember(5)]
        public String IntelligenceValue
        {
            get { return _intelligenceValue; }
            set { _intelligenceValue = value; NotifyByModify(); }
        }

        private String _intelligenceNum;

        /// <summary>
        /// 智力概率
        /// </summary>
        [ProtoMember(6)]
        public String IntelligenceNum
        {
            get { return _intelligenceNum; }
            set { _intelligenceNum = value; NotifyByModify(); }
        }
    }
}