import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');
from action import *
from System import *
from System.Collections.Generic import *
from lang import Lang
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model.ConfigModel import *
from ZyGames.Framework.Common import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Tianjiexing.Component.Chat import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Component import *

#圣吉塔排行榜
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.RankList=[]

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    return urlParam;


def takeAction(urlParam, parent):
    actionResult = ActionResult()
    rankList = RankingFactory.Get[UserRank](ShengJiTaRanking.RankingKey)
    userRankArray = rankList.GetRange(1, 60)
    actionResult.RankList=userRankArray[0]
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(len(actionResult.RankList))
    for info in actionResult.RankList:
        DsItem = DataStruct()
        DsItem.PushIntoStack(info.SJTRankId)
        DsItem.PushIntoStack(info.NickName)
        DsItem.PushIntoStack(info.UserLv)
        DsItem.PushIntoStack(info.MaxTierNum)
        DsItem.PushIntoStack(info.ScoreStar)
        DsItem.PushIntoStack(info.HaveRankNum)
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.UserID))
        DsItem.PushIntoStack(MathUtils.ToInt(info.SJTRankType))
        writer.PushIntoStack(DsItem)
    return True;