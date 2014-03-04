import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');

from action import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from System import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Action import *
from ZyGames.Tianjiexing.Component import *

# 1091_购买精力提示接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.useGold = 0
        self.currDayUseNum = 0
        self.currDayUseMaxNum = 0
        self.recoverEnergy = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.UserId;

    contextUser = GameDataCacheSet[GameUser]().FindKey(userId.ToString())
    if not contextUser:
        actionResult.Result = False;
        return actionResult;
    
    # 当日已经使用次数
    cacheSet = GameDataCacheSet[UserDailyRestrain]();
    userRestrain = cacheSet.FindKey(userId.ToString());
    if userRestrain != None and DateTime.Now.Date != userRestrain.RefreshDate.Date:
        userRestrain.Funtion4 = 0;
    if userRestrain != None and userRestrain.Funtion4 >= VipHelper.GetVipUseNum(contextUser.VipLv, RestrainType.GouMaiJingLi) and DateTime.Now.Date == userRestrain.RefreshDate.Date:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1010_JingliFull;
        actionResult.Result = False;
        return actionResult;
    actionResult.currDayUseNum = userRestrain.Funtion4;

    # 使用金石数
    addNum = FestivalHelper.SurplusPurchased(userId.ToString(), FestivalType.PurchasedEnergy);
    # RestrainType.GouMaiJingLi 替换 payType
    actionResult.useGold = int(Action1010.GetPayEnergyGold(userId.ToString(), contextUser.VipLv, int(RestrainType.GouMaiJingLi)) * addNum)  # 花费晶石数
    # 当日使用最大次数
    actionResult.currDayUseMaxNum = VipHelper.GetVipUseNum(contextUser.VipLv, RestrainType.GouMaiJingLi);
    # 恢复精力数
    actionResult.recoverEnergy = GameConfigSet.RecoverEnergy;

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.currDayUseNum);
    writer.PushIntoStack(actionResult.currDayUseMaxNum);
    writer.PushIntoStack(actionResult.useGold);
    writer.PushIntoStack(actionResult.recoverEnergy);
    return True