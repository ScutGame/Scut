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

        /// <summary>
        /// 成就ID
        /// </summary>
        [ProtoMember(1)]
        public int AchieveID { get; set; }

        /// <summary>
        /// 成就状态
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
    }
}
