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
using System.Reflection;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// 实体架构信息集合
    /// </summary>
    public static class EntitySchemaSet
    {
        private static readonly DictionaryExtend<string, SchemaTable> SchemaSet = new DictionaryExtend<string, SchemaTable>();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        public static void LoadAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(p => p.GetCustomAttributes(typeof(EntityTableAttribute), false).Count() > 0).ToList();
            foreach (var type in types)
            {
                InitSchema(type);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public static void InitSchema(Type type)
        {
            try
            {

                SchemaTable schema = new SchemaTable();
                schema.EntityType = type;
                //加载表
                var entityTable = FindAttribute<EntityTableAttribute>(type.GetCustomAttributes(false));
                if (entityTable != null)
                {
                    schema.AccessLevel = entityTable.AccessLevel;
                    schema.CacheType = entityTable.CacheType;
                    schema.IsStoreInDb = entityTable.IsStoreInDb;
                    schema.Name = string.IsNullOrEmpty(entityTable.TableName) ? type.Name : entityTable.TableName;
                    schema.ConnectKey = string.IsNullOrEmpty(entityTable.ConnectKey) ? "" : entityTable.ConnectKey;
                    schema.Condition = entityTable.Condition;
                    schema.OrderColumn = entityTable.OrderColumn;
                    //schema.DelayLoad = entityTable.DelayLoad;
                    SetPeriodTime(schema);
                    schema.Capacity = entityTable.Capacity;
                    schema.PersonalName = entityTable.PersonalName;
                }
                InitSchema(type, schema);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("InitSchema type:{0} error:\r\n{1}", type.FullName, ex);
            }
        }

        private static void SetPeriodTime(SchemaTable schema)
        {
            if (schema.AccessLevel == AccessLevel.ReadOnly)
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
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (Exits(type))
            {
                return;
            }
            //加载成员属性
            HashSet<string> keySet = new HashSet<string>();
            PropertyInfo[] propertyList = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            SchemaColumn column;
            int number = 0;
            foreach (PropertyInfo property in propertyList)
            {
                number++;
                if (!Equals(property.DeclaringType, property.ReflectedType))
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
                    column.IsKey = entityField.IsKey;
                    column.ColumnLength = entityField.ColumnLength;
                    column.ColumnScale = entityField.ColumnScale;
                    column.Isnullable = entityField.Isnullable;
                    column.FieldModel = entityField.FieldModel;
                    column.Disable = entityField.Disable;
                    column.MinRange = entityField.MinRange;
                    column.MaxRange = entityField.MaxRange;
                    column.IsJson = entityField.IsJsonSerialize;
                    column.JsonDateTimeFormat = entityField.JsonDateTimeFormat;
                    column.ColumnType = property.PropertyType;
                    column.DbType = entityField.DbType;
                    column.CanRead = property.CanRead;
                    column.CanWrite = property.CanWrite;
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
                if (column.IsKey)
                {
                    keySet.Add(column.Name);
                }
            }
            schema.Keys = new string[keySet.Count];
            keySet.CopyTo(schema.Keys, 0);

            CheckTableSchema(schema);
            AddSchema(type, schema);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool TryGet<T>(out SchemaTable schema)
        {
            return SchemaSet.TryGetValue(typeof(T).FullName, out schema);
        }

        /// <summary>
        /// 
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
            return SchemaSet.TryGetValue(type.FullName, out schema);
        }

        private static T FindAttribute<T>(object[] attrList) where T : Attribute, new()
        {
            if (attrList.Length > 0)
            {
                foreach (var attr in attrList)
                {
                    if (attr != null && attr is T)
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
            try
            {

                DbBaseProvider dbprovider = DbConnectionProvider.CreateDbProvider(schema);
                if (dbprovider == null)
                {
                    //throw new Exception(string.Format("The {0} ConnectKey:{1} is empty.", schema.Name, schema.ConnectKey));
                    return;
                }

                DbColumn[] columns;
                if (dbprovider.CheckTable(schema.Name, out columns))
                {
                    var list = new List<DbColumn>();
                    foreach (var keypair in schema.Columns)
                    {
                        string name = keypair.Value.Name;
                        var column = Array.Find(columns, p => string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase));
                        if (column == null)
                        {
                            column = new DbColumn();
                            column.Id = keypair.Value.Id;
                            column.Name = name;
                            column.Type = keypair.Value.ColumnType;
                            column.Length = keypair.Value.ColumnLength;
                            column.Scale = keypair.Value.ColumnScale;
                            column.Isnullable = keypair.Value.Isnullable;
                            column.IsKey = keypair.Value.IsKey;
                            column.DbType = keypair.Value.DbType.ToString();
                            list.Add(column);
                        }
                        else
                        {
                            if ((column.Type == typeof(decimal) &&
                                    keypair.Value.ColumnScale > 0 &&
                                    column.Scale != keypair.Value.ColumnScale)
                                || (!keypair.Value.IsJson &&
                                    column.Type != keypair.Value.ColumnType &&
                                    keypair.Value.ColumnType.IsEnum && column.Type != typeof(short))
                                )
                            {
                                column.Type = keypair.Value.ColumnType;
                                column.Length = keypair.Value.ColumnLength;
                                column.Scale = keypair.Value.ColumnScale;
                                column.Isnullable = keypair.Value.Isnullable;
                                column.IsKey = keypair.Value.IsKey;
                                column.DbType = keypair.Value.DbType.ToString();
                                column.IsModify = true;
                                list.Add(column);
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        list.Sort((a, b) => a.Id.CompareTo(b.Id));
                        dbprovider.CreateColumn(schema.Name, list.ToArray());
                    }
                }
                else
                {
                    var list = new List<DbColumn>();
                    foreach (var keypair in schema.Columns)
                    {
                        var column = new DbColumn();
                        column.Id = keypair.Value.Id;
                        column.Name = keypair.Value.Name;
                        column.Type = keypair.Value.ColumnType;
                        column.Length = keypair.Value.ColumnLength;
                        column.Scale = keypair.Value.ColumnScale;
                        column.Isnullable = keypair.Value.Isnullable;
                        column.IsKey = keypair.Value.IsKey;
                        column.DbType = keypair.Value.DbType.ToString();
                        list.Add(column);
                    }
                    list.Sort((a, b) => a.Id.CompareTo(b.Id));
                    dbprovider.CreateTable(schema.Name, list.ToArray());
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CheckTableSchema {0} error:{1}", schema.Name, ex);
            }
        }

    }
}