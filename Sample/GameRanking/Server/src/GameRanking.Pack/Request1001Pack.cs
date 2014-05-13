using System;
using ProtoBuf;

namespace GameRanking.Pack
{
    [ProtoContract]
    public class Request1001Pack
    {
        [ProtoMember(101)]
        public int PageIndex { get; set; }

        [ProtoMember(102)]
        public int PageSize { get; set; }
    }
}
