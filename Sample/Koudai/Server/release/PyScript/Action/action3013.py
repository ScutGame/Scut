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
from lang import Lang
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.BLL.Base import *

# 3013_精灵祝福奖励信息接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.reward = None;

class Reward():
    def __init__(self):
        self.content = ''
        self.num = 0
        self.headPic = ''
        self.quality = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;

    # 加载数据出错
    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    userInfo = GameDataCacheSet[GameUser]().FindKey(userId);
    if not userInfo:
        return loadError();

    # 判断精灵数目 True if x >= 1 else False
    if userInfo.WizardNum >= 1:
        cacheSetFestivalInfo = ShareCacheStruct[FestivalInfo]().Find(match=lambda x:x.FestivalType == FestivalType.SpiritBlessing);
        if not cacheSetFestivalInfo or not cacheSetFestivalInfo.Reward:
            return loadError();
        
        rewardInfo = UserPrayHelper.GetUserTake(cacheSetFestivalInfo.Reward.ToList(),userId,1);
       
        if not rewardInfo:
            return loadError();

        userInfo.WizardNum = userInfo.WizardNum - 1;

        itemInfo = rewardInfo.split('*');
        reward = Reward()
        reward.content = itemInfo[0]
        reward.num = itemInfo[1]
        reward.headPic = itemInfo[2]
        reward.quality = itemInfo[3]
        actionResult.reward = reward;
    else:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St3013_NoWizard");
        actionResult.Result = False;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    reward = actionResult.reward;
    writer.PushIntoStack(reward.content);
    writer.PushIntoStack(MathUtils.ToInt(reward.num));
    writer.PushIntoStack(reward.headPic);
    writer.PushIntoStack(MathUtils.ToInt(reward.quality));
    return True;