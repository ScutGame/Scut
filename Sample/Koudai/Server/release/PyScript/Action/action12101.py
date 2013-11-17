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
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *
#取宝详细接口
#龙穴取宝类型
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.LairTreasureType = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.LairList = []
        self.LastNum = 0
        self.ActivityEndTime = ""
        self.ExpendNum = 0
        self.HaveNum = 0
        self.position = 0
        self.UserType = 0
        self.MaxNum = 0
def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("LairTreasureType"):
        urlParam.LairTreasureType = httpGet.GetIntValue("LairTreasureType")
    else:
        urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    #需要实现
    contextUser = parent.Current.User
    userExtend = contextUser.UserExtend
    LairTreasure=ShareCacheStruct[LairTreasureInfo]().FindKey(urlParam.LairTreasureType)
    if not LairTreasure:
         parent.ErrorCode = Lang.getLang("ErrorCode");
         parent.ErrorInfo = Lang.getLang("St12101_NotLairTreasure");
         actionResult.Result = False;
         return actionResult;
    actionResult.LairList=LairTreasure.LairTreasureList
    actionResult.ExpendNum=LairTreasure.UseNum
    if LairTreasure.UseType==0:
        actionResult.HaveNum=contextUser.GameCoin
    if LairTreasure.UseType==1:
        actionResult.HaveNum=contextUser.GoldNum
    actionResult.UserType = LairTreasure.UseType
    actionResult.MaxNum = LairTreasure.LairTreasureNum
    if(userExtend.LairDate.Date != DateTime.Now.Date):
        userExtend.LairNum = 0
        userExtend.LairDate = DateTime.Now
    if(userExtend.DaLairDate.Date != DateTime.Now.Date):
        userExtend.DaLairNum = 0
        userExtend.DaLairDate = DateTime.Now
    if(userExtend.ZhongLairDate.Date != DateTime.Now.Date):
        userExtend.ZhongLairNum = 0
        userExtend.ZhongLairDate = DateTime.Now
    if urlParam.LairTreasureType == MathUtils.ToInt(LairTreasureType.ZhuanJiaQuBao):
        #actionResult.MaxNum = ConfigEnvSet.GetInt("UserLair.Num")
        actionResult.LastNum = MathUtils.Subtraction(actionResult.MaxNum,userExtend.LairNum)
    if urlParam.LairTreasureType == MathUtils.ToInt(LairTreasureType.DaShiQuBao):
        #actionResult.MaxNum = ConfigEnvSet.GetInt("UserDaLair.Num")
        actionResult.LastNum = MathUtils.Subtraction(actionResult.MaxNum,userExtend.DaLairNum)
    if urlParam.LairTreasureType == MathUtils.ToInt(LairTreasureType.ZongShiQuBao):
        #actionResult.MaxNum = ConfigEnvSet.GetInt("UserZhongLair.Num")
        actionResult.LastNum = MathUtils.Subtraction(actionResult.MaxNum,userExtend.ZhongLairNum)
    cacheSetFes =  ShareCacheStruct[FestivalInfo]();
    festivalInfo = cacheSetFes.Find(lambda s:s.FestivalType==FestivalType.KaoGu);
    if festivalInfo:
        index = MathUtils.ToInt(MathUtils.DiffDate(DateTime.Now, festivalInfo.EndDate).TotalDays);
        if(index==0):
            actionResult.ActivityEndTime = Lang.getLang("Today")
        else:
            if(index == -1):
                actionResult.ActivityEndTime = Lang.getLang("Tomorrow")
            else:
                if(index < -1):
                    actionResult.ActivityEndTime = festivalInfo.EndDate.ToString(Lang.getLang("DateFormatMMdd"))

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(len(actionResult.LairList))
    for info in actionResult.LairList:
        DsItem = DataStruct()
        DsItem.PushIntoStack(info.LairPosition)
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.HeadID))
        writer.PushIntoStack(DsItem)
    writer.PushIntoStack(actionResult.LastNum)
    writer.PushIntoStack(actionResult.ActivityEndTime)
    writer.PushIntoStack(actionResult.ExpendNum)
    writer.PushIntoStack(actionResult.HaveNum)
    writer.PushIntoStack(actionResult.UserType)
    writer.PushIntoStack(actionResult.MaxNum)
    return True;