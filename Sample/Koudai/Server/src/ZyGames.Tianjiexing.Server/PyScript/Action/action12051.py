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

# 12051_九宫格怪物接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.plotID = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.mapList = [];
        self.plotArcheologyList = [];
        self.plotPackage = [];
        self.plotInfo = None;
        self.plotNpcList = [];
        self.playAnimat = 0;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("PlotID"):
        urlParam.plotID = httpGet.GetIntValue("PlotID");
    else:
        urlParam.Result = False;

    return urlParam;

# 每周一 0:00 所有数据至初始状态（已开启的高级地图，默认开启)
def resetData(userPlotPackage):
    plotPackage = userPlotPackage.PlotPackage.FindAll(match=lambda x:x.PlotType == PlotType.KaoGuPlot);
    plotCacheSet = ConfigCacheSet[PlotInfo]();
    if plotPackage:
        for item in plotPackage:
            item.HasMapCount = 0;
            item.CurrPlotNpcID = 0;
            plotInfo = plotCacheSet.FindKey(item.PlotID);
            if plotInfo:
                item.BossChallengeCount = plotInfo.ChallengeNum;
            item.IsFirstIn = False;
            item.HasChallengeBossWin = False;
            item.ArcheologyPackage.Clear();

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    userExtend = contextUser.UserExtend
    # 加载数据出错
    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    if(userExtend.LairDate.Date != DateTime.Now.Date):
        userExtend.LairNum = 0
        userExtend.LairDate = DateTime.Now
    userPlotPackage = GameDataCacheSet[UserPlotPackage]().FindKey(userId);
    plotPackage = userPlotPackage.PlotPackage.Find(match=lambda x:x.PlotID == urlParam.plotID);  # 玩家地图信息，and 类型为考古
    if not plotPackage:
        return loadError();

    # 每周一 0:00 所有数据至初始状态（已开启的高级地图，默认开启)
    mondayDate = UserArchaeologyHelper.MondayDate();
    if mondayDate.Date >= userPlotPackage.EngageDate.Date:
        if mondayDate.Date == userPlotPackage.EngageDate.Date:
            if userPlotPackage.EngageDate.Hour == 0:
                resetData(userPlotPackage);
                userPlotPackage.EngageDate = DateTime.Now.AddHours(1);
        else:
            resetData(userPlotPackage);
            userPlotPackage.EngageDate = DateTime.Now;


    # 如果第一次进入，1、增加该地图的怪物九宫格信息和宝箱信息 2、当前副本Boss置为第一个Boss
    if plotPackage.IsFirstIn == False:
        plotPackage.IsFirstIn = True;
        list = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        random.shuffle(list);  # 打乱数组
        # 随机九宫格怪物的位置
        plotNpcList = ShareCacheStruct[PlotNPCInfo]().FindAll(match=lambda x:x.PlotID == urlParam.plotID and x.IsBoss == False and x.NpcSeqNo != 0);
        if plotNpcList.Count > 0:
            plotArcheologyList = plotPackage.ArcheologyPackage;
            j = 0;
            for plotArcheology in plotNpcList:
                for i in range(0, plotArcheology.MonsterNum):
                    userPlotArcheology = UserPlotArcheology();
                    userPlotArcheology.PlotNpcID = plotArcheology.PlotNpcID;
                    userPlotArcheology.Position = list[j];
                    j += 1;
                    userPlotArcheology.Quality = plotArcheology.NpcSeqNo;
                    plotArcheologyList.Add(userPlotArcheology);
                    
        # 增加宝箱的位置
        plotNpcBox = ShareCacheStruct[PlotNPCInfo]().Find(match=lambda x:x.PlotID == urlParam.plotID and x.NpcSeqNo == 0);
        if plotNpcBox:
            plotArcheologyList = plotPackage.ArcheologyPackage;
            for boxRewardInfo in (0, plotNpcBox.MonsterNum):
                index = random.choice(list)
                userPlotArcheology = UserPlotArcheology();
                userPlotArcheology.PlotNpcID = plotNpcBox.PlotNpcID;
                userPlotArcheology.Position = index;
                list.remove(index);
                plotArcheologyList.Add(userPlotArcheology);

            # 随机一个白色品质的怪物，默认开启
            archeologyList = plotArcheologyList.FindAll(match=lambda x:x.Quality == 1);
            if not archeologyList:
                return loadError();
            archeologyInfo = random.choice(archeologyList);
            archeologyInfo.IsOpen = True;

        # 当前副本Boss置为第一个Boss NpcID
        bossList = ShareCacheStruct[PlotNPCInfo]().FindAll(match=lambda x:x.PlotID == urlParam.plotID and x.IsBoss == True)
        if bossList.Count > 0:
            for bossInfo in bossList:
                boss = bossList.Find(match=lambda x:x.BehindNpcID == bossInfo.PlotNpcID)
                if not boss:
                    plotPackage.CurrPlotNpcID = bossInfo.PlotNpcID;
                
    # 下怪物列表信息
    actionResult.plotArcheologyList = plotPackage.ArcheologyPackage.FindAll(match=lambda x:x.IsOpen == True);

    plotNpcList = ConfigCacheSet[PlotNPCInfo]().FindAll(match=lambda x:x.PlotID == urlParam.plotID); # 获取怪物名称，图片，品质等
    if not plotNpcList:
        return loadError();
    actionResult.plotNpcList = plotNpcList;

    # 当前地图的最大碎片数
    actionResult.plotInfo = ConfigCacheSet[PlotInfo]().FindKey(urlParam.plotID);

    # 碎片数与精力
    userPlotPackage = GameDataCacheSet[UserPlotPackage]().FindKey(userId);
    if not userPlotPackage:
        return loadError();
    plotPackage = userPlotPackage.PlotPackage.Find(match=lambda x:x.PlotID == urlParam.plotID);  # 玩家地图信息，and 类型为考古
    if not plotPackage:
        return loadError();
    actionResult.plotPackage = plotPackage;

    actionResult.playAnimat = plotPackage.PlayAnimat

    actionResult.energyNum = contextUser.EnergyNum; # 当前剩余精力
    actionResult.maxEnergyNum = ConfigEnvSet.GetInt("User.MaxEnergyNum"); # 最大精力值
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    plotInfo = actionResult.plotInfo;
    plotArcheologyList = actionResult.plotArcheologyList;
    plotNpcList = actionResult.plotNpcList;
    plotPackage = actionResult.plotPackage;

    # 九宫格怪物列表
    writer.PushIntoStack(len(plotArcheologyList));
    for info in plotArcheologyList:
        dsItem = DataStruct();
        dsItem.PushIntoStack(info.PlotNpcID);
        dsItem.PushShortIntoStack(info.LightMapCount);
        dsItem.PushShortIntoStack(info.Position);
        tempPlotInfo = plotNpcList.Find(match=lambda x:x.PlotNpcID == info.PlotNpcID);
        dsItem.PushIntoStack(tempPlotInfo.NpcName if plotInfo else '');
        dsItem.PushIntoStack(tempPlotInfo.HeadID if plotInfo else '');
        dsItem.PushIntoStack(tempPlotInfo.NpcSeqNo if plotInfo else 0);
        if info.Quality == 0:  # 是宝箱
            dsItem.PushIntoStack(1);
        else:
            dsItem.PushIntoStack(0);

        writer.PushIntoStack(dsItem);

    writer.PushShortIntoStack(plotPackage.HasMapCount); # 当前地图获得碎片总数
    writer.PushIntoStack(plotInfo.FragmentNum);
    writer.PushShortIntoStack(1);  # 默认消耗1精力，无需配置
    writer.PushShortIntoStack(actionResult.energyNum);
    writer.PushShortIntoStack(actionResult.maxEnergyNum);
    writer.PushIntoStack(actionResult.playAnimat);
    return True;