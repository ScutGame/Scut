using System.Collections.Generic;
using ProtoBuf;

namespace GameRanking.Pack
{
    [ProtoContract]
    public class Response1001Pack
    {
        [ProtoMember(101)]
        public int PageCount { get; set; }

        public List<RankData> Items { get; set; }
    }

    [ProtoContract]
    public class RankData
    {

        [ProtoMember(101)]
        public string UserName { get; set; }

        [ProtoMember(102)]
        public int Score { get; set; }
    }
}
