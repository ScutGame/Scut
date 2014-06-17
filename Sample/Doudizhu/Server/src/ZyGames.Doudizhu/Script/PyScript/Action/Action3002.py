"""3002_成就详情接口"""
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


#成就详情接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.AchieveID = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.AchieveName = ''
        self.AchieveType = 0
        self.HeadID = ''
        self.AchieveDesc = ''
        self.IsComplete = 1
        self.CompleteNum = 0
        self.AchieveNum = 0


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("AchieveID"):
        urlParam.AchieveID = httpGet.GetIntValue("AchieveID")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId;
    user = parent.Current.User

    achievement = ConfigCacheSet[AchievementInfo]().FindKey(urlParam.AchieveID)
    if achievement:
        actionResult.AchieveName = achievement.Name
        actionResult.AchieveType = achievement.Type
        actionResult.HeadID = achievement.HeadIcon
        actionResult.AchieveDesc = achievement.Description
        actionResult.AchieveNum = achievement.TargetNum
    cacheSet = GameDataCacheSet[UserAchieve]()
    achievePackage = cacheSet.FindKey(userId);
    if achievePackage:
        userAchievement = achievePackage.AchievePackage.Find(match = lambda s: s.AchieveID == urlParam.AchieveID)
        if userAchievement:
            actionResult.IsComplete = MathUtils.ToShort(userAchievement.TaskStatus)
            actionResult.CompleteNum = userAchievement.TaskNum

    #需要实现
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.AchieveName))
    writer.PushShortIntoStack(MathUtils.ToShort(actionResult.AchieveType))
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.HeadID))
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.AchieveDesc))
    writer.PushShortIntoStack(MathUtils.ToShort(actionResult.IsComplete))
    writer.PushIntoStack(actionResult.CompleteNum)
    writer.PushIntoStack(actionResult.AchieveNum)

    return True