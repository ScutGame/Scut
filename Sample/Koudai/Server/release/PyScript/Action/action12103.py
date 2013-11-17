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
from random import *
from System.Collections.Generic import *
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
from ZyGames.Tianjiexing.BLL.Base import *


class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.LairTreasureType = 0
        self.postion = 0
        self.ID = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("LairTreasureType")\
    and httpGet.Contains("postion")\
    and httpGet.Contains("ID"):
        urlParam.LairTreasureType = httpGet.GetEnum[LairTreasureType]("LairTreasureType")
        urlParam.postion = httpGet.GetIntValue("postion")
        urlParam.ID = httpGet.GetIntValue("ID")
    else:
        urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    contextUser = parent.Current.User
    LairTreasuerHelp.GetLaiReward(urlParam.LairTreasureType ,contextUser,urlParam.ID,urlParam.postion)
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    return True;