"""排行榜.py系统公告"""
import ReferenceLib
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll import *

def inquiry(httpGet, head, writer):
    rankType = httpGet.GetIntValue("type")
    topNum = httpGet.GetIntValue("top")
    pageCount = 0
    rankList = None
    rankobject = None

    if rankType == int(RankType.GameCoin):    
        rankobject = RankingFactory.Get[UserRank](BeansRanking.RankingKey)
    elif rankType == int(RankType.Wining):
        rankobject = RankingFactory.Get[UserRank](WinRanking.RankingKey)
    result = rankobject.GetRange(1, topNum)
    if result:
        rankList = result[0]
        pageCount = result[1]
        jsonstr =  MathUtils.ToJson(rankList)
        writer.PushIntoStack(jsonstr)
