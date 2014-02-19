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


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6009_公会公会设置接口
    /// </summary>
    public class Action6009 : BaseAction
    {
        private int contentType;
        private string content;


        public Action6009(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6009, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ContentType", ref contentType)
                 && httpGet.GetString("Content", ref content))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int length = System.Text.Encoding.Default.GetByteCount(content);
            if (string.IsNullOrEmpty(content))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6009_ContentNotEmpty;
                return false;
            }
            else if (length >= 199)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6009_ContentTooLong;
                return false;
            }
            var memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID == ContextUser.UserID && m.PostType != PostType.Member);
            if (memberArray.Count == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6007_AuditPermissions;
                return false;
            }
            UserGuild guildInfo = new ShareCacheStruct<UserGuild>().FindKey(memberArray[0].GuildID);

            if (contentType == 1)
            {
                guildInfo.GuildDesc = content;
            }
            else if (contentType == 2)
            {
                guildInfo.Announcement = content;
            }
            //guildInfo.Update();
            return true;
        }
    }
}