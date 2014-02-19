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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 4008_加速或晶石扫荡接口
    /// </summary>
    public class Action4008 : BaseAction
    {
        private int plotID;
        private int ops;
        private int SweepOps;

        public Action4008(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4008, httpGet)
        {

        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref plotID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                httpGet.GetInt("SweepOps", ref SweepOps);
                return true;
            }

            return false;
        }

        public override bool TakeAction()
        {
            UserQueue userQueue = null;
            int energyNum;
            int coleTime = GetSweepColdTime(out energyNum);
            short surEnergy = MathUtils.Addition(ContextUser.EnergyNum, ContextUser.SurplusEnergy, short.MaxValue);

            if ((ContextUser.EnergyNum == 0 && ContextUser.SurplusEnergy == 0) || surEnergy < energyNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_EnergyNotEnough;
                return false;
            }
            int goldNum = GetGoldNum(out userQueue);
            if (goldNum == 0)
            {
                return false;
            }
            if (ops == 1)
            {
                //提示晶石操作
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St4008_Tip, goldNum);
            }
            else if (ops == 2)
            {
                //使用晶石确认操作
                int userGoldNum = ContextUser.GoldNum;
                if (userGoldNum < goldNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                //晶石扫荡
                if (userQueue != null)
                {
                    DoAccelerateQueue(goldNum, userQueue.DoRefresh(), userQueue.GetTiming(), userQueue.TotalColdTime);
                    //加速完清除队列
                    var queueCacheSet = new GameDataCacheSet<UserQueue>();
                    queueCacheSet.Delete(userQueue);
                    ContextUser.ResetSweepPool(0);
                    //ContextUser.Update();
                }
                else
                {
                    //清除扫荡池
                    var sweepCacheSet = new GameDataCacheSet<UserSweepPool>();
                    List<UserSweepPool> sweepPoolList = sweepCacheSet.FindAll(ContextUser.UserID);
                    foreach (UserSweepPool sweepPool in sweepPoolList)
                    {
                        if (sweepPool != null)
                        {
                            sweepCacheSet.Delete(sweepPool);
                        }
                    }
                    var itemList = UserItemHelper.GetItems(Uid).FindAll(m => m.ItemStatus == ItemStatus.BeiBao);
                    if (itemList.Count >= ContextUser.GridNum)
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St4007_BeiBaoTimeOut;
                        return false;
                    }
                    DoAccelerateQueue(goldNum, coleTime, 0, coleTime);
                }
                DateTime startDate = userQueue != null ? userQueue.Timing : DateTime.Now;
                UserLogHelper.AppenRaidsLog(ContextUser.UserID, 2, startDate, DateTime.Now, (short)GetSweepEnergy(out energyNum), goldNum);
            }
            else
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            return true;
        }

        private void DoAccelerateQueue(int goldNum, int coldTime, int timing, int totalTime)
        {
            //获取加速后的所需的精力
            int npcCount = new ConfigCacheSet<PlotNPCInfo>().FindAll(m=>m.PlotID == plotID).Count;
            int battleNum = PlotHelper.GetBattleNum(coldTime);
            int turnsNum = PlotHelper.GetTurnsNum(npcCount, battleNum);
            int energyNum = turnsNum * PlotInfo.BattleEnergyNum;
            ContextUser.RemoveEnergyNum((short)energyNum);
            ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, goldNum, int.MaxValue);
            ContextUser.UserStatus = UserStatus.Normal;
            //ContextUser.Update();

            int totalTurnsNum = PlotHelper.GetTurnsNum(npcCount, PlotHelper.GetBattleNum(totalTime));
            int tempNum = PlotHelper.GetBattleNum(timing);
            int sweepCount = PlotHelper.GetTurnsNum(npcCount, tempNum);
            int timesNum = PlotHelper.GetTimesNum(npcCount, tempNum);
            sweepCount = sweepCount == 0 ? 1 : sweepCount;

            for (int i = sweepCount; i <= totalTurnsNum; i++)
            {
                int j = 1;
                if (i == sweepCount)
                {
                    j = timesNum;
                }
                for (; j <= npcCount; j++)
                {
                    //TrumpAbilityAttack.CombatTrumpLift(ContextUser.UserID);
                    PlotHelper.DoPlotSweepPrize(ContextUser.UserID, plotID, i, j, npcCount);
                }
            }
        }

        private int GetGoldNum(out UserQueue userQueue)
        {
            userQueue = null;
            List<UserQueue> userQueueList = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType== QueueType.SaoDang);
            int coldTime = 0;
            if (userQueueList.Count > 0)
            {
                //加速正在扫荡的队列
                userQueue = userQueueList[0];
                coldTime = userQueue.DoRefresh();
            }
            else
            {
                //晶石扫荡
                int userEnergyNum;
                coldTime = GetSweepColdTime(out userEnergyNum);
            }
            return (int)Math.Ceiling((double)coldTime / PlotInfo.BattleGold);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="energyNum">精力</param>
        /// <returns>倒计时间</returns>
        private int GetSweepEnergy(out int energyNum)
        {
            energyNum = 0;
            int coldTime = 0;
            List<PlotNPCInfo> npcList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m=>m.PlotID == plotID);
            int plotNpcNum = npcList.Count;
            int battleNum;

            if (SweepOps == 1)
            {
                energyNum = MathUtils.Addition(ContextUser.EnergyNum, ContextUser.SurplusEnergy, short.MaxValue);
                energyNum = energyNum > 200 ? 200 : energyNum;
                battleNum = (energyNum / PlotInfo.BattleEnergyNum) * plotNpcNum;
                energyNum = PlotHelper.GetTurnsNum(plotNpcNum, battleNum) * PlotInfo.BattleEnergyNum;
            }
            else if (SweepOps == 2)
            {
                coldTime = 1800;
                battleNum = PlotHelper.GetBattleNum(coldTime);
                energyNum = PlotHelper.GetTurnsNum(plotNpcNum, battleNum) * PlotInfo.BattleEnergyNum;
            }
            else if (SweepOps == 3)
            {
                coldTime = 3600;
                battleNum = PlotHelper.GetBattleNum(coldTime);
                energyNum = PlotHelper.GetTurnsNum(plotNpcNum, battleNum) * PlotInfo.BattleEnergyNum;
            }

            return energyNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="energyNum">精力</param>
        /// <returns>倒计时间</returns>
        private int GetSweepColdTime(out int energyNum)
        {
            energyNum = 0;
            int coldTime = 0;
            List<PlotNPCInfo> npcList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m=>m.PlotID == plotID);
            int plotNpcNum = npcList.Count;
            int battleNum;

            if (SweepOps == 1)
            {
                energyNum = (int)MathUtils.Addition(ContextUser.EnergyNum, ContextUser.SurplusEnergy, short.MaxValue);
                //energyNum = ContextUser.EnergyNum > 200 ? 200 : ContextUser.EnergyNum;
                energyNum = energyNum > 200 ? 200 : energyNum;
                battleNum = (energyNum / PlotInfo.BattleEnergyNum) * plotNpcNum;
                coldTime = battleNum * PlotInfo.BattleSpeedNum;
            }
            else if (SweepOps == 2)
            {
                coldTime = 1800;
                battleNum = PlotHelper.GetBattleNum(coldTime);
                energyNum = PlotHelper.GetTurnsNum(plotNpcNum, battleNum) * PlotInfo.BattleEnergyNum;
            }
            else if (SweepOps == 3)
            {
                coldTime = 3600;
                battleNum = PlotHelper.GetBattleNum(coldTime);
                energyNum = PlotHelper.GetTurnsNum(plotNpcNum, battleNum) * PlotInfo.BattleEnergyNum;
            }

            return coldTime;
        }
    }
}