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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6008_公会转让接口
    /// </summary>
    public class Action6008 : BaseAction
    {
        private string guildID;
        private string memberID;
        private int postType;
        private int ops;


        public Action6008(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6008, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("GuildID", ref guildID)
                 && httpGet.GetString("MemberID", ref memberID)
                 && httpGet.GetInt("PostType", ref postType)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            PostType pType = (PostType)Enum.Parse(typeof(PostType), postType.ToString());
            GuildMember gMember = new ShareCacheStruct<GuildMember>().FindKey(guildID, memberID);
            GuildMember userMember = new ShareCacheStruct<GuildMember>().FindKey(guildID, ContextUser.UserID);
            if (userMember.PostType != PostType.Chairman)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6008_NotChairman;
                return false;
            }
            if (ops == 1)
            {
                var memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == guildID && m.PostType == PostType.VicePresident);
                if (memberArray.Count >= 2)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6008_VicePresidentNum;
                    return false;
                }
                if (gMember != null)
                {
                    gMember.PostType = pType;
                    //gMember.Update();
                }
            }
            else if (ops == 2)
            {
                if (gMember.PostType != PostType.VicePresident)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6008_NotVicePresident;
                    return false;
                }
                if (gMember.PostType != PostType.Chairman)
                {
                    gMember.PostType = PostType.Chairman;
                    //gMember.Update();
                }
                if (userMember.PostType != PostType.Member)
                {
                    userMember.PostType = PostType.Member;
                    //userMember.Update();
                }
            }
            else if (ops == 3)
            {
                if (gMember.PostType != PostType.VicePresident)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6008_NotVicePresidentCeXiao;
                    return false;
                }

                if (gMember.PostType != PostType.Member)
                {
                    gMember.PostType = PostType.Member;
                    //gMember.Update();
                }
            }
            return true;
        }
    }
}