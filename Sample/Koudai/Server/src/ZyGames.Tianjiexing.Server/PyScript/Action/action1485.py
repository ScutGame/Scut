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
#魂技升级详细接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.UserItemID = ''

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.AbilityLv = 0
        self.AbilityInfo = []

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    if httpGet.Contains("UserItemID"):
        urlParam.UserItemID =  httpGet.GetStringValue("UserItemID", 36, 36);
    else:
        urlParam.Result = False;
    return urlParam;
def takeAction(urlParam,parent):
    #GameService.getUser();
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    actionResult =ActionResult();
    userId =parent.Current.UserId;
    cacheSetAbility =  ConfigCacheSet[AbilityInfo]();
    cacheSetUserAbility =  GameDataCacheSet[UserAbility]();
    userAbility = cacheSetUserAbility.FindKey(userId.ToString());
    if (userAbility != None and userAbility.AbilityList != None):
        ability =  userAbility.AbilityList.Find(lambda  s:MathUtils.ToNotNullString(s.UserItemID) == MathUtils.ToNotNullString(urlParam.UserItemID));
        if(ability):
            actionResult.AbilityLv = ability.AbilityLv;
            actionResult.AbilityInfo = cacheSetAbility.FindKey(ability.AbilityID);
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    info = actionResult.AbilityInfo
    if info:
        writer.PushIntoStack(actionResult.AbilityInfo.AbilityID);
        writer.PushIntoStack(actionResult.AbilityInfo.HeadID);
        writer.PushIntoStack(MathUtils.ToNotNullString(info.AbilityName));
        writer.PushIntoStack(MathUtils.ToNotNullString(info.AbilityDesc));
        writer.PushIntoStack(MathUtils.ToNotNullString(info.EffectDesc));
        writer.PushIntoStack(MathUtils.ToNotNullString(info.MaxHeadID));
        writer.PushIntoStack(MathUtils.ToNotNullString(info.FntHeadID));
        writer.PushIntoStack(actionResult.AbilityLv);
        writer.PushIntoStack(info.AbilityQuality);
        writer.PushIntoStack(MathUtils.ToInt(info.AttackUnit));
    else:
        writer.PushIntoStack(0);
        writer.PushIntoStack('');
        writer.PushIntoStack('');
        writer.PushIntoStack('');
        writer.PushIntoStack('');
        writer.PushIntoStack('');
        writer.PushIntoStack('');
        writer.PushIntoStack(0);
        writer.PushIntoStack(0);
        writer.PushIntoStack(0);
    return True;