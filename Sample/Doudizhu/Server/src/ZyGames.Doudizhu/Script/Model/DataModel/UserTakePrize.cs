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
    [EntityTable(CacheType.Entity, DbConfig.Data, Condition = "IsTasked=0")]
    public class UserTakePrize : ShareEntity
    {

        /// <summary>
        /// </summary>
        public UserTakePrize()
            : base(false)
        {
            
        }        
        /// <summary>
        /// </summary>
        public UserTakePrize(string iD)
            : this()
        {
            ID = iD;
        }
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(true)]
        public string ID{ get; set; }

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
        public int GameCoin{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public int Gold{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public string ItemPackage{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public string MailContent{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public bool IsTasked{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(8)]
        [EntityField]
        public DateTime TaskDate{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(9)]
        [EntityField]
        public int OpUserID{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(10)]
        [EntityField]
        public DateTime CreateDate{ get; set; }
    
	}
}