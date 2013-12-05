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
using System.Text;
using System.Web;
using System.Web.Caching;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="reason"></param>
    public delegate void CacheListenerHandle(string key, object value, CacheItemRemovedReason reason);

    /// <summary>
    /// 缓存定时监听器,
    /// 适用场合：刷新的时间间隔并不要求精确
    /// </summary>
    public class CacheListener
    {
        private static IMonitorStrategy _monitorStrategy;
        private static Cache _cacheListener;
        private readonly string _cacheKey;
        private readonly int _expireTime;
        private readonly CacheListenerHandle _listenerHandle;
        private readonly string _dependency;

        static CacheListener()
        {
            _monitorStrategy = new MonitorLockStrategy(3000);
            _cacheListener = HttpRuntime.Cache;
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
            _cacheKey = cacheKey;
            _expireTime = secondsTimeOut > 0 ? secondsTimeOut : 0;
            _listenerHandle = listenerHandle;
            _dependency = dependency;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (_cacheListener[_cacheKey] != null) return;
            using (var l = _monitorStrategy.Lock())
            {
                if (l != null && l.TryEnterLock())
                {
                    _cacheListener.Insert(_cacheKey, true,
                        string.IsNullOrEmpty(_dependency) ? null : new CacheDependency(_dependency),
                        _expireTime == 0 ? DateTime.MaxValue : DateTime.Now.AddSeconds(_expireTime),
                        TimeSpan.Zero,//Cache.NoSlidingExpiration,
                        CacheItemPriority.High,
                        CacheItemOnRemoved);
                }
            }
        }

        /// <summary>
        /// 缓存过期的回调函数
        /// </summary>
        /// <param name="key">缓存的名字</param>
        /// <param name="value">缓存的值</param>
        /// <param name="reason">缓存原因</param>
        protected virtual void CacheItemOnRemoved(string key, object value, CacheItemRemovedReason reason)
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
                        Start();
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
            if (_cacheListener[_cacheKey] != null) return;
            using (var l = _monitorStrategy.Lock())
            {
                if (l != null && l.TryEnterLock())
                {
                    _cacheListener[_cacheKey] = false;
                    _cacheListener.Remove(_cacheKey);
                }
            }
        }
    }
}