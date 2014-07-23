using System;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(DbConfig.Data)]
    public class UserTask : BaseEntity
    {

        /// <summary>
        /// </summary>
        public UserTask()
            : base(false)
        {
            TaskPackage = new CacheList<UserTaskInfo>();
        }
        /// <summary>
        /// </summary>
        public UserTask(int userID)
            : this()
        {
            UserID = userID;
        }


        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(true)]
        public int UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField( true,  ColumnDbType.Text)]
        public CacheList<UserTaskInfo> TaskPackage { get; set; }

        protected override int GetIdentityId()
        {
            return UserID;
        }

    }
}