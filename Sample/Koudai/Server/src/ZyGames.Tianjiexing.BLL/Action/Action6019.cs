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
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 6019_公会申请审核通过接口
    /// </summary>
    public class Action6019 : BaseAction
    {
        private string guildID = string.Empty;
        private string memberID = string.Empty;


        public Action6019(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6019, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("GuildID", ref guildID)
                 && httpGet.GetString("MemberID", ref memberID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(guildID, ContextUser.UserID);
            if (member != null && member.PostType == PostType.Member)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6007_AuditPermissions;
                return false;
            }

            List<GuildMember> memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == guildID);
            UserGuild guildInfo = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guildInfo != null)
            {
                GuildLvInfo guildLvInfo = new ConfigCacheSet<GuildLvInfo>().FindKey(guildInfo.GuildLv);
                if (guildLvInfo != null)
                {
                    int maxPeople = MathUtils.Addition(guildInfo.AddMember, guildLvInfo.MaxPeople);
                    if (memberArray.Count > 0 && memberArray.Count >= maxPeople)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St6019_GuildMaxPeople;
                        return false;
                    }
                }
            }
            GameUser userInfo = UserCacheGlobal.CheckLoadUser(memberID);
            if (userInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;

            }
            var memberList = new ShareCacheStruct<GuildMember>().FindAll(s => s.UserID == memberID);
            if (memberList.Count > 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6019_AddGuild;
                return false;
            }

            GuildMember gMember = new GuildMember()
            {
                GuildID = guildID,
                UserID = memberID,
                PostType = PostType.Member,
                Contribution = 0,
                TotalContribution = 0,
                IncenseNum = 0,
                DevilNum = 0,
                IsDevil = 0,
                InsertDate = DateTime.Now
            };
            new ShareCacheStruct<GuildMember>().Add(gMember);
            userInfo.MercenariesID = guildID;
            CombatHelper.LoadGuildAbility(userInfo.UserID); //加载公会技能

            var cacheSet = new ShareCacheStruct<UserApply>();
            List<UserApply> appliesArray = cacheSet.FindAll(m => m.UserID == memberID);
            foreach (UserApply apply in appliesArray)
            {
                cacheSet.Delete(apply);
            }

            return true;
        }
    }
}