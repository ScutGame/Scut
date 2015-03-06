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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Cache.Generic.Pool
{
    /// <summary>
    /// 缓存池对象,主键：T的类型名，值：CacheContainer对象
    /// </summary>
    internal abstract class BaseCachePool : BaseDisposable
    {
        private ITransponder _dbTransponder;
        private ITransponder _redisTransponder;
        private readonly ICacheSerializer _serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTransponder"></param>
        /// <param name="redisTransponder"></param>
        /// <param name="serializer"></param>
        protected BaseCachePool(ITransponder dbTransponder, ITransponder redisTransponder, ICacheSerializer serializer)
        {
            _dbTransponder = dbTransponder;
            _redisTransponder = redisTransponder;
            _serializer = serializer;
        }

        internal abstract void Init();

        internal CacheSetting Setting { get; set; }

        /// <summary>
        /// 初始化容器
        /// </summary>
        /// <param name="containerKey">空器主键</param>
        public CacheContainer InitContainer(string containerKey)
        {
            if (string.IsNullOrEmpty(containerKey))
            {
                throw new ArgumentNullException("containerKey");
            }
            return GetOrAdd<CacheContainer>(containerKey);
        }

        /// <summary>
        /// 是否包括指定Key
        /// </summary>
        /// <param name="rootKey"></param>
        /// <returns></returns>
        public abstract bool ContainsKey(string rootKey);
        /// <summary>
        /// 尝试增加
        /// </summary>
        /// <param name="rootKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool TryAdd(string rootKey, CacheContainer data);
        /// <summary>
        /// 尝试增加子项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootKey"></param>
        /// <param name="itemKey"></param>
        /// <param name="itemData"></param>
        /// <returns></returns>
        public abstract bool TryAddItem<T>(string rootKey, string itemKey, T itemData);
        /// <summary>
        /// 尝试获取或增加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootKey"></param>
        /// <returns></returns>
        public abstract T GetOrAdd<T>(string rootKey) where T : CacheContainer;
        /// <summary>
        /// 尝试获取
        /// </summary>
        /// <param name="rootKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool TryGetValue(string rootKey, out CacheContainer data);
        /// <summary>
        /// 尝试获取或增加子项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootKey"></param>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public abstract T GetItemOrAdd<T>(string rootKey, string itemKey) where T : BaseCollection;
        /// <summary>
        /// 尝试获取子项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootKey"></param>
        /// <param name="itemKey"></param>
        /// <param name="itemData"></param>
        /// <returns></returns>
        public abstract bool TryGetItem<T>(string rootKey, string itemKey, out T itemData);

        /// <summary>
        /// 尝试移除
        /// </summary>
        /// <param name="rootKey"></param>
        /// <param name="data"></param>
        /// <param name="callback">移除成功回调方法</param>
        /// <returns></returns>
        public abstract bool TryRemove(string rootKey, out CacheContainer data, Func<CacheContainer, bool> callback);

        /// <summary>
        /// 尝试移除子项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootKey"></param>
        /// <param name="itemKey"></param>
        /// <param name="itemData"></param>
        /// <param name="callback">移除成功回调方法</param>
        /// <returns></returns>
        public abstract bool TryRemoveItem<T>(string rootKey, string itemKey, out T itemData, Func<T, bool> callback);
        /// <summary>
        /// 清除
        /// </summary>
        public abstract void Clear();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract KeyValuePair<string, CacheContainer>[] ToArray();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<KeyValuePair<string, CacheContainer>> GetEnumerator();
        /// <summary>
        /// 
        /// </summary>
        public abstract ICollection<string> Keys { get; }
        /// <summary>
        /// 是否只读
        /// </summary>
        public abstract bool IsReadOnly { get; }
        /// <summary>
        /// 数量
        /// </summary>
        public abstract int Count { get; }
        /// <summary>
        /// 是否为空
        /// </summary>
        public abstract bool IsEmpty { get; }

        /// <summary>
        /// 尝试接收数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="receiveParam"></param>
        /// <param name="dataList"></param>
        /// <returns>return null is load error</returns>
        public bool TryReceiveData<T>(TransReceiveParam receiveParam, out List<T> dataList) where T : AbstractEntity, new()
        {
            dataList = null;
            bool hasDbConnect = DbConnectionProvider.CreateDbProvider(receiveParam.Schema) != null;
            var schema = receiveParam.Schema;
            //表为空时，不加载数据
            if (schema == null ||
                string.IsNullOrEmpty(schema.EntityName))
            {
                //DB is optional and can no DB configuration
                dataList = new List<T>();
                return true;
            }

            //配置库不放到Redis，尝试从DB加载
            if (schema.StorageType.HasFlag(StorageType.ReadOnlyDB) ||
                 schema.StorageType.HasFlag(StorageType.ReadWriteDB))
            {
                if (!hasDbConnect)
                {
                    dataList = new List<T>();
                    return true;
                }
                var result = _dbTransponder.TryReceiveData(receiveParam, out dataList);
                TraceLog.ReleaseWriteDebug("The readonly-data:{0} has been loaded {1}", receiveParam.RedisKey, dataList.Count);
                return result;
            }

            if (schema.StorageType.HasFlag(StorageType.ReadOnlyRedis) ||
                schema.StorageType.HasFlag(StorageType.ReadWriteRedis))
            {
                if (!string.IsNullOrEmpty(receiveParam.RedisKey) &&
                    _redisTransponder.TryReceiveData(receiveParam, out dataList))
                {
                    if (dataList.Count > 0)
                    {
                        TraceLog.ReleaseWriteDebug("The data:{0} has been loaded {1}", receiveParam.RedisKey, dataList.Count);
                        return true;
                    }
                    //从Redis历史记录表中加载
                    if (hasDbConnect && Setting != null && Setting.IsStorageToDb)
                    {
                        var result = TryLoadHistory(receiveParam.RedisKey, out dataList);
                        TraceLog.ReleaseWriteDebug("The data:{0} has been loaded {1} from history.", receiveParam.RedisKey, dataList.Count);
                        return result;
                    }
                    dataList = new List<T>();
                    return true;
                }
                //read faild from redis.
                return false;
            }
            return true;
        }

        /// <summary>
        /// 尝试从数据库中加载数据,并更新到Redis
        /// </summary>
        /// <returns></returns>
        public bool TryLoadFromDb<T>(TransReceiveParam receiveParam, out List<T> dataList) where T : AbstractEntity, new()
        {
            if (_dbTransponder.TryReceiveData(receiveParam, out dataList))
            {
                if (dataList.Count > 0)
                {
                    TraceLog.ReleaseWriteDebug("The data:{0} has been loaded {1} from db.", receiveParam.RedisKey, dataList.Count);
                    //load to Redis
                    return RedisConnectionPool.TryUpdateEntity(dataList);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从历史库中加载数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool TryLoadHistory<T>(string redisKey, out List<T> dataList)
        {
            string entityNameKey = RedisConnectionPool.GetRedisEntityKeyName(redisKey);
            bool result = false;
            dataList = null;
            SchemaTable schemaTable;
            if (EntitySchemaSet.TryGet<EntityHistory>(out schemaTable))
            {
                try
                {
                    var provider = DbConnectionProvider.CreateDbProvider(schemaTable);
                    if (provider == null)
                    {
                        //DB is optional and can no DB configuration
                        dataList = new List<T>();
                        return true;
                    }
                    TransReceiveParam receiveParam = new TransReceiveParam(entityNameKey);
                    receiveParam.Schema = schemaTable;
                    int maxCount = receiveParam.Schema.Capacity;
                    var filter = new DbDataFilter(maxCount);
                    string key = schemaTable.Keys[0];
                    filter.Condition = provider.FormatFilterParam(key);
                    filter.Parameters.Add(key, entityNameKey);
                    receiveParam.DbFilter = filter;

                    List<EntityHistory> historyList;
                    if (_dbTransponder.TryReceiveData(receiveParam, out historyList))
                    {
                        EntityHistory history = historyList.Count > 0 ? historyList[0] : null;
                        if (history != null && history.Value != null && history.Value.Length > 0)
                        {
                            //从DB备份中取使用protobuf
                            byte[][] bufferBytes = ProtoBufUtils.Deserialize<byte[][]>(history.Value);
                            byte[][] keys = bufferBytes.Where((b, index) => index % 2 == 0).ToArray();
                            byte[][] values = bufferBytes.Where((b, index) => index % 2 == 1).ToArray();
                            RedisConnectionPool.Process(client => client.HMSet(entityNameKey, keys, values));
                            dataList = values.Select(value => (T)_serializer.Deserialize(value, typeof(T))).ToList();
                            result = true;
                        }
                        else
                        {
                            dataList = new List<T>();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Try load Redis's history key:{0}\r\nerror:{1}", entityNameKey, ex);
                }
            }
            else
            {
                dataList = new List<T>();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 尝试发送数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="sendParam"></param>
        public void SendData<T>(T[] dataList, TransSendParam sendParam) where T : AbstractEntity, new()
        {
            //modify reason:提交到Redis同步队列处理
            DataSyncQueueManager.Send(dataList);
            //if (!sendParam.OnlyRedis)
            //{
            //    _dbTransponder.SendData(dataList, sendParam);
            //}
            //_redisTransponder.SendData(dataList, sendParam);
        }

        /// <summary>
        /// 检查数据是否改变
        /// </summary>
        /// <param name="rootKey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool CheckChanged(string rootKey, string key)
        {
            CacheContainer container;
            CacheItemSet itemSet;
            if (TryGetValue(rootKey, out container)
                && container.Collection.TryGetValue(key, out itemSet))
            {
                return itemSet.HasChanged;
            }
            return false;
        }

        /// <summary>
        /// 获取改变的数据,只读的数据不更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootKey">根主键，规则以实体(T)类型名</param>
        /// <param name="changeKey">实体的主键</param>
        /// <param name="isChange">更新到库中是全部数据还是改变的数据</param>
        /// <returns></returns>
        public virtual List<KeyValuePair<string, CacheItemSet>> GetChangeList<T>(string rootKey, string changeKey, bool isChange)
        {

            var containerList = new KeyValuePair<string, CacheContainer>[0];
            if (!string.IsNullOrEmpty(rootKey))
            {
                CacheContainer container;
                if (TryGetValue(rootKey, out container))
                {
                    containerList = new[] { new KeyValuePair<string, CacheContainer>(rootKey, container) };
                }
            }
            else
            {
                containerList = ToArray();
            }

            var changeList = new List<KeyValuePair<string, CacheItemSet>>();
            foreach (var containerPair in containerList)
            {
                CacheContainer container = containerPair.Value;
                if (container == null || container.IsReadOnly)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(changeKey))
                {
                    var itemSetList = container.Collection.ToList<CacheItemSet>();
                    var items = itemSetList.Where(itemPair => !isChange || itemPair.Value.HasChanged);
                    changeList.AddRange(items);
                }
                else
                {
                    CacheItemSet itemSet;
                    if (container.Collection.TryGetValue(changeKey, out itemSet)
                        && (!isChange || itemSet.HasChanged))
                    {
                        changeList.Add(new KeyValuePair<string, CacheItemSet>(changeKey, itemSet));
                    }
                }
            }
            return changeList;
        }

        /// <summary>
        /// 清除过期缓存
        /// </summary>
        public virtual void DisposeCache()
        {

            KeyValuePair<string, CacheContainer>[] containerList = ToArray();
            foreach (var containerPair in containerList)
            {
                var itemSetList = containerPair.Value.Collection.ToList<CacheItemSet>();
                foreach (var itemPair in itemSetList)
                {
                    if (itemPair.Value.HasChanged || !itemPair.Value.IsPeriod)
                    {
                        //clear sub item is expired.
                        itemPair.Value.RemoveExpired(itemPair.Key);
                        continue;
                    }
                    CacheItemSet itemSet;
                    if (containerPair.Value.Collection.TryRemove(itemPair.Key, out itemSet))
                    {
                        TraceLog.ReleaseWrite("Cache item:{0} key:{1} expired has been removed.",
                            containerPair.Key,
                            itemPair.Key);
                        itemSet.ProcessExpired(itemPair.Key);
                        itemSet.SetRemoveStatus();
                        itemSet.Dispose();
                    }
                }
                if (containerPair.Value.Collection.Count == 0)
                {
                    containerPair.Value.SetRemoveStatus();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //释放 托管资源 
                _dbTransponder = null;
                _redisTransponder = null;
            }
            base.Dispose(disposing);
        }
    }
}