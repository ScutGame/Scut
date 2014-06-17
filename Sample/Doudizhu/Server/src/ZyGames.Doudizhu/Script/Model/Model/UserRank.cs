using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 玩家排行
    /// </summary>
    [Serializable, ProtoContract]
    public class UserRank : RankingItem
    {
        [ProtoMember(1)]
        public int UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家昵称
        /// </summary>
        [ProtoMember(2)]
        public string NickName
        {
            get;
            set;
        }

        /// <summary>
        /// 排行
        /// </summary>
        [ProtoMember(3)]
        public override int RankId
        {
            get;
            set;
        }

        /// <summary>
        /// 金豆
        /// </summary>
        [ProtoMember(4)]
        public int GameCoin
        {
            get;
            set;
        }

        /// <summary>
        /// 胜利局数
        /// </summary>
        [ProtoMember(5)]
        public int WinNum { get; set; }

        /// <summary>
        /// 失败局数
        /// </summary>
        [ProtoMember(6)]
        public int FailNum { get; set; }

        /// <summary>
        /// 胜率
        /// </summary>
        [ProtoMember(7)]
        public decimal Wining { get; set; }
    }
}
