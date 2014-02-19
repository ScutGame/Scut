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
using System.Data;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6407_详细战报列表接口
    /// </summary>
    public class Action6407 : BaseAction
    {
        private string combatID;
        private int pageIndex;
        private int pageSize;
        private string guildName1;
        private string guildName2;
        private string guildName3;
        private int pageCount;
        private List<MemberGroup> memberGroupList = new List<MemberGroup>();

        public Action6407(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6407, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(guildName1);
            this.PushIntoStack(guildName2);
            this.PushIntoStack(guildName3);
            this.PushIntoStack(pageCount);
            this.PushIntoStack(memberGroupList.Count);
            foreach (var member in memberGroupList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(GameUserName(member.ConquerMember).ToNotNullString());
                dsItem.PushIntoStack(UserGuildName(member.ConquerMember).ToNotNullString());
                dsItem.PushIntoStack(GameUserName(member.DefeatMember).ToNotNullString());
                dsItem.PushIntoStack(UserGuildName(member.DefeatMember).ToNotNullString());
                dsItem.PushIntoStack(member.ID.ToNotNullString());

                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("CombatID", ref combatID)
                 && httpGet.GetInt("PageIndex", ref pageIndex)
                 && httpGet.GetInt("PageSize", ref pageSize))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ServerFightGroup fightGroup = new ShareCacheStruct<ServerFightGroup>().FindKey(combatID);
            if (fightGroup != null)
            {
                guildName1 = GuildFightCombat.GuildName(fightGroup.GuildIDA);
                guildName2 = GuildFightCombat.GuildName(fightGroup.GuildIDB);
                guildName3 = GuildFightCombat.GuildName(fightGroup.WinGuildID);
                var groupList =
                    new ShareCacheStruct<MemberGroup>().FindAll(s => !s.IsRemove && s.FastID == fightGroup.FastID && s.GuildIDA == fightGroup.GuildIDA && s.GuildIDB == fightGroup.GuildIDB);

                foreach (MemberGroup member in groupList)
                {
                    if (member.GuildIDA == fightGroup.WinGuildID)
                    {
                        member.IsEnemy = true;
                    }
                    else
                    {
                        member.IsEnemy = false;
                    }
                    if (member.Win)
                    {
                        member.ConquerMember = member.UserIDA;
                        member.DefeatMember = member.UserIDB;
                    }
                    else
                    {
                        member.ConquerMember = member.UserIDB;
                        member.DefeatMember = member.UserIDA;
                    }
                }
                memberGroupList = groupList.GetPaging(pageIndex, pageSize, out pageCount);
            }
            return true;
        }

        public static string GameUserName(string userID)
        {
            var gameUser = UserCacheGlobal.CheckLoadUser(userID);
            if (gameUser != null)
            {
                return gameUser.NickName;
            }
            return string.Empty;
        }

        public static string UserGuildName(string userID)
        {
            var memberList = new ShareCacheStruct<GuildMember>().FindAll(s => s.UserID == userID);
            if (memberList.Count > 0)
            {
                var userGuild = new ShareCacheStruct<UserGuild>().FindKey(memberList[0].GuildID);
                if (userGuild != null)
                {
                    return userGuild.GuildName;
                }
            }
            return string.Empty;
        }
    }
}