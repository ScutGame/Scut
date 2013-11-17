import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');

from action import *
from System import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
#物品使用接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.UserItemID = ''

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.GoldNum = 0;

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    if httpGet.Contains("UserItemID"):
        urlParam.BackpackType =  httpGet.GetIntValue("UserItemID");
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam,parent):
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    userId =parent.Current.UserId;
    cacheSetUser =  GameDataCacheSet[GameUser]();
    user = cacheSetUser.FindKey(userId.ToString());
    
    actionResult =ActionResult();
    cacheSetUserPack =  GameDataCacheSet[UserPack]();
    cacheSetBackPack =  ConfigCacheSet[BackpackConfigInfo]();
    cacheSetUserItemPackage =  GameDataCacheSet[UserItemPackage]();
    userItemPackage = cacheSetUserItemPackage.FindKey(userId.ToString());
    backpackConfigInfo = cacheSetBackPack.FindKey(urlParam.BackpackType);
    userPack = cacheSetUserPack.FindKey(userId.ToString());
    if (backpackConfigInfo == None or user == None or userItemPackage == None or userItemPackage.ItemPackage == None):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
        actionResult.Result = False;
        return actionResult;
    itemPack = userItemPackage.ItemPackage.Find(lambda s:s.UserItemID == urlParam.UserItemID);
    if (itemPack == None):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
        actionResult.Result = False;
        return actionResult;
    
   

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(actionResult.GoldNum);
    #for info in actionResult.List:
    #    ds = DataStruct();
    #    ds.PushIntoStack(info.TaskID);
    #    ds.PushIntoStack(info.Status);
    #    writer.PushIntoStack(ds);
    return True;