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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 魂技类
    /// </summary>
    public class UserAbilityHelper
    {
        public static GameDataCacheSet<UserAbility> _cacheSetAbility = new GameDataCacheSet<UserAbility>();
        /// <summary>
        /// 添加玩家魂技
        /// </summary>
        /// <param name="abilityId"></param>
        /// <param name="userId"></param>
        public static void AddUserAbility(int abilityId, int userId, int generalID, int position)
        {
            var userAbility = _cacheSetAbility.FindKey(userId.ToString());
            var ability = userAbility != null && userAbility.AbilityList != null
                              ? userAbility.AbilityList.Find(s => s.AbilityID == abilityId)
                              : null;
            var abilityLv = new ConfigCacheSet<AbilityLvInfo>().FindKey(abilityId, 1);
            int experienceNum = abilityLv != null ? abilityLv.Experience : 0;
            if (userAbility == null)
            {
                userAbility = new UserAbility(userId);
                ability = new Ability();
                userAbility.CreateDate = DateTime.Now;
                ability.UserItemID = Guid.NewGuid().ToString();
                ability.AbilityID = abilityId;
                ability.AbilityLv = 1;
                ability.GeneralID = generalID;
                ability.ExperienceNum = experienceNum;
                ability.Position = position;
                userAbility.AbilityList.Add(ability);
                _cacheSetAbility.Add(userAbility);
                // 添加到玩家集邮册
                UserAlbumHelper.AddUserAlbum(userId.ToString(), AlbumType.Ability, abilityId);
            }
            else
            {
                ability = new Ability();
                ability.UserItemID = Guid.NewGuid().ToString();
                userAbility.CreateDate = DateTime.Now;
                ability.AbilityID = abilityId;
                ability.AbilityLv = 1;
                ability.GeneralID = generalID;
                ability.Position = position;
                ability.ExperienceNum = experienceNum;
                userAbility.AbilityList.Add(ability);
                // 添加到玩家集邮册
                UserAlbumHelper.AddUserAlbum(userId.ToString(), AlbumType.Ability, abilityId);
            }

            UserAlbumHelper.AddUserAlbum(userId.ToString(), AlbumType.General, generalID);
        }

        /// <summary>
        /// 获得的物品是否魂技卡
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static bool UseUserItem(string userID, int itemID)
        {
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (itemInfo == null || itemInfo.PropType != 17 || itemInfo.EffectNum <= 0)
            {
                return false;
            }
            AbilityInfo abilityInfo = new ConfigCacheSet<AbilityInfo>().FindKey(itemInfo.EffectNum);
            if (abilityInfo != null)
            {
                AddUserAbility(itemInfo.EffectNum, userID.ToInt(), 0, 0);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 被动魂技效果
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<SelfAbilityEffect> GetSelfAbilityEffectList(string userId, short role)
        {
            List<SelfAbilityEffect> selfAbilityEffectList = new List<SelfAbilityEffect>();
            var cacheSetUserAbility = new GameDataCacheSet<UserAbility>();
            var cacheSetUserEmbattle = new GameDataCacheSet<UserEmbattle>();
            var cacheSetAbilityInfo = new ConfigCacheSet<AbilityInfo>();
            var userAbility = cacheSetUserAbility.FindKey(userId);
            var userMagic = new GameDataCacheSet<UserMagic>().Find(userId, s => s.IsEnabled);
            if (userAbility != null && userAbility.AbilityList != null)
            {
                var userEmbattleList = cacheSetUserEmbattle.FindAll(userId, s => s.MagicID == userMagic.MagicID);
                userEmbattleList.ForEach(obj =>
                {
                    if (obj.GeneralID > 0)
                    {
                        var abilityList =
                            userAbility.AbilityList.FindAll(s => s.GeneralID == obj.GeneralID);
                        abilityList.ForEach(ability =>
                        {
                            var abilityInfo = cacheSetAbilityInfo.FindKey(ability.AbilityID);
                            if (abilityInfo != null && abilityInfo.IsActive == 1)
                            {
                                SelfAbilityEffect selfAbilityEffect = new SelfAbilityEffect();
                                selfAbilityEffect.GeneralID = obj.GeneralID;
                                selfAbilityEffect.EffectID1 = abilityInfo.EffectID1;
                                selfAbilityEffect.FntHeadID = abilityInfo.FntHeadID;
                                selfAbilityEffect.IsIncrease = abilityInfo.IsIncrease;
                                selfAbilityEffect.Position = obj.Position;
                                selfAbilityEffect.Role = role;
                                selfAbilityEffectList.Add(selfAbilityEffect);
                            }
                        });
                    }
                });
            }
            return selfAbilityEffectList;
        }

        
    }
}