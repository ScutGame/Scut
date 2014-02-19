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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Combat;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 4004_副本战斗详情接口
    /// </summary>
    public class Action4004 : BaseAction
    {
        private int PlotNpcID;
        private UserPlotCombat userPlotCombat;
        private CacheList<PrizeItemInfo> prizeItems = new CacheList<PrizeItemInfo>();
        private ConfigCacheSet<GeneralInfo> _cacheSetGeneral = new ConfigCacheSet<GeneralInfo>();
        //private List<PlotEmbattleInfo> plotEmbattleList = new PlotEmbattleInfo[0];
        //private List<UserEmbattle> userEmbattleList = new UserEmbattle[0];
        private CombatProcessContainer combatProcessList = new CombatProcessContainer();
        private List<SelfAbilityEffect> selfAbilityEffectList = new List<SelfAbilityEffect>();
        private int _honourNum = 0;
        private int GotoNum;
        private int _userTalPriority = 0;
        private int _npcTalPriority = 0;
        public Action4004(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4004, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(userPlotCombat.IsWin.ToShort());
            this.PushIntoStack(userPlotCombat.Experience);
            this.PushIntoStack(prizeItems.Count);
            foreach (PrizeItemInfo prizeItem in prizeItems)
            {
                ItemBaseInfo item = new ConfigCacheSet<ItemBaseInfo>().FindKey(prizeItem.ItemID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.ItemName.ToNotNullString());
                dsItem.PushIntoStack(item.HeadID.ToNotNullString());
                dsItem.PushIntoStack(item.QualityType.ToShort());
                dsItem.PushIntoStack(prizeItem.Num);

                PushIntoStack(dsItem);
            }
            //攻方阵形
            PushIntoStack(combatProcessList.AttackList.Count);
            foreach (CombatEmbattle combatEmbattle in combatProcessList.AttackList)
            {
                int isAttWait = combatEmbattle.IsWait ? 1 : 0;
                var general = _cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatEmbattle.GeneralID);
                dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                //dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
                dsItem.PushIntoStack(general == null ? string.Empty : general.BattleHeadID.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                dsItem.PushIntoStack(combatEmbattle.LiveNum);
                dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                dsItem.PushIntoStack(combatEmbattle.AbilityID);
                dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                dsItem.PushIntoStack((short)isAttWait);
                // 增加佣兵品质
                dsItem.PushShortIntoStack(general == null ? 0 : general.GeneralQuality.ToShort());


                PushIntoStack(dsItem);
            }
            //防方阵形
            PushIntoStack(combatProcessList.DefenseList.Count);
            foreach (CombatEmbattle combatEmbattle in combatProcessList.DefenseList)
            {
                int isDefWait = combatEmbattle.IsWait ? 1 : 0;
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatEmbattle.GeneralID);
                dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                dsItem.PushIntoStack(combatEmbattle.LiveNum);
                dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                dsItem.PushIntoStack(combatEmbattle.AbilityID);
                dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                dsItem.PushIntoStack((short)isDefWait);
                // 增加佣兵品质
                var general = _cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
                dsItem.PushShortIntoStack(general == null ? 0 : general.GeneralQuality.ToShort());

                PushIntoStack(dsItem);
            }
            //战斗过程
            PushIntoStack(combatProcessList.ProcessList.Count);
            foreach (CombatProcess combatProcess in combatProcessList.ProcessList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatProcess.GeneralID);
                dsItem.PushIntoStack(combatProcess.LiveNum);
                dsItem.PushIntoStack(combatProcess.Momentum);
                dsItem.PushIntoStack(combatProcess.AttackTaget.ToShort());
                dsItem.PushIntoStack(combatProcess.AttackUnit.ToShort());
                dsItem.PushIntoStack(combatProcess.AbilityProperty.ToShort());
                dsItem.PushIntoStack(combatProcess.AttStatus.ToShort());
                dsItem.PushIntoStack(combatProcess.DamageNum);
                dsItem.PushIntoStack(combatProcess.AttEffectID.ToNotNullString());
                dsItem.PushIntoStack(combatProcess.TargetEffectID.ToNotNullString());
                dsItem.PushIntoStack(combatProcess.IsMove.ToShort());
                dsItem.PushIntoStack(combatProcess.Position.ToShort());
                dsItem.PushIntoStack(combatProcess.Role.ToShort());


                dsItem.PushIntoStack(combatProcess.DamageStatusList.Count);
                foreach (AbilityEffectStatus effectStatus in combatProcess.DamageStatusList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(effectStatus.AbilityType.ToShort());
                    dsItem1.PushIntoStack(effectStatus.DamageNum);
                    dsItem1.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                    dsItem.PushIntoStack(dsItem1);
                }

                dsItem.PushIntoStack(combatProcess.TargetList.Count);
                foreach (TargetProcess targetProcess in combatProcess.TargetList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(targetProcess.GeneralID);
                    dsItem1.PushIntoStack(targetProcess.LiveNum);
                    dsItem1.PushIntoStack(targetProcess.Momentum);
                    dsItem1.PushIntoStack(targetProcess.DamageNum);
                    dsItem1.PushIntoStack(targetProcess.IsShanBi.ToShort());
                    dsItem1.PushIntoStack(targetProcess.IsGeDang.ToShort());
                    dsItem1.PushIntoStack(targetProcess.IsBack.ToShort());
                    dsItem1.PushIntoStack(targetProcess.IsMove.ToShort());
                    dsItem1.PushIntoStack(targetProcess.BackDamageNum);
                    dsItem1.PushIntoStack(targetProcess.TargetStatus.ToShort());
                    dsItem1.PushIntoStack(targetProcess.Position.ToShort());
                    dsItem1.PushIntoStack(targetProcess.Role.ToShort());
                    //目标中招效果
                    dsItem1.PushIntoStack(targetProcess.DamageStatusList.Count);
                    foreach (AbilityEffectStatus effectStatus in targetProcess.DamageStatusList)
                    {
                        DataStruct dsItem12 = new DataStruct();
                        dsItem12.PushIntoStack(effectStatus.AbilityType.ToShort());
                        dsItem12.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                        dsItem1.PushIntoStack(dsItem12);
                    }

                    dsItem1.PushIntoStack(targetProcess.IsBaoji.ToShort());
                    dsItem1.PushIntoStack(targetProcess.TrumpStatusList.Count);
                    foreach (var item in targetProcess.TrumpStatusList)
                    {
                        DataStruct dsItem13 = new DataStruct();
                        dsItem13.PushIntoStack((short)item.AbilityID);
                        dsItem13.PushIntoStack(item.Num);
                        dsItem1.PushIntoStack(dsItem13);
                    }

                    dsItem.PushIntoStack(dsItem1);
                }
                dsItem.PushIntoStack(combatProcess.TrumpStatusList.Count);
                foreach (var item in combatProcess.TrumpStatusList)
                {
                    DataStruct dsItem14 = new DataStruct();
                    dsItem14.PushIntoStack((short)item.AbilityID);
                    dsItem14.PushIntoStack(item.Num);
                    dsItem.PushIntoStack(dsItem14);
                }
                dsItem.PushIntoStack(combatProcess.FntHeadID.ToNotNullString());
                dsItem.PushIntoStack(combatProcess.AbilityID);
                PushIntoStack(dsItem);
            }

            PushIntoStack(userPlotCombat.BlessExperience);
            PushIntoStack(GotoNum);
            PushIntoStack(selfAbilityEffectList.Count);
            foreach (var selfAbilityEffect in selfAbilityEffectList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(selfAbilityEffect.GeneralID);
                dsItem.PushIntoStack(selfAbilityEffect.EffectID1.ToNotNullString());
                dsItem.PushIntoStack(selfAbilityEffect.FntHeadID.ToNotNullString());
                dsItem.PushIntoStack(selfAbilityEffect.IsIncrease ? 1.ToShort() : 0.ToShort());
                dsItem.PushIntoStack(selfAbilityEffect.Position);
                dsItem.PushIntoStack(selfAbilityEffect.Role.ToInt());
                PushIntoStack(dsItem);
            }
            PushIntoStack(_userTalPriority);
            PushIntoStack(_npcTalPriority);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotNpcID", ref PlotNpcID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {

            //if (ContextUser.EnergyNum <= 0)
            //{
            //    ErrorCode = LanguageManager.GetLang().ErrorCode;
            //    ErrorInfo = LanguageManager.GetLang().St_EnergyNotEnough;
            //    return false;
            //}
            //if (ContextUser.EnergyNum <= 0 && ContextUser.TempEnergyNum == 0)
            //{
            //    ErrorCode = LanguageManager.GetLang().ErrorCode;
            //    ErrorInfo = LanguageManager.GetLang().St_EnergyNotEnough;
            //    return false;
            //}
            if (ContextUser.UserStatus == UserStatus.SaoDang)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4007_Saodanging;
                return false;
            }
            if (ContextUser.UseMagicID == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4004_NoUseMagic;
                return false;
            }
            if (new GameDataCacheSet<UserEmbattle>().FindAll(Uid, m => m.MagicID == ContextUser.UseMagicID).Count == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4004_EmbattleEmpty;
                return false;
            }

            //if (ContextUser.TempEnergyNum == 0)
            //{
            //    ErrorCode = LanguageManager.GetLang().ErrorCode;
            //    ErrorInfo = LanguageManager.GetLang().St_EnergyNotEnough;
            //    return false;
            //}

            ContextUser.UserStatus = UserStatus.Combat;
            try
            {
                //触发战斗
                PlotNPCInfo npcInfo = new ConfigCacheSet<PlotNPCInfo>().FindKey(PlotNpcID);

                //原因：碰npc时掉线，再请求战斗详情
                if (npcInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().LoadDataError;
                    return false;
                }

                //原因：最后一个npc时，清空玩家保存副本
                if (ContextUser.UserExtend != null && ContextUser.UserExtend.MercenarySeq > npcInfo.NpcSeqNo)
                {
                    ContextUser.UserStatus = UserStatus.Normal;
                    ContextUser.UserExtend.UpdateNotify(obj =>
                    {
                        ContextUser.UserExtend.PlotStatusID = 0;
                        ContextUser.UserExtend.PlotNpcID = -1;
                        ContextUser.UserExtend.MercenarySeq = 1;
                        ContextUser.UserExtend.IsBoss = false;
                        return true;
                    });
                    //ErrorCode = LanguageManager.GetLang().ErrorCode;
                    //ErrorInfo = LanguageManager.GetLang().St4011_NoMonster;
                    //return false;
                }

                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(npcInfo.PlotID);
                if (plotInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().LoadDataError;
                    return false;
                }

                var cacheSetUserPlot = new GameDataCacheSet<UserPlotPackage>();
                var cacheSetItemInfo = new ShareCacheStruct<ItemBaseInfo>();
                var cacheSet = new GameDataCacheSet<UserPlotCombat>();
                var userPlotPack = cacheSetUserPlot.FindKey(ContextUser.UserID);
                var userPlot = userPlotPack != null ? userPlotPack.PlotPackage.Find(s => s.PlotID == npcInfo.PlotID) : null;
                //PlotHelper.IsKill(ContextUser.UserID, plotInfo.PlotID, plotInfo.CityID)
                if (userPlot != null && userPlot.PlotNum >= plotInfo.ChallengeNum)
                {
                    if (plotInfo.PlotType == PlotType.Elite)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St4002_EliteUsed;
                        return false;
                    }
                    else if (plotInfo.PlotType == PlotType.HeroPlot)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St4002_HeroPlotNum;
                        return false;
                    }
                }

                //if (userPlot != null && userPlot.ItemList != null)
                //{
                //    userPlot.UpdateNotify(obj =>
                //    {
                //        userPlot.ItemList.Clear();
                //        return true;
                //    });
                //}
                if (npcInfo.NpcSeqNo == 1)
                {
                    userPlot.ItemList.Clear();
                    ContextUser.IsItem = false;
                }
                userPlotCombat = cacheSet.FindKey(ContextUser.UserID, PlotNpcID);
                if (userPlotCombat != null && userPlotCombat.IsWin && ContextUser.UserExtend != null &&
                     PlotNpcID == ContextUser.UserExtend.PlotNpcID && !ContextUser.IsItem)
                {
                    prizeItems = PlotHelper.GetPlotMonsterItems(Uid, npcInfo.PlotNpcID);
                }
                else
                {
                    ISingleCombat plotCombater = CombatFactory.TriggerPlot(ContextUser, PlotNpcID);
                    if (plotCombater == null)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().LoadDataError;
                        return false;
                    }
                    if (userPlotCombat == null)
                    {
                        userPlotCombat = new UserPlotCombat()
                                             {
                                                 UserID = ContextUser.UserID,
                                                 PlotNpcID = PlotNpcID
                                             };
                        cacheSet.Add(userPlotCombat);
                        userPlotCombat = cacheSet.FindKey(ContextUser.UserID, PlotNpcID);
                    }
                    userPlotCombat.PlotID = npcInfo.PlotID;
                    userPlotCombat.CombatDate = DateTime.Now;
                    userPlotCombat.IsWin = plotCombater.Doing();
                    if (!userPlotCombat.IsWin)
                    {
                        userPlotCombat.OverNum += 1;
                    }
                    ContextUser.IsItem = npcInfo.NpcSeqNo == 1 ? false : ContextUser.IsItem;
                    combatProcessList = (CombatProcessContainer)plotCombater.GetProcessResult();
                    userPlotCombat.GeneralNum = combatProcessList.AttackList.Count;
                    userPlotCombat.GeneralOverNum = GetOverGeneralNum(combatProcessList.AttackList.ToList());
                    //userPlotCombat.CombatProcess = JsonUtils.Serialize(combatProcessList);
                    int generalNum = combatProcessList.DefenseList.FindAll(s => s.LiveNum > 0).Count;
                    if (userPlotCombat.IsWin)
                    {
                        if (ContextUser.UserExtend != null)
                        {
                            ContextUser.UserExtend.UpdateNotify(obj =>
                            {
                                ContextUser.UserExtend.PlotStatusID = npcInfo.PlotID;
                                ContextUser.UserExtend.PlotNpcID = PlotNpcID;
                                ContextUser.UserExtend.MercenarySeq = MathUtils.Addition(npcInfo.NpcSeqNo, (short)1);
                                ContextUser.UserExtend.IsBoss = npcInfo.IsBoss; //是否最后一个副本npc
                                return true;
                            });
                        }
                        //玩家通过一个城市的所有副本时，增加聊天频道系统提示
                        if (userPlot.PlotStatus != PlotStatus.Completed && npcInfo.IsBoss)      //玩家此副本胜利
                        {
                            var city = new ConfigCacheSet<CityInfo>().FindKey(plotInfo.CityID);
                            var nextPlot = new ConfigCacheSet<PlotInfo>().FindKey(plotInfo.AftPlotID);
                            if (city != null && nextPlot != null && nextPlot.CityID != plotInfo.CityID)            //此城市的最后一个副本
                            {
                                string content = string.Format(LanguageManager.GetLang().St_systemprompts, ContextUser.NickName,
                                                        city.CityName);
                                new TjxChatService().SystemSend(ChatType.World, content);
                            }
                        }
                        PlotHelper.DoPlotPrize(Uid, npcInfo, userPlotCombat, generalNum, out _honourNum);
                        if (npcInfo.NpcSeqNo == 1)
                        {
                            ContextUser.EnergyNum = MathUtils.Subtraction(ContextUser.EnergyNum, ContextUser.TempEnergyNum).ToShort();
                            //原因:刷晶石BUG
                            ContextUser.TempEnergyNum = 0;

                        }
                        if (!ContextUser.IsItem)
                        {
                            prizeItems = PlotHelper.GetPlotMonsterItems(Uid, npcInfo.PlotNpcID);
                        }
                        TaskHelper.KillPlotMonster(Uid, npcInfo.PlotID, PlotNpcID);

                        //var stroyTaskList = new ConfigCacheSet<StoryTaskInfo>().FindAll(s => s.PlotID == plotInfo.AftPlotID);
                        //foreach (var story in stroyTaskList)
                        //{
                        //    var usertask = new GameDataCacheSet<UserTask>().FindKey(ContextUser.UserID, story.PlotID);
                        //    if (usertask != null)
                        //    {
                        PlotHelper.EnablePlot(Uid, plotInfo.AftPlotID);
                        //    }
                        //}

                        if (plotInfo.PlotType == PlotType.Elite)
                        {
                            EliteDailyRestrain(npcInfo);
                            NoviceHelper.ElitePlotFestivalList(ContextUser); //通关精英副本获得奖励
                        }
                        else if (plotInfo.PlotType == PlotType.Kalpa)
                        {
                            KalpaDailyRestrain(npcInfo);
                        }
                        else if (plotInfo.PlotType == PlotType.HeroPlot)
                        {
                            PlotHelper.EnableHeroPlot(ContextUser.UserID, plotInfo.PlotID);
                            PlotHelper.HeroDailyRestrain(ContextUser.UserID, plotInfo.PlotID, plotInfo.CityID);
                        }

                    }
                    else
                    {

                        ContextUser.GeneralAllCount = 0;
                        ContextUser.GeneralKillCount = 0;
                    }


                    var restrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid);
                    if (restrain != null)
                    {
                        var restrainSet = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.PlotGoto);
                        if (restrainSet != null)
                            GotoNum = MathUtils.Subtraction(restrainSet.MaxNum, restrain.Funtion14, 0);
                    }
                }
                //原因：最后一个npc时，清空玩家保存副本
                if (ContextUser.UserExtend != null && ContextUser.UserExtend.IsBoss)
                {
                    ContextUser.UserExtend.UpdateNotify(obj =>
                    {
                        ContextUser.UserExtend.PlotStatusID = 0;
                        ContextUser.UserExtend.PlotNpcID = -1;
                        ContextUser.UserExtend.MercenarySeq = 0;
                        ContextUser.UserExtend.IsBoss = npcInfo.IsBoss;
                        return true;
                    });
                }

                if (!ContextUser.IsItem)
                {
                    foreach (var prize in prizeItems)
                    {
                        if (prize.Type == 0 && userPlot != null)
                        {
                            var itemInfo = cacheSetItemInfo.FindKey(prize.ItemID);
                            UniversalInfo universalInfo = new UniversalInfo();
                            universalInfo.HeadID = itemInfo != null ? itemInfo.HeadID : string.Empty;
                            universalInfo.ItemID = itemInfo != null ? itemInfo.ItemID : 0;
                            universalInfo.ItemDesc = itemInfo != null ? itemInfo.ItemDesc : string.Empty;
                            universalInfo.MaxHeadID = itemInfo != null ? itemInfo.MaxHeadID : string.Empty;
                            universalInfo.Name = itemInfo != null ? itemInfo.ItemName : string.Empty;
                            universalInfo.Num = prize.Num;
                            if (userPlot.ItemList != null && userPlot.ItemList.Count > 0)
                            {
                                var item = userPlot.ItemList.Find(s => s.ItemID == prize.ItemID);
                                if (item != null)
                                {
                                    item.UpdateNotify(obj =>
                                    {
                                        item.Num = MathUtils.Addition(item.Num, prize.Num);
                                        return true;
                                    });
                                }
                                else
                                {
                                    userPlot.UpdateNotify(obj =>
                                    {
                                        userPlot.ItemList.Add(universalInfo);
                                        return true;
                                    });
                                }
                            }
                            else
                            {
                                userPlot.UpdateNotify(obj =>
                                {
                                    userPlot.ItemList.Add(universalInfo);
                                    return true;
                                });
                            }

                        }
                        ContextUser.IsItem = true;
                    }
                }
                var a = userPlot.ItemList;
            }
            finally
            {
                ContextUser.UserStatus = UserStatus.Normal;
            }
            //日志
            UserCombatLog log = new UserCombatLog();
            log.CombatLogID = Guid.NewGuid().ToString("N");
            log.UserID = userPlotCombat.UserID;
            log.CityID = ContextUser.CityID;
            log.PlotID = userPlotCombat.PlotID;
            log.NpcID = userPlotCombat.PlotNpcID;
            log.CombatType = CombatType.Plot;
            log.HostileUser = string.Empty;
            log.IsWin = userPlotCombat.IsWin;
            log.CombatProcess = JsonUtils.Serialize(combatProcessList);
            log.PrizeItem = prizeItems;
            log.CreateDate = DateTime.Now;
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);

            UserHelper.GetGeneralLife(ContextUser.UserID);
            UserHelper.RegainGeneralLife(ContextUser.UserID);

            //var userEmbattleList = EmbattleHelper.CurrEmbattle(ContextUser.UserID, true);
            //           foreach (var userEmbattle in userEmbattleList)
            //           {
            //               _userTalPriority = MathUtils.Addition(_userTalPriority, PriorityHelper.GeneralTotalPriority(ContextUser.UserID, userEmbattle.GeneralID));
            //           }
            selfAbilityEffectList = UserAbilityHelper.GetSelfAbilityEffectList(ContextUser.UserID, 0);
            //var userEmbattleList = EmbattleHelper.CurrEmbattle(ContextUser.UserID, true);

            _userTalPriority = CombatHelper.TotalPriorityNum(ContextUser.UserID, 0);

            return true;
        }

        /// <summary>
        /// 精英副本挑战次数
        /// </summary>
        /// <param name="plotNPCInfo"></param>
        private void EliteDailyRestrain(PlotNPCInfo plotNPCInfo)
        {
            UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid);
            if (userRestrain == null)
            {
                userRestrain = new UserDailyRestrain() { UserID = Uid, FunPlot = new CacheList<FunPlot>() };
            }

            int plotNum = 0;
            FunPlot funPlot = null;
            if (userRestrain.FunPlot.Count > 0)
            {
                funPlot = userRestrain.FunPlot.Find(m => m.PlotID == plotNPCInfo.PlotID);
            }
            if (funPlot == null)
            {
                plotNum = 1;
            }
            else
            {
                plotNum = MathUtils.Addition(funPlot.Num, 1, int.MaxValue);
                userRestrain.FunPlot.Remove(funPlot);
            }
            funPlot = new FunPlot
                          {
                              PlotID = plotNPCInfo.PlotID,
                              Num = plotNum
                          };
            userRestrain.FunPlot.Add(funPlot);

        }

        /// <summary>
        /// 天地劫挑战次数
        /// </summary>
        /// <param name="plotNPCInfo"></param>
        private void KalpaDailyRestrain(PlotNPCInfo plotNPCInfo)
        {
            UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid);
            if (userRestrain == null)
            {
                userRestrain = new UserDailyRestrain() { UserID = Uid, FunPlot = new CacheList<FunPlot>() };
            }
            if (userRestrain.UserExtend == null)
            {
                userRestrain.UserExtend = new DailyUserExtend();
                userRestrain.UserExtend.KalpaPlot = new CacheList<FunPlot>();
            }
            int plotNum = 0;
            FunPlot funPlot = userRestrain.UserExtend.KalpaPlot.Find(m => m.PlotID == plotNPCInfo.PlotID);
            if (funPlot == null)
            {
                plotNum = 1;
            }
            else
            {
                plotNum = MathUtils.Addition(funPlot.Num, 1, int.MaxValue);
                userRestrain.UserExtend.UpdateNotify(obj =>
                    {
                        userRestrain.UserExtend.KalpaPlot.Remove(funPlot);
                        return true;
                    });
            }
            funPlot = new FunPlot
            {
                PlotID = plotNPCInfo.PlotID,
                Num = plotNum
            };
            userRestrain.UserExtend.UpdateNotify(obj =>
                {
                    userRestrain.UserExtend.KalpaPlot.Add(funPlot);
                    return true;
                });

        }


        private int GetOverGeneralNum(List<CombatEmbattle> userEmbattleList)
        {
            int num = 0;
            foreach (CombatEmbattle item in userEmbattleList)
            {
                if (!string.IsNullOrEmpty(item.UserID))
                {
                    var general = UserGeneral.GetMainGeneral(item.UserID);
                    if (general != null && general.IsOver)
                    {
                        num += 1;
                    }
                }
            }
            return num;
        }
    }
}