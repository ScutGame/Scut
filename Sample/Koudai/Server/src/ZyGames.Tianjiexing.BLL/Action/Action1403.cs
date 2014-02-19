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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1403_佣兵详情接口
    /// </summary>
    public class Action1403 : BaseAction
    {
        private int generalID;
        private string toUserID;
        private string generalName;
        private string headID;
        private string picturesID;
        private string _battleHeadId;
        private GeneralQuality generalQuality;
        private int currExperience;
        private int maxExperience;
        private short powerNum;
        private short soulNum;
        private short intellectNum;
        private short genLv;
        private short generalStatus;
        private int lifeNum;
        private int lifeMaxNum;
        private int attackNum;
        private int vitalityNum;
        private int talentAbility;
        private string talentName;
        private int _potential;
        private int soulGrid;
        private string _battleHeadID;
        private decimal _hitProbability;
        private List<UserItemInfo> userItemArray = new List<UserItemInfo>();
        private CareerInfo careerInfo = null;
        private List<GeneralProperty> gPropertyList = new List<GeneralProperty>();
        private List<UserFunction> functionList = new List<UserFunction>();
        private UserGeneral userGeneral = null;
        private List<Ability> abilityList = new List<Ability>();
        private string description;
        private CacheList<Karma> karmaList = null;
        //玩家佣兵的职业名称
        private string userCareerName;

        public Action1403(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1403, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(generalName.ToNotNullString());
            this.PushIntoStack(headID.ToNotNullString());
            this.PushIntoStack(picturesID.ToNotNullString());

            this.PushIntoStack((short)generalQuality);
            this.PushIntoStack(currExperience);
            this.PushIntoStack(maxExperience);
            this.PushIntoStack(powerNum);
            this.PushIntoStack(soulNum);
            this.PushIntoStack(intellectNum);
            this.PushIntoStack(careerInfo == null ? 0 : careerInfo.CareerID);
            this.PushIntoStack(careerInfo == null ? string.Empty : careerInfo.CareerName.ToNotNullString());
            this.PushIntoStack(lifeNum);
            this.PushIntoStack(lifeMaxNum);
            this.PushIntoStack(genLv);
            this.PushIntoStack(generalStatus);
            this.PushIntoStack(description.ToNotNullString());
            this.PushIntoStack(attackNum);
            this.PushIntoStack(vitalityNum);
            this.PushIntoStack(talentAbility);
            this.PushIntoStack(talentName.ToNotNullString());
            this.PushIntoStack(userItemArray.Count);
            foreach (var item in userItemArray)
            {
                int isSynthesis;
                DataStruct dsItem = new DataStruct();
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                if (IsSynthesis(ContextUser.UserID, item.ItemID, genLv))
                {
                    isSynthesis = 1;
                }
                else
                {
                    isSynthesis = 2;
                }
                dsItem.PushIntoStack(item.UserItemID.ToNotNullString());
                dsItem.PushIntoStack(item.ItemID);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? 0 : (int)itemInfo.EquParts);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack((short)item.ItemLv);
                dsItem.PushIntoStack((short)isSynthesis);
                dsItem.PushIntoStack(itemInfo == null ? 0 : itemInfo.QualityType.ToInt());
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(abilityList.Count);
            foreach (var ability in abilityList)
            {
                DataStruct dsItem = new DataStruct();
                AbilityInfo abilityInfo = new ConfigCacheSet<AbilityInfo>().FindKey(ability.AbilityID);
                dsItem.PushIntoStack(ability.AbilityID);
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.AbilityName.ToNotNullString());
                dsItem.PushIntoStack((short)ability.AbilityLv);
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.AbilityDesc.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(ability.Position);
                dsItem.PushIntoStack(ability.UserItemID.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo != null ? abilityInfo.AbilityQuality : 0);
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(gPropertyList.Count);
            foreach (var property in gPropertyList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(property.AbilityType.ToShort());
                if (property.AbilityType == AbilityType.MingZhong)
                {
                    string value = (property.AbilityValue + _hitProbability).ToNotNullString();
                    dsItem.PushIntoStack(value);
                }
                else
                {
                    dsItem.PushIntoStack(property.AbilityValue.ToNotNullString());
                }

                this.PushIntoStack(dsItem);
            }

            this.PushIntoStack(functionList.Count);
            foreach (var function in functionList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(function.FunEnum.ToShort());
                this.PushIntoStack(dsItem);
            }
            PushIntoStack(_potential);
            this.PushIntoStack(soulGrid);
            this.PushIntoStack(_battleHeadId.ToNotNullString());
            PushIntoStack(userGeneral != null ? userGeneral.AbilityNum : 3);

            // 缘分信息
            if (karmaList != null)
            {
                PushIntoStack(karmaList.Count);
                foreach (var karmaInfo in karmaList)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(karmaInfo.KarmaName);
                    dsItem.PushIntoStack(karmaInfo.KarmaDesc);
                    dsItem.PushIntoStack(karmaInfo.IsActive);
                    this.PushIntoStack(dsItem);
                }
            }
            else
            {
                PushIntoStack(0);
            }

            PushIntoStack(userCareerName);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID))
            {
                httpGet.GetString("ToUserID", ref toUserID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int currMaxLv = GameConfigSet.CurrMaxLv;
            string lifeUserID = string.Empty;

            UserCrystalInfo[] userCrystalsArrray = new UserCrystalInfo[0];
            if (!string.IsNullOrEmpty(toUserID))
            {
                var packageCrystal = UserCrystalPackage.Get(toUserID);
                userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(toUserID, generalID);
                userCrystalsArrray = packageCrystal.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(generalID)).ToArray();
                var package = UserItemPackage.Get(toUserID);
                userItemArray = package.ItemPackage.FindAll(
                    u => !u.IsRemove && u.GeneralID.Equals(generalID) &&
                        u.ItemStatus == ItemStatus.YongBing);
                lifeUserID = toUserID;
            }
            else
            {
                var packageCrystal = UserCrystalPackage.Get(ContextUser.UserID);
                userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
                userCrystalsArrray = packageCrystal.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(generalID)).ToArray();

                var package = UserItemPackage.Get(ContextUser.UserID);
                userItemArray = package.ItemPackage.FindAll(u => !u.IsRemove && u.GeneralID.Equals(generalID) && u.ItemStatus == ItemStatus.YongBing);
                lifeUserID = ContextUser.UserID;
            }
            UserCacheGlobal.LoadOffline(lifeUserID);
            GeneralInfo generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            generalQuality = generalInfo == null ? (short)0 : generalInfo.GeneralQuality;
            if (generalInfo != null && userGeneral != null)
            {
                description = generalInfo.Description;
                if (userGeneral.GeneralLv >= currMaxLv)
                {
                    genLv = (short)currMaxLv;
                    userGeneral.CurrExperience = 0;
                }
                else
                {
                    genLv = userGeneral.GeneralLv;
                }
                generalName = userGeneral.GeneralName;
                headID = userGeneral.HeadID;
                picturesID = userGeneral.PicturesID;
                _battleHeadId = generalInfo.BattleHeadID;
                _hitProbability = generalInfo.HitProbability;
                careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(userGeneral.CareerID);
                GeneralEscalateInfo generalEscalate = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(genLv);
                if (generalEscalate != null)
                {
                    currExperience = userGeneral.CurrExperience;
                    maxExperience = generalEscalate.UpExperience;
                }
                powerNum = MathUtils.Addition(userGeneral.PowerNum, userGeneral.TrainingPower, short.MaxValue);
                soulNum = MathUtils.Addition(userGeneral.SoulNum, userGeneral.TrainingSoul, short.MaxValue);
                intellectNum = MathUtils.Addition(userGeneral.IntellectNum, userGeneral.TrainingIntellect, short.MaxValue);
                if (!string.IsNullOrEmpty(ContextUser.MercenariesID) && generalID == LanguageManager.GetLang().GameUserGeneralID)
                {
                    //公会技能加成
                    powerNum = MathUtils.RoundCustom(powerNum * CombatHelper.GetGuildAbilityNum(ContextUser.UserID, GuildAbilityType.PowerNum)).ToShort();
                    soulNum = MathUtils.RoundCustom(soulNum * CombatHelper.GetGuildAbilityNum(ContextUser.UserID, GuildAbilityType.SoulNum)).ToShort();
                    intellectNum = MathUtils.RoundCustom(intellectNum * CombatHelper.GetGuildAbilityNum(ContextUser.UserID, GuildAbilityType.IntellectNum)).ToShort();
                }
                if (generalID == LanguageManager.GetLang().GameUserGeneralID)
                {
                    //法宝基础属性加成
                    powerNum = MathUtils.Addition(powerNum, TrumpAbilityAttack.TrumpPropertyNum(ContextUser.UserID, generalID, AbilityType.PowerNum));
                    soulNum = MathUtils.Addition(soulNum, TrumpAbilityAttack.TrumpPropertyNum(ContextUser.UserID, generalID, AbilityType.SoulNum));
                    intellectNum = MathUtils.Addition(intellectNum, TrumpAbilityAttack.TrumpPropertyNum(ContextUser.UserID, generalID, AbilityType.IntelligenceNum));

                    //法宝--技能属性转换获得的值
                    //法宝--技能属性转换获得的值
                    decimal trumpPower = TrumpAbilityAttack.ConversionPropertyNum(ContextUser.UserID, powerNum, soulNum, intellectNum, AbilityType.PowerNum);
                    decimal trumpsoul = TrumpAbilityAttack.ConversionPropertyNum(ContextUser.UserID, powerNum, soulNum, intellectNum, AbilityType.SoulNum);
                    decimal trumpintellect = TrumpAbilityAttack.ConversionPropertyNum(ContextUser.UserID, powerNum, soulNum, intellectNum, AbilityType.IntelligenceNum);
                    powerNum = MathUtils.Addition(trumpPower.ToShort(), powerNum);
                    soulNum = MathUtils.Addition(trumpsoul.ToShort(), soulNum);
                    intellectNum = MathUtils.Addition(trumpintellect.ToShort(), intellectNum);
                }
                lifeNum = userGeneral.LifeNum;
                lifeMaxNum = UserHelper.GetMaxLife(lifeUserID, generalID);
                genLv = userGeneral.GeneralLv;
                generalStatus = (short)userGeneral.GeneralStatus;

                talentAbility = userGeneral.AbilityID;
                _potential = userGeneral.Potential;
                _battleHeadID = generalInfo.BattleHeadID;
                //玩家佣兵职业名称
                userCareerName = careerInfo.CareerName;
            }
            else if (generalInfo != null)
            {
                genLv = 1;
                generalName = generalInfo.GeneralName;
                headID = generalInfo.HeadID;
                picturesID = generalInfo.PicturesID;
                _battleHeadId = generalInfo.BattleHeadID;
                careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(generalInfo.CareerID);
                GeneralEscalateInfo generalEscalate = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(genLv);
                if (generalEscalate != null)
                {
                    currExperience = 0;
                    maxExperience = generalEscalate.UpExperience;
                }
                powerNum = generalInfo.PowerNum;
                soulNum = generalInfo.SoulNum;
                intellectNum = generalInfo.IntellectNum;
                lifeNum = generalInfo.LifeNum;
                lifeMaxNum = generalInfo.LifeNum;
                genLv = generalInfo.GeneralLv;
                generalStatus = (short)GeneralStatus.KeZhaoMu;

                talentAbility = generalInfo.AbilityID;
            }
            AbilityInfo ability = new ConfigCacheSet<AbilityInfo>().FindKey(talentAbility);
            talentName = ability == null ? string.Empty : ability.AbilityName;

            gPropertyList = CombatHelper.GetAbility(lifeUserID, generalID, userGeneral);

            int sumCrystal = 0;
            foreach (UserCrystalInfo crystal in userCrystalsArrray)
            {
                sumCrystal = MathUtils.Addition(sumCrystal, crystal.CurrExprience, int.MaxValue);
            }
            vitalityNum = (sumCrystal / 10);
            functionList = ViewHelper.GetFunctionList(lifeUserID);

            //佣兵魂技
            UserAbility userAbility = new GameDataCacheSet<UserAbility>().FindKey(ContextUser.UserID);
            if (userAbility != null)
            {
                abilityList = userAbility.AbilityList.FindAll(s => s.GeneralID == generalID);
            }
            soulGrid = UserPackHelper.GetPackTypePositionNum(BackpackType.HunJi, ContextUser.UserID);

            // 缘分系统
            KarmaInfo karmaInfo = new ShareCacheStruct<KarmaInfo>().FindKey(generalID);

            if (karmaInfo != null && karmaInfo.KarmaList != null)
            {
                // 满足缘分条件显示激活状态

                // 判断佣兵
                UserMagic userMagic = new GameDataCacheSet<UserMagic>().Find(ContextUser.UserID, s => s.IsEnabled);
                var cacheSetUserEmbattle = new GameDataCacheSet<UserEmbattle>();
                List<Karma> yongBingList = karmaInfo.KarmaList.FindAll(s => s.KarmaType == KarmaType.YongBing);
                yongBingList.ForEach(yongBingInfo =>
                                         {
                                             //yongBingInfo.IsActive = 1;
                                             // 若阵型中存在该记录，该缘分为激活状态
                                             if (yongBingInfo.ValueID.Contains(","))
                                             {
                                                 string[] valueID = yongBingInfo.ValueID.Split(',');
                                                 int i = 0;
                                                 foreach (var id in valueID)
                                                 {
                                                     if (cacheSetUserEmbattle.Find(ContextUser.UserID, s => s.MagicID == userMagic.MagicID && s.GeneralID == MathUtils.ToInt(id)) != null)
                                                     {
                                                         i++;
                                                     }
                                                 }
                                                 yongBingInfo.IsActive = i == valueID.Length ? 1 : 0;

                                             }
                                             else
                                             {
                                                 if (cacheSetUserEmbattle.Find(ContextUser.UserID, s => s.MagicID == userMagic.MagicID && s.GeneralID == MathUtils.ToInt(yongBingInfo.ValueID)) != null)
                                                 {
                                                     yongBingInfo.IsActive = 1;
                                                 }
                                                 else
                                                 {
                                                     yongBingInfo.IsActive = 0;
                                                 }

                                             }
                                         });
                // 判断装备
                UserItemPackage itemPackage = new GameDataCacheSet<UserItemPackage>().FindKey(UserId.ToString());
                if (itemPackage != null)
                {
                    List<Karma> itemList = karmaInfo.KarmaList.FindAll(s => s.KarmaType == KarmaType.ZhuangBei);
                    itemList.ForEach(itemInfo =>
                    {
                        if (itemPackage.ItemPackage.Find(s => s.ItemID == MathUtils.ToInt(itemInfo.ValueID) && s.GeneralID == generalID) != null)
                        {
                            itemInfo.IsActive = 1;
                        }
                        else
                        {
                            itemInfo.IsActive = 0;
                        }
                    });
                }

                // 判断命运水晶
                UserCrystalPackage crystalPackage = new GameDataCacheSet<UserCrystalPackage>().FindKey(UserId.ToString());
                if (itemPackage != null)
                {
                    List<Karma> crystalList = karmaInfo.KarmaList.FindAll(s => s.KarmaType == KarmaType.ShuiJing);
                    crystalList.ForEach(crystalInfo =>
                    {
                        if (crystalPackage.CrystalPackage.Find(s => s.CrystalID == MathUtils.ToInt(crystalInfo.ValueID) && s.GeneralID == generalID) != null)
                        {
                            crystalInfo.IsActive = 1;
                        }
                        else
                        {
                            crystalInfo.IsActive = 0;
                        }
                    });
                }

                // 判断技能
                UserAbility userAbilityInfo = new GameDataCacheSet<UserAbility>().FindKey(UserId.ToString());
                if (userAbilityInfo != null)
                {
                    List<Karma> abilityList = karmaInfo.KarmaList.FindAll(s => s.KarmaType == KarmaType.JiNen);
                    abilityList.ForEach(abilityInfo =>
                                            {
                                                if (userAbilityInfo.AbilityList.Find(s => s.AbilityID == MathUtils.ToInt(abilityInfo.ValueID) && s.GeneralID == generalID) != null)
                                                {
                                                    abilityInfo.IsActive = 1;
                                                }
                                                else
                                                {
                                                    abilityInfo.IsActive = 0;
                                                }
                                            });
                }
                karmaList = karmaInfo.KarmaList;
                if (userGeneral != null)
                {
                    lifeNum = userGeneral.TLifeNum;
                    lifeMaxNum = userGeneral.TLifeMaxNun;
                }
            }
            return true;
        }

        /// <summary>
        /// 是否升级
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static bool IsSynthesis(string userID, int itemID, short userLv)
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (itemInfo == null || itemInfo.ItemType == ItemType.TuZhi)
            {
                return false;
            }
            List<ItemSynthesisInfo> itemSynthesisInfosArray =
                                     new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.SynthesisID == itemID);
            if (itemSynthesisInfosArray.Count == 0 || itemSynthesisInfosArray[0].DemandLv > userLv)
            {
                return false;
            }
            List<ItemSynthesisInfo> infoArray =
                    new ConfigCacheSet<ItemSynthesisInfo>().FindAll(m => m.ItemID == itemSynthesisInfosArray[0].ItemID);
            foreach (ItemSynthesisInfo info in infoArray)
            {
                var uItemArray = UserItemHelper.GetItems(userID).FindAll(u => u.ItemID == info.SynthesisID && u.ItemType == ItemType.TuZhi && u.ItemStatus != ItemStatus.Sell);

                if (uItemArray.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}