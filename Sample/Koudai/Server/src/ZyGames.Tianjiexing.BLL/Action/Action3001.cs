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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3001_剧情任务列表接口
    /// 注：交付任务后，下发可接的任务到UserTask表，并移除已交付的任务到Log表中
    /// </summary>
    public class Action3001 : BaseAction
    {
        private TaskType TaskType;
        private List<UserTask> userTaskList = new List<UserTask>();

        public Action3001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3001, httpGet)
        {

        }

        public override void BuildPacket()
        {
            int rankID = 0;
            this.PushIntoStack(userTaskList.Count);
            foreach (UserTask userTask in userTaskList)
            {
                SaveLog("任务ID:" + userTask.TaskID + ", 任务排行:" + MathUtils.Addition(rankID, 1));
                StoryTaskInfo taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(userTask.TaskID);
                CityNpcInfo reNpcInfo = null;
                CityNpcInfo deNpcInfo = null;
                PlotInfo plotInfo = null;
                PlotNPCInfo npcInfo = null;
                ItemBaseInfo itemInfo = null;
                string[] monsterList = new string[0];
                string[] monsterNumList = new string[0];
                string[] monsterCurrNumList = new string[0];
                if (taskInfo != null)
                {
                    reNpcInfo = new ConfigCacheSet<CityNpcInfo>().FindKey(taskInfo.ReleaseNpcID);
                    deNpcInfo = new ConfigCacheSet<CityNpcInfo>().FindKey(taskInfo.DeliveryNpcID);
                    plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(taskInfo.PlotID);
                    npcInfo = new ConfigCacheSet<PlotNPCInfo>().FindKey(taskInfo.PlotNpcID);
                    itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(taskInfo.TargetItemID);
                    monsterList = taskInfo.TargetMonsterID.ToNotNullString().Split(new char[] { ',' });
                    monsterNumList = taskInfo.TargetMonsterNum.ToNotNullString().Split(new char[] { ',' });
                    monsterCurrNumList = userTask.TaskTargetNum.ToNotNullString().Split(new char[] { ',' });
                }

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.TaskID);
                dsItem.PushIntoStack(taskInfo == null ? string.Empty : taskInfo.TaskName.ToNotNullString());
                dsItem.PushIntoStack(taskInfo == null ? LanguageManager.GetLang().shortInt : taskInfo.TaskLv);
                dsItem.PushIntoStack((short)userTask.TaskState);
                dsItem.PushIntoStack(taskInfo == null ? string.Empty : taskInfo.TaskDescp.ToNotNullString());
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.ReleaseNpcID);
                dsItem.PushIntoStack(reNpcInfo == null ? string.Empty : reNpcInfo.NpcName.ToNotNullString());
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.DeliveryNpcID);
                dsItem.PushIntoStack(deNpcInfo == null ? string.Empty : deNpcInfo.NpcName.ToNotNullString());
                dsItem.PushIntoStack(taskInfo == null ? LanguageManager.GetLang().shortInt : (short)taskInfo.TermsType);

                dsItem.PushIntoStack(plotInfo != null ? plotInfo.CityID : 0);
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.PlotID);
                dsItem.PushIntoStack(plotInfo != null ? (short)plotInfo.PlotType : (short)0);
                dsItem.PushIntoStack(plotInfo != null ? plotInfo.PlotName.ToNotNullString() : string.Empty);
                dsItem.PushIntoStack(npcInfo == null ? string.Empty : npcInfo.NpcName.ToNotNullString());

                dsItem.PushIntoStack(monsterList.Length);
                for (int i = 0; i < monsterList.Length; i++)
                {
                    int monsterID = monsterList[i].ToInt();
                    int monsterNum = monsterNumList[i].ToInt();
                    int currNum = monsterCurrNumList.Length > i ? monsterCurrNumList[i].ToInt() : 0;
                    MonsterInfo monsterInfo = new ConfigCacheSet<MonsterInfo>().FindKey(monsterID);

                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(monsterInfo != null ? monsterInfo.GeneralName.ToNotNullString() : string.Empty);
                    dsItem1.PushIntoStack(monsterNum);
                    dsItem1.PushIntoStack(currNum);
                    dsItem.PushIntoStack(dsItem1);
                }

                dsItem.PushIntoStack(itemInfo != null ? itemInfo.ItemName.ToNotNullString() : string.Empty);
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.TargetItemNum);
                dsItem.PushIntoStack(UserItemHelper.CheckItemNum(ContextUser.UserID, taskInfo == null ? 0 : taskInfo.TargetItemID));
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.Experience);
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.GameCoin);

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum<TaskType>("TaskType", ref TaskType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            TaskHelper.CheckStoryCompleted(ContextUser.UserID);
            //增加初始任务
            userTaskList = TaskHelper.SendAllowTask(ContextUser, ContextUser.TaskProgress);

            userTaskList = new GameDataCacheSet<UserTask>().FindAll(ContextUser.UserID, m => m.TaskType.Equals(TaskType) && m.TaskState != TaskState.Close);
            userTaskList.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.TaskID.CompareTo(y.TaskID);
            });
            return true;
        }
    }
}