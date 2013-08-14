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
