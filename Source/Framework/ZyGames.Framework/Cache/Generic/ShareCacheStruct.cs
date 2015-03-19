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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 共享的缓存模型
    /// </summary>
    public static class ShareCacheStruct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="createFactory"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(object key, Lazy<T> createFactory) where T : ShareEntity, new()
        {
            return GetOrAdd<T>(new[] { key }, createFactory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <param name="createFactory"></param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static T GetOrAdd<T>(object[] keys, Lazy<T> createFactory) where T : ShareEntity, new()
        {
            var cache = new ShareCacheStruct<T>();
            T result = cache.FindKey(keys);
            if (cache.LoadingStatus != LoadingStatus.Success)
            {
                throw new Exception(string.Format("Entity {0} load  key:{1} error", typeof(T).Name, keys));
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
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static T Get<T>(object key, bool allowNull = false) where T : ShareEntity, new()
        {
            return Get<T>(new[] { key }, allowNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <param name="allowNull"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns></returns>
        public static T Get<T>(object[] keys, bool allowNull) where T : ShareEntity, new()
        {
            var cache = new ShareCacheStruct<T>();
            T result = cache.FindKey(keys);
            if (cache.LoadingStatus != LoadingStatus.Success)
            {
                throw new Exception(string.Format("Entity {0} load  key:{1} error", typeof(T).Name, string.Join("-", keys)));
            }
            if (!allowNull && result == null)
            {
                throw new NullReferenceException(string.Format("Not found entity {0} key:{1}.", typeof(T).Name, string.Join("-", keys)));
            }
            return result;
        }

        #region Get

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2>(object key, out T1 t1, out T2 t2, bool allowNull = false)
            where T1 : ShareEntity, new()
            where T2 : ShareEntity, new()
        {
            t1 = Get<T1>(key, allowNull);
            t2 = Get<T2>(key, allowNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2, T3>(object key, out T1 t1, out T2 t2, out T3 t3, bool allowNull = false)
            where T1 : ShareEntity, new()
            where T2 : ShareEntity, new()
            where T3 : ShareEntity, new()
        {
            t1 = Get<T1>(key, allowNull);
            t2 = Get<T2>(key, allowNull);
            t3 = Get<T3>(key, allowNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2, T3, T4>(object key, out T1 t1, out T2 t2, out T3 t3, out T4 t4, bool allowNull = false)
            where T1 : ShareEntity, new()
            where T2 : ShareEntity, new()
            where T3 : ShareEntity, new()
            where T4 : ShareEntity, new()
        {
            t1 = Get<T1>(key, allowNull);
            t2 = Get<T2>(key, allowNull);
            t3 = Get<T3>(key, allowNull);
            t4 = Get<T4>(key, allowNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2, T3, T4, T5>(object key, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, bool allowNull = false)
            where T1 : ShareEntity, new()
            where T2 : ShareEntity, new()
            where T3 : ShareEntity, new()
            where T4 : ShareEntity, new()
            where T5 : ShareEntity, new()
        {
            t1 = Get<T1>(key, allowNull);
            t2 = Get<T2>(key, allowNull);
            t3 = Get<T3>(key, allowNull);
            t4 = Get<T4>(key, allowNull);
            t5 = Get<T5>(key, allowNull);
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
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        /// <param name="t6"></param>
        /// <param name="allowNull"></param>
        public static void Get<T1, T2, T3, T4, T5, T6>(object key, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, bool allowNull = false)
            where T1 : ShareEntity, new()
            where T2 : ShareEntity, new()
            where T3 : ShareEntity, new()
            where T4 : ShareEntity, new()
            where T5 : ShareEntity, new()
            where T6 : ShareEntity, new()
        {
            t1 = Get<T1>(key, allowNull);
            t2 = Get<T2>(key, allowNull);
            t3 = Get<T3>(key, allowNull);
            t4 = Get<T4>(key, allowNull);
            t5 = Get<T5>(key, allowNull);
            t6 = Get<T6>(key, allowNull);
        }
        #endregion
    }

    /// <summary>
    /// 共享的缓存模型
    /// </summary>
    public class ShareCacheStruct<T> : BaseCacheStruct<T> where T : ShareEntity, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public ShareCacheStruct()
        {
        }

        /// <summary>
        /// Get loading status from redis or DB
        /// </summary>
        public LoadingStatus LoadingStatus
        {
            get { return DataContainer.LoadStatus; }
        }

        /// <summary>
        /// 遍历数据
        /// </summary>
        /// <param name="func"></param>
        public void Foreach(Func<string, T, bool> func)
        {
            ForeachEntity(func);
        }

        /// <summary>
        /// 通过Key查找
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public T FindKey(params object[] keys)
        {
            T data;
            string key = AbstractEntity.CreateKeyCode(keys);
            if (TryGetEntity(key, out data))
            {
                return data;
            }
            return default(T);
        }

        /// <summary>
        /// 查找第一个匹配数据
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T Find(Predicate<T> match)
        {
            T t = default(T);
            ForeachEntity((key, value) =>
            {
                if (match == null || match(value))
                {
                    t = value;
                    return false;
                }
                return true;
            });
            return t;
        }

        /// <summary>
        /// 查找所有匹配数据
        /// </summary>
        /// <param name="isSort"></param>
        /// <returns></returns>
        public List<T> FindAll(bool isSort = true)
        {
            return FindAll(m => true, isSort);
        }

        /// <summary>
        /// 查找所有匹配数据
        /// </summary>
        /// <param name="match"></param>
        /// <param name="isSort"></param>
        /// <returns></returns>
        public List<T> FindAll(Predicate<T> match, bool isSort = true)
        {
            List<T> list = new List<T>();
            ForeachEntity((key, value) =>
            {
                if (match == null || match(value))
                {
                    list.Add(value);
                }
                return true;
            });
            if (isSort)
            {
                return list.QuickSort();
            }
            return list;
        }

        /// <summary>
        /// 数据项是否改变
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public bool HasChange(string keyCode)
        {
            return HasChangeCache(keyCode);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool IsExist(Predicate<T> match)
        {
            return IsExistEntity(match);
        }

        /// <summary>
        /// 自动加载
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool AutoLoad(DbDataFilter filter)
        {
            if (!IsExistData())
            {
                return OnLoad(filter);
            }
            return true;
        }

        private bool OnLoad(DbDataFilter filter)
        {
            var redisKey = CreateRedisKey();
            TransReceiveParam receiveParam = new TransReceiveParam(redisKey);
            receiveParam.Schema = SchemaTable();
            receiveParam.DbFilter = filter;
            int periodTime = receiveParam.Schema == null ? 0 : receiveParam.Schema.PeriodTime;
            return TryLoadCache(receiveParam, periodTime);
        }

        private bool IsExistData()
        {
            return !IsEmpty;
        }

        /// <summary>
        /// 更新所有的数据
        /// </summary>
        /// <param name="isChange"></param>
        public override void Update(bool isChange)
        {
            UpdateEntity(isChange);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="t"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool Add(T t, int period = 0)
        {
            string key = t.GetKeyCode();
            SchemaTable schemaTable = SchemaTable();
            int periodTime = schemaTable == null ? 0 : schemaTable.PeriodTime;
            return TryAddEntity(key, t, periodTime);
        }

        /// <summary>
        /// add or update
        /// </summary>
        /// <param name="t"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddOrUpdate(T t, int period = 0)
        {
            string key = t.GetKeyCode();
            SchemaTable schemaTable = SchemaTable();
            int periodTime = schemaTable == null ? 0 : schemaTable.PeriodTime;
            return AddOrUpdateEntity(key, t, periodTime);
        }

        /// <summary>
        /// The value has be removed from the cache
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool RemoveCache(T value)
        {
            string key = value.GetKeyCode();
            return TryRemove(key, item => true);
        }
        /// <summary>
        /// 删除数据并移出缓存
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Delete(T value)
        {
            string key = value.GetKeyCode();
            return TryRemove(key, item =>
            {
                value.OnDelete();
                TransSendParam sendParam = new TransSendParam() { IsChange = true };
                DoSend(new[] { new KeyValuePair<string, T>(key, value) }, sendParam);
                return true;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool LoadFactory()
        {
            int capacity = 0;
            SchemaTable schemaTable;
            if (EntitySchemaSet.TryGet<T>(out schemaTable))
            {
                capacity = schemaTable.Capacity;
            }
            return OnLoad(new DbDataFilter(capacity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override bool LoadItemFactory(string key)
        {
            //string redisKey = CreateRedisKey(key);
            //var schema = SchemaTable();
            //if (schema != null && schema.AccessLevel == AccessLevel.ReadWrite)
            //{
            //    int periodTime = schema.PeriodTime;
            //    List<T> dataList;
            //    if (DataContainer.TryLoadHistory(redisKey, out dataList))
            //    {
            //        InitCache(dataList, periodTime);
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        protected override bool InitCache(List<T> dataList, int periodTime)
        {
            foreach (var data in dataList)
            {
                if (data == null) continue;
                data.Reset();
                string key = data.GetKeyCode();
                bool result = AddOrUpdateEntity(key, data, periodTime);
                if (!result)
                {
                    TraceLog.WriteError("Load data:\"{0}\" tryadd key:\"{1}\" error.", DataContainer.RootKey, key);
                    return false;
                }
                DataContainer.UnChangeNotify(key);
            }
            return true;
        }
    }
}