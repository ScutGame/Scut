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
using ProtoBuf;
using ZyGames.Framework.Event;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// Cache Bag
    /// </summary>
    [ProtoContract, Serializable]
    public class CacheBag<T> : EntityChangeEvent, IDataExpired, IEnumerable<T>, IEnumerable
    {
        private ConcurrentBag<T> _data;
        /// <summary>
        /// init
        /// </summary>
        public CacheBag()
            : base(false)
        {
            _data = new ConcurrentBag<T>();
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="isReadonly"></param>
        public CacheBag(IEnumerable<T> collection, bool isReadonly)
            : base(isReadonly)
        {
            _data = new ConcurrentBag<T>(collection);
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            _data.Add(value);
        }

        /// <summary>
        /// RemoveFirst
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool RemoveFirst(out T value)
        {
            return _data.TryTake(out value);
        }

        /// <summary>
        /// PeekFirst
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool PeekFirst(out T value)
        {
            return _data.TryPeek(out value);
        }
        /// <summary>
        /// RemoveExpired
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
        /// handle
        /// </summary>
        public Func<string, CacheBag<T>, bool> ExpiredHandle
        {
            get;
            set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Count
        /// </summary>
        public int Count
        {
            get { return _data.Count; }
        }
        /// <summary>
        /// SyncRoot
        /// </summary>
        public object SyncRoot { get; private set; }
        /// <summary>
        /// IsSynchronized
        /// </summary>
        public bool IsSynchronized { get; private set; }
    }
}
