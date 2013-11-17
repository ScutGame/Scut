import clr
import sys

clr.AddReference('ZyGames.Framework.Common')
clr.AddReference('ZyGames.Framework')
clr.AddReference('ZyGames.Framework.Game')
clr.AddReference('ZyGames.Tianjiexing.Model')
clr.AddReference('ZyGames.Tianjiexing.BLL')
clr.AddReference('ZyGames.Tianjiexing.Lang')
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.BLL.Base import *
from lang import Lang
#竞技场兑换商店购买

class UrlParam():
    Result = True
    ItemId = 0
    ItemNum = 0

class ActionResult():
    sportsIntegral = 0;
    Result = True


def getUrlElement(httpGet,parent):
    urlParam = UrlParam()
    
    if httpGet.Contains("ItemId") and httpGet.Contains("ItemNum"):
        urlParam.ItemId = httpGet.GetInt("ItemId")
        urlParam.ItemNum = httpGet.GetInt("ItemNum")
    else:
    	urlParam.Result = False
    return urlParam

def takeAction(urlParam,parent):
    actionResult = ActionResult()
    userId = parent.Current.UserId
    itemConfig = ConfigCacheSet[ItemBaseInfo]().FindKey(urlParam.ItemId)
    if not itemConfig or itemConfig.Athletics <= 0:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError
        actionResult.Result = False
        return actionResult
    userId = parent.Current.UserId
    cacheSetUser = GameDataCacheSet[GameUser]()
    user = cacheSetUser.FindKey(userId.ToString())
    if not user:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError
        actionResult.Result = False
        return actionResult
    sportsIntegral = itemConfig.Athletics * urlParam.ItemNum
    if user.SportsIntegral < sportsIntegral:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode
        parent.ErrorInfo =  Lang.getLang("St7012_IntegralNotEnough");
        actionResult.Result = False
        return actionResult
    result = UserPackHelper.PackIsFull(user,BackpackType.BeiBao,urlParam.ItemNum)
    if result[0]:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode
        parent.ErrorInfo = result[1]
        actionResult.Result = False
        return actionResult
    UserItemHelper.AddUserItem(str(userId),urlParam.ItemId,urlParam.ItemNum,ItemStatus.BeiBao,0)
    user.SportsIntegral = MathUtils.Subtraction(user.SportsIntegral, sportsIntegral)
    actionResult.sportsIntegral = MathUtils.ToInt(user.SportsIntegral)
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(actionResult.sportsIntegral)
    return True;