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

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.StarNum=0
        self.EffNum=''
        self.IsRountStar=0
def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId
    gameUser = parent.Current.User
    userShengJiTa = GameDataCacheSet[UserShengJiTa]().FindKey(userId)    #获取玩家信息
    actionResult.StarNum=userShengJiTa.ScoreStar

    if userShengJiTa.IsYesterday == 1:
        actionResult.EffNum = 0;
    else:
        actionResult.EffNum=actionResult.StarNum/4
    userShengJiTa.IsRountStar=0
    userShengJiTa.LastScoreStar=0
    userShengJiTa.SJTStatus=1
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.StarNum)
    writer.PushIntoStack(actionResult.EffNum)
    return True;