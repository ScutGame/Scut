using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ProtoBuf;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 聊天
    /// </summary>
    [Serializable, ProtoContract]
    public class ChatData : ChatMessage
    {
        /// <summary>
        /// 发信人名称
        /// </summary>
        [ProtoMember(1)]
        public string FromUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 聊天类型
        /// </summary>
        [ProtoMember(2)]
        public ChatType ChatType
        {
            get;
            set;
        }

        /// <summary>
        /// 房间ID
        /// </summary>
        [ProtoMember(3)]
        public int RoomId
        {
            get; 
            set;
        }

        /// <summary>
        /// 桌子ID
        /// </summary>
        [ProtoMember(4)]
        public int TableId
        {
            get; 
            set;
        }

        /// <summary>
        /// 聊天ID
        /// </summary>
        [ProtoMember(5)]
        public int ChatID
        {
            get;
            set;
        }
    }
}
