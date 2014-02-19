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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Combat;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Component;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 4303_天地劫副本战斗详情接口
    /// </summary>
    public class Action4303 : BaseAction
    {
        private int PlotNpcID;
        private int ops;
        private UserPlotCombat userPlotCombat;
        private CacheList<PrizeItemInfo> prizeItems = new CacheList<PrizeItemInfo>();
        private CombatProcessContainer combatProcessList = new CombatProcessContainer();
        private UniversalInfo[] universalArray = new UniversalInfo[0];

        public Action4303(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4303, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(userPlotCombat.IsWin.ToShort());
            PushIntoStack(userPlotCombat.Experience);
            PushIntoStack(prizeItems.Count);
            foreach (PrizeItemInfo prizeItem in prizeItems)
            {
                SparePartInfo partInfo = new ConfigCacheSet<SparePartInfo>().FindKey(prizeItem.ItemID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(partInfo == null ? string.Empty : partInfo.Name.ToNotNullString());
                dsItem.PushIntoStack(partInfo == null ? string.Empty : partInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(partInfo == null ? (short)0 : partInfo.QualityType.ToShort());
                dsItem.PushIntoStack(prizeItem.Num);

                PushIntoStack(dsItem);
            }
            //攻方阵形
            PushIntoStack(combatProcessList.AttackList.Count);
            foreach (CombatEmbattle combatEmbattle in combatProcessList.AttackList)
            {
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
                dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                PushIntoStack(dsItem);
            }
            //防方阵形
            PushIntoStack(combatProcessList.DefenseList.Count);
            foreach (CombatEmbattle combatEmbattle in combatProcessList.DefenseList)
            {
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
                dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
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
                PushIntoStack(dsItem);
            }
            PushIntoStack(universalArray.Length);
            foreach (var item in universalArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.Name.ToNotNullString());
                dsItem.PushIntoStack(item.HeadID.ToNotNullString());
                dsItem.PushIntoStack(item.Num);
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotNpcID", ref PlotNpcID))
            {
                httpGet.GetInt("Ops", ref ops);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cacheEnvSet = new ConfigCacheSet<ConfigEnvSet>();
            var envset = cacheEnvSet.FindKey("CombatMaxBout");
            if (envset != null)
            {

            }
            PlotNPCInfo npcInfo = new ConfigCacheSet<PlotNPCInfo>().FindKey(PlotNpcID);
            if (npcInfo == null)
            {
                return false;
            }
            PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(npcInfo.PlotID);
            if (plotInfo == null)
            {
                return false;
            }
            //判断是否有下一关
            int nextLayerNum = MathUtils.Addition(ContextUser.UserExtend.LayerNum, 1);
            int nextHurdleNum = MathUtils.Addition(ContextUser.UserExtend.HurdleNum, 1);
            if ((UserHelper.IsLastLayer(plotInfo) || !IsGotoNextLayer(nextLayerNum)))
            {
                if (UserHelper.IsKill(ContextUser, npcInfo.PlotID))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St4303_PlotNotEnableLayerNum;
                    return false;
                }
            }

            if (ops != 1 && UserHelper.IsPromptBlood(ContextUser.UserID))
            {
                ErrorCode = 1;
                ErrorInfo = LanguageManager.GetLang().St4002_PromptBlood;
                return false;
            }
            //原因：灵件背包满时未提示
            if (ops != 1 && UserHelper.IsSpareGridNum(ContextUser, 0))
            {
                ErrorCode = 1;
                ErrorInfo = LanguageManager.GetLang().St_User_SpareBeiBaoMsg;
                return false;
            }

            if (ops != 1 && !string.IsNullOrEmpty(plotInfo.EnchantID) && plotInfo.EnchantProbability > 0 && EnchantHelper.IsEnchantPackage(ContextUser.UserID))
            {
                ErrorCode = 1;
                ErrorInfo = LanguageManager.GetLang().St4002_EnchantPackageFull;
                return false;
            }

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
            //PlotNPCInfo npcInfo = new ConfigCacheSet<PlotNPCInfo>().FindKey(PlotNpcID);
            if (IsPlotOut(npcInfo))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4303_PlotNotEnable;
                return false;
            }

            UserHelper.UserGeneralPromptBlood(ContextUser);//佣兵自动使用绷带补血

            //触发战斗
            ISingleCombat plotCombater = CombatFactory.TriggerPlot(ContextUser, PlotNpcID);
            if (plotCombater == null)
            {
                return false;
            }
            var cacheSet = new GameDataCacheSet<UserPlotCombat>();
            userPlotCombat = cacheSet.FindKey(ContextUser.UserID, PlotNpcID);
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
            combatProcessList = (CombatProcessContainer)plotCombater.GetProcessResult();
            userPlotCombat.GeneralNum = combatProcessList.AttackList.Count;
            userPlotCombat.GeneralOverNum = GetOverGeneralNum(combatProcessList.AttackList.ToList());

            if (userPlotCombat.IsWin)
            {
                int honourNum = 0;
                PlotHelper.DoPlotPrize(Uid, npcInfo, userPlotCombat, 0, out honourNum);
                prizeItems = PlotHelper.GetKalpaPlotMonsterItems(Uid, npcInfo.PlotID, npcInfo.PlotNpcID);
                universalArray = GetUniversalList().ToArray();
                if (plotInfo.PlotType == PlotType.Kalpa)
                {

                    KalpaDailyRestrain(npcInfo);
                    if (ContextUser.UserExtend == null)
                    {
                        ContextUser.UserExtend = new GameUserExtend();
                    }

                    PlotInfo[] plotInfoList = new ConfigCacheSet<PlotInfo>().FindAll(m => m.LayerNum == ContextUser.UserExtend.LayerNum && m.PlotSeqNo == nextHurdleNum && m.PlotType == PlotType.Kalpa).ToArray();
                    if (plotInfoList.Length > 0)
                    {
                        int layerNum = ContextUser.UserExtend.LayerNum;
                        int hurdleNum = ContextUser.UserExtend.HurdleNum;

                        if (CheckHurdleNum(ContextUser.UserID, layerNum, hurdleNum))
                        {
                            ContextUser.UserExtend.UpdateNotify(obj =>
                                {
                                    ContextUser.UserExtend.HurdleNum = nextHurdleNum;
                                    return true;
                                });
                            //ContextUser.Update();
                            PlotHelper.EnablePlot(ContextUser.UserID, plotInfo.AftPlotID);
                        }
                    }
                    else if (IsGotoNextLayer(nextLayerNum))
                    {
                        //判断是否能到下一层
                        plotInfoList = new ConfigCacheSet<PlotInfo>().FindAll(m => m.LayerNum == nextLayerNum && m.PlotType == PlotType.Kalpa).ToArray();
                        if (plotInfoList.Length > 0)
                        {
                            ContextUser.UserExtend.UpdateNotify(obj =>
                            {
                                ContextUser.UserExtend.LayerNum = nextLayerNum;
                                ContextUser.UserExtend.HurdleNum = 1;
                                return true;
                            });
                            //ContextUser.Update();
                            PlotHelper.EnablePlot(ContextUser.UserID, plotInfo.AftPlotID);
                        }
                    }
                }
            }


            //日志
            UserCombatLog log = new UserCombatLog();
            log.CombatLogID = Guid.NewGuid().ToString();
            log.UserID = userPlotCombat.UserID;
            log.CityID = ContextUser.CityID;
            log.PlotID = userPlotCombat.PlotID;
            log.NpcID = userPlotCombat.PlotNpcID;
            log.CombatType = CombatType.Kalpa;
            log.HostileUser = string.Empty;
            log.IsWin = userPlotCombat.IsWin;
            log.CombatProcess = JsonUtils.Serialize(combatProcessList);
            log.PrizeItem = prizeItems;
            log.CreateDate = DateTime.Now;
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);

            UserHelper.GetGeneralLife(ContextUser.UserID);

            return true;
        }

        private bool CheckHurdleNum(string userID, int layerNum, int hurdleNum)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.UserExtend.KalpaPlot.Count > 0)
            {
                List<FunPlot> funPlotArray = dailyRestrain.UserExtend.KalpaPlot.ToList();
                var plotInfosArray = new ConfigCacheSet<PlotInfo>().FindAll(u => u.PlotSeqNo == hurdleNum && u.LayerNum == layerNum && u.PlotType == PlotType.Kalpa);
                if (plotInfosArray.Count > 0)
                {
                    foreach (FunPlot plot in funPlotArray)
                    {
                        if (plot.PlotID == plotInfosArray[0].PlotID)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        private bool IsPlotOut(PlotNPCInfo npcInfo)
        {
            if (npcInfo == null) return true;
            var userPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, npcInfo.PlotID);
            if (userPlot != null && UserHelper.IsKill(ContextUser, userPlot.PlotID))
            {
                int layerNum = ContextUser.UserExtend.LayerNum;
                int nextLayerNum = MathUtils.Addition(layerNum, 1);
                if (new ConfigCacheSet<PlotInfo>().FindAll(m => m.LayerNum == nextLayerNum && m.PlotType == PlotType.Kalpa).Count == 0)
                {
                    int nextHurdleNum = MathUtils.Addition(ContextUser.UserExtend.HurdleNum, 1);
                    //是否有下一关
                    return new ConfigCacheSet<PlotInfo>().FindAll(m => m.LayerNum == layerNum && m.PlotSeqNo == nextHurdleNum && m.PlotType == PlotType.Kalpa).Count == 0;
                }
            }
            return false;
        }

        private bool IsGotoNextLayer(int nextLayerNum)
        {
            string[] tempList = ConfigEnvSet.GetString("Plot.LayerNumLv").ToNotNullString().Split(',');
            foreach (var temp in tempList)
            {
                if (temp == null) continue;
                string[] lvList = temp.Split('=');
                if (lvList.Length == 2)
                {
                    int layerNum = lvList[0].ToInt();
                    if (layerNum != nextLayerNum) continue;
                    return ContextUser.UserLv >= lvList[1].ToShort();
                }
            }
            return false;
        }

        /// <summary>
        /// 天地劫挑战次数
        /// </summary>
        /// <param name="plotNPCInfo"></param>
        private void KalpaDailyRestrain(PlotNPCInfo plotNPCInfo)
        {
            var cacheSet = new GameDataCacheSet<UserDailyRestrain>();
            UserDailyRestrain userRestrain = cacheSet.FindKey(Uid);
            if (userRestrain == null)
            {
                userRestrain = new UserDailyRestrain() { UserID = Uid };
                cacheSet.Add(userRestrain);
                userRestrain = cacheSet.FindKey(Uid);
            }
            if (userRestrain.UserExtend == null)
            {
                userRestrain.UserExtend = new DailyUserExtend();
                //userRestrain.UserExtend.KalpaPlot = new List<FunPlot>();
            }
            int plotNum = 0;
            FunPlot funPlot = userRestrain.UserExtend.KalpaPlot.Find(m => m.PlotID == plotNPCInfo.PlotID);
            if (funPlot == null)
            {
                plotNum = 1;
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

        /// <summary>
        /// 通关获得的物品奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="npcInfo"></param>
        /// <returns></returns>
        private List<UniversalInfo> GetUniversalList()
        {
            List<UniversalInfo> universalList = new List<UniversalInfo>();
            foreach (PrizeItemInfo info in prizeItems)
            {
                if (info.Type == 0)
                {
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(info.ItemID);
                    if (itemInfo != null)
                    {
                        universalList.Add(new UniversalInfo() { Name = itemInfo.ItemName, HeadID = itemInfo.HeadID, Num = info.Num });
                    }
                }
                else if (info.Type == 1)
                {
                    SparePartInfo sparePart = new ConfigCacheSet<SparePartInfo>().FindKey(info.ItemID);
                    if (sparePart != null)
                    {
                        universalList.Add(new UniversalInfo() { Name = sparePart.Name, HeadID = sparePart.HeadID, Num = info.Num });
                    }
                }
                else if (info.Type == 2)
                {
                    EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(info.ItemID);
                    if (enchantInfo != null)
                    {
                        universalList.Add(new UniversalInfo() { Name = enchantInfo.EnchantName, HeadID = enchantInfo.HeadID, Num = info.Num });
                        PlotHelper.EnchantAddUser(ContextUser, enchantInfo.EnchantID);
                    }
                }
            }
            return universalList;
        }
    }
}