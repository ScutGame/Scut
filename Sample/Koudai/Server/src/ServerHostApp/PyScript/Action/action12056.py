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

# 12056_考古系统开启宝箱接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.plotID = 0;
        self.plotNpcID = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.rewardList = [];

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("PlotID")\
    and httpGet.Contains("PlotNpcID"):
        urlParam.plotID = httpGet.GetIntValue("PlotID")
        urlParam.plotNpcID = httpGet.GetIntValue("PlotNpcID")
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    plotNpcInfo = ConfigCacheSet[PlotNPCInfo]().FindKey(urlParam.plotNpcID);
    if not plotNpcInfo:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    # 移除宝箱记录并开启该位置卡片
    userPlotPackage = GameDataCacheSet[UserPlotPackage]().FindKey(userId);
    plotPackage = userPlotPackage.PlotPackage.Find(match=lambda x:x.PlotID == urlParam.plotID);  # 玩家地图信息，and 类型为考古
    if not plotPackage:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    boxInfo = plotPackage.ArcheologyPackage.Find(match=lambda x:x.PlotNpcID == urlParam.plotNpcID and x.IsOpen == True);
    if not boxInfo:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    monsterInfo = plotPackage.ArcheologyPackage.Find(match=lambda x:x.Position == boxInfo.Position and x.Quality != 0);
    if monsterInfo:
        monsterInfo.IsOpen = True;  # 开启该位置的怪物卡片

    plotPackage.ArcheologyPackage.Remove(boxInfo);  # 移除该宝箱记录

    actionResult.rewardList = UserPrayHelper.GetUserTake(plotNpcInfo.BoxReward.ToList(),userId,1);

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    item = actionResult.rewardList.split('*');
    count = len(item);
    writer.PushIntoStack(item[0] if count >= 1 else '');
    writer.PushIntoStack(MathUtils.ToInt(item[1]) if count >= 2 else 0);
    writer.PushIntoStack(item[2] if count >= 3 else '');
    return True;