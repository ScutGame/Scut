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
using System.Linq;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;
using ZyGames.Framework.Profile;

namespace ZyGames.Framework.Net.Sql
{
    /// <summary>
    /// 
    /// </summary>
    internal class SqlDataSender : IDataSender
    {
        private readonly bool _isChange;
        private readonly string _connectKey;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isChange"></param>
        /// <param name="connectKey"></param>
        public SqlDataSender(bool isChange, string connectKey = null)
        {
            _isChange = isChange;
            _connectKey = connectKey;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool Send<T>(params T[] dataList) where T : AbstractEntity
        {
            return Send(dataList, GetPropertyValue, GetPostColumns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="getFunc"></param>
        /// <param name="postColumnFunc"></param>
        /// <param name="synchronous">是否同步</param>
        /// <returns></returns>
        public bool Send<T>(IEnumerable<T> dataList, EntityPropertyGetFunc<T> getFunc, EnttiyPostColumnFunc<T> postColumnFunc, bool synchronous = false) where T : ISqlEntity
        {
            bool result = true;
            foreach (var data in dataList)
            {
                var r = SendToDb(data, getFunc, postColumnFunc, synchronous);
                if (!r) result = r;
            }
            return result;
        }

        /// <summary>
        /// Generate sql statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="getFunc"></param>
        /// <param name="postColumnFunc"></param>
        /// <returns></returns>
        internal SqlStatement GenerateSqlQueue<T>(T data, EntityPropertyGetFunc<T> getFunc = null, EnttiyPostColumnFunc<T> postColumnFunc = null) where T : ISqlEntity
        {
            SchemaTable schemaTable = EntitySchemaSet.Get(data.GetType());
            DbBaseProvider dbProvider = DbConnectionProvider.CreateDbProvider(schemaTable.ConnectKey);
            if (dbProvider != null)
            {
                //process all columns.
                CommandStruct command = GenerateCommand(dbProvider, data, schemaTable, getFunc, postColumnFunc);
                if (command != null)
                {
                    int identityId = data.GetMessageQueueId();
                    return dbProvider.GenerateSql(identityId, command);
                }
            }
            return null;
        }

        private object GetPropertyValue<T>(T data, SchemaColumn column) where T : ISqlEntity
        {
            object value = null;
            if (data is AbstractEntity)
            {
                value = (data as AbstractEntity).GetPropertyValue(column.CanRead, column.Name);
                CovertDataValue(column, ref value);
            }
            return value;
        }

        private IList<string> GetPostColumns(ISqlEntity data, SchemaTable schemaTable, bool isChange)
        {
            List<string> columns = null;
            if (data is AbstractEntity)
            {
                var entity = data as AbstractEntity;
                //修正not change column
                if (!entity.IsNew && !entity.IsDelete &&
                    entity.HasChangePropertys && isChange &&
                    schemaTable.Columns.Keys.Count > schemaTable.Keys.Length)
                {
                    columns = entity.DequeueChangePropertys().ToList();
                }
                else
                {
                    if (entity.HasChangePropertys)
                    {
                        entity.ResetChangePropertys();
                    }
                    columns = schemaTable.GetColumnNames();
                }
            }
            return columns;
        }


        public void Dispose()
        {

        }

        private bool SendToDb<T>(T data, EntityPropertyGetFunc<T> getFunc, EnttiyPostColumnFunc<T> postColumnFunc, bool synchronous) where T : ISqlEntity
        {
            if (Equals(data, null))
            {
                return false;
            }
            SchemaTable schemaTable = EntitySchemaSet.Get(data.GetType());
            DbBaseProvider dbProvider = DbConnectionProvider.CreateDbProvider(_connectKey ?? schemaTable.ConnectKey);
            if (dbProvider == null)
            {
                return false;
            }
            CommandStruct command = GenerateCommand(dbProvider, data, schemaTable, getFunc, postColumnFunc);
            if (command != null)
            {
                int result;
                if (synchronous)
                {
                    //同时采集
                    ProfileManager.PostSqlOfMessageQueueTimes(command.TableName, 1);
                    ProfileManager.ProcessSqlOfMessageQueueTimes(command.TableName);
                    result = dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
                }
                else
                {
                    //put into pool
                    result = dbProvider.ExecuteNonQuery(data.GetMessageQueueId(), CommandType.Text, command.TableName, command.Sql, command.Parameters);
                }
                data.ResetState();
                return result > 0;
            }
            return false;
        }

        private CommandStruct GenerateCommand<T>(DbBaseProvider dbProvider, T data, SchemaTable schemaTable, EntityPropertyGetFunc<T> getFunc, EnttiyPostColumnFunc<T> postColumnFunc) where T : ISqlEntity
        {
            CommandStruct command;
            if (!(schemaTable.StorageType.HasFlag(StorageType.ReadOnlyDB) ||
                schemaTable.StorageType.HasFlag(StorageType.ReadWriteDB) ||
                schemaTable.StorageType.HasFlag(StorageType.WriteOnlyDB)) ||
                (string.IsNullOrEmpty(schemaTable.ConnectKey) &&
                 string.IsNullOrEmpty(schemaTable.ConnectionString)))
            {
                return null;
            }
            if (getFunc == null) getFunc = GetPropertyValue;

            IList<string> columns = postColumnFunc != null
                ? postColumnFunc(data, schemaTable, _isChange)
                : schemaTable.GetColumnNames();
            if (columns == null || columns.Count == 0)
            {
                TraceLog.WriteError("Class:{0} is not change column.", data.GetType().FullName);
                return null;
            }
            string tableName = schemaTable.GetTableName(data.GetCreateTime());
            if (data.IsDelete)
            {
                command = dbProvider.CreateCommandStruct(tableName, CommandMode.Delete);
            }
            else if (schemaTable.AccessLevel == AccessLevel.WriteOnly)
            {
                command = dbProvider.CreateCommandStruct(tableName, CommandMode.Insert);
            }
            else
            {
                command = dbProvider.CreateCommandStruct(tableName, CommandMode.ModifyInsert);
            }
            //StringBuilder changeLog = new StringBuilder();
            //changeLog.AppendFormat("\"Keys\":\"{0}\"", data.GetKeyCode());
            //处理列
            foreach (string columnName in columns)
            {
                if (columnName.IsEmpty()) continue;
                try
                {
                    SchemaColumn schemaColumn;
                    if (schemaTable.Columns.TryGetValue(columnName, out schemaColumn))
                    {
                        if (schemaColumn.Disable || schemaColumn.IsIdentity)
                        {
                            continue;
                        }
                        object value = getFunc(data, schemaColumn);
                        IDataParameter parameter = CreateParameter(dbProvider, columnName, schemaColumn.DbType, value);
                        command.AddParameter(parameter);

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("get {0} column val error.", columnName), ex);
                }
            }
            //处理条件
            string[] keyList = schemaTable.Keys;
            if (keyList.Length == 0)
            {
                throw new ArgumentNullException(string.Format("Table:{0} key is empty.", schemaTable.EntityName));
            }
            string condition = string.Empty;
            command.Filter = dbProvider.CreateCommandFilter();
            foreach (string columnName in keyList)
            {
                try
                {
                    SchemaColumn schemaColumn;
                    if (schemaTable.Columns.TryGetValue(columnName, out schemaColumn))
                    {
                        string keyName = columnName;
                        string paramName = "F_" + columnName;
                        if (condition.Length > 0) condition += " AND ";
                        condition += dbProvider.FormatFilterParam(schemaColumn.Name, "", paramName);
                        object value = getFunc(data, schemaColumn);
                        IDataParameter parameter = CreateParameter(dbProvider, paramName, schemaColumn.DbType, value);
                        command.Filter.AddParam(parameter);
                        //主键且自增列更新时，MSSQL与MySql处理不同，MySql需要有主键列
                        command.AddKey(CreateParameter(dbProvider, keyName, schemaColumn.DbType, value), schemaColumn.IsIdentity);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("get {0} column val error.", columnName), ex);
                }
            }
            command.Filter.Condition = condition;
            command.Parser();
            return command;
        }

        private static IDataParameter CreateParameter(DbBaseProvider dbProvider, string columnName, ColumnDbType dbType, object value)
        {
            IDataParameter parameter;
            switch (dbType)
            {
                case ColumnDbType.UniqueIdentifier:
                    parameter = dbProvider.CreateParameterByGuid(columnName, (Guid)value);
                    break;
                case ColumnDbType.LongText:
                    parameter = dbProvider.CreateParameterByLongText(columnName, value);
                    break;
                case ColumnDbType.Text:
                    parameter = dbProvider.CreateParameterByText(columnName, value);
                    break;
                case ColumnDbType.LongBlob:
                    parameter = dbProvider.CreateParameterLongBlob(columnName, value);
                    break;
                case ColumnDbType.Blob:
                    parameter = dbProvider.CreateParameterByBlob(columnName, value);
                    break;
                default:
                    parameter = dbProvider.CreateParameter(columnName, value);
                    break;
            }
            return parameter;
        }

        private static void CovertDataValue(SchemaColumn schemaColumn, ref object value)
        {
            if (value is DateTime && value.ToDateTime() < MathUtils.SqlMinDate)
            {
                value = MathUtils.SqlMinDate;
                return;
            }

            //序列化Json
            if (schemaColumn.IsSerialized)
            {
                if (schemaColumn.DbType == ColumnDbType.LongBlob || schemaColumn.DbType == ColumnDbType.Blob)
                {
                    value = SerializeBinaryObject(value);
                }
                else
                {
                    value = SerializeJson(schemaColumn, value);
                }
            }
        }

        private static object SerializeBinaryObject(object value)
        {
            return ProtoBufUtils.Serialize(value);
        }

        private static object SerializeJson(SchemaColumn schemaColumn, object value)
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
            return value;
        }
    }
}