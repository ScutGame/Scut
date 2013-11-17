import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');
from action import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Framework.Common import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *

# 1446_传承界面接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.ops = 0
        self.heritageName = ''
        self.heritageLv = 0
        self.disGeneralName = ''
        self.userID = ''

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.heritageList = List[GeneralHeritage]()
        self.opsInfoList = List[OpsInfo]()

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("Ops"):
       urlParam.ops = httpGet.GetIntValue("Ops", 1, 3 );
    else:
       urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    urlParam.userID = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    if contextUser.HeritageList.Count > 0:
        actionResult.heritageList = contextUser.HeritageList.ToList();
        GeneralHelper.HeritageGeneral(contextUser, urlParam.ops);
    heritage = actionResult.heritageList.Find(lambda s:s.Type == HeritageType.Heritage);
    if heritage != None:
        uGeneral = GameDataCacheSet[UserGeneral]().FindKey(userId, heritage.GeneralID);
        if uGeneral == None:
            urlParam.disGeneralName = '';
        else:
            urlParam.disGeneralName = uGeneral.GeneralName;        
        generlv = heritage.GeneralLv;
    heritageGeneral = actionResult.heritageList.Find(lambda s:s.Type == HeritageType.IsHeritage);
    if heritageGeneral != None:
        uGeneral = GameDataCacheSet[UserGeneral]().FindKey(userId, heritageGeneral.GeneralID);
        if uGeneral == None:
            urlParam.heritageName = '';
        else:
            urlParam.heritageName = uGeneral.GeneralName;
        urlParam.heritageLv = heritageGeneral.GeneralLv;   
    
    if not MathUtils.IsNullOrDbNull(GameConfigSet.HeritageList):
        actionResult.opsInfoList = JsonUtils.Deserialize[List[OpsInfo]](GameConfigSet.HeritageList);
        opsInfo = actionResult.opsInfoList.Find(lambda s:s.Type == urlParam.ops);
        if opsInfo != None and opsInfo.VipLv > contextUser.VipLv:
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
            actionResult.Result = False;
            return actionResult;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.heritageList.Count);
    for item in actionResult.heritageList:
        userGeneral = GameDataCacheSet[UserGeneral]().FindKey(urlParam.userID, item.GeneralID);
        general = ConfigCacheSet[GeneralInfo]().FindKey(item.GeneralID);
        dsItem = DataStruct();
        dsItem.PushShortIntoStack(MathUtils.ToShort(item.Type));
        if userGeneral == None:
            dsItem.PushIntoStack(0);
        else:
            dsItem.PushIntoStack(userGeneral.GeneralID);
        if userGeneral == None:
            dsItem.PushIntoStack('');
        else:
            dsItem.PushIntoStack(ObjectExtend.ToNotNullString(userGeneral.GeneralName));
        if general == None:
            dsItem.PushIntoStack('');
        else:
            dsItem.PushIntoStack(ObjectExtend.ToNotNullString(general.BattleHeadID));
        dsItem.PushShortIntoStack(item.GeneralLv);
        dsItem.PushShortIntoStack(item.PowerNum);
        dsItem.PushShortIntoStack(item.SoulNum);
        dsItem.PushShortIntoStack(item.IntellectNum);
        if userGeneral == None:
            dsItem.PushShortIntoStack(MathUtils.ToShort(0));
        else:
            dsItem.PushShortIntoStack(MathUtils.ToShort(userGeneral.GeneralQuality));
        writer.PushIntoStack(dsItem);
    writer.PushIntoStack(actionResult.opsInfoList.Count);

    for item in actionResult.opsInfoList:
        dsItem = DataStruct();
        dsItem.PushIntoStack(item.Type);
        dsItem.PushShortIntoStack(item.VipLv);
        dsItem.PushIntoStack(item.UseGold);
        dsItem.PushIntoStack(item.ItemID);
        dsItem.PushIntoStack(item.ItemNum);
        writer.PushIntoStack(dsItem);
    writer.PushIntoStack(MathUtils.ToNotNullString(urlParam.heritageName));
    writer.PushShortIntoStack(MathUtils.ToShort(urlParam.heritageLv));
    writer.PushIntoStack(MathUtils.ToNotNullString(urlParam.disGeneralName));

    return True;