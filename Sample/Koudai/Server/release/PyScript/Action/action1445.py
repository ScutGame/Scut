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
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *

# 1445_佣兵升级卡片选择接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.generalCard = ''
        self.generalID = 0
        self.GeneralCardNum = ''

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("GeneralCard")\
    and httpGet.Contains("GeneralID")\
    and httpGet.Contains("GeneralCardNum"):
        urlParam.generalCard = httpGet.GetStringValue("GeneralCard");
        urlParam.generalID = httpGet.GetIntValue("GeneralID");
        urlParam.GeneralCardNum = httpGet.GetStringValue("GeneralCardNum")    #对应佣兵列表的数量
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    if not urlParam.generalCard:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        actionResult.Result = False;
        return actionResult;
    userGeneral = GameDataCacheSet[UserGeneral]().FindKey(userId, urlParam.generalID);
    if userGeneral != None:
        userGeneral.GeneralCard = urlParam.generalCard;
        userGeneral.GeneralCardNum=MathUtils.ToNotNullString(urlParam.GeneralCardNum)         #对应佣兵列表的数量
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
   
    return True;