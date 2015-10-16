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
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 商场管理员
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class MallController
    {
        private readonly ITrade _tradeStrategy;
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Com.Mall.MallController"/> class.
		/// </summary>
		/// <param name="tradeStrategy">Trade strategy.</param>
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

		/// <summary>
		/// Trades the goods.
		/// </summary>
		/// <returns><c>true</c>, if goods was traded, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="goods">Goods.</param>
		/// <param name="goodsNum">Goods number.</param>
		/// <param name="errorCode">Error code.</param>
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