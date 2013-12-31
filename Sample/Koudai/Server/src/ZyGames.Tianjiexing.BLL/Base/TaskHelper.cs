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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Component;


namespace ZyGames.Tianjiexing.BLL.Base
{
    public static class TaskHelper
    {
        /// <summary>
        /// 下发任务
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taskID"></param>
        public static List<UserTask> SendAllowTask(GameUser user, int taskID)
        {
            List<UserTask> userTaskList = new List<UserTask>();
            var cacheSet = new GameDataCacheSet<UserTask>();
            UserTask preTask = cacheSet.FindKey(user.UserID, taskID);
            if (taskID > 0 && preTask != null && (preTask.TaskState != TaskState.Close || preTask.TaskType == TaskType.Offset))
            {
                return userTaskList;
            }
            if (taskID == 0 && cacheSet.FindAll(user.UserID, m => m.TaskType < TaskType.Daily).Count > 0)
            {
                return userTaskList;
            }

            IList<StoryTaskInfo> taskList = StoryTaskInfo.GetNextTask(taskID);
            bool isSend = true;
            foreach (StoryTaskInfo taskInfo in taskList)
            {
                if (taskInfo.CountryID != CountryType.None && user.CountryID != taskInfo.CountryID)
                {
                    continue;
                }
                UserTask userTask = cacheSet.FindKey(user.UserID, taskInfo.TaskID);
                if (userTask == null)
                {
                    userTask = new UserTask
                    {
                        TaskID = taskInfo.TaskID,
                        UserID = user.UserID,
                        TaskType = taskInfo.TaskType,
                        TaskState = taskInfo.TaskLv <= user.UserLv ? TaskState.AllowTake : TaskState.NoTake,
                        CompleteNum = 0,
                        CreateDate = DateTime.Now
                    };
                    cacheSet.Add(userTask);
                    userTask = cacheSet.FindKey(user.UserID, taskInfo.TaskID);
                    userTaskList.Add(userTask);
                }
                else
                {
                    //判断是否已下发过
                    isSend = false;
                    break;
                }
            }
            if (isSend)
            {
                return userTaskList;
            }
            return new List<UserTask>();
        }

