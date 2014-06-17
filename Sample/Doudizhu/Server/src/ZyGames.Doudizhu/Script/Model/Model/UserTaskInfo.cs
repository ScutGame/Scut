using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Event;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Doudizhu.Model
{

    [Serializable, ProtoContract]
    public class UserTaskInfo : EntityChangeEvent
    {
        public UserTaskInfo()
            : base(false)
        {

        }

        /// <summary>
        /// 任务ID
        /// </summary>
        [ProtoMember(1)]
        public int TaskID { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [ProtoMember(2)]
        public TaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 任务进度
        /// </summary>
        [ProtoMember(3)]
        public int TaskNum { get; set; }

        /// <summary>
        /// 任务完成时间
        /// </summary>
        [ProtoMember(4)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 任务刷新时间
        /// </summary>
        [ProtoMember(5)]
        public DateTime RefreshDate { get; set; }

        public TaskType TaskType
        {
            get
            {
                var taskInfo = new ConfigCacheSet<TaskInfo>().FindKey(TaskID);
                return taskInfo == null ? TaskType.Mature : taskInfo.TaskType;
            }
        }

    }
}
