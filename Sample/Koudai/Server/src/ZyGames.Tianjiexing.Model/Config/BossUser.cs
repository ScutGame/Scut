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
using ZyGames.Framework.Model;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 参与BOSS战的用户
    /// </summary>
    [Serializable, ProtoContract]
    public class BossUser : BaseEntity
    {
        [ProtoMember(1)]
        public string UserId { get; set; }

        [ProtoMember(2)]
        public string NickName { get; set; }

        /// <summary>
        /// 战斗次数
        /// </summary>
        [ProtoMember(3)]
        public int CombatNum
        {
            get;
            set;
        }

        /// <summary>
        /// 战斗伤害
        /// </summary>
        [ProtoMember(4)]
        public int DamageNum
        {
            get;
            set;
        }
        /// <summary>
        /// 鼓舞加成
        /// </summary>
        [ProtoMember(5)]
        public double InspirePercent
        {
            get;
            set;
        }

        /// <summary>
        /// 复活鼓舞加成
        /// </summary>
        [ProtoMember(6)]
        public double ReliveInspirePercent
        {
            get;
            set;
        }

        /// <summary>
        /// 复活次数
        /// </summary>
        [ProtoMember(7)]
        public int ReliveNum
        {
            get;
            set;
        }
        [ProtoMember(8)]
        public bool IsRelive
        {
            get;
            set;
        }
        /// <summary>
        /// 复活开始时间
        /// </summary>
        [ProtoMember(9)]
        public DateTime ReliveBeginDate
        {
            get;
            set;
        }

        public int CodeTime
        {
            get
            {
                int codeTime = ConfigEnvSet.GetInt("BossCombat.ReliveCodeTime");
                if (IsRelive)
                {
                    DateTime endTime = ReliveBeginDate.AddSeconds(codeTime);
                    if (endTime > DateTime.Now)
                    {
                        return (int)Math.Floor((endTime - DateTime.Now).TotalSeconds);
                    }
                    else
                    {
                        IsRelive = false;
                    }
                }
                return 0;
            }
        }

        public override int CompareTo(BaseEntity itemData)
        {
            BossUser other = itemData as BossUser;
            int result = 0;
            if (this == null && other == null) return 0;
            if (this != null && other == null) return 1;
            if (this == null && other != null) return -1;

            if (this.DamageNum > other.DamageNum)
            {
                result = - 1;
            }
            else if (this.DamageNum < other.DamageNum)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        protected override object this[string index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override int GetIdentityId()
        {
            return DefIdentityId;
        }
    }
}