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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Component;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6409_争夺战成员列表接口
    /// </summary>
    public class Action6409 : BaseAction
    {
        private string cityName;
        private int codeTime;
        private string[] guildStr = new string[0];
        private FightCombat fightCombat = new FightCombat();
        private FightStage stage;
        private string fatigue;

        public Action6409(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6409, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(cityName.ToNotNullString());
            this.PushIntoStack(codeTime);
            this.PushIntoStack(stage.ToShort());
            this.PushIntoStack(guildStr.Length);
            foreach (var guildID in guildStr)
            {
                short isLead;
                short guildNum = 0;
                List<FightUser> memberCombat = new List<FightUser>();
                UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
                ServerFight fight = new ShareCacheStruct<ServerFight>().FindKey(fightCombat.FastID, guildID);
                if (fight != null && !string.IsNullOrEmpty(fight.CombatMember))
                {
                    string combatMember = fight.CombatMember.TrimEnd(',');
                    if (combatMember.Length > 1)
                    {
                        guildNum = (short)combatMember.Split(',').Length;
                    }
                    memberCombat = GuildFightCombat._fightUserList.FindAll(m => !m.IsRemove && m.GuildID == guildID);
                }
                isLead = GuildIsLead(guildID, stage, fightCombat.FastID) ? (short)1 : (short)0;
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(guildID.ToNotNullString());
                dsItem.PushIntoStack(userGuild == null ? string.Empty : userGuild.GuildName.ToNotNullString());
                dsItem.PushIntoStack(userGuild == null ? (short)0 : (short)userGuild.GuildLv);
                dsItem.PushIntoStack((short)guildNum);
                dsItem.PushIntoStack((short)isLead);
                dsItem.PushIntoStack(memberCombat.Count);
                foreach (var member in memberCombat)
                {
                    GameUser user = UserCacheGlobal.CheckLoadUser(member.UserId);
                    UserGeneral userGeneral = UserGeneral.GetMainGeneral(member.UserId);
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(member.UserId.ToNotNullString());
                    dsItem1.PushIntoStack(user == null ? string.Empty : user.NickName.ToNotNullString());
                    dsItem1.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.HeadID.ToNotNullString());
                    dsItem1.PushIntoStack(user == null ? (short)0 : (short)user.UserLv);

                    dsItem.PushIntoStack(dsItem1);
                }
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(fatigue.ToNotNullString());
        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                return true;
            }
        }

        public override bool TakeAction()
        {
            if (string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6203_GuildMemberNotEnough;
                return false;
            }
            string guildFight = string.Empty;
            DateTime nextDate;
            stage = GuildFightCombat.GetStage(out nextDate);
            if (stage != FightStage.Apply)
            {
                codeTime = (int)(nextDate - DateTime.Now).TotalSeconds;
            }

            ServerFight fight = new ShareCacheStruct<ServerFight>().FindKey(fightCombat.FastID, ContextUser.MercenariesID);
            if (fight != null)
            {
                CityInfo cityInfo = new ConfigCacheSet<CityInfo>().FindKey(fight.CityID);
                if (cityInfo != null)
                {
                    cityName = cityInfo.CityName;
                }
                FightStage fightStage;
                if (stage <= FightStage.quarter_final)
                {
                    fightStage = FightStage.quarter_final;
                }
                else if (stage > FightStage.quarter_final && stage < FightStage.final_Wait)
                {
                    fightStage = FightStage.semi_final;
                }
                else
                {
                    fightStage = FightStage.final;
                }
                var fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == fightCombat.FastID && s.Stage == fightStage);
                if (fightGroupList.Count == 0)
                {
                    fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == fightCombat.FastID && s.Stage < fightStage);
                    if (fightGroupList.Count == 0)
                    {
                        fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == fightCombat.FastID && s.Stage >= fightStage);
                    }
                }
                foreach (var fightGroup in fightGroupList)
                {
                    if (!string.IsNullOrEmpty(fightGroup.WinGuildID) && fightGroup.WinGuildID == ContextUser.MercenariesID)
                    {
                        guildFight = fightGroup.WinGuildID;
                        continue;
                    }
                    if (fightGroup.GuildIDA == ContextUser.MercenariesID || fightGroup.GuildIDB == ContextUser.MercenariesID)
                    {
                        guildFight = fightGroup.GuildIDA + "," + fightGroup.GuildIDB;
                    }
                }
                if (!string.IsNullOrEmpty(guildFight))
                {
                    guildStr = guildFight.Split(',');
                }
                TraceLog.ReleaseWriteFatal("6409公会争斗战配对城市{0} 阶段：{1}，公会:{2}。", fight.CityID, stage, guildFight.ToNotNullString());
            }
            var totalfatigue = ContextUser.Fatigue * GameConfigSet.Fatigue;
            int tfatigue = (int)(totalfatigue * 100);
            fatigue = string.Format(LanguageManager.GetLang().St6409_fatigueDesc, ContextUser.Fatigue, tfatigue);
            if (guildStr.Length == 0)
            {
                guildFight = ContextUser.MercenariesID + ",";
                guildStr = guildFight.Split(',');
            }
            return true;
        }

        public static bool GuildIsLead(string guildID, FightStage stage, int fastID)
        {
            var fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == fastID && s.Stage == stage && (s.GuildIDA == guildID || s.GuildIDB == guildID));
            if (fightGroupList.Count > 0)
            {
                ServerFightGroup fightGroup = fightGroupList[0];
                if (fightGroup.GuildIDA == guildID && fightGroup.Awin >= fightGroup.Bwin)
                {
                    return true;
                }
                else if (fightGroup.GuildIDB == guildID && fightGroup.Bwin >= fightGroup.Awin)
                {
                    return true;
                }
            }
            return false;
        }
    }
}