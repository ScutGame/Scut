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
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6006_公会申请接口
    /// </summary>
    public class Action6006 : BaseAction
    {
        private string guildID = string.Empty;


        public Action6006(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6006, httpGet)
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
            List<UserQueue> queueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.TuiChuGongHui);
            if (queueArray.Count > 0 && queueArray[0].DoRefresh() > 0 && queueArray[0].IsSuspend == false)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St6006_GuildMemberNotDate;
                return false;
            }

            List<UserApply> applyArray = new ShareCacheStruct<UserApply>().FindAll(m => m.UserID == ContextUser.UserID);
            if (applyArray.Count > 3)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St6006_ApplyMaxGuild;
                return false;
            }

            UserApply apply = new ShareCacheStruct<UserApply>().FindKey(guildID, ContextUser.UserID);
            if (apply != null)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St6006_ApplyMember;
                return false;
            }

            List<GuildMember> guildMemberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID == ContextUser.UserID);
            if (guildMemberArray.Count > 0)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St6006_AlreadyMember;
                return false;
            }

            UserApply userApply = new UserApply()
            {
                GuildID = guildID,
                UserID = ContextUser.UserID,
                ApplyDate = DateTime.Now
            };
            new ShareCacheStruct<UserApply>().Add(userApply);
            return true;
        }
    }
}