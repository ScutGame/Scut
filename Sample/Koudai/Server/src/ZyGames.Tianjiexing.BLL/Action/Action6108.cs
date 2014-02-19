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
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6108_Boss战时间列表接口
    /// </summary>
    public class Action6108 : BaseAction
    {
        List<BossDate> bossDatesList = new List<BossDate>();
        private short isWeek = 0;
        private string guildID = string.Empty;

        public Action6108(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6108, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(bossDatesList.Count);
            foreach (BossDate bossDate in bossDatesList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)bossDate.EnableWeek);
                dsItem.PushIntoStack(IsSelect(guildID, bossDate));
                PushIntoStack(dsItem);
            }
            PushIntoStack(isWeek);
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
            bossDatesList = UserHelper.GetBossDate();
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null && guild.GuildBossInfo != null && guild.GuildBossInfo.RefreshDate != MathUtils.SqlMinDate)
            {
                if (UserHelper.IsCurrentWeek(guild.GuildBossInfo.RefreshDate))
                {
                    isWeek = 1;
                }
                else
                {
                    isWeek = 0;
                }
            }
            return true;
        }

        public static int IsSelect(string guildID, BossDate bossDate)
        {
            DateTime priod = new DateTime();
            DateTime endPriod = new DateTime();
            int isSelect = 0;
            GameActive gameActive = new ShareCacheStruct<GameActive>().FindKey(UserGuild.ActiveID);

            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null && guild.GuildBossInfo != null)
            {
                if (UserHelper.IsCurrentWeek(guild.GuildBossInfo.RefreshDate))
                {
                    priod = UserHelper.GuildBossDate(guild.GuildBossInfo);
                    endPriod = priod.AddMinutes(gameActive.Minutes);

                    if (guild.GuildBossInfo.EnableWeek == (int)bossDate.EnableWeek)
                    {
                        isSelect = 1;
                    }
                    else if (DateTime.Now > endPriod)
                    {
                        isSelect = 2;
                    }
                    else if ((priod - DateTime.Now).TotalSeconds > 3600)
                    {
                        isSelect = 0;
                    }
                }
                else
                {
                    priod = UserHelper.GuildEnableWeek(bossDate.EnableWeek); 
                    endPriod = priod.AddMinutes(gameActive.Minutes);
                    if ((priod - DateTime.Now).TotalSeconds > 3600)
                    {
                        isSelect = 0;
                    }
                    else if (DateTime.Now > endPriod)
                    {
                        isSelect = 2;
                    }
                }
            }
            else
            {
                priod = UserHelper.GuildEnableWeek(bossDate.EnableWeek);
                endPriod = priod.AddMinutes(gameActive.Minutes);
                if (DateTime.Now > endPriod)
                {
                    isSelect = 2;
                }
                else if ((priod - DateTime.Now).TotalSeconds > 3600)
                {
                    isSelect = 0;
                }
                else
                {
                    isSelect = 2;
                }
            }

            return isSelect;
        }
    }
}