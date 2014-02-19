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
    /// 剧情任务配置信息下发
    /// </summary>
    public class Action3008 : BaseStruct
    {
        private int ClientVersion = 0;
        private List<StoryTaskInfo> taskList = new List<StoryTaskInfo>();

        public Action3008(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3008, httpGet)
        {
        }

        public override bool TakeAction()
        {
            int currVersion = new ConfigCacheSet<ConfigVersion>().FindKey(VersionType.StoryTask).CurVersion;
            taskList = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.Version > ClientVersion && m.Version <= currVersion);
            return true;
        }

        public override void BuildPacket()
        {
            this.PushIntoStack(taskList.Count);
            foreach (StoryTaskInfo taskInfo in taskList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(taskInfo.TaskID);
                ds.PushIntoStack(taskInfo.TaskName.ToNotNullString());
                ds.PushIntoStack(taskInfo.TaskType.ToShort());
                ds.PushIntoStack(taskInfo.TaskDescp.ToNotNullString());
                ds.PushIntoStack(taskInfo.ReleaseNpcID);
                ds.PushIntoStack(taskInfo.ReleaseDialogue.Count);
                foreach (DialogueInfo dialogueInfo in taskInfo.ReleaseDialogue)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(dialogueInfo.NPC.ToNotNullString());
                    dsItem.PushIntoStack(dialogueInfo.Role.ToNotNullString());

                    ds.PushIntoStack(dsItem);
                }
                ds.PushIntoStack(taskInfo.TakedDialogue.Count);
                foreach (DialogueInfo dialogueInfo in taskInfo.TakedDialogue)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(dialogueInfo.NPC.ToNotNullString());
                    dsItem.PushIntoStack(dialogueInfo.Role.ToNotNullString());

                    ds.PushIntoStack(dsItem);
                }

                ds.PushIntoStack(taskInfo.DeliveryNpcID);
                ds.PushIntoStack(taskInfo.DeliveryDialogue.Count);
                foreach (DialogueInfo dialogueInfo in taskInfo.DeliveryDialogue)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(dialogueInfo.NPC.ToNotNullString());
                    dsItem.PushIntoStack(dialogueInfo.Role.ToNotNullString());

                    ds.PushIntoStack(dsItem);
                }

                ds.PushIntoStack(taskInfo.TermsType.ToShort());
                ds.PushIntoStack(taskInfo.PlotID);
                ds.PushIntoStack(taskInfo.FunctionEnum.ToInt());
                ds.PushIntoStack(taskInfo.TaskLv);

                this.PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ClientVersion", ref ClientVersion))
            {
                return true;
            }
            return false;
        }
    }
}