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


#12002_抽奖接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.Ops = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.Postion = 0
        self.RewardContent = ''
        self.FreeNum = 0
        self.UserCoin = 0
        self.UserGold = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("Ops"):
        urlParam.Ops = httpGet.GetIntValue("Ops")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId
    user = parent.Current.User
    gameRoom = GameRoom.Current
    dailyFreeNum = ConfigEnvSet.GetInt("User.DailyFreeNum", 3);
    useNum = 0
    useGold = 0;
    userRestrain = GameDataCacheSet[UserDailyRestrain]().FindKey(userId)
    if userRestrain == None:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    gameRoom.RefreshRestrain(userRestrain)
    gameHall = GameHall(user)
    if userRestrain.RestrainProperty!= None:
        useNum = userRestrain.RestrainProperty.DialFreeNum
    if useNum > dailyFreeNum :
        useGold =  MathUtils.Subtraction(useNum,dailyFreeNum) * 2
        useGold = MathUtils.Addition(useGold,2)
    if useNum == dailyFreeNum:
        useGold= 2
    if useGold>20:
        useGold = 20
    if urlParam.Ops == 1 and useNum >= dailyFreeNum:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St12002_FreeNotEnough");
        actionResult.Result = False;
        return actionResult;       
    #useNum = userRestrain.RestrainProperty.DialFreeNum
    if urlParam.Ops == 1:
        userRestrain.RestrainProperty.DialFreeNum = MathUtils.Addition(userRestrain.RestrainProperty.DialFreeNum, 1);
    if urlParam.Ops == 2:
        if useNum < dailyFreeNum:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St12002_FreeEnough");
            actionResult.Result = False;
            return actionResult;        
        parent.ErrorCode = urlParam.Ops 
        parent.ErrorInfo = Lang.getLang("St12002_UseGoldTurntable") %useGold;
        actionResult.Result = False;
        return actionResult;
    if urlParam.Ops == 3:        
        if gameHall.UserGold < useGold:
            parent.ErrorCode = 4
            parent.ErrorInfo = Lang.getLang("St_2909_StGoldNumNullEnough");
            actionResult.Result = False;
            return actionResult;
        user.UseGold = MathUtils.Addition(user.UseGold,useGold)
        userRestrain.RestrainProperty.DialFreeNum = MathUtils.Addition(userRestrain.RestrainProperty.DialFreeNum, 1);
    postion = gameHall.DialPrizePostion()
    dialList = ConfigCacheSet[DialInfo]().FindAll()
    if dialList.Count > postion:
        dialInfo = dialList[postion];
        user.GameCoin = MathUtils.Addition(user.GameCoin, dialInfo.GameCoin);
        actionResult.Postion = MathUtils.Addition(postion, 1);    
        actionResult.RewardContent = Lang.getLang("St12002_GoldenBeanAwards") %dialInfo.GameCoin

    #需要实现
    parent.ErrorCode = urlParam.Ops 
    if dailyFreeNum > userRestrain.RestrainProperty.DialFreeNum:
        actionResult.FreeNum = MathUtils.Subtraction(dailyFreeNum,userRestrain.RestrainProperty.DialFreeNum)
    actionResult.UserCoin = user.GameCoin
    actionResult.UserGold = gameHall.UserGold
    GameTable.Current.NotifyUserChange(user.UserId)
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushShortIntoStack(actionResult.Postion)
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.RewardContent))
    writer.PushIntoStack(actionResult.FreeNum)
    writer.PushIntoStack(actionResult.UserCoin)
    writer.PushIntoStack(actionResult.UserGold)
    return True