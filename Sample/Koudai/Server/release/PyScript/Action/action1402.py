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

# 1402_酒馆佣兵列表接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.recruitList = []
        self.userId = ''
        self.contextUser = None

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)        

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    urlParam.recruitList = ConfigCacheSet[RecruitRule]().FindAll();

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    urlParam.userId = parent.Current.User.PersonalId;
    urlParam.contextUser = parent.Current.User;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    
    def getGeneralQuality(_generalQuality):
        genquality = '';
        for quality in _generalQuality:
            genquality += str(MathUtils.ToInt(quality.Quality)) + ",";
        return genquality.TrimEnd(',');

    writer.PushIntoStack(urlParam.contextUser.GoldNum);
    writer.PushIntoStack(urlParam.contextUser.GameCoin);
    writer.PushIntoStack(urlParam.recruitList.Count);
    for rule in urlParam.recruitList:
        mallPrice = MathUtils.ToInt(FestivalHelper.StoreDiscount() * rule.GoldNum);
        isFirst = 0
        rtype = MathUtils.ToEnum[RecruitType](rule.RecruitType)
        if GeneralHelper.IsFirstRecruit(urlParam.contextUser,rtype):
            isFirst = 1
        dsItem = DataStruct();
        dsItem.PushShortIntoStack(MathUtils.ToShort(rule.RecruitType));
        dsItem.PushIntoStack(MathUtils.ToNotNullString(getGeneralQuality(rule.GeneralQuality)));
        dsItem.PushIntoStack(GeneralHelper.SurplusNum(urlParam.userId, rule.FreeNum,MathUtils.ToEnum[RecruitType](rule.RecruitType)));
        dsItem.PushIntoStack(rule.FreeNum);
        dsItem.PushIntoStack(mallPrice);
        dsItem.PushIntoStack(GeneralHelper.UserQueueCodeTime(urlParam.userId, MathUtils.ToEnum[RecruitType](rule.RecruitType)));
        dsItem.PushIntoStack(MathUtils.ToShort(isFirst));
        writer.PushIntoStack(dsItem);
    return True;