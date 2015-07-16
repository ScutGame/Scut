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

namespace ZyGames.Framework.Net
{
    /// <summary>
    /// 数据库过滤条件
    /// </summary>
    public class DbDataFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public DbDataFilter()
        {
            Parameters = new Parameters();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public DbDataFilter(int capacity)
            : this()
        {
            Capacity = capacity;
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 获取结果的容量
        /// </summary>
        public int Capacity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取或设置条件
        /// </summary>
        public string Condition
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置排序字段
        /// </summary>
        public string OrderColumn
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置参数, 数据名不需要@前缀
        /// </summary>
        public Parameters Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// 动态表名指定的日期
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}