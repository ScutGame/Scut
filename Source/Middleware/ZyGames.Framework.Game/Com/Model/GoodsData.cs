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
using System.Threading;
using ProtoBuf;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 商品实体
    /// </summary>
    [Serializable, ProtoContract]
    public class GoodsData
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Com.Model.GoodsData"/> class.
		/// </summary>
        public GoodsData()
        {

        }

        /// <summary>
        /// 商家Id
        /// </summary>
        public int MerchantId
        {
            get;
            set;
        }
        /// <summary>
        /// 商品标识
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 商品类型
        /// </summary>
        public int GoodsType
        {
            get;
            set;
        }

        /// <summary>
        /// 排列位置
        /// </summary>
        public int SeqNo
        {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descption
        {
            get;
            set;
        }

        /// <summary>
        /// 商品图标
        /// </summary>
        public string HeadIcon
        {
            get;
            set;
        }

        /// <summary>
        /// 交易使用代币类型
        /// </summary>
        public int TradeType
        {
            get;
            set;
        }

        /// <summary>
        /// 出售价格
        /// </summary>
        public double SellPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 市场价格
        /// </summary>
        public double MarketPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 商品状态
        /// </summary>
        public GoodsStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 库存数量，0为限制库存
        /// </summary>
        public int Quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 限制数量
        /// </summary>
        public int LimitNumber
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 商品状态
    /// </summary>
    public enum GoodsStatus
    {
        /// <summary>
        /// 禁止出售
        /// </summary>
        Disable = 0,
        /// <summary>
        /// 出售中
        /// </summary>
        Selling,
        /// <summary>
        /// 限购
        /// </summary>
        LimitSelling

    }
}