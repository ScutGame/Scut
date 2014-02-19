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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Cache.Generic;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6002_公会成员列表接口
    /// </summary>
    public class Action6002 : BaseAction
    {
        private int pageIndex = 0;
        private int pageSize = 0;
        private string guildID = string.Empty;
        private int pageCount = 0;
        private List<GuildMember> memberArray = new List<GuildMember>();

        public Action6002(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6002, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(memberArray.Count);
            foreach (GuildMember member in memberArray)
            {
                GameUser gameUser = UserCacheGlobal.CheckLoadUser(member.UserID);
                int isOnline = 0;
                if (gameUser != null && gameUser.IsOnline)
                {
                    isOnline = 1;
                }
                else
                {
                    isOnline = 2;
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(member.UserID);
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.NickName.ToNotNullString());
                dsItem.PushIntoStack(gameUser == null ? LanguageManager.GetLang().shortInt : gameUser.UserLv);
                dsItem.PushIntoStack((short)member.PostType);
                dsItem.PushIntoStack(gameUser == null ? 0 : gameUser.RankID);
                dsItem.PushIntoStack(member.Contribution);
                dsItem.PushIntoStack(member.TotalContribution);
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.LoginTime.ToString("t"));
                dsItem.PushIntoStack(isOnline);

                PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                 && httpGet.GetInt("PageSize", ref pageSize))
            {
                httpGet.GetString("GuildID", ref guildID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var tempArray = new List<GuildMember>();
            if (!string.IsNullOrEmpty(guildID))
            {
                tempArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == guildID);
            }
            else
            {
                tempArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == ContextUser.MercenariesID);
            }
            //todo
            if (tempArray.Count == 0)
            {
                int membercount = new ShareCacheStruct<GuildMember>().Count;
                TraceLog.ReleaseWriteFatal("6002 公会成员数量{0}，自己公会{1}，查看其他玩家的公会{2}", membercount, ContextUser.MercenariesID, guildID);
            }
            //排序
            tempArray.QuickSort((a, b) => a.CompareTo(b));
            memberArray = tempArray.GetPaging(pageIndex, pageSize, out pageCount);
            return true;
        }
    }
}