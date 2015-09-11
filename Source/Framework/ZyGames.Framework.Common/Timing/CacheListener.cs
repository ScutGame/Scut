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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Caching;
using System.Threading;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// Cachey removed reason.
    /// </summary>
    public enum CacheRemovedReason
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,
        /// <summary>
        /// The expired.
        /// </summary>
        Expired = 1,
        /// <summary>
        /// The append.
        /// </summary>
        Append,
        /// <summary>
        /// The changed.
        /// </summary>
        Changed,
        /// <summary>
        /// The removed.
        /// </summary>
        Removed,
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="reason"></param>
    public delegate void CacheListenerHandle(string key, object value, CacheRemovedReason reason);

    /// <summary>
    /// 缓存定时监听器,
    /// 适用场合：刷新的时间间隔并不要求精确
    /// </summary>
    public class CacheListener
    {
        private static ObjectCache _cacheListener;
        private readonly string _cacheKey;
        private readonly int _expireTime;
        private readonly CacheListenerHandle _listenerHandle;
        private readonly string _dependency;
        private CacheListenerHandle _callback;
        private Timer _dueThread;
        private int isRunning = 0;

        static CacheListener()
        {
            //_cacheListener = HttpRuntime.Cache;
            _cacheListener = MemoryCache.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        public static ObjectCache Items
        {
            get { return _cacheListener; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="secondsTimeOut"></param>
        /// <param name="listenerHandle"></param>
        public CacheListener(string cacheKey, int secondsTimeOut, CacheListenerHandle listenerHandle)
            : this(cacheKey, secondsTimeOut, listenerHandle, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="secondsTimeOut">0:不过期</param>
        /// <param name="listenerHandle">过期回调方法</param>
        /// <param name="dependency">缓存文件依赖</param>
        public CacheListener(string cacheKey, int secondsTimeOut, CacheListenerHandle listenerHandle, string dependency)
        {
            DueTime = 100;
            _cacheKey = cacheKey;
            _expireTime = secondsTimeOut > 0 ? secondsTimeOut : 0;
            _listenerHandle = listenerHandle;
            _dependency = dependency;
            _callback += OnRemoveCallback;
        }

        /// <summary>
        /// 延迟执行时间，单位毫秒
        /// </summary>
        public int DueTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRunning { get { return isRunning == 1; } }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _dueThread = new Timer(OnDueFirstRun, null, DueTime, Timeout.Infinite);
            Interlocked.Exchange(ref isRunning, 1);
        }

        private void OnDueFirstRun(object obj)
        {
            try
            {
                //First run
                if (_listenerHandle != null)
                {
                    try
                    {
                        _listenerHandle(_cacheKey, true, CacheRemovedReason.Append);
                    }
                    catch (Exception er)
                    {
                        TraceLog.WriteError("cache listener callback {0} error:{1}", _cacheKey, er);
                    }
                }

                CreateCacheItem();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnDueFirstRun {0} error:{1}", _cacheKey, ex);
            }
            finally
            {
                try
                {
                    _dueThread.Dispose();
                }
                catch (Exception)
                {
                }
            }
        }

        private void CreateCacheItem()
        {
            if (_cacheListener[_cacheKey] != null)
            {
                return;
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = _expireTime == 0 ? DateTime.MaxValue : DateTime.Now.AddSeconds(_expireTime);
            policy.RemovedCallback += new CacheEntryRemovedCallback(OnCacheEntryRemoved);
            if (!string.IsNullOrEmpty(_dependency) && File.Exists(_dependency))
            {
                List<string> paths = new List<string>();
                paths.Add(_dependency);
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(paths));
            }
            _cacheListener.Set(_cacheKey, true, policy);
        }

        private void OnCacheEntryRemoved(CacheEntryRemovedArguments arg)
        {
            string key = arg.CacheItem.Key;
            CacheRemovedReason reason = CacheRemovedReason.None;
            switch (arg.RemovedReason)
            {
                case CacheEntryRemovedReason.Removed:
                    reason = CacheRemovedReason.Removed;
                    break;
                case CacheEntryRemovedReason.Expired:
                    reason = CacheRemovedReason.Expired;
                    break;
                case CacheEntryRemovedReason.ChangeMonitorChanged:
                    reason = CacheRemovedReason.Changed;
                    break;
                case CacheEntryRemovedReason.Evicted:
                case CacheEntryRemovedReason.CacheSpecificEviction:
                    break;
                default:
                    break;
            }
            this._callback.BeginInvoke(key, arg.CacheItem.Value, reason, EndCallback, null);
        }

        private void EndCallback(IAsyncResult ar)
        {
        }

        private void OnRemoveCallback(string key, object value, CacheRemovedReason reason)
        {
            try
            {
                if (key == _cacheKey)
                {
                    if (_listenerHandle != null)
                    {
                        _listenerHandle(key, value, reason);
                    }
                    if (value != null && value.ToBool())
                    {
                        CreateCacheItem();
                    }
                    else
                    {
                        TraceLog.ReleaseWrite("Cache listener expire key:\"{0}\",reason:{1} is stoped.", key, reason);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Cache listener expire key:\"{0}\",reason:{1},error:{2}", key, reason, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            Interlocked.Exchange(ref isRunning, 0);
            if (_cacheListener[_cacheKey] != null)
                return;
            _cacheListener[_cacheKey] = false;
            _cacheListener.Remove(_cacheKey);
        }
    }
}