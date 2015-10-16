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
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace ZyGames.Framework.Common
{
    /// <summary>
    /// Object pool
    /// </summary>
    /// <typeparam name="T">The type of object in the pool.</typeparam>
    public sealed class ObjectPoolWithExpire<T> : IDisposable
    {
        struct PoolItem : IEquatable<PoolItem>
        {
            public T Item;
            public DateTime AccessTime;

            public override int GetHashCode()
            {
                return Item.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj is PoolItem)
                {
                    return this.Equals((PoolItem)obj);
                }
                return false;
            }

            public bool Equals(PoolItem other)
            {
                return object.ReferenceEquals(other.Item, this.Item);
            }
        }

        private readonly HashSet<PoolItem> pool;
        private object syncRoot = new object();
        private Func<T> factory;
        private Timer expireTimer;
        private int expireTime;
        private int isInTimer;
        private int minPoolSize;
        private static Logger Logger = LogManager.GetLogger("ObjectPool");

        /// <summary>
        /// pool min size
        /// </summary>
        public int MinPoolSize
        {
            get { return minPoolSize; }
            set { minPoolSize = value; }
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="factory">factory</param>
        /// <param name="enableExpire"></param>
        /// <param name="expireTime">sec</param>
        /// <param name="minPoolSize"></param>
        public ObjectPoolWithExpire(Func<T> factory, bool enableExpire = false, int expireTime = 300, int minPoolSize = 5)
        {
            if (factory == null) throw new ArgumentNullException("factory");

            this.factory = factory;
            this.expireTime = expireTime;
            this.minPoolSize = minPoolSize <= 0 ? 5 : minPoolSize;
            pool = new HashSet<PoolItem>();
            if (enableExpire)
            {
                expireTimer = new Timer(ExpireCheck, null, 60000, 60000);
            }
        }

        private void ExpireCheck(object state)
        {
            if (Interlocked.CompareExchange(ref isInTimer, 1, 0) == 1)
            {
                return;
            }
            try
            {
                List<PoolItem> expiredList = new List<PoolItem>();
                lock (syncRoot)
                {
                    int leftnum = pool.Count;
                    foreach (var item in pool)
                    {
                        if (leftnum <= minPoolSize) break;
                        if (MathUtils.Now.Subtract(item.AccessTime).TotalSeconds >= expireTime)
                        {
                            expiredList.Add(item);
                            leftnum--;
                        }
                    }
                    foreach (var item in expiredList)
                    {
                        pool.Remove(item);
                    }
                }

                foreach (var item in expiredList)
                {
                    try
                    {
                        IDisposable dispose = item.Item as IDisposable;
                        if (dispose != null)
                        {
                            dispose.Dispose();
                        }
                        else
                        {
                            var wcfProxy = item.Item as ICommunicationObject;
                            if (wcfProxy != null)
                            {
                                try { wcfProxy.Close(); }
                                catch (CommunicationException) { wcfProxy.Abort(); }
                                catch (TimeoutException) { wcfProxy.Abort(); }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Dispose PoolItem", ex);
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref isInTimer, 0);
            }
        }

        /// <summary>
        /// Pool count
        /// </summary>
        public int PoolCount { get { return pool.Count; } }

        /// <summary>
        /// get item 
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            //bool needSleep = false;
            //lock (syncRoot)
            //{
            //    if (pool.Count == 0) needSleep = true;
            //}
            //if (needSleep) Thread.Sleep(10);
            lock (syncRoot)
            {
                if (pool.Count == 0) return factory();
                PoolItem result = pool.First();
                pool.Remove(result);
                return result.Item;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Create()
        {
            return factory();
        }
        /// <summary>
        /// Use default factory create.
        /// </summary>
        public void Put()
        {
            T item = factory();
            Put(item);
        }

        /// <summary>
        /// set item
        /// </summary>
        /// <param name="item"></param>
        public void Put(T item)
        {
            lock (syncRoot)
            {
                pool.Add(new PoolItem { Item = item, AccessTime = MathUtils.Now });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (expireTimer != null) expireTimer.Dispose();

            lock (syncRoot)
            {
                while (pool.Count != 0)
                {
                    PoolItem result = pool.First();
                    pool.Remove(result);
                    try
                    {
                        IDisposable dispose = result.Item as IDisposable;
                        if (dispose != null)
                        {
                            dispose.Dispose();
                        }
                        else
                        {
                            var wcfProxy = result.Item as ICommunicationObject;
                            if (wcfProxy != null)
                            {
                                try { wcfProxy.Close(); }
                                catch (CommunicationException) { wcfProxy.Abort(); }
                                catch (TimeoutException) { wcfProxy.Abort(); }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Dispose PoolItem", ex);
                    }
                }
            }
        }
    }
}
