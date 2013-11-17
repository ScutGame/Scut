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
using ZyGames.Framework.Event;

namespace ZyGames.Tianjiexing.Model.Config
{
    [Serializable, ProtoContract]
    public class UserPlotArcheology : EntityChangeEvent
    {
        public UserPlotArcheology()
            : base(false)
        {

        }

        private int _plotNpcId;

        /// <summary>
        /// 怪物 ID
        /// </summary>
        [ProtoMember(1)]
        public Int32 PlotNpcID
        {
            get { return _plotNpcId; }
            set
            {
                _plotNpcId = value;
                NotifyByModify();
            }
        }

        private short _lightMapCount;

        /// <summary>
        /// 已点亮碎片数
        /// </summary>
        [ProtoMember(2)]
        public Int16 LightMapCount
        {
            get { return _lightMapCount; }
            set
            {
                _lightMapCount = value;
                NotifyByModify();
            }
        }

        private bool _isOpen;

        /// <summary>
        /// 是否翻开
        /// </summary>
        [ProtoMember(3)]
        public Boolean IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                NotifyByModify();
            }
        }

        private int _position;

        /// <summary>
        /// 怪物位置
        /// </summary>
        [ProtoMember(4)]
        public Int32 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                NotifyByModify();
            }
        }

        private short _quality;

        /// <summary>
        /// 怪物品质
        /// </summary>
        [ProtoMember(5)]
        public Int16 Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                NotifyByModify();
            }
        }
    }
}