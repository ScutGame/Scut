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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6206_公会技能配置信息下发接口
    /// </summary>
    public class Action6206 : BaseAction
    {
        private int clientVersion;

        private List<GuildAbilityInfo> abilityInfoArray = new List<GuildAbilityInfo>();

        public Action6206(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6206, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(abilityInfoArray.Count);
            foreach (GuildAbilityInfo abilityInfo in abilityInfoArray)
            {
                var guildAbilityLvInfos = new ConfigCacheSet<GuildAbilityLvInfo>().FindAll(m => m.ID == abilityInfo.ID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(abilityInfo.ID);
                dsItem.PushIntoStack(abilityInfo.IsCityCombat ? (short)1 : (short)0);
                dsItem.PushIntoStack(abilityInfo.AbilityName.ToNotNullString());
                dsItem.PushIntoStack((short)abilityInfo.GuildAbilityType);
                dsItem.PushIntoStack(abilityInfo.AbilityHead.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo.AbilityDesc.ToNotNullString());

                dsItem.PushIntoStack(guildAbilityLvInfos.Count);
                foreach (GuildAbilityLvInfo lvInfo in guildAbilityLvInfos)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(lvInfo.AbilityLv);
                    dsItem1.PushIntoStack(lvInfo.UpDonateNum);
                    dsItem1.PushIntoStack(lvInfo.EffectNum.ToString().ToNotNullString());
                    dsItem.PushIntoStack(dsItem1);
                }
                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ClientVersion", ref clientVersion))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int currVersion = new ConfigCacheSet<ConfigVersion>().FindKey(VersionType.GuildAbility).CurVersion;
            abilityInfoArray = new ConfigCacheSet<GuildAbilityInfo>().FindAll(m => m.Version > clientVersion && m.Version <= currVersion);
            return true;
        }
    }
}