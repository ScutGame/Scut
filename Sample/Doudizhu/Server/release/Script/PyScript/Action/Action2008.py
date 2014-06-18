"""2008_明牌通知接口"""
import ReferenceLib
from action import *
from lang import Lang
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll.Logic import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.Cards = None
        self.MultipleNum = 0
        self.AnteNum = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if True:
        urlParam.Result = True
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    table = GameRoom.Current.GetTableData(user)
    if not table or not user:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False
        return actionResult
    actionResult.Cards = GameTable.Current.GetLandlordCardData(table)
    actionResult.MultipleNum = table.MultipleNum
    actionResult.AnteNum = table.AnteNum
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.Cards.Count)
    for cardId in actionResult.Cards:
        dsItem = DataStruct()
        dsItem.PushIntoStack(cardId)
        writer.PushIntoStack(dsItem)
    writer.PushIntoStack(actionResult.MultipleNum)
    writer.PushIntoStack(actionResult.AnteNum)


    return True