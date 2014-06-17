"""3001_成就界面接口"""
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


#成就界面接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.AchieveType = 0
        self.PageIndex = 0
        self.PageSize = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.CompleteNum = 0
        self.AchieveNum = 0
        self.PageCount = 0
        self.AchieveList = List[AchievementInfo]
        self.userID = ''

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("AchieveType")\
    and httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.AchieveType = httpGet.GetEnum[AchieveType]("AchieveType")
        urlParam.PageIndex = httpGet.GetIntValue("PageIndex")
        urlParam.PageSize = httpGet.GetIntValue("PageSize")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId;
    actionResult.userID = userId
    user = parent.Current.User

    cacheSet = GameDataCacheSet[UserAchieve]()
    achievePackage = cacheSet.FindKey(userId);
    if not achievePackage:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False
        return actionResult
    AchievementList =  List[AchievementInfo]
    if urlParam.AchieveType == AchieveType.QuanBu:
        AchievementList= ConfigCacheSet[AchievementInfo]().FindAll()
        actionResult.CompleteNum = achievePackage.AchievePackage.FindAll(match = lambda s:s.TaskStatus == TaskStatus.Complete).Count
    else :
        AchievementList= ConfigCacheSet[AchievementInfo]().FindAll(match = lambda s:s.AchieveType == urlParam.AchieveType)
        actionResult.CompleteNum = achievePackage.AchievePackage.FindAll(match = lambda s: s.AchieveType == urlParam.AchieveType and s.TaskStatus == TaskStatus.Complete).Count
    actionResult.AchieveNum = AchievementList.Count
    result = MathUtils.GetPaging[AchievementInfo](AchievementList,urlParam.PageIndex, urlParam.PageSize) 
    if result:
        actionResult.AchieveList = result[0]
        actionResult.PageCount = result[1]
    
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.CompleteNum)
    writer.PushIntoStack(actionResult.AchieveNum)
    writer.PushIntoStack(actionResult.PageCount)
    writer.PushIntoStack(len(actionResult.AchieveList))
    for info in actionResult.AchieveList:        
        isGain = AchieveTask.GetAchieveStatus(actionResult.userID,info.Id)
        dsItem = DataStruct()
        dsItem.PushIntoStack(info.Id)
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.Name))
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.HeadIcon))
        dsItem.PushShortIntoStack(MathUtils.ToShort(isGain))
        writer.PushIntoStack(dsItem)


    return True