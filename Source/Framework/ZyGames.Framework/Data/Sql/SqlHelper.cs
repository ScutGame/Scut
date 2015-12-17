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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace ZyGames.Framework.Data.Sql
{
    /// <summary>
    /// MSSQL数据库查询、删除、更新操作辅助类
    /// </summary>
    internal sealed class SqlHelper
    {
        private enum SqlConnectionOwnership
        {
            Internal,
            External
        }
        /// <summary>
        /// 
        /// </summary>
        public static string defaultConnectionString = SqlHelper.GetConnectionString();
        private SqlHelper()
        {
        }
        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlHelper.SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            bool flag = false;
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader result;
            try
            {
                SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, commandParameters, out flag);
                SqlDataReader sqlDataReader;
                if (connectionOwnership == SqlHelper.SqlConnectionOwnership.External)
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                }
                else
                {
                    sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
                bool flag2 = true;
                foreach (SqlParameter sqlParameter in sqlCommand.Parameters)
                {
                    if (sqlParameter.Direction != ParameterDirection.Input)
                    {
                        flag2 = false;
                    }
                }
                if (flag2)
                {
                    sqlCommand.Parameters.Clear();
                }
                SqlHelper.OperatingLog(commandText);
                result = sqlDataReader;
            }
            catch
            {
                if (flag)
                {
                    connection.Close();
                }
                throw;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            SqlDataReader result = SqlHelper.ExecuteReader(connectionString, commandType, commandText, null);
            SqlHelper.OperatingLog(commandText);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            SqlConnection sqlConnection = null;
            SqlDataReader result;
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(sqlConnection, null, commandType, commandText,
                    commandParameters, SqlHelper.SqlConnectionOwnership.Internal);
                result = sqlDataReader;
            }
            catch
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                throw;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                SqlDataReader result = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
                SqlHelper.OperatingLog(spName);
                return result;
            }
            SqlDataReader result2 = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            SqlHelper.OperatingLog(spName);
            return result2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteReader(connection, commandType, commandText, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlDataReader result = SqlHelper.ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.External);
            SqlHelper.OperatingLog(commandText);
            return result;
        }
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, bool externalConn, params SqlParameter[] commandParameters)
        {
            SqlDataReader result = SqlHelper.ExecuteReader(connection, null, commandType, commandText, commandParameters,
                externalConn ? SqlConnectionOwnership.External : SqlConnectionOwnership.Internal);
            SqlHelper.OperatingLog(commandText);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }
        private static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteReader(transaction, commandType, commandText, null);
        }
        private static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            return SqlHelper.ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.External);
        }
        private static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }
        private static string GetConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["LocalSqlServer"] != null)
            {
                return ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string commandText)
        {
            return SqlHelper.ExecuteScalar(SqlHelper.defaultConnectionString, CommandType.Text, commandText);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string commandText)
        {
            return SqlHelper.ExecuteReader(SqlHelper.defaultConnectionString, CommandType.Text, commandText);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText)
        {
            return SqlHelper.ExecuteNonQuery(SqlHelper.defaultConnectionString, CommandType.Text, commandText);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string commandText)
        {
            return SqlHelper.ExecuteDataset(SqlHelper.defaultConnectionString, CommandType.Text, commandText);
        }
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandParameters != null)
            {
                for (int i = 0; i < commandParameters.Length; i++)
                {
                    SqlParameter sqlParameter = commandParameters[i];
                    if (sqlParameter != null)
                    {
                        if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
                        {
                            sqlParameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(sqlParameter);
                    }
                }
            }
        }
        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if (commandParameters == null || dataRow == null)
            {
                return;
            }
            int num = 0;
            for (int i = 0; i < commandParameters.Length; i++)
            {
                SqlParameter sqlParameter = commandParameters[i];
                if (sqlParameter.ParameterName == null || sqlParameter.ParameterName.Length <= 1)
                {
                    throw new Exception(string.Format("请在 #{0} 处提供一个有效的参数名, 参数名属性现有值如下: '{1}'.", num, sqlParameter.ParameterName));
                }
                if (dataRow.Table.Columns.IndexOf(sqlParameter.ParameterName.Substring(1)) != -1)
                {
                    sqlParameter.Value = dataRow[sqlParameter.ParameterName.Substring(1)];
                }
                num++;
            }
        }
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if (commandParameters == null || parameterValues == null)
            {
                return;
            }
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("参数数量与参数值数量不匹配.");
            }
            int i = 0;
            int num = commandParameters.Length;
            while (i < num)
            {
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter dbDataParameter = parameterValues[i] as IDbDataParameter;
                    if (dbDataParameter.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = dbDataParameter.Value;
                    }
                }
                else
                {
                    if (parameterValues[i] == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = parameterValues[i];
                    }
                }
                i++;
            }
        }
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandText == null || commandText.Length == 0)
            {
                throw new ArgumentNullException("commandText");
            }
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException("打开状态的事务允许数据操作回滚或者提交。", "事务");
                }
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                SqlHelper.AttachParameters(command, commandParameters);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                int num = SqlHelper.ExecuteNonQuery(sqlConnection, commandType, commandText, commandParameters);
                result = num;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery(connection, commandType, commandText, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters, out flag);
            int result = sqlCommand.ExecuteNonQuery();
            sqlCommand.Parameters.Clear();
            if (flag)
            {
                connection.Close();
            }
            SqlHelper.OperatingLog(commandText);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            int result;
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                result = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            else
            {
                result = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
            return result;
        }
        private static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery(transaction, commandType, commandText, null);
        }
        private static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters, out flag);
            SqlHelper.OperatingLog(commandText);
            int result = sqlCommand.ExecuteNonQuery();
            sqlCommand.Parameters.Clear();
            return result;
        }
        private static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            int result;
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                result = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            else
            {
                result = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                DataSet dataSet = SqlHelper.ExecuteDataset(sqlConnection, commandType, commandText, commandParameters);
                result = dataSet;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset(connection, commandType, commandText, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters, out flag);
            DataSet result;
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                sqlCommand.Parameters.Clear();
                if (flag)
                {
                    connection.Close();
                }
                SqlHelper.OperatingLog(commandText);
                result = dataSet;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }
        private static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset(transaction, commandType, commandText, null);
        }
        private static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters, out flag);
            SqlHelper.OperatingLog(commandText);
            DataSet result;
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                sqlCommand.Parameters.Clear();
                result = dataSet;
            }
            return result;
        }
        private static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteScalar(connectionString, commandType, commandText, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                object obj = SqlHelper.ExecuteScalar(sqlConnection, commandType, commandText, commandParameters);
                result = obj;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteScalar(connection, commandType, commandText, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters, out flag);
            object result = sqlCommand.ExecuteScalar();
            sqlCommand.Parameters.Clear();
            if (flag)
            {
                connection.Close();
            }
            SqlHelper.OperatingLog(commandText);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                object result = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
                SqlHelper.OperatingLog(spName);
                return result;
            }
            object result2 = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            SqlHelper.OperatingLog(spName);
            return result2;
        }
        private static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteScalar(transaction, commandType, commandText, null);
        }
        private static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters, out flag);
            object result = sqlCommand.ExecuteScalar();
            sqlCommand.Parameters.Clear();
            SqlHelper.OperatingLog(commandText);
            return result;
        }
        private static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteXmlReader(connection, commandType, commandText, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            bool flag = false;
            SqlCommand sqlCommand = new SqlCommand();
            XmlReader result;
            try
            {
                SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters, out flag);
                XmlReader xmlReader = sqlCommand.ExecuteXmlReader();
                sqlCommand.Parameters.Clear();
                SqlHelper.OperatingLog(commandText);
                result = xmlReader;
            }
            catch
            {
                if (flag)
                {
                    connection.Close();
                }
                throw;
            }
            return result;
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="spName"></param>
        ///<param name="parameterValues"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }
        private static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteXmlReader(transaction, commandType, commandText, null);
        }
        private static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters, out flag);
            SqlHelper.OperatingLog(commandText);
            XmlReader result = sqlCommand.ExecuteXmlReader();
            sqlCommand.Parameters.Clear();
            return result;
        }
        private static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlHelper.FillDataset(sqlConnection, commandType, commandText, dataSet, tableNames);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="commandParameters"></param>
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlHelper.FillDataset(sqlConnection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="spName"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="parameterValues"></param>
        public static void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlHelper.FillDataset(sqlConnection, spName, dataSet, tableNames, parameterValues);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            SqlHelper.FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="commandType"></param>
        ///<param name="commandText"></param>
        ///<param name="dataSet"></param>
        ///<param name="tableNames"></param>
        ///<param name="commandParameters"></param>
        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            SqlHelper.FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="spName"></param>
        ///<param name="dataSet"></param>
        ///<param name="tableNames"></param>
        ///<param name="parameterValues"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public static void FillDataset(SqlConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                SqlHelper.FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
                return;
            }
            SqlHelper.FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
        }
        ///<summary>
        ///</summary>
        ///<param name="transaction"></param>
        ///<param name="commandType"></param>
        ///<param name="commandText"></param>
        ///<param name="dataSet"></param>
        ///<param name="tableNames"></param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            SqlHelper.FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }
        ///<summary>
        ///</summary>
        ///<param name="transaction"></param>
        ///<param name="commandType"></param>
        ///<param name="commandText"></param>
        ///<param name="dataSet"></param>
        ///<param name="tableNames"></param>
        ///<param name="commandParameters"></param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            SqlHelper.FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }
        ///<summary>
        ///</summary>
        ///<param name="transaction"></param>
        ///<param name="spName"></param>
        ///<param name="dataSet"></param>
        ///<param name="tableNames"></param>
        ///<param name="parameterValues"></param>
        ///<exception cref="ArgumentNullException"></exception>
        ///<exception cref="ArgumentException"></exception>
        public static void FillDataset(SqlTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (parameterValues != null && parameterValues.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
                SqlHelper.FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
                return;
            }
            SqlHelper.FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
        }
        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, commandParameters, out flag);
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                if (tableNames != null && tableNames.Length > 0)
                {
                    string text = "Table";
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        if (tableNames[i] == null || tableNames[i].Length == 0)
                        {
                            throw new ArgumentException("输入表名必须为数组列表, 无时可为null 或空字符串.", "表名");
                        }
                        sqlDataAdapter.TableMappings.Add(text, tableNames[i]);
                        text += (i + 1).ToString();
                    }
                }
                sqlDataAdapter.Fill(dataSet);
                sqlCommand.Parameters.Clear();
                SqlHelper.OperatingLog(commandText);
            }
            if (flag)
            {
                connection.Close();
            }
        }
        ///<summary>
        ///</summary>
        ///<param name="insertCommand"></param>
        ///<param name="deleteCommand"></param>
        ///<param name="updateCommand"></param>
        ///<param name="dataSet"></param>
        ///<param name="tableName"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null)
            {
                throw new ArgumentNullException("insertCommand");
            }
            if (deleteCommand == null)
            {
                throw new ArgumentNullException("deleteCommand");
            }
            if (updateCommand == null)
            {
                throw new ArgumentNullException("updateCommand");
            }
            if (tableName == null || tableName.Length == 0)
            {
                throw new ArgumentNullException("tableName");
            }
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
            {
                sqlDataAdapter.UpdateCommand = updateCommand;
                sqlDataAdapter.InsertCommand = insertCommand;
                sqlDataAdapter.DeleteCommand = deleteCommand;
                sqlDataAdapter.Update(dataSet, tableName);
                dataSet.AcceptChanges();
            }
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="spName"></param>
        ///<param name="sourceColumns"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            SqlCommand sqlCommand = new SqlCommand(spName, connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            if (sourceColumns != null && sourceColumns.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                for (int i = 0; i < sourceColumns.Length; i++)
                {
                    spParameterSet[i].SourceColumn = sourceColumns[i];
                }
                SqlHelper.AttachParameters(sqlCommand, spParameterSet);
            }
            SqlHelper.OperatingLog(spName);
            return sqlCommand;
        }
        ///<summary>
        ///</summary>
        ///<param name="connectionString"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static int ExecuteNonQueryTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static int ExecuteNonQueryTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="transaction"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        ///<exception cref="ArgumentException"></exception>
        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="connectionString"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static DataSet ExecuteDatasetTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="transaction"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        ///<exception cref="ArgumentException"></exception>
        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="connectionString"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static SqlDataReader ExecuteReaderTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="transaction"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        ///<exception cref="ArgumentException"></exception>
        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="connectionString"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static object ExecuteScalarTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static object ExecuteScalarTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="transaction"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        ///<exception cref="ArgumentException"></exception>
        public static object ExecuteScalarTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }
        ///<summary>
        ///</summary>
        ///<param name="transaction"></param>
        ///<param name="spName"></param>
        ///<param name="dataRow"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        ///<exception cref="ArgumentException"></exception>
        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("该事务已经被回滚或提交, 请提供一个正打开的事务.", "事务");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                SqlHelper.AssignParameterValues(spParameterSet, dataRow);
                return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }
        private static void OperatingLog(string OperatingSql)
        {
            return;
        }
        private static void ExecuteNonQueryaasd(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlHelper.ExecuteNonQueryaasd(sqlConnection, commandType, commandText, commandParameters);
            }
        }
        ///<summary>
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="commandType"></param>
        ///<param name="commandText"></param>
        ///<param name="commandParameters"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static int ExecuteNonQueryaasd(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqlCommand sqlCommand = new SqlCommand();
            bool flag = false;
            SqlHelper.PrepareCommand(sqlCommand, connection, null, commandType, commandText, commandParameters, out flag);
            int result = sqlCommand.ExecuteNonQuery();
            sqlCommand.Parameters.Clear();
            if (flag)
            {
                connection.Close();
            }
            return result;
        }
        //private static int LoadCookie()
        //{
        //    if (HttpContext.Current.Request.Cookies["userid"] != null)
        //    {
        //        int result = 0;
        //        string value = HttpContext.Current.Request.Cookies["userid"].Value;
        //        if (int.TryParse(value, out result))
        //        {
        //            return result;
        //        }
        //    }
        //    return 0;
        //}
    }
}