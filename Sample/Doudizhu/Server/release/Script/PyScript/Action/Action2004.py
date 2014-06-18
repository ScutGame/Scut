"""2004_发牌通知接口"""
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
        self.LandlordId = 0
        self.LandlordName = ''
        self.CodeTime = 0
        self.BackCardData = None
        self.UserCardData = None


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

    actionResult.LandlordId = table.CallLandlordId
    actionResult.LandlordName = table.CallLandlordName
    actionResult.CodeTime = GameTable.Current.CodeTime
    actionResult.BackCardData = table.BackCardData
    actionResult.UserCardData = GameTable.Current.GetUserCardData(user, table)
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.LandlordId)
    writer.PushIntoStack(actionResult.LandlordName)
    writer.PushIntoStack(actionResult.CodeTime)
    writer.PushIntoStack(actionResult.BackCardData.Count)
    for cardId in actionResult.BackCardData:
        dsItem = DataStruct()
        dsItem.PushIntoStack(cardId)
        writer.PushIntoStack(dsItem)

    writer.PushIntoStack(actionResult.UserCardData.Count)
    for cardId in actionResult.UserCardData:
        dsItem = DataStruct()
        dsItem.PushIntoStack(cardId)
        writer.PushIntoStack(dsItem)
    return True