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
using System.Management.Instrumentation;
using System.Reflection;
using System.Text;
using System.Web.Caching;
using ZyGames.Framework.Cache.Generic.Pool;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存池管理者
    /// </summary>
    public static class CacheFactory
    {
        private delegate bool UpdateEvent(bool isChange);

        private const int DefInterval = 600;
        private static readonly object ListenerLock = new object();
        /// <summary>
        /// 缓存定时更新Change数据监听器
        /// </summary>
        private static CacheListener _cacheUpdateListener;
        /// <summary>
        /// 过期缓存定时清理监听器
        /// </summary>
        private static CacheListener _cacheExpiredListener;
        private static BaseCachePool _readonlyPools;
        private static BaseCachePool _writePools;
        /// <summary>
        /// 缓存Change数据更新回调委托
        /// </summary>
        private static event UpdateEvent UpdateCallbackHandle;
        private static bool _isRunning;
        /// <summary>
        /// 是否已释放
        /// </summary>
        private static int _isDisposed;


        /// <summary>
        /// 缓存初始化方法
        /// </summary>
        /// <param name="expiredInterval">缓存监听器过期时间，单位：秒</param>
        /// <param name="updateInterval">缓存数据更新间隔，单位：秒</param>
        /// <param name="autoRun">是否自动开启缓存写库监听，默认:自动开启</param>
        public static void Initialize(int expiredInterval, int updateInterval = DefInterval, bool autoRun = true)
        {
            Initialize(new DbTransponder(), new RedisTransponder(), expiredInterval, updateInterval, autoRun);
        }

        /// <summary>
        /// 缓存初始化方法
        /// </summary>
        /// <param name="dbTransponder">执行数据操作的转发器对象</param>
        /// <param name="redisTransponder">执行Redis操作的转发器对象</param>
        /// <param name="expiredInterval">缓存监听器过期时间，单位：秒</param>
        /// <param name="updateInterval">缓存数据更新间隔，单位：秒</param>
        /// <param name="autoRun">是否自动开启缓存写库监听，默认:自动开启</param>
        public static void Initialize(ITransponder dbTransponder, ITransponder redisTransponder, int expiredInterval, int updateInterval, bool autoRun)
        {
            _readonlyPools = new CachePool(dbTransponder, redisTransponder, true);
            _writePools = new CachePool(dbTransponder, redisTransponder, false);
            
            InitListener("__CachePoolListener", expiredInterval, "__CachePoolUpdateListener", updateInterval);
            if (autoRun)
            {
                StartListener();
            }
        }

        private static void InitListener(string listenerKey, int expiredInterval, string updateListentKey, int updateInterval)
        {

            lock (ListenerLock)
            {
                if (_cacheUpdateListener == null)
                {
                    _cacheUpdateListener = new CacheListener(updateListentKey, updateInterval, (key, value, reason) =>
                    {
                        try
                        {

                            if (!_isRunning)
                            {
                                _isRunning = true;
                                TraceLog.ReleaseWrite("缓存延迟更新执行开始");
                                UpdateNotify(true);
                                TraceLog.ReleaseWrite("缓存延迟更新执行结束");
                                _isRunning = false;
                            }
                            else
                            {
                                TraceLog.ReleaseWrite("缓存延迟更新正在执行中...");
                            }
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("Cache manager timing error:{0}", ex);
                        }
                    });
                }
                if (_cacheExpiredListener == null)
                {
                    _cacheExpiredListener = new CacheListener(listenerKey, expiredInterval, (key, value, reason) =>
                    {
                        if (reason == CacheItemRemovedReason.Expired)
                        {
                            try
                            {
                                _readonlyPools.DisposeCache();
                                _writePools.DisposeCache();
                                TraceLog.ReleaseWrite("清理过期缓存结束...");
                            }
                            catch (Exception ex)
                            {
                                TraceLog.WriteError("Cache manager timing error:{0}", ex);
                            }
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 启动缓存写库监听
        /// </summary>
        public static void StartListener()
        {
            if (_cacheExpiredListener != null)
            {
                _cacheExpiredListener.Start();
            }
            if (_cacheUpdateListener != null)
            {
                _cacheUpdateListener.Start();
            }
            System.Threading.Interlocked.Exchange(ref _isDisposed, 0);
            TraceLog.WriteInfo("CacheFactory listen has started...");
        }

        /// <summary>
        /// 停止缓存写库监听
        /// </summary>
        public static void StopListener()
        {
            if (_cacheExpiredListener != null)
            {
                _cacheExpiredListener.Stop();
                _cacheExpiredListener = null;
            }
            if (_cacheUpdateListener != null)
            {
                _cacheUpdateListener.Stop();
                _cacheUpdateListener = null;
            }
            System.Threading.Interlocked.Exchange(ref _isDisposed, 1);
            TraceLog.WriteInfo("CacheFactory listen has stoped");
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public static void Dispose()
        {
            StopListener();
            if (_readonlyPools != null)
            {
                _readonlyPools.Dispose();
                _readonlyPools = null;
            }
            if (_writePools != null)
            {
                _writePools.Dispose();
                _writePools = null;
            }
            UpdateCallbackHandle = null;
        }

        /// <summary>
        /// 获取缓存容器对象，不存在则创建空容器对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDataContainer<T> GetOrCreate<T>() where T : IItemChangeEvent, IDataExpired, new()
        {
            return GetOrCreate<T>(false, () => true, (key) => true);
        }

        /// <summary>
        /// 获取缓存容器对象，不存在则创建空容器对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isReadonly">内存是否是只读</param>
        /// <param name="loadFactory"></param>
        /// <param name="loadItemFactory"></param>
        /// <returns></returns>
        public static IDataContainer<T> GetOrCreate<T>(bool isReadonly, Func<bool> loadFactory, Func<string, bool> loadItemFactory) where T : IItemChangeEvent, IDataExpired, new()
        {
            if (_isDisposed == 1)
            {
                TraceLog.WriteError("Access to cache \"{0}\" data failed because the object has been disposed.", typeof(T).FullName);
            }
            EntityContainer<T> cacheSet = null;
            if (isReadonly)
            {
                cacheSet = new EntityContainer<T>(_readonlyPools, loadFactory, loadItemFactory) { IsReadonly = true };
            }
            else
            {
                cacheSet = new EntityContainer<T>(_writePools, loadFactory, loadItemFactory);
            }
            return cacheSet;
        }

        internal static void RegistUpdateNotify<T>(BaseCacheStruct<T> cacheStruct) where T : AbstractEntity, new()
        {
            UpdateCallbackHandle += isChange =>
            {
                cacheStruct.Update(isChange);
                return true;
            };
        }

        /// <summary>
        /// 注册全局共享缓存变更通知事件
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        internal static void RegistShareUpdateNotify<T>() where T : ShareEntity, new()
        {
            UpdateCallbackHandle += isChange =>
            {
                new ShareCacheStruct<T>().Update(isChange);
                return true;
            };
        }

        /// <summary>
        /// 注册私有缓存变更通知事件（如单个玩家的数据）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        internal static void RegistPersonalUpdateNotify<T>() where T : BaseEntity, new()
        {
            UpdateCallbackHandle += isChange =>
            {
                new PersonalCacheStruct<T>().Update(isChange);
                return true;
            };
        }

        /// <summary>
        /// 触发缓存数据更新
        /// </summary>
        /// <param name="isChange">更新的模式：是否仅更新有发生isChange的实例</param>
        public static void UpdateNotify(bool isChange)
        {

            if (UpdateCallbackHandle != null)
            {
                Delegate[] handleList = UpdateCallbackHandle.GetInvocationList();
                foreach (dynamic handle in handleList)
                {
                    if (handle != null)
                    {
                        handle(isChange);
                    }
                }
            }
        }

    }
}