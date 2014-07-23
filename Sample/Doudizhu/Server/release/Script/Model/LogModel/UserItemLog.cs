using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, ProtoContract]
	[EntityTable(AccessLevel.WriteOnly, DbConfig.Log, "UserItemLog")]
    public class UserItemLog : LogEntity
    {
        /// <summary>
        /// </summary>
        public UserItemLog()
        {
            
        }        
        /// <summary>
        /// </summary>
        public UserItemLog(long iD)
            : this()
        {
            ID = iD;
        }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(true, IsIdentity = true)]
        public long ID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public int Uid{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public string UserItemID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public int OpType{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public int ItemID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public int Num{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public DateTime CreateDate { get; set; }
        

	}
}