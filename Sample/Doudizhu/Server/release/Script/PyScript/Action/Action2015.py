"""2015_桌子重连通知接口"""
import ReferenceLib
from action import *
from lang import Lang
from cardAILogic import CardAILogic
from System.Collections.Generic import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Logic import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.MultipleNum = 0
        self.AnteNum = 0
        self.IsAI = 0
        self.IsShow = 0
        self.LandlordId = 0
        self.PlayerList = None
        self.BackCardData = None
        self.CodeTime = 0
        self.OutCardUserId = 0
        self.IsReNew = 0
        self.GameCoin = 0
        self.PreCardData = []
        self.DeckType = 0
        self.CardSize = 0
        self.PreUserId = 0

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
    if not user or not table:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False
        return actionResult

    position = GameTable.Current.GetUserPosition(user, table)
    actionResult.MultipleNum = table.MultipleNum > 0 and table.MultipleNum or table.MinMultipleNum
    actionResult.AnteNum = table.MinAnteNum
    actionResult.LandlordId = table.LandlordId
    actionResult.IsAI = position.IsAI and 1 or 0
    actionResult.IsShow = table.IsShow and 1 or 0
    actionResult.BackCardData = table.BackCardData
    actionResult.PlayerList = table.Positions
    actionResult.CodeTime = GameTable.Current.CodeTime
    actionResult.OutCardUserId = table.OutCardUserId
    if not table.PreCardData or table.PreCardData.PosId==position.Id:
        actionResult.IsReNew = 1
    actionResult.GameCoin = user.GameCoin
    if table.PreCardData:
        actionResult.PreUserId = table.PreCardData.UserId
        actionResult.DeckType = int(table.PreCardData.Type)
        actionResult.CardSize = table.PreCardData.CardSize
        for card in table.PreCardData.Cards:
            actionResult.PreCardData.append(card)
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.MultipleNum)
    writer.PushIntoStack(actionResult.AnteNum)
    writer.PushByteIntoStack(actionResult.IsAI)
    writer.PushByteIntoStack(actionResult.IsShow)
    writer.PushIntoStack(actionResult.LandlordId)

    writer.PushIntoStack(actionResult.PlayerList.Length)
    for pos in actionResult.PlayerList:
        dsItem = DataStruct()
        dsItem.PushIntoStack(pos.UserId)
        dsItem.PushIntoStack(pos.NickName)
        dsItem.PushIntoStack(pos.HeadIcon)
        dsItem.PushIntoStack(pos.Id)
        #剩余牌
        dsItem.PushIntoStack(pos.CardData.Count)
        for card in pos.CardData:
            dsItem1 = DataStruct()
            dsItem1.PushIntoStack(card)
            dsItem.PushIntoStack(dsItem1)
        writer.PushIntoStack(dsItem)
    #底牌
    writer.PushIntoStack(actionResult.BackCardData.Count)
    for card in actionResult.BackCardData:
        dsItem = DataStruct()
        dsItem.PushIntoStack(card)
        writer.PushIntoStack(dsItem)
        
    writer.PushIntoStack(actionResult.CodeTime)
    writer.PushIntoStack(actionResult.OutCardUserId)
    writer.PushByteIntoStack(actionResult.IsReNew)
    writer.PushIntoStack(actionResult.GameCoin)
    #上次出的牌
    writer.PushIntoStack(len(actionResult.PreCardData))
    for card in actionResult.PreCardData:
        dsItem = DataStruct()
        dsItem.PushIntoStack(card)
        writer.PushIntoStack(dsItem)
    writer.PushShortIntoStack(actionResult.DeckType)
    writer.PushIntoStack(actionResult.CardSize)
    writer.PushIntoStack(actionResult.PreUserId)

    return True