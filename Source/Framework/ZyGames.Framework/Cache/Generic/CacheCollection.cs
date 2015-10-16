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
using System.Collections.Concurrent;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 读写缓存集合
    /// </summary>
    [ProtoContract, Serializable]
    public class CacheCollection : BaseCollection, IDictionary<string, object>
    {
        private ConcurrentDictionary<string, object> _cacheStruct;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disableEvent"></param>
        public CacheCollection(bool disableEvent = false)
            : this(0, disableEvent)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="disableEvent">是否禁用事件</param>
        public CacheCollection(int capacity, bool disableEvent = false)
            : base(false, disableEvent)
        {
            _cacheStruct = capacity > 0
                ? new ConcurrentDictionary<string, object>(Environment.ProcessorCount, capacity)
                : new ConcurrentDictionary<string, object>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TryGetValue(string key, out object value)
        {
            return _cacheStruct.TryGetValue(key, out value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object this[string key]
        {
            get
            {
                return _cacheStruct[key];
            }
            set
            {
                _cacheStruct[key] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public override void Add<T>(string key, T value)
        {
            TryAdd(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="updateValueFactory"></param>
        /// <returns></returns>
        public override T AddOrUpdate<T>(string key, T data, Func<string, T, T> updateValueFactory)
        {

            //CacheItemChangeType changeType = CacheItemChangeType.Add;
            Func<string, object, object> func = (updateKey, updateValue) =>
            {
                //changeType = CacheItemChangeType.Modify;
                return updateValueFactory(updateKey, (T)updateValue);
            };
            T temp = (T)_cacheStruct.AddOrUpdate(key, data, func);
            //AddChildrenListener(temp);
            //Notify(temp, changeType, PropertyName);
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool ContainsKey(string key)
        {
            return _cacheStruct.ContainsKey(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public override void Add(string key, object value)
        {
            TryAdd(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Remove(string key)
        {
            object data;
            return TryRemove(key, out data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool TryAdd<T>(string key, T data)
        {

            if (_cacheStruct.TryAdd(key, data))
            {
                //AddChildrenListener(data);
                //Notify(data, CacheItemChangeType.Add, PropertyName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public override T GetOrAdd<T>(string key, Func<string, T> valueFactory)
        {
            return (T)_cacheStruct.GetOrAdd(key, updateKey => valueFactory(updateKey));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool TryGetValue<T>(string key, out T data)
        {

            data = default(T);
            object temp;
            if (_cacheStruct.TryGetValue(key, out temp))
            {
                data = (T)temp;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        /// <param name="compData"></param>
        /// <returns></returns>
        public override bool TryUpdate<T>(string key, T newData, T compData)
        {

            if (_cacheStruct.TryUpdate(key, newData, compData))
            {
                //AddChildrenListener(newData);
                //Notify(newData, CacheItemChangeType.Modify, PropertyName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool TryRemove<T>(string key, out T data)
        {

            data = default(T);
            object temp;
            if (_cacheStruct.TryRemove(key, out temp))
            {
                data = (T)temp;
                //Notify(data, CacheItemChangeType.Remove, PropertyName);
                //RemoveChildrenListener(data);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            _cacheStruct.Clear();
            //ClearChildrenEvent();
            //Notify(this, CacheItemChangeType.Clear, PropertyName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Contains(KeyValuePair<string, object> item)
        {
            return ContainsKey(item.Key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public override void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {

            int index = 0;
            var enumerator = _cacheStruct.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (index >= arrayIndex && index < array.Length)
                {
                    array[index] = enumerator.Current;
                }
                index++;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Remove(KeyValuePair<string, object> item)
        {
            object data;
            return TryRemove(item.Key, out data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override List<KeyValuePair<string, T>> ToList<T>()
        {

            var list = new List<KeyValuePair<string, T>>();
            var enumerator = _cacheStruct.GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(new KeyValuePair<string, T>(enumerator.Current.Key, (T)enumerator.Current.Value));
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        public override void Foreach<T>(Func<string, T, bool> func)
        {
            var enumerator = _cacheStruct.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!func(enumerator.Current.Key, (T)enumerator.Current.Value))
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="groupKey"></param>
        public override void Foreach<T>(Func<string, string, T, bool> func, string groupKey)
        {

            var enumerator = _cacheStruct.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!func(groupKey, enumerator.Current.Key, (T)enumerator.Current.Value))
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<KeyValuePair<string, object>> GetEnumerable()
        {
            return _cacheStruct;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _cacheStruct.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        public override ICollection<string> Keys
        {
            get
            {
                return _cacheStruct.Keys;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override ICollection<object> Values
        {
            get
            {
                return _cacheStruct.Values;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override int Count
        {
            get { return _cacheStruct.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool IsEmpty
        {
            get { return _cacheStruct.IsEmpty; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /*event
                /// <summary>
                /// 
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="eventArgs"></param>
                protected override void Notify(object sender, CacheItemEventArgs eventArgs)
                {
                    base.Notify(sender, eventArgs);
                }
        */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cacheStruct = null;
            }
            base.Dispose(disposing);
        }

    }
}