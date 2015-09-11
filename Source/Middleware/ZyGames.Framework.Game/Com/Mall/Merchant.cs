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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 商家
    /// </summary>
    [Serializable, ProtoContract]
    public class Merchant
    {
        private readonly int _merchantId;
        private readonly MallController _controller;
        private List<GoodsData> _goodsList = new List<GoodsData>();
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Com.Mall.Merchant"/> class.
		/// </summary>
		/// <param name="merchantId">Merchant identifier.</param>
		/// <param name="controller">Controller.</param>
        public Merchant(int merchantId, MallController controller)
        {
            if (controller == null)
            {
                throw new ArgumentException("controller");
            }
            _merchantId = merchantId;
            _controller = controller;
        }

        /// <summary>
        /// 初始化商品
        /// </summary>
        public void InitializeGoods()
        {
            var list = _controller.LoadGoodsData(_merchantId);
            foreach (var goods in list)
            {
                _goodsList.InsertSort(goods, _controller.SortGoods);
            }
        }

        /// <summary>
        /// 展示商品
        /// </summary>
        /// <returns></returns>
        public IList<GoodsData> ShowGoods(Predicate<GoodsData> match, int pageIndex, int pageSize, out int pageCount)
        {
            if (match == null)
            {
                return _goodsList.GetPaging(pageIndex, pageSize, out pageCount);
            }
            return _goodsList.FindAll(match).GetPaging(pageIndex, pageSize, out pageCount);
        }

        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="goodsId"></param>
        /// <param name="goodsNum"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public bool BuyGoods(BaseUser user, int goodsId, int goodsNum, out TradeErrorCode errorCode)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var goods = _goodsList.Find(m => m.Id == goodsId);
            if (goods != null)
            {
                return _controller.TradeGoods(user, goods, goodsNum, out errorCode);
            }
            errorCode = TradeErrorCode.NoGodds;
            return false;
        }
    }
}