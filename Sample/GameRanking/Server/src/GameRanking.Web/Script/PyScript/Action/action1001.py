"""1001_排名列表接口"""
import ReferenceLib
from action import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Service import *
from GameServer.Model import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.PageIndex = 0
        self.PageSize = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PageCount = 0
        self.List = []


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.PageIndex = httpGet.GetIntValue("PageIndex")
        urlParam.PageSize = httpGet.GetIntValue("PageSize")
    else:
        urlParam.Result = False

    return urlParam
   
def compareTo(x, y):
    return y.Score - x.Score

def takeAction(urlParam, parent):
 
    actionResult = ActionResult()
    cache = ShareCacheStruct[UserRanking]()
    actionResult.List = cache.FindAll()
    actionResult.List = MathUtils.QuickSort[UserRanking](actionResult.List, compareTo)
    pagingResult = MathUtils.GetPaging[UserRanking](actionResult.List, urlParam.PageIndex, urlParam.PageSize)
    if pagingResult :
        actionResult.List = pagingResult[0]
        actionResult.PageCount = pagingResult[1]

    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PageCount)
    writer.PushIntoStack(len(actionResult.List))
    for info in actionResult.List:
        dsItem = DataStruct()
        dsItem.PushIntoStack(info.UserName)
        dsItem.PushIntoStack(info.Score)
        writer.PushIntoStack(dsItem)


    return True