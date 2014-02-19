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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;
using System;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6109_Boss战时间设定接口
    /// </summary>
    public class Action6109 : BaseAction
    {
        private BossDateType dateType;

        public Action6109(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6109, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("BossDateType", ref dateType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild != null)
            {
                GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
                if (member == null || member.PostType != PostType.Chairman)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6008_NotChairman;
                    return false;
                }
                GameActive active = new ShareCacheStruct<GameActive>().FindKey(UserGuild.ActiveID);
                List<BossDate> bossDateList = UserHelper.GetBossDate().FindAll(u => u.EnableWeek == dateType);
                GuildBossInfo guildBossInfo = guild.GuildBossInfo;
                if (bossDateList.Count > 0)
                {
                    BossDate bossDate = bossDateList[0];

                    if (guildBossInfo != null)
                    {
                        if (guildBossInfo.RefreshDate != MathUtils.SqlMinDate)
                        {
                            if (UserHelper.IsCurrentWeek(guild.GuildBossInfo.RefreshDate))
                            {
                                ErrorCode = LanguageManager.GetLang().ErrorCode;
                                ErrorInfo = LanguageManager.GetLang().St6109_GuildBossTime;
                                return false;
                            }
                        }

                        if (guildBossInfo.BossLv == 0)
                        {
                            guildBossInfo.BossLv = (short)active.BossLv;
                        }
                        guildBossInfo.IsKill = false;
                        guildBossInfo.EnablePeriod = bossDate.EnablePeriod;
                        guildBossInfo.EnableWeek = (int)dateType;
                        guildBossInfo.RefreshDate = DateTime.Now;
                        guild.GuildBossInfo = guildBossInfo;
                        //guild.Update();
                    }
                    else
                    {
                        guildBossInfo=new GuildBossInfo();
                        guildBossInfo.IsKill = false;
                        guildBossInfo.BossLv = (short)active.BossLv;
                        guildBossInfo.EnablePeriod = bossDate.EnablePeriod;
                        guildBossInfo.EnableWeek = (int) dateType;
                        guildBossInfo.RefreshDate = DateTime.Now;
                        guild.GuildBossInfo = guildBossInfo;
                        //guild.Update();
                    }
                }
            }
            return true;
        }
    }
}