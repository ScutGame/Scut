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
from ZyGames.Framework.Common.MathUtils import *
#魂技列表接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.PageIndex = 1;
        self.PageSize = 20;
        self.Ops = 0;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.PageCount = 1;
        self.UserID =0 ;
        self.List = [];

def getUrlElement(httpGet,parent):
    urlParam = UrlParam();
    if httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize")\
    and httpGet.Contains("Ops"):
        urlParam.PageIndex =  httpGet.GetIntValue("PageIndex", 1, 100 );
        urlParam.PageSize =  httpGet.GetIntValue("PageSize");
        urlParam.Ops =  httpGet.GetIntValue("Ops", 0, 2);
    else:
        urlParam.Result = False;
    return urlParam;

def sortAbility(x, y):
    result = 0;
    if x == None and y == None:
        return 0;
    elif x and y == None:
        return 1;
    elif x == None:
        return -1;

    result = y.AbilityQuality.CompareTo(x.AbilityQuality);
    if result == 0:
        result = y.GeneralID.CompareTo(x.GeneralID);
    return result;

def takeAction(urlParam,parent):
    #GameService.getUser();
    #for key, value in urlParam.items():
    #    TraceLog.ReleaseWrite('{0}={1}',key,value);
    #TraceLog.ReleaseWrite('1004 param BackpackType:{0}', urlParam.BackpackType);
    actionResult =ActionResult();
    userId =parent.Current.UserId;
    actionResult.UserID = userId;
    cacheSetUserAbility =  GameDataCacheSet[UserAbility]();
    cacheSetGeneral = GameDataCacheSet[UserGeneral]();
    userAbility = cacheSetUserAbility.FindKey(userId.ToString());
    if userAbility and userAbility.AbilityList :

        list = List[Ability]();
        if(urlParam.Ops==0):
            list = userAbility.AbilityList.ToList();
        else:
            if(urlParam.Ops==1):
                list = userAbility.AbilityList.FindAll(lambda  s:s.GeneralID<=0);
            else:
                if(urlParam.Ops==2):
                    allList = userAbility.AbilityList;
                    for info in allList:
                        if(info.GeneralID > 0):
                            generalAbility = cacheSetGeneral.Find(userId.ToString(),lambda s:s.AbilityID == info.AbilityID );
                            if(generalAbility == None):
                                list.Add(info);
                        else:
                            list.Add(info);

        # 添加排序
        MathUtils.QuickSort[Ability](list, lambda x,y:sortAbility(x,y));
        pageCount = 1;
        result = MathUtils.GetPaging[Ability](list,urlParam.PageIndex,urlParam.PageSize);
        if result:
            actionResult.List =  result[0];
            actionResult.PageCount = result[1];
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    #输出
    cacheSetAbility =  ConfigCacheSet[AbilityInfo]();
    cacheSetGeneral =  ConfigCacheSet[GeneralInfo]();
    cacheSetUserGeneral =  GameDataCacheSet[UserGeneral]();
    writer.PushIntoStack(actionResult.PageCount);
    writer.PushIntoStack(len(actionResult.List));
    for info in actionResult.List:
        abilityInfo = cacheSetAbility.FindKey(info.AbilityID);
        userGeneral = cacheSetUserGeneral.FindKey(actionResult.UserID.ToString(),info.GeneralID);
        generalInfo = cacheSetGeneral.FindKey(info.GeneralID);
        if abilityInfo :
            ds = DataStruct();
            ds.PushIntoStack(info.AbilityID);
            ds.PushIntoStack(abilityInfo.AbilityName);
            ds.PushIntoStack(abilityInfo.HeadID);
            ds.PushIntoStack(abilityInfo.AbilityDesc);
            ds.PushIntoStack(info.AbilityLv);
            ds.PushIntoStack(info.GeneralID);
            if(userGeneral):
                ds.PushIntoStack(userGeneral.GeneralName);
            else:
                ds.PushIntoStack('');
            ds.PushIntoStack(info.UserItemID);
            ds.PushIntoStack(Convert.ToInt32(abilityInfo.AttackUnit));
            ds.PushIntoStack(abilityInfo.AbilityQuality);
            ds.PushIntoStack(MathUtils.ToInt((abilityInfo.RatioNum*100)));
            writer.PushIntoStack(ds);
        else:
            ds = DataStruct();
            ds.PushIntoStack(0);
            ds.PushIntoStack('');
            ds.PushIntoStack('');
            ds.PushIntoStack('');
            ds.PushIntoStack(0);
            ds.PushIntoStack(0);
            #ds.PushIntoStack('');
            ds.PushIntoStack('');
            ds.PushIntoStack('');
            ds.PushIntoStack(0);
            ds.PushIntoStack(0);
            writer.PushIntoStack(ds);
    return True;