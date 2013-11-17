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
from random import *
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
from ZyGames.Tianjiexing.BLL.Base import *
#取宝接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.LairTreasureType = 0


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.position=0
        self.LairRewardName=""
        self.LairRewardNum=0
        self.LairRewardHead=""
        self.ID=0
        self.IsItem=0
        self.ItemInfo = None
        self.LairRewardType = None

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("LairTreasureType"):
        urlParam.LairTreasureType = httpGet.GetEnum[LairTreasureType]("LairTreasureType")
    else:
        urlParam.Result = False;

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    #需要实现
    contextUser = parent.Current.User
    userExtend = contextUser.UserExtend
    if(userExtend.LairDate.Date != DateTime.Now.Date):
        userExtend.LairNum = 0
        userExtend.LairDate = DateTime.Now
    if(userExtend.DaLairDate.Date != DateTime.Now.Date):
        userExtend.DaLairNum = 0
        userExtend.DaLairDate = DateTime.Now
    if(userExtend.ZhongLairDate.Date != DateTime.Now.Date):
        userExtend.ZhongLairNum = 0
        userExtend.ZhongLairDate = DateTime.Now
    LairTreasure=ConfigCacheSet[LairTreasureInfo]().FindKey(MathUtils.ToInt(urlParam.LairTreasureType))
    if LairTreasure == None:
         parent.ErrorCode = Lang.getLang("ErrorCode");
         parent.ErrorInfo = Lang.getLang("LoadError");
         actionResult.Result = False;
         return actionResult;
    maxNum = LairTreasure.LairTreasureNum
    lairNum = 0
    if urlParam.LairTreasureType == LairTreasureType.ZhuanJiaQuBao:
        #maxNum = ConfigEnvSet.GetInt("UserLair.Num")
        lairNum = userExtend.LairNum
    if urlParam.LairTreasureType == LairTreasureType.DaShiQuBao:
        #maxNum = ConfigEnvSet.GetInt("UserDaLair.Num")
        lairNum = userExtend.DaLairNum
    if urlParam.LairTreasureType == LairTreasureType.ZongShiQuBao:
        #maxNum = ConfigEnvSet.GetInt("UserZhongLair.Num")
        lairNum = userExtend.ZhongLairNum
    
    if(lairNum >= maxNum):
         parent.ErrorCode = Lang.getLang("ErrorCode");
         parent.ErrorInfo = Lang.getLang("St12102_LairNumNot");
         actionResult.Result = False;
         return actionResult;
    
    if LairTreasure.UseType==0:
        if contextUser.GameCoin<LairTreasure.UseNum:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St12102_GameCoinNotEnough");
            actionResult.Result = False;
            return actionResult;
    if LairTreasure.UseType==1:
        if contextUser.GoldNum<LairTreasure.UseNum:
            parent.ErrorCode = 1;
            parent.ErrorInfo = Lang.getLang("St12102_PayGoldNotEnough");
            actionResult.Result = False;
            return actionResult;
    #index = RandomUtils.GetRandom(0,LairTreasure.LairTreasureList.Count)
    #count =LairTreasure.LairTreasureList.Count
    #precent = []
    #for lairTreasure in LairTreasure.LairTreasureList:
    #    precent.append(lairTreasure.Probability*1000)
    index = LairTreasuerHelp.GetLaiRewardIndex(LairTreasure.LairTreasureList.ToList())
    #index = 0
    #actionResult.postion=LairTreasuerHelp.ChestLairTreasuerPosition(urlParam.LairTreasureType)
    #LairTreasure=ConfigCacheSet[LairTreasureInfo]().FindKey(MathUtils.ToInt(urlParam.LairTreasureType))
    lair=LairTreasure.LairTreasureList[index]
    actionResult.position = lair.LairPosition;
    actionResult.LairRewardType = lair.LairRewardType
    if(lair.LairRewardType == LairRewardType.Gold):
        contextUser.GiftGold = MathUtils.Addition(contextUser.GiftGold,lair.Num)
        actionResult.LairRewardName = Lang.getLang("Gold")
        actionResult.LairRewardNum = lair.Num
        actionResult.LairRewardHead = lair.HeadID
        actionResult.ID = 0
    else:
        if(lair.LairRewardType == LairRewardType.GameGoin):
            contextUser.GameCoin = MathUtils.Addition(contextUser.GameCoin,lair.Num)
            actionResult.LairRewardName = Lang.getLang("GameGoin")
            actionResult.LairRewardNum = lair.Num
            actionResult.LairRewardHead = lair.HeadID
            actionResult.ID = 0
            
        else:
            if(lair.LairRewardType == LairRewardType.WuPing):
                actionResult.ItemInfo=LairTreasuerHelp.ShowLairReward(lair , contextUser,urlParam.LairTreasureType)
                if(actionResult.ItemInfo==None):
                    parent.ErrorCode = Lang.getLang("ErrorCode")
                    parent.ErrorInfo = Lang.getLang("LoadError")
                    actionResult.Result = False
                    return actionResult
                actionResult.LairRewardName = actionResult.ItemInfo.ItemName
                actionResult.LairRewardNum = lair.Num
                actionResult.LairRewardHead = actionResult.ItemInfo.HeadID;
                actionResult.ID = 0
    
    if urlParam.LairTreasureType == LairTreasureType.ZhuanJiaQuBao:
        contextUser.UserExtend.LairNum = MathUtils.Addition(contextUser.UserExtend.LairNum,1)
    if urlParam.LairTreasureType == LairTreasureType.DaShiQuBao:
        contextUser.UserExtend.DaLairNum = MathUtils.Addition(contextUser.UserExtend.DaLairNum,1)
    if urlParam.LairTreasureType == LairTreasureType.ZongShiQuBao:
        contextUser.UserExtend.ZhongLairNum = MathUtils.Addition(contextUser.UserExtend.ZhongLairNum,1)
    contextUser.UserExtend.LairDate = DateTime.Now
    if LairTreasure.UseType==0:
        contextUser.GameCoin = MathUtils.Subtraction(contextUser.GameCoin,LairTreasure.UseNum) 
    else: 
        contextUser.UseGold = MathUtils.Addition(contextUser.UseGold,LairTreasure.UseNum);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.position)
    writer.PushIntoStack(actionResult.LairRewardName)
    writer.PushIntoStack(actionResult.LairRewardNum)
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.LairRewardHead))
    writer.PushIntoStack(MathUtils.ToInt(actionResult.LairRewardType))
    writer.PushIntoStack(MathUtils.ToInt(actionResult.ID))
    return True;


