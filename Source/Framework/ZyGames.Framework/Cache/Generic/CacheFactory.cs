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
using System.Collections.Generic;
using System.Linq;
using ZyGames.Framework.Cache.Generic.Pool;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Profile;
using ZyGames.Framework.Redis;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存池管理者
    /// </summary>
    public static class CacheFactory
    {
        private delegate bool UpdateEvent(bool isChange);
        private static CacheListener _cacheUpdateListener;
        private static CacheListener _cacheExpiredListener;
        private static BaseCachePool _readonlyPools;
        private static BaseCachePool _writePools;
        private static BaseCachePool _memoryPools = new CachePool(null, null, false, new ProtobufCacheSerializer());
        private static event UpdateEvent UpdateCallbackHandle;
        private static bool _isRunning;

        private static bool enableUpdateListent = false;
        private static int _isDisposed;
        private static string entityTypeNameFormat = "System.Collections.Generic.Dictionary`2[[System.String],[{0},{1}]]";

        internal static BaseCachePool MemoryCache
        {
            get { return _memoryPools; }
        }
        /// <summary>
        /// Initialize cache.
        /// </summary>
        public static void Initialize(CacheSetting setting, ICacheSerializer serializer)
        {
            Initialize(new DbTransponder(), new RedisTransponder(), setting, serializer);
        }

        /// <summary>
        /// Initialize cache.
        /// </summary>
        /// <param name="dbTransponder">db trans object</param>
        /// <param name="redisTransponder">redis trans object</param>
        /// <param name="setting">setting.</param>
        /// <param name="serializer"></param>
        public static void Initialize(ITransponder dbTransponder, ITransponder redisTransponder, CacheSetting setting, ICacheSerializer serializer)
        {
            _readonlyPools = new CachePool(dbTransponder, redisTransponder, true, serializer);
            _writePools = new CachePool(dbTransponder, redisTransponder, false, serializer) { Setting = setting };

            EntitySchemaSet.InitSchema(typeof(EntityHistory));
            DataSyncQueueManager.Start(setting, serializer);
            ProfileManager.Start();
            InitListener("__CachePoolListener", setting.ExpiredInterval, "__CachePoolUpdateListener", setting.UpdateInterval);
            if (setting.AutoRunEvent)
            {
                StartListener();
            }
        }

        /// <summary>
        /// Check queue is completed.
        /// </summary>
        /// <param name="keyIndex"></param>
        /// <returns></returns>
        public static bool CheckCompleted(int keyIndex = 0)
        {
            var keys = new string[DataSyncQueueManager.SyncQueueCount];
            if (keys.Length == 1) keys[0] = (DataSyncQueueManager.RedisSyncQueueKey);
            else
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    keys[i] = string.Format("{0}:{1}", DataSyncQueueManager.RedisSyncQueueKey, i);
                }
            }

            bool result = false;
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                var values = client.MGet(keys);
                result = values == null || values.Length == 0 || values.Any(t => t == null);
            });
            return result;
        }
        /// <summary>
        /// Reset cache.
        /// </summary>
        public static void ResetCache()
        {
            _readonlyPools.Init();
            _writePools.Init();
            _memoryPools.Init();
        }

        /// <summary>
        /// 通过Redis键获取实体对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isRemove"></param>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static dynamic GetEntityFromRedis(string key, bool isRemove, Type type, ICacheSerializer serializer)
        {
            string typeName;
            string asmName;
            bool isEntityType;
            string redisKey;
            string entityKey = GetEntityTypeFromKey(key, out typeName, ref type, out asmName, out isEntityType, out redisKey);
            dynamic entity = null;
            RedisConnectionPool.Process(client =>
            {
                if (isEntityType)
                {
                    var data = client.Get<byte[]>(redisKey);
                    if (data != null && type != null)
                    {
                        entity = serializer.Deserialize(data, type);
                    }
                }
                else
                {
                    var data = client.Get<byte[]>(redisKey);
                    if (data != null && type != null)
                    {
                        var dict = (IDictionary)serializer.Deserialize(data, type);
                        entity = dict[entityKey];
                    }
                }
                if (entity == null)
                {
                    //新版本Hash格式
                    var data = client.HGet(typeName, RedisConnectionPool.ToByteKey(entityKey));
                    if (data != null && type != null)
                    {
                        entity = serializer.Deserialize(data, type);
                    }
                }

                if (isRemove && entity == null && type != null)
                {
                    //临时队列删除Entity
                    string setId = (isEntityType ? RedisConnectionPool.EncodeTypeName(typeName) : redisKey) + ":remove";
                    IDictionary dict = null;
                    RedisConnectionPool.ProcessTrans(client, new string[] { setId }, () =>
                    {
                        var data = client.Get<byte[]>(setId);
                        if (data == null)
                        {
                            return false;
                        }
                        dict = (IDictionary)serializer.Deserialize(data, type);
                        entity = dict[entityKey];
                        dict.Remove(entityKey);

                        return true;
                    }, trans =>
                    {
                        if (dict != null && dict.Count > 0)
                        {
                            trans.QueueCommand(c => c.Set(setId, serializer.Serialize(dict)));
                        }
                        else
                        {
                            trans.QueueCommand(c => c.Remove(setId));
                        }
                    }, null);

                }

            });
            return entity;
        }

        private static string GetEntityTypeFromKey(string key, out string typeName, ref Type type, out string asmName, out bool isEntityType, out string redisKey)
        {
            int index = key.IndexOf(',');
            var arr = (index > -1 ? key.Substring(0, index) : key).Split('_');
            typeName = arr[0];
            asmName = index == -1 ? "" : key.Substring(index + 1, key.Length - index - 1);
            string persionKey = string.Empty;
            string entityKey = string.Empty;
            if (arr.Length > 1)
            {
                entityKey = arr[1];
                var tempArr = entityKey.Split('|');
                if (tempArr.Length > 1)
                {
                    persionKey = tempArr[0];
                    entityKey = tempArr[1];
                }
            }
            isEntityType = false;
            if (string.IsNullOrEmpty(persionKey))
            {
                isEntityType = true;
                redisKey = string.Format("{0}_{1}", RedisConnectionPool.EncodeTypeName(typeName), entityKey);
            }
            else
            {
                //私有类型
                redisKey = string.Format("{0}_{1}", RedisConnectionPool.EncodeTypeName(typeName), persionKey);
            }
            string formatString = entityTypeNameFormat;
            if (isEntityType)
            {
                formatString = "{0},{1}";
            }
            if (type == null)
            {
                string entityTypeName = RedisConnectionPool.DecodeTypeName(typeName);
                type = Type.GetType(string.Format(formatString, entityTypeName, asmName), false, true);
                if (Equals(type, null))
                {
                    var enitityAsm = ScriptEngines.GetEntityAssembly();
                    if (enitityAsm != null)
                    {
                        asmName = enitityAsm.GetName().Name;
                        type = Type.GetType(string.Format(formatString, entityTypeName, asmName), false, true);
                        if (Equals(type, null))
                        {
                            //调试模式下type为空处理
                            type = enitityAsm.GetType(entityTypeName, false, true);
                        }
                    }
                }
            }
            return entityKey;
        }

        /// <summary>
        /// Get entity of personal object
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="personalId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static dynamic GetPersonalEntity(string entityType, string personalId, params object[] keys)
        {
            string key = AbstractEntity.EncodeKeyCode(personalId);
            if (keys.Length > 0)
            {
                key += "|" + AbstractEntity.CreateKeyCode(keys);
            }
            string redisKey = string.Format("{0}_{1}", RedisConnectionPool.EncodeTypeName(entityType), key);
            CacheItemSet itemSet;
            return GetPersonalEntity(redisKey, out itemSet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainEntityKey(Type type, string key)
        {
            CacheContainer container;
            key = AbstractEntity.EncodeKeyCode(key);
            if (_writePools != null && _writePools.TryGetValue(type.FullName, out container))
            {
                CacheItemSet itemSet;
                return container.Collection.TryGetValue(key, out itemSet) &&
                    itemSet.LoadingStatus == LoadingStatus.Success &&
                    !itemSet.IsEmpty;
            }
            return false;
        }

        /// <summary>
        /// 通过Redis键从缓存中获取实体对象
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static dynamic GetPersonalEntity(string redisKey)
        {
            CacheItemSet itemSet;
            return GetPersonalEntity(redisKey, out itemSet);
        }

        /// <summary>
        /// 通过Redis键从缓存中获取实体对象
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="itemSet"></param>
        /// <returns></returns>
        public static dynamic GetPersonalEntity(string redisKey, out CacheItemSet itemSet)
        {
            itemSet = null;
            dynamic entity = null;
            KeyValuePair<string, CacheItemSet> itemPair;
            if (TryGetCacheItem(redisKey, out itemPair))
            {
                itemSet = itemPair.Value;
                switch (itemPair.Value.ItemType)
                {
                    case CacheType.Entity:
                        entity = itemPair.Value.ItemData;
                        break;
                    case CacheType.Dictionary:
                        var set = itemPair.Value.ItemData as BaseCollection;
                        if (set != null)
                        {
                            set.TryGetValue(itemPair.Key, out entity);
                        }
                        break;
                    default:
                        TraceLog.WriteError("Not suported CacheType:{0} for GetPersonalEntity key:{1}", itemPair.Value.ItemType, redisKey);
                        break;
                }
            }
            if (entity == null)
            {
                //while is remove entity is empty.
                //TraceLog.WriteComplement("GetPersonalEntity key:{0} is empty.", redisKey);
            }
            return entity;
        }

        internal static string GenerateEntityKey(AbstractEntity entity)
        {
            if (entity == null) return string.Empty;

            return string.Format("{0}_{1}|{2}",
                RedisConnectionPool.EncodeTypeName(entity.GetType().FullName),
                entity.PersonalId,
                entity.GetKeyCode());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="itemSet"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public static bool AddOrUpdateEntity(AbstractEntity entity, out CacheItemSet itemSet, int periodTime = 0)
        {
            return AddOrUpdateEntity(GenerateEntityKey(entity), entity, out itemSet, periodTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="entity"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public static bool AddOrUpdateEntity(string redisKey, AbstractEntity entity, int periodTime = 0)
        {
            CacheItemSet itemSet;
            return AddOrUpdateEntity(redisKey, entity, out itemSet, periodTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="entity"></param>
        /// <param name="itemSet"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public static bool AddOrUpdateEntity(string redisKey, AbstractEntity entity, out CacheItemSet itemSet, int periodTime = 0)
        {
            itemSet = null;
            KeyValuePair<string, CacheItemSet> itemPair;
            if (TryGetCacheItem(redisKey, out itemPair, periodTime))
            {
                itemSet = itemPair.Value;
                switch (itemPair.Value.ItemType)
                {
                    case CacheType.Entity:
                        itemPair.Value.SetItem(entity);
                        entity.IsInCache = true;
                        return true;
                    case CacheType.Dictionary:
                        var set = itemPair.Value.ItemData as BaseCollection;
                        if (set != null)
                        {
                            if (set.AddOrUpdate(itemPair.Key, entity, (k, t) => entity) == entity)
                            {
                                entity.IsInCache = true;
                                return true;
                            }
                            return false;
                        }
                        break;
                    default:
                        TraceLog.WriteError("Not suported CacheType:{0} for GetPersonalEntity key:{1}", itemPair.Value.ItemType, redisKey);
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="itemPair">key:entity's key, value:</param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public static bool TryGetCacheItem(string redisKey, out KeyValuePair<string, CacheItemSet> itemPair, int periodTime = 0)
        {
            itemPair = default(KeyValuePair<string, CacheItemSet>);
            CacheItemSet cacheItem;
            string[] keys = (redisKey ?? "").Split('_');
            if (keys.Length == 2 && !string.IsNullOrEmpty(keys[0]))
            {
                CacheContainer container = null;
                string typeName = RedisConnectionPool.DecodeTypeName(keys[0]);
                var schema = EntitySchemaSet.Get(typeName);
                periodTime = periodTime > 0 ? periodTime : schema.PeriodTime;
                if (_writePools != null && !_writePools.TryGetValue(typeName, out container))
                {
                    _writePools.InitContainer(typeName);
                    _writePools.TryGetValue(typeName, out container);
                }
                if (container == null) return false;

                string[] childKeys = keys[1].Split('|');
                string personalKey = childKeys[0];
                string entityKey = childKeys.Length > 1 ? childKeys[1] : "";
                if (schema.CacheType == CacheType.Dictionary)
                {
                    var lazy = new Lazy<CacheItemSet>(() =>
                    {
                        bool isReadonly = schema.AccessLevel == AccessLevel.ReadOnly;
                        BaseCollection itemCollection = isReadonly
                            ? (BaseCollection)new ReadonlyCacheCollection()
                            : new CacheCollection(schema.IsMutilKey ? 0 : 1);
                        var itemSet = new CacheItemSet(schema.CacheType, periodTime, isReadonly);
                        if (!isReadonly && _writePools.Setting != null)
                        {
                            itemSet.OnChangedNotify += _writePools.Setting.OnChangedNotify;
                        }
                        itemSet.HasCollection = true;
                        itemSet.SetItem(itemCollection);
                        return itemSet;
                    });
                    cacheItem = container.Collection.GetOrAdd(personalKey, key => lazy.Value);
                    itemPair = new KeyValuePair<string, CacheItemSet>(entityKey, cacheItem);
                    return true;
                }
                if (schema.CacheType == CacheType.Entity)
                {
                    var lazy = new Lazy<CacheItemSet>(() =>
                    {
                        bool isReadonly = schema.AccessLevel == AccessLevel.ReadOnly;
                        var itemSet = new CacheItemSet(schema.CacheType, periodTime, isReadonly);
                        if (!isReadonly && _writePools.Setting != null)
                        {
                            itemSet.OnChangedNotify += _writePools.Setting.OnChangedNotify;
                        }
                        return itemSet;
                    });
                    cacheItem = container.Collection.GetOrAdd(entityKey, key => lazy.Value);

                    itemPair = new KeyValuePair<string, CacheItemSet>(entityKey, cacheItem);
                    return true;
                }
                if (schema.CacheType == CacheType.Queue)
                {
                    TraceLog.WriteError("Not support CacheType.Queue get cache, key:{0}.", redisKey);
                }

                ////存在分类id与实体主键相同情况, 要优先判断实体主键
                //if (!string.IsNullOrEmpty(personalKey) && container.Collection.TryGetValue(entityKey, out cacheItem))
                //{
                //    itemPair = new KeyValuePair<string, CacheItemSet>(entityKey, cacheItem);
                //    return true;
                //}
                //if (!string.IsNullOrEmpty(personalKey) && container.Collection.TryGetValue(personalKey, out cacheItem))
                //{
                //    itemPair = new KeyValuePair<string, CacheItemSet>(entityKey, cacheItem);
                //    return true;
                //}

            }
            return false;
        }

        /// <summary>
        /// 从Redis内存移除，并保存到数据库,
        /// </summary>
        /// <param name="match">实体类型, 实体Key列表</param>
        public static void RemoveToDatabase(params KeyValuePair<Type, IList<string>>[] match)
        {
            var removeEntityKeys = new List<KeyValuePair<string, byte[][]>>();
            var entityList = new List<EntityHistory>();
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                foreach (var express in match)
                {
                    try
                    {
                        string hashtId = RedisConnectionPool.GetRedisEntityKeyName(express.Key);
                        byte[][] keyBytes = express.Value.Select(t => RedisConnectionPool.ToByteKey(t)).ToArray();
                        if (keyBytes.Length == 0) continue;
                        removeEntityKeys.Add(new KeyValuePair<string, byte[][]>(hashtId, keyBytes));
                        //转存到DB使用protobuf
                        byte[][] valueBytes = client.HMGet(hashtId, keyBytes);
                        for (int i = 0; i < keyBytes.Length; i++)
                        {
                            entityList.Add(new EntityHistory()
                            {
                                Key = string.Format("{0}_{1}", hashtId, RedisConnectionPool.ToStringKey(keyBytes[i])),
                                Value = valueBytes[i]
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Redis cache remove key:{0} to Database error:{1}", express, ex);
                    }
                }
            });

            if (entityList.Count > 0)
            {
                DataSyncManager.SendSql<EntityHistory>(entityList, false, true);
                RedisConnectionPool.ProcessReadOnly(client =>
                {
                    foreach (var pair in removeEntityKeys)
                    {
                        client.HDel(pair.Key, pair.Value);
                    }
                });
            }
        }

        private static void InitListener(string listenerKey, int expiredInterval, string updateListentKey, int updateInterval)
        {
            if (enableUpdateListent && _cacheUpdateListener == null)
            {
                _cacheUpdateListener = new CacheListener(updateListentKey, updateInterval, (key, value, reason) =>
                {
                    try
                    {

                        if (!_isRunning)
                        {
                            _isRunning = true;
                            TraceLog.WriteLine("{0} Cache sync to storage start...", DateTime.Now.ToString("HH:mm:ss"));
                            UpdateNotify(true);
                            TraceLog.WriteLine("{0} Cache sync to storage end.", DateTime.Now.ToString("HH:mm:ss"));
                            _isRunning = false;
                        }
                        else
                        {
                            TraceLog.WriteLine("{0} Cache sync to storage doing...", DateTime.Now.ToString("HH:mm:ss"));
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Cache manager timing error:{0}", ex);
                    }
                });
            }
            if (_cacheExpiredListener == null)
            {
                _cacheExpiredListener = new CacheListener(listenerKey, expiredInterval, (key, value, reason) =>
                {
                    if (reason == CacheRemovedReason.Expired)
                    {
                        try
                        {
                            _readonlyPools.DisposeCache();
                            _writePools.DisposeCache();
                            TraceLog.WriteLine("{0} Clear expired cache end.", DateTime.Now.ToString("HH:mm:ss"));
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("Cache manager timing error:{0}", ex);
                        }
                    }
                });
            }
        }

        //todo test
        ///// <summary>
        ///// 
        ///// </summary>
        //public static void TestDisposeCache()
        //{
        //    try
        //    {
        //        _readonlyPools.DisposeCache();
        //        _writePools.DisposeCache();
        //        TraceLog.WriteLine("{0} Clear expired cache end.", DateTime.Now.ToString("HH:mm:ss"));
        //    }
        //    catch (Exception ex)
        //    {
        //        TraceLog.WriteError("Cache manager timing error:{0}", ex);
        //    }
        //}

        /// <summary>
        /// 启动缓存写库监听
        /// </summary>
        public static void StartListener()
        {
            if (_cacheExpiredListener != null)
            {
                _cacheExpiredListener.Start();
            }
            if (_cacheUpdateListener != null)
            {
                _cacheUpdateListener.Start();
            }
            System.Threading.Interlocked.Exchange(ref _isDisposed, 0);
            TraceLog.WriteLine("{0} CacheFactory listen has started...", DateTime.Now.ToString("HH:mm:ss"));
        }

        /// <summary>
        /// 停止缓存写库监听
        /// </summary>
        public static void StopListener()
        {
            if (_cacheExpiredListener != null)
            {
                _cacheExpiredListener.Stop();
                _cacheExpiredListener = null;
            }
            if (_cacheUpdateListener != null)
            {
                _cacheUpdateListener.Stop();
                _cacheUpdateListener = null;
            }
            //System.Threading.Interlocked.Exchange(ref _isDisposed, 1);
            TraceLog.WriteLine("CacheFactory listen has stoped");
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public static void Dispose()
        {
            StopListener();
            //if (_readonlyPools != null)
            //{
            //    _readonlyPools.Dispose();
            //    _readonlyPools = null;
            //}
            //if (_writePools != null)
            //{
            //    _writePools.Dispose();
            //    _writePools = null;
            //}
            UpdateCallbackHandle = null;
        }

        /// <summary>
        /// 获取缓存容器对象，不存在则创建空容器对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static EntityContainer<T> GetOrCreate<T>() where T : IItemChangeEvent, IDataExpired, new()
        {
            return GetOrCreate<T>(false, (r) => true, (r, key) => true);
        }

        /// <summary>
        /// 获取缓存容器对象，不存在则创建空容器对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isReadonly">内存是否是只读</param>
        /// <param name="loadFactory"></param>
        /// <param name="loadItemFactory"></param>
        /// <returns></returns>
        public static EntityContainer<T> GetOrCreate<T>(bool isReadonly, Func<bool, bool> loadFactory, Func<string, bool, bool> loadItemFactory) where T : IItemChangeEvent, IDataExpired, new()
        {
            if (_isDisposed == 1)
            {
                TraceLog.WriteError("Access to cache \"{0}\" data failed because the object has been disposed.", typeof(T).FullName);
            }

            EntityContainer<T> cacheSet = null;
            if (isReadonly)
            {
                cacheSet = new EntityContainer<T>(_readonlyPools, loadFactory, loadItemFactory) { IsReadonly = true };
            }
            else
            {
                cacheSet = new EntityContainer<T>(_writePools, loadFactory, loadItemFactory);
            }
            return cacheSet;
        }

        internal static void RegistUpdateNotify<T>(BaseCacheStruct<T> cacheStruct) where T : AbstractEntity, new()
        {
            UpdateCallbackHandle += isChange =>
            {
                cacheStruct.Update(isChange);
                return true;
            };
        }

        /// <summary>
        /// 注册全局共享缓存变更通知事件
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        [Obsolete]
        internal static void RegistShareUpdateNotify<T>() where T : ShareEntity, new()
        {
            UpdateCallbackHandle += isChange =>
            {
                new ShareCacheStruct<T>().Update(isChange);
                return true;
            };
        }

        /// <summary>
        /// 注册私有缓存变更通知事件（如单个玩家的数据）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        [Obsolete]
        internal static void RegistPersonalUpdateNotify<T>() where T : BaseEntity, new()
        {
            UpdateCallbackHandle += isChange =>
            {
                new PersonalCacheStruct<T>().Update(isChange);
                return true;
            };
        }

        /// <summary>
        /// 触发缓存数据更新
        /// </summary>
        /// <param name="isChange">更新的模式：是否仅更新有发生isChange的实例</param>
        public static void UpdateNotify(bool isChange)
        {

            if (UpdateCallbackHandle != null)
            {
                Delegate[] handleList = UpdateCallbackHandle.GetInvocationList();
                foreach (dynamic handle in handleList)
                {
                    if (handle != null)
                    {
                        handle(isChange);
                    }
                }
            }
        }

    }
}