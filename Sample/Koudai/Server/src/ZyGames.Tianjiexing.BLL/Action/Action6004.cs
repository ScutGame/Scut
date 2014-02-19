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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;



namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6004_公会活动列表接口
    /// </summary>
    public class Action6004 : BaseAction
    {
        private string guildID = string.Empty;
        private int demandExperience = 0;
        private List<ActivityShow> activityArray = new List<ActivityShow>();
        private short nextLv = 0;
        private UserGuild userGuild = null;

        public Action6004(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6004, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(userGuild == null ? LanguageManager.GetLang().shortInt : (short)userGuild.GuildLv);
            this.PushIntoStack(nextLv);
            this.PushIntoStack(userGuild == null ? 0 : userGuild.CurrExperience);
            this.PushIntoStack(demandExperience);
            this.PushIntoStack(activityArray.Count);
            foreach (ActivityShow show in activityArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(show.ActivityID);
                this.PushIntoStack(dsItem);
            }
            PushIntoStack(GetBossDateType(userGuild == null ? null : userGuild.GuildBossInfo));
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
            userGuild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (userGuild != null)
            {
                nextLv = MathUtils.Addition(userGuild.GuildLv, (short) 1, (short) GuildLvInfo.GuildMaxLv);
                GuildLvInfo guildLvInfo = new ConfigCacheSet<GuildLvInfo>().FindKey(nextLv);
                if (guildLvInfo != null)
                {
                    demandExperience = MathUtils.Subtraction(guildLvInfo.UpExperience, userGuild.CurrExperience, 0);
                    activityArray = guildLvInfo.ActivityDesc.ToList();
                }
            }
            return true;
        }

        public static short GetBossDateType(GuildBossInfo bossInfo)
        {
            if (bossInfo != null)
            {
                if (UserHelper.IsCurrentWeek(bossInfo.RefreshDate))
                {
                    return (short)bossInfo.EnableWeek;
                }
            }
            return (short)0;
        }
    }
}