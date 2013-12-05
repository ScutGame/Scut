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
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.MSMQ;
using MySql.Data.MySqlClient;

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
        /// <param name="connectionString"></param>
        public MySqlDataProvider(string connectionString)
            : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(CommandType commandType, string commandText, params IDataParameter[] parameters)
        {
            return MySqlHelper.ExecuteReader(ConnectionString, commandText, ConvertParam<MySqlParameter>(parameters));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override object ExecuteScalar(CommandType commandType, string commandText, params IDataParameter[] parameters)
        {
            return MySqlHelper.ExecuteScalar(ConnectionString, commandText, ConvertParam<MySqlParameter>(parameters));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteQuery(CommandType commandType, string commandText, params IDataParameter[] parameters)
        {
            return MySqlHelper.ExecuteNonQuery(ConnectionString, commandText, ConvertParam<MySqlParameter>(parameters));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityID"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(int identityID, CommandType commandType, string commandText, params IDataParameter[] parameters)
        {
            ActionMSMQ.Instance().SendSqlCmd(identityID, "MySqlDataProvider", ConnectionString, commandType, commandText, ConvertParam<MySqlParameter>(parameters));

            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public override bool CheckTable(string tableName, out DbColumn[] columns)
        {
            columns = null;
            string commandText = string.Format("SELECT count(1) FROM information_schema.`columns` WHERE table_name='{0}'", tableName);
            if (MySqlHelper.ExecuteScalar(ConnectionString, commandText).ToInt() > 0)
            {
                var list = new List<DbColumn>();
                commandText = string.Format("SELECT Column_Name AS ColumnName,Data_Type AS ColumnType, NUMERIC_SCALE AS scale, CHARACTER_MAXIMUM_LENGTH AS Length FROM information_schema.`columns` WHERE table_name='{0}'", tableName);

                using (var dataReader = MySqlHelper.ExecuteReader(ConnectionString, commandText))
                {
                    while (dataReader.Read())
                    {
                        var column = new DbColumn();
                        column.Name = dataReader[0].ToNotNullString();
                        column.DbType = dataReader[1].ToNotNullString();
                        column.Scale = dataReader[2].ToInt();
                        column.Length = dataReader[3].ToInt();
                        column.Type = ConvertToObjectType(ConvertToDbType(column.DbType));

                        list.Add(column);
                    }
                }
                columns = list.ToArray();
                return true;
            }
            return false;
        }

        private Type ConvertToObjectType(MySqlDbType toEnum)
        {
            switch (toEnum)
            {
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
                case MySqlDbType.Float:
                    return typeof(Double);
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
                case "decimal":
                    dbType = MySqlDbType.Decimal;
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

                case "numeric":
                    dbType = MySqlDbType.Decimal;
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
                    dbType = MySqlDbType.LongText;
                    break;
            }
            return dbType;
        }

        private string ConvertToDbType(Type type, string dbType, int length, int scale, bool isKey, string fieldName)
        {
            if (type.Equals(typeof(Int64)))
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
                return "Float";
            }
            if (type.Equals(typeof(Int32)))
            {
                return "Int";
            }
            if (type.Equals(typeof(Single)))
            {
                return "Real";
            }
            if (type.IsEnum || type.Equals(typeof(Int16)))
            {
                return "SmallInt";
            }
            if (type.Equals(typeof(Byte)))
            {
                return "TinyInt";
            }
            if (type.Equals(typeof(Byte[])))
            {
                return "Binary";
            }

            if (string.Equals(dbType, "uniqueidentifier", StringComparison.CurrentCultureIgnoreCase) ||
                type.Equals(typeof(Guid)))
            {
                return "VarChar(32)";
            }
            if (string.Equals(dbType, "varchar", StringComparison.CurrentCultureIgnoreCase) ||
                type.Equals(typeof(String)))
            {
                if (isKey && length == 0)
                {
                    return "VarChar(100)";
                }
                return length > 0
                    ? length >= 4000 ? "text" : "VarChar(" + length + ")"
                    : "VarChar(1000)";
            }

            if (string.Equals(dbType, "text", StringComparison.CurrentCultureIgnoreCase))
            {
                return dbType;
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
                command.AppendLine("Create Table `" + tableName + "`");
                command.AppendLine("(");
                List<string> keys;
                bool hasColumn = CheckProcessColumns(command, columns, out keys);
                if (keys.Count > 0)
                {
                    command.AppendLine(",");
                    command.AppendFormat("PRIMARY KEY ({0})", string.Join(",", keys));
                }
                command.AppendLine("");
                command.AppendLine(")");
                if (hasColumn)
                {
                    MySqlHelper.ExecuteNonQuery(ConnectionString, command.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Execute sql error:{0}", command), ex);
            }
        }

        private bool CheckProcessColumns(StringBuilder command, DbColumn[] columns, out List<string> keys, bool isModify = false)
        {
            keys = new List<string>();
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
                    keys.Add(dbColumn.Name);
                }
                command.AppendFormat("    `{0}` {1}{2}",
                                     dbColumn.Name,
                                     ConvertToDbType(dbColumn.Type, dbColumn.DbType, dbColumn.Length, dbColumn.Scale, dbColumn.IsKey, dbColumn.Name),
                                     dbColumn.Isnullable ? "" : " not null");
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
            try
            {
                command.Append("Alter Table `" + tableName + "`");
                command.AppendLine(" Add Column ");
                List<string> keys;
                bool hasColumn = CheckProcessColumns(command, columns, out keys);
                command.Append(";");
                if (hasColumn)
                {
                    MySqlHelper.ExecuteNonQuery(ConnectionString, command.ToString());
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

                    command.AppendFormat("ALTER TABLE `{0}` CHANGE `{1}` {2} {3};",
                                         tableName,
                                         dbColumn.Name,
                                         ConvertToDbType(dbColumn.Type, dbColumn.DbType, dbColumn.Length, dbColumn.Scale, dbColumn.IsKey, dbColumn.Name),
                                         dbColumn.Isnullable ? "" : " not null");
                    index++;
                }
                //此处MySQL的处理方式不太一样
                if (keyColumns.Count > 0)
                {
                    string[] keyArray = new string[keyColumns.Count];
                    command.AppendFormat("ALTER TABLE `{0}` DROP PRIMARY KEY;", tableName);
                    command.AppendLine();
                    int i = 0;
                    foreach (var keyColumn in keyColumns)
                    {
                        keyArray[i] = keyColumn.Name;
                        command.AppendFormat("ALTER TABLE `{0}` Change `{1}` `{1}` {2} not null;",
                                             tableName,
                                             keyColumn.Name,
                                             ConvertToDbType(keyColumn.Type, keyColumn.DbType, keyColumn.Length, keyColumn.Scale, keyColumn.IsKey, keyColumn.Name));
                        command.AppendLine();
                        i++;
                        index++;
                    }
                    command.AppendFormat("ALTER TABLE `{0}` ADD PRIMARY KEY ({1});", tableName, string.Join(",", keyArray));
                }
                if (index > 0)
                {
                    MySqlHelper.ExecuteNonQuery(ConnectionString, command.ToString());
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
        public override IDataParameter CreateParameterByText(string paramName, object value)
        {
            return MySqlParamHelper.MakeInParam(paramName, MySqlDbType.Text, 0, value);
        }

        public override CommandStruct CreateCommandStruct(string tableName, CommandMode editType)
        {
            return new MySqlCommandStruct(tableName, editType);
        }

        public override CommandFilter CreateCommandFilter()
        {
            return new MySqlCommandFilter();
        }

        public override string FormatFilterParam(string fieldNname, string paramName)
        {
            return string.Format("`{0}`={1}", fieldNname, MySqlParamHelper.FormatParamName(paramName));
        }

        public override string FormatQueryColumn(string splitChat, string[] columns)
        {
            return string.Format("`{0}`", string.Join("`" + splitChat + "`", columns));
        }
    }
}