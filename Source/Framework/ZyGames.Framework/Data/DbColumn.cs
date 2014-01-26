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

namespace ZyGames.Framework.Data
{
    /// <summary>
    /// 数据列对象
    /// </summary>
    public class DbColumn
    {
        /// <summary>
        /// 
        /// </summary>
        public DbColumn()
        {
            Isnullable = true;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsKey { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// decimal类型指精度范围
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool Isnullable { get; set; }
        /// <summary>
        /// 是否是修改列，False则新增列
        /// </summary>
        public bool IsModify { get; set; }

		/// <summary>
		/// 是否是自增列
		/// </summary>
		/// <value><c>true</c> if this instance is identity; otherwise, <c>false</c>.</value>
		public bool IsIdentity{ get; set; }

        /// <summary>
        /// Db映射类型
        /// </summary>
        public string DbType
        {
            get;
            set;
        }
    }
}