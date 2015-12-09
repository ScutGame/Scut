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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Config;
using ZyGames.Framework.Data;
using ZyGames.Framework.Event;
using ZyGames.Framework.Redis;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// 实体架构信息集合
    /// </summary>
    public static class EntitySchemaSet
    {
        private static EntitySection GetEntitySection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<EntitySection>();
        }
        private static readonly object syncAssemblyRoot = new object();
        private static DictionaryExtend<string, SchemaTable> SchemaSet = new DictionaryExtend<string, SchemaTable>();
        private static ConcurrentQueue<SchemaTable> _dynamicTables = new ConcurrentQueue<SchemaTable>();
        private static CacheListener _tableListener = new CacheListener("__EntitySchemaSet_CheckDynamicTable", 60 * 60, OnCheckDynamicTable);//间隔1小时
        private static Assembly _entityAssembly;

        /// <summary>
        /// 实体的程序集
        /// </summary>
        public static Assembly EntityAssembly
        {
            get
            {
                lock (syncAssemblyRoot)
                {
                    return _entityAssembly;
                }
            }
            internal set
            {
                lock (syncAssemblyRoot)
                {
                    _entityAssembly = value;
                }
            }
        }

        /// <summary>
        /// 全局缓存生命周期
        /// </summary>
        public static int CacheGlobalPeriod { get; set; }

        /// <summary>
        /// 玩家缓存生命周期
        /// </summary>
        public static int CacheUserPeriod { get; set; }

        /// <summary>
        /// 聊天缓存生命周期
        /// </summary>
        public static int CacheQueuePeriod { get; set; }


        private static void OnCheckDynamicTable(string key, object value, CacheRemovedReason reason)
        {
            try
            {
                DateTime date = DateTime.Now;
                int logPriorBuildMonth = GetEntitySection().LogPriorBuildMonth;
                var tableTypes = _dynamicTables.ToList();
                foreach (var schema in tableTypes)
                {
                    DbBaseProvider dbprovider = DbConnectionProvider.CreateDbProvider(schema);
                    if (dbprovider == null)
                    {
                        continue;
                    }
                    string tableName = "";
                    int count = logPriorBuildMonth > 1 ? logPriorBuildMonth : 2;
                    for (int i = 0; i < count; i++)
                    {
                        tableName = schema.GetTableName(date, i);

                        DbColumn[] columns;
                        if (!dbprovider.CheckTable(tableName, out columns))
                        {
                            CreateTableSchema(schema, dbprovider, tableName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnCheckLogTable error:{0}", ex);
            }
        }

        /// <summary>
        /// 启动创建表结构检查定时器
        /// </summary>
        public static void StartCheckTableTimer()
        {
            if (_tableListener.IsRunning) return;
            _tableListener.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Init()
        {
            SchemaSet = new DictionaryExtend<string, SchemaTable>();
            StartCheckTableTimer();
        }

        /// <summary>
        /// 生成存储在Redis的Key
        /// </summary>
        /// <returns></returns>
        public static string GenerateRedisKey<T>(string personalKey) where T : ISqlEntity
        {
            return GenerateRedisKey(typeof(T), personalKey);
        }

        /// <summary>
        /// 生成存储在Redis的Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="personalKey"></param>
        /// <returns></returns>
        public static string GenerateRedisKey(Type type, string personalKey)
        {
            if (string.IsNullOrEmpty(personalKey))
            {
                return string.Format("{0}", RedisConnectionPool.EncodeTypeName(type.FullName));
            }
            return string.Format("{0}_{1}", RedisConnectionPool.EncodeTypeName(type.FullName), personalKey);
        }

        /// <summary>
        /// 加载实体程序集
        /// </summary>
        /// <param name="assembly"></param>
        public static void LoadAssembly(Assembly assembly)
        {
            TraceLog.WriteLine("{0} Start checking table schema, please wait.", DateTime.Now.ToString("HH:mm:ss"));

            EntityAssembly = assembly;
            var types = assembly.GetTypes().Where(p => p.GetCustomAttributes(typeof(EntityTableAttribute), false).Count() > 0).ToList();
            foreach (var type in types)
            {
                InitSchema(type);
            }
            TraceLog.WriteLine("{0} Check table schema successfully.", DateTime.Now.ToString("HH:mm:ss"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isReset"></param>
        public static SchemaTable InitSchema(Type type, bool isReset = false)
        {
            SchemaTable schema;
            if (!isReset && TryGet(type, out schema))
            {
                return schema;
            }
            schema = new SchemaTable();
            try
            {
                schema.IsEntitySync = type.GetCustomAttributes(typeof(EntitySyncAttribute), false).Length > 0;
                schema.EntityType = type;
                //加载表
                var entityTable = FindAttribute<EntityTableAttribute>(type.GetCustomAttributes(false));
                if (entityTable != null)
                {
                    schema.AccessLevel = entityTable.AccessLevel;
                    schema.StorageType |= entityTable.StorageType;
                    schema.CacheType = entityTable.CacheType;
                    schema.IncreaseStartNo = entityTable.IncreaseStartNo;
                    schema.IsExpired = entityTable.IsExpired;
                    schema.EntityName = string.IsNullOrEmpty(entityTable.TableName) ? type.Name : entityTable.TableName;
                    schema.ConnectKey = string.IsNullOrEmpty(entityTable.ConnectKey) ? "" : entityTable.ConnectKey;
                    schema.Indexs = entityTable.Indexs;
                    schema.Condition = entityTable.Condition;
                    schema.OrderColumn = entityTable.OrderColumn;
                    //schema.DelayLoad = entityTable.DelayLoad;
                    schema.NameFormat = entityTable.TableNameFormat;
                    SetPeriodTime(schema);
                    //model other set
                    if (entityTable.IsExpired && entityTable.PeriodTime > 0)
                    {
                        schema.PeriodTime = entityTable.PeriodTime;
                    }
                    schema.Capacity = entityTable.Capacity;
                    schema.PersonalName = entityTable.PersonalName;

                    if (string.IsNullOrEmpty(schema.ConnectKey)
                        && type == typeof(EntityHistory))
                    {
                        schema.IsInternal = true;
                        var dbPair = DbConnectionProvider.Find(DbLevel.Game);
                        if (dbPair.Value == null)
                        {
                            dbPair = DbConnectionProvider.FindFirst();
                        }
                        if (dbPair.Value != null)
                        {
                            schema.ConnectKey = dbPair.Key;
                            schema.ConnectionProviderType = dbPair.Value.ProviderTypeName;
                            schema.ConnectionString = dbPair.Value.ConnectionString;
                        }
                        //else
                        //{
                        //    TraceLog.WriteWarn("Not found Redis's history record of db connect config.");
                        //}
                    }
                }
                InitSchema(type, schema);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("InitSchema type:{0} error:\r\n{1}", type.FullName, ex);
            }
            //check cachetype
            if ((schema.CacheType == CacheType.Entity && !type.IsSubclassOf(typeof(ShareEntity))) ||
                (schema.CacheType == CacheType.Dictionary &&
                    schema.AccessLevel == AccessLevel.ReadWrite &&
                    !type.IsSubclassOf(typeof(BaseEntity)))
                )
            {
                throw new ArgumentException(string.Format("\"EntityTable.CacheType:{1}\" attribute of {0} class is error", type.FullName, schema.CacheType), "CacheType");
            }
            return schema;
        }

        /// <summary>
        /// Export to sync model format content.
        /// </summary>
        /// <returns></returns>
        public static string ExportSync(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            //write head
            sb.AppendLine("------------------------------------------------------------------");
            sb.AppendFormat("-- {0}", Path.GetFileName(fileName));//ScutSchemaInfo.lua
            sb.AppendLine();
            sb.AppendLine("-- Author     :");
            sb.AppendLine("-- Version    : 1.0");
            sb.AppendLine("-- Date       :");
            sb.AppendLine("-- Description: The schema of entity info.");
            sb.AppendLine("------------------------------------------------------------------");
            sb.AppendFormat("module(\"{0}\", package.seeall)", Path.GetFileNameWithoutExtension(fileName));
            sb.AppendLine();
            sb.AppendLine("--The following code is automatically generated");
            sb.AppendLine("g__schema = {}");

            var list = SchemaSet.Values.Where(p => p.IsEntitySync).OrderBy(p => p.EntityName).ToList();
            foreach (var schema in list)
            {
                sb.AppendLine(ExportSync(schema));
                sb.AppendLine();
            }
            return sb.ToString();
        }
        /// <summary>
        /// Export entity schema for client sync.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static string ExportSync(SchemaTable schema)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("g__schema[\"{0}\"] = ", schema.EntityName);
            sb.AppendLine("{");
            var columns = schema.GetColumns();
            int index = 0;
            int endIndex = columns.Count - 1;
            int depth = 1;
            foreach (var col in columns)
            {
                bool isEnd = index == endIndex;
                if (col.HasChild)
                {
                    WriteChildSchema(sb, col, depth + 1, isEnd);
                }
                else
                {
                    WriteColumnSchema(sb, col, depth, isEnd);
                }
                index++;
            }
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static void WriteChildSchema(StringBuilder sb, SchemaColumn parent, int depth, bool isParentEnd)
        {
            string parentChar = "".PadLeft((depth - 1) * 4, ' ');
            string preChar = "".PadLeft(depth * 4, ' ');
            sb.AppendFormat("{0}[\"{1}\"] = ", parentChar, parent.Name);
            sb.AppendLine("{");
            sb.AppendFormat("{0}{1},", preChar, parent.Id);
            sb.AppendLine();
            sb.AppendFormat("{0}{1},", preChar, "[\"_hasChild\"] = true");
            sb.AppendLine();

            if (parent.IsDictionary)
            {
                var key = parent.Children[0];
                var col = parent.Children[1];
                WriteColumnSchema(sb, key, depth, false);
                if (IsSupportType(col.ColumnType))
                {
                    WriteColumnSchema(sb, col, depth, true);
                }
                else
                {
                    WriteChildSchema(sb, col, depth + 1, true);
                }
            }
            else if (parent.IsList)
            {
                var col = parent.Children[0];
                if (IsSupportType(col.ColumnType))
                {
                    WriteColumnSchema(sb, col, depth, true);
                }
                else
                {
                    WriteChildSchema(sb, col, depth + 1, true);
                }
            }
            else
            {
                int index = 0;
                int endIndex = parent.Children.Count - 1;
                foreach (var col in parent.Children)
                {
                    bool isEnd = index == endIndex;
                    if (col.HasChild)
                    {
                        WriteChildSchema(sb, col, depth + 1, isEnd);
                    }
                    else
                    {
                        WriteColumnSchema(sb, col, depth, isEnd);
                    }
                    index++;
                }

            }
            sb.AppendFormat("{0}", parentChar);
            sb.AppendLine(!isParentEnd ? "}," : "}");
        }

        private static void WriteColumnSchema(StringBuilder sb, SchemaColumn col, int depth, bool isEnd)
        {
            sb.AppendFormat("{0}[\"{1}\"] = ", "".PadLeft(depth * 4, ' '), col.Name);
            sb.Append("{");
            string colType = GetColumnType(col.ColumnType);
            sb.AppendFormat(col.IsKey ? "{0}, \"{1}\", true" : "{0}, \"{1}\"", col.Id, colType);
            sb.AppendLine(!isEnd ? "}," : "}");
        }

        /// <summary>
        /// The type has be supported.
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public static bool IsSupportType(Type fieldType)
        {
            return fieldType == typeof(int) ||
                   fieldType == typeof(short) ||
                   fieldType == typeof(string) ||
                   fieldType == typeof(byte) ||
                   fieldType == typeof(long) ||
                   fieldType == typeof(float) ||
                   fieldType == typeof(double) ||
                   fieldType == typeof(bool) ||
                   fieldType == typeof(decimal) ||
                   fieldType == typeof(DateTime) ||
                   fieldType == typeof(ulong) ||
                   fieldType == typeof(uint) ||
                   fieldType == typeof(ushort) ||
                   fieldType.IsEnum;
        }

        /// <summary>
        /// Get column type to string.
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public static string GetColumnType(Type fieldType)
        {
            if (fieldType == typeof(int)) return "int";
            if (fieldType == typeof(short)) return "short";
            if (fieldType == typeof(string)) return "string";
            if (fieldType == typeof(byte)) return "byte";
            if (fieldType == typeof(long)) return "long";
            if (fieldType == typeof(float)) return "float";
            if (fieldType == typeof(double)) return "double";
            if (fieldType == typeof(bool)) return "bool";
            if (fieldType == typeof(decimal)) return "decimal";
            if (fieldType == typeof(ulong)) return "ulong";
            if (fieldType == typeof(uint)) return "uint";
            if (fieldType == typeof(ushort)) return "ushort";
            if (fieldType == typeof(DateTime)) return "datetime";
            if (fieldType.IsEnum) return "int";
            return fieldType.Name.ToLower();
        }

        private static void SetPeriodTime(SchemaTable schema)
        {
            if (schema.AccessLevel == AccessLevel.ReadOnly || !schema.IsExpired)
            {
                schema.PeriodTime = 0;
                return;
            }
            switch (schema.CacheType)
            {
                case CacheType.None:
                    break;
                case CacheType.Entity:
                    schema.PeriodTime = CacheGlobalPeriod;
                    break;
                case CacheType.Dictionary:
                    schema.PeriodTime = CacheUserPeriod;
                    break;
                case CacheType.Queue:
                    schema.PeriodTime = CacheQueuePeriod;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初始化架构信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="schema"></param>
        public static void InitSchema(Type type, SchemaTable schema)
        {
            var section = GetEntitySection();
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.IsSubclassOf(typeof(LogEntity)) && string.IsNullOrEmpty(schema.NameFormat))
            {
                schema.NameFormat = section.LogTableNameFormat;
            }
            if (!string.IsNullOrEmpty(schema.NameFormat) && !Exits(type))
            {
                _dynamicTables.Enqueue(schema);
            }

            //加载成员属性
            HashSet<string> keySet = new HashSet<string>();
            PropertyInfo[] propertyList = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            SchemaColumn column;
            int number = 0;
            foreach (PropertyInfo property in propertyList)
            {
                number++;
                //Ignore parent property
                bool isExtend = property.GetCustomAttributes<EntityFieldExtendAttribute>(false).Count() > 0;
                if (!isExtend && property.DeclaringType != property.ReflectedType)
                {
                    continue;
                }
                column = new SchemaColumn();
                var entityField = FindAttribute<EntityFieldAttribute>(property.GetCustomAttributes(false));
                if (entityField != null)
                {
                    column.Id = number;
                    column.Name = string.IsNullOrEmpty(entityField.FieldName) ? property.Name : entityField.FieldName;
                    column.IsIdentity = entityField.IsIdentity;
                    column.IdentityNo = entityField.IdentityNo;
                    column.IsKey = entityField.IsKey;
                    column.IsUnique = entityField.IsUnique;
                    column.ColumnLength = entityField.ColumnLength;
                    column.ColumnScale = entityField.ColumnScale;
                    column.Isnullable = entityField.Isnullable;
                    column.FieldModel = entityField.FieldModel;
                    column.Disable = entityField.Disable;
                    column.MinRange = entityField.MinRange;
                    column.MaxRange = entityField.MaxRange;
                    column.IsSerialized = entityField.IsJsonSerialize;
                    column.JsonDateTimeFormat = entityField.JsonDateTimeFormat;
                    column.ColumnType = property.PropertyType;
                    column.DbType = entityField.DbType;
                    column.CanRead = property.CanRead;
                    column.CanWrite = property.CanWrite;

                    AddChildType(column);
                }
                else
                {
                    //必须要加EntityField标识
                    continue;
                    //column.Id = number;
                    //column.Name = property.Name;
                    //column.IsIdentity = false;
                    //column.IsKey = string.Compare(property.Name, "id", true) == 0;
                    //column.FieldModel = FieldModel.All;
                    //column.ColumnType = property.PropertyType;
                    //column.DbType = ColumnDbType.Varchar;
                }
                schema.Columns.TryAdd(column.Name, column);
                if (column.IsSerialized) schema.HasObjectColumns = true;
                if (column.IsKey)
                {
                    keySet.Add(column.Name);
                }
            }
            //add modify time field
            if (section.EnableModifyTimeField && schema.AccessLevel == AccessLevel.ReadWrite)
            {
                column = new SchemaColumn();
                column.Id = number;
                column.Name = "TempTimeModify";
                column.Isnullable = true;
                column.FieldModel = FieldModel.All;
                column.ColumnType = typeof(DateTime);
                column.DbType = ColumnDbType.DateTime;
                column.JsonDateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                schema.Columns.TryAdd(column.Name, column);
            }

            schema.Keys = new string[keySet.Count];
            keySet.CopyTo(schema.Keys, 0);

            CheckTableSchema(schema);
            AddSchema(type, schema);
        }

        private static void AddChildType(SchemaColumn column)
        {
            Type type = column.ColumnType;

            if (type.IsSubclassOf(typeof(IItemChangeEvent)))
            {
                var args = type.GetGenericArguments();

                if (args.Length > 0)
                {
                    column.GenericArgs = args.Length;
                    column.Children = new List<SchemaColumn>();
                    int number = 0;
                    foreach (var genericArg in args)
                    {
                        number++;
                        var item = new SchemaColumn();
                        item.Id = number;
                        item.ColumnType = genericArg;

                        AddChildType(item);
                        column.Children.Add(item);
                    }
                    if (column.IsDictionary)
                    {
                        column.Children[0].Name = "_key";
                        column.Children[0].IsKey = true;
                        column.Children[1].Name = "_value";
                    }
                    else if (column.IsList)
                    {
                        column.Children[0].Name = "_list";
                    }
                }
                else
                {
                    var propertyList = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Where(p => p.GetCustomAttributes(typeof(ProtoMemberAttribute), false).Length > 0).ToList();
                    if (propertyList.Count > 0)
                    {
                        column.Children = new List<SchemaColumn>();
                    }
                    int number = 0;
                    foreach (var property in propertyList)
                    {
                        number++;
                        var child = new SchemaColumn();
                        child.Id = number;
                        child.Name = property.Name;
                        child.ColumnType = property.PropertyType;
                        child.CanRead = property.CanRead;
                        child.CanWrite = property.CanWrite;

                        AddChildType(child);
                        column.Children.Add(child);
                    }
                }
            }
        }

        /// <summary>
        /// 更新表架构信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="periodTime">过期时间</param>
        /// <param name="capacity"></param>
        /// <param name="condition"></param>
        /// <param name="orderColumn"></param>
        public static void UpdateTableSchema<T>(int periodTime, int capacity, string condition, string orderColumn)
            where T : AbstractEntity, new()
        {
            SchemaTable schema;
            if (TryGet<T>(out schema))
            {
                schema.PeriodTime = periodTime;
                schema.Capacity = capacity;
                schema.Condition = condition;
                schema.OrderColumn = orderColumn;
            }
        }

        /// <summary>
        /// 增加架构配置表，存在则更新
        /// </summary>
        /// <param name="type"></param>
        /// <param name="schema"></param>
        public static void AddSchema(Type type, SchemaTable schema)
        {
            if (type != null && !string.IsNullOrEmpty(type.FullName))
            {
                SchemaTable oldValue;
                if (SchemaSet.TryGetValue(type.FullName, out oldValue))
                {
                    SchemaSet.TryUpdate(type.FullName, schema, oldValue);
                }
                else
                {
                    SchemaSet.TryAdd(type.FullName, schema);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Exits(Type type)
        {
            SchemaTable schema;
            return TryGet(type, out schema);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static SchemaTable Get(string typeName)
        {
            SchemaTable schema;
            TryGet(typeName, out schema);
            return schema;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SchemaTable Get(Type type)
        {
            return Get(type.FullName);
        }

        /// <summary>
        /// Get entity schema.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SchemaTable Get<T>()
        {
            return Get(typeof(T));
        }

        /// <summary>
        /// Get entity schema.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool TryGet<T>(out SchemaTable schema)
        {
            return TryGet(typeof(T).FullName, out schema);
        }

        /// <summary>
        /// Get entity schema.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool TryGet(Type type, out SchemaTable schema)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return TryGet(type.FullName, out schema);
        }

        /// <summary>
        /// Get schema for typename
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool TryGet(string typeName, out SchemaTable schema)
        {
            typeName = RedisConnectionPool.DecodeTypeName(typeName);
            return SchemaSet.TryGetValue(typeName, out schema);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SchemaTable> GetEnumerable()
        {
            return SchemaSet.Select(pair => pair.Value);
        }


        private static T FindAttribute<T>(object[] attrList) where T : Attribute, new()
        {
            if (attrList.Length > 0)
            {
                foreach (var attr in attrList)
                {
                    if (attr is T)
                    {
                        return (T)attr;
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// 检测数据表结构变动
        /// </summary>
        /// <param name="schema"></param>
        private static void CheckTableSchema(SchemaTable schema)
        {
            string tableName = schema.GetTableName(DateTime.Now);
            try
            {
                if (DbConnectionProvider.Count == 0)
                {
                    //not seting connection.
                    return;
                }
                DbBaseProvider dbprovider = DbConnectionProvider.CreateDbProvider(schema);
                if (dbprovider == null)
                {
                    if (schema.StorageType.HasFlag(StorageType.ReadOnlyDB) ||
                        schema.StorageType.HasFlag(StorageType.ReadWriteDB) ||
                        schema.StorageType.HasFlag(StorageType.WriteOnlyDB))
                    {
                        TraceLog.WriteError("Not found DB connection key \"{1}\" of the {0} entity.", schema.EntityName, schema.ConnectKey);
                    }
                    return;
                }
                DbColumn[] columns;
                if (dbprovider.CheckTable(tableName, out columns))
                {
                    ModifyTableSchema(schema, dbprovider, tableName, columns);
                }
                else
                {
                    CreateTableSchema(schema, dbprovider, tableName);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CheckTableSchema {0} error:{1}", tableName, ex);
            }
        }

        private static void ModifyTableSchema(SchemaTable schema, DbBaseProvider dbprovider, string tableName, DbColumn[] columns)
        {
            var list = new List<DbColumn>();
            foreach (var keypair in schema.Columns)
            {
                var field = keypair.Value;
                string name = field.Name;
                var dbColumn = Array.Find(columns, p => MathUtils.IsEquals(p.Name, name, true));
                if (dbColumn == null)
                {
                    dbColumn = new DbColumn();
                    dbColumn.Id = field.Id;
                    dbColumn.Name = name;
                    dbColumn.Type = field.ColumnType;
                    dbColumn.Length = field.ColumnLength;
                    dbColumn.Scale = field.ColumnScale;
                    dbColumn.Isnullable = field.Isnullable;
                    dbColumn.IsKey = field.IsKey;
                    dbColumn.IsUnique = field.IsUnique;
                    dbColumn.DbType = field.DbType.ToString();
                    dbColumn.IsIdentity = field.IsIdentity;
                    dbColumn.IdentityNo = field.IdentityNo;
                    list.Add(dbColumn);
                }
                else
                {
                    var fieldType = field.ColumnType;
                    //no modify type: text,blob,byte[], enum,list,dict
                    if (
                        //对象序列化类型
                        (field.IsSerialized &&
                           (
                           (field.DbType == ColumnDbType.Varchar && (dbColumn.Type != typeof(string) || (field.ColumnLength > 0 && dbColumn.Length != field.ColumnLength))) ||
                           (field.DbType != ColumnDbType.Varchar && (dbColumn.Type != typeof(string) || dbColumn.DbType.StartsWith("varchar", true)))
                           )
                        ) ||
                        //特殊值类型
                        (dbColumn.Type == typeof(decimal) && field.ColumnScale > 0 && dbColumn.Scale != field.ColumnScale) ||
                        (fieldType.IsEnum && dbColumn.Type != typeof(int)) ||
                        (fieldType == typeof(ushort) && dbColumn.Type != typeof(short)) ||
                        (fieldType == typeof(uint) && dbColumn.Type != typeof(int)) ||
                        (fieldType == typeof(ulong) && dbColumn.Type != typeof(long)) ||
                        (fieldType == typeof(string) && field.ColumnLength > 0 && dbColumn.Length != field.ColumnLength) ||
                        //非对象类型
                        (!field.IsSerialized &&
                            !fieldType.IsEnum &&
                            !field.IsDictionary &&
                            !field.IsList &&
                            fieldType != typeof(byte[]) &&
                            fieldType != typeof(ushort) &&
                            fieldType != typeof(uint) &&
                            fieldType != typeof(ulong) &&
                            dbColumn.Type != fieldType
                            ) ||
                        //check key
                        ((field.IsKey && dbColumn.KeyNo == 0) || (!field.IsKey && dbColumn.KeyNo > 0))
                        )
                    {
                        dbColumn.Type = fieldType;
                        dbColumn.Length = field.ColumnLength;
                        dbColumn.Scale = field.ColumnScale;
                        dbColumn.Isnullable = field.Isnullable;
                        dbColumn.IsKey = field.IsKey;
                        dbColumn.DbType = field.DbType.ToString();
                        dbColumn.IsIdentity = field.IsIdentity;
                        dbColumn.IdentityNo = field.IdentityNo;
                        dbColumn.IsModify = true;
                        list.Add(dbColumn);
                    }
                }
            }

            if (list.Count > 0)
            {
                list.Sort((a, b) => a.Id.CompareTo(b.Id));
                dbprovider.CreateColumn(tableName, list.ToArray());
            }
        }

        private static void CreateTableSchema(SchemaTable schema, DbBaseProvider dbprovider, string tableName)
        {
            var list = new List<DbColumn>();
            foreach (var keypair in schema.Columns)
            {
                var field = keypair.Value;
                var column = new DbColumn();
                column.Id = field.Id;
                column.Name = field.Name;
                column.Type = field.ColumnType;
                column.Length = field.ColumnLength;
                column.Scale = field.ColumnScale;
                column.Isnullable = field.Isnullable;
                column.IsKey = field.IsKey;
                column.IsUnique = field.IsUnique;
                column.DbType = field.DbType.ToString();
                column.IsIdentity = field.IsIdentity;
                column.IdentityNo = field.IdentityNo;
                list.Add(column);
            }
            list.Sort((a, b) => a.Id.CompareTo(b.Id));
            dbprovider.CreateTable(tableName, list.ToArray());
            if (schema.Indexs != null && schema.Indexs.Length > 0)
            {
                dbprovider.CreateIndexs(tableName, schema.Indexs);
            }
        }
    }
}