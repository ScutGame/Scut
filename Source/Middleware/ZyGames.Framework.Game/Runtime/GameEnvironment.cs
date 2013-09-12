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
