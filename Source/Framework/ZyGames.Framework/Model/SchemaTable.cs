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
using System.Globalization;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// StorageType
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        ReadOnlyDB = 1,
        /// <summary>
        /// 
        /// </summary>
        ReadWriteDB = 2,
        /// <summary>
        /// 
        /// </summary>
        WriteOnlyDB = 4,
        /// <summary>
        /// 
        /// </summary>
        ReadOnlyRedis = 8,
        /// <summary>
        /// 
        /// </summary>
        ReadWriteRedis = 16
    }
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
        /// StorageType
        /// </summary>
        public StorageType StorageType { get; set; }

        /// <summary>
        /// 存储在缓存的类型
        /// </summary>
        public CacheType CacheType { get; set; }

        /// <summary>
        /// Whether synchronous entity, for sync mode use.
        /// </summary>
        public bool IsEntitySync { get; set; }

        /// <summary>
        /// 是否持久化到DB，当从Redis内存移除后
        /// </summary>
        [Obsolete("", true)]
        public bool IsPersistence { get; set; }

        /// <summary>
        /// 自增的启始编号
        /// </summary>
        public long IncreaseStartNo { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; set; }

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
        /// 表名的格式:$date[yyyyMMdd] or $week
        /// </summary>
        public string NameFormat { get; set; }

        /// <summary>
        /// 是否是内部使用的
        /// </summary>
        public bool IsInternal { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMutilKey { get { return Keys != null && Keys.Length > 1; } }
        /// <summary>
        /// 主键
        /// </summary>
        public string[] Keys
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entity name.
        /// </summary>
        public string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// 绑定分组字段
        /// </summary>
        public string PersonalName
        {
            get;
            set;
        }
        /// <summary>
        /// index column
        /// </summary>
        public string[] Indexs { get; set; }

        private ConcurrentDictionary<string, SchemaColumn> _columns;

        internal bool HasObjectColumns;

        /// <summary>
        /// Get schema column.
        /// </summary>
        internal ConcurrentDictionary<string, SchemaColumn> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Get ordered schema columns.
        /// </summary>
        /// <returns></returns>
        public List<SchemaColumn> GetColumns()
        {
            return _columns.Values.OrderBy(col => col.Id).ToList();
        }
        /// <summary>
        /// Get serialized object columns.
        /// </summary>
        /// <returns></returns>
        public List<SchemaColumn> GetObjectColumns()
        {
            return _columns.Values.Where(t => t.IsSerialized).OrderBy(col => col.Id).ToList();
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
        /// get column
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SchemaColumn this[string name]
        {
            get { return _columns.ContainsKey(name) ? _columns[name] : null; }
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
        private const string DateVarString = "$date";
        private const string WeekOfYearString = "$week";

        /// <summary>
        /// 获取动态的表名
        /// </summary>
        /// <param name="date"></param>
        /// <param name="increase">使用表达式时的日期增量,默认当天</param>
        /// <returns></returns>
        public string GetTableName(DateTime date, int increase = 0)
        {
            string format = NameFormat ?? "";
            //date = DateTime.Now;
            int weekOfYear = 0;
            //处理日期周
            if (format.ToLower().IndexOf(WeekOfYearString, StringComparison.Ordinal) != -1)
            {
                date = date.AddDays(increase * 7);
                weekOfYear = MathUtils.ToWeekOfYear(date);
                format = format.Replace(WeekOfYearString, weekOfYear.ToString());
            }
            int index = format.ToLower().IndexOf(DateVarString, StringComparison.Ordinal);
            if (index > -1)
            {
                string formatStart = format.Substring(0, index);
                string dateStr = date.AddMonths(increase).ToString("yyyyMM");
                string formatEnd = "";
                if (format.Length - index > DateVarString.Length)
                {
                    formatEnd = format.Substring(index + DateVarString.Length);
                    //查找是否有日期格式
                    int dateIndex = formatEnd.IndexOf("[", StringComparison.Ordinal);
                    if (dateIndex > -1)
                    {
                        int indexTo = formatEnd.IndexOf("]", StringComparison.Ordinal);
                        if (indexTo > -1 && indexTo > dateIndex)
                        {
                            string dateFormat = formatEnd.Substring(dateIndex + 1, indexTo - dateIndex - 1);
                            //若是周数有增加时，其它则不增了
                            if (weekOfYear == 0)
                            {
                                date = GetIncreaseDateTime(increase, dateFormat, date);
                            }
                            dateStr = date.ToString(dateFormat);
                            formatEnd = formatEnd.Substring(indexTo + 1);
                        }
                    }
                }
                format = formatStart + dateStr + formatEnd;
            }
            return string.IsNullOrEmpty(format) ? EntityName : string.Format(format, EntityName);
        }

        private static DateTime GetIncreaseDateTime(int increase, string dateFormat, DateTime date)
        {
            if (dateFormat.EndsWith("yy", StringComparison.Ordinal))
            {
                date = date.AddYears(increase);
            }
            else if (dateFormat.EndsWith("MM", StringComparison.Ordinal))
            {
                date = date.AddMonths(increase);
            }
            else if (dateFormat.EndsWith("d", StringComparison.Ordinal))
            {
                date = date.AddDays(increase);
            }
            else if (dateFormat.EndsWith("h", StringComparison.Ordinal))
            {
                date = date.AddHours(increase);
            }
            return date;
        }
    }
}