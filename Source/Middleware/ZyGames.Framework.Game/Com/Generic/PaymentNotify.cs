using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Framework.Game.Com.Generic
{
    /// <summary>
    /// 付款通知
    /// </summary>
    public abstract class PaymentNotify
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Notify(BaseUser user)
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
