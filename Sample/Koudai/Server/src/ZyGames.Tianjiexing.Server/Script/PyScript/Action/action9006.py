import clr, sys
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');

from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.List = []

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    return urlParam;


def takeAction(urlParam,parent):
    actionResult =  ActionResult();
    userId =parent.Current.UserId;
    rewardInfo=FestivalHelper.GetInfo(FestivalType.FirstReward);
    if rewardInfo:
        actionResult.List = rewardInfo.Reward.ToList();
    #处理结果存储在字典中
    return actionResult;



def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(len(actionResult.List));

    for info in actionResult.List:
        if info :
            ds = DataStruct();
            itemInfo = ConfigCacheSet[ItemBaseInfo]().FindKey(info.ItemID);
            HeadID='';
            ItemName='';
            ItemDesc='';
            if itemInfo:
                HeadID=itemInfo.HeadID;
                ItemName=itemInfo.ItemName;
                ItemDesc=itemInfo.ItemDesc;
            ds.PushIntoStack(info.ItemID);
            ds.PushIntoStack(Convert.ToInt32(info.Type));
            ds.PushIntoStack(info.Num);
            ds.PushIntoStack(HeadID);
            ds.PushIntoStack(ItemName);
            ds.PushIntoStack(ItemDesc);
            writer.PushIntoStack(ds);
    return True;




