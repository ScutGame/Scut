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
from System.Collections.Generic import *
from lang import Lang
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
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Tianjiexing.Component.Chat import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Component import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.BattleRount=0
        self.LastBattleRount=0
        self.MaxTierNum =0
        self.ScoreStar =0
        self.UserRank=0
        self.UserLv=0
        self.IsStartProperty=0
        self.IsTireNum=0
        self.IsRountStar=0
        self.SJTStatus=0
def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId
    gameUser =parent.Current.User
    actionResult.UserLv=gameUser.UserLv   
    if actionResult.UserLv>=10:       
        userShengJiTa = GameDataCacheSet[UserShengJiTa]().FindKey(userId)    #获取玩家信息
        if userShengJiTa == None:
            GameDataCacheSet[UserShengJiTa]().Add(UserShengJiTa(MathUtils.ToInt(userId)))
            userShengJiTa = GameDataCacheSet[UserShengJiTa]().FindKey(userId)    #获取玩家信息
        if userShengJiTa.EndTime==None or DateTime.Now.Date!=userShengJiTa.EndTime.Date:  #判断时间是否同一天
            userShengJiTa.MaxTierNum = 0;
            userShengJiTa.ScoreStar = 0;
            userShengJiTa.LifeNum = 0;
            userShengJiTa.WuLiNum = 0;
            userShengJiTa.FunJiNum = 0;
            userShengJiTa.MofaNum = 0;
            userShengJiTa.FiveTierRewardList.Clear();
            userShengJiTa.IsTierNum=0     #当前层数
            userShengJiTa.IsTierStar=0             #当前五层得分  
            userShengJiTa.BattleRount=0
            userShengJiTa.SJTStatus=0
            userShengJiTa.RoundPoor = 0;
            List=[]
            List=userShengJiTa.RewardStatusList            #附加奖励是否领取置为否
            for i in List:
                i.IsReceive=0
        actionResult.BattleRount=userShengJiTa.BattleRount
        if actionResult.BattleRount>3:
             parent.ErrorCode = Lang.getLang("ErrorCode");
             parent.ErrorInfo = Lang.getLang("St13002_BattleRount")
        actionResult.LastBattleRount=3-actionResult.BattleRount
        actionResult.MaxTierNum=userShengJiTa.MaxTierNum
        actionResult.ScoreStar=userShengJiTa.ScoreStar
        rankList = RankingFactory.Get[UserRank](ShengJiTaRanking.RankingKey)   #获取玩家的排行
        ranKingUser=rankList.Find(lambda u:u.UserID==userId)
        if ranKingUser:
            actionResult.UserRank=ranKingUser.SJTRankId   
        else:
            actionResult.UserRank=0     #未进入排行榜
        actionResult.SJTStatus=userShengJiTa.SJTStatus   #跳转界面
    else:
         parent.ErrorCode = Lang.getLang("ErrorCode");
         parent.ErrorInfo = Lang.getLang("St4405_UserLvNotEnough")
         actionResult.Result = False;
         return actionResult;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.BattleRount)
    writer.PushIntoStack(actionResult.LastBattleRount)
    writer.PushIntoStack(actionResult.MaxTierNum)
    writer.PushIntoStack(actionResult.ScoreStar)
    writer.PushIntoStack(actionResult.UserRank)
    writer.PushIntoStack(MathUtils.ToInt(actionResult.UserLv))
    writer.PushShortIntoStack(actionResult.SJTStatus)
    return True