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

# 4407_圣吉塔兑换属性界面接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self);
        self.isWin = 1;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self);
        self.layerNum = 0;
        self.isRountStar = 0;
        self.sulplusNum = 0;
        self.propertyTypeList = [];
        self.effNumList = [];
        self.userSJTInfo = None;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    urlParam.isWin = httpGet.GetWordValue("IsWin");
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    gameUser = parent.Current.User
    # contextUser = parent.Current.User;

    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;

    # 是否战斗成功
    if urlParam.isWin == 0:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St4407_FightFail");
        actionResult.Result = False;
        return actionResult;

    userSJTInfo = GameDataCacheSet[UserShengJiTa]().FindKey(userId);
    if not userSJTInfo:
        return loadError();

    # 是否属于三层奖励
    sjtPlotInfo = ShareCacheStruct[SJTPlotInfo]().FindKey(userSJTInfo.IsTierNum);
    if not sjtPlotInfo:
        return loadError();

    if sjtPlotInfo.IsProperty == False:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St4407_NoAddProperty");
        actionResult.Result = False;
        return actionResult;

    actionResult.userSJTInfo = userSJTInfo;
    actionResult.layerNum = userSJTInfo.IsTierNum;
    actionResult.isRountStar = userSJTInfo.IsRountStar;  # 当前每轮得分
    actionResult.sulplusNum = userSJTInfo.LastScoreStar;

    # 位置是否已经存在
    positions = userSJTInfo.RandomPosition
    if positions:
        positions = positions.split(',')
    if positions == None or (userSJTInfo.IsTierNum != MathUtils.ToInt(positions[0])):
    # 获取属性类型个数
        typeCount = GetEnumCountHelper.EnumCount('ZyGames.Tianjiexing.Model.Enum.PropertyType,ZyGames.Tianjiexing.Model')
        # 随机加成属性
        listOne = [i for i in range(1, typeCount+1)];
        listOne.remove(random.choice(listOne));
        random.shuffle(listOne)
        actionResult.propertyTypeList = listOne;

        tempPos = str(userSJTInfo.IsTierNum) + ','
        for i in range(0, 3):
            tempPos += str(listOne[i]) + ','
        userSJTInfo.RandomPosition = tempPos;

    else:
        actionResult.propertyTypeList.append(positions[1]);
        actionResult.propertyTypeList.append(positions[2]);
        actionResult.propertyTypeList.append(positions[3]);


    listTwo = [3, 15, 30];
    actionResult.effNumList = listTwo;
    userSJTInfo.SJTStatus=3
    return actionResult;

def formatData(param):
    return '{0}%'.format(MathUtils.ToInt(param*100));

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.layerNum);
    writer.PushIntoStack(actionResult.isRountStar);
    writer.PushIntoStack(actionResult.sulplusNum);

    writer.PushIntoStack(len(actionResult.effNumList));
    propertyTypeList = actionResult.propertyTypeList
    effNumList = actionResult.effNumList
    for item in zip(propertyTypeList, effNumList):
        dsItem = DataStruct();
        dsItem.PushShortIntoStack(MathUtils.ToShort(item[0]));
        effNum = item[1];
        dsItem.PushIntoStack('{0}%'.format(effNum));
        dsItem.PushIntoStack(effNum);
        if actionResult.sulplusNum < effNum:
            dsItem.PushShortIntoStack(0);
        else:
            dsItem.PushShortIntoStack(1);
        writer.PushIntoStack(dsItem);
    
    userSJTInfo = actionResult.userSJTInfo;
    writer.PushIntoStack(formatData(userSJTInfo.LifeNum));
    writer.PushIntoStack(formatData(userSJTInfo.WuLiNum));
    writer.PushIntoStack(formatData(userSJTInfo.MofaNum));
    writer.PushIntoStack(formatData(userSJTInfo.FunJiNum));
    return True;