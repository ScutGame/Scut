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

        private int _TaskID;

        /// <summary>
        /// 任务ID
        /// </summary>
        [ProtoMember(1)]
        public int TaskID
        {
            get { return _TaskID; }
            set
            {
                _TaskID = value;
                NotifyByModify();
            }
        }

        private TaskStatus _TaskStatus;

        /// <summary>
        /// 任务状态
        /// </summary>
        [ProtoMember(2)]
        public TaskStatus TaskStatus
        {
            get { return _TaskStatus; }
            set
            {
                _TaskStatus = value;
                NotifyByModify();
            }
        }

        private int _TaskNum;

        /// <summary>
        /// 任务进度
        /// </summary>
        [ProtoMember(3)]
        public int TaskNum
        {
            get { return _TaskNum; }
            set
            {
                _TaskNum = value;
                NotifyByModify();
            }
        }

        private DateTime _CreateDate;

        /// <summary>
        /// 任务完成时间
        /// </summary>
        [ProtoMember(4)]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set
            {
                _CreateDate = value;
                NotifyByModify();
            }
        }

        private DateTime _RefreashDate;

        /// <summary>
        /// 任务刷新时间
        /// </summary>
        [ProtoMember(5)]
        public DateTime RefreshDate
        {
            get { return _RefreashDate; }
            set
            {
                _RefreashDate = value;
                NotifyByModify();
            }
        }

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
