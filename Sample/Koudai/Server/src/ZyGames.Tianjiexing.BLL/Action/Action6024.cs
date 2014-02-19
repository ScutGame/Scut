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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 公会增加成员上限道具接口
    /// </summary>
    public class Action6024 : BaseAction
    {
        private string guildID = string.Empty;
        private const int maxMember = 10;
        private int addMember = 5;
        private const int itemID = 7004;


        public Action6024(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6024, httpGet)
        {

        }

        public override void BuildPacket()
        {

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
            if (string.IsNullOrEmpty(ContextUser.MercenariesID) || guildID != ContextUser.MercenariesID)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9203_ChaTypeNotGuildMember;
                return false;
            }
            UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (userGuild == null)
                return false;
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member != null)
            {
                if (member.PostType == PostType.Member)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6024_AuditPermissions;
                    return false;
                }
            }
            else
            {
                return false;
            }
            //UserItem userItem = UserItem.FindKey(itemID);
            var package = UserItemPackage.Get(Uid);
            UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.ItemID == itemID);
            if (userItem == null || userItem.Num <= 0)
            {
                ErrorCode = 1;
                return false;
            }
            if (userGuild.AddMember >= maxMember)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6024_GuildAddMemberToLong;
                return false;
            }
            ItemBaseInfo item = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (item != null)
            {
                addMember = item.EffectNum;
            }
            userGuild.AddMember += addMember;
            //userGuild.Update();
            UserItemHelper.UseUserItem(ContextUser.UserID, itemID, 1);
            return true;
        }
    }
}