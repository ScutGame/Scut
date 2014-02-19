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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3007_剧情任务交付接口
    /// </summary>
    public class Action3007 : BaseAction
    {
        private int TaskID;
        private StoryTaskInfo taskInfo;
        private UserGeneral _userGeneral;


        public Action3007(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3007, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack((short)taskInfo.FunctionEnum);
            PushIntoStack(_userGeneral.GeneralLv);
            PushIntoStack(_userGeneral.LifeNum);
            PushIntoStack(_userGeneral.LifeMaxNum);
            PushIntoStack(_userGeneral.CurrExperience);
            var generalEscalate = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(_userGeneral.GeneralLv);
            PushIntoStack(generalEscalate != null ? generalEscalate.UpExperience : 0);
            PushIntoStack(ContextUser.GameCoin);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("TaskID", ref TaskID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            bool result = true;
            taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(TaskID);
            if (taskInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                //this.ErrorInfo = LanguageManager.GetLang().St3002_NotFind;
                SaveLog(new Exception(string.Format("{0},User:{1},task:{2}", LanguageManager.GetLang().St3002_NotFind, ContextUser.UserID, TaskID)));
                return false;
            }
            _userGeneral = UserGeneral.GetMainGeneral(Uid);
            //获得奖励
            UserTask userTask = new GameDataCacheSet<UserTask>().FindKey(ContextUser.UserID, TaskID);
            if (userTask == null || userTask.TaskState != TaskState.Completed)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St3007_NoCompleted;
                return false;
            }
            //交付物品任务扣除材料
            if (taskInfo.TermsType == PlotTermsType.Shouji || taskInfo.TermsType == PlotTermsType.Delivery)
            {
                UserItemHelper.UseUserItem(Uid, taskInfo.TargetItemID, taskInfo.TargetItemNum);
            }
            userTask.TaskState = TaskState.Close;
            UserTaskLog taskLog = new UserTaskLog()
            {
                LogID = Guid.NewGuid().ToString(),
                TaskID = userTask.TaskID,
                UserID = userTask.UserID,
                TaskType = userTask.TaskType,
                TaskState = userTask.TaskState,
                TaskPrize = string.Format("Experience:{0},GameCoin:{1};GameCoin:{2} ",
                    taskInfo.Experience,
                    taskInfo.GameCoin,
                    ContextUser.GameCoin
                ),
                CreateDate = DateTime.Now
            };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(taskLog);

            ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, taskInfo.GameCoin, int.MaxValue);
            if (taskInfo.TaskType == TaskType.Master && ContextUser.TaskProgress < TaskID)
            {
                ContextUser.TaskProgress = TaskID;
            }
            //ContextUser.Update();

            //任务加佣兵经验
            //var userEmbattles = new GameDataCacheSet<UserEmbattle>().FindAll(UserEmbattle.Index_UserID_MagicID, Uid, ContextUser.UseMagicID);
            //foreach (var userEmbattle in userEmbattles)
            //{
            //    UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, userEmbattle.GeneralID);
            //    if (userGeneral == null) continue;
            //    userGeneral.CurrExperience = MathUtils.Addition(userGeneral.CurrExperience, taskInfo.Experience, int.MaxValue);
            //    //userGeneral.Update();
            //}
            UserHelper.UserGeneralExp(ContextUser.UserID,taskInfo.Experience);

            //开启功能
            TaskHelper.EnableFunction(ContextUser, taskInfo.FunctionEnum);

            //下发可接任务
            TaskHelper.SendAllowTask(ContextUser, TaskID);

            //开启副本
            PlotHelper.EnablePlot(Uid, taskInfo.EnablePlot);

            //保留主线任务
            var cacheSet = new GameDataCacheSet<UserTask>();
            if (userTask.TaskType == TaskType.Master)
            {
                //userTask.Update();
            }
            else
            {
                //原因:最后支线任务无限刷新BUG
                if (!TaskHelper.IsLastTask(userTask))
                {
                    cacheSet.Delete(userTask);
                }
            }
            return result;
        }
    }
}