using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ServiceStack.Common.Extensions;
using ZyGames.Framework.Model;

namespace FrameworkUnitTest.Cache.Model
{
    public enum ChatType
    {
        World = 0
    }

    [ProtoContract]
    [EntityTable(CacheType.Rank, MyDataConfigger.DbKey)]
    public class ChatData : RankEntity
    {
        private readonly ChatType _type;

        public ChatData()
        {
        }

        public ChatData(ChatType type)
        {
            _type = type;
        }

        public override string Key
        {
            get { return _type.ToString(); }
        }


        [ProtoMember(1)]
        public string Message { get; set; }

        [ProtoMember(2)]
        public int SenderId { get; set; }

        [ProtoMember(3)]
        public string SenderName { get; set; }

        [ProtoMember(4)]
        public int ReceiverId { get; set; }

        [ProtoMember(5)]
        public string ReceiverName { get; set; }

        [ProtoMember(6)]
        public DateTime SendTime { get; set; }
    }
}
