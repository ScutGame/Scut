"""房间进入接口"""
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
from ZyGames.Doudizhu.Bll.Base import *
from ZyGames.Doudizhu.Bll.Com.Chat import *
from ZyGames.Doudizhu.Script.CsScript.Action import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.RoomId = 0
        self.Op = 1


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.GameCoin = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("RoomId"):
        urlParam.RoomId = httpGet.GetIntValue("RoomId")
        urlParam.Op = httpGet.GetIntValue("Op")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()    
    user = parent.Current.User;
    gameRoom = GameRoom.Current
    roomInfo = gameRoom.GetRoom(urlParam.RoomId)
    if not roomInfo or not user:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False
        return actionResult
    #if not user.RemoteAddress or user.RemoteAddress=='':
    #    parent.ErrorCode = Lang.getLang("ErrorCode")
    #    parent.ErrorInfo = Lang.getLang("St2001_ConnectError")
    #    actionResult.Result = False
    #    return actionResult
    if urlParam.Op == 2:
        #续局
        GameRoom.Current.Exit(user)
    else:
        #每日赠送金豆
        result = gameRoom.CheckDailyGiffCoin(user, roomInfo)
        if result:
            parent.ErrorCode = 3
            parent.ErrorInfo = gameRoom.Tip( Lang.getLang("St2001_GiffCoin"),  roomInfo.GiffCion)
            pass

    if user.GameCoin < roomInfo.MinGameCion:
        parent.ErrorCode = 2
        parent.ErrorInfo = gameRoom.Tip( Lang.getLang("St2001_CoinNotEnough"), user.GameCoin, roomInfo.MinGameCion)
        actionResult.Result = False
        return actionResult
    
    table = GameRoom.Current.GetTableData(user)
    actionResult.GameCoin = user.GameCoin
    if table and table.IsStarting and user.Property.TableId > 0:
        GameTable.Current.SyncNotifyAction(ActionIDDefine.Cst_Action2015, user, None, None)
        return actionResult
    else:
        gameRoom.Enter(user, roomInfo)
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.GameCoin)
    return True