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
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 玩家附魔
    /// </summary>
    [Serializable, ProtoContract]
    public class UserEnchantInfo : JsonEntity
    {
        public UserEnchantInfo()
        {

        }

        /// <summary>
        /// 武器最多镶嵌6个附魔符
        /// </summary>
        public const short EnchantMaxGrid = 6;


        [ProtoMember(1)]
        public Int32 EnchantID
        {
            get;
            set;
        }
        /// <summary>
        /// 附魔符唯一标识
        /// </summary>
        [ProtoMember(2)]
        public string UserEnchantID { get; set; }

        /// <summary>
        /// 附魔符放置的武器唯一标识
        /// </summary>
        [ProtoMember(3)]
        public string UserItemID { get; set; }

        /// <summary>
        /// 灵件位置
        /// </summary>
        [ProtoMember(4)]
        public short Position { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [ProtoMember(5)]
        public short EnchantLv { get; set; }

        /// <summary>
        /// 經驗
        /// </summary>
        [ProtoMember(6)]
        public int CurrExprience { get; set; }

        /// <summary>
        /// 成长率
        /// </summary>
        [ProtoMember(7)]
        public Int16 MaxMature { get; set; }


        public AbilityType AbilityType
        {
            get
            {
                EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(EnchantID);
                if (enchantInfo != null)
                {
                    return enchantInfo.AbilityType;
                }
                return Model.AbilityType.Empty;
            }
        }

        public void SetPosition(short pos)
        {
            pos = pos > 0 ? pos : (short)1;
            Position = pos > EnchantMaxGrid ? EnchantMaxGrid : pos;
        }

        public int CompareTo(UserEnchantInfo item)
        {
            int result = 0;
            if (this == null && item == null) return 0;
            if (this != null && item == null) return 1;
            if (this == null && item != null) return -1;

            result = ((int)item.EnchantLv).CompareTo((int)EnchantLv);
            if (result == 0)
            {
                result = item.CurrExprience.CompareTo(item.CurrExprience);
            }

            return result;
        }
    }
}