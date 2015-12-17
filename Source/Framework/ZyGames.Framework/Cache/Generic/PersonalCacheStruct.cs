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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 私有的缓存模型
    /// </summary>
    public static class PersonalCacheStruct
    {
        /// <summary>
        /// Get entity from redis, but not surported mutil key of entity.
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static object[] Load(string personalId, params Type[] types)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            var param = types.Where(t => !CacheFactory.ContainEntityKey(t, personalId)).ToArray();
            if (param.Length == 0) return null;

            var result = RedisConnectionPool.GetAllEntity(personalId, param);
            foreach (var t in result)
            {
                var entity = t as AbstractEntity;
                if (entity == null) continue;
                CacheItemSet itemSet;
                if (CacheFactory.AddOrUpdateEntity(entity, out itemSet))
                {
                    itemSet.OnLoadSuccess();
                }
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        public static void ReLoad(string personalId, Predicate<SchemaTable> match)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            foreach (var schemaTable in EntitySchemaSet.GetEnumerable())
            {
                if (match(schemaTable))
                {
                    ReLoad(personalId, schemaTable);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="schemaTable"></param>
        public static void ReLoad(string personalId, SchemaTable schemaTable)
        {
            CallMethod(schemaTable, obj => obj.ReLoad(personalId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="match"></param>
        /// <param name="hasChanged"></param>
        public static void Update(string personalId, Predicate<SchemaTable> match, bool hasChanged = false)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            foreach (var schemaTable in EntitySchemaSet.GetEnumerable())
            {
                if (match(schemaTable))
                {
                    Update(personalId, schemaTable, hasChanged);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="schemaTable"></param>
        /// <param name="hasChanged"></param>
        public static void Update(string personalId, SchemaTable schemaTable, bool hasChanged = false)
        {
            CallMethod(schemaTable, obj => obj.Update(hasChanged, personalId));
        }

        private static void CallMethod(SchemaTable schemaTable, Action<dynamic> func)
        {
            Type cachType = typeof(PersonalCacheStruct<>);
            Type entityType = schemaTable.EntityType;
            string typeName = string.Format("{0}[[{1}, {2}]], {3}",
                cachType.FullName,
                entityType.FullName,
                entityType.Assembly.FullName,
                cachType.Assembly.FullName);
            Type type = Type.GetType(typeName, false, true);
            if (type == null) return;

            func((dynamic)type.CreateInstance());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="personalId"></param>
        /// <param name="isAutoLoad"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeOrLoad<T>(string personalId, bool isAutoLoad = true) where T : BaseEntity, new()
        {
            return new PersonalCacheStruct<T>().TakeOrLoad(personalId, isAutoLoad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="personalList"></param>
        /// <param name="isAutoLoad"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeOrLoad<T>(IEnumerable<string> personalList, bool isAutoLoad = true) where T : BaseEntity, new()
        {
            return new PersonalCacheStruct<T>().TakeOrLoad(personalList, isAutoLoad);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Take<T>() where T : BaseEntity, new()
        {
            return new PersonalCacheStruct<T>().Take();
        }

        /// <summary>
        /// Take entity by keys from cache
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static T Take<T>(params object[] keys) where T : BaseEntity, new()
        {
            return new PersonalCacheStruct<T>().Take(keys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static object GetCache(string entityName)
        {
            SchemaTable schema;
            if (EntitySchemaSet.TryGet(entityName, out schema))
            {
                Type cachType = typeof(PersonalCacheStruct<>);
                Type entityType = schema.EntityType;
                string typeName = string.Format("{0}[[{1}, {2}]], {3}",
                    cachType.FullName,
                    entityType.FullName,
                    entityType.Assembly.FullName,
                    cachType.Assembly.FullName);
                Type type = Type.GetType(typeName, false, true);
                if (type != null)
                {
                    return type.CreateInstance();
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="personalId"></param>
        /// <param name="createFactory"></param>
        /// <param name="keys"></param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static T GetOrAdd<T>(string personalId, Lazy<T> createFactory, params object[] keys) where T : BaseEntity, new()
        {
            var cache = new PersonalCacheStruct<T>();
            T result;
            LoadingStatus status;
            if ((status = cache.TryFindKey(personalId, out result, keys)) != LoadingStatus.Success)
            {
                throw new Exception(string.Format("Entity {0} load  personalId:{1} error:{2}", typeof(T).Name, personalId, status));
            }
            if (result == default(T))
            {
                result = createFactory.Value;
                cache.Add(result);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool AddRange(params BaseEntity[] enumerable)
        {
            return AddRange(true, enumerable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkMutilKey">The entity need to reload when it has be mutil key</param>
        /// <param name="enumerable"></param>
        public static bool AddRange(bool checkMutilKey, params BaseEntity[] enumerable)
        {
            if (DataSyncQueueManager.SendSync(enumerable))
            {
                foreach (var t in enumerable)
                {
                    var schema = t.GetSchema();
                    CacheItemSet itemSet;
                    if (CacheFactory.AddOrUpdateEntity(t, out itemSet))
                    {
                        if (checkMutilKey && schema.Keys.Length > 1 && !itemSet.HasLoadSuccess)
                        {
                            ReLoad(t.PersonalId, schema);
                        }
                        else
                        {
                            itemSet.OnLoadSuccess();
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="personalId"></param>
        /// <param name="allowNull"></param>
        /// <param name="keys"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns></returns>
        public static T Get<T>(string personalId, bool allowNull = false, params object[] keys) where T : BaseEntity, new()
        {
            var cache = new PersonalCacheStruct<T>();
            T result;
            LoadingStatus status;
            if ((status = cache.TryFindKey(personalId, out result, keys)) != LoadingStatus.Success)
            {
                throw new Exception(string.Format("Entity {0} load  personalId:{1} error:{2}", typeof(T).Name, personalId, status));
            }
            if (!allowNull && result == null)
            {
                throw new NullReferenceException(string.Format("Not found entity {0} personalId:{1}.", typeof(T).Name, personalId));
            }
            return result;
        }
        #region Get
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="personalId"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2>(string personalId, out T1 t1, out T2 t2, bool allowNull = false)
            where T1 : BaseEntity, new()
            where T2 : BaseEntity, new()
        {
            t1 = Get<T1>(personalId, allowNull);
            t2 = Get<T2>(personalId, allowNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="personalId"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2, T3>(string personalId, out T1 t1, out T2 t2, out T3 t3, bool allowNull = false)
            where T1 : BaseEntity, new()
            where T2 : BaseEntity, new()
            where T3 : BaseEntity, new()
        {
            t1 = Get<T1>(personalId, allowNull);
            t2 = Get<T2>(personalId, allowNull);
            t3 = Get<T3>(personalId, allowNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="personalId"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2, T3, T4>(string personalId, out T1 t1, out T2 t2, out T3 t3, out T4 t4, bool allowNull = false)
            where T1 : BaseEntity, new()
            where T2 : BaseEntity, new()
            where T3 : BaseEntity, new()
            where T4 : BaseEntity, new()
        {
            t1 = Get<T1>(personalId, allowNull);
            t2 = Get<T2>(personalId, allowNull);
            t3 = Get<T3>(personalId, allowNull);
            t4 = Get<T4>(personalId, allowNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="personalId"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2, T3, T4, T5>(string personalId, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, bool allowNull = false)
            where T1 : BaseEntity, new()
            where T2 : BaseEntity, new()
            where T3 : BaseEntity, new()
            where T4 : BaseEntity, new()
            where T5 : BaseEntity, new()
        {
            t1 = Get<T1>(personalId, allowNull);
            t2 = Get<T2>(personalId, allowNull);
            t3 = Get<T3>(personalId, allowNull);
            t4 = Get<T4>(personalId, allowNull);
            t5 = Get<T5>(personalId, allowNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="personalId"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        /// <param name="t6"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2, T3, T4, T5, T6>(string personalId, out T1 t1, out T2 t2, out T3 t3, out T4 t4,
            out T5 t5, out T6 t6, bool allowNull = false)
            where T1 : BaseEntity, new()
            where T2 : BaseEntity, new()
            where T3 : BaseEntity, new()
            where T4 : BaseEntity, new()
            where T5 : BaseEntity, new()
            where T6 : BaseEntity, new()
        {
            t1 = Get<T1>(personalId, allowNull);
            t2 = Get<T2>(personalId, allowNull);
            t3 = Get<T3>(personalId, allowNull);
            t4 = Get<T4>(personalId, allowNull);
            t5 = Get<T5>(personalId, allowNull);
            t6 = Get<T6>(personalId, allowNull);
        }

        #endregion
    }

    /// <summary>
    /// 私有的缓存模型
    /// </summary>
    public class PersonalCacheStruct<T> : BaseCacheStruct<T> where T : BaseEntity, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public PersonalCacheStruct()
        {
        }

        /// <summary>
        /// 数据项是否改变
        /// </summary>
        /// <param name="personalId"></param>
        /// <returns></returns>
        public bool HasChange(string personalId)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            return HasChangeCache(personalId);
        }

        /// <summary>
        /// Contain key of memory.
        /// </summary>
        /// <param name="personalId"></param>
        /// <returns></returns>
        public bool ContainKey(string personalId)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            return ContainGroupKey(personalId);
        }


        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool IsExistKey(string personalId, params object[] keys)
        {
            return FindKey(personalId, keys) == default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="data"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public LoadingStatus TryFindKey(string personalId, out T data, params object[] keys)
        {
            LoadingStatus loadStatus;
            BaseCollection collection;
            data = default(T);
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            if (TryGetGroup(personalId, out collection, out loadStatus))
            {
                string key = keys.Length > 0 ? AbstractEntity.CreateKeyCode(keys) : personalId;
                if (!collection.TryGetValue(key, out data))
                {
                    //修正旧版本无personalId参数调用
                    var tempKeys = new List<object>();
                    tempKeys.Add(personalId);
                    if (keys.Length > 0) tempKeys.AddRange(keys);
                    key = string.IsNullOrEmpty(key) ? personalId : AbstractEntity.CreateKeyCode(tempKeys.ToArray());
                    collection.TryGetValue(key, out data);
                }
            }
            return loadStatus;
        }

        /// <summary>
        /// 通过Key查找
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public T FindKey(string personalId, params object[] keys)
        {
            T data;
            TryFindKey(personalId, out data, keys);
            return data;
        }

        /// <summary>
        /// Take entity from cache
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Take()
        {
            return DataContainer.ToGroupEnumerable();
        }

        /// <summary>
        /// Take entity by keys from cache
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public T Take(params object[] keys)
        {
            string key = AbstractEntity.CreateKeyCode(keys);
            return DataContainer.TakeEntityFromKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="isAutoLoad"></param>
        /// <returns></returns>
        public IEnumerable<T> TakeOrLoad(string personalId, bool isAutoLoad = true)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            return DataContainer.TakeOrLoadGroup(personalId, isAutoLoad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalList"></param>
        /// <param name="isAutoLoad"></param>
        /// <returns></returns>
        public IEnumerable<T> TakeOrLoad(IEnumerable<string> personalList, bool isAutoLoad = true)
        {
            return DataContainer.TakeOrLoadGroup(personalList.Select(AbstractEntity.EncodeKeyCode), isAutoLoad);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public T Find(string personalId, Predicate<T> match)
        {
            T t;
            TryFind(personalId, match, out t);
            return t;
        }

        /// <summary>
        /// 查找第一个匹配数据
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="data"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public LoadingStatus TryFind(string personalId, Predicate<T> match, out T data)
        {
            LoadingStatus loadStatus;
            T t = default(T);
            BaseCollection collection;
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            if (TryGetGroup(personalId, out collection, out loadStatus))
            {
                collection.Foreach<T>((key, value) =>
                {
                    if (match == null || match(value))
                    {
                        t = value;
                        return false;
                    }
                    return true;
                });
            }
            data = t;
            return loadStatus;
        }

        /// <summary>
        /// 查找所有匹配数据
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="isSort"></param>
        /// <returns></returns>
        public List<T> FindAll(string personalId, bool isSort = true)
        {
            return FindAll(personalId, m => true, isSort);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="match"></param>
        /// <param name="isSort"></param>
        /// <returns></returns>
        public List<T> FindAll(string personalId, Predicate<T> match, bool isSort = true)
        {
            List<T> list;
            TryFindAll(personalId, match, isSort, out list);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="isSort"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public LoadingStatus TryFindAll(string personalId, bool isSort, out List<T> list)
        {
            return TryFindAll(personalId, m => true, isSort, out list);
        }

        /// <summary>
        /// 查找所有匹配数据
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="match"></param>
        /// <param name="isSort"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public LoadingStatus TryFindAll(string personalId, Predicate<T> match, bool isSort, out List<T> list)
        {
            LoadingStatus loadStatus;
            List<T> tempList = new List<T>();
            BaseCollection collection;
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            if (TryGetGroup(personalId, out collection, out loadStatus))
            {
                collection.Foreach<T>((key, value) =>
                {
                    if (match == null || match(value))
                    {
                        tempList.Add(value);
                    }
                    return true;
                });
            }
            list = tempList;
            if (isSort)
            {
                list.QuickSort();
            }
            return loadStatus;
        }

        /// <summary>
        /// 在整个缓存中查找,不加载数据
        /// </summary>
        /// <param name="match">查找匹配条件</param>
        /// <returns></returns>
        public List<T> FindGlobal(Predicate<T> match)
        {
            List<T> list = new List<T>();
            Foreach((p, k, m) =>
            {
                if (match(m))
                {
                    list.Add(m);
                }
                return true;
            });
            return list;
        }

        /// <summary>
        /// 遍历数据
        /// </summary>
        /// <param name="func">第一个参数为分组Key,第二个为实体Key,返回值为:false结束遍历</param>
        public void Foreach(Func<string, string, T, bool> func)
        {
            ForeachGroup(func);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool IsExist(Predicate<T> match)
        {
            return IsExistGroup(match);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="personalId"></param>
        /// <returns></returns>
        public BaseCollection GetCache(string personalId)
        {
            LoadingStatus loadStatus;
            BaseCollection collection;
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            if (TryGetGroup(personalId, out collection, out loadStatus))
            {
                return collection;
            }
            return null;
        }

        /// <summary>
        /// 自动加载
        /// </summary>
        /// <param name="personalId">私有分组ID</param>
        /// <returns></returns>
        public bool AutoLoad(string personalId)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            if (!IsExistData(personalId))
            {
                return TryLoadItem(personalId, false);
            }
            return true;
        }

        /// <summary>
        /// 从Redis加载所有缓存
        /// </summary>
        /// <param name="match"></param>
        /// <param name="isReplace"></param>
        public void LoadFrom(Predicate<T> match, bool isReplace = false)
        {
            string redisKey = CreateRedisKey();
            TransReceiveParam receiveParam = new TransReceiveParam(redisKey);
            receiveParam.Schema = SchemaTable();
            int maxCount = receiveParam.Schema.Capacity;
            var filter = new DbDataFilter(maxCount);
            receiveParam.DbFilter = filter;
            //receiveParam.Capacity = maxCount;
            LoadFrom(receiveParam, match, isReplace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="period"></param>
        /// <param name="checkMutilKey">The entity need to reload when it has be mutil key</param>
        /// <returns></returns>
        public bool AddRange(IEnumerable<T> enumerable, int period = 0, bool checkMutilKey = true)
        {
            if (!Update(enumerable))
            {
                return false;
            }
            foreach (var t in enumerable)
            {
                string key = t.GetKeyCode();
                int periodTime = 0;
                if (TryAddGroup(t.PersonalId, key, t, periodTime, true))
                {
                    if (checkMutilKey)
                    {
                        CheckMutilKeyAndLoad(t.PersonalId);
                    }
                    SetLoadSuccess(t.PersonalId);
                }
            }
            return true;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="t"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool Add(T t, int period = 0)
        {
            if (string.IsNullOrEmpty(t.PersonalId))
            {
                throw new ArgumentNullException("t", "t.PersonalId is null");
            }
            string key = t.GetKeyCode();
            int periodTime = 0;
            if (Update(t) && TryAddGroup(t.PersonalId, key, t, periodTime, true))
            {
                CheckMutilKeyAndLoad(t.PersonalId);
                SetLoadSuccess(t.PersonalId);
                return true;
            }
            return false;
        }
        /// <summary>
        /// add or update
        /// </summary>
        /// <param name="t"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddOrUpdate(T t, int period = 0)
        {
            if (string.IsNullOrEmpty(t.PersonalId))
            {
                throw new ArgumentNullException("t", "t.PersonalId is null");
            }
            string key = t.GetKeyCode();
            int periodTime = 0;
            if (Update(t) && AddOrUpdateGroup(t.PersonalId, key, t, periodTime, true))
            {
                CheckMutilKeyAndLoad(t.PersonalId);
                SetLoadSuccess(t.PersonalId);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddOrUpdate(IEnumerable<T> enumerable, int period = 0)
        {
            if (!Update(enumerable))
            {
                return false;
            }
            foreach (var t in enumerable)
            {
                string key = t.GetKeyCode();
                int periodTime = 0;
                if (AddOrUpdateGroup(t.PersonalId, key, t, periodTime, true))
                {
                    CheckMutilKeyAndLoad(t.PersonalId);
                    SetLoadSuccess(t.PersonalId);
                    continue;
                }
            }
            return true;
        }


        /// <summary>
        /// 是否有多个键且未加载成功状态
        /// </summary>
        /// <param name="personalId"></param>
        private void CheckMutilKeyAndLoad(string personalId)
        {
            CacheItemSet itemSet;
            var schema = EntitySchemaSet.Get<T>();
            if (schema.Keys.Length > 1 &&
                DataContainer.TryGetCacheItem(personalId, out itemSet) &&
               !itemSet.HasLoadSuccess)
            {
                TryLoadItem(personalId, true);
            }
        }


        /// <summary>
        /// 更新自已的数据
        /// </summary>
        /// <param name="personalId"></param>
        public void UpdateSelf(string personalId)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            UpdateGroup(true, personalId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool LoadFactory(bool isReplace)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        protected override bool LoadItemFactory(string key, bool isReplace)
        {
            //Model实体设置检查
            if ("10000".Equals(key))
            {
                TraceLog.WriteError("The {0} entity's attr cacheType is share.", DataContainer.RootKey);
            }
            return ProcessLoadParam(key, isReplace);
        }

        /// <summary>
        /// 处理加载数据参数
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        protected bool ProcessLoadParam(string personalId, bool isReplace)
        {
            string redisKey = CreateRedisKey(personalId);
            TransReceiveParam receiveParam = new TransReceiveParam(redisKey);
            receiveParam.Schema = SchemaTable();
            string paramName = receiveParam.Schema.PersonalName;
            int periodTime = receiveParam.Schema.PeriodTime;
            int maxCount = receiveParam.Schema.Capacity;

            var provider = DbConnectionProvider.CreateDbProvider(receiveParam.Schema);
            if (receiveParam.Schema.StorageType.HasFlag(StorageType.ReadOnlyDB) ||
                receiveParam.Schema.StorageType.HasFlag(StorageType.ReadWriteDB))
            {
                if (provider == null)
                {
                    TraceLog.WriteError("Not found db connection of {0} entity.", receiveParam.Schema.EntityName);
                    return false;
                }
                var filter = new DbDataFilter(maxCount);
                if (!string.IsNullOrEmpty(personalId))
                {
                    filter.Condition = provider.FormatFilterParam(paramName);
                    filter.Parameters.Add(paramName, personalId);
                }
                receiveParam.DbFilter = filter;
            }
            return TryLoadCache(personalId, receiveParam, periodTime, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="isReplace"></param>
        public void InitData(List<T> dataList, bool isReplace = true)
        {
            var st = SchemaTable();
            int periodTime = st == null ? 0 : st.PeriodTime;
            foreach (var pair in dataList.GroupBy(t => t.PersonalId))
            {
                CacheItemSet itemSet = InitContainer(pair.Key, periodTime);
                InitCache(pair.ToList(), periodTime, isReplace);
                itemSet.OnLoadSuccess();
            }

        }

        /// <summary>
        /// 更新所有的数据
        /// </summary>
        /// <param name="isChange"></param>
        /// <param name="personalId"></param>
        public override bool Update(bool isChange, string personalId = null)
        {
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            return UpdateGroup(isChange, personalId);
        }
        /// <summary>
        /// The value has be removed from the cache
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool RemoveCache(T value)
        {
            string key = value.GetKeyCode();
            string personalId = value.PersonalId;
            return TryRemove(personalId, key, entity => true);
        }

        /// <summary>
        /// 删除数据并移出缓存
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Delete(T value)
        {
            string key = value.GetKeyCode();
            string personalId = value.PersonalId;
            return TryRemove(personalId, key, entity =>
            {
                entity.OnDelete();
                TransSendParam sendParam = new TransSendParam() { IsChange = true };
                return DoSend(new[] { new KeyValuePair<string, T>(personalId, value) }, sendParam);
            });
        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="personalId"></param>
        /// <returns></returns>
        public bool IsExistData(string personalId)
        {
            LoadingStatus loadStatus;
            BaseCollection enityGroup;
            personalId = AbstractEntity.EncodeKeyCode(personalId);
            return TryGetGroup(personalId, out enityGroup, out loadStatus);
        }

    }
}