"""2003_桌子进入通知接口"""
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
        self.PlayerList = None


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
    table = GameRoom.Current.GetTableDataByUserId(parent.Current.UserId)
    if not table or not user:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False
        return actionResult

    actionResult.PlayerList = table.Positions
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PlayerList.Length)
    for info in actionResult.PlayerList:
        dsItem = DataStruct()
        dsItem.PushIntoStack(info.UserId)
        dsItem.PushIntoStack(info.NickName)
        dsItem.PushIntoStack(info.HeadIcon)
        dsItem.PushIntoStack(info.Id)
        writer.PushIntoStack(dsItem)


    return True