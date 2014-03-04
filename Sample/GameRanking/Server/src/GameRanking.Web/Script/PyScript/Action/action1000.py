"""1000_排名录入接口"""
import ReferenceLib
from action import *
from ZyGames.Framework.Cache.Generic import *
from GameServer.Model import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.UserName = ''
        self.Score = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("UserName")\
    and httpGet.Contains("Score"):
        urlParam.UserName = httpGet.GetStringValue("UserName")
        urlParam.Score = httpGet.GetIntValue("Score")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    cache = ShareCacheStruct[UserRanking]()
    ranking = cache.Find( match = lambda m : m.UserName==urlParam.UserName )
    if not ranking:
        ranking = UserRanking()
        ranking.UserID = cache.GetNextNo()
        ranking.UserName = urlParam.UserName
        ranking.Score = urlParam.Score
        cache.Add(ranking)
    else:
        ranking.UserName = urlParam.UserName
        ranking.Score = urlParam.Score

    return actionResult

def buildPacket(writer, urlParam, actionResult):

    return True