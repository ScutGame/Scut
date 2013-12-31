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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Tianjiexing.BLL.GM
{
    class EnableTaskCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int taskID = args.Length > 0 ? args[0].Trim().ToInt() : 0;
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(UserID);
            if (user == null)
            {
                UserCacheGlobal.Load(UserID);
                user = new GameDataCacheSet<GameUser>().FindKey(UserID);
            }
            var taskList = StoryTaskInfo.GetNextTask(taskID);
            foreach (StoryTaskInfo taskInfo in taskList)
            {
                if (taskInfo.CountryID != CountryType.None && user.CountryID != taskInfo.CountryID)
                {
                    continue;
                }
                var cacheSet = new GameDataCacheSet<UserTask>();
                UserTask userTask = cacheSet.FindKey(UserID, taskInfo.TaskID);
                if (userTask == null)
                {
                    userTask = new UserTask
                    {
                        TaskID = taskInfo.TaskID,
                        UserID = UserID,
                        TaskType = taskInfo.TaskType,
                        TaskState = TaskState.AllowTake,
                        CreateDate = DateTime.Now
                    };
                    cacheSet.Add(userTask);
                }
                else
                {
                    userTask.TaskState = TaskState.AllowTake;
                }
            }

        }
    }
}