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
using System.Collections.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 先攻值计算
    /// </summary>
    public class PriorityHelper
    {
        /// <summary>
        /// 佣兵先功值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static int GeneralPriority(string userID, int generalID)
        {
            int priorityNum = 0;
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (userGeneral != null)
            {
                priorityNum = (int)(userGeneral.GeneralLv * PriorityBaseNum(PriorityType.Lv, PriorityQuality.Zero));
            }
            return priorityNum;
        }

        /// <summary>
        /// 装备先攻值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static int GeneralEquPriority(string userID, int generalID)
        {
            int itemEquPriority = 0;
            var package = UserItemPackage.Get(userID);
            List<UserItemInfo> userItemArray = package.ItemPackage.FindAll(m => m.GeneralID.Equals(generalID));
            foreach (UserItemInfo item in userItemArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                if (itemInfo != null)
                {
                    PriorityQuality quality = UserEquPriorityQuality(itemInfo.QualityType); //itemInfo.QualityType.ToInt().ToEnum<PriorityQuality>();
                    int baseNum = PriorityBaseNum(PriorityType.Equ, quality);
                    int effectNum = PriorityEffectNum(PriorityType.Equ, quality);
                    itemEquPriority += (MathUtils.Addition(baseNum, item.ItemLv * effectNum));
                    itemEquPriority += GeneralSparePriority(userID, item.UserItemID);
                }
            }
            return itemEquPriority;
        }

        /// <summary>
        /// 命运水晶
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static int GeneralCrystalPriority(string userID, int generalID)
        {
            int crystalPriority = 0;
            var package = UserCrystalPackage.Get(userID);
            if (package != null)
            {
                List<UserCrystalInfo> userCrystalArray = package.CrystalPackage.FindAll(m => m.GeneralID.Equals(generalID));
                foreach (UserCrystalInfo crystal in userCrystalArray)
                {
                    PriorityQuality quality = crystal.CrystalQuality.ToInt().ToEnum<PriorityQuality>();
                    int baseNum = PriorityBaseNum(PriorityType.Crystal, quality);
                    int effectNum = crystal.CrystalLv >= 1 ? PriorityEffectNum(PriorityType.Crystal, quality) : 0;
                    crystalPriority += (MathUtils.Addition(baseNum, crystal.CrystalLv * effectNum));
                }
            }
            return crystalPriority;
        }

        /// <summary>
        /// 灵件先攻
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="_userItemID"></param>
        /// <returns></returns>
        public static int GeneralSparePriority(string userID, string _userItemID)
        {
            int sparePriority = 0;
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user != null)
            {
                UserSparePart[] sparePartsArray = user.SparePartList.FindAll(m => m.UserItemID.Equals(_userItemID)).ToArray();
                foreach (UserSparePart sparePart in sparePartsArray)
                {
                    SparePartInfo partInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePart.SparePartId);
                    if (partInfo != null)
                    {
                        SparePartQuality sparequality = partInfo.QualityType.ToEnum<SparePartQuality>();
                        PriorityQuality quality = UserSparePriorityQuality(sparequality);
                        int baseNum = PriorityBaseNum(PriorityType.Spare, quality);
                        int effectNum = PriorityEffectNum(PriorityType.Spare, quality);
                        var propertyArray = sparePart.Propertys;
                        foreach (SparePartProperty property in propertyArray)
                        {
                            if (property.AbilityType != AbilityType.Empty)
                            {
                                sparePriority += (MathUtils.Addition(baseNum, effectNum * property.ValueIndex));
                            }
                        }
                    }
                }
            }
            return sparePriority;
        }

        /// <summary>
        /// 药剂先功
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static int GeneralMedicinePriority(string userID, int generalID)
        {
            int medicinePriority = 0;
            var medicineArray = new GameDataCacheSet<GeneralMedicine>().FindAll(userID, m => m.GeneralID == generalID);
            foreach (GeneralMedicine medicine in medicineArray)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(medicine.MedicineID);
                if (itemInfo != null)
                {
                    PriorityQuality quality = itemInfo.MedicineLv.ToInt().ToEnum<PriorityQuality>();
                    int baseNum = PriorityBaseNum(PriorityType.Medicine, quality);
                    medicinePriority += baseNum;
                }
            }
            return medicinePriority;
        }

        /// <summary>
        /// 基础值
        /// </summary>
        /// <param name="priorityType"></param>
        /// <param name="priorityQuality"></param>
        /// <returns></returns>
        public static int PriorityBaseNum(PriorityType priorityType, PriorityQuality priorityQuality)
        {
            int baseNum = 0;
            PriorityInfo priority = new ConfigCacheSet<PriorityInfo>().FindKey(priorityType, priorityQuality);
            if (priority != null)
            {
                baseNum = priority.BaseNum;
            }
            return baseNum;
        }

        /// <summary>
        /// 加成值
        /// </summary>
        /// <param name="priorityType"></param>
        /// <param name="priorityQuality"></param>
        /// <returns></returns>
        public static int PriorityEffectNum(PriorityType priorityType, PriorityQuality priorityQuality)
        {
            int baseNum = 0;
            PriorityInfo priority = new ConfigCacheSet<PriorityInfo>().FindKey(priorityType, priorityQuality);
            if (priority != null)
            {
                baseNum = priority.EffectNum;
            }
            return baseNum;
        }

        /// <summary>
        /// 玩家阵法先攻值
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private static int EmbattlePriorityNum(string userID)
        {
            int priorityNum = 0;
            //魔术先攻
            var magicArray = new GameDataCacheSet<UserMagic>().FindAll(userID, m => m.MagicType == MagicType.JiNeng);
            foreach (UserMagic magic in magicArray)
            {
                int mlv = magic.MagicLv;
                MagicLvInfo lvInfo = new ConfigCacheSet<MagicLvInfo>().FindKey(magic.MagicID, mlv);
                if (lvInfo != null && lvInfo.AbilityType == AbilityType.FirstStrike)
                {
                    priorityNum = MathUtils.Addition(priorityNum, (int)lvInfo.EffectNum);
                }
            }
            return priorityNum;
        }

        /// <summary>
        /// 佣兵总先攻
        /// </summary>
        /// <returns></returns>
        public static int GeneralTotalPriority(string userID, int generalID)
        {
            int generalPri = GeneralPriority(userID, generalID);
            int embattlePri = EmbattlePriorityNum(userID);
            int equPri = GeneralEquPriority(userID, generalID);
            int crystalpri = GeneralCrystalPriority(userID, generalID);
            int medicinePri = GeneralMedicinePriority(userID, generalID);
            int temp = MathUtils.Addition(generalPri, embattlePri);
            temp = MathUtils.Addition(temp, equPri);
            temp = MathUtils.Addition(temp, crystalpri);
            temp = MathUtils.Addition(temp, medicinePri);
            int totalPri = temp;
            return totalPri;
        }

        /// <summary>
        /// 装备对应品质
        /// </summary>
        /// <param name="qualityType"></param>
        /// <returns></returns>
        public static PriorityQuality UserEquPriorityQuality(QualityType qualityType)
        {
            PriorityQuality quality = new PriorityQuality();
            switch (qualityType)
            {
                case QualityType.BaiSe:
                    quality = PriorityQuality.First;
                    break;
                case QualityType.LanSe:
                    quality = PriorityQuality.Three;
                    break;
                case QualityType.ZiSe:
                    quality = PriorityQuality.Four;
                    break;
                case QualityType.HuangSe:
                    quality = PriorityQuality.Five;
                    break;
                case QualityType.HongSe:
                    quality = PriorityQuality.Six;
                    break;
            }
            return quality;
        }

        /// <summary>
        /// 零件对应品质
        /// </summary>
        /// <param name="qualityType"></param>
        /// <returns></returns>
        public static PriorityQuality UserSparePriorityQuality(SparePartQuality qualityType)
        {
            PriorityQuality quality = new PriorityQuality();
            switch (qualityType)
            {
                case SparePartQuality.LanSe:
                    quality = PriorityQuality.Three;
                    break;
                case SparePartQuality.ZiSe:
                    quality = PriorityQuality.Four;
                    break;
                case SparePartQuality.Yellow:
                    quality = PriorityQuality.Five;
                    break;
            }
            return quality;
        }
    }
}