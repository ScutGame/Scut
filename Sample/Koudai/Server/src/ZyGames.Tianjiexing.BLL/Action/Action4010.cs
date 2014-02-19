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
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4010_扫荡取消接口
    /// </summary>
    public class Action4010 : BaseAction
    {
        private int plotID;


        public Action4010(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4010, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref plotID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cacheSet = new GameDataCacheSet<UserQueue>();
            List<UserQueue> userQueueList = cacheSet.FindAll(ContextUser.UserID, m => m.QueueType == QueueType.SaoDang);

            foreach (UserQueue queue in userQueueList)
            {
                if (queue != null)
                {
                    int npcCount = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == plotID).Count;
                    //战斗次数
                    int totalBattleNum = (int)Math.Floor((double)(queue.TotalColdTime - queue.DoRefresh()) / PlotInfo.BattleSpeedNum);
                    int turnsNum = totalBattleNum / npcCount;
                    int battleNum = totalBattleNum % npcCount;
                    //没发生战斗或上未通关都返还精力
                    //if (turnsNum == 0 || battleNum > 0)
                    //{
                    //    ContextUser.EnergyNum = ContextUser.EnergyNum.Addition(PlotInfo.BattleEnergyNum, short.MaxValue);
                    //}
                    cacheSet.Delete(queue);
                }
            }

            if (ContextUser.UserStatus == UserStatus.SaoDang)
            {
                ContextUser.ResetSweepPool(0);
                ContextUser.UserStatus = UserStatus.Normal;
                //ContextUser.Update();
            }
            return true;
        }
    }
}