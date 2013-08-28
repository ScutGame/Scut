using System.Collections.Generic;
using System.Threading;
using System.Web.Caching;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Rank
{
    /// <summary>
    /// 排行榜工厂
    /// </summary>
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
                if (reason == CacheItemRemovedReason.Expired)
                {
                    if (_running == 0)
                    {
                        Interlocked.Exchange(ref _running, 1);
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
