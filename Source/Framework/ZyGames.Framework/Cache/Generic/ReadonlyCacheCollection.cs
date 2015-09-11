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
using ProtoBuf;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 只读的缓存集合
    /// </summary>
    [ProtoContract, Serializable]
    public class ReadonlyCacheCollection : BaseCollection
    {
        private Dictionary<string, object> _cacheStruct;
        /// <summary>
        /// 
        /// </summary>
        public ReadonlyCacheCollection()
            : base(true, true)
        {
            _cacheStruct = new Dictionary<string, object>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TryGetValue(string key, out object value)
        {
            return TryGetValue(key, out value);
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
        /// <param name="data"></param>
        /// <param name="updateValueFactory"></param>
        /// <returns></returns>
        public override T AddOrUpdate<T>(string key, T data, Func<string, T, T> updateValueFactory)
        {

            T updateData = updateValueFactory(key, data);
            if (!_cacheStruct.ContainsKey(key))
            {
                _cacheStruct.Add(key, updateData);
            }
            else
            {
                _cacheStruct[key] = updateData;
            }
            return updateData;
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
        /// <returns></returns>
        public override bool TryAdd<T>(string key, T data)
        {

            if (!_cacheStruct.ContainsKey(key))
            {
                _cacheStruct.Add(key, data);
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

            T newValue = valueFactory(key);
            if (!_cacheStruct.ContainsKey(key))
            {
                _cacheStruct.Add(key, newValue);
                return newValue;
            }
            return (T)_cacheStruct[key];
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
            if (_cacheStruct.ContainsKey(key))
            {
                data = (T)_cacheStruct[key];
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

            if (_cacheStruct.ContainsKey(key))
            {
                T oldValue = (T)_cacheStruct[key];
                _cacheStruct[key] = newData;
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
            if (_cacheStruct.ContainsKey(key))
            {
                data = (T)_cacheStruct[key];
                _cacheStruct.Remove(key);
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
            return _cacheStruct.Remove(item.Key);
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
            get { return _cacheStruct.Keys; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override ICollection<object> Values
        {
            get { return _cacheStruct.Values; }
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
            get { return _cacheStruct.Count == 0; }
        }
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