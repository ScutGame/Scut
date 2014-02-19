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
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3004_NPC任务列表接口
    /// </summary>
    public class Action3004 : BaseAction
    {
        private int NpcID;
        private List<UserTask> userTaskList = new List<UserTask>();

        public Action3004(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3004, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(userTaskList.Count);
            foreach (UserTask userTask in userTaskList)
            {
                StoryTaskInfo taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(userTask.TaskID);

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(taskInfo.TaskID);
                dsItem.PushIntoStack(taskInfo.TaskName.ToNotNullString());
                dsItem.PushIntoStack((short)userTask.TaskState);
                dsItem.PushIntoStack((short)taskInfo.TaskType);
                dsItem.PushIntoStack(taskInfo.Experience);
                dsItem.PushIntoStack(taskInfo.GameCoin);

                var dialogueList = new List<DialogueInfo>();
                if (userTask.TaskState == TaskState.AllowTake && taskInfo.ReleaseDialogue != null)
                {
                    dialogueList = taskInfo.ReleaseDialogue.ToList();
                }
                else if (userTask.TaskState == TaskState.Taked && taskInfo.TakedDialogue != null)
                {
                    dialogueList = taskInfo.TakedDialogue.ToList();
                }
                else if (userTask.TaskState == TaskState.Completed && taskInfo.DeliveryDialogue != null)
                {
                    dialogueList = taskInfo.DeliveryDialogue.ToList();
                }

                dsItem.PushIntoStack(dialogueList.Count);
                foreach (DialogueInfo dialogue in dialogueList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(dialogue.NPC);
                    dsItem1.PushIntoStack(dialogue.Role);

                    dsItem.PushIntoStack(dsItem1);
                }

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("NpcID", ref NpcID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            userTaskList = new GameDataCacheSet<UserTask>().FindAll(ContextUser.UserID,
                m =>
                {
                    StoryTaskInfo taskInfo = new ConfigCacheSet<StoryTaskInfo>().FindKey(m.TaskID);
                    return m.TaskType < TaskType.Daily && (taskInfo != null && (m.TaskState == TaskState.Completed && taskInfo.DeliveryNpcID == NpcID) || ((m.TaskState == TaskState.AllowTake || m.TaskState == TaskState.Taked) && taskInfo.ReleaseNpcID == NpcID));
                }
            );
            return true;
        }
    }
}