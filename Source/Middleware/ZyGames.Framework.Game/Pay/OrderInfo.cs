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
        /// 支付状态(1:为未支付,2:完成,3:fail)
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