"""1013_背包物品列表通知接口"""
import ReferenceLib
from action import *
from System import *
from mathUtils import MathUtils

from System.Collections.Generic import *
from ZyGames.Framework.SyncThreading import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Logic import *
from ZyGames.Doudizhu.Bll.Com.Chat import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Cache import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.PageIndex = 0
        self.PageSize = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PageCount = 0
        self.userItemList = List[UserItem]()


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.PageIndex = httpGet.GetIntValue("PageIndex")
        urlParam.PageSize = httpGet.GetIntValue("PageSize")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId;
    user = parent.Current.User
    cacheSet = GameDataCacheSet[UserItemPackage]()
    package = cacheSet.FindKey(userId);
    itemList = List[UserItem]
    if package:
        itemList = package.ItemPackage.FindAll(match = lambda s:s.Num > 0)
    result = MathUtils.GetPaging[UserItem](itemList,urlParam.PageIndex, urlParam.PageSize)
    if result:
        actionResult.userItemList = result[0]
        actionResult.PageCount = result[1]

    #需要实现
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PageCount)
    writer.PushIntoStack(len(actionResult.userItemList))
    for info in actionResult.userItemList:
        itemName = ''
        headID = ''    
        shopInfo = ConfigCacheSet[ShopInfo]().FindKey(info.ItemID)
        if shopInfo:
            itemName = shopInfo.ShopName
            headID = shopInfo.HeadID
        dsItem = DataStruct()
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.UserItemID))
        dsItem.PushIntoStack(info.ItemID)
        dsItem.PushIntoStack(MathUtils.ToNotNullString(itemName))
        dsItem.PushShortIntoStack(MathUtils.ToShort(info.ShopType))
        dsItem.PushIntoStack(info.Num)
        dsItem.PushIntoStack(MathUtils.ToNotNullString(headID))
        writer.PushIntoStack(dsItem)
    return True