using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Common;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 桌子的座位对象
    /// </summary>
    [Serializable, ProtoContract]
    public class PositionData
    {
        private int _id;

        public PositionData(int id)
        {
            _id = id;
            _cardData = new List<int>();
            Init();
        }

        /// <summary>
        /// 初始化AI
        /// </summary>
        public void InitAI(int roomId, int tableId, int userId, string nickName, string head)
        {
            var ai = new GameUser(userId);
            ai.NickName = nickName;
            ai.HeadIcon = head;
            ai.Property.InitTablePos(roomId, tableId, _id);
            Init(ai, true);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(GameUser user = null, bool isAI = false)
        {
            UserId = user == null ? 0 : user.UserId;
            NickName = user == null ? "" : user.NickName;
            HeadIcon = user == null ? "" : user.HeadIcon;
            IsAI = isAI;
            ReSendCard();
        }

        /// <summary>
        /// 重新发牌
        /// </summary>
        public void ReSendCard()
        {
            ScoreNum = 0;
            CoinNum = 0;
            IsShow = false;
            IsLandlord = false;
            OutTwoTimes = 0;
            _cardData.Clear();
        }

        /// <summary>
        /// 座位编号
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }
        }
        /// <summary>
        /// 是否机器人或托管玩家
        /// </summary>
        public bool IsAI { get; set; }

        /// <summary>
        /// 是否地主
        /// </summary>
        public bool IsLandlord { get; set; }

        /// <summary>
        /// 是否明牌
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HeadIcon { get; set; }
        /// <summary>
        /// 增加或扣除积分
        /// </summary>
        public int ScoreNum { get; set; }
        /// <summary>
        /// 增加或扣除金豆 
        /// </summary>
        public int CoinNum { get; set; }

        /// <summary>
        /// 对家剩余2张牌时，配合对家出对次数
        /// </summary>
        public int OutTwoTimes { get; set; }

        private List<int> _cardData;

        /// <summary>
        /// 牌数据
        /// </summary>
        public List<int> CardData
        {
            get { return _cardData; }
        }

    }
}
