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

# 4408_圣吉塔属性兑换接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.propertyType = 0;
        self.starNum = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);


def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("PropertyType")\
    and httpGet.Contains("StarNum"):
        urlParam.propertyType = httpGet.GetEnum[PropertyType]("PropertyType");
        urlParam.starNum = httpGet.GetIntValue("StarNum");
    else:
        urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    # 更新属性加成
    percent = 100.0;
    userSJTInfo = GameDataCacheSet[UserShengJiTa]().FindKey(userId);

    # 判断星星数是否足够兑换
    if userSJTInfo.LastScoreStar < urlParam.starNum:
        return loadError();

    if urlParam.propertyType == PropertyType.Life:
        userSJTInfo.LifeNum = userSJTInfo.LifeNum + (urlParam.starNum / percent);
    elif urlParam.propertyType == PropertyType.WuLi:
        userSJTInfo.WuLiNum = userSJTInfo.WuLiNum + (urlParam.starNum / percent);
    elif urlParam.propertyType == PropertyType.Mofa:
        userSJTInfo.MofaNum = userSJTInfo.MofaNum + (urlParam.starNum / percent);
    elif urlParam.propertyType == PropertyType.FunJi:
        userSJTInfo.FunJiNum = userSJTInfo.FunJiNum + (urlParam.starNum / percent);
    else:
        return loadError();

    # 更新星星数
    userSJTInfo.LastScoreStar -= urlParam.starNum;

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
   
    return True;