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
from ZyGames.Tianjiexing.BLL.Base import *
#祈祷接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.Cue = 0

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    return urlParam;
def takeAction(urlParam,parent):
    #GameService.getUser();
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    actionResult =ActionResult();
    userId =parent.Current.UserId;
    _cacheSetUserPray =  GameDataCacheSet[UserPray]();
    _cacheSetPray =  ConfigCacheSet[PrayInfo]();
    userPray = _cacheSetUserPray.FindKey(userId.ToString());
    prayInfo = _cacheSetPray.FindKey(Convert.ToInt32(userPray.PrayType));
    if (prayInfo == None or userPray == None):
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().LoadDataError;
        actionResult.Result = False;
        return actionResult;
     
    dayNum = Convert.ToInt32(MathUtils.DiffDate(DateTime.Now.Date,userPray.PrayDate.Date).TotalDays);
    if(dayNum < 0):
        dayNum = 100;
    if  dayNum == 0 and prayInfo.DayNum > userPray.PrayNum:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St3302_IsPray;
        actionResult.Result = False;
        return actionResult;
    if(dayNum > 1 or (dayNum==1 and  prayInfo.DayNum == userPray.PrayNum)):
        prayType = RandomUtils.GetRandom(1,4);
        if(prayType==1):
            userPray.PrayType = PrayType.SanTianQiDao;
        if(prayType==2):
            userPray.PrayType = PrayType.WuTianQiDao;
        if(prayType==3):
            userPray.PrayType = PrayType.QiTianQiDao;
        if(prayType==4):
            userPray.PrayType = PrayType.JiuTianQiDao;
        userPray.PrayDate = DateTime.Now;
        userPray.PrayNum = 1
        userPray.IsPray = False;
        dayNum = Convert.ToInt32(MathUtils.DiffDate(DateTime.Now.Date,userPray.PrayDate.Date).TotalDays);
    if(dayNum==1 and prayInfo.DayNum > userPray.PrayNum):
        userPray.PrayDate = DateTime.Now;
        userPray.PrayNum = MathUtils.Addition(userPray.PrayNum,1);
        userPray.IsPray = True;
    if( prayInfo.DayNum <= userPray.PrayNum and userPray.IsPray == True):
        actionResult.Cue = UserPrayHelper.GetUserTake(prayInfo.PrayReward.ToList(),userId.ToString());
        userPray.IsPray = False;
        #actionResult.PrayType = Convert.ToInt32(userPray.PrayType);
        #actionResult.IsPrayNum = userPray.PrayNum;
        #if prayInfo:
        #    actionResult.PrayNum = prayInfo.DayNum;
        #    actionResult.PrayDesc = prayInfo.Describe;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(actionResult.Cue);
    return True;