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
using ZyGames.Framework.Cache.Generic.Pool;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
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
        private static event UpdateEvent UpdateCallbackHandle;
        private static bool _isRunning;

        private static bool enableUpdateListent = false;
        private static int _isDisposed;
        private static string entityTypeNameFormat = "System.Collections.Generic.Dictionary`2[[System.String],[{0},{1}]]";


        /// <summary>
        /// Initialize cache.
        /// </summary>
        public static void Initialize(CacheSetting setting)
        {
            Initialize(new DbTransponder(), new RedisTransponder(), setting);
        }

        /// <summary>
        /// Initialize cache.
        /// </summary>
        /// <param name="dbTransponder">db trans object</param>
        /// <param name="redisTransponder">redis trans object</param>
        /// <param name="setting">setting.</param>
        public static void Initialize(ITransponder dbTransponder, ITransponder redisTransponder, CacheSetting setting)
        {
            _readonlyPools = new CachePool(dbTransponder, redisTransponder, true);
            _writePools = new CachePool(dbTransponder, redisTransponder, false) { Setting = setting };

            EntitySchemaSet.InitSchema(typeof(EntityHistory));
            DataSyncQueueManager.Start(setting);
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
            bool result = false;
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                result = client.SearchKeys(DataSyncQueueManager.RedisSyncQueueKey + "*").Count == 0;
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
            MemoryCacheStruct<MemoryEntity>.Reset();
        }

        /// <summary>
        /// 通过Redis键获取实体对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isRemove"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static dynamic GetEntityFromRedis(string key, bool isRemove, Type type = null)
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
                        entity = ProtoBufUtils.Deserialize(data, type);
                    }
                }
                else
                {
                    var data = client.Get<byte[]>(redisKey);
                    if (data != null && type != null)
                    {
                        var dict = (IDictionary)ProtoBufUtils.Deserialize(data, type);
                        entity = dict[entityKey];
                    }
                }
                if (isRemove && entity == null && type != null)
                {
                    string setId = (isEntityType ? typeName : redisKey) + ":remove";
                    var data = client.Get<byte[]>(setId);
                    if (data != null)
                    {
                        var dict = (IDictionary)ProtoBufUtils.Deserialize(data, type);
                        entity = dict[entityKey];
                        dict.Remove(entityKey);
                        if (dict.Count > 0)
                        {
                            client.Set(setId, ProtoBufUtils.Serialize(dict));
                        }
                        else
                        {
                            client.Remove(setId);
                        }
                    }
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
                redisKey = string.Format("{0}_{1}", typeName, entityKey);
            }
            else
            {
                //私有类型
                redisKey = string.Format("{0}_{1}", typeName, persionKey);
            }
            string formatString = entityTypeNameFormat;
            if (isEntityType)
            {
                formatString = "{0},{1}";
            }
            if (type == null)
            {
                type = Type.GetType(string.Format(formatString, typeName, asmName), false, true);
                if (Equals(type, null))
                {
                    var enitityAsm = ScriptEngines.GetEntityAssembly();
                    if (enitityAsm != null)
                    {
                        asmName = enitityAsm.GetName().Name;
                        type = Type.GetType(string.Format(formatString, typeName, asmName), false, true);
                        if (Equals(type, null))
                        {
                            //调试模式下type为空处理
                            type = enitityAsm.GetType( typeName, false, true);
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
            string key = personalId;
            if (keys.Length > 0)
            {
                key += "|" + AbstractEntity.CreateKeyCode(keys);
            }
            string redisKey = string.Format("{0}_{1}", entityType, key);
            CacheItemSet itemSet;
            return GetPersonalEntity(redisKey, out itemSet);
        }


        /// <summary>
        /// 通过Redis键获取实体对象
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static dynamic GetPersonalEntity(string redisKey)
        {
            CacheItemSet itemSet;
            return GetPersonalEntity(redisKey, out itemSet);
        }

        /// <summary>
        /// 通过Redis键获取实体对象
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="itemSet"></param>
        /// <returns></returns>
        public static dynamic GetPersonalEntity(string redisKey, out CacheItemSet itemSet)
        {
            itemSet = null;
            dynamic entity = null;
            string[] keys = (redisKey ?? "").Split('_');
            if (keys.Length == 2 && !string.IsNullOrEmpty(keys[0]))
            {
                CacheContainer container;
                if (_writePools != null && _writePools.TryGetValue(keys[0], out  container))
                {
                    string[] childKeys = keys[1].Split('|');
                    string personalKey = childKeys[0];
                    if (!string.IsNullOrEmpty(personalKey) &&
                        container.Collection.TryGetValue(personalKey, out itemSet))
                    {
                        switch (itemSet.ItemType)
                        {
                            case CacheType.Entity:
                                entity = itemSet.ItemData;
                                break;
                            case CacheType.Dictionary:
                                var set = itemSet.ItemData as BaseCollection;
                                if (set != null)
                                {
                                    set.TryGetValue(childKeys[1], out entity);
                                }
                                break;
                            default:
                                TraceLog.WriteError("Not suported CacheType:{0} for GetPersonalEntity key:{1}", itemSet.ItemType, redisKey);
                                break;
                        }
                    }
                }
            }
            if (entity == null)
            {
                //while is remove entity is empty.
                //TraceLog.WriteComplement("GetPersonalEntity key:{0} is empty.", redisKey);
            }
            return entity;
        }


        /// <summary>
        /// 从Redis内存移除，并保存到数据库
        /// </summary>
        /// <param name="keys"></param>
        public static void RemoveToDatabase(params string[] keys)
        {
            var entityKeys = new List<string>();
            var entityList = new List<EntityHistory>();
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                foreach (var k in keys)
                {
                    try
                    {
                        string key = k;
                        string setId = key + "_remove";
                        if (key.EndsWith("_remove"))
                        {
                            setId = key;
                            key = key.Replace("_remove", "");
                        }
                        else
                        {
                            if (client.ContainsKey(key))
                            {
                                client.Rename(key, setId);
                            }
                        }
                        entityKeys.Add(setId);
                        byte[] keyValues = ProtoBufUtils.Serialize(client.HGetAll(setId));
                        var history = new EntityHistory() { Key = key, Value = keyValues };
                        entityList.Add(history);
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Redis cache remove key:{0} to Database error:{1}", k, ex);
                    }
                }
            });

            if (entityList.Count > 0)
            {
                DataSyncManager.GetDataSender().Send<EntityHistory>(entityList);
                RedisConnectionPool.ProcessReadOnly(client => client.RemoveAll(entityKeys));
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
                            TraceLog.ReleaseWrite("缓存延迟更新执行开始");
                            UpdateNotify(true);
                            TraceLog.ReleaseWrite("缓存延迟更新执行结束");
                            _isRunning = false;
                        }
                        else
                        {
                            TraceLog.ReleaseWrite("缓存延迟更新正在执行中...");
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
                            TraceLog.ReleaseWrite("清理过期缓存结束...");
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("Cache manager timing error:{0}", ex);
                        }
                    }
                });
            }
        }

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
            TraceLog.WriteInfo("CacheFactory listen has started...");
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
            TraceLog.WriteInfo("CacheFactory listen has stoped");
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
        public static IDataContainer<T> GetOrCreate<T>() where T : IItemChangeEvent, IDataExpired, new()
        {
            return GetOrCreate<T>(false, () => true, (key) => true);
        }

        /// <summary>
        /// 获取缓存容器对象，不存在则创建空容器对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isReadonly">内存是否是只读</param>
        /// <param name="loadFactory"></param>
        /// <param name="loadItemFactory"></param>
        /// <returns></returns>
        public static IDataContainer<T> GetOrCreate<T>(bool isReadonly, Func<bool> loadFactory, Func<string, bool> loadItemFactory) where T : IItemChangeEvent, IDataExpired, new()
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