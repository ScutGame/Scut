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
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6001_公会列表接口
    /// </summary>
    public class Action6001 : BaseAction
    {
        private int pageIndex = 0;
        private int pageSize = 0;
        private int pageCount = 0;
        private List<UserGuild> userGuildArray = new List<UserGuild>();
        private int isApply;
        private int maxPeople = 0;

        public Action6001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6001, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(userGuildArray.Count);
            foreach (UserGuild guild in userGuildArray)
            {
                GuildLvInfo guildLvInfo = new ConfigCacheSet<GuildLvInfo>().FindKey(guild.GuildLv);
                if (guildLvInfo != null)
                {
                    maxPeople = MathUtils.Addition(guildLvInfo.MaxPeople, guild.AddMember);
                }
                GameUser gameUser = null;
                List<GuildMember> memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == guild.GuildID);
                if (memberArray.Count > 0)
                {
                    foreach (GuildMember guildMember in memberArray)
                    {
                        if (guildMember.PostType == PostType.Chairman)
                        {
                            gameUser = new GameDataCacheSet<GameUser>().FindKey(guildMember.UserID);
                        }
                    }
                }

                UserApply applyInfo = new ShareCacheStruct<UserApply>().FindKey(guild.GuildID, ContextUser.UserID);
                if (applyInfo == null && string.IsNullOrEmpty(ContextUser.MercenariesID))
                {
                    isApply = 1;
                }
                else
                {
                    isApply = 2;
                }

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(guild.GuildRank);
                dsItem.PushIntoStack(guild.GuildID.ToNotNullString());
                dsItem.PushIntoStack(guild.GuildName.ToNotNullString());
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.NickName.ToNotNullString());
                dsItem.PushIntoStack((short)guild.GuildLv);
                dsItem.PushIntoStack((short)memberArray.Count);
                dsItem.PushIntoStack((short)maxPeople);
                dsItem.PushIntoStack(isApply);
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                && httpGet.GetInt("PageSize", ref pageSize))
            {
                return true;
            }

            return false;
        }

        public override bool TakeAction()
        {
            List<UserGuild> guildsArray = new ShareCacheStruct<UserGuild>().FindAll();
            guildsArray.QuickSort((x, y) =>
            {
                int result = 0;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                result = y.GuildLv.CompareTo(x.GuildLv);
                if (result == 0)
                {
                    result = y.CurrExperience.CompareTo(x.CurrExperience);
                }
                return result;
            });
            int ranking = 0;
            foreach (UserGuild guild in guildsArray)
            {
                ranking = MathUtils.Addition(ranking, 1, int.MaxValue);
                guild.GuildRank = ranking;
                //guild.Update();
            }
            if (!string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                guildsArray = new ShareCacheStruct<UserGuild>().FindAll(u => u.GuildID != ContextUser.MercenariesID);
            }
            userGuildArray = guildsArray.GetPaging(pageIndex, pageSize, out pageCount);

            return true;
        }
    }
}