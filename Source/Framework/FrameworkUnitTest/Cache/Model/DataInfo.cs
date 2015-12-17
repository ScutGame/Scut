using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Model;

namespace FrameworkUnitTest.Cache.Model
{

    [EntityTable(AccessLevel.ReadOnly, MyDataConfigger.DbKey)]
    public class DataInfo: ShareEntity
    {
        public DataInfo()
            : base(true)
        {
        }

        [EntityField(true)]
        public uint ChapterId { get; set; }

        [EntityField]
        public uint CheckPointId { get; set; }

    }
}
