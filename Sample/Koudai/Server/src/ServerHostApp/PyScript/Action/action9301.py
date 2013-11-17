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

# 9301_好友发件接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.title = ''
        self.content = ''
        self.toUserID = 0
        self.toUserName = ''
        self.isGuide = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("Title")\
    and httpGet.Contains("Content")\
    and httpGet.Contains("ToUserID")\
    and httpGet.Contains("ToUserName"):
        urlParam.toUserID = httpGet.GetIntValue("ToUserID")
        urlParam.toUserName = httpGet.GetString("ToUserName");
        urlParam.title = httpGet.GetString("Title");
        urlParam.content = httpGet.GetString("Content");
        urlParam.isGuide = httpGet.GetIntValue("IsGuide");
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.UserId;
    userName = parent.Current.User.NickName;
    
    mailInfoCacheSet = ShareCacheStruct[MailInfo]();
    mailInfo = mailInfoCacheSet.Find(lambda s:s.MailType ==  MathUtils.ToInt(MailType.Friends));
    if not mailInfo:
        actionResult.Result = False;
        return actionResult;

    # 判断是否为好友
    userFriendsCacheSet = ShareCacheStruct[UserFriends]();
    isUserFriends = userFriendsCacheSet.FindKey(userId.ToString(), urlParam.toUserID);
    isFriends = userFriendsCacheSet.FindKey(urlParam.toUserID, userId.ToString());
    if (isUserFriends and (isUserFriends.FriendType == FriendType.Friend)) or (isFriends and (isFriends.FriendType == FriendType.Friend)):
        contentLength = Text.Encoding.Default.GetByteCount(urlParam.content)
        if contentLength > mailInfo.MaxLength:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St9302_OverMaxLength");
            actionResult.Result = False;
            return actionResult;
        tempMail = UserMail(Guid.NewGuid());
        tempMail.UserId = urlParam.toUserID;
        tempMail.MailType =  MailType.Friends;
        tempMail.FromUserId = userId;
        tempMail.FromUserName = userName;
        tempMail.ToUserID = urlParam.toUserID;
        tempMail.ToUserName = urlParam.toUserName;
        tempMail.Title = urlParam.title;
        tempMail.Content = urlParam.content;
        tempMail.SendDate = DateTime.Now;
        tempMail.IsGuide = urlParam.isGuide;
        tjxMailService = TjxMailService(parent.Current.User);
        tjxMailService.Send(tempMail);
        return actionResult;

    parent.ErrorCode = Lang.getLang("ErrorCode");
    parent.ErrorInfo = Lang.getLang("St9302_IsNotFriend");
    actionResult.Result = False;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    
    return True;