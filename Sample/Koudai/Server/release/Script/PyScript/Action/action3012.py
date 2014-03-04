import clr, sys
import time
import datetime
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');

from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *

# 3012_活动列表信息接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.activeList = [];
        self.UserID = '';
        self.contextUser = None;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    actionResult.UserID = userId;
    actionResult.contextUser = contextUser;
    # 加载 ActiveListInfo 表
    cacheSetActiveListInfo = ShareCacheStruct[FestivalInfo]();
    expample = DateTime.Now.TimeOfDay
    festivalList = cacheSetActiveListInfo.FindAll(lambda s:s.StartDate <= DateTime.Now and s.EndDate > DateTime.Now ,True);
        #and s.TimePriod.Start.TimeOfDay <= DateTime.Now.TimeOfDay and s.TimePriod.End.TimeOfDay > DateTime.Now.TimeOfDay
    if FestivalHelper.FestivalCompleted(userId,FestivalType.UpgradeGiveGift):
        festivalList = festivalList.FindAll(lambda s:s.FestivalType != FestivalType.UpgradeGiveGift);
    if FestivalHelper.FestivalCompleted(userId,FestivalType.FirstPayDoubleSpar):
        festivalList = festivalList.FindAll(lambda s:s.FestivalType != FestivalType.FirstPayDoubleSpar);
    if FestivalHelper.FestivalCompleted(userId,FestivalType.FirstReward):
        festivalList = festivalList.FindAll(lambda s:s.FestivalType != FestivalType.FirstReward);
    actionResult.activeList = festivalList;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(len(actionResult.activeList));
    for item in actionResult.activeList:
        restrainNum =FestivalHelper.FestivalSurplusNum(actionResult.UserID,item.FestivalID)
        isReceive = FestivalHelper.FestivalIsReceive(actionResult.contextUser,item.FestivalID);
        dsItem = DataStruct();
        dsItem.PushIntoStack(item.FestivalID);
        dsItem.PushIntoStack(item.FestivalName);
        dsItem.PushShortIntoStack(MathUtils.ToShort(item.FestivalType));
        dsItem.PushIntoStack(item.StartDate.ToString('yyyy-MM-dd HH:mm:ss'));
        dsItem.PushIntoStack(item.EndDate.ToString('yyyy-MM-dd HH:mm:ss'));
        dsItem.PushIntoStack(item.HeadID);
        dsItem.PushIntoStack(restrainNum);
        dsItem.PushIntoStack(item.FestivalDesc);
        dsItem.PushIntoStack(isReceive);         
        writer.PushIntoStack(dsItem);
    return True;