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


#12001_转盘界面接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.IsFree = 0
        self.FreeNum = 0
        self.DailList = List[DialInfo]        
        self.UserCoin = 0
        self.UserGold = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if True:
        urlParam.Result = True
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId;
    user = parent.Current.User
    gameRoom = GameRoom.Current
    dailyFreeNum = ConfigEnvSet.GetInt("User.DailyFreeNum", 3);
    useNum = 0
    userRestrain = GameDataCacheSet[UserDailyRestrain]().FindKey(userId)
    if userRestrain!=None:
        gameRoom.RefreshRestrain(userRestrain)
        if userRestrain.RestrainProperty!= None:
            useNum = userRestrain.RestrainProperty.DialFreeNum
    if dailyFreeNum > useNum:
        actionResult.FreeNum = MathUtils.Subtraction(dailyFreeNum,useNum)
    else:
        actionResult.IsFree = 1;
    actionResult.DailList = ConfigCacheSet[DialInfo]().FindAll();
    actionResult.UserCoin = user.GameCoin
    gameHall = GameHall(user)
    actionResult.UserGold = gameHall.UserGold
    #需要实现
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    postion = 0
    writer.PushShortIntoStack(actionResult.IsFree)
    writer.PushIntoStack(actionResult.FreeNum)
    writer.PushIntoStack(len(actionResult.DailList))
    for info in actionResult.DailList:
        postion = MathUtils.Addition(postion, 1);
        Probability = PythonHelper.TransformString(info.Probability)
        dsItem = DataStruct()
        dsItem.PushIntoStack(postion)
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.HeadID))
        dsItem.PushIntoStack(MathUtils.ToNotNullString(Probability))
        dsItem.PushIntoStack(MathUtils.ToNotNullString(info.ItemDesc))
        dsItem.PushIntoStack(info.GameCoin)
        writer.PushIntoStack(dsItem)
    writer.PushIntoStack(actionResult.UserCoin)
    writer.PushIntoStack(actionResult.UserGold)
    return True