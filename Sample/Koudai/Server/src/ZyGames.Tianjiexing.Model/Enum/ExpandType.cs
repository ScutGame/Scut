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
    public enum ExpandType
    {
        /// <summary>
        /// 开启强化队列
        /// </summary>
        KaiQiQiangHuaDuiLie = 1,

        /// <summary>
        /// 升级魔术同时花费晶石消除冷却时间2
        /// </summary>
        MoShuXiaoChuLengQueShiJian,

        /// <summary>
        /// 培养增加更多高级培养模式3
        /// </summary>
        GaoJiPeiYang,

        /// <summary>
        /// 获得伙伴跟随功能4
        /// </summary>
        HuoPangGenSui,

        /// <summary>
        /// 刷新神秘商店5
        /// </summary>
        ShuaXinShenMiShangDian,

        /// <summary>
        /// BOSS战欲火重生功能6
        /// </summary>
        BossChongSheng,

        /// <summary>
        /// 晶石鼓舞7
        /// </summary>
        JingShiGuWu,

        /// <summary>
        /// 自动参加领土战8
        /// </summary>
        ZiDongLingTuZhan,

        /// <summary>
        /// 天地劫重置爬塔9
        /// </summary>
        ChongZhiPaTa,

        /// <summary>
        /// 消耗晶石宠物赛跑直接加速和直接完成10
        /// </summary>
        ChongWuSaZhiJieWanCheng,

        /// <summary>
        /// 强化时使用晶石消除冷却时间11
        /// </summary>
        EquXiaoChuLengQueShiJian,

        /// <summary>
        /// 制作卷轴直接完成12
        /// </summary>
        JuanZouZhiJieWanCheng,

        /// <summary>
        /// 宠物赛跑召唤狮子13
        /// </summary>
        ZhaoHuanShiZi,

        /// <summary>
        /// 消耗晶石刷出最高星级每日任务14
        /// </summary>
        ShuanChuMaxXingJiRenWu,

        /// <summary>
        /// 药园土地升级15
        /// </summary>
        YaoYuanLandUp,

        /// <summary>
        /// 药园刷出满星级种子16
        /// </summary>
        YaoYuanShuanXinManXingJi,

        /// <summary>
        /// 自动猎命功能17
        /// </summary>
        ZiDongLieMing,

        /// <summary>
        /// 晶石猎命18
        /// </summary>
        JinShiLieMing,

        /// <summary>
        /// 装备批量灵魂功能19
        /// </summary>
        EquPiLiangLingHun,

        /// <summary>
        /// 替身娃娃功能20
        /// </summary>
        TiShenWaWa,

        /// <summary>
        /// 召唤宙斯21
        /// </summary>
        ZhaoHuanZhouSi,

        /// <summary>
        /// 永久消耗CD22
        /// </summary>
        YongJiuXiaoHaoCD,

        /// <summary>
        /// 药园批量种植23
        /// </summary>
        YaoYuanPiLiangPlant,

        /// <summary>
        /// 自动进入金矿洞24
        /// </summary>
        ZiDongJinRuJinKuangDong,

        /// <summary>
        /// 自动购买精力25
        /// </summary>
        ZiDongGouMaiJingLi,

        /// <summary>
        /// 直接使用药剂26
        /// </summary>
        UseYaoJi,

        /// <summary>
        /// 竞技场开启晶石挑战次数27
        /// </summary>
        JingJiChangKaiQiTiaoZhanNum,

        /// <summary>
        /// 帮派成员可上苏合香28
        /// </summary>
        GuildMemberShangSuHeXiang,

        /// <summary>
        /// 拜祭关公上苏合香29
        /// </summary>
        BaiJiGuangGongShangSuHeXiang,

        /// <summary>
        /// 修炼延长到24小时	30　
        /// </summary>
        XiuLianYanChangErShiSiXiaoShi,

        /// <summary>
        /// 帮派成员可上天木香31
        /// </summary>
        GuildMemberShangTianMuXiang,

        /// <summary>
        /// 拜祭关公上天木香32
        /// </summary>
        BaiJiGuangGongShangTianMuXiang,

        /// <summary>
        /// 重置精英副本33
        /// </summary>
        ZhongZhiJingYingPlot,

        /// <summary>
        /// 每日任务开启直接完成34
        /// </summary>
        DailyRenWuZhiJieWanCheng,

        /// <summary>
        /// 开启至尊培养35
        /// </summary>
        KaiQiZhiZunPeiYang,

        /// <summary>
        /// 刷新每日任务36
        /// </summary>
        RefreashDailyTask,

        /// <summary>
        /// 第二次重置精英副本37
        /// </summary>
        SecondZhongZhiJingYingPlot,

        /// <summary>
        /// 第二次刷新天地劫功能38
        /// </summary>
        SecondRefreshKalpa,

        /// <summary>
        /// 晶石传承39
        /// </summary>
        GoldHeritage,

        /// <summary>
        /// 至尊传承40
        /// </summary>
        ExtremeHeritage,

        /// <summary>
        /// 重置英雄副本41
        /// </summary>
        HeroRefreshPlot,

        /// <summary>
        /// 第二次重置英雄副本42
        /// </summary>
        HeroSecondRefreshPlot,

    }
}