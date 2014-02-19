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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6003_公会日志列表接口
    /// </summary>
    public class Action6003 : BaseAction
    {
        private int pageIndex = 0;
        private int pageSize = 0;
        private int pageCount = 0;
        private List<MemberLog> guildLogArray = new List<MemberLog>();

        public Action6003(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6003, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(guildLogArray.Count);
            foreach (MemberLog guildLog in guildLogArray)
            {
                UserCacheGlobal.CheckLoadUser(guildLog.UserID);
                GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(guildLog.UserID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(guildLog.UserID);
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.NickName.ToNotNullString());
                dsItem.PushIntoStack(guildLog.GainObtion);
                dsItem.PushIntoStack(guildLog.Experience);
                dsItem.PushIntoStack(GetGainDate(guildLog.InsertDate));
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
            List<GuildMember> guildArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID == ContextUser.UserID);
            if (guildArray.Count > 0)
            {
                var memberLog = new ShareCacheStruct<GuildMemberLog>().FindKey(guildArray[0].GuildID) ?? new GuildMemberLog();
                var userGuildLogArray = memberLog.GetLog(null);
                userGuildLogArray.QuickSort((x, y) =>
                {
                    if (x == null && y == null) return 0;
                    if (x != null && y == null) return 1;
                    if (x == null) return -1;
                    return y.InsertDate.CompareTo(x.InsertDate);
                });

                guildLogArray = userGuildLogArray.GetPaging(pageIndex, pageSize, out pageCount);

            }
            return true;
        }

        public static string GetGainDate(DateTime gainDate)
        {
            string insertDate = string.Empty;
            if (gainDate.AddDays(1).Date == DateTime.Now.Date)
            {
                insertDate = LanguageManager.GetLang().Date_Yesterday + gainDate.ToString("t");
            }
            else if (gainDate.AddDays(2).Date == DateTime.Now.Date)
            {
                insertDate = LanguageManager.GetLang().Date_BeforeYesterday + gainDate.ToString("t");
            }
            else if (gainDate.AddDays(3).Date <= DateTime.Now.Date)
            {
                int totaldays = (int)(DateTime.Now - gainDate).TotalDays;
                insertDate = string.Format(LanguageManager.GetLang().Date_Day, MathUtils.Addition(totaldays, 1)) + gainDate.ToString("t");
            }
            else
            {
                insertDate = gainDate.ToString("t");
            }

            return insertDate;
        }
    }
}