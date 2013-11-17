import clr, sys
import random
import time
import datetime
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.BLL.Combat');

from lang import Lang
from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.BLL.Combat import *
from ZyGames.Tianjiexing.Model.Enum import *

# 12052_守卫 Boss 界面接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.plotID = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.npcInfo = None;
        self.remainderChallengeNum = 0;  # 剩余挑战次数
        self.energyNum = 0;
        self.maxEnergyNum = 0;
        self.rewardList = [];
        self.bossLv = 0;
        self.behindNpcID = 0;
        self.plotPackage = None;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("PlotID"):
        urlParam.plotID = httpGet.GetIntValue("PlotID");
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

        # 加载数据出错
    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    userPlotPackage = GameDataCacheSet[UserPlotPackage]().FindKey(userId);
    if not userPlotPackage:
        return loadError();
    plotPackage = userPlotPackage.PlotPackage.Find(match=lambda x:x.PlotID == urlParam.plotID);  # 玩家地图信息，and 类型为考古
    if not plotPackage:
        return loadError();
    actionResult.plotPackage = plotPackage;

    plotNpcInfo = ConfigCacheSet[PlotNPCInfo]().FindKey(plotPackage.CurrPlotNpcID);
    if not plotNpcInfo or plotNpcInfo.IsBoss == False:
        return loadError();
    actionResult.npcInfo = plotNpcInfo;
    # 获取
    plotEmbattleInfo = ShareCacheStruct[PlotEmbattleInfo]().Find(match=lambda x:x.PlotNpcID == plotNpcInfo.PlotNpcID);
    if not plotEmbattleInfo:
        return loadError();
    monsterInfo = ShareCacheStruct[MonsterInfo]().FindKey(plotEmbattleInfo.MonsterID)
    if not monsterInfo:
        return loadError();
    actionResult.bossLv = monsterInfo.GeneralLv;  # Boss 等级

    actionResult.remainderChallengeNum = plotPackage.BossChallengeCount;
    actionResult.energyNum = contextUser.EnergyNum; # 当前剩余精力
    actionResult.maxEnergyNum = ConfigEnvSet.GetInt("User.MaxEnergyNum"); # 最大精力值
    actionResult.behindNpcID = plotNpcInfo.BehindNpcID;  # 下一个Boss NpcID

    # 掉落物品
    actionResult.rewardList = plotNpcInfo.BoxReward;
    # 播放动画
    plotPackage.PlayAnimat = 1;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    npcInfo = actionResult.npcInfo;
    plotPackage = actionResult.plotPackage;
    writer.PushIntoStack(npcInfo.PlotNpcID);
    writer.PushIntoStack(npcInfo.NpcName);
    writer.PushIntoStack(npcInfo.HeadID);
    writer.PushShortIntoStack(npcInfo.NpcSeqNo);
    writer.PushShortIntoStack(actionResult.bossLv);  # Level
    writer.PushShortIntoStack(4);  # Boss 默认紫色品质
    writer.PushShortIntoStack(1);  # 默认消耗1精力
    writer.PushShortIntoStack(actionResult.energyNum);
    writer.PushShortIntoStack(actionResult.maxEnergyNum);
    writer.PushShortIntoStack(actionResult.remainderChallengeNum);
    writer.PushShortIntoStack(npcInfo.ChallengeNum);
    writer.PushShortIntoStack(1 if actionResult.behindNpcID else 0);
    
    rewardList = actionResult.rewardList;
    writer.PushIntoStack(len(rewardList));
    for info in rewardList:
        dsItem = DataStruct();
        itemInfo = ConfigCacheSet[ItemBaseInfo]().FindKey(info.ItemID);
        dsItem.PushIntoStack(itemInfo.ItemName if itemInfo else '');
        dsItem.PushIntoStack(info.HeadID);
        writer.PushIntoStack(dsItem);

    writer.PushShortIntoStack(1 if plotPackage.HasChallengeBossWin else 0);
    return True;