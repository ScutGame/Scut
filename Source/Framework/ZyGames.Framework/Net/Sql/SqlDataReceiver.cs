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
using System.Data;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Net.Sql
{
    class SqlDataReceiver : IDataReceiver
    {
        private SchemaTable _schema;
        private DbDataFilter _filter;

        public SqlDataReceiver(SchemaTable schema, DbDataFilter filter)
        {
            _schema = schema;
            _filter = filter;
        }

        public void Dispose()
        {
            _schema = null;
            _filter = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool TryReceive<T>(out List<T> dataList) where T : ISqlEntity, new()
        {
            return TryReceive<T>(SetPropertyFunc, out dataList);
        }

        private void SetPropertyFunc<T>(T entity, SchemaColumn column, object fieldValue) where T : new()
        {
            var t = entity as AbstractEntity;
            if (t != null)
            {
                t.Init();
                t.SetValueBefore();
                SetEntityValue(t, column, fieldValue);
                t.SetValueAfter();
            }
        }

        public bool TryReceive<T>(EntityPropertySetFunc<T> setFunc, out List<T> dataList) where T : new()
        {
            bool result = false;
            dataList = null;
            if ((string.IsNullOrEmpty(_schema.ConnectKey) && string.IsNullOrEmpty(_schema.ConnectionString)) ||
                _schema.Columns.Count == 0)
            {
                dataList = new List<T>();
                return true;
            }
            try
            {

                using (IDataReader reader = GetReader<T>())
                {
                    if (reader != null)
                    {
                        dataList = new List<T>();
                        while (reader.Read())
                        {
                            T entity = ReadEntityProperty(reader, setFunc);
                            dataList.Add(entity);
                        }
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Read entity property error:{0}", ex);
            }
            return result;
        }

        public int Capacity { get { return _filter != null ? _filter.Capacity : 0; } }

        private IDataReader GetReader<T>()
        {

            string sql = string.Empty;
            try
            {
                if (Capacity < 0 || _filter == null || _schema == null)
                {
                    TraceLog.WriteError("The {0} schema config is empty.", typeof(T).FullName);
                    return null;
                }
                string tableName = _schema.GetTableName(_filter.CreateTime);
                if (string.IsNullOrEmpty(tableName))
                {
                    TraceLog.WriteError("The {0} schema table name is empty.", _schema.EntityType.FullName);
                    return null;
                }

                DbBaseProvider dbprovider = DbConnectionProvider.CreateDbProvider(_schema);
                if (dbprovider == null)
                {
                    TraceLog.WriteError("The {0} ConnectKey:{1} is empty.", _schema.EntityType.FullName, _schema.ConnectKey);
                    return null;
                }
                var command = dbprovider.CreateCommandStruct(tableName, CommandMode.Inquiry);
                var columns = _schema.GetColumnNames();
                command.Columns = string.Join(",", columns);
                command.OrderBy = (string.IsNullOrEmpty(_filter.OrderColumn) ? _schema.OrderColumn : _filter.OrderColumn) ?? "";
                if (Capacity < int.MaxValue && Capacity > 0)
                {
                    command.Top = Capacity;
                    if (string.IsNullOrEmpty(command.OrderBy))
                    {
                        string orderStr = "";
                        foreach (var key in _schema.Keys)
                        {
                            if (orderStr.Length > 0)
                            {
                                orderStr += ",";
                            }
                            orderStr += string.Format("{0}", dbprovider.FormatName(key));
                        }
                        command.OrderBy = orderStr;
                    }
                }
                command.Filter = dbprovider.CreateCommandFilter();

                command.Filter.Condition = MergerCondition(_filter.Condition, _schema.Condition);
                ParseDataParameter(command.Filter);

                command.Parser();
                sql = command.Sql;
                return dbprovider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("DB execute reader sql:\"{0}\" error:{1}", sql, ex);
            }
            return null;
        }


        private string MergerCondition(string cond1, string cond2)
        {
            if (!string.IsNullOrEmpty(cond1) && !string.IsNullOrEmpty(cond2))
            {
                return cond1 + " AND " + cond2;
            }
            if (string.IsNullOrEmpty(cond1) && !string.IsNullOrEmpty(cond2))
            {
                return cond2;
            }
            if (!string.IsNullOrEmpty(cond1) && string.IsNullOrEmpty(cond2))
            {
                return cond1;
            }
            return "";
        }

        private void ParseDataParameter(CommandFilter filter)
        {
            foreach (var parameter in _filter.Parameters)
            {
                filter.AddParam(parameter.Key, parameter.Value);
            }
        }


        private T ReadEntityProperty<T>(IDataReader reader, EntityPropertySetFunc<T> setFunc) where T : new()
        {
            T entity = new T();
            var columns = _schema.GetColumns();
            foreach (var column in columns)
            {
                try
                {
                    object fieldValue = reader[column.Name];
                    if (setFunc != null) setFunc(entity, column, fieldValue);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("read {0} table's {1} column error.", _schema.EntityName, column.Name), ex);
                }
            }
            return entity;
        }

        private void SetEntityValue(AbstractEntity entity, SchemaColumn fieldAttr, object value)
        {
            string columnName = fieldAttr.Name;
            object fieldValue;
            if (fieldAttr.IsSerialized)
            {
                //指定序列化方式
                if (fieldAttr.DbType == ColumnDbType.LongBlob || fieldAttr.DbType == ColumnDbType.Blob)
                {
                    fieldValue = DeserializeBinaryObject(value, fieldAttr);
                }
                else
                {
                    fieldValue = DeserializeJsonObject(value, fieldAttr);
                }
                if (fieldValue is EntityChangeEvent)
                {
                    ((EntityChangeEvent)fieldValue).PropertyName = columnName;
                }
            }
            else
            {
                fieldValue = AbstractEntity.ParseValueType(value, fieldAttr.ColumnType);
            }
            if (fieldAttr.CanWrite)
            {
                entity.SetPropertyValue(columnName, fieldValue);
            }
            else
            {
                entity.SetFieldValue(columnName, fieldValue);
            }
        }

        private object DeserializeBinaryObject(object value, SchemaColumn fieldAttr)
        {
            if (value is byte[])
            {
                byte[] buffer = value as byte[];
                return ProtoBufUtils.Deserialize(buffer, fieldAttr.ColumnType);
            }
            throw new Exception(string.Format("The {0} column value is not byte[] type.", fieldAttr.Name));

        }

        private static object DeserializeJsonObject(object value, SchemaColumn fieldAttr)
        {
            if (fieldAttr.ColumnType.IsSubclassOf(typeof(Array)))
            {
                value = value.ToString().StartsWith("[") ? value : "[" + value + "]";
            }
            string tempValue = value.ToNotNullString();
            if (!string.IsNullOrEmpty(fieldAttr.JsonDateTimeFormat) &&
                tempValue.IndexOf(@"\/Date(", StringComparison.Ordinal) == -1)
            {
                return JsonUtils.DeserializeCustom(tempValue, fieldAttr.ColumnType, fieldAttr.JsonDateTimeFormat);
            }
            return JsonUtils.Deserialize(tempValue, fieldAttr.ColumnType);

        }
    }
}