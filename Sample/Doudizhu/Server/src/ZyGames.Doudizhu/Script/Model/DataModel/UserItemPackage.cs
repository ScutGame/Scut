using System;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 玩家物品表
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(DbConfig.Data)]
    public class UserItemPackage : BaseEntity
    {

        /// <summary>
        /// </summary>
        public UserItemPackage()
            : base(false)
        {
            ItemPackage = new CacheList<UserItem>();
        }
        /// <summary>
        /// </summary>
        public UserItemPackage(int userID)
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
        [EntityField(true, ColumnDbType.Text)]
        public CacheList<UserItem> ItemPackage { get; set; }

        protected override int GetIdentityId()
        {
            return UserID;
        }

    }
}