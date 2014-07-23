
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
    public class UserGoldLog : LogEntity
    {
        /// <summary>
        /// </summary>
        public UserGoldLog()
        {
            
        }        
        /// <summary>
        /// </summary>
        public UserGoldLog(long iD)
            : this()
        {
            ID = iD;
        }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField("ID", IsKey = true)]
        public long ID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public int UserID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public short BalanceType{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public short CurrencyType{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public short OpType{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public int DetailID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public int Num{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(8)]
        [EntityField]
        public int Gold{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(9)]
        [EntityField]
        public int BanGold{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(10)]
        [EntityField]
        public int SurplusGold{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(11)]
        [EntityField]
        public int SurplusBanGold{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(12)]
        [EntityField]
        public DateTime CreateDate { get; set; }
        

	}
}