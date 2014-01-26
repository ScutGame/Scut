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
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// The cache setting info.
    /// </summary>
    public class CacheSetting
    {
        private static readonly int CacheUpdateInterval;
        private static readonly int CacheExpiredInterval;

        static CacheSetting()
        {
            CacheUpdateInterval = ConfigUtils.GetSetting("Cache.update.interval", 600); //10 Minute
            CacheExpiredInterval = ConfigUtils.GetSetting("Cache.expired.interval", 600);
        }
        /// <summary>
        /// The cache setting init.
        /// </summary>
        public CacheSetting()
        {
            AutoRunEvent = true;
            UpdateInterval = CacheUpdateInterval;
            ExpiredInterval = CacheExpiredInterval;
        }

        /// <summary>
        /// is auto run listen event.
        /// </summary>
        public bool AutoRunEvent { get; set; }

        /// <summary>
        /// The cache expiry interval.
        /// </summary>
        public int ExpiredInterval { get; set; }

        /// <summary>
        /// The cache update interval.
        /// </summary>
        public int UpdateInterval { get; set; }

        /// <summary>
        /// The entity has be changed event notify.
        /// </summary>
        public event EntityChangedNotifyEvent ChangedHandle;

        internal void OnChangedNotify(AbstractEntity sender, CacheItemEventArgs e)
        {
            if(ChangedHandle != null)
            {
                ChangedHandle(sender, e);
            }
        }
    }
}