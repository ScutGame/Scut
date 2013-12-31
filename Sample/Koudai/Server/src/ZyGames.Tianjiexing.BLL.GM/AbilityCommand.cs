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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.GM
{
    public class AbilityCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int abilityId = args.Length > 0 ? args[0].Trim().ToInt() : 0;
            if (abilityId > 0)
            {
                AddUserAbility(abilityId, UserID.ToInt(), 0, 0);
            }
        }

        /// <summary>
        /// 添加玩家魂技
        /// </summary>
        /// <param name="abilityId"></param>
        /// <param name="userId"></param>
        private void AddUserAbility(int abilityId, int userId, int generalID, int position)
        {
             GameDataCacheSet<UserAbility> _cacheSetAbility = new GameDataCacheSet<UserAbility>();
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
            }
        }
    }
}