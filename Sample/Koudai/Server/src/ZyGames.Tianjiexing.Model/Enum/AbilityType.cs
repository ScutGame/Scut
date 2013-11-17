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
    public enum AbilityType
    {
        Empty = 0,
        /// <summary>
        /// 生命
        /// </summary>
        ShengMing = 1,

        /// <summary>
        /// 物理攻击
        /// </summary>
        WuLiGongJi,

        /// <summary>
        /// 魂技攻击
        /// </summary>
        HunJiGongJi,

        /// <summary>
        /// 魔法攻击
        /// </summary>
        MoFaGongJi,

        /// <summary>
        /// 5 物理防御
        /// </summary>
        WuLiFangYu,

        /// <summary>
        /// 魂技防御
        /// </summary>
        HunJiFangYu,

        /// <summary>
        /// 魔法防御
        /// </summary>
        MoFaFangYu,

        /// <summary>
        /// 暴击
        /// </summary>
        BaoJi,

        /// <summary>
        /// 命中
        /// </summary>
        MingZhong,

        /// <summary>
        /// 10 破击
        /// </summary>
        PoJi,

        /// <summary>
        /// 韧性
        /// </summary>
        RenXing,

        /// <summary>
        /// 闪避
        /// </summary>
        ShanBi,

        /// <summary>
        /// 格挡
        /// </summary>
        GeDang,

        /// <summary>
        /// 必杀
        /// </summary>
        BiSha,

        /// <summary>
        /// 15 眩晕
        /// </summary>
        Yunxuan = 15,
        /// <summary>
        /// 昏睡
        /// </summary>
        Hunshui,
        /// <summary>
        /// 冰冻
        /// </summary>
        Bingdong,
        /// <summary>
        /// 迷失
        /// </summary>
        Mishi,
        /// <summary>
        /// 定身
        /// </summary>
        Dingshen,
        /// <summary>
        /// 20 中毒
        /// </summary>
        Zhongdu,
        /// <summary>
        /// 出血
        /// </summary>
        Chuxie,
        /// <summary>
        /// 气势
        /// </summary>
        Qishi,
        /// <summary>
        /// 混乱
        /// </summary>
        Hunluan,
        /// <summary>
        /// 绝对防御
        /// </summary>
        Jueduifangyu,

        /// <summary>
        /// 25 先攻值
        /// </summary>
        FirstStrike,

        /// <summary>
        /// 力量
        /// </summary>
        PowerNum,
        /// <summary>
        /// 魂力
        /// </summary>
        SoulNum,
        /// <summary>
        /// 智力
        /// </summary>
        IntelligenceNum,

        /// <summary>
        /// 暴击加成
        /// </summary>
        BaoJiJiaCheng,

        /// <summary>
        /// 30被暴击减免
        /// </summary>
        IsBaoJiReduce,

        /// <summary>
        /// 复活
        /// </summary>
        Resurrect,

        /// <summary>
        /// 攻击加血
        /// </summary>
        AttackLife,

        /// <summary>
        /// 狂怒 -- 生命低于x%，战力加成
        /// </summary>
        Furious,

        /// <summary>
        /// 普通攻击破防
        /// </summary>
        NormalAttackPoFang,

        /// <summary>
        /// 35 攻击破盾
        /// </summary>
        AttackPoDun,

       /// <summary>
       /// 反伤
       /// </summary>
        FanShang,

        /// <summary>
        /// 替换佣兵
        /// </summary>
        ReplaceGeneral,
        /// <summary>
        /// 潜力点
        /// </summary>
        Potential
    }
}