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
using ZyGames.Framework.Model;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 参与城市争斗战的会员
    /// </summary>
    [Serializable, ProtoContract]
    public class FightUser : BaseEntity
    {
        [ProtoMember(1)]
        public string GuildID { get; set; }

        /// <summary>
        /// 玩家ID
        /// </summary>
        [ProtoMember(2)]
        public string UserId
        {
            get;
            set;
        }
       
        /// <summary>
        /// 玩家Name
        /// </summary>
        [ProtoMember(3)]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 城市ID
        /// </summary>
        [ProtoMember(4)]
        public int CityID
        {
            get;
            set;
        }

        /// <summary>
        /// 赢的场数
        /// </summary>
        [ProtoMember(5)]
        public int WinCount
        {
            get;
            set;
        }
        /// <summary>
        /// 获得的声望
        /// </summary>
        [ProtoMember(6)]
        public int ObtainNum
        {
            get;
            set;
        }
       
        /// <summary>
        /// 鼓舞加成
        /// </summary>
        [ProtoMember(7)]
        public double InspirePercent
        {
            get;
            set;
        }

        /// <summary>
        /// 战报版本
        /// </summary>
        [ProtoMember(8)]
        public int Version { get; set; }

        /// <summary>
        /// 是否移除
        /// </summary>
        [ProtoMember(9)]
        public bool IsRemove { get; set; }

        /// <summary>
        /// 是否对方没人获胜
        /// </summary>
        [ProtoMember(10)]
        public bool IsNotEnough { get; set; }
      
        protected override object this[string index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        protected override int GetIdentityId()
        {
            return DefIdentityId;
        }
    }
}