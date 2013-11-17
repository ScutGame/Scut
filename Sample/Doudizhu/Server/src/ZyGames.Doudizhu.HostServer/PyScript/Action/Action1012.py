"""1012_卡牌数据配置接口"""
import clr, sys
from action import *
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework.Game')
clr.AddReference('ZyGames.Doudizhu.Lang')
clr.AddReference('ZyGames.Doudizhu.Model')
clr.AddReference('ZyGames.Doudizhu.Bll')
from ZyGames.Framework.Common import MathUtils
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll.Logic import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PokerList = None


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    actionResult.PokerList = GameTable.Current.PokerList
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PokerList.Count)
    for info in actionResult.PokerList:
        dsItem = DataStruct()
        dsItem.PushIntoStack(info.Id)
        dsItem.PushIntoStack(info.Name)
        dsItem.PushShortIntoStack(MathUtils.ToShort(info.Color))
        dsItem.PushShortIntoStack(info.Value)
        dsItem.PushIntoStack(info.HeadIcon)
        writer.PushIntoStack(dsItem)


    return True