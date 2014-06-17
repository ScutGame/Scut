import ReferenceLib
from action import *
from System import *
from mathUtils import MathUtils

from System.Collections.Generic import *
from ZyGames.Framework.SyncThreading import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Cache import *


#排行榜接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.RankType = 0;
        self.PageIndex = 0
        self.PageSize = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.userRankList = List[UserRank]
        self.PageCount = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("RankType"):
        urlParam.RankType = httpGet.GetEnum[RankType]("RankType")
        urlParam.Result = True;
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();

    ranking = Ranking[UserRank];
    if urlParam.RankType == RankType.GameCoin:    
        ranking = RankingFactory.Get[UserRank](BeansRanking.RankingKey)
    if urlParam.RankType == RankType.Wining:
        ranking = RankingFactory.Get[UserRank](WinRanking.RankingKey)
    result = ranking.GetRange(urlParam.PageIndex, urlParam.PageSize); 
    if result:
        actionResult.userRankList = result[0]
        actionResult.PageCount = result[1]
    #需要实现
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PageCount)
    writer.PushIntoStack(len(actionResult.userRankList))
    for info in actionResult.userRankList:
        wining = PythonHelper.TransformString(info.Wining)
        DsItem = DataStruct()
        DsItem.PushIntoStack(info.RankId)
        DsItem.PushIntoStack(info.UserID)
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.NickName))
        DsItem.PushIntoStack(info.GameCoin)
        DsItem.PushIntoStack(MathUtils.ToNotNullString(wining))
        writer.PushIntoStack(DsItem)
    return True;