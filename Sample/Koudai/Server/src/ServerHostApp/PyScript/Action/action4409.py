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
from ZyGames.Tianjiexing.Model.Config import *

# 圣吉塔奖励界面接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.MoreStar=0
        self.GameCoin =0
        self.IsTierStar=0
        self.rewarAll=[]
        self.IsTierNum=0
        self.Modulus=0
        self.gameCoin = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    return urlParam;


def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId
    gameUser=parent.Current.User

    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    userShengJiTa = GameDataCacheSet[UserShengJiTa]().FindKey(userId)    #获取玩家信息
    if userShengJiTa == None:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False;
        return actionResult;
    IsTierNum=userShengJiTa.IsTierNum
    #第一次加载每层的默认值
    rewardStatusList=userShengJiTa.RewardStatusList.FindAll(match=lambda u:u.SJTID==IsTierNum and u.SJTID%5==0)
    if not rewardStatusList:
        list=[1,3,6,9]
        for i in list:
            rewardList=RewardStatus()
            rewardList.SJTID=IsTierNum
            rewardList.IsReceive=0
            rewardList.StarNum=5*i
            rewardList.ExperienceNum=0
            rewardList.RewardType=1
            userShengJiTa.RewardStatusList.Add(rewardList)
    rewardInfo=ShareCacheStruct[SJTRewarInfo]().FindKey(IsTierNum)
    if rewardInfo == None:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False;
        return actionResult;
    if IsTierNum%5==0 and IsTierNum!=0:
        #userId = parent.Current.User.PersonalId
        actionResult.IsTierStar=userShengJiTa.IsTierStar
        actionResult.Modulus=rewardInfo.GameCoin

        # 计算比上一个五层多出的星星数
        diffStar = 0
        currentFiveTier = userShengJiTa.FiveTierRewardList.Find(match=lambda x:x.BattleNum == userShengJiTa.BattleRount and x.FiveTierNum == IsTierNum);
        if not currentFiveTier:
            return loadError();
        lastBattleNum = userShengJiTa.BattleRount-1;
        lastFiveTier = userShengJiTa.FiveTierRewardList.Find(match=lambda x:x.BattleNum == lastBattleNum and x.FiveTierNum == IsTierNum);
        if lastFiveTier:
            actionResult.MoreStar = currentFiveTier.FiveTierStarNum - lastFiveTier.FiveTierStarNum;
        else:
            actionResult.MoreStar = currentFiveTier.FiveTierStarNum
        actionResult.MoreStar = actionResult.MoreStar if actionResult.MoreStar > 0 else 0
        rewardStatusList = userShengJiTa.RewardStatusList.FindAll(match=lambda u:u.SJTID==IsTierNum)
        for i in rewardStatusList: 
            if i.IsReceive==0 and i.ExperienceNum<=userShengJiTa.IsTierStar:
                rewar=rewardInfo.SJTRewarList.FindAll(match=lambda u:u.StarNum==i.StarNum and u.StarNum<=userShengJiTa.IsTierStar)
                if rewar:
                    actionResult.rewarAll.append(rewar)
        actionResult.IsTierNum=IsTierNum
        if actionResult.MoreStar>0:
            actionResult.gameCoin = actionResult.MoreStar*rewardInfo.GameCoin 
    if len(actionResult.rewarAll)==0:   
        userShengJiTa.IsTierStar=0

    if IsTierNum == 50:
        userShengJiTa.SJTStatus = 0
        userShengJiTa.BattleRount += 1;
        userShengJiTa.IsTierNum = 0;
        userShengJiTa.LifeNum = 0
        userShengJiTa.WuLiNum = 0
        userShengJiTa.FunJiNum = 0
        userShengJiTa.MofaNum = 0
        userShengJiTa.IsTierStar = 0
        userShengJiTa.IsRountStar = 0;
    else:
        userShengJiTa.SJTStatus=4
   
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.MoreStar)
    writer.PushIntoStack(actionResult.IsTierStar)
    writer.PushIntoStack(actionResult.IsTierNum)
    writer.PushIntoStack(len(actionResult.rewarAll))
    for info in actionResult.rewarAll:
        if info:
            tempInfo = info[0];
            DsItem = DataStruct()
            DsItem.PushIntoStack(MathUtils.ToInt(tempInfo.ItemId))
            DsItem.PushIntoStack(MathUtils.ToInt(tempInfo.SJTRewarType))
            DsItem.PushIntoStack(tempInfo.Num)
            if info[0].ItemId==0:
                DsItem.PushIntoStack(MathUtils.ToNotNullString(0))
            else:
                rewardInfo=ShareCacheStruct[ItemBaseInfo]().FindKey(info[0].ItemId)
                DsItem.PushIntoStack(MathUtils.ToNotNullString(rewardInfo.HeadID))
            writer.PushIntoStack(DsItem)
        else:
            writer.PushIntoStack(0)
    writer.PushIntoStack(actionResult.Modulus)
    writer.PushIntoStack(actionResult.gameCoin)
    return True;