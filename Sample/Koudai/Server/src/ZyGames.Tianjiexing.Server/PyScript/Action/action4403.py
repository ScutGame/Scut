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
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Tianjiexing.Component.Chat import *
from ZyGames.Tianjiexing.Model import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.PropertyType = 0
        self.EffNum = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.IsTireNum=0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("PropertyType")\
    and httpGet.Contains("EffNum"):
        urlParam.PropertyType = httpGet.GetEnum[PropertyType]("PropertyType")
        urlParam.EffNum = httpGet.GetIntValue("EffNum")
    else:
        urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId
    userShengJiTa = GameDataCacheSet[UserShengJiTa]().FindKey(userId)    #获取玩家信息
    percent = 100.0;
    if urlParam.PropertyType==PropertyType.Life:
        userShengJiTa.LifeNum=userShengJiTa.LifeNum+(urlParam.EffNum/percent)
    elif urlParam.PropertyType==PropertyType.WuLi:
        userShengJiTa.WuLiNum=userShengJiTa.WuLiNum+(urlParam.EffNum/percent)
    elif urlParam.PropertyType==PropertyType.FunJi:
        userShengJiTa.FunJiNum=userShengJiTa.FunJiNum+(urlParam.EffNum/percent)
    elif urlParam.PropertyType==PropertyType.Mofa:
        userShengJiTa.MofaNum=userShengJiTa.MofaNum+(urlParam.EffNum/percent)
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    return True;