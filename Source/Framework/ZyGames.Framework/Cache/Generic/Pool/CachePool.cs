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
using System.Collections.Concurrent;
using System.Collections.Generic;
using ProtoBuf;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Cache.Generic.Pool
{
    /// <summary>
    /// 缓存池
    /// </summary>
    [ProtoContract, Serializable]
    class CachePool : BaseCachePool
    {
        private readonly bool _isReadOnly;
        private ConcurrentDictionary<string, CacheContainer> _cacheStruct;

        public CachePool(ITransponder dbTransponder, ITransponder redisTransponder, bool isReadOnly, ICacheSerializer serializer)
            : base(dbTransponder, redisTransponder, serializer)
        {
            _isReadOnly = isReadOnly;
            _cacheStruct = new ConcurrentDictionary<string, CacheContainer>();
        }

        internal override void Init()
        {
            _cacheStruct = new ConcurrentDictionary<string, CacheContainer>();
        }

        public override bool ContainsKey(string key)
        {
            return _cacheStruct.ContainsKey(key);
        }

        public override bool TryAdd(string key, CacheContainer container)
        {
            return _cacheStruct.TryAdd(key, container);
        }

        public override bool TryAddItem<T>(string key, string itemKey, T itemData)
        {
            
            CacheContainer container = GetOrAdd<CacheContainer>(key);
            return container.Collection.TryAdd(itemKey, itemData);
        }

        public override T GetOrAdd<T>(string key)
        {
            
            var lazy = new Lazy<CacheContainer>(() => new CacheContainer(IsReadOnly));
            return (T)_cacheStruct.GetOrAdd(key, name => lazy.Value);
        }

        public override bool TryGetValue(string key, out CacheContainer data)
        {
            return _cacheStruct.TryGetValue(key, out data);
        }

        public override T GetItemOrAdd<T>(string key, string itemKey)
        {
            
            var container = GetOrAdd<CacheContainer>(key);
            var lazy = new Lazy<BaseCollection>(() => new CacheCollection(true));
            return container.Collection.GetOrAdd(itemKey, name => (T)lazy.Value);
        }

        public override bool TryGetItem<T>(string key, string itemKey, out T itemData)
        {
            
            CacheContainer container;
            itemData = default(T);
            if (_cacheStruct.TryGetValue(key, out container))
            {
                return container.Collection.TryGetValue(itemKey, out itemData);
            }
            return false;
        }

        public override bool TryRemove(string key, out CacheContainer container, Func<CacheContainer, bool> callback)
        {
            if (_cacheStruct.TryRemove(key, out container))
            {
                return callback != null ? callback(container) : true;
            }
            return false;
        }

        public override bool TryRemoveItem<T>(string key, string itemKey, out T itemData, Func<T, bool> callback)
        {
            
            CacheContainer container;
            itemData = default(T);
            if (_cacheStruct.TryGetValue(key, out container))
            {
                if (container.Collection.TryRemove(itemKey, out itemData))
                {
                    return callback != null ? callback(itemData) : true;
                }
            }
            return false;
        }

        public override void Clear()
        {
            _cacheStruct.Clear();
        }

        public override KeyValuePair<string, CacheContainer>[] ToArray()
        {
            return _cacheStruct.ToArray();
        }

        public override IEnumerator<KeyValuePair<string, CacheContainer>> GetEnumerator()
        {
            return _cacheStruct.GetEnumerator();
        }

        public override ICollection<string> Keys
        {
            get
            {
                return _cacheStruct.Keys;
            }
        }

        public override bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        public override int Count
        {
            get
            {
                return _cacheStruct.Count;
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return _cacheStruct.IsEmpty;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}