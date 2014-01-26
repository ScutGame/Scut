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
using ProtoBuf;
using ZyGames.Framework.Cache.Generic.Pool;
using ZyGames.Framework.Common;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存容器操作类,负责数据加载管理
    /// </summary>
    [ProtoContract, Serializable]
    internal class EntityContainer<T> : BaseDisposable, IDataContainer<T>
        where T : IItemChangeEvent, IDataExpired, new()
    {
        private string _containerKey;
        private BaseCachePool _cachePool;
        private readonly Func<bool> _loadFactory;
        private readonly Func<string, bool> _loadItemFactory;
        private CacheContainer _container;

        internal EntityContainer(BaseCachePool cachePool, Func<bool> loadFactory, Func<string, bool> loadItemFactory)
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

        /// <summary>
        /// 分配内存
        /// </summary>
        private void Initialize()
        {
            _cachePool.InitContainer(_containerKey);
        }

        public bool IsReadonly
        {
            get;
            set;
        }

        public string RootKey
        {
            get { return _containerKey; }
        }

        public int Count
        {
            get
            {
                return _container.Collection.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return _container.Collection.IsEmpty;
            }
        }

        public LoadingStatus LoadStatus
        {
            get { return _container.LoadingStatus; }
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
        /// <param name="itemSet"></param>
        /// <returns></returns>
        public bool TryGetCache(string key, out CacheItemSet itemSet)
        {
            if (Container.TryGetValue(key, out itemSet))
            {
                return true;
            }
            return false;
        }

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

        public List<KeyValuePair<string, CacheItemSet>> ToList()
        {

            CheckLoad();
            return Container.ToList<CacheItemSet>();
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
                    //ReLoad时需要清除掉之前的
                    _container.Collection.Clear();
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
        /// <returns></returns>
        public bool LoadItem(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            lock (key)
            {
                CacheItemSet itemSet;
                bool isExist = Container.TryGetValue(key, out itemSet);
                if (isExist && !itemSet.HasLoadError)
                {
                    return true;
                }
                if (_loadItemFactory != null)
                {
                    return _loadItemFactory(key);
                }
            }
            return false;
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
                itemSet.SetRemoveStatus();
            }
            return LoadItem(key);
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
        /// <returns></returns>
        public bool AddOrUpdateEntity(string entityKey, T entityData, int periodTime)
        {
            CacheItemSet itemSet;
            if (!Container.TryGetValue(entityKey, out itemSet))
            {
                itemSet = CreateItemSet(CacheType.Entity, periodTime);

                if (!Container.TryAdd(entityKey, itemSet))
                {
                    return false;
                }
            }
            itemSet.SetItem(entityData);
            itemSet.OnLoadSuccess();
            return true;
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
        /// 
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public bool TryAddEntity(string entityKey, T entityData, int periodTime)
        {
            var itemSet = CreateItemSet(CacheType.Entity, periodTime);
            itemSet.SetItem(entityData);
            if (Container.TryAdd(entityKey, itemSet))
            {
                itemSet.OnLoadSuccess();
                return true;
            }
            return false;
        }

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
        /// <returns></returns>
        public bool AddOrUpdateGroup(string groupKey, string key, T entityData, int periodTime)
        {
            CacheItemSet itemSet = InitGroupContainer(groupKey, periodTime);
            if (itemSet != null && !Equals(entityData, default(T)))
            {
                var data = (BaseCollection)itemSet.GetItem();
                T oldValue;
                if (data.TryGetValue(key, out oldValue))
                {
                    return data.TryUpdate(key, entityData, oldValue);
                }
                return data.TryAdd(key, entityData);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="key"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public bool TryAddGroup(string groupKey, string key, T entityData, int periodTime)
        {
            CacheItemSet itemSet = InitGroupContainer(groupKey, periodTime);
            if (itemSet != null)
            {
                if (!Equals(entityData, default(T)) && ((BaseCollection)itemSet.GetItem()).TryAdd(key, entityData))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 初始化容器
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public CacheItemSet InitGroupContainer(string groupKey, int periodTime)
        {
            var lazy = new Lazy<CacheItemSet>(() =>
            {
                BaseCollection itemData = IsReadonly
                    ? (BaseCollection)new ReadonlyCacheCollection()
                    : new CacheCollection();
                var itemSet = CreateItemSet(CacheType.Dictionary, periodTime);
                itemSet.SetItem(itemData);
                return itemSet;
            });

            return Container.GetOrAdd(groupKey, name => lazy.Value);
        }

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
                return temp;
            });

            var itemSet = Container.GetOrAdd(groupKey, name => lazy.Value);
            if (itemSet != null)
            {
                ((CacheQueue<T>)itemSet.GetItem()).Enqueue(entityData);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool TryRemove(string groupKey, Func<CacheItemSet, bool> callback)
        {

            CacheItemSet itemSet;
            if (Container.TryRemove(groupKey, out itemSet))
            {
                bool result = true;
                if (callback != null)
                {
                    result = callback(itemSet);
                }
                if (result)
                {
                    itemSet.SetRemoveStatus();
                    itemSet.Dispose();
                }
                return result;
            }
            return false;
        }
        /// <summary>
        /// 
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
                if (((BaseCollection)itemSet.GetItem()).TryRemove(key, out entityData))
                {
                    bool result = true;
                    if (callback != null)
                    {
                        result = callback(entityData);
                    }
                    if (result)
                    {
                        entityData.Dispose();
                    }
                    return result;
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
                container.SetRemoveStatus();
                container.Dispose();
                return true;
            }
            return false;
        }

        public bool TryReceiveData<V>(TransReceiveParam receiveParam, out List<V> dataList) where V : AbstractEntity, new()
        {
            return _cachePool.TryReceiveData(receiveParam, out dataList);
        }

        public bool TryLoadHistory<V>(string redisKey, out List<V> dataList) where V : AbstractEntity, new()
        {
            return _cachePool.TryLoadHistory(redisKey, out dataList);
        }

        public void SendData<V>(V[] dataList, TransSendParam sendParam) where V : AbstractEntity, new()
        {

            _cachePool.SendData(dataList, sendParam);
        }

        protected List<KeyValuePair<string, CacheItemSet>> GetChangeList(string changeKey, bool isChange)
        {

            return _cachePool.GetChangeList<KeyValuePair<string, CacheItemSet>>(_containerKey, changeKey, isChange);
        }

        public List<KeyValuePair<string, T>> GetChangeEntity(string changeKey, bool isChange)
        {

            List<KeyValuePair<string, CacheItemSet>> changeList = GetChangeList(changeKey, isChange);
            return changeList.Select(pair =>
            {
                T entity = (T)pair.Value.ItemData;
                return new KeyValuePair<string, T>(pair.Key, entity);
            }).ToList();
        }

        public List<KeyValuePair<string, T>> GetChangeGroup(string changeKey, bool isChange)
        {

            List<KeyValuePair<string, CacheItemSet>> changeList = GetChangeList(changeKey, isChange);
            return (from pair in changeList
                    let itemList = (BaseCollection)pair.Value.ItemData
                    let itemArray = itemList.ToList<T>()
                    from itemPair in itemArray
                    where itemPair.Value.HasChanged
                    select new KeyValuePair<string, T>(pair.Key, itemPair.Value)).ToList();
        }

        public List<KeyValuePair<string, T>> GetChangeQueue(string changeKey, bool isChange)
        {

            List<KeyValuePair<string, CacheItemSet>> changeList = GetChangeList(changeKey, isChange);
            return (from pair in changeList
                    let itemList = (CacheQueue<T>)pair.Value.ItemData
                    let itemArray = itemList.ToArray()
                    from item in itemArray
                    where item.HasChanged
                    select new KeyValuePair<string, T>(pair.Key, item)).ToList();
        }

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
                itemSet.UnChangeNotify(this, e);
            }
        }

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
            if (Container.TryGetValue(groupKey, out itemSet) && !itemSet.HasLoadError)
            {
                itemData = (TValue)itemSet.GetItem();
                loadStatus = itemSet.LoadingStatus;
                return true;
            }
            if (isAutoLoad && LoadItem(groupKey))
            {
                if (Container.TryGetValue(groupKey, out itemSet) && !itemSet.HasLoadError)
                {
                    loadStatus = itemSet.LoadingStatus;
                    itemData = (TValue)itemSet.GetItem();
                    return true;
                }
            }
            if (Container.TryGetValue(groupKey, out itemSet))
            {
                loadStatus = itemSet.LoadingStatus;
            }
            return false;
        }

    }
}