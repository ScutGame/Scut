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
using System.Linq;
using System.Threading;
using Microsoft.Scripting;
using ServiceStack.Redis;
using ServiceStack.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net.Sql;
using ZyGames.Framework.Redis;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets.Threading;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// Data sync queue manager
    /// </summary>
    public abstract class DataSyncQueueManager
    {
        private static int sqlWaitPackSize = 1000;
        /// <summary>
        /// 同步缓存数据到Redis的队列名,存储格式:key值:typename_keycode, value值:len(4)+head[id(4)+state(4)] + value,state:1 移除操作
        /// </summary>
        public static readonly string RedisSyncQueueKey = "__QUEUE_REDIS_SYNC";
        /// <summary>
        /// 同步缓存出错队列，格式同RedisSyncQueueKey
        /// </summary>
        public static readonly string RedisSyncErrorQueueKey = "__QUEUE_REDIS_SYNC_ERROR";
        /// <summary>
        /// 同步到数据库的Sql等待队列, 存储格式:key值:typename_keycode, value值:head[id(4)+state(4)]
        /// </summary>
        public static readonly string SqlSyncWaitQueueKey = "__QUEUE_SQL_SYNC_WAIT";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string SqlSyncWaitErrirQueueKey = "__QUEUE_SQL_SYNC_WAIT_ERROR";

        private static SmartThreadPool _threadPools;
        private static Timer[] _queueWatchTimers;
        private static bool _enableWriteToDb;
        private static Timer[] _sqlWaitTimers;
        private static int[] _isRedisSyncWorking;
        private static int[] _isSqlWaitSyncWorking;
        private const int DefSqlSyncWaitQueueNum = 2;
        private const int DefDataSyncQueueNum = 2;

        /// <summary>
        /// Data sync queue num
        /// </summary>
        public static int DataSyncQueueNum
        {
            get;
            set;
        }
        /// <summary>
        /// Sql wait sync queue num
        /// </summary>
        public static int SqlWaitSyncQueueNum
        {
            get;
            set;
        }

        static DataSyncQueueManager()
        {
            DataSyncQueueNum = ConfigUtils.GetSetting("DataSyncQueueNum", DefDataSyncQueueNum);
            if (DataSyncQueueNum < 1) DataSyncQueueNum = DefDataSyncQueueNum;
            SqlWaitSyncQueueNum = ConfigUtils.GetSetting("SqlWaitSyncQueueNum", DefSqlSyncWaitQueueNum);
            if (SqlWaitSyncQueueNum < 1) SqlWaitSyncQueueNum = DefSqlSyncWaitQueueNum;

            _isRedisSyncWorking = new int[DataSyncQueueNum];
            _isSqlWaitSyncWorking = new int[SqlWaitSyncQueueNum];
        }

        /// <summary>
        /// Start
        /// </summary>
        /// <param name="setting"></param>
        public static void Start(CacheSetting setting)
        {
            _queueWatchTimers = new Timer[DataSyncQueueNum];
            for (int i = 0; i < DataSyncQueueNum; i++)
            {
                _queueWatchTimers[i] = new Timer(OnCheckRedisSyncQueue, i, 100, 100);
            }
            _threadPools = new SmartThreadPool(180 * 1000, 100, 5);
            _threadPools.Start();
            _enableWriteToDb = setting.EnableWriteToDb;
            if (_enableWriteToDb)
            {
                int sqlSyncInterval = ConfigUtils.GetSetting("Game.Cache.UpdateDbInterval", 300 * 1000);
                _sqlWaitTimers = new Timer[SqlWaitSyncQueueNum];
                for (int i = 0; i < SqlWaitSyncQueueNum; i++)
                {
                    _sqlWaitTimers[i] = new Timer(OnCheckSqlWaitSyncQueue, i, 100, sqlSyncInterval);
                }
                SqlStatementManager.Start();
            }
        }

        #region Test Method
#if TEST_METHOD

        public static void TestInit()
        {
            _enableWriteToDb = true;
        }

        public static void TestCheckRedisSyncQueue(int identity)
        {
            if (Interlocked.Exchange(ref _isRedisSyncWorking[identity], 1) == 0)
            {
                string queueKey = GetRedisSyncQueueKey(identity);
                try
                {
                    string workingKey = queueKey + "_temp";
                    byte[][] keys = null;
                    byte[][] values = null;
                    RedisConnectionPool.ProcessReadOnly(client =>
                    {
                        bool hasWorkingQueue = client.HLen(workingKey) > 0;
                        bool hasNewWorkingQueue = client.HLen(queueKey) > 0;

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
                            catch
                            {
                            }
                        }

                        var keyValuePairs = client.HGetAll(workingKey);
                        if (keyValuePairs != null && keyValuePairs.Length > 0)
                        {
                            keys = keyValuePairs.Where((buffs, index) => index % 2 == 0).ToArray();
                            values = keyValuePairs.Where((buffs, index) => index % 2 == 1).ToArray();
                            client.Remove(workingKey);
                        }
                    });

                    if (keys != null && keys.Length > 0)
                    {
                        DoProcessRedisSyncQueue(workingKey, keys, values);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("OnCheckRedisSyncQueue error:{0}", ex);
                }
                finally
                {
                    Interlocked.Exchange(ref _isRedisSyncWorking[identity], 0);
                }
            }
        }

        public static void TestCheckSqlWaitSyncQueue(int identity)
        {
            if (Interlocked.Exchange(ref _isSqlWaitSyncWorking[identity], 1) == 0)
            {
                try
                {
                    string queueKey = GetSqlWaitSyncQueueKey(identity);
                    string workingKey = queueKey + "_temp";
                    byte[][] keys = null;
                    byte[][] values = null;
                    RedisConnectionPool.ProcessReadOnly(client =>
                    {
                        bool hasWorkingQueue = client.HLen(workingKey) > 0;
                        bool hasNewWorkingQueue = client.HLen(queueKey) > 0;

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
                            catch
                            {
                            }
                        }
                        var keyValuePairs = client.HGetAll(workingKey);
                        if (keyValuePairs != null && keyValuePairs.Length > 0)
                        {
                            keys = keyValuePairs.Where((buffs, index) => index % 2 == 0).ToArray();
                            values = keyValuePairs.Where((buffs, index) => index % 2 == 1).ToArray();
                            client.Remove(workingKey);
                        }
                    });
                    if (keys != null && keys.Length > 0)
                    {
                        try
                        {

                            int pos = 0;
                            byte[][] subKeys, subValues;

                            while (true)
                            {
                                int count = pos + sqlWaitPackSize < keys.Length ? sqlWaitPackSize : keys.Length - pos;
                                if (count <= 0)
                                {
                                    break;
                                }
                                subKeys = new byte[count][];
                                subValues = new byte[count][];
                                Array.Copy(keys, pos, subKeys, 0, subKeys.Length);
                                Array.Copy(values, pos, subValues, 0, subValues.Length);
                                DoProcessSqlWaitSyncQueue(workingKey, subKeys, subValues);
                                pos += sqlWaitPackSize;
                            }
                        }
                        catch (Exception er)
                        {
                            TraceLog.WriteError("OnCheckSqlWaitSyncQueue error:{0}", er);
                            RedisConnectionPool.Process(client => client.HMSet(SqlSyncWaitErrirQueueKey, keys, values));
                        }
                    }

                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("OnCheckSqlWaitSyncQueue error:{0}", ex);
                }
                finally
                {
                    Interlocked.Exchange(ref _isSqlWaitSyncWorking[identity], 0);
                }
            }
        }

