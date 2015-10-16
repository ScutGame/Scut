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

namespace ZyGames.Framework.SyncThreading
{
    ///<summary>
    ///</summary>
    public delegate bool SyncLimit();

    ///<summary>
    ///</summary>
    public static class SyncHelper
    {
        ///<summary>
        ///</summary>
        public const string FmtLockKey = "{0}_{1}";
        private const int TimeOut = 3000;
        private static Dictionary<string, object> _syncRootKeys = new Dictionary<string, object>();
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// 同步lock锁
        /// </summary>
        /// <param name="syncRoot"></param>
        /// <param name="action"></param>
        public static void SyncFun(object syncRoot, Action action)
        {
            SyncFun(syncRoot, () => true, action);
        }

        /// <summary>
        /// 同步lock锁
        /// </summary>
        /// <param name="syncRoot"></param>
        /// <param name="syncLimit">进入锁内条件</param>
        /// <param name="action"></param>
        public static void SyncFun(object syncRoot, SyncLimit syncLimit, Action action)
        {
            if (syncLimit == null || syncRoot == null || action == null)
            {
                return;
            }
            if (syncLimit())
            {
                lock (syncRoot)
                {
                    if (syncLimit())
                    {
                        action();
                    }
                }
            }
        }

        /// <summary>
        /// 同步Monitor锁
        /// </summary>
        /// <param name="syncRoot"></param>
        /// <param name="msTimeOut"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool TrySyncFun(object syncRoot, int msTimeOut, Action action)
        {
            if (action == null || syncRoot == null || action == null)
            {
                return false;
            }

            object obj;
            if (TryGetLock(obj = syncRoot, msTimeOut))
            {
                try
                {
                    action();
                }
                finally
                {
                    ExitLock(obj);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 同步Monitor锁
        /// </summary>
        /// <param name="syncRoot"></param>
        public static void GetLock(object syncRoot)
        {
            if (syncRoot != null) Monitor.Enter(syncRoot);
        }

        /// <summary>
        /// 同步Monitor锁
        /// </summary>
        /// <param name="syncRoot"></param>
        /// <param name="msTimeOut"></param>
        /// <returns></returns>
        public static bool TryGetLock(object syncRoot, int msTimeOut)
        {
            return syncRoot != null && Monitor.TryEnter(syncRoot, msTimeOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncRoot"></param>
        public static void ExitLock(object syncRoot)
        {
            Monitor.Exit(syncRoot);
        }

        /// <summary>
        /// 获取锁字符串Key同步Monitor锁
        /// </summary>
        /// <param name="lock_key"></param>
        /// <returns></returns>
        public static bool LockKey(string lock_key)
        {
            if (string.IsNullOrEmpty(lock_key))
            {
                return false;
            }
            SyncFun(_syncRoot, () => !_syncRootKeys.ContainsKey(lock_key), () =>
            {
                object syncRoot = null;
                Interlocked.CompareExchange(ref syncRoot, new object(), null);
                _syncRootKeys.Add(lock_key, syncRoot);
            });

            if (_syncRootKeys.ContainsKey(lock_key))
            {
                return TryGetLock(_syncRootKeys[lock_key], TimeOut);
            }
            return false;
        }

        /// <summary>
        /// 退出锁字符串Key同步Monitor锁
        /// </summary>
        /// <param name="lock_key"></param>
        public static void UnLockKey(string lock_key)
        {
            if (string.IsNullOrEmpty(lock_key))
            {
                return;
            }
            if (_syncRootKeys.ContainsKey(lock_key))
            {
                Monitor.Exit(_syncRootKeys[lock_key]);
            }
        }

        /// <summary>
        /// 提供原子操作值类型增加
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int TryIncrement(ref int value)
        {
            return Interlocked.Increment(ref value);
        }

        /// <summary>
        /// 提供原子操作值类型增加
        /// </summary>
        /// <param name="value"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static int TryIncrement(ref int value, int increment)
        {
            return Interlocked.Exchange(ref value, value + increment);
        }

        /// <summary>
        /// 提供原子操作值类型减少
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int TryDecrement(ref int value)
        {
            return Interlocked.Decrement(ref value);
        }

        /// <summary>
        /// 提供原子操作值类型减少
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static int TryDecrement(ref int value, int decrement)
        {
            return Interlocked.Exchange(ref value, value - decrement);
        }
    }
}