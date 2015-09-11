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
using System.Web.Caching;
using ProtoBuf;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Rank
{
    /// <summary>
    /// 排行榜工厂
    /// </summary>
    [Serializable, ProtoContract]
    public static class RankingFactory
    {
        private static readonly string CacheKey = "__RankingFactoryListener";
        private static CacheListener _cacheListener;
        private static int _running = 0;
        private static Dictionary<string, IRanking> _rankList = new Dictionary<string, IRanking>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secondTimeOut"></param>
        public static void Start(int secondTimeOut)
        {
            _cacheListener = new CacheListener(CacheKey, secondTimeOut, (key, value, reason) =>
            {
				if (reason == CacheRemovedReason.Expired)
                {
                    if (Interlocked.Exchange(ref _running, 1) == 0)
                    {
                        DoRefresh();
                        Interlocked.Exchange(ref _running, 0);
                    }
                }
            });

            DoRefresh();
            _cacheListener.Start();
        }
        
        private static void DoRefresh()
        {
            TraceLog.ReleaseWrite("The ranking refresh is start...");
            var er = _rankList.GetEnumerator();
            while (er.MoveNext())
            {
                if (er.Current.Value != null)
                {
                    er.Current.Value.Refresh();
                }
            }
            TraceLog.ReleaseWrite("The ranking refresh is end.");
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
            _cacheListener.Stop();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ranking"></param>
        public static void Add(IRanking ranking)
        {
            _rankList.Add(ranking.Key, ranking);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Ranking<T> Get<T>(string key) where T : RankingItem
        {
            if(_rankList.ContainsKey(key))
            {
                return _rankList[key] as Ranking<T>;
            }
            return null;
        }

    }
}