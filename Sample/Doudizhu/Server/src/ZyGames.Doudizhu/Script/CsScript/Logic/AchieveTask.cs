using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Doudizhu.Bll.Logic
{
    /// <summary>
    /// 玩家成就、任务
    /// </summary>
    public class AchieveTask
    {
        /// <summary>
        /// 玩家成就
        /// </summary>
        /// <returns></returns>
        public static UserAchieve Get(string userID)
        {
            var cacheSet = new GameDataCacheSet<UserAchieve>();
            var data = cacheSet.FindKey(userID);
            if (data != null)
            {
                return data;
            }
            TraceLog.WriteError(string.Format("User:{0} Achieve package is null.", userID));
            return null;
        }

        /// <summary>
        /// 成就状态
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="achieveID"></param>
        /// <returns></returns>
        public static TaskStatus GetAchieveStatus(string userID, int achieveID)
        {
            var package = Get(userID);
            if (package != null)
            {
                var achieveInfo = package.AchievePackage.Find(s => s.AchieveID == achieveID);
                if (achieveInfo != null)
                {
                    return achieveInfo.TaskStatus;
                }
            }
            return TaskStatus.Unfinished;
        }

        /// <summary>
        /// 玩家任务
        /// </summary>
        /// <returns></returns>
        public static UserTask GetTask(string userID)
        {
            var cacheSet = new GameDataCacheSet<UserTask>();
            var data = cacheSet.FindKey(userID);
            if (data != null)
            {
                return data;
            }
            TraceLog.WriteError(string.Format("User:{0} Task package is null.", userID));
            return null;
        }

        /// <summary>
        /// 任务ID信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public static UserTaskInfo GetUserTaskInfo(string userID, int taskID)
        {
            var package = GetTask(userID);
            if (package != null)
            {
                var taskInfo = package.TaskPackage.Find(s => s.TaskID == taskID);
                return taskInfo;
            }
            return null;
        }

        /// <summary>
        /// 新增、修改任务信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="taskID"></param>
        /// <param name="taskNum"></param>
        /// <returns></returns>
        public static void SaveUserTask(string userID, int taskID, int taskNum)
        {
            var package = GetTask(userID);
            if (package != null)
            {
                RefreashUserTask(userID);
                var tempItem = package.TaskPackage.Find(m => m.TaskID == taskID);
                if (tempItem == null)
                {
                    tempItem = new UserTaskInfo();
                    tempItem.TaskID = taskID;
                    tempItem.TaskNum = 0;
                    tempItem.TaskStatus = TaskStatus.Unfinished;
                    tempItem.RefreshDate = DateTime.Now;
                    tempItem.CreateDate = DateTime.Now;
                    package.TaskPackage.Add(tempItem);
                }

                TaskInfo taskInfo = new ConfigCacheSet<TaskInfo>().FindKey(taskID);
                if (tempItem.TaskStatus == TaskStatus.Unfinished)
                {
                    tempItem.TaskNum = MathUtils.Addition(tempItem.TaskNum, taskNum);
                    if (taskInfo != null && tempItem.TaskNum >= taskInfo.RestraintNum)
                    {
                        tempItem.TaskStatus = TaskStatus.Complete;
                        tempItem.CreateDate = DateTime.Now;
                    }
                }
                if (taskInfo != null && taskInfo.AchieveID > 0)
                {
                    SaveUserAchieve(userID, taskInfo.AchieveID, tempItem.TaskNum);
                }
            }
        }

        /// <summary>
        /// 新增、修改任务信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="taskClass"></param>
        /// <param name="taskNum"></param>
        /// <returns></returns>
        public static void SaveUserTask(string userID, TaskClass taskClass, int taskNum)
        {
            var taskList = new ConfigCacheSet<TaskInfo>().FindAll(s => s.TaskClass == taskClass);
            foreach (var info in taskList)
            {
                SaveUserTask(userID, info.TaskID, taskNum);
            }
        }

        /// <summary>
        /// 新增、修改成就信息
        /// </summary>
        public static void SaveUserAchieve(string userID, int achieveID, int taskNum)
        {
            var package = Get(userID);
            if (package != null)
            {
                var tempItem = package.AchievePackage.Find(m => m.AchieveID == achieveID);
                if (tempItem == null)
                {
                    tempItem = new UserAchieveInfo();
                    tempItem.AchieveID = achieveID;
                    tempItem.TaskNum = 0;
                    tempItem.TaskStatus = TaskStatus.Unfinished;
                    tempItem.CreateDate = DateTime.Now;
                    package.AchievePackage.Add(tempItem);
                }

                if (tempItem.TaskStatus == TaskStatus.Unfinished)
                {
                    AchievementInfo mentInfo = new ConfigCacheSet<AchievementInfo>().FindKey(achieveID);
                    if (mentInfo != null)
                    {
                        tempItem.TaskNum = taskNum;
                        if (mentInfo.TargetNum <= taskNum)
                        {
                            tempItem.TaskStatus = TaskStatus.Complete;
                            tempItem.CreateDate = DateTime.Now;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 刷新日常任务信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static void RefreashUserTask(string userID)
        {
            var package = GetTask(userID);
            if (package != null)
            {
                var tempItemList = package.TaskPackage.FindAll(m => m.TaskType == TaskType.Daily && m.RefreshDate.Date != DateTime.Now.Date);
                foreach (UserTaskInfo info in tempItemList)
                {
                    info.TaskStatus = TaskStatus.Unfinished;
                    info.TaskNum = 0;
                    info.RefreshDate = DateTime.Now;
                }
            }
        }
    }
}
