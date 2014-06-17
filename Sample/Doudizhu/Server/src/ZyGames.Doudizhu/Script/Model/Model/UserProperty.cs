using System;
using ProtoBuf;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 玩家属性
    /// </summary>
    [Serializable, ProtoContract]
    public class UserProperty
    {
        public UserProperty()
        {
        }

        [ProtoMember(1)]
        public int GameId { get; set; }

        [ProtoMember(2)]
        public int ServerId { get; set; }

        /// <summary>
        /// 是否加载中
        /// </summary>
        [ProtoMember(3)]
        public bool IsRefreshing { get; set; }

        /// <summary>
        /// 玩家聊天时间
        /// </summary>
        [ProtoMember(4)]
        public DateTime ChatDate { get; set; }

        /// <summary>
        /// [临时缓存]聊天版本
        /// </summary>
        [ProtoMember(5)]
        public int ChatVesion { get; set; }

        /// <summary>
        /// [临时缓存]广播版本
        /// </summary>
        [ProtoMember(6)]
        public int BroadcastVesion { get; set; }

        /// <summary>
        /// [临时缓存]房间ID
        /// </summary>
        [ProtoMember(7)]
        public int RoomId { get; set; }

        /// <summary>
        /// [临时缓存]桌子ID
        /// </summary>
        [ProtoMember(8)]
        public int TableId { get; set; }

        /// <summary>
        /// [临时缓存]桌子座位ID
        /// </summary>
        [ProtoMember(9)]
        public int PositionId { get; set; }

        /// <summary>
        /// [临时缓存]当前斗地主局数
        /// </summary>
        [ProtoMember(10)]
        public int InningNum { get; set; }


        /// <summary>
        /// 初始化桌子
        /// </summary>
        public void InitTablePos(int roomid = 0, int tableId = 0, int posId = 0)
        {
            RoomId = roomid;
            TableId = tableId;
            PositionId = posId;
        }
    }
}
