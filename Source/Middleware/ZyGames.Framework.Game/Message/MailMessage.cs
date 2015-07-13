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

namespace ZyGames.Framework.Game.Message
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable("", "", 0, "")]
    public class MailMessage : BaseEntity
    {
        /// <summary>
        /// </summary>
        public MailMessage()
            : base(AccessLevel.ReadWrite)
        {
        }

        /// <summary>
        /// </summary>
        public MailMessage(Guid MailID)
            : this()
        {
            this._MailID = MailID;
        }

        #region 自动生成属性
		/// <summary>
		/// The _ mail I.
		/// </summary>
        protected Guid _MailID;
        /// <summary>
        /// </summary>        
        [ProtoMember(1)]
        [EntityFieldExtend]
        [EntityField(true, DbType = ColumnDbType.UniqueIdentifier)]
        public virtual Guid MailID
        {
            get
            {
                return _MailID;
            }
            set
            {
                SetChange("MailID", value);
            }

        }
		/// <summary>
		/// The _ user identifier.
		/// </summary>
        protected Int32 _UserId;
        /// <summary>
        /// </summary>        
        [ProtoMember(2)]
        [EntityFieldExtend]
        [EntityField(true)]
        public virtual Int32 UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                SetChange("UserId", value);
            }
        }
		/// <summary>
		/// The type of the _ mail.
		/// </summary>
        protected Int32 _MailType;
        /// <summary>
        /// </summary>        
        [ProtoMember(3)]
        [EntityFieldExtend]
        [EntityField]
        public virtual Int32 MailType
        {
            get
            {
                return _MailType;
            }
            set
            {
                SetChange("MailType", value);
            }
        }
		/// <summary>
		/// The _ from user identifier.
		/// </summary>
        protected Int32 _FromUserId;
        /// <summary>
        /// </summary>        
        [ProtoMember(4)]
        [EntityFieldExtend]
        [EntityField]
        public virtual Int32 FromUserId
        {
            get
            {
                return _FromUserId;
            }
            set
            {
                SetChange("FromUserId", value);
            }
        }
		/// <summary>
		/// The name of the _ from user.
		/// </summary>
        protected String _FromUserName;
        /// <summary>
        /// </summary>        
        [ProtoMember(5)]
        [EntityFieldExtend]
        [EntityField]
        public virtual String FromUserName
        {
            get
            {
                return _FromUserName;
            }
            set
            {
                SetChange("FromUserName", value);
            }
        }
		/// <summary>
		/// The _ to user I.
		/// </summary>
        protected Int32 _ToUserID;
        /// <summary>
        /// </summary>        
        [ProtoMember(6)]
        [EntityFieldExtend]
        [EntityField]
        public virtual Int32 ToUserID
        {
            get
            {
                return _ToUserID;
            }
            set
            {
                SetChange("ToUserID", value);
            }
        }
		/// <summary>
		/// The name of the _ to user.
		/// </summary>
        protected String _ToUserName;
        /// <summary>
        /// </summary>        
        [ProtoMember(7)]
        [EntityFieldExtend]
        [EntityField]
        public virtual String ToUserName
        {
            get
            {
                return _ToUserName;
            }
            set
            {
                SetChange("ToUserName", value);
            }
        }
		/// <summary>
		/// The _ title.
		/// </summary>
        protected String _Title;
        /// <summary>
        /// </summary>        
        [ProtoMember(8)]
        [EntityFieldExtend]
        [EntityField]
        public virtual String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                SetChange("Title", value);
            }
        }
		/// <summary>
		/// The content of the _.
		/// </summary>
        protected String _Content;
        /// <summary>
        /// </summary>        
        [ProtoMember(9)]
        [EntityFieldExtend]
        [EntityField]
        public virtual String Content
        {
            get
            {
                return _Content;
            }
            set
            {
                SetChange("Content", value);
            }
        }
		/// <summary>
		/// The _ send date.
		/// </summary>
        protected DateTime _SendDate;
        /// <summary>
        /// </summary>        
        [ProtoMember(10)]
        [EntityFieldExtend]
        [EntityField]
        public virtual DateTime SendDate
        {
            get
            {
                return _SendDate;
            }
            set
            {
                SetChange("SendDate", value);
            }
        }
		/// <summary>
		/// The _ is read.
		/// </summary>
        protected Boolean _IsRead;
        /// <summary>
        /// </summary>        
        [ProtoMember(11)]
        [EntityFieldExtend]
        [EntityField]
        public virtual Boolean IsRead
        {
            get
            {
                return _IsRead;
            }
            set
            {
                SetChange("IsRead", value);
            }
        }
		/// <summary>
		/// The _ is remove.
		/// </summary>
        protected Boolean _IsRemove;
        /// <summary>
        /// </summary>        
        [ProtoMember(12)]
        [EntityFieldExtend]
        [EntityField]
        public virtual Boolean IsRemove
        {
            get
            {
                return _IsRemove;
            }
            set
            {
                SetChange("IsRemove", value);
            }
        }
		/// <summary>
		/// The _ remove date.
		/// </summary>
        protected DateTime _RemoveDate;
        /// <summary>
        /// </summary>        
        [ProtoMember(13)]
        [EntityFieldExtend]
        [EntityField]
        public virtual DateTime RemoveDate
        {
            get
            {
                return _RemoveDate;
            }
            set
            {
                SetChange("RemoveDate", value);
            }
        }
		/// <summary>
		/// 对象索引器属性
		/// </summary>
		/// <returns></returns>
		/// <param name="index">Index.</param>
        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "MailID": return MailID;
                    case "UserId": return UserId;
                    case "MailType": return MailType;
                    case "FromUserId": return FromUserId;
                    case "FromUserName": return FromUserName;
                    case "ToUserID": return ToUserID;
                    case "ToUserName": return ToUserName;
                    case "Title": return Title;
                    case "Content": return Content;
                    case "SendDate": return SendDate;
                    case "IsRead": return IsRead;
                    case "IsRemove": return IsRemove;
                    case "RemoveDate": return RemoveDate;
                    default: throw new ArgumentException(string.Format("MailMessage index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "MailID":
                        _MailID = (Guid)value;
                        break;
                    case "UserId":
                        _UserId = value.ToInt();
                        break;
                    case "MailType":
                        _MailType = value.ToInt();
                        break;
                    case "FromUserId":
                        _FromUserId = value.ToInt();
                        break;
                    case "FromUserName":
                        _FromUserName = value.ToNotNullString();
                        break;
                    case "ToUserID":
                        _ToUserID = value.ToInt();
                        break;
                    case "ToUserName":
                        _ToUserName = value.ToNotNullString();
                        break;
                    case "Title":
                        _Title = value.ToNotNullString();
                        break;
                    case "Content":
                        _Content = value.ToNotNullString();
                        break;
                    case "SendDate":
                        _SendDate = value.ToDateTime();
                        break;
                    case "IsRead":
                        _IsRead = value.ToBool();
                        break;
                    case "IsRemove":
                        _IsRemove = value.ToBool();
                        break;
                    case "RemoveDate":
                        _RemoveDate = value.ToDateTime();
                        break;
                    default: throw new ArgumentException(string.Format("MailMessage index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion
		/// <summary>
		/// Gets the identity identifier.
		/// </summary>
		/// <returns>The identity identifier.</returns>
        protected override int GetIdentityId()
        {
            return UserId;
        }
    }
}