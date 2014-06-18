"""2010_出牌通知接口"""
import ReferenceLib
from action import *
from lang import Lang
from ZyGames.Framework.Common.Log import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll.Logic import *

def writeLog(msg):
    TraceLog.WriteComplement(msg)

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.UserId = 0
        self.NextUserId = 0
        self.DeckType = 0
        self.CardSize = 0
        self.Cards = []
        self.IsReNew = 0
        self.MultipleNum = 0
        self.AnteNum = 0
        self.IsAI = False
        self.PlayerList = None


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if True:
        urlParam.Result = True
    else:
        urlParam.Result = False

    return urlParam

def compeareTo(x, y):
    result = (y % 100)-(x % 100)
    if result == 0:
        result = (y / 100) - (x / 100)
    return result

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

    if table.OutCardList.Count > 0:
        cardData = table.OutCardList[table.OutCardList.Count-1];
        actionResult.UserId = cardData.UserId
        actionResult.DeckType = int(cardData.Type)
        actionResult.CardSize = cardData.CardSize
        actionResult.Cards = sorted(cardData.Cards, cmp = compeareTo)
        actionResult.NextUserId = table.OutCardUserId
        if not table.PreCardData or table.PreCardData.PosId==position.Id:
            actionResult.IsReNew = 1
        actionResult.MultipleNum = table.MultipleNum
        actionResult.AnteNum = table.AnteNum
        actionResult.PlayerList = table.Positions
        actionResult.IsAI = position.IsAI and 1 or 0

    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.UserId)
    writer.PushIntoStack(actionResult.NextUserId)
    writer.PushShortIntoStack(actionResult.DeckType)
    writer.PushIntoStack(actionResult.CardSize)
    writer.PushIntoStack(len(actionResult.Cards))
    for card in actionResult.Cards:
        dsItem = DataStruct()
        dsItem.PushIntoStack(card)
        writer.PushIntoStack(dsItem)
        
    writer.PushByteIntoStack(actionResult.IsReNew)
    writer.PushIntoStack(actionResult.MultipleNum)
    writer.PushIntoStack(actionResult.AnteNum)

    writer.PushByteIntoStack(actionResult.IsAI)
    #玩家剩余牌
    writer.PushIntoStack(actionResult.PlayerList.Length)
    for pos in actionResult.PlayerList:
        dsItem = DataStruct()
        dsItem.PushIntoStack(pos.UserId)
        dsItem.PushIntoStack(pos.CardData.Count)
        for card in pos.CardData:
            dsItem1 = DataStruct()
            dsItem1.PushIntoStack(card)
            dsItem.PushIntoStack(dsItem1)

        writer.PushIntoStack(dsItem)

    return True