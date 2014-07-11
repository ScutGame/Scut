import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
class UrlParam():
    Result = True;
    Ops = 0;
    MultipleType = 1;
    GeneralID = 0;
class ActionResult():
    Result = True;
    PotenceNum = 0;
    ThoughtNum = 0;
    IntelligenceNum = 0;
    AttributeIndexList =[];
    AttributeChance = [];
     
#获取属性概率
def GetGeneralAttribute(actionResult):
    index = RandomUtils.GetHitIndexByTH(actionResult.AttributeChance);
    if(len(actionResult.AttributeIndexList)<=0):
        actionResult.AttributeIndexList.append(index);
    if(len(actionResult.AttributeIndexList)< 2):
        isAttribute = False;
        for attributeIndex in actionResult.AttributeIndexList:
            if(index == attributeIndex):
                isAttribute = True
        if(isAttribute == False):
            actionResult.AttributeIndexList.append(index);
        GetGeneralAttribute(actionResult);
#获取属性值
#def GetAttributeValue(actionResult,bringUpInfo):

def getUrlElement(httpGet,parent):
    result = False;
    urlParam = UrlParam();
    result = httpGet.GetInt("Ops", 1);
    if result[0]:
        result = httpGet.GetInt("MultipleType", 1);
        urlParam.Ops = result[1];
        if result[0]:
            urlParam.MultipleType = result[1];
            result = httpGet.GetInt("GeneralID", 0);
            if result[0]:
                urlParam.GeneralID = result[1];
            else:
                urlParam.Result = False;
        else:
            urlParam.Result = False;
    else:
        urlParam.Result = False;
    return urlParam;


def takeAction(urlParam,parent):
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    actionResult =ActionResult();
    cacheSetUserGeneral =  GameDataCacheSet[UserGeneral]();
    cacheSetUserItem = GameDataCacheSet[UserItem]();
    cacheSetBringUp =  ConfigCacheSet[BringUpInfo]();
    if(urlParam.Ops==1):
        bringUpInfo = cacheSetBringUp.FindKey(urlParam.MultipleType);
        userItem = cacheSetUserItem.FindAll(lambda s:s.UserID==parent.ContextUser.UserID and s.UserItemID==0,True);
        userGeneral = cacheSetUserGeneral.FindKey(parent.ContextUser.UserID,urlParam.GeneralID);
        if (userGeneral == None or bringUpInfo == None):
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
            actionResult.Result = False;
        if (userGeneral.Potential <= 0):
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
            actionResult.Result = False;
        if (bringUpInfo.UseUpType==2 and parent.ContextUser.GoldNum < bringUpInfo.UseUpNum):
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
            actionResult.Result = False;
        if (bringUpInfo.UseUpType == 1 and (userItem == None or userItem.Num < bringUpInfo.UseUpNum)):
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
            actionResult.Result = False;
        attributeChance = bringUpInfo.AttributeChance;
        actionResult.AttributeChance.append(attributeChance.PotenceNum * 10);
        actionResult.AttributeChance.append(attributeChance.ThoughtNum * 10);
        actionResult.AttributeChance.append(attributeChance.IntelligenceNum * 10);
        GetGeneralAttribute(actionResult);
    
    #ctionResult = ['he'];
    #处理结果存储在字典中
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(actionResult.PotenceNum);
    writer.PushIntoStack(actionResult.ThoughtNum);
    writer.PushIntoStack(actionResult.IntelligenceNum);
    #for info in actionResult.List:
    #    ds = DataStruct();
    #    ds.PushIntoStack(info.TaskID);
    #    ds.PushIntoStack(info.Status);
    #    writer.PushIntoStack(ds);
    return True;