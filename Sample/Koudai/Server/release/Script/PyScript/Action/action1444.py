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

# 1444_佣兵升级卡片列表接口
class UrlParam:
    Result = True;
    pageIndex = 0;
    pageSize = 0;   
    itemNum = 0;
    userID = ''; 

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.pageCount = 1
        self.useritemList = []
        self.userItemNum=1      #使用经验卡的数量   

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("PageIndex")\
    and httpGet.Contains("PageSize"):
        urlParam.pageIndex = httpGet.GetIntValue("PageIndex", 1, 100 );
        urlParam.pageSize = httpGet.GetIntValue("PageSize");
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    urlParam.userID = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    package = UserItemPackage.Get(userId);
    if package != None:
        useritemArray = package.ItemPackage.FindAll(lambda s : s.ItemType == ItemType.DaoJu and s.PropType == 14);   
        result = MathUtils.GetPaging[UserItemInfo](useritemArray,urlParam.pageIndex,urlParam.pageSize);
        if result :
           actionResult.useritemList =  result[0];
           actionResult.pageCount = result[1];
    actionResult.userItemNum=1                  #界面使用经验卡数量默认为1
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    itemName ='';
    headId='';
    effectNum =0;
    writer.PushIntoStack(actionResult.pageCount);
    writer.PushIntoStack(actionResult.useritemList.Count);
    for item in actionResult.useritemList:
        itemInfo = ConfigCacheSet[ItemBaseInfo]().FindKey(item.ItemID);
        #itemNum = UserItemHelper.CheckItemNum(urlParam.userID, item.ItemID);
        if itemInfo != None:
            itemName = itemInfo.ItemName;
            headId = itemInfo.HeadID;
            effectNum = itemInfo.EffectNum;
        dsItem = DataStruct();
        dsItem.PushIntoStack(MathUtils.ToNotNullString(item.UserItemID));
        dsItem.PushIntoStack(item.ItemID);
        dsItem.PushIntoStack(MathUtils.ToNotNullString(itemName));
        dsItem.PushIntoStack(MathUtils.ToNotNullString(headId));
        dsItem.PushIntoStack(item.Num);
        dsItem.PushIntoStack(effectNum);
        dsItem.PushIntoStack(actionResult.userItemNum);     #界面使用经验卡数量默认为1
        writer.PushIntoStack(dsItem);
    return True;