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
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3005_日常任务列表接口
    /// </summary>
    public class Action3005 : BaseAction
    {
        private static int useProGold = ConfigEnvSet.GetInt("UserTask.RefreashUseGold");
        private static int useFullGold = ConfigEnvSet.GetInt("UserTask.RefreashFullUseGold");
        private static int completedUseGold = ConfigEnvSet.GetInt("UserTask.CompletedUseGold");
        private int TaskID;
        private int ops = 0;
        private short userLv;
        private int currNum;
        private List<UserTask> userTaskList = new List<UserTask>();


        public Action3005(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3005, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(TaskHelper.DailyEveryMaxNum);
            this.PushIntoStack(currNum);
            this.PushIntoStack(userTaskList.Count);
            foreach (UserTask userTask in userTaskList)
            {
                DailyTaskInfo dtaskInfo = new ConfigCacheSet<DailyTaskInfo>().FindKey(userTask.TaskID);
                var userItem = userTask.GetDailyItem(userLv);
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem != null ? userItem.ItemID : 0);

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(userTask.TaskID);
                dsItem.PushIntoStack(dtaskInfo.TaskName.ToNotNullString());
                dsItem.PushIntoStack((short)userTask.TaskState);
                dsItem.PushIntoStack((short)userTask.TaskType);
                dsItem.PushIntoStack((short)userTask.TaskStar);
                dsItem.PushIntoStack(dtaskInfo.TaskDescp.ToNotNullString());
                dsItem.PushIntoStack(dtaskInfo.TaskTarget);
                dsItem.PushIntoStack((int)userTask.TaskStar);
                dsItem.PushIntoStack(userTask.TaskTargetNum.ToInt());
                dsItem.PushIntoStack(userTask.GetDailyExpNum(userLv));
                dsItem.PushIntoStack(itemInfo != null ? itemInfo.ItemName.ToNotNullString() : string.Empty);
                dsItem.PushIntoStack(userItem != null ? userItem.Num : 0);

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            //httpGet.GetInt("TaskID", ref TaskID);
            if (httpGet.GetInt("Ops", ref ops))
            {
                httpGet.GetInt("TaskID", ref TaskID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            TaskStar taskOpsStar = TaskStar.Star1;
            userLv = ContextUser.UserLv;
            this.ErrorCode = ops;
            if (ops == 1)
            {
                if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.RefreashDailyTask))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                    return false;
                }
                List<UserTask> taskArray = new GameDataCacheSet<UserTask>().FindAll(ContextUser.UserID, u => u.TaskState == TaskState.AllowTake && u.TaskType == TaskType.Daily);
                if (taskArray.Count > 0)
                {
                    if (taskArray[0].TaskStar == TaskStar.Star5)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St3005_RefreashStarFull;
                        return false;
                    }
                }
                ErrorInfo = string.Format(LanguageManager.GetLang().St3005_RefreashUseGold, useProGold);
                return false;
            }
            else if (ops == 2)
            {
                if (ContextUser.GoldNum < useProGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                List<UserTask> taskArray = new GameDataCacheSet<UserTask>().FindAll(ContextUser.UserID, u => u.TaskState != TaskState.Disable && u.TaskType == TaskType.Daily);
                if (taskArray.Count > 0)
                {
                    taskOpsStar = TaskHelper.GetTaskStar(taskArray[0].TaskStar, taskArray[0].TakeDate);
                }
                foreach (UserTask taskInfo in taskArray)
                {
                    taskInfo.TaskStar = taskOpsStar;
                    //taskInfo.Update();
                }
                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useProGold, int.MaxValue);
                //ContextUser.Update();
                if (taskArray.Count > 0)
                {
                    userTaskList = taskArray;
                    //UserTask userTask = taskArray[0];
                    //刷新任务
                    //userTaskList = TaskHelper.RefreshDailyTask(ContextUser.UserID, userTask);
                }
            }
            else if (ops == 3)
            {
                if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.ShuanChuMaxXingJiRenWu))
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                    return false;
                }
                this.ErrorInfo = string.Format(LanguageManager.GetLang().St3005_RefreashUseGold, useFullGold);
                return false;
            }
            else if (ops == 4)
            {
                if (ContextUser.GoldNum < useFullGold)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                taskOpsStar = TaskStar.Star5;
                List<UserTask> taskArray = new GameDataCacheSet<UserTask>().FindAll(ContextUser.UserID, u => u.TaskState == TaskState.AllowTake && u.TaskType == TaskType.Daily);

                foreach (UserTask taskInfo in taskArray)
                {
                    taskInfo.TaskStar = taskOpsStar;
                    //taskInfo.Update();
                }
                if (taskArray.Count > 0)
                {
                    UserTask userTask = taskArray[0];
                    //刷新任务
                    userTaskList = TaskHelper.RefreshDailyTask(ContextUser.UserID, userTask);
                }

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useFullGold, int.MaxValue);
                //ContextUser.Update();


            }
            else if (ops == 5)
            {
                if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.DailyRenWuZhiJieWanCheng))
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                    return false;
                }
                this.ErrorInfo = string.Format(LanguageManager.GetLang().St3005_CompletedUseGold, completedUseGold);
                return false;

            }
            else if (ops == 6)
            {
                if (ContextUser.GoldNum < completedUseGold)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                List<UserTask> taskArray = new GameDataCacheSet<UserTask>().FindAll(ContextUser.UserID, u => u.TaskState == TaskState.Taked && u.TaskType == TaskType.Daily);
                if (taskArray.Count > 0)
                {
                    UserTask userTask = taskArray[0];
                    DailyTaskInfo dtaskInfo = new ConfigCacheSet<DailyTaskInfo>().FindKey(taskArray[0].TaskID);
                    //交付任务

                    //奖励
                    int userExp = ContextUser.ExpNum;
                    int expNum = userTask.GetDailyExpNum(userLv);
                    ContextUser.ExpNum = MathUtils.Addition(ContextUser.ExpNum, expNum, int.MaxValue);
                    //ContextUser.Update();
                    var userItem = userTask.GetDailyItem(userLv);
                    if (userItem != null)
                    {
                        UserItemHelper.AddUserItem(ContextUser.UserID, userItem.ItemID, userItem.Num);

                    }
                    userTask.TaskState = TaskState.Close;
                    userTask.CompleteNum = MathUtils.Addition(userTask.CompleteNum, 1, int.MaxValue);
                    //userTask.Update();
                    //奖励日志
                    UserTaskLog taskLog = new UserTaskLog()
                    {
                        LogID = System.Guid.NewGuid().ToString(),
                        TaskID = userTask.TaskID,
                        UserID = userTask.UserID,
                        TaskType = userTask.TaskType,
                        TaskState = userTask.TaskState,
                        TaskPrize = string.Format("ExpNum:{0},item:{1}={2};User expnum:{3}", expNum,
                            userItem != null ? userItem.ItemID : 0,
                            userItem != null ? userItem.Num : 0,
                            userExp
                        ),
                        CreateDate = DateTime.Now
                    };
                    var sender = DataSyncManager.GetDataSender();
                    sender.Send(taskLog);
                    //刷新任务
                    userTaskList = TaskHelper.RefreshDailyTask(ContextUser.UserID, userTask);

                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, completedUseGold, int.MaxValue);
                    //ContextUser.Update();
                }
            }
            else if (ops == 0)
            {
                if (TaskID > 0)
                {
                    UserTask userTask = new GameDataCacheSet<UserTask>().FindKey(ContextUser.UserID, TaskID);
                    if (userTask == null || userTask.TaskState != TaskState.Completed)
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St3007_NoCompleted;
                        return false;
                    }
                    if (TaskHelper.GetTaskCompleteNum(ContextUser.UserID) >= TaskHelper.DailyEveryMaxNum)
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St3005_CompletedTimeout;
                        return false;
                    }
                    DailyTaskInfo dtaskInfo = new ConfigCacheSet<DailyTaskInfo>().FindKey(userTask.TaskID);
                    //交付任务

                    //奖励
                    int userExp = ContextUser.ExpNum;
                    int expNum = userTask.GetDailyExpNum(userLv);
                    ContextUser.ExpNum = MathUtils.Addition(ContextUser.ExpNum, expNum, int.MaxValue);
                    //ContextUser.Update();
                    var userItem = userTask.GetDailyItem(userLv);
                    if (userItem != null)
                    {
                        UserItemHelper.AddUserItem(ContextUser.UserID, userItem.ItemID, userItem.Num);
                    }
                    userTask.TaskState = TaskState.Close;
                    userTask.CompleteNum = MathUtils.Addition(userTask.CompleteNum, 1, int.MaxValue);
                    //userTask.Update();
                    //奖励日志
                    UserTaskLog taskLog = new UserTaskLog()
                    {
                        LogID = System.Guid.NewGuid().ToString(),
                        TaskID = userTask.TaskID,
                        UserID = userTask.UserID,
                        TaskType = userTask.TaskType,
                        TaskState = userTask.TaskState,
                        TaskPrize = string.Format("ExpNum:{0},item:{1}={2};User expnum:{3}", expNum,
                            userItem != null ? userItem.ItemID : 0,
                            userItem != null ? userItem.Num : 0,
                            userExp
                        ),
                        CreateDate = DateTime.Now
                    };

                    var sender = DataSyncManager.GetDataSender();
                    sender.Send(taskLog);
                    //刷新任务
                    userTaskList = TaskHelper.RefreshDailyTask(ContextUser.UserID, userTask);

                }
                else
                {
                    //刷新当天任务
                    if (!TaskHelper.CheckDailyTaskTimeout(ContextUser.UserID, out userTaskList))
                    {
                        userTaskList = new GameDataCacheSet<UserTask>().FindAll(ContextUser.UserID,
                            m => m.TaskType.Equals(TaskType.Daily) &&
                                !m.TaskState.Equals(TaskState.NoTake) &&
                                !m.TaskState.Equals(TaskState.Disable)
                        );
                    }

                    if (userTaskList.Count == 0)
                    {
                        //初始化时刷新任务
                        userTaskList = TaskHelper.RefreshDailyTask(ContextUser.UserID, null);
                    }

                    //刷新状态
                    foreach (UserTask userTask in userTaskList)
                    {
                        bool isModify = false;

                        if (userTask.TaskState == TaskState.Taked &&
                            userTask.TaskTargetNum.ToInt() >= (int)userTask.TaskStar)
                        {
                            isModify = true;
                            userTask.TaskState = TaskState.Completed;
                        }
                        if (isModify)
                        {
                            //userTask.Update();
                        }
                    }
                }
            }

            //计算当天完成次数
            currNum = TaskHelper.GetTaskCompleteNum(ContextUser.UserID);
            return true;
        }
    }
}