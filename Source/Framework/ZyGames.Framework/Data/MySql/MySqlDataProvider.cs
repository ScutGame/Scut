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
using System.Text;
using ZyGames.Framework.Common;
using MySql.Data.MySqlClient;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Data.MySql
{
    /// <summary>
    /// MSSQL数据库服务提供者
    /// </summary>
    public class MySqlDataProvider : DbBaseProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionSetting">connection setting.</param>
        public MySqlDataProvider(ConnectionSetting connectionSetting)
            : base(connectionSetting)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public override void ClearAllPools()
        {
            try
            {
                MySqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                TraceLog.WriteSqlError("ClearAllPools error:{0}", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CheckConnect()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(CommandType commandType, int? commandTimeout, string commandText, params IDataParameter[] parameters)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            try
            {
                conn.Open();
        }
            catch (Exception ex)
            {
                throw new DbConnectionException(ex.Message, ex);
            }
            //internal close connection
            return DoExecuteReader(conn, null, commandTimeout, commandText, false, ConvertParam<MySqlParameter>(parameters));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override object ExecuteScalar(CommandType commandType, int? commandTimeout, string commandText, params IDataParameter[] parameters)
        {
            object result = null;
            OpenConnection(conn =>
            {
                if (!commandTimeout.HasValue)
                {
                    result = MySqlHelper.ExecuteScalar(conn, commandText, ConvertParam<MySqlParameter>(parameters));
                    return;
        }
                using (var mySqlCommand = CreateMySqlCommand(conn, null, commandTimeout, commandText))
                {
                    result = mySqlCommand.ExecuteScalar();
                }
            });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <exception cref="DbConnectionException"></exception>
        /// <returns></returns>
        public override int ExecuteQuery(CommandType commandType, int? commandTimeout, string commandText, params IDataParameter[] parameters)
        {
            int result = 0;
            OpenConnection(conn =>
            {
                if (!commandTimeout.HasValue)
                {
                    result = MySqlHelper.ExecuteNonQuery(conn, commandText, ConvertParam<MySqlParameter>(parameters));
                    return;
        }
                using (var mySqlCommand = CreateMySqlCommand(conn, null, commandTimeout, commandText))
                {
                    result = mySqlCommand.ExecuteNonQuery();
                }
            });
            return result;
        }

        private IDataReader DoExecuteReader(MySqlConnection connection, MySqlTransaction transaction, int? commandTimeout, string commandText, bool externalConn, params MySqlParameter[] commandParameters)
        {
            MySqlDataReader result;
            using (var mySqlCommand = CreateMySqlCommand(connection, null, commandTimeout, commandText))
            {
                if (commandParameters != null)
                {
                    for (int i = 0; i < commandParameters.Length; i++)
                    {
                        MySqlParameter value = commandParameters[i];
                        mySqlCommand.Parameters.Add(value);
                    }
                }
                if (externalConn)
                {
                    result = mySqlCommand.ExecuteReader();
                }
                else
                {
                    //it has closed while used end.
                    result = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            return result;
        }

        private static MySqlCommand CreateMySqlCommand(MySqlConnection connection, MySqlTransaction transaction, int? commandTimeout, string commandText)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.Transaction = transaction;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.Text;
            if (commandTimeout != null)
            {
                cmd.CommandTimeout = commandTimeout.Value;
            }
            return cmd;
        }


        private void OpenConnection(Action<MySqlConnection> action)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw new DbConnectionException(ex.Message, ex);
                }
                action(conn);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public override IEnumerable<int> ExecuteQuery(IEnumerable<CommandStruct> commands)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw new DbConnectionException(ex.Message, ex);
                }
                foreach (var command in commands)
                {
                    command.Parser();
                    yield return MySqlHelper.ExecuteNonQuery(conn, command.Sql, ConvertParam<MySqlParameter>(command.Parameters));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="commandType"></param>
        /// <param name="tableName"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(int identityId, CommandType commandType, string tableName, string commandText, params IDataParameter[] parameters)
        {
            SqlStatement statement = new SqlStatement();
            statement.IdentityID = identityId;
            statement.Table = tableName;
            statement.ConnectionString = ConnectionString;
            statement.ProviderType = "MySqlDataProvider";
            statement.CommandType = commandType;
            statement.CommandText = commandText;
            statement.Params = SqlStatementManager.ConvertSqlParam(parameters);
            return SqlStatementManager.Put(statement) ? 1 : 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public override SqlStatement GenerateSql(int identityId, CommandStruct command)
        {
            command.Parser();
            SqlStatement statement = new SqlStatement();
            statement.IdentityID = identityId;
            statement.Table = command.TableName;
            statement.ConnectionString = ConnectionString;
            statement.ProviderType = "MySqlDataProvider";
            statement.CommandType = command.CommandType;
            statement.CommandText = command.Sql;
            statement.Params = SqlStatementManager.ConvertSqlParam(command.Parameters);
            return statement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override bool CheckTable(string tableName, out DbColumn[] columns)
        {
            string commandText = string.Format("SELECT TABLE_NAME FROM `INFORMATION_SCHEMA`.`TABLES` WHERE `TABLE_SCHEMA`='{0}' AND `TABLE_NAME`='{1}'", ConnectionSetting.DatabaseName, tableName);
            bool result = false;
            var list = new List<DbColumn>();
            OpenConnection(conn =>
            {
                bool flag = false;
                using (var dr = MySqlHelper.ExecuteReader(conn, commandText))
                {
                    if (dr.Read() && !dr[0].Equals(DBNull.Value))
                    {
                        flag = true;
                    }
                }
                if (!flag) return;

                commandText = string.Format("SELECT Column_Name AS ColumnName,Data_Type AS ColumnType, NUMERIC_SCALE AS scale, CHARACTER_MAXIMUM_LENGTH AS Length, IS_NULLABLE, COLUMN_KEY, COLUMN_DEFAULT, EXTRA FROM information_schema.`columns` WHERE `TABLE_SCHEMA`='{0}' AND `TABLE_NAME`='{1}'", ConnectionSetting.DatabaseName, tableName);

                using (var dataReader = MySqlHelper.ExecuteReader(conn, commandText))
                {
                    while (dataReader.Read())
                    {
                        var column = new DbColumn();
                        column.Name = dataReader[0].ToNotNullString();
                        column.DbType = dataReader[1].ToNotNullString();
                        column.Scale = dataReader[2].ToInt();
                        column.Length = dataReader[3].ToLong();
                        column.Isnullable = dataReader[4].ToNotNullString().ToUpper() == "YES";
                        column.KeyNo = dataReader[5].ToNotNullString().ToUpper() == "PRI" ? 1 : 0;
                        column.HaveIncrement = dataReader["EXTRA"].ToNotNullString().ToLower().Contains("auto_increment");
                        column.Type = ConvertToObjectType(ConvertToDbType(column.DbType));
                        list.Add(column);
                    }
                }
                result = true;
            });
                columns = list.ToArray();
            return result;
            }

        private Type ConvertToObjectType(MySqlDbType toEnum)
        {
            switch (toEnum)
            {
                case MySqlDbType.Guid:
                    return typeof(Guid);

                case MySqlDbType.Int64:
                    return typeof(Int64);
                case MySqlDbType.Binary:
                    return typeof(Byte[]);
                case MySqlDbType.Bit:
                    return typeof(Boolean);

                case MySqlDbType.TinyText:
                case MySqlDbType.MediumText:
                case MySqlDbType.String:
                case MySqlDbType.Text:
                case MySqlDbType.VarChar:
                case MySqlDbType.LongText:
                    return typeof(String);
                case MySqlDbType.DateTime:
                    return typeof(DateTime);
                case MySqlDbType.Decimal:
                    return typeof(Decimal);
                case MySqlDbType.Double:
                    return typeof(Double);
                case MySqlDbType.Float:
                    return typeof(float);
                case MySqlDbType.LongBlob:
                    return typeof(Object);
                case MySqlDbType.Int32:
                    return typeof(Int32);
                case MySqlDbType.Int24:
                    return typeof(Int16);
                case MySqlDbType.Int16:
                    return typeof(Byte);
                case MySqlDbType.Timestamp:
                    return typeof(Object);
                case MySqlDbType.Byte:
                    return typeof(Byte);

                case MySqlDbType.Blob:
                case MySqlDbType.VarBinary:
                    return typeof(Object);

                default:
                    throw new ArgumentOutOfRangeException("toEnum");
            }
        }

        private MySqlDbType ConvertToDbType(string sqlDbType)
        {

            MySqlDbType dbType = MySqlDbType.Blob;//默认为Object

            switch (sqlDbType)
            {
                //case "guid":
                //    dbType = MySqlDbType.Guid;
                //    break;
                case "int":
                    dbType = MySqlDbType.Int32;
                    break;
                case "varchar":
                    dbType = MySqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = MySqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = MySqlDbType.DateTime;
                    break;
                case "numeric":
                case "decimal":
                    dbType = MySqlDbType.Decimal;
                    break;
                case "double":
                    dbType = MySqlDbType.Double;
                    break;
                case "float":
                    dbType = MySqlDbType.Float;
                    break;
                case "image":
                    dbType = MySqlDbType.Blob;
                    break;
                case "text":
                    dbType = MySqlDbType.Text;
                    break;
                case "bigint":
                    dbType = MySqlDbType.Int64;
                    break;
                case "binary":
                    dbType = MySqlDbType.Binary;
                    break;
                case "char":
                    dbType = MySqlDbType.VarChar;
                    break;
                case "timestamp":
                    dbType = MySqlDbType.Timestamp;
                    break;
                case "smallint":
                    dbType = MySqlDbType.Int24;
                    break;
                case "tinyint":
                    dbType = MySqlDbType.Int16;
                    break;
                case "varbinary":
                    dbType = MySqlDbType.VarBinary;
                    break;
                case "xml":
                case "longtext":
                    dbType = MySqlDbType.LongText;
                    break;
            }
            return dbType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dbType">ColumnDbType枚举类型</param>
        /// <param name="length"></param>
        /// <param name="scale"></param>
        /// <param name="isKey"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private string ConvertToDbType(Type type, string dbType, long length, int scale, bool isKey, string fieldName)
        {
            if (MathUtils.IsEquals(dbType, "text", true))
            {
                return "text";
            }
            if (MathUtils.IsEquals(dbType, "longtext", true))
            {
                return "longtext";
            }
            if (MathUtils.IsEquals(dbType, "longblob", true))
            {
                return "longblob";
            }

            if (type.Equals(typeof(Int64)) || type.Equals(typeof(UInt64)))
            {
                return "BigInt";
            }
            if (type.Equals(typeof(Boolean)))
            {
                return "Bit";
            }
            if (type.Equals(typeof(DateTime)))
            {
                return "DateTime";
            }
            if (type.Equals(typeof(Decimal)))
            {
                return "Decimal(18, " + (scale == 0 ? 4 : scale) + ")";
            }
            if (type.Equals(typeof(Double)))
            {
                return "Double";
            }
            if (type.IsEnum || type.Equals(typeof(Int32)) || type.Equals(typeof(UInt32)))
            {
                return "Int";
            }
            if (type.Equals(typeof(Single)) || type.Equals(typeof(float)))
            {
                return "Float";
            }
            if (type.Equals(typeof(Int16)) || type.Equals(typeof(UInt16)))
            {
                return "SmallInt";
            }
            if (type.Equals(typeof(Byte)))
            {
                return "TinyInt";
            }
            if (type.Equals(typeof(Byte[])))
            {
                return "LongBlob";
            }

            if (MathUtils.IsEquals(dbType, "uniqueidentifier", true) ||
                type.Equals(typeof(Guid)))
            {
                return "VarChar(36)";
            }
            if (MathUtils.IsEquals(dbType, "varchar", true) ||
                type.Equals(typeof(String)))
            {
                if (isKey && length == 0)
                {
                    return "VarChar(100)";
                }
                return length > 0
                    ? length >= 4000 ? "longtext" : "VarChar(" + length + ")"
                    : "VarChar(255)";
            }

            return "blob";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override void CreateTable(string tableName, DbColumn[] columns)
        {
            StringBuilder command = new StringBuilder();
            try
            {
                command.AppendFormat("CREATE TABLE {0}", FormatName(tableName));
                command.AppendLine("(");
                List<string> keys;
                List<string> uniques;
                int identityNo;
                bool hasColumn = CheckProcessColumns(command, columns, out keys, out uniques, out identityNo);
                if (keys.Count > 0)
                {
                    command.AppendLine(",");
                    command.AppendFormat("PRIMARY KEY ({0})", FormatQueryColumn(",", keys));
                }
                if (uniques.Count > 0)
                {
                    command.AppendLine(",");
                    command.AppendFormat("UNIQUE KEY ({0})", FormatQueryColumn(",", uniques));
                }
                command.AppendLine("");
                string charSet = string.IsNullOrEmpty(ConnectionSetting.CharSet)
                    ? " CharSet=gbk"
                    : " CharSet=" + ConnectionSetting.CharSet;
                string autoincrement = identityNo > 0 ? " AUTO_INCREMENT=" + identityNo : "";
                command.AppendFormat("){0} ENGINE=InnoDB{1};", autoincrement, charSet);
                if (hasColumn)
                {
                    ExecuteQuery(CommandType.Text, command.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Execute sql error:{0}", command), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="indexs"></param>
        public override void CreateIndexs(string tableName, string[] indexs)
        {
            StringBuilder command = new StringBuilder();
            try
            {
                foreach (var item in indexs)
                {
                    string[] columns = item.Split(',');
                    if (command.Length > 0)
                        command.AppendLine("");

                    command.AppendFormat("CREATE INDEX INDEX_{1} ON {0} ({2});",
                        FormatName(tableName),
                        string.Join("_", columns),
                        FormatQueryColumn(",", columns)
                        );
                }
                ExecuteQuery(CommandType.Text, command.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Execute sql error:{0}", command), ex);
            }
        }

        private bool CheckProcessColumns(StringBuilder command, DbColumn[] columns, out List<string> keys, out List<string> uniques, out int identityNo, bool isModify = false)
        {
            keys = new List<string>();
            uniques = new List<string>();
            int index = 0;
            identityNo = 0;
            foreach (var dbColumn in columns)
            {
                if (isModify != dbColumn.IsModify)
                {
                    continue;
                }
                if (index > 0)
                {
                    command.AppendLine(",");
                }
                if (dbColumn.IsKey)
                {
                    keys.Add(FormatName(dbColumn.Name));
                }
                if (dbColumn.IsUnique)
                {
                    uniques.Add(FormatName(dbColumn.Name));
                }
                if (dbColumn.IsIdentity) identityNo = dbColumn.IdentityNo;

                command.AppendFormat("    {0} {1}{2}{3}",
                                     FormatName(dbColumn.Name),
                                     ConvertToDbType(dbColumn.Type, dbColumn.DbType, dbColumn.Length, dbColumn.Scale, dbColumn.IsKey, dbColumn.Name),
                                     dbColumn.Isnullable ? "" : " NOT NULL",
                                     (dbColumn.IsIdentity ? " AUTO_INCREMENT" : ""));
                index++;
            }
            return index > 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override void CreateColumn(string tableName, DbColumn[] columns)
        {
            StringBuilder command = new StringBuilder();
            OpenConnection(conn =>
            {
            try
            {
                string dbTableName = FormatName(tableName);
                command.AppendFormat("ALTER TABLE {0}", dbTableName);
                command.AppendLine(" ADD COLUMN (");
                List<string> keys;
                List<string> uniques;
                int identityNo;
                bool hasColumn = CheckProcessColumns(command, columns, out keys, out uniques, out identityNo);
                command.Append(");");
                if (hasColumn)
                {
                        MySqlHelper.ExecuteNonQuery(conn, command.ToString());
                    if (identityNo > 0)
                    {
                            MySqlHelper.ExecuteNonQuery(conn, string.Format("ALTER TABLE {0} AUTO_INCREMENT={1};", dbTableName, identityNo));
                    }
                }

                command.Clear();
                List<DbColumn> keyColumns = new List<DbColumn>();
                int index = 0;
                foreach (var dbColumn in columns)
                {
                    if (!dbColumn.IsModify)
                    {
                        continue;
                    }
                    if (dbColumn.IsKey)
                    {
                        keyColumns.Add(dbColumn);
                        continue;
                    }
                    if (index > 0)
                    {
                        command.AppendLine("");
                    }
                    //ALTER TABLE `test`.`tb1`     CHANGE `Id4` `Id4t` BIGINT(20) NULL ;
                    command.AppendFormat("ALTER TABLE {0} CHANGE {1} {1} {2} {3};",
                                         dbTableName,
                                         FormatName(dbColumn.Name),
                                         ConvertToDbType(dbColumn.Type, dbColumn.DbType, dbColumn.Length, dbColumn.Scale, dbColumn.IsKey, dbColumn.Name),
                                         dbColumn.Isnullable ? "" : " NOT NULL");
                    index++;
                }
                //此处MySQL的处理主键方式不太一样
                if (keyColumns.Count > 0)
                {
                    string[] keyArray = new string[keyColumns.Count];
                        if (keyColumns.Any(t => t.KeyNo > 0))
                        {
                            //check haved key in db table
                            command.AppendFormat("ALTER TABLE {0} DROP PRIMARY KEY;", dbTableName);
                            command.AppendLine();
                        }
                    int i = 0;
                    foreach (var keyColumn in keyColumns)
                    {
                        keyArray[i] = FormatName(keyColumn.Name);
                        command.AppendFormat("ALTER TABLE {0} CHANGE {1} {1} {2} not null;",
                                             dbTableName,
                                             FormatName(keyColumn.Name),
                                             ConvertToDbType(keyColumn.Type, keyColumn.DbType, keyColumn.Length, keyColumn.Scale, keyColumn.IsKey, keyColumn.Name));
                        command.AppendLine();
                        i++;
                        index++;
                    }
                    command.AppendFormat("ALTER TABLE {0} ADD PRIMARY KEY ({1});", dbTableName, FormatQueryColumn(",", keyArray));
                }
                if (index > 0)
                {
                        MySqlHelper.ExecuteNonQuery(conn, command.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Execute sql error:{0}", command), ex);
            }

            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameter(string paramName, object value)
        {
            return MySqlParamHelper.MakeInParam(paramName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameter(string paramName, int dbType, int size, object value)
        {
            return MySqlParamHelper.MakeInParam(paramName, (MySqlDbType)dbType, size, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterByGuid(string paramName, object value)
        {
            return MySqlParamHelper.MakeInParam(paramName, MySqlDbType.VarChar, 0, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterByLongText(string paramName, object value)
        {
            return MySqlParamHelper.MakeInParam(paramName, MySqlDbType.LongText, 0, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterByText(string paramName, object value)
        {
            return MySqlParamHelper.MakeInParam(paramName, MySqlDbType.Text, 0, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterLongBlob(string paramName, object value)
        {
            return MySqlParamHelper.MakeInParam(paramName, MySqlDbType.LongBlob, 0, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterByBlob(string paramName, object value)
        {
            return MySqlParamHelper.MakeInParam(paramName, MySqlDbType.Blob, 0, value);
        }

        /// <summary>
        /// 创建CommandStruct对象
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="editType"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override CommandStruct CreateCommandStruct(string tableName, CommandMode editType, string columns = "")
        {
            return new MySqlCommandStruct(tableName, editType, columns);
        }
        /// <summary>
        /// 创建CommandFilter对象
        /// </summary>
        /// <returns></returns>
        public override CommandFilter CreateCommandFilter()
        {
            return new MySqlCommandFilter();
        }

        /// <summary>
        /// 格式化条件语句中的参数
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="compareChar"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public override string FormatFilterParam(string fieldName, string compareChar = "", string paramName = "")
        {
            return MySqlParamHelper.FormatFilterParam(fieldName, compareChar, paramName);
        }
        /// <summary>
        /// 格式化Select语句中的列名
        /// </summary>
        /// <param name="splitChat"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override string FormatQueryColumn(string splitChat, ICollection<string> columns)
        {
            return MySqlParamHelper.FormatQueryColumn(splitChat, columns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatName(string name)
        {
            return MySqlParamHelper.FormatName(name);
        }
    }
}