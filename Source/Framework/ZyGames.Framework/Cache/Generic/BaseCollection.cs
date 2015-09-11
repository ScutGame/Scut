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
using System.Collections.Generic;
using ProtoBuf;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存块数据
    /// </summary>
    [ProtoContract, Serializable]
    public abstract class BaseCollection : IDataExpired, IDictionary<string, object>, IDisposable
    {
        private readonly bool _isReadOnly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isReadOnly"></param>
        /// <param name="disableEvent"></param>
        protected BaseCollection(bool isReadOnly, bool disableEvent)
            //: base(disableEvent)
        {
            _isReadOnly = isReadOnly;
        }

        /// <summary>
        /// 增加或更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="updateValueFactory"></param>
        /// <returns></returns>
        public abstract T AddOrUpdate<T>(string key, T data, Func<string, T, T> updateValueFactory);

        /// <summary>
        /// 是否存在键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool ContainsKey(string key);

        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract void Add(string key, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool Remove(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool TryGetValue(string key, out object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract object this[string key] { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract void Add<T>(string key, T value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool TryAdd<T>(string key, T data);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public abstract T GetOrAdd<T>(string key, Func<string, T> valueFactory);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool TryGetValue<T>(string key, out T data);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        /// <param name="compData"></param>
        /// <returns></returns>
        public abstract bool TryUpdate<T>(string key, T newData, T compData);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool TryRemove<T>(string key, out T data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public abstract void Add(KeyValuePair<string, object> item);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract bool Contains(KeyValuePair<string, object> item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public abstract void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract bool Remove(KeyValuePair<string, object> item);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract List<KeyValuePair<string, T>> ToList<T>();

        /// <summary>
        /// 遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">返回值为:false结束遍历</param>
        public abstract void Foreach<T>(Func<string, T, bool> func);

        /// <summary>
        /// 遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="groupKey"></param>
        public abstract void Foreach<T>(Func<string, string, T, bool> func, string groupKey);

        /// <summary>
        /// 判断指定条件是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool IsExist<T>(Predicate<T> match)
        {
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                var value = (T)enumerator.Current.Value;
                if (match(value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 查找指定条件匹配数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="match"></param>
        /// <returns></returns>
        public virtual List<T> Find<T>(Predicate<T> match)
        {
            List<T> list = new List<T>();
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                var value = (T)enumerator.Current.Value;
                if (match(value))
                {
                    list.Add(value);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<KeyValuePair<string, object>> GetEnumerable();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        public abstract ICollection<string> Keys { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract ICollection<object> Values { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsEmpty { get; }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 移除过期数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveExpired(string key)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}