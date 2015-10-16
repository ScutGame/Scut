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

using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Config;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// The cache setting info.
    /// </summary>
    public class CacheSetting
    {

        private CacheSection _cacheConfig;

        /// <summary>
        /// The cache setting init.
        /// </summary>
        public CacheSetting()
        {
            AutoRunEvent = true;
            _cacheConfig = ConfigManager.Configger.GetFirstOrAddConfig<CacheSection>();
        }

        /// <summary>
        /// is auto run listen event.
        /// </summary>
        public bool AutoRunEvent { get; set; }

        /// <summary>
        /// The cache expiry interval.
        /// </summary>
        public int ExpiredInterval
        {
            get { return _cacheConfig.ExpiredInterval; }
            set { _cacheConfig.ExpiredInterval = value; }
        }

        /// <summary>
        /// The cache update interval.
        /// </summary>
        public int UpdateInterval
        {
            get { return _cacheConfig.UpdateInterval; }
            set { _cacheConfig.UpdateInterval = value; }
        }

        /// <summary>
        /// Redis data is storage to Db.
        /// </summary>
        public bool IsStorageToDb
        {
            get { return _cacheConfig.IsStorageToDb; }
            set { _cacheConfig.IsStorageToDb = value; }
        }

        /// <summary>
        /// The entity has be changed event notify.
        /// </summary>
        public event EntityChangedNotifyEvent ChangedHandle;

        internal void OnChangedNotify(AbstractEntity sender, CacheItemEventArgs e)
        {
            if (ChangedHandle != null)
            {
                ChangedHandle(sender, e);
            }
        }
    }
}