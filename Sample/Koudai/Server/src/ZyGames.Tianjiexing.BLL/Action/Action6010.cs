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
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6010_公会退出接口
    /// </summary>
    public class Action6010 : BaseAction
    {
        private string guildID;


        public Action6010(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6010, httpGet)
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
            int totalDate = (ConfigEnvSet.GetInt("UserQueue.GuildMemberDel") * 60 * 60);
            var memberSet = new ShareCacheStruct<GuildMember>();
            List<GuildMember> memberArray = memberSet.FindAll(m => m.GuildID == guildID);
            GuildMember userMember = memberSet.FindKey(guildID, ContextUser.UserID);
            if (userMember != null && userMember.PostType == PostType.Chairman && memberArray.Count > 1)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6010_Chairman;
                return false;
            }
            if (userMember != null)
            {
                if (!string.IsNullOrEmpty(ContextUser.MercenariesID))
                {
                    memberSet.Delete(userMember);
                    ContextUser.MercenariesID = string.Empty;
                    //ContextUser.Update();
                    CombatHelper.RemoveGuildAbility(ContextUser);
                }
                var guildSet = new ShareCacheStruct<UserGuild>();
                UserGuild guildInfo = guildSet.FindKey(guildID);
                if (guildInfo != null && userMember.PostType == PostType.Chairman && memberArray.Count <= 1)
                {
                    guildSet.Delete(guildInfo);
                }

                List<UserQueue> queueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.TuiChuGongHui);
                if (queueArray.Count > 0)
                {
                    UserQueue queue = queueArray[0];
                    queue.Timing = DateTime.Now;
                    queue.TotalColdTime = totalDate;
                    queue.ColdTime = totalDate;
                    //queue.Update();
                }
                else
                {
                    UserQueue userQueue = new UserQueue()
                    {
                        QueueID =
                            Guid.NewGuid().ToString(),
                        QueueName = QueueType.TuiChuGongHui.ToString(),
                        QueueType = QueueType.TuiChuGongHui,
                        TotalColdTime = totalDate,
                        ColdTime = totalDate,
                        Timing = DateTime.Now,
                        IsSuspend = false,
                        UserID = ContextUser.UserID
                    };
                    new GameDataCacheSet<UserQueue>().Add(userQueue);
                }
            }
            return true;
        }
    }
}