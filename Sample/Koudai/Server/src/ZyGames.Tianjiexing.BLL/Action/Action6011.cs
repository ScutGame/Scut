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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6011_公会祭神列表接口
    /// </summary>
    public class Action6011 : BaseAction
    {
        private string guildID = string.Empty;
        private GuildIdol idolInfo = null;
        private List<GuildIdolInfo> idolInfoArray = new List<GuildIdolInfo>();
        private List<MemberLog> guildLogArray = new List<MemberLog>();
        private int maxAura = 0;
        private int isShow = 0;
        private int isXs = 0;

        public Action6011(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6011, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(idolInfo == null ? LanguageManager.GetLang().shortInt : idolInfo.IdolLv);
            PushIntoStack(idolInfo == null ? 0 : idolInfo.CurrExperience);
            PushIntoStack(maxAura);
            PushIntoStack(guildLogArray.Count);
            foreach (MemberLog log in guildLogArray)
            {
                GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(log.UserID);
                if (gameUser == null)
                {
                    gameUser = UserCacheGlobal.CheckLoadUser(log.UserID);
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(log.UserID);
                dsItem.PushIntoStack(gameUser == null ? string.Empty : gameUser.NickName.ToNotNullString());
                dsItem.PushIntoStack(log.LogType);
                dsItem.PushIntoStack(log.GainAura);
                dsItem.PushIntoStack(GetGainDate(log.InsertDate));
                dsItem.PushIntoStack(log.IdolID);
                PushIntoStack(dsItem);
            }
            PushIntoStack(idolInfoArray.Count);
            foreach (GuildIdolInfo guildIdol in idolInfoArray)
            {
                if (isXs == 2)
                {
                    isShow = 2;
                }
                else
                {
                    if (guildIdol.IdolID == 1)
                    {
                        isShow = 1;
                    }
                    if (guildIdol.IdolID == 2 && VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.GuildMemberShangSuHeXiang))
                    {
                        isShow = 1;
                    }
                    else if (guildIdol.IdolID == 2)
                    {
                        isShow = 2;
                    }
                    if (guildIdol.IdolID == 3 && VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.GuildMemberShangTianMuXiang))
                    {
                        isShow = 1;
                    }
                    else if (guildIdol.IdolID == 3)
                    {
                        isShow = 2;
                    }
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)guildIdol.IdolID);
                dsItem.PushIntoStack(guildIdol.GainObtion);
                dsItem.PushIntoStack(guildIdol.GainBlessing);
                dsItem.PushIntoStack(guildIdol.GainAura);
                dsItem.PushIntoStack(guildIdol.UseExpNum);
                dsItem.PushIntoStack(guildIdol.UseGold);
                dsItem.PushIntoStack(isShow);

                PushIntoStack(dsItem);
            }

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
            if (string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6011_GuildMemberNotMember;
                return false;
            }
            idolInfo = new ShareCacheStruct<GuildIdol>().FindKey(guildID);
            if (idolInfo != null)
            {
                maxAura = new ConfigCacheSet<IdolLvInfo>().FindKey(idolInfo.IdolLv).UpExperience;
            }
            idolInfoArray = new ConfigCacheSet<GuildIdolInfo>().FindAll();

            var memberLog = new ShareCacheStruct<GuildMemberLog>().FindKey(guildID) ?? new GuildMemberLog();
            List<MemberLog> guildArray = memberLog.GetLog(u => u.LogType == 2);

            foreach (MemberLog guildLog in guildArray)
            {
                if (guildLog.UserID == ContextUser.UserID && DateTime.Now.Date == guildLog.InsertDate.Date)
                {
                    isXs = 2;
                    break;
                }
            }
            guildArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return y.InsertDate.CompareTo(x.InsertDate);
            });
            int recordCount = 0;

            guildLogArray = guildArray.GetPaging(0, guildArray.Count > 5 ? 5 : guildArray.Count, out recordCount);

            return true;
        }

        public static string GetGainDate(DateTime gainDate)
        {
            string insertDate = string.Empty;
            if (gainDate.AddDays(1).Date == DateTime.Now.Date)
            {
                insertDate = LanguageManager.GetLang().Date_Yesterday + gainDate.ToString("t");
            }
            else if (gainDate.AddDays(2).Date == DateTime.Now.Date)
            {
                insertDate = LanguageManager.GetLang().Date_BeforeYesterday + gainDate.ToString("t");
            }
            else if (gainDate.AddDays(3).Date <= DateTime.Now.Date)
            {
                int totaldays = (int)(DateTime.Now - gainDate).TotalDays;
                insertDate = string.Format(LanguageManager.GetLang().Date_Day, MathUtils.Addition(totaldays, 1, int.MaxValue)) + gainDate.ToString("t");
            }
            else
            {
                insertDate = gainDate.ToString("t");
            }
            return insertDate;
        }
    }
}