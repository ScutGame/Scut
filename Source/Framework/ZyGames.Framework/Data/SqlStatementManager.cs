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
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using MySql.Data.MySqlClient;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Threading;
using ZyGames.Framework.Config;
using ZyGames.Framework.Profile;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Data
{
    /// <summary>
    /// sql命令管理
    /// </summary>
    public abstract class SqlStatementManager
    {
        private static string SlaveMessageQueue;
        /// <summary>
        /// 同步到数据库的Sql队列, 存储格式List:SqlStatement对象
        /// </summary>
        public static readonly string SqlSyncQueueKey = "__QUEUE_SQL_SYNC";
        /// <summary>
        /// 同步到数据库的Sql出错队列，格式同SqlSyncQueueKey
        /// </summary>
        public static readonly string SqlSyncErrorQueueKey = "__QUEUE_SQL_SYNC_ERROR";
        /// <summary>
        /// 
        /// </summary>
        public static readonly string SqlSyncConnErrorQueueKey = "__QUEUE_SQL_SYNC_CONN_ERROR";
        private static Timer[] _queueWatchTimers;
        //private static SmartThreadPool _threadPools;
        private static int[] _isWatchWorking;
        private const int sqlSyncPackSize = 101;


        static SqlStatementManager()
        {
        }

        private static MessageQueueSection GetSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<MessageQueueSection>();
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsUseSyncQueue
        {
            get { return _queueWatchTimers != null; }
        }

        /// <summary>
        /// Start
        /// </summary>
        public static void Start()
        {
            TraceLog.ReleaseWriteDebug("Sql write queue start init...");
            MessageQueueSection section = GetSection();
            SlaveMessageQueue = section.SlaveMessageQueue;
            if (_queueWatchTimers != null && _queueWatchTimers.Length != section.SqlSyncQueueNum)
            {
                foreach (var timer in _queueWatchTimers)
                {
                    try
                    {
                        timer.Dispose();
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Sql write queue stop error:{0}", ex);
                    }
                }
                _queueWatchTimers = null;
            }
            if (_queueWatchTimers == null)
            {
                _isWatchWorking = new int[section.SqlSyncQueueNum];
                _queueWatchTimers = new Timer[_isWatchWorking.Length];
                for (int i = 0; i < _queueWatchTimers.Length; i++)
                {
                    _queueWatchTimers[i] = new Timer(OnCheckSqlSyncQueue, i, 100, 100);
                }
                //_threadPools = new SmartThreadPool(180 * 1000, 100, 5);
                //_threadPools.Start();
            }
        }

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

        private static IDataParameter[] ToSqlParameter(DbBaseProvider dbProvider, SqlParam[] paramList)
        {
            IDataParameter[] list = new IDataParameter[paramList.Length];
            for (int i = 0; i < paramList.Length; i++)
            {
                SqlParam param = paramList[i];
                list[i] = dbProvider.CreateParameter(param.ParamName, param.DbTypeValue, param.Size, param.Value.Value);
            }
            return list;
        }

        /// <summary>
        /// 放到Redsi中
        /// </summary>
        /// <param name="statement"></param>
        public static bool Put(SqlStatement statement)
        {
            bool result = false;
            try
            {
                if (!IsUseSyncQueue)
                {
                    return false;
                }
                string tableName = statement.Table;
                string key = GetSqlQueueKey(statement.IdentityID);
                byte[] value = ProtoBufUtils.Serialize(statement);
                RedisConnectionPool.Process(client =>
                {
                    client.ZAdd(key, DateTime.Now.Ticks, value);
                    ProfileManager.PostSqlOfMessageQueueTimes(tableName, 1);
                });
                result = true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Sql update queue write error:{0}\r\n{1}", ex, JsonUtils.SerializeCustom(statement));
            }

            return result;
        }

        /// <summary>
        /// put error sql
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool PutError(byte[] value, string key = null)
        {
            bool result = false;
            try
            {
                RedisConnectionPool.Process(client => client.ZAdd(SlaveMessageQueue + (string.IsNullOrEmpty(key) ? SqlSyncErrorQueueKey : key), DateTime.Now.Ticks, value));
                result = true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Sql error queue write error:{0}", ex);
            }
            return result;
        }

        /// <summary>
        /// Sql process queue
        /// </summary>
        /// <param name="identityId"></param>
        /// <returns></returns>
        internal static string GetSqlQueueKey(int identityId)
        {
            int index = Math.Abs(identityId) % _queueWatchTimers.Length;
            string queueKey = string.Format("{0}{1}{2}",
                SlaveMessageQueue,
                SqlSyncQueueKey,
                _queueWatchTimers.Length > 1 ? ":" + index : "");
            return queueKey;
        }

        private static void OnCheckSqlSyncQueue(object state)
        {
            int identity = (int)state;
            if (Interlocked.CompareExchange(ref _isWatchWorking[identity], 1, 0) == 0)
            {
                try
                {
                    string queueKey = GetSqlQueueKey(identity);
                    string workingKey = queueKey + "_temp";
                    bool result;
                    byte[][] bufferBytes = new byte[0][];
                    do
                    {
                        result = false;
                        RedisConnectionPool.ProcessReadOnly(client =>
                        {
                            bool hasWorkingQueue = client.ContainsKey(workingKey);
                            bool hasNewWorkingQueue = client.ContainsKey(queueKey);

                            if (!hasWorkingQueue && !hasNewWorkingQueue)
                            {
                                return;
                            }
                            if (!hasWorkingQueue)
                            {
                                try
                                {
                                    client.Rename(queueKey, workingKey);
                                }
                                catch { }
                            }

                            bufferBytes = client.ZRange(workingKey, 0, sqlSyncPackSize);
                            if (bufferBytes.Length > 0)
                            {
                                client.ZRemRangeByRank(workingKey, 0, sqlSyncPackSize);
                                result = true;
                            }
                            else
                            {
                                client.Remove(workingKey);
                            }
                        });
                        if (!result)
                        {
                            break;
                        }
                       DoProcessSqlSyncQueue(workingKey, bufferBytes);
                    } while (true);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("OnCheckSqlSyncQueue error:{0}", ex);
                }
                finally
                {
                    Interlocked.Exchange(ref _isWatchWorking[identity], 0);
                }
            }
        }

        private static void DoProcessSqlSyncQueue(string workingKey, byte[][] bufferBytes)
        {
            try
            {
                bool hasClear = false;
                foreach (var buffer in bufferBytes)
                {
                    DbBaseProvider dbProvider = null;
                    SqlStatement statement = null;
                    int result = 0;
                    try
                    {
                        statement = ProtoBufUtils.Deserialize<SqlStatement>(buffer);
                        dbProvider = DbConnectionProvider.CreateDbProvider("", statement.ProviderType,
                            statement.ConnectionString);
                        var paramList = ToSqlParameter(dbProvider, statement.Params);
                        result = dbProvider.ExecuteQuery(statement.CommandType, statement.CommandText, paramList);
                    }
                    catch (DbConnectionException connError)
                    {
                        TraceLog.WriteSqlError("SqlSync Error:{0}\r\nSql>>\r\n{1}", connError,
                            statement != null ? statement.ToString() : "");
                        if (dbProvider != null)
                        {
                            //modify error: 40 - Could not open a connection to SQL Server
                            dbProvider.ClearAllPools();

                            //resend
                            var paramList = ToSqlParameter(dbProvider, statement.Params);
                            result = dbProvider.ExecuteQuery(statement.CommandType, statement.CommandText, paramList);
                        }
                        else
                        {
                            PutError(buffer, SqlSyncConnErrorQueueKey);
                        }
                    }
                    catch (Exception e)
                    {
                        TraceLog.WriteSqlError("SqlSync Error:{0}\r\nSql>>\r\n{1}", e,
                            statement != null ? statement.ToString() : "");
                        PutError(buffer);
                        if (!hasClear && dbProvider != null)
                        {
                            //modify error: 40 - Could not open a connection to SQL Server
                            hasClear = true;
                            dbProvider.ClearAllPools();
                        }
                    }
                    finally
                    {
                        if (result > 0)
                        {
                            ProfileManager.ProcessSqlOfMessageQueueTimes(statement != null ? statement.Table : null);
                        }
                        else
                        {
                            ProfileManager.ProcessFailSqlOfMessageQueueTimes(statement != null ? statement.Table : null, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("DoProcessSqlSyncQueue error:{0}", ex);
            }
        }

    }
}