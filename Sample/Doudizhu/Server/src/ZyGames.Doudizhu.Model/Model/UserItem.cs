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
        
        private string _UserItemID;

        /// <summary>
        /// 玩家物品ID
        /// </summary>
        [ProtoMember(1)]
        public string UserItemID
        {
            get { return _UserItemID; }
            set
            {
                _UserItemID = value;
                NotifyByModify();
            }
        }

        private int _ItemID;

        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(2)]
        public int ItemID
        {
            get { return _ItemID; }
            set
            {
                _ItemID = value;
                NotifyByModify();
            }
        }

        private int _Num;

        /// <summary>
        /// 物品数量
        /// </summary>
        [ProtoMember(3)]
        public int Num
        {
            get { return _Num; }
            set
            {
                _Num = value;
                NotifyByModify();
            }
        }

        private ShopType _ShopType;

        /// <summary>
        /// 物品类型
        /// </summary>
        [ProtoMember(4)]
        public ShopType ShopType
        {
            get { return _ShopType; }
            set
            {
                _ShopType = value;
                NotifyByModify();
            }
        }
    }
}
