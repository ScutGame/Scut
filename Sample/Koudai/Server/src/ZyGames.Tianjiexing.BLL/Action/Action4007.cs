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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 4007_扫荡开始接口
    /// </summary>
    public class Action4007 : BaseAction
    {
        private const int Time_30 = 1800;
        private const int Time_60 = 3600;
        private int plotID;
        private int ops;
        private int coldTime;


        public Action4007(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4007, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(coldTime);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref plotID)
                && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            List<UserQueue> queueList = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.SaoDang);
            if (queueList.Count > 0)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St4007_Saodanging;
                return false;
            }
            var itemList = UserItemHelper.GetItems(Uid).FindAll(m => m.ItemStatus == ItemStatus.BeiBao);
            if (itemList.Count >= ContextUser.GridNum)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St4007_BeiBaoTimeOut;
                return false;
            }
            List<PlotNPCInfo> npcList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == plotID);
            //战斗次数
            int plotNpcNum = npcList.Count;
            int energyNum = 0;
            int battleNum;

            if (ops == 1)
            {
                //用尽精力，玩家可扫荡轮数
                energyNum = ContextUser.EnergyNum > 200 ? 200 : ContextUser.EnergyNum.ToInt();
                int userTurnsNum = (int)Math.Floor((double)energyNum / PlotInfo.BattleEnergyNum);
                battleNum = userTurnsNum * plotNpcNum;
                coldTime = battleNum * PlotInfo.BattleSpeedNum;
            }
            else if (ops == 2)
            {
                //30分钟
                coldTime = Time_30;
                battleNum = PlotHelper.GetBattleNum(Time_30);
                energyNum = PlotHelper.GetTurnsNum(plotNpcNum, battleNum) * PlotInfo.BattleEnergyNum;
            }
            else if (ops == 3)
            {
                //60分钟
                coldTime = Time_60;
                battleNum = PlotHelper.GetBattleNum(Time_60);
                energyNum = PlotHelper.GetTurnsNum(plotNpcNum, battleNum) * PlotInfo.BattleEnergyNum;
            }
            else
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (ContextUser.EnergyNum == 0 || ContextUser.EnergyNum < energyNum)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St_EnergyNotEnough;
                return false;
            }
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

            if (coldTime > 0)
            {
                //在开始战斗时扣一次玩家精力,无战斗离开返还
                ContextUser.ResetSweepPool(plotID);
                //开始扫荡暂不扣精力
                //ContextUser.RemoveEnergyNum(PlotInfo.BattleEnergyNum);
                ContextUser.UserStatus = UserStatus.SaoDang;
                //ContextUser.Update();

                //放入队列中                    
                UserQueue userQueue = new UserQueue()
                {
                    QueueID = Guid.NewGuid().ToString(),
                    UserID = ContextUser.UserID,
                    QueueType = QueueType.SaoDang,
                    QueueName = QueueType.SaoDang.ToString(),
                    TotalColdTime = coldTime,
                    Timing = DateTime.Now,
                    ColdTime = coldTime,
                    IsSuspend = false
                };
                new GameDataCacheSet<UserQueue>().Add(userQueue);

            }
            return true;
        }
    }
}