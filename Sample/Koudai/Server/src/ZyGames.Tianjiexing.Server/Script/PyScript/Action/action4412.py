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
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Tianjiexing.BLL.Base import *


class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.UserID = ''


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.embattleList=[]

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("UserID"):
        urlParam.UserID = httpGet.GetStringValue("UserID")
    else:
        urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    actionResult.embattleList = EmbattleHelper.CurrEmbattle(urlParam.UserID, True)
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(len(actionResult.embattleList))
    for info in actionResult.embattleList:
        userGenearl=GameDataCacheSet[UserGeneral]().FindKey(info.UserID, info.GeneralID);
        DsItem = DataStruct()
        DsItem.PushIntoStack(MathUtils.ToNotNullString(userGenearl.GeneralName))
        DsItem.PushIntoStack(MathUtils.ToNotNullString(userGenearl.HeadID))
        DsItem.PushIntoStack(MathUtils.ToInt(userGenearl.GeneralQuality))
        DsItem.PushShortIntoStack(userGenearl.GeneralLv)
        writer.PushIntoStack(DsItem)


    return True;