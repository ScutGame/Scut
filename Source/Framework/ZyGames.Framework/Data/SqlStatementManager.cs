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
using MySql.Data.MySqlClient;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Data
{
    /// <summary>
    /// sql命令管理
    /// </summary>
    public static class SqlStatementManager
    {
        private static readonly string GlobalRedisKey = "__GLOBAL_SQL_STATEMENT";
        private static int MessageQueueNum = 1;//ConfigUtils.GetSetting("MessageQueueNum", 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal static SqlParam[] ConvertSqlParam(IDataParameter[] parameters)
        {
            SqlParam[] paramList = new SqlParam[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                int size = 0;
                int dbType = (int)p.DbType;
                if (p is SqlParameter)
                {
                    size = ((SqlParameter)p).Size;
                    dbType = Convert.ToInt32(((SqlParameter)p).SqlDbType);
                }
                else if (p is MySqlParameter)
                {
                    size = ((MySqlParameter)p).Size;
                    dbType = Convert.ToInt32(((MySqlParameter)p).MySqlDbType);
                }
                paramList[i] = new SqlParam()
                {
                    ParamName = p.ParameterName,
                    DbTypeValue = dbType,
                    Size = size,
                    Value = new ProtoObject(p.Value)
                };
            }
            return paramList;
        }

        /// <summary>
        /// 放到Redsi中
        /// </summary>
        /// <param name="statement"></param>
        public static bool Put(SqlStatement statement)
        {
            bool result = false;
            RedisManager.Process(client =>
            {
                try
                {
                    string key = GetStatementKey(statement.IdentityID);
                    byte[] value = ProtoBufUtils.Serialize(statement);
                    client.ZAdd(key, value);
                    result = true;
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Sql statement put to redis error:{0}\r\n{1}", ex, JsonUtils.SerializeCustom(statement));
                }
            });
            return result;
        }

        /// <summary>
        /// 从Redis队列中取出Sql命令
        /// </summary>
        /// <param name="keyIndex">Redis队列的Index，默认从0开始</param>
        /// <param name="min"></param>
        /// <param name="max">从队列取的最大值，0为不限制</param>
        /// <returns></returns>
        public static List<SqlStatement> Pop(int keyIndex = 0, int min = 0, int max = 0)
        {
            if(max == 0)
            {
                max = int.MaxValue;
            }
            List<SqlStatement> list = new List<SqlStatement>();
            string key = GetKey(keyIndex);
            RedisManager.Process(client =>
            {
                string setId = key + "_temp";
                try
                {
                    if (!client.ContainsKey(setId) && client.ContainsKey(key))
                    {
                        client.Rename(key, setId);
                    }
                    byte[][] buffers = client.ZRange(setId, min, max);
                    if (buffers == null || buffers.Length == 0)
                    {
                        client.Remove(setId);
                        return;
                    }

                    foreach (var buffer in buffers)
                    {
                        list.Add(ProtoBufUtils.Deserialize<SqlStatement>(buffer));
                        client.ZRemove(setId, buffer);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("SqlStatement Pop setId:{0} error:{1}", setId, ex);
                }
            });

            return list;
        }

        private static string GetStatementKey(int identityID)
        {
            int index = MessageQueueNum > 1 ? (identityID % MessageQueueNum) : 0;
            return GetKey(index);
        }

        private static string GetKey(int index)
        {
            return string.Format("{0}_{1}", GlobalRedisKey, index);
        }
    }
}