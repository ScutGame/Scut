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
using System.Runtime.Serialization;
using ProtoBuf;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 战斗中的阵形
    /// </summary>
    [Serializable, ProtoContract]
    public class CombatEmbattle
    {
        [ProtoMember(1)]
        public string UserID
        {
            get;
            set;
        }
        [ProtoMember(2)]
        public int GeneralID
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public short GeneralLv
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public string GeneralName
        {
            get;
            set;
        }
        [ProtoMember(5)]
        public string HeadID
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public int AbilityID
        {
            get;
            set;
        }

        [ProtoMember(7)]
        public int Position
        {
            get;
            set;
        }

        [ProtoMember(8)]
        public int LiveNum
        {
            get;
            set;
        }

        [ProtoMember(9)]
        public int LiveMaxNum
        {
            get;
            set;
        }

        [ProtoMember(10)]
        public short MomentumNum
        {
            get;
            set;
        }

        [ProtoMember(11)]
        public short MaxMomentumNum
        {
            get;
            set;
        }

        [ProtoMember(12)]
        public bool IsWait
        {
            get;
            set;
        }

        [ProtoMember(13)]
        public string BattleHead
        {
            get;
            set;
        }
    }
}