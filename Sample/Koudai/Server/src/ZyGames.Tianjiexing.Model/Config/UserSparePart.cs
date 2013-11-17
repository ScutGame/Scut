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
using System.Runtime.Serialization;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Event;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 玩家灵件
    /// </summary>
    [Serializable, ProtoContract]
    public class UserSparePart : EntityChangeEvent
    {
        private const int MaxPropertyCount = 3;

        public UserSparePart()
            : base(false)
        {
            Propertys = new CacheList<SparePartProperty>();
        }

        private void SetPropertys()
        {
            for (int i = 0; i < MaxPropertyCount; i++)
            {
                Propertys.Add(null);
            }
        }

        /// <summary>
        /// 装备最多封6个灵件
        /// </summary>
        public const short PartMaxGrid = 6;

        [ProtoMember(1)]
        public Int32 SparePartId
        {
            get;
            set;
        }
        /// <summary>
        /// 灵件唯一标识
        /// </summary>
        [ProtoMember(2)]
        public string UserSparepartID { get; set; }

        /// <summary>
        /// 灵件放置的装备唯一标识
        /// </summary>
        [ProtoMember(3)]
        public string UserItemID { get; set; }

        /// <summary>
        /// 灵件位置
        /// </summary>
        [ProtoMember(4)]
        public short Position { get; set; }

        [ProtoMember(5)]
        public CacheList<SparePartProperty> Propertys
        {
            get;
            set;
        }

        public void SetPosition(short pos)
        {
            pos = pos > 0 ? pos : (short)1;
            Position = pos > PartMaxGrid ? PartMaxGrid : pos;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLayerNum">天地劫当前层数</param>
        /// <returns></returns>
        public bool CheckEnable(int userLayerNum)
        {
            return Position > 0 && Position <= userLayerNum;
        }

        /// <summary>
        /// 取随机灵件
        /// </summary>
        /// <param name="sparePartId"></param>
        /// <returns></returns>
        public static UserSparePart GetRandom(int sparePartId)
        {
            var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePartId);
            if (sparePartInfo == null) return null;
            var sparePart = BuildSparePart(sparePartId);
            if (sparePart != null)
            {
                SparePartProperty[] propertys = new[] { RandomProperty(sparePartInfo, true, null) };
                SetSparePartProperty(sparePart, propertys);
            }
            return sparePart;
        }

        public static UserSparePart CreateSparePart(int sparePartId, string[] propertys, char splitChar)
        {
            var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePartId);
            if (sparePartInfo == null) return null;

            var sparePart = BuildSparePart(sparePartId);
            if (sparePart != null)
            {
                var tempPropertys = new List<SparePartProperty>();
                for (int i = 0; i < MaxPropertyCount; i++)
                {
                    if (i >= propertys.Length) continue;

                    string[] tempList = propertys[i].ToNotNullString().Split(splitChar);
                    if (tempList.Length != 2) continue;

                    AbilityType abilityType = tempList[0].ToEnum<AbilityType>();
                    int rangeIndex = tempList[1].ToInt();

                    foreach (SparePartProperty property in sparePartInfo.PropertyRange)
                    {
                        if (property != null && property.AbilityType.Equals(abilityType))
                        {
                            var userSpareProperty = BuildPartProperty(property, rangeIndex, false);
                            if (userSpareProperty != null)
                            {
                                tempPropertys.Add(userSpareProperty);
                            }
                            break;
                        }
                    }
                }

                SetSparePartProperty(sparePart, tempPropertys.ToArray());
            }
            return sparePart;
        }

        private static UserSparePart BuildSparePart(int sparePartId)
        {
            var a = new Array[3];
            var sparePart = new UserSparePart()
            {
                SparePartId = sparePartId,
                UserSparepartID = Guid.NewGuid().ToString(),
                Position = 0,
                UserItemID = string.Empty
            };
            sparePart.SetPropertys();
            return sparePart;
        }

        public static void SetSparePartProperty(UserSparePart sparePart, SparePartProperty[] propertys)
        {
            for (int i = 0; i < sparePart.Propertys.Count; i++)
            {
                if (i < propertys.Length)
                {
                    sparePart.Propertys[i] = propertys[i];
                }
                else
                {
                    sparePart.Propertys[i] = new SparePartProperty() { IsEnable = false };
                }
            }
        }

        public static SparePartProperty BuildPartProperty(SparePartProperty property, int rangeIndex, bool isDefault)
        {
            SparePartProperty ability = CopyProperty(property);
            double valNum = ability.DefNum;
            if (!isDefault)
            {
                if (rangeIndex < 0) rangeIndex = 0;
                if (rangeIndex >= ability.NumRange.Length) rangeIndex = ability.NumRange.Length - 1;
                ability.ValueIndex = (short)rangeIndex;
                if (rangeIndex < ability.NumRange.Length)
                {
                    if (ability.HitMinValue % 1 == 0)
                    {
                        valNum = RandomUtils.GetRandom(ability.HitMinValue.ToInt(), ability.HitMaxValue.ToInt());
                    }
                    else
                    {
                        int minVal = (int)MathUtils.RoundCustom(ability.HitMinValue * 1000);
                        int maxVal = (int)MathUtils.RoundCustom(ability.HitMaxValue * 1000);
                        int temnum = RandomUtils.GetRandom(minVal, maxVal);
                        valNum = (double)temnum / 1000;
                    }
                }
            }

            SparePartProperty partProperty = new SparePartProperty();
            partProperty.DefNum = ability.DefNum;
            partProperty.NumRange = ability.NumRange;
            partProperty.Rate = ability.Rate;
            partProperty.ValueIndex = (short)rangeIndex;
            partProperty.AbilityType = ability.AbilityType;
            partProperty.Num = valNum;
            partProperty.IsEnable = true;
            return partProperty;
        }

        /// <summary>
        /// 刷新随机灵件属性
        /// </summary>
        /// <param name="partInfo"></param>
        /// <param name="isDefault"></param>
        /// <param name="ignorePropertys">忽视的</param>
        /// <returns></returns>
        public static SparePartProperty RandomProperty(SparePartInfo partInfo, bool isDefault, CacheList<SparePartProperty> ignorePropertys)
        {
            SparePartProperty partProperty = new SparePartProperty();
            if (partInfo.PropertyRange.Length > 0)
            {
                SparePartProperty ability = RandomAbilityProperty(partInfo, ignorePropertys);
                int rangeIndex = RandomUtils.GetHitIndex(ability.Rate);
                partProperty = BuildPartProperty(ability, rangeIndex, isDefault);
            }
            return partProperty;
        }

        private static SparePartProperty RandomAbilityProperty(SparePartInfo partInfo, CacheList<SparePartProperty> ignorePropertys)
        {
            var tempPropertys = new List<SparePartProperty>();
            foreach (var property in partInfo.PropertyRange)
            {
                if (ignorePropertys != null
                    && ignorePropertys.Exists(m => m != null && m.AbilityType.Equals(property.AbilityType))
                   )
                {
                    continue;
                }
                tempPropertys.Add(property);
            }
            int index = RandomUtils.GetRandom(0, tempPropertys.Count);
            return index < tempPropertys.Count ? tempPropertys[index] : null;
        }

        private static SparePartProperty CopyProperty(SparePartProperty property)
        {
            return new SparePartProperty()
            {
                AbilityType = property.AbilityType,
                NumRange = property.NumRange,
                Rate = property.Rate,
                DefNum = property.DefNum,
                ValueIndex = property.ValueIndex
            };
        }
    }
}