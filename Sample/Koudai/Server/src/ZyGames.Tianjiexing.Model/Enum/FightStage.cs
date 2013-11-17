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
    public enum FightStage
    {
        Close = -1,
        /// <summary>
        /// 报名
        /// </summary>
        Apply = 0,
        /// <summary>
        /// 准备阶段
        /// </summary>
        Ready = 1,
        /// <summary>
        /// 8强赛等待时间
        /// </summary>
        Wait = 2,
        /// <summary>
        ///  8强赛
        /// </summary>
        quarter_final = 3,

        /// <summary>
        /// 半决赛等待
        /// </summary>
        semi_Wait = 4,
        /// <summary>
        /// 半决赛
        /// </summary>
        semi_final = 5,

        /// <summary>
        /// 决赛等待
        /// </summary>
        final_Wait = 6,
        /// <summary>
        /// 决赛
        /// </summary>
        final = 7,
        /// <summary>
        /// 冠军
        /// </summary>
        champion = 8,


    }
}