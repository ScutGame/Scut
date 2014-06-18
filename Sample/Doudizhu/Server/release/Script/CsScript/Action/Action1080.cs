using ZyGames.Doudizhu.Bll;
using ZyGames.Doudizhu.Bll.Base;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;

namespace ZyGames.Doudizhu.Script.CsScript.Action
{
    /// <summary>
    /// 触控安卓充值
    /// </summary>
    public class Action1080 : BaseAction
    {
        private string orderno = string.Empty;
        private int _gameID = 0;
        private int _serviceID = 0;
        //private string _passportId = "";
        private string _deviceId = string.Empty;
        private string amount = string.Empty;
        private string gamecoins = string.Empty;

        public Action1080(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1080, httpGet)
        {
        }

        public override bool TakeAction()
        {
            Paymentfo payinfo = new Paymentfo();
            payinfo.account = this.ContextUser.Pid;
            payinfo.Amount = amount.ToInt();
            payinfo.deviceId = _deviceId;
            payinfo.game = _gameID;
            payinfo.orderno = orderno;
            payinfo.server = _serviceID;
            payinfo.silver = (payinfo.Amount * 6.5).ToInt();
            PaymentService.AddAndRoidOrder(payinfo, ContextUser.RetailId);
            return true;
        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            TraceLog.ReleaseWriteFatal("PayInfo---error");
            if (httpGet.GetInt("gameID", ref _gameID)
                && httpGet.GetInt("ServerID", ref _serviceID)
                //&& httpGet.GetString("PassportID", ref _passportId)
                && httpGet.GetString("amount", ref amount)
                && httpGet.GetString("gameconis", ref gamecoins)
                && httpGet.GetString("orderno", ref orderno))
            {
                httpGet.GetString("deviceId", ref _deviceId);
                return true;
            }
            return false;
        }

    }


}
