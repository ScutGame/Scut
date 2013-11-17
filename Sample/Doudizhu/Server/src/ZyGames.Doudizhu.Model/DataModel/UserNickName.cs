using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 玩家昵称缓存
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(CacheType.Entity, DbConfig.Data, "GameUser", IsStoreInDb = false)]
    public class UserNickName : ShareEntity
    {
        public UserNickName()
            : base(AccessLevel.ReadWrite)
        {
        }

        public UserNickName(int userId)
            : this()
        {
            _userId = userId;
        }

        private int _userId;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField("UserId", IsKey = true)]
        public int UserId
        {
            get
            {
                return _userId;
			}
			set
			{
				SetChange("UserId", value);
			}

        }

        private string _nickName;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField("NickName")]
        public string NickName
        {
            get
            {
                return _nickName;
            }
            set
            {
                SetChange("NickName", value);
            }
        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "UserId": return _userId;
                    case "NickName": return _nickName;
                    default: throw new ArgumentException(string.Format("UserNickName index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "UserId":
                        _userId = value.ToInt();
                        break;
                    case "NickName":
                        _nickName = value.ToNotNullString();
                        break;
                    default: throw new ArgumentException(string.Format("UserNickName index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }
    }
}
