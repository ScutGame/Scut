import clr, sys
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

# 4405_战斗详细信息接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.diffcultyType = 0;
        self.difficultNum = 0;
        self.plotID = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.prizeItems = CacheList[PrizeItemInfo]();
        self._cacheSetGeneral = ConfigCacheSet[GeneralInfo]();
        self.combatProcessList = None;
        self.honourNum = 0;
        self.gotoNum = 0;
        self._userTalPriority = 0;
        self._npcTalPriority = 0;
        # self.isGotAddAttribute = 0;
        self.isWin = False;
        self.bgScene = '';
        self.exchange = 0;
        self.receive = 0;
        self.starNum = 0;
        self.generalNum = 0;
        

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("DiffcultyType")\
    and httpGet.Contains("DifficultNum")\
    and httpGet.Contains("PlotID"):
        urlParam.diffcultyType = httpGet.GetIntValue("DiffcultyType");
        urlParam.difficultNum = httpGet.GetStringValue("DifficultNum");
        urlParam.plotID = httpGet.GetStringValue("PlotID");

    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
 
    # 判断玩家等级是否达到 10 级
    if contextUser.UserLv < 10:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St4405_UserLvNotEnough");
        actionResult.Result = False;
        return actionResult;

    # 判断今日挑战次数是否已用完
    userSJTInfo = GameDataCacheSet[UserShengJiTa]().FindKey(userId);
    if not userSJTInfo:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        
    if userSJTInfo.BattleRount > 3:  # 最大挑战次数为 3
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St4405_ChallengeChanceNotEnough");
        actionResult.Result = False;
        return actionResult;
    
    # 获取战斗场景图
    sjpPlotCacheSet = ShareCacheStruct[SJTPlotInfo]().FindKey(urlParam.plotID);
    if not sjpPlotCacheSet:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    actionResult.bgScene = sjpPlotCacheSet.BgScene;

    # contextUser.DifficultyNum = urlParam.diffcultyType;  # 难度系数

    difficultNum = MathUtils.ToDouble(urlParam.difficultNum);

    plotNpcID = MathUtils.ToInt(urlParam.plotID)
    # 战斗
    plotCombater = CombatFactory.TriggerSJTPlot(contextUser, plotNpcID, difficultNum);
    if (plotCombater == None):
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    actionResult.isWin = plotCombater.Doing();

    #代表打赢副本处理相应业务
    if(actionResult.isWin == True):
        userSJTInfo.IsTierNum = plotNpcID;
        # 当天最高层
        if userSJTInfo.IsTierNum > userSJTInfo.MaxTierNum:
            userSJTInfo.MaxTierNum = userSJTInfo.IsTierNum;
        if userSJTInfo.IsTierNum !=0:
        # 三层奖励
            for i in range(1, 3):
                if (userSJTInfo.IsTierNum+i)%3==0:
                    actionResult.exchange = i;

        # 五层奖励
            for i in range(1, 5):
                if (userSJTInfo.IsTierNum+i)%5==0:
                    actionResult.receive = i;
        
        if userSJTInfo.IsTierNum == 50:  # 达到50层时，返回活动首页
            userSJTInfo.SJTStatus = 0;
        if actionResult.exchange==0:
            userSJTInfo.SJTStatus=3
        if actionResult.receive ==0:
            userSJTInfo.SJTStatus=4
        if actionResult.exchange!=0 and actionResult.receive !=0:
            userSJTInfo.SJTStatus=2

        cacheSetGeneral = GameDataCacheSet[UserGeneral]();
        userMagic = GameDataCacheSet[UserMagic]().Find(userId, lambda s:s.IsEnabled);
        userMagicID = 0 if userMagic == None else userMagic.MagicID;
        userEmbattleList = GameDataCacheSet[UserEmbattle]().FindAll(userId, lambda s:s.MagicID == userMagicID and s.GeneralID > 0);

        def method(userEmbattle):
            userGeneral = cacheSetGeneral.FindKey(userId, userEmbattle.GeneralID);
            if userGeneral != None and userGeneral.LifeNum > 0:
                actionResult.generalNum = MathUtils.Addition(actionResult.generalNum, 1);
                
        userEmbattleList.ForEach(lambda userEmbattle:method(userEmbattle));
        starNum = PlotHelper.GetStar(contextUser, actionResult.generalNum);  # 获取星星
        if not starNum:
            return loadError();
        actionResult.starNum = starNum[0];
        actionResult.score = urlParam.diffcultyType * starNum[0];
        
        userSJTInfo = GameDataCacheSet[UserShengJiTa]().FindKey(userId);
        if not userSJTInfo:
            return loadError();
        
        # 更新玩家信息
        userSJTInfo.LastScoreStar += actionResult.score;
        userSJTInfo.IsRountStar += actionResult.score;
    
        # 累计当前五层的分数
        userSJTInfo.IsTierStar += actionResult.score;

        tier = (userSJTInfo.IsTierNum + 4)/5 * 5
        # 初始化 FiveTierRewardList 数据
        fiveTierInfo = userSJTInfo.FiveTierRewardList.Find(match=lambda x:x.BattleNum == userSJTInfo.BattleRount and x.FiveTierNum == tier)
        if not fiveTierInfo:
            fiveTierReward = FiveTierReward()
            fiveTierReward.BattleNum = userSJTInfo.BattleRount
            fiveTierReward.FiveTierNum = tier;
            fiveTierReward.FiveTierStarNum = actionResult.score;
            userSJTInfo.FiveTierRewardList.Add(fiveTierReward);
        else:
            fiveTierInfo.FiveTierStarNum += actionResult.score;

        # 当前轮的最高分数如果大于当天某一轮的最高分数，更新当天某一轮的最高分数
        if userSJTInfo.IsRountStar > userSJTInfo.ScoreStar:
            userSJTInfo.RoundPoor = userSJTInfo.IsRountStar - userSJTInfo.ScoreStar;
            userSJTInfo.ScoreStar = userSJTInfo.IsRountStar;
        else:
            userSJTInfo.RoundPoor=0

    else:
        userSJTInfo.IsTierNum =0;  # 将玩家当前挑战层级置 0
        userSJTInfo.SJTStatus=0;  # 战斗失败，跳转为活动界面
        userSJTInfo.LifeNum = 0
        userSJTInfo.WuLiNum = 0
        userSJTInfo.FunJiNum = 0
        userSJTInfo.MofaNum = 0
        userSJTInfo.IsTierStar = 0

        userSJTInfo.IsRountStar = 0;

        userSJTInfo.BattleRount += 1;  # 挑战轮数加 1

    # 玩家加血
    UserHelper.GetGeneralLife(userId);
    UserHelper.RegainGeneralLife(userId);
     

    # userPlotCombat = GameDataCacheSet[UserPlotCombat]().FindKey(userId, urlParam.plotNpcID);
    userSJTInfo.EndTime = DateTime.Now;
    actionResult.combatProcessList = plotCombater.GetProcessResult();

    

    # 以下是 4004.cs 转过来的
    userEmbattleList = EmbattleHelper.CurrEmbattle(userId, True);
    for userEmbattle in userEmbattleList:
        actionResult._userTalPriority = MathUtils.Addition(actionResult._userTalPriority, PriorityHelper.GeneralTotalPriority(userId, userEmbattle.GeneralID));

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushShortIntoStack(MathUtils.ToShort(actionResult.isWin));
    writer.PushIntoStack(actionResult.bgScene);
    #writer.PushIntoStack(userPlotCombat.Experience);
    #writer.PushIntoStack(actionResult.prizeItems.Count);
    #for prizeItem in actionResult.prizeItems:
    #    item = ConfigCacheSet[ItemBaseInfo]().FindKey(prizeItem.ItemID);
    #    dsItem = DataStruct();
    #    dsItem.PushIntoStack(item.ItemName.ToNotNullString());
    #    dsItem.PushIntoStack(item.HeadID.ToNotNullString());
    #    dsItem.PushShortIntoStack(MathUtils.ToShort(item.QualityType));
    #    dsItem.PushIntoStack(prizeItem.Num);
    #    writer.PushIntoStack(dsItem);

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

    # writer.PushIntoStack(actionResult.diffcultyType); 包含在 contextUser 中
    writer.PushShortIntoStack(urlParam.diffcultyType);
    writer.PushShortIntoStack(actionResult.starNum);
    
    return True;