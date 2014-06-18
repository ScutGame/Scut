"""notice.py系统公告"""
import ReferenceLib
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.RPC.IO import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Com.Chat import *
from ZyGames.Doudizhu.Bll.Logic import *
from ZyGames.Doudizhu.Bll.Base import *
from ZyGames.Doudizhu.Script.CsScript.Action import *

def compareTo(x, y):
    result = y.IsTop.CompareTo(x.IsTop);
    if result == 0:
        return y.CreateDate.CompareTo(x.CreateDate)
    return result

def inquiry(httpGet, head, writer):
    pageIndex = httpGet.GetIntValue("pageIndex")
    pageSize = httpGet.GetIntValue("pageSize")
    pageCount = 0
    recordCount = 0

    list = ShareCacheStruct[GameNotice]().FindAll()
    MathUtils.QuickSort[GameNotice](list, compareTo)
    result = MathUtils.GetPaging[GameNotice](list, pageIndex, pageSize)
    if result:
        noticeList = result[0]
        pageCount = result[1]
        recordCount = result[2]
        writer.PushIntoStack(int(1))
        writer.PushIntoStack(recordCount)
        for info in noticeList:
            dsItem = MessageStructure()
            dsItem.PushIntoStack(info.NoticeID)
            dsItem.PushIntoStack(info.Title)
            dsItem.PushIntoStack(info.Content)
            dsItem.PushIntoStack(info.IsBroadcast and 1 or 0)
            dsItem.PushIntoStack(info.IsTop and 1 or 0)
            dsItem.PushIntoStack(info.Creater)
            dsItem.PushIntoStack(info.CreateDate.ToString('yyyy-MM-dd HH:mm:ss'))
            dsItem.PushIntoStack(info.ExpiryDate.ToString('yyyy-MM-dd HH:mm:ss'))
            writer.PushIntoStack(dsItem)
    
def send(httpGet, head, writer):
    noticeID = httpGet.GetStringValue("NoticeID")
    title = httpGet.GetStringValue("Title")
    content = httpGet.GetStringValue("Content")
    expiryDate = MathUtils.ToDateTime(httpGet.GetStringValue("ExpiryDate"))
    isTop = MathUtils.ToBool(httpGet.GetStringValue("IsTop"))
    isBroadcast = MathUtils.ToBool(httpGet.GetStringValue("IsBroadcast"))
    creater = httpGet.GetStringValue("Creater")
    createDate = MathUtils.ToDateTime(httpGet.GetStringValue("CreateDate"))
    noticeType = httpGet.GetIntValue("NoticeType")

    cacheSet = ShareCacheStruct[GameNotice]()
    gameNotice = cacheSet.FindKey(noticeID)
    if not gameNotice:
        gameNotice = GameNotice()
        gameNotice.NoticeID = Guid.NewGuid().ToString()
        cacheSet.Add(gameNotice)
        gameNotice = cacheSet.FindKey(gameNotice.NoticeID)

    gameNotice.Title = title
    gameNotice.Content = content
    gameNotice.ExpiryDate = expiryDate
    gameNotice.IsTop = isTop
    gameNotice.IsBroadcast = isBroadcast
    if not gameNotice.Creater or len(gameNotice.Creater) == 0:
        gameNotice.Creater = creater
        gameNotice.CreateDate = createDate
    gameNotice.NoticeType = noticeType
    if gameNotice.IsBroadcast:
        DdzBroadcastService.Send(gameNotice.Content)
        ClientNotifier.NotifyOnlineUserAction(ActionIDDefine.Cst_Action9202, None, None)
    
    writer.PushIntoStack(int(2))
    
def remove(httpGet, head, writer):
    noticeID = httpGet.GetStringValue("NoticeID")
    cacheSet = ShareCacheStruct[GameNotice]()
    gameNotice = cacheSet.FindKey(noticeID)
    if gameNotice:
        cacheSet.Delete(gameNotice)
    
    writer.PushIntoStack(int(3))
        