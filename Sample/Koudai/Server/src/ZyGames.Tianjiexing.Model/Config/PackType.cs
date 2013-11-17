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
    public class PackType : EntityChangeEvent
    {
        public PackType()
            : base(false)
        {
        }

        private BackpackType _backpackType;

        /// <summary>
        /// 背包类型
        /// </summary>
        [ProtoMember(1)]
        public BackpackType BackpackType
        {
            get { return _backpackType; }
            set
            {
                _backpackType = value;
                NotifyByModify();
            }
        }

        private int _openNum;

        /// <summary>
        /// 开启次数
        /// </summary>
        [ProtoMember(2)]
        public Int32 OpenNum
        {
            get { return _openNum; }
            set { _openNum = value; NotifyByModify(); }
        }

        private int _position;

        /// <summary>
        /// 位置数量
        /// </summary>
        [ProtoMember(3)]
        public Int32 Position
        {
            get { return _position; }
            set { _position = value; NotifyByModify(); }
        }
    }
}