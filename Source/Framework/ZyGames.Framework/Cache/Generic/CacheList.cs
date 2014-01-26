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
using ZyGames.Framework.Common;
using ZyGames.Framework.Event;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存List结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ProtoContract, Serializable]
    public class CacheList<T> : EntityChangeEvent, IDataExpired, IList<T>
    {
        private List<T> _list;

        /// <summary>
        /// 
        /// </summary>
        public CacheList()
            : this(0, false)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isReadonly"></param>
        public CacheList(bool isReadonly)
            : this(0, isReadonly)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="isReadonly"></param>
        public CacheList(int length, bool isReadonly)
            : this(length, null, isReadonly)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="expiredHandle"></param>
        /// <param name="isReadonly"></param>
        public CacheList(int length, Func<string, CacheList<T>, bool> expiredHandle, bool isReadonly)
            : base(isReadonly)
        {
            ExpiredHandle = expiredHandle;
            _list = length > 0 ? new List<T>(length) : new List<T>();

        }

        /// <summary>
        /// 
        /// </summary>
        public Func<string, CacheList<T>, bool> ExpiredHandle
        {
            get;
            set;
        }

        void IList<T>.RemoveAt(int index)
        {
            RemoveAt(index);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return index < _list.Count ? _list[index] : default(T);
            }
            set
            {
                lock (_list)
                {
                    if (index < _list.Count)
                    {
                        RemoveChildrenListener(_list[index]);
                        _list[index] = value;
                    }
                    else
                    {
                        _list.Add(value);
                    }
                    AddChildrenListener(value);
                    Notify(value, CacheItemChangeType.Modify, PropertyName);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (_list)
            {
                _list.Add(item);
                AddChildrenListener(item);
                Notify(item, CacheItemChangeType.Add, PropertyName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="math"></param>
        public bool TryAdd(T item, Predicate<T> math)
        {
            lock (_list)
            {
                if (math(item))
                {
                    _list.Add(item);
                    AddChildrenListener(item);
                    Notify(item, CacheItemChangeType.Add, PropertyName);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            lock (_list)
            {
                _list.Clear();
                Notify(this, CacheItemChangeType.Clear, PropertyName);
                ClearChildrenEvent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return _list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            var list = new List<T>();
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                list.Add(e.Current);
            }
            return list;
        }

        /// <summary>
        /// Func返回false跳出遍历
        /// </summary>
        public void Foreach(Func<T, bool> pre)
        {
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                if (!pre(e.Current))
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            lock (_list)
            {
                if (_list.Remove(item))
                {
                    Notify(item, CacheItemChangeType.Remove, PropertyName);
                    RemoveChildrenListener(item);
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            lock (_list)
            {
                _list.Insert(index, item);
                AddChildrenListener(item);
                Notify(item, CacheItemChangeType.Remove, PropertyName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveAt(int index)
        {
            lock (_list)
            {
                var temp = _list[index];
                _list.RemoveAt(index);
                Notify(temp, CacheItemChangeType.Remove, PropertyName);
                RemoveChildrenListener(temp);
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public int RemoveAll(Predicate<T> match)
        {
            lock (_list)
            {
                var tempList = _list.FindAll(match);
                int count = 0;
                foreach (var item in tempList)
                {
                    if (_list.Remove(item))
                    {
                        count++;
                        RemoveChildrenListener(item);
                    }
                }
                if (count > 0)
                {
                    Notify(this, CacheItemChangeType.RemoveAll, PropertyName);
                }
                return count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
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
        /// 以指定的顺序，插入项
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparison"></param>
        public void InsertSort(T item, Comparison<T> comparison)
        {
            lock (_list)
            {
                _list.InsertSort(item, comparison);
                AddChildrenListener(item);
                Notify(item, CacheItemChangeType.Add, PropertyName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool Exists(Predicate<T> match)
        {
            return _list.Exists(match);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T Find(Predicate<T> match)
        {
            return _list.Find(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T FindLast(Predicate<T> match)
        {
            return _list.FindLast(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<T> FindAll(Predicate<T> match)
        {
            if (match == null)
            {
                return _list;
            }
            return _list.FindAll(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> GetListRange(int index, int count)
        {
            return _list.GetRange(index, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<T> GetRange(int pageIndex, int pageSize, out int pageCount)
        {
            return _list.GetPaging(pageIndex, pageSize, out pageCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<T> GetRange(int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            return _list.GetPaging(pageIndex, pageSize, out pageCount, out recordCount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _list = null;
            }
            base.Dispose(disposing);
        }
    }
}