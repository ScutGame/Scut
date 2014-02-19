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
using System.Diagnostics;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6101_公会Boss战参加接口
    /// </summary>
    public class Action6101 : BaseAction
    {
        private int _pageIndex;
        private int _pageSize;
        private UserGuild _guild;
        private List<BossUser> _bossUserList = new List<BossUser>();
        private int _regNum;
        private double _inspirePercent;
        private int _reLiveNum = 0;
        private double _reliveInspirePercent;
        private int _activeId;
        private string guildID = string.Empty;
        private int bossPlotID = 0;

        public Action6101(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6101, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(bossPlotID);
            PushIntoStack(_guild == null ? (short)0 : _guild.GuildBossInfo.BossLv);
            PushIntoStack(_guild == null ? 0 : _guild.ColdTime);
            PushIntoStack(_regNum);
            PushIntoStack(_guild == null ? (short)0 : _guild.CombatStatus.ToShort());
            PushIntoStack((_inspirePercent * 100).ToInt());
            PushIntoStack(_reLiveNum);


            PushIntoStack(_bossUserList.Count);
            foreach (BossUser bossUser in _bossUserList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(bossUser.NickName.ToNotNullString());
                UserGeneral userGeneral = UserGeneral.GetMainGeneral(bossUser.UserId);
                dsItem.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.HeadID.ToNotNullString());
                dsItem.PushIntoStack(bossUser.IsRelive ? 0 : 1);
                dsItem.PushIntoStack(bossUser.CodeTime);
                dsItem.PushIntoStack(NoviceHelper.IsWingFestivalInfo(bossUser.UserId) ? (short)1 : (short)0);
                dsItem.PushIntoStack(0);
                PushIntoStack(dsItem);
            }
            PushIntoStack((_reliveInspirePercent * 100).ToInt());
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("GuildID", ref guildID)
                 && httpGet.GetInt("PageIndex", ref _pageIndex)
                && httpGet.GetInt("PageSize", ref _pageSize)
                && httpGet.GetInt("ActiveId", ref _activeId))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            if (!string.IsNullOrEmpty(ContextUser.MercenariesID) && ContextUser.MercenariesID == guildID)
            {
                if (CombatHelper.GuildBossKill(ContextUser.MercenariesID))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St5405_BossKilled;
                    return false;
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                GameActive gameActive = new ShareCacheStruct<GameActive>().FindKey(_activeId);
                if (gameActive != null)
                {
                    bossPlotID = gameActive.BossPlotID;
                    GuildBossCombat bossCombat = new GuildBossCombat(guildID);
                    if (bossCombat.HasCombat)
                    {
                        _guild = bossCombat.UserGuild;
                        if (_guild != null && _guild.GuildBossInfo != null)
                        {
                            if (_guild.GuildBossInfo.EnableWeek > 0 && _guild.GuildBossInfo.BossLv > 0)
                            {
                                _guild.BeginTime = UserHelper.GuildBossDate(_guild.GuildBossInfo);
                                _guild.EndTime = _guild.BeginTime.AddMinutes(gameActive.Minutes);
                                CombatStatus combatStatus = _guild.GuildBossRefreshStatus();
                                if (UserHelper.IsCurrentWeek(_guild.BeginTime) &&
                                    UserHelper.IsCurrentWeek(_guild.GuildBossInfo.RefreshDate))
                                {
                                    if (DateTime.Now < _guild.BeginTime)
                                    {
                                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                                        ErrorInfo = LanguageManager.GetLang().St6101_GuildBossNotOpen;
                                        return false;
                                    }
                                    else if ((combatStatus == CombatStatus.Killed && DateTime.Now < _guild.EndTime) ||
                                             DateTime.Now > _guild.EndTime)
                                    {
                                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                                        ErrorInfo = LanguageManager.GetLang().St6101_GuildBossOver;
                                        return true;
                                    }
                                }
                                else
                                {
                                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                                    ErrorInfo = LanguageManager.GetLang().St6101_GuildBossSet;
                                    return false;
                                }

                                if (combatStatus == CombatStatus.Wait || combatStatus == CombatStatus.Combat)
                                {
                                    bossCombat.Append(ContextUser);
                                    BossUser bossUser = bossCombat.GetCombatUser(ContextUser.UserID);
                                    if (bossUser != null)
                                    {
                                        _inspirePercent = bossUser.InspirePercent;
                                        _reliveInspirePercent = bossUser.ReliveInspirePercent;
                                        _reLiveNum = bossUser.ReliveNum;
                                    }
                                }
                                List<BossUser> userList = bossCombat.GetCombatUser();
                                _regNum = userList.Count;
                                int recordCount = 0;
                                _bossUserList = userList.GetPaging(_pageIndex, _pageSize, out recordCount);
                            }
                        }
                    }
                    else
                    {
                        UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
                        if (guild != null && guild.GuildBossInfo != null)
                        {
                            if (guild.GuildBossInfo.EnableWeek > 0 && guild.GuildBossInfo.BossLv > 0)
                            {
                                guild.BeginTime = UserHelper.GuildBossDate(guild.GuildBossInfo);
                                guild.EndTime = guild.BeginTime.AddMinutes(gameActive.Minutes);
                                if (UserHelper.IsCurrentWeek(guild.BeginTime) &&
                                    UserHelper.IsCurrentWeek(guild.GuildBossInfo.RefreshDate))
                                {
                                    if (DateTime.Now < guild.BeginTime.AddMinutes(1))
                                    {
                                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                                        ErrorInfo = LanguageManager.GetLang().St6101_GuildBossNotOpen;
                                        return false;
                                    }
                                    else
                                    {
                                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                                        ErrorInfo = LanguageManager.GetLang().St6101_GuildBossOver;
                                        return true;
                                    }
                                }
                                else
                                {
                                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                                    ErrorInfo = LanguageManager.GetLang().St6101_GuildBossSet;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St6101_GuildBossSet;
                            return false;
                        }
                    }
                }
                stopwatch.Stop();
                new BaseLog().SaveLog("公会boss6101所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");
            }
            return true;
        }
    }
}