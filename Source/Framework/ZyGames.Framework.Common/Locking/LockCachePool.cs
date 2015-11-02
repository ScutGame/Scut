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
using System.Threading;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Locking
{
    /// <summary>
    /// Lock cache pool
    /// </summary>
    public sealed class LockCachePool
    {
        class ExpireItem : IDisposable
        {

            public ExpireItem()
            {
            }

            public long Key { get; set; }

            public Timer ExpireTimer { get; set; }
            public DateTime AccessTime { get; set; }

            internal long WaitTimeoutNum;

            public object Target { get; set; }

            public void Dispose()
            {
                if (ExpireTimer != null)
                {
                    ExpireTimer.Dispose();
                }
                ExpireTimer = null;
                GC.SuppressFinalize(this);
            }

        }
        private object syncRoot = new object();
        private readonly int _expireTime;
        private readonly int _checkTime;
        private readonly Dictionary<long, ExpireItem> _lockPool = new Dictionary<long, ExpireItem>();

        /// <summary>
        /// init
        /// </summary>
        public LockCachePool()
        {
            WaitTimeoutMaxNum = 3;
        }

        /// <summary>
        /// init, enable timer check expire item.
        /// </summary>
        /// <param name="expireTime">ms</param>
        /// <param name="checkTime">ms</param>
        public LockCachePool(int expireTime, int checkTime = 10000)
            : this()
        {
            _expireTime = expireTime;
            _checkTime = checkTime;
        }
        /// <summary>
        /// 
        /// </summary>
        public int WaitTimeoutMaxNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return _lockPool.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            lock (syncRoot)
            {
                foreach (var pair in _lockPool)
                {
                    var lockItem = pair.Value;
                    DoExit(lockItem);
                }
                _lockPool.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Add(long key)
        {
            GetOrCreate(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <param name="waitTimeOutNum"></param>
        /// <returns></returns>
        public bool TryEnter(long key, int timeout, out long waitTimeOutNum)
        {
            ExpireItem lockItem = GetOrCreate(key);
            waitTimeOutNum = lockItem.WaitTimeoutNum;
            if (WaitTimeoutMaxNum > 0 && lockItem.WaitTimeoutNum > WaitTimeoutMaxNum) return false;

            if (Monitor.TryEnter(lockItem, timeout))
            {
                return true;
            }
            Interlocked.Increment(ref lockItem.WaitTimeoutNum);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <param name="wait">wait get lock object</param>
        /// <param name="enter">enter lock object</param>
        /// <param name="waitTimeOutNum"></param>
        /// <returns></returns>
        public bool TryEnter(long key, int timeout, object wait, out object enter, out long waitTimeOutNum)
        {
            ExpireItem lockItem = GetOrCreate(key);
            enter = lockItem.Target;
            waitTimeOutNum = lockItem.WaitTimeoutNum;
            if (WaitTimeoutMaxNum > 0 && lockItem.WaitTimeoutNum > WaitTimeoutMaxNum) return false;

            if (Monitor.TryEnter(lockItem, timeout))
            {
                lockItem.Target = wait;
                return true;
            }
            Interlocked.Increment(ref lockItem.WaitTimeoutNum);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Enter(long key)
        {
            ExpireItem lockItem = GetOrCreate(key);
            Monitor.Enter(lockItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Exit(long key)
        {
            ExpireItem lockItem;
            if (_lockPool.TryGetValue(key, out lockItem))
            {
                DoExit(lockItem);
            }
        }

        private void DoExit(ExpireItem lockItem)
        {
            if (lockItem != null && Monitor.IsEntered(lockItem))
            {
                Monitor.Exit(lockItem);
            }
            if (lockItem != null) Interlocked.Exchange(ref lockItem.WaitTimeoutNum, 0);
        }

        private ExpireItem GetOrCreate(long key)
        {
            lock (syncRoot)
            {
                ExpireItem child;
                if (!_lockPool.ContainsKey(key))
                {
                    child = new ExpireItem();
                    child.Key = key;
                    child.AccessTime = DateTime.Now;
                    if (_expireTime > 0 && _checkTime > 0)
                    {
                        int dueTime = 100 + (_lockPool.Count % 100); //Random start
                        child.ExpireTimer = new Timer(RemoveItem, child, dueTime, _checkTime);
                    }
                    _lockPool[key] = child;
                }
                else
                {
                    child = _lockPool[key];
                    child.AccessTime = MathUtils.Now;
                }
                return child;
            }
        }

        private void RemoveItem(object state)
        {
            try
            {
                ExpireItem expireItem = state as ExpireItem;
                if (expireItem == null) return;
                if (MathUtils.Now.Subtract(expireItem.AccessTime).TotalMilliseconds >= _expireTime)
                {
                    lock (syncRoot)
                    {
                        _lockPool.Remove(expireItem.Key);
                    }
                    DoExit(expireItem);
                    expireItem.Dispose();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("LockCachePool ExpireCheck error:{0}", ex);
            }
        }
    }
}
