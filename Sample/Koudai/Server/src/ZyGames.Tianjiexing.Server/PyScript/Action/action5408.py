import clr
import sys
clr.AddReference('ZyGames.Framework.Common')
clr.AddReference('ZyGames.Framework')
clr.AddReference('ZyGames.Framework.Game')
clr.AddReference('ZyGames.Tianjiexing.Model')
clr.AddReference('ZyGames.Tianjiexing.BLL')
clr.AddReference('ZyGames.Tianjiexing.Lang')

from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from action import *
#boss战详细
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.functionEnum = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.gameActiveList = []

def getUrlElement(httpGet,parent):
    urlParam = UrlParam()
    if httpGet.Contains("FunctionEnum"):
        urlParam.functionEnum = httpGet.GetIntValue("FunctionEnum")
    else:
        urlParam.Result = False
    return urlParam
def takeAction(urlParam,parent):
    actionResult = ActionResult()
    #gameActiveList = ShareCacheStruct[GameActive]().FindAll()
    
    gameActiveList = GameActiveCenter(Uid).GetActiveList();
    if not gameActiveList:
        return actionResult
    for gameActive in gameActiveList:
        actionResult.gameActiveList = gameActiveList.FindAll(lambda x:int(x.ActiveType) == urlParam.functionEnum and x.State,True)

    return actionResult

def buildPacket(writer, urlParam, actionResult):
    #输出
    if actionResult.gameActiveList:
        writer.PushIntoStack(actionResult.gameActiveList.Count)
        for gameActive in actionResult.gameActiveList:
            ds = DataStruct()
            ds.PushIntoStack(gameActive.ActiveId)
            ds.PushIntoStack(gameActive.ActiveName)
            ds.PushIntoStack(gameActive.HeadID)
            ds.PushIntoStack(gameActive.Descption)
            ds.PushShortIntoStack(gameActive.BossLv)
            ds.PushIntoStack(gameActive.EnablePeriod)
            writer.PushIntoStack(ds)
    else:
        writer.PushIntoStack(0)
    return True;