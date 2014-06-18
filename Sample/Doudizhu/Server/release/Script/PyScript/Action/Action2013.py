"""2013_逃跑通知接口【未完成】"""
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
        self.FleeUserId = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.FleeUserId = 0
        self.FleeNickName = ''
        self.GameCoin = 0
        self.ScoreNum = 0
        self.InsScoreNum = 0
        self.InsCoinNum = 0



def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("FleeUserId"):
        urlParam.FleeUserId = httpGet.GetIntValue("FleeUserId")
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
    #需要实现
    actionResult.FleeUserId = urlParam.FleeUserId
    if actionResult.FleeUserId > 0:
        fuser = GameTable.Current.GetUser(actionResult.FleeUserId)
        if fuser:
            actionResult.FleeNickName = fuser.NickName
    table = GameRoom.Current.GetTableData(user)
    if table:
        pos = GameTable.Current.GetUserPosition(user, table)
        actionResult.InsScoreNum = pos.ScoreNum
        actionResult.InsCoinNum = pos.CoinNum
    actionResult.GameCoin = user.GameCoin
    actionResult.ScoreNum = user.ScoreNum
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.FleeUserId)
    writer.PushIntoStack(actionResult.FleeNickName)
    writer.PushIntoStack(actionResult.GameCoin)
    writer.PushIntoStack(actionResult.ScoreNum)
    writer.PushIntoStack(actionResult.InsScoreNum)
    writer.PushIntoStack(actionResult.InsCoinNum)


    return True