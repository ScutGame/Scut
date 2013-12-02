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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.Model.DataModel
{
    /// <summary>
    /// 玩家拉新卡奖励表
    /// </summary>
    [Serializable, ProtoContract, EntityTable(DbConfig.Data, "UserCardReward", DbConfig.PeriodTime, DbConfig.PersonalName)]

    public class UserCardReward : BaseEntity
    {
        public const string Index_UserID = "Index_UserID";

        public UserCardReward()
            : base(AccessLevel.ReadWrite)
        {

        }

        #region auto-generated Property

        public int _ID;
        [ProtoMember(1)]
        [EntityField("ID", IsKey = true, IsIdentity = true)]
        public int ID
        {
            get { return _ID; }
            set { SetChange("ID", value); }
        }

        private string _UserID;
        /// <summary>
        /// 用户ID
        /// </summary>
        [ProtoMember(2)]
        [EntityField("UserID")]
        public string UserID
        {
            get { return _UserID; }
            set { SetChange("UserID", value); }
        }

        private string _CardUserID;
        /// <summary>
        ///  拉新卡用户ID
        /// </summary>
        [ProtoMember(3)]
        [EntityField("CardUserID")]
        public string CardUserID
        {
            get { return _CardUserID; }
            set { SetChange("CardUserID", value); }
        }

        private Int16 _UserLv;
        /// <summary>
        /// 新手等级
        /// </summary>
        [ProtoMember(4)]
        [EntityField("UserLv")]
        public Int16 UserLv
        {
            get { return _UserLv; }
            set { SetChange("UserLv", value); }
        }

        private DateTime _CreateDate;

        /// <summary>
        /// 领取时间
        /// </summary>
        [ProtoMember(5)]
        [EntityField("CreateDate")]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { SetChange("CreateDate", value); }
        }

        protected override int GetIdentityId()
        {
            return UserID.ToInt();
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
                    case "CardUserID": return CardUserID;
                    case "UserLv": return UserLv;
                    case "CreateDate":
                        return CreateDate;
                    default: throw new ArgumentException(string.Format("UserCardReward index[{0}] isn't exist.", index));
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
                    case "UserID":
                        _UserID = value.ToNotNullString();
                        break;
                    case "CardUserID":
                        _CardUserID = value.ToNotNullString();
                        break;
                    case "UserLv":
                        _UserLv = value.ToShort();
                        break;
                    case "CreateDate":
                        _CreateDate = value.ToDateTime();
                        break;
                    default: throw new ArgumentException(string.Format("UserCardReward index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }
        #endregion
    }
}