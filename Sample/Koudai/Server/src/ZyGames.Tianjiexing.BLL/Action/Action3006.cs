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
using System.Data;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3006_日常任务领取放弃接口
    /// </summary>
    public class Action3006 : BaseAction
    {
        private int Ops;
        private int TaskID;


        public Action3006(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3006, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref Ops, 1, 2) &&
                httpGet.GetInt("TaskID", ref TaskID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserTask userTask = new GameDataCacheSet<UserTask>().FindKey(ContextUser.UserID, TaskID);
            if (userTask == null || userTask.TaskState == TaskState.NoTake || userTask.TaskState == TaskState.Disable)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                //this.ErrorInfo = LanguageManager.GetLang().St3002_NotFind;
                return false;
            }
            if (Ops == 1 && TaskHelper.GetTaskCompleteNum(ContextUser.UserID) >= TaskHelper.DailyEveryMaxNum)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St3005_CompletedTimeout;
                return false;
            }
            this.ErrorCode = Ops;

            if (Ops == 1)
            {
                //领取
                userTask.TaskState = TaskState.Taked;
                userTask.TakeDate = DateTime.Now;
            }
            else if (Ops == 2)
            {
                //放弃
                userTask.TaskTargetNum = string.Empty;
                userTask.TaskState = TaskState.AllowTake;
            }
            //userTask.Update();
            List<UserTask> userTaskList = new GameDataCacheSet<UserTask>().FindAll(ContextUser.UserID, m => m.TaskType.Equals(TaskType.Daily) && !m.TaskID.Equals(TaskID) && (m.TaskState == TaskState.Completed || m.TaskState == TaskState.Taked));

            foreach (UserTask item in userTaskList)
            {
                item.TaskState = TaskState.AllowTake;
                //item.Update();
            }
            return true;
        }
    }
}