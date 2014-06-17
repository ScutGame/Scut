import ReferenceLib
from action import *
from System import *
from mathUtils import MathUtils
from lang import Lang

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
from ZyGames.Framework.Net import *
from ZyGames.Doudizhu.Script.CsScript.Action import *

#商店物品购买接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.ItemID = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)      

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("ItemID"):
        urlParam.ItemID = httpGet.GetIntValue("ItemID")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    
    shopinfo = ConfigCacheSet[ShopInfo]().FindKey(urlParam.ItemID)
    if shopinfo == None:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St_7002_ShopOffTheShelf");
        actionResult.Result = False;
        return actionResult;
    gameHall = GameHall(user)
    useGold = shopinfo.Price;
    if user.VipLv>0 :
        userGold = shopinfo.VipPrice
    if gameHall.UserGold < useGold:
        parent.ErrorCode = 1
        parent.ErrorInfo = Lang.getLang("St_2909_StGoldNumNullEnough");
        actionResult.Result = False;
        return actionResult;
    if shopinfo.ShopType == ShopType.HeadID:        
        if gameHall.CheckUserItem(urlParam.ItemID):
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St_7002_PurchasedHeadID");
            actionResult.Result = False;
            return actionResult;        
        gameHall.AddUserItem(urlParam.ItemID,shopinfo.ShopType)     
        ClientNotifier.NotifyAction(ActionIDDefine.Cst_Action1013, user, None);
    if shopinfo.ShopType == ShopType.Beans:
         user.GameCoin = MathUtils.Addition(user.GameCoin,shopinfo.GameCoin)       
    user.UseGold = MathUtils.Addition(user.UseGold,useGold)
    GameTable.Current.NotifyUserChange(user.UserId)    
    
    mallItemLog = MallItemLog()
    mallItemLog.ItemID = urlParam.ItemID
    mallItemLog.Uid = user.UserId
    mallItemLog.Num = 1
    mallItemLog.CurrencyType = MathUtils.ToInt(shopinfo.ShopType)
    mallItemLog.Amount = useGold
    mallItemLog.CreateDate = MathUtils.Now
    mallItemLog.RetailID = user.RetailId
    mallItemLog.MobileType = user.MobileType
    mallItemLog.Pid = user.Pid
    mallItemLog.ItemName = shopinfo.ShopName
    sender = DataSyncManager.GetDataSender()
    sender.Send(mallItemLog)

    #需要实现
    return actionResult

def buildPacket(writer, urlParam, actionResult):

    return True