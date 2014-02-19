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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6017_创建公会接口
    /// </summary>
    public class Action6017 : BaseAction
    {
        private string guildName = string.Empty;


        public Action6017(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6017, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("GuildName", ref guildName))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int nameLength = System.Text.Encoding.Default.GetByteCount(guildName);
            List<UserQueue> queueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.TuiChuGongHui);
            if (queueArray.Count > 0 && queueArray[0].DoRefresh() > 0 && queueArray[0].IsSuspend == false)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6006_GuildMemberNotDate;
                return false;
            }
            int createGoinNum = ConfigEnvSet.GetInt("UserGuild.CreateGoinNum");
            int createLv = ConfigEnvSet.GetInt("UserGuild.CreateUserLv");
            if (ContextUser.UserLv < createLv)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_LevelNotEnough;
                return false;
            }

            if (ContextUser.GameCoin < createGoinNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                return false;
            }
            if (guildName == "")
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6017_GuildNameNotEmpty;
                return false;
            }

            if (nameLength < 4 || nameLength > 12)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6017_GuildNameTooLong;
                return false;
            }

            List<GuildMember> memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID == ContextUser.UserID);
            if (memberArray.Count > 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6017_UnionMembers;
                return false;
            }
            List<UserGuild> guildArray = new ShareCacheStruct<UserGuild>().FindAll(u => u.GuildName == guildName);
            if (guildArray.Count > 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6017_GuildRename;
                return false;
            }
            List<UserGuild> userGuildArray = new ShareCacheStruct<UserGuild>().FindAll();
            int rankID = 0;
            if (userGuildArray.Count > 0)
            {
                rankID = MathUtils.Addition(userGuildArray.Count, 1, int.MaxValue);
            }
            else
            {
                rankID = 1;
            }

            string guildID = Guid.NewGuid().ToString();
            ContextUser.MercenariesID = guildID;

            ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, createGoinNum, 0);

            UserGuild userGuild = new UserGuild()
            {
                GuildID = guildID,
                UserID = ContextUser.UserID,
                GuildName = guildName,
                GuildLv = 1,
                GuildRank = rankID,
                WeekExperience = 0,
                CurrExperience = 0,
                CreateDate = DateTime.Now,
            };
            new ShareCacheStruct<UserGuild>().Add(userGuild);


            GuildMember member = new GuildMember()
            {
                GuildID = guildID,
                UserID = ContextUser.UserID,
                PostType = PostType.Chairman,
                Contribution = 0,
                TotalContribution = 0,
                DevilNum = 0,
                SummonNum = 0,
                IsDevil = 0,
                IncenseNum = 0,
                InsertDate = DateTime.Now
            };
            new ShareCacheStruct<GuildMember>().Add(member);
            var temp = new ShareCacheStruct<GuildMember>().FindKey(guildID, ContextUser.UserID);
            if (temp.HasChanged)
            {
                TraceLog.WriteInfo("6017 GuildMember update success.");
            }
            GuildIdol idolInfo = new GuildIdol()
            {
                GuildID = guildID,
                IdolLv = 1,
                CurrExperience = 0
            };
            new ShareCacheStruct<GuildIdol>().Add(idolInfo);
            return true;
        }
    }
}