using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Model;

namespace GameRanking.Model
{
    /// <summary>
    /// 玩家排行榜实体类
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(CacheType.Entity, "ConnData")]
    public class UserRanking : ShareEntity
    {
        public UserRanking()
            : base(false)
        {
            CreateDate = DateTime.Now;
        }

        [ProtoMember(1)]
        [EntityField(true)]
        public int UserID
        {
            get;
            set;
        }

        [ProtoMember(2)]
        [EntityField]
        public string UserName
        {
            get;
            set;
        }

        [ProtoMember(3)]
        [EntityField]
        public int Score
        {
            get;
            set;
        }

        [ProtoMember(4)]
        [EntityField]
        public DateTime CreateDate
        {
            get;
            set;
        }

        protected override int GetIdentityId()
        {
            return UserID;
        }
    }
}