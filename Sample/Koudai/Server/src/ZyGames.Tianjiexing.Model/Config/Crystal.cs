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
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Tianjiexing.Model.Config
{
    [Serializable, ProtoContract]
    public class Crystal
    {
        private ConfigCacheSet<CrystalInfo> _cacheSet;

        public Crystal()
        {
            _cacheSet = new ConfigCacheSet<CrystalInfo>();
        }
        [ProtoMember(1)]
        public string UserCrystalID
        {
            get;
            set;
        }

        /// <summary>
        /// 佣兵ID
        /// </summary>
        [ProtoMember(2)]
        public int GeneralID
        {
            get;
            set;
        }

        /// <summary>
        /// 水晶ID
        /// </summary>
        [ProtoMember(3)]
        public Int32 CrystalID
        {
            get;
            set;
        }

        public CrystalQualityType CrystalQuality
        {
            get
            {
                var item = _cacheSet.FindKey(CrystalID) ?? new CrystalInfo();
                return item.CrystalQuality.ToEnum<CrystalQualityType>();
            }
        }

        public AbilityType AbilityType
        {
            get
            {
                var item = _cacheSet.FindKey(CrystalID) ?? new CrystalInfo();
                return item.AbilityID.ToEnum<AbilityType>();
            }
        }

        /// <summary>
        /// 水晶等级
        /// </summary>
        [ProtoMember(4)]
        public Int16 CrystalLv
        {
            get;
            set;
        }

        /// <summary>
        /// 当前经验
        /// </summary>
        [ProtoMember(5)]
        public int CurrExprience
        {
            get;
            set;
        }

        /// <summary>
        /// 位置
        /// </summary>
        [ProtoMember(6)]
        public Int16 Position
        {
            get;
            set;
        }

        /// <summary>
        /// 是否卖出 1:可以卖出 2：不可卖出
        /// </summary>
        [ProtoMember(7)]
        public Int16 IsSale
        {
            get;
            set;
        }

        /// <summary>
        /// 获得时间
        /// </summary>
        [ProtoMember(8)]
        public DateTime CreateDate { get; set; }

        public int IsTelegrams
        {
            get
            {
                var item = _cacheSet.FindKey(CrystalID) ?? new CrystalInfo();
                return item.IsTelegrams;
            }
        }
    }
}