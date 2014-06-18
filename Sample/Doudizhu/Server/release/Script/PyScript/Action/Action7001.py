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
from ZyGames.Doudizhu.Bll.Base import *


#商店物品列表接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.ShopType = ShopType.HeadID
        self.PageIndex = 0
        self.PageSize = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PageCount = 0
        self.GameCoin = 0
        self.GoldNum = 0
        self.ShopList = List[ShopInfo]();

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("ShopType")\
    and httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.ShopType = httpGet.GetEnum[ShopType]("ShopType")
        urlParam.PageIndex = httpGet.GetIntValue("PageIndex")
        urlParam.PageSize = httpGet.GetIntValue("PageSize")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    
    PaymentService.Trigger(user)
    shopList = ConfigCacheSet[ShopInfo]().FindAll(match=lambda s:s.ShopType == urlParam.ShopType)
    result = MathUtils.GetPaging[ShopInfo](shopList,urlParam.PageIndex, urlParam.PageSize)
    if result:
        actionResult.ShopList = result[0]
        actionResult.PageCount = result[1]
    actionResult.GameCoin = user.GameCoin
    gameHall = GameHall(user)
    actionResult.GoldNum = gameHall.UserGold
    #需要实现
    return actionResult


def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PageCount)
    writer.PushIntoStack(actionResult.GameCoin)
    writer.PushIntoStack(actionResult.GoldNum)
    writer.PushIntoStack(len(actionResult.ShopList))
    for info in actionResult.ShopList:
        dsItem = DataStruct()
        dsItem.PushIntoStack(info.ShopID)
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.ShopName))
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.HeadID))
        dsItem.PushIntoStack(info.Price)
        dsItem.PushIntoStack(info.VipPrice)
        dsItem.PushIntoStack(info.GameCoin)
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.ShopDesc))
        dsItem.PushIntoStack(info.SeqNO)
        writer.PushIntoStack(dsItem)
    return True