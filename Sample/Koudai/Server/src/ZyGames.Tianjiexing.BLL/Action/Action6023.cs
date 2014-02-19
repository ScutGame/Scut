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


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 公会改名接口
    /// </summary>
    public class Action6023 : BaseAction
    {
        private string guildID = string.Empty;
        private string guildName = string.Empty;
        private const int itemID = 7001;


        public Action6023(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6023, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("GuildID", ref guildID)
                && httpGet.GetString("GuildName", ref guildName))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (string.IsNullOrEmpty(ContextUser.MercenariesID) || guildID != ContextUser.MercenariesID)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6011_GuildMemberNotMember;
                return false;
            }
            UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (userGuild == null)
                return false;
            List<UserGuild> guildArray = new ShareCacheStruct<UserGuild>().FindAll(m => m.GuildName== guildName);
            if (guildArray.Count > 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6017_Rename;
                return false;
            }

            int nameLength = System.Text.Encoding.Default.GetByteCount(guildName);
            if (nameLength < 4 || nameLength > 12)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6017_GuildNameTooLong;
                return false;
            }

            //UserItem userItem = UserItem.FindKey(itemID);
            var package = UserItemPackage.Get(Uid);
            UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.ItemID == itemID);
            if (userItem == null || userItem.Num <= 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member != null)
            {
                if (member.PostType == PostType.Member)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6007_AuditPermissions;
                    return false;
                }
            }
            else
            {
                return false;
            }
            userGuild.GuildName = guildName;
            //userGuild.Update();
            UserItemHelper.UseUserItem(ContextUser.UserID, itemID, 1);
            return true;
        }
    }
}