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
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Framework.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheSection : ConfigSection
    {
        /// <summary>
        /// 
        /// </summary>
        public CacheSection()
        {
            UpdateInterval = ConfigUtils.GetSetting("Cache.update.interval", 600); //10 Minute
            ExpiredInterval = ConfigUtils.GetSetting("Cache.expired.interval", 600);
            IsStorageToDb = ConfigUtils.GetSetting("Cache.IsStorageToDb", false);
            SerializerType = ConfigUtils.GetSetting("Cache.Serializer", "Protobuf");
            ShareExpirePeriod = ConfigUtils.GetSetting("Cache.global.period", 3 * 86400); //72 hour
            PersonalExpirePeriod = ConfigUtils.GetSetting("Cache.user.period", 86400); //24 hour
        }

        /// <summary>
        /// The cache expiry interval.
        /// </summary>
        public int ExpiredInterval { get; set; }

        /// <summary>
        /// The cache update interval.
        /// </summary>
        public int UpdateInterval { get; set; }


        /// <summary>
        /// Redis data is storage to Db.
        /// </summary>
        public bool IsStorageToDb { get; set; }

        /// <summary>
        /// cache serialize to redis's type, protobuf or json
        /// </summary>
        public string SerializerType { get; set; }

        /// <summary>
        /// Personal cache expire period, default 24h
        /// </summary>
        public int PersonalExpirePeriod { get; set; }
        /// <summary>
        /// cache expire period
        /// </summary>
        public int ShareExpirePeriod { get; set; }

    }
}
