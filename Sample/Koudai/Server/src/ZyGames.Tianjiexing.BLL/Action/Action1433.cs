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
using System.Data;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1433_将星佣兵任务奖励领取接口
    /// </summary>
    public class Action1433 : BaseAction
    {
        private int taskID;


        public Action1433(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1433, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("TaskID", ref taskID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            StoryTaskInfo storyTask = new ConfigCacheSet<StoryTaskInfo>().FindKey(taskID);
            if (storyTask != null && storyTask.TaskLv <= ContextUser.UserLv)
            {
                UserTask userTask = new GameDataCacheSet<UserTask>().FindKey(ContextUser.UserID, taskID);
                if (userTask != null && userTask.TaskState == TaskState.Close)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1433_RewardAlreadyReceive;
                    return false;
                }
                int collectNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, storyTask.TargetItemID);
                if (collectNum < storyTask.TargetItemNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1462_ItemNumNotEnough;
                    return false;
                }
                ItemUseHelper itemuse = new ItemUseHelper();

                if (!itemuse.GeneralPrize(ContextUser, storyTask.Reward.ToList()))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1433_StoryTaskGridNotEnough;
                    return false;
                }
                if (itemuse.DoPrize(ContextUser, storyTask.Reward.ToList()))
                {
                    UserItemHelper.UseUserItem(ContextUser.UserID, storyTask.TargetItemID, storyTask.TargetItemNum);
                    if (userTask != null && userTask.TaskState != TaskState.Close)
                    {
                        userTask.TaskState = TaskState.Close;
                    }

                    if (userTask == null)
                    {
                        userTask = new UserTask
                        {
                            UserID = ContextUser.UserID,
                            TaskID = taskID,
                            TaskType = storyTask.TaskType,
                            TaskStar = TaskStar.Star5,
                            TaskState = TaskState.Close,
                            TaskTargetNum = "0",
                            CompleteNum = 0,
                            CreateDate = DateTime.Now
                        };
                        new GameDataCacheSet<UserTask>().Add(userTask);
                    }

                    ErrorCode = 0;
                    ErrorInfo = itemuse.content.Trim(',');
                }
            }
            return true;
        }
    }
}