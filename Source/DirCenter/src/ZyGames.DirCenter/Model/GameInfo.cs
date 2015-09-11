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
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.DirCenter.Model
{
    /// <summary>
    /// 游戏信息对象
    /// </summary>
    public class GameInfo : MemoryEntity
    {
        public GameInfo()
        {
            ReleaseDate = MathUtils.SqlMinDate;
            Currency = "";
            GameWord = "";
            AgentsID = "";
            PayStyle = "";
            SocketServer = "";
        }

        /// <summary>
        /// 游戏编号
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏名
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 代币名称（晶石，元宝）
        /// </summary>
        public string Currency
        {
            get;
            set;
        }

        /// <summary>
        /// 倍率，1元=n代币
        /// </summary>
        public decimal Multiple
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏拼音缩写
        /// </summary>
        public string GameWord
        {
            get;
            set;
        }

        /// <summary>
        /// 代理商编号官网：0000;触控：2001
        /// </summary>
        public string AgentsID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否正式发布
        /// </summary>
        public bool IsRelease
        {
            get;
            set;
        }

        /// <summary>
        /// 正式发布时间（年月日时分）
        /// </summary>
        public DateTime ReleaseDate
        {
            get;
            set;
        }

        /// <summary>
        /// 充值页面样式
        /// </summary>
        public string PayStyle
        {
            get;
            set;
        }

        /// <summary>
        /// SOCKET游戏解析域名
        /// </summary>
        public string SocketServer
        {
            get;
            set;
        }

        /// <summary>
        /// SOCKET端口
        /// </summary>
        public int SocketPort
        {
            get;
            set;
        }

    }
}