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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ZyGames.Framework.Event;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 线程安全的字典集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    [ProtoContract, Serializable]
    public class CacheDictionary<T, V> : EntityChangeEvent, IDictionary<T, V>, IReadOnlyDictionary<T, V>, IDataExpired, IDisposable
    {
        private readonly ConcurrentDictionary<T, V> _cacheStruct;
        private bool _isReadOnly;
        /// <summary>
        /// 
        /// </summary>
        public CacheDictionary()
            : this(false, null)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expiredHandle"></param>
        public CacheDictionary(Func<string, CacheDictionary<T, V>, bool> expiredHandle)
            : this(false, expiredHandle)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public CacheDictionary(bool isReadOnly)
            : this(isReadOnly, null)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isReadOnly"></param>
        /// <param name="expiredHandle"></param>
        public CacheDictionary(bool isReadOnly, Func<string, CacheDictionary<T, V>, bool> expiredHandle)
            : base(isReadOnly)
        {
            _isReadOnly = isReadOnly;
            _cacheStruct = new ConcurrentDictionary<T, V>();
            ExpiredHandle = expiredHandle;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeEvent"></param>
        public override void AddChildrenListener(object changeEvent)
        {
            CheckSingleBindEvent(changeEvent);
            base.AddChildrenListener(changeEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        public Func<string, CacheDictionary<T, V>, bool> ExpiredHandle
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<T, V>[] ToArray()
        {
            return _cacheStruct.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerator">返回False结束遍历</param>
        public void Foreach(Func<T, V, bool> enumerator)
        {
            var er = _cacheStruct.GetEnumerator();
            while (er.MoveNext())
            {
                if (!enumerator(er.Current.Key, er.Current.Value))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
        {
            return _cacheStruct.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<T, V> item)
        {
            AddChildrenListener(item.Value);
            if (_cacheStruct.TryAdd(item.Key, item.Value))
            {
                Notify(item.Value, CacheItemChangeType.Add, PropertyName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            var values = _cacheStruct.Values.ToList();
            _cacheStruct.Clear();
            Notify(this, CacheItemChangeType.Clear, PropertyName);
            ClearChildrenEvent(values);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<T, V> item)
        {
            return _cacheStruct.ContainsKey(item.Key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<T, V>[] array, int arrayIndex)
        {
            int index = 0;
            var er = _cacheStruct.GetEnumerator();
            while (er.MoveNext())
            {
                if (index == arrayIndex && index < array.Length)
                {
                    array[index] = er.Current;
                }
                index++;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<T, V> item)
        {
            V value;
            if (_cacheStruct.TryRemove(item.Key, out value))
            {
                Notify(value, CacheItemChangeType.Remove, PropertyName);
                RemoveChildrenListener(value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _cacheStruct.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(T key)
        {
            return _cacheStruct.ContainsKey(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(T key, V value)
        {
            AddChildrenListener(value);
            if (_cacheStruct.TryAdd(key, value))
            {
                Notify(value, CacheItemChangeType.Add, PropertyName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(T key)
        {
            V value;
            if (_cacheStruct.TryRemove(key, out value))
            {
                Notify(value, CacheItemChangeType.Remove, PropertyName);
                RemoveChildrenListener(value);
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
        public bool TryGetValue(T key, out V value)
        {
            return _cacheStruct.TryGetValue(key, out value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V this[T key]
        {
            get { return _cacheStruct[key]; }
            set
            {
                V old;
                if (TryGetValue(key, out old))
                {
                    RemoveChildrenListener(old);
                }
                AddChildrenListener(value);
                _cacheStruct[key] = value;
                Notify(value, CacheItemChangeType.Modify, PropertyName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<T> Keys
        {
            get { return _cacheStruct.Keys; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<V> Values
        {
            get { return _cacheStruct.Values; }
        }

        IEnumerable<T> IReadOnlyDictionary<T, V>.Keys
        {
            get { return _cacheStruct.Keys; }
        }

        IEnumerable<V> IReadOnlyDictionary<T, V>.Values
        {
            get { return _cacheStruct.Values; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveExpired(string key)
        {
            if (ExpiredHandle != null)
            {
                return ExpiredHandle(key, this);
            }
            return false;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }


    }
}