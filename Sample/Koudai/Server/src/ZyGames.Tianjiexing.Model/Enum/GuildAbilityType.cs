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

namespace ZyGames.Tianjiexing.Model.Enum
{
    /// <summary>
    /// 公会技能效果类型枚举
    /// </summary>
    public enum GuildAbilityType
    {
        /// <summary>
        /// 增加副本金钱加成
        /// </summary>
        CoinNum = 1,
        /// <summary>
        /// 增加副本阅历加成
        /// </summary>
        ExpNum,
        /// <summary>
        /// 增加副本经验加成
        /// </summary>
        Experience,
        /// <summary>
        /// 增加主角力量加成
        /// </summary>
        PowerNum,
        /// <summary>
        /// 增加主角魂力加成
        /// </summary>
        SoulNum,
        /// <summary>
        /// 增加主角智力加成
        /// </summary>
        IntellectNum,
        /// <summary>
        /// 帮会战时该技能每等级提高全角色普通和法术攻击
        /// </summary>
        AttackNum,
        /// <summary>
        /// 每等级提高角色生命最大值
        /// </summary>
        LifeNum,
        /// <summary>
        /// 每等级提高帮会占领城市获得的特产数量
        /// </summary>
        SpecialtyNum,
    }
}