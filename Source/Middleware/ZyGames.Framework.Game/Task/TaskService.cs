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
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Task
{
    /// <summary>
    /// 游戏任务服务
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// def get(userId, currTaskId):
    ///     return list;
    /// 
    /// def acceptTask(userId, taskId):#接受任务
    ///     return True;
    /// 
    /// def deliveryTask(userId, taskId):#交付任务
    ///     return True;
    /// 
    /// def receivePrize(userId, taskId):#领取奖励
    ///     return True;
    /// 
    /// ]]>
    /// </example>
    public class TaskService<T, TC>
        where T : ITaskItem
        where TC : TaskBaseConfig
    {
        /// <summary>
        /// The user identifier.
        /// </summary>
        protected readonly int UserId;
        private dynamic _taskScope;
        private bool _isUsedPy;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public TaskService(int userId)
        {
            UserId = userId;
            _isUsedPy = InitTask();
        }
        /// <summary>
        /// Inits the task.
        /// </summary>
        /// <returns><c>true</c>, if task was inited, <c>false</c> otherwise.</returns>
        protected bool InitTask()
        {
            _taskScope = ScriptEngines.Execute("Lib.Task", null);
            return _taskScope != null;
        }

        /// <summary>
        /// 获取玩家未结束的任务
        /// </summary>
        /// <returns></returns>
        public virtual List<T> Get(int currTaskId)
        {
            List<T> taskList = new List<T>();
            if (_isUsedPy)
            {
               var list = _taskScope.get(UserId, currTaskId);
                foreach (var item in list)
                {
                    taskList.Add((T)item);
                }
            }
            return taskList;
        }

        /// <summary>
        /// 接受任务
        /// </summary>
        /// <returns></returns>
        public bool Accept(int taskId)
        {
            if (_isUsedPy)
            {
                return _taskScope.acceptTask(UserId, taskId);
            }
            return false;
        }

        /// <summary>
        /// 交付
        /// </summary>
        /// <returns></returns>
        public bool Delivery(int taskId)
        {
            if (_isUsedPy)
            {
                return _taskScope.deliveryTask(UserId, taskId);
            }
            return false;
        }

        /// <summary>
        /// 领取奖励
        /// </summary>
        public bool ReceivePrize(int taskId)
        {
            if (_isUsedPy)
            {
                return _taskScope.receivePrize(UserId, taskId);
            }
            return false;
        }
    }
}