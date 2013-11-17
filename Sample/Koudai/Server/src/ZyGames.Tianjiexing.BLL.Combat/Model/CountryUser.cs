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
using ZyGames.Base.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Base.Entity;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 参与国家领土战的用户
    /// </summary>
    public class CountryUser : BaseEntity<CountryUser>
    {
        public CountryUser()
        {

        }
        public CountryType GroupType { get; set; }

        /// <summary>
        /// 玩家ID
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        public bool IsAdvanced { get; set; }

        public short UserLv
        {
            get;
            set;
        }

        public short UserVipLv
        {
            get;
            set;
        }

        public int UserExpNum
        {
            get;
            set;
        }
        /// <summary>
        /// 玩家Name
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 最高连胜
        /// </summary>
        public int MaxWinNum
        {
            get;
            set;
        }

        /// <summary>
        /// 当前连胜
        /// </summary>
        public int CurrWinNum
        {
            get;
            set;
        }

        /// <summary>
        /// 赢的场数
        /// </summary>
        public int WinCount
        {
            get;
            set;
        }
        /// <summary>
        /// 输的场数
        /// </summary>
        public int FailCount
        {
            get;
            set;
        }

        /// <summary>
        /// 获得声望
        /// </summary>
        public int ObtainNum
        {
            get;
            set;
        }
        /// <summary>
        /// 获得金币
        /// </summary>
        public int GameCoin
        {
            get;
            set;
        }
        /// <summary>
        /// 刷新战斗时间
        /// </summary>
        public DateTime Refresh
        {
            get;
            set;
        }
        /// <summary>
        /// 鼓舞加成
        /// </summary>
        public double InspirePercent
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有战斗
        /// </summary>
        public bool HasCombat { get; set; }

        /// <summary>
        /// 参战状态0:未参战，1:等待状态，2:战斗状态
        /// </summary>
        public short Status
        {
            get;
            set;
        }
        /// <summary>
        /// 战报版本
        /// </summary>
        public int Version { get; set; }

        public override object this[string index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}