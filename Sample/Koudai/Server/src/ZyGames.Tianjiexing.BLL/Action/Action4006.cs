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
using System.Collections.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;

using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 4006_扫荡界面接口
    /// </summary>
    public class Action4006 : BaseAction
    {
        private int plotID;
        private short userItemCount = 0;
        private short npcCount = 0;
        private int coldTime = 0;
        private int turnsNum;
        private int timesNum;
        private PlotInfo plotInfo;
        private Dictionary<int, MonsterInfo> monsterDict = new Dictionary<int, MonsterInfo>();
        private Dictionary<int, int> monsterNumDict = new Dictionary<int, int>();

        public Action4006(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4006, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(plotInfo.PlotName.ToNotNullString());
            PushIntoStack((short)MathUtils.Subtraction(ContextUser.GridNum, userItemCount, 0));
            PushIntoStack(npcCount);
            PushIntoStack(PlotInfo.BattleSpeedNum);
            PushIntoStack(coldTime);
            PushIntoStack(turnsNum);
            PushIntoStack(timesNum);

            PushIntoStack(monsterDict.Count);
            foreach (KeyValuePair<int, MonsterInfo> item in monsterDict)
            {
                int monsterNum = monsterNumDict.ContainsKey(item.Key) ? monsterNumDict[item.Key] : 0;
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(item.Key);
                ds.PushIntoStack((short)monsterNum);
                ds.PushIntoStack(item.Value.GeneralName);
                ds.PushIntoStack((short)item.Value.GeneralLv);
                PushIntoStack(ds);
            }

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
            List<PlotNPCInfo> plotNPCInfoList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == plotID);
            npcCount = (short)plotNPCInfoList.Count;
            var cacheSet = new GameDataCacheSet<UserQueue>();
            List<UserQueue> userQueueList = cacheSet.FindAll(ContextUser.UserID, m => m.QueueType == QueueType.SaoDang);
            if (userQueueList.Count > 0)
            {
                UserQueue userQueue = userQueueList[0];
                coldTime = userQueue.DoRefresh();
                if (coldTime == 0)
                {
                    cacheSet.Delete(userQueue);
                    ContextUser.ResetSweepPool(0);
                    ContextUser.UserStatus = UserStatus.Normal;
                    //ContextUser.Update();
                }
                int tempNum = PlotHelper.GetBattleNum(userQueue.GetTiming()) + 1;
                if (npcCount > 0)
                {
                    turnsNum = PlotHelper.GetTurnsNum(npcCount, tempNum);
                    timesNum = PlotHelper.GetTimesNum(npcCount, tempNum);
                }
                //PlotHelper.CheckSweepCount(npcCount, userQueue.GetTiming(), out turnsNum, out battleNum);

                //if (battleNum >= npcCount)
                //{
                //    turnsNum = turnsNum + 1;
                //}
                //else
                //{
                //    battleNum = battleNum + 1;
                //}
            }
            userItemCount = (short)UserItemHelper.GetItems(Uid).FindAll(m => m.ItemStatus == ItemStatus.BeiBao).Count;
            plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
            if (plotInfo == null)
            {
                return false;
            }
            foreach (PlotNPCInfo npcInfo in plotNPCInfoList)
            {
                if (npcInfo == null)
                {
                    continue;
                }
                List<PlotEmbattleInfo> embattleInfoList = new ConfigCacheSet<PlotEmbattleInfo>().FindAll(m => m.PlotNpcID == npcInfo.PlotNpcID);
                foreach (PlotEmbattleInfo embattleInfo in embattleInfoList)
                {
                    if (monsterDict.ContainsKey(embattleInfo.MonsterID))
                    {
                        if (monsterNumDict.ContainsKey(embattleInfo.MonsterID))
                        {
                            monsterNumDict[embattleInfo.MonsterID] = monsterNumDict[embattleInfo.MonsterID] + 1;
                        }
                    }
                    else
                    {
                        MonsterInfo monsterInfo = new ConfigCacheSet<MonsterInfo>().FindKey(embattleInfo.MonsterID);
                        if (monsterInfo != null)
                        {
                            monsterDict.Add(embattleInfo.MonsterID, monsterInfo);
                            monsterNumDict.Add(embattleInfo.MonsterID, 1);
                        }
                    }
                }
            }
            return true;
        }
    }
}