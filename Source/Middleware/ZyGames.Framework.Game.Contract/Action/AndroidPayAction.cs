using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
    /// <summary>
    /// 触控安卓充值
    /// </summary>
    public abstract class AndroidPayAction : AuthorizeAction
    {
        private string orderno = string.Empty;
        private int _gameID = 0;
        private int _serviceID = 0;
        private string _deviceId = string.Empty;
        private string amount = string.Empty;
        private string gamecoins = string.Empty;
        private string _passportId = "";
        private string _RetailID;

        public AndroidPayAction(short actionID, HttpGet httpGet)
            : base(actionID, httpGet)
        {
        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            TraceLog.ReleaseWriteFatal("PayInfo---error");
            if (httpGet.GetInt("gameID", ref _gameID)
                && httpGet.GetInt("ServerID", ref _serviceID)
                && httpGet.GetString("amount", ref amount)
                && httpGet.GetString("gameconis", ref gamecoins)
                && httpGet.GetString("orderno", ref orderno)
                && httpGet.GetString("PassportID", ref _passportId))
            {
                httpGet.GetString("RetailID", ref _RetailID);
                httpGet.GetString("deviceId", ref _deviceId);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            decimal Amount = amount.ToDecimal();
            int silver = (Amount * (decimal)6.5).ToInt();
            PayManager.AddOrderInfo(orderno, Amount, _passportId, _serviceID, _gameID, silver, _deviceId, _RetailID);
            return true;
        }
    }
}
