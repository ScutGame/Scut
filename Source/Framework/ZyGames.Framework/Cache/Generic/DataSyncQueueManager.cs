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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Scripting;
using ServiceStack.Redis;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Event;
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

        private static int _entityQueueRunning;
        public static Timer _entityQueueWacher;
        public static Timer _entityQueueTimer;
        private static readonly object entitySyncRoot = new object();
        public static HashSet<string> _entitySet = new HashSet<string>();
        public static HashSet<string> _entityRemoteSet = new HashSet<string>();

        private static SmartThreadPool _threadPools;
        /// <summary>
        /// 是否启用Redis队列
        /// </summary>
        private static bool _enableRedisQueue;
        private static Timer[] _queueWatchTimers;
        private static bool _enableWriteToDb;
        private static Timer[] _sqlWaitTimers;
        private static int[] _isRedisSyncWorking;
        private static int[] _isSqlWaitSyncWorking;
        private const int DefSqlSyncWaitQueueNum = 2;
        private const int DefDataSyncQueueNum = 2;
        private static System.Action<string, byte[][], byte[][]> _asyncSendHandle;
        //todo test
        private static ConcurrentDictionary<string, KeyValuePair<int, string>> _checkVersions = new ConcurrentDictionary<string, KeyValuePair<int, string>>();

        public static long SendWaitCount;
        public static long ExecuteSuccessCount;
        public static long ExecuteFailCount;

        /// <summary>
        /// Is run completed.
        /// </summary>
        public static bool IsRunCompleted
        {
            get { return _entityQueueRunning == 0 && _entitySet.Count == 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int ExecuteCount = 20;
        /// <summary>
        /// 
        /// </summary>
        public static int MinCheckCount = 100;

        /// <summary>
        /// 
        /// </summary>
        public static int MaxCheckCount = 100000;

        /// <summary>
        /// test
        /// </summary>
        public static void ClearTrace()
        {
            _checkVersions.Clear();
        }

        private static ICacheSerializer _serializer { get; set; }

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
            _asyncSendHandle += OnAsyncSend;
            _serializer = new ProtobufCacheSerializer();
            DataSyncQueueNum = ConfigUtils.GetSetting("DataSyncQueueNum", DefDataSyncQueueNum);
            if (DataSyncQueueNum < 1) DataSyncQueueNum = DefDataSyncQueueNum;
            SqlWaitSyncQueueNum = ConfigUtils.GetSetting("SqlWaitSyncQueueNum", DefSqlSyncWaitQueueNum);
            if (SqlWaitSyncQueueNum < 1) SqlWaitSyncQueueNum = DefSqlSyncWaitQueueNum;

            _isRedisSyncWorking = new int[DataSyncQueueNum];
            _isSqlWaitSyncWorking = new int[SqlWaitSyncQueueNum];
        }

        private static void OnAsyncSend(string queueKey, byte[][] keyBytes, byte[][] valueBytes)
        {
            try
            {
                RedisConnectionPool.Process(client => client.HMSet(queueKey, keyBytes, valueBytes));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Async send to redis's key:{0} error:{1}", queueKey, ex);
            }
        }


        /// <summary>
        /// Start
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="serializer"></param>
        public static void Start(CacheSetting setting, ICacheSerializer serializer)
        {
            _serializer = serializer;
            _entityQueueTimer = new Timer(OnEntitySyncQueue, null, 60, 500);
            _entityQueueWacher = new Timer(CheckEntityQueue, null, 60, 60000);

            _enableRedisQueue = setting.EnableRedisQueue;
            if (_enableRedisQueue)
            {
                _queueWatchTimers = new Timer[DataSyncQueueNum];
                for (int i = 0; i < DataSyncQueueNum; i++)
                {
                    _queueWatchTimers[i] = new Timer(OnCheckRedisSyncQueue, i, 100, 100);
                }
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

        private static void CheckEntityQueue(object state)
        {
            try
            {
                //todo trace
                int count = _entitySet.Count;
                if (count > MinCheckCount)
                {
                    TraceLog.WriteWarn("CheckEntityQueue warn count:{0}, execute:{1}/{2}", count, ExecuteSuccessCount, SendWaitCount);
                    if (count > MaxCheckCount)
                    {
                        //大于1W，清空掉
                        Interlocked.Exchange(ref _entityRemoteSet, new HashSet<string>());
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CheckEntityQueue:{0)", ex);
            }
        }

        /// <summary>
        /// Send entity to db saved.
        /// </summary>
        /// <param name="entityList"></param>
        public static void SendToDb(params AbstractEntity[] entityList)
        {
            string key = "";
            try
            {
                if (entityList == null || entityList.Length == 0) return;

                var sender = new SqlDataSender();
                var groupList = entityList.GroupBy(t => t.GetIdentityId());
                var sqlList = new List<KeyValuePair<string, KeyValuePair<byte[], long>>>();

                foreach (var g in groupList)
                {
                    var valueList = g.ToList();

                    foreach (var entity in valueList)
                    {
                        if (entity == null) continue;

                        SqlStatement statement = sender.GenerateSqlQueue(entity);
                        if (statement == null)
                        {
                            throw new Exception(string.Format("Generate sql of \"{0}\" entity error", entity.GetType().FullName));
                        }

                        var sqlValueBytes = ProtoBufUtils.Serialize(statement);
                        string sqlQueueKey = SqlStatementManager.GetSqlQueueKey(statement.IdentityID);
                        sqlList.Add(new KeyValuePair<string, KeyValuePair<byte[], long>>(sqlQueueKey,
                            new KeyValuePair<byte[], long>(sqlValueBytes, DateTime.Now.Ticks)));
                    }
                }
                RedisConnectionPool.Process(client =>
                {
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
                TraceLog.WriteError("Send To Db key:{0} error:{1}", key, ex);
            }
        }



        private static void OnEntitySyncQueue(object state)
        {
            if (Interlocked.CompareExchange(ref _entityQueueRunning, 1, 0) == 0)
            {
                try
                {
                    var tempRemove = Interlocked.Exchange(ref _entityRemoteSet, new HashSet<string>());
                    var temp = Interlocked.Exchange(ref _entitySet, new HashSet<string>());
                    if (temp.Count == 0) return;
                    TraceLog.WriteWarn("OnEntitySyncQueue execute count:{0}, success:{1}/total {2}, fail:{3} start...", temp.Count, ExecuteSuccessCount, SendWaitCount, ExecuteFailCount);

                    RedisConnectionPool.Process(client =>
                    {
                        while (temp.Count > 0)
                        {
                            var dumpSet = temp.Take(ExecuteCount).ToArray();
                            var pipeline = client.CreatePipeline();
                            try
                            {
                                bool hasPost = false;
                                foreach (var key in dumpSet)
                                {
                                    var keyValues = key.Split('_', '|');
                                    if (keyValues.Length != 3)
                                    {
                                        TraceLog.WriteWarn("OnEntitySyncQueue:{0}", key);
                                        ExecuteFailCount++;
                                        continue;
                                    }

                                    AbstractEntity entity = CacheFactory.GetPersonalEntity(key) as AbstractEntity;
                                    int id = keyValues[1].ToInt();
                                    string keyCode = keyValues[2];
                                    string redisKey = string.Format("{0}_{1}", keyValues[0], keyCode);
                                    string hashId = GetRedisSyncQueueKey(id);
                                    byte[] idBytes = BufferUtils.GetBytes(id);
                                    var keyBytes = RedisConnectionPool.ToByteKey(redisKey);
                                    bool isDelete;
                                    byte[] entityBytes;
                                    if (entity != null)
                                    {
                                        isDelete = entity.IsDelete;
                                        entityBytes = _serializer.Serialize(entity);
                                        //modify resean: set unchange status.
                                        entity.Reset();
                                    }
                                    else if (tempRemove.Contains(key))
                                    {
                                        entityBytes = new byte[0];
                                        isDelete = true;
                                    }
                                    else
                                    {
                                        ExecuteFailCount++;
                                        TraceLog.WriteError("EntitySync queue key {0} faild object is null.", key);
                                        continue;
                                    }
                                    byte[] stateBytes = BufferUtils.GetBytes(isDelete ? 1 : 0);
                                    byte[] values =
                                        BufferUtils.MergeBytes(BufferUtils.GetBytes(idBytes.Length + stateBytes.Length),
                                            idBytes, stateBytes, entityBytes);
                                    pipeline.QueueCommand(c => ((RedisClient)c).HSet(hashId, keyBytes, values));
                                    hasPost = true;
                                    ExecuteSuccessCount++;
                                }
                                if (hasPost)
                                {
                                    pipeline.Flush();
                                }
                            }
                            finally
                            {
                                pipeline.Dispose();
                            }
                            try
                            {
                                foreach (var key in dumpSet)
                                {
                                    temp.Remove(key);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    });

                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("EntitySync queue {0}", ex);
                }
                finally
                {
                    Interlocked.Exchange(ref _entityQueueRunning, 0);
                }
            }
        }

        /// <summary>
        /// Send to queue pool
        /// </summary>
        /// <param name="entityList"></param>
        public static void Send(params AbstractEntity[] entityList)
        {
            string key = "";
            try
            {
                //modify season:异步调用时不能保证提交的顺序，造成更新到Redis中不是最后一次的
                lock (entitySyncRoot)
                {
                    foreach (var entity in entityList)
                    {
                        if (entity == null) continue;//cacheCollection has changed

                        key = string.Format("{0}_{1}|{2}",
                            RedisConnectionPool.EncodeTypeName(entity.GetType().FullName),
                            entity.GetIdentityId(),
                          entity.GetKeyCode());
                        if (_entitySet.Add(key))
                        {
                            SendWaitCount++;
                        }
                        if (entity.IsDelete)
                        {
                            _entityRemoteSet.Add(key);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Post changed key:{0} error:{1}", key, ex);
            }
        }

        /// <summary>
        /// Sync to redis queue's key
        /// </summary>
        /// <param name="identityId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check to be synchronized queue of redis
        /// </summary>
        /// <param name="state"></param>
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

        /// <summary>
        /// Process synchronized queue of redis
        /// </summary>
        /// <param name="sysnWorkingQueueKey"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
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
                        string entityTypeName = RedisConnectionPool.DecodeTypeName(queueKey[0]);
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

                        bool isStoreInDb = true;
                        SchemaTable schema;
                        if (EntitySchemaSet.TryGet(entityTypeName, out schema))
                        {
                            isStoreInDb = schema.IsStoreInDb;
                        }
                        if (_enableWriteToDb && isStoreInDb)
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

        /// <summary>
        /// Check to be synchronized wait queue of Sql
        /// </summary>
        /// <param name="state"></param>
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

        /// <summary>
        /// Process synchronized wait queue of Sql
        /// </summary>
        /// <param name="waitQueueKey"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        private static void DoProcessSqlWaitSyncQueue(string waitQueueKey, byte[][] keys, byte[][] values)
        {
            try
            {
                var sqlSender = new SqlDataSender();

                RedisConnectionPool.Process(client =>
                {
                    var sqlList = GenerateSqlFrom(sqlSender, client, keys, values);
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

        /// <summary>
        /// Generate Sql statements from the Keys-Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="client"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<string, KeyValuePair<byte[], long>>> GenerateSqlFrom(SqlDataSender sender, RedisClient client, byte[][] keys, byte[][] values)
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
                typeKeyValuePairs.Add(new KeyValuePair<string, byte[][]>(typeName, new[] { entityKeyBytes, keyBytes, headBytes })
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

                    Type type = Type.GetType(string.Format("{0}{1}", RedisConnectionPool.DecodeTypeName(typeName), asmName), false, true);
                    if (type == null)
                    {
                        //调试模式下type为空处理
                        type = enitityAsm != null ? enitityAsm.GetType(RedisConnectionPool.DecodeTypeName(typeName), false, true) : null;
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
                        var buffer = valueBuffers != null && valueBuffers.Length > i ? valueBuffers[i] : null;
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
                            entity = _serializer.Deserialize(buffer, type) as AbstractEntity;
                        }
                        if (entity != null)
                        {
                            if (state == 1) entity.IsDelete = true;
                            SqlStatement statement = sender.GenerateSqlQueue(entity);
                            if (statement == null)
                            {
                                throw new Exception(string.Format("Generate sql of \"{0}\" entity error", typeName));
                            }
                            var sqlValueBytes = ProtoBufUtils.Serialize(statement);
                            string sqlQueueKey = SqlStatementManager.GetSqlQueueKey(statement.IdentityID);
                            sqlList.Add(new KeyValuePair<string, KeyValuePair<byte[], long>>(sqlQueueKey,
                                new KeyValuePair<byte[], long>(sqlValueBytes, DateTime.Now.Ticks)));
                        }
                        else
                        {
                            throw new Exception(string.Format("Get \"{0}\" entity is null, keycode:{1},buffer len:{2},state:{3}",
                                typeName, keyCode, buffer == null ? -1 : buffer.Length, state));
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
