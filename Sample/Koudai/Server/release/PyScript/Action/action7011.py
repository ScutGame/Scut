import clr
import sys
clr.AddReference('ZyGames.Framework.Common')
clr.AddReference('ZyGames.Framework')
clr.AddReference('ZyGames.Framework.Game')
clr.AddReference('ZyGames.Tianjiexing.Model')
clr.AddReference('ZyGames.Tianjiexing.BLL')
clr.AddReference('ZyGames.Tianjiexing.Lang')
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
from ZyGames.Tianjiexing.BLL.Base import *
#竞技场兑换商店
class UrlParam():
    Result = True
    MallType = 1
    PageIndex = 1
    PageSize = 1

class ActionResult():
    Result = True
    PageCount = 1
    List = List[ItemBaseInfo]()
    sportsIntegral = 0;

def getUrlElement(httpGet,parent):
    urlParam = UrlParam()
    
    if httpGet.Contains("PageIndex") and httpGet.Contains("PageSize"):
        urlParam.PageIndex = httpGet.GetInt("PageIndex")
        urlParam.PageSize = httpGet.GetInt("PageSize")
    else:
    	urlParam.Result = False
    return urlParam

def takeAction(urlParam,parent):
    actionResult = ActionResult()
    userId = parent.Current.UserId
    contextUser = parent.Current.User;
    itemConfigList = ConfigCacheSet[ItemBaseInfo]().FindAll(lambda m:m.Athletics > 0,True)
    result = MathUtils.GetPaging[ItemBaseInfo](itemConfigList,urlParam.PageIndex,urlParam.PageSize)
    if result :
           actionResult.List = result[0]
           actionResult.PageCount = result[1]
    actionResult.sportsIntegral = MathUtils.ToInt(contextUser.SportsIntegral);
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(actionResult.PageCount)
    list = actionResult.List
    writer.PushIntoStack(list.Count)
    for itemInfo in list:
        ds = DataStruct()
        ds.PushIntoStack(itemInfo.ItemID)
        ds.PushIntoStack(itemInfo.ItemName)
        ds.PushIntoStack(itemInfo.HeadID)
        ds.PushIntoStack(itemInfo.MaxHeadID)
        ds.PushIntoStack(itemInfo.ItemDesc)
        ds.PushIntoStack(MathUtils.ToInt(itemInfo.QualityType))
        ds.PushIntoStack(itemInfo.Athletics)
        writer.PushIntoStack(ds)
    writer.PushIntoStack(actionResult.sportsIntegral)
    return True;