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
using System.Text;
using System.Net;
using System.IO;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// AppStore充值
    /// </summary>
    public class Action1066 : BaseStruct
    {
        private string _orderInfo = string.Empty;
        private int _gameID = 0;
        private int _serviceID = 0;
        private string _passportId = "";
        private string _servicename = string.Empty;
        private string _deviceId = "";
        private string AppUrl = "https://buy.itunes.apple.com/verifyReceipt";

        public Action1066(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1066, httpGet)
        {
        }

        public override bool TakeAction()
        {
            TraceLog.ReleaseWriteFatal(_orderInfo);
            //string AppUrl = ConfigUtils.GetSetting("AppStoreUrl");
            if (AppUrl == string.Empty || _orderInfo.IndexOf("Sandbox") > 0)
            {
                AppUrl = "https://sandbox.itunes.apple.com/verifyReceipt";
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(AppUrl);
            req.Method = "POST";
            byte[] ReceiptData = Encoding.UTF8.GetBytes(_orderInfo);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("receipt-data", Convert.ToBase64String(ReceiptData));
            byte[] content = Encoding.UTF8.GetBytes(JsonUtils.Serialize(dict));
            req.ContentLength = content.Length;
            Stream stream = req.GetRequestStream();
            stream.Write(content, 0, content.Length);
            stream.Close();
            WebResponse resp = req.GetResponse();
            stream = resp.GetResponseStream();

            StreamReader reader = new StreamReader(stream);
            string response = reader.ReadToEnd();
            req.Abort();
            resp.Close();
            TraceLog.ReleaseWriteFatal(response);

            AppStoreInfo appStoreInfo = new AppStoreInfo();
            try
            {
                appStoreInfo = JsonUtils.Deserialize<AppStoreInfo>(response);
            }
            catch (Exception ex)
            {
                this.SaveLog(ex);
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().LoadDataError;
                return false;
            }
            if (appStoreInfo.status != 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1066_PayError;
                return false;
            }
           //int silver = AppStoreHelper.GetSilverPiece(appStoreInfo.receipt.product_id, _gameID);
            //PaymentService.GetAppStore(_gameID, _serviceID, _passportId, silver, appStoreInfo.receipt.transaction_id, _deviceId);

            AppStoreHelper appStore = AppStoreHelper.GetSilverPiece(appStoreInfo.receipt.product_id, _gameID);
            PaymentService.GetAppStore(_gameID, _serviceID, _passportId, appStore.SilverPiece, appStore.RMB, appStoreInfo.receipt.transaction_id, _deviceId);
           // TraceLog.ReleaseWriteFatal("appstore完成");
            //TraceLog.ReleaseWriteFatal("GameID" + _gameID + "ServerID" + _serviceID);

            //int silver = 10;
            //string test = "1234567894561111111";
            //PaymentService.GetAppStore(_gameID, _serviceID, _passportId, silver, test, _deviceId);
            return true;
        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("OrderInfo", ref _orderInfo)
                && httpGet.GetInt("gameID", ref _gameID)
                && httpGet.GetInt("Server", ref _serviceID)
                && httpGet.GetString("ServiceName", ref _servicename)
                && httpGet.GetString("PassportID", ref _passportId))
            {
                httpGet.GetString("mac", ref _deviceId);
                return true;
            }
            return false;
        }

    }

    public class AppStoreInfo
    {
        public int status
        {
            get;
            set;
        }
        public Receipt receipt = new Receipt();

    }

    public class Receipt
    {
        public string unique_identifier
        {
            get;
            set;
        }
        public string original_purchase_date_pst
        {
            get;
            set;
        }
        public string purchase_date_ms
        {
            get;
            set;
        }
        public string original_transaction_id
        {
            get;
            set;
        }
        public string original_purchase_date_ms
        {
            get;
            set;
        }
        public string transaction_id   //交易的标识;订单号000000038265962
        {
            get;
            set;
        }
        public string quantity  //购买商品的数量。
        {
            get;
            set;
        }
        public string bvrs //iPhone程序的bundle标识 1.1
        {
            get;
            set;
        }
        public string hosted_iap_version
        {
            get;
            set;
        }
        public string product_id  //商品的标识com.36you.by0001
        {
            get;
            set;
        }
        public string purchase_date //2012-04-01 05:31:01 Etc/GMT
        {
            get;
            set;
        }
        public string original_purchase_date  //对于恢复的transaction对象，该键对应了原始的交易日期
        {
            get;
            set;
        }
        public string purchase_date_pst //   2012-03-31 22:31:01 America/Los_Angeles
        {
            get;
            set;
        }
        public string bid  //iPhone程序的bundle标识
        {
            get;
            set;
        }
        public string item_id  //
        {
            get;
            set;
        }


        //public string quantity;//     购买商品的数量。对应SKPayment对象中的quantity属性
        //public string product_id;//    商品的标识，对应SKPayment对象的productIdentifier属性。
        //public string transaction_id;//         交易的标识，对应SKPaymentTransaction的transactionIdentifier属性
        //public string purchase_date_ms;//    交易的日期，对应SKPaymentTransaction的transactionDate属性
        //public string original_transaction_id;//    对于恢复的transaction对象，该键对应了原始的transaction标识
        //public string original_purchase_date_pst;
        //public string original_purchase_date;//     对于恢复的transaction对象，该键对应了原始的交易日期
        //public string app_item_id;//     App Store用来标识程序的字符串。一个服务器可能需要支持多个server的支付功能，可以用这个标识来区分程序。链接sandbox用来测试的程序的不到这个值，因此该键不存在。
        //public string version_external;// _-identifier    用来标识程序修订数。该键在sandbox环境下不存在
        //public string bid;//    iPhone程序的bundle标识
        //public string bvrs;//    iPhone程序的版本号
    }

}