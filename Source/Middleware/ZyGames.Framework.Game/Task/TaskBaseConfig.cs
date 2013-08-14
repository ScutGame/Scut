using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Task
{
    /// <summary>
    /// 任务配置信息
    /// </summary>
    public abstract class TaskBaseConfig : ShareEntity
    {
        protected TaskBaseConfig()
            : base(AccessLevel.ReadOnly)
        {

        }
        protected TaskBaseConfig(int taskID)
            : base(AccessLevel.ReadOnly)
        {
            TaskID = taskID;
        }

        /// <summary>
        /// 任务配置
        /// </summary>
        public virtual int TaskID
        {
            get;
            private set;
        }
    }
}
