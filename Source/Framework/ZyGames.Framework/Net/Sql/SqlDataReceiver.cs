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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZyGames.Framework.Collection;
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

        public SqlDataReceiver(SchemaTable schema, int capacity, DbDataFilter filter)
        {
            _schema = schema;
            _filter = filter;
            Capacity = capacity;
        }

        public void Dispose()
        {
            _schema = null;
            _filter = null;
        }

        public List<T> Receive<T>() where T : AbstractEntity, new()
        {
            if (string.IsNullOrEmpty(_schema.ConnectKey) && string.IsNullOrEmpty(_schema.ConnectionString))
            {
                return new List<T>();
            }
            using (IDataReader reader = GetReader<T>())
            {
                if (reader != null)
                {
                    var list = new List<T>();
                    while (reader.Read())
                    {
                        T entity = new T();
                        entity.Init();
                        entity.SetValueBefore();
                        SetEntityValue(_schema, reader, entity);
                        entity.SetValueAfter();
                        list.Add(entity);
                    }
                    return list;
                }
            }
            return null;
        }

        public int Capacity { get; set; }

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
                if (string.IsNullOrEmpty(_schema.Name))
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

                string[] columns = new string[_schema.Columns.Count];
                _schema.Columns.Keys.CopyTo(columns, 0);
                string condition = string.IsNullOrEmpty(_filter.Condition)
                    ? _schema.Condition
                    : MergerCondition(_filter.Condition, _schema.Condition);
                string order = string.IsNullOrEmpty(_filter.OrderColumn) ? _schema.OrderColumn : _filter.OrderColumn;
                order = string.IsNullOrEmpty(order) ? string.Empty : " ORDER BY " + order;

                if (!string.IsNullOrEmpty(condition))
                {
                    condition = condition.ToLower().StartsWith("where")
                        ? condition
                        : " Where " + condition;
                }

                sql = string.Format("SELECT {1} {2} FROM {0}{3}{4}"
                    , _schema.Name
                    , Capacity == int.MaxValue || Capacity == 0 ? "" : "TOP " + Capacity
                    , dbprovider.FormatQueryColumn(",", columns)
                    , condition
                    , order);

                IDataParameter[] parameters = CreateDataParameter(dbprovider);
                try
                {
                    if (parameters.Length > 0)
                    {
                        return dbprovider.ExecuteReader(CommandType.Text, sql, parameters);
                    }
                    return dbprovider.ExecuteReader(CommandType.Text, sql);
                }
                catch (Exception)
                {
                    //重执行一次
                    if (parameters.Length > 0)
                    {
                        return dbprovider.ExecuteReader(CommandType.Text, sql, parameters);
                    }
                    return dbprovider.ExecuteReader(CommandType.Text, sql);
                }
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
                return cond1 + " and " + cond2;
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

        private IDataParameter[] CreateDataParameter(DbBaseProvider dbprovider)
        {
            var list = new List<IDataParameter>(_filter.Parameters.Count);
            foreach (var parameter in _filter.Parameters)
            {
                var param = dbprovider.CreateParameter(parameter.Key, parameter.Value);
                list.Add(param);
            }
            return list.ToArray();
        }

        private void SetEntityValue(SchemaTable schemaTable, IDataReader reader, AbstractEntity entity)
        {
            string[] columns = new string[schemaTable.Columns.Count];
            schemaTable.Columns.Keys.CopyTo(columns, 0);
            foreach (var columnName in columns)
            {
                SchemaColumn fieldAttr;
                if (!schemaTable.Columns.TryGetValue(columnName, out fieldAttr))
                {
                    continue;
                }
                object fieldValue = null;
                if (fieldAttr.IsJson)
                {
                    var value = reader[columnName];
                    if (fieldAttr.ColumnType.BaseType == typeof(Array))
                    {
                        value = value.ToString().StartsWith("[") ? value : "[" + value + "]";
                    }
                    try
                    {
                        string tempValue = value.ToNotNullString();
                        if (!string.IsNullOrEmpty(fieldAttr.JsonDateTimeFormat) &&
                            tempValue.IndexOf(@"\/Date(") == -1)
                        {
                            fieldValue = JsonUtils.DeserializeCustom(tempValue, fieldAttr.ColumnType, fieldAttr.JsonDateTimeFormat);
                        }
                        else
                        {
                            fieldValue = JsonUtils.Deserialize(tempValue, fieldAttr.ColumnType);
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Table:{0} column:{1} deserialize json error:{2} to {3}\r\nException:{4}",
                            schemaTable.Name,
                            columnName,
                            fieldValue,
                            fieldAttr.ColumnType.FullName,
                            ex);
                    }
                    if (fieldValue is EntityChangeEvent)
                    {
                        ((EntityChangeEvent)fieldValue).PropertyName = columnName;
                    }
                }
                else
                {
                    try
                    {
                        fieldValue = ParseValueType(reader[columnName], fieldAttr.ColumnType);
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Table:{0} column:{1} parse value error:\r\n{0}", schemaTable.Name,columnName, ex);
                    }
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
        }

        private object ParseValueType(object value, Type columnType)
        {
            if (columnType == typeof(int))
            {
                return value.ToInt();
            }
            if (columnType == typeof(string))
            {
                return value.ToNotNullString();
            }
            if (columnType == typeof(decimal))
            {
                return value.ToDecimal();
            }
            if (columnType == typeof(double))
            {
                return value.ToDouble();
            }
            if (columnType == typeof(bool))
            {
                return value.ToBool();
            }
            if (columnType == typeof(byte))
            {
                return value.ToByte();
            }
            if (columnType == typeof(DateTime))
            {
                return value.ToDateTime();
            }
            if (columnType == typeof(Guid))
            {
                return (Guid)value;
            }
            if (columnType.IsEnum)
            {
                return value.ToEnum(columnType);
            }
            return value;
        }
    }
}