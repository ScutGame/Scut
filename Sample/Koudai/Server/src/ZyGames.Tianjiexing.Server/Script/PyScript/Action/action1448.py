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
from ZyGames.Framework.Common import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *
from lang import Lang


# 1448_传承与被传承人选择接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.generalID = 0
        self.heritageType = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("GeneralID")\
    and httpGet.Contains("HeritageType"):
       urlParam.generalID = httpGet.GetIntValue("GeneralID");
       urlParam.heritageType = httpGet.GetEnum[HeritageType]("HeritageType");
    else:
       urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    
    heritageList = CacheList[GeneralHeritage]();
    heritage = GeneralHeritage();
    general = GameDataCacheSet[UserGeneral]().FindKey(userId, urlParam.generalID);
    if general == None:
        actionResult.Result = False;
        return actionResult;
    if contextUser.HeritageList.Count > 0:
        heritageList = contextUser.HeritageList;
        if heritageList.Find(lambda m:m.Type == urlParam.heritageType) != None:
            heritage = heritageList.Find(lambda m:m.Type == urlParam.heritageType);
            
    if heritage and urlParam.heritageType == HeritageType.Heritage:
            if GeneralHelper.IsGeneralEqu(userId,urlParam.generalID):
                 parent.ErrorCode = Lang.getLang("ErrorCode");
                 parent.ErrorInfo = Lang.getLang("St1449_GeneralHaveEqu");
                 actionResult.Result = False;
                 return actionResult;
            if GeneralHelper.IsGeneralCrystal(userId,urlParam.generalID):
                 parent.ErrorCode = Lang.getLang("ErrorCode");
                 parent.ErrorInfo = Lang.getLang("St1449_GeneralHaveCrystal");
                 actionResult.Result = False;
                 return actionResult;
            if GeneralHelper.IsGeneralAbility(userId,urlParam.generalID):
                 parent.ErrorCode = Lang.getLang("ErrorCode");
                 parent.ErrorInfo = Lang.getLang("St1449_GeneralHaveAbility");
                 actionResult.Result = False;
                 return actionResult;
    if urlParam.heritageType == HeritageType.Heritage:
        opsid=0;
        genlv = 0;
        opsInfo = GeneralHelper.HeritageOpsInfo(opsid);
        gHeritage = heritageList.Find(lambda m:m.Type == HeritageType.IsHeritage);
          

        if opsInfo != None:
            if gHeritage != None and  general.GeneralLv < genlv:
                parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
                parent.ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                actionResult.Result = False;
                return actionResult;
            if gHeritage != None:
                genlv = MathUtils.ToFloorInt(general.GeneralLv * opsInfo.Num)
                if genlv < gHeritage.GeneralLv:
                    genlv = gHeritage.GeneralLv;
                contextUser.HeritageList.Remove(heritage);
                heritage.GeneralID = urlParam.generalID;
                heritage.Type = urlParam.heritageType;
                heritage.GeneralLv = general.GeneralLv;
                heritage.opsType = 1;
                contextUser.HeritageList.Add(heritage);
                gHeritage.GeneralLv = genlv;
                        
    elif urlParam.heritageType == HeritageType.IsHeritage:
         contextUser.HeritageList = CacheList[GeneralHeritage]();
         heritage.GeneralID = urlParam.generalID;
         heritage.GeneralLv = general.GeneralLv;
         heritage.Type = urlParam.heritageType;
         contextUser.HeritageList.Add(heritage);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
   
    return True;