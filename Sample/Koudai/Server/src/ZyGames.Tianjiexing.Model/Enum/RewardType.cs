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

namespace ZyGames.Tianjiexing.Model
{
    /// <summary>
    /// 奖励类型
    /// </summary>
    public enum RewardType
    {
        /// <summary>
        /// 金币
        /// </summary>
        GameGoin = 1,

        /// <summary>
        /// 声望
        /// </summary>
        Obtion,

        /// <summary>
        /// 阅历
        /// </summary>
        ExpNum,

        /// <summary>
        /// 精力
        /// </summary>
        EnergyNum,

        /// <summary>
        /// 经验
        /// </summary>
        Experience = 5,

        /// <summary>
        /// 晶石奖励
        /// </summary>
        Gold,

        /// <summary>
        /// 物品奖励
        /// </summary>
        Item,

        /// <summary>
        /// 命运水晶品质概率奖励
        /// </summary>
        CrystalType,

        /// <summary>
        /// 命运水晶ID
        /// </summary>
        CrystalId,

        /// <summary>
        /// 10 灵件
        /// </summary>
        Spare,

        /// <summary>
        /// 附魔符
        /// </summary>
        Enchant,

        /// <summary>
        /// 12再来一次
        /// </summary>
        Again,

        /// <summary>
        /// 心情愉快
        /// </summary>
        Mood,

        /// <summary>
        /// 充值返还
        /// </summary>
        Recharge,

        /// <summary>
        /// 15 根据职业物品奖励
        /// </summary>
        ItemByJob,

        /// <summary>
        /// 荣誉值
        /// </summary>
        HonourNum,

        /// <summary>
        /// 17 vip晶石待遇
        /// </summary>
        PayGold,

        /// <summary>
        /// 怪物卡
        /// </summary>
        MonsterCard,

        /// <summary>
        /// 佣兵魂魄
        /// </summary>
        GeneralSoul,

        /// <summary>
        /// 技能
        /// </summary>
        Ability,
    }
}