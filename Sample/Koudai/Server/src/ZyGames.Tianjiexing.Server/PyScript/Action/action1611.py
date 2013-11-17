import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');
clr.AddReference('ZyGames.Tianjiexing.BLL.Combat');


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
from ZyGames.Tianjiexing.BLL.Combat import *


# 1611_集邮册卡牌详细信息显示接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.albumType = 0;
        self.cardID = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.generalInfo = None;
        self.itemList = [];
        self.abilityList = [];
        self.userId = '';


def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("AlbumType")\
    and httpGet.Contains("CardID"):
        urlParam.albumType = httpGet.GetEnum[AlbumType]("AlbumType")
        urlParam.cardID = httpGet.GetIntValue("CardID")
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    actionResult.userId = userId;
    # 佣兵
    if urlParam.albumType == AlbumType.General:
        actionResult.generalInfo = ConfigCacheSet[GeneralInfo]().FindKey(urlParam.cardID);
    # 装备
    if urlParam.albumType == AlbumType.Item:
        actionResult.itemList = ConfigCacheSet[ItemBaseInfo]().FindKey(urlParam.cardID);
    # 魂技
    if urlParam.albumType == AlbumType.Ability:
        actionResult.abilityList = ConfigCacheSet[AbilityInfo]().FindKey(urlParam.cardID);

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    abilityCacheSet = ConfigCacheSet[AbilityInfo]();
    # 佣兵信息
    general = actionResult.generalInfo;
    item = actionResult.itemList;
    ability = actionResult.abilityList;

    writer.PushIntoStack(MathUtils.ToNotNullString(general.GeneralName) if general else '');
    writer.PushIntoStack(MathUtils.ToNotNullString(general.PicturesID) if general else '');
    writer.PushIntoStack(MathUtils.ToNotNullString(general.PicturesID) if general else '');
    writer.PushShortIntoStack(MathUtils.ToShort(general.GeneralQuality) if general else 0);
    writer.PushShortIntoStack(general.PowerNum if general else 0);
    writer.PushShortIntoStack(general.SoulNum if general else 0);
    writer.PushShortIntoStack(general.IntellectNum if general else 0);
    writer.PushShortIntoStack(general.CareerID if general else 0);
    writer.PushIntoStack(general.LifeNum if general else 0);
    writer.PushShortIntoStack(general.GeneralLv if general else 0);
    writer.PushIntoStack(general.Description if general else '');
    writer.PushIntoStack(general.AbilityID if general else '');

    generalAbilityInfo = abilityCacheSet.FindKey(general.AbilityID if general else 0);
    writer.PushIntoStack(generalAbilityInfo.AbilityName if generalAbilityInfo else '');
    writer.PushIntoStack(MathUtils.ToInt(generalAbilityInfo.AbilityQuality) if generalAbilityInfo else 0);
    writer.PushIntoStack(generalAbilityInfo.HeadID if generalAbilityInfo else '');

    gPropertyList = None;
    ugeneral = GameDataCacheSet[UserGeneral]().FindKey(actionResult.userId, (general.GeneralID if general else 0));
    if ugeneral:
        gPropertyList = CombatHelper.GetAbility(actionResult.userId, ugeneral.GeneralID,ugeneral)
    if gPropertyList:
        writer.PushIntoStack(gPropertyList.Count);
        for property in gPropertyList:
            dsItem = DataStruct();
            dsItem.PushShortIntoStack(MathUtils.ToShort(property.AbilityType));
            dsItem.PushIntoStack(MathUtils.ToNotNullString(property.AbilityValue));
            writer.PushIntoStack(dsItem);
    else:
        writer.PushIntoStack(0);


    # 装备
    writer.PushIntoStack(item.ItemID if item else 0);
    writer.PushIntoStack(item.ItemName if item else '');
    writer.PushIntoStack(item.MaxHeadID if item else '');
    writer.PushIntoStack(MathUtils.ToInt(item.QualityType) if item else 0);

    writer.PushIntoStack(item.SalePrice if item else 0);
    writer.PushIntoStack(item.ItemDesc if item else '');
    itemid = 0;
    if item:
        itemid =item.ItemID
    itemEquAttrList = ConfigCacheSet[ItemEquAttrInfo]().FindAll(match=lambda x:x.ItemID == itemid);
    writer.PushIntoStack(len(itemEquAttrList));
    for item in itemEquAttrList:
        dsItem = DataStruct();
        dsItem.PushIntoStack(MathUtils.ToInt(item.AttributeID));
        dsItem.PushIntoStack(item.BaseNum);        
        writer.PushIntoStack(dsItem);

    # 魂技
    writer.PushIntoStack(ability.AbilityID if ability else 0);
    writer.PushIntoStack(ability.AbilityName if ability else '');
    writer.PushIntoStack(ability.MaxHeadID if ability else '');
    writer.PushIntoStack(ability.AbilityDesc if ability else '');
    writer.PushIntoStack(MathUtils.ToInt(ability.AbilityQuality) if ability else 0);
    return True;