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
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.BLL.Action import *

# 1023_玩家改名
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.nickName = ''

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("NickName"):
        urlParam.nickName = httpGet.GetStringValue("NickName");
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    nickName = urlParam.nickName.lstrip().rstrip();

    MaxLength = ConfigEnvSet.GetInt("User.MaxLength");
    itemID = ConfigEnvSet.GetInt("UserName.ItemID");
    length = Text.Encoding.Default.GetByteCount(nickName);
    if length <= 0 or length > MaxLength:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1005_KingNameTooLong.format(MaxLength);
        actionResult.Result = False;
        return actionResult;
    # 屏蔽敏感词
    if UserHelper.GetKeyWordSubstitution(nickName):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1005_RegistNameKeyWord;
        actionResult.Result = False;
        return actionResult;

    # 是否已经存在该昵称
    if GameUser.IsNickName(nickName):
        parent.ErrorCode = 1;
        parent.ErrorInfo = LanguageManager.GetLang().St1005_Rename;
        actionResult.Result = False;
        return actionResult;
    package = UserItemPackage.Get(userId);
    userItem = package.ItemPackage.Find(match=lambda m:not m.IsRemove and m.ItemID == itemID);
    if userItem == None or userItem.Num <= 0:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        actionResult.Result = False;
        return actionResult;
    contextUser.NickName = nickName;
        
    ranking = RankingFactory.Get[UserRank](CombatRanking.RankingKey);
    if ranking.TryGetRankNo(match=lambda m:m.UserID == userId):
        rankInfo = ranking.Find(match=lambda s:s.UserID == userId);
        if rankInfo:
            rankInfo.NickName = nickName;
    UserItemHelper.UseUserItem(userId, itemID, 1);

    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    return True 