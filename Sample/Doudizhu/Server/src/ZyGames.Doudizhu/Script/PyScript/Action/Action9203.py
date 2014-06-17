import ReferenceLib
from action import *
from System import *
from mathUtils import MathUtils
from System.Collections.Generic import *
from ZyGames.Framework.SyncThreading import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Model import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Logic import *
from ZyGames.Doudizhu.Bll.Com.Chat import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Cache import *


#9203_公告列表接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.PageIndex = 0
        self.PageSize = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.ChatMaxNum = 0
        self.gameNoticelsit = List[GameNotice]
        self.PageCount = 0

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if  httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.PageIndex = httpGet.GetIntValue("PageIndex")
        urlParam.PageSize = httpGet.GetIntValue("PageSize")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    def sortNotice(x,y):
        if x == None and y == None:
            return 0;
        if x != None and y == None:
            return 1;
        if x == None:
            return -1;
        result = y.IsTop.CompareTo(x.IsTop);
        if (result == 0):
            result = y.CreateDate.CompareTo(x.CreateDate);
        return result;
    actionResult = ActionResult();
    #user = parent.Current.User;
   
    actionResult.gameNoticelsit = ShareCacheStruct[GameNotice]().FindAll(match=lambda m:filterNotice(m));
    MathUtils.QuickSort[GameNotice](actionResult.gameNoticelsit,  lambda x,y:sortNotice(x,y))
    result = MathUtils.GetPaging[GameNotice](actionResult.gameNoticelsit,urlParam.PageIndex, urlParam.PageSize)
    if result:
        actionResult.gameNoticelsit = result[0]
        actionResult.PageCount = result[1]
    return actionResult;

def filterNotice(s):
   minData = MathUtils.SqlMinDate
   isExpiry = s.ExpiryDate <= minData
   return isExpiry or (not isExpiry and s.ExpiryDate >= DateTime.Now )

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.PageCount)
    writer.PushIntoStack(len(actionResult.gameNoticelsit))
    for info in actionResult.gameNoticelsit:
        DsItem = DataStruct()
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.Title))
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.Content))
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.CreateDate))
        writer.PushIntoStack(DsItem)
    return True