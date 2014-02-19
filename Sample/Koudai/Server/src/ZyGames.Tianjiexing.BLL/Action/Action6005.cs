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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6005_公会详情接口
    /// </summary>
    public class Action6005 : BaseAction
    {
        private string guildID = string.Empty;
        private string chairman = string.Empty;
        private int currPeople = 0;
        private int maxPeople = 0;
        private int upExperience;
        private UserGuild guild = null;
        private short memberPost = 0;
        private int weekExperience = 0;

        public Action6005(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6005, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(guild == null ? string.Empty : guild.GuildName.ToNotNullString());
            PushIntoStack(chairman.ToNotNullString());
            PushIntoStack(guild == null ? LanguageManager.GetLang().shortInt : (short)guild.GuildLv);
            PushIntoStack(guild == null ? 0 : guild.GuildRank);
            PushIntoStack(currPeople);
            PushIntoStack(maxPeople);
            PushIntoStack(guild == null ? 0 : guild.CurrExperience);
            PushIntoStack(upExperience);
            PushIntoStack(guild == null ? string.Empty : guild.GuildDesc.ToNotNullString());
            PushIntoStack(guild == null ? string.Empty : guild.Announcement.ToNotNullString());
            PushIntoStack(memberPost);
            PushIntoStack(weekExperience);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("GuildID", ref guildID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserHelper.ChecheDailyContribution(ContextUser.MercenariesID, ContextUser.UserID);
            guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null)
            {
                var memberLog = new ShareCacheStruct<GuildMemberLog>().FindKey(guildID) ?? new GuildMemberLog();
                List<MemberLog> guildLogArray = memberLog.GetLog(null);
                foreach (MemberLog log in guildLogArray)
                {
                    if (IsCurrentWeek(log.InsertDate))
                    {
                        weekExperience = MathUtils.Addition(weekExperience, log.Experience, int.MaxValue);
                    }
                }
                List<GuildMember> memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == guild.GuildID);
                foreach (GuildMember member in memberArray)
                {
                    if (member.PostType == PostType.Chairman)
                    {
                        GameUser userInfo = UserCacheGlobal.CheckLoadUser(member.UserID); 
                        if (userInfo != null)
                        {
                            chairman = userInfo.NickName;
                        }
                    }
                    if (member.UserID == ContextUser.UserID)
                    {
                        memberPost = (short)member.PostType;
                    }
                }
                currPeople = memberArray.Count;
                GuildLvInfo guildLvInfo = new ConfigCacheSet<GuildLvInfo>().FindKey(guild.GuildLv);
                if (guildLvInfo != null)
                {
                    int upLv =MathUtils.Addition(guild.GuildLv, (short)1, (short)GuildLvInfo.GuildMaxLv);
                    if (new ConfigCacheSet<GuildLvInfo>().FindKey(upLv) != null)
                    {
                        upExperience = new ConfigCacheSet<GuildLvInfo>().FindKey(upLv).UpExperience;
                    }
                    maxPeople = MathUtils.Addition(guildLvInfo.MaxPeople, guild.AddMember);
                }
            }
            return true;
        }

        /// <summary>
        /// 是否本周时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static bool IsCurrentWeek(DateTime dateTime)
        {
            DateTime currDt = DateTime.Now.Date;
            int currWeek = currDt.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)currDt.DayOfWeek;
            var fromDate = currDt.AddDays((int)DayOfWeek.Monday - currWeek);
            var toDate = fromDate.AddDays(7);
            if (fromDate <= dateTime.Date && toDate > dateTime.Date)
            {
                return true;
            }
            return false;
        }
    }
}