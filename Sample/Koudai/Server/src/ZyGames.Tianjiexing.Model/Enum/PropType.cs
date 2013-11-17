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
    public enum PropType
    {
        /// <summary>
        /// 加血
        /// </summary>
        Blood = 1,
        /// <summary>
        /// 加速
        /// </summary>
        Speed,
        /// <summary>
        /// 变身
        /// </summary>
        Transfiguration,
        /// <summary>
        /// 喇叭
        /// </summary>
        Speaker,
        /// <summary>
        /// 玩家改名
        /// </summary>
        PlayersRenamed,

        /// <summary>
        /// 工公改名
        /// </summary>
        GuildRenamed,

        /// <summary>
        /// 公会增加成员
        /// </summary>
        GuildPlus,

        /// <summary>
        /// 双倍收益道具
        /// </summary>
        DoubleDaoji,

        /// <summary>
        /// 战力加成道具
        /// </summary>
        Combat,

        /// <summary>
        /// 法宝升级道具
        /// </summary>
        TrumpUp,

        /// <summary>
        /// 法宝延寿道具
        /// </summary>
        LifeExtension,

        /// <summary>
        /// 法宝洗练道具
        /// </summary>
        Xilian,
    }
}