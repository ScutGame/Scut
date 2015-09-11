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
using ZyGames.Framework.Common.Reflect;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 内存中一定时间存在的实体，不存储数据库
    /// </summary>
    public class MemoryCacheStruct<T> : BaseDisposable
        where T : MemoryEntity, new()
    {

        private CacheContainer _container;
        private string containerKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isLoad"></param>
        public MemoryCacheStruct(bool isLoad = true)
        {
            containerKey = typeof(T).FullName;
            _container = CacheFactory.MemoryCache.InitContainer(containerKey);
            if (isLoad) Initialize(false);
        }

        private void Initialize(bool isReload)
        {
            _container.OnLoadFactory(InitCache, isReload);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            Initialize(true);
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContainerKey
        {
            get
            {
                return containerKey;
            }
        }

        /// <summary>
        /// 是否空
        /// </summary>
        public bool IsEmpty
        {
            get { return _container.IsEmpty; }
        }

        /// <summary>
        /// 容器数量
        /// </summary>
        public int Count
        {
            get { return _container.Collection.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void Foreach(Func<string, T, bool> func)
        {
            _container.Collection.Foreach(func);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(string key, T value)
        {
            return _container.Collection.TryAdd(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdate(string key, T value)
        {
            return _container.Collection.AddOrUpdate(key, value, (k, t) => value) == value;
        }
        /// <summary>
        /// 尝试移除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback">移除成功回调</param>
        /// <returns></returns>
        public bool TryRemove(string key, Func<T, bool> callback = null)
        {
            T data;
            if (_container.Collection.TryRemove<T>(key, out  data))
            {
                if (callback != null)
                {
                    return callback(data);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string key, out T value)
        {
            return _container.Collection.TryGetValue(key, out value);
        }

        /// <summary>
        /// 查找第一个匹配数据
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T Find(Predicate<T> match)
        {
            return _container.Collection.Find(match).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<T> FindAll()
        {
            return FindAll(t => true);
        }

        /// <summary>
        /// 查找所有匹配数据
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<T> FindAll(Predicate<T> match)
        {
            return _container.Collection.Find(match);
        }

        /// <summary>
        /// Inits the cache.
        /// </summary>
        /// <returns><c>true</c>, if cache was inited, <c>false</c> otherwise.</returns>
        protected virtual bool InitCache(bool isReplace)
        {
            return InitCache();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitCache()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        protected virtual object GetPropertyValue(T entity, SchemaColumn column)
        {
            return ObjectAccessor.Create(entity)[column.Name];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        protected virtual void SetPropertyValue(T entity, SchemaColumn column, object value)
        {
            object fieldValue;
            if (column.IsSerialized)
            {
                //指定序列化方式
                if (column.DbType == ColumnDbType.LongBlob ||
                    column.DbType == ColumnDbType.Blob)
                {
                    fieldValue = ProtoBufUtils.Deserialize(value as byte[], column.ColumnType);
                }
                else
                {
                    fieldValue = DeserializeJsonObject(value, column);
                }
            }
            else
            {
                fieldValue = AbstractEntity.ParseValueType(value, column.ColumnType);
            }
            ObjectAccessor.Create(entity)[column.Name] = fieldValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        protected bool TryLoadFromDb(DbDataFilter filter, out List<T> dataList)
        {
            var schema = EntitySchemaSet.Get<T>();
            return DataSyncManager.TryReceiveSql<T>(schema, filter, SetPropertyValue, out dataList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="synchronous"></param>
        /// <returns></returns>
        protected bool TrySaveToDb(IEnumerable<T> entityList, bool synchronous = false)
        {
            return DataSyncManager.SendSql<T>(entityList, true, GetPropertyValue, null, synchronous);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        protected bool TryLoadFromRedis(out List<T> dataList, string personalId = null)
        {
            string redisKey = string.IsNullOrEmpty(personalId) ? containerKey : string.Format("{0}_{1}", containerKey, personalId);
            SchemaTable schema = EntitySchemaSet.Get<T>();
            return RedisConnectionPool.TryGetEntity(redisKey, schema, out dataList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        protected bool TrySaveToRedis(IEnumerable<T> entityList)
        {
            return RedisConnectionPool.TryUpdateEntity(entityList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldAttr"></param>
        /// <returns></returns>
        protected object DeserializeJsonObject(object value, SchemaColumn fieldAttr)
        {
            if (fieldAttr.ColumnType.IsSubclassOf(typeof(Array)))
            {
                value = value.ToString().StartsWith("[") ? value : "[" + value + "]";
            }
            string tempValue = value.ToNotNullString();
            if (!string.IsNullOrEmpty(fieldAttr.JsonDateTimeFormat) &&
                tempValue.IndexOf(@"\/Date(", StringComparison.Ordinal) == -1)
            {
                return JsonUtils.DeserializeCustom(tempValue, fieldAttr.ColumnType, fieldAttr.JsonDateTimeFormat);
            }
            return JsonUtils.Deserialize(tempValue, fieldAttr.ColumnType);

        }
    }
}