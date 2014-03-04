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

# 12055_考古系统购买挑战次数接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.plotNpcID = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);


def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("PlotNpcID"):
        urlParam.plotNpcID = httpGet.GetIntValue("PlotNpcID");
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    goldNum = ConfigEnvSet.GetInt("Archeology.ChallengeBossGold");

    # 判断晶石是否足够使用
    if contextUser.GoldNum < goldNum:
        parent.ErrorCode = Lang.getLang("1");  # 晶石不足，跳转到充值界面
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    # 减晶石数
    contextUser.UseGold = MathUtils.Addition(contextUser.UseGold, goldNum, sys.maxint)

    userPlotPackage = GameDataCacheSet[UserPlotPackage]().FindKey(userId);
    if not userPlotPackage:
        return loadError();

    plotPackage = userPlotPackage.PlotPackage.Find(lambda x:x.CurrPlotNpcID == urlParam.plotNpcID);
    if not plotPackage:
        return loadError();
    plotPackage.BossChallengeCount += 1;

    return actionResult;

def buildPacket(writer, urlParam, actionResult):

    return True;