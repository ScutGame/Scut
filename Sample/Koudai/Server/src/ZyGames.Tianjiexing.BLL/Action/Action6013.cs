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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6013_公会封魔列表接口
    /// </summary>
    public class Action6013 : BaseAction
    {
        private List<GuildMember> memberArray = new List<GuildMember>();
        private int isSuccess = 0;
        private int currNum = 0;
        private int isPilgrimage = 0;
        private int chaoShengNum = 0;

        public Action6013(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6013, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(isSuccess);
            PushIntoStack(memberArray.Count);
            foreach (GuildMember member in memberArray)
            {
                UserCacheGlobal.CheckLoadUser(member.UserID);
                GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(member.UserID);
                UserGeneral general = UserGeneral.GetMainGeneral(member.UserID);
                int isHelp = 0;
                if (member.DevilNum > 1)
                {
                    isHelp = 1;
                }
                else
                {
                    isHelp = 2;
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(member.UserID);
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.NickName.ToNotNullString());
                dsItem.PushIntoStack(general == null ? string.Empty : general.HeadID.ToNotNullString());
                dsItem.PushIntoStack(isHelp);

                PushIntoStack(dsItem);
            }
            PushIntoStack(currNum);
            PushIntoStack(isPilgrimage);
            PushIntoStack(chaoShengNum);
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            UserHelper.ChecheDailyContribution(ContextUser.MercenariesID, ContextUser.UserID);
            chaoShengNum = ConfigEnvSet.GetInt("GuildMember.QiXingZhaoSheng");
            var guidCacheSet = new ShareCacheStruct<GuildMember>();
            GuildMember member = guidCacheSet.FindKey(ContextUser.MercenariesID, ContextUser.UserID);
            if (member != null)
            {
                List<GuildMember> gmemberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == member.GuildID && m.IsDevil == 1);
                foreach (GuildMember guildMember in gmemberArray)
                {
                    if (guildMember.DevilDate.Date != DateTime.Now.Date)
                    {
                        guildMember.IsDevil = 2;
                        guildMember.DevilNum = 0;
                    }
                }
                memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == member.GuildID && m.IsDevil == 1 && (DateTime.Now.Date == m.DevilDate.Date));
                foreach (GuildMember guildMember in memberArray)
                {
                    currNum = MathUtils.Addition(currNum, guildMember.CurrNum, int.MaxValue);
                    if (guildMember.UserID == ContextUser.UserID) { isPilgrimage = 1; }
                }
                int totalNum = MathUtils.Addition(memberArray.Count, currNum, int.MaxValue);
                if (totalNum >= 7)
                {
                    isSuccess = 1;
                    foreach (GuildMember guildMember in memberArray)
                    {
                        guildMember.IsDevil = 2;
                        if (guildMember.DevilNum <= 1)
                        {
                            GameUser gameUser = UserCacheGlobal.CheckLoadUser(guildMember.UserID);
                            if (gameUser != null)
                            {
                                gameUser.ObtainNum = MathUtils.Addition(gameUser.ObtainNum, chaoShengNum, int.MaxValue);
                                UserHelper.Contribution(guildMember.UserID, chaoShengNum);
                                GuildMemberLog.AddLog(member.GuildID, new MemberLog()
                                {
                                    UserID = guildMember.UserID,
                                    IdolID = 0,
                                    LogType = 1,
                                    GainObtion = chaoShengNum,
                                    Experience = chaoShengNum,
                                    GainAura = 0,
                                    InsertDate = DateTime.Now,
                                });

                            }
                            else
                            {
                                return false;
                            }
                        }
                        guildMember.CurrNum = 0;
                    }
                }
                else
                {
                    isSuccess = 2;
                }
            }
            return true;
        }
    }
}