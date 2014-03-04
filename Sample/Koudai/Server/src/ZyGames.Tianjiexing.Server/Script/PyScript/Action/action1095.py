import clr
import sys
clr.AddReference('ZyGames.Framework.Common')
clr.AddReference('ZyGames.Framework')
clr.AddReference('ZyGames.Framework.Game')
clr.AddReference('ZyGames.Tianjiexing.Model')
clr.AddReference('ZyGames.Tianjiexing.BLL')
clr.AddReference('ZyGames.Tianjiexing.Lang')
clr.AddReference('ZyGames.Tianjiexing.Component')

from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Component.Chat import *


#新手引导进度
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.GuideId = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PrizeStr = ''
        self.isPass = 0
        self.GuideId = 0
        selt.RewardStatus = 0;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId
    contextUser = parent.Current.User
    noviceUser = GameDataCacheSet[NoviceUser]().FindKey(userId)
    if noviceUser and noviceUser.IsClose :
        if((DateTime.Now- ContextUser.LoginTime).Minutes >=10):
            UserItemHelper.AddUserItem(userId,1804,1)  
            actionResult.RewardStatus = 1
        else:
            actionResult.RewardStatus = 3
    else:
        if(noviceUser and noviceUser.IsClose == True):
            actionResult.RewardStatus = 2
    actionResult.generalInfo = ShareCacheStruct[NoviceTaskInfo]().FindKey(urlParam.GeneralID)
    return actionResult
def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.RewardStatus)
    return True;