using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 商店信息配置
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, DbConfig.Config)]
    public class ShopInfo : ShareEntity
    {

        /// <summary>
        /// </summary>
        public ShopInfo()
            : base(true)
        {

        }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField( true)]
        public int ShopID{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public string ShopName{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public string HeadID{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public ShopType ShopType{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public short SeqNO{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public int Price{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public int VipPrice{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(8)]
        [EntityField]
        public int GameCoin{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(9)]
        [EntityField]
        public string ShopDesc{ get; set; }

    }
}