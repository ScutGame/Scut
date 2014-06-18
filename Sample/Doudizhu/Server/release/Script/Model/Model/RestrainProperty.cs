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

        /// <summary>
        /// 每日赠送金豆次数
        /// </summary>
        [ProtoMember(1)]
        public int DailyGiffCoinTime { get; set; }


        /// <summary>
        /// 每日转盘免费次数
        /// </summary>
        [ProtoMember(2)]
        public int DialFreeNum { get; set; }
    }
}
