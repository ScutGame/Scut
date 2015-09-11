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
    /// 游戏服信息对象
    /// </summary>
    public class ServerInfo  : MemoryEntity, IComparable<ServerInfo>
    {
        public ServerInfo()
        {
            EnableDate = MathUtils.SqlMinDate;
        }

        /// <summary>
        /// 游戏服编号
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏编号
        /// </summary>
        public int GameID
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏服名称
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏服访问地址
        /// </summary>
        public string ServerUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 内网程序访问地址
        /// </summary>
        public string IntranetAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 分服状态，停服、顺畅、拥挤等
        /// </summary>
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// 活跃人数
        /// </summary>
        public int ActiveNum
        {
            get;
            set;
        }

        /// <summary>
        /// 排序的权重比
        /// </summary>
        public int Weight
        {
            get;
            set;
        }

        /// <summary>
        /// 合服转向目录务器
        /// </summary>
        public int TargetServer
        {
            get;
            set;
        }

        /// <summary>
        /// 是否正式开服
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }

        /// <summary>
        /// 正式开服时间
        /// </summary>
        public DateTime EnableDate
        {
            get;
            set;
        }

        #region IComparable<ServerInfo> 成员

        /// <summary>
        /// 排序规则：权重高到低，活跃数低到高排序
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ServerInfo other)
        {
            int result = other.Weight.CompareTo(Weight);
            if (result == 0)
            {
                //升序
                result = this.ActiveNum.CompareTo(other.ActiveNum);
                if (result == 0)
                {
                    result = other.ID.CompareTo(this.ID);
                }
            }
            return result;
        }

        #endregion
    }
}