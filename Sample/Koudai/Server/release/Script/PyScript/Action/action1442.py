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
from lang import Lang
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

# 1442_佣兵升级接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.generalID = 0

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("GeneralID"):
        urlParam.generalID = httpGet.GetIntValue("GeneralID");
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
   
    package = UserItemPackage.Get(userId);
    userGeneral = GameDataCacheSet[UserGeneral]().FindKey(userId, urlParam.generalID);
    general = ConfigCacheSet[GeneralInfo]().FindKey(urlParam.generalID);
    if package == None or general == None:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    if userGeneral == None:
        parent.ErrorCode = Lang.getLang("ErrorCode");
        parent.ErrorInfo = Lang.getLang("LoadError");
        actionResult.Result = False;
        return actionResult;
    if not userGeneral.GeneralCard:
        parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
        parent.ErrorInfo = LanguageManager.GetLang().St1442_SelectTheExperienceCard;
        actionResult.Result = False;
        return actionResult;
    if userGeneral.GeneralLv >= (contextUser.UserLv*3):
       parent.ErrorCode = Lang.getLang("ErrorCode");
       parent.ErrorInfo = Lang.getLang("St1442_GeneralLvNotUserLv");
       actionResult.Result = False;
       return actionResult;
    itemStr = userGeneral.GeneralCard.Split(',');
    itemNum=userGeneral.GeneralCardNum.Split(',');   #获取佣兵经验卡的数量
    isUse = False;
    #总的经验
    i=0               #用于循环到第几个佣兵
    for str in itemStr:
        exprience = 0; 
        useritem = package.ItemPackage.Find(lambda s:not s.IsRemove and s.UserItemID == str);
        if useritem :
            itemInfo = ConfigCacheSet[ItemBaseInfo]().FindKey(useritem.ItemID);  
            if itemInfo != None:
                conunt = itemNum[i]
                conunt =MathUtils.ToInt(conunt)
                for expri in range(conunt):
                    exprience = MathUtils.ToInt(itemInfo.EffectNum);    #经验多张佣兵卡
                    if general.ItemID == itemInfo.ItemID:
                        exprience = MathUtils.Addition(exprience,exprience*GameConfigSet.ExpMultiple);   #经验多张佣兵卡
                    # 判断佣兵等级是否达到上限
                    status = UserHelper.GeneralLvIsUserLv(userId,urlParam.generalID,exprience,contextUser.UserLv);
                    if status == 0:
                        GeneralHelper.UserGeneralExp(userId, urlParam.generalID, exprience);  # 佣兵加经验
                        UserItemHelper.UseUserItem(userId, useritem.ItemID, 1);  # 使用物品
                    isUse = True;
                    #if status == 2:
                    #    parent.ErrorCode = Lang.getLang("ErrorCode");
                    #    parent.ErrorInfo = Lang.getLang("St1442_GeneralLvNotUserLv");
                    #    actionResult.Result = False;
                    #    return actionResult;
                    if status == 3:
                        parent.ErrorCode = Lang.getLang("ErrorCode");
                        parent.ErrorInfo = Lang.getLang("LoadError");
                        actionResult.Result = False;
                        return actionResult;
                    if status == 1:
                        parent.ErrorCode = Lang.getLang("ErrorCode");
                        parent.ErrorInfo = Lang.getLang("St1442_GeneralLvIsMax");
                        actionResult.Result = False;
                        return actionResult;
                    if isUse:
                        userGeneral.GeneralCard = '';
        i+=1  
    userGeneral.RefreshMaxLife();
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    
    return True;