using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.Redis
{
    /// <summary>
    /// Server redis info
    /// </summary>
    public class RedisInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public RedisInfo()
        {
            SlaveSet = new Dictionary<string, RedisInfo>();
        }
        /// <summary>
        /// Server info hash
        /// </summary>
        public string HashCode { get; set; }

        /// <summary>
        /// Redis client version
        /// </summary>
        public int ClientVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SerializerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StarTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, RedisInfo> SlaveSet { get; set; }

    }
}
