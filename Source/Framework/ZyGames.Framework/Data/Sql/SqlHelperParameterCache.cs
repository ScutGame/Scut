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
using System.Data;
using System.Data.SqlClient;

namespace ZyGames.Framework.Data.Sql
{
    ///<summary>
    /// MSSQL数据库参数缓存
    ///</summary>
    internal sealed class SqlHelperParameterCache
    {
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        private SqlHelperParameterCache()
        {
        }
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
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
            connection.Open();
            SqlCommandBuilder.DeriveParameters(sqlCommand);
            connection.Close();
            if (!includeReturnValueParameter)
            {
                sqlCommand.Parameters.RemoveAt(0);
            }
            int count = sqlCommand.Parameters.Count;
            SqlParameter[] array = new SqlParameter[count];
            sqlCommand.Parameters.CopyTo(array, 0);
            SqlParameter[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                SqlParameter sqlParameter = array2[i];
                sqlParameter.Value = DBNull.Value;
            }
            return array;
        }
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            int num = originalParameters.Length;
            SqlParameter[] array = new SqlParameter[num];
            int i = 0;
            int num2 = num;
            while (i < num2)
            {
                array[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
                i++;
            }
            return array;
        }
        ///<summary>
        ///</summary>
        ///<param name="connectionString"></param>
        ///<param name="commandText"></param>
        ///<param name="commandParameters"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (commandText == null || commandText.Length == 0)
            {
                throw new ArgumentNullException("commandText");
            }
            string key = connectionString + ":" + commandText;
            SqlHelperParameterCache.paramCache[key] = commandParameters;
        }
        ///<summary>
        ///</summary>
        ///<param name="connectionString"></param>
        ///<param name="commandText"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (commandText == null || commandText.Length == 0)
            {
                throw new ArgumentNullException("commandText");
            }
            string key = connectionString + ":" + commandText;
            SqlParameter[] array = SqlHelperParameterCache.paramCache[key] as SqlParameter[];
            if (array == null)
            {
                return null;
            }
            return SqlHelperParameterCache.CloneParameters(array);
        }
        ///<summary>
        ///</summary>
        ///<param name="connectionString"></param>
        ///<param name="spName"></param>
        ///<returns></returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return SqlHelperParameterCache.GetSpParameterSet(connectionString, spName, false);
        }
        ///<summary>
        ///</summary>
        ///<param name="connectionString"></param>
        ///<param name="spName"></param>
        ///<param name="includeReturnValueParameter"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            SqlParameter[] result;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlParameter[] spParameterSetInternal = SqlHelperParameterCache.GetSpParameterSetInternal(sqlConnection, spName, includeReturnValueParameter);
                result = spParameterSetInternal;
            }
            return result;
        }
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return SqlHelperParameterCache.GetSpParameterSet(connection, spName, false);
        }
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqlParameter[] result;
            using (SqlConnection sqlConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                SqlParameter[] spParameterSetInternal = SqlHelperParameterCache.GetSpParameterSetInternal(sqlConnection, spName, includeReturnValueParameter);
                result = spParameterSetInternal;
            }
            return result;
        }
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("spName");
            }
            string key = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
            SqlParameter[] array = SqlHelperParameterCache.paramCache[key] as SqlParameter[];
            if (array == null)
            {
                SqlParameter[] array2 = SqlHelperParameterCache.DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                SqlHelperParameterCache.paramCache[key] = array2;
                array = array2;
            }
            return SqlHelperParameterCache.CloneParameters(array);
        }
    }
}