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
from lang import Lang
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
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Component.Chat import *

# 1610_集邮册显示接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.albumType = 0;
        self.PageIndex = 1;
        self.PageSize = 20;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.listRecord = [];
        self.PageCount = 1;

class Record:
    def __init__(self):
        self.headID = '';
        self.name = '';
        self.status = '';
        self.cardId = 0;
        self.quality = 0;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("AlbumType")\
    and httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.albumType = httpGet.GetEnum[AlbumType]("AlbumType")
        urlParam.PageIndex = httpGet.GetIntValue("PageIndex", 1, 100 )
        urlParam.PageSize = httpGet.GetIntValue("PageSize")
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    #userId = parent.Current.UserId;
    result = [];
    list = [];

    # 分页
    def getPage(objName, list):
        return MathUtils.GetPaging[objName](list,urlParam.PageIndex,urlParam.PageSize);

    # 加载数据出错
    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    
    userAlbumInfo = GameDataCacheSet[UserAlbum]().FindKey(userId);
    if not userAlbumInfo:
        return loadError();

    # 佣兵
    if urlParam.albumType == AlbumType.General:
        userAlbumList = userAlbumInfo.AlbumList.FindAll(lambda s:s.AlbumProperty == AlbumType.General);
        generalCacheSet = ShareCacheStruct[GeneralInfo]();
        generalList = generalCacheSet.FindAll();
        if not generalList:
            return loadError();
        result = getPage(GeneralInfo,generalList);
        if result :
            for item in result[0]:
                record = Record();
                record.headID = item.HeadID;
                record.name = item.GeneralName;
                general = userAlbumList.Find(lambda s:s.ID == item.GeneralID);
                if general:
                    record.status = 1;
                else:
                    record.status = 0;
                record.cardId = item.GeneralID;
                record.quality = item.GeneralQuality;
                list.append(record);

    # 装备
    elif urlParam.albumType == AlbumType.Item:
        userAlbumList = userAlbumInfo.AlbumList.FindAll(lambda s:s.AlbumProperty == AlbumType.Item);

        itemBaseCacheSet = ShareCacheStruct[ItemBaseInfo]();
        itemBaseList = itemBaseCacheSet.FindAll();
        if not itemBaseList:
            return loadError();
        result = getPage(ItemBaseInfo,itemBaseList);
        if result:
            for item in result[0]:
                record = Record();
                record.headID = item.HeadID;
                record.name = item.ItemName;
                userItem = userAlbumList.Find(lambda x:x.ID == item.ItemID);
                if userItem:
                    record.status = 1;
                else:
                    record.status = 0;
                record.cardId = item.ItemID;
                record.quality = item.QualityType;
                list.append(record);

    # 魂技
    elif urlParam.albumType == AlbumType.Ability:
        userAlbumList = userAlbumInfo.AlbumList.FindAll(lambda s:s.AlbumProperty == AlbumType.Ability);

        abilityInfoCacheSet = ShareCacheStruct[AbilityInfo]();
        abilityList = abilityInfoCacheSet.FindAll();
        if not abilityList:
            return loadError();
        result = getPage(AbilityInfo,abilityList);
        if result:
            for item in result[0]:
                record = Record();
                record.headID = item.HeadID;
                record.name = item.AbilityName;
                ability = userAlbumList.Find(lambda x:x.ID == item.AbilityID);
                if ability:
                    record.status = 1;
                else:
                    record.status = 0;
                record.cardId = item.AbilityID;
                record.quality = item.AbilityQuality;
                #userAbilityInfo = userAbilityList.Find(lambda m:m.AbilityID == item.AbilityID);
                #if userAbilityInfo:
                #    record.id = userAbilityInfo.UserItemID;
                #else:
                #    record.id = '';
                list.append(record);
    else:
        pass

    actionResult.listRecord = list;
    if result:
        actionResult.PageCount = result[1];
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PageCount);
    writer.PushIntoStack(len(actionResult.listRecord));
    for item in actionResult.listRecord:
        dsItem = DataStruct();
        dsItem.PushIntoStack(item.headID);
        dsItem.PushIntoStack(item.name);
        dsItem.PushIntoStack(item.status);
        dsItem.PushIntoStack(item.cardId);
        dsItem.PushShortIntoStack(MathUtils.ToShort(item.quality));
        writer.PushIntoStack(dsItem);
    return True;