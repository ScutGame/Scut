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
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Event;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model.Config
{
    [Serializable, ProtoContract]
    public class UserPlotInfo : EntityChangeEvent, IComparable<UserPlotInfo>
    {
        public UserPlotInfo()
            : base(false)
        {
            ItemList = new CacheList<UniversalInfo>();
            ArcheologyPackage = new CacheList<UserPlotArcheology>();
        }

        /// <summary>
        /// 副本编号
        /// </summary>
        [ProtoMember(1)]
        public Int32 PlotID { get; set; }

        /// <summary>
        /// 副本状态，PlotStatus枚举
        /// </summary>
        [ProtoMember(2)]
        public PlotStatus PlotStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 副本评分
        /// </summary>
        [ProtoMember(3)]
        public short ScoreNum
        {
            get;
            set;
        }
        /// <summary>
        /// 攻击分（不使用）
        /// </summary>
        [ProtoMember(4)]
        public short AttackScore
        {
            get;
            set;
        }

        /// <summary>
        /// 防御分（不使用）
        /// </summary>
        [ProtoMember(5)]
        public short DefenseScore
        {
            get;
            set;
        }

        /// <summary>
        /// 星级评价
        /// </summary>
        [ProtoMember(6)]
        public Int16 StarScore
        {
            get;
            set;
        }

        /// <summary>
        /// 通关阅历
        /// </summary>
        [ProtoMember(7)]
        public Int32 ExpNum
        {
            get;
            set;
        }

        /// <summary>
        /// 通关经验
        /// </summary>
        [ProtoMember(8)]
        public Int32 Experience
        {
            get;
            set;
        }

        /// <summary>
        /// 宝箱金币奖励
        /// </summary>
        [ProtoMember(9)]
        public Int32 PennyNum
        {
            get;
            set;
        }

        /// <summary>
        /// 宝箱的晶石
        /// </summary>
        [ProtoMember(10)]
        public Int32 GoldNum
        {
            get;
            set;
        }

        /// <summary>
        /// 宝箱的物品ID
        /// </summary>
        [ProtoMember(11)]
        public Int32 ItemID
        {
            get;
            set;
        }
        /// <summary>
        /// 通关时间
        /// </summary>
        [ProtoMember(12)]
        public DateTime CompleteDate
        {
            get;
            set;
        }

        /// <summary>
        /// 刷新时间
        /// </summary>
        [ProtoMember(13)]
        public DateTime RefleshDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [ProtoMember(14)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 祝福金币奖励
        /// </summary>
        [ProtoMember(15)]
        public Int32 BlessPennyNum
        {
            get;
            set;
        }

        /// <summary>
        /// 祝福通关经验
        /// </summary>
        [ProtoMember(16)]
        public Int32 BlessExperience
        {
            get;
            set;
        }

        /// <summary>
        /// 附魔符id
        /// </summary>
        [ProtoMember(17)]
        public Int32 EnchantID
        {
            get;
            set;
        }

        /// <summary>
        /// 副本胜利类型
        /// </summary>
        [ProtoMember(18)]
        public PlotSuccessType PlotSuccessType { get; set; }

        /// <summary>
        /// 副本失败类型
        /// </summary>
        [ProtoMember(19)]
        public PlotFailureType PlotFailureType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(20)]
        public Int32 HonourNum { get; set; }

        /// <summary>
        /// 荣誉值
        /// </summary>
        [ProtoMember(21)]
        public CacheList<UniversalInfo> ItemList
        {
            get;
            set;
        }

        /// <summary>
        /// 副本次数
        /// </summary>
        [ProtoMember(22)]
        public Int32 PlotNum { get; set; }

        /// <summary>
        /// 副本类型
        /// </summary>
        public PlotType PlotType
        {
            get
            {
                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(PlotID);
                if (plotInfo != null)
                {
                    return plotInfo.PlotType;
                }
                return PlotType.Normal;
            }
        }

        private int _hasMapCount;

        /// <summary>
        /// 得到的宝图碎片总数
        /// </summary>
        public Int32 HasMapCount
        {
            get { return _hasMapCount; }
            set
            {
                _hasMapCount = value;
                NotifyByModify();
            }
        }

        private CacheList<UserPlotArcheology> _archeologyPackage;

        /// <summary>
        /// 九宫格怪物信息
        /// </summary>
        public CacheList<UserPlotArcheology> ArcheologyPackage
        {
            get { return _archeologyPackage; }
            set
            {
                _archeologyPackage = value;
                NotifyByModify();
            }
        }



        private int _currPlotNpcId;

        /// <summary>
        /// Boss NPC ID
        /// </summary>
        public Int32 CurrPlotNpcID
        {
            get { return _currPlotNpcId; }
            set
            {
                _currPlotNpcId = value;
                NotifyByModify();
            }
        }

        private short _bossChallengeCount;

        /// <summary>
        /// Boss 挑战次数
        /// </summary>
        public Int16 BossChallengeCount
        {
            get { return _bossChallengeCount; }
            set
            {
                _bossChallengeCount = value;
                NotifyByModify();
            }
        }

        private bool _isFirstIn;

        /// <summary>
        /// 是否第一次进入地图
        /// </summary>
        public Boolean IsFirstIn
        {
            get { return _isFirstIn; }
            set
            {
                _isFirstIn = value;
                NotifyByModify();
            }
        }

        private bool _isChallengeBossWin;
        /// <summary>
        /// Boss 是否已经战胜
        /// </summary>
        public Boolean HasChallengeBossWin
        {
            get { return _isChallengeBossWin; }
            set
            {
                _isChallengeBossWin = value;
                NotifyByModify();

            }
        }

        private int _playAnimat;
        /// <summary>
        /// 是否已经播放过动画
        /// </summary>
        public int PlayAnimat
        {
            get { return _playAnimat; }
            set
            {
                _playAnimat = value;
                NotifyByModify();
            }
        }

        private bool _firstWin;
        /// <summary>
        /// 精英副本首次获得3星
        /// </summary>
        public bool FirstWin
        {
            get { return _firstWin; }
            set
            {
                _firstWin = value;
                NotifyByModify();
            }
        }


        public int CompareTo(UserPlotInfo other)
        {
            if (this == null && other == null) return 0;
            if (this != null && other == null) return 1;
            if (this == null && other != null) return -1;

            return PlotID.CompareTo(other.PlotID);
        }
    }
}