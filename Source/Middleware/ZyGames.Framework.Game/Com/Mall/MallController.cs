using System;
using System.Collections.Generic;
using System.Linq;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 商场管理员
    /// </summary>
    public abstract class MallController
    {
        private readonly ITrade _tradeStrategy;

        protected MallController(ITrade tradeStrategy)
        {
            _tradeStrategy = tradeStrategy;
        }

        /// <summary>
        /// 加载商品数据
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public abstract IList<GoodsData> LoadGoodsData(int merchantId);

        /// <summary>
        /// 商品排序规则
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual int SortGoods(GoodsData x, GoodsData y)
        {
            int result = 0;
            if (x == null && y == null)
            {
                return 0;
            }
            if (x != null && y == null)
            {
                return 1;
            }
            if (x == null)
            {
                return -1;
            }
            result = x.SeqNo.CompareTo(y.SeqNo);
            if (result == 0)
            {
                x.Id.CompareTo(y.Id);
            }
            return result;
        }


        public bool TradeGoods(BaseUser user, GoodsData goods, int goodsNum, out TradeErrorCode errorCode)
        {
            bool result = false;
            if (_tradeStrategy.TryEnterTraded(goods))
            {
                try
                {
                    double payAmount;
                    if (HasTraded(user, goods, goodsNum, out payAmount, out errorCode))
                    {
                        var items = ProduceGoods(goods, goodsNum);
                        if (SendGoods(user, items) && PayAmount(user, goods.TradeType, payAmount))
                        {
                            TradeSuccess(user, goods, goodsNum, payAmount, items);
                            errorCode = TradeErrorCode.Sucess;
                            result = true;
                        }
                        else
                        {
                            errorCode = TradeErrorCode.Fail;
                        }
                    }
                }
                finally
                {
                    _tradeStrategy.ExitTraded();
                }
            }
            else
            {
                errorCode = TradeErrorCode.TradeTimeout;
            }
            return result;
        }

        /// <summary>
        /// 交易成功
        /// </summary>
        /// <param name="user"></param>
        /// <param name="goods"></param>
        /// <param name="goodsNum"></param>
        /// <param name="payAmount"></param>
        /// <param name="items"></param>
        protected virtual void TradeSuccess(BaseUser user, GoodsData goods, int goodsNum, double payAmount, object items)
        {

        }

        /// <summary>
        /// 生产物品
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="goodsNum"></param>
        /// <returns></returns>
        protected abstract object ProduceGoods(GoodsData goods, int goodsNum);

        /// <summary>
        /// 检查是否可交易
        /// </summary>
        /// <param name="user"></param>
        /// <param name="goods"></param>
        /// <param name="goodsNum"></param>
        /// <param name="amount"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        protected abstract bool HasTraded(BaseUser user, GoodsData goods, int goodsNum, out double amount, out TradeErrorCode errorCode);

        /// <summary>
        /// 支付代币
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tradeType"></param>
        /// <param name="payAmount"></param>
        /// <returns></returns>
        protected abstract bool PayAmount(BaseUser user, int tradeType, double payAmount);

        /// <summary>
        /// 派送商品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected abstract bool SendGoods(BaseUser user, object items);


    }
}
