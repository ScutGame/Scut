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
    public enum FestivalType
    {
        /// <summary>
        /// 无
        /// </summary>
        Default = 0,
        /// <summary>
        /// 登录领好礼
        /// </summary>
        LoginReceive = 1,

        /// <summary>
        /// 充值累计
        /// </summary>
        PayCount = 2,

        /// <summary>
        /// 普通副本活动
        /// </summary>
        NormalCount,

        /// <summary>
        /// 精英副本活动
        /// </summary>
        EliteCount,

        /// <summary>
        /// 5 渠道登录
        /// </summary>
        RetailLogin,

        /// <summary>
        /// 购买精力活动
        /// </summary>
        PurchasedEnergy,

        /// <summary>
        /// 庄园加成
        /// </summary>
        ManorAddition,

        /// <summary>
        /// 晶石消费
        /// </summary>
        SparConsumption,

        /// <summary>
        /// 金币消费
        /// </summary>
        GameCoin,

        /// <summary>
        /// 10 精力消耗
        /// </summary>
        Energy,

        /// <summary>
        /// 龟兔赛跑活动奖励
        /// </summary>
        TortoiseHare,

        /// <summary>
        /// vip活动
        /// </summary>
        VIPFestival,
        
        /// <summary>
        /// 首充奖励
        /// </summary>
        FirstReward,

        /// <summary>
        /// 庄园种植
        /// </summary>
        ManorPlant,

        /// <summary>
        /// 15 每日探险
        /// </summary>
        ExpeditionEveryDay,

        /// <summary>
        /// 每日送精力
        /// </summary>
        GiveEnergyEveryDay,

        /// <summary>
        /// 金矿洞探险
        /// </summary>
        GoldHoleExpedition,

        /// <summary>
        /// 充值返利
        /// </summary>
        PayReward,

        /// <summary>
        /// 登入送好礼
        /// </summary>
        LoginGiveGift,

        /// <summary>
        /// 20招募佣兵送魂魄
        /// </summary>
        RecruitGeneral,

        /// <summary>
        /// 升级送好礼
        /// </summary>
        UpgradeGiveGift,

        /// <summary>
        /// 首次充值晶石翻倍
        /// </summary>
        FirstPayDoubleSpar,

        /// <summary>
        /// 商城打折活动
        /// </summary>
        StoreDiscount,

        /// <summary>
        /// 副本双倍掉落
        /// </summary>
        DuplicateDropDouble,

        /// <summary>
        /// 25累计消费送礼
        /// </summary>
        PayAccumulation,

        /// <summary>
        /// 祈祷
        /// </summary>
        Pray,

        /// <summary>
        /// 精灵祝福
        /// </summary>
        SpiritBlessing,
        /// <summary>
        /// 考古
        /// </summary>
        KaoGu,

        /// <summary>
        /// 通关考古副本活动
        /// </summary>
        Archeology,

    }
}