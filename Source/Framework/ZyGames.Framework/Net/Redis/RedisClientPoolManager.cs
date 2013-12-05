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
using ServiceStack.Common.Extensions;
using ServiceStack.Logging;
using ServiceStack.Redis;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Net.Redis
{
    /// <summary>
    /// Redis客户端连接池管理
    /// </summary>
    [Obsolete("", true)]
    public class RedisClientPoolManager
    {
        private static int poolTimeOut;
        private static int connectTimeout;
        private const int PoolSizeMultiplier = 10;
        private static PooledRedisClientManager prcm;

        static RedisClientPoolManager()
        {
            try
            {
                connectTimeout = ConfigUtils.GetSetting("Redis.ConnectTimeout", 1);
                poolTimeOut = ConfigUtils.GetSetting("Redis.PoolTimeOut", 300);
                var readWriteHosts = ConfigUtils.GetSetting("Redis.Host.ReadWrite").Split(',');
                var readOnlyHosts = ConfigUtils.GetSetting("Redis.Host.ReadOnly").Split(',');
                var password = ConfigUtils.GetConnectionString("Redis.RequirePass");
                var dbIndex = ConfigUtils.GetSetting("Redis.Db", 0);
                dbIndex = dbIndex < 0 ? 0 : dbIndex;

                readWriteHosts = ConvertHost(readWriteHosts, password);
                readOnlyHosts = ConvertHost(readOnlyHosts, password);

                var config = new RedisClientManagerConfig()
                {
                    //“写”链接池链接数
                    MaxWritePoolSize = ConfigUtils.GetSetting("Redis.WritePool.MaxSize", 200),
                    //“读”链接池链接数
                    MaxReadPoolSize = ConfigUtils.GetSetting("Redis.Read.MaxSize", 200),
                    AutoStart = true
                };
                prcm = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, config, dbIndex, PoolSizeMultiplier, poolTimeOut);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Init the redis pool manager error:{0}", ex);
            }
        }

        /// <summary>
        /// 格式:password@host:port
        /// </summary>
        /// <param name="hosts"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string[] ConvertHost(string[] hosts, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return hosts;
            }
            string[] tempList = new string[hosts.Length];
            for (int i = 0; i < tempList.Length; i++)
            {
                tempList[i] = string.Format("{0}@{1}", password, hosts[i]);
            }
            return tempList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetReadOnlyClient()
        {
            try
            {
                return prcm.GetReadOnlyClient();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Connect-readonly to redis error:{0}", ex);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient()
        {
            try
            {
                return prcm.GetClient();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Connect to redis error:{0}", ex);
            }
            return null;
        }
    }

}