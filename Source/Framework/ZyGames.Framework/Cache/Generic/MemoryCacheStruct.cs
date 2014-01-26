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
using ZyGames.Framework.Cache.Generic.Pool;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 内存中一定时间存在的实体，不存储数据库
    /// </summary>
    public class MemoryCacheStruct<T> : BaseDisposable
        where T : MemoryEntity, new()
    {
        private static BaseCachePool _momoryPools;

        static MemoryCacheStruct()
        {
            _momoryPools = new CachePool(null, null, false);
        }

        private CacheContainer _container;
        private string containerKey;

        /// <summary>
        /// 
        /// </summary>
        public MemoryCacheStruct()
        {
            containerKey = typeof(T).FullName;
            _container = _momoryPools.InitContainer(containerKey);
            Initialize();
        }

        private void Initialize()
        {
            _container.OnLoadFactory(InitCache, false);
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
        protected virtual bool InitCache()
        {
            return true;
        }
    }
}