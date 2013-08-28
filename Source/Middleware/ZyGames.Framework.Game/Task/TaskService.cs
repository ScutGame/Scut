using System;
using System.Collections.Generic;
using System.IO;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Plugin.PythonScript;

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
        protected readonly int UserId;
        private PythonContext _taskContext;
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

        protected bool InitTask()
        {
            string path;
            var manager = PythonScriptManager.Current;
            if (manager.TryGetLib("task", out path))
            {
                if (manager.TryLoadPython(path, out _taskContext))
                {
                    return true;
                }
            }
            path = Path.Combine("Lib", "Task.py");
            return manager.TryLoadPython(path, out _taskContext);
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
                IronPython.Runtime.List list = _taskContext.Scope.get(UserId, currTaskId);
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
                return _taskContext.Scope.acceptTask(UserId, taskId);
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
                return _taskContext.Scope.deliveryTask(UserId, taskId);
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
                return _taskContext.Scope.receivePrize(UserId, taskId);
            }
            return false;
        }
    }
}
