import clr, sys
import hashlib
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');
clr.AddReference('ZyGames.Tianjiexing.BLL.Combat');

from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Action import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Tianjiexing.BLL.Combat import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Tianjiexing.Component.Chat import *

# 1008_用户角色详情接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.headID = '';
        self.lifeNum = 0;
        self.lifeMaxNum = 0;
        self.maxEnergyNum = 0;
        self.careerID = 0;
        self.sex = 0;
        self.generalID = 0;
        self.guildID = '';
        self.userGeneralInfo = None;
        self.escalateInfo = None;
        self._itemLiveNum = 0;
        self._itemLiveMaxNum = 0;
        self.genlv = 0;
        self.pictureTime = 0;
        self.pictureID = '';
        self._blessingList = List[BlessingInfo]()
        self.demandGold = 0;
        self.isHelper = 0;
        self.plotstatucID = 0;
        self.mercenarySeq = 0;
        self.cardUserID = '';
        self.battleNum = 0;
        self.totalBattle = 0;
        self.rstore = 0;
        self.totalRstore = 0;
        self._honourNum = 0;
        self._nextHonourNum = 0;
        self._talPriority = 0;
        self.functionList = List[UserFunction]();
        self.contextUser = None;
        self.unReadCount = 0;
        self.lastLv = 0  # 玩家升级前的等级

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User
    actionResult.contextUser = contextUser;
    lastLv = contextUser.UserLv

    currMaxLv = MathUtils.ToShort(ConfigEnvSet.GetInt("User.CurrMaxLv"));
    NoviceHelper.GetFunctionEnum(userId); #默认开启金币，精力大作战功能
    if contextUser.MercenariesID:
        UserHelper.ChecheDailyContribution(contextUser.MercenariesID, contextUser.UserID);
        
    PaymentService.Trigger(contextUser);
    if contextUser.UserLv > currMaxLv:
        actionResult.genlv = currMaxLv;
    else:
        actionResult.genlv = contextUser.UserLv;

    cacheSetGeneralEscalate = ConfigCacheSet[GeneralEscalateInfo]();
    GeneralEscalateHelper.AddUserLv(contextUser, 0);
    actionResult._honourNum = contextUser.HonourNum;
    lv = contextUser.UserLv;
    lv = 1 if lv < 0 else (lv + 1);
    generalEscalate = cacheSetGeneralEscalate.Find(lambda s:s.GeneralType == GeneralType.YongHu and s.GeneralLv == lv);
    if generalEscalate:
        actionResult._nextHonourNum = generalEscalate.UpExperience;
        
    actionResult.guildID = contextUser.MercenariesID;
    careerInfo = None;
    actionResult.userGeneralInfo = UserGeneral.GetMainGeneral(contextUser.UserID);
    if actionResult.userGeneralInfo:
        #wuzf 去掉刷新血量，其它改变血量接口有触发刷新
        #actionResult.userGeneralInfo.RefreshMaxLife();
        actionResult.generalID = actionResult.userGeneralInfo.GeneralID;
        #careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(actionResult.userGeneralInfo.CareerID);
        actionResult.headID = actionResult.userGeneralInfo.HeadID; # contextUser.Sex ? careerInfo.HeadID2 : careerInfo.HeadID;
        actionResult.escalateInfo = ConfigCacheSet[GeneralEscalateInfo]().FindKey(actionResult.genlv);
        actionResult.lifeNum = actionResult.userGeneralInfo.LifeNum;
        actionResult.careerID = actionResult.userGeneralInfo.CareerID;
        
    actionResult.lifeMaxNum = UserHelper.GetMaxLife(contextUser.UserID, UserGeneral.MainGeneralID(contextUser.UserID));
    actionResult.maxEnergyNum = MathUtils.ToShort(ConfigEnvSet.GetInt("User.MaxEnergyNum"));
    actionResult.sex = 1 if contextUser.Sex else 0;

    # 道具图标
    actionResult._blessingList = UserHelper.BlessingInfoList(contextUser);
    # 变身卡图标
    userPropsList = GameDataCacheSet[UserProps]().FindAll(contextUser.UserID, lambda u:u.PropType == 3 and u.ItemID != 5200 and u.ItemID != 7003);
    if userPropsList.Count > 0:
        props = userPropsList[0];
        pTime = props.DoRefresh();
        itemInfo = ConfigCacheSet[ItemBaseInfo]().FindKey(props.ItemID);
        if itemInfo and pTime > actionResult.pictureTime:
            actionResult.pictureID = itemInfo.PictrueID;
            actionResult.pictureTime = pTime;
            
    # 兼容客户端上已版本血量图标
    userPropsList2 = GameDataCacheSet[UserProps]().FindAll(contextUser.UserID, lambda u:u.PropType == 1);
    if userPropsList2.Count > 0:
        props = userPropsList2[0];
        pTime = props.DoRefresh();
        itemInfo = ConfigCacheSet[ItemBaseInfo]().FindKey(props.ItemID);
        if itemInfo and pTime > actionResult.pictureTime:
            actionResult._itemLiveNum = props.SurplusNum;
            actionResult._itemLiveMaxNum = itemInfo.EffectNum;
            
    # 加量,领土战不能加血wuzf)
    if contextUser.UserStatus != UserStatus.CountryCombat:
        UserHelper.GetGeneralLife(contextUser.UserID);
         
    # 精力恢复
    energyQueueArray = GameDataCacheSet[UserQueue]().FindAll(contextUser.UserID, lambda m:m.QueueType == QueueType.EnergyHuiFu);
    if energyQueueArray.Count > 0:
        energyQueue = energyQueueArray[0];
        energyMaxNum = MathUtils.ToShort(ConfigEnvSet.GetInt("User.MaxEnergyNum"));
        restorationDate = ConfigEnvSet.GetInt("UserQueue.EnergyRestorationDate");  # 半小时
        restorationNum = ConfigEnvSet.GetInt("UserQueue.EnergyRestorationNum");  # 恢复5点
        
        if energyQueue.Timing > DateTime.Now:
            energyQueue.Timing = DateTime.Now;
        
        # 原因：玩家满精力时，精力恢复累加
        timeCount = MathUtils.ToInt((DateTime.Now - energyQueue.Timing).TotalSeconds / restorationDate);
        if timeCount > 0:
            energyNum = MathUtils.ToShort(timeCount * restorationNum);
            if contextUser.EnergyNum < energyMaxNum:
                contextUser.EnergyNum = MathUtils.Addition(contextUser.EnergyNum, energyNum, energyMaxNum);
            energyQueue.Timing = DateTime.Now;
        else:
            if energyMaxNum > contextUser.EnergyNum:
                actionResult.rstore = MathUtils.ToInt((energyQueue.Timing.AddSeconds(restorationDate) - DateTime.Now).TotalSeconds);
                actionResult.totalRstore =MathUtils.ToInt((energyQueue.Timing.AddSeconds((energyMaxNum - contextUser.EnergyNum) * restorationDate) - DateTime.Now).TotalSeconds);
    else:
        queue = UserQueue();
        queue.QueueID = MathUtils.ToNotNullString(Guid.NewGuid());
        queue.UserID = contextUser.UserID;
        queue.QueueType = QueueType.EnergyHuiFu;
        queue.QueueName = MathUtils.ToNotNullString(QueueType.EnergyHuiFu);
        queue.Timing = DateTime.Now;
        queue.ColdTime = 0;
        queue.TotalColdTime = 0;
        queue.IsSuspend = False;
        queue.StrengNum = 0;
        GameDataCacheSet[UserQueue]().Add(queue);
    lvInfo = ConfigCacheSet[VipLvInfo]().FindKey(MathUtils.Addition(contextUser.VipLv, 1, int.MaxValue));
    if lvInfo:
        actionResult.demandGold = MathUtils.Subtraction(lvInfo.PayGold, contextUser.PayGold, 0);
        actionResult.demandGold = MathUtils.Subtraction(actionResult.demandGold, contextUser.ExtGold, 0);
    UserHelper.GetGameUserCombat(contextUser.UserID);

    if MathUtils.ToInt((DateTime.Now - contextUser.DailyLoginTime).TotalSeconds) <= 5 and contextUser.UserLv > 10:
        actionResult.isHelper = 1;
    FestivalHelper.DoFestival(contextUser);
    if contextUser.UserExtend:
        if (actionResult.plotstatucID > 0 or contextUser.TempEnergyNum == 0) and not contextUser.UserExtend.IsBoss:
            contextUser.TempEnergyNum = 5;
        if (actionResult.plotstatucID == 0 and contextUser.TempEnergyNum == 0) or contextUser.UserExtend.IsBoss:
            contextUser.UserExtend.PlotStatusID = 0;
            contextUser.UserExtend.PlotNpcID = -1;
            contextUser.UserExtend.MercenarySeq = 0;
            contextUser.UserExtend.IsBoss = False;
        actionResult.plotstatucID = contextUser.UserExtend.PlotStatusID;
        actionResult.mercenarySeq = contextUser.UserExtend.MercenarySeq;
        actionResult.cardUserID = contextUser.UserExtend.CardUserID;

    # 公会晨练结束，退出公会晨练
    activeID = 11;
    active = ShareCacheStruct[GameActive]().FindKey(activeID);
    if active:
        stratTime = active.BeginTime;
        endTime = active.BeginTime.AddMinutes(active.Minutes);
        if contextUser.UserLocation == Location.GuildExercise and (DateTime.Now < stratTime or DateTime.Now > endTime):
            contextUser.UserLocation = Location.Guid;
    # out 参数
    stage = GuildFightCombat.GetStage()[1];
    # 公会战结束后
    if stage == FightStage.Apply and contextUser.UserStatus == UserStatus.FightCombat:
        contextUser.UserStatus = UserStatus.Normal;
    #actionResult.battleNum = EmbattleHelper.CurrEmbattle(contextUser.UserID, True).Count;
    actionResult.battleNum = len(EmbattleHelper.CurrEmbattle(contextUser.UserID, True))
    actionResult.totalBattle = EmbattleHelper.CurrEmbattle(contextUser.UserID, False).Count;
    userEmbattleList = EmbattleHelper.CurrEmbattle(contextUser.UserID, True);
    #for userEmbattle in userEmbattleList:
    actionResult._talPriority = CombatHelper.TotalPriorityNum(contextUser.UserID, 0);
    actionResult.functionList = GameDataCacheSet[UserFunction]().FindAll(userId);

    # 精灵祝福
    if contextUser:
        if MathUtils.SqlMinDate >= contextUser.WizardDate:  # 玩家第一次进入
            contextUser.WizardDate = DateTime.Now;
            contextUser.WizardNum = 1;
        else:
            diffHours = (DateTime.Now - contextUser.WizardDate).TotalHours;
            if diffHours >= 1:
                contextUser.WizardNum = MathUtils.Addition(contextUser.WizardNum, Convert.ToInt32(diffHours), 3);
                contextUser.WizardDate = DateTime.Now;
    actionResult.parent = parent

    # 信件未读数目
    tjxMailService = TjxMailService(parent.Current.User);
    tempMailList = tjxMailService.GetMail();
    if tempMailList:
        actionResult.unReadCount = tempMailList[1];
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    contextUser = actionResult.contextUser
    writer.PushIntoStack(contextUser.CityID);
    writer.PushShortIntoStack(contextUser.PointX);
    writer.PushShortIntoStack(contextUser.PointY);
    writer.PushIntoStack(actionResult.generalID);
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.guildID));
    writer.PushIntoStack(MathUtils.ToNotNullString(contextUser.NickName));
    writer.PushShortIntoStack(actionResult.genlv);
    writer.PushShortIntoStack(actionResult.careerID);
    writer.PushIntoStack(actionResult.sex);
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.headID));
    writer.PushIntoStack(contextUser.GoldNum);
    writer.PushIntoStack(contextUser.GameCoin);
    writer.PushIntoStack(actionResult.lifeNum);
    writer.PushIntoStack(actionResult.lifeMaxNum);
    writer.PushShortIntoStack(contextUser.EnergyNum);
    writer.PushShortIntoStack(actionResult.maxEnergyNum);
    #writer.PushIntoStack(actionResult.userGeneralInfo == null ? 0 : actionResult.userGeneralInfo.CurrExperience);
    writer.PushIntoStack(0 if actionResult.userGeneralInfo == None else actionResult.userGeneralInfo.CurrExperience);
    #writer.PushIntoStack(escalateInfo == null ? 0 : escalateInfo.UpExperience);
    writer.PushIntoStack(0 if actionResult.escalateInfo == None else actionResult.escalateInfo.UpExperience);
    
    writer.PushShortIntoStack(contextUser.VipLv);
    writer.PushShortIntoStack(MathUtils.ToShort(contextUser.CountryID));
    
    # 加状态
    writer.PushIntoStack(actionResult._itemLiveNum);
    writer.PushIntoStack(actionResult._itemLiveMaxNum);
    writer.PushIntoStack(len(actionResult._blessingList));
    for blessing in actionResult._blessingList:
        dsItem = DataStruct();
        dsItem.PushShortIntoStack(blessing.BlessingType);
        dsItem.PushIntoStack(blessing.BlessingNum);
        dsItem.PushIntoStack(blessing.PropDate);
        dsItem.PushIntoStack('');
        dsItem.PushIntoStack(MathUtils.ToNotNullString(blessing.PropDesc));
        writer.PushIntoStack(dsItem);
        
    writer.PushShortIntoStack(MathUtils.ToShort(contextUser.UserLocation));
    writer.PushIntoStack(contextUser.ExpNum);
    writer.PushShortIntoStack(MathUtils.ToShort(contextUser.UserStatus));
    #writer.PushIntoStack(contextUser.SweepPool == null ? 0 : contextUser.SweepPool.PlotID);
    writer.PushIntoStack(0 if not contextUser.SweepPool else contextUser.SweepPool.PlotID);
    #writer.PushIntoStack(contextUser.IsUseupItem ? (short)1 : (short)0);
    writer.PushShortIntoStack(1 if contextUser.IsUseupItem else 0);
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.pictureID));
    writer.PushIntoStack(actionResult.pictureTime);
    writer.PushIntoStack(actionResult.demandGold);
    writer.PushShortIntoStack(contextUser.SurplusEnergy);
    writer.PushShortIntoStack(actionResult.isHelper);
    writer.PushIntoStack(actionResult.plotstatucID);
    writer.PushIntoStack(actionResult.mercenarySeq);
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.cardUserID));
    writer.PushIntoStack(0);
    writer.PushIntoStack(actionResult.battleNum);
    writer.PushIntoStack(actionResult.totalBattle);
    writer.PushIntoStack(actionResult.rstore);
    writer.PushIntoStack(actionResult.totalRstore);
    writer.PushIntoStack(actionResult._honourNum);
    writer.PushIntoStack(actionResult._nextHonourNum);
    writer.PushIntoStack(contextUser.CombatNum);
    writer.PushIntoStack(actionResult._talPriority);
    writer.PushShortIntoStack(1 if contextUser.IsLv else 0);
    functionList = actionResult.functionList
    writer.PushIntoStack(functionList.Count);
    for i in functionList:
        dsItem = DataStruct();
        dsItem.PushShortIntoStack(MathUtils.ToShort(i.FunEnum));
        writer.PushIntoStack(dsItem);
        
    if contextUser.OpenFun != None and contextUser.OpenFun.Count > 0:
        OpenFun = contextUser.OpenFun;
        writer.PushIntoStack(OpenFun.Count);
        for item in OpenFun:
            dsItem = DataStruct();
            dsItem.PushShortIntoStack(MathUtils.ToShort(item.FunEnum));
            writer.PushIntoStack(dsItem);
        contextUser.OpenFun.Clear();
    else:
        writer.PushIntoStack(0);  # 记录数用 PushIntoStack()

    # 信件未读数目
    writer.PushIntoStack(actionResult.unReadCount);
    # 精灵数目
    writer.PushIntoStack(contextUser.WizardNum);
    
    return True