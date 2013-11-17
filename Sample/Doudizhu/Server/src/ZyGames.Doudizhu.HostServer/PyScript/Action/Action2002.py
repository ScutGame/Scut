"""2002_房间离开接口"""
import clr, sys
from action import *
clr.AddReference('ZyGames.Framework.Game')
clr.AddReference('ZyGames.Doudizhu.Lang')
clr.AddReference('ZyGames.Doudizhu.Model')
clr.AddReference('ZyGames.Doudizhu.Bll')
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll.Logic import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User;
    GameRoom.Current.Exit(user)
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    return True