using ProtoBuf;
using ZyGames.Framework.Model;

namespace FrameworkUnitTest.Cache.Model
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    [EntityTable(CacheType.Rank, "", StorageType = StorageType.ReadWriteRedis)]
    public class LvRank : RankEntity
    {
        public const string Lv = "Lv";
        public override string Key
        {
            get { return Lv; }
        }

        [ProtoMember(1)]
        public int UserId { get; set; }


        public override int GetHashCode()
        {
            return UserId;
        }
    }
}
