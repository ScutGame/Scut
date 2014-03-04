import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');

from action import *
from lang import Lang
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *

# 1484_佣兵技能更换接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.generaID = 0;
        self.abilityID = 0;
        self.userItemID = '';
        self.ops = 0;
        self.Position = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("Ops")\
    and httpGet.Contains("AbilityID")\
    and httpGet.Contains("UserItemID")\
    and httpGet.Contains("GeneralID"):
        urlParam.ops = httpGet.GetIntValue("Ops", 0, 1 )
        urlParam.abilityID = httpGet.GetIntValue("AbilityID")
        urlParam.userItemID = httpGet.GetStringValue("UserItemID", 36, 36 )
        urlParam.generaID = httpGet.GetIntValue("GeneralID")
        urlParam.Position = httpGet.GetIntValue("Position")
    else:
        urlParam.Result = False;
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.UserId;

    # 获取匹配的 GeneralID 记录
    cacheSetGeneral = ConfigCacheSet[GeneralInfo]();
    general = cacheSetGeneral.FindKey(urlParam.generaID.ToString())

    cacheSetAbility = ConfigCacheSet[AbilityInfo]();
    ability = cacheSetAbility.FindKey(urlParam.abilityID.ToString());

    # 根据 UserID 和 GeneralID 获取匹配的一条记录
    cacheSetUserGeneral = GameDataCacheSet[UserGeneral]();
    userGeneral = cacheSetUserGeneral.FindKey(userId.ToString(),urlParam.generaID.ToString());

    # 根据 UserID 获取 AbilityList
    cacheSetUserAbility = GameDataCacheSet[UserAbility]();
    userAbility = cacheSetUserAbility.FindKey(userId.ToString());

    if (userGeneral == None or userAbility == None or general == None or ability == None):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
        actionResult.Result = False;
        return actionResult;

   

    # 获取 JSON 中的一个匹配记录对象
    userAbilityInfo = userAbility.AbilityList.Find(lambda x: x.UserItemID ==  urlParam.userItemID);
    if not userAbilityInfo:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1484_OperateDefaultAbility;
        actionResult.Result = False;
        return actionResult;

    # 当该魂技的值已经存在且佣兵的值不为0时，直接返回
    #if userAbilityInfo.AbilityID == urlParam.abilityID and userAbilityInfo.GeneralID == userGeneral.GeneralID:
    ##if userAbilityInfo.AbilityID == urlParam.abilityID and userGeneral.GeneralID == urlParam.generaID:
    #    actionResult.Result = False;
    #    return actionResult;

    # 不允许对默认附带魂技进行操作(不管是装配还是卸下)
    if general.AbilityID == ability.AbilityID:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St1484_OperateDefaultAbilityError");
        actionResult.Result = False;
        return actionResult;

    if urlParam.ops == 0: # 装配
        # 查找该佣兵是否已经存在该魂技
        isExist = userAbility.AbilityList.Find(lambda x: x.AbilityID ==  urlParam.abilityID and x.GeneralID == userGeneral.GeneralID);
        if isExist:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St1484_GeneralAndAbilityIsExist");
            actionResult.Result = False;
            return actionResult;

        # 更换魂技(根据该佣兵ID和位置将原来的记录赋0)
        generalAbilitiyRecord = userAbility.AbilityList.Find(lambda x: x.GeneralID ==  urlParam.generaID and x.Position ==  urlParam.Position);
        if generalAbilitiyRecord:
            generalAbilitiyRecord.GeneralID = 0;
            generalAbilitiyRecord.Position = 0;

        userAbilityInfo.GeneralID = urlParam.generaID;
        userAbilityInfo.Position = urlParam.Position;
    else: # 卸下
        userAbilityInfo.GeneralID = 0;
        userAbilityInfo.Position = 0;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    return True;