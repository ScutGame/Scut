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
using ZyGames.Framework.Event;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTransponder"></param>
        /// <param name="redisTransponder"></param>
        protected BaseCachePool(ITransponder dbTransponder, ITransponder redisTransponder)
        {
            _dbTransponder = dbTransponder;
            _redisTransponder = redisTransponder;
        }


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
            bool result = false;
            dataList = null;
            //表为空时，不加载数据
            if (receiveParam.Schema != null && string.IsNullOrEmpty(receiveParam.Schema.Name))
            {
                dataList = new List<T>();
                return true;
            }

            if (!string.IsNullOrEmpty(receiveParam.RedisKey) &&
                _redisTransponder.TryReceiveData(receiveParam, out dataList))
            {
                if (dataList.Count > 0)
                {
                    return true;
                }
                if (receiveParam.Schema == null || DbConnectionProvider.CreateDbProvider(receiveParam.Schema) == null)
                {
                    //DB is optional and can no DB configuration
                    dataList = new List<T>();
                    return true;
                }

                //配置库不放到Redis，尝试从DB加载
                if (receiveParam.Schema.AccessLevel == AccessLevel.ReadOnly)
                {
                    return _dbTransponder.TryReceiveData(receiveParam, out dataList);
                }
                //从Redis历史记录表中加载
                result = TryLoadHistory(receiveParam.RedisKey, out dataList);
            }

            return result;

        }

        public bool TryLoadHistory<T>(string redisKey, out List<T> dataList)
        {
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
                    TransReceiveParam receiveParam = new TransReceiveParam(redisKey);
                    receiveParam.Schema = schemaTable;
                    int maxCount = receiveParam.Schema.Capacity;
                    var filter = new DbDataFilter(maxCount);
                    string key = schemaTable.Keys[0];
                    filter.Condition = provider.FormatFilterParam(key);
                    filter.Parameters.Add(key, redisKey);
                    receiveParam.DbFilter = filter;
                    receiveParam.Capacity = maxCount;

                    List<EntityHistory> historyList;
                    if (_dbTransponder.TryReceiveData(receiveParam, out historyList))
                    {
                        if (historyList.Count == 0)
                        {
                            dataList = new List<T>();
                            result = true;
                        }
                        else
                        {
                            EntityHistory history = historyList[0];
                            RedisManager.Process(client => client.Set(redisKey, history.Value));
                            var dataSet = ProtoBufUtils.Deserialize<Dictionary<string, T>>(history.Value);
                            if (dataSet != null)
                            {
                                dataList = dataSet.Values.ToList();
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Try load Redis's history key:{0}\r\nerror:{1}", redisKey, ex);
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
            //todo modify use redis
            CacheChangeManager.Current.SetEntity(dataList);
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
                        itemPair.Value.RemoveExpired(itemPair.Key);
                        continue;
                    }
                    CacheItemSet itemSet;
                    if (containerPair.Value.Collection.TryRemove(itemPair.Key, out itemSet))
                    {
                        TraceLog.ReleaseWrite("Cache item:{0} key:{1} expired has been removed.",
                            containerPair.Key,
                            itemPair.Key);
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