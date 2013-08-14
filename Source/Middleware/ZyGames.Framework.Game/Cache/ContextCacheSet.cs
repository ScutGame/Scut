using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Cache;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Cache
{
    /// <summary>
    /// 上下文缓存集
    /// </summary>
    public class ContextCacheSet<T> : BaseDisposable where T : CacheItem, new()
    {
        private string _cacheKey;
        private IDataContainer<T> _container;

        public ContextCacheSet(string cacheKey)
        {
            _container = CacheFactory.GetOrCreate<T>();
            _cacheKey = cacheKey;
        }

        public bool IsExistKey(params object[] keys)
        {
            BaseCollection collection;
            LoadingStatus loadStatus;
            if (_container.TryGetGroup(_cacheKey, out collection, out loadStatus))
            {
                T data;
                return collection.TryGetValue(BaseEntity.CreateKeyCode(keys), out data);
            }
            return false;
        }

        public bool TryAdd(string key, T t)
        {
            BaseCollection collection;
            LoadingStatus loadStatus;
            if (_container.TryGetGroup(_cacheKey, out collection, out loadStatus))
            {
                return collection.TryAdd(key, t);
            }
            return false;
        }

        public T this[string key]
        {
            get
            {
                BaseCollection collection;
                LoadingStatus loadStatus;
                if (_container.TryGetGroup(_cacheKey, out collection, out loadStatus))
                {
                    T data;
                    collection.TryGetValue(key, out data);
                    return data;
                }
                return default(T);
            }
        }

        public void Clear()
        {
            _container.TryRemove(_cacheKey, item => true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cacheKey = null;
                _container = null;
            }
            base.Dispose(disposing);
        }

    }
}
