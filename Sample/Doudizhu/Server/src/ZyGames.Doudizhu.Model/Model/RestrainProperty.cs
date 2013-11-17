using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Event;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 玩家限制属性
    /// </summary>
    [Serializable, ProtoContract]
    public class RestrainProperty : EntityChangeEvent
    {
        public RestrainProperty()
            : base(false)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            DailyGiffCoinTime = 0;
            DialFreeNum = 0;
        }

        private int _dailyGiffCoinTime;

        /// <summary>
        /// 每日赠送金豆次数
        /// </summary>
        [ProtoMember(1)]
        public int DailyGiffCoinTime
        {
            get { return _dailyGiffCoinTime; }
            set
            {
                _dailyGiffCoinTime = value;
                NotifyByModify();
            }
        }

        private int _dialFreeNum;

        /// <summary>
        /// 每日转盘免费次数
        /// </summary>
        [ProtoMember(2)]
        public int DialFreeNum
        {
            get { return _dialFreeNum; }
            set
            {
                _dialFreeNum = value;
                NotifyByModify();
            }
        }
    }
}
