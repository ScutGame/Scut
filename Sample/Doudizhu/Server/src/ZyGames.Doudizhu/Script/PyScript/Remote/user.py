""""user.py 玩家信息"""
import ReferenceLib
from System import *
from ZyGames.Framework.Net import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll import *

def setStatus(httpGet, head, writer):
    op = httpGet.GetStringValue("op")
    endDate = MathUtils.ToDateTime(httpGet.GetStringValue("EndDate"))
    userID = httpGet.GetStringValue("userID")
    oAOperationLog = OAOperationLog(Guid.NewGuid().ToString())
    oAOperationLog.UserID = userID
    oAOperationLog.OpUserID = httpGet.GetIntValue("OpUserID")
    oAOperationLog.CreateDate = DateTime.Now
    oAOperationLog.EndDate = endDate
    oAOperationLog.Reason = httpGet.GetStringValue("Reason")
    cacheSet = GameDataCacheSet[GameUser]()
    gameUser = cacheSet.FindKey(userID)
    if gameUser:
        if (op == "disableid"):
            gameUser.UserStatus = UserStatus.FengJin
            oAOperationLog.OpType = 1
        elif (op == "enableid"):
            gameUser.UserStatus = UserStatus.Normal
            oAOperationLog.OpType = 2
        elif (op == "disablemsg"):
            gameUser.MsgState = False
            oAOperationLog.OpType = 3
        elif (op == "enablemsg"):
            gameUser.MsgState = True
            oAOperationLog.OpType = 4
        cacheSet.UpdateSelf(userID)

    DataSyncManager.GetDataSender().Send(oAOperationLog)