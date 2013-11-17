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
    public class AttributeChance : EntityChangeEvent
    {
        public AttributeChance()
            : base(false)
        {
        }

        private Int32 _potenceNum;

        /// <summary>
        /// 力量概率
        /// </summary>
        [ProtoMember(1)]
        public Int32 PotenceNum
        {
            get { return _potenceNum; }
            set
            {
                _potenceNum = value;
                NotifyByModify();
            }
        }

        private int _thoughtNum;
        /// <summary>
        /// 魂力概率
        /// </summary>
        [ProtoMember(2)]
        public Int32 ThoughtNum
        {
            get { return _thoughtNum; }
            set { _thoughtNum = value; NotifyByModify(); }
        }

        private int _intelligenceNum;

        /// <summary>
        /// 智力概率
        /// </summary>
        [ProtoMember(3)]
        public Int32 IntelligenceNum
        {
            get { return _intelligenceNum; }
            set { _intelligenceNum = value; NotifyByModify(); }
        }
    }
}