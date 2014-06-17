"""2006_叫地主通知接口"""
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
        self.IsEnd = 0
        self.LandlordId = 0
        self.LandlordName = ''
        self.MultipleNum = 0
        self.PreOpt = 0
        self.IsRob = False


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("IsCall"):
        urlParam.IsCall = httpGet.GetByteValue("IsCall")
        urlParam.IsRob = httpGet.GetByteValue("IsRob")
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

    if table.IsCallEnd:
        actionResult.IsEnd = 1;
        actionResult.LandlordId = table.LandlordId
        actionResult.LandlordName = ''
    else:
        actionResult.IsEnd = 0;
        actionResult.LandlordId = table.CallLandlordId
        actionResult.LandlordName = table.CallLandlordName
    actionResult.MultipleNum = table.MultipleNum > 0 and table.MultipleNum or table.MinMultipleNum
    actionResult.AnteNum = table.AnteNum
    actionResult.IsCall = urlParam.IsCall
    actionResult.IsRob = urlParam.IsRob
    #需要实现
    return actionResult


def buildPacket(writer, urlParam, actionResult):
    writer.PushByteIntoStack(actionResult.IsEnd)
    writer.PushIntoStack(actionResult.LandlordId)
    writer.PushIntoStack(actionResult.LandlordName)
    writer.PushIntoStack(actionResult.MultipleNum)
    writer.PushIntoStack(actionResult.AnteNum)
    writer.PushByteIntoStack(actionResult.IsCall)
    writer.PushByteIntoStack(actionResult.IsRob)

    return True