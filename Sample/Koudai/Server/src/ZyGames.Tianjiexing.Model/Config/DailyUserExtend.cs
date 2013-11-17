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
using ZyGames.Framework.Event;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 每日限制扩展字段
    /// </summary>
    [Serializable, ProtoContract]
    public class DailyUserExtend : EntityChangeEvent
    {
        public DailyUserExtend():base(false)
        {
            KalpaPlot = new CacheList<FunPlot>();
            LastKalpaPlot = new CacheList<FunPlot>();
            MorePlot = new CacheList<FunPlot>();
            HeroPlot = new List<HeroPlot>();
            WaterNum = new List<DailyRestrain>();
            //待确认，事件绑定
        }

        /// <summary>
        /// 收集活动限制
        /// </summary>
        [ProtoMember(1)]
        public string Funtion16 { get; set; }

        /// <summary>
        /// 每日领取物品限制
        /// </summary>
        [ProtoMember(2)]
        public DateTime Funtion17 { get; set; }

        /// <summary>
        /// 多人副本，尽享战友礼包！每日只能领取一次
        /// </summary>
        [ProtoMember(3)]
        public DateTime Comrades { get; set; }

        /// <summary>
        /// 天界大冲级奖励限制
        /// </summary>
        [ProtoMember(4)]
        public string Leveling { get; set; }

        /// <summary>
        /// 每日宠物晶石刷新次数
        /// </summary>
        [ProtoMember(5)]
        public int PetRefeshNum { get; set; }

        /// <summary>
        /// 每日宠物赛跑次数
        /// </summary>
        [ProtoMember(6)]
        public int PetRunTimes { get; set; }

        /// <summary>
        /// 每日宠物拦截次数
        /// </summary>
        [ProtoMember(7)]
        public int PetIntercept { get; set; }

        /// <summary>
        /// 每日宠物助好友完面成次数
        /// </summary>
        [ProtoMember(8)]
        public int PetHelp { get; set; }

        /// <summary>
        /// 天地劫刷新时间
        /// </summary>
        [ProtoMember(9)]
        public DateTime KalpaDate { get; set; }

        /// <summary>
        /// 天地劫刷新次数
        /// </summary>
        [ProtoMember(10)]
        public int KalpaNum { get; set; }

        /// <summary>
        /// 天地劫刷新上一层次数
        /// </summary>
        [ProtoMember(11)]
        public int LastKalpaNum { get; set; }

        /// <summary>
        /// 天地劫限制次数
        /// </summary>
        [ProtoMember(12)]
        public CacheList<FunPlot> KalpaPlot
        {
            get;
            set;
        }

        /// <summary>
        /// 天地劫限制上一层次数
        /// </summary>
        [ProtoMember(13)]
        public CacheList<FunPlot> LastKalpaPlot
        {
            get;
            set;
        }

        /// <summary>
        /// 多人副本限制次数
        /// </summary>
        [ProtoMember(14)]
        public CacheList<FunPlot> MorePlot { get; set; }

        /// <summary>
        /// 竞技场排名
        /// </summary>
        [ProtoMember(15)]
        public int UserRankID { get; set; }

        /// <summary>
        /// 晶石赠送次数
        /// </summary>
        [ProtoMember(16)]
        public Int16 GoldNum { get; set; }

        /// <summary>
        /// 英雄副本限制次数
        /// </summary>
        [ProtoMember(17)]
        public List<HeroPlot> HeroPlot { get; set; }

        /// <summary>
        /// 龙神圣水使用次数
        /// </summary>
        [ProtoMember(18)]
        public List<DailyRestrain> WaterNum { get; set; }

        /// <summary>
        /// 城市争斗战广播次数
        /// </summary>
        [ProtoMember(19)]
        public int BroadcastNum { get; set; }

        /// <summary>
        /// 十里挑一
        /// </summary>
        [ProtoMember(20)]
        public int ShiLiTiaoYi { get; set; }

        /// <summary>
        /// 百里挑一
        /// </summary>
        [ProtoMember(21)]
        public int BaiLiTiaoYi { get; set; }

        /// <summary>
        /// 千载难逢
        /// </summary>
        [ProtoMember(22)]
        public int Golden { get; set; }

        /// <summary>
        /// 探险触控神秘事件次数
        /// </summary>
        [ProtoMember(23)]
        public int ExploreNum { get; set; }

        /// <summary>
        /// 竞技控神秘事件次数
        /// </summary>
        [ProtoMember(24)]
        public int sportsNum { get; set; }
    }
}