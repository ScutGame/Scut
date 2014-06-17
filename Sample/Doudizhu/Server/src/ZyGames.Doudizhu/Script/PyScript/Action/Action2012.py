"""2012_牌局结束通知接口"""
import ReferenceLib
from action import *
from lang import Lang
from ZyGames.Framework.Common.Log import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll.Logic import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

def writeLog(msg):
    TraceLog.WriteComplement(msg)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.IsLandlord = 0
        self.IsLandlordWin = 0
        self.ScoreNum = 0
        self.CoinNum = 0
        self.GameCoin = 0
        self.UserList = []
        self.LastCards = []
        self.LastUserId = 0
        self.MultipleNum = 0
        self.AnteNum = 0

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
    actionResult.IsLandlord = position.IsLandlord and 1 or 0
    actionResult.IsLandlordWin = table.IsLandlordWin and 1 or 0
    actionResult.ScoreNum = position.ScoreNum
    actionResult.CoinNum = position.CoinNum
    actionResult.GameCoin = user.GameCoin
    for card in table.PreCardData.Cards:
        actionResult.LastCards.append(card)
    actionResult.LastCards = sorted(actionResult.LastCards, cmp = compeareTo)
    actionResult.LastUserId = table.PreCardData.UserId
    actionResult.MultipleNum = table.MultipleNum
    actionResult.AnteNum = table.AnteNum
    for pos in table.Positions:
        if pos.UserId != user.UserId:
            actionResult.UserList.append(pos)

    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushByteIntoStack(actionResult.IsLandlord)
    writer.PushByteIntoStack(actionResult.IsLandlordWin)
    writer.PushIntoStack(actionResult.ScoreNum)
    writer.PushIntoStack(actionResult.CoinNum)
    writer.PushIntoStack(actionResult.GameCoin)
    writer.PushIntoStack(len(actionResult.UserList))
    for pos in actionResult.UserList:
        dsItem = DataStruct()
        dsItem.PushIntoStack(pos.UserId)
        dsItem.PushIntoStack(pos.CardData.Count)
        for card in pos.CardData:
            dsItem1 = DataStruct()
            dsItem1.PushIntoStack(card)
            dsItem.PushIntoStack(dsItem1)

        writer.PushIntoStack(dsItem)
    
    writer.PushIntoStack(len(actionResult.LastCards))
    for card in actionResult.LastCards:
        dsItem = DataStruct()
        dsItem.PushIntoStack(card)
        writer.PushIntoStack(dsItem)

    writer.PushIntoStack(actionResult.LastUserId)
    writer.PushIntoStack(actionResult.MultipleNum)
    writer.PushIntoStack(actionResult.AnteNum)
    #writeLog('2012:%s' % writer.GetTraceString())
    return True