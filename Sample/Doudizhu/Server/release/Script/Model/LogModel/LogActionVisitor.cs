
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
    public class LogActionVisitor : LogEntity
    {
        /// <summary>
        /// </summary>
        public LogActionVisitor()
        {
            
        }        
        /// <summary>
        /// </summary>
        public LogActionVisitor(long logID)
            : this()
        {
           LogID = logID;
        }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField( true, IsIdentity = true)]
        public long LogID{ get; set; }
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
        public int ActionID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public int ActionStat{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public string VisitUrl{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public string RespCont{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public string IP{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(8)]
        [EntityField]
        public DateTime VisitBeginTime{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(9)]
        [EntityField]
        public DateTime VisitEndTime{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(10)]
        [EntityField]
        public DateTime Addtime{ get; set; }

	}
}