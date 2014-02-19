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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3002_剧情任务领取放弃接口
    /// </summary>
    public class Action3002 : BaseAction
    {
        private int TaskID;
        private int Ops;


        public Action3002(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3002, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("TaskID", ref TaskID)
                 && httpGet.GetInt("Ops", ref Ops, 1, 2))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserTask usertask = new GameDataCacheSet<UserTask>().FindKey(ContextUser.UserID, TaskID);
            StoryTaskInfo taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(TaskID);
            if(taskInfo==null)
            {
                return false;
            }
            if(usertask==null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                //ErrorInfo = LanguageManager.GetLang().St3002_NotFind;
                SaveLog(new Exception(string.Format("{0},User:{1},task:{2}", LanguageManager.GetLang().St3002_NotFind, ContextUser.UserID, TaskID)));
                return false;
            }
            ErrorCode = Ops;
            if (Ops == 1)
            {
                //领取
                if (usertask.TaskState != TaskState.AllowTake)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    //ErrorInfo = LanguageManager.GetLang().St3002_NoAllowTaked;
                    return false;
                }
                if (taskInfo.TaskLv > ContextUser.UserLv)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St3002_LvNotEnough;
                    return false;
                }

                TaskState taskState = TaskState.Taked;
                if (taskInfo.TermsType == PlotTermsType.Dialogue)
                {
                    //对话直接完成
                    taskState = TaskState.Completed;
                }
                if (new GameDataCacheSet<UserTask>().FindKey(usertask.UserID, usertask.TaskID) == null)
                {
                    usertask = new UserTask()
                    {
                        UserID = ContextUser.UserID,
                        TaskID = TaskID,
                        TaskType = taskInfo.TaskType,
                        TaskTargetNum = string.Empty,
                        TaskState = taskState,
                        TakeDate = DateTime.Now,
                        CompleteNum = 0,
                        CreateDate = DateTime.Now
                    };
                    new GameDataCacheSet<UserTask>().Add(usertask);
                }
                else
                {
                    usertask.TaskState = taskState;
                    usertask.TakeDate = DateTime.Now;
                    //usertask.Update();
                }
                PlotHelper.EnablePlot(Uid, taskInfo.PlotID);

            }
            else if (Ops == 2)
            {
                
                if (usertask.TaskState == TaskState.Completed)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St3002_Completed;
                    return false;
                }
                if (usertask.TaskState != TaskState.Taked)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    //ErrorInfo = LanguageManager.GetLang().St3002_NoTaked;
                    return false;

                }
                usertask.TaskTargetNum = string.Empty;
                usertask.TaskState = TaskState.AllowTake;
                //usertask.Update();
            }

            return true;
        }
    }
}