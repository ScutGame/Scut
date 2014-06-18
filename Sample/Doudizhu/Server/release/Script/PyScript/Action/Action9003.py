import ReferenceLib
from action import *
from System import *
from mathUtils import MathUtils
from lang import Lang
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


#聊天记录列表接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.ChatMaxNum = 0
        self.chatList = List[ChatMessage]

def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if True:
        urlParam.Result = True
    else:
        urlParam.Result = False
    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    user = parent.Current.User;
    table = GameRoom.Current.GetTableData(user)
    if not table or not user:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("St9002_NotJoinedTheRoom");
        actionResult.Result = False;
        return actionResult;
    chatService = DdzChatService(user)
    actionResult.ChatMaxNum = chatService.CurrVersion;
    chatAll = chatService.Receive();
    actionResult.chatList = chatAll.FindAll(lambda s:s.RoomId == user.Property.RoomId and s.TableId == user.Property.TableId)
    user.Property.ChatVesion = actionResult.ChatMaxNum;

    #需要实现
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.ChatMaxNum)
    writer.PushIntoStack(len(actionResult.chatList))
    for info in actionResult.chatList:
        DsItem = DataStruct()
        DsItem.PushIntoStack(info.FromUserID)
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.FromUserName))
        DsItem.PushIntoStack(info.ChatID)
        DsItem.PushIntoStack(MathUtils.ToNotNullString(info.Content))
        DsItem.PushIntoStack(info.SendDate.ToString("HH:mm:ss"))
        writer.PushIntoStack(DsItem)


    return True