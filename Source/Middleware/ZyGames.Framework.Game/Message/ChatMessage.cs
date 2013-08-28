using System;
using ProtoBuf;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// 聊天消息
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadWrite, CacheType.Queue, false)]
    public class ChatMessage : ShareEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatMessage()
            : base(AccessLevel.ReadWrite)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1001)]
        public virtual int Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1002)]
        public virtual string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1003)]
        public virtual DateTime SendDate { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        [ProtoMember(1004)]
        public virtual int FromUserID { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        [ProtoMember(1005)]
        public virtual int ToUserID { get; set; }


        protected override int GetIdentityId()
        {
            return DefIdentityId;
        }

        protected override object this[string index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
