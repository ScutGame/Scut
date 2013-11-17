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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{

    public class AbilityDispose
    {
        private static GameDataCacheSet<UserAbility> _cacheSetUserAbility = new GameDataCacheSet<UserAbility>();
        private static ConfigCacheSet<AbilityInfo> _cacheSetAbility = new ConfigCacheSet<AbilityInfo>();
        private static ConfigCacheSet<AbilityLvInfo> _cacheSetAbilityLv = new ConfigCacheSet<AbilityLvInfo>();
        /// <summary>
        /// 获取技能等级加成
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="generalId"></param>
        /// <param name="abilityId"></param>
        /// <returns></returns>
        public static decimal GetAbilityEffect(string userId, int generalId, int abilityId)
        {

            var userAbility = _cacheSetUserAbility.FindKey(userId);
            decimal effectValue = 0;
            if (userAbility != null && userAbility.AbilityList != null)
            {
                var ability2 =
                    userAbility.AbilityList.Find(
                        s => s.AbilityID == abilityId && s.GeneralID == generalId);
                if (ability2 != null)
                {
                    var cacheSetAbilityLv = new ConfigCacheSet<AbilityLvInfo>();
                    var abilityLv = cacheSetAbilityLv.FindKey(ability2.AbilityID, ability2.AbilityLv);
                    if (abilityLv != null)
                    {
                        effectValue = abilityLv.EffectValue;
                    }
                }
            }
            return effectValue;
        }

        /// <summary>
        /// 触发魂技
        /// </summary>
        /// <param name="abilityInfoList"></param>
        /// <returns></returns>
        public static AbilityInfo TriggerAbilityList(List<AbilityInfo> abilityInfoList)
        {
            if (abilityInfoList != null && abilityInfoList.Count > 0)
            {
                int index = abilityInfoList.Count > 1 ? RandomUtils.GetRandom(0, abilityInfoList.Count) : 0;
                var abilityInfo = abilityInfoList[index];
                int notProbability = 1000 - (abilityInfo.Probability * 1000).ToInt();
                int[] precent = new int[] { notProbability, (abilityInfo.Probability * 1000).ToInt() };
                int indexPrecent = RandomUtils.GetHitIndexByTH(precent);
                if (indexPrecent == 1)
                {
                    return abilityInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取攻击魂技
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gentralId"></param>
        /// <param name="abilityId"></param>
        /// <returns></returns>
        public static List<AbilityInfo> GetTriggerAbilityList(string userId, int gentralId, int abilityId)
        {
            var userAbility = _cacheSetUserAbility.FindKey(userId);
            List<AbilityInfo> abilityInfoList = new List<AbilityInfo>();
            if (userAbility != null && userAbility.AbilityList != null)
            {
                var abilityList =
                    userAbility.AbilityList.FindAll(s => s.GeneralID == gentralId && s.AbilityID != abilityId);
                abilityList.ForEach(ability =>
                {
                    var abilityInfo = _cacheSetAbility.FindKey(ability.AbilityID);
                    if (abilityInfo != null && abilityInfo.IsActive == 0)
                    {
                        abilityInfoList.Add(abilityInfo);
                    }

                });
            }
            return abilityInfoList;
        }

        /// <summary>
        ///  获取自身魂技
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gentralId"></param>
        /// <param name="abilityId"></param>
        /// <returns></returns>
        public static List<AbilityInfo> GetSelfAbilityList(string userId, int gentralId, int abilityId, out decimal SelfEffectValue)
        {
            SelfEffectValue = 0;
            decimal effectNum = 0;
            List<AbilityInfo> abilityInfoList = new List<AbilityInfo>();
            var userAbility = _cacheSetUserAbility.FindKey(userId);
            if (userAbility != null && userAbility.AbilityList != null)
            {
                var abilityList = userAbility.AbilityList.FindAll(s => s.GeneralID == gentralId && s.AbilityID != abilityId);
                abilityList.ForEach(ability =>
                {
                    var abilityInfo = _cacheSetAbility.FindKey(ability.AbilityID);
                    var abilityLv = _cacheSetAbilityLv.FindKey(ability.AbilityID, ability.AbilityLv);
                    if (abilityInfo != null && abilityInfo.IsActive == 1)
                    {
                        abilityInfoList.Add(abilityInfo);
                        effectNum = MathUtils.Addition(effectNum, abilityInfo.RatioNum);
                    }
                    if (abilityLv != null)
                    {
                        effectNum = MathUtils.Addition(effectNum, abilityLv.EffectValue);
                    }

                });

            }
            SelfEffectValue = effectNum;
            return abilityInfoList;
        }

    }
}