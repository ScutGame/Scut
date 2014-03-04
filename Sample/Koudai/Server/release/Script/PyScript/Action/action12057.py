import clr, sys
import random
import time
import datetime
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.BLL.Combat');

from lang import Lang
from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.BLL.Combat import *
from ZyGames.Tianjiexing.Model.Enum import *

# 12057_地图列表接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.plotID = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.mapList = [];
        self.userPlotPackage = None;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    # 加载数据出错
    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    # 判断玩家等级是否达到 20 级
    if contextUser.UserLv < 20:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St12057_UserLvNotEnough");
        actionResult.Result = False;
        return actionResult;


    userPlotPackage = GameDataCacheSet[UserPlotPackage]().FindKey(userId);
    # 下发地图列表
    plotList = ConfigCacheSet[PlotInfo]().FindAll(match=lambda x:x.PlotType == PlotType.KaoGuPlot); # 副本地图信息
    if not plotList or not userPlotPackage:
        return loadError();

    # 当玩家等级达到 20 级时，初始化地图数据
    plotMapList = userPlotPackage.PlotPackage.FindAll(match=lambda x:x.PlotType == PlotType.KaoGuPlot);
    if not plotMapList and contextUser.UserLv >= 20:
        UserArchaeologyHelper.InitializeMapInfo(userId);
        userPlotPackage = GameDataCacheSet[UserPlotPackage]().FindKey(userId);

    actionResult.mapList = plotList;
    actionResult.userPlotPackage = userPlotPackage.PlotPackage;

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    mapList = actionResult.mapList
    userPlotPackage = actionResult.userPlotPackage;

    # 地图列表
    writer.PushIntoStack(len(mapList));
    for info in mapList:
        dsItem = DataStruct();
        dsItem.PushIntoStack(info.PlotID);
        dsItem.PushIntoStack(info.PlotName);
        dsItem.PushIntoStack(info.BossHeadID);
        dsItem.PushIntoStack(info.KgScene);
        mapInfo = userPlotPackage.Find(match=lambda x:x.PlotID == info.PlotID);
        dsItem.PushShortIntoStack(1 if mapInfo else 0);        
        writer.PushIntoStack(dsItem);

    return True;