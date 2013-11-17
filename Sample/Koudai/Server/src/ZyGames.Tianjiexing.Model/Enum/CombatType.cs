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
    public enum CombatType
    {
        /// <summary>
        /// 玩家竞技
        /// </summary>
        User = 1,
        /// <summary>
        /// 普通副本
        /// </summary>
        Plot,
        /// <summary>
        /// 扫荡
        /// </summary>
        Sweep,
        /// <summary>
        /// Boss
        /// </summary>
        BossPlot,
        /// <summary>
        /// 多人副本5
        /// </summary>
        MultiPlot,
        /// <summary>
        /// 领土战
        /// </summary>
        Country,
        /// <summary>
        /// 公会战
        /// </summary>
        Guild,
        /// <summary>
        /// 宠物赛跑
        /// </summary>
        PetRun,
        /// <summary>
        /// 天地劫
        /// </summary>
        Kalpa,
        /// <summary>
        /// 圣吉塔10
        /// </summary>
        ShengJiTa,
        /// <summary>
        /// 考古副本
        /// </summary>
        KaoGuPlot
    }
}