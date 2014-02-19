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
    /// 3003_剧情任务跟踪接口
    /// </summary>
    public class Action3003 : BaseAction
    {
        private int _ops;
        private List<UserTask> _userTaskList = new List<UserTask>();

        public Action3003(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3003, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_userTaskList.Count);
            foreach (UserTask userTask in _userTaskList)
            {
                StoryTaskInfo taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(userTask.TaskID);
                //CityNpcInfo reNpcInfo = new ConfigCacheSet<CityNpcInfo>().FindKey(taskInfo.ReleaseNpcID);

                CityNpcInfo deNpcInfo = null;
                PlotInfo plotInfo = null;
                PlotNPCInfo npcInfo = null;
                ItemBaseInfo itemInfo = null;
                string[] monsterList = new string[0];
                string[] monsterNumList = new string[0];
                string[] monsterCurrNumList = new string[0];
                if (taskInfo != null)
                {
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
                dsItem.PushIntoStack((short)userTask.TaskState);
                dsItem.PushIntoStack(taskInfo == null ? LanguageManager.GetLang().shortInt : (short)taskInfo.TaskType);
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.ReleaseNpcID);
                dsItem.PushIntoStack(taskInfo == null ? LanguageManager.GetLang().shortInt : (short)taskInfo.TermsType);
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.DeliveryNpcID);
                dsItem.PushIntoStack(deNpcInfo == null ? string.Empty : deNpcInfo.NpcName.ToNotNullString());

                dsItem.PushIntoStack(plotInfo == null ? 0 : plotInfo.CityID);
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.PlotID);
                dsItem.PushIntoStack(plotInfo == null ? string.Empty : plotInfo.PlotName.ToNotNullString());
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

                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(taskInfo == null ? 0 : taskInfo.TargetItemNum);
                dsItem.PushIntoStack(UserItemHelper.CheckItemNum(ContextUser.UserID, taskInfo == null ? 0 : taskInfo.TargetItemID));
                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref _ops, 0, 2))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            TaskHelper.CheckStoryCompleted(ContextUser.UserID);

            List<StoryTaskInfo> taskinfoArray = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.PlotID == 1001);
            if (taskinfoArray.Count == 0)
            {
                return false;
            }
            //增加初始任务
            _userTaskList = TaskHelper.SendAllowTask(ContextUser, ContextUser.TaskProgress);

            if (_ops == 0)
            {
                _userTaskList = new GameDataCacheSet<UserTask>().FindAll( ContextUser.UserID,
                    m => m.TaskType < TaskType.Daily &&
                        (m.TaskState == TaskState.AllowTake ||
                        m.TaskState == TaskState.Taked ||
                        m.TaskState == TaskState.Completed ||
                        (m.TaskType == TaskType.Master && m.TaskState == TaskState.NoTake))
                );
            }
            else if (_ops == 1)
            {
                //当前任务
                _userTaskList = new GameDataCacheSet<UserTask>().FindAll(
                    ContextUser.UserID,
                    m => m.TaskType < TaskType.Daily && (m.TaskState == TaskState.Taked || m.TaskState == TaskState.Completed)
                );
            }
            else if (_ops == 2)
            {
                //可接任务
                List<UserTask> taskArray = new GameDataCacheSet<UserTask>().FindAll(
                    ContextUser.UserID,
                    m => m.TaskType < TaskType.Daily && m.TaskState == TaskState.NoTake
                );
                foreach (UserTask userTask in taskArray)
                {
                    StoryTaskInfo taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(userTask.TaskID);
                    if (taskInfo != null && taskInfo.TaskLv <= ContextUser.UserLv)
                    {
                        userTask.TaskState = TaskState.AllowTake;
                        //userTask.Update();
                    }
                }

                _userTaskList = new GameDataCacheSet<UserTask>().FindAll(
                    ContextUser.UserID,
                    m => m.TaskType < TaskType.Daily && (m.TaskState == TaskState.AllowTake)
                );
            }

            _userTaskList.QuickSort(
                (x, y) =>
                {
                    if (x.TaskState == TaskState.AllowTake && y.TaskState == TaskState.Taked)
                    {
                        return 1;
                    }
                    else if (y.TaskState == TaskState.AllowTake && x.TaskState == TaskState.Taked)
                    {
                        return -1;
                    }
                    else
                    {
                        return x.TaskState.CompareTo(y.TaskState);
                    }
                }
            );
            return true;
        }
    }
}