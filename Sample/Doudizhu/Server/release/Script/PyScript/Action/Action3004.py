"""3004_任务奖励领取接口"""
import ReferenceLib
from action import *
from System import *
from mathUtils import MathUtils
from lang import Lang

from System.Collections.Generic import *
from ZyGames.Framework.SyncThreading import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Logic import *
from ZyGames.Doudizhu.Bll.Com.Chat import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Cache import *


#任务奖励领取接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.TaskID = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("TaskID"):
        urlParam.TaskID = httpGet.GetIntValue("TaskID")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    userId = parent.Current.User.PersonalId;
    AchieveTask.RefreashUserTask(userId)
    usertaskInfo = AchieveTask.GetUserTaskInfo(userId,urlParam.TaskID)
    if not usertaskInfo or usertaskInfo.TaskStatus == TaskStatus.Unfinished:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St3004_CurrentTaskIsNotCompleted");
        actionResult.Result = False;
        return actionResult;
    if usertaskInfo.TaskStatus == TaskStatus.Receive :
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St3004_CurrentTaskrewardHasReceived");
        actionResult.Result = False;
        return actionResult;

    taskInfo =  ConfigCacheSet[TaskInfo]().FindKey(urlParam.TaskID);
    if taskInfo:
        user.GameCoin = MathUtils.Addition(user.GameCoin,taskInfo.GameCoin)
        usertaskInfo.TaskStatus = TaskStatus.Receive        
        GameTable.Current.NotifyUserChange(user.UserId)
    #需要实现
    return actionResult

def buildPacket(writer, urlParam, actionResult):

    return True