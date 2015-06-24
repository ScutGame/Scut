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

namespace ZyGames.Framework.Collection.Generic
{
    /// <summary>
    /// 分组List
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class GroupList<K, V>
        where V : new()
    {
        private readonly ConcurrentDictionary<K, List<V>> _group = new ConcurrentDictionary<K, List<V>>();
        /// <summary>
        /// 
        /// </summary>
        public ICollection<K> Keys
        {
            get
            {
                return _group.Keys;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(K key, V value)
        {
            
            Func<K, List<V>> func = (k) => new List<V>();
            var list = _group.GetOrAdd(key, func);
            list.Add(value);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool TryGetValue(K key, out List<V> list)
        {
            return _group.TryGetValue(key, out list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool TryRemove(K key, out List<V> list)
        {
            return _group.TryRemove(key, out list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<K, List<V>>[] ToArray()
        {
            return _group.ToArray();
        }
    }
}