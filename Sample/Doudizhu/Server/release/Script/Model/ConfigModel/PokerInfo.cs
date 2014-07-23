using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 扑克牌配置
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, DbConfig.Config)]
    public class PokerInfo : ShareEntity
    {

        /// <summary>
        /// </summary>
        public PokerInfo()
            : base(true)
        {
            
        }        

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(true)]
        public int Id{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public string Name{ get; set; }

        /// <summary>
        /// 牌花色[Enum<CardColor>]
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public CardColor Color{ get; set; }

        /// <summary>
        /// 牌大小值
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public short Value{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public string HeadIcon{ get; set; }
    
	}
}