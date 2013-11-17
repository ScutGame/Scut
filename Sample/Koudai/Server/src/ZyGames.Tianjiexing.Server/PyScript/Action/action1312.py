import clr, sys
from mathUtils import MathUtils

#引用示例

clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.Component');
from action import *
from System.Collections.Generic import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component.Chat import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Cache import *
from ZyGames.Tianjiexing.BLL.Base import *

#一键获取命运水晶接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.goldNum = 0
        self.gameCoin = 0
        self.freeNum = 0
        self.isSale = 0
        self.issTelegrams = 0
        self.prompt = ''
        self.grayCrystalArray = List[GrayCrystal]()
        self.userLightArray = List[UserLight]()

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    userID = '';
    gainCount =14;
    return urlParam;


def takeAction(urlParam, parent):
    def compareToCrystal(x,y):
        if x == None and y == None:
            return 0;
        if x != None and y == None:
            return 1;
        if x == None:
            return -1;
        return x.CreateDate.CompareTo(y.CreateDate);

    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    urlParam.userID = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    if contextUser.VipLv < 5:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
        actionResult.Result = False;
        return actionResult;

    UserHelper.GetUserLightOpen(userId);
    if CrystalHelper.CheckAllowCrystall(contextUser) == False:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1305_FateBackpackFull;
        actionResult.Result = False;
        return actionResult;
    saleNum = CrystalHelper.SellGrayCrystal(contextUser, None);
    actionResult.prompt = CrystalHelper.AkeyHuntingLife(contextUser);    
    dailyRestrainSet = ShareCacheStruct[DailyRestrainSet]().FindKey(RestrainType.MianFeiLieMing);
    userRestrain = GameDataCacheSet[UserDailyRestrain]().FindKey(userId);
    if dailyRestrainSet is not None and  userRestrain is not None:
        if MathUtils.Now.Date == userRestrain.RefreshDate.Date:
            actionResult.freeNum = VipHelper.GetVipUseNum(contextUser.VipLv, RestrainType.MianFeiLieMing);
            actionResult.freeNum = MathUtils.Subtraction(actionResult.freeNum, userRestrain.Funtion2, 0);
        else :    
            actionResult.freeNum = VipHelper.GetVipUseNum(contextUser.VipLv, RestrainType.MianFeiLieMing);
    
    actionResult.userLightArray = GameDataCacheSet[UserLight]().FindAll(contextUser.UserID);
    allowSale = False;
    allowTake = False;
    list =CrystalHelper.GetNotSaleCrystalNum(contextUser);
    if list.Count >0:
        actionResult.grayCrystalArray = list[0];
        MathUtils.QuickSort[GrayCrystal](actionResult.grayCrystalArray,  lambda x,y:compareToCrystal(x,y))
    if list.Count >1:
        allowSale=list[1];
    if list.Count >2:
        allowTake=list[2];
  
    if allowSale is True:
        actionResult.isSale=1;
    else:
        actionResult.isSale =2;
    if allowTake is True:
        actionResult.issTelegrams = 1;
    else:
        actionResult.issTelegrams = 2;
    actionResult.goldNum = contextUser.GoldNum;
    actionResult.gameCoin= contextUser.GameCoin;
    #需要实现
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.goldNum)
    writer.PushIntoStack(actionResult.gameCoin)
    writer.PushIntoStack(actionResult.freeNum)
    writer.PushIntoStack(actionResult.isSale)
    writer.PushIntoStack(actionResult.issTelegrams)
    writer.PushIntoStack(len(actionResult.grayCrystalArray))
    for info in actionResult.grayCrystalArray:
        crystalName = '';
        headID ='';
        crystalQuality = 0;
        crystalInfo = ConfigCacheSet[CrystalInfo]().FindKey(info.CrystalID)
        if crystalInfo is not None:
            crystalName= crystalInfo.CrystalName;
            headID = crystalInfo.HeadID;
            crystalQuality = MathUtils.ToShort(crystalInfo.CrystalQuality);
        DsItem = DataStruct()
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.UserCrystalID))       
        DsItem.PushIntoStack(info.CrystalID)
        DsItem.PushIntoStack(MathUtils.ToNotNullString(crystalName))
        DsItem.PushIntoStack(MathUtils.ToNotNullString(headID))
        DsItem.PushShortIntoStack(crystalQuality)            
        writer.PushIntoStack(DsItem)

    writer.PushIntoStack(actionResult.userLightArray.Count)
    for info in actionResult.userLightArray:
        probabilityInfo = ConfigCacheSet[ProbabilityInfo]().FindKey(info.HuntingID);
        price = 0;
        if probabilityInfo is not None:
            price = probabilityInfo.Price;
        DsItem = DataStruct()
        DsItem.PushIntoStack(info.HuntingID)
        DsItem.PushIntoStack(price)
        DsItem.PushIntoStack(info.IsLight)
        writer.PushIntoStack(DsItem)

    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.prompt))

    return True; 