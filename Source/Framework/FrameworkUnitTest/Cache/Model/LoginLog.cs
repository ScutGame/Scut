using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Model;

namespace FrameworkUnitTest.Cache.Model
{
    [EntityTable(AccessLevel.WriteOnly, MyDataConfigger.DbKey, Indexs = new[] { "UserId" })]
    public class LoginLog :  LogEntity
    {
        [EntityField(true, IsIdentity = true)]
        public long LogId { get; set; }

        [EntityField]
        public int UserId { get; set; }

        [EntityField]
        public string NickName { get; set; }

        [EntityField]
        public ushort RoleLv { get; set; }

        [EntityField]
        public string RetailId { get; set; }

        [EntityField]
        public DateTime CreateTime { get; set; }
    }
}
