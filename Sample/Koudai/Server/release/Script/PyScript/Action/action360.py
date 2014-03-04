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
from ZyGames.Framework.Game.Context import *
from ZyGames.Framework.Game.Sns.Section import *
from ZyGames.Framework.Game.Sns import *
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

# 360_支付充值实时获取ACCESS_TOKEN
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.RefeshToken = ''
        self.Scope = ''
        self.RetailID = ''

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.AccessToken = ''

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("RefeshToken")\
    and httpGet.Contains("RetailID")\
    and httpGet.Contains("Scope"):
        urlParam.RefeshToken = httpGet.GetStringValue("RefeshToken")
        urlParam.Scope = httpGet.GetStringValue("Scope")
        urlParam.RetailID = httpGet.GetStringValue("RetailID")
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    sec = SdkSectionFactory.Section360;
    appKey =''
    appSecret = ''
    url = '{0}?grant_type=refresh_token&refresh_token={1}&client_id={2}&client_secret={3}&scope={4}'
    
    if sec:
        els = sec.Channels[urlParam.RetailID];
        if els:
            appKey = els.AppKey
            appSecret = els.AppSecret
            url =url.format(sec.GetAceessTokenUrl,urlParam.RefeshToken,appKey,appSecret,urlParam.Scope)
            TraceLog.WriteError('获取360 access_token：url={0}',url)
    result = HttpRequestManager.GetStringData(url,'GET')
    
    getToken = JsonUtils.Deserialize[Login360_V2.SDK360GetTokenError](result)
    if getToken == None:
        actionResult.AccessToken = contextUser.AccessToken
    else:
        if getToken and getToken.error_code != None  and getToken.error_code !='':
            parent.ErrorCode = Lang.getLang("ErrorCode")
            parent.ErrorInfo = Lang.getLang("GetAccessFailure")
            actionResult.Result = False;
            TraceLog.WriteError('获取360 access_token 失败：url={0},result={1},error_code={2},error={3}',url,result,getToken.error_code,getToken.error)
            return actionResult;
        actionResult.AccessToken = getToken.access_token
        contextUser.AccessToken = actionResult.AccessToken
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.AccessToken)
    return True;