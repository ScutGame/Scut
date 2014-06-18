"""2014_托管通知接口"""
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
        self.UserId = 0
        self.Status = 0


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("Status"):
        urlParam.Status = httpGet.GetByteValue("Status")
    else:
        urlParam.Result = False
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    if not user:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False
        return actionResult
    actionResult.UserId = user.UserId
    actionResult.Status = urlParam.Status
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.UserId)
    writer.PushByteIntoStack(actionResult.Status)
    return True