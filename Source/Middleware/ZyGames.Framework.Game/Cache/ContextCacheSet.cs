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