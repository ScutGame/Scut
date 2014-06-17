using System;
using ZyGames.Doudizhu.Bll.Com.Share;
using ZyGames.Doudizhu.Bll.Logic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Sns;
using ZyGames.Doudizhu.Model;


namespace ZyGames.Doudizhu.Bll.Base
{
    delegate void Payment91Trigger(Paymentfo paymentfo);

    public sealed class PaymentService
    {
        /// <summary>
        /// 获取个人元宝
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void Trigger(GameUser user)
        {
            new DdzPaymentNotify().Notify(user);
        }

        /// <summary>
        /// 是否本周时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsCurrentWeek(DateTime dateTime)
        {
            DateTime currDt = DateTime.Now.Date;
            int currWeek = currDt.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)currDt.DayOfWeek;
            var fromDate = currDt.AddDays((int)DayOfWeek.Monday - currWeek);
            var toDate = fromDate.AddDays(7);
            if (fromDate <= dateTime.Date && toDate > dateTime.Date)
            {
                return true;
            }
            return false;
        }

        public static void Get91Payment(int game, int server, string account, string servicename, string orderno)
        {
            var trigger = new Payment91Trigger(Get91Payment);
            trigger.BeginInvoke(new Paymentfo
            {
                game = game,
                server = server,
                account = account,
                servicename = servicename,
                orderno = orderno
            }, null, null);
        }

        public static void GetAppStore(int game, int server, string account, int silver, int Amount, string orderno, string deviceId)
        {
            var trigger = new Payment91Trigger(GetAppStore);
            trigger.BeginInvoke(new Paymentfo
            {
                game = game,
                server = server,
                account = account,
                silver = silver,
                orderno = orderno,
                deviceId = deviceId,
                Amount = Amount,
            }, null, null);
        }

        private static void Get91Payment(Paymentfo paymentfo)
        {
            PayManager.get91PayInfo(paymentfo.game, paymentfo.server, paymentfo.account, paymentfo.servicename, paymentfo.orderno, getRetailID(paymentfo.account));
        }

        private static void GetAppStore(Paymentfo paymentfo)
        {
            PayManager.AppStorePay(paymentfo.game, paymentfo.server, paymentfo.account, paymentfo.silver, paymentfo.Amount, paymentfo.orderno, getRetailID(paymentfo.account), paymentfo.deviceId);
            //PayManager.AppStorePay(paymentfo.game, paymentfo.server, paymentfo.account, paymentfo.silver, paymentfo.orderno, getRetailID(paymentfo.account), paymentfo.deviceId);
        }

        public static string getRetailID(string passportID)
        {
            SnsUser user = SnsManager.GetUserInfo(passportID);
            return user == null ? "0000" : user.RetailID;
        }

        public static void AddAndRoidOrder(Paymentfo payinfo, string RetailID)
        {
            PayManager.AddOrderInfo(payinfo.orderno, Convert.ToDecimal(payinfo.Amount), payinfo.account, payinfo.server, payinfo.game, payinfo.silver, payinfo.deviceId, RetailID);
        }
    }

    public class Paymentfo
    {
        public int game { get; set; }

        public int server { get; set; }

        public string account { get; set; }

        public int silver { get; set; }

        public int Amount { get; set; }

        public string servicename { get; set; }

        public string orderno { get; set; }

        public string deviceId
        {
            get;
            set;
        }

    }
}
