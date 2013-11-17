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

# 五层领取奖励接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    userId = parent.Current.User.PersonalId
    gameUser= parent.Current.User
    userShengJiTa = GameDataCacheSet[UserShengJiTa]().FindKey(userId)    #获取玩家信息
    rewardInfo=ShareCacheStruct[SJTRewarInfo]().FindKey(userShengJiTa.IsTierNum)
    if userShengJiTa.RoundPoor>0:
         gameUser.GameCoin=gameUser.GameCoin+userShengJiTa.RoundPoor*rewardInfo.GameCoin 
    list =userShengJiTa.RewardStatusList.FindAll(lambda s:s.SJTID==userShengJiTa.IsTierNum and s.IsReceive == 0)
    for i in list:
        if i.IsReceive==0 and i.ExperienceNum<=userShengJiTa.IsTierStar:
            rewar=rewardInfo.SJTRewarList.FindAll(lambda u:u.StarNum==i.StarNum and u.StarNum<=userShengJiTa.IsTierStar)
            for rewarinfo in rewar:
                if  rewarinfo.SJTRewarType==SJTRewarType.JinBi:
                    gameUser.GameCoin=gameUser.GameCoin + rewarinfo.Num
                    i.IsReceive=1
                elif rewarinfo.SJTRewarType==SJTRewarType.WuPing:
                     UserItemHelper.AddUserItem(userId,rewarinfo.ItemId,rewarinfo.Num)
                     i.IsReceive=1
                elif rewarinfo.SJTRewarType==SJTRewarType.JinShi:
                    gameUser.GiftGold=gameUser.GiftGold+rewarinfo.Num
                    i.IsReceive=1
    userShengJiTa.IsTierStar=0    #领取奖励清空每五层的分数
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    return True;