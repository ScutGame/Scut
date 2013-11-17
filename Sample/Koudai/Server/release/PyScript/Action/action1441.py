import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');
from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model.ConfigModel import *
from ZyGames.Framework.Common import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *

# 1441_佣兵升级界面接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.generalID = 0
        self.isUp = 0
        self.strUserItemID = []
        self.userID=''
        self.strUserCardNum = []

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.userGeneral = None;
        self.upExperience = 0;
        self.nextLv = 0
        self.maxUserLv = 0;
        self.userLv = 0;
        self.costExperience = 0;
        self.currentGeneralExper = 0;  # 佣兵当前经验值
        self.experiencePercent = 0  # 升级经验百分比

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("GeneralID"):
        urlParam.generalID = httpGet.GetIntValue("GeneralID");
    else:
        urlParam.Result = False;
    return urlParam;

def GetItemBaseInfo(userID, userItemID):
    package = UserItemPackage.Get(userID);
    if package != None:
        useritem = package.ItemPackage.Find(lambda s:not s.IsRemove and s.UserItemID == userItemID);
        if useritem != None and useritem.Num > 0:
            return ConfigCacheSet[ItemBaseInfo]().FindKey(useritem.ItemID);
    return None;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    urlParam.userID = parent.Current.User.PersonalId;
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    usergeneralList = GameDataCacheSet[UserGeneral]().FindAll(userId, lambda s:s.GeneralID != urlParam.generalID and s.GeneralType == GeneralType.YongBing,True);
    for item in usergeneralList:
        item.GeneralCard ='';
    userGeneral = GameDataCacheSet[UserGeneral]().FindKey(userId, urlParam.generalID);
    actionResult.userGeneral = userGeneral;
    if userGeneral != None:
        actionResult.nextLv = userGeneral.GeneralLv;
        if userGeneral.GeneralCard:
            urlParam.strUserItemID = userGeneral.GeneralCard.TrimEnd(',').Split(',');
    strList = "";
    for str in urlParam.strUserItemID:
        if(GetItemBaseInfo(userId,str)):
            strList += str + ",";
    if(strList!=""):
        strList = strList.TrimEnd(',');
        urlParam.strUserItemID = strList.Split(',');
        urlParam.strUserCardNum = userGeneral.GeneralCardNum.Split(',');
    else:
        urlParam.strUserItemID = [];
    generalInfo = ConfigCacheSet[GeneralInfo]().FindKey(urlParam.generalID);
    if generalInfo != None:
        urlParam.isUp = 1;       
    #generalEscalate = ConfigCacheSet[GeneralEscalateInfo]().FindKey(actionResult.nextLv, GeneralType.YongBing);
    #if generalEscalate is not None:
    #    actionResult.upExperience =generalEscalate.UpExperience;
    # 玩家最高等级
    actionResult.maxUserLv = ObjectExtend.ToShort(ConfigEnvSet.GetInt("User.CurrMaxLv"));
    # 玩家等级
    actionResult.userLv = contextUser.UserLv;
    # 升级经验百分比
    generalEscalate = ConfigCacheSet[GeneralEscalateInfo]().FindKey(actionResult.nextLv + 1, GeneralType.YongBing);
    if generalEscalate:
        actionResult.experiencePercent = generalEscalate.UpExperience

    return actionResult;

# 提升到最高等级
def improveLv(sumExperience, actionResult, tempUserLv):
    #while True:
    generalEscalate = ConfigCacheSet[GeneralEscalateInfo]().FindKey(actionResult.nextLv + 1, GeneralType.YongBing);
    if generalEscalate:
        if sumExperience >= generalEscalate.UpExperience:
            if actionResult.nextLv >= tempUserLv:
                return False;
            else:
                actionResult.nextLv += 1;
                sumExperience -= generalEscalate.UpExperience;
                actionResult.costExperience += generalEscalate.UpExperience;
                improveLv(sumExperience, actionResult, tempUserLv)
        else:
            return False;

def buildPacket(writer, urlParam, actionResult):
    userGeneral = actionResult.userGeneral
    actionResult.currentGeneralExper = userGeneral.CurrExperience;
    writer.PushIntoStack(len(urlParam.strUserItemID));
    index = 0;
    experienceCards = 0;
    for str in urlParam.strUserItemID:
        #if not str:
        #    itemInfo = None;
        #else:
        #    itemInfo = GetItemBaseInfo(urlParam.userID, str);
        itemInfo = None if str == None else GetItemBaseInfo(urlParam.userID, str);
        dsItem = DataStruct();
        dsItem.PushIntoStack(MathUtils.ToNotNullString(str));
        dsItem.PushIntoStack(0 if itemInfo == None else itemInfo.ItemID);
        dsItem.PushIntoStack('' if itemInfo == None else MathUtils.ToNotNullString(itemInfo.ItemName));
        dsItem.PushIntoStack('' if itemInfo == None else MathUtils.ToNotNullString(itemInfo.HeadID));
        count = MathUtils.ToInt(urlParam.strUserCardNum[index]);
        dsItem.PushIntoStack(count);
        # 经验卡
        experienceCards += itemInfo.EffectNum * count;
        writer.PushIntoStack(dsItem);
        index = index + 1;
    writer.PushIntoStack('' if userGeneral == None else MathUtils.ToNotNullString(userGeneral.GeneralName));
    writer.PushShortIntoStack(0 if userGeneral == None else MathUtils.ToShort(userGeneral.GeneralLv));
    # 当前经验 + 经验卡总经验
    sumExperience = actionResult.currentGeneralExper + experienceCards
    tempUserLv = actionResult.userLv * 3
    improveLv(sumExperience, actionResult, tempUserLv);
    if userGeneral.GeneralLv >= actionResult.maxUserLv or userGeneral.GeneralLv >= (actionResult.userLv * 3):
        actionResult.nextLv = 0;
    
    writer.PushShortIntoStack(MathUtils.ToShort(actionResult.nextLv));
    writer.PushIntoStack('' if userGeneral == None else MathUtils.ToNotNullString(userGeneral.PicturesID));
    writer.PushIntoStack(0 if userGeneral == None else userGeneral.LifeNum);
    diffExper = actionResult.costExperience - userGeneral.CurrExperience
    if actionResult.nextLv >= actionResult.userLv * 3 or actionResult.nextLv == 0:
        writer.PushIntoStack(diffExper);  # 实际获得经验
    else:
        writer.PushIntoStack(experienceCards);  # 获得经验卡经验
    writer.PushIntoStack(0 if userGeneral == None else userGeneral.AbilityID);
    writer.PushShortIntoStack(MathUtils.ToShort(urlParam.isUp));
    # 当前已有经验
    writer.PushIntoStack(0 if userGeneral == None else userGeneral.CurrExperience);
    
    # 当前已有经验占升级所需经验百分比
    if actionResult.experiencePercent > 0:
        proportion = userGeneral.CurrExperience / (actionResult.experiencePercent * 1.0);
        tempPro = '{0}%'.format(MathUtils.ToInt(proportion * 100));
        writer.PushIntoStack(tempPro);
    else:
        writer.PushIntoStack('0%');

    if actionResult.nextLv >= actionResult.userLv * 3 and experienceCards > diffExper:
        writer.PushIntoStack(1);
    else:
        writer.PushIntoStack(0);
    return True;