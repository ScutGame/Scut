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
from randomUtils import ZyRandomUtils;
from mathUtils import ZyMathUtils;


#活动奖励领取接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.FestivalID = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("FestivalID"):
        urlParam.FestivalID = httpGet.GetIntValue("FestivalID")
    else:
        urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    info = ShareCacheStruct[FestivalInfo]().FindKey(urlParam.FestivalID);
    isReceive = FestivalHelper.FestivalIsReceive(contextUser,urlParam.FestivalID);
    cacheSet = GameDataCacheSet[FestivalRestrain]()
    fRest = cacheSet.FindKey(userId, urlParam.FestivalID);
    if fRest !=None and fRest.IsReceive:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1433_RewardAlreadyReceive;
        actionResult.Result = False;
        return actionResult; 
    if info != None and isReceive == 1:
        result = FestivalHelper.UseFestivalRestrain(contextUser,urlParam.FestivalID);
        if result :
            isUse = result[0];
            pricontent = result[1];
            if not isUse:
                parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
                parent.ErrorInfo = pricontent;
                actionResult.Result = False;
                return actionResult; 
            else :
                parent.ErrorCode = 1;
                parent.ErrorInfo = pricontent;
        if fRest != None:
            fRest.IsReceive = True;
        else :
            fRest = FestivalRestrain();
            fRest.UserID = userId;
            fRest.FestivalID = urlParam.FestivalID;
            fRest.RefreashDate = MathUtils.Now;
            fRest.RestrainNum = info.RestrainNum;
            fRest.IsReceive = True;
            cacheSet.Add(fRest);
    #需要实现
    return actionResult;

def buildPacket(writer, urlParam, actionResult):

    return True;