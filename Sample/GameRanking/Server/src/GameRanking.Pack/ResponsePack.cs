using System;
using ProtoBuf;

namespace GameRanking.Pack
{
    [ProtoContract]
    public class ResponsePack
    {
        [ProtoMember(1)]
        public int MsgId { get; set; }

        [ProtoMember(2)]
        public int ActionId { get; set; }

        [ProtoMember(10)]
        public int ErrorCode { get; set; }
        [ProtoMember(11)]
        public string ErrorInfo { get; set; }

    }

}
