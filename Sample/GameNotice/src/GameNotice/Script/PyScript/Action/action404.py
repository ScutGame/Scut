"""404错误处理"""
import ReferenceLib
from action import *
from ZyGames.Framework.Common.Log import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.ErrorInfo = ''


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("ErrorInfo"):
        urlParam.ErrorInfo = httpGet.GetStringValue("ErrorInfo")
    else:
        urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();

    TraceLog.WriteError("Client error:{0}", urlParam.ErrorInfo)
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    return True;