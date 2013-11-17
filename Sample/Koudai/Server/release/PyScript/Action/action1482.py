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
#魂技升级详细接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.UserItemID = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.AbilityName = '';
        self.AbilityLv = 0;
        self.IsExperienceNum = 0;
        self.NextExperienceNum = 0;
        self.HeadID = '';
        self.IsMaxLv = 0;
        self.List = [];

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    if httpGet.Contains("UserItemID"):
        urlParam.UserItemID =  httpGet.GetStringValue("UserItemID", 36, 36 );
    else:
        urlParam.Result = False;
    return urlParam;
def takeAction(urlParam,parent):
    #GameService.getUser();
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    actionResult =ActionResult();
    userId =parent.Current.UserId;
    user = parent.Current.User;
    cacheSetUserAbility =  GameDataCacheSet[UserAbility]();
    cacheSetAbility =  ConfigCacheSet[AbilityInfo]();
    cacheSetAbilityLv =  ConfigCacheSet[AbilityLvInfo]();
   
    userAbility = cacheSetUserAbility.FindKey(userId.ToString());
    if (userAbility == None ):
       parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
       parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
       actionResult.Result = False;
       return actionResult;
    ability =  userAbility.AbilityList.Find(lambda  s:MathUtils.ToNotNullString(s.UserItemID) == MathUtils.ToNotNullString(urlParam.UserItemID));
    if (ability == None ):
       parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
       parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
       actionResult.Result = False;
       return actionResult;
    abilityInfo = cacheSetAbility.FindKey(ability.AbilityID);
    if (abilityInfo == None ):
       parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
       parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
       actionResult.Result = False;
       return actionResult;
    actionResult.List = userAbility.AbilityList.FindAll(lambda s:s.GeneralID == 0 and s.UserItemID !=  urlParam.UserItemID);
    if ability and abilityInfo :
        abilityLvInfo = cacheSetAbilityLv.FindKey(ability.AbilityID,ability.AbilityLv+1);
        actionResult.AbilityName = abilityInfo.AbilityName;
        actionResult.AbilityLv = ability.AbilityLv;
        actionResult.IsExperienceNum = ability.ExperienceNum;
        actionResult.HeadID = abilityInfo.HeadID;
        if abilityLvInfo :
            actionResult.NextExperienceNum = abilityLvInfo.Experience;
        else :
            abilityLvInfo = cacheSetAbilityLv.FindKey(ability.AbilityID,ability.AbilityLv);
            if abilityLvInfo:
                actionResult.NextExperienceNum = abilityLvInfo.Experience;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    cacheSetAbility =  ConfigCacheSet[AbilityInfo]();
    writer.PushIntoStack(actionResult.AbilityName);
    writer.PushIntoStack(actionResult.AbilityLv);
    writer.PushIntoStack(actionResult.IsExperienceNum);
    writer.PushIntoStack(actionResult.NextExperienceNum);
    writer.PushIntoStack(actionResult.HeadID);
    writer.PushIntoStack(actionResult.IsMaxLv);
    writer.PushIntoStack(len(actionResult.List));
    for info in actionResult.List:
        abilityInfo = cacheSetAbility.FindKey(info.AbilityID);
        if abilityInfo :
            ds = DataStruct();
            ds.PushIntoStack(info.UserItemID);
            ds.PushIntoStack(abilityInfo.AbilityID);
            ds.PushIntoStack(abilityInfo.HeadID);
            ds.PushIntoStack(abilityInfo.AbilityName);
            ds.PushIntoStack(abilityInfo.AbilityDesc);
            ds.PushIntoStack(info.ExperienceNum);
            ds.PushIntoStack(info.AbilityLv);
            writer.PushIntoStack(ds);
        else:
            ds = DataStruct();
            ds.PushIntoStack('');
            ds.PushIntoStack(0);
            ds.PushIntoStack('');
            ds.PushIntoStack('');
            ds.PushIntoStack('');
            ds.PushIntoStack(info.ExperienceNum);
            ds.PushIntoStack(info.AbilityLv);
            writer.PushIntoStack(ds);
    return True;