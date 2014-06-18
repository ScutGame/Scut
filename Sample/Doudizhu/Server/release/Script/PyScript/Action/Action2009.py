"""2009_出牌接口"""
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
        self.Cards = ''

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        
def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("Cards"):
        urlParam.Cards = httpGet.GetStringValue("Cards")
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
    position = GameTable.Current.GetUserPosition(user, table)
    if not position:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False
        return actionResult
    if (not table.PreCardData or table.PreCardData.PosId==position.Id)\
        and urlParam.Cards == '':
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("St2009_ReOutCardError")
        actionResult.Result = False
        return actionResult

    if position.IsAI:
        position.IsAI = False
        GameTable.Current.NotifyAutoAiUser(user.UserId, False)
    result = GameTable.Current.DoOutCard(user.UserId, user.Property.PositionId, table, urlParam.Cards)
    if not result[0]:
        errorCode = result[1]
        parent.ErrorCode = Lang.getLang("ErrorCode")
        if errorCode == 1:
            parent.ErrorInfo = Lang.getLang("St2009_OutCardError")
        elif errorCode == 2:
            parent.ErrorInfo = Lang.getLang("St2009_OutCardNoExist")
        elif errorCode == 3:
            parent.ErrorInfo = Lang.getLang("St2009_OutCardExitPos")
        actionResult.Result = False
        return actionResult

    GameTable.Current.ReStarTableTimer(table)
    return actionResult

def buildPacket(writer, urlParam, actionResult):

    return True