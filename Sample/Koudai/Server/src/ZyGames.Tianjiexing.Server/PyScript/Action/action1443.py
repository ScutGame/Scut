import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');

from action import *
from ZyGames.Framework.Common.Log import *
from System.Collections.Generic import *
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

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.ItemNum = 0
        self.BringUpInfoList = List[BringUpInfo]()

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    return urlParam;

def takeAction(urlParam,parent):
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    actionResult =ActionResult();
    itemId = ConfigEnvSet.GetInt('User.DrugItemID');
    userId =parent.Current.UserId;
    cacheSetUserItem =  GameDataCacheSet[UserItemPackage]();
    cacheSetBringUp =  ConfigCacheSet[BringUpInfo]();
    actionResult.BringUpInfoList = cacheSetBringUp.FindAll();
    userItem = cacheSetUserItem.FindKey(userId.ToString());
    if(userItem and userItem.ItemPackage):
        itemList = userItem.ItemPackage.FindAll(lambda s:s.ItemID == itemId);
        for info in itemList:
            actionResult.ItemNum = MathUtils.Addition(actionResult.ItemNum,info.Num);
    #ctionResult = ['he'];
    #处理结果存储在字典中
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(len(actionResult.BringUpInfoList));
    for info in actionResult.BringUpInfoList:
        ds = DataStruct();
        ds.PushIntoStack(info.BringUpType);
        ds.PushIntoStack(info.UseUpType);
        ds.PushIntoStack(info.UseUpNum);
        writer.PushIntoStack(ds);
    writer.PushIntoStack(actionResult.ItemNum);
    return True;