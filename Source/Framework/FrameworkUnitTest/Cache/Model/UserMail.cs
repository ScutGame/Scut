using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ZyGames.Framework.Model;

namespace FrameworkUnitTest.Cache.Model
{
    [Serializable]
    [ProtoContract]
    [EntityTable(CacheType.Dictionary, MyDataConfigger.DbKey)]
    public class UserMail: BaseEntity
    {
        [ProtoMember(1)]
        [EntityField(true)]
        public long UserId { get; set; }

        [ProtoMember(2)]
        [EntityField(true)]
        public Guid MailId { get; set; }

        [ProtoMember(3)]
        [EntityField]
        public bool Enable { get; set; }

        protected override int GetIdentityId()
        {
            return (int) UserId;
        }
    }
}
