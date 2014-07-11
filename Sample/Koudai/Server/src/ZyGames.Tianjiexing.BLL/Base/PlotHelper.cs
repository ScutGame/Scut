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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Cache.Generic;


namespace ZyGames.Tianjiexing.BLL.Base
{
    public static class PlotHelper
    {
        private static readonly string[] ScorePercent = ConfigEnvSet.GetString("Plot.ScorePercent").Split(new[] { ',' });
        private static readonly double ScorePrizePercent = ConfigEnvSet.GetDouble("Plot.ScorePrizePercent");
        public static GameDataCacheSet<UserDailyRestrain> _cacheSetUserDaily = new GameDataCacheSet<UserDailyRestrain>();
        /// <summary>
        /// 开启副本
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enablePlot"></param>
        public static void EnablePlot(string userId, string enablePlot)
        {
            string[] plotList = enablePlot.ToNotNullString().Split(new[] { ',' });
            foreach (string str in plotList)
            {
                int plotId = str.ToInt();
                EnablePlot(userId, plotId);
            }
        }

        public static void EnablePlot(string userId, int plotId)
        {
            var packge = UserPlotPackage.Get(userId);
            if (packge == null)
            {
                return;
            }
            var userPlot = UserPlotHelper.GetUserPlotInfo(userId, plotId);

            if (userPlot == null)
            {
                userPlot = new UserPlotInfo();
                userPlot.PlotID = plotId;
                userPlot.PlotStatus = PlotStatus.NoComplete;
                userPlot.CompleteDate = DateTime.Now;
                packge.SaveItem(userPlot);
                if (userPlot.PlotType == PlotType.Kalpa)
                {
                    PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotId);
                    GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userId);
                    if (plotInfo == null || userInfo == null)
                    {
                        return;
                    }
                    if (userInfo.UserExtend == null)
                    {
                        userInfo.UserExtend = new GameUserExtend();
                    }
                    userInfo.UserExtend.UpdateNotify(obj =>
                    {
                        userInfo.UserExtend.LayerNum = plotInfo.LayerNum;
                        userInfo.UserExtend.HurdleNum = plotInfo.PlotSeqNo;
                        return true;
                    });
                }
            }
        }

        /// <summary>
        /// 英雄副本开启
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        public static void EnableHeroPlot(string userID, int plotID)
        {
            var packge = UserPlotPackage.Get(userID);
            if (packge == null)
            {
                return;
            }
            var userPlot = UserPlotHelper.GetUserPlotInfo(userID, plotID);
            if (userPlot != null)
            {
                PlotInfo[] plotInfoArray = new ConfigCacheSet<PlotInfo>().FindAll(m => m.PlotType == PlotType.HeroPlot && m.PrePlotID == plotID).ToArray();
                foreach (PlotInfo info in plotInfoArray)
                {
                    var uplot = UserPlotHelper.GetUserPlotInfo(userID, info.PlotID);
                    if (uplot == null)
                    {
                        uplot = new UserPlotInfo();
                        uplot.PlotID = info.PlotID;
                        uplot.PlotStatus = PlotStatus.NoComplete;
                        uplot.CreateDate = DateTime.Now;
                        packge.SaveItem(uplot);
                    }
                }
            }
        }

        /// <summary>
        /// 刷新副本奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        public static bool RefleshPrize(string userID, int plotID)
        {
            int turnsNum = 0;
            int battleNum = 0;

            List<UserQueue> queueList = new GameDataCacheSet<UserQueue>().FindAll(userID, m => m.QueueType == QueueType.SaoDang);
            if (queueList.Count == 0)
            {
                return false;
            }
            UserQueue saodangQueue = queueList[0];
            var npcList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == plotID);
            int npcCount = npcList.Count;
            GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(userID);

            while (HasSweep(userID, plotID, saodangQueue.GetTiming(), npcCount, out turnsNum, out battleNum))
            {
                int tempbattleNum = 0;
                if (gameUser.SweepPool.TurnsNum > 0)
                {
                    tempbattleNum = (gameUser.SweepPool.TurnsNum - 1) * npcCount + gameUser.SweepPool.BattleNum;
                }
                tempbattleNum += 1;
                turnsNum = GetTurnsNum(npcCount, tempbattleNum);
                battleNum = GetTimesNum(npcCount, tempbattleNum);
                DoPlotSweepPrize(userID, plotID, turnsNum, battleNum, npcCount);
                if (gameUser.SweepPool.TurnsNum < turnsNum)
                {
                    if (gameUser.SurplusEnergy > 0)
                    {
                        //每轮扣一次精力
                        gameUser.SurplusEnergy = MathUtils.Subtraction(gameUser.SurplusEnergy, PlotInfo.BattleEnergyNum, (short)0);
                    }
                    else
                    {
                        //每轮扣一次精力
                        gameUser.EnergyNum = MathUtils.Subtraction(gameUser.EnergyNum, PlotInfo.BattleEnergyNum, (short)0);
                    }
                }

                gameUser.SweepPool.UpdateNotify(obj =>
                {
                    gameUser.SweepPool.PlotID = plotID;
                    gameUser.SweepPool.TurnsNum = turnsNum;
                    gameUser.SweepPool.BattleNum = battleNum;
                    return true;
                });
                //gameUser.Update();

            }
            if (saodangQueue.DoRefresh() == 0)
            {
                if (gameUser.UserStatus != UserStatus.Normal)
                {
                    var cacheSet = new GameDataCacheSet<UserQueue>();
                    cacheSet.Delete(saodangQueue);

                    gameUser.UserStatus = UserStatus.Normal;
                    gameUser.ResetSweepPool(0);
                    //gameUser.Update();
                }
            }

            return true;
        }

        /// <summary>
        /// 第几次
        /// </summary>
        /// <param name="npcCount"></param>
        /// <param name="battleNum">扫荡次数</param>
        /// <returns></returns>
        internal static int GetTimesNum(int npcCount, int battleNum)
        {
            return (battleNum - 1) % npcCount + 1;
        }
        /// <summary>
        /// 第几轮
        /// </summary>
        /// <param name="npcCount"></param>
        /// <param name="battleNum">扫荡次数</param>
        /// <returns></returns>
        internal static int GetTurnsNum(int npcCount, int battleNum)
        {
            return (battleNum - 1) / npcCount + 1;
        }
        /// <summary>
        /// 扫荡次数
        /// </summary>
        /// <param name="coldTime">倒计时间</param>
        /// <returns></returns>
        internal static int GetBattleNum(int coldTime)
        {
            return coldTime / PlotInfo.BattleSpeedNum;
        }
        /// <summary>
        /// 判断是否有扫荡记录
        /// </summary>
        internal static bool HasSweep(string userId, int plotID, int timing, int npcCount, out int turnsNum, out int timesNum)
        {
            turnsNum = 0;
            timesNum = 0;

            GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(userId);
            if (gameUser.SweepPool == null)
            {
                gameUser.SweepPool = new SweepPoolInfo { PlotID = plotID, TurnsNum = 0, BattleNum = 0 };
            }
            int battleNum = GetBattleNum(timing);

            if (battleNum == 0)
            {
                return false;
            }
            turnsNum = GetTurnsNum(npcCount, battleNum);
            timesNum = GetTimesNum(npcCount, battleNum);
            if (gameUser.SweepPool.TurnsNum < turnsNum ||
                (gameUser.SweepPool.TurnsNum == turnsNum && gameUser.SweepPool.BattleNum < timesNum))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 副本扫荡奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        /// <param name="turnsNum"></param>
        /// <param name="battleNum"></param>
        /// <param name="npcCount"></param>
        internal static void DoPlotSweepPrize(string userID, int plotID, int turnsNum, int battleNum, int npcCount)
        {
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user == null)
            {
                return;
            }

            int experience = 0;
            var npcList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == plotID && m.NpcSeqNo == battleNum);
            if (npcList.Count == 0) return;
            PlotNPCInfo npcInfo = npcList[0];
            experience += npcInfo.Experience;
            var cacheSet = new GameDataCacheSet<UserSweepPool>();
            UserSweepPool sweepPool = cacheSet.FindKey(userID, turnsNum, battleNum);
            if (sweepPool == null)
            {
                sweepPool = new UserSweepPool
                {
                    UserID = userID,
                    CreateDate = DateTime.Now
                };
            }
            sweepPool.PlotID = plotID;
            sweepPool.TurnsNum = turnsNum;
            sweepPool.BattleNum = battleNum;
            sweepPool.Experience = npcInfo.Experience;
            sweepPool.ExpNum = npcInfo.ExpNum;
            sweepPool.GameCoin = npcInfo.GameCoin;
            sweepPool.Gold = npcInfo.GetRandomGold();
            sweepPool.PrizeItems = GetPlotMonsterItems(userID, npcInfo.PlotNpcID);
            sweepPool.IsSend = false;
            if (cacheSet.FindKey(userID, turnsNum, battleNum) == null)
            {
                cacheSet.Add(sweepPool);
            }
            //else
            //{
            //    sweepPool.Update();
            //}

            user.ExpNum = MathUtils.Addition(user.ExpNum, sweepPool.ExpNum, int.MaxValue);
            user.GameCoin = MathUtils.Addition(user.GameCoin, sweepPool.GameCoin + sweepPool.BlessPennyNum, int.MaxValue);
            user.ItemGold = MathUtils.Addition(user.ItemGold, sweepPool.Gold, int.MaxValue);

            if (battleNum == npcCount)
            {
                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);

                experience += plotInfo.Experience;
                //通关奖励
                int tempNum = 10;
                //var cacheSet = new GameDataCacheSet<UserSweepPool>();
                if (cacheSet.FindKey(userID, turnsNum, tempNum) == null)
                {
                    sweepPool = new UserSweepPool
                                    {
                                        UserID = userID,
                                        CreateDate = DateTime.Now
                                    };
                }
                sweepPool.PlotID = plotID;
                sweepPool.TurnsNum = turnsNum;
                sweepPool.BattleNum = tempNum;
                sweepPool.Experience = plotInfo.Experience;
                sweepPool.ExpNum = plotInfo.ExpNum;
                sweepPool.GameCoin = plotInfo.GameCoin;
                sweepPool.Gold = plotInfo.GetRandomGold();
                sweepPool.PrizeItems = GetPrizeItems(userID, plotInfo.ItemProbability, plotInfo.ItemRank, plotInfo.PlotID, null);
                sweepPool.IsSend = false;

                //祝福加金币

                if (UserHelper.GainBlessing(user, 0) > 0)
                {
                    UserHelper.GainBlessing(user, 1);
                    sweepPool.BlessPennyNum = (int)Math.Floor(plotInfo.GameCoin * new GuildMember().BlessingCionPercent);
                }
                if (!string.IsNullOrEmpty(user.MercenariesID))
                {
                    //公会技能加成
                    sweepPool.Experience = MathUtils.RoundCustom(sweepPool.Experience * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.Experience)).ToInt();
                    sweepPool.ExpNum = MathUtils.RoundCustom(sweepPool.ExpNum * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.ExpNum)).ToInt();
                    sweepPool.GameCoin = MathUtils.RoundCustom(sweepPool.GameCoin * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.CoinNum)).ToInt();
                }
                if (cacheSet.FindKey(userID, turnsNum, tempNum) == null)
                {
                    cacheSet.Add(sweepPool);
                }

                user.ExpNum = MathUtils.Addition(user.ExpNum, sweepPool.ExpNum, int.MaxValue);
                user.GameCoin = MathUtils.Addition(user.GameCoin, sweepPool.GameCoin, int.MaxValue);
                user.ItemGold = MathUtils.Addition(user.ItemGold, sweepPool.Gold, int.MaxValue);

                //日常任务-通关副本
                TaskHelper.TriggerDailyTask(userID, 4005);

            }

            //user.Update();
            //佣兵经验
            //UserHelper.UserGeneralExp(user.UserID, experience);
            TaskHelper.KillPlotMonster(userID, plotID, npcList[0].PlotNpcID);
            TrumpAbilityAttack.CombatTrumpLift(userID);
        }

        /// <summary>
        /// 副本奖励，如果通关下发通关奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotNpcInfo"></param>
        /// <param name="userPlotCombat"></param>
        internal static void DoPlotPrize(string userID, PlotNPCInfo plotNpcInfo, UserPlotCombat userPlotCombat, int npcGeneralNum, out int honourNum)
        {

            honourNum = 0;
            int experience = 0;
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotNpcInfo.PlotID);
            var package = UserPlotPackage.Get(userID);
            if (plotInfo == null || plotNpcInfo == null || userPlotCombat == null || package == null)
            {
                return;
            }

            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);

            if (user != null)
            {

                experience += plotNpcInfo.Experience;
                userPlotCombat.Experience = plotNpcInfo.Experience;
                if (!string.IsNullOrEmpty(user.MercenariesID))
                {
                    //公会技能加成
                    userPlotCombat.Experience = MathUtils.RoundCustom(experience * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.Experience)).ToInt();
                    experience = userPlotCombat.Experience;
                }
                var cacheSetGeneral = new GameDataCacheSet<UserGeneral>();
                var userMagic = new GameDataCacheSet<UserMagic>().Find(user.UserID, s => s.IsEnabled);
                int userMagicID = userMagic == null ? 0 : userMagic.MagicID;
                var userEmbattleList = new GameDataCacheSet<UserEmbattle>().FindAll(userID, s => s.MagicID == userMagicID && s.GeneralID > 0);
                int generalNum = 0;
                userEmbattleList.ForEach(userEmbattle =>
                {
                    var userGeneral = cacheSetGeneral.FindKey(userID, userEmbattle.GeneralID);
                    generalNum = userGeneral != null && userGeneral.LifeNum > 0
                                     ? MathUtils.Addition(generalNum, 1)
                                     : generalNum;
                });
                user.GeneralAllCount = MathUtils.Addition(user.GeneralAllCount, userPlotCombat.GeneralNum);
                user.GeneralKillCount = MathUtils.Addition(user.GeneralKillCount,
                                                           (userPlotCombat.GeneralNum - generalNum));
                //怪物掉落物品)
                if (plotNpcInfo.IsBoss)
                {

                    if (plotInfo.PlotType == PlotType.Normal && user.PlotProgress < plotInfo.PlotID)
                    {
                        user.PlotProgress = plotInfo.PlotID;
                    }


                    //日常任务-通关副本
                    TaskHelper.TriggerDailyTask(userID, 4005);

                    DateTime currDate = DateTime.Now;
                    //通关奖励

                    var userPlot = UserPlotHelper.GetUserPlotInfo(userID, plotNpcInfo.PlotID);
                    if (userPlot == null)
                    {
                        userPlot = new UserPlotInfo();
                        userPlot.PlotID = plotNpcInfo.PlotID;
                        userPlot.CreateDate = currDate;
                        package.SaveItem(userPlot);
                    }

                    NoviceHelper.PlotFestivalList(user, plotInfo.PlotID); //活动集合
                    List<UserPlotCombat> preUserPlotList = new GameDataCacheSet<UserPlotCombat>().FindAll(userID, m => !m.PlotNpcID.Equals(userPlotCombat.PlotNpcID) && m.PlotID == plotNpcInfo.PlotID);
                    preUserPlotList.Add(userPlotCombat);
                    List<UserPlotCombat> plotCombatList = preUserPlotList;
                    short starScore;
                    PlotSuccessType plotSuccessType = PlotSuccessType.No;
                    userPlot.ScoreNum = GetPlotScoreNum(plotCombatList, out starScore);

                    userPlot.PlotStatus = PlotStatus.Completed;
                    userPlot.AttackScore = 0;
                    userPlot.DefenseScore = 0;
                    userPlot.ItemID = 0;
                    userPlot.EnchantID = 0;

                    userPlot.PlotSuccessType = plotSuccessType;
                    //获得星星等级
                    if (userPlotCombat.IsWin)
                    {
                        double pren = 0;
                        GetStar(user, generalNum, out starScore, out plotSuccessType, out  pren);
                        userPlot.StarScore = starScore;
                        userPlot.PlotSuccessType = plotSuccessType;
                        //获得荣誉值
                        honourNum = plotInfo.HonourNum;



                        userPlot.PlotNum = MathUtils.Addition(userPlot.PlotNum, 1);
                        if (GetPlotChallengeNum(userID, plotNpcInfo.PlotID) == 1)
                        {
                            if (starScore >= 3)
                            {
                                if (plotInfo.PlotType==PlotType.Elite)  // 如果是精英副本
                                {
                                    if (userPlot.FirstWin==false)  // 如果是首次获得3星
                                    {
                                        userPlot.FirstWin = true;
                                        userPlot.ExpNum = (int)(plotInfo.ExpNum * 2);
                                        userPlot.PennyNum = (int)(plotInfo.GameCoin * 2);
                                    }
                                    else
                                    {
                                        userPlot.ExpNum = plotInfo.ExpNum;
                                        userPlot.PennyNum = plotInfo.GameCoin;
                                    }
                                }
                                else
                                {
                                    userPlot.ExpNum = (int)(plotInfo.ExpNum * 2);
                                    userPlot.PennyNum = (int)(plotInfo.GameCoin * 2);
                                }
                            }
                            else
                            {
                                userPlot.ExpNum = plotInfo.ExpNum;
                                userPlot.PennyNum = plotInfo.GameCoin;
                            }
                            honourNum = honourNum * starScore;
                        }
                        else
                        {
                            userPlot.ExpNum = plotInfo.ExpNum;
                            userPlot.PennyNum = plotInfo.GameCoin;
                        }
                        userPlot.HonourNum = honourNum;
                        GeneralEscalateHelper.AddUserLv(user, honourNum);
                    }
                    else
                    {
                        PlotFailureType plotFailureType;
                        GetFailure(npcGeneralNum, out plotFailureType);
                        userPlot.PlotFailureType = plotFailureType;

                    }

                    // userPlot.ExpNum = plotInfo.ExpNum;
                    userPlot.GoldNum = plotInfo.GetRandomGold();
                    if (!string.IsNullOrEmpty(user.MercenariesID))
                    {
                        //公会技能加成
                        userPlot.Experience = MathUtils.RoundCustom(userPlot.Experience * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.Experience)).ToInt();
                        userPlot.ExpNum = MathUtils.RoundCustom(userPlot.ExpNum * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.ExpNum)).ToInt();
                        userPlot.PennyNum = MathUtils.RoundCustom(userPlot.PennyNum * CombatHelper.GetGuildAbilityNum(user.UserID, GuildAbilityType.CoinNum)).ToInt();
                    }
                    CacheList<PrizeItemInfo> itemList = new CacheList<PrizeItemInfo>();
                    string itemName = string.Empty;
                    if (plotInfo.PlotType != PlotType.Kalpa)
                    {
                        var copyList = GetPrizeItems(userID, plotInfo.ItemProbability, plotInfo.ItemRank, plotInfo.PlotID, userPlot);
                        copyList.Foreach(item =>
                        {
                            if (item.Type == 0)
                            {
                                item.RemoveParentEvent();
                                itemList.Add(item);
                            }
                            return true;
                        });

                        if (itemList.Count > 0)
                        {
                            userPlot.ItemID = itemList[0].ItemID;
                            var item = new ConfigCacheSet<ItemBaseInfo>().FindKey(userPlot.ItemID);
                            itemName = item != null ? item.ItemName : string.Empty;
                        }
                    }
                    if (!string.IsNullOrEmpty(plotInfo.EnchantID) && plotInfo.EnchantProbability > 0)
                    {
                        var copyList = GetKalpaPrizeItems(userID, plotInfo.ItemProbability, plotInfo.ItemRank, plotInfo.PlotID, userPlot);
                        copyList.Foreach(item =>
                        {
                            if (item.Type == 2)
                            {
                                itemList.Add(item);
                                if (plotInfo.PlotType != PlotType.Kalpa)
                                {
                                    userPlot.EnchantID = item.ItemID;
                                    EnchantAddUser(user, item.ItemID);
                                }
                            }
                            return true;
                        });

                    }

                    if (plotInfo.PlotType == PlotType.Elite)
                    {
                        //精英副本奖励发聊天<label color='0,255,0' >{0}</label><label>{0}</label>
                        string content = string.Format(LanguageManager.GetLang().St_PlotRewardNotice, user.NickName, plotNpcInfo.NpcName,
                           itemName);
                        new TjxChatService().SystemSend(ChatType.World, content);
                    }
                    //祝福加金币
                    if (UserHelper.GainBlessing(user, 0) > 0)
                    {
                        UserHelper.GainBlessing(user, 1);
                        userPlot.BlessPennyNum = (int)Math.Floor(userPlot.PennyNum * new GuildMember().BlessingCionPercent);
                        userPlot.PennyNum = MathUtils.Addition(userPlot.PennyNum, userPlot.BlessPennyNum);
                        user.GameCoin = MathUtils.Addition(user.GameCoin, userPlot.PennyNum, int.MaxValue);
                    }

                    if (userPlot.CompleteDate <= MathUtils.SqlMinDate)
                    {
                        userPlot.CompleteDate = currDate;
                    }
                    userPlot.RefleshDate = currDate;
                    //experience = userPlot.Experience;
                    //userPlotCombat.Experience = experience;
                    experience += userPlot.Experience + userPlot.BlessExperience;
                    user.GameCoin = MathUtils.Addition(user.GameCoin, userPlot.PennyNum, int.MaxValue);
                    user.ExpNum = MathUtils.Addition(user.ExpNum, userPlot.ExpNum, int.MaxValue);
                    user.ItemGold = MathUtils.Addition(user.ItemGold, userPlot.GoldNum, int.MaxValue);
                    //user.Update();


                    //奖励日志
                    UserPlotPrizeLog prizeLog = new UserPlotPrizeLog
                    {
                        PrizeLogID = Guid.NewGuid().ToString(),
                        ScoreNum = userPlot.ScoreNum,
                        StarScore = userPlot.StarScore,
                        PrizeItem = itemList,
                        Experience = userPlot.Experience,
                        ExpNum = userPlot.ExpNum,
                        GameCoin = userPlot.PennyNum,
                        PlotID = userPlot.PlotID,
                        UserID = userID,
                        CreateDate = DateTime.Now
                    };
                    var sender = DataSyncManager.GetDataSender();
                    sender.Send(prizeLog);
                    //重置评价
                    foreach (UserPlotCombat plotCombat in plotCombatList)
                    {
                        //只有一个NPC时更新不了
                        //if (plotCombatList.Count > 1 && plotCombat.Equals(userPlotCombat)) continue;

                        plotCombat.GeneralNum = 0;
                        plotCombat.GeneralOverNum = 0;
                        plotCombat.OverNum = 0;
                        //plotCombat.Update();

                    }
                    //通关副本 副本ID清空
                    if (user.UserExtend != null)
                    {
                        user.UserExtend.UpdateNotify(obj =>
                        {
                            user.UserExtend.PlotStatusID = 0;
                            user.UserExtend.PlotNpcID = 0;
                            user.UserExtend.MercenarySeq = 0;
                            return true;
                        });
                    }
                    package.SaveItem(userPlot);
                    user.GeneralAllCount = 0;
                    user.GeneralKillCount = 0;
                    if (plotInfo.PlotType == PlotType.Normal)
                    {
                        int multiple = FestivalHelper.DuplicateDropDouble(user.UserID);
                        if (multiple > 1)
                        {
                            FestivalHelper.DuplicateDropDoubleRestrain(user.UserID);
                        }
                    }
                }


                //佣兵经验
                AddExprerience(user, experience);


            }
        }

        /// <summary>
        /// 获得星星
        /// </summary>
        /// <param name="generalNum"></param>
        /// <param name="star"></param>
        /// <param name="plotSuccessType"></param>
        public static void GetStar(GameUser user, int generalNum, out short star, out PlotSuccessType plotSuccessType, out double pren)
        {
            plotSuccessType = PlotSuccessType.XiaoSheng;
            star = 1;
            if (user.GeneralKillCount > 0)
            {
                pren = user.GeneralKillCount.ToDouble() / user.GeneralAllCount;
            }
            else
            {
                pren = 1;
            }
            if (pren >= 0.8 && pren <= 1)
            {
                star = 3;
            }
            else if (pren >= 0.4 && pren <= 0.79)
            {
                star = 2;
            }
            else if (pren >= 0 && pren <= 0.39)
            {
                star = 1;
            }
            //switch (generalNum)
            //{
            //    case 5:
            //        plotSuccessType = PlotSuccessType.WangSheng;
            //        star = 3;
            //        break;
            //    case 4:
            //        plotSuccessType = PlotSuccessType.DaSheng;
            //        star = 2;
            //        break;
            //    case 3:
            //        plotSuccessType = PlotSuccessType.ShentLi;
            //        star = 2;
            //        break;
            //    case 2:
            //        star = 1;
            //        plotSuccessType = PlotSuccessType.XiaoSheng;
            //        break;
            //    case 1:
            //        plotSuccessType = PlotSuccessType.XiangSheng;
            //        star = 1;
            //        break;
            //}
        }
        /// <summary>
        /// 获得失败类型
        /// </summary>
        /// <param name="generalNum"></param>
        /// <param name="star"></param>
        /// <param name="plotSuccessType"></param>
        public static void GetFailure(int generalNum, out PlotFailureType plotFailureType)
        {
            plotFailureType = PlotFailureType.XiaoBai;

            switch (generalNum)
            {
                case 5:
                    plotFailureType = PlotFailureType.WangBai;
                    break;
                case 4:
                    plotFailureType = PlotFailureType.KuiBai;

                    break;
                case 3:
                    plotFailureType = PlotFailureType.ChangBai;
                    break;
                case 2:
                    plotFailureType = PlotFailureType.DaBai;
                    break;
                case 1:
                    plotFailureType = PlotFailureType.XiaoBai;

                    break;
            }
        }
        public static void AddExprerience(GameUser user, int experience)
        {
            string userID = user.UserID;
            var userEmbattleList = new GameDataCacheSet<UserEmbattle>().FindAll(userID, m => m.MagicID == user.UseMagicID);
            HashSet<int> generalHash = new HashSet<int>();
            foreach (UserEmbattle userEmbattle in userEmbattleList)
            {
                //wuzf 8-18 修复多个相同佣兵阵形数据
                if (generalHash.Contains(userEmbattle.GeneralID))
                {
                    userEmbattle.GeneralID = 0;
                    //userEmbattle.Update();
                    continue;
                }
                else
                {
                    generalHash.Add(userEmbattle.GeneralID);
                }
                //UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, userEmbattle.GeneralID);
                //if (userGeneral != null)
                //{
                //    userGeneral.CurrExperience = MathUtils.Addition(userGeneral.CurrExperience, experience);
                //}
            }
            //UserHelper.UserGeneralExp(user.UserID, experience);
        }


        private static short GetPlotScoreNum(List<UserPlotCombat> plotCombatList, out short starScore)
        {
            starScore = 1;
            int generalNum = 0;
            int generalOverNum = 0;
            int overNum = 0;
            foreach (UserPlotCombat plotCombat in plotCombatList)
            {
                generalNum += plotCombat.GeneralNum;
                generalOverNum += plotCombat.GeneralOverNum;
                overNum += plotCombat.OverNum;
            }
            short scoreNum = (short)Math.Ceiling(MathUtils.Subtraction((double)generalNum, generalOverNum) * 100 / generalNum);

            for (int i = 0; i < ScorePercent.Length; i++)
            {
                if (scoreNum <= ScorePercent[i].ToShort())
                {
                    starScore = (short)(i + 1);
                    break;
                }
            }
            if (overNum > 0)
            {
                starScore = 1;
            }
            return scoreNum;
        }

        public static CacheList<PrizeItemInfo> GetPlotMonsterItems(string userID, int plotNpcID)
        {
            CacheList<PrizeItemInfo> itemList = new CacheList<PrizeItemInfo>();
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (userInfo != null)
            {
                int doubleitem = GetDouble(userID, plotNpcID);
                int multiple = FestivalHelper.DuplicateDropDouble(userID);
                List<PlotEmbattleInfo> embattleInfoList = new ConfigCacheSet<PlotEmbattleInfo>().FindAll(m => m.PlotNpcID == plotNpcID);
                foreach (PlotEmbattleInfo embattleInfo in embattleInfoList)
                {
                    MonsterInfo monster = new ConfigCacheSet<MonsterInfo>().FindKey(embattleInfo.MonsterID);
                    if (monster == null)
                    {
                        continue;
                    }
                    //原因：活动类型修改
                    if (RandomUtils.IsHit(NoviceHelper.FestivalMultiple(monster.ItemProbability)))
                    //if (RandomUtils.IsHit(FestivalHelper.DuplicateDropDouble(userID, monster.ItemProbability)))
                    {
                        if (ItemBaseInfo.IsExist(monster.ItemID))
                        {
                            PrizeItemInfo itemInfo = itemList.Find(m => m.ItemID == monster.ItemID);
                            if (itemInfo == null)
                            {
                                itemInfo = new PrizeItemInfo
                                {
                                    Type = 0,
                                    ItemID = monster.ItemID,
                                    Num = 1 * doubleitem * multiple
                                };
                                itemList.Add(itemInfo);
                            }
                            else
                            {
                                itemInfo.Num += 1 * doubleitem * multiple;
                            }
                        }
                    }
                }

                foreach (var itemInfo in itemList)
                {
                    UserItemHelper.AddUserItem(userID, itemInfo.ItemID, itemInfo.Num);
                    CacheList<PrizeItemInfo> prizeItemInfos = new CacheList<PrizeItemInfo>();
                    prizeItemInfos.Add(new PrizeItemInfo() { Type = 0, ItemID = itemInfo.ItemID, Num = itemInfo.Num });

                    if (prizeItemInfos.Count > 0)
                    {
                        userInfo.UserExtend.UpdateNotify(obj =>
                            {
                                userInfo.UserExtend.ItemList = prizeItemInfos;
                                return true;
                            });
                        //userInfo.Update();
                    }
                }
            }
            return itemList;
        }

        /// <summary>
        /// 天地劫副本奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotNpcID"></param>
        /// <returns></returns>
        public static CacheList<PrizeItemInfo> GetKalpaPlotMonsterItems(string userID, int plotID, int plotNpcID)
        {
            var chatService = new TjxChatService();
            CacheList<PrizeItemInfo> itemList = new CacheList<PrizeItemInfo>();
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (userInfo == null)
            {
                return itemList;
            }
            PlotNPCInfo npcInfo = new ConfigCacheSet<PlotNPCInfo>().FindKey(plotNpcID);
            GetKalpaplotSparePart(userInfo, itemList, npcInfo, chatService);
            GetKalpaplotEnchant(userInfo, itemList, plotID);
            return itemList;
        }

        /// <summary>
        /// 天地劫获取灵件
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="itemList"></param>
        /// <param name="npcInfo"></param>
        /// <param name="chatService"></param>
        private static void GetKalpaplotSparePart(GameUser userInfo, CacheList<PrizeItemInfo> itemList, PlotNPCInfo npcInfo, TjxChatService chatService)
        {
            if (npcInfo != null && RandomUtils.IsHit(npcInfo.SparePartProbability))
            {
                SparePartInfo partInfo = new ConfigCacheSet<SparePartInfo>().FindKey(npcInfo.SparePartID);
                if (partInfo != null && SparePartInfo.IsExist(npcInfo.SparePartID))
                {
                    UserSparePart sparePart = UserSparePart.GetRandom(npcInfo.SparePartID);
                    if (sparePart != null)
                    {
                        PrizeItemInfo itemInfo = itemList.Find(m => m.ItemID == npcInfo.SparePartID);
                        if (itemInfo == null)
                        {
                            itemInfo = new PrizeItemInfo
                            {
                                Type = 1,
                                ItemID = npcInfo.SparePartID,
                                Num = 1
                            };
                            itemList.Add(itemInfo);
                        }
                        else
                        {
                            itemInfo.Num += 1;
                        }
                        if (UserHelper.AddSparePart(userInfo, sparePart))
                        {
                            //userInfo.Update();
                        }
                        else
                        {
                            //掉落灵件
                            chatService.SystemSendWhisper(userInfo, string.Format(LanguageManager.GetLang().St4303_SparePartFalling, partInfo.Name));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 天地劫获取附魔符
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="itemList"></param>
        /// <param name="npcInfo"></param>
        /// <param name="chatService"></param>
        private static void GetKalpaplotEnchant(GameUser userInfo, CacheList<PrizeItemInfo> itemList, int plotID)
        {
            EnchantInfo enchantInfo = GetPrizeEnchant(userInfo.UserID, plotID);
            if (enchantInfo == null || enchantInfo.EnchantID == 0)
                return;
            PrizeItemInfo itemInfo = itemList.Find(m => m.ItemID == enchantInfo.EnchantID);
            if (itemInfo == null)
            {
                itemInfo = new PrizeItemInfo
                               {
                                   Type = 2,
                                   ItemID = enchantInfo.EnchantID,
                                   Num = 1
                               };
                itemList.Add(itemInfo);
            }
            else
            {
                itemInfo.Num += 1;
            }
        }

        /// <summary>
        /// 通關副本掉落物品
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemProbability"></param>
        /// <param name="itemRank"></param>
        /// <returns></returns>
        private static CacheList<PrizeItemInfo> GetKalpaPrizeItems(string userID, decimal itemProbability, string itemRank, int plotID, UserPlotInfo userPlot)
        {
            var itemList = new CacheList<PrizeItemInfo>();
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
            if (plotInfo == null)
            {
                return itemList;
            }

            string[] items = itemRank.Trim().Split(new[] { ',' });
            int count = items.Length + 1;
            int[] precent = new int[count];
            int precentNum = 0;
            for (int i = 0; i < count; i++)
            {
                if (i != (count - 1))
                {
                    var itemInfo = items[i].Split('=');
                    decimal prize = itemInfo[2].ToDecimal();
                    precent[i] = (prize * 1000).ToInt();
                    precentNum = MathUtils.Addition(precentNum, precent[i]);
                }
                else
                {
                    precent[i] = (1000 - precentNum);
                }
            }

            int index = RandomUtils.GetHitIndexByTH(precent);
            if (plotInfo.PlotType != PlotType.Kalpa && index != (count - 1))
            {
                if (items.Length == 0)
                {
                    return itemList;
                }
                string[] itemArray = items[index].Split('=');
                if (itemArray.Length == 2)
                {
                    int itemId = itemArray[0].ToInt();
                    if (new ConfigCacheSet<ItemBaseInfo>().FindKey(itemId) != null)
                    {
                        PrizeItemInfo itemInfo = itemList.Find(m => m.ItemID == itemId);
                        if (itemInfo == null)
                        {
                            itemInfo = new PrizeItemInfo
                                           {
                                               Type = 0,
                                               ItemID = itemId,
                                               Num = itemArray[1].ToInt()
                                           };
                            itemList.Add(itemInfo);
                        }
                        else
                        {
                            itemInfo.Num += itemArray[1].ToInt();
                        }
                    }
                }
            }
            List<UniversalInfo> universalInfoList = new List<UniversalInfo>();
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            foreach (var itemInfo in itemList)
            {
                UserItemHelper.AddUserItem(userID, itemInfo.ItemID, itemInfo.Num, universalInfoList);
                if (userInfo != null)
                {
                    CacheList<PrizeItemInfo> prizeItemInfos = new CacheList<PrizeItemInfo>();
                    prizeItemInfos.Add(new PrizeItemInfo() { Type = 0, ItemID = itemInfo.ItemID, Num = itemInfo.Num });

                    if (prizeItemInfos.Count > 0)
                    {
                        userInfo.UserExtend.UpdateNotify(obj =>
                        {
                            userInfo.UserExtend.ItemList = prizeItemInfos;
                            return true;
                        });
                        //userInfo.Update();
                    }
                }
            }

            if (universalInfoList.Count > 0 && userPlot != null)
            {
                universalInfoList.ForEach(universalInfo =>
                {
                    userPlot.ItemList.Add(universalInfo);
                });
            }
            NoviceHelper.PlotFestivalList(userInfo, plotID); //活动集合
            GetKalpaplotEnchant(userInfo, itemList, plotID); //副本掉落附魔符
            return itemList;
        }

        /// <summary>
        /// 扫荡副本
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="itemProbability"></param>
        /// <param name="itemRank"></param>
        /// <returns></returns>
        private static CacheList<PrizeItemInfo> GetPrizeItems(string userID, decimal itemProbability, string itemRank, int plotID, UserPlotInfo userPlot)
        {
            List<UniversalInfo> universalInfoList = new List<UniversalInfo>();
            var itemList = new CacheList<PrizeItemInfo>();

            string[] items = itemRank.Trim().Split(new[] { ',' });
            int count = items.Length + 1;
            int[] precent = new int[count];
            int precentNum = 0;
            for (int i = 0; i < count; i++)
            {
                if (i != (count - 1))
                {
                    var itemInfo = items[i].Split('=');
                    decimal prize = itemInfo[2].ToDecimal();
                    precent[i] = (prize * 1000).ToInt();
                    precentNum = MathUtils.Addition(precentNum, precent[i]);
                }
                else
                {
                    precent[i] = (1000 - precentNum);
                }
            }

            int index = RandomUtils.GetHitIndexByTH(precent);
            if (index != (count - 1))
            {
                int itemId = 0;
                int num = 0;
                if (items.Length > 1)
                {

                    itemId = items[index].Split('=')[0].ToInt();
                    num = items[index].Split('=')[1].ToInt();
                }
                else
                {
                    itemId = items[0].Split('=')[0].ToInt();
                    num = items[0].Split('=')[1].ToInt();
                }

                var itemBase = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemId);
                if (itemBase != null)
                {
                    PrizeItemInfo prizeItem = itemList.Find(m => m.ItemID == itemId);
                    if (prizeItem == null)
                    {
                        prizeItem = new PrizeItemInfo
                        {
                            Type = 0,
                            ItemID = itemId,
                            Num = num
                        };
                        itemList.Add(prizeItem);
                    }
                    else
                    {
                        prizeItem.Num += num;
                    }
                }

            }
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            foreach (var itemInfo in itemList)
            {

                UserItemHelper.AddUserItem(userID, itemInfo.ItemID, itemInfo.Num, universalInfoList);
                if (userInfo != null)
                {
                    CacheList<PrizeItemInfo> prizeItemInfos = new CacheList<PrizeItemInfo>();
                    prizeItemInfos.Add(new PrizeItemInfo() { Type = 0, ItemID = itemInfo.ItemID, Num = itemInfo.Num });

                    if (prizeItemInfos.Count > 0)
                    {
                        userInfo.UserExtend.UpdateNotify(obj =>
                            {
                                userInfo.UserExtend.ItemList = prizeItemInfos;
                                return true;
                            });
                        //userInfo.Update();
                    }
                }
            }
            NoviceHelper.PlotFestivalList(userInfo, plotID); //活动集合

            if (universalInfoList.Count > 0)
            {
                foreach (var item in universalInfoList)
                {
                    if (userPlot.ItemList != null)
                    {
                        userPlot.ItemList.Add(item);
                    }
                }
            }
            return itemList;
        }

        /// <summary>
        /// 副本双倍道具
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        private static int GetDouble(string UserID, int plotNpcID)
        {
            PlotNPCInfo npcInfo = new ConfigCacheSet<PlotNPCInfo>().FindKey(plotNpcID);
            if (npcInfo != null)
            {
                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(npcInfo.PlotID);
                if (plotInfo != null && plotInfo.PlotType == PlotType.Normal)
                {
                    int ItemID = 7003;
                    UserProps props = new GameDataCacheSet<UserProps>().FindKey(UserID, ItemID);
                    if (props != null && props.SurplusNum > 0)
                    {
                        props.SurplusNum = MathUtils.Subtraction(props.SurplusNum, 1, 0);
                        //props.Update();
                        return 2;
                    }
                }
            }
            return 1;
        }

        public static int JingYingPlotReset(string userID, int vipLv)
        {
            int resetNum = 0;
            UserHelper.ChechDailyRestrain(userID);
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (VipHelper.GetVipOpenFun(vipLv, ExpandType.ZhongZhiJingYingPlot))
            {
                resetNum = MathUtils.Addition(resetNum, 1, int.MaxValue);
            }
            if (VipHelper.GetVipOpenFun(vipLv, ExpandType.SecondZhongZhiJingYingPlot))
            {
                resetNum = MathUtils.Addition(resetNum, 1, int.MaxValue);
            }
            if (dailyRestrain != null && dailyRestrain.RefreshDate.Date >= DateTime.Now.Date)
            {
                resetNum = MathUtils.Subtraction(resetNum, dailyRestrain.Funtion10, 0);
            }
            return resetNum;
        }

        /// <summary>
        /// 当前城市英雄副本重置次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cityID"></param>
        /// <returns></returns>
        public static int HeroRefreshNum(string userID, int cityID)
        {
            int heroNum = 0;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.UserExtend != null && dailyRestrain.UserExtend.HeroPlot.Count > 0)
            {
                List<HeroPlot> heroPlotsList = dailyRestrain.UserExtend.HeroPlot;
                HeroPlot plot = heroPlotsList.Find(m => m.CityID.Equals(cityID));
                if (plot != null)
                {
                    heroNum = plot.HeroNum;
                }
            }
            return heroNum;
        }

        /// <summary>
        /// 当前城市英雄副本剩余重置次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cityID"></param>
        /// <param name="vipLv"></param>
        /// <returns></returns>
        public static int HeroSurplusNum(string userID, int cityID, int vipLv)
        {
            int maxHeroNum = 0;
            int heroNum = HeroRefreshNum(userID, cityID);
            DailyRestrainSet restrainSet = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.HeroRefreshNum);
            if (restrainSet != null)
            {
                maxHeroNum = restrainSet.MaxNum;
            }
            if (VipHelper.GetVipOpenFun(vipLv, ExpandType.HeroRefreshPlot))
            {
                maxHeroNum = MathUtils.Addition(maxHeroNum, 1, int.MaxValue);
            }
            if (VipHelper.GetVipOpenFun(vipLv, ExpandType.HeroSecondRefreshPlot))
            {
                maxHeroNum = MathUtils.Addition(maxHeroNum, 1, int.MaxValue);
            }

            return MathUtils.Subtraction(maxHeroNum, heroNum, 0);
        }

        /// <summary>
        /// 英雄副本副本增加挑战次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        /// <param name="_cityID"></param>
        public static void HeroDailyRestrain(string userID, int plotID, int _cityID)
        {
            var cacheSet = new GameDataCacheSet<UserDailyRestrain>();
            UserDailyRestrain userRestrain = cacheSet.FindKey(userID);
            if (userRestrain != null)
            {
                DailyUserExtend userExtend = new DailyUserExtend();
                List<HeroPlot> heroList = new List<HeroPlot>();
                if (userRestrain.UserExtend != null)
                {
                    userExtend = userRestrain.UserExtend;
                    if (userRestrain.UserExtend.HeroPlot.Count > 0)
                    {
                        heroList = userRestrain.UserExtend.HeroPlot;
                    }
                }
                HeroPlot plot = heroList.Find(m => m.CityID.Equals(_cityID));
                if (plot != null)
                {
                    plot.HeroList.Add(new FunPlot() { PlotID = plotID });
                }
                else
                {
                    FunPlot funPlot = new FunPlot();
                    funPlot.PlotID = plotID;
                    List<FunPlot> funPlotsList = new List<FunPlot>();
                    funPlotsList.Add(funPlot);
                    heroList.Add(new HeroPlot() { CityID = _cityID, HeroNum = 0, HeroList = funPlotsList });
                }
                userExtend.HeroPlot = heroList;
                userRestrain.UserExtend = userExtend;
                cacheSet.Update();
            }
        }

        public static bool IsKill(string userID, int plotID, int cityID)
        {
            bool isKill = false;
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
            UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (plotInfo.PlotType == PlotType.Elite)
            {
                if (userRestrain != null && userRestrain.FunPlot != null)
                {
                    FunPlot funPlot = userRestrain.FunPlot.FindLast(m => m.PlotID == plotID);
                    var restrainSet = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.JingYingPlot);
                    if (funPlot != null && restrainSet != null && userRestrain.RefreshDate.Date >= DateTime.Now.Date)
                    {
                        int funMaxNum = MathUtils.Addition(restrainSet.MaxNum, userRestrain.Funtion10, int.MaxValue);
                        if (funPlot.Num >= funMaxNum)
                        {
                            isKill = true;
                        }
                    }
                }
            }
            else if (plotInfo.PlotType == PlotType.HeroPlot)
            {
                if (userRestrain != null && userRestrain.UserExtend != null && userRestrain.UserExtend.HeroPlot.Count > 0)
                {
                    List<HeroPlot> heroPlotsList = userRestrain.UserExtend.HeroPlot;
                    HeroPlot heroPlot = heroPlotsList.Find(m => m.CityID.Equals(cityID));
                    if (heroPlot != null && heroPlot.HeroList.Count > 0)
                    {
                        List<FunPlot> funPlotsList = heroPlot.HeroList.FindAll(m => m.PlotID.Equals(plotID));
                        if (funPlotsList.Count > 0 && funPlotsList.Count > heroPlot.HeroNum)
                        {
                            isKill = true;
                        }
                    }
                }
            }
            return isKill;
        }

        /// <summary>
        /// 通关副本掉落附魔符
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plotID"></param>
        /// <returns></returns>
        private static EnchantInfo GetPrizeEnchant(string userID, int plotID)
        {
            EnchantInfo enchantInfo = new EnchantInfo();
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
            if (plotInfo != null)
            {
                GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
                UserFunction userFunction = new GameDataCacheSet<UserFunction>().FindKey(userID, FunctionEnum.Enchant);
                if (userInfo != null && userFunction != null && RandomUtils.IsHit(plotInfo.EnchantProbability))
                {
                    string[] enchants = plotInfo.EnchantID.Trim().Split(new[] { ',' });
                    int index = RandomUtils.GetRandom(0, enchants.Length);
                    int enID = enchants[index].ToInt();
                    if (new ConfigCacheSet<EnchantInfo>().FindKey(enID) != null)
                    {
                        enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enID);
                    }
                }
            }
            return enchantInfo;
        }

        /// <summary>
        /// 添加副本掉落的附魔符
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="enchantID"></param>
        public static void EnchantAddUser(GameUser userInfo, int enchantID)
        {
            UserEnchantInfo userenchant = EnchantHelper.GetUserEnchantInfo(enchantID);
            EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchantID);
            var package = UserEnchant.Get(userInfo.UserID);
            if (userenchant != null && enchantInfo != null && package != null)
            {
                int enchantNum = package.EnchantPackage.FindAll(m => string.IsNullOrEmpty(m.UserItemID)).Count;

                if (userInfo.UserExtend != null && userInfo.UserExtend.EnchantGridNum > enchantNum)
                {
                    UserLogHelper.AppenEnchantLog(userInfo.UserID, 1, userenchant, new CacheList<SynthesisInfo>());
                    package.SaveEnchant(userenchant);
                }
                else
                {
                    var chatService = new TjxChatService();
                    chatService.SystemSendWhisper(userInfo, string.Format(LanguageManager.GetLang().St4303_EnchantingCharacterFalling, enchantInfo.EnchantName));
                    UserLogHelper.AppenEnchantLog(userInfo.UserID, 6, userenchant, new CacheList<SynthesisInfo>());
                }
            }
        }

        /// <summary>
        /// 获取挑战次数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="plotId"></param>
        /// <returns></returns>
        public static int GetPlotChallengeNum(string userId, int plotId)
        {
            int num = 0;
            var cacheSetUserPlot = new GameDataCacheSet<UserPlotPackage>();
            var userPlot = cacheSetUserPlot.FindKey(userId);
            var plot = userPlot != null && userPlot.PlotPackage != null
                           ? userPlot.PlotPackage.Find(s => s.PlotID == plotId)
                           : null;
            if (plot != null)
            {
                num = plot.PlotNum;
            }

            return num;
        }

        /// <summary>
        /// 判断是否第一次开启挑战次数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="plotId"></param>
        /// <returns></returns>
        public static bool GetPlotIsOne(string userId, int plotId)
        {
            bool isOne = false;
            var cacheSetUserPlot = new GameDataCacheSet<UserPlotPackage>();
            var userPlot = cacheSetUserPlot.FindKey(userId);
            var plot = userPlot != null && userPlot.PlotPackage != null
                           ? userPlot.PlotPackage.Find(s => s.PlotID == plotId)
                           : null;
            if (plot != null && plot.PlotNum <= 0)
            {
                isOne = true;
            }
            return isOne;
        }


    }
}