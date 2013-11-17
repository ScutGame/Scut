/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Event;

namespace ZyGames.Tianjiexing.Model.Config
{
    [Serializable, ProtoContract]
    public class UserItemInfo : EntityChangeEvent, IComparable<UserItemInfo>
    {

        public UserItemInfo()
            : base(false)
        {
        }

        [ProtoMember(1)]
        public string UserItemID
        {
            get;
            set;
        }

        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(2)]
        public int ItemID
        {
            get;
            set;
        }

        /// <summary>
        /// 物品类别
        /// </summary>
        [ProtoMember(3)]
        public ItemType ItemType
        {
            get;
            set;
        }

        /// <summary>
        /// 物品等级
        /// </summary>
        [ProtoMember(4)]
        public short ItemLv
        {
            get;
            set;
        }

        /// <summary>
        /// 物品数量
        /// </summary>
        [ProtoMember(5)]
        public int Num
        {
            get;
            set;
        }

        /// <summary>
        /// 1.物品在背包，2.物品在仓库，3装备在佣兵身上 4卖出
        /// </summary>
        [ProtoMember(6)]
        public ItemStatus ItemStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 佣兵或用户ID
        /// </summary>
        [ProtoMember(7)]
        public int GeneralID
        {
            get;
            set;
        }

        /// <summary>
        /// 售出时间
        /// </summary>
        [ProtoMember(8)]
        public DateTime SoldDate
        {
            get;
            set;
        }

        [ProtoMember(9)]
        public bool IsRemove
        {
            get;
            set;
        }

        public Int16 PropType
        {
            get
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(ItemID);
                if (itemInfo != null)
                {
                    return itemInfo.PropType;
                }
                return 0;
            }
        }

        /// <summary>
        /// 是否能够使用
        /// </summary>
        public Boolean CanUse
        {
            get
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(ItemID);
                if (itemInfo != null)
                {
                    return itemInfo.IsUse;
                }
                return false;
            }
        }

        public EquParts Equparts
        {
            get
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(ItemID);
                if (itemInfo != null)
                {
                    return itemInfo.EquParts;
                }
                return 0;
            }
        }

        public bool IsNotUsed
        {
            get { return GeneralID == 0; }
        }

        // 获取装备品质
        public QualityType ItemQuality
        {
            get
            {
                ItemBaseInfo itemInfo = null;
                itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(ItemID);

                if (itemInfo != null)
                {
                    return itemInfo.QualityType;
                }
                return QualityType.BaiSe;
            }
        }

        public int CompareTo(UserItemInfo item)
        {
            int result = 0;
            if (this == null && item == null) return 0;
            if (this != null && item == null) return 1;
            if (this == null && item != null) return -1;

            result = ((int)ItemType).CompareTo((int)item.ItemType);
            if (result == 0)
            {
                var itemInfo1 = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID) ?? new ItemBaseInfo();
                var itemInfo2 = new ConfigCacheSet<ItemBaseInfo>().FindKey(this.ItemID) ?? new ItemBaseInfo();

                result = ((int)itemInfo1.QualityType).CompareTo((int)itemInfo2.QualityType);
                if (result == 0)
                {
                    result = ((int)item.ItemLv).CompareTo((int)ItemLv);
                }
            }

            return result;
        }

    }
}