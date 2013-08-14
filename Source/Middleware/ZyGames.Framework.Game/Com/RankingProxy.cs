using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Game.Com.Rank;

namespace ZyGames.Framework.Game.Com
{
    /// <summary>
    /// 排行榜操作代理类
    /// </summary>
    public class RankingProxy : ComProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ranking"></param>
        public virtual void Register(IRanking ranking)
        {
            RankingFactory.Add(ranking);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Ranking<T> Get<T>(string key) where T : RankingItem
        {
            return RankingFactory.Get<T>(key);
        }
    }
}
