using ZyGames.Doudizhu.Bll;
using ZyGames.Doudizhu.Bll.Base;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Doudizhu.Script.CsScript.Action
{
    /// <summary>
    /// 91SDK充值
    /// </summary>
    public class Action1065 : BaseStruct
    {
        private string OrderID = string.Empty;
        private int gameID = 0;
        private int serviceID = 0;
        private string passportId = string.Empty;
        private string servicename = string.Empty;

        public Action1065(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1065, httpGet)
        {
        }

        public override bool TakeAction()
        {
            SaveLog(string.Format("91SKD充值>>Order:{0},Pid:{1},servicename:{2}", OrderID, passportId, servicename));
            PaymentService.Get91Payment(gameID, serviceID, passportId, servicename, OrderID);
            return true;
        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            TraceLog.ReleaseWriteFatal("url");
            if (httpGet.GetString("OrderID", ref OrderID)
                && httpGet.GetInt("gameID", ref gameID)
                && httpGet.GetInt("Server", ref serviceID)
                && httpGet.GetString("ServiceName", ref servicename)
                && httpGet.GetString("PassportID", ref passportId))
            {
                return true;
            }
            return false;
        }
    }

    public class SDKPayInfo
    {
        public string ConsumeStreamId { get; set; }

        public string CooOrderSerial { get; set; }

        public string MerchantId { get; set; }

        public string AppId { get; set; }

        public string ProductName { get; set; }

        public string Uin { get; set; }

        public string GoodsId { get; set; }

        public string GoodsInfo { get; set; }

        public string GoodsCount { get; set; }

        public string OriginalMoney { get; set; }

        public string OrderMoney { get; set; }

        public string Note { get; set; }

        public string PayStatus { get; set; }

        public string CreateTime { get; set; }

        public string Sign { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDesc { get; set; }
    }
}
