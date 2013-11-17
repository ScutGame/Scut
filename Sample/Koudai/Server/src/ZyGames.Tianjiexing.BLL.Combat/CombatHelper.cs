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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    public class CombatHelper
    {
        

        /// <summary>
        /// 佣兵属性集合
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static List<GeneralProperty> GetAbility(string userID, int generalID,UserGeneral userGeneral)
        {
           
            List<GeneralProperty> gPropertyList = new List<GeneralProperty>();
            var cacheSetUserEmbattle = new GameDataCacheSet<UserEmbattle>();
            GeneralProperty generalProperty = new GeneralProperty();
            var userMagic = new GameDataCacheSet<UserMagic>().Find(userID, s => s.IsEnabled);
            CombatGeneral combatGeneral = new CombatGeneral()
            {
                UserID = userID,
                GeneralID = generalID,
                ExtraAttack = new CombatProperty(),
                ExtraDefense = new CombatProperty(),
                LifeNum = userGeneral.LifeNum,
                LifeMaxNum = userGeneral.LifeMaxNum
            };
            
            
            var userEmbattleList = cacheSetUserEmbattle.FindAll(userID,
                                                                s =>
                                                                s.MagicID == (userMagic != null ? userMagic.MagicID : 0));
            UserEmbattleQueue.embattleList = userEmbattleList;
            UserEmbattleQueue.KarmaAddition(userID, combatGeneral);
            userGeneral.TLifeNum = combatGeneral.LifeNum;
            userGeneral.TLifeMaxNun = combatGeneral.LifeMaxNum;
            //int LifeReply = 0; // 生命回复
            //物理攻击
            generalProperty = GetAbilityProperty(userID, generalID, AbilityType.WuLiGongJi);
            generalProperty.AbilityValue = MathUtils.Addition(generalProperty.AbilityValue, generalProperty.AbilityValue*combatGeneral.ExtraAttack.AdditionWuliNum);
            gPropertyList.Add(generalProperty);
            //物理防御
            generalProperty = GetAbilityProperty(userID, generalID, AbilityType.WuLiFangYu);
            generalProperty.AbilityValue = MathUtils.Addition(generalProperty.AbilityValue, generalProperty.AbilityValue * combatGeneral.ExtraDefense.AdditionWuliNum);
            gPropertyList.Add(generalProperty);
            //魂技攻击
            generalProperty = GetAbilityProperty(userID, generalID, AbilityType.HunJiGongJi);
            generalProperty.AbilityValue = MathUtils.Addition(generalProperty.AbilityValue, generalProperty.AbilityValue * combatGeneral.ExtraAttack.AdditionHunjiNum);
            gPropertyList.Add(generalProperty);
            //魂技防御   
            generalProperty = GetAbilityProperty(userID, generalID, AbilityType.HunJiFangYu);
            generalProperty.AbilityValue = MathUtils.Addition(generalProperty.AbilityValue, generalProperty.AbilityValue * combatGeneral.ExtraDefense.AdditionHunjiNum);
            gPropertyList.Add(generalProperty);
            //魔法攻击
            generalProperty = GetAbilityProperty(userID, generalID, AbilityType.MoFaGongJi);
            generalProperty.AbilityValue = MathUtils.Addition(generalProperty.AbilityValue, generalProperty.AbilityValue * combatGeneral.ExtraAttack.AdditionMofaNum);
            gPropertyList.Add(generalProperty);
            //魔法防御
            generalProperty = GetAbilityProperty(userID, generalID, AbilityType.MoFaFangYu);
            generalProperty.AbilityValue = MathUtils.Addition(generalProperty.AbilityValue, generalProperty.AbilityValue * combatGeneral.ExtraDefense.AdditionMofaNum);
            gPropertyList.Add(generalProperty);

            //暴击
            gPropertyList.Add(GetAbilityProperty(userID, generalID, AbilityType.BaoJi));
            //命中
            gPropertyList.Add(GetAbilityProperty(userID, generalID, AbilityType.MingZhong));
            //破击 
            gPropertyList.Add(GetAbilityProperty(userID, generalID, AbilityType.PoJi));
            //韧性
            gPropertyList.Add(GetAbilityProperty(userID, generalID, AbilityType.RenXing));
            //闪避
            gPropertyList.Add(GetAbilityProperty(userID, generalID, AbilityType.ShanBi));
            //格挡
            gPropertyList.Add(GetAbilityProperty(userID, generalID, AbilityType.GeDang));
            //必杀
            gPropertyList.Add(GetAbilityProperty(userID, generalID, AbilityType.BiSha));

            //先攻
            gPropertyList.Add(new GeneralProperty() { AbilityType = AbilityType.FirstStrike, AbilityValue = (decimal)TotalPriorityNum(userID, generalID) });

            return gPropertyList;
        }

        private static GeneralProperty GetAbilityProperty(string userID, int generalID, AbilityType abilityType)
        {
            decimal val = 0;
            int careerID = 0;
            UserGeneral ugeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            GeneralInfo generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            if (ugeneral != null)
            {
                careerID = ugeneral.CareerID;
            }
            else if (generalInfo != null)
            {
                careerID = generalInfo.CareerID;
            }

            if (abilityType == AbilityType.WuLiGongJi || abilityType == AbilityType.WuLiFangYu || abilityType == AbilityType.HunJiGongJi || abilityType == AbilityType.HunJiFangYu || abilityType == AbilityType.MoFaGongJi || abilityType == AbilityType.MoFaFangYu)
            {
                int equNum = GetEquNum(userID, generalID, abilityType).ToInt();
                int fateNum = GetFateNum(userID, generalID, abilityType).ToInt();
                int propertyNum = GetPropertyNum(userID, generalID, abilityType).ToInt();
                int magicNum = (int)GetEmbattleNum(userID, abilityType);
                int upgradeNum = GeneralUpGradeProperty(userID, generalID, abilityType).ToInt();

                val = MathUtils.Addition(equNum, fateNum, int.MaxValue);
                val = MathUtils.Addition(val, propertyNum, int.MaxValue);
                val = MathUtils.Addition(val, magicNum, int.MaxValue);
                val = MathUtils.Addition(val, upgradeNum);

                //佣兵好感度数值加成
                int feelNum = FeelEffectNum(userID, generalID, abilityType);
                val = MathUtils.Addition(val, feelNum, int.MaxValue);
                //法宝附加属性值
                int trumpPro = (int)TrumpAbilityAttack.TrumpGeneralProperty(userID, generalID, abilityType);
                val = MathUtils.Addition(val, trumpPro, int.MaxValue);

                //附魔符属性值
                int enchantNum = EnchantProprety(userID, generalID, abilityType).ToInt();
                val = MathUtils.Addition(val, enchantNum);

                //属性转换
                //val = MathUtils.Addition(val, (int)TrumpAbilityAttack.ConversionPropertyNum(userID, val, abilityType), int.MaxValue);
            }
            else if (abilityType == AbilityType.BaoJi || abilityType == AbilityType.MingZhong || abilityType == AbilityType.PoJi || abilityType == AbilityType.RenXing || abilityType == AbilityType.ShanBi || abilityType == AbilityType.GeDang || abilityType == AbilityType.BiSha)
            {
                val = GetCareerNum(careerID, abilityType);
                val = MathUtils.Addition(val, GetEquNum(userID, generalID, abilityType), decimal.MaxValue);
                val = MathUtils.Addition(val, GetFateNum(userID, generalID, abilityType), decimal.MaxValue);
                val = MathUtils.Addition(val, GetEmbattleNum(userID, abilityType), decimal.MaxValue);
                val = MathUtils.Addition(val, GeneralUpGradeProperty(userID, generalID, abilityType).ToDecimal());
                //法宝附加属性值
                decimal trumpPro = TrumpAbilityAttack.TrumpGeneralProperty(userID, generalID, abilityType);
                val = MathUtils.Addition(val, trumpPro, int.MaxValue);

                //附魔符属性值
                decimal enchantNum = EnchantProprety(userID, generalID, abilityType);
                val = MathUtils.Addition(val, enchantNum);

                //属性转换
                //val = MathUtils.Addition(val, TrumpAbilityAttack.ConversionPropertyNum(userID, val, abilityType), decimal.MaxValue);
            }
            return new GeneralProperty() { AbilityType = abilityType, AbilityValue = val };
        }

        /// <summary>
        /// 人物属性
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static int GetPropertyNum(string userID, int generalID, AbilityType abilityType)
        {
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            int propertyNum = 0;
            int powerNum = 0;       //力量
            int soulNum = 0;        //魂力
            int intellectNum = 0;   //智力
            int generalLv = 0;
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (general != null)
            {
                generalLv = (int)general.GeneralLv;
                powerNum = (int)MathUtils.Addition(general.PowerNum, general.TrainingPower);
                soulNum = (int)MathUtils.Addition(general.SoulNum, general.TrainingSoul);
                intellectNum = (int)MathUtils.Addition(general.IntellectNum, general.TrainingIntellect);
                //公会技能加成
                if (user != null && !string.IsNullOrEmpty(user.MercenariesID) && general.GeneralID == LanguageManager.GetLang().GameUserGeneralID)
                {
                    powerNum = MathUtils.RoundCustom(powerNum * GetGuildAbilityNum(user.UserID, GuildAbilityType.PowerNum)).ToShort();
                    soulNum = MathUtils.RoundCustom(soulNum * GetGuildAbilityNum(user.UserID, GuildAbilityType.SoulNum)).ToShort();
                    intellectNum = MathUtils.RoundCustom(intellectNum * GetGuildAbilityNum(user.UserID, GuildAbilityType.IntellectNum)).ToShort();
                }

                if (general.GeneralID == LanguageManager.GetLang().GameUserGeneralID)
                {
                    //法宝基础属性加成
                    powerNum = MathUtils.Addition(powerNum, TrumpAbilityAttack.TrumpPropertyNum(userID, general.GeneralID, AbilityType.PowerNum));
                    soulNum = MathUtils.Addition(soulNum, TrumpAbilityAttack.TrumpPropertyNum(userID, general.GeneralID, AbilityType.SoulNum));
                    intellectNum = MathUtils.Addition(intellectNum, TrumpAbilityAttack.TrumpPropertyNum(userID, general.GeneralID, AbilityType.IntelligenceNum));

                    //法宝--技能属性转换获得的值
                    decimal trumpPower = TrumpAbilityAttack.ConversionPropertyNum(userID, powerNum, soulNum, intellectNum, AbilityType.PowerNum);
                    decimal trumpsoul = TrumpAbilityAttack.ConversionPropertyNum(userID, powerNum, soulNum, intellectNum, AbilityType.SoulNum);
                    decimal trumpintellect = TrumpAbilityAttack.ConversionPropertyNum(userID, powerNum, soulNum, intellectNum, AbilityType.IntelligenceNum);
                    powerNum = MathUtils.Addition(trumpPower.ToInt(), powerNum);
                    soulNum = MathUtils.Addition(trumpsoul.ToInt(), soulNum);
                    intellectNum = MathUtils.Addition(trumpintellect.ToInt(), intellectNum);
                }
                if (abilityType == AbilityType.WuLiGongJi)
                {
                    //物理攻击
                    propertyNum = (generalLv * powerNum * 1 / 10);
                }
                else if (abilityType == AbilityType.WuLiFangYu)
                {
                    //物理防御
                    propertyNum = (generalLv * powerNum * 7 / 100);
                }
                else if (abilityType == AbilityType.HunJiGongJi)
                {
                    //魂技攻击
                    propertyNum = (generalLv * soulNum * 7 / 100);
                }
                else if (abilityType == AbilityType.HunJiFangYu)
                {
                    //魂技防御
                    propertyNum = (generalLv * soulNum * 7 / 100);
                }
                else if (abilityType == AbilityType.MoFaGongJi)
                {
                    //魔法攻击
                    propertyNum = (generalLv * intellectNum * 7 / 100);
                }
                else if (abilityType == AbilityType.MoFaFangYu)
                {
                    //魔法防御
                    propertyNum = (generalLv * intellectNum * 7 / 100);
                }
            }
            if (propertyNum.IsValid())
            {
                return propertyNum;
            }
            throw new ArgumentOutOfRangeException(string.Format("玩家{0}佣兵{1}属性值溢出:{2}", userID, generalID, propertyNum));
        }

        /// <summary>
        /// 职业加成值
        /// </summary>
        /// <param name="careerID"></param>
        /// <param name="abilityID"></param>
        /// <returns></returns>
        public static Decimal GetCareerNum(int careerID, AbilityType abilityID)
        {
            decimal careerNum = 0;
            CareerAdditionInfo careerAddInfo = new ConfigCacheSet<CareerAdditionInfo>().FindKey(careerID, abilityID);
            if (careerAddInfo != null)
            {
                careerNum = careerAddInfo.AdditionNum;
            }
            return careerNum;
        }

        /// <summary>
        /// 装备的属性值
        /// </summary>
        /// <returns></returns>
        public static decimal GetEquNum(string userID, int generalID, AbilityType abilityType)
        {
            int valueNum = 0;
            decimal equSumNum = 0;
            var userItemArray = UserItemHelper.GetItems(userID).FindAll(u => u.GeneralID.Equals(generalID) && u.ItemStatus == ItemStatus.YongBing);
            foreach (var item in userItemArray)
            {
                //ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID);
                var itemEquArray = new ConfigCacheSet<ItemEquAttrInfo>().FindAll(m => m.ItemID == item.ItemID);
                foreach (ItemEquAttrInfo equ in itemEquArray)
                {
                    if (equ.AttributeID == abilityType)
                    {
                        valueNum = MathUtils.Addition(equ.BaseNum, (equ.IncreaseNum * item.ItemLv), int.MaxValue);
                        equSumNum = MathUtils.Addition(equSumNum, valueNum, int.MaxValue);
                        break;
                    }
                }
                //灵件配置
                var user = new GameDataCacheSet<GameUser>().FindKey(userID);
                if (user != null)
                {
                    var sparepartList = user.SparePartList.FindAll(m => m.UserItemID.Equals(item.UserItemID));
                    foreach (var sparepart in sparepartList)
                    {
                        foreach (var property in sparepart.Propertys)
                        {
                            if (property.AbilityType == abilityType)
                            {
                                equSumNum = MathUtils.Addition(equSumNum, property.Num.ToDecimal(), decimal.MaxValue);
                            }
                        }
                    }
                }
            }
            return equSumNum;
        }

        /// <summary>
        /// 命运的属性值
        /// </summary>
        public static Decimal GetFateNum(string userID, int generalID, AbilityType abilityType)
        {
            decimal abilityNum = 0;
            //命运水晶生命
            var packageCrystal = UserCrystalPackage.Get(userID);
            UserCrystalInfo[] crystalList = packageCrystal.CrystalPackage.FindAll(m => m.GeneralID.Equals(generalID)).ToArray();

            foreach (UserCrystalInfo item in crystalList)
            {
                decimal effectNum = 0;
                CrystalLvInfo lvInfo = new ConfigCacheSet<CrystalLvInfo>().FindKey(item.CrystalID, item.CrystalLv);
                if (lvInfo != null)
                {
                    effectNum = lvInfo.AbilityNum;
                }
                if (abilityType == item.AbilityType)
                {
                    if (abilityType != AbilityType.ShengMing)
                    {
                        abilityNum = effectNum;
                    }
                }
            }
            return abilityNum;
        }

        /// <summary>
        /// 魔术属性
        /// </summary>
        /// <returns></returns>
        public static Decimal GetEmbattleNum(string userID, AbilityType abilityType)
        {
            decimal magicNum = 0;
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user != null)
            {
                int _currMaxLv = ConfigEnvSet.GetInt("User.CurrMaxLv"); //玩家最大等级
                var magicArray = new GameDataCacheSet<UserMagic>().FindAll(userID, m => m.MagicType == MagicType.JiNeng);
                foreach (UserMagic magic in magicArray)
                {
                    int mlv = 0;
                    if (magic.MagicLv > _currMaxLv)
                    {
                        mlv = _currMaxLv;
                    }
                    else
                    {
                        mlv = magic.MagicLv;
                    }
                    MagicLvInfo lvInfo = new ConfigCacheSet<MagicLvInfo>().FindKey(magic.MagicID, mlv);
                    if (lvInfo != null && lvInfo.AbilityType == abilityType)
                    {
                        magicNum = MathUtils.Addition(magicNum, lvInfo.EffectNum, decimal.MaxValue);
                    }
                }
            }
            return magicNum;
        }


        /// <summary>
        /// 战力数值
        /// </summary>
        /// <returns></returns>
        public static int GetGeneralPowerNum()
        {
            return 0;
        }

        /// <summary>
        /// 好感度加成属性
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static int FeelEffectNum(string userID, int generalID, AbilityType abilityType)
        {
            int effectNum = 0;
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (general != null)
            {
                FeelLvInfo lvInfo = new ConfigCacheSet<FeelLvInfo>().FindKey(general.FeelLv);
                if (lvInfo != null)
                {
                    GeneralProperty property = lvInfo.Property.Find(m => m.AbilityType.Equals(abilityType));
                    if (property != null)
                    {
                        effectNum = property.AbilityValue.ToInt();
                    }
                }
            }
            return effectNum;
        }

        /// <summary>
        /// 修复公会技能
        /// </summary>
        /// <param name="guildID"></param>
        public static void RepairGuildAbility(string guildID)
        {
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null && guild.AbilityInfo.Count > 0)
            {
                var abilitiesList = guild.AbilityInfo;
                foreach (GuildAbility ability in abilitiesList)
                {
                    GuildAbilityInfo abilityInfo = new ConfigCacheSet<GuildAbilityInfo>().FindKey(ability.ID);
                    if (ability.Lv > 0 && ability.Num == 0 && abilityInfo != null)
                    {
                        GuildAbilityLvInfo abilityLvInfo = new ConfigCacheSet<GuildAbilityLvInfo>().FindKey(abilityInfo.ID, ability.Lv);
                        if (abilityLvInfo != null)
                        {
                            ability.UpdateNotify(o =>
                            {
                                TraceLog.ReleaseWriteFatal("修复公会技能;公会ID{0}", guildID);
                                var obj = o as GuildAbility;
                                if (obj == null)
                                {
                                    return false;
                                }
                                obj.Type = abilityInfo.GuildAbilityType;
                                obj.Num = abilityLvInfo.EffectNum;
                                return true;
                            });
                            //guild.Update();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 玩家佣兵先攻
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static int TotalPriorityNum(string userID, int generalID)
        {
            //UserMagic userMagic = new GameDataCacheSet<UserMagic>().Find(publicUserID, s => s.IsEnabled);
            int priorityNum = 0;
            if (generalID > 0)
            {
                priorityNum = PriorityHelper.GeneralTotalPriority(userID, generalID);
            }
            else
            {
                var user = new GameDataCacheSet<GameUser>().FindKey(userID);
                if (user != null)
                {
                    var userMagic = new GameDataCacheSet<UserMagic>().Find(userID, s => s.IsEnabled);
                    if (userMagic != null)
                    {
                        var embattlesArray = new List<UserEmbattle>();
                        embattlesArray = new GameDataCacheSet<UserEmbattle>().FindAll(userID, m => m.MagicID == userMagic.MagicID && m.GeneralID > 0);
                        foreach (UserEmbattle embattle in embattlesArray)
                        {
                            priorityNum = MathUtils.Addition(priorityNum, PriorityHelper.GeneralTotalPriority(embattle.UserID, embattle.GeneralID));
                        }
                    }
                }
            }
            return priorityNum;
        }

        /// <summary>
        /// 加载玩家属性数值
        /// </summary>
        public static void LoadProperty(GameUser user)
        {
            //玩家公会技能加成
            LoadGuildAbility(user.UserID);
        }

        /// <summary>
        /// 加载玩家公会技能加成
        /// </summary>
        public static void LoadGuildAbility(string userID)
        {
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user == null)
            {
                UserCacheGlobal.LoadOffline(userID);
                user = new GameDataCacheSet<GameUser>().FindKey(userID);
            }
            if (!string.IsNullOrEmpty(user.MercenariesID))
            {
                UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(user.MercenariesID);
                if (guild != null && guild.AbilityInfo.Count > 0)
                {
                    if (user.PropertyInfo.Count == 0)
                    {
                        foreach (var guildAbility in guild.AbilityInfo)
                        {
                            GuildAbility ability = new GuildAbility();
                            ability.Type = guildAbility.Type;
                            ability.Num = guildAbility.Num;
                            user.PropertyInfo.Add(ability);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 公会技能升级
        /// </summary>
        public static void UpGuildAbilityLv(string guildID, GuildAbility ability)
        {
            var memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == guildID);
            foreach (GuildMember member in memberArray)
            {
                GameUser user = new GameDataCacheSet<GameUser>().FindKey(member.UserID);
                if (user == null)
                {
                    user = UserCacheGlobal.CheckLoadUser(member.UserID);
                }
                if (user != null && !string.IsNullOrEmpty(user.MercenariesID))
                {
                    UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(user.MercenariesID);
                    if (guild != null && guild.AbilityInfo.Count > 0)
                    {
                        LoadGuildAbility(member.UserID);
                        if (user.PropertyInfo != null && user.PropertyInfo.Count > 0)
                        {
                            GuildAbility guildAbility = user.PropertyInfo.Find(m => m.Type == ability.Type);
                            if (guildAbility != null)
                            {
                                guildAbility.Num = ability.Num;
                            }
                            else
                            {
                                guildAbility = new GuildAbility();
                                guildAbility.Type = ability.Type;
                                guildAbility.Num = ability.Num;
                                user.PropertyInfo.Add(guildAbility);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清除公会技能
        /// </summary>
        /// <param name="user"></param>
        public static void RemoveGuildAbility(GameUser user)
        {
            if (string.IsNullOrEmpty(user.MercenariesID))
            {
                if (user.PropertyInfo != null && user.PropertyInfo.Count > 0)
                {
                    user.PropertyInfo.Clear();
                }
            }
        }


        /// <summary>
        /// 公会技能加成效果
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static decimal GetGuildAbilityNum(string userID, GuildAbilityType abilityType)
        {
            decimal effectNum = 0;
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user != null && user.PropertyInfo != null && user.PropertyInfo.Count > 0)
            {
                var abilitiesList = user.PropertyInfo;
                GuildAbility ability = abilitiesList.Find(m => m.Type == abilityType);
                if (ability != null)
                {
                    effectNum = MathUtils.Addition(effectNum, ability.Num, decimal.MaxValue);
                }
            }
            effectNum = MathUtils.Addition(effectNum, 1, decimal.MaxValue);
            return effectNum;
        }

        /// <summary>
        /// 先攻值
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int GetGeneralpriorityNum(string userID, int generalID)
        {
            int priorityNum = 0;
            List<GeneralProperty> propertiList = GetAbility(userID, generalID,new UserGeneral());
            GeneralProperty property = propertiList.Find(m => m.AbilityType == AbilityType.FirstStrike);
            if (property != null)
            {
                priorityNum = property.AbilityValue.ToInt();
            }
            return priorityNum;
        }


        /// <summary>
        ///  使用道具后的战力加成
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static double InspirePercentEffect(string userID)
        {
            double effectNum = 0;
            //int itemid = 5200; 
            var propsArray = new GameDataCacheSet<UserProps>().FindAll(userID, u => u.PropType == 9 || u.ItemID == 5200);
            foreach (UserProps props in propsArray)
            {
                if (props.DoRefresh() > 0)
                {
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(props.ItemID);
                    if (itemInfo != null && itemInfo.ItemPack.Count > 0)
                    {
                        effectNum = itemInfo.ItemPack[0].EffectNum;
                        break;
                    }
                }
            }
            return effectNum;
        }

        /// <summary>
        /// 世界boss是否已杀
        /// </summary>
        /// <returns></returns>
        public static bool IsBossKill(int activeId)
        {
            GameActive activeInfo = new ShareCacheStruct<GameActive>().FindKey(activeId);
            string envSet = ServerEnvSet.Get(ServerEnvKey.BossKillDate, "");
            if (envSet != null && activeInfo != null)
            {
                DateTime killDate = envSet.ToDateTime();
                string[] list = activeInfo.EnablePeriod.Split(new char[] { ',' });
                //有多个时间段
                foreach (string item in list)
                {
                    DateTime beginTime = item.ToDateTime(DateTime.MinValue);
                    DateTime endTime = beginTime.AddMinutes(activeInfo.Minutes);
                    if (beginTime < killDate && endTime > killDate)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 时间boss被杀时间
        /// </summary>
        public static void BossKillDate()
        {
            ServerEnvSet.Set(ServerEnvKey.BossKillDate, DateTime.Now);
        }

        /// <summary>
        /// boss是否已杀
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool GuildBossKill(string guildID)
        {
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null && guild.GuildBossInfo != null)
            {
                return guild.GuildBossInfo.IsKill;
            }
            return false;
        }

        /// <summary>
        /// boss状态存入公会
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="typeID">公会boss </param>
        public static void UpdateGuildBossKill(string guildID)
        {
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null && guild.GuildBossInfo != null)
            {
                guild.GuildBossInfo.UpdateNotify(obj =>
                {
                    guild.GuildBossInfo.IsKill = true;
                    return true;
                });
            }
        }

        /// <summary>
        /// 多人副本获胜加一次
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        public static void DailyMorePlotRestrainNum(string userID, int plotID)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.UserExtend != null)
            {
                FunPlot plot = new FunPlot();
                plot.PlotID = plotID;
                plot.Num = 1;
                plot.MoreDate = DateTime.Now;

                dailyRestrain.UserExtend.UpdateNotify(obj =>
                {
                    if (dailyRestrain.UserExtend.MorePlot == null)
                    {
                        dailyRestrain.UserExtend.MorePlot = new CacheList<FunPlot>();
                    }
                    dailyRestrain.UserExtend.MorePlot.Add(plot);
                    return true;
                });
                //dailyRestrain.Update();
            }
        }

        /// <summary>
        /// 取出多人副本次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        public static int GetDailyMorePlotNum(string userID, int plotID)
        {
            int moreplotNum = 0;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.UserExtend != null)
            {
                if (dailyRestrain.UserExtend.MorePlot != null && dailyRestrain.UserExtend.MorePlot.Count > 0)
                {
                    moreplotNum = dailyRestrain.UserExtend.MorePlot.FindAll(m => m.PlotID == plotID).Count;
                }
            }
            return moreplotNum;
        }

        /// <summary>
        /// 清空多人副本次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        public static void RemoveDailyMorePlot(string userID, FunctionEnum activeType)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.UserExtend != null)
            {
                if (dailyRestrain.UserExtend.MorePlot != null && dailyRestrain.UserExtend.MorePlot.Count > 0)
                {
                    FunPlot funPlots = dailyRestrain.UserExtend.MorePlot[0];
                    if (funPlots != null)
                    {
                        DateTime endDate = funPlots.MoreDate.AddHours(1).ToString("yyyy-MM-dd HH:00:00").ToDateTime();
                        if (endDate < DateTime.Now && DateTime.Now > MorePlotEndDate(userID, activeType))
                        {
                            dailyRestrain.UserExtend.UpdateNotify(obj =>
                            {
                                if (dailyRestrain.UserExtend.MorePlot == null)
                                {
                                    dailyRestrain.UserExtend.MorePlot = new CacheList<FunPlot>();
                                }
                                dailyRestrain.UserExtend.MorePlot.Clear();
                                return true;
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 多人副本结束时间
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static DateTime MorePlotEndDate(string userID, FunctionEnum activeType)
        {
            DateTime enableTime = new DateTime();
            GameActive[] gameActivesArray
                = new List<GameActive>(new GameActiveCenter(userID).GetActiveList()).FindAll(m => m.ActiveType == activeType).ToArray();
            if (gameActivesArray.Length > 0)
            {
                GameActive active = gameActivesArray[0];
                if (active != null)
                {
                    enableTime = active.EndTime;
                }
            }
            return enableTime;
        }


        /// <summary>
        /// 附魔符属性
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public static decimal EnchantProprety(string userID, int generalID, AbilityType abilityType)
        {
            decimal proNum = 0;
            var package = UserItemPackage.Get(userID);
            var enchantPackage = UserEnchant.Get(userID);
            if (package != null && enchantPackage != null)
            {
                var useritem = package.ItemPackage.Find(m => !m.IsRemove && m.GeneralID == generalID && m.Equparts == EquParts.WuQi);
                if (useritem != null)
                {
                    var enchantList = enchantPackage.EnchantPackage.FindAll(m => m.UserItemID == useritem.UserItemID && m.AbilityType == abilityType);
                    foreach (var info in enchantList)
                    {
                        decimal abilityNum = EnchantFinalNum(info);
                        proNum = MathUtils.Addition(proNum, abilityNum);
                    }
                }
            }
            return proNum;
        }

        public static decimal EnchantFinalNum(UserEnchantInfo userEnchant)
        {
            //最终属性=基础属性*成长率*附魔符等级
            decimal growthRate = EnchantAbilityNum(userEnchant.EnchantID, userEnchant.EnchantLv) * userEnchant.MaxMature / GameConfigSet.MaxEnchantMature * userEnchant.EnchantLv;
            if (growthRate >= 1)
            {
                growthRate = MathUtils.RoundCustom(growthRate, 0);
            }
            if (growthRate > 0 && growthRate < 1)
            {
                growthRate = MathUtils.RoundCustom(growthRate, 3);
            }
            return growthRate;
        }

        /// <summary>
        /// 附魔符属性值
        /// </summary>
        /// <param name="enchantID"></param>
        /// <param name="enchantLv"></param>
        /// <returns></returns>
        public static decimal EnchantAbilityNum(int enchantID, short enchantLv)
        {
            decimal abilityNum = 0;
            EnchantLvInfo enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(enchantID, enchantLv);
            if (enchantLvInfo != null)
            {
                abilityNum = enchantLvInfo.Num;
            }
            return abilityNum;
        }

        /// <summary>
        /// 佣兵位置还原
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static void EmbattlePostion(string userID)
        {
            var magicList = new GameDataCacheSet<UserMagic>().FindAll(userID, m => m.MagicType == MagicType.MoFaZhen && m.IsEnabled);
            if (magicList.Count > 0)
            {
                var embattleList = new GameDataCacheSet<UserEmbattle>().FindAll(userID, m => m.MagicID == magicList[0].MagicID);
                foreach (UserEmbattle embattle in embattleList)
                {
                    var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, embattle.GeneralID);
                    if (userGeneral != null)
                    {
                        userGeneral.ResetEmbatleReplace();
                        userGeneral.LifeNum = userGeneral.LifeMaxNum;
                    }
                }
            }
        }

        /// <summary>
        /// 佣兵升级属性
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        private static decimal GeneralUpGradeProperty(string userID, int generalID, AbilityType abilityType)
        {
            decimal val = 0;
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (general != null && general.Attribute.Count > 0)
            {
                var property = general.Attribute.Find(s => s.AbilityType == abilityType);
                if (property != null)
                {
                    val = property.AbilityValue;
                }
            }
            return val;
        }
    }
}