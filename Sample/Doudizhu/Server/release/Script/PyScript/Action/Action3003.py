"""3003_任务界面接口"""

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


#任务界面接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.TaskType = 0
        self.PageIndex = 0
        self.PageSize = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PageCount = 0
        self.TaskList = List[TaskInfo]
        self.userID = ''

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("TaskType")\
    and httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.TaskType = httpGet.GetEnum[TaskType]("TaskType")
        urlParam.PageIndex = httpGet.GetIntValue("PageIndex")
        urlParam.PageSize = httpGet.GetIntValue("PageSize")
    else:
        urlParam.Result = False
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    actionResult.userID = parent.Current.User.PersonalId;
    AchieveTask.RefreashUserTask(actionResult.userID)
    taskList =  ConfigCacheSet[TaskInfo]().FindAll(match = lambda s:s.TaskType == urlParam.TaskType)
    result = MathUtils.GetPaging[TaskInfo](taskList,urlParam.PageIndex, urlParam.PageSize) 
    if result:
        actionResult.TaskList = result[0]
        actionResult.PageCount = result[1]

    #需要实现
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PageCount)
    writer.PushIntoStack(len(actionResult.TaskList))
    for info in actionResult.TaskList:
        CompleteNum = 0
        IsReceive = 1
        usertaskInfo = AchieveTask.GetUserTaskInfo(actionResult.userID,info.TaskID)
        if usertaskInfo:
            CompleteNum = usertaskInfo.TaskNum
            if CompleteNum>info.RestraintNum:
                CompleteNum =info.RestraintNum
            IsReceive = MathUtils.ToShort(usertaskInfo.TaskStatus)
        dsItem = DataStruct()
        dsItem.PushIntoStack(info.TaskID)
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.TaskName))
        dsItem.PushIntoStack(CompleteNum)
        dsItem.PushIntoStack(info.RestraintNum)
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.TaskDesc))
        dsItem.PushIntoStack(info.GameCoin)
        dsItem.PushShortIntoStack(IsReceive)
        writer.PushIntoStack(dsItem)
    return True