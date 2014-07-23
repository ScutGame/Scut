using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 每日限制表
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(DbConfig.Data)]
    public class UserDailyRestrain : BaseEntity
    {

        /// <summary>
        /// </summary>
        public UserDailyRestrain()
            : base(false)
        {
            RestrainProperty = new RestrainProperty();
        }        
        /// <summary>
        /// </summary>
        public UserDailyRestrain(int userId)
            : this()
        {
            UserId = userId;
        }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(true)]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public DateTime RefreshDate{ get; set; }

        /// <summary>
        /// 玩家限制属性[RestrainProperty]
        /// </summary>        
        [ProtoMember(3)]
        [EntityField( true, ColumnDbType.Text)]
        public RestrainProperty RestrainProperty{ get; set; }

        protected override int GetIdentityId()
        {
            return UserId;
        }        

	}
}