#endif
        #endregion

        /// <summary>
        /// Send to queue pool
        /// </summary>
        /// <param name="entityList"></param>
        public static void Send(AbstractEntity[] entityList)
        {
            string key = "";
            try
            {
                if (entityList == null || entityList.Length == 0) return;
                var groupList = entityList.GroupBy(t => t.GetIdentityId());
                foreach (var g in groupList)
                {
                    var valueList = g.ToList();
                    byte[][] keyBytes = new byte[valueList.Count][];
                    byte[][] valueBytes = new byte[valueList.Count][];
                    string queueKey = GetRedisSyncQueueKey(g.Key);
                    byte[] idBytes = BufferUtils.GetBytes(g.Key);

                    int index = 0;
                    foreach (var entity in valueList)
                    {
                        key = string.Format("{0}_{1}", entity.GetType().FullName, entity.GetKeyCode());
                        keyBytes[index] = RedisConnectionPool.ToByteKey(key);
                        byte[] stateBytes = BufferUtils.GetBytes(entity.IsDelete ? 1 : 0);
                        valueBytes[index] = BufferUtils.MergeBytes(
                            BufferUtils.GetBytes(idBytes.Length + stateBytes.Length),
                            idBytes,
                            stateBytes,
                            ProtoBufUtils.Serialize(entity));
                        index++;
                    }
                    RedisConnectionPool.Process(client => client.HMSet(queueKey, keyBytes, valueBytes));

                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Post changed key:{0} error:{1}", key, ex);
            }
        }

        private static string GetRedisSyncQueueKey(int identityId)
        {
            int queueIndex = identityId % DataSyncQueueNum;
            string queueKey = string.Format("{0}{1}",
                RedisSyncQueueKey,
                DataSyncQueueNum > 1 ? ":" + queueIndex : "");
            return queueKey;
        }

        private static string GetSqlWaitSyncQueueKey(int identityId)
        {
            int queueIndex = identityId % SqlWaitSyncQueueNum;
            string queueKey = string.Format("{0}{1}",
                SqlSyncWaitQueueKey,
                SqlWaitSyncQueueNum > 1 ? ":" + queueIndex : "");
            return queueKey;
        }
        private static void OnCheckRedisSyncQueue(object state)
        {
            int identity = (int)state;
            if (Interlocked.CompareExchange(ref _isRedisSyncWorking[identity], 1, 0) == 0)
            {
                try
                {
                    string queueKey = GetRedisSyncQueueKey(identity);
                    string workingKey = queueKey + "_temp";
                    byte[][] keys = new byte[0][];
                    byte[][] values = new byte[0][];
                    RedisConnectionPool.ProcessReadOnly(client =>
                    {
                        bool hasWorkingQueue = client.HLen(workingKey) > 0;
                        bool hasNewWorkingQueue = client.HLen(queueKey) > 0;

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
                            catch
                            {
                            }
                        }

                        var keyValuePairs = client.HGetAll(workingKey);
                        if (keyValuePairs != null && keyValuePairs.Length > 0)
                        {
                            keys = keyValuePairs.Where((buffs, index) => index % 2 == 0).ToArray();
                            values = keyValuePairs.Where((buffs, index) => index % 2 == 1).ToArray();
                            client.Remove(workingKey);
                        }
                    });

                    if (keys != null && keys.Length > 0)
                    {
                        _threadPools.QueueWorkItem(DoProcessRedisSyncQueue, workingKey, keys, values);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("OnCheckRedisSyncQueue error:{0}", ex);
                }
                finally
                {
                    Interlocked.Exchange(ref _isRedisSyncWorking[identity], 0);
                }
            }
        }


        private static void DoProcessRedisSyncQueue(string sysnWorkingQueueKey, byte[][] keys, byte[][] values)
        {
            try
            {
                var redisSyncErrorQueue = new List<byte[][]>();
                var setList = new List<KeyValuePair<string, byte[][]>>();
                var removeList = new List<KeyValuePair<string, byte[]>>();
                var sqlWaitSyncQueue = new List<KeyValuePair<string, byte[][]>>();

                for (int i = 0; i < keys.Length; i++)
                {
                    byte[] keyBytes = keys[i];
                    byte[] valueBytes = values[i];
                    try
                    {
                        string[] queueKey = RedisConnectionPool.ToStringKey(keyBytes).Split('_');
                        string entityParentKey = RedisConnectionPool.GetRedisEntityKeyName(queueKey[0]);
                        byte[] entityKeyBytes = RedisConnectionPool.ToByteKey(queueKey[1]);

                        byte[] headBytes;
                        byte[] entityValBytes;
                        int state;
                        int identity;
                        DecodeValueBytes(valueBytes, out headBytes, out entityValBytes, out state, out identity);

                        if (state == 1)
                        {
                            removeList.Add(new KeyValuePair<string, byte[]>(entityParentKey, entityKeyBytes));
                        }
                        else
                        {
                            setList.Add(new KeyValuePair<string, byte[][]>(entityParentKey, new[] { entityKeyBytes, entityValBytes }));
                        }
                        if (_enableWriteToDb)
                        {
                            //增加到Sql等待队列
                            string sqlWaitQueueKey = GetSqlWaitSyncQueueKey(identity);
                            sqlWaitSyncQueue.Add(new KeyValuePair<string, byte[][]>(sqlWaitQueueKey, new[] { keyBytes, headBytes }));
                        }
                    }
                    catch
                    {
                        redisSyncErrorQueue.Add(new[] { keyBytes, valueBytes });
                    }
                }
                var redisErrorKeys = redisSyncErrorQueue.Select(p => p[0]).ToArray();
                var redisErrorValues = redisSyncErrorQueue.Select(p => p[1]).ToArray();
                var sqlWaitGroups = sqlWaitSyncQueue.GroupBy(p => p.Key);
                var setGroups = setList.GroupBy(p => p.Key);
                var removeGroups = removeList.GroupBy(p => p.Key);

                RedisConnectionPool.Process(client =>
                {
                    foreach (var g in setGroups)
                    {
                        var entityKeys = g.Select(p => p.Value[0]).ToArray();
                        var entityValues = g.Select(p => p.Value[1]).ToArray();
                        client.HMSet(g.Key, entityKeys, entityValues);
                    }
                    foreach (var g in removeGroups)
                    {
                        var keybytes = g.Select(p => p.Value).ToArray();
                        client.HDel(g.Key, keybytes);
                    }
                    if (redisErrorKeys.Length > 0)
                    {
                        client.HMSet(RedisSyncErrorQueueKey, redisErrorKeys, redisErrorValues);
                    }

                    foreach (var g in sqlWaitGroups)
                    {
                        var sqlWaitKeys = g.Select(p => p.Value[0]).ToArray();
                        var sqlWaitValues = g.Select(p => p.Value[1]).ToArray();
                        client.HMSet(g.Key, sqlWaitKeys, sqlWaitValues);
                    }
                });
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("DoProcessRedisSyncQueue error:{0}", ex);
                try
                {
                    RedisConnectionPool.Process(client => client.HMSet(RedisSyncErrorQueueKey, keys, values));
                }
                catch (Exception er)
                {
                    TraceLog.WriteError("Put RedisSyncErrorQueue error:{0}", er);
                }
            }
        }


        private static void OnCheckSqlWaitSyncQueue(object state)
        {
            int identity = (int)state;
            if (Interlocked.CompareExchange(ref _isSqlWaitSyncWorking[identity], 1, 0) == 0)
            {
                try
                {
                    string queueKey = GetSqlWaitSyncQueueKey(identity);
                    string workingKey = queueKey + "_temp";
                    byte[][] keys = null;
                    byte[][] values = null;
                    RedisConnectionPool.ProcessReadOnly(client =>
                    {
                        bool hasWorkingQueue = client.HLen(workingKey) > 0;
                        bool hasNewWorkingQueue = client.HLen(queueKey) > 0;

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
                            catch
                            {
                            }
                        }

                        var keyValuePairs = client.HGetAll(workingKey);
                        if (keyValuePairs != null && keyValuePairs.Length > 0)
                        {
                            keys = keyValuePairs.Where((buffs, index) => index % 2 == 0).ToArray();
                            values = keyValuePairs.Where((buffs, index) => index % 2 == 1).ToArray();
                            client.Remove(workingKey);
                        }
                    });
                    if (keys != null && keys.Length > 0)
                    {
                        try
                        {

                            int pos = 0;
                            byte[][] subKeys, subValues;

                            while (true)
                            {
                                int count = pos + sqlWaitPackSize < keys.Length ? sqlWaitPackSize : keys.Length - pos;
                                if (count <= 0)
                                {
                                    break;
                                }
                                subKeys = new byte[count][];
                                subValues = new byte[count][];
                                Array.Copy(keys, pos, subKeys, 0, subKeys.Length);
                                Array.Copy(values, pos, subValues, 0, subValues.Length);
                                _threadPools.QueueWorkItem(DoProcessSqlWaitSyncQueue, workingKey, subKeys, subValues);
                                pos += sqlWaitPackSize;
                            }
                        }
                        catch (Exception er)
                        {
                            TraceLog.WriteError("OnCheckSqlWaitSyncQueue error:{0}", er);
                            RedisConnectionPool.Process(client => client.HMSet(SqlSyncWaitErrirQueueKey, keys, values));
                        }
                    }

                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("OnCheckSqlWaitSyncQueue error:{0}", ex);
                }
                finally
                {
                    Interlocked.Exchange(ref _isSqlWaitSyncWorking[identity], 0);
                }
            }
        }

        private static void DoProcessSqlWaitSyncQueue(string waitQueueKey, byte[][] keys, byte[][] values)
        {
            try
            {
                var sqlSender = new SqlDataSender();

                RedisConnectionPool.Process(client =>
                {
                    var sqlList = FindEntityFromRedis(sqlSender, client, keys, values);
                    var groupSqlList = sqlList.GroupBy(t => t.Key);
                    foreach (var g in groupSqlList)
                    {
                        var pairs = g.Select(t => t.Value).ToList();
                        client.ZAdd(g.Key, pairs);
                    }
                });
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("DoProcessSqlWaitSyncQueue error:{0}", ex);
            }
        }

        private static List<KeyValuePair<string, KeyValuePair<byte[], long>>> FindEntityFromRedis(SqlDataSender sender, RedisClient client, byte[][] keys, byte[][] values)
        {
            var typeKeyValuePairs = new List<KeyValuePair<string, byte[][]>>();
            for (int i = 0; i < keys.Length; i++)
            {
                byte[] keyBytes = keys[i];
                byte[] headBytes = values[i];
                string entityTypeKey = RedisConnectionPool.ToStringKey(keyBytes);
                string[] entityKeys = entityTypeKey.Split(',')[0].Split('_');
                string typeName = entityKeys[0];
                byte[] entityKeyBytes = RedisConnectionPool.ToByteKey(entityKeys[1]);
                typeKeyValuePairs.Add(
                    new KeyValuePair<string, byte[][]>(typeName, new[] { entityKeyBytes, keyBytes, headBytes })
                );
            }
            var sqlList = new List<KeyValuePair<string, KeyValuePair<byte[], long>>>();
            var typeGroup = typeKeyValuePairs.GroupBy(p => p.Key);
            foreach (var g in typeGroup)
            {
                var typeName = g.Key;
                try
                {
                    string entityParentKey = RedisConnectionPool.GetRedisEntityKeyName(typeName);
                    string asmName = "";
                    var enitityAsm = EntitySchemaSet.EntityAssembly;
                    if (enitityAsm != null)
                    {
                        asmName = "," + enitityAsm.GetName().Name;
                    }
                   
                    Type type = Type.GetType(string.Format("{0}{1}", typeName, asmName), false, true);
                    if (type == null)
                    { 
                        //调试模式下type为空处理
                        type = enitityAsm.GetType(typeName, false, true);
                        if (type == null)
                        {
                            throw new ArgumentTypeException(string.Format("Get entity \"{0}\" type is null", entityParentKey));
                        }
                    }
                    var keyBuffers = g.Select(p => p.Value[0]).ToArray();
                    var headBuffers = g.Select(p => p.Value[2]).ToArray();
                    var valueBuffers = client.HMGet(entityParentKey, keyBuffers);

                    for (int i = 0; i < keyBuffers.Length; i++)
                    {
                        string keyCode = RedisConnectionPool.ToStringKey(keyBuffers[i]);
                        var buffer = valueBuffers[i];
                        var headBytes = headBuffers[i];
                        int identity;
                        int state;
                        DecodeHeadBytes(headBytes, out identity, out state);

                        AbstractEntity entity = null;
                        if (state == 1 && buffer == null)
                        {
                            //entity remove ops 
                            entity = type.CreateInstance(new object[0]) as AbstractEntity;
                            entity.IsDelete = true;
                            entity.SetKeyValue(keyCode, typeName);
                        }
                        else if (buffer != null)
                        {
                            entity = ProtoBufUtils.Deserialize(buffer, type) as AbstractEntity;
                        }
                        if (entity != null)
                        {
                            if (state == 1) entity.IsDelete = true;
                            SqlStatement statement = sender.GenerateSqlQueue(entity);
                            var sqlValueBytes = ProtoBufUtils.Serialize(statement);
                            string sqlQueueKey = SqlStatementManager.GetSqlQueueKey(statement.IdentityID);
                            sqlList.Add(new KeyValuePair<string, KeyValuePair<byte[], long>>(sqlQueueKey,
                                new KeyValuePair<byte[], long>(sqlValueBytes, DateTime.Now.Ticks)));
                        }
                        else
                        {
                            throw new Exception(string.Format("Get \"{0}\" entity is null, keycode:{1},buffer len:{2}",
                                typeName, keyCode, buffer == null ? -1 : buffer.Length));
                        }
                    }
                }
                catch (Exception er)
                {
                    TraceLog.WriteError("FindEntityFromRedis {0} error:{1}", typeName, er);
                    var errorKeys = g.Select(p => p.Value[1]).ToArray();
                    var errorValues = g.Select(p => p.Value[2]).ToArray();
                    client.HMSet(SqlSyncWaitErrirQueueKey, errorKeys, errorValues);

                }
            }
            return sqlList;
        }


        private static void DecodeValueBytes(byte[] buffer, out byte[] headBytes, out byte[] valBytes, out int state, out int identity)
        {
            var headLen = BitConverter.ToInt32(buffer, 0);
            headBytes = new byte[headLen + 4];
            valBytes = new byte[buffer.Length - headBytes.Length];
            Buffer.BlockCopy(buffer, 0, headBytes, 0, headBytes.Length);
            Buffer.BlockCopy(buffer, headBytes.Length, valBytes, 0, valBytes.Length);
            DecodeHeadBytes(headBytes, out identity, out state);
        }
        private static void DecodeHeadBytes(byte[] buffer, out int identity, out int state)
        {
            var idBytes = new byte[4];
            var stateBytes = new byte[4];
            int pos = 4;//Head's length(4)
            Buffer.BlockCopy(buffer, pos, idBytes, 0, idBytes.Length);
            pos += idBytes.Length;
            Buffer.BlockCopy(buffer, pos, stateBytes, 0, stateBytes.Length);
            pos += stateBytes.Length;

            identity = BitConverter.ToInt32(idBytes, 0);
            state = BitConverter.ToInt32(stateBytes, 0);
        }
    }
}
