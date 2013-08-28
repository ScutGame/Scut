using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 单例交易
    /// </summary>
    public class SingleTrade : ITrade
    {
        private GoodsData _goods;
        private readonly int _timeOut;

        public SingleTrade(int timeOut = 1000)
        {
            _timeOut = timeOut;
        }

        public bool TryEnterTraded(GoodsData goods)
        {
            if (goods == null)
            {
                throw new ArgumentNullException("goods");
            }
            _goods = goods;
            return Monitor.TryEnter(_goods, _timeOut);
        }

        public void ExitTraded()
        {
            Monitor.Exit(_goods);
        }

    }
}
