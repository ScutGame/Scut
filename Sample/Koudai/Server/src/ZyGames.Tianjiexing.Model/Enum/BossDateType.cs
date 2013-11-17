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
    /// <summary>
    /// boss挑战时间
    /// </summary>
    public enum BossDateType
    {
        /// <summary>
        /// 周一晚上 21:00
        /// </summary>
        Monday = 1,

        /// <summary>
        /// 周二晚上 21:00
        /// </summary>
        Tuesday,

        /// <summary>
        /// 周三晚上 21:00
        /// </summary>
        Wednesday,

        /// <summary>
        /// 周四晚上 21:00
        /// </summary>
        Thursday,

        /// <summary>
        /// 周五晚上 21:00
        /// </summary>
        Friday,

        /// <summary>
        /// 周六晚上 21:00
        /// </summary>
        Saturday,

        /// <summary>
        /// 周日下午 16:00
        /// </summary>
        SundayAfternoon,

        /// <summary>
        /// 周日晚上 22:00
        /// </summary>
        Sunday
    }
}