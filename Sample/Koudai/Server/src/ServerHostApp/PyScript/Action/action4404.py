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

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PlotID=0
        self.IsTierStar=0
        self.IsRountStar =0
        self.LastScoreStar=0
        self.HeadID =''
        self.MonsterName=''
        self.MonsterNum =0
        self.LifeNum=0
        self.PhyNum =0
        self.MagNum =0
        self.AbiNum =0
        self.MaxTierNum=0
        self.ScoreStar=0
        self.difficultyList=[]

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId
    gameUser = parent.Current.User
    userShengJiTa = GameDataCacheSet[UserShengJiTa]().FindKey(userId)    #获取玩家信息
    actionResult.PlotID=userShengJiTa.IsTierNum+1
    actionResult.IsTierStar=userShengJiTa.IsTierStar
    actionResult.IsRountStar=userShengJiTa.IsRountStar
    actionResult.LastScoreStar=userShengJiTa.LastScoreStar
    actionResult.LifeNum=userShengJiTa.LifeNum
    actionResult.PhyNum=userShengJiTa.WuLiNum
    actionResult.MagNum =userShengJiTa.MofaNum
    actionResult.AbiNum=userShengJiTa.FunJiNum
    actionResult.MaxTierNum=userShengJiTa.MaxTierNum
    actionResult.ScoreStar=userShengJiTa.ScoreStar
    PlotInfo=ShareCacheStruct[SJTPlotInfo]().FindKey(actionResult.PlotID)
    actionResult.HeadID=PlotInfo.HeadID
    actionResult.MonsterName=PlotInfo.NpcName
    actionResult.MonsterNum=PlotInfo.PartInNum
    actionResult.difficultyList=PlotInfo.DifficultyList
    userShengJiTa.SJTStatus=2
    return actionResult;

def formatData(param):
    return '{0}%'.format(MathUtils.ToInt(param*100));

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PlotID)
    writer.PushIntoStack(actionResult.IsRountStar)
    writer.PushIntoStack(actionResult.LastScoreStar)
    writer.PushIntoStack(MathUtils.ToString(actionResult.HeadID))
    writer.PushIntoStack(MathUtils.ToString(actionResult.MonsterName))
    writer.PushIntoStack(actionResult.MonsterNum)
    writer.PushIntoStack(len(actionResult.difficultyList))
    for info in actionResult.difficultyList:
        DsItem = DataStruct()
        DsItem.PushIntoStack(MathUtils.ToInt(info.DifficultyType))
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.DifficultyNum))
        writer.PushIntoStack(DsItem)
    writer.PushIntoStack(formatData(actionResult.LifeNum))
    writer.PushIntoStack(formatData(actionResult.PhyNum))
    writer.PushIntoStack(formatData(actionResult.MagNum))
    writer.PushIntoStack(formatData(actionResult.AbiNum))
    writer.PushIntoStack(actionResult.MaxTierNum)
    writer.PushIntoStack(actionResult.ScoreStar)

    return True;