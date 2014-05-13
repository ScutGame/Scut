using System;
using ProtoBuf;

namespace GameRanking.Pack
{
    [ProtoContract]
    public class MessagePack
    {
        [ProtoMember(1)]
        public int MsgId { get; set; }

        [ProtoMember(2)]
        public int ActionId { get; set; }

        [ProtoMember(3)]
        public string SessionId { get; set; }

        [ProtoMember(4)]
        public int UserId { get; set; }

    }
}
