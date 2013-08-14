using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 排行榜数据项
    /// </summary>
    public class RankingItem
    {
        /// <summary>
        /// 排名,从1开始
        /// </summary>
        public virtual int RankId
        {
            get;
            set;
        }
    }
}
