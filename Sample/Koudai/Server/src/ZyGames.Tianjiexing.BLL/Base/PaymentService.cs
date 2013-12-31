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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Base
{
    delegate bool PaymentTrigger(int game, int server, string account, string userID);

    delegate void Payment91Trigger(Paymentfo paymentfo);

    public class PaymentService
    {
        /// <summary>
        /// 获取个人元宝
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void Trigger(GameUser user)
        {
            var trigger = new PaymentTrigger(GetPayment);
            trigger.BeginInvoke(user.GameId, user.ServerId, user.Pid, user.UserID, null, null);
            int vipLv;
            int vipGold = MathUtils.Addition(user.PayGold, user.ExtGold, int.MaxValue);
            List<VipLvInfo> vipLvArray = new ConfigCacheSet<VipLvInfo>().FindAll(u => u.PayGold <= vipGold);
            vipLv = vipLvArray.Count > 0 ? vipLvArray[vipLvArray.Count - 1].VipLv : (short)0;
            user.VipLv = vipLv;
            //user.Update();
        }

        private static bool GetPayment(int game, int server, string account, string userID)
        {
            try
            {
                GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
                if (userInfo == null) return false;
                var chatService = new TjxChatService();
                OrderInfo[] model = PayManager.getPayment(game, server, account);
                foreach (OrderInfo order in model)
                {
                    userInfo.PayGold = MathUtils.Addition(userInfo.PayGold, order.GameCoins, int.MaxValue);
                    //userInfo.Update();
                    PayManager.Abnormal(order.OrderNO);

                    DialHelper.ReturnRatioGold(userID, order.GameCoins);  //大转盘抽奖奖励充值返还
                    chatService.SystemSendWhisper(userInfo, string.Format(LanguageManager.GetLang().PaySuccessMsg, order.GameCoins));

                    DoGiff(userID, order);
                    FestivalHelper.GetPayReward(userInfo, order.GameCoins, FestivalType.PayReward);
                }
                return true;
            }
            catch (Exception ex)
            {
                BaseLog log = new BaseLog("PaymentLog");
                log.SaveLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 处理礼包
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="order"></param>
        private static void DoGiff(string userID, OrderInfo order)
        {
            int packType = 0;
            List<PackageReceive> packageReceivess = new List<PackageReceive>();
            UserRechargeLog rechargeLog = new UserRechargeLog
            {
                LogID = Guid.NewGuid().ToString(),
                UserID = userID,
                OrderNo = order.OrderNO,
                ChargeAmount = order.Amount,
                PayGold = order.GameCoins,
                ReargeDate = DateTime.Now

            };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(rechargeLog);

            UserRecharge recharge = new GameDataCacheSet<UserRecharge>().FindKey(userID);
            if (recharge == null)
            {
                recharge = new UserRecharge() { UserID = userID };
            }
            //处理充值活动
            recharge.LastNum = order.GameCoins;
            TriggerFestivalPack(recharge, order.GameCoins);
            if (IsCurrentWeek(recharge.ChargeDate))
            {
                recharge.WeekNum = MathUtils.Addition(recharge.WeekNum, order.GameCoins, int.MaxValue);
            }
            else
            {
                recharge.WeekNum = order.GameCoins;
            }
            if (recharge.ChargeDate.Month == DateTime.Now.Month)
            {
                recharge.MonthNum = MathUtils.Addition(recharge.MonthNum, order.GameCoins, int.MaxValue);
            }
            else
            {
                recharge.MonthNum = order.GameCoins;
            }

            recharge.TotalGoldNum = MathUtils.Addition(recharge.TotalGoldNum, order.GameCoins, int.MaxValue);
            recharge.ChargeDate = DateTime.Now;
            var rechargeCacheSet = new GameDataCacheSet<UserRecharge>();
            if (rechargeCacheSet.FindKey(userID) == null)
            {
                packType = 1;
                recharge.FirstNum = order.GameCoins;
                TriggerReceivePack(userID, packType, recharge.FirstNum);
                rechargeCacheSet.Add(recharge);
                FestivalHelper.GetFirstReward(userID, FestivalType.FirstReward);
                GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
                if (userInfo != null)
                {
                    FestivalHelper.GetPayReward(userInfo, order.GameCoins, FestivalType.FirstPayDoubleSpar);
                }
            }

            //触发总累计充值礼包
            packType = 2;
            TriggerReceivePack(userID, packType, recharge.TotalGoldNum);
            List<RechargePacks> rechargePackseArray = new ConfigCacheSet<RechargePacks>().FindAll(m => m.PacksType == packType);
            foreach (RechargePacks rechargePackse in rechargePackseArray)
            {
                RemoveCharge(userID, rechargePackse.PacksID);
            }
            TriggerWeekPack(userID, recharge.WeekNum); //每周礼包
            TriggerMonthPack(userID, recharge.MonthNum); //每月礼包
        }

        /// <summary>
        /// 累计充值活动
        /// </summary>
        private static void TriggerFestivalPack(UserRecharge userRecharge, int GameCoins)
        {
            FestivalInfo fest = FestivalHelper.GetInfo(FestivalType.PayCount);
            if (fest == null)
            {
                return;
            }
            if (userRecharge.FestivalDate == null || userRecharge.FestivalDate < fest.StartDate)
            {
                userRecharge.FestivalDate = fest.StartDate;
                userRecharge.FestivalCount = GameCoins;
            }
            else
            {
                userRecharge.FestivalCount += GameCoins;
            }
        }

        /// <summary>
        /// 触发充值礼包
        /// </summary>
        public static void TriggerReceivePack(string userID, int packType, int gameCoin)
        {
            List<RechargePacks> rechargePackseArray = new ConfigCacheSet<RechargePacks>().FindAll(m => m.PacksType == packType);
            foreach (RechargePacks rechargePackse in rechargePackseArray)
            {
                List<PackageReceive> packageReceivess = new GameDataCacheSet<PackageReceive>().FindAll(userID, m => m.PacksID == rechargePackse.PacksID);
                if (packageReceivess.Count == 0)
                {
                    if (rechargePackse.RechargeNum <= gameCoin)
                    {
                        PackageReceive receive = new PackageReceive();
                        receive.ReceiveID = Guid.NewGuid().ToString();
                        receive.PacksID = rechargePackse.PacksID;
                        receive.UserID = userID;
                        receive.IsReceive = false;
                        receive.ReceiveDate = DateTime.Now;
                        new GameDataCacheSet<PackageReceive>().Add(receive);
                    }
                }
            }
        }

        public static void TriggerWeekPack(string userID, int gameCoin)
        {
            int packType = 3;
            List<RechargePacks> rechargePackseArray = new ConfigCacheSet<RechargePacks>().FindAll(m => m.PacksType == packType);
            foreach (RechargePacks rechargePackse in rechargePackseArray)
            {
                if (rechargePackse.RechargeNum <= gameCoin)
                {
                    var packCacheSet = new GameDataCacheSet<PackageReceive>();
                    List<PackageReceive> packageReceivess = packCacheSet.FindAll(userID, m => m.PacksID == rechargePackse.PacksID);
                    if (packageReceivess.Count == 0)
                    {
                        PackageReceive receive = new PackageReceive();
                        receive.ReceiveID = Guid.NewGuid().ToString();
                        receive.PacksID = rechargePackse.PacksID;
                        receive.UserID = userID;
                        receive.IsReceive = false;
                        receive.ReceiveDate = DateTime.Now;
                        packCacheSet.Add(receive);
                    }
                    else if (!IsHaveWeek(packageReceivess))
                    {
                        PackageReceive receive = packageReceivess[0];

                        if (receive.IsReceive)
                        {
                            receive.IsReceive = false;
                            receive.ReceiveDate = DateTime.Now;
                            //receive.Update();
                        }
                        for (int i = 1; i <= packageReceivess.Count - 1; i++)
                        {
                            packCacheSet.Delete(packageReceivess[i]);
                        }
                    }
                }
            }
        }

        public static void TriggerMonthPack(string userID, int gameCoin)
        {
            int packType = 4;
            List<RechargePacks> rechargePackseArray = new ConfigCacheSet<RechargePacks>().FindAll(m => m.PacksType == packType);
            foreach (RechargePacks rechargePackse in rechargePackseArray)
            {
                if (rechargePackse.RechargeNum <= gameCoin)
                {
                    List<PackageReceive> packageReceivess = new GameDataCacheSet<PackageReceive>().FindAll(userID, m => m.PacksID == rechargePackse.PacksID);
                    if (packageReceivess.Count == 0)
                    {
                        PackageReceive receive = new PackageReceive();
                        receive.ReceiveID = Guid.NewGuid().ToString();
                        receive.PacksID = rechargePackse.PacksID;
                        receive.UserID = userID;
                        receive.IsReceive = false;
                        receive.ReceiveDate = DateTime.Now;
                        new GameDataCacheSet<PackageReceive>().Add(receive);
                    }
                    else if (!IsHaveMonth(packageReceivess))
                    {
                        PackageReceive receive = packageReceivess[0];

                        if (receive.IsReceive)
                        {
                            receive.IsReceive = false;
                            receive.ReceiveDate = DateTime.Now;
                            //receive.Update();
                        }


                        var cacheSet = new GameDataCacheSet<PackageReceive>();
                        for (int i = 1; i <= packageReceivess.Count - 1; i++)
                        {
                            cacheSet.Delete(packageReceivess[i]);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 删除累计充值礼包多个，只余一个
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="packID"></param>
        /// <returns></returns>
        public static void RemoveCharge(string userID, int packID)
        {
            List<PackageReceive> packageReceiveArray = new GameDataCacheSet<PackageReceive>().FindAll(userID, m => m.PacksID == packID);
            if (packageReceiveArray.Count > 1)
            {
                var packCacheSet = new GameDataCacheSet<PackageReceive>();
                packageReceiveArray = packCacheSet.FindAll(userID, u => u.IsReceive && u.PacksID == packID);
                if (packageReceiveArray.Count > 0)
                {
                    packageReceiveArray = packCacheSet.FindAll(userID, u => !u.IsReceive && u.PacksID == packID);
                    foreach (PackageReceive packageReceive in packageReceiveArray)
                    {
                        packCacheSet.Delete(packageReceive);
                    }
                }
                else
                {
                    packageReceiveArray = packCacheSet.FindAll(userID, u => !u.IsReceive && u.PacksID == packID);
                    for (int i = 1; i <= packageReceiveArray.Count - 1; i++)
                    {
                        packCacheSet.Delete(packageReceiveArray[i]);
                    }
                }
            }
        }


        /// <summary>
        /// 是否本月时间
        /// </summary>
        /// <param name="packageReceivesArray"></param>
        /// <returns></returns>
        public static bool IsHaveMonth(List<PackageReceive> packageReceivesArray)
        {
            foreach (PackageReceive packageReceive in packageReceivesArray)
            {
                if (packageReceive.ReceiveDate.Month == DateTime.Now.Month)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 列表是否有本周时间的数据
        /// </summary>
        /// <param name="packageReceivesArray"></param>
        /// <returns></returns>
        private static bool IsHaveWeek(List<PackageReceive> packageReceivesArray)
        {
            foreach (PackageReceive packageReceive in packageReceivesArray)
            {
                if (IsCurrentWeek(packageReceive.ReceiveDate))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 本周数据是否大于11
        /// </summary>
        /// <param name="packageReceivesArray"></param>
        /// <returns></returns>
        public static bool IsCurrentWeekTwo(List<PackageReceive> packageReceivesArray)
        {
            int num = 0;
            foreach (PackageReceive packageReceive in packageReceivesArray)
            {
                if (IsCurrentWeek(packageReceive.ReceiveDate))
                {
                    num = MathUtils.Addition(num, 1, int.MaxValue);
                }
            }
            if (num > 1)
            {
                return true;
            }
            return false;
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