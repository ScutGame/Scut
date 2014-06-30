"""100_Hello World"""
import ReferenceLib
from action import *
from ZyGames.Framework.Cache.Generic import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self._content = ''


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    urlParam.Result = True
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    actionResult._content = 'Hello World for Python!'
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult._content)

    return True