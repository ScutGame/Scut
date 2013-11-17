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
from System.Diagnostics import *


#新手引导进度
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.isPass = 0
        self.GuideId = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userExtend = parent.Current.User.UserExtend;
    tjxNoviceGuideService = TjxNoviceGuideService(parent.Current.UserId)
    if tjxNoviceGuideService.HasClose:
        actionResult.isPass = 1
        return actionResult
    
    a = tjxNoviceGuideService.CurrentProgress
    result = tjxNoviceGuideService.Check()
    if userExtend and userExtend.NoviceIsPase == True:
        actionResult.isPass = 1
    else:
        if result:
            if result[0]:
                guideInfo = result[1] 
                if guideInfo:
                    actionResult.GuideId = guideInfo.GuideId
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.isPass)
    writer.PushIntoStack(actionResult.GuideId)
    return True;