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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Event;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 活动限制
    /// </summary>
    [Serializable, ProtoContract]
    public class RestrainExtend : EntityChangeEvent
    {
        public RestrainExtend()
            : base(false)
        {

        }

        private int _ID;
        /// <summary>
        /// 活动Key,(考古:NpcID)
        /// </summary>
        [ProtoMember(1)]
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                NotifyByModify();
            }
        }

        private int _Num;
        /// <summary>
        /// 活动次数,(考古:通关次数)
        /// </summary>
        [ProtoMember(2)]
        public int Num
        {
            get { return _Num; }
            set
            {
                _Num = value;
                NotifyByModify();
            }
        }
    }
}