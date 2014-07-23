
using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, ProtoContract]
	[EntityTable(AccessLevel.WriteOnly, DbConfig.Log)]
    public class MallItemLog : LogEntity
    {
        /// <summary>
        /// </summary>
        public MallItemLog()
        {

        }
        /// <summary>
        /// </summary>
        public MallItemLog(long iD)
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
        public int ItemID{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public int Uid{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public int Num{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public int CurrencyType{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public int Amount{ get; set; }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public DateTime CreateDate{ get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>        
        [ProtoMember(8)]
        [EntityField]
        public String RetailID{ get; set; }
        /// <summary>
        /// 手机类型
        /// </summary>        
        [ProtoMember(9)]
        [EntityField]
        public MobileType MobileType{ get; set; }
        /// <summary>
        /// 通行证ID
        /// </summary>        
        [ProtoMember(10)]
        [EntityField]
        public String Pid{ get; set; }
        /// <summary>
        /// 物品名称
        /// </summary>        
        [ProtoMember(11)]
        [EntityField]
        public String ItemName{ get; set; }

    }
}