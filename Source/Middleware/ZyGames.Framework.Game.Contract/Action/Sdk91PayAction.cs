using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract.Action
{
    public class Sdk91PayAction : BaseStruct
    {
        private string OrderID = string.Empty;
        private int gameID = 0;
        private int serviceID = 0;
        private string passportId = string.Empty;
        private string servicename = string.Empty;
        private string _RetailID = "0000";

        public Sdk91PayAction(short aActionId, HttpGet httpGet)
            : base(aActionId, httpGet)
        {
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
                httpGet.GetString("RetailID", ref _RetailID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            SaveLog(string.Format("91SKD充值>>Order:{0},Pid:{1},servicename:{2}", OrderID, passportId, servicename));
            //PaymentService.Get91Payment(gameID, serviceID, passportId, servicename, OrderID);

            PayManager.get91PayInfo(gameID, serviceID, passportId, servicename, OrderID, _RetailID);
            return true;
        }
    }
}
