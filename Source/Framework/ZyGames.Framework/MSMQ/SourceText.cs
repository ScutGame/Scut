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

namespace ZyGames.Framework.MSMQ
{
    /// <summary>
    /// 文本资源
    /// </summary>
    internal class SourceText
    {
        /// <summary>
        /// {0}天{1}小时{2}分
        /// </summary>
        public const string St1021_Days = "{0}天{1}小时{2}分";
        /// <summary>
        /// {0}小时{1}分
        /// </summary>
        public const string St1021_Hours = "{0}小时{1}分";
        /// <summary>
        /// {0}分
        /// </summary>
        public const string St1021_Minutes = "{0}分";
        /// <summary>
        /// LockMsmq
        /// </summary>
        public static string StLockMsmq = "LockMsmq";
    }
}