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
using ZyGames.Tianjiexing.Model;
using ZyGames.Base.Entity;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 参与BOSS战的用户
    /// </summary>
    public class BossUser : BaseEntity<BossUser>
    {
        public string UserId { get; set; }

        public string NickName { get; set; }

        /// <summary>
        /// 战斗次数
        /// </summary>
        public int CombatNum
        {
            get;
            set;
        }

        /// <summary>
        /// 战斗伤害
        /// </summary>
        public int DamageNum
        {
            get;
            set;
        }
        /// <summary>
        /// 鼓舞加成
        /// </summary>
        public double InspirePercent
        {
            get;
            set;
        }

        /// <summary>
        /// 复活鼓舞加成
        /// </summary>
        public double ReliveInspirePercent
        {
            get;
            set;
        }

        /// <summary>
        /// 复活次数
        /// </summary>
        public int ReliveNum
        {
            get;
            set;
        }
        public bool IsRelive
        {
            get;
            set;
        }
        /// <summary>
        /// 复活开始时间
        /// </summary>
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

        public override int CompareTo(BossUser other)
        {
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

        public override object this[string index]
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
    }
}