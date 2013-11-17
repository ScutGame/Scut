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
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Tianjiexing.Model.ConfigModel
{
    /// <summary>
    /// 大转盘奖励配置表
    /// </summary>
    [Serializable, ProtoContract, EntityTable(AccessLevel.ReadOnly, DbConfig.Config, "DialInfo")]
    public class DialInfo : ShareEntity
    {
        public const string Index_GroupID = "Index_GroupID";
        public const string Index_DialType = "Index_DialType";
        public const string Index_ID = "Index_ID";
        public DialInfo()
            : base(AccessLevel.ReadOnly)
        {
        }

        #region auto-generated Property

        private Int32 _ID;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("ID", IsKey = true)]
        public Int32 ID
        {
            get
            {
                return _ID;
            }
            private set
            {
                SetChange("ID", value);
            }
        }

        private Int32 _GroupID;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("GroupID")]
        public Int32 GroupID
        {
            get
            {
                return _GroupID;
            }
            private set
            {
                SetChange("GroupID", value);
            }
        }

        private Int16 _DialType;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("DialType")]
        public Int16 DialType
        {
            get
            {
                return _DialType;
            }
            private set
            {
                SetChange("DialType", value);
            }
        }

        private RewardType _RewardType;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("RewardType")]
        public RewardType RewardType
        {
            get
            {
                return _RewardType;
            }
            private set
            {
                SetChange("RewardType", value);
            }
        }

        private Int32 _ItemID;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("ItemID")]
        public Int32 ItemID
        {
            get
            {
                return _ItemID;
            }
            private set
            {
                SetChange("ItemID", value);
            }
        }

        private decimal _ItemNum;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("ItemNum")]
        public decimal ItemNum
        {
            get
            {
                return _ItemNum;
            }
            private set
            {
                SetChange("ItemNum", value);
            }
        }

        private string _HeadID;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("HeadID")]
        public string HeadID
        {
            get
            {
                return _HeadID;
            }
            private set
            {
                SetChange("HeadID", value);
            }
        }

        private decimal _Probability;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("Probability")]
        public decimal Probability
        {
            get
            {
                return _Probability;
            }
            private set
            {
                SetChange("Probability", value);
            }
        }

        private Boolean _IsBroadcast;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("IsBroadcast")]
        public Boolean IsBroadcast
        {
            get
            {
                return _IsBroadcast;
            }
            private set
            {
                SetChange("IsBroadcast", value);
            }
        }

        private string _BroadContent;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("BroadContent")]
        public string BroadContent
        {
            get
            {
                return _BroadContent;
            }
            private set
            {
                SetChange("BroadContent", value);
            }
        }

        private Boolean _IsShow;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("IsShow")]
        public Boolean IsShow
        {
            get
            {
                return _IsShow;
            }
            private set
            {
                SetChange("IsShow", value);
            }
        }

        private string _ItemDesc;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("ItemDesc")]
        public string ItemDesc
        {
            get
            {
                return _ItemDesc;
            }
            private set
            {
                SetChange("ItemDesc", value);
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
                    case "GroupID": return GroupID;
                    case "DialType": return DialType;
                    case "RewardType": return RewardType;
                    case "ItemID": return ItemID;
                    case "ItemNum": return ItemNum;
                    case "HeadID": return HeadID;
                    case "Probability": return Probability;
                    case "IsBroadcast": return IsBroadcast;
                    case "BroadContent": return BroadContent;
                    case "IsShow": return IsShow;
                    case "ItemDesc": return ItemDesc;
                    default: throw new ArgumentException(string.Format("DialInfo index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "ID":
                        _ID = value.ToInt();
                        break;
                    case "GroupID":
                        _GroupID = value.ToInt();
                        break;
                    case "DialType":
                        _DialType = value.ToShort();
                        break;
                    case "RewardType":
                        _RewardType = value.ToEnum<RewardType>();
                        break;
                    case "ItemID":
                        _ItemID = value.ToInt();
                        break;
                    case "ItemNum":
                        _ItemNum = value.ToDecimal();
                        break;
                    case "HeadID":
                        _HeadID = value.ToNotNullString();
                        break;
                    case "Probability":
                        _Probability = value.ToDecimal();
                        break;
                    case "IsBroadcast":
                        _IsBroadcast = value.ToBool();
                        break;
                    case "BroadContent":
                        _BroadContent = value.ToNotNullString();
                        break;
                    case "IsShow":
                        _IsShow = value.ToBool();
                        break;
                    case "ItemDesc":
                        _ItemDesc = value.ToNotNullString();
                        break;
                    default: throw new ArgumentException(string.Format("DialInfo index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion

        protected override int GetIdentityId()
        {
            return DefIdentityId;
        }
    }
}