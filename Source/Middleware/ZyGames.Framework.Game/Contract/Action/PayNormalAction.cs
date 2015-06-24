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
using System.Text;
using System.Xml;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Configuration;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
    /// <summary>
    /// 充值通用接口
    /// </summary>
    public class PayNormalAction : BaseStruct
    {
        /// <summary>
        /// The _amount.
        /// </summary>
        protected int _amount;
        /// <summary>
        /// The _game I.
        /// </summary>
        protected int _gameID;
        /// <summary>
        /// The _server I.
        /// </summary>
        protected int _serverID;
        /// <summary>
        /// The name of the _server.
        /// </summary>
        protected string _serverName;
        /// <summary>
        /// The _order no.
        /// </summary>
        protected string _orderNo;
        /// <summary>
        /// The _passport I.
        /// </summary>
        protected string _passportID;
        /// <summary>
        /// The _currency.
        /// </summary>
        protected string _currency;
        /// <summary>
        /// The _retail I.
        /// </summary>
        protected string _retailID;
        /// <summary>
        /// The _device I.
        /// </summary>
        protected string _deviceID;
        /// <summary>
        /// The _product I.
        /// </summary>
        protected string _productID;
        /// <summary>
        /// The type of the _pay.
        /// </summary>
        protected string _payType;
        /// <summary>
        /// The name of the _game.
        /// </summary>
        protected string _gameName;
        /// <summary>
        /// The _pay status.
        /// </summary>
        protected int _payStatus;
        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Action.PayNormalAction"/> class.
        /// </summary>
        /// <param name="aActionId">A action identifier.</param>
        /// <param name="httpGet">Http get.</param>
        public PayNormalAction(short aActionId, ActionGetter httpGet)
            : base(aActionId, httpGet)
        {
        }
        /// <summary>
        /// 创建返回协议内容输出栈
        /// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(_payStatus);
        }
        /// <summary>
        /// 接收用户请求的参数，并根据相应类进行检测
        /// </summary>
        /// <returns></returns>
        public override bool GetUrlElement()
        {
            if (actionGetter.GetInt("GameID", ref _gameID)
                && actionGetter.GetInt("ServerID", ref _serverID)
                && actionGetter.GetString("ServerName", ref _serverName)
                && actionGetter.GetString("OrderNo", ref _orderNo)
                && actionGetter.GetString("PassportID", ref _passportID)
                && actionGetter.GetString("PayType", ref _payType))
            {
                if (!actionGetter.GetString("Currency", ref _currency))
                {
                    _currency = "CNY";
                }
                if (!actionGetter.GetString("RetailID", ref _retailID))
                {
                    _retailID = _payType;
                }
                actionGetter.GetString("DeviceID", ref _deviceID);
                actionGetter.GetString("ProductID", ref _productID);
                actionGetter.GetInt("Amount", ref _amount);
                actionGetter.GetString("GameName", ref _gameName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 子类实现Action处理
        /// </summary>
        /// <returns></returns>
        public override bool TakeAction()
        {
            OrderInfo orderInfo = new OrderInfo();
            orderInfo.OrderNO = _orderNo;
            orderInfo.MerchandiseName = _productID;
            orderInfo.Currency = _currency;
            orderInfo.Amount = _amount;
            orderInfo.PassportID = _passportID;
            orderInfo.RetailID = _retailID;
            orderInfo.PayStatus = 1;
            orderInfo.GameID = _gameID;
            orderInfo.ServerID = _serverID;
            orderInfo.GameName = _gameName;
            orderInfo.ServerName = _serverName;
            orderInfo.GameCoins = (_amount * 10).ToInt();
            orderInfo.SendState = 1;
            orderInfo.PayType = _payType;
            orderInfo.Signature = "000000";
            orderInfo.DeviceID = _deviceID;

            if (PayManager.AddOrder(orderInfo))
            {
                DoSuccess(orderInfo);
                _payStatus = orderInfo.PayStatus;
                TraceLog.ReleaseWriteFatal(string.Format("PayNormal Order:{0},pid:{1} successfully!", _orderNo, _passportID));
                return true;
            }
            ErrorCode = Language.Instance.ErrorCode;
            ErrorInfo = Language.Instance.AppStorePayError;
            TraceLog.ReleaseWriteFatal(string.Format("PayNormal Order:{0},pid:{1} faild!", _orderNo, _passportID));
            return false;
        }

        private void DoSuccess(OrderInfo orderInfo)
        {
            //移动MM SDK
            Check10086Payment(orderInfo);
        }

        private void Check10086Payment(OrderInfo orderInfo)
        {
            try
            {
                string url = "http://ospd.mmarket.com:8089/trust";
                string appId = "";
                string version = "1.0.0";
                int orderType = 1;
                GameChannel gameChannel = ZyGameBaseConfigManager.GameSetting.GetChannelSetting(ChannelType.channel10086);
                if (gameChannel != null)
                {
                    url = gameChannel.Url;
                    version = gameChannel.Version;
                    orderType = gameChannel.CType.ToInt();
                    GameSdkSetting setting = gameChannel.GetSetting(orderInfo.PayType);
                    if (setting != null)
                    {
                        appId = setting.AppId;
                    }
                    else
                    {
                        return;
                    }
                }
                StringBuilder paramData = new StringBuilder();
                paramData.Append("<?xml version=\"1.0\"?>");
                paramData.AppendFormat("<Trusted2ServQueryReq>");
                paramData.AppendFormat("<MsgType>{0}</MsgType>", "Trusted2ServQueryReq");
                paramData.AppendFormat("<Version>{0}</Version>", version);
                paramData.AppendFormat("<AppID>{0}</AppID>", appId);
                paramData.AppendFormat("<OrderID>{0}</OrderID>", orderInfo.OrderNO);
                paramData.AppendFormat("<OrderType>{0}</OrderType>", orderType);
                paramData.AppendFormat("</Trusted2ServQueryReq>");

                var stream = HttpUtils.Post(url, paramData.ToString(), Encoding.UTF8, HttpUtils.XmlContentType);
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);
                TraceLog.ReleaseWriteFatal("10068 order:{0} response:{1}", orderInfo.OrderNO, doc.InnerXml);
                var returnCode = doc.SelectSingleNode("Trusted2ServQueryResp/ReturnCode");
                if (returnCode != null && !string.IsNullOrEmpty(returnCode.InnerText))
                {
                    int code = returnCode.InnerText.ToInt();
                    if (code == 0)
                    {
                        string orderNo = "";
                        var orderIDNode = doc.SelectSingleNode("Trusted2ServQueryResp/OrderID");
                        if (orderIDNode != null)
                        {
                            orderNo = orderIDNode.InnerText;
                        }
                        var priceNode = doc.SelectSingleNode("Trusted2ServQueryResp/TotalPrice");
                        if (priceNode != null)
                        {
                            decimal amount = priceNode.InnerText.ToDecimal();
                            orderInfo.Amount = amount;
                            orderInfo.GameCoins = (int)amount * 10;
                        }
                        PayManager.PaySuccess(orderNo, orderInfo);
                    }
                    TraceLog.ReleaseWriteFatal("10086 payment order:{0} fail code:{1}", orderInfo.OrderNO, code);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("10086 payment error:", ex);
            }
        }
    }
}