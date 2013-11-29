using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Event;

namespace ZyGames.Doudizhu.Model
{
    [Serializable, ProtoContract]
    public class UserAchieveInfo : EntityChangeEvent
    {
        public UserAchieveInfo()
            : base(false)
        {
        }

        private int _AchieveID;

        /// <summary>
        /// 成就ID
        /// </summary>
        [ProtoMember(1)]
        public int AchieveID
        {
            get { return _AchieveID; }
            set
            {
                _AchieveID = value;
                NotifyByModify();
            }
        }

        private TaskStatus _TaskStatus;

        /// <summary>
        /// 成就状态
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
    }
}
