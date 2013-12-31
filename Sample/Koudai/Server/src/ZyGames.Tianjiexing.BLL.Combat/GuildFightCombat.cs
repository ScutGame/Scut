using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Combat;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    public class GuildFightCombat
    {
        private static FightCombat info = new FightCombat();

        private static BaseLog _log = new BaseLog();
        private static CacheList<FightCombatProcess> _combatProcessList = new CacheList<FightCombatProcess>();

        /// <summary>
        /// 参战队列
        /// </summary>
        public static CacheList<FightUser> _fightUserList = new CacheList<FightUser>();

        public static void GuildFightCombatUserList()
        {
            _fightUserList = new CacheList<FightUser>();
            var fightList = new ShareCacheStruct<ServerFight>().FindAll(m => m.FastID == info.FastID);
            foreach (ServerFight fight in fightList)
            {
                if (string.IsNullOrEmpty(fight.CombatMember))
                {
                    continue;
                }
                string[] strList = fight.CombatMember.Split(',');
                foreach (string s in strList)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    var FightStatusUser = _fightUserList.Find(m => !m.IsRemove && m.UserId == s);
                    if (FightStatusUser != null)
                    {
                        continue;
                    }
                    FightUser fightUser = new FightUser();
                    fightUser.GuildID = fight.GuildID;
                    fightUser.UserId = s;
                    GameUser user = UserCacheGlobal.CheckLoadUser(s);
                    if (user != null)
                    {
                        fightUser.UserName = user.NickName;
                        user.UserStatus = UserStatus.FightCombat;
                    }
                    fightUser.WinCount = 0;
                    fightUser.CityID = fight.CityID;
                    fightUser.ObtainNum = 0;
                    fightUser.InspirePercent = 0;
                    fightUser.IsRemove = false;
                    fightUser.IsNotEnough = false;
                    _fightUserList.Add(fightUser);
                }
            }
        }

        /// <summary>
        /// 战斗结束后退出
        /// </summary>
        public static void RemoveCombatUserList()
        {
            foreach (var fightUser in _fightUserList)
            {
                GameUser user = UserCacheGlobal.CheckLoadUser(fightUser.UserId);
                if (user != null && user.UserStatus == UserStatus.FightCombat)
                {
                    user.UserStatus = UserStatus.Normal;
                }
            }
        }

        /// <summary>
        /// 改变当前阶段
        /// </summary>
        public static void ChangStage(FightStage stage)
        {
            var currStage = info.CurStage.ToEnum<FightStage>();
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
            if (currStage != stage)
            {
                if (stage == FightStage.Close)
                    return;
                if (stage == FightStage.Apply)
                {
                    info.FastID++;
                    info.IsGuildPrize = 0;
                    info.CurStage = stage.ToInt();
                    info.Round = 0;
                    NextGuildBattleDate();
                    RemoveCombatUserList();
                    TraceLog.WriteInfo("当前第{0}届公会争夺战", info.FastID);
                    return;
                }
                if (stage == FightStage.Ready)
                {
                    GuildFightCombatUserList();
                }
                UserListFatigue();
                RaceLevel(info.CurStage.ToEnum<FightStage>());
                info.CurStage = stage.ToInt();
                info.Round = 0;
                TraceLog.WriteInfo("目前阶段{0},开始分组!", info.FastID);
                var fightInfoList = new ConfigCacheSet<GuildFightInfo>().FindAll();
                foreach (var fightInfo in fightInfoList)
                {
                    Grouping(info.FastID, info.Lenght, stage, info.Round, fightInfo.CityID);
                }
            }
        }

        /// <summary>
        /// 升级
        /// </summary>
        public static void RaceLevel(FightStage stage)
        {
            TraceLog.WriteInfo("目前阶段{0}升级中", stage);
            var fightInfoList = new ConfigCacheSet<GuildFightInfo>().FindAll();
            foreach (var fightInfo in fightInfoList)
            {
                ServerFight[] applyList = GetApply(stage, fightInfo.CityID);
                foreach (var item in applyList)
                {
                    item.Stage = RaceLevelStage(stage);
                }
            }
        }

        /// <summary>
        /// 升级的公会争斗战状态
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        public static FightStage RaceLevelStage(FightStage stage)
        {
            FightStage fightStage = stage;
            if (stage == FightStage.Apply || stage == FightStage.Ready)
            {
                fightStage = FightStage.quarter_final;
            }
            else if (stage == FightStage.quarter_final)
            {
                fightStage = FightStage.semi_final;
            }
            else if (stage == FightStage.semi_final)
            {
                fightStage = FightStage.final;
            }
            else if (stage == FightStage.final)
            {
                fightStage = FightStage.champion;
            }
            return fightStage;
        }

        /// <summary>
        /// 晋级公会
        /// </summary>
        /// <returns></returns>
        public static ServerFight[] GetApply(FightStage stage, int cityID)
        {
            int fastID = info.FastID;
            FightStage fightStage = FightStage.Ready;
            if (stage == FightStage.Apply && info.FastID > 1)
            {
                fastID = MathUtils.Subtraction(fastID, 1);
                fightStage = FightStage.final;
            }
            if (stage == FightStage.semi_Wait)
            {
                fightStage = FightStage.quarter_final;
            }
            else if (stage == FightStage.final_Wait)
            {
                fightStage = FightStage.semi_final;
            }
            var fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == fastID && s.Stage == fightStage);
            foreach (var fightgroup in fightGroupList)
            {
                if (string.IsNullOrEmpty(fightgroup.WinGuildID))
                {
                    ServerFightGroupWinGuildID(fightgroup, fastID);
                }
            }

            var applyList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == info.FastID && s.Stage == stage && s.CityID == cityID);
            applyList.QuickSort((x, y) =>
            {
                int result;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                result = (int)y.GetResult(stage).CompareTo(x.GetResult(stage));
                if (result == 0)
                {
                    var userGuildA = new ShareCacheStruct<UserGuild>().FindKey(x.GuildID);
                    var userGuildB = new ShareCacheStruct<UserGuild>().FindKey(y.GuildID);
                    if (userGuildA != null && userGuildB != null)
                    {
                        result = userGuildB.GuildLv.CompareTo(userGuildA.GuildLv);
                        if (result == 0)
                        {
                            result = userGuildB.CurrExperience.CompareTo(userGuildA.CurrExperience);
                        }
                    }
                }
                return result;
            });

            var apply = new CacheList<ServerFight>();
            for (int i = 0; i < info.Lenght && i < applyList.Count; i++)
            {
                apply.Add(applyList[i]);
            }
            return apply.ToArray();
        }

        /// <summary>
        /// 触发战斗
        /// </summary>
        private static void TriggerNomarlCombat(FightStage state)
        {
            try
            {
                var fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == info.FastID && s.Stage == state);
                if (fightGroupList.Count == 0)
                {
                    FightStage fightStage = state - 1;
                    fightGroupList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == info.FastID && s.Stage == fightStage);
                }
                Stopwatch watck = new Stopwatch();
                watck.Start();
                foreach (ServerFightGroup fightGroup in fightGroupList)
                {
                    if (string.IsNullOrEmpty(fightGroup.GuildIDA) || string.IsNullOrEmpty(fightGroup.GuildIDB))
                    {
                        if (!string.IsNullOrEmpty(fightGroup.WinGuildID))
                        {
                            continue;
                        }
                        string guildID = string.Empty;
                        if (!string.IsNullOrEmpty(fightGroup.GuildIDA))
                        {
                            guildID = fightGroup.GuildIDA;
                            fightGroup.Awin = MathUtils.Addition(fightGroup.Awin, 1);
                        }
                        else
                        {
                            guildID = fightGroup.GuildIDB;
                            fightGroup.Bwin = MathUtils.Addition(fightGroup.Bwin, 1);
                        }
                        fightGroup.WinGuildID = guildID;
                        var serverfight = new ShareCacheStruct<ServerFight>().FindKey(info.FastID, guildID);
                        if (serverfight != null)
                        {
                            serverfight.LostCount = MathUtils.Addition(serverfight.LostCount, (short)1);
                        }
                        continue;
                    }
                    var mogemaUserList = _fightUserList.FindAll(m => !m.IsRemove && m.GuildID == fightGroup.GuildIDA);
                    var hashideUserList = _fightUserList.FindAll(m => !m.IsRemove && m.GuildID == fightGroup.GuildIDB);
                    TriggeerMemberNumNotEnouth(mogemaUserList, hashideUserList, fightGroup);
                    if (mogemaUserList.Count > 0 && hashideUserList.Count > 0 && string.IsNullOrEmpty(fightGroup.WinGuildID))
                    {
                        TriggerPairCombat(mogemaUserList, hashideUserList, fightGroup);
                        TraceLog.ReleaseWriteFatal("触发战斗A公会：{0}人数{1}，B公会：{2}人数{3}", fightGroup.GuildIDA, mogemaUserList.Count, fightGroup.GuildIDB, hashideUserList.Count);
                    }
                }
                if (state == FightStage.final)
                {
                    info.GuildEndTime = DateTime.Now;
                }
                watck.Stop();
            }
            catch (Exception ex)
            {
                _log.SaveLog("领土战普通组配对", ex);
            }
        }

        /// <summary>
        /// 一方公会没人参战时判断
        /// </summary>
        /// <param name="mogemaUserList"></param>
        /// <param name="hashideUserList"></param>
        public static void TriggeerMemberNumNotEnouth(List<FightUser> mogemaUserList, List<FightUser> hashideUserList, ServerFightGroup fightGroup)
        {
            if ((mogemaUserList.Count > 0 && hashideUserList.Count > 0) || !string.IsNullOrEmpty(fightGroup.WinGuildID))
            {
                return;
            }
            if (mogemaUserList.Count > 0 && hashideUserList.Count == 0)
            {
                for (int i = 0; i < mogemaUserList.Count; i++)
                {
                    FightUser cuser1 = mogemaUserList[i];
                    if (cuser1.IsNotEnough) return;
                    cuser1.IsNotEnough = true;
                    cuser1.WinCount = MathUtils.Addition(cuser1.WinCount, 1);
                    GameUser user1 = UserCacheGlobal.CheckLoadUser(cuser1.UserId);
                    if (user1 == null)
                    {
                        return;
                    }
                    var fightInfo = new ConfigCacheSet<GuildFightInfo>().FindKey(cuser1.CityID);
                    if (fightInfo != null)
                    {
                        int obtainNum = (user1.UserLv * fightInfo.Victory).ToInt();
                        user1.ObtainNum = MathUtils.Addition(user1.ObtainNum, obtainNum);
                    }
                    fightGroup.Awin = MathUtils.Addition(fightGroup.Awin, 1);
                    //一方公会没人参战状态
                    _log.SaveDebugLog(string.Format("公会城市争斗战[{0}]配对[{1}]战斗", cuser1.UserName, string.Empty));
                }
            }

            if (mogemaUserList.Count == 0 && hashideUserList.Count > 0)
            {
                for (int i = 0; i < hashideUserList.Count; i++)
                {
                    FightUser cuser = hashideUserList[i];
                    if (cuser.IsNotEnough) return;
                    cuser.IsNotEnough = true;
                    cuser.WinCount = MathUtils.Addition(cuser.WinCount, 1);
                    GameUser user = UserCacheGlobal.CheckLoadUser(cuser.UserId);
                    if (user == null)
                    {
                        return;
                    }
                    var fightInfo = new ConfigCacheSet<GuildFightInfo>().FindKey(cuser.CityID);
                    if (fightInfo != null)
                    {
                        int obtainNum = (user.UserLv * fightInfo.Victory).ToInt();
                        user.ObtainNum = MathUtils.Addition(user.ObtainNum, obtainNum);
                    }
                    fightGroup.Bwin = MathUtils.Addition(fightGroup.Bwin, 1);
                    //一方公会没人参战状态
                    _log.SaveDebugLog(string.Format("公会城市争斗战[{0}]配对[{1}]战斗", string.Empty, cuser.UserName));
                }
            }
            ServerFightGroupWinGuildID(fightGroup, info.FastID);
        }

        /// <summary>
        /// 配对战斗
        /// </summary>
        private static void TriggerPairCombat(List<FightUser> mogemaUserList, List<FightUser> hashideUserList, ServerFightGroup fightGroup)
        {
            int combatCount = 0;
            if (mogemaUserList.Count > hashideUserList.Count)
            {
                combatCount = hashideUserList.Count;
            }
            else
            {
                combatCount = mogemaUserList.Count;
            }
            for (int i = 0; i < combatCount; i++)
            {
                FightCombatProcess process = new FightCombatProcess();
                process.stage = fightGroup.Stage;
                process.FastID = info.FastID;
                FightUser cuser1 = mogemaUserList[i];
                int toIndex = i + 1;
                FightUser cuser2 = toIndex < hashideUserList.Count ? hashideUserList[toIndex] : hashideUserList[0];
                if (cuser1 == null || cuser2 == null) continue;
                if (cuser1.UserId.Equals(cuser2.UserId)) continue;
                //cuser1.IsRemove = true;
                //cuser2.IsRemove = true;
                //配对状态
                TraceLog.ReleaseWriteFatal("触发战斗A公会：{0}名称：{1}，B公会：{2}名称：{3}", cuser1.GuildID, cuser1.UserName, cuser2.GuildID, cuser2.UserName);
                AsyncDoCombat(cuser1, cuser2, process);
            }
            ServerFightGroupWinGuildID(fightGroup, info.FastID);
        }

        private static void AsyncDoCombat(FightUser cuser1, FightUser cuser2, FightCombatProcess process)
        {
            GameUser user1 = UserCacheGlobal.LoadOffline(cuser1.UserId); //new GameDataCacheSet<GameUser>().FindKey();
            GameUser user2 = UserCacheGlobal.LoadOffline(cuser2.UserId); // new GameDataCacheSet<GameUser>().FindKey(cuser2.UserId);
            if (user1 == null || user2 == null)
            {
                return;
            }
            decimal victory = 0;
            decimal failure = 0;
            var fightInfo = new ConfigCacheSet<GuildFightInfo>().FindKey(cuser1.CityID);
            if (fightInfo != null)
            {
                victory = fightInfo.Victory;
                failure = fightInfo.Failure;
            }
            ISingleCombat combater = CombatFactory.TriggerTournament(user1, user2);
            bool isWin = combater.Doing();
            if (isWin)
            {
                FightUserListCombat(cuser1, cuser2, true);
                process.WinUserId = user1.UserID;
                process.WinUserName = user1.NickName;
                process.FailUserId = user2.UserID;
                process.FailUserName = user2.NickName;
                process.WinObtainNum = (user1.UserLv * victory).ToInt();
                user1.ObtainNum = MathUtils.Addition(user1.ObtainNum, process.WinObtainNum);
                process.FailObtainNum = (user2.UserLv * failure).ToInt();
                user2.ObtainNum = MathUtils.Addition(user2.ObtainNum, process.FailObtainNum);
                user2.UserStatus = UserStatus.Normal;
            }
            else
            {
                FightUserListCombat(cuser2, cuser1, true);
                process.WinUserId = user2.UserID;
                process.WinUserName = user2.NickName;
                process.FailUserId = user1.UserID;
                process.FailUserName = user1.NickName;

                process.WinObtainNum = (user2.UserLv * victory).ToInt();
                user2.ObtainNum = MathUtils.Addition(user2.ObtainNum, process.WinObtainNum);
                process.FailObtainNum = (user1.UserLv * failure).ToInt();
                user1.ObtainNum = MathUtils.Addition(user1.ObtainNum, process.FailObtainNum);

                user1.UserStatus = UserStatus.Normal;
            }

            process.ProcessContainer = (CombatProcessContainer)combater.GetProcessResult();

            MemberGroupProcess(cuser1, cuser2, isWin, process);

            if (_combatProcessList != null) _combatProcessList.Add(process);
            //日志
            UserCombatLog log = new UserCombatLog();
            log.CombatLogID = Guid.NewGuid().ToString();
            log.UserID = cuser1.UserId;
            log.CityID = 0;
            log.PlotID = 0;
            log.NpcID = 0;
            log.CombatType = CombatType.Country;
            log.HostileUser = cuser2.UserId;
            log.IsWin = isWin;
            log.CombatProcess = JsonUtils.Serialize(process);
            log.CreateDate = DateTime.Now;
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }

        public static void FightUserListCombat(FightUser cuser1, FightUser cuser2, bool orderly)
        {
            GameUser user1 = new GameDataCacheSet<GameUser>().FindKey(cuser1.UserId);
            GameUser user2 = new GameDataCacheSet<GameUser>().FindKey(cuser2.UserId);
            if (user1 == null || user2 == null)
            {
                return;
            }
            var serverfightgroup = new ShareCacheStruct<ServerFightGroup>().FindAll(m => m.FastID == info.FastID && m.GuildIDA == user1.MercenariesID && m.GuildIDB == user2.MercenariesID);
            if (serverfightgroup.Count > 0)
            {
                var fightInfo = new ConfigCacheSet<GuildFightInfo>().FindKey(cuser1.CityID);
                if (fightInfo != null)
                {
                    if (orderly)
                    {
                        cuser1.ObtainNum = MathUtils.Addition(cuser1.ObtainNum, (user1.UserLv * fightInfo.Victory).ToInt());
                        cuser2.ObtainNum = MathUtils.Addition(cuser2.ObtainNum, (user2.UserLv * fightInfo.Failure).ToInt());
                        cuser2.IsRemove = true;
                        user1.Fatigue = MathUtils.Addition(user1.Fatigue, 1, 5);
                        cuser1.WinCount = MathUtils.Addition(cuser1.WinCount, 1);
                        serverfightgroup[0].Awin = MathUtils.Addition(serverfightgroup[0].Awin, 1);
                    }
                    else
                    {
                        cuser2.ObtainNum = MathUtils.Addition(cuser2.ObtainNum, (user2.UserLv * fightInfo.Victory).ToInt());
                        cuser1.ObtainNum = MathUtils.Addition(cuser1.ObtainNum, (user1.UserLv * fightInfo.Failure).ToInt());
                        cuser2.WinCount = MathUtils.Addition(cuser2.WinCount, 1);
                        cuser1.IsRemove = true;
                        user2.Fatigue = MathUtils.Addition(user2.Fatigue, 1, 5);
                        serverfightgroup[0].Bwin = MathUtils.Addition(serverfightgroup[0].Bwin, 1);
                    }
                }
            }
            else
            {
                if (!orderly)
                {
                    FightUserListCombat(cuser2, cuser1, true);
                }
                else
                {
                    FightUserListCombat(cuser2, cuser1, false);
                }
            }
        }

        public static void ServerFightGroupWinGuildID(ServerFightGroup fightGroup, int fastID)
        {
            var mogemaUserList = _fightUserList.FindAll(m => !m.IsRemove && m.GuildID == fightGroup.GuildIDA);
            var hashideUserList = _fightUserList.FindAll(m => !m.IsRemove && m.GuildID == fightGroup.GuildIDB);
            if (mogemaUserList.Count > 0 && hashideUserList.Count > 0)
            {
                return;
            }
            if (fightGroup.Awin > fightGroup.Bwin)
            {
                fightGroup.WinGuildID = fightGroup.GuildIDA;
                ServerFightLostCount(fightGroup.WinGuildID);
                FightRankID(fightGroup.Stage, fightGroup.GuildIDB);
            }
            else if (fightGroup.Bwin > fightGroup.Awin)
            {
                fightGroup.WinGuildID = fightGroup.GuildIDB;
                ServerFightLostCount(fightGroup.WinGuildID);
                FightRankID(fightGroup.Stage, fightGroup.GuildIDA);
            }
            else
            {
                fightGroup.WinGuildID = ServerFightGuildID(fightGroup.GuildIDA, fightGroup.GuildIDB);
                ServerFightLostCount(fightGroup.WinGuildID);
                FightRankID(fightGroup.Stage, fightGroup.WinGuildID);
            }

            if (fightGroup.Stage == FightStage.final && !string.IsNullOrEmpty(fightGroup.WinGuildID))
            {
                ServerFight serverFight = new ShareCacheStruct<ServerFight>().FindKey(fastID, fightGroup.WinGuildID);
                if (serverFight != null)
                {
                    serverFight.RankID = 1;
                    foreach (var fightUser in _fightUserList)
                    {
                        fightUser.IsRemove = true;
                        GameUser user = UserCacheGlobal.CheckLoadUser(fightUser.UserId);
                        if (user != null)
                        {
                            user.UserStatus = UserStatus.Normal;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 胜利公会，次数加1
        /// </summary>
        /// <param name="guildID"></param>
        public static void ServerFightLostCount(string guildID)
        {
            ServerFight fight = new ShareCacheStruct<ServerFight>().FindKey(info.FastID, guildID);
            if (fight != null)
            {
                if (fight.Stage == FightStage.quarter_final)
                {
                    fight.LostCount = 1;
                }
                else if (fight.Stage == FightStage.semi_final)
                {
                    fight.LostCount = 2;
                }
                else if (fight.Stage == FightStage.final)
                {
                    fight.LostCount = 3;
                }
            }
        }

        /// <summary>
        /// 公会排名
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="guildID"></param>
        /// <param name="isWin"></param>
        public static void FightRankID(FightStage stage, string guildID)
        {
            ServerFight serverFight = new ShareCacheStruct<ServerFight>().FindKey(info.FastID, guildID);
            if (serverFight != null)
            {
                if (stage == FightStage.quarter_final)
                {
                    serverFight.RankID = 4;
                }
                else if (stage == FightStage.semi_final)
                {
                    serverFight.RankID = 3;
                }
                else if (stage == FightStage.final)
                {
                    serverFight.RankID = 2;
                }
            }
        }

        /// <summary>
        /// 比较两个公会的等级，经验
        /// </summary>
        /// <param name="guildIDA"></param>
        /// <param name="guildIDB"></param>
        /// <returns></returns>
        public static string ServerFightGuildID(string guildIDA, string guildIDB)
        {
            UserGuild userGuildA = new ShareCacheStruct<UserGuild>().FindKey(guildIDA);
            UserGuild userGuildB = new ShareCacheStruct<UserGuild>().FindKey(guildIDB);
            if (userGuildA != null && userGuildB != null)
            {
                if (userGuildA.GuildLv > userGuildB.GuildLv)
                {
                    return userGuildA.GuildID;
                }
                else if (userGuildB.GuildLv > userGuildA.GuildLv)
                {
                    return userGuildB.GuildID;
                }
                else
                {
                    if (userGuildA.CurrExperience > userGuildB.CurrExperience)
                    {
                        return userGuildA.GuildID;
                    }
                    else
                    {
                        return userGuildB.GuildID;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 玩家成员分组表
        /// </summary>
        /// <param name="cuser1"></param>
        /// <param name="cuser2"></param>
        /// <param name="isWin"></param>
        /// <param name="process"></param>
        public static void MemberGroupProcess(FightUser cuser1, FightUser cuser2, bool isWin, FightCombatProcess process)
        {
            GameUser user1 = new GameDataCacheSet<GameUser>().FindKey(cuser1.UserId);
            GameUser user2 = new GameDataCacheSet<GameUser>().FindKey(cuser2.UserId);
            if (user1 == null || user2 == null)
            {
                return;
            }
            var cacheSet = new ShareCacheStruct<MemberGroup>();
            MemberGroup memberGroup = new MemberGroup(Guid.NewGuid().ToString())
                                          {
                                              FastID = info.FastID,
                                              GuildIDA = cuser1.GuildID,
                                              GuildIDB = cuser2.GuildID,
                                              UserIDA = cuser1.UserId,
                                              UserIDB = cuser2.UserId,
                                              Win = isWin,
                                              CombatProcess = process,
                                              IsRemove = false,
                                          };
            cacheSet.Add(memberGroup);
        }

        /// <summary>
        /// 获得战斗过程
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        public static List<FightCombatProcess> GetCombatProcess(string userID, FightStage stage)
        {
            List<FightCombatProcess> list = new List<FightCombatProcess>();
            int version = 0;
            FightUser fightUser = _fightUserList.Find(m => m.UserId == userID);
            if (fightUser != null)
            {
                version = fightUser.Version;
                if (_combatProcessList != null)
                {
                    list = _combatProcessList.FindAll(m => m.FastID == info.FastID && m.stage == stage && (m.WinUserId == userID || m.FailUserId == userID) && m.Version > version);
                }
                else if (_combatProcessList != null)
                {
                    list = _combatProcessList.FindAll(m => m.FastID == info.FastID && m.stage == stage && m.Version > version);
                }

                foreach (var process in list)
                {
                    if (process.Version > fightUser.Version)
                    {
                        fightUser.Version = process.Version;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 循环比赛
        /// </summary>
        /// <param name="stage"></param>
        public static bool ServerFinal(FightStage stage)
        {
            if (stage == FightStage.Close)
                return false;
            int round = MathUtils.Subtraction(info.Round, 1);
            DateTime endDate;
            DateTime combatDate = FightCombatStartDate(out endDate).AddMinutes(round);
            if (IsCurrentWeek(combatDate) && DateTime.Now > combatDate)
            {
                TriggerNomarlCombat(stage);
                info.Round++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="FastID"></param>
        /// <param name="Lenght"></param>
        /// <param name="Stage"></param>
        /// <param name="Round"></param>
        public static void Grouping(int FastID, int Lenght, FightStage Stage, int Round, int cityID)
        {
            switch (Stage)
            {
                case FightStage.Close:
                case FightStage.Apply:
                    return;
                case FightStage.Ready:
                    finalskonckout(FastID, Lenght, Round, cityID);
                    break;
                case FightStage.semi_Wait:
                case FightStage.final_Wait:
                    Grouping(FastID, Stage, cityID);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 8强
        /// </summary>
        /// <param name="FastID"></param>
        /// <param name="Lenght"></param>
        /// <param name="Stage"></param>
        /// <param name="Round"></param>
        public static void finalskonckout(int FastID, int Lenght, int Round, int cityID)
        {
            ServerFight[] applyList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == FastID && s.Stage == FightStage.quarter_final && s.CityID == cityID).ToArray();
            Grouping(FastID, Lenght, Round, cityID, applyList);
        }

        /// <summary>
        /// 只有三个公会或两个公会是配对
        /// </summary>
        /// <param name="No"></param>
        /// <param name="guildIDA"></param>
        /// <param name="guildIDB"></param>
        /// <param name="cityID"></param>
        public static void AppendServerFightGroup(int No, string guildIDA, string guildIDB, int cityID)
        {
            var cacheSet = new ShareCacheStruct<ServerFightGroup>();
            ServerFightGroup group = new ServerFightGroup(Guid.NewGuid().ToString());
            group.CityID = cityID;
            group.Stage = FightStage.semi_final;
            group.NO = No;
            group.Round = 1;
            group.FastID = info.FastID;
            group.GuildIDA = guildIDA;
            group.Awin = 0;
            group.GuildIDB = guildIDB;
            group.Bwin = 0;
            group.WinGuildID = string.Empty;
            group.IsRemove = false;
            UserGuild guildA = new ShareCacheStruct<UserGuild>().FindKey(group.GuildIDA);
            UserGuild guildB = new ShareCacheStruct<UserGuild>().FindKey(group.GuildIDB);
            string guildAName = guildA == null ? string.Empty : guildA.GuildName;
            string guildBName = guildB == null ? string.Empty : guildB.GuildName;
            TraceLog.ReleaseWriteFatal("Round={0},NameA={1},NameB={2},No={3},cityID={4}", 1, guildAName, guildBName, No, cityID);
            cacheSet.Add(group);
        }

        /// <summary>
        /// 决赛对阵
        /// </summary>
        public static void Grouping(int FastID, FightStage Stage, int cityID)
        {
            int gstage = (int)Stage - 1;
            FightStage Rstage = (FightStage)gstage;
            if (Stage == FightStage.semi_Wait)
            {
                Rstage = FightStage.quarter_final;
            }
            else if (Stage == FightStage.final_Wait)
            {
                Rstage = FightStage.semi_final;
            }
            var againstList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == FastID && s.Stage == Rstage && s.CityID == cityID);
            againstList.QuickSort((x, y) =>
            {
                return x.NO.CompareTo(y.NO);
            });
            if (againstList.Count < 4 && Stage == FightStage.semi_Wait)
            {
                if (againstList.Count == 2)
                {
                    AppendServerFightGroup(1, againstList[0].WinGuildID, string.Empty, againstList[0].CityID);
                    AppendServerFightGroup(2, againstList[1].WinGuildID, string.Empty, againstList[1].CityID);
                }
                else if (againstList.Count == 3)
                {
                    AppendServerFightGroup(1, againstList[0].WinGuildID, againstList[1].WinGuildID, againstList[0].CityID);
                    AppendServerFightGroup(2, againstList[2].WinGuildID, string.Empty, againstList[2].CityID);
                }
            }
            else
            {
                for (int i = 0; i < againstList.Count / 2; i++)
                {
                    ServerFightGroup againstA = againstList[i * 2];
                    ServerFightGroup againstB = againstList[(i * 2) + 1];
                    ServerFightGroup against = new ServerFightGroup(Guid.NewGuid().ToString());
                    against.NO = i + 1;
                    against.Stage = Rstage;
                    if (Stage == FightStage.semi_Wait)
                    {
                        against.Stage = FightStage.semi_final;
                    }
                    else if (Stage == FightStage.final_Wait)
                    {
                        against.Stage = FightStage.final;
                    }
                    against.Round = 1;
                    against.FastID = FastID;
                    against.CityID = cityID;
                    if (againstA.Result == 1)
                    {
                        against.GuildIDA = againstA.GuildIDA;
                    }
                    else
                    {
                        against.GuildIDA = againstA.GuildIDB;
                    }

                    if (againstB.Result == 1)
                    {
                        against.GuildIDB = againstB.GuildIDA;
                    }
                    else
                    {
                        against.GuildIDB = againstB.GuildIDB;
                    }
                    against.IsRemove = false;
                    new ShareCacheStruct<ServerFightGroup>().Add(against);
                }
            }
        }

        /// <summary>
        /// 随机分组
        /// </summary>
        public static void Grouping(int FastID, int Lenght, int round, int cityID, ServerFight[] fightList)
        {
            if (IsFightLenth(fightList))
            {
                return;
            }
            List<int> intList = new List<int>();
            for (int i = 0; i < fightList.Length; i++)
            {
                intList.Add(i);
            }
            int guildLenght = fightList.Length;
            int No = 1;
            int subNum = 0;
            while (guildLenght > 0)
            {
                if (fightList.Length == 2 && No == 2)
                {
                    No++;
                    continue;
                }
                var cacheSet = new ShareCacheStruct<ServerFightGroup>();
                ServerFightGroup group = new ServerFightGroup(Guid.NewGuid().ToString());
                group.NO = No;
                group.Stage = FightStage.quarter_final;
                group.Round = round;
                group.FastID = FastID;
                group.IsRemove = false;
                int indexA = GetIndex(intList);
                int indexB = -1;
                if ((guildLenght > 4 && No == 1) || (guildLenght > 3 && No == 2) || (guildLenght > 2 && No == 3) || (guildLenght > 1 && No == 4))
                {
                    indexB = GetIndex(intList);
                    subNum = 2;
                }
                else
                {
                    subNum = 1;
                }

                if (indexA < 0 && indexB < 0)
                {
                    break;
                }
                string guildIdA = string.Empty;
                if (indexA >= 0)
                {
                    ServerFight fightA = fightList[indexA];
                    guildIdA = fightA.GuildID;
                }
                group.GuildIDA = guildIdA;

                string guildIdB = string.Empty;
                if (indexB >= 0)
                {
                    ServerFight fightB = fightList[indexB];
                    guildIdB = fightB.GuildID;
                }
                group.GuildIDB = guildIdB;
                group.CityID = cityID;
                UserGuild guildA = new ShareCacheStruct<UserGuild>().FindKey(group.GuildIDA);
                UserGuild guildB = new ShareCacheStruct<UserGuild>().FindKey(group.GuildIDB);
                string guildAName = guildA == null ? string.Empty : guildA.GuildName;
                string guildBName = guildB == null ? string.Empty : guildB.GuildName;
                TraceLog.ReleaseWriteFatal("Round={0},IndexA={1},IndexB={2},NameA={3},NameB={4},No={5},cityID={6}",
                    round, indexA, indexB, guildAName, guildBName, No, cityID);
                cacheSet.Add(group);
                guildLenght = guildLenght - subNum;
                No++;
            }
        }

        /// <summary>
        /// 分组抽签
        /// </summary>
        /// <param name="intList"></param>
        /// <returns></returns>
        private static int GetIndex(List<int> intList)
        {
            int Result = 0;
            if (intList.Count == 0)
                return -1;
            int index = RandomUtils.GetRandom(0, intList.Count);
            Result = intList[index];
            intList.Remove(intList[index]);
            return Result;

        }

        /// <summary>
        /// 获取当前时间所在阶段
        /// </summary>
        /// <returns></returns>
        public static FightStage GetStage(out DateTime nextDate)
        {
            FightStage fstage = new FightStage();
            DateTime endDate;
            DateTime begintime = FightCombatStartDate(out endDate);
            DateTime dt = DateTime.Now;
            nextDate = MathUtils.SqlMinDate;
            DateTime applyEnd = begintime.AddMinutes(-GameConfigSet.BattleEndBeforeDate);
            //DateTime endchampion = endDate.AddMinutes(2);
            //if (endDate < DateTime.Now && DateTime.Now < endchampion)
            //{
            //    fstage = FightStage.champion;
            //}
            if (dt < applyEnd)
            {
                fstage = FightStage.Apply;
                nextDate = applyEnd;
            }
            else if (dt > applyEnd && dt < begintime)
            {
                fstage = FightStage.Ready;
                nextDate = begintime;
            }
            else if (dt > begintime)
            {
                int waitDate = GameConfigSet.BattleWaitDateTime;
                if (dt < begintime.AddMinutes(waitDate))
                {
                    fstage = FightStage.Wait;
                    nextDate = begintime.AddMinutes(waitDate);
                }
                else if (dt > begintime.AddMinutes(waitDate) && dt < BattleIntervalDate(begintime, 1, 0))
                {
                    fstage = FightStage.quarter_final;
                    nextDate = BattleIntervalDate(begintime, 1, 0);
                }
                else if (dt > BattleIntervalDate(begintime, 1, 0) && dt < BattleIntervalDate(begintime, 1, waitDate))
                {
                    fstage = FightStage.semi_Wait;
                    nextDate = BattleIntervalDate(begintime, 1, waitDate);
                }
                else if (dt > BattleIntervalDate(begintime, 1, waitDate) && dt < BattleIntervalDate(begintime, 2, 0))
                {
                    fstage = FightStage.semi_final;
                    nextDate = BattleIntervalDate(begintime, 2, 0);
                }
                else if (dt > BattleIntervalDate(begintime, 2, 0) && dt < BattleIntervalDate(begintime, 2, waitDate))
                {
                    fstage = FightStage.final_Wait;
                    nextDate = BattleIntervalDate(begintime, 2, waitDate);
                }
                else if (dt > BattleIntervalDate(begintime, 2, waitDate) && dt < BattleIntervalDate(begintime, 3, waitDate))
                {
                    fstage = FightStage.final;
                    nextDate = BattleIntervalDate(begintime, 3, 0);
                }
            }
            return fstage;
        }

        public static DateTime BattleIntervalDate(DateTime beginDate, int num, int waitDate)
        {
            int intervalDate = GameConfigSet.BattleIntervalDate;
            int totaldate = MathUtils.Addition(intervalDate * num, waitDate);
            return beginDate.AddMinutes(totaldate);
        }


        /// <summary>
        /// 公会战奖励
        /// </summary>
        public static void GetCombatPrize()
        {
            DateTime enddate = DateTime.Now.Date.AddMinutes(10);
            if (DateTime.Now > info.GuildEndTime.AddDays(1).Date && DateTime.Now < info.GuildStartTime && DateTime.Now < enddate) //公会战结束后第二天开始领取
            {
                int fastID = MathUtils.Subtraction(info.FastID, 1, 1);
                if (info.IsGuildPrize == 1 && FightCombat.GuildPrizeTime.Date == DateTime.Now.Date)
                {
                    return;
                }
                var serverFightList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == fastID);
                foreach (var fight in serverFightList)
                {
                    GuildFightInfo fightInfo = new ConfigCacheSet<GuildFightInfo>().FindKey(fight.CityID);
                    if (fightInfo != null)
                    {
                        GuildWarRewards(fight.RankID, fight.GuildID, fightInfo);
                    }
                }
                info.IsGuildPrize = 1;
                FightCombat.GuildPrizeTime = DateTime.Now;
                TraceLog.ReleaseWriteFatal("当前第{0}届公会争夺战，下发奖励时间{1}", fastID, DateTime.Now);
            }
            else
            {
                info.IsGuildPrize = 0;
            }
        }

        public static void GuildWarRewards(int rankID, string guildID, GuildFightInfo fightInfo)
        {
            int itemID = 0;
            string mailContent = string.Empty;
            var memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.GuildID == guildID);
            if (rankID == 1)
            {
                AwardWinner(guildID, fightInfo);
                itemID = fightInfo.FirstPackID;
                mailContent = LanguageManager.GetLang().St6404_GuildWarFirstPackID;
            }
            else if (rankID == 2)
            {
                itemID = fightInfo.SecondPackID;
                mailContent = LanguageManager.GetLang().St6404_GuildWarSecondPackID;
            }
            else if (rankID == 3)
            {
                itemID = fightInfo.ThirdPackID;
                mailContent = LanguageManager.GetLang().St6404_GuildWarThirdPackID;
            }
            else
            {
                itemID = fightInfo.ParticipateID;
                mailContent = LanguageManager.GetLang().St6404_GuildWarParticipateID;
            }
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemID);
            if (itemInfo != null)
            {
                mailContent = string.Format(mailContent, itemInfo.ItemName);
            }

            foreach (var member in memberArray)
            {
                AddPrize(member.UserID, itemID, mailContent);
            }
        }

        public static void AddPrize(string UserID, int itemID, string mailContent)
        {
            string ItemPackage = itemID + "=1=1";
            var cacheset = new ShareCacheStruct<UserTakePrize>();
            UserTakePrize takeprize = new UserTakePrize()
            {
                CreateDate = DateTime.Now,
                CrystalPackage = string.Empty,
                EnergyNum = 0,
                ExpNum = 0,
                GainBlessing = 0,
                GameCoin = 0,
                Gold = 0,
                ID = Guid.NewGuid().ToString(),
                ItemPackage = ItemPackage,
                IsTasked = false,
                MailContent = mailContent,
                ObtainNum = 0,
                OpUserID = 0,
                SparePackage = string.Empty,
                UserID = UserID.ToInt(),
                VipLv = 0,
            };
            cacheset.Add(takeprize);
        }

        /// <summary>
        ///  公会城市争斗战冠军奖励
        /// </summary>
        /// <param name="guildID"></param>
        /// <param name="fightInfo"></param>
        public static void AwardWinner(string guildID, GuildFightInfo fightInfo)
        {
            CityInfo cityInfo = new ConfigCacheSet<CityInfo>().FindKey(fightInfo.CityID);
            if (cityInfo == null)
            {
                return;
            }
            var memberArray = new ShareCacheStruct<GuildMember>().FindAll(s => s.GuildID == guildID);
            foreach (var guildMember in memberArray)
            {
                var user = UserCacheGlobal.CheckLoadUser(guildMember.UserID);
                if (user == null)
                {
                    continue;
                }
                var cachePrize = new ShareCacheStruct<UserTakePrize>();
                var takePrize = GetUserTake(fightInfo.Coefficient, user.UserID, user.UserLv, cityInfo.CityName);
                cachePrize.Add(takePrize);
            }
        }

        /// <summary>
        /// 公会城市争斗战冠军奖励
        /// </summary>
        /// <param name="user"></param>
        public static UserTakePrize GetUserTake(CacheList<PrizeInfo> prizeList, string userID, int userlv, string cityName)
        {
            UserTakePrize userPrize = new UserTakePrize();
            userPrize.CreateDate = DateTime.Now;
            userPrize.ID = Guid.NewGuid().ToString();
            userPrize.UserID = Convert.ToInt32(userID);
            userPrize.IsTasked = false;
            userPrize.TaskDate = MathUtils.SqlMinDate;
            userPrize.ItemPackage = string.Empty;
            userPrize.SparePackage = string.Empty;
            userPrize.CrystalPackage = string.Empty;
            userPrize.OpUserID = 10000;
            foreach (var prize in prizeList)
            {
                if (prize.Type == RewardType.GameGoin)
                {
                    userPrize.GameCoin = (prize.Coefficient * userlv).ToInt();

                }
                else if (prize.Type == RewardType.ExpNum)
                {
                    userPrize.ExpNum = (prize.Coefficient * userlv).ToInt();

                }
                else if (prize.Type == RewardType.Gold)
                {
                    userPrize.Gold = (prize.Coefficient * userlv).ToInt();
                }
            }
            userPrize.MailContent = string.Format(LanguageManager.GetLang().ChampionWelfare, cityName,
                                                  userPrize.GameCoin, userPrize.Gold, userPrize.ExpNum);
            return userPrize;
        }

        /// <summary>
        /// 公会名称
        /// </summary>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public static string GuildName(string guildID)
        {
            var guild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
            if (guild != null)
            {
                return guild.GuildName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 公会会长名称
        /// </summary>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public static string GuildChairman(string guildID)
        {
            string chairMan = string.Empty;
            var member = new ShareCacheStruct<GuildMember>().Find(s => s.GuildID == guildID && s.PostType == PostType.Chairman);
            if (member != null)
            {
                string userid = member.UserID;
                UserCacheGlobal.CheckLoadUser(userid);
                GameUser user = new GameDataCacheSet<GameUser>().FindKey(userid);
                if (user != null)
                {
                    chairMan = user.NickName;
                }
            }
            return chairMan;
        }

        /// <summary>
        /// 是否本周时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsCurrentWeek(DateTime dateTime)
        {
            DateTime currDt = DateTime.Now.Date;
            int currWeek = currDt.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)currDt.DayOfWeek;
            var fromDate = currDt.AddDays((int)DayOfWeek.Monday - currWeek);
            var toDate = fromDate.AddDays(7);
            if (fromDate <= dateTime.Date && toDate > dateTime.Date)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否公会争斗战时间
        /// </summary>
        /// <returns></returns>
        public static bool IsFightDate()
        {
            DateTime endDate;
            DateTime currDate = FightCombatStartDate(out endDate);
            if (currDate <= MathUtils.SqlMinDate || !IsCurrentWeek(info.GuildStartTime))
            {
                currDate = NextGuildBattleDate();
            }
            if (currDate > MathUtils.SqlMinDate && DateTime.Now > currDate && DateTime.Now < endDate)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 本次公会战报名截止时间
        /// </summary>
        /// <returns></returns>
        public static DateTime CurrFightDate()
        {
            DateTime endDate;
            DateTime combatDate = FightCombatStartDate(out endDate);
            if (combatDate <= MathUtils.SqlMinDate)
            {
                combatDate = NextGuildBattleDate();
            }
            DateTime applyDate = combatDate.AddMinutes(-GameConfigSet.BattleEndBeforeDate);
            return applyDate;
        }

        /// <summary>
        /// 下一次公会争斗战战斗时间
        /// </summary>
        /// <param name="dateType"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NextGuildBattleDate()
        {
            DateTime currDate = info.GuildStartTime;
            if (string.IsNullOrEmpty(GameConfigSet.BattleTime))
            {
                return currDate;
            }
            List<FightDate> fightDatesList = JsonUtils.Deserialize<List<FightDate>>(GameConfigSet.BattleTime);
            foreach (var fightDate in fightDatesList)
            {
                DateTime startDate = GuildBattleDate(fightDate.EnableWeek, fightDate.StartPeriod);
                if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday) && startDate < DateTime.Now)
                {
                    int currWeek = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Now.DayOfWeek;
                    DateTime bossDate = DateTime.Now.AddDays((int)DayOfWeek.Sunday - currWeek);
                    int dateNum = MathUtils.Addition((int)fightDate.EnableWeek, 7);
                    string dayDate = bossDate.AddDays(dateNum).ToString("d");
                    startDate = (dayDate + " " + fightDate.StartPeriod).ToDateTime();
                    if (currDate < DateTime.Now)
                    {
                        currDate = startDate;
                        if (currDate > MathUtils.SqlMinDate)
                        {
                            info.GuildStartTime = currDate;
                        }
                    }
                }
                if (startDate > DateTime.Now && startDate > currDate)
                {
                    if (currDate < DateTime.Now)
                    {
                        currDate = startDate;
                        if (currDate > MathUtils.SqlMinDate)
                        {
                            info.GuildStartTime = currDate;
                        }
                    }
                    continue;
                }
            }
            return currDate;
        }

        /// <summary>
        /// 公会争斗战战斗时间
        /// </summary>
        /// <param name="dateType"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GuildBattleDate(BossDateType dateType, string date)
        {
            DateTime currDt = DateTime.Now.Date;
            int currWeek = currDt.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)currDt.DayOfWeek;
            DateTime bossDate = currDt.AddDays((int)DayOfWeek.Sunday - currWeek);
            string dayDate = bossDate.AddDays((int)dateType).ToString("d");
            DateTime fightDate = (dayDate + " " + date).ToDateTime();
            return fightDate;
        }

        /// <summary>
        /// 是否已报名参加本届城市争斗战
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="fightMember"></param>
        /// <returns></returns>
        public static bool IsFightWar(string userID, string fightMember)
        {
            string[] memberArray = fightMember.Split(',');
            foreach (var s in memberArray)
            {
                if (s == userID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 公会争斗战开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime FightCombatStartDate(out DateTime guildEndDate)
        {
            DateTime currDate = MathUtils.SqlMinDate;
            guildEndDate = MathUtils.SqlMinDate;
            if (string.IsNullOrEmpty(GameConfigSet.BattleTime))
            {
                return currDate;
            }
            List<FightDate> fightDatesList = JsonUtils.Deserialize<List<FightDate>>(GameConfigSet.BattleTime);
            foreach (var fightDate in fightDatesList)
            {
                DateTime startDate = GuildBattleDate(fightDate.EnableWeek, fightDate.StartPeriod);
                DateTime endDate = GuildBattleDate(fightDate.EnableWeek, fightDate.EndPeriod);
                if (startDate < DateTime.Now && endDate > DateTime.Now)
                {
                    guildEndDate = endDate;
                    return startDate;
                }
                if (startDate > DateTime.Now && startDate > currDate)
                {
                    if (currDate < DateTime.Now)
                    {
                        guildEndDate = endDate;
                        currDate = startDate;
                    }
                    continue;
                }
            }
            return currDate;
        }

        /// <summary>
        /// 公会争斗战冠军
        /// </summary>
        /// <param name="cityID"></param>
        /// <returns></returns>
        public static string FightChampion(int cityID)
        {
            FightCombat combat = new FightCombat();
            int fastID = MathUtils.Subtraction(combat.FastID, 1, 1);
            var applyList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == fastID && !s.IsRemove && s.CityID == cityID && s.RankID == 1);
            if (applyList.Count > 0)
            {
                return applyList[0].GuildID;
            }
            return string.Empty;
        }

        /// <summary>
        /// 下届争斗战当日凌晨0点删除上届数据
        /// </summary>
        public static void RemoveGuildFight()
        {
            if (DateTime.Now.Date == info.GuildStartTime.Date && DateTime.Now > info.GuildStartTime.Date && DateTime.Now < info.GuildStartTime.Date.AddMinutes(2))
            {
                int fastID = MathUtils.Subtraction(info.FastID, 1, 1);
                var applyList = new ShareCacheStruct<ServerFight>().FindAll(s => s.FastID == fastID);
                foreach (var fight in applyList)
                {
                    fight.IsRemove = true;
                }

                var fightGroupsList = new ShareCacheStruct<ServerFightGroup>().FindAll(s => s.FastID == fastID);
                foreach (var fightGroup in fightGroupsList)
                {
                    fightGroup.IsRemove = true;
                }

                var memberGroupList = new ShareCacheStruct<MemberGroup>().FindAll(s => s.FastID == fastID);
                foreach (var member in memberGroupList)
                {
                    member.IsRemove = true;
                }
                TraceLog.WriteInfo("删除第{0}届公会争夺战数据", fastID);
            }
        }

        /// <summary>
        /// 冠军登陆该城市，广播 
        /// </summary>
        /// <param name="cityID"></param>
        /// <param name="user"></param>
        public static void SantoVisit(int cityID, GameUser user)
        {
            string guildID = FightChampion(cityID);
            GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(guildID, user.UserID);
            if (member != null && guildID == user.MercenariesID && member.PostType == PostType.Chairman)
            {
                UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(guildID);
                CityInfo cityInfo = new ConfigCacheSet<CityInfo>().FindKey(cityID);
                UserDailyRestrain userDaily = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
                if (userDaily != null && userDaily.UserExtend != null && userDaily.UserExtend.BroadcastNum >= 10)
                {
                    return;
                }
                if (userGuild != null && cityInfo != null)
                {
                    if (userDaily != null)
                    {
                        if (userDaily.UserExtend == null)
                        {
                            userDaily.UserExtend = new DailyUserExtend();
                        }
                        userDaily.UserExtend.BroadcastNum = MathUtils.Addition(userDaily.UserExtend.BroadcastNum, 1);
                    }
                    new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St6413_SantoVisit, cityInfo.CityName, user.NickName));
                }
            }
        }

        /// <summary>
        /// 消除疲劳值
        /// </summary>
        public static void UserListFatigue()
        {
            foreach (var fightuser in _fightUserList)
            {
                GameUser user = UserCacheGlobal.CheckLoadUser(fightuser.UserId);
                if (user != null && user.Fatigue > 0)
                {
                    user.Fatigue = 0;
                }
            }
        }

        public static bool IsFightLenth(ServerFight[] fightArray)
        {
            if (fightArray.Length == 1)
            {
                fightArray[0].RankID = 1;
                fightArray[0].Stage = FightStage.final;
                GetServerFightGroup(fightArray[0].CityID, fightArray[0].GuildID, string.Empty);
                return true;
            }
            return false;
        }

        public static void GetServerFightGroup(int cityID, string guildIDA, string guildIDB)
        {
            var cacheSet = new ShareCacheStruct<ServerFightGroup>();
            ServerFightGroup group = new ServerFightGroup(Guid.NewGuid().ToString());
            group.NO = 1;
            group.Stage = FightStage.final;
            group.Round = 1;
            group.FastID = info.FastID;
            group.IsRemove = false;
            group.GuildIDA = guildIDA;
            group.GuildIDB = guildIDB;
            group.CityID = cityID;
            group.WinGuildID = guildIDA;
            cacheSet.Add(group);
        }
    }
}