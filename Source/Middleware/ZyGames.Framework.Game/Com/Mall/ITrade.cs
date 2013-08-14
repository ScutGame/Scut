using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 交易出错代码
    /// </summary>
    public enum TradeErrorCode
    {
        Sucess = 0,
        Fail = 1000,
        NoGodds,
        TradeTimeout,
        AmountNotEnough,
        /// <summary>
        /// 库存
        /// </summary>
        QuantityNotEnough,
        LimtNumber
    }

    /// <summary>
    /// 交易策略
    /// </summary>
    public interface ITrade
    {
        bool TryEnterTraded(GoodsData goods);
        
        void ExitTraded();
    }
}
