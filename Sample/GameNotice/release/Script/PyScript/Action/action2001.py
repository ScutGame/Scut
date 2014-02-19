"""2001_游戏公告信息【完成】"""
import ReferenceLib
from action import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from GameNotice.Model import *


class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.GameType = 0
        self.ServerID = 0
        self.PageIndex = 0
        self.PageSize = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PageCount = 0
        self.DsItemCollect = None


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("GameType")\
    and httpGet.Contains("ServerID")\
    and httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.GameType = httpGet.GetIntValue("GameType")
        urlParam.ServerID = httpGet.GetIntValue("ServerID")
        urlParam.PageIndex = httpGet.GetIntValue("PageIndex")
        urlParam.PageSize = httpGet.GetIntValue("PageSize")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    noticeList = ShareCacheStruct[Notice]().FindAll()
    result = MathUtils.GetPaging[Notice](noticeList, urlParam.PageIndex, urlParam.PageSize)
    if result:
        actionResult.dsItemCollect = result[0]
        actionResult.PageCount = result[1]
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PageCount)
    writer.PushIntoStack(len(actionResult.dsItemCollect))
    for info in actionResult.dsItemCollect:
        dsItem = DataStruct()
        dsItem.PushIntoStack(info.Title)
        dsItem.PushIntoStack(info.Content)
        dsItem.PushIntoStack(info.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"))
        writer.PushIntoStack(dsItem)


    return True