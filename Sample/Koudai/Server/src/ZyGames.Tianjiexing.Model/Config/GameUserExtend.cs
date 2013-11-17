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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Event;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// GameUser 新增字段表
    /// </summary>
    [Serializable, ProtoContract]
    public class GameUserExtend : EntityChangeEvent
    {
        public GameUserExtend()
            : base(false)
        {
            UserShops = new CacheList<MysteryShops>();
            ItemList = new CacheList<PrizeItemInfo>();
        }

        private DateTime _refreashDate;

        /// <summary>
        /// 刷新时间
        /// </summary>
        [ProtoMember(1)]
        public DateTime RefreashDate
        {
            get { return _refreashDate; }
            set
            {
                _refreashDate = value;
                NotifyByModify();
            }
        }

        private CacheList<PrizeItemInfo> _itemList;

        /// <summary>
        /// 每日获得材料
        /// </summary>
        [ProtoMember(2)]
        public CacheList<PrizeItemInfo> ItemList
        {
            get { return _itemList; }
            set
            {
                _itemList = value;
                NotifyByModify();
            }
        }

        private int _gainBlessing;

        /// <summary>
        /// 祝福次数
        /// </summary>
        [ProtoMember(3)]
        public int GainBlessing
        {
            get { return _gainBlessing; }
            set
            {
                _gainBlessing = value;
                NotifyByModify();
            }
        }

        private CacheList<MysteryShops> _userShops;

        /// <summary>
        /// 黑市商店刷新
        /// </summary>
        [ProtoMember(4)]
        public CacheList<MysteryShops> UserShops
        {
            get { return _userShops; }
            set
            {
                _userShops = value;
                NotifyByModify();
            }
        }

        private DailyRefresh _dailyInfo;

        /// <summary>
        /// 每日刷新
        /// </summary>
        [ProtoMember(5)]
        public DailyRefresh DailyInfo
        {
            get { return _dailyInfo; }
            set
            {
                _dailyInfo = value;
                NotifyByModify();
            }
        }

        private int _grayCrystalNum;

        /// <summary>
        /// 灰色水晶个数
        /// </summary>
        [ProtoMember(6)]
        public int GrayCrystalNum
        {
            get { return _grayCrystalNum; }
            set
            {
                _grayCrystalNum = value;
                NotifyByModify();
            }
        }


        private int _lightPetId;

        /// <summary>
        /// 当前点亮的宠物
        /// </summary>
        [ProtoMember(7)]
        public int LightPetID
        {
            get { return _lightPetId; }
            set
            {
                _lightPetId = value;
                NotifyByModify();
            }
        }

        private short _sparePartGridNum;

        /// <summary>
        /// 灵件背包格子数
        /// </summary>
        [ProtoMember(8)]
        public short SparePartGridNum
        {
            get { return _sparePartGridNum; }
            set
            {
                _sparePartGridNum = value;
                NotifyByModify();
            }
        }

        private int _lingshiNum;

        /// <summary>
        /// 灵石
        /// </summary>
        [ProtoMember(9)]
        public int LingshiNum
        {
            get { return _lingshiNum; }
            set
            {
                _lingshiNum = value;
                NotifyByModify();
            }
        }

        private int _maxLayerNum;

        /// <summary>
        /// 最高层数
        /// </summary>
        [ProtoMember(10)]
        public int MaxLayerNum
        {
            get { return _maxLayerNum; }
            set
            {
                _maxLayerNum = value;
                NotifyByModify();
            }
        }

        private int _layerNum;

        /// <summary>
        /// 天地劫层数
        /// </summary>
        [ProtoMember(11)]
        public int LayerNum
        {
            get { return _layerNum; }
            set
            {
                _layerNum = value;
                NotifyByModify();
            }
        }

        private int _hurdleNum;

        /// <summary>
        /// 天地劫关数
        /// </summary>
        [ProtoMember(12)]
        public int HurdleNum
        {
            get { return _hurdleNum; }
            set
            {
                _hurdleNum = value;
                NotifyByModify();
            }
        }


        private int _victoryNum;

        /// <summary>
        /// 竞技场连胜数
        /// </summary>
        [ProtoMember(13)]
        public int VictoryNum
        {
            get { return _victoryNum; }
            set
            {
                _victoryNum = value;
                NotifyByModify();
            }
        }

        private int _plotStatusId;

        /// <summary>
        /// 副本ID
        /// </summary>
        [ProtoMember(14)]
        public int PlotStatusID
        {
            get { return _plotStatusId; }
            set
            {
                _plotStatusId = value;
                NotifyByModify();
            }
        }

        private int _plotNpcId;

        /// <summary>
        /// 副本NpcID
        /// </summary>
        [ProtoMember(15)]
        public int PlotNpcID
        {
            get { return _plotNpcId; }
            set
            {
                _plotNpcId = value;
                NotifyByModify();
            }
        }

        private int _mercenarySeq;

        /// <summary>
        /// 出现的次序
        /// </summary>
        [ProtoMember(16)]
        public int MercenarySeq
        {
            get { return _mercenarySeq; }
            set
            {
                _mercenarySeq = value;
                NotifyByModify();
            }
        }

        private string _cardUserId;

        /// <summary>
        /// 玩家拉新卡号
        /// </summary>
        [ProtoMember(17)]
        public string CardUserID
        {
            get { return _cardUserId; }
            set
            {
                _cardUserId = value;
                NotifyByModify();
            }
        }

        private bool _isBoss;

        /// <summary>
        /// 当前副本是否boss
        /// </summary>
        [ProtoMember(18)]
        public bool IsBoss
        {
            get { return _isBoss; }
            set
            {
                _isBoss = value;
                NotifyByModify();
            }
        }

        private int _moJingNum;

        /// <summary>
        /// 魔晶
        /// </summary>
        [ProtoMember(19)]
        public int MoJingNum
        {
            get { return _moJingNum; }
            set
            {
                _moJingNum = value;
                NotifyByModify();
            }
        }


        private int _enchantGridNum;

        /// <summary>
        /// 附魔背包格子数
        /// </summary>
        [ProtoMember(20)]
        public int EnchantGridNum
        {
            get { return _enchantGridNum; }
            set
            {
                _enchantGridNum = value;
                NotifyByModify();
            }
        }

        private int _trumpCombat;

        /// <summary>
        /// 法宝战斗次数
        /// </summary>
        [ProtoMember(21)]
        public int TrumpCombat
        {
            get { return _trumpCombat; }
            set
            {
                _trumpCombat = value;
                NotifyByModify();
            }
        }

        private bool _BaiLanse;

        /// <summary>
        /// 百里挑一首招得蓝色品质佣兵
        /// </summary>
        [ProtoMember(22)]
        public bool BaiLanse
        {
            get { return _BaiLanse; }
            set
            {
                _BaiLanse = value;
                NotifyByModify();
            }
        }

        private bool _GoldenZise;

        /// <summary>
        /// 千载难逢首招得紫色品质佣兵
        /// </summary>
        [ProtoMember(23)]
        public bool GoldenZise
        {
            get { return _GoldenZise; }
            set
            {
                _GoldenZise = value;
                NotifyByModify();
            }
        }

        private int _LairNum;

        /// <summary>
        /// 专家取宝当前次数
        /// </summary>
        [ProtoMember(24)]
        public int LairNum
        {
            get { return _LairNum; }
            set
            {
                _LairNum = value;
                NotifyByModify();
            }
        }

        private DateTime _LairDate;

        /// <summary>
        /// 专家取宝时间
        /// </summary>
        [ProtoMember(25)]
        public DateTime LairDate
        {
            get { return _LairDate; }
            set
            {
                _LairDate = value;
                NotifyByModify();
            }
        }

        private int _DaLairNum;

        /// <summary>
        /// 大师取宝当前次数
        /// </summary>
        [ProtoMember(26)]
        public int DaLairNum
        {
            get { return _DaLairNum; }
            set
            {
                _DaLairNum = value;
                NotifyByModify();
            }
        }

        private DateTime _DaLairDate;

        /// <summary>
        /// 大师取宝时间
        /// </summary>
        [ProtoMember(27)]
        public DateTime DaLairDate
        {
            get { return _DaLairDate; }
            set
            {
                _DaLairDate = value;
                NotifyByModify();
            }
        }

        private int _ZhongLairNum;

        /// <summary>
        /// 宗师取宝当前次数
        /// </summary>
        [ProtoMember(28)]
        public int ZhongLairNum
        {
            get { return _ZhongLairNum; }
            set
            {
                _ZhongLairNum = value;
                NotifyByModify();
            }
        }

        private DateTime _ZhongLairDate;

        /// <summary>
        /// 宗师取宝时间
        /// </summary>
        [ProtoMember(29)]
        public DateTime ZhongLairDate
        {
            get { return _ZhongLairDate; }
            set
            {
                _ZhongLairDate = value;
                NotifyByModify();
            }
        }

        private bool _bossFirstWin;

        /// <summary>
        /// 考古Boss首胜
        /// </summary>
        [ProtoMember(30)]
        public bool BossFirstWin
        {
            get { return _bossFirstWin; }
            set
            {
                _bossFirstWin = value;
                NotifyByModify();
            }
        }

        private bool _monsterFirstWin;

        /// <summary>
        /// 考古怪物首胜
        /// </summary>
        [ProtoMember(31)]
        public bool MonsterFirstWin
        {
            get { return _monsterFirstWin; }
            set
            {
                _monsterFirstWin = value;
                NotifyByModify();
            }
        }

        private string _monsterList;

        /// <summary>
        /// 考古怪物列表（包含Boss）
        /// </summary>
        public string MonsterList
        {
            get { return _monsterList; }
            set
            {
                _monsterList = value;
                NotifyByModify();
            }
        }

        private bool _noviceIsPase;

        /// <summary>
        /// 新手引导是否通过
        /// </summary>
        public bool NoviceIsPase
        {
            get { return _noviceIsPase; }
            set
            {
                _noviceIsPase = value;
                NotifyByModify();
            }
        }
    }
}