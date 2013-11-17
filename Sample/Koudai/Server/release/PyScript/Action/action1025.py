import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');

from action import *
from System import *
from System.Collections.Generic import *
from lang import Lang
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model.ConfigModel import *
from ZyGames.Framework.Common import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *


# 玩家等级升级奖励提示
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.GoldNum = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    escalateInfo =  ConfigCacheSet[GeneralEscalateInfo]().FindKey(contextUser.UserLv, GeneralType.YongHu);
    if(escalateInfo):
        for info in escalateInfo.Award:
            actionResult.GoldNum = MathUtils.Addition(actionResult.GoldNum,info.Num);
        contextUser.IsLv = False;
    contextUser.GiftGold = MathUtils.Addition(contextUser.GiftGold,actionResult.GoldNum);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #writer.PushIntoStack(urlParam.gainNum);
    writer.PushIntoStack(actionResult.GoldNum);
    return True;