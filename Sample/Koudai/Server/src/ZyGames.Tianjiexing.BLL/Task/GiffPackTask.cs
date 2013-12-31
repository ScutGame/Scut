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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Task
{
    /// <summary>
    /// 每天检查派送礼包
    /// </summary>
    public class GiffPackTask : BaseTask
    {
        private BaseLog _log = new BaseLog();

        public GiffPackTask()
            : base(60000)
        {
            TaskName = "GiffPackTask";
            Timing = "00:00:00";
        }

        protected override void DoExecute(object obj)
        {
            int packType = 0;
            new GameDataCacheSet<UserRecharge>().Foreach((personalId, key, recharge) =>
            {
                if (!IsCurrentWeek(recharge.ChargeDate) && recharge.WeekNum > 0)
                {
                    packType = 3;
                    TriggerReceivePack(recharge.UserID, packType, recharge.WeekNum);
                    recharge.WeekNum = 0;
                    //recharge.Update();
                    _log.SaveDebugLog("上周礼包派送userID：" + recharge.UserID + ",上周累计金额：" + recharge.WeekNum);
                }
                if (recharge.ChargeDate.Month != DateTime.Now.Month && recharge.MonthNum > 0)
                {
                    packType = 4;
                    TriggerReceivePack(recharge.UserID, packType, recharge.MonthNum);
                    recharge.MonthNum = 0;
                    //recharge.Update();
                    _log.SaveDebugLog("上月礼包派送userID：" + recharge.UserID + ",上月累计金额：" + recharge.WeekNum);
                }
                return true;
            });
        }

        /// <summary>
        /// 触发充值礼包
        /// </summary>
        public static void TriggerReceivePack(string userID, int packType, int gameCoin)
        {
            PackageReceive receive = new PackageReceive();
            List<RechargePacks> rechargePackseArray = new ConfigCacheSet<RechargePacks>().FindAll(m => m.PacksType == packType);
            foreach (RechargePacks rechargePackse in rechargePackseArray)
            {
                if (rechargePackse.RechargeNum <= gameCoin)
                {
                    receive.ReceiveID = Guid.NewGuid().ToString();
                    receive.PacksID = rechargePackse.PacksID;
                    receive.UserID = userID;
                    receive.IsReceive = false;
                    receive.ReceiveDate = DateTime.Now;
                    new GameDataCacheSet<PackageReceive>().Add(receive);
                }
            }
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
    }
}