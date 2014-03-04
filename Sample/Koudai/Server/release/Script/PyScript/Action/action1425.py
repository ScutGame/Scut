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
from randomUtils import ZyRandomUtils;
from mathUtils import ZyMathUtils;

# 佣兵灵魂突破接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.GeneralID = 0
        self.Ops = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.GeneralName = ''
        self.Potential = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("GeneralID")\
    and httpGet.Contains("Ops"):
        urlParam.GeneralID = httpGet.GetIntValue("GeneralID");
        urlParam.Ops = httpGet.GetIntValue("Ops");
    else:
        return False;
    return urlParam;


def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    soulGeneral = GameDataCacheSet[UserGeneral]().FindKey(userId,urlParam.GeneralID);
    generalInfo = ConfigCacheSet[GeneralInfo]().Find(lambda s:s.SoulID == urlParam.GeneralID);
    #if generalInfo is None or soulGeneral is None or soulGeneral.GeneralType is not GeneralType.Soul:
    if soulGeneral.GeneralType != GeneralType.Soul:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St1425_MercenarySoulNotEnough");
        actionResult.Result = False;
        return actionResult;
    if soulGeneral is None:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St1425_MercenarySoulNotEnough");
        actionResult.Result = False;
        return actionResult;
    if generalInfo is None:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St1425_MercenarySoulNotEnough");
        actionResult.Result = False;
        return actionResult;
    if urlParam.Ops == 1:
        parent.ErrorCode = urlParam.Ops;
        parent.ErrorInfo = Lang.getLang("St1425_MercenaryOverfulfil");
        actionResult.Result = False;
        return actionResult;
    if urlParam.Ops == 2:
        PotentialArry = ConfigEnvSet.GetString("User.GeneralCardPotential").Split(',');
        if len(PotentialArry)==0:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            actionResult.Result = False;
            return actionResult;
        userGeneral = GameDataCacheSet[UserGeneral]().FindKey(userId,generalInfo.GeneralID);
        if userGeneral is None:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St1425_MercenaryNoRecruitment");
            actionResult.Result = False;
            return actionResult;
        if soulGeneral.AtmanNum < generalInfo.DemandNum:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St1425_OverfulfilNumNotEnough");
            actionResult.Result = False;
            return actionResult;
        actionResult.Potential = MathUtils.ToInt(PotentialArry[MathUtils.ToInt(generalInfo.GeneralQuality)-1]);
        userGeneral.Potential = MathUtils.Addition(userGeneral.Potential,actionResult.Potential);
        actionResult.GeneralName = userGeneral.GeneralName;
        soulGeneral.AtmanNum = MathUtils.Subtraction(soulGeneral.AtmanNum,generalInfo.DemandNum);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #writer.PushIntoStack(urlParam.gainNum);
    writer.PushIntoStack(actionResult.GeneralName);
    writer.PushIntoStack(actionResult.Potential);
    return True;