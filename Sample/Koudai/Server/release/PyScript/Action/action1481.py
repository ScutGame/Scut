import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');

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
#魂技升级接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.AbilityID = 0;
        self.UserItemID = '';
        self.UserItemIDs = '';

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.UserItemIDList = [];
        self.AbilityList = [];
        self.ExperienceNum = 0;
        self.AbilityLv = 0;

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    if httpGet.Contains("AbilityID")\
    and httpGet.Contains("UserItemID")\
    and httpGet.Contains("UserItemIDs"):
        urlParam.AbilityID =  httpGet.GetIntValue("AbilityID");
        urlParam.UserItemID =  httpGet.GetStringValue("UserItemID", 36, 36);
        urlParam.UserItemIDs =  httpGet.GetStringValue("UserItemIDs");
    else:
        urlParam.Result = False;
    return urlParam;
def takeAction(urlParam,parent):
    #GameService.getUser();
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    actionResult = ActionResult();
    actionResult.UserItemIDList = urlParam.UserItemIDs.Split(',');
    userId = parent.Current.UserId;
    user = parent.Current.User;
    cacheSetItemBaseInfo =  ConfigCacheSet[ItemBaseInfo]();
    cacheSetUserAbility =  GameDataCacheSet[UserAbility]();
    cacheSetUserItemPackage =  GameDataCacheSet[UserItemPackage]();
    cacheSetAbility =  ConfigCacheSet[AbilityInfo]();
    cacheSetAbilityLv =  ConfigCacheSet[AbilityLvInfo]();
    userAbility = cacheSetUserAbility.FindKey(userId.ToString());
    abilityInfo = cacheSetAbility.FindKey(urlParam.AbilityID);
    if (userAbility == None or abilityInfo == None):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
        actionResult.Result = False;
        return actionResult;
    ability =  userAbility.AbilityList.Find(lambda  s:s.AbilityID == urlParam.AbilityID and s.UserItemID == urlParam.UserItemID);
    
    if (ability == None ):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
        actionResult.Result = False;
        return actionResult;
    abilityLv = cacheSetAbilityLv.FindKey(urlParam.AbilityID,ability.AbilityLv);
    if (abilityLv == None):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
        actionResult.Result = False;
        return actionResult;
    if (abilityLv.IsMaxLv):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1481_AbilityIsMaxLv;
        actionResult.Result = False;
        return actionResult;
    for item in actionResult.UserItemIDList:
        useAbility = userAbility.AbilityList.Find(lambda s:s.UserItemID == item);
        if (useAbility == None):
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
            actionResult.Result = False;
            return actionResult;
        if (useAbility.GeneralID > 0):
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St1481_AbilityIsGeneral;
            actionResult.Result = False;
            return actionResult;
        if(urlParam.UserItemID == item):
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St1481_AbilityEcho;
            actionResult.Result = False;
            return actionResult;
        actionResult.ExperienceNum = MathUtils.Addition(actionResult.ExperienceNum, (useAbility.ExperienceNum+useAbility.GoupExperienceNum));
    user.AbilityExperienceNum =  MathUtils.Addition(user.AbilityExperienceNum, actionResult.ExperienceNum );
    ability.ExperienceNum = MathUtils.Addition(ability.ExperienceNum,actionResult.ExperienceNum);
    abilityLvList = cacheSetAbilityLv.FindAll(lambda s:s.AbilityID == ability.AbilityID and s.Experience<= ability.ExperienceNum,True);
    abilityLv = ability.AbilityLv;
    abilityLvInfo = AbilityLvInfo();
    for info in abilityLvList:
        if(info.Lv > abilityLv):
            abilityLv = info.Lv;
            abilityLvInfo = info;
    
    if (abilityLvInfo and ability.AbilityLv < abilityLv):
        ability.ExperienceNum = MathUtils.Subtraction(ability.ExperienceNum, abilityLvInfo.Experience);
        ability.GoupExperienceNum = MathUtils.Addition(ability.GoupExperienceNum, abilityLvInfo.Experience);
    ability.AbilityLv = abilityLv;
    actionResult.AbilityLv = abilityLv;
    for item in actionResult.UserItemIDList:
        useAbility = userAbility.AbilityList.Find(lambda s:s.UserItemID == item);
        if (useAbility):
            userAbility.AbilityList.Remove(useAbility);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(actionResult.ExperienceNum);
    writer.PushIntoStack(MathUtils.ToInt(actionResult.AbilityLv));
    return True;