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
using System.Threading;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Net.Redis
{
    /// <summary>
    /// 连接池管理
    /// </summary>
    public static class RedisConnectionPool
    {
        private static readonly object lockObject = new object();
        private static Dictionary<int, RedisConnection> _readPools = new Dictionary<int, RedisConnection>();
        private static Dictionary<int, RedisConnection> _writePools = new Dictionary<int, RedisConnection>();
        private static int _connectErrorTimes = 0;
        private static readonly int MaxErrorTimes;
        private static readonly int PoolTimeOut;
        private static readonly int ConnectTimeout;
        private static readonly int DbIndex;
        private static readonly string ReadWriteHost;
        private static readonly string ReadOnlyHost;
        private static readonly string Password;
        private static int WritePoolMaxSize;
        private static int ReadPoolMaxSize;

        static RedisConnectionPool()
        {
            MaxErrorTimes = ConfigUtils.GetSetting("Redis.MaxErrorTimes", 10);
            EnableRedis = ConfigUtils.GetSetting("Redis.Enable", false);
            ConnectTimeout = ConfigUtils.GetSetting("Redis.ConnectTimeout", 1);
            PoolTimeOut = ConfigUtils.GetSetting("Redis.PoolTimeOut", 300);
            ReadWriteHost = ConfigUtils.GetSetting("Redis.Host.ReadWrite");
            ReadOnlyHost = ConfigUtils.GetSetting("Redis.Host.ReadOnly");
            WritePoolMaxSize = ConfigUtils.GetSetting("Redis.WritePool.MaxSize", 200);
            ReadPoolMaxSize = ConfigUtils.GetSetting("Redis.Read.MaxSize", 200);
            Password = ConfigUtils.GetConnectionString("Redis.RequirePass");
            DbIndex = ConfigUtils.GetSetting("Redis.Db", 0);
            DbIndex = DbIndex < 0 ? 0 : DbIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        public static void IncreaseError()
        {
            Interlocked.Increment(ref _connectErrorTimes);
            TraceLog.ReleaseWrite("Redis connection error num:\"{0}\"!", _connectErrorTimes);
            if (_connectErrorTimes > MaxErrorTimes)
            {
                EnableRedis = false;
                TraceLog.WriteError("Redis is disabled,Connection error exceeded the maximum:\"{0}\"!", MaxErrorTimes);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool EnableRedis
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RedisConnection GetReadOnlyClient()
        {
            return GetPoolConnection(_readPools, ReadPoolMaxSize, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RedisConnection GetClient()
        {
            return GetPoolConnection(_writePools, WritePoolMaxSize, false);
        }
        private static RedisConnection CreateReadOnly(int poolId)
        {
            return Create(poolId, ReadOnlyHost, Password, true);
        }
        private static RedisConnection CreateReadWrite(int poolId)
        {
            return Create(poolId, ReadWriteHost, Password, false);
        }

        private static RedisConnection GetPoolConnection(Dictionary<int, RedisConnection> pools, int maxSize, bool isReadonly)
        {
            RedisConnection client = null;
            int poolCouont = maxSize + 1;
            for (int i = 0; i < poolCouont; i++)
            {
                if (client != null && i >= pools.Count)
                {
                    break;
                }

                if (client == null)
                {
                    RedisConnection pool = null;
                    int index = i;
                    if (!pools.ContainsKey(index))
                    {
                        lock (lockObject)
                        {
                            if (!pools.ContainsKey(index))
                            {
                                if (index >= maxSize)
                                {
                                    TraceLog.WriteError(isReadonly
                                        ? "Redis connection readonly-pool beyond the maximum:{0}"
                                        : "Redis connection pool beyond the maximum:{0}", maxSize);
                                    break;
                                }
                                pool = isReadonly ? CreateReadOnly(index) : CreateReadWrite(index);
                                pools.Add(index, pool);
                                TraceLog.ReleaseWrite("Redis host:\"{0}:{1}\" connection {2} is created.",
                                    pool.Host,
                                    pool.Port,
                                    pool.IsReadonlyPool
                                        ? "readonly-pool[" + pool.PoolId + "]"
                                        : "pool[" + pool.PoolId + "]"
                                );
                            }
                        }
                    }
                    pool = pools.ContainsKey(index) ? pools[index] : null;
                    if (pool != null && !pool.IsActive && pool.IsConnected)
                    {
                        client = pool;
                    }
                }
                else if (pools.ContainsKey(i) && !pools[i].IsActive && !pools[i].IsTimeout)
                {
                    //移除超时连接池
                    lock (lockObject)
                    {
                        if (pools.ContainsKey(i))
                        {
                            var pool = pools[i];
                            pool.Dispose();
                            TraceLog.ReleaseWrite("Redis host:\"{0}:{1}\" connection pool[{2}] timeout is disposed.", pool.Host, pool.Port, pool.PoolId);
                        }
                    }
                }
            }
            return client;
        }

        private static RedisConnection Create(int poolId, string hostConfig, string password, bool isReadonly)
        {
            string host = "";
            int port = 6379;
            if (!string.IsNullOrEmpty(hostConfig) && hostConfig.IndexOf(":") != -1)
            {
                var tempList = hostConfig.Split(':');
                if (tempList.Length > 1)
                {
                    host = tempList[0];
                    int.TryParse(tempList[1], out port);
                }
            }
            else
            {
                host = hostConfig;
            }
            return Create(poolId, host, port, password, isReadonly);
        }

        private static RedisConnection Create(int poolId, string host, int port, string password, bool isReadonly)
        {
            return new RedisConnection(host, port, password, DbIndex, PoolTimeOut, ConnectTimeout)
                       {
                           PoolId = poolId,
                           IsReadonlyPool = isReadonly
                       };
        }

    }
}