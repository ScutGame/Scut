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
using ZyGames.Framework.Common.Threading;
using ZyGames.Framework.Config;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Net.Sql;
using ZyGames.Framework.Profile;
using ZyGames.Framework.Redis;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// Data sync queue manager
    /// </summary>
    public abstract class DataSyncQueueManager
    {
        class RedisEntityItem
        {
            public string HashId { get; set; }
            public int UserId { get; set; }

            public byte[] KeyBytes { get; set; }
            public byte[] ValueBytes { get; set; }

            public int State { get; set; }

            public bool HasMutilKey { get; set; }
        }

        private static string SlaveMessageQueue;

        private static int sqlWaitPackSize = 1000;

        /// <summary>
        /// Write redis sync queue count
        /// </summary>
        public static int SyncQueueCount
        {
            get { return _queueWatchTimers.Length; }
        }
        /// <summary>
        /// 同步缓存数据到Redis的队列名,存储格式:key值:typename_keycode, value值:len(4)+head[id(4)+state(4)] + value,state:1 移除操作
        /// </summary>
        public static string RedisSyncQueueKey
        {
            get { return SlaveMessageQueue + "__QUEUE_REDIS_SYNC"; }
        }

        /// <summary>
        /// 同步缓存出错队列，格式同RedisSyncQueueKey
        /// </summary>
        public static string RedisSyncErrorQueueKey
        {
            get { return SlaveMessageQueue + "__QUEUE_REDIS_SYNC_ERROR"; }
        }

        /// <summary>
        /// 同步到数据库的Sql等待队列, 存储格式:key值:typename_keycode, value值:head[id(4)+state(4)]
        /// </summary>
        public static string SqlSyncWaitQueueKey
        {
            get { return SlaveMessageQueue + "__QUEUE_SQL_SYNC_WAIT"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string SqlSyncWaitErrirQueueKey
        {
            get { return SlaveMessageQueue + "__QUEUE_SQL_SYNC_WAIT_ERROR"; }
        }

        private static int _entityQueueRunning;
        /// <summary>
        /// 
        /// </summary>
        private static Timer _entityQueueTimer;
        private static readonly object entitySyncRoot = new object();
        /// <summary>
        /// 
        /// </summary>
        private static HashSet<string> _entitySet = new HashSet<string>();
        /// <summary>
        /// 
        /// </summary>
        private static HashSet<string> _entityRemoteSet = new HashSet<string>();

        //private static SmartThreadPool _threadPools;
        /// <summary>
        /// 是否启用Redis队列
        /// </summary>
        private static bool _enableRedisQueue;
        private static Timer[] _queueWatchTimers;
        private static bool _enableWriteToDb;
        private static Timer[] _sqlWaitTimers;
        private static int[] _isRedisSyncWorking;
        private static int[] _isSqlWaitSyncWorking;
        private static ICacheSerializer _serializer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static void SetWriteToDbState(bool enable)
        {
            _enableWriteToDb = enable;
        }
        /// <summary>
        /// Is run completed.
        /// </summary>
        public static bool IsRunCompleted
        {
            get
            {
                return _entityQueueRunning == 0 &&
                       _entitySet.Count == 0;
            }
        }

        private static MessageQueueSection GetSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<MessageQueueSection>();
        }


        static DataSyncQueueManager()
        {
            _serializer = new ProtobufCacheSerializer();
            ConfigManager.ConfigReloaded += OnConfigReload;
        }

        private static void OnConfigReload(object sender, ConfigReloadedEventArgs e)
        {
            try
            {
                MessageQueueSection section = GetSection();
                InitRedisQueue(section);
                InitSqlQueue(section);
                DbConnectionProvider.Initialize();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ConfigReload error:{0}", ex);
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
            //_threadPools = new SmartThreadPool(180 * 1000, 100, 5);
            _entityQueueTimer = new Timer(OnEntitySyncQueue, null, 60, 100);
            MessageQueueSection section = GetSection();
            InitRedisQueue(section);
            InitSqlQueue(section);
            //_threadPools.Start();
        }

        private static void InitRedisQueue(MessageQueueSection section)
        {
            SlaveMessageQueue = section.SlaveMessageQueue;
            TraceLog.ReleaseWriteDebug("Redis write queue start init...");
            if (_queueWatchTimers != null && (!section.EnableRedisQueue || _queueWatchTimers.Length != section.DataSyncQueueNum))
            {
                foreach (var timer in _queueWatchTimers)
                {
                    try
                    {
                        timer.Dispose();
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Redis write queue stop error:{0}", ex);
                    }
                }
                _queueWatchTimers = null;
            }
            _enableRedisQueue = section.EnableRedisQueue;
            if (_enableRedisQueue && _queueWatchTimers == null)
            {
                _isRedisSyncWorking = new int[section.DataSyncQueueNum];
                _queueWatchTimers = new Timer[_isRedisSyncWorking.Length];
                for (int i = 0; i < _queueWatchTimers.Length; i++)
                {
                    _queueWatchTimers[i] = new Timer(OnCheckRedisSyncQueue, i, 100, 100);
                }
            }
        }

        private static void InitSqlQueue(MessageQueueSection section)
        {
            TraceLog.ReleaseWriteDebug("Sql wait write queue start init...");
            if (_sqlWaitTimers != null && (!section.EnableWriteToDb || _sqlWaitTimers.Length != section.SqlWaitSyncQueueNum))
            {
                foreach (var timer in _sqlWaitTimers)
                {
                    try
                    {
                        timer.Dispose();
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Sql wait write queue stop error:{0}", ex);
                    }
                }
                _sqlWaitTimers = null;
            }
            else if (_sqlWaitTimers != null && section.EnableWriteToDb)
            {
                foreach (var timer in _sqlWaitTimers)
                {
                    timer.Change(100, section.SqlSyncInterval);
                }
                SqlStatementManager.Start();
            }
            _enableWriteToDb = section.EnableWriteToDb;
            if (_enableWriteToDb && _sqlWaitTimers == null)
            {
                _isSqlWaitSyncWorking = new int[section.SqlWaitSyncQueueNum];
                _sqlWaitTimers = new Timer[_isSqlWaitSyncWorking.Length];
                for (int i = 0; i < _sqlWaitTimers.Length; i++)
                {
                    _sqlWaitTimers[i] = new Timer(OnCheckSqlWaitSyncQueue, i, 100, section.SqlSyncInterval);
                }
                SqlStatementManager.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityList"></param>
        public static void SendToDb<T>(params T[] entityList) where T : ISqlEntity
        {
            SendToDb<T>(null, null, entityList);
        }

        /// <summary>
        /// Send entity to db saved.
        /// </summary>
        /// <param name="postColumnFunc"></param>
        /// <param name="getPropertyFunc"></param>
        /// <param name="entityList"></param>
        public static void SendToDb<T>(EntityPropertyGetFunc<T> getPropertyFunc, EnttiyPostColumnFunc<T> postColumnFunc, params T[] entityList) where T : ISqlEntity
        {
            string key = "";
            try
            {
                if (entityList == null || entityList.Length == 0) return;
                var sender = new SqlDataSender(false);
                var groupList = entityList.GroupBy(t => t.GetMessageQueueId());
                var sqlList = new List<KeyValuePair<string, KeyValuePair<byte[], long>>>();

                foreach (var g in groupList)
                {
                    key = g.Key.ToString();
                    var valueList = g.ToList();

                    foreach (var entity in valueList)
                    {
                        if (entity == null) continue;

                        SqlStatement statement = sender.GenerateSqlQueue<T>(entity, getPropertyFunc, postColumnFunc);
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
                RedisConnectionPool.ProcessPipeline(p =>
                {
                    bool hasPost = false;
                    var groupSqlList = sqlList.GroupBy(t => t.Key);
                    int sqlCount = sqlList.Count;
                    foreach (var g in groupSqlList)
                    {
                        var pairs = g.Select(t => t.Value).ToList();
                        p.QueueCommand(client => ((RedisClient)client).ZAdd(g.Key, pairs), () =>
                        {//onSuccess
                            ProfileManager.PostSqlOfMessageQueueTimes(g.Key, sqlCount);
                        });
                        hasPost = true;
                    }
                    if (hasPost)
                    {
                        p.Flush();
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
                    if (temp.Count == 0 || _queueWatchTimers == null) return;

                    RedisConnectionPool.ProcessPipeline(pipeline =>
                    {
                        bool hasPost = false;
                        foreach (var key in temp)
                        {
                            var keyValues = key.Split('_', '|');
                            if (keyValues.Length != 3)
                            {
                                TraceLog.WriteWarn("OnEntitySyncQueue:{0}", key);
                                continue;
                            }

                            AbstractEntity entity = CacheFactory.GetPersonalEntity(key) as AbstractEntity;
                            int id = AbstractEntity.DecodeKeyCode(keyValues[1]).ToInt();
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
                                TraceLog.WriteError("EntitySync queue key {0} faild object is null.", key);
                                continue;
                            }
                            byte[] stateBytes = BufferUtils.GetBytes(isDelete ? 1 : 0);
                            byte[] values =
                                BufferUtils.MergeBytes(BufferUtils.GetBytes(idBytes.Length + stateBytes.Length),
                                    idBytes, stateBytes, entityBytes);
                            pipeline.QueueCommand(c =>
                            {
                                ((RedisClient)c).HSet(hashId, keyBytes, values);
                            });
                            hasPost = true;
                        }
                        if (hasPost)
                        {
                            pipeline.Flush();
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
            AbstractEntity temp = null;
            try
            {
                //modify season:异步调用时不能保证提交的顺序，造成更新到Redis中不是最后一次的
                var expireList = new List<AbstractEntity>();
                foreach (var entity in entityList)
                {
                    if (entity == null) continue;
                    if (entity.IsExpired || !entity.IsInCache)
                    {
                        expireList.Add(entity);
                        continue;
                    }
                    temp = entity;
                    TransSend(entity);
                }
                if (expireList.Count > 0)
                {
                    SendSync(expireList);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Post changed key:{0} error:{1}", GetQueueFormatKey(temp), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static void TransSend(AbstractEntity entity)
        {
            //watch post entity changed times.
            ProfileManager.PostEntityOfMessageQueueTimes(entity.GetType().FullName, entity.GetKeyCode(), GetOperateMode(entity));

            entity.TempTimeModify = MathUtils.Now;
            string key = GetQueueFormatKey(entity);
            if (!entity.IsInCache)
            {
                CacheFactory.AddOrUpdateEntity(key, entity);
            }
            lock (entitySyncRoot)
            {
                _entitySet.Add(key);
                if (entity.IsDelete)
                {
                    _entityRemoteSet.Add(key);
                }
            }
        }

        private static OperateMode GetOperateMode(AbstractEntity entity)
        {
            if (entity.IsDelete) return OperateMode.Remove;
            if (entity.IsNew) return OperateMode.Add;
            return OperateMode.Modify;
        }

        private static string GetQueueFormatKey(AbstractEntity entity)
        {
            return CacheFactory.GenerateEntityKey(entity);
        }

        /// <summary>
        /// Sync to redis queue's key
        /// </summary>
        /// <param name="identityId"></param>
        /// <returns></returns>
        private static string GetRedisSyncQueueKey(int identityId)
        {
            int queueIndex = Math.Abs(identityId) % _queueWatchTimers.Length;
            string queueKey = string.Format("{0}{1}",
                RedisSyncQueueKey,
                _queueWatchTimers.Length > 1 ? ":" + queueIndex : "");
            return queueKey;
        }

        private static string GetSqlWaitSyncQueueKey(int identityId)
        {
            int queueIndex = Math.Abs(identityId) % _sqlWaitTimers.Length;
            string queueKey = string.Format("{0}{1}",
                SqlSyncWaitQueueKey,
                _sqlWaitTimers.Length > 1 ? ":" + queueIndex : "");
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
        /// <summary>
        /// 以同步的方式更新Entity
        /// </summary>
        /// <param name="entityList"></param>
        public static bool SendSync(IEnumerable<AbstractEntity> entityList)
        {
            var keyList = new List<byte[]>();
            var valueList = new List<byte[]>();
            foreach (var entity in entityList)
            {
                if (entity == null)
                {
                    continue;
                }
                //watch post entity changed times.
                ProfileManager.PostEntityOfMessageQueueTimes(entity.GetType().FullName, entity.GetKeyCode(), GetOperateMode(entity));

                entity.TempTimeModify = MathUtils.Now;
                string key = GetQueueFormatKey(entity);
                var keyValues = key.Split('_', '|');
                int id = AbstractEntity.DecodeKeyCode(keyValues[1]).ToInt();
                string keyCode = keyValues[2];
                string redisKey = string.Format("{0}_{1}", keyValues[0], keyCode);
                byte[] idBytes = BufferUtils.GetBytes(id);
                var keyBytes = RedisConnectionPool.ToByteKey(redisKey);
                byte[] entityBytes;
                bool isDelete = entity.IsDelete;
                entityBytes = _serializer.Serialize(entity);
                //modify resean: set unchange status.
                entity.Reset();

                byte[] stateBytes = BufferUtils.GetBytes(isDelete ? 1 : 0);
                byte[] values = BufferUtils.MergeBytes(BufferUtils.GetBytes(idBytes.Length + stateBytes.Length), idBytes, stateBytes, entityBytes);
                keyList.Add(keyBytes);
                valueList.Add(values);
            }
            return ProcessRedisSyncQueue(string.Empty, keyList.ToArray(), valueList.ToArray());

        }

        private static void DoProcessRedisSyncQueue(string sysnWorkingQueueKey, byte[][] keys, byte[][] values)
        {
            ProcessRedisSyncQueue(sysnWorkingQueueKey, keys, values);
        }

        /// <summary>
        /// Process synchronized queue of redis
        /// </summary>
        /// <param name="sysnWorkingQueueKey"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        private static bool ProcessRedisSyncQueue(string sysnWorkingQueueKey, byte[][] keys, byte[][] values)
        {
            bool result = false;
            try
            {
                var redisSyncErrorQueue = new List<byte[][]>();
                var entityList = new List<RedisEntityItem>();
                var entityRemList = new List<RedisEntityItem>();
                var mutilKeyMapList = new List<RedisEntityItem>();
                var mutilKeyMapRemList = new List<RedisEntityItem>();
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
                        bool hasMutilKey = false;
                        bool isStoreInDb = true;
                        SchemaTable schema;
                        if (EntitySchemaSet.TryGet(entityTypeName, out schema))
                        {
                            hasMutilKey = RedisConnectionPool.CurrRedisInfo.ClientVersion >= RedisStorageVersion.HashMutilKeyMap &&
                                schema.EntityType.IsSubclassOf(typeof(BaseEntity)) &&
                                schema.Keys.Length > 1;
                            isStoreInDb = schema.StorageType.HasFlag(StorageType.WriteOnlyDB) || schema.StorageType.HasFlag(StorageType.ReadWriteDB);
                        }

                        byte[] headBytes;
                        byte[] entityValBytes;
                        int state;
                        int identity;
                        DecodeValueBytes(valueBytes, out headBytes, out entityValBytes, out state, out identity);

                        var entityItem = new RedisEntityItem()
                        {
                            HashId = entityParentKey,
                            UserId = identity,
                            KeyBytes = entityKeyBytes,
                            ValueBytes = entityValBytes,
                            State = state,
                            HasMutilKey = hasMutilKey
                        };
                        if (entityItem.State == 1)
                        {
                            entityRemList.Add(entityItem);
                            if (hasMutilKey) mutilKeyMapRemList.Add(entityItem);
                        }
                        else
                        {
                            entityList.Add(entityItem);
                            if (hasMutilKey) mutilKeyMapList.Add(entityItem);
                        }
                        if (_enableWriteToDb && isStoreInDb)
                        {
                            //增加到Sql等待队列
                            string sqlWaitQueueKey = GetSqlWaitSyncQueueKey(identity);
                            sqlWaitSyncQueue.Add(new KeyValuePair<string, byte[][]>(sqlWaitQueueKey, new[] { keyBytes, headBytes }));
                        }
                    }
                    catch (Exception error)
                    {
                        TraceLog.WriteError("RedisSync key:{0} error:{1}", RedisConnectionPool.ToStringKey(keyBytes), error);
                        redisSyncErrorQueue.Add(new[] { keyBytes, valueBytes });
                    }
                }
                var redisErrorKeys = redisSyncErrorQueue.Select(p => p[0]).ToArray();
                var redisErrorValues = redisSyncErrorQueue.Select(p => p[1]).ToArray();
                var sqlWaitGroups = sqlWaitSyncQueue.GroupBy(p => p.Key);
                var setGroups = entityList.GroupBy(p => p.HashId);
                var removeGroups = entityRemList.GroupBy(p => p.HashId);
                var mutilKeyMapGroups = mutilKeyMapList.GroupBy(p => p.HashId);
                var mutilKeyMapRemGroups = mutilKeyMapRemList.GroupBy(p => p.HashId);

                RedisConnectionPool.ProcessPipeline(pipeline =>
                {
                    bool hasPost = false;

                    foreach (var g in setGroups)
                    {
                        string typeName = RedisConnectionPool.DecodeTypeName(g.Key);
                        var keyCodes = new List<string>();
                        var entityKeys = g.Select(p =>
                        {
                            keyCodes.Add(RedisConnectionPool.ToStringKey(p.KeyBytes));
                            return p.KeyBytes;
                        }).ToArray();
                        var entityValues = g.Select(p => p.ValueBytes).ToArray();
                        pipeline.QueueCommand(client => ((RedisClient)client).HMSet(g.Key, entityKeys, entityValues), () =>
                        {//onsuccess
                            ProfileManager.ProcessEntityOfMessageQueueTimes(typeName, keyCodes, OperateMode.Add | OperateMode.Modify);
                        });
                        hasPost = true;
                    }
                    foreach (var g in removeGroups)
                    {
                        string typeName = RedisConnectionPool.DecodeTypeName(g.Key);
                        var keyCodes = new List<string>();
                        var keybytes = g.Select(p =>
                        {
                            keyCodes.Add(RedisConnectionPool.ToStringKey(p.KeyBytes));
                            return p.KeyBytes;
                        }).ToArray();
                        pipeline.QueueCommand(client => ((RedisClient)client).HDel(g.Key, keybytes), () =>
                        {//onsuccess
                            ProfileManager.ProcessEntityOfMessageQueueTimes(typeName, keyCodes, OperateMode.Remove);
                        });
                        hasPost = true;
                    }
                    foreach (var g in mutilKeyMapGroups)
                    {
                        //create mutil-key index from storage.
                        string hashId = g.Key;
                        var subGroup = g.GroupBy(t => t.UserId);
                        foreach (var @group in subGroup)
                        {
                            string firstKey = AbstractEntity.EncodeKeyCode(@group.Key.ToString());
                            var keybytes = @group.Select(p => p.KeyBytes).ToArray();
                            pipeline.QueueCommand(client => RedisConnectionPool.SetMutilKeyMap((RedisClient)client, hashId, firstKey, keybytes));
                            hasPost = true;
                        }
                    }
                    foreach (var g in mutilKeyMapRemGroups)
                    {
                        //delete mutil-key index from storage.
                        string hashId = g.Key;
                        var subGroup = g.GroupBy(t => t.UserId);
                        foreach (var @group in subGroup)
                        {
                            string firstKey = AbstractEntity.EncodeKeyCode(@group.Key.ToString());
                            var keybytes = @group.Select(p => p.KeyBytes).ToArray();
                            pipeline.QueueCommand(client => RedisConnectionPool.RemoveMutilKeyMap((RedisClient)client, hashId, firstKey, keybytes));
                            hasPost = true;
                        }
                    }

                    if (redisErrorKeys.Length > 0)
                    {
                        pipeline.QueueCommand(client => ((RedisClient)client).HMSet(RedisSyncErrorQueueKey, redisErrorKeys, redisErrorValues));
                        hasPost = true;
                    }
                    foreach (var g in sqlWaitGroups)
                    {
                        var sqlWaitKeys = g.Select(p => p.Value[0]).ToArray();
                        var sqlWaitValues = g.Select(p => p.Value[1]).ToArray();
                        int count = sqlWaitKeys.Length;
                        pipeline.QueueCommand(client => ((RedisClient)client).HMSet(g.Key, sqlWaitKeys, sqlWaitValues), () =>
                        {//onsuccess
                            ProfileManager.WaitSyncSqlOfMessageQueueTimes(null, count);
                        });
                        hasPost = true;
                    }
                    if (hasPost)
                    {
                        pipeline.Flush();
                    }
                    result = redisErrorKeys.Length == 0;
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
            return result;
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
                var sqlSender = new SqlDataSender(false);
                IEnumerable<KeyValuePair<string, KeyValuePair<byte[], long>>> sqlList = null;
                RedisConnectionPool.ProcessPipeline(client =>
                {
                    sqlList = GenerateSqlFrom(sqlSender, client, keys, values);
                }, p =>
                {
                    if (sqlList == null) return 0;
                    var keyValuePairs = sqlList.ToList();
                    var groupSqlList = keyValuePairs.GroupBy(t => t.Key);
                    int sqlCount = keyValuePairs.Count();
                    bool hasPost = false;
                    long result = 0;
                    foreach (var g in groupSqlList)
                    {
                        var pairs = g.Select(t => t.Value).ToList();
                        p.QueueCommand(c => ((RedisClient)c).ZAdd(g.Key, pairs), r =>
                        {//onsuccess, 已经存在的返回值为0
                            result += r;
                            ProfileManager.PostSqlOfMessageQueueTimes(g.Key, sqlCount);
                        });
                        hasPost = true;
                    }
                    if (hasPost)
                    {
                        p.Flush();
                    }
                    return result;
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
            string asmName = "";
            var enitityAsm = EntitySchemaSet.EntityAssembly;
            if (enitityAsm != null)
            {
                asmName = "," + enitityAsm.GetName().Name;
            }
            var entityKeyList = new List<string>();
            var typeKeyValuePairs = new List<KeyValuePair<string, byte[][]>>();
            for (int i = 0; i < keys.Length; i++)
            {
                byte[] keyBytes = keys[i];
                byte[] headBytes = values[i];
                string entityTypeKey = RedisConnectionPool.ToStringKey(keyBytes);
                entityKeyList.Add(entityTypeKey);
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
                    string entityTypeName = RedisConnectionPool.DecodeTypeName(typeName);
                    Type type = Type.GetType(string.Format("{0}{1}", entityTypeName, asmName), false, true);
                    if (type == null)
                    {
                        //调试模式下type为空处理
                        type = enitityAsm != null ? enitityAsm.GetType(entityTypeName, false, true) : null;
                        if (type == null)
                        {
                            throw new ArgumentTypeException(string.Format("Get entity \"{0}\" type is null", entityTypeName));
                        }
                    }
                    var keyBuffers = g.Select(p => p.Value[0]).ToArray();
                    var headBuffers = g.Select(p => p.Value[2]).ToArray();
                    var valueBuffers = client.HMGet(entityParentKey, keyBuffers);

                    for (int i = 0; i < keyBuffers.Length; i++)
                    {
                        string keyCode = RedisConnectionPool.ToStringKey(keyBuffers[i]);
                        var buffer = valueBuffers != null && i < valueBuffers.Length ? valueBuffers[i] : null;
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
                        else
                        {

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
                            throw new Exception(string.Format("Get \"{0}\" entity is null, keycode:{1},buffer len:{2}, isRemove:{3}",
                                typeName, keyCode, buffer == null ? 0 : buffer.Length, state == 1));
                        }
                    }
                }
                catch (Exception er)
                {
                    TraceLog.WriteError("FindEntityFromRedis {0} keys:{1}\r\nError:{2}", typeName, entityKeyList.Count, er);
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
