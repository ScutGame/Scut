import clr, sys
import random
import time
import datetime
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.BLL.Combat');

from lang import Lang
from festivalBll import * 
from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.BLL.Combat import *
from ZyGames.Tianjiexing.Model.Enum import *

# 12053_考古系统战斗接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.monsterID = 0;
        
class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self._cacheSetGeneral = ConfigCacheSet[GeneralInfo]();
        self.combatProcessList = None;
        self.honourNum = 0;
        self._userTalPriority = 0;
        self._npcTalPriority = 0;
        self.isWin = False;
        self.bgScene = '';
        self.plotNpcID = 0;
        self.gameCoin = 0;
        self.gold = 0;
        self.agentNum = 0;
        self.dropReward = '';
        self.starNum = 0;
        self.isBoss = False;
        self.generalNum = 0;
        self.monsterPosition = 0;
        self.maxHonourNum = 0;
        self.plotName = '';
        self.currentHonourNum = 0;
        self.lastMaxHonourNum = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("PlotNpcID") and httpGet.Contains("MonsterPosition"):
        urlParam.plotNpcID = httpGet.GetIntValue("PlotNpcID");
        urlParam.monsterPosition = httpGet.GetIntValue("MonsterPosition");
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    # 加载数据出错
    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    plotNpcID = urlParam.plotNpcID;

    plotNpcInfo = ConfigCacheSet[PlotNPCInfo]().FindKey(plotNpcID);
    userPlotPackage = GameDataCacheSet[UserPlotPackage]().FindKey(userId);
    if not plotNpcInfo or not userPlotPackage:
        return loadError();

    plotID = plotNpcInfo.PlotID;
    # 精力不足，请等待精力恢复后继续战斗！
    if contextUser.EnergyNum < 1:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St12053_EnergyNotEnough");
        actionResult.Result = False;
        return actionResult;

    # 获取战斗场景
    tempPlotInfo = ConfigCacheSet[PlotInfo]().FindKey(plotID);
    if not tempPlotInfo:
        return loadError();
    actionResult.bgScene = tempPlotInfo.BgScene;
    actionResult.plotName = tempPlotInfo.PlotName;
    # 获取 UserPlotInfo
    plotInfo =  userPlotPackage.PlotPackage.Find(match=lambda x:x.PlotID == plotID);
    if not plotInfo:
        return loadError();

    monsterInfo = None;
    if plotNpcInfo.IsBoss == True:
        #monsterInfo = plotInfo.ArcheologyPackage.Find(match=lambda x:x.PlotNpcID == plotNpcID );
        #if not monsterInfo:
        #    return loadError();
        # Boss 挑战次数是否已用完
        if plotInfo.BossChallengeCount < 1:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St12053_BuyOneChallenge");
            actionResult.Result = False;
            return actionResult;
    else:  # 如果是怪物，判断是否已经集齐碎片，不再触发战斗
        # 获取怪物 UserPlotArcheology
       
        monsterInfo = plotInfo.ArcheologyPackage.Find(match=lambda x:x.PlotNpcID == plotNpcID and x.Position == urlParam.monsterPosition);
        if not monsterInfo:
            return loadError();
        if monsterInfo.LightMapCount >= monsterInfo.Quality:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St12053_HasEnoughMapCount");
            actionResult.Result = False;
            return actionResult;

    # 战斗
    plotCombater = CombatFactory.TriggerArchaeologyPlot(contextUser, plotNpcID);
    if (plotCombater == None):
        return loadError();
    actionResult.isWin = plotCombater.Doing();
    # 精力值减 1
    contextUser.EnergyNum = MathUtils.Subtraction(contextUser.EnergyNum,1);

    if(actionResult.isWin == True):

        cacheSetGeneral = GameDataCacheSet[UserGeneral]();
        userMagic = GameDataCacheSet[UserMagic]().Find(userId, lambda s:s.IsEnabled);
        userMagicID = 0 if userMagic == None else userMagic.MagicID;
        userEmbattleList = GameDataCacheSet[UserEmbattle]().FindAll(userId, lambda s:s.MagicID == userMagicID and s.GeneralID > 0);
        _festivalBll = FestivalBll()
        _festivalBll.UpdateArcheologyRestrain(contextUser,plotNpcInfo.PlotNpcID)
        def method(userEmbattle):
            userGeneral = cacheSetGeneral.FindKey(userId, userEmbattle.GeneralID);
            if userGeneral != None and userGeneral.LifeNum > 0:
                actionResult.generalNum = MathUtils.Addition(actionResult.generalNum, 1);
                
        userEmbattleList.ForEach(lambda userEmbattle:method(userEmbattle));
        starNum = PlotHelper.GetStar(contextUser, actionResult.generalNum);  # 星级评价
        if not starNum:
            return loadError();
        actionResult.starNum = starNum[0];

        #代表打赢副本处理相应业务
        tempList = []
        monsterList = contextUser.UserExtend.MonsterList;
        if monsterList:
            arrayList = monsterList.split(",");
            for item in arrayList:
                tempList.append(item);
        else:
            contextUser.UserExtend.MonsterList = ','
        tempPlotNpcID = str(plotNpcID)

        if plotNpcInfo.IsBoss == True:
            plotInfo.HasChallengeBossWin = True; # Boss 挑战胜利
            # 掉落物品(目前只能掉落一种)
            actionResult.isBoss = True;
            actionResult.dropReward = UserPrayHelper.GetUserTake(plotNpcInfo.BoxReward.ToList(),userId,1);

            if tempPlotNpcID not in tempList: # 首胜
                actionResult.gameCoin = plotNpcInfo.GameCoin  # 获取金币
                contextUser.GameCoin = MathUtils.Addition(contextUser.GameCoin, plotNpcInfo.GameCoin);
                actionResult.gold = plotNpcInfo.Gold   # 晶石
                contextUser.ItemGold = MathUtils.Addition(contextUser.ItemGold, plotNpcInfo.Gold);
                contextUser.UserExtend.MonsterList = contextUser.UserExtend.MonsterList + tempPlotNpcID + ",";

            # 荣誉值
            actionResult.honourNum = plotNpcInfo.HonourNum;
            contextUser.HonourNum = MathUtils.Addition(contextUser.HonourNum, plotNpcInfo.HonourNum);
            # UserPlotPackage 中的 PlotNpcID 置为 BehindNpcID
            nextBoss = ConfigCacheSet[PlotNPCInfo]().FindKey(plotNpcInfo.BehindNpcID)
            if nextBoss:
                plotInfo.CurrPlotNpcID = plotNpcInfo.BehindNpcID;
                plotInfo.BossChallengeCount = nextBoss.ChallengeNum;
                plotInfo.HasChallengeBossWin = False; # Boss 挑战

            

            # 如果打赢 Boss,开启下一个地图
            afterPlotID = tempPlotInfo.AftPlotID;
            
            if plotInfo.HasChallengeBossWin and afterPlotID != 0:
                hasPlotInfo = userPlotPackage.PlotPackage.Find(match=lambda x:x.PlotID == afterPlotID);
                if not hasPlotInfo:
                    userPlotInfo = UserPlotInfo()
                    userPlotInfo.PlotID = afterPlotID;
                    nextPlot = ConfigCacheSet[PlotInfo]().FindKey(afterPlotID);
                    if nextPlot:
                        userPlotInfo.BossChallengeCount = nextPlot.ChallengeNum;
                        userPlotPackage.PlotPackage.Add(userPlotInfo);
            
        else:
            if tempPlotNpcID not in tempList:  # 首胜
                actionResult.gameCoin = plotNpcInfo.GameCoin  # 获取金币
                contextUser.GameCoin = MathUtils.Addition(contextUser.GameCoin, plotNpcInfo.GameCoin);
                actionResult.agentNum = plotNpcInfo.AgentNum  # 培养丹
                UserItemHelper.AddUserItem(userId, 1225, plotNpcInfo.AgentNum, MathUtils.ToShort(1));
                
                contextUser.UserExtend.MonsterList = contextUser.UserExtend.MonsterList + tempPlotNpcID + ",";

            # 九宫格怪物掉落
            actionResult.dropReward = UserPrayHelper.GetUserTake(plotNpcInfo.BoxReward.ToList(),userId,1);
            # 荣誉值
            actionResult.honourNum = plotNpcInfo.HonourNum;
            contextUser.HonourNum = MathUtils.Addition(contextUser.HonourNum, plotNpcInfo.HonourNum);
            # HasChallenged 置为 True
            # 怪物的碎片加1，碎片总数加1
            monsterInfo.LightMapCount += 1;
            plotInfo.HasMapCount += 1;

            # 翻起周围的怪物卡牌，如果有宝箱则先开启宝箱
            #monsterInfo = plotInfo.ArcheologyPackage.Find(match=lambda x:x.PlotNpcID == plotNpcID );
            currMonsterPosition = monsterInfo.Position
            tempList = []
            if currMonsterPosition != 4 and currMonsterPosition != 7:
                tempList.append(currMonsterPosition - 1);
            if currMonsterPosition != 3 and currMonsterPosition != 6:
                tempList.append(currMonsterPosition + 1);
            tempList.append(currMonsterPosition - 3);
            tempList.append(currMonsterPosition + 3);
            tempList = [i for i in tempList if i > 0]
            for i in tempList:
                tempBoxInfo = plotInfo.ArcheologyPackage.Find(match=lambda x:x.Position == i and x.Quality == 0);
                if tempBoxInfo:
                    tempBoxInfo.IsOpen = True;
                else:
                    mosnterInfo = plotInfo.ArcheologyPackage.Find(match=lambda x:x.Position == i);
                    if mosnterInfo:
                        mosnterInfo.IsOpen = True;


    else:
        #代表打输副本处理相应业务
        if plotNpcInfo.IsBoss == True:
            plotInfo.BossChallengeCount -= 1;
        else:
            # 九宫格怪物战斗失败暂未做任何处理
            pass
    
    actionResult.currentHonourNum = contextUser.HonourNum

    # 玩家加血
    UserHelper.GetGeneralLife(userId);
    UserHelper.RegainGeneralLife(userId);

    actionResult.combatProcessList = plotCombater.GetProcessResult();
    userEmbattleList = EmbattleHelper.CurrEmbattle(userId, True);
    for userEmbattle in userEmbattleList:
        actionResult._userTalPriority = MathUtils.Addition(actionResult._userTalPriority, PriorityHelper.GeneralTotalPriority(userId, userEmbattle.GeneralID));
    # 获取玩家最大荣誉值
    cacheSetGeneralEscalate = ConfigCacheSet[GeneralEscalateInfo]();
    GeneralEscalateHelper.AddUserLv(contextUser, 0);
    lv = contextUser.UserLv;
    lv = 1 if lv < 0 else lv + 1
    generalEscalate = cacheSetGeneralEscalate.Find(match=lambda s:s.GeneralType == GeneralType.YongHu and s.GeneralLv == lv);
    if generalEscalate:
        actionResult.maxHonourNum = generalEscalate.UpExperience;
    lastGeneralEscalate = cacheSetGeneralEscalate.Find(match=lambda s:s.GeneralType == GeneralType.YongHu and s.GeneralLv == (lv-1));
    if lastGeneralEscalate:
        actionResult.lastMaxHonourNum = lastGeneralEscalate.UpExperience;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushShortIntoStack(MathUtils.ToShort(actionResult.isWin));
    writer.PushIntoStack(actionResult.bgScene);
    combatProcessList = actionResult.combatProcessList;
    # 攻方阵形
    writer.PushIntoStack(combatProcessList.AttackList.Count);
    for combatEmbattle in combatProcessList.AttackList:
        isAttWait = 1 if combatEmbattle.IsWait else 0;
        general = actionResult._cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
        dsItem = DataStruct();
        dsItem.PushIntoStack(combatEmbattle.GeneralID);
        dsItem.PushIntoStack(MathUtils.ToNotNullString(combatEmbattle.GeneralName));
        # dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
        dsItem.PushIntoStack(MathUtils.ToNotNullString(general.BattleHeadID) if general else '');
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatEmbattle.Position));
        dsItem.PushIntoStack(combatEmbattle.LiveNum);
        dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
        dsItem.PushShortIntoStack(combatEmbattle.MomentumNum);
        dsItem.PushShortIntoStack(combatEmbattle.MaxMomentumNum);
        dsItem.PushIntoStack(combatEmbattle.AbilityID);
        dsItem.PushShortIntoStack(combatEmbattle.GeneralLv);
        dsItem.PushShortIntoStack(MathUtils.ToShort(isAttWait));
        dsItem.PushShortIntoStack(MathUtils.ToShort(general.GeneralQuality) if general else 0);

        writer.PushIntoStack(dsItem);

    # 防方阵形
    writer.PushIntoStack(combatProcessList.DefenseList.Count);
    for combatEmbattle in combatProcessList.DefenseList:
        isDefWait = 1 if combatEmbattle.IsWait else 0;
        dsItem = DataStruct();
        dsItem.PushIntoStack(combatEmbattle.GeneralID);
        dsItem.PushIntoStack(MathUtils.ToNotNullString(combatEmbattle.GeneralName));
        dsItem.PushIntoStack(MathUtils.ToNotNullString(combatEmbattle.HeadID));
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatEmbattle.Position));
        dsItem.PushIntoStack(combatEmbattle.LiveNum);
        dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
        dsItem.PushShortIntoStack(combatEmbattle.MomentumNum);
        dsItem.PushShortIntoStack(combatEmbattle.MaxMomentumNum);
        dsItem.PushIntoStack(combatEmbattle.AbilityID);
        dsItem.PushShortIntoStack(combatEmbattle.GeneralLv);
        dsItem.PushShortIntoStack(MathUtils.ToShort(isDefWait));
        # 增加佣兵品质
        general = actionResult._cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
        dsItem.PushShortIntoStack(MathUtils.ToShort(general.GeneralQuality) if general else 0);
        writer.PushIntoStack(dsItem);

    # 战斗过程
    writer.PushIntoStack(combatProcessList.ProcessList.Count);
    for combatProcess in combatProcessList.ProcessList:
        dsItem = DataStruct();
        dsItem.PushIntoStack(combatProcess.GeneralID);
        dsItem.PushIntoStack(combatProcess.LiveNum);
        dsItem.PushShortIntoStack(combatProcess.Momentum);
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatProcess.AttackTaget));
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatProcess.AttackUnit));
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatProcess.AbilityProperty));
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatProcess.AttStatus));
        dsItem.PushIntoStack(combatProcess.DamageNum);
        dsItem.PushIntoStack(MathUtils.ToNotNullString(combatProcess.AttEffectID));
        dsItem.PushIntoStack(MathUtils.ToNotNullString(combatProcess.TargetEffectID));
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatProcess.IsMove));
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatProcess.Position));
        dsItem.PushShortIntoStack(MathUtils.ToShort(combatProcess.Role));


        dsItem.PushIntoStack(combatProcess.DamageStatusList.Count);
        for effectStatus in combatProcess.DamageStatusList:
            dsItem1 = DataStruct();
            dsItem1.PushShortIntoStack(MathUtils.ToShort(effectStatus.AbilityType));
            dsItem1.PushIntoStack(effectStatus.DamageNum);
            dsItem1.PushIntoStack(1 if effectStatus.IsIncrease else 0);
            dsItem.PushIntoStack(dsItem1);
            
        dsItem.PushIntoStack(combatProcess.TargetList.Count);
        for targetProcess in combatProcess.TargetList:
            dsItem1 = DataStruct();
            dsItem1.PushIntoStack(targetProcess.GeneralID);
            dsItem1.PushIntoStack(targetProcess.LiveNum);
            dsItem1.PushShortIntoStack(targetProcess.Momentum);
            dsItem1.PushIntoStack(targetProcess.DamageNum);
            dsItem1.PushShortIntoStack(MathUtils.ToShort(targetProcess.IsShanBi));
            dsItem1.PushShortIntoStack(MathUtils.ToShort(targetProcess.IsGeDang));
            dsItem1.PushShortIntoStack(MathUtils.ToShort(targetProcess.IsBack));
            dsItem1.PushShortIntoStack(MathUtils.ToShort(targetProcess.IsMove));
            dsItem1.PushIntoStack(targetProcess.BackDamageNum);
            dsItem1.PushShortIntoStack(MathUtils.ToShort(targetProcess.TargetStatus));
            dsItem1.PushShortIntoStack(MathUtils.ToShort(targetProcess.Position));
            dsItem1.PushShortIntoStack(MathUtils.ToShort(targetProcess.Role));
            # 目标中招效果
            dsItem1.PushIntoStack(targetProcess.DamageStatusList.Count);
            for effectStatus in targetProcess.DamageStatusList:
                dsItem12 = DataStruct();
                dsItem12.PushShortIntoStack(MathUtils.ToShort(effectStatus.AbilityType));
                dsItem12.PushIntoStack(1 if effectStatus.IsIncrease else 0);
                
                dsItem1.PushIntoStack(dsItem12);
                
            dsItem1.PushShortIntoStack(MathUtils.ToShort(targetProcess.IsBaoji));
            dsItem1.PushIntoStack(targetProcess.TrumpStatusList.Count);
            for item in targetProcess.TrumpStatusList:
                dsItem13 = DataStruct();
                dsItem13.PushShortIntoStack(MathUtils.ToShort(item.AbilityID));
                dsItem13.PushIntoStack(item.Num);
                dsItem1.PushIntoStack(dsItem13);

            dsItem.PushIntoStack(dsItem1);
            
        dsItem.PushIntoStack(combatProcess.TrumpStatusList.Count);
        for item in combatProcess.TrumpStatusList:
            dsItem14 = DataStruct();
            dsItem14.PushShortIntoStack(MathUtils.ToShort(item.AbilityID));
            dsItem14.PushIntoStack(item.Num);
            dsItem.PushIntoStack(dsItem14);
            
        dsItem.PushIntoStack(MathUtils.ToNotNullString(combatProcess.FntHeadID));
        dsItem.PushIntoStack(combatProcess.AbilityID);
        writer.PushIntoStack(dsItem);
        
    # writer.PushIntoStack(userPlotCombat.BlessExperience);
    # writer.PushIntoStack(actionResult.GotoNum);
    writer.PushIntoStack(combatProcessList.SelfAbilityEffectList.Count);
    for selfAbilityEffect in combatProcessList.SelfAbilityEffectList:
        dsItem = DataStruct();
        dsItem.PushIntoStack(selfAbilityEffect.GeneralID);
        dsItem.PushIntoStack(MathUtils.ToNotNullString(selfAbilityEffect.EffectID1));
        dsItem.PushIntoStack(MathUtils.ToNotNullString(selfAbilityEffect.FntHeadID));
        dsItem.PushShortIntoStack(1 if selfAbilityEffect.IsIncrease else 0);
        dsItem.PushIntoStack(selfAbilityEffect.Position);
        dsItem.PushIntoStack(MathUtils.ToInt(selfAbilityEffect.Role));
        writer.PushIntoStack(dsItem);
        
    writer.PushIntoStack(actionResult._userTalPriority);
    writer.PushIntoStack(actionResult._npcTalPriority);
    writer.PushIntoStack(urlParam.plotNpcID);

    writer.PushIntoStack(actionResult.gameCoin);
    

    writer.PushIntoStack(actionResult.gold);
    writer.PushIntoStack(actionResult.agentNum);
    writer.PushIntoStack(actionResult.honourNum);
    writer.PushShortIntoStack(actionResult.starNum);

    dropItem = actionResult.dropReward.split(',');
    writer.PushIntoStack(len(dropItem));
    # 掉落物品
    for info in dropItem:
        dsItem = DataStruct()
        item = info.split('*');
        count = len(item);
        dsItem.PushIntoStack(item[0] if count >= 1 else '')
        dsItem.PushIntoStack(MathUtils.ToInt(item[1]) if count >= 2 else 0)
        dsItem.PushIntoStack(item[2] if count >= 3 else '')
        writer.PushIntoStack(dsItem)

    writer.PushIntoStack(actionResult.plotName);
    writer.PushIntoStack(actionResult.maxHonourNum);
    writer.PushIntoStack(actionResult.currentHonourNum);
    writer.PushIntoStack(actionResult.lastMaxHonourNum);
    writer.PushIntoStack(1 if actionResult.currentHonourNum >= actionResult.lastMaxHonourNum else 0);
    return True;