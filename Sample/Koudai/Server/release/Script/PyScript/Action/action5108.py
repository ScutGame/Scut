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
clr.AddReference('ZyGames.Tianjiexing.Component');

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
from ZyGames.Tianjiexing.Component.Chat import *
from ZyGames.Framework.Common.Serialization import *
from ZyGames.Tianjiexing.Model.Enum import *

# 5108_竞技场战斗报告接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.mailID = '';
        self.toUserID = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.isWin = 0;
        self._userTalPriority = 0;
        self._npcTalPriority = 0;
        self.combatProcessList = None;
        self._cacheSetGeneral = ConfigCacheSet[GeneralInfo]();
        self.gameCoin = 0
        self.obtion = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("ToUserID")\
    and httpGet.Contains("MailID"):
        urlParam.toUserID = httpGet.GetStringValue("ToUserID")
        urlParam.mailID = httpGet.GetStringValue("MailID")
    else:
        urlParam.Result = False
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    # 错误信息提示
    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St5108_CombatReplayFail");
        actionResult.Result = False;
        return actionResult;
    tjxMailService = TjxMailService(contextUser);
    mailList = tjxMailService.GetMail();
    if not mailList or not mailList[0]:
        return loadError();

    list = mailList[0]
    mailInfo = list.Find(match=lambda x:x.MailID == Guid(urlParam.mailID))
    #mailInfo = mailList[0].Find(match=lambda x:x.UserId == MathUtils.ToInt(userId) and x.CounterattackUserID == toUserId and\
        #x.MailType == MailType.Fight and x.SendDate == sendDate);
    if not mailInfo:
        return loadError()

    actionResult.combatProcessList = JsonUtils.Deserialize(mailInfo.CombatProcess, CombatProcessContainer);
    actionResult.isWin = mailInfo.IsWin;
    actionResult.gameCoin = mailInfo.GameCoin
    actionResult.obtion = mailInfo.Obtion
    
    userEmbattleList = EmbattleHelper.CurrEmbattle(userId, True);
    for userEmbattle in userEmbattleList:
        actionResult._userTalPriority = MathUtils.Addition(actionResult._userTalPriority, PriorityHelper.GeneralTotalPriority(userId, userEmbattle.GeneralID));

    actionResult._npcTalPriority = CombatHelper.TotalPriorityNum(urlParam.toUserID, 0);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushShortIntoStack(MathUtils.ToShort(actionResult.isWin));
    combatProcessList = actionResult.combatProcessList;
    

    # 防方阵形
    writer.PushIntoStack(combatProcessList.DefenseList.Count);
    for combatEmbattle in combatProcessList.DefenseList:
        isDefWait = 1 if combatEmbattle.IsWait else 0;
        general = actionResult._cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
        dsItem = DataStruct();
        dsItem.PushIntoStack(combatEmbattle.GeneralID);
        dsItem.PushIntoStack(MathUtils.ToNotNullString(combatEmbattle.GeneralName));
        dsItem.PushIntoStack(MathUtils.ToNotNullString(general.BattleHeadID) if general else '');
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
        dsItem.PushShortIntoStack(MathUtils.ToShort(general.GeneralQuality) if general else MathUtils.ToShort(0));
        #dsItem.PushIntoStack(combatEmbattle.EvolutionNum);
        dsItem.PushIntoStack(0);
        writer.PushIntoStack(dsItem);


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
        dsItem.PushShortIntoStack(MathUtils.ToShort(general.GeneralQuality) if general else MathUtils.ToShort(0));
        #dsItem.PushIntoStack(combatEmbattle.EvolutionNum);
        dsItem.PushIntoStack(0);
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
        dsItem.PushShortIntoStack(0 if MathUtils.ToShort(combatProcess.Role) == 1 else 1);


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
            dsItem1.PushShortIntoStack(0 if MathUtils.ToShort(targetProcess.Role) == 1 else 1);
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
        dsItem.PushIntoStack(0 if MathUtils.ToInt(selfAbilityEffect.Role) == 1 else 1);
        writer.PushIntoStack(dsItem);
        
    writer.PushIntoStack(actionResult._userTalPriority);
    writer.PushIntoStack(actionResult._npcTalPriority);
    writer.PushIntoStack(actionResult.gameCoin);
    writer.PushIntoStack(actionResult.obtion);
    return True;