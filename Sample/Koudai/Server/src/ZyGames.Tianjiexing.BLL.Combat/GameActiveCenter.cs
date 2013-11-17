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
using ZyGames.Framework.Game.Model;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model;
using System.Diagnostics;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 游戏活动中心
    /// </summary>
    public class GameActiveCenter
    {
        private static Timer _timer;
        private static int sleepTime = 15;//秒
        private static List<GameActive> _gameActiveList;
        private string _userId;

        static GameActiveCenter()
        {
            _timer = new Timer(InitActive, null, 6000, sleepTime * 1000);
            // _gameActiveList = new ShareCacheStruct<GameActive>().FindAll(m => m.State && m.ActiveStyle != 4);
            _gameActiveList = new ShareCacheStruct<GameActive>().FindAll(m => m.State);
            _gameActiveList.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.ActiveId.CompareTo(y.ActiveId);
            });
        }


        public static void Stop()
        {
            _timer.Dispose();
        }

        /// <summary>
        /// 刷新活动
        /// </summary>
        public static void InitActive(object sender)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(RefreshActive);
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog("刷新活动线程出现问题", ex);
            }
        }

        public GameActiveCenter(string userId)
        {
            _userId = userId;
        }

        /// <summary>
        /// 是否有活动
        /// </summary>
        /// <returns></returns>
        public bool HasActive()
        {
            GameActive[] activeList = GetActiveList();
            foreach (var gameActive in activeList)
            {
                var status = gameActive.RefreshStatus();
                if (status == CombatStatus.Wait || status == CombatStatus.Combat)
                {
                    return true;
                }

            }
            return false;
        }
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <returns></returns>
        public GameActive[] GetActiveList()
        {
            List<GameActive> activeList = new List<GameActive>();
            foreach (var gameActive in _gameActiveList)
            {
                UserFunction function = new GameDataCacheSet<UserFunction>().FindKey(_userId, gameActive.ActiveType);
                if (function == null) continue;
                if (function.FunEnum != FunctionEnum.Gonghui)
                {
                    LoadData(gameActive);
                    activeList.Add(gameActive);
                }
                else if (function.FunEnum == FunctionEnum.Gonghui)
                {
                    GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(_userId);
                    if (userInfo != null)
                    {
                        UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(userInfo.MercenariesID);
                        if (guild != null)
                        {
                            GuildLvInfo lvInfo = new ConfigCacheSet<GuildLvInfo>().FindKey(guild.GuildLv);
                            if (lvInfo != null)
                            {
                                var activityArray = lvInfo.ActivityDesc;
                                foreach (ActivityShow activityShow in activityArray)
                                {
                                    if (activityShow.ActivityID == (int)gameActive.ActiveStyle)
                                    {
                                        activeList.Add(gameActive);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return activeList.ToArray();
        }

        public GameActive[] GetActiveList(FunctionEnum activeType)
        {
            List<GameActive> activeList = new List<GameActive>();
            foreach (var gameActive in _gameActiveList)
            {
                if (gameActive.ActiveType != FunctionEnum.Gonghui && (gameActive.ActiveType != activeType ||
                    new GameDataCacheSet<UserFunction>().FindKey(_userId, gameActive.ActiveType) == null)) continue;
                activeList.Add(gameActive);
                if (activeType == FunctionEnum.Gonghui)
                {
                    if (gameActive.ActiveType == FunctionEnum.Gonghui)
                    {
                        GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(_userId);
                        if (userInfo != null)
                        {
                            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(userInfo.MercenariesID);
                            if (guild != null)
                            {
                                GuildLvInfo lvInfo = new ConfigCacheSet<GuildLvInfo>().FindKey(guild.GuildLv);
                                if (lvInfo != null)
                                {
                                    var activityArray = lvInfo.ActivityDesc;
                                    foreach (ActivityShow activityShow in activityArray)
                                    {
                                        if (activityShow.ActivityID == (int)gameActive.ActiveStyle)
                                        {
                                            activeList.Add(gameActive);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return activeList.ToArray();
        }

        /// <summary>
        /// 刷新活动
        /// </summary>
        private static void RefreshActive(object sender)
        {
            try
            {
                Trace.WriteLine("刷新活动被执行");
                foreach (var gameActive in _gameActiveList)
                {
                    string[] list = gameActive.EnablePeriod.Split(new char[] { ',' });
                    //有多个时间段
                    foreach (string item in list)
                    {
                        gameActive.BeginTime = item.ToDateTime(DateTime.MinValue);
                        gameActive.EndTime = gameActive.BeginTime.AddMinutes(gameActive.Minutes);
                        if (gameActive.BossPrize == null && gameActive.ActiveType == FunctionEnum.Booszhang)
                            gameActive.BossPrize = JsonUtils.Deserialize<BossActivePrize>(gameActive.ActivePize);
                        DateTime currTime = DateTime.Now;

                        if (gameActive.CombatStatus != CombatStatus.Killed &&
                            gameActive.WaitMinutes > 0 && currTime >= gameActive.BeginTime
                            && currTime <= gameActive.BeginTime.AddMinutes(gameActive.WaitMinutes))
                        {
                            LoadData(gameActive);
                            gameActive.CombatStatus = CombatStatus.Wait;
                        }
                        else if (gameActive.CombatStatus != CombatStatus.Killed &&
                            currTime >= gameActive.BeginTime && currTime <= gameActive.EndTime)
                        {
                            LoadData(gameActive);
                            gameActive.CombatStatus = CombatStatus.Combat;
                        }
                        else if (currTime < gameActive.BeginTime)
                        {
                            gameActive.CombatStatus = CombatStatus.NoStart;
                        }
                        else if (currTime > gameActive.EndTime)
                        {
                            gameActive.CombatStatus = CombatStatus.Over;
                        }

                        CombatStatus combatStatus = gameActive.RefreshStatus();

                        if (combatStatus == CombatStatus.Wait)
                        {
                            //有等待时间
                            int minute = gameActive.WaitMinutes;
                            if (combatStatus != CombatStatus.Killed &&
                                currTime >= gameActive.BeginTime.AddMinutes(minute) && currTime <= gameActive.EndTime)
                            {
                                ServerEnvSet.Set(ServerEnvKey.KillBossUserID, 0);
                                gameActive.CombatStatus = CombatStatus.Combat;
                            }
                            break;
                        }
                        else if (combatStatus == CombatStatus.Combat)
                        {
                            break;
                        }
                        else if (combatStatus == CombatStatus.Over)
                        {
                            DisposeData(gameActive);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        private static void LoadData(GameActive gameActive)
        {
            if ((gameActive.CombatStatus == CombatStatus.Wait || gameActive.CombatStatus == CombatStatus.Combat) && !gameActive.LoadSuccess)
            {
                if (!string.IsNullOrEmpty(gameActive.Broadcast))
                {
                    var broadcastService = new TjxBroadcastService(null);
                    var msg = broadcastService.Create(NoticeType.Game, gameActive.Broadcast);

                    if (gameActive.ActiveId == 11)
                    {
                        int invertal = (int)new TimeSpan(0, 0, gameActive.WaitMinutes, 0).TotalSeconds / 5;
                        string startTime = DateTime.Now.ToString("HH:mm:ss");
                        string endTime = DateTime.Now.AddMinutes(gameActive.WaitMinutes).ToString("HH:mm:ss");
                        broadcastService.SendTimer(msg, startTime, endTime, true, invertal);//秒
                    }
                    else
                    {
                        broadcastService.Send(msg);
                    }

                }
                gameActive.LoadSuccess = true;
                if (gameActive.ActiveType == FunctionEnum.Booszhang)
                {
                    BossCombat.InitBoss(gameActive);
                }
                else if (gameActive.ActiveType == FunctionEnum.Lintuzhang)
                {
                    CountryCombat.Init(gameActive);
                }
                else if (gameActive.ActiveType == FunctionEnum.Multiplot)
                {
                    PlotTeamCombat.Init(gameActive);
                }
                else if (gameActive.ActiveType == FunctionEnum.MorePlotCoin)
                {
                    PlotTeamCombat.Init(gameActive);
                }
                else if (gameActive.ActiveType == FunctionEnum.MorePlotEnergy)
                {
                    PlotTeamCombat.Init(gameActive);
                }
            }
        }

        private static void DisposeData(GameActive gameActive)
        {
            if (gameActive.LoadSuccess)
            {
                gameActive.LoadSuccess = false;
                if (gameActive.ActiveType == FunctionEnum.Booszhang)
                {
                    BossCombat.Dispose(gameActive);
                }
                else if (gameActive.ActiveType == FunctionEnum.Lintuzhang)
                {
                    CountryCombat.Dispose(gameActive);
                }
                else if (gameActive.ActiveType == FunctionEnum.Multiplot)
                {
                    PlotTeamCombat.Dispose(gameActive);
                }
                else if (gameActive.ActiveType == FunctionEnum.MorePlotCoin)
                {
                    PlotTeamCombat.Dispose(gameActive);
                }
                else if (gameActive.ActiveType == FunctionEnum.MorePlotEnergy)
                {
                    PlotTeamCombat.Dispose(gameActive);
                }
            }
        }
    }
}