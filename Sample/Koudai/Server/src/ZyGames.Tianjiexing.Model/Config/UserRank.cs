/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model.Config
{

    [Serializable, ProtoContract]
    public class UserRank : RankingItem
    {
        public UserRank()
        {
        }

        [ProtoMember(1)]
        public string UserID
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
        /// 竞技排行
        /// </summary>
        [ProtoMember(3)]
        public override int RankId
        {
            get;
            set;
        }

        /// <summary>
        /// 等级
        /// </summary>
        [ProtoMember(4)]
        public short UserLv
        {
            get;
            set;
        }

        /// <summary>
        /// 声望
        /// </summary>
        [ProtoMember(5)]
        public int ObtainNum
        {
            get;
            set;
        }

        /// <summary>
        /// 佣兵头像
        /// </summary>
        [ProtoMember(6)]
        public string HeadID { get; set; }


        /// <summary>
        /// 金币
        /// </summary>
        [ProtoMember(7)]
        public Int32 GameCoin { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        [ProtoMember(8)]
        public bool IsOnline
        {
            get;
            set;
        }

        /// <summary>
        /// 当前排行
        /// </summary>
        [ProtoMember(9)]
        public int CurrRank { get; set; }

        /// <summary>
        /// 战力
        /// </summary>
        [ProtoMember(10)]
        public int TotalCombatNum { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [ProtoMember(11)]
        public CountryType CountryID { get; set; }

        /// <summary>
        ///vip等级
        /// </summary>
        [ProtoMember(12)]
        public short VipLv { get; set; }

        /// <summary>
        /// 连胜次数
        /// </summary>
        [ProtoMember(13)]
        public int VictoryNum { get; set; }

        /// <summary>
        /// 竞技场积分
        /// </summary>
        [ProtoMember(14)]
        public int SportsIntegral { get; set; }

        /// <summary>
        /// 排名时间
        /// </summary>
        [ProtoMember(15)]
        public DateTime RankDate { get; set; }

        /// <summary>
        /// 得分                      圣吉塔排行榜
        /// </summary>
        [ProtoMember(16)]
        public int ScoreStar { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [ProtoMember(17)]
        public int  MaxTierNum { get; set; }

        /// <summary>
        /// 连续上榜天数
        /// </summary>
        [ProtoMember(18)]
        public int HaveRankNum { get; set; }

        /// <summary>
        /// 挑战结束时间
        /// </summary>
        [ProtoMember(20)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 排行类型
        /// </summary>
        [ProtoMember(21)]
        public SJTRankType SJTRankType { get; set; }
        /// <summary>
        /// 圣吉塔排名
        /// </summary>
        [ProtoMember(22)]
        public int SJTRankId { get; set; }


    }
}