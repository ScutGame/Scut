using System;

namespace ZyGames.Framework.Game.Pay
{
    /// <summary>
    /// 订单信息表
    /// </summary>
    [Serializable]
    public class OrderInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNO
        {
            set;
            get;
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string MerchandiseName
        {
            set;
            get;
        }
        /// <summary>
        /// 支付ID
        /// </summary>
        public string PayType
        {
            set;
            get;
        }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Amount
        {
            set;
            get;
        }
        /// <summary>
        /// 币种
        /// </summary>
        public string Currency
        {
            set;
            get;
        }
        /// <summary>
        /// 买家支付宝账号
        /// </summary>
        public string Expand
        {
            set;
            get;
        }
        /// <summary>
        /// 串行号
        /// </summary>
        public string SerialNumber
        {
            set;
            get;
        }
        /// <summary>
        /// 帐号
        /// </summary>
        public string PassportID
        {
            set;
            get;
        }
        /// <summary>
        /// 分服器ID
        /// </summary>
        public int ServerID
        {
            set;
            get;
        }
        /// <summary>
        /// 游戏ID
        /// </summary>
        public int GameID
        {
            set;
            get;
        }
        /// <summary>
        /// 游戏名
        /// </summary>
        public string GameName
        {
            set;
            get;
        }
        /// <summary>
        /// 分服器名
        /// </summary>
        public string ServerName
        {
            set;
            get;
        }
        /// <summary>
        /// 支付状态(1:为未支付,2:完成)
        /// </summary>
        public int PayStatus
        {
            set;
            get;
        }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature
        {
            set;
            get;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            set;
            get;
        }
        /// <summary>
        /// 游戏币
        /// </summary>
        public int GameCoins
        {
            set;
            get;
        }
        /// <summary>
        /// 同步分服传送标识,1:未下发,2：已下发
        /// </summary>
        public int SendState
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            set;
            get;
        }
        /// <summary>
        /// 下发时间
        /// </summary>
        public DateTime SendDate
        {
            set;
            get;
        }

        /// <summary>
        /// 渠道商ID
        /// </summary>
        public string RetailID
        {
            set;
            get;
        }

        /// <summary>
        /// 用户mac地址
        /// </summary>
        public string DeviceID
        {
            get;
            set;
        }
    }
}
