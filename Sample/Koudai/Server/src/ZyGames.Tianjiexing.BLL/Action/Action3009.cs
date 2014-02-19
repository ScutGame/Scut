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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3009_活动列表接口
    /// </summary>
    public class Action3009 : BaseAction
    {
        private GameActive[] _gameActiveList = new GameActive[0];
        private UserGuild guild = null;

        public Action3009(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3009, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_gameActiveList.Length);
            foreach (GameActive gameActive in _gameActiveList)
            {
                DateTime enableTime = DateTime.MinValue;
                DateTime endTime = DateTime.MinValue;
                short isEnable = 0;
                RefEnableTime(gameActive, ref enableTime, ref endTime, ref isEnable);

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(gameActive.ActiveId);
                dsItem.PushIntoStack(gameActive.ActiveName.ToNotNullString());
                dsItem.PushIntoStack(gameActive.ActiveType.ToShort());
                dsItem.PushIntoStack(enableTime.ToString("HH:mm").ToNotNullString());
                dsItem.PushIntoStack(endTime.ToString("HH:mm").ToNotNullString());
                dsItem.PushIntoStack(isEnable);
                dsItem.PushIntoStack(gameActive.BossLv);
                dsItem.PushIntoStack(gameActive.HeadID.ToNotNullString());
                dsItem.PushIntoStack(gameActive.Descption.ToNotNullString());
                dsItem.PushIntoStack(gameActive.ActiveStyle);
                dsItem.PushIntoStack(!string.IsNullOrEmpty(ContextUser.MercenariesID)
                                         ? ContextUser.MercenariesID
                                         : string.Empty);
                PushIntoStack(dsItem);
            }

        }

        private void RefEnableTime(GameActive gameActive, ref DateTime enableTime, ref DateTime endTime, ref short isEnable)
        {
            try
            {
                gameActive = gameActive ?? new GameActive();
                if (gameActive.ActiveType == FunctionEnum.Gonghui && !string.IsNullOrEmpty(ContextUser.MercenariesID))
                {
                    if (gameActive.ActiveId == UserGuild.ActiveID)
                    {
                        enableTime = guildBossDate(ContextUser.MercenariesID);
                        endTime = enableTime.AddMinutes(gameActive.Minutes);
                        guild.GuildBossRefreshStatus();
                        //gameActive.RefreshStatus();
                        if (guild.CombatStatus == CombatStatus.Wait || guild.CombatStatus == CombatStatus.Combat)
                        {
                            isEnable = 1;
                        }
                        else if (guild.CombatStatus == CombatStatus.Over)
                        {
                            isEnable = 2;//已结束
                        }
                        else if (guild.CombatStatus == CombatStatus.Killed)
                        {
                            isEnable = 3;//已被击杀
                        }
                        if (guild.GuildBossInfo != null && !UserHelper.IsCurrentWeek(guild.GuildBossInfo.RefreshDate))
                        {
                            isEnable = 2;
                        }
                    }
                    else
                    {
                        enableTime = gameActive.BeginTime.ToDateTime(DateTime.MinValue);
                        DateTime intervalDate = gameActive.BeginTime.AddMinutes(gameActive.WaitMinutes);
                        endTime = enableTime.AddMinutes(gameActive.Minutes);
                        if (gameActive.ActiveId == 11 && intervalDate < DateTime.Now && DateTime.Now < endTime)
                        {
                            isEnable = 4; //置灰
                        }
                        else
                        {
                            if (DateTime.Now < enableTime)
                            {
                                isEnable = 0;
                            }
                            else if (DateTime.Now > endTime)
                            {
                                isEnable = 2;
                            }
                            else
                            {
                                isEnable = 1;
                            }
                        }
                    }

                }
                else
                {
                    endTime = gameActive.EndTime;
                    enableTime = gameActive.BeginTime;
                    gameActive.RefreshStatus();
                    if (gameActive.CombatStatus == CombatStatus.Wait || gameActive.CombatStatus == CombatStatus.Combat)
                    {
                        isEnable = 1;
                    }
                    else if (gameActive.CombatStatus == CombatStatus.Over)
                    {
                        isEnable = 2;//已结束
                    }
                    else if (gameActive.CombatStatus == CombatStatus.Killed)
                    {
                        isEnable = 3;//已被击杀
                    }
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            _gameActiveList = new GameActiveCenter(Uid).GetActiveList();
            guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            return true;
        }

        public static DateTime guildBossDate(string guildID)
        {
            DateTime enableTime = DateTime.MinValue;// new DateTime();
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null && guild.GuildBossInfo != null && !string.IsNullOrEmpty(guild.GuildBossInfo.EnablePeriod))
            {
                //enableTime = guild.GuildBossInfo.EnablePeriod.ToDateTime(DateTime.MinValue);
                enableTime = UserHelper.GuildBossDate(guild.GuildBossInfo);
            }
            return enableTime;
        }

        public static short GuildEnable(string guildID)
        {
            short isEnable = 0;
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null)
            {
                if (guild.CombatStatus == CombatStatus.Wait || guild.CombatStatus == CombatStatus.Combat)
                {
                    isEnable = 1;
                }
                else if (guild.CombatStatus == CombatStatus.Over)
                {
                    isEnable = 2; //已结束
                }
                else if (guild.CombatStatus == CombatStatus.Killed)
                {
                    isEnable = 3; //已被击杀
                }
            }
            return isEnable;
        }
    }
}