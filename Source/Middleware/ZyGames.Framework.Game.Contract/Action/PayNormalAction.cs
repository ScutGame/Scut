using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns.Section;

namespace ZyGames.Framework.Game.Contract.Action
{
    /// <summary>
    /// 充值通用接口
    /// </summary>
    public class PayNormalAction : BaseStruct
    {
        protected int _amount;
        protected int _gameID;
        protected int _serverID;
        protected string _serverName;
        protected string _orderNo;
        protected string _passportID;
        protected string _currency;
        protected string _retailID;
        protected string _deviceID;
        protected string _productID;
        protected string _payType;
        protected string _gameName;
        protected int _payStatus;

        public PayNormalAction(short aActionId, HttpGet httpGet)
            : base(aActionId, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(_payStatus);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GameID", ref _gameID)
                && httpGet.GetInt("ServerID", ref _serverID)
                && httpGet.GetString("ServerName", ref _serverName)
                && httpGet.GetString("OrderNo", ref _orderNo)
                && httpGet.GetString("PassportID", ref _passportID)
                && httpGet.GetString("PayType", ref _payType))
            {
                if (!httpGet.GetString("Currency", ref _currency))
                {
                    _currency = "CNY";
                }
                if (!httpGet.GetString("RetailID", ref _retailID))
                {
                    _retailID = _payType;
                }
                httpGet.GetString("DeviceID", ref _deviceID);
                httpGet.GetString("ProductID", ref _productID);
                httpGet.GetInt("Amount", ref _amount);
                httpGet.GetString("GameName", ref _gameName);
                return true;
            }
            return false;
        }

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
            ErrorCode = LanguageHelper.GetLang().ErrorCode;
            ErrorInfo = LanguageHelper.GetLang().AppStorePayError;
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
            string url = "http://ospd.mmarket.com:8089/trust";
            string appId = "";
            string version = "1.0.0";
            int orderType = 1;
            var section = SdkSectionFactory.Section10086;
            if (section != null && !string.IsNullOrEmpty(orderInfo.PayType))
            {
                url = section.Url;
                version = section.Version;
                orderType = section.OrderType;
                Channel10086Element channel10086 = section.Channels[orderInfo.PayType];
                if (channel10086 != null)
                {
                    appId = channel10086.AppId;
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
            HttpRequest request = new HttpRequest();
            request.Post(HttpRequest.ContentTypeXml, url, paramData.ToString(), stream =>
            {
                try
                {
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
                    var doc = request.ToXml(stream);
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
                            return PayManager.PaySuccess(orderNo, orderInfo);
                        }
                        TraceLog.ReleaseWriteFatal("10086 payment order:{0} fail code:{1}", orderInfo.OrderNO, code);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("10086 payment error:", ex);
                }
                return false;
            });
        }
    }
}