        /// <summary>
        /// 开启功能[未完成]
        /// </summary>
        /// <param name="user"></param>
        /// <param name="functionEnum"></param>
        public static void EnableFunction(GameUser user, FunctionEnum functionEnum)
        {
            if (functionEnum <= 0)
            {
                return;
            }
            var cacheSet = new GameDataCacheSet<UserFunction>();
            UserFunction uf = cacheSet.FindKey(user.UserID, functionEnum);
            if (uf == null)
            {
                uf = new UserFunction
                {
                    UserID = user.UserID,
                    FunEnum = functionEnum,
                    CreateDate = DateTime.Now
                };

                cacheSet.Add(uf);
                uf = cacheSet.FindKey(user.UserID, functionEnum);
            }

            switch (functionEnum)
            {
                //case FunctionEnum.Qianghuaqueue:
                //    user.QueueNum = 2;
                //    user.Update();
                //    break;
                case FunctionEnum.Qianghua:
                    user.QueueNum = (short)new GameUser().QueueMinNum;
                    //user.Update();
                    break;
                case FunctionEnum.Shengmishangdian:
                    UserHelper.RefrshShopsSparData(user, false);
                    break;
                case FunctionEnum.Mofazheng:
                    InitMagicEmbattle(user.UserID, user.UserLv);
                    break;
                case FunctionEnum.Zhongzhijiyangshu:
                    GetUserLand(user.UserID);
                    break;
                case FunctionEnum.Zhongzhijingqianshu:
                    GoinUserQueue(user.UserID);
                    break;
                case FunctionEnum.Enchant:
                    EnchantHelper.EnchantFunctionOpen(user);
                    break;
                case FunctionEnum.ReplaceGeneral:
                    GeneralHelper.OpenReplaceGeneral(user);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初始化魔法阵
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userLv"></param>
        private static void InitMagicEmbattle(string userId, short userLv)
        {
            List<UserMagic> userMagicList = new GameDataCacheSet<UserMagic>().FindAll(userId, m => m.MagicType == MagicType.MoFaZhen && m.MagicID != new GameUser().UserMagicID);
            if (userMagicList.Count == 0)
            {
                List<MagicInfo> magicInfoList = new ConfigCacheSet<MagicInfo>().FindAll(m => m.MagicType == MagicType.MoFaZhen && m.DemandLv <= userLv);
                foreach (var magicInfo in magicInfoList)
                {
                    var userMagic = new GameDataCacheSet<UserMagic>().FindKey(userId, magicInfo.MagicID);
                    if (userMagic != null) continue;

                    userMagic = new UserMagic
                    {
                        UserID = userId,
                        MagicID = magicInfo.MagicID,
                        MagicLv = magicInfo.MagicLv,
                        MagicType = magicInfo.MagicType,
                        IsEnabled = false
                    };
                    new GameDataCacheSet<UserMagic>().Add(userMagic);
                    MagicLvInfo magicLvInfo = new ConfigCacheSet<MagicLvInfo>().FindKey(magicInfo.MagicID, magicInfo.MagicLv);
                    short position = magicLvInfo.GetFirstGrid();
                    var userEmbattle = new UserEmbattle
                    {
                        UserID = userId,
                        GeneralID = 0, // LanguageManager.GetLang().GameUserGeneralID,
                        MagicID = magicInfo.MagicID,
                        Position = position
                    };
                    new GameDataCacheSet<UserEmbattle>().Add(userEmbattle);
                }
            }
            //UserEmbattle
        }

        /// <summary>
        /// 刷新杀怪数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="plotId"></param>
        /// <param name="plotNpcId"></param>
        public static void KillPlotMonster(string userId, int plotId, int plotNpcId)
        {
            List<StoryTaskInfo> taskList = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.PlotID == plotId);
            foreach (StoryTaskInfo taskInfo in taskList)
            {
                UserTask userTask = new GameDataCacheSet<UserTask>().FindKey(userId, taskInfo.TaskID);
                if (userTask == null || userTask.TaskState != TaskState.Taked) continue;

                List<string> targetNumList = new List<string>();
                if (userTask.TaskType != TaskType.Daily)
                {
                    targetNumList = FindTargetMonsterNum(taskInfo, plotNpcId, userTask.TaskTargetNum.ToNotNullString().Split(new[] { ',' }));
                }

                userTask.TaskTargetNum = string.Join(",", targetNumList.ToArray());
                //userTask.Update();
            }

        }

        /// <summary>
        /// 触发日常任务
        /// </summary>
        public static void TriggerDailyTask(string userId, int taskId)
        {
            lock (userId)
            {
                DailyTaskInfo taskInfo = new ConfigCacheSet<DailyTaskInfo>().FindKey(taskId);
                UserTask userTask = new GameDataCacheSet<UserTask>().FindKey(userId, taskId);
                if (taskInfo == null || userTask == null) return;
                if (userTask.TaskState == TaskState.Taked)
                {
                    userTask.TaskTargetNum = (MathUtils.Addition(userTask.TaskTargetNum.ToInt(), 1, (int)userTask.TaskStar)).ToString();
                    //userTask.Update();
                }

            }
        }

        /// <summary>
        /// 检索目标数量
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <param name="plotNpcId"></param>
        /// <param name="currMonsterList"></param>
        /// <returns></returns>
        private static List<string> FindTargetMonsterNum(StoryTaskInfo taskInfo, int plotNpcId, string[] currMonsterList)
        {
            string[] targetMonsterList = taskInfo.TargetMonsterID.ToNotNullString().Split(',');
            string[] targetMonsterNumList = ObjectExtend.ToNotNullString(taskInfo.TargetMonsterNum).Split(',');
            List<string> targetNumList = new List<string>();
            int index = 0;
            foreach (string monster in targetMonsterList)
            {
                int num = currMonsterList.Length > index ? ObjectExtend.ToInt(currMonsterList[index]) : 0;
                int maxNum = targetMonsterNumList.Length > index ? ObjectExtend.ToInt(targetMonsterNumList[index]) : 0;
                if (num < maxNum)
                {
                    var plotEmbattleList = new ConfigCacheSet<PlotEmbattleInfo>().FindAll(m => m.PlotNpcID == plotNpcId);
                    foreach (PlotEmbattleInfo plotEmbattle in plotEmbattleList)
                    {
                        if (plotEmbattle.MonsterID == ObjectExtend.ToInt(monster))
                        {
                            num += 1;
                        }
                    }
                    targetNumList.Add(num.ToString());
                }
                else
                {
                    targetNumList.Add(maxNum.ToString());
                }
                index++;
            }
            return targetNumList;
        }

        /// <summary>
        /// 检查是否完成
        /// 1.对话任务=>直接更新成可完成
        /// 2.副本战斗=>战斗结束需要回写玩家任务表目标数量
        /// </summary>
        public static void CheckStoryCompleted(string userID)
        {
            List<UserTask> userTaskList = new GameDataCacheSet<UserTask>().FindAll(userID, m => m.TaskType < TaskType.Daily && (m.TaskState == TaskState.Taked || m.TaskState == TaskState.NoTake));
            foreach (UserTask userTask in userTaskList)
            {
                StoryTaskInfo taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(userTask.TaskID);

                if (taskInfo == null) continue;

                if (userTask.TaskState == TaskState.NoTake)
                {
                    GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(userID);
                    short lv = gameUser == null ? (short)0 : gameUser.UserLv;
                    userTask.TaskState = taskInfo.TaskLv <= lv ? TaskState.AllowTake : TaskState.NoTake;
                    //userTask.Update();
                    continue;
                }

                bool isUpdate = false;
                switch (taskInfo.TermsType)
                {
                    case PlotTermsType.Shouji:
                    case PlotTermsType.Delivery:
                        int currNum = UserItemHelper.CheckItemNum(userID, taskInfo.TargetItemID);
                        if (currNum >= taskInfo.TargetItemNum)
                        {
                            userTask.TaskState = TaskState.Completed;
                            isUpdate = true;
                        }
                        break;
                    case PlotTermsType.StoryBattle:
                        var userPlot = UserPlotHelper.GetUserPlotInfo(userID, taskInfo.PlotID);
                        if (userPlot != null && userPlot.PlotStatus == PlotStatus.Completed)
                        {
                            userTask.TaskState = TaskState.Completed;
                            isUpdate = true;
                        }
                        break;
                    case PlotTermsType.Battle:
                        string[] monsterNumList = taskInfo.TargetMonsterNum.ToNotNullString().Split(new[] { ',' });
                        string[] userNumList = userTask.TaskTargetNum.ToNotNullString().Split(new[] { ',' });
                        bool flag = true;
                        for (int i = 0; i < monsterNumList.Length; i++)
                        {
                            int mNum = monsterNumList[i].ToInt();
                            int uNum = userNumList.Length > 0 ? userNumList[i].ToInt() : 0;
                            if (uNum < mNum)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            userTask.TaskState = TaskState.Completed;
                            isUpdate = true;
                        }
                        break;
                    case PlotTermsType.Dialogue:
                        userTask.TaskState = TaskState.Completed;
                        isUpdate = true;
                        break;
                    default:
                        break;
                }
                if (isUpdate)
                {
                    //userTask.Update();
                }
            }
        }


        public static string[] DailyIncreasePesent = ConfigEnvSet.GetString("Task.DailyIncreasePesent").Split(new[] { ',' });
        public static int DailyWeekReflesh = ConfigEnvSet.GetInt("Task.DailyWeekReflesh");
        public static int DailyMaxCount = ConfigEnvSet.GetInt("Task.DailyMaxCount");
        public static int DailyEveryMaxNum = ConfigEnvSet.GetInt("Task.DailyEveryMaxNum");


        /// <summary>
        /// 检查是否有过期任务
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userTaskList"></param>
        /// <returns></returns>
        public static bool CheckDailyTaskTimeout(string userID, out List<UserTask> userTaskList)
        {
            bool result = false;
            userTaskList = new List<UserTask>();
            List<UserTask> userTaskArray = new GameDataCacheSet<UserTask>().FindAll(userID, m => m.TaskType == TaskType.Daily && !m.TaskState.Equals(TaskState.Disable));
            //不是当天的清空数据
            if (userTaskArray != null && userTaskArray.Count > 0 && !userTaskArray[0].CreateDate.Date.Equals(DateTime.Now.Date))
            {
                UserTask userTask = userTaskArray[0];
                foreach (UserTask userTaskTemp in userTaskArray)
                {
                    //userTaskTemp.Remove();//注释8-12 wuzf 修改不删除日常任务
                    userTaskTemp.TaskState = TaskState.Disable;
                    userTaskTemp.CompleteNum = 0;
                }
                userTaskList = RefreshDailyTask(userID, userTask);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 刷新日常任务,当天的记录不清空
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userTask"></param>
        /// <returns></returns>
        public static List<UserTask> RefreshDailyTask(string userID, UserTask userTask)
        {
            List<UserTask> userTaskList = new GameDataCacheSet<UserTask>().FindAll(userID, m => !m.TaskState.Equals(TaskState.Disable) && m.TaskType.Equals(TaskType.Daily));
            foreach (UserTask userTaskTemp in userTaskList)
            {
                //关掉任务
                userTaskTemp.TaskState = TaskState.Disable;
                //userTaskTemp.Update();
            }

            TaskStar taskStar = TaskStar.Star1;
            if (userTask != null)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && userTask.CreateDate.Date < DateTime.Now.Date)
                {
                    taskStar = TaskStar.Star1;
                }
                else
                {
                    taskStar = GetTaskStar(userTask.TaskStar, userTask.TakeDate);
                }
            }

            DailyTaskInfo[] dtaskList = RandomUtils.GetRandomArray(new ConfigCacheSet<DailyTaskInfo>().FindAll().ToArray(), DailyMaxCount);
            foreach (DailyTaskInfo item in dtaskList)
            {
                UserTask tempTask = new GameDataCacheSet<UserTask>().FindKey(userID, item.TaskID);
                if (tempTask == null)
                {
                    tempTask = new UserTask
                    {
                        UserID = userID,
                        TaskID = item.TaskID,
                        TaskType = item.TaskType,
                        TaskStar = taskStar,
                        TaskState = TaskState.AllowTake,
                        TaskTargetNum = "0",
                        CompleteNum = 0,
                        CreateDate = DateTime.Now
                    };
                    new GameDataCacheSet<UserTask>().Add(tempTask);
                }
                else
                {
                    tempTask.TaskStar = taskStar;
                    tempTask.TaskState = TaskState.AllowTake;
                    tempTask.TaskTargetNum = "0";
                    if (!tempTask.CreateDate.Date.Equals(DateTime.Now.Date))
                    {
                        tempTask.CompleteNum = 0;
                    }
                    tempTask.CreateDate = DateTime.Now;
                }

            }

            return new GameDataCacheSet<UserTask>().FindAll(userID,
                m => m.TaskType.Equals(TaskType.Daily) &&
                    (m.TaskState.Equals(TaskState.Taked) || m.TaskState.Equals(TaskState.AllowTake) || m.TaskState.Equals(TaskState.Completed)) &&
                    m.CreateDate.Date.Equals(DateTime.Now.Date)
            );
        }

        public static void ResetStar(UserTask userTask)
        {

        }

        /// <summary>
        /// 获取刷新星级
        /// </summary>
        /// <param name="currStar"></param>
        /// <param name="taskDate"></param>
        /// <returns></returns>
        public static TaskStar GetTaskStar(TaskStar currStar, DateTime taskDate)
        {
            TaskStar taskStar;
            if (currStar == TaskStar.Star5 &&
                (DateTime.Now.DayOfWeek != DayOfWeek.Monday ||
                taskDate.DayOfWeek == DayOfWeek.Monday)
                )
            {
                //保留5星
                taskStar = currStar;
                return taskStar;
            }
            string persent = DailyIncreasePesent.Length > (int)currStar - 1 ? DailyIncreasePesent[(int)currStar - 1] : string.Empty;
            double persentNum = persent.ToDouble();
            bool isIncrease = RandomUtils.IsHit(persentNum);
            if (isIncrease && currStar < TaskStar.Star5)
            {
                taskStar = (TaskStar)((int)currStar + 1);
            }
            else
            {
                taskStar = currStar;
            }

            return taskStar;
        }

        /// <summary>
        /// 获取当天完成次数
        /// </summary>
        /// <returns></returns>
        public static int GetTaskCompleteNum(string userID)
        {
            int index = 0;
            List<UserTask> userTaskList = new GameDataCacheSet<UserTask>().FindAll(userID, m => m.TaskType == TaskType.Daily && m.CreateDate.Date.Equals(DateTime.Now.Date));
            foreach (UserTask userTask in userTaskList)
            {
                index += userTask.CompleteNum;
            }
            return index;
        }

        //开启种植功能
        public static void GetUserLand(string userID)
        {
            UserPlant plant = new GameDataCacheSet<UserPlant>().FindKey(userID);
            if (plant == null)
            {
                UserPlant userPlant = new UserPlant()
                {
                    UserID = userID,
                    LandNum = 1,
                    DewNum = ConfigEnvSet.GetInt("UserQueue.ShengShuiMaxNum"),
                    PayDewTime = 0,
                };
                new GameDataCacheSet<UserPlant>().Add(userPlant);
            }
            int postion = 1;
            UserLand land = new GameDataCacheSet<UserLand>().FindKey(userID, postion);
            if (land == null)
            {
                UserLand userLand = new UserLand()
                                      {
                                          UserID = userID,
                                          LandPositon = postion,
                                          IsRedLand = 2,
                                          IsBlackLand = 2,
                                          IsGain = 2,
                                          PlantType = PlantType.Experience,
                                          PlantQuality = PlantQualityType.PuTong,
                                          GeneralID = 0,
                                      };
                new GameDataCacheSet<UserLand>().Add(userLand);

            }
        }

        /// <summary>
        /// 开启金币种植圣水恢复队列
        /// </summary>
        /// <param name="userID"></param>
        public static void GoinUserQueue(string userID)
        {
            List<UserQueue> ququeArray = new GameDataCacheSet<UserQueue>().FindAll(userID, m => m.QueueType == QueueType.ShengShuiHuiFu);
            if (ququeArray.Count == 0)
            {
                UserQueue queue = new UserQueue
                {
                    QueueID = Guid.NewGuid().ToString(),
                    QueueName = QueueType.ShengShuiHuiFu.ToString(),
                    QueueType = QueueType.ShengShuiHuiFu,
                    UserID = userID,
                    Timing = DateTime.Now,
                    ColdTime = 0,
                    TotalColdTime = 0,
                    IsSuspend = false,
                    StrengNum = 0
                };
                new GameDataCacheSet<UserQueue>().Add(queue);
            }
        }

        /// <summary>
        /// 是否最后一个主线任务的支线
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public static bool IsLastTask(UserTask taskInfo)
        {
            int currTaskID = ConfigEnvSet.GetInt("StoryTaskInfo.CurrMaxTaskID");
            StoryTaskInfo taskOffset = new ConfigCacheSet<StoryTaskInfo>().FindKey(taskInfo.TaskID);
            if (taskOffset != null && (taskOffset.TaskType == TaskType.Offset || taskOffset.TaskType == TaskType.Elite) && taskOffset.PreTaskID.Length > 0)
            {
                if (taskOffset.PreTaskID[0] == currTaskID)
                {
                    taskInfo.TaskState = TaskState.Close;
                    return true;
                }
            }
            return false;
        }
    }
}