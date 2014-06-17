"""1011_社交资料保存接口"""
import ReferenceLib
from action import *
from lang import Lang
from System import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Message import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll.Logic import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.Name = ''
        self.Sex = 0
        self.Birthday = ''
        self.Profession = ''
        self.Hobby = ''


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if httpGet.Contains("Name")\
    and httpGet.Contains("Sex")\
    and httpGet.Contains("Birthday")\
    and httpGet.Contains("Profession")\
    and httpGet.Contains("Hobby"):
        urlParam.Name = httpGet.GetStringValue("Name")
        urlParam.Sex = httpGet.GetByteValue("Sex")
        urlParam.Birthday = httpGet.GetStringValue("Birthday")
        urlParam.Profession = httpGet.GetStringValue("Profession")
        urlParam.Hobby = httpGet.GetStringValue("Hobby")
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    if not user:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("LoadError")
        actionResult.Result = False
        return actionResult
    if len(urlParam.Name)>0 and (getGbLen(urlParam.Name) < 2 or getGbLen(urlParam.Name) > 8):
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("St_1011RealNameRangeOut")
        actionResult.Result = False
        return actionResult
    
    birthday = DateTime.TryParse(urlParam.Birthday)
    if not birthday[0]:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("St_1011BirthdayError")
        actionResult.Result = False
        return actionResult
    
    if getGbLen(urlParam.Hobby) > 8:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("St_1011HobbyRangeOut")
        actionResult.Result = False
        return actionResult
    
    if getGbLen(urlParam.Profession) > 8:
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("St_1011ProfessionRangeOut")
        actionResult.Result = False
        return actionResult
    service = SensitiveWordService()
    if service.IsVerified(urlParam.Name):
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("St_1011RealNameExistKeyWord")
        actionResult.Result = False
        return actionResult

    if service.IsVerified(urlParam.Hobby):
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("St_1011HobbyExistKeyWord")
        actionResult.Result = False
        return actionResult
    if service.IsVerified(urlParam.Profession):
        parent.ErrorCode = Lang.getLang("ErrorCode")
        parent.ErrorInfo = Lang.getLang("St_1011ProfessionExistKeyWord")
        actionResult.Result = False
        return actionResult

    user.RealName = urlParam.Name
    user.Sex = urlParam.Sex
    user.Birthday = birthday[1]
    user.Hobby = urlParam.Hobby
    user.Profession = urlParam.Profession
    return actionResult

def buildPacket(writer, urlParam, actionResult):

    return True