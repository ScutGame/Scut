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
    public class UserAchieve : BaseEntity
    {

        /// <summary>
        /// </summary>
        public UserAchieve()
            : base(false)
        {
            AchievePackage = new CacheList<UserAchieveInfo>();
        }

        public UserAchieve(int userid)
            :this()
        {
            UserID = userid;
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
        [EntityField( true, ColumnDbType.Text)]
        public CacheList<UserAchieveInfo> AchievePackage { get; set; }


        protected override int GetIdentityId()
        {
            return UserID;
        }

    }
}