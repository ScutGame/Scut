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

# 佣兵卡使用接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.UserItemID = ''
        self.Ops = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.GeneralName = ''
        self.Potential = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("UserItemID")\
    and httpGet.Contains("Ops"):
        urlParam.UserItemID = httpGet.GetStringValue("UserItemID", 36, 36);
        urlParam.Ops = httpGet.GetIntValue("Ops");
    else:
        return False;
    return urlParam;


def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    cacheSetItem = GameDataCacheSet[UserItemPackage]();
    cacheSetUserAbility = GameDataCacheSet[UserAbility]();
    cacheSetUserGeneral = GameDataCacheSet[UserGeneral]();
    cacheSetGeneral = ConfigCacheSet[GeneralInfo]();
    cacheSetItemInfo = ConfigCacheSet[ItemBaseInfo]();
    cacheSet = GameDataCacheSet[UserGeneral]();
    userItem = cacheSetItem.FindKey(userId);
    PotentialArry = ConfigEnvSet.GetString("User.GeneralCardPotential").Split(',');
    if(userItem == None or userItem.ItemPackage == None):
         parent.ErrorCode = Lang.getLang("ErrorCode");
         parent.ErrorInfo = Lang.getLang("LoadError");
         actionResult.Result = False;
         return actionResult;
    item = userItem.ItemPackage.Find(lambda s:s.UserItemID == urlParam.UserItemID);
    if(item == None):
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    itemInfo = cacheSetItemInfo.FindKey(item.ItemID);
    if(itemInfo == None):
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    generalInfo = cacheSetGeneral.FindKey(itemInfo.EffectNum);

    if(generalInfo  == None ):
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    userGeneral = cacheSetUserGeneral.FindKey(userId,generalInfo.GeneralID);
    if(userGeneral ==None):

        UserAbilityHelper.AddUserAbility(generalInfo.AbilityID, MathUtils.ToInt(userId),generalInfo.GeneralID,1);
        userGeneral = UserGeneral();
        userGeneral.UserID = userId;
        userGeneral.GeneralID = generalInfo.GeneralID;
        userGeneral.GeneralName = generalInfo.GeneralName;
        userGeneral.HeadID = generalInfo.HeadID;
        userGeneral.PicturesID = generalInfo.PicturesID;
        userGeneral.GeneralLv = MathUtils.ToShort(generalInfo.GeneralLv);
        userGeneral.LifeNum = generalInfo.LifeNum;
        userGeneral.GeneralType = GeneralType.YongBing;
        userGeneral.CareerID = generalInfo.CareerID;
        userGeneral.PowerNum = generalInfo.PowerNum;
        userGeneral.SoulNum = generalInfo.SoulNum;
        userGeneral.IntellectNum = generalInfo.IntellectNum;
        userGeneral.TrainingPower = 0;
        userGeneral.TrainingSoul = 0;
        userGeneral.TrainingIntellect = 0;
        userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
        userGeneral.AbilityID = generalInfo.AbilityID;
        userGeneral.Momentum = 0;
        userGeneral.Description = generalInfo.Description;
        userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
        userGeneral.CurrExperience = 0;
        userGeneral.Experience1 = 0;
        userGeneral.Experience2 = 0;
        userGeneral.AbilityNum = 3;
        cacheSet.Add(userGeneral);
        actionResult.GeneralName=generalInfo.GeneralName;
    else:
        if(urlParam.Ops == 0):
            parent.ErrorCode = 1;
            actionResult.Result = False;
            return actionResult;
        if(urlParam.Ops == 1):
            actionResult.Potential = MathUtils.ToInt(PotentialArry[MathUtils.ToInt(generalInfo.GeneralQuality)-1]);
            userGeneral.Potential = MathUtils.Addition(userGeneral.Potential,actionResult.Potential);

    UserItemHelper.UseUserItem(userId, item.ItemID, 1);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #writer.PushIntoStack(urlParam.gainNum);
    writer.PushIntoStack(actionResult.GeneralName);
    writer.PushIntoStack(actionResult.Potential);
    return True;