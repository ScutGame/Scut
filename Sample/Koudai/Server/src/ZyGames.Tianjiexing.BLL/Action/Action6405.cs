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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6405_设置旗帜接口
    /// </summary>
    public class Action6405 : BaseAction
    {
        private string name;


        public Action6405(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6405, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("Name", ref name))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (string.IsNullOrEmpty(name))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6405_GuildBannerNotEnough;
                return false;
            }
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6011_GuildMemberNotMember;
                return false;
            }
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member == null || member.PostType == PostType.Member)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6404_OrdinaryMemberNotCompetence;
                return false;
            }
            FightCombat fightCombat = new FightCombat();
            int fastID = MathUtils.Subtraction(fightCombat.FastID, 1);
            ServerFight fight = new ShareCacheStruct<ServerFight>().FindKey(fastID, ContextUser.MercenariesID);
            if (fight != null && fight.IsBanner)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6413_HaveBeenModified;
                return false;
            }

            if (fight == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6405_GuildNotEnterName;
                return false;
            }

            if (name.Length > 1)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6405_FillInACharacter;
                return false;
            }

            if (fight.FastID == fastID && fight.RankID == 1 && !fight.IsRemove)
            {
                if (fight.IsBanner)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6413_HaveBeenModified;
                    return false;
                }
                fight.IsBanner = true;
            }
            fight.GuildBanner = name.Substring(0, 1);
            ErrorCode = 0;
            ErrorInfo = LanguageManager.GetLang().St6405_SettingTheBannerSuccess;
            return true;
        }
    }
}