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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Model;
using ZyGames.Framework.MSMQ;

namespace ZyGames.Framework.Game.Runtime
{
    public delegate void RuntimeEventHandler();

    /// <summary>
    /// 游戏运行环境
    /// </summary>
    public static class GameEnvironment
    {
        public static readonly string PythonScriptTaskCacheKey = "__PythonScript_Task";

        private static int _isRunning;

        static GameEnvironment()
        {
            ProductDesEnKey = "BF3856AD";
            ClientDesDeKey = "SC3U9T8G";
            ProductCode = ConfigUtils.GetSetting("Product.Code").ToInt();
            ProductName = ConfigUtils.GetSetting("Product.Name");
            ProductServerId = ConfigUtils.GetSetting("Product.ServerId").ToInt();
            CacheGlobalPeriod = ConfigUtils.GetSetting("Cache.global.period", "0").ToInt(); //24小时
            CacheUserPeriod = ConfigUtils.GetSetting("Cache.user.period", "0").ToInt(); //8小时
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expiredInterval">定时清理过期缓存时间</param>
        /// <param name="updateInterval">定时更新缓存时间</param>
        public static void InitializeCache(int expiredInterval, int updateInterval)
        {
            CacheFactory.Initialize(expiredInterval, updateInterval);
        }

        /// <summary>
        /// 全局变量集合
        /// </summary>
        public static ContextCacheSet<CacheItem> Global
        {
            get;
            private set;
        }

        /// <summary>
        /// 帐户密码的8位长度Des加密密钥
        /// </summary>
        public static string ProductDesEnKey { get; set; }

        /// <summary>
        /// 客户端的8位长度Des解密密钥
        /// </summary>
        public static string ClientDesDeKey { get; set; }

        /// <summary>
        /// 全局缓存生命周期
        /// </summary>
        public static int CacheGlobalPeriod { get; set; }
        /// <summary>
        /// 玩家缓存生命周期
        /// </summary>
        public static int CacheUserPeriod { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        public static int ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public static string ProductName { get; set; }

        /// <summary>
        /// 游戏服代码
        /// </summary>
        public static int ProductServerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsRunning
        {
            get { return _isRunning == 1; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheInterval"></param>
        /// <param name="loadDataFactory"></param>
        /// <param name="expiredInterval">定时清理过期缓存时间</param>
        public static void Start(int cacheInterval, Func<bool> loadDataFactory, int expiredInterval = 600)
        {
            bool result = false;
            CacheFactory.Initialize(expiredInterval, cacheInterval);
            var pythonManager = PythonScriptManager.Current;
            //建立消息队列连接
            ActionMSMQ.Instance();
            Global = new ContextCacheSet<CacheItem>("__gameenvironment_global");

            if (loadDataFactory != null)
            {
                result = loadDataFactory();
            }
            if (result)
            {
                Interlocked.Exchange(ref _isRunning, 1);
            }
        }

        public static void Stop()
        {
            CacheFactory.UpdateNotify(true);
            CacheFactory.Dispose();
            Interlocked.Exchange(ref _isRunning, 0);
        }

    }
}