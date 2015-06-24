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
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Action.AndroidPayAction"/> class.
		/// </summary>
		/// <param name="actionID">Action I.</param>
		/// <param name="httpGet">Http get.</param>
        public AndroidPayAction(short actionID, ActionGetter httpGet)
            : base(actionID, httpGet)
        {
        }
		/// <summary>
		/// 创建返回协议内容输出栈
		/// </summary>
        public override void BuildPacket()
        {
        }
		/// <summary>
		/// 接收用户请求的参数，并根据相应类进行检测
		/// </summary>
		/// <returns></returns>
        public override bool GetUrlElement()
        {
            TraceLog.ReleaseWriteFatal("PayInfo---error");
            if (actionGetter.GetInt("gameID", ref _gameID)
                && actionGetter.GetInt("ServerID", ref _serviceID)
                && actionGetter.GetString("amount", ref amount)
                && actionGetter.GetString("gameconis", ref gamecoins)
                && actionGetter.GetString("orderno", ref orderno)
                && actionGetter.GetString("PassportID", ref _passportId))
            {
                actionGetter.GetString("RetailID", ref _RetailID);
                actionGetter.GetString("deviceId", ref _deviceId);
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
            decimal Amount = amount.ToDecimal();
            int silver = (Amount * (decimal)6.5).ToInt();
            PayManager.AddOrderInfo(orderno, Amount, _passportId, _serviceID, _gameID, silver, _deviceId, _RetailID);
            return true;
        }
    }
}