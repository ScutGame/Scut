import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');

from action import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

#class ActionResult():
#    Result = True;   
#    generalList = [];

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.generalList = [];

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    return urlParam;

def takeAction(urlParam,parent):
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    actionResult =ActionResult();
    actionResult.generalList = ShareCacheStruct[GeneralInfo]().FindAll(lambda s:s.IsCreate,True);
    
    #ctionResult = ['he'];
    #处理结果存储在字典中
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(len(actionResult.generalList));
    for general in rang(1,actionResult.generalList):
        abilityName = '';
        abilityInfo = ConfigCacheSet[AbilityInfo].FindKey(general.AbilityID);
        if(abilityInfo!=null):
            abilityName = abilityInfo.AbilityName;
        ds = DataStruct();
        ds.PushIntoStack(general.GeneralID);
        ds.PushIntoStack(General.GeneralName.ToNotNullString());
        ds.PushIntoStack(General.PicturesID.ToNotNullString());
        ds.PushIntoStack(abilityName.ToNotNullString());
        ds.PushIntoStack(General.Description.ToNotNullString());
        writer.PushIntoStack(ds);
    return True;