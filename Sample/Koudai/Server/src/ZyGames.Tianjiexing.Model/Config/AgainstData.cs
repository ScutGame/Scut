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
using System.Runtime.Serialization;

namespace ZyGames.Tianjiexing.Model.DataModel
{

    /// <summary>
    /// 对阵数据
    /// </summary>
    [Serializable, ProtoContract]
    public class AgainstData
    {
        [ProtoMember(1)]
        public string ID
        {
            get;
            set;
        }
        [ProtoMember(2)]
        public int Stage
        {
            get;
            set;
        }
        [ProtoMember(3)]
        public int No
        {
            get;
            set;
        }
        [ProtoMember(4)]
        public string APlayID
        {
            get;
            set;
        }
        [ProtoMember(5)]
        public int AServerID
        {
            get;
            set;
        }
        [ProtoMember(6)]
        public string ANickName
        {
            get;
            set;
        }
        [ProtoMember(7)]
        public int AUserLv
        {
            get;
            set;
        }
        [ProtoMember(8)]
        public int ATotalCombatNum
        {
            get;
            set;
        }
        [ProtoMember(9)]
        public int AObtainNum
        {
            get;
            set;
        }
        [ProtoMember(10)]
        public string BPlayID
        {
            get;
            set;
        }
        [ProtoMember(11)]
        public int BServerID
        {
            get;
            set;
        }
        [ProtoMember(12)]
        public string BNickName
        {
            get;
            set;
        }
        [ProtoMember(13)]
        public int BUserLv
        {
            get;
            set;
        }
        [ProtoMember(14)]
        public int BTotalCombatNum
        {
            get;
            set;
        }
        [ProtoMember(15)]
        public int BObtainNum
        {
            get;
            set;
        }
        [ProtoMember(16)]
        public int Result
        {
            get;
            set;
        }

    }
}