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
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6022_封魔召集接口
    /// </summary>
    public class Action6022 : BaseAction
    {
        private string guildID = string.Empty;


        public Action6022(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6022, httpGet)
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
            int maxNum = 7;
            int currNum = 0;
            List<GuildMember> memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == guildID && m.IsDevil == 1 && (DateTime.Now.Date == m.DevilDate.Date));
            foreach (GuildMember guildMember in memberArray)
            {
                currNum = MathUtils.Addition(currNum, guildMember.CurrNum, int.MaxValue);
            }
            currNum = MathUtils.Subtraction(maxNum, MathUtils.Addition(memberArray.Count, currNum, int.MaxValue), 0);
            string content = string.Format(LanguageManager.GetLang().St6022_GuildConvene, ContextUser.NickName, currNum);
            new TjxChatService(ContextUser).Send(ChatType.Guild, content);
            return true;
        }
    }
}