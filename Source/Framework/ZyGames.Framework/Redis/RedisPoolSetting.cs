using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.Redis
{
    /// <summary>
    /// Redis Pool Setting
    /// </summary>
    public class RedisPoolSetting
    {
        /// <summary>
        /// Host, format:password@ip:port
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// ReadOnlyHost
        /// </summary>
        public string ReadOnlyHost { get; set; }
        /// <summary>
        /// MaxWritePoolSize
        /// </summary>
        public int MaxWritePoolSize { get; set; }
        /// <summary>
        /// MaxReadPoolSize
        /// </summary>
        public int MaxReadPoolSize { get; set; }
        /// <summary>
        /// ConnectTimeout(ms)
        /// </summary>
        public int ConnectTimeout { get; set; }
        /// <summary>
        /// PoolTimeOut(ms), default 2000ms
        /// </summary>
        public int PoolTimeOut { get; set; }
        /// <summary>
        /// DbIndex
        /// </summary>
        public int DbIndex { get; set; }
    }
}
