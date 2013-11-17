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
using System.Threading;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    public class GuildGameActiveCenter
    {
        private static Timer _timer;
        private static int sleepTime = 20;//秒
        private static List<GameActive> _gameActiveList;
        private string _userId;

        static GuildGameActiveCenter()
        {
            _timer = new Timer(GuildInitActive, null, 60000, sleepTime * 1000);
            _gameActiveList = new ShareCacheStruct<GameActive>().FindAll(m => m.State);
        }

        public static void Stop()
        {
            _timer.Dispose();
        }

        /// <summary>
        /// 刷新活动
        /// </summary>
        public static void GuildInitActive(object sender)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(BossRefreshActive);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("刷新活动线程出现问题{0}", ex);
            }
        }

        public GuildGameActiveCenter(string userId)
        {
            _userId = userId;
        }

        /// <summary>
        /// 刷新公会boss活动
        /// </summary>
        public static void BossRefreshActive(object sender)
        {
            try
            {
                var guildsArray = new ShareCacheStruct<UserGuild>().FindAll();
                GameActive gameActive = new ShareCacheStruct<GameActive>().FindKey(UserGuild.ActiveID);
                DateTime currTime = DateTime.Now;
                if (gameActive != null)
                {
                    foreach (UserGuild guild in guildsArray)
                    {
                        if (guild.GuildBossInfo != null && !string.IsNullOrEmpty(guild.GuildBossInfo.EnablePeriod))
                        {
                            guild.BeginTime = GuildBossDate(guild.GuildBossInfo);
                            //guild.GuildBossInfo.EnablePeriod.ToDateTime(DateTime.MinValue);
                            guild.EndTime = guild.BeginTime.AddMinutes(gameActive.Minutes);
                            if (guild.BossPrize == null && gameActive.ActiveType == FunctionEnum.Gonghui)
                                guild.BossPrize = JsonUtils.Deserialize<BossActivePrize>(gameActive.ActivePize);
                        else if (currTime > guild.EndTime)
                        {
                            guild.CombatStatus = CombatStatus.Over;
                        }

                            if (guild.CombatStatus != CombatStatus.Killed &&
                                gameActive.WaitMinutes > 0 && currTime >= guild.BeginTime
                                && currTime <= guild.EndTime)
                            {
                                guild.CombatStatus = CombatStatus.Wait;
                                LoadData(guild);
                            }
                            else if (guild.CombatStatus != CombatStatus.Killed &&
                                     currTime >= guild.BeginTime && currTime <= guild.EndTime)
                            {
                                guild.CombatStatus = CombatStatus.Combat;
                                LoadData(guild);
                            }
                            else if (currTime < guild.BeginTime)
                            {
                                guild.CombatStatus = CombatStatus.NoStart;
                            }
                            else if (currTime > guild.EndTime)
                            {
                                guild.CombatStatus = CombatStatus.Over;
                            }

                            //CombatStatus combatStatus = guild.GuildBossRefreshStatus();
                            guild.CombatStatus = guild.GuildBossRefreshStatus();
                            if (guild.CombatStatus == CombatStatus.Wait)
                            {
                                //LoadData(guild);
                                //有等待时间
                                int minute = gameActive.WaitMinutes;
                                if (guild.CombatStatus != CombatStatus.Killed &&
                                    currTime >= guild.BeginTime.AddMinutes(minute) && currTime <= guild.EndTime)
                                {
                                    guild.CombatStatus = CombatStatus.Combat;
                                }
                                // break;
                            }
                            else if (guild.CombatStatus == CombatStatus.Combat)
                            {
                                // break;
                            }
                            else if (guild.CombatStatus == CombatStatus.Over)
                            {
                                DisposeData(guild);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        public static void LoadData(UserGuild userGuild)
        {
            if ((userGuild.CombatStatus == CombatStatus.Wait || userGuild.CombatStatus == CombatStatus.Combat) && !userGuild.LoadSuccess)
            {
                GameActive gameActive = new ShareCacheStruct<GameActive>().FindKey(UserGuild.ActiveID);
                userGuild.LoadSuccess = true;
                if (gameActive != null && gameActive.ActiveType == FunctionEnum.Gonghui)
                {
                    GuildBossCombat.InitBoss(userGuild);
                }
            }
        }

        public static void DisposeData(UserGuild userGuild)
        {
            if (userGuild.LoadSuccess)
            {
                GameActive gameActive = new ShareCacheStruct<GameActive>().FindKey(UserGuild.ActiveID);
                userGuild.LoadSuccess = false;
                if (gameActive != null && gameActive.ActiveType == FunctionEnum.Goujili)
                {
                    GuildBossCombat.Dispose(userGuild);
                }
            }
        }

        /// <summary>
        /// 根据周几取出时间
        /// </summary>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(BossDateType dateType)
        {
            DateTime currDt = DateTime.Now.Date;
            int currWeek = currDt.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)currDt.DayOfWeek;
            DateTime bossDate = currDt.AddDays((int)DayOfWeek.Monday - currWeek);
            //if (dateType == BossDateType.Monday)
            //{
            //    bossDate = bossDate;
            //}
            //else
            if (dateType == BossDateType.Tuesday)
            {
                bossDate = bossDate.AddDays(1);
            }
            else if (dateType == BossDateType.Wednesday)
            {
                bossDate = bossDate.AddDays(2);
            }
            else if (dateType == BossDateType.Thursday)
            {
                bossDate = bossDate.AddDays(3);
            }
            else if (dateType == BossDateType.Friday)
            {
                bossDate = bossDate.AddDays(4);
            }
            else if (dateType == BossDateType.Saturday)
            {
                bossDate = bossDate.AddDays(5);
            }
            else if (dateType == BossDateType.SundayAfternoon || dateType == BossDateType.Sunday)
            {
                bossDate = bossDate.AddDays(6);
            }
            return bossDate;
        }

        /// <summary>
        /// 公会boss挑战时间
        /// </summary>
        /// <param name="bossInfo"></param>
        /// <returns></returns>
        public static DateTime GuildBossDate(GuildBossInfo bossInfo)
        {
            BossDateType bossDateType = (BossDateType)Enum.Parse(typeof(BossDateType), bossInfo.EnableWeek.ToString());
            string weekDateTime = GetDateTime(bossDateType).ToString("d");
            string hourDate = DateTime.Parse(bossInfo.EnablePeriod).ToString("T");
            string currDate = weekDateTime + " " + hourDate;
            DateTime priod = DateTime.Parse(currDate);
            return priod;
        }

    }
}