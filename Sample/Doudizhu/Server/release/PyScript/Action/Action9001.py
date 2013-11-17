import clr, sys
from action import *
from System import *
from mathUtils import MathUtils


clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Doudizhu.Bll');
clr.AddReference('ZyGames.Doudizhu.Model');
clr.AddReference('ZyGames.Doudizhu.Lang');
from System.Collections.Generic import *
from ZyGames.Framework.SyncThreading import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Com.Chat import *
from ZyGames.Doudizhu.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Cache import *


#即时聊天配置接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.chatList = List[ChatInfo]        

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if True:
        urlParam.Result = True
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult();

    actionResult.chatList = ConfigCacheSet[ChatInfo]().FindAll();
    #需要实现
    return actionResult;


def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(len(actionResult.chatList))
    for info in actionResult.chatList:
        DsItem = DataStruct()
        DsItem.PushIntoStack(info.Id)
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.Content))
        writer.PushIntoStack(DsItem)
    return True