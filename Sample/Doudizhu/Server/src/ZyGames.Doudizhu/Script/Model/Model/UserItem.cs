using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Event;

namespace ZyGames.Doudizhu.Model
{
    [Serializable, ProtoContract]
    public class UserItem : EntityChangeEvent
    {
        public UserItem()
            : base(false)
        {
        }
        
        /// <summary>
        /// 玩家物品ID
        /// </summary>
        [ProtoMember(1)]
        public string UserItemID { get; set; }

        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(2)]
        public int ItemID { get; set; }


        /// <summary>
        /// 物品数量
        /// </summary>
        [ProtoMember(3)]
        public int Num { get; set; }

        /// <summary>
        /// 物品类型
        /// </summary>
        [ProtoMember(4)]
        public ShopType ShopType { get; set; }
    }
}
