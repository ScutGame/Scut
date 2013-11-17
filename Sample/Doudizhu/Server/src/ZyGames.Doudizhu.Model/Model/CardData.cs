using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 牌面数据
    /// </summary>
    [Serializable, ProtoContract]
    public class CardData
    {
        public CardData(int userId, int posId)
        {
            UserId = userId;
            PosId = posId;
            Cards = new int[0];
            Type = DeckType.None;
        }

        public int PosId { get; private set; }

        public int UserId { get; private set; }

        public DeckType Type { get; set; }

        /// <summary>
        /// 牌面最小值
        /// </summary>
        public int CardSize { get; set; }

        public int[] Cards { get; set; }
    }
}
