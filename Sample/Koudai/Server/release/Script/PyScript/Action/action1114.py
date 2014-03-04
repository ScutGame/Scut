import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');
from action import *
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

# 1114_礼包显示接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.userItemID = ''

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.prizeInfoList = []

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("UserItemID"):
        urlParam.userItemID = httpGet.GetStringValue("UserItemID", 36, 36 )
    else:
        urlParam.Result = False
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    packageCacheSet = GameDataCacheSet[UserItemPackage]();
    if not packageCacheSet:
        actionResult.Result = False;
        return actionResult;
    userItemPack = packageCacheSet.FindKey(userId);
    if not userItemPack or not userItemPack.ItemPackage:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    userItem = userItemPack.ItemPackage.Find(lambda s:s.UserItemID == urlParam.userItemID);
    if userItem == None:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    itemID = userItem.ItemID;
    if userItem.ItemType != ItemType.DaoJu or userItem.PropType != 19:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St1114_IsNotGiftBag");
        actionResult.Result = False;
        return actionResult;
    itemBaseInfoCacheStruct = ShareCacheStruct[ItemBaseInfo]();
    baseInfo = itemBaseInfoCacheStruct.FindKey(itemID.ToString());
    if not baseInfo:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    if baseInfo.ItemType != ItemType.DaoJu or baseInfo.PropType != 19:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St1114_IsNotGiftBag");
        actionResult.Result = False;
        return actionResult;
    actionResult.prizeInfoList = baseInfo.ItemPack;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    cacehSetItem =  ShareCacheStruct[ItemBaseInfo]();
    writer.PushIntoStack(len(actionResult.prizeInfoList));
    for info in actionResult.prizeInfoList:
        dsItem = DataStruct();
        dsItem.PushIntoStack(MathUtils.ToInt(info.Type));
        dsItem.PushIntoStack(info.Num);
        if info.Type == RewardType.Item:
             itemInfo = cacehSetItem.FindKey(info.ItemID);
             if(itemInfo):
                 dsItem.PushIntoStack(info.ItemID);
                 dsItem.PushIntoStack(itemInfo.HeadID);
                 dsItem.PushIntoStack(itemInfo.ItemName);
             else:
                 dsItem.PushIntoStack(0);
                 dsItem.PushIntoStack("");
                 dsItem.PushIntoStack("");
        else:
            dsItem.PushIntoStack(0);
            dsItem.PushIntoStack("")
            dsItem.PushIntoStack("");
        writer.PushIntoStack(dsItem);
    return True;