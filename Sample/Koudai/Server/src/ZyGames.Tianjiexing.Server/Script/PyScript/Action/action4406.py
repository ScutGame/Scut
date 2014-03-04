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

# 4408_圣吉塔战斗结果奖励接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.isWin = 1;
        self.score = 0;
        self.starNum = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.generalNum = 0;
        self.combatRound = 1;
        self.exchange = 0;
        self.receive = 0
        self.maxHonourNum = 0;
        self.currentLayer = 0;
        self.honourNum = 0;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("Score")\
    and httpGet.Contains("StarNum"):
        urlParam.score = httpGet.GetWordValue("Score");
        urlParam.starNum = httpGet.GetWordValue("StarNum");
        urlParam.isWin = httpGet.GetWordValue("IsWin");

    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;

    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    # 战斗成功
    if urlParam.isWin == 0:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St4407_FightFail");
        actionResult.Result = False;
        return actionResult;

    # 获取玩家最大荣誉值
    cacheSetGeneralEscalate = ConfigCacheSet[GeneralEscalateInfo]();
    GeneralEscalateHelper.AddUserLv(contextUser, 0);
    lv = contextUser.UserLv;
    lv = 1 if lv < 0 else lv + 1
    generalEscalate = cacheSetGeneralEscalate.Find(match=lambda s:s.GeneralType == GeneralType.YongHu and s.GeneralLv == lv);
    if generalEscalate:
        actionResult.maxHonourNum = generalEscalate.UpExperience;
   
    actionResult.honourNum = contextUser.HonourNum
    userSJTInfo = GameDataCacheSet[UserShengJiTa]().FindKey(userId);
    if not userSJTInfo:
        return loadError();
    # 当前层数
    actionResult.currentLayer = userSJTInfo.IsTierNum;

    if userSJTInfo.IsTierNum !=0:
        # 三层奖励
        for i in range(1, 3):
            if (userSJTInfo.IsTierNum+i)%3 == 0:
                actionResult.exchange = i;

        # 五层奖励
        for i in range(1, 5):
            if (userSJTInfo.IsTierNum+i)%5 == 0:
                actionResult.receive = i;


    ## 当前轮的最高分数如果大于当天某一轮的最高分数，更新当天某一轮的最高分数
    #if userSJTInfo.IsRountStar > userSJTInfo.ScoreStar:
    #    userSJTInfo.RoundPoor = userSJTInfo.IsRountStar - userSJTInfo.ScoreStar;
    #    userSJTInfo.ScoreStar = userSJTInfo.IsRountStar;

    actionResult.difficultyNum = contextUser.DifficultyNum;
    #if actionResult.exchange == actionResult.receive:
    #    pass  # 冲突的时候先领取奖励
    if actionResult.exchange==0:
        userSJTInfo.SJTStatus=3
    if actionResult.receive ==0:
       userSJTInfo.SJTStatus=4
    if actionResult.exchange!=0 and actionResult.receive !=0:
        userSJTInfo.SJTStatus=2
    return actionResult;


def buildPacket(writer, urlParam, actionResult):
    writer.PushShortIntoStack(urlParam.score);
    writer.PushShortIntoStack(urlParam.starNum);
    writer.PushShortIntoStack(actionResult.combatRound);
    writer.PushShortIntoStack(actionResult.exchange);
    writer.PushShortIntoStack(actionResult.receive);
    writer.PushIntoStack(actionResult.currentLayer);
    writer.PushIntoStack(actionResult.maxHonourNum);
    writer.PushIntoStack(actionResult.honourNum);
    return True;