import ReferenceLib
from action import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Logic import *
from ZyGames.Doudizhu.Bll.Base import *
from ZyGames.Doudizhu.Script.CsScript.Action import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.Pid = ''
        self.HeadIcon = ''
        self.NickName = ''
        self.GameCoin = 0
        self.Gold = 0
        self.VipLv = 0
        self.WinNum = 0
        self.FailNum = 0
        self.TitleName = ''
        self.ScoreNum = 0
        self.WinRate = 0
        self.Rooms = None
        self.RoomId = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    urlParam.Result = True;
    return urlParam;

def takeAction(urlParam, parent):
    gameRoom = GameRoom.Current
    actionResult = ActionResult();
    actionResult.Rooms = gameRoom.RoomList
    user = parent.Current.User;
    if user:
        PaymentService.Trigger(user)
        gameHall = GameHall(user)
        actionResult.Pid = user.Pid
        actionResult.HeadIcon = user.HeadIcon
        actionResult.NickName = user.NickName
        actionResult.GameCoin = user.GameCoin
        actionResult.Gold = gameHall.UserGold
        actionResult.VipLv = user.VipLv
        actionResult.WinNum = user.WinNum
        actionResult.FailNum = user.FailNum
        actionResult.TitleName = gameHall.GetTitle()
        actionResult.ScoreNum = user.ScoreNum
        actionResult.WinRate = gameHall.GetWinRate()
        
        table = gameRoom.GetTableData(user)
        if table and table.IsStarting:
            actionResult.RoomId = user.Property.RoomId
    else:
        parent.ErrorCode = Language.Instance.ErrorCode
        parent.ErrorInfo = Language.Instance.LoadDataError
        actionResult.Result = False
        return actionResult
    ClientNotifier.NotifyAction(ActionIDDefine.Cst_Action9202, user, None)
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.Pid)
    writer.PushIntoStack(actionResult.HeadIcon)
    writer.PushIntoStack(actionResult.NickName)
    writer.PushIntoStack(actionResult.GameCoin)
    writer.PushIntoStack(actionResult.Gold)
    writer.PushIntoStack(actionResult.VipLv)
    writer.PushIntoStack(actionResult.WinNum)
    writer.PushIntoStack(actionResult.FailNum)
    writer.PushIntoStack(actionResult.TitleName)
    writer.PushIntoStack(actionResult.ScoreNum)
    writer.PushIntoStack(actionResult.WinRate)
    writer.PushIntoStack(actionResult.Rooms.Count)
    for info in actionResult.Rooms:
        item = DataStruct()
        item.PushIntoStack(info.Id)
        item.PushIntoStack(info.Name)
        item.PushShortIntoStack(info.MultipleNum)
        item.PushIntoStack(info.MinGameCion)
        item.PushIntoStack(info.GiffCion)
        item.PushIntoStack(info.Description)
        item.PushIntoStack(info.AnteNum)
        writer.PushIntoStack(item)
    writer.PushIntoStack(actionResult.RoomId)

    return True;