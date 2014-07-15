using System.Collections.Generic;
using ProtoBuf;

namespace GameRanking.Pack
{
    [ProtoContract]
    public class Response1001Pack
    {
        public Response1001Pack()
        {
            Items = new List<RankData>();
        }

        [ProtoMember(101)]
        public int PageCount { get; set; }

        [ProtoMember(102)]
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
