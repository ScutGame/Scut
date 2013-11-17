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
using ZyGames.Framework;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 玩家阵形
    /// </summary>
    public class UserEmbattleQueue : EmbattleQueue
    {
        /// <summary>
        /// 战斗初始气势
        /// </summary>
        private int CombatMomentum = ConfigEnvSet.GetInt("Combat.Momentum");
        private decimal CombatBaojiNum = ConfigEnvSet.GetDecimal("Combat.BaojiNum");
        private decimal CombatHitiNum = ConfigEnvSet.GetDecimal("Combat.HitiNum");
        private int MagicPriorityIncrease = ConfigEnvSet.GetInt("Combat.MagicPriorityIncrease");

        private static int LansePriorityMinLv = 0;
        private static int LansePriorityIncreaseNum = ConfigEnvSet.GetInt("Combat.LanSeEquPriorityIncrease");

        private static int ZisePriorityMinLv = 0;
        private static int ZisePriorityIncreaseNum = ConfigEnvSet.GetInt("Combat.ZiseEquPriorityIncrease");
        public static List<UserEmbattle> embattleList = new List<UserEmbattle>();
        private string _userID;
        private int _magicID;
        private CombatType _combatType;

        static UserEmbattleQueue()
        {
            string[] valList = ConfigEnvSet.GetString("Combat.LanSeEquPriorityMinNum").Split(new char[] { '=' });
            if (valList.Length == 2)
            {
                LansePriorityMinLv = valList[0].ToInt();
                LansePriorityIncreaseNum = valList[1].ToInt();
            }

            valList = ConfigEnvSet.GetString("Combat.ZiseEquPriorityMinNum").Split(new char[] { '=' });
            if (valList.Length == 2)
            {
                ZisePriorityMinLv = valList[0].ToInt();
                ZisePriorityIncreaseNum = valList[1].ToInt();
            }
        }

        public UserEmbattleQueue(string userID, int magicID)
            : this(userID, magicID, 0)
        {
        }
        public UserEmbattleQueue(string userID, int magicID, double inspirePercent)
            : this(userID, magicID, inspirePercent, CombatType.Plot)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">玩家ID</param>
        /// <param name="magicID">使用的阵形</param>
        /// <param name="inspirePercent">鼓舞加成</param>
        /// <param name="combatType">战斗类型</param>
        public UserEmbattleQueue(string userID, int magicID, double inspirePercent, CombatType combatType)
        {
            inspirePercent = MathUtils.Addition(inspirePercent, CombatHelper.InspirePercentEffect(userID), double.MaxValue); //使用战力道具后的战力
            _userID = userID;
            _magicID = magicID;
            _combatType = combatType;
            //修复玩家开启的阵法与GameUser中的阵法ID不同
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (userInfo != null)
            {
                var magicsArray = new GameDataCacheSet<UserMagic>().FindAll(userID, m => m.MagicType == MagicType.MoFaZhen && m.IsEnabled);
                if (magicsArray.Count > 0 && userInfo.UseMagicID != magicsArray[0].MagicID)
                {
                    userInfo.UseMagicID = magicsArray[0].MagicID;
                    //userInfo.Update();
                    _magicID = magicsArray[0].MagicID;
                }
            }
            embattleList = new GameDataCacheSet<UserEmbattle>().FindAll(userID, m => m.MagicID == magicID);

            short replacePotion = GeneralReplaceAttack.CheckReplacePostion(userID);

            HashSet<int> generalHash = new HashSet<int>();
            foreach (UserEmbattle embattle in embattleList)
            {
                //wuzf 8-18 修复多个相同佣兵阵形数据
                if (embattle.GeneralID == 0) continue;
                if (generalHash.Contains(embattle.GeneralID))
                {
                    embattle.GeneralID = 0;
                    //embattle.Update();
                    continue;
                }
                else
                {
                    generalHash.Add(embattle.GeneralID);
                }
                int index = embattle.Position - 1;
                IGeneral general = Create(embattle, inspirePercent, replacePotion);
                if (general != null && general.ReplacePosition > 0)
                {
                    index = general.Position - 1;
                }
                SetQueue(index, general);
                this.PriorityNum += PriorityHelper.GeneralTotalPriority(embattle.UserID, embattle.GeneralID);
            }

            if (embattleList.Count == 0)
            {
                new BaseLog().SaveLog(new Exception(string.Format("加载玩家阵形异常UserId:{0},magicID:{1}", userID, magicID)));
            }
        }

        /// <summary>
        /// 获取命力
        /// </summary>
        /// <param name="embattle"></param>
        /// <returns></returns>
        public double GetMingliNum(UserEmbattle embattle)
        {
            double num = 0;
            var packageCrystal = UserCrystalPackage.Get(embattle.UserID);
            if (packageCrystal != null)
            {
                UserCrystalInfo[] userCrystalList = packageCrystal.CrystalPackage.FindAll(m => m.GeneralID.Equals(embattle.GeneralID)).ToArray();
                foreach (UserCrystalInfo userCrystal in userCrystalList)
                {
                    num += (userCrystal.CurrExprience / 10);
                }
            }
            return num;
        }

        private CombatGeneral Create(UserEmbattle embattle, double inspirePercent, short replacePotion)
        {
            GameUser userInfo = UserCacheGlobal.CheckLoadUser(embattle.UserID);
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(embattle.UserID, embattle.GeneralID);
            if (userGeneral == null || userInfo == null)
            {
                return null;
            }

            AbilityInfo ability = new ConfigCacheSet<AbilityInfo>().FindKey(userGeneral.AbilityID);
            CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(userGeneral.CareerID);

            if (ability == null || careerInfo == null)
            {
                throw new Exception("职业或技能为空");
            }
            //职业加成
            decimal baojiNum = GetCareerAddition(careerInfo, AbilityType.BaoJi);
            decimal bishaNum = GetCareerAddition(careerInfo, AbilityType.BiSha);
            decimal renxingNum = GetCareerAddition(careerInfo, AbilityType.RenXing);
            decimal hitNum = GetCareerAddition(careerInfo, AbilityType.MingZhong);
            decimal shanbiNum = GetCareerAddition(careerInfo, AbilityType.ShanBi);
            decimal gedangNum = GetCareerAddition(careerInfo, AbilityType.GeDang);
            decimal pojiNum = GetCareerAddition(careerInfo, AbilityType.PoJi);
            //公会技能加成

            short powerNum = MathUtils.Addition(userGeneral.PowerNum, userGeneral.TrainingPower, short.MaxValue);
            short soulNum = MathUtils.Addition(userGeneral.SoulNum, userGeneral.TrainingSoul, short.MaxValue);
            short intellectNum = MathUtils.Addition(userGeneral.IntellectNum, userGeneral.TrainingIntellect, short.MaxValue);
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(embattle.UserID);
            if (user != null && !string.IsNullOrEmpty(user.MercenariesID) && userGeneral.GeneralID == LanguageManager.GetLang().GameUserGeneralID)
            {
                powerNum = MathUtils.RoundCustom(powerNum * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.PowerNum)).ToShort();
                soulNum = MathUtils.RoundCustom(soulNum * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.SoulNum)).ToShort();
                intellectNum = MathUtils.RoundCustom(intellectNum * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.IntellectNum)).ToShort();
            }

            if (embattle.GeneralID == LanguageManager.GetLang().GameUserGeneralID)
            {
                //法宝基础属性加成
                powerNum = MathUtils.Addition(powerNum, TrumpAbilityAttack.TrumpPropertyNum(embattle.UserID, embattle.GeneralID, AbilityType.PowerNum));
                soulNum = MathUtils.Addition(soulNum, TrumpAbilityAttack.TrumpPropertyNum(embattle.UserID, embattle.GeneralID, AbilityType.SoulNum));
                intellectNum = MathUtils.Addition(intellectNum, TrumpAbilityAttack.TrumpPropertyNum(embattle.UserID, embattle.GeneralID, AbilityType.IntelligenceNum));

                //法宝--技能属性转换获得的值
                //法宝--技能属性转换获得的值
                decimal trumpPower = TrumpAbilityAttack.ConversionPropertyNum(embattle.UserID, powerNum, soulNum, intellectNum, AbilityType.PowerNum);
                decimal trumpsoul = TrumpAbilityAttack.ConversionPropertyNum(embattle.UserID, powerNum, soulNum, intellectNum, AbilityType.SoulNum);
                decimal trumpintellect = TrumpAbilityAttack.ConversionPropertyNum(embattle.UserID, powerNum, soulNum, intellectNum, AbilityType.IntelligenceNum);
                powerNum = MathUtils.Addition(trumpPower.ToShort(), powerNum);
                soulNum = MathUtils.Addition(trumpsoul.ToShort(), soulNum);
                intellectNum = MathUtils.Addition(trumpintellect.ToShort(), intellectNum);
            }

            if (userGeneral.LifeMaxNum == 0 || userGeneral.LifeNum > userGeneral.LifeMaxNum)
            {
                userGeneral.RefreshMaxLife();
                userGeneral.LifeNum = userGeneral.LifeMaxNum;
            }
            //
            decimal effectValue = 0;// AbilityDispose.GetAbilityEffect(embattle.UserID, embattle.GeneralID, ability.AbilityID);
            decimal selfEffectValue = 0;
            List<AbilityInfo> selfAbilityList = AbilityDispose.GetSelfAbilityList(embattle.UserID, embattle.GeneralID,
                                                                                  ability.AbilityID, out selfEffectValue);
            CombatGeneral general = new CombatGeneral()
            {
                UserID = embattle.UserID,
                Position = embattle.Position,
                GeneralID = embattle.GeneralID,
                CombatType = _combatType,
                GeneralName = userGeneral.GeneralName,
                HeadID = userGeneral.HeadID,
                CareerID = userGeneral.CareerID,
                CareerType = careerInfo.CareerType,
                IsMove = careerInfo.IsMove,
                LifeNum = userGeneral.LifeNum,
                LifeMaxNum = userGeneral.LifeMaxNum,
                Lv = userGeneral.GeneralLv,
                Momentum = (short)CombatMomentum,
                Ability = ability,
                IsAttrMove = ability.IsMove,
                BaojiNum = MathUtils.Addition(CombatBaojiNum, baojiNum, decimal.MaxValue),
                BishaNum = bishaNum,
                RenxingNum = renxingNum,
                HitNum = MathUtils.Addition(userGeneral.HitProbability, hitNum, decimal.MaxValue),
                ShanbiNum = shanbiNum,
                GedangNum = gedangNum,
                PojiNum = pojiNum,
                BattleStatus = BattleStatus.Normal,
                PowerNum = powerNum,
                SoulNum = soulNum,
                IntellectNum = intellectNum,
                ExtraAttack = new CombatProperty(),
                ExtraDefense = new CombatProperty(),
                InspirePercent = inspirePercent,
                Fatigue = userInfo.Fatigue,
                UserStatus = userInfo.UserStatus,
                IsMonster = false,
                IsWait = false,
                EffectValue = effectValue,
                AbilityInfoList = AbilityDispose.GetTriggerAbilityList(embattle.UserID, embattle.GeneralID, ability.AbilityID),
                SelfAbilityInfoList = selfAbilityList,
                SelfEffectValue = selfEffectValue
            };

            if (_combatType == CombatType.User)
            {
                //玩家竞技都是满血
                general.LifeNum = general.LifeMaxNum;
                userGeneral.ResetEmbatleReplace();
            }

            //判断是否替补佣兵
            if (replacePotion == general.Position)
            {
                if (_combatType == CombatType.Country && general.LifeNum > 0)
                {
                    general.ReplacePosition = userGeneral.ReplacePosition;
                }
                if (general.ReplacePosition > 0)
                {
                    general.Position = general.ReplacePosition;
                }
                else
                {
                    general.IsWait = true;
                }
            }
            SetExtraProperty(general);
            AbilityAddition(selfAbilityList, general);
            KarmaAddition(embattle.UserID, general);
            SJTAddition(embattle, general);
            
            return general;
        }

        private static decimal GetCareerAddition(CareerInfo careerInfo, AbilityType abilityType)
        {
            CareerAdditionInfo addition = new ConfigCacheSet<CareerAdditionInfo>().FindKey(careerInfo.CareerID, abilityType);
            return addition != null ? addition.AdditionNum : 0;
        }
        /// <summary>
        /// 圣吉塔加成
        /// </summary>
        /// <param name="embattle"></param>
        /// <param name="combatGeneral"></param>
        private void SJTAddition(UserEmbattle embattle, CombatGeneral combatGeneral)
        {
            if(_combatType==CombatType.ShengJiTa)
            {
                var cacheSetUserSJT = new GameDataCacheSet<UserShengJiTa>();
                var userSJT = cacheSetUserSJT.FindKey(embattle.UserID);
                if (userSJT != null)
                {
                    combatGeneral.LifeNum = MathUtils.Addition(combatGeneral.LifeNum, (combatGeneral.LifeNum * userSJT.LifeNum).ToInt());
                    combatGeneral.LifeMaxNum = MathUtils.Addition(combatGeneral.LifeMaxNum, (combatGeneral.LifeNum * userSJT.LifeNum).ToInt());
                    combatGeneral.ExtraAttack.AdditionWuliNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionWuliNum, (combatGeneral.ExtraAttack.AdditionWuliNum * userSJT.WuLiNum));
                    combatGeneral.ExtraAttack.AdditionHunjiNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionHunjiNum, (combatGeneral.ExtraAttack.AdditionHunjiNum * userSJT.FunJiNum));
                    combatGeneral.ExtraAttack.AdditionMofaNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionMofaNum, (combatGeneral.ExtraAttack.AdditionMofaNum * userSJT.MofaNum));
                    combatGeneral.ExtraDefense.AdditionWuliNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionWuliNum, (combatGeneral.ExtraDefense.AdditionWuliNum * userSJT.WuLiNum));
                    combatGeneral.ExtraDefense.AdditionHunjiNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionHunjiNum, (combatGeneral.ExtraDefense.AdditionHunjiNum * userSJT.FunJiNum));
                    combatGeneral.ExtraDefense.AdditionMofaNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionMofaNum, (combatGeneral.ExtraDefense.AdditionMofaNum * userSJT.MofaNum));
                }
            }
        }
        /// <summary>
        /// 缘分加成
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="general"></param>
        public static void KarmaAddition(string userId, CombatGeneral general)
        {
            var cacheSetKarma = new ConfigCacheSet<KarmaInfo>();
            var karmaInfo = cacheSetKarma.FindKey(general.GeneralID);
            if (karmaInfo != null && karmaInfo.KarmaList != null && karmaInfo.KarmaList.Count > 0)
            {
                var karmaList = karmaInfo.KarmaList;
                foreach (var karma in karmaList)
                {
                    bool isKarma = true;
                    string[] valueIDArray = karma.ValueID.Split(',');
                    switch (karma.KarmaType)
                    {
                        case KarmaType.YongBing:
                            foreach (var id in valueIDArray)
                            {
                                if (embattleList.Find(s => s.GeneralID == id.ToInt()) == null)
                                {
                                    isKarma = false;
                                    break;
                                }
                            }
                            if (isKarma)
                            {
                                KarmaAdditionValue(karma, general);
                            }


                            break;
                        case KarmaType.ZhuangBei:
                            var cacheSetItem = new GameDataCacheSet<UserItemPackage>();
                            var userItem = cacheSetItem.FindKey(userId);
                            if (userItem != null && userItem.ItemPackage != null)
                            {

                                foreach (var id in valueIDArray)
                                {
                                    if (userItem.ItemPackage.Find(s => s.ItemID == id.ToInt() && s.GeneralID == general.GeneralID) == null)
                                    {
                                        isKarma = false;
                                        break;
                                    }
                                }
                                if (isKarma)
                                {
                                    KarmaAdditionValue(karma, general);
                                }

                            }
                            break;
                        case KarmaType.JiNen:
                            var cacheSetAbility = new GameDataCacheSet<UserAbility>();
                            var ability = cacheSetAbility.FindKey(userId);
                            if (ability != null && ability.AbilityList != null)
                            {

                                var abilityList = ability.AbilityList;

                                foreach (var id in valueIDArray)
                                {
                                    if (abilityList.Find(s => s.AbilityID == id.ToInt() && s.GeneralID == general.GeneralID) == null)
                                    {
                                        isKarma = false;
                                        break;
                                    }
                                }
                                if (isKarma)
                                {
                                    KarmaAdditionValue(karma, general);
                                }
                            }
                            break;
                        case KarmaType.ShuiJing:
                            var cacheSetCrystal = new GameDataCacheSet<UserCrystalPackage>();
                            var userCrystal = cacheSetCrystal.FindKey(userId);

                            if (userCrystal != null && userCrystal.CrystalPackage != null)
                            {

                                var crystalList = userCrystal.CrystalPackage;

                                foreach (var id in valueIDArray)
                                {
                                    if (crystalList.Find(s => s.CrystalID == id.ToInt() && s.GeneralID == general.GeneralID) == null)
                                    {
                                        isKarma = false;
                                        break;
                                    }
                                }
                                if (isKarma)
                                {
                                    KarmaAdditionValue(karma, general);
                                }
                            }
                            break;
                    }
                }
            }
        }
        private static void KarmaAdditionValue(Karma karma, CombatGeneral combatGeneral)
        {
            decimal attValue = karma.AttValue;
            switch (karma.AbilityType.ToInt())
            {
                //生命回复
                case 1:

                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.LifeNum = MathUtils.Addition(combatGeneral.LifeNum, (combatGeneral.LifeNum * attValue).ToInt());
                        combatGeneral.LifeMaxNum = MathUtils.Addition(combatGeneral.LifeMaxNum, (combatGeneral.LifeMaxNum * attValue).ToInt());
                    }
                    else
                    {
                        combatGeneral.LifeNum = MathUtils.Addition(combatGeneral.LifeNum, attValue).ToInt();
                        combatGeneral.LifeMaxNum = MathUtils.Addition(combatGeneral.LifeMaxNum, attValue).ToInt();
                    }
                    break;
                //物理攻击
                case 2:
                   
                        if (combatGeneral.ExtraAttack != null)
                        {
                            combatGeneral.ExtraAttack.AdditionWuliNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionWuliNum, attValue);
                        }

                    break;
                //魂技攻击
                case 3:
                   
                        if (combatGeneral.ExtraAttack != null)
                        {
                            combatGeneral.ExtraAttack.AdditionHunjiNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionHunjiNum, attValue);
                        }

                    break;
                //魔法攻击
                case 4:
                   
                        if (combatGeneral.ExtraAttack != null)
                        {
                            combatGeneral.ExtraAttack.AdditionMofaNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionMofaNum, attValue);
                        }
                    break;
                //物理防御
                case 5:
  
                        if (combatGeneral.ExtraDefense != null)
                        {
                            combatGeneral.ExtraDefense.AdditionWuliNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionWuliNum, attValue);
                        }
                    break;
                //魂技防御
                case 6:
                    
                        if (combatGeneral.ExtraDefense != null)
                        {
                            combatGeneral.ExtraDefense.AdditionHunjiNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionHunjiNum, attValue);
                        }
                    break;
                //魔法防御
                case 7:
                   
                        if (combatGeneral.ExtraDefense != null)
                        {
                            combatGeneral.ExtraDefense.AdditionMofaNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionMofaNum, attValue);
                        }

                    break;
                //暴击
                case 8:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.BaojiNum = MathUtils.Addition(combatGeneral.BaojiNum, (combatGeneral.BaojiNum * attValue));

                    }
                    else
                    {
                        combatGeneral.BaojiNum = MathUtils.Addition(combatGeneral.BaojiNum, attValue);

                    }
                    break;
                //命中
                case 9:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.HitNum = MathUtils.Addition(combatGeneral.HitNum, (combatGeneral.HitNum * attValue));

                    }
                    else
                    {
                        combatGeneral.HitNum = MathUtils.Addition(combatGeneral.HitNum, attValue);

                    }
                    break;
                //破击
                case 10:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.PojiNum = MathUtils.Addition(combatGeneral.PojiNum, (combatGeneral.PojiNum * attValue));

                    }
                    else
                    {
                        combatGeneral.PojiNum = MathUtils.Addition(combatGeneral.PojiNum, attValue);

                    }
                    break;
                //韧性
                case 11:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.RenxingNum = MathUtils.Addition(combatGeneral.RenxingNum, (combatGeneral.RenxingNum * attValue));

                    }
                    else
                    {
                        combatGeneral.RenxingNum = MathUtils.Addition(combatGeneral.RenxingNum, attValue);

                    }
                    break;
                //闪避
                case 12:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.ShanbiNum = MathUtils.Addition(combatGeneral.ShanbiNum, (combatGeneral.ShanbiNum * attValue));

                    }
                    else
                    {
                        combatGeneral.ShanbiNum = MathUtils.Addition(combatGeneral.ShanbiNum, attValue);

                    }
                    break;
                //格挡
                case 13:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.GedangNum = MathUtils.Addition(combatGeneral.GedangNum, (combatGeneral.GedangNum * attValue));

                    }
                    else
                    {
                        combatGeneral.GedangNum = MathUtils.Addition(combatGeneral.GedangNum, attValue);

                    }
                    break;
                //必杀
                case 14:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.BishaNum = MathUtils.Addition(combatGeneral.BishaNum, (combatGeneral.BishaNum * attValue));

                    }
                    else
                    {
                        combatGeneral.BishaNum = MathUtils.Addition(combatGeneral.BishaNum, attValue);

                    }
                    break;
                //力量
                case 26:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.PowerNum = MathUtils.Addition(combatGeneral.PowerNum, (combatGeneral.PowerNum * attValue).ToInt());

                    }
                    else
                    {
                        combatGeneral.PowerNum = MathUtils.Addition(combatGeneral.PowerNum, attValue.ToInt());
                    }
                    break;
                //魂力
                case 27:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.SoulNum = MathUtils.Addition(combatGeneral.SoulNum, (combatGeneral.SoulNum * attValue).ToShort());

                    }
                    else
                    {
                        combatGeneral.SoulNum = MathUtils.Addition(combatGeneral.SoulNum, attValue.ToShort());
                    }
                    break;
                //智力
                case 28:
                    if (karma.AttValueType == 2)
                    {
                        combatGeneral.IntellectNum = MathUtils.Addition(combatGeneral.IntellectNum, (combatGeneral.IntellectNum * attValue).ToShort());

                    }
                    else
                    {
                        combatGeneral.IntellectNum = MathUtils.Addition(combatGeneral.IntellectNum, attValue.ToShort());
                    }
                    break;

            }
        }
        /// <summary>
        /// 被动技能加成
        /// </summary>
        /// <param name="abilityInfoList"></param>
        /// <param name="combatGeneral"></param>
        private  void AbilityAddition(List<AbilityInfo> abilityInfoList, CombatGeneral combatGeneral)
        {
            if (abilityInfoList != null && abilityInfoList.Count > 0)
            {
                abilityInfoList.ForEach(obj =>
                {
                    decimal increaseNum = obj.IncreaseNum.ToDecimal();
                    int baseEffectNum = obj.BaseEffectNum.ToInt();
                    string[] abilityTypeArray = obj.AbilityType.Split(',');
                    foreach (var s in abilityTypeArray)
                    {
                        switch (s)
                        {
                            //生命回复
                            case "1":

                                if (increaseNum > 0)
                                {
                                    combatGeneral.LifeNum = MathUtils.Addition(combatGeneral.LifeNum, (combatGeneral.LifeNum * increaseNum).ToInt());
                                    combatGeneral.LifeMaxNum = MathUtils.Addition(combatGeneral.LifeMaxNum, (combatGeneral.LifeNum * increaseNum).ToInt());
                                }
                                else
                                {
                                    combatGeneral.LifeNum = MathUtils.Addition(combatGeneral.LifeNum, baseEffectNum);
                                    combatGeneral.LifeMaxNum = MathUtils.Addition(combatGeneral.LifeMaxNum, baseEffectNum);
                                }
                                break;
                            //物理攻击
                            case "2":
                                
                                    if (combatGeneral.ExtraAttack != null)
                                    {
                                        combatGeneral.ExtraAttack.AdditionWuliNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionWuliNum, baseEffectNum);
                                    }
                                
                                break;
                            //魂技攻击
                            case "3":
                                if (increaseNum > 0 && combatGeneral.ExtraAttack != null)
                                {
                                    combatGeneral.ExtraAttack.AdditionHunjiNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionHunjiNum, (combatGeneral.ExtraAttack.AdditionHunjiNum * increaseNum).ToInt());

                                }
                                else
                                {
                                    if (combatGeneral.ExtraAttack != null)
                                    {
                                        combatGeneral.ExtraAttack.AdditionHunjiNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionHunjiNum, baseEffectNum);
                                    }
                                }
                                break;
                            //魔法攻击
                            case "4":
                                if (increaseNum > 0 && combatGeneral.ExtraAttack != null)
                                {
                                    combatGeneral.ExtraAttack.AdditionMofaNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionMofaNum, (combatGeneral.ExtraAttack.AdditionMofaNum * increaseNum).ToInt());

                                }
                                else
                                {
                                    if (combatGeneral.ExtraAttack != null)
                                    {
                                        combatGeneral.ExtraAttack.AdditionMofaNum = MathUtils.Addition(combatGeneral.ExtraAttack.AdditionMofaNum, baseEffectNum);
                                    }
                                }
                                break;
                            //物理防御
                            case "5":
                                if (increaseNum > 0 && combatGeneral.ExtraDefense != null)
                                {
                                    combatGeneral.ExtraDefense.AdditionWuliNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionWuliNum, (combatGeneral.ExtraDefense.AdditionWuliNum * increaseNum).ToInt());

                                }
                                else
                                {
                                    if (combatGeneral.ExtraDefense != null)
                                    {
                                        combatGeneral.ExtraDefense.WuliNum = MathUtils.Addition(combatGeneral.ExtraDefense.WuliNum, baseEffectNum);
                                    }
                                }
                                break;
                            //魂技防御
                            case "6":
                                if (increaseNum > 0 && combatGeneral.ExtraDefense != null)
                                {
                                    combatGeneral.ExtraDefense.AdditionHunjiNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionHunjiNum, (combatGeneral.ExtraDefense.AdditionHunjiNum * increaseNum).ToInt());

                                }
                                else
                                {
                                    if (combatGeneral.ExtraDefense != null)
                                    {
                                        combatGeneral.ExtraDefense.AdditionHunjiNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionHunjiNum, baseEffectNum);
                                    }
                                }
                                break;
                            //魔法防御
                            case "7":
                                if (increaseNum > 0 && combatGeneral.ExtraDefense != null)
                                {
                                    combatGeneral.ExtraDefense.AdditionMofaNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionMofaNum, (combatGeneral.ExtraDefense.AdditionMofaNum * increaseNum).ToInt());

                                }
                                else
                                {
                                    if (combatGeneral.ExtraDefense != null)
                                    {
                                        combatGeneral.ExtraDefense.AdditionMofaNum = MathUtils.Addition(combatGeneral.ExtraDefense.AdditionMofaNum, baseEffectNum);
                                    }
                                }
                                break;
                            //暴击
                            case "8":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.BaojiNum = MathUtils.Addition(combatGeneral.BaojiNum, (combatGeneral.BaojiNum * increaseNum));

                                }
                                else
                                {
                                    combatGeneral.BaojiNum = MathUtils.Addition(combatGeneral.BaojiNum, baseEffectNum);

                                }
                                break;
                            //命中
                            case "9":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.HitNum = MathUtils.Addition(combatGeneral.HitNum, (combatGeneral.HitNum * increaseNum));

                                }
                                else
                                {
                                    combatGeneral.HitNum = MathUtils.Addition(combatGeneral.HitNum, baseEffectNum);

                                }
                                break;
                            //破击
                            case "10":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.PojiNum = MathUtils.Addition(combatGeneral.PojiNum, (combatGeneral.PojiNum * increaseNum));

                                }
                                else
                                {
                                    combatGeneral.PojiNum = MathUtils.Addition(combatGeneral.PojiNum, baseEffectNum);

                                }
                                break;
                            //韧性
                            case "11":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.RenxingNum = MathUtils.Addition(combatGeneral.RenxingNum, (combatGeneral.RenxingNum * increaseNum));

                                }
                                else
                                {
                                    combatGeneral.RenxingNum = MathUtils.Addition(combatGeneral.RenxingNum, baseEffectNum);

                                }
                                break;
                            //闪避
                            case "12":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.ShanbiNum = MathUtils.Addition(combatGeneral.ShanbiNum, (combatGeneral.ShanbiNum * increaseNum));

                                }
                                else
                                {
                                    combatGeneral.ShanbiNum = MathUtils.Addition(combatGeneral.ShanbiNum, baseEffectNum);

                                }
                                break;
                            //格挡
                            case "13":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.GedangNum = MathUtils.Addition(combatGeneral.GedangNum, (combatGeneral.GedangNum * increaseNum));

                                }
                                else
                                {
                                    combatGeneral.GedangNum = MathUtils.Addition(combatGeneral.GedangNum, baseEffectNum);

                                }
                                break;
                            //必杀
                            case "14":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.BishaNum = MathUtils.Addition(combatGeneral.BishaNum, (combatGeneral.BishaNum * increaseNum));

                                }
                                else
                                {
                                    combatGeneral.BishaNum = MathUtils.Addition(combatGeneral.BishaNum, baseEffectNum);

                                }
                                break;
                            //力量
                            case "26":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.PowerNum = MathUtils.Addition(combatGeneral.PowerNum, (combatGeneral.PowerNum * increaseNum).ToInt());

                                }
                                else
                                {
                                    combatGeneral.PowerNum = MathUtils.Addition(combatGeneral.PowerNum, baseEffectNum.ToInt());
                                }
                                break;
                            //魂力
                            case "27":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.SoulNum = MathUtils.Addition(combatGeneral.SoulNum, (combatGeneral.SoulNum * increaseNum).ToInt());

                                }
                                else
                                {
                                    combatGeneral.SoulNum = MathUtils.Addition(combatGeneral.SoulNum, baseEffectNum.ToInt());
                                }
                                break;
                            //智力
                            case "28":
                                if (increaseNum > 0)
                                {
                                    combatGeneral.IntellectNum = MathUtils.Addition(combatGeneral.IntellectNum, (combatGeneral.IntellectNum * increaseNum).ToInt());

                                }
                                else
                                {
                                    combatGeneral.IntellectNum = MathUtils.Addition(combatGeneral.IntellectNum, baseEffectNum.ToInt());
                                }
                                break;

                        }
                    }

                });

            }
        }
        /// <summary>
        /// 附加加成：武器攻击 + 命运水晶攻击 + 魔术技能攻击 + 阵法攻击 + 附魔攻击
        /// </summary>
        /// <returns></returns>
        private void SetExtraProperty(CombatGeneral general)
        {
            //命运水晶攻击
            SetCrystal(general);
            //武器攻击
            var package = UserItemPackage.Get(general.UserID);
            var equList = package.ItemPackage.FindAll(m => !m.IsRemove && m.GeneralID.Equals(general.GeneralID) && m.ItemStatus == ItemStatus.YongBing);
            foreach (var item in equList)
            {
                //general.LifeNum += GetEquAttrEffect(item, AbilityType.ShengMing);
                general.ExtraAttack.WuliNum += GetEquAttrEffect(item, AbilityType.WuLiGongJi);
                general.ExtraAttack.HunjiNum += GetEquAttrEffect(item, AbilityType.HunJiGongJi);
                general.ExtraAttack.MofaNum += GetEquAttrEffect(item, AbilityType.MoFaGongJi);

                general.ExtraDefense.WuliNum += GetEquAttrEffect(item, AbilityType.WuLiFangYu);
                general.ExtraDefense.HunjiNum += GetEquAttrEffect(item, AbilityType.HunJiFangYu);
                general.ExtraDefense.MofaNum += GetEquAttrEffect(item, AbilityType.MoFaFangYu);

                SetSparePart(general, item);
            }
            //阵法攻击
            int magicLv = 0;
            UserMagic userMagic = new GameDataCacheSet<UserMagic>().FindKey(this._userID, this._magicID);
            if (userMagic != null)
            {
                magicLv = userMagic.MagicLv;
                MagicLvInfo mlv = new ConfigCacheSet<MagicLvInfo>().FindKey(this._magicID, magicLv);
                if (mlv != null)
                {
                    SetAbilityValue(general, mlv.AbilityType, mlv.EffectNum);
                }
            }

            //魔术技能攻击
            var userMagicList = new GameDataCacheSet<UserMagic>().FindAll(_userID, m => m.MagicType == MagicType.JiNeng);
            foreach (UserMagic item in userMagicList)
            {
                MagicLvInfo magicLvItem = new ConfigCacheSet<MagicLvInfo>().FindKey(item.MagicID, item.MagicLv);
                if (magicLvItem != null)
                {
                    SetAbilityValue(general, magicLvItem.AbilityType, magicLvItem.EffectNum);
                }
            }
            int feelWuliNum = CombatHelper.FeelEffectNum(general.UserID, general.GeneralID, AbilityType.WuLiFangYu);
            int feelHunjiNum = CombatHelper.FeelEffectNum(general.UserID, general.GeneralID, AbilityType.HunJiFangYu);
            int feelMofaNum = CombatHelper.FeelEffectNum(general.UserID, general.GeneralID, AbilityType.MoFaFangYu);
            general.ExtraDefense.WuliNum += feelWuliNum;
            general.ExtraDefense.HunjiNum += feelHunjiNum;
            general.ExtraDefense.MofaNum += feelMofaNum;

            //附魔符数值
            SetEnchant(general);
            //法宝数值
            SetTrump(general);
        }

        /// <summary>
        /// 灵件配置
        /// </summary>
        /// <param name="general"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static void SetSparePart(CombatGeneral general, UserItemInfo item)
        {
            var user = new GameDataCacheSet<GameUser>().FindKey(general.UserID);
            if (user != null)
            {
                var sparepartList = user.SparePartList.FindAll(m => m.UserItemID.Equals(item.UserItemID));
                foreach (var sparepart in sparepartList)
                {
                    foreach (var property in sparepart.Propertys)
                    {
                        switch (property.AbilityType)
                        {
                            case AbilityType.WuLiGongJi:
                                general.ExtraAttack.WuliNum += property.Num.ToInt();
                                break;
                            case AbilityType.HunJiGongJi:
                                general.ExtraAttack.HunjiNum += property.Num.ToInt();
                                break;
                            case AbilityType.MoFaGongJi:
                                general.ExtraAttack.MofaNum += property.Num.ToInt();
                                break;
                            case AbilityType.WuLiFangYu:
                                general.ExtraDefense.WuliNum += property.Num.ToInt();
                                break;
                            case AbilityType.HunJiFangYu:
                                general.ExtraDefense.HunjiNum += property.Num.ToInt();
                                break;
                            case AbilityType.MoFaFangYu:
                                general.ExtraDefense.MofaNum += property.Num.ToInt();
                                break;
                            case AbilityType.MingZhong:
                                general.HitNum = MathUtils.Addition(general.HitNum, property.Num.ToDecimal(), decimal.MaxValue);
                                break;
                            case AbilityType.ShanBi:
                                general.ShanbiNum = MathUtils.Addition(general.ShanbiNum, property.Num.ToDecimal(), decimal.MaxValue);
                                break;
                            case AbilityType.RenXing:
                                general.RenxingNum = MathUtils.Addition(general.RenxingNum, property.Num.ToDecimal(), decimal.MaxValue);
                                break;
                            case AbilityType.BaoJi:
                                general.BaojiNum = MathUtils.Addition(general.BaojiNum, property.Num.ToDecimal(), decimal.MaxValue);
                                break;
                            case AbilityType.PoJi:
                                general.PojiNum = MathUtils.Addition(general.PojiNum, property.Num.ToDecimal(), decimal.MaxValue);
                                break;
                            case AbilityType.GeDang:
                                general.GedangNum = MathUtils.Addition(general.GedangNum, property.Num.ToDecimal(), decimal.MaxValue);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 附魔符配置
        /// </summary>
        /// <param name="general"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static void SetEnchant(CombatGeneral general)
        {
            var enchantList = new List<UserEnchantInfo>();
            var user = new GameDataCacheSet<GameUser>().FindKey(general.UserID);
            if (user != null)
            {
                var package = UserItemPackage.Get(general.UserID);
                var enchantPackage = UserEnchant.Get(general.UserID);
                if (package != null && enchantPackage != null)
                {
                    var useritem = package.ItemPackage.Find(m => !m.IsRemove && m.GeneralID == general.GeneralID && m.Equparts == EquParts.WuQi);
                    if (useritem != null)
                    {
                        enchantList = enchantPackage.EnchantPackage.FindAll(m => m.UserItemID == useritem.UserItemID);
                    }
                }
                foreach (var enchantInfo in enchantList)
                {
                    decimal abilityNum = CombatHelper.EnchantFinalNum(enchantInfo);
                    SetAbilityValue(general, enchantInfo.AbilityType, abilityNum);
                }
            }
        }

        /// <summary>
        /// 法宝配置
        /// </summary>
        /// <param name="general"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static void SetTrump(CombatGeneral general)
        {
            var user = new GameDataCacheSet<GameUser>().FindKey(general.UserID);
            if (user != null)
            {
                UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(general.UserID, TrumpInfo.CurrTrumpID);
                if (userTrump != null && userTrump.LiftNum > 0 && general.GeneralID == LanguageManager.GetLang().GameUserGeneralID && userTrump.PropertyInfo.Count > 0)
                {
                    userTrump.PropertyInfo.Foreach(property =>
                    {
                        SetAbilityValue(general, property.AbilityType, property.AbilityValue);
                        return true;
                    });
                }
            }
        }

        private static int GetEquAttrEffect(UserItemInfo item, AbilityType abilityType)
        {
            ItemEquAttrInfo equAttr = new ConfigCacheSet<ItemEquAttrInfo>().FindKey(item.ItemID, abilityType);
            return (equAttr != null ? equAttr.GetEffectNum(item.ItemLv) : 0);
        }

        private void SetCrystal(CombatGeneral general)
        {
            var packageCrystal = UserCrystalPackage.Get(general.UserID);
            if (packageCrystal != null)
            {
                UserCrystalInfo[] crystalList = packageCrystal.CrystalPackage.FindAll(m => m.GeneralID.Equals(general.GeneralID)).ToArray();
                foreach (UserCrystalInfo item in crystalList)
                {
                    AbilityType abilityType = new ConfigCacheSet<CrystalInfo>().FindKey(item.CrystalID).AbilityID;
                    var crystalLv = new ConfigCacheSet<CrystalLvInfo>().FindKey(item.CrystalID, item.CrystalLv);
                    decimal effectNum = crystalLv == null ? 0 : crystalLv.AbilityNum;
                    SetAbilityValue(general, abilityType, effectNum);
                }
            }
        }

        private static void SetAbilityValue(CombatGeneral general, AbilityType abilityType, decimal effectNum)
        {
            switch (abilityType)
            {
                case AbilityType.ShengMing:
                    //注释原因：佣兵取最大生命时已经计算
                    //general.LifeNum += effectNum.ToInt();
                    break;
                case AbilityType.WuLiGongJi:
                    general.ExtraAttack.WuliNum += effectNum.ToInt();
                    break;
                case AbilityType.HunJiGongJi:
                    general.ExtraAttack.HunjiNum += effectNum.ToInt();
                    break;
                case AbilityType.MoFaGongJi:
                    general.ExtraAttack.MofaNum += effectNum.ToInt();
                    break;
                case AbilityType.WuLiFangYu:
                    general.ExtraDefense.WuliNum += effectNum.ToInt();
                    break;
                case AbilityType.HunJiFangYu:
                    general.ExtraDefense.HunjiNum += effectNum.ToInt();
                    break;
                case AbilityType.MoFaFangYu:
                    general.ExtraDefense.MofaNum += effectNum.ToInt();
                    break;
                case AbilityType.BaoJi:
                    general.BaojiNum += effectNum;
                    break;
                case AbilityType.MingZhong:
                    general.HitNum += effectNum;
                    break;
                case AbilityType.PoJi:
                    general.PojiNum += effectNum;
                    break;
                case AbilityType.RenXing:
                    general.RenxingNum += effectNum;
                    break;
                case AbilityType.ShanBi:
                    general.ShanbiNum += effectNum;
                    break;
                case AbilityType.GeDang:
                    general.GedangNum += effectNum;
                    break;
                case AbilityType.BiSha:
                    general.BishaNum += effectNum;
                    break;
                case AbilityType.Qishi:
                    general.Momentum += effectNum.ToShort();
                    break;

                default:
                    break;
            }
        }

    }
}