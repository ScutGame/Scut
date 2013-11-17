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
    public enum PlotType
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 精英
        /// </summary>
        Elite,

        /// <summary>
        /// 世界Boss
        /// </summary>
        WorldBoss,

        /// <summary> 
        /// 多人副本
        /// </summary>
        MorePlot,

        /// <summary> 
        /// 多人副本金币大作战
        /// </summary>
        MorePlotCoin,

        /// <summary>
        /// 多人副本精力大作战
        /// </summary>
        MorePlotEnergy,

        /// <summary>
        /// 天地劫副本
        /// </summary>
        Kalpa,

        /// <summary>
        /// 英雄副本
        /// </summary>
        HeroPlot,
        /// <summary>
        /// 考古副本
        /// </summary>
        KaoGuPlot
    }
}