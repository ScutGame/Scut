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

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 娓犻亾鍟嗘彁渚涚殑Sdk閰嶇疆
    /// </summary>
    internal class GameSdkSetting
    {
        /// <summary>
        /// 娓犻亾鍙?
        /// </summary>
        public string RetailId { get; set; }

        /// <summary>
        /// UC閰嶅鏄痗pId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// UC閰嶅鏄痑piKey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GameId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServerId { get; set; }
    }
}