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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Data.Sql
{
    /// <summary>
    /// MSSQL数据库服务提供者
    /// </summary>
    public class SqlDataProvider : DbBaseProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionSetting">connection setting</param>
        public SqlDataProvider(ConnectionSetting connectionSetting)
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
                SqlConnection.ClearAllPools();
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
            using (SqlConnection conn = new SqlConnection(ConnectionString))
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
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                throw new DbConnectionException(ex.Message, ex);
            }
            //internal close connection
            if (!commandTimeout.HasValue)
            {
                return SqlHelper.ExecuteReader(conn, commandType, commandText, false, ConvertParam<SqlParameter>(parameters));
            }
            using (var cmd = CreateSqlCommand(conn, null, commandTimeout, commandText))
            {
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        public override object ExecuteScalar(CommandType commandType, int? commandTimeout, string commandText, params IDataParameter[] parameters)
        {
            object result = null;
            OpenConnection(conn =>
            {
                if (!commandTimeout.HasValue)
                {
                    result = SqlHelper.ExecuteScalar(conn, commandType, commandText, ConvertParam<SqlParameter>(parameters));
                    return;
                }
                using (var cmd = CreateSqlCommand(conn, null, commandTimeout, commandText))
                {
                    result = cmd.ExecuteScalar();
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
        /// <returns></returns>
        public override int ExecuteQuery(CommandType commandType, int? commandTimeout, string commandText, params IDataParameter[] parameters)
        {
            int result = 0;
            OpenConnection(conn =>
            {
                if (!commandTimeout.HasValue)
                {
                    result = SqlHelper.ExecuteNonQuery(conn, commandType, commandText, ConvertParam<SqlParameter>(parameters));
                    return;
                }
                using (var cmd = CreateSqlCommand(conn, null, commandTimeout, commandText))
                {
                    result = cmd.ExecuteNonQuery();
                }
            });
            return result;
        }


        private static SqlCommand CreateSqlCommand(SqlConnection connection, SqlTransaction transaction, int? commandTimeout, string commandText)
        {
            SqlCommand cmd = new SqlCommand();
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


        private void OpenConnection(Action<SqlConnection> action)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
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
            using (SqlConnection conn = new SqlConnection(ConnectionString))
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
                    yield return SqlHelper.ExecuteNonQuery(conn, command.CommandType, command.Sql, ConvertParam<SqlParameter>(command.Parameters));
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
        public override int ExecuteNonQuery(long identityId, CommandType commandType, string tableName, string commandText, params IDataParameter[] parameters)
        {
            SqlStatement statement = new SqlStatement();
            statement.IdentityID = identityId;
            statement.Table = tableName;
            statement.ConnectionString = ConnectionString;
            statement.ProviderType = "SqlDataProvider";
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
        public override SqlStatement GenerateSql(long identityId, CommandStruct command)
        {
            command.Parser();
            SqlStatement statement = new SqlStatement();
            statement.IdentityID = identityId;
            statement.Table = command.TableName;
            statement.ConnectionString = ConnectionString;
            statement.ProviderType = "SqlDataProvider";
            statement.CommandType = command.CommandType;
            statement.CommandText = command.Sql;
            statement.Params = SqlStatementManager.ConvertSqlParam(command.Parameters);
            return statement;
        }

        private string GetParametersToString(IDataParameter[] parameters)
        {
            string str = "";
            foreach (var param in parameters)
            {
                str += string.Format("{0}={1}\r\n,", param.ParameterName, param.Value);
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override bool CheckTable(string tableName, out DbColumn[] columns)
        {
            bool result = false;
            var list = new List<DbColumn>();
            OpenConnection(conn =>
            {
                string commandText = string.Format("SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'{0}') AND type in (N'U')", tableName);
                if (SqlHelper.ExecuteScalar(conn, CommandType.Text, commandText).ToInt() > 0)
                {
                    commandText = string.Format(@"
select 
  c.name as ColumnName, 
  (select top 1 name from systypes where xtype=c.xtype) as ColumnType, 
  isnull(c.scale,0) as scale, 
  c.length,
  c.isnullable,
  isnull((select top 1 k.keyno from sysobjects o, sysindexkeys k, sysindexes i
    where o.xtype='PK' and o.parent_obj=c.id and k.id=c.id and k.colid=c.colid and i.id=c.id and o.name=i.name and k.indid=i.indid),0) as keyno,
  isnull((select top 1 1 from sysobjects where xtype = 'U' and columnproperty(c.id, c.name, 'IsIdentity') = 1 and id=c.id),0) as auto_increment
from syscolumns c 
where c.id=object_id('{0}')
order by colorder ASC
", tableName);

                    using (var dataReader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandText))
                    {
                        while (dataReader.Read())
                        {
                            var column = new DbColumn();
                            column.Name = dataReader[0].ToNotNullString();
                            column.DbType = dataReader[1].ToNotNullString();
                            column.Scale = dataReader[2].ToInt();
                            column.Length = dataReader[3].ToLong();
                            column.Isnullable = dataReader[4].ToBool();
                            column.KeyNo = dataReader[5].ToInt();
                            column.HaveIncrement = dataReader["auto_increment"].ToBool();
                            column.Type = ConvertToObjectType(ConvertToDbType(column.DbType));
                            list.Add(column);
                        }
                    }
                    result = true;
                }
            });
            columns = list.ToArray();
            return result;
        }

        private Type ConvertToObjectType(SqlDbType toEnum)
        {
            switch (toEnum)
            {
                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid);
                case SqlDbType.BigInt:
                    return typeof(Int64);
                case SqlDbType.Binary:
                    return typeof(Byte[]);
                case SqlDbType.Bit:
                    return typeof(Boolean);
                case SqlDbType.Char:
                    return typeof(String);
                case SqlDbType.DateTime:
                    return typeof(DateTime);
                case SqlDbType.Decimal:
                    return typeof(Decimal);
                case SqlDbType.Real:
                    return typeof(float);
                case SqlDbType.Float:
                    return typeof(Double);
                case SqlDbType.Image:
                    return typeof(Object);
                case SqlDbType.Int:
                    return typeof(Int32);
                case SqlDbType.Money:
                    return typeof(Decimal);
                case SqlDbType.NChar:
                    return typeof(String);
                case SqlDbType.NText:
                    return typeof(String);
                case SqlDbType.NVarChar:
                    return typeof(String);
                case SqlDbType.SmallDateTime:
                    return typeof(DateTime);
                case SqlDbType.SmallInt:
                    return typeof(Int16);
                case SqlDbType.SmallMoney:
                    return typeof(Decimal);
                case SqlDbType.Text:
                    return typeof(String);
                case SqlDbType.Timestamp:
                    return typeof(Object);
                case SqlDbType.TinyInt:
                    return typeof(Byte);
                case SqlDbType.Udt://自定义的数据类型
                    return typeof(Object);
                case SqlDbType.VarBinary:
                    return typeof(Object);
                case SqlDbType.VarChar:
                    return typeof(String);
                case SqlDbType.Variant:
                    return typeof(Object);
                case SqlDbType.Xml:
                    return typeof(Object);
                default:
                    throw new ArgumentOutOfRangeException("toEnum");
            }
        }

        private SqlDbType ConvertToDbType(string sqlDbType)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (sqlDbType)
            {
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "numeric":
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "double":
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }

        private string ConvertToDbType(Type type, string dbType, long length, int scale, bool isKey)
        {
            if (MathUtils.IsEquals(dbType, "text", true) ||
               MathUtils.IsEquals(dbType, "longtext", true))
            {
                return "text";
            }
            if (MathUtils.IsEquals(dbType, "blob", true))
            {
                return "varbinary(max)";
            }
            if (MathUtils.IsEquals(dbType, "longblob", true))
            {
                return "image";
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
                return "Real";
            }
            if (type.Equals(typeof(Single)))
            {
                return "Float";
            }
            if (type.IsEnum || type.Equals(typeof(Int32)) || type.Equals(typeof(UInt32)))
            {
                return "Int";
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
                return "varbinary(max)";
            }

            if (MathUtils.IsEquals(dbType, "uniqueidentifier", true) ||
                type.Equals(typeof(Guid)))
            {
                return "UniqueIdentifier";
            }
            if (MathUtils.IsEquals(dbType, "varchar", true) ||
                type.Equals(typeof(String)))
            {
                if (isKey && length == 0)
                {
                    return "VarChar(100)";
                }
                return length > 0
                    ? length >= 4000 ? "text" : "VarChar(" + length + ")"
                    : "VarChar(255)";
            }


            return "sql_variant";
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
                command.AppendFormat("Create Table {0}", FormatName(tableName));
                command.AppendLine("(");
                List<string> keys;
                List<string> uniques;
                bool hasColumn = CheckProcessColumns(command, columns, out keys, out uniques);
                if (keys.Count > 0)
                {
                    command.AppendLine(",");
                    command.AppendFormat("constraint PK_{0} primary key({1})", tableName, FormatQueryColumn(",", keys));
                }
                if (uniques.Count > 0)
                {
                    command.AppendLine(",");
                    command.AppendFormat("constraint UQ_{0} unique({1})", tableName, FormatQueryColumn(",", uniques));
                }
                command.AppendLine("");
                command.AppendLine(")");
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

        private bool CheckProcessColumns(StringBuilder command, DbColumn[] columns, out List<string> keys, out List<string> uniques, bool isModify = false)
        {
            keys = new List<string>();
            uniques = new List<string>();
            int index = 0;
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
                command.AppendFormat("    {0} {1}{2}{3}",
                                     FormatName(dbColumn.Name),
                                     ConvertToDbType(dbColumn.Type, dbColumn.DbType, dbColumn.Length, dbColumn.Scale, dbColumn.IsKey),
                                     (dbColumn.Isnullable ? "" : " NOT NULL"),
                                     (dbColumn.IsIdentity ? dbColumn.IdentityNo > 0 ? string.Format(" IDENTITY({0},1)", dbColumn.IdentityNo) : " IDENTITY(1,1)" : ""));
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
                    command.AppendFormat("Alter Table {0}", dbTableName);
                    command.AppendLine(" Add");
                    List<string> keys;
                    List<string> uniques;
                    bool hasColumn = CheckProcessColumns(command, columns, out keys, out uniques);
                    command.Append(";");
                    if (hasColumn)
                    {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.Text, command.ToString());
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
                        command.AppendFormat("Alter Table {0} ALTER COLUMN {1} {2}{3}{4};",
                                             dbTableName,
                                             FormatName(dbColumn.Name),
                                             ConvertToDbType(dbColumn.Type, dbColumn.DbType, dbColumn.Length, dbColumn.Scale, dbColumn.IsKey),
                                             dbColumn.Isnullable ? "" : " not null",
                                             (dbColumn.IsIdentity ? dbColumn.IdentityNo > 0 ? string.Format(" IDENTITY({0},1)", dbColumn.IdentityNo) : " IDENTITY(1,1)" : ""));
                        index++;
                    }
                    if (keyColumns.Count > 0)
                    {
                        string[] keyArray = new string[keyColumns.Count];
                        if (keyColumns.Any(t => t.KeyNo > 0))
                        {
                            //check haved key in db table
                            command.AppendFormat("ALTER TABLE {0} DROP CONSTRAINT PK_{1};", dbTableName, tableName);
                            command.AppendLine();
                        }
                        int i = 0;
                        foreach (var keyColumn in keyColumns)
                        {
                            keyArray[i] = FormatName(keyColumn.Name);
                            command.AppendFormat("Alter Table {0} ALTER COLUMN {1} {2} not null;",
                                                 dbTableName,
                                                 FormatName(keyColumn.Name),
                                                 ConvertToDbType(keyColumn.Type, keyColumn.DbType, keyColumn.Length, keyColumn.Scale, keyColumn.IsKey));
                            command.AppendLine();
                            i++;
                            index++;
                        }
                        command.AppendFormat("ALTER TABLE {0} ADD CONSTRAINT PK_{1} PRIMARY KEY({2});",
                            dbTableName,
                            tableName,
                            FormatQueryColumn(",", keyArray));
                    }
                    if (index > 0)
                    {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.Text, command.ToString());
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
            return SqlParamHelper.MakeInParam(paramName, value);
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
            return SqlParamHelper.MakeInParam(paramName, (SqlDbType)dbType, size, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterByGuid(string paramName, object value)
        {
            return SqlParamHelper.MakeInParam(paramName, SqlDbType.UniqueIdentifier, 0, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterByLongText(string paramName, object value)
        {
            return SqlParamHelper.MakeInParam(paramName, SqlDbType.Text, 0, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterByText(string paramName, object value)
        {
            return SqlParamHelper.MakeInParam(paramName, SqlDbType.Text, 0, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterLongBlob(string paramName, object value)
        {
            return SqlParamHelper.MakeInParam(paramName, SqlDbType.Image, 0, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameterByBlob(string paramName, object value)
        {
            return SqlParamHelper.MakeInParam(paramName, SqlDbType.VarBinary, 0, value);
        }

        /// <summary>
        /// 创建CommandStruct对象
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="editType"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override CommandStruct CreateCommandStruct(string tableName, CommandMode editType, string columns)
        {
            return new CommandStruct(tableName, editType, columns);
        }
        /// <summary>
        /// 创建CommandFilter对象
        /// </summary>
        /// <returns></returns>
        public override CommandFilter CreateCommandFilter()
        {
            return new CommandFilter();
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
            return SqlParamHelper.FormatFilterParam(fieldName, compareChar, paramName);
        }
        /// <summary>
        /// 格式化Select语句中的列名
        /// </summary>
        /// <param name="splitChat"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override string FormatQueryColumn(string splitChat, ICollection<string> columns)
        {
            return SqlParamHelper.FormatQueryColumn(splitChat, columns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatName(string name)
        {
            return SqlParamHelper.FormatName(name);
        }
    }
}