import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');
from action import *
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


# 1447_传承与被传承人列表接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.heritageType = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.generalArray = List[UserGeneral]()

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("HeritageType"):
       urlParam.heritageType = httpGet.GetEnum[HeritageType]("HeritageType");
    else:
       urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    generalID = 0;
    heGeneralID = 0;
    generalLv = 0;    
    isGeneralID = 0;

    if contextUser.HeritageList.Count > 0:
        heritageGenral = contextUser.HeritageList.Find(lambda m:m.Type == HeritageType.IsHeritage);
        if heritageGenral != None:           
            heGeneralID = heritageGenral.GeneralID;
            hUserGeneral = GameDataCacheSet[UserGeneral]().FindKey(userId,heritageGenral.GeneralID);
            if hUserGeneral!=None:
                generalLv = MathUtils.Addition(hUserGeneral.GeneralLv,MathUtils.ToShort(3));        
        IsGenral = contextUser.HeritageList.Find(lambda m:m.Type == HeritageType.Heritage);
        if IsGenral != None:
            isGeneralID = IsGenral.GeneralID;
    if urlParam.heritageType == HeritageType.Heritage:
        generalList = GameDataCacheSet[UserGeneral]().FindAll(userId,lambda u:u.GeneralID != heGeneralID and u.GeneralID != isGeneralID and u.IsBattle== False\
            and u.GeneralType != GeneralType.Battle and  u.GeneralType !=GeneralType.Soul \
            and u.GeneralLv >= generalLv and u.GeneralStatus == GeneralStatus.DuiWuZhong,True);
    elif urlParam.heritageType == HeritageType.IsHeritage:         
              generalList = GameDataCacheSet[UserGeneral]().FindAll(userId,lambda u:u.GeneralStatus == GeneralStatus.DuiWuZhong\
                  and u.GeneralID != generalID and u.GeneralID != heGeneralID  and u.IsBattle== False,True);
    actionResult.generalArray.Clear();
    for userGeneral in generalList:
        if not EmbattleHelper.IsEmbattleGeneral(userGeneral.UserID, userGeneral.GeneralID):
            actionResult.generalArray.Add(userGeneral);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.generalArray.Count);
    for item in actionResult.generalArray:
        careerInfo = ConfigCacheSet[CareerInfo]().FindKey(item.CareerID);
        dsItem = DataStruct();
        dsItem.PushIntoStack(item.GeneralID);
        dsItem.PushIntoStack(MathUtils.ToNotNullString(item.GeneralName));
        dsItem.PushIntoStack(MathUtils.ToNotNullString(item.HeadID));
        dsItem.PushShortIntoStack(item.GeneralLv);
        if careerInfo == None:
            dsItem.PushIntoStack('');
        else:
            dsItem.PushIntoStack(MathUtils.ToNotNullString(careerInfo.CareerName));
        dsItem.PushShortIntoStack(item.TrainingPower);
        dsItem.PushShortIntoStack(item.TrainingSoul);
        dsItem.PushShortIntoStack(item.TrainingIntellect);
        writer.PushIntoStack(dsItem);
    return True;