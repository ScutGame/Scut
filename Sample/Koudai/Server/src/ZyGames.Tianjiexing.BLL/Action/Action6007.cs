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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6007_公会申请审核列表接口
    /// </summary>
    public class Action6007 : BaseAction
    {
        private int pageIndex = 0;
        private int pageSize = 0;
        private int pageCount = 0;
        private List<UserApply> applyArray = new List<UserApply>();

        public Action6007(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6007, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(applyArray.Count);
            foreach (UserApply apply in applyArray)
            {
                GameUser gameUser = UserCacheGlobal.CheckLoadUser(apply.UserID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(apply.UserID);
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.NickName.ToNotNullString());
                dsItem.PushIntoStack(gameUser == null ? LanguageManager.GetLang().shortInt : gameUser.UserLv);
                dsItem.PushIntoStack(gameUser == null ? 0 : gameUser.RankID);
                dsItem.PushIntoStack(apply.ApplyDate.ToString("t"));

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
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member != null)
            {
                if (member.PostType == PostType.Member)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St6007_AuditPermissions;
                    return false;
                }
                else
                {
                    var cacheSet = new ShareCacheStruct<UserApply>();
                    List<UserApply> userApplyArray = cacheSet.FindAll(m => m.GuildID == member.GuildID);
                    foreach (UserApply apply in userApplyArray)
                    {
                        List<GuildMember> gMemberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID == apply.UserID);
                        if (gMemberArray.Count > 0)
                        {
                            cacheSet.Delete(apply);
                        }
                    }
                    applyArray = userApplyArray.GetPaging(pageIndex, pageSize, out pageCount);
                }
            }
            return true;
        }
    }
}