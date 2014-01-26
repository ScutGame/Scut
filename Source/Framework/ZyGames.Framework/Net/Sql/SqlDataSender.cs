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
using System.Data;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Net.Sql
{
    internal class SqlDataSender : IDataSender
    {
        public void Send<T>(T data, bool isChange = true) where T : AbstractEntity
        {
            Send(new T[] { data }, isChange, null, null);
        }

        public void Send<T>(T[] dataList) where T : AbstractEntity
        {
            Send(dataList, true, null, null);
        }

        public void Send<T>(T[] dataList, bool isChange) where T : AbstractEntity
        {
            Send(dataList, isChange, null, null);
        }

        public void Send<T>(T[] dataList, bool isChange, string connectKey, EntityBeforeProcess handle) where T : AbstractEntity
        {
            foreach (var data in dataList)
            {
                UpdateToDb(data, isChange, connectKey, handle);
            }
        }

        public void Dispose()
        {

        }

        private void UpdateToDb<T>(T data, bool isChange, string connectKey, EntityBeforeProcess handle) where T : AbstractEntity
        {
            if (data == null)
            {
                return;
            }

            SchemaTable schemaTable = data.GetSchema();
            if (!schemaTable.IsStoreInDb ||
                (string.IsNullOrEmpty(schemaTable.ConnectKey) &&
                    string.IsNullOrEmpty(schemaTable.ConnectionString)))
            {
                return;
            }

            string[] columns = GetColumns(schemaTable, data, isChange);
            if (columns == null || columns.Length == 0)
            {
                TraceLog.WriteError("Class:{0} is not change column.", data.GetType().FullName);
                return;
            }

            DbBaseProvider dbProvider = DbConnectionProvider.CreateDbProvider(connectKey ?? schemaTable.ConnectKey);
            if (dbProvider == null)
            {
                //TraceLog.WriteError("DbBaseProvider:{0} is null.", (connectKey ?? schemaTable.ConnectKey));
                return;
            }
            CommandStruct command = null;
            if (data.IsDelete)
            {
                command = dbProvider.CreateCommandStruct(schemaTable.Name, CommandMode.Delete);
            }
            else if (schemaTable.AccessLevel == AccessLevel.WriteOnly)
            {
                command = dbProvider.CreateCommandStruct(schemaTable.Name, CommandMode.Insert);
            }
            else
            {
                command = dbProvider.CreateCommandStruct(schemaTable.Name, CommandMode.ModifyInsert);
            }
            //StringBuilder changeLog = new StringBuilder();
            //changeLog.AppendFormat("\"Keys\":\"{0}\"", data.GetKeyCode());
            //处理列
            foreach (string columnName in columns)
            {
                if (columnName.IsEmpty()) continue;

                SchemaColumn schemaColumn;
                if (schemaTable.Columns.TryGetValue(columnName, out schemaColumn))
                {
                    if (schemaColumn.Disable || schemaColumn.IsIdentity)
                    {
                        continue;
                    }
                    object value = data.GetPropertyValue(schemaColumn.CanRead, columnName);
                    if (handle != null)
                    {
                        var e = new EntityEvent() { Data = data, FieldName = columnName, FieldValue = value };
                        value = handle(e);
                    }
                    if (CovertDataValue(schemaTable, schemaColumn, ref value))
                    {
                        //changeLog.AppendFormat(",\"{0}\":\"{1}\"", columnName, value);
                        IDataParameter parameter = CreateParameter(dbProvider, columnName, schemaColumn.DbType, value);
                        command.AddParameter(parameter);
                    }
                }
            }
            //处理条件
            string[] keyList = schemaTable.Keys;
            if (keyList.Length == 0)
            {
                throw new ArgumentNullException(string.Format("Table:{0} key is empty.", schemaTable.Name));
            }
            string condition = string.Empty;
            command.Filter = dbProvider.CreateCommandFilter();
            foreach (string columnName in keyList)
            {
                SchemaColumn schemaColumn;
                if (schemaTable.Columns.TryGetValue(columnName, out schemaColumn))
                {
                    string keyName = columnName;
                    string paramName = "F_" + columnName;
                    if (condition.Length > 0) condition += " AND ";
                    condition += dbProvider.FormatFilterParam(schemaColumn.Name, "", paramName);

                    object value = data.GetPropertyValue(schemaColumn.CanRead, columnName);
                    if (handle != null)
                    {
                        var e = new EntityEvent() { Data = data, FieldName = columnName, FieldValue = value };
                        value = handle(e);
                    }
                    if (CovertDataValue(schemaTable, schemaColumn, ref value))
                    {
                        IDataParameter parameter = CreateParameter(dbProvider, paramName, schemaColumn.DbType, value);
                        command.Filter.AddParam(parameter);
                        if (!schemaColumn.IsIdentity)
                        {
                            command.AddKey(CreateParameter(dbProvider, keyName, schemaColumn.DbType, value));
                        }
                    }
                }
            }
            command.Filter.Condition = condition;
            command.Parser();
            //if (schemaTable.AccessLevel == AccessLevel.ReadWrite)
            //{
            //    TraceLog.ReleaseWriteDebug("Update change \"{0}\" data:{1}", data.GetType().FullName, changeLog.ToString());
            //}
            dbProvider.ExecuteNonQuery(data.GetIdentityId(), CommandType.Text, command.Sql, command.Parameters);
            data.OnUnNew();
        }

        private static IDataParameter CreateParameter(DbBaseProvider dbProvider, string columnName, ColumnDbType dbType, object value)
        {
            IDataParameter parameter = null;
            switch (dbType)
            {
                case ColumnDbType.UniqueIdentifier:
                    parameter = dbProvider.CreateParameterByGuid(columnName, (Guid)value);
                    break;
                case ColumnDbType.Text:
                    parameter = dbProvider.CreateParameterByText(columnName, value);
                    break;
                default:
                    parameter = dbProvider.CreateParameter(columnName, value);
                    break;
            }
            return parameter;
        }

        private string[] GetColumns(SchemaTable schemaTable, AbstractEntity data, bool isChange)
        {
            //修正not change column
            string[] columns;
            if (!data.IsNew && !data.IsDelete &&
                data.HasChangePropertys && isChange &&
                schemaTable.Columns.Keys.Count > schemaTable.Keys.Length)
            {
                columns = data.DequeueChangePropertys();
            }
            else
            {
                if (data.HasChangePropertys)
                {
                    data.DequeueChangePropertys();
                }
                columns = new string[schemaTable.Columns.Keys.Count];
                schemaTable.Columns.Keys.CopyTo(columns, 0);
            }
            return columns;
        }

        private static bool CovertDataValue(SchemaTable schemaTable, SchemaColumn schemaColumn, ref object value)
        {
            if (value is DateTime && value.ToDateTime() < MathUtils.SqlMinDate)
            {
                return false;
            }

            //序列化Json
            if (schemaColumn.IsJson)
            {
                try
                {
                    value = value ?? string.Empty;
                    if (!string.IsNullOrEmpty(schemaColumn.JsonDateTimeFormat))
                    {
                        value = JsonUtils.SerializeCustom(value);
                    }
                    else
                    {
                        value = JsonUtils.Serialize(value);
                    }

                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Table:{0} column:\"{0}\" json serialize error:\r\n:{1}",
                        schemaTable.Name,
                        schemaColumn.Name,
                        ex);
                    return false;
                }
            }
            return true;
        }
    }
}