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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
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

        public Action1080(ZyGames.Framework.Game.Contract.HttpGet httpGet)
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
            PaymentService.AddAndRoidOrder(payinfo, ContextUser.RetailID);
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