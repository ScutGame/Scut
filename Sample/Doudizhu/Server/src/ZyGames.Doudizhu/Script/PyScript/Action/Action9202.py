"""9202_公告广播通知接口"""
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
from ZyGames.Doudizhu.Bll.Com.Chat import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.GameType = 0
        self.ServerID = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.MessageList = None


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    urlParam.Result = True

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    if not user:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False
        return actionResult

    broadcastService = DdzBroadcastService(user)
    actionResult.MessageList = broadcastService.GetMessages()
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.MessageList.Count)
    for info in actionResult.MessageList:
        dsItem = DataStruct()
        dsItem.PushShortIntoStack(int(info.NoticeType))
        dsItem.PushIntoStack(info.Content)
        dsItem.PushIntoStack(int(1))
        writer.PushIntoStack(dsItem)


    return True