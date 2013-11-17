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
    public enum ActiveType
    {
        /// <summary>
        /// 庄园功能
        /// </summary>
        ZhuangYuan = 1,

        /// <summary>
        /// 每日探险
        /// </summary>
        MeiYiTanXian,

        /// <summary>
        /// 每日送精力
        /// </summary>
        SendEnergyEveryDay,

        /// <summary>
        /// 金矿洞
        /// </summary>
        JinKuangDon,

        /// <summary>
        /// 充值返利
        /// </summary>
        ChongZhiFanLi,

        /// <summary>
        /// 升级送好礼
        /// </summary>
        ShengJiSongHaoLi,

        /// <summary>
        /// 首次充值奖励
        /// </summary>
        ShouCiChongZhi,

        /// <summary>
        /// 商城打折活动
        /// </summary>
        ShangChengDaZhe,

        /// <summary>
        /// 双倍收益活动
        /// </summary>
        ShuangBeiShouyi,

        /// <summary>
        /// 招募佣兵送灵魂
        /// </summary>
        SongLingHun,
    }
}