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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Redis;

namespace ZyGames.Framework.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisSection : ConfigSection
    {
        /// <summary>
        /// 
        /// </summary>
        public RedisSection()
            : this(true)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public RedisSection(bool useConfig)
        {
            if (useConfig)
            {
                Host = ConfigUtils.GetSetting("Redis.Host", "localhost");
                MaxWritePoolSize = ConfigUtils.GetSetting("Redis.MaxWritePoolSize", 100);
                MaxReadPoolSize = ConfigUtils.GetSetting("Redis.MaxReadPoolSize", 100);
                ConnectTimeout = ConfigUtils.GetSetting("Redis.ConnectTimeout", 0);
                PoolTimeOut = ConfigUtils.GetSetting("Redis.PoolTimeOut", 300);
                DbIndex = ConfigUtils.GetSetting("Redis.Db", 0);
                ReadOnlyHost = ConfigUtils.GetSetting("Redis.ReadHost", Host);
                ClientVersion = ConfigUtils.GetSetting("Redis.ClientVersion", (int)RedisStorageVersion.Hash).ToEnum<RedisStorageVersion>();
            }
            else
            {
                ClientVersion = RedisStorageVersion.Hash;
            }
        }


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
        /// Pool timeout release time(s), default 300s
        /// </summary>
        public int PoolTimeOut { get; set; }
        /// <summary>
        /// DbIndex
        /// </summary>
        public int DbIndex { get; set; }

        /// <summary>
        /// ver: 0 is old versin
        /// </summary>
        public RedisStorageVersion ClientVersion { get; set; }
    }
}
