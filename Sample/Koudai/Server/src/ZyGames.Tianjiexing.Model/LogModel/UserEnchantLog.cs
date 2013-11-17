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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model.LogModel
{
    /// <summary>
    /// 玩家附魔符Log
    /// </summary>
    [Serializable, ProtoContract, EntityTable(CacheType.None, DbConfig.Log, "UserEnchantLog")]
    public class UserEnchantLog : BaseEntity
    {
        #region auto-generated static method
        static UserEnchantLog()
        {
            EntitySchemaSet.InitSchema(typeof(UserEnchantLog));
        }
        #endregion

        public UserEnchantLog()
            : base(AccessLevel.WriteOnly)
        {
        }

        #region auto-generated Property
        private String _ID;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("ID", IsKey = true)]
        public String ID
        {
            private get
            {
                return _ID;
            }
            set
            {
                SetChange("ID", value);
            }
        }
        private String _UserID;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("UserID")]
        public String UserID
        {
            private get
            {
                return _UserID;
            }
            set
            {
                SetChange("UserID", value);
            }
        }
        private Int16 _OpType;
        /// <summary>
        /// 操作类型 1：副本掉落 2：oa补偿 3：附魔服培养 4：附魔符合成 5：卖出附魔符 6：背包满掉落
        /// </summary>
        [EntityField("OpType")]
        public Int16 OpType
        {
            private get
            {
                return _OpType;
            }
            set
            {
                SetChange("OpType", value);
            }
        }

        private string _UserEnchantID;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("UserEnchantID")]
        public string UserEnchantID
        {
            private get
            {
                return _UserEnchantID;
            }
            set
            {
                SetChange("UserEnchantID", value);
            }
        }

        private Int32 _EnchantID;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("EnchantID")]
        public Int32 EnchantID
        {
            private get
            {
                return _EnchantID;
            }
            set
            {
                SetChange("EnchantID", value);
            }
        }

        private Int16 _EnchantLv;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("EnchantLv")]
        public Int16 EnchantLv
        {
            private get
            {
                return _EnchantLv;
            }
            set
            {
                SetChange("EnchantLv", value);
            }
        }

        private Int16 _MaxMature;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("MaxMature")]
        public Int16 MaxMature
        {
            private get
            {
                return _MaxMature;
            }
            set
            {
                SetChange("MaxMature", value);
            }
        }

        private Int32 _Experience;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("Experience")]
        public Int32 Experience
        {
            private get
            {
                return _Experience;
            }
            set
            {
                SetChange("Experience", value);
            }
        }

        private CacheList<SynthesisInfo> _SynthesisEnchant;
        /// <summary>
        /// 0:掉落，1增加，2使用,3出售，4出售删除 5合成 6购回 7传入仓库或取出物品后合并物品   8丢弃  9合成删除物品
        /// </summary>
        [EntityField("SynthesisEnchant", IsJsonSerialize = true)]
        public CacheList<SynthesisInfo> SynthesisEnchant
        {
            private get
            {
                return _SynthesisEnchant;
            }
            set
            {
                SetChange("SynthesisEnchant", value);
            }
        }
        private DateTime _CreateDate;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("CreateDate")]
        public DateTime CreateDate
        {
            private get
            {
                return _CreateDate;
            }
            set
            {
                SetChange("CreateDate", value);
            }
        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "ID": return ID;
                    case "UserID": return UserID;
                    case "OpType": return OpType;
                    case "UserEnchantID": return UserEnchantID;
                    case "EnchantID": return EnchantID;
                    case "EnchantLv": return EnchantLv;
                    case "MaxMature": return MaxMature;
                    case "Experience": return Experience;
                    case "SynthesisEnchant": return SynthesisEnchant;
                    case "CreateDate": return CreateDate;
                    default: throw new ArgumentException(string.Format("UserEnchnatLog index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "ID":
                        _ID = value.ToNotNullString();
                        break;
                    case "UserID":
                        _UserID = value.ToNotNullString();
                        break;
                    case "OpType":
                        _OpType = value.ToShort();
                        break;
                    case "UserEnchantID":
                        _UserEnchantID = value.ToNotNullString();
                        break;
                    case "EnchantID":
                        _EnchantID = value.ToInt();
                        break;
                    case "EnchantLv":
                        _EnchantLv = value.ToShort();
                        break;
                    case "MaxMature":
                        _MaxMature = value.ToShort();
                        break;
                    case "Experience":
                        _Experience = value.ToInt();
                        break;
                    case "SynthesisEnchant":
                        _SynthesisEnchant = ConvertCustomField<CacheList<SynthesisInfo>>(value, index);
                        break;
                    case "CreateDate":
                        _CreateDate = value.ToDateTime();
                        break;
                    default: throw new ArgumentException(string.Format("UserEnchnatLog index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion

        protected override int GetIdentityId()
        {
            return UserID.ToInt();
        }
    }
}