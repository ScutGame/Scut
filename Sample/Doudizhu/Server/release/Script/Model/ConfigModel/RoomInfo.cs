using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 房间配置
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, DbConfig.Config)]
    public class RoomInfo : ShareEntity
    {

        /// <summary>
        /// </summary>
        public RoomInfo()
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
        /// 底注分
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public int AnteNum{ get; set; }

        /// <summary>
        /// 房间倍数
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public short MultipleNum{ get; set; }

        /// <summary>
        /// 金豆数限制
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public int MinGameCion{ get; set; }

        /// <summary>
        /// 每日赠豆,0:不赠送
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public int GiffCion{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public string Description{ get; set; }

        /// <summary>
        /// 玩家人数
        /// </summary>        
        [ProtoMember(8)]
        [EntityField]
        public int PlayerNum{ get; set; }
        /// <summary>
        /// 卡牌几副
        /// </summary>        
        [ProtoMember(9)]
        [EntityField]
        public int CardPackNum{ get; set; }
    
	}
}