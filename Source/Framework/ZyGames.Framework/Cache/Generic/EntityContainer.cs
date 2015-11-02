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
using ProtoBuf;
using ZyGames.Framework.Cache.Generic.Pool;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存容器操作类,负责数据加载管理
    /// </summary>
    [ProtoContract, Serializable]
    public class EntityContainer<T> : BaseDisposable
        where T : IItemChangeEvent, IDataExpired, new()
    {
        private string _containerKey;
        private BaseCachePool _cachePool;
        private readonly Func<bool, bool> _loadFactory;
        private readonly Func<string, bool, bool> _loadItemFactory;
        private CacheContainer _container;

        internal EntityContainer(BaseCachePool cachePool, Func<bool, bool> loadFactory, Func<string, bool, bool> loadItemFactory)
        {
            _cachePool = cachePool;
            _loadFactory = loadFactory;
            _loadItemFactory = loadItemFactory;
            _containerKey = typeof(T).FullName;
            if (_cachePool != null && !_cachePool.TryGetValue(_containerKey, out _container))
            {
                Initialize();
                _cachePool.TryGetValue(_containerKey, out _container);
            }
        }

        #region Init
        /// <summary>
        /// 初始化容器
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public CacheItemSet InitGroupContainer(string groupKey, int periodTime)
        {
            bool isMutilKey = EntitySchemaSet.Get<T>().IsMutilKey;
            var lazy = new Lazy<CacheItemSet>(() =>
            {
                BaseCollection itemCollection = IsReadonly
                    ? (BaseCollection)new ReadonlyCacheCollection()
                    : new CacheCollection(isMutilKey ? 0 : 1);
                var itemSet = CreateItemSet(CacheType.Dictionary, periodTime);
                itemSet.HasCollection = true;
                itemSet.SetItem(itemCollection);
                return itemSet;
            });

            return Container.GetOrAdd(groupKey, name => lazy.Value);
        }

        /// <summary>
        /// 分配内存
        /// </summary>
        private void Initialize()
        {
            _cachePool.InitContainer(_containerKey);
        }
        #endregion

        #region Property
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadonly
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RootKey
        {
            get { return _containerKey; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return _container.Collection.Count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _container.Collection.IsEmpty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public LoadingStatus LoadStatus
        {
            get { return _container.LoadingStatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public CacheItemSet[] ChildrenItem
        {
            get
            {
                int index = 0;
                CacheItemSet[] items = new CacheItemSet[Container.Count];
                Container.Foreach<CacheItemSet>((key, itemSet) =>
                {
                    items[index] = itemSet;
                    index++;
                    return true;
                });
                return items;
            }
        }

        private BaseCollection Container
        {
            get
            {

                if (_container != null)
                {
                    return _container.Collection;
                }
                throw new Exception(string.Format("CacheContainer \"{0}\" is not created.", _containerKey));
            }
        }
        #endregion

        #region Contains
        private void CheckLoad()
        {
            //兼容调用AutoLoad方法时加载的部分数据
            if (_container != null && !_container.HasLoadSuccess)
            {
                Load();
            }
        }

        /// <summary>
        /// 数据是否改变
        /// </summary>
        /// <param name="key">实体Key</param>
        /// <returns></returns>
        public bool HasChange(string key)
        {
            return _cachePool.CheckChanged(_containerKey, key);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainGroupKey(string key)
        {
            CacheItemSet itemSet;
            if (Container.TryGetValue(key, out itemSet) && itemSet.HasLoadSuccess)
            {
                return itemSet.LoadingStatus == LoadingStatus.Success;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool IsExistEntity(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            CheckLoad();
            bool result = false;
            Container.Foreach<CacheItemSet>((key, itemSet) =>
            {
                T entity = (T)itemSet.GetItem();
                if (match(entity))
                {
                    result = true;
                    return false;
                }
                return true;
            });
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool IsExistGroup(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            CheckLoad();
            bool result = false;
            Container.Foreach<CacheItemSet>((key, itemSet) =>
            {
                BaseCollection enityGroup = (BaseCollection)itemSet.GetItem();
                if (enityGroup.IsExist(match))
                {
                    result = true;
                    return false;
                }
                return true;
            });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="itemSet"></param>
        /// <returns></returns>
        public bool TryGetCacheItem(string key, out CacheItemSet itemSet)
        {
            if (Container.TryGetValue(key, out itemSet))
            {
                return true;
            }
            return false;
        }


        #endregion

        #region Foreach
        /// <summary>
        /// 遍历实体
        /// </summary>
        /// <param name="func"></param>
        public void ForeachEntity(Func<string, T, bool> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            CheckLoad();
            Container.Foreach<CacheItemSet>((key, itemSet) =>
            {
                T entity = (T)itemSet.GetItem();
                return func(key, entity);
            });
        }

        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="func">第一个参数为分组Key,第二个为实体Key</param>
        public void ForeachGroup(Func<string, string, T, bool> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            CheckLoad();
            Container.Foreach<CacheItemSet>((key, itemSet) =>
            {
                BaseCollection enityGroup = (BaseCollection)itemSet.GetItem();
                enityGroup.Foreach(func, key);
                return true;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void ForeachQueue(Func<string, T, bool> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            CheckLoad();
            Container.Foreach<CacheItemSet>((key, itemSet) =>
            {
                CacheQueue<T> enityGroup = (CacheQueue<T>)itemSet.GetItem();
                enityGroup.Foreach(func, key);
                return true;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, CacheItemSet>> ToList()
        {

            CheckLoad();
            return Container.ToList<CacheItemSet>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> ToEntityEnumerable(bool isLoad = false)
        {
            if (isLoad) CheckLoad();
            return Container.GetEnumerable().Select(pari => pari.Value).OfType<CacheItemSet>().Select(itemSet => (T)itemSet.GetItem());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> ToGroupEnumerable()
        {
            CheckLoad();
            foreach (var pair in Container.GetEnumerable())
            {
                var itemSet = pair.Value as CacheItemSet;
                if (itemSet == null) continue;
                var enityGroup = (BaseCollection)itemSet.GetItem();

                foreach (var @enum in enityGroup.GetEnumerable())
                {
                    yield return @enum.Value as T;
                }
            }
        }
        #endregion

        #region Entity
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T TakeEntityFromKey(string key)
        {
            T value = default(T);
            foreach (var pair in Container.GetEnumerable())
            {
                var itemSet = pair.Value as CacheItemSet;
                if (itemSet == null) continue;
                var enityGroup = (BaseCollection)itemSet.GetItem();

                if (enityGroup.TryGetValue(key, out value))
                {
                    break;
                }
            }
            //string entityKey = key;
            //Container.Foreach<CacheItemSet>((k, itemSet) =>
            //{
            //    BaseCollection enityGroup = (BaseCollection)itemSet.GetItem();
            //    if (enityGroup.ContainsKey(entityKey))
            //    {
            //        value = enityGroup[entityKey] as T;
            //        return false;
            //    }
            //    return true;
            //});
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="isAutoLoad"></param>
        /// <returns></returns>
        public IEnumerable<T> TakeOrLoadGroup(IEnumerable<string> keys, bool isAutoLoad)
        {
            return GetMutilCacheItem(keys, isAutoLoad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isAutoLoad"></param>
        /// <returns></returns>
        public IEnumerable<T> TakeOrLoadGroup(string key, bool isAutoLoad)
        {
            BaseCollection enityGroup;
            LoadingStatus loadStatus;
            if (TryGetCacheItem(key, isAutoLoad, out enityGroup, out loadStatus))
            {
                foreach (var @enum in enityGroup.GetEnumerable())
                {
                    yield return @enum.Value as T;
                }
            }
            else
            {
                throw new Exception(string.Format("Load {0}.{1} fail.", _containerKey, key));
            }
        }

        /// <summary>
        /// 单一实体模型
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="entityData"></param>
        /// <returns></returns>
        public bool TryGetEntity(string entityKey, out T entityData)
        {
            LoadingStatus loadStatus;
            return TryGetCacheItem(entityKey, true, out entityData, out loadStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        /// <param name="isLoad"></param>
        /// <returns></returns>
        public bool AddOrUpdateEntity(string entityKey, T entityData, int periodTime, bool isLoad = false)
        {
            CacheItemSet itemSet;
            return AddOrUpdateEntity(entityKey, entityData, periodTime, out itemSet, isLoad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        /// <param name="itemSet"></param>
        /// <param name="isLoad"></param>
        /// <returns></returns>
        public bool AddOrUpdateEntity(string entityKey, T entityData, int periodTime, out  CacheItemSet itemSet, bool isLoad = false)
        {
            if (!Container.TryGetValue(entityKey, out itemSet))
            {
                itemSet = CreateItemSet(CacheType.Entity, periodTime);

                if (!Container.TryAdd(entityKey, itemSet))
                {
                    return false;
                }
            }
            CheckEventBind(entityData as AbstractEntity);
            itemSet.SetItem(entityData);
            if (!isLoad)
            {
                entityData.TriggerNotify();
            }
            entityData.IsInCache = true;
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        /// <param name="itemSet"></param>
        /// <param name="isLoad"></param>
        /// <returns></returns>
        public bool TryAddEntity(string entityKey, T entityData, int periodTime, out  CacheItemSet itemSet, bool isLoad = false)
        {
            itemSet = CreateItemSet(CacheType.Entity, periodTime);
            itemSet.SetItem(entityData);
            if (Container.TryAdd(entityKey, itemSet))
            {
                CheckEventBind(entityData as AbstractEntity);
                if (!isLoad)
                {
                    entityData.TriggerNotify();
                }
                entityData.IsInCache = true;
                return true;
            }
            return false;
        }
        #endregion

        #region Personal
        /// <summary>
        /// 分组集合模型
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="enityGroup"></param>
        /// <param name="loadStatus"></param>
        /// <returns></returns>
        public bool TryGetGroup(string groupKey, out BaseCollection enityGroup, out LoadingStatus loadStatus)
        {
            return TryGetCacheItem(groupKey, true, out enityGroup, out loadStatus);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="key"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        /// <param name="isLoad"></param>
        /// <returns></returns>
        public bool AddOrUpdateGroup(string groupKey, string key, T entityData, int periodTime, bool isLoad = false)
        {
            bool result = false;
            CacheItemSet itemSet = InitGroupContainer(groupKey, periodTime);
            if (itemSet != null && !Equals(entityData, default(T)))
            {
                var data = (BaseCollection)itemSet.GetItem();
                result = data.AddOrUpdate(key, entityData, (k, t) => entityData) == entityData;
                if (result)
                {
                    var temp = entityData as AbstractEntity;
                    CheckEventBind(temp);
                    if (!isLoad)
                    {
                        entityData.TriggerNotify();
                    }
                    entityData.IsInCache = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="key"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        /// <param name="isLoad"></param>
        /// <returns></returns>
        public bool TryAddGroup(string groupKey, string key, T entityData, int periodTime, bool isLoad = false)
        {
            CacheItemSet itemSet = InitGroupContainer(groupKey, periodTime);
            if (itemSet != null)
            {
                if (!Equals(entityData, default(T)) && ((BaseCollection)itemSet.GetItem()).TryAdd(key, entityData))
                {
                    CheckEventBind(entityData as AbstractEntity);
                    if (!isLoad)
                    {
                        entityData.TriggerNotify();
                    }
                    entityData.IsInCache = true;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool TryTakeGroup(string key, out T[] list)
        {
            list = default(T[]);
            CacheItemSet itemSet;
            if (Container.TryGetValue(key, out itemSet) && itemSet.HasLoadSuccess)
            {
                BaseCollection enityGroup = (BaseCollection)itemSet.GetItem();
                list = enityGroup.Values.Cast<T>().ToArray();
                return true;
            }
            return false;
        }
        #endregion

        #region Queue
        /// <summary>
        /// 队列模型
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="enityGroup"></param>
        /// <returns></returns>
        public bool TryGetQueue(string groupKey, out CacheQueue<T> enityGroup)
        {
            LoadingStatus loadStatus;
            return TryGetCacheItem(groupKey, false, out enityGroup, out loadStatus);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        /// <param name="expiredHandle"></param>
        /// <returns></returns>
        public bool TryAddQueue(string groupKey, T entityData, int periodTime, Func<string, CacheQueue<T>, bool> expiredHandle)
        {

            var lazy = new Lazy<CacheItemSet>(() =>
            {
                var temp = new CacheItemSet(CacheType.Queue, periodTime, IsReadonly);
                temp.SetItem(new CacheQueue<T>(expiredHandle));
                temp.OnLoadSuccess();
                return temp;
            });

            var itemSet = Container.GetOrAdd(groupKey, name => lazy.Value);
            if (itemSet != null)
            {
                //队列不存Redis,不触发事件
                ((CacheQueue<T>)itemSet.GetItem()).Enqueue(entityData);
                entityData.IsInCache = true;
                return true;
            }
            return false;
        }
        #endregion

        #region Rank

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool TryGetRankCount(string key, out int count)
        {
            count = 0;
            LoadingStatus loadStatus;
            CacheList<T> cacheItems;
            if (TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                count = cacheItems.Count;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public IEnumerable<int> GetRankNo<V>(string key, Predicate<V> match) where V : RankEntity, new()
        {
            LoadingStatus loadStatus;
            CacheList<V> cacheItems;
            if (!TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                throw new Exception(string.Format("Not key:{0} items", key));
            }
            int index = 0;
            foreach (var cacheItem in cacheItems)
            {
                index++;
                if (match(cacheItem))
                {
                    yield return index;
                }
            }
            yield return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetRankNo<V>(string key, V item) where V : RankEntity, new()
        {
            LoadingStatus loadStatus;
            CacheList<V> cacheItems;
            if (!TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                throw new Exception(string.Format("Not key:{0} items", key));
            }
            int index = cacheItems.IndexOf(item);
            return index > 0 ? index + 1 : -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool TryGetRangeRank(string key, int? count, out IEnumerable<T> list)
        {
            list = null;
            LoadingStatus loadStatus;
            CacheList<T> cacheItems;
            if (TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                list = !count.HasValue ? cacheItems : cacheItems.Take(count.Value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataList"></param>
        public bool SetRangeRank<V>(string key, params V[] dataList) where V : RankEntity, new()
        {
            LoadingStatus loadStatus;
            CacheList<V> cacheItems;
            if (!TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                return false;
            }
            var schema = EntitySchemaSet.Get<V>();
            TransSendParam sendParam = new TransSendParam(key) { Schema = schema };
            foreach (var item in dataList)
            {
                item.IsInCache = true;
                cacheItems.InsertSort(item, (x, y) => x.CompareTo(y));
            }
            return _cachePool.TrySetRankData(sendParam, dataList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public void RankSort<V>(string key, V item) where V : RankEntity, new()
        {
            LoadingStatus loadStatus;
            CacheList<V> cacheItems;
            if (!TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                return;
            }
            //score desc
            cacheItems.MoveBySort(item, (x, y) => y.Score.CompareTo(x.Score));
            _cachePool.TryUpdateRankEntity(key, cacheItems.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        public void RankSort<V>(string key) where V : RankEntity, new()
        {
            LoadingStatus loadStatus;
            CacheList<V> cacheItems;
            if (!TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                return;
            }
            cacheItems.SortDesc(t => t.Score);
            _cachePool.TryUpdateRankEntity(key, cacheItems.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public bool TryExchangeRank<V>(string key, V t1, V t2) where V : RankEntity, new()
        {
            LoadingStatus loadStatus;
            CacheList<V> cacheItems;
            if (!TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                return false;
            }
            if (_cachePool.TryExchangeRankData(key, t1, t2))
            {
                int index1 = cacheItems.BinarySearch(t1);
                int index2 = cacheItems.BinarySearch(t2);
                cacheItems.Exchange(index1, index2, (x, y) =>
                {
                    var a = x.Score;
                    var b = y.Score;
                    x.Score = b;
                    y.Score = a;
                });
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromScore"></param>
        /// <param name="toScore">-1: all</param>
        /// <returns></returns>
        public bool RemoveRankByScore<V>(string key, double fromScore, double? toScore) where V : RankEntity, new()
        {
            LoadingStatus loadStatus;
            CacheList<V> cacheItems;
            if (!TryGetCacheItem(key, true, out cacheItems, out loadStatus))
            {
                return false;
            }
            List<V> removeList;
            if (!cacheItems.RemoveAll(t => t.Score >= fromScore && (!toScore.HasValue || t.Score <= toScore.Value), out removeList))
            {
                return true;
            }
            foreach (var entity in removeList)
            {
                entity.IsInCache = false;
                entity.IsExpired = true;
                entity.OnDelete();
            }
            var schema = EntitySchemaSet.Get<V>();
            TransSendParam sendParam = new TransSendParam(key) { Schema = schema };
            return _cachePool.TrySetRankData(sendParam, removeList.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataList"></param>
        /// <param name="periodTime"></param>
        /// <param name="isLoad"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        internal bool TryLoadRangeRank<V>(string key, IEnumerable<V> dataList, int periodTime = 0, bool isLoad = false, bool isReplace = false) where V : RankEntity, new()
        {
            CacheItemSet itemSet;
            if (!TryGetOrAddRank(key, out itemSet, periodTime)) return false;
            var cacheItems = (CacheList<V>)itemSet.GetItem();
            if (isReplace && cacheItems.Count > 0)
            {
                cacheItems.Clear();
            }
            foreach (var item in dataList)
            {
                item.IsInCache = true;
                cacheItems.InsertSort(item, (x, y) => x.CompareTo(y));
            }
            if (isLoad) itemSet.OnLoadSuccess();
            return true;
        }

        internal bool TryGetOrAddRank(string key, out CacheItemSet itemSet, int periodTime = 0)
        {
            if (!Container.TryGetValue(key, out itemSet))
            {
                itemSet = new CacheItemSet(CacheType.Rank, periodTime, IsReadonly);
                itemSet.SetItem(new CacheList<T>());
                if (!Container.TryAdd(key, itemSet))
                {
                    return false;
                }
            }
            return true;
        }


        #endregion

        #region Load
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="receiveParam"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool TryReceiveData<V>(TransReceiveParam receiveParam, out List<V> dataList) where V : AbstractEntity, new()
        {
            return _cachePool.TryReceiveData(receiveParam, out dataList);
        }

        /// <summary>
        /// 尝试从DB中恢复数据
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="receiveParam"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool TryRecoverFromDb<V>(TransReceiveParam receiveParam, out List<V> dataList) where V : AbstractEntity, new()
        {
            return _cachePool.TryLoadFromDb(receiveParam, out dataList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool TryLoadHistory<V>(string redisKey, out List<V> dataList) where V : AbstractEntity, new()
        {
            return _cachePool.TryLoadHistory(redisKey, out dataList);
        }


        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <returns></returns>
        public LoadingStatus ReLoad()
        {
            if (_container != null && _loadFactory != null)
            {
                //修正刷新缓存中已删除的数据问题
                if (_container.IsEmpty)
                {
                    Initialize();
                }
                else
                {
                    _container.Collection.Clear();
                    _container.ResetStatus();
                }
                _container.OnLoadFactory(_loadFactory, true);

                return _container.LoadingStatus;
            }
            return LoadingStatus.None;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public LoadingStatus Load()
        {
            if (_container != null && _loadFactory != null)
            {
                if (_container.IsEmpty)
                {
                    Initialize();
                }
                if (_container.LoadingStatus != LoadingStatus.None)
                {
                    //如果是新增加的不能清除
                    _container.Collection.Clear();
                    _container.ResetStatus();
                }
                _container.OnLoadFactory(_loadFactory, false);
                return _container.LoadingStatus;
            }
            return LoadingStatus.None;
        }

        /// <summary>
        /// 加载指定Key数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        public bool LoadItem(string key, bool isReplace)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            CacheItemSet itemSet;
            bool isExist = Container.TryGetValue(key, out itemSet);
            if (isExist && itemSet.HasLoadSuccess)
            {
                //已同步成功，不需再次加载
                return true;
            }

            if (_loadItemFactory != null)
            {
                return _loadItemFactory(key, isReplace);
            }
            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="receiveParam"></param>
        /// <returns></returns>
        public List<V> LoadFrom<V>(TransReceiveParam receiveParam) where V : AbstractEntity, new()
        {
            List<V> dataList = null;
            if (_container != null && _loadFactory != null)
            {
                if (_container.IsEmpty)
                {
                    Initialize();
                }
                if (_container.LoadingStatus != LoadingStatus.None)
                {
                    //新增的数据不能清除掉
                    _container.Collection.Clear();
                    _container.ResetStatus();
                }
                _container.OnLoadFactory((r) =>
                {
                    if (_cachePool.TryReceiveData(receiveParam, out dataList))
                    {
                        return true;
                    }
                    return false;
                }, true);

            }
            return dataList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ReLoadItem(string key)
        {
            CacheItemSet itemSet;
            if (Container.TryGetValue(key, out itemSet))
            {
                itemSet.ResetStatus();
            }
            return LoadItem(key, true);
        }

        #endregion

        #region SendTo
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="sendParam"></param>
        public void SendData<V>(V[] dataList, TransSendParam sendParam) where V : AbstractEntity, new()
        {

            _cachePool.SendData(dataList, sendParam);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public bool SendSync<V>(IEnumerable<V> dataList) where V : AbstractEntity, new()
        {
            return DataSyncQueueManager.SendSync(dataList);
        }
        #endregion

        #region GetChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeKey"></param>
        /// <param name="isChange"></param>
        /// <returns></returns>
        protected List<KeyValuePair<string, CacheItemSet>> GetChangeList(string changeKey, bool isChange)
        {

            return _cachePool.GetChangeList<KeyValuePair<string, CacheItemSet>>(_containerKey, changeKey, isChange);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeKey"></param>
        /// <param name="isChange"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, T>> GetChangeEntity(string changeKey, bool isChange)
        {
            List<KeyValuePair<string, CacheItemSet>> changeList = GetChangeList(changeKey, isChange);
            return changeList.Select(pair =>
            {
                T entity = (T)pair.Value.ItemData;
                return new KeyValuePair<string, T>(pair.Key, entity);
            }).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeKey"></param>
        /// <param name="isChange"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, T>> GetChangeGroup(string changeKey, bool isChange)
        {
            List<KeyValuePair<string, CacheItemSet>> changeList = GetChangeList(changeKey, isChange);
            return (from pair in changeList
                    let itemList = (BaseCollection)pair.Value.ItemData
                    let itemArray = itemList.ToList<T>()
                    from itemPair in itemArray
                    select new KeyValuePair<string, T>(pair.Key, itemPair.Value)).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeKey"></param>
        /// <param name="isChange"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, T>> GetChangeQueue(string changeKey, bool isChange)
        {
            List<KeyValuePair<string, CacheItemSet>> changeList = GetChangeList(changeKey, isChange);
            return (from pair in changeList
                    let itemList = (CacheQueue<T>)pair.Value.ItemData
                    let itemArray = itemList.ToArray()
                    from item in itemArray
                    select new KeyValuePair<string, T>(pair.Key, item)).ToList();
        }

        #endregion

        #region Remove
        /// <summary>
        /// Remove entity from the cache, if use loaded from Redis
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool TryRemove(string groupKey, Func<CacheItemSet, bool> callback)
        {

            CacheItemSet itemSet;
            if (Container.TryRemove(groupKey, out itemSet))
            {
                var entity = itemSet.ItemData as IItemChangeEvent;
                if (entity != null)
                {
                    entity.IsInCache = false;
                    entity.IsExpired = true;
                }
                if (callback != null && callback(itemSet))
                {
                    itemSet.ResetStatus();
                    itemSet.Dispose();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove entity from the cache, if use loaded from Redis
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool TryRemove(string groupKey, string key, Func<T, bool> callback)
        {

            CacheItemSet itemSet;
            if (Container.TryGetValue(groupKey, out itemSet))
            {
                T entityData;
                var items = (BaseCollection)itemSet.GetItem();
                if (items.TryRemove(key, out entityData))
                {
                    entityData.IsInCache = false;
                    entityData.IsExpired = true;
                    if (callback != null && callback(entityData))
                    {
                        //Not trigger event notify
                        entityData.Dispose();
                    }
                    return true;
                }
                if (items.Count == 0 && Container.TryRemove(groupKey, out itemSet))
                {
                    itemSet.ResetStatus();
                    itemSet.Dispose();
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool TryRemove(Func<CacheContainer, bool> callback)
        {
            CacheContainer container;
            if (_cachePool.TryRemove(_containerKey, out container, callback))
            {
                container.Collection.Clear();
                container.ResetStatus();
                return true;
            }
            return false;
        }
        #endregion

        #region event
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void UnChangeNotify(string key)
        {
            CacheItemSet itemSet;
            if (Container.TryGetValue(key, out itemSet))
            {
                //触发Unchange事件通知
                var e = new CacheItemEventArgs
                {
                    ChangeType = CacheItemChangeType.UnChange
                };
                var itemData = itemSet.ItemData as IItemChangeEvent;
                if (itemData != null)
                {
                    itemData.UnChangeNotify(this, e);
                }
                else if (itemSet.ItemData is BaseCollection)
                {
                    //Not notify
                }
            }
        }
        #endregion

        #region private
        /// <summary>
        /// 尝试取缓存项，若不存在则自动加载
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="isAutoLoad">是否要自动加载</param>
        /// <param name="itemData"></param>
        /// <param name="loadStatus"></param>
        /// <returns></returns>
        private bool TryGetCacheItem<TValue>(string groupKey, bool isAutoLoad, out TValue itemData, out LoadingStatus loadStatus)
        {
            CheckLoad();
            itemData = default(TValue);
            loadStatus = LoadingStatus.None;
            if (string.IsNullOrEmpty(groupKey))
            {
                return false;
            }
            //处理分组方法加载，在内存中存在且加载不出错时，才不需要重读
            CacheItemSet itemSet;
            if (Container.TryGetValue(groupKey, out itemSet) && itemSet.HasLoadSuccess)
            {
                itemData = (TValue)itemSet.GetItem();
                loadStatus = itemSet.LoadingStatus;
                return true;
            }
            if (isAutoLoad && LoadItem(groupKey, false))
            {
                if (Container.TryGetValue(groupKey, out itemSet) && itemSet.HasLoadSuccess)
                {
                    loadStatus = itemSet.LoadingStatus;
                    itemData = (TValue)itemSet.GetItem();
                    return true;
                }
            }
            //fail
            if (Container.TryGetValue(groupKey, out itemSet))
            {
                loadStatus = itemSet.LoadingStatus;
            }
            return false;
        }

        private IEnumerable<T> GetMutilCacheItem(IEnumerable<string> keys, bool isAutoLoad)
        {
            CheckLoad();
            //保证返回顺序
            var result = new List<T>();
            var loadKey = new List<string>();
            var indexList = new Queue<int>();
            foreach (var key in keys)
            {
                CacheItemSet itemSet;
                if (isAutoLoad && Container.TryGetValue(key, out itemSet) && itemSet.HasLoadSuccess)
                {
                    var itemData = itemSet.GetItem();
                    var collection = itemData as BaseCollection;
                    if (collection == null)
                    {
                        result.Add((T)itemData);
                    }
                    else
                    {
                        result.AddRange(collection.Select(pair => (T)pair.Value));
                    }
                }
                else
                {
                    loadKey.Add(key);
                    indexList.Enqueue(result.Count);
                    result.Add(null);
                }
            }
            if (loadKey.Count == 0)
            {
                return result;
            }
            var enumratable = RedisConnectionPool.GetAllEntity<T>(loadKey);
            foreach (var t in enumratable)
            {
                var entity = t as AbstractEntity;
                if (entity == null) continue;
                CacheItemSet itemSet;
                if (CacheFactory.AddOrUpdateEntity(entity, out itemSet))
                {
                    itemSet.OnLoadSuccess();
                }
                int index = indexList.Count > 0 ? indexList.Dequeue() : -1;
                if (index == -1)
                {
                    result.Add(t);
                }
                else
                {
                    result[index] = t;
                }
            }
            return result;
        }


        private CacheItemSet CreateItemSet(CacheType cacheType, int periodTime)
        {
            CacheItemSet itemSet = new CacheItemSet(cacheType, periodTime, IsReadonly);
            if (!IsReadonly && _cachePool.Setting != null)
            {
                itemSet.OnChangedNotify += _cachePool.Setting.OnChangedNotify;
            }
            return itemSet;
        }

        /// <summary>
        /// 检查是否有绑定事件，防止没有绑定导致数据丢失
        /// </summary>
        /// <param name="data"></param>
        private void CheckEventBind(AbstractEntity data)
        {
            if (data == null || data.IsReadOnly) return;
            var schema = data.GetSchema();
            if (!schema.HasObjectColumns) return;

            var columns = schema.GetObjectColumns();
            foreach (var column in columns)
            {
                var temp = data.GetPropertyValue(column.Name) as IItemChangeEvent;
                if (temp != null && temp.ItemEvent.Parent == null)
                {
                    temp.PropertyName = column.Name;
                    data.AddChildrenListener(temp);
                    temp.IsInCache = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _containerKey = null;
                _cachePool = null;
                _container = null;
            }
            base.Dispose(disposing);
        }
        #endregion


    }
}