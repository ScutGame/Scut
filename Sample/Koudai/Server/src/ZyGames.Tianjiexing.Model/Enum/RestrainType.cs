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
    public enum RestrainType
    {
        /// <summary>
        /// 日常活动
        /// </summary>
        RiChangHuoDong = 1,

        /// <summary>
        /// 免费猎命
        /// </summary>
        MianFeiLieMing,

        /// <summary>
        /// 挖金矿
        /// </summary>
        WaJinKuang,

        /// <summary>
        /// 购买精力
        /// </summary>
        GouMaiJingLi,

        /// <summary>
        /// 购买圣水
        /// </summary>
        GouMaiShengShui,

        /// <summary>
        /// 帮派上香
        /// </summary>
        BangPaiShangXiang,

        /// <summary>
        /// 关公上香
        /// </summary>
        GuangGongShangXiang,

        /// <summary>
        /// 领取俸禄
        /// </summary>
        LingQuFengLu,

        /// <summary>
        /// 竞技场挑战
        /// </summary>
        JingJiChangTiaoZhan,

        /// <summary>
        /// 精英副本-10
        /// </summary>
        JingYingPlot,

        /// <summary>
        /// 每日副本战斗跳过
        /// </summary>
        PlotGoto,

        /// <summary>
        /// 每日公会祈祷次数
        /// </summary>
        Prayer,

        /// <summary>
        /// 每日剩余探险次数
        /// </summary>
        Explore,

        /// <summary>
        /// 每日剩余种植次数
        /// </summary>
        Plant,
        /// <summary>
        /// 每日宠物赛跑次数
        /// </summary>
        PetRun,
        /// <summary>
        /// 每日宠物拦截次数
        /// </summary>
        PetIntercept,
        /// <summary>
        /// 每日宠物助好友完面成次数-17
        /// </summary>
        PetHelp,

        /// <summary>
        /// 刷新天地劫次数
        /// </summary>
        RefreshKalpa,

        /// <summary>
        /// 刷新天地劫上一层次数19
        /// </summary>
        RefreshLastKalpa,

        /// <summary>
        /// 好感度晶石赠送次数 20
        /// </summary>
        PresentationGoldNum,

        /// <summary>
        /// 重置英雄副本次数21
        /// </summary>
        HeroRefreshNum,

        /// <summary>
        /// 新手卡激活次数
        /// </summary>
        NewHand,

        /// <summary>
        /// 龙神圣水
        /// </summary>
        DragonHolyWater = 23,

        /// <summary>
        /// 竞技场神秘奖励
        /// </summary>
        SportsPrize = 24,

    }
}