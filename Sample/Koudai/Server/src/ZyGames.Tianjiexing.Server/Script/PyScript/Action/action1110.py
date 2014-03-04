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

#开户佣兵、魂技、图纸、背包格式接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.BackpackType = 0
        self.Ops = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.GoldNum = 0

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    
    if httpGet.Contains("BackpackType")\
    and httpGet.Contains("Ops"):
        urlParam.BackpackType =  httpGet.GetIntValue("BackpackType", 1, 4 );
        urlParam.Ops =  httpGet.GetIntValue("Ops", 1, 2 );
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
    #user = parent.Current.GetUser[GameUser]();
    
    actionResult =ActionResult();
    cacheSetUserPack =  GameDataCacheSet[UserPack]();
    cacheSetBackPack =  ConfigCacheSet[BackpackConfigInfo]();
    backpackType = BackpackType.BeiBao;
    if(urlParam.BackpackType == 1):
        backpackType = BackpackType.ZhuangBei;
    elif(urlParam.BackpackType == 2):
        backpackType = BackpackType.YongBing;
    elif(urlParam.BackpackType == 3):
        backpackType = BackpackType.HunJi;
    elif(urlParam.BackpackType == 4):
        backpackType = BackpackType.BeiBao;
    backpackConfigInfo = cacheSetBackPack.FindKey(backpackType);
    userPack = cacheSetUserPack.FindKey(userId.ToString());
    packType = (userPack and userPack.PackTypeList)  and userPack.PackTypeList.Find(lambda  s:s.BackpackType == backpackType) or None;
    if (backpackConfigInfo == None or user == None):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
        actionResult.Result = False;
        return actionResult;
    goldNum = 0;
    if(packType == None):
        goldNum = backpackConfigInfo.RequiredGoldNum;
    else:
        goldNum = backpackConfigInfo.RequiredGoldNum * (packType.OpenNum+1);
    if(urlParam.Ops == 1):
        parent.ErrorCode = 1;
        parent.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
        actionResult.GoldNum = goldNum;
        return actionResult;
    if (user.GoldNum < goldNum):
        parent.ErrorCode = 3;
        parent.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
        actionResult.Result = False;
        return actionResult;
    if (packType != None and packType.OpenNum >= backpackConfigInfo.MaxOpenNum):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1110_OverExpansion;
        actionResult.Result = False;
        return actionResult;
    if (userPack == None):
        userPack =  UserPack(userId);
        packType =  PackType();
        userPack.CreateDate = DateTime.Now;
        packType.OpenNum = 1;
        packType.BackpackType = BackpackType.backpackType;
        packType.OpenNum = MathUtils.Addition(backpackConfigInfo.DefaultNum, backpackConfigInfo.EveryAddNum);
        userPack.PackTypeList.Add(packType);
        cacheSetUserPack.Add(userPack, userId);
    elif (packType == None):
        packType =  PackType();
        userPack.CreateDate = DateTime.Now;
        packType.OpenNum = 1;
        packType.BackpackType = BackpackType.backpackType;
        packType.OpenNum = MathUtils.Addition(backpackConfigInfo.DefaultNum, backpackConfigInfo.EveryAddNum);
        cacheSetUserPack.Add(userPack,userId);
        packType.Position = MathUtils.Addition(packType.Position, backpackConfigInfo.EveryAddNum);
        userPack.PackTypeList.Add(packType);
    else:
        packType.OpenNum = MathUtils.Addition(packType.OpenNum, 1);
        packType.Position = MathUtils.Addition(packType.Position, backpackConfigInfo.EveryAddNum);
        userPack.AddChildrenListener(packType);
    user.UseGold = MathUtils.Addition(user.UseGold, goldNum);
    parent.ErrorCode = 2;
    #ctionResult = ['he'];
    #处理结果存储在字典中
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