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

using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Config;
using ZyGames.Framework.Game.Message;

namespace ZyGames.Framework.Game.Cache
{
    /// <summary>
    /// 广播缓存
    /// </summary>
    public class BroadcastCacheSet : ShareCacheStruct<NoticeMessage>
    {
        private readonly string _groupKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Cache.BroadcastCacheSet"/> class.
		/// </summary>
        public BroadcastCacheSet()
            : this("10000")
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Cache.BroadcastCacheSet"/> class.
		/// </summary>
		/// <param name="groupKey">Group key.</param>
        public BroadcastCacheSet(string groupKey)
        {
            _groupKey = groupKey;
        }
		/// <summary>
		/// Gets the broadcast.
		/// </summary>
		/// <returns>The broadcast.</returns>
        public NoticeMessage[] GetBroadcast()
        {
            CacheQueue<NoticeMessage> cacheQueue;
            if (DataContainer.TryGetQueue(_groupKey, out cacheQueue))
            {
                return cacheQueue.ToArray();
            }
            return new NoticeMessage[0];
        }
		/// <summary>
		/// Send the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
        public void Send(NoticeMessage message)
        {
            if (DataContainer.TryAddQueue(_groupKey, message, 0, OnExpired))
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="periodTime"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        protected override bool InitCache(List<NoticeMessage> dataList, int periodTime, bool isReplace)
        {
            bool result = false;
            foreach (var data in dataList)
            {
                string key = data.GetKeyCode();
                result = DataContainer.TryAddQueue(key, data, periodTime, OnExpired);
                if (!result)
                {
                    TraceLog.WriteError("Load data:\"{0}\" tryadd key:\"{1}\" error.", DataContainer.RootKey, key);
                    return false;
                }
            }
            return result;
        }

        private static bool OnExpired(string groupKey, CacheQueue<NoticeMessage> messageQueue)
        {
            var section = ConfigManager.Configger.GetFirstOrAddConfig<MiddlewareSection>();
            while (messageQueue != null && messageQueue.Count > 0)
            {
                NoticeMessage msg;
                if (messageQueue.TryPeek(out msg))
                {
                    if (msg != null && MathUtils.DiffDate(msg.SendDate).TotalSeconds > section.BroadcastTimeout)
                    {
                        NoticeMessage temp;
                        messageQueue.TryDequeue(out temp);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return true;
        }
       
    }
}