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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool TryReceive<T>(out List<T> dataList) where T : AbstractEntity, new()
        {
            bool result = false;
            dataList = null;
            if (string.IsNullOrEmpty(_schema.ConnectKey) && string.IsNullOrEmpty(_schema.ConnectionString))
            {
                dataList = new List<T>();
                return true;
            }
            using (IDataReader reader = GetReader<T>())
            {
                if (reader != null)
                {
                    dataList = new List<T>();
                    while (reader.Read())
                    {
                        T entity = new T();
                        entity.Init();
                        entity.SetValueBefore();
                        SetEntityValue(_schema, reader, entity);
                        entity.SetValueAfter();
                        dataList.Add(entity);
                    }
                    result = true;
                }
            }
            return result;
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
                var command = dbprovider.CreateCommandStruct(_schema.Name, CommandMode.Inquiry);
                var columns = _schema.GetColumnNames();
                command.Columns = string.Join(",", columns);
                command.OrderBy = (string.IsNullOrEmpty(_filter.OrderColumn) ? _schema.OrderColumn : _filter.OrderColumn) ?? "";
                if (Capacity != int.MaxValue && Capacity > 0)
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
                try
                {
                    return dbprovider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters);
                }
                catch (Exception)
                {
                    //重执行一次
                    return dbprovider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters);
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

        private void SetEntityValue(SchemaTable schemaTable, IDataReader reader, AbstractEntity entity)
        {
            var columns = schemaTable.GetColumnNames();
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
                    if (fieldAttr.ColumnType.IsSubclassOf(typeof(Array)))
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
                        TraceLog.WriteError("Table:{0} key:{1} column:{2} deserialize json error:{3} to {4}\r\nException:{5}",
                            schemaTable.Name,
                            entity.GetKeyCode(),
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
                        TraceLog.WriteError("Table:{0} column:{1} parse value error:\r\n{0}", schemaTable.Name, columnName, ex);
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