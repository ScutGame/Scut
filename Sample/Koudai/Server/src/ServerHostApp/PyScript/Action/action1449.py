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

# 1449_传承接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.ops = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

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
    contextUser = parent.Current.User;

    heritageList = List[GeneralHeritage]();
    if contextUser.HeritageList.Count > 0:
        heritageList = contextUser.HeritageList.ToList();
        heritage = heritageList.Find(lambda m:m.Type == HeritageType.Heritage);
        gheritage = heritageList.Find(lambda m:m.Type == HeritageType.IsHeritage);
        if heritage == None:
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St1418_HeritageNotEnough;
            actionResult.Result = False;
            return actionResult;
        elif gheritage == None:
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St1419_IsHeritageNotEnough;
            actionResult.Result = False;
            return actionResult;
        cacheSet = GameDataCacheSet[UserGeneral]();
        general = cacheSet.FindKey(userId, heritage.GeneralID);
        heritagegeneral = cacheSet.FindKey(userId, gheritage.GeneralID);
            
        if GeneralHelper.IsGeneralEqu(userId, heritage.GeneralID):
             parent.ErrorCode = Lang.getLang("ErrorCode");
             parent.ErrorInfo = Lang.getLang("St1449_GeneralHaveEqu");
             actionResult.Result = False;
             return actionResult;
        if GeneralHelper.IsGeneralCrystal(userId, heritage.GeneralID):
             parent.ErrorCode = Lang.getLang("ErrorCode");
             parent.ErrorInfo = Lang.getLang("St1449_GeneralHaveCrystal");
             actionResult.Result = False;
             return actionResult;
        if GeneralHelper.IsGeneralAbility(userId, heritage.GeneralID):
             parent.ErrorCode = Lang.getLang("ErrorCode");
             parent.ErrorInfo = Lang.getLang("St1449_GeneralHaveAbility");
             actionResult.Result = False;
             return actionResult;

        if general == None or heritagegeneral == None or general.GeneralID == heritagegeneral.GeneralID:
            actionResult.Result = False;
            return actionResult;
        if general.GeneralID == heritagegeneral.GeneralID:
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St1419_HeritageNotInIsHeritage;
            actionResult.Result = False;
            return actionResult;
        if general.HeritageType == HeritageType.Heritage:
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            parent.ErrorInfo = LanguageManager.GetLang().St1419_HeritageInUse;
            actionResult.Result = False;
            return actionResult;
        ishgenerLv = heritagegeneral.GeneralLv;
        useGold = 0;
        opsType = 0;
        vipLv = 0;
        if urlParam.ops == 1:
            parent.ErrorCode = urlParam.ops;
            list = isNomalHeritage(userId, urlParam.ops);
            isNomal = False;            
            itemName ='';
            if list.Count >0:
                isNomal=list[0];
            if list.Count >1:
                itemInfo=list[1];
                if itemInfo is not None:
                    itemName=itemInfo.ItemName;
            if not isNomal:
                parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
                parent.ErrorInfo = LanguageManager.GetLang().St1419_DanInsufficientHeritage %itemName;
                actionResult.Result = False;
                return actionResult;
        elif urlParam.ops == 2:
            opsType = 2;
            parent.ErrorCode =opsType;
            useGold = HeritageUseGold(opsType, vipLv);
            parent.ErrorCode = urlParam.ops;
            parent.ErrorInfo = LanguageManager.GetLang().St1419_ExtremeHeritage %useGold;
            actionResult.Result = False;
            return actionResult;
        elif urlParam.ops == 3:
            opsType = 2;
            useGold = HeritageUseGold(opsType, vipLv);
            if vipLv > contextUser.VipLv:
                parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
                parent.ErrorInfo  = LanguageManager.GetLang().St_VipNotEnough;
                actionResult.Result = False;
                return actionResult;

            if contextUser.GoldNum < useGold:
                parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
                parent.ErrorInfo  = LanguageManager.GetLang().St_GoldNotEnough;
                actionResult.Result = False;
                return actionResult;
            if gheritage.opsType != opsType:
                parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
                actionResult.Result = False;
                return actionResult;
            contextUser.UseGold = MathUtils.Addition(contextUser.UseGold, useGold);     
        parent.ErrorCode = urlParam.ops;
        heritagegeneral.GeneralLv = gheritage.GeneralLv;
        general.GeneralStatus = GeneralStatus.YinCang;
        lv = MathUtils.Subtraction(heritagegeneral.GeneralLv,ishgenerLv)
        for i in range(0,lv):
            GeneralHelper.GeneralUpgradeproperty(heritagegeneral)
        embattleList = GameDataCacheSet[UserEmbattle]().FindAll(userId, lambda s:s.GeneralID == general.GeneralID,True);
        for embattle in embattleList:
            embattle.GeneralID = 0;
        contextUser.HeritageList.Remove(heritage);
        parent.ErrorInfo = LanguageManager.GetLang().St1419_HeritageSuccess;
        parent.ErrorCode = urlParam.ops;
    return actionResult;

def HeritageUseGold(opsType,vipLv):
    useGold = 0;
    vipLv = 0;
    opsInfosList = UserHelper.HeritageOpsTypeList();
    opsInfo = opsInfosList.Find(lambda m:m.Type == opsType);
    if opsInfo != None:
        useGold = opsInfo.UseGold;
        vipLv = opsInfo.VipLv;
    return useGold;

def isNomalHeritage(userID, opsType):
    itemid = 0;
    itemnum = 0;
    itemInfo = None;
    isNomal = False;
    opsInfo =  GeneralHelper.HeritageOpsInfo(opsType);
    if opsInfo != None:
        itemid = opsInfo.ItemID;
        itemnum = opsInfo.ItemNum;
        itemInfo = ConfigCacheSet[ItemBaseInfo]().FindKey(itemid);
    if itemid > 0 and itemnum > 0:
        package = UserItemPackage.Get(userID);
        if package != None:
            num = 0;
            useritem = package.ItemPackage.FindAll(lambda s:s.ItemID == itemid);
            for item in useritem:
                num += item.Num;
            if num >= itemnum:
                UserItemHelper.UseUserItem(userID, itemid, itemnum);
                isNomal =True;
                return isNomal,itemInfo;
            else :
                return isNomal,itemInfo;
    else:
        isNomal = True;        
        return isNomal,itemInfo;

def buildPacket(writer, urlParam, actionResult):
    
    return True;