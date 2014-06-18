"""1009_社交资料接口"""
import ReferenceLib
from action import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Game.Lang import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll.Logic import *

class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)


class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.Name = ''
        self.Sex = 0
        self.Birthday = ''
        self.Hobby = ''
        self.Profession = ''


def getUrlElement(httpGet, parent):
    urlParam = UrlParam()
    if True:
        urlParam.Result = True
    else:
        urlParam.Result = False

    return urlParam

def takeAction(urlParam, parent):
    actionResult = ActionResult()
    user = parent.Current.User
    if not user:
        parent.ErrorCode = Language.Instance.ErrorCode
        parent.ErrorInfo = Language.Instance.LoadDataError
        actionResult.Result = False
        return actionResult
    actionResult.Name = user.RealName
    actionResult.Sex = user.Sex
    actionResult.Birthday = user.Birthday.ToString('yyyy-MM-dd')
    actionResult.Hobby = user.Hobby
    actionResult.Profession = user.Profession
    return actionResult

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(actionResult.Name)
    writer.PushByteIntoStack(actionResult.Sex)
    writer.PushIntoStack(actionResult.Birthday)
    writer.PushIntoStack(actionResult.Hobby)
    writer.PushIntoStack(actionResult.Profession)

    return True