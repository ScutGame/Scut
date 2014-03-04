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
#祈祷祥细接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PrayType = '';
        self.PrayNum = 0;
        self.IsPrayNum = 0;
        self.PrayDesc = 0;
        self.IsStatu = 1;

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
    
    if userPray:
        prayInfo = _cacheSetPray.FindKey(Convert.ToInt32(userPray.PrayType));
        actionResult.PrayType = Convert.ToInt32(userPray.PrayType);
        actionResult.IsPrayNum = userPray.PrayNum;
        if prayInfo:
            dayNum = Convert.ToInt32(MathUtils.DiffDate(DateTime.Now.Date,userPray.PrayDate.Date).TotalDays);
            actionResult.PrayNum = prayInfo.DayNum;
            actionResult.PrayDesc = prayInfo.Describe;
            if(dayNum==0):
                actionResult.IsStatu = 1;
            else:
                actionResult.IsStatu = 0;
            if((actionResult.PrayNum <= actionResult.IsPrayNum and dayNum != 0) or (actionResult.PrayNum > actionResult.IsPrayNum and (dayNum>1 or dayNum< 0))):
                prayType = RandomUtils.GetRandom(1,4);
                if(prayType==1):
                    userPray.PrayType = PrayType.SanTianQiDao;
                if(prayType==2):
                    userPray.PrayType = PrayType.WuTianQiDao;
                if(prayType==3):
                    userPray.PrayType = PrayType.QiTianQiDao;
                if(prayType==4):
                    userPray.PrayType = PrayType.JiuTianQiDao;
                userPray.PrayDate = DateTime.Now.AddDays(-1);
                userPray.PrayNum = 0
                userPray.IsPray = False;
                actionResult.IsPrayNum = 0;
                actionResult.PrayType = prayType;
                prayInfo = _cacheSetPray.FindKey(prayType);
                if(prayInfo):
                    actionResult.PrayDesc = prayInfo.Describe;
                    actionResult.PrayNum = prayInfo.DayNum;
                    #userPray.PrayNum = prayInfo.DayNum;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    writer.PushIntoStack(actionResult.PrayType);
    writer.PushIntoStack(actionResult.PrayNum);
    writer.PushIntoStack(actionResult.IsPrayNum);
    writer.PushIntoStack(actionResult.PrayDesc);
    writer.PushIntoStack(actionResult.IsStatu);
    return True;