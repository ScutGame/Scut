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
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// 数据库映射表
    /// </summary>
    public class SchemaTable
    {
        /// <summary>
        /// 
        /// </summary>
        public SchemaTable()
            : this(AccessLevel.ReadWrite, CacheType.Dictionary, true)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessLevel">访问级别</param>
        /// <param name="cacheType">缓存的类型</param>
        /// <param name="isStoreInDb">是否存储到DB</param>
        public SchemaTable(AccessLevel accessLevel, CacheType cacheType, bool isStoreInDb)
        {
            AccessLevel = accessLevel;
            CacheType = cacheType;
            IsStoreInDb = isStoreInDb;
            Keys = new string[0];
            _columns = new ConcurrentDictionary<string, SchemaColumn>();
        }

        /// <summary>
        /// 绑定的实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 访问权限级别
        /// </summary>
        public AccessLevel AccessLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 存储在缓存的类型
        /// </summary>
        public CacheType CacheType { get; set; }

        /// <summary>
        /// Whether synchronous entity.
        /// </summary>
        public bool IsEntitySync { get; set; }

        /// <summary>
        /// 是否存储到DB（可从Db取数据，但不能更新回DB）
        /// </summary>
        public bool IsStoreInDb
        {
            get;
            set;
        }

        /// <summary>
        /// 是否持久化到DB，当从Redis内存移除后
        /// </summary>
        public bool IsPersistence { get; set; }

        /// <summary>
        /// 生命周期，单位秒
        /// </summary>
        public int PeriodTime { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 数据库配置连接KEY
        /// </summary>
        public string ConnectKey
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库配置连接类型:Sql,mysql
        /// </summary>
        public string ConnectionProviderType
        {
            get;
            set;
        }
        /// <summary>
        /// 数据库配置连接字串
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是日志表
        /// </summary>
        public bool IsLog { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string[] Keys
        {
            get;
            set;
        }

        /// <summary>
        /// 实体名
        /// </summary>
        public string Name
        {
            get
            {
                if (IsLog)
                {
                    string format = EntitySchemaSet.LogTableNameFormat.Replace("$date", DateTime.Now.ToString("yyyyMM"));
                    return string.Format(format, SpecialName);
                }
                return SpecialName;
            }
            set
            {
                SpecialName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the special.
        /// </summary>
        /// <value>The name of the special.</value>
        internal string SpecialName
        {
            get;
            private set;
        }

        /// <summary>
        /// 绑定分组字段
        /// </summary>
        public string PersonalName
        {
            get;
            set;
        }

        private ConcurrentDictionary<string, SchemaColumn> _columns;

        /// <summary>
        /// Get schema column.
        /// </summary>
        internal ConcurrentDictionary<string, SchemaColumn> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Get schema column to list.
        /// </summary>
        /// <returns></returns>
        public List<SchemaColumn> GetColumns()
        {
            return _columns.Values.OrderBy(col => col.Id).ToList();
        }

        /// <summary>
        /// Get schema column name to list.
        /// </summary>
        /// <returns></returns>
        public List<string> GetColumnNames()
        {
            return _columns.Values.OrderBy(col => col.Id).Select(col => col.Name).ToList();
        }

        /// <summary>
        /// 条件，不需要加Where
        /// </summary>
        public string Condition
        {
            get;
            set;
        }
        /// <summary>
        /// 排序列,多个以逗号分隔
        /// </summary>
        public string OrderColumn
        {
            get;
            set;
        }

    }
}