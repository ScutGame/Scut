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
using System.Runtime.Serialization;
using ProtoBuf;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model.DataModel
{
    /// <summary>
    /// 玩家大转盘奖励表
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(DbConfig.Data, "UserDial", DbConfig.PeriodTime, DbConfig.PersonalName)]
    public class UserDial : BaseEntity
    {
        public UserDial()
            : base(AccessLevel.ReadWrite)
        {
            PrizeInfo = new TreasureInfo();
            Treasure = new CacheList<TreasureInfo>();
        }

        //protected override void BindChangeEvent()
        //{
        //    PrizeInfo.BindParentChangeEvent(this);
        //    Treasure.BindParentChangeEvent(this);
        //}

        public UserDial(String UserID)
            : this()
        {
            this.UserID = UserID;
        }
        #region 自动生成属性

        private string _UserID;

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [EntityField("UserID", IsKey = true)]
        public string UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                SetChange("UserID", value);
            }
        }

        private TreasureInfo _PrizeInfo;
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(2)]
        [EntityField("PrizeInfo", IsJsonSerialize = true)]
        public TreasureInfo PrizeInfo
        {
            get
            {
                return _PrizeInfo;
            }
            set
            {
                SetChange("PrizeInfo", value);
            }
        }

        private decimal _ReturnRatio;
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(3)]
        [EntityField("ReturnRatio")]
        public decimal ReturnRatio
        {
            get
            {
                return _ReturnRatio;
            }
            set
            {
                SetChange("ReturnRatio", value);
            }
        }

        private string _HeadID;

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(4)]
        [EntityField("HeadID")]
        public string HeadID
        {
            get
            {
                return _HeadID;
            }
            set
            {
                SetChange("HeadID", value);
            }
        }


        private Int16 _DialNum;

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(5)]
        [EntityField("DialNum")]
        public Int16 DialNum
        {
            get
            {
                return _DialNum;
            }
            set
            {
                SetChange("DialNum", value);
            }
        }


        private DateTime _RefreshDate;

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(6)]
        [EntityField("RefreshDate")]
        public DateTime RefreshDate
        {
            get
            {
                return _RefreshDate;
            }
            set
            {
                SetChange("RefreshDate", value);
            }
        }


        private CacheList<TreasureInfo> _Treasure;

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(7)]
        [EntityField("Treasure", IsJsonSerialize = true)]
        public CacheList<TreasureInfo> Treasure
        {
            get
            {
                return _Treasure;
            }
            set
            {
                SetChange("Treasure", value);
            }
        }

        private int _GroupID;

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(8)]
        [EntityField("GroupID")]
        public int GroupID
        {
            get
            {
                return _GroupID;
            }
            set
            {
                SetChange("GroupID", value);
            }
        }

        [ProtoMember(9)]
        public string UserItemID { get; set; }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "UserID": return UserID;
                    case "PrizeInfo": return PrizeInfo;
                    case "ReturnRatio": return ReturnRatio;
                    case "HeadID": return HeadID;
                    case "DialNum": return DialNum;
                    case "RefreshDate": return RefreshDate;
                    case "Treasure": return Treasure;
                    case "GroupID": return GroupID;
                    default: throw new ArgumentException(string.Format("UserDial index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "UserID":
                        _UserID = value.ToNotNullString();
                        break;
                    case "PrizeInfo":
                        _PrizeInfo = ConvertCustomField<TreasureInfo>(value, index);
                        break;
                    case "ReturnRatio": _ReturnRatio = value.ToDecimal();
                        break;
                    case "HeadID": _HeadID = value.ToNotNullString();
                        break;
                    case "DialNum":
                        _DialNum = value.ToShort();
                        break;
                    case "RefreshDate":
                        _RefreshDate = value.ToDateTime();
                        break;
                    case "Treasure": _Treasure = ConvertCustomField<CacheList<TreasureInfo>>(value, index);
                        break;
                    case "GroupID":
                        _GroupID = value.ToInt();
                        break;
                    default: throw new ArgumentException(string.Format("UserDial index[{0}] isn't exist.", index));
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