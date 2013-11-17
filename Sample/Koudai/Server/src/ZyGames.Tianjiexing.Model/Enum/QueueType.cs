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
    public enum QueueType
    {
        Nomal = 0,
        /// <summary>
        /// 装备强化
        /// </summary>
        EquipmentStrong = 1,

        /// <summary>
        /// 魔术升级
        /// </summary>
        MagicStrong,

        /// <summary>
        /// 修练
        /// </summary>
        XiuLian,

        /// <summary>
        /// 扫荡
        /// </summary>
        SaoDang,

        /// <summary>
        /// 竞技场挑战
        /// </summary>
        JingJiTiaoZhan,

        /// <summary>
        /// 每日探险
        /// </summary>
        TianXianStrong,

        /// <summary>
        /// 庄园种植
        /// </summary>
        ZhongZhiStrong,

        /// <summary>
        /// 精力恢复
        /// </summary>
        EnergyHuiFu,

        /// <summary>
        /// 退出工会后8小时可重新加入
        /// </summary>
        TuiChuGongHui,

        /// <summary>
        /// 变身
        /// </summary>
        Transfiguration,

        /// <summary>
        /// 圣水恢复
        /// </summary>
        ShengShuiHuiFu,

        /// <summary>
        /// 饱食度恢复
        /// </summary>
        FeelHunger,

        /// <summary>
        /// 十里挑一
        /// </summary>
        ShiLiTiaoYi,

        /// <summary>
        /// 百里挑一
        /// </summary>
        BaiLiTiaoYi,

        /// <summary>
        /// 千载难逢
        /// </summary>
        Golden,
    }
}