
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
	[EntityTable(AccessLevel.WriteOnly, DbConfig.Log)]
    public class OAOperationLog : LogEntity
    {
        /// <summary>
        /// </summary>
        public OAOperationLog()
        {
            
        }        
        /// <summary>
        /// </summary>
        public OAOperationLog(string iD)
            : this()
        {
            ID = iD;
        }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(IsKey = true)]
        public string ID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public string UserID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public short OpType{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public string Reason{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public DateTime EndDate{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public int OpUserID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public DateTime CreateDate { get; set; }
        

	}
}