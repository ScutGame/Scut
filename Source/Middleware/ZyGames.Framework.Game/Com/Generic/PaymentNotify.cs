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
using ProtoBuf;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Framework.Game.Com.Generic
{
    /// <summary>
    /// 付款通知
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class PaymentNotify
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Notify(IUser user)
        {
            int gameId = GameEnvironment.ProductCode;
            int serverId = GameEnvironment.ProductServerId;
            int userId = user.GetUserId();
            string pid = user.GetPassportId();
            OrderInfo[] orderList = PayManager.getPayment(gameId, serverId, pid);
            foreach (var orderInfo in orderList)
            {
                if (DoNotify(userId, orderInfo))
                {
                    PayManager.Abnormal(orderInfo.OrderNO);
                    TraceLog.ReleaseWriteFatal("Payment order:{0},Pid:{1} notify success", orderInfo.OrderNO, pid);
                }
                else
                {
                    TraceLog.ReleaseWriteFatal("Payment order:{0},Pid:{1} notify faild", orderInfo.OrderNO, pid);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        protected abstract bool DoNotify(int userId, OrderInfo orderInfo);
    }
}