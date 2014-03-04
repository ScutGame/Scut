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
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Component import *
from ZyGames.Framework.Common.Serialization import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Component.Chat import *

# 9302_玩家收件接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.mailType = None;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.userMail = []

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    urlParam.mailType = httpGet.GetEnum[MailType]("MailType");
    return urlParam;

# 多于 15 条需移除
def removeMail(userMailList,mailCount):
    cacheSet = GameDataCacheSet[UserMail]();
    maxMailNumber = 15;
    if mailCount > maxMailNumber:
        for i in range(maxMailNumber, mailCount):
            item = userMailList[i];
            item.IsRemove = 1;
            item.RemoveDate = DateTime.Now;
            cacheSet.RemoveCache(item);

# 所有邮件默认隔一周清空一次
def clearMail(userMailList):
    cacheSet = GameDataCacheSet[UserMail]();
    for mail in userMailList:
        days = (DateTime.Now - mail.SendDate).Days;
        if days >= 7:
            mail.IsRemove = 1;
            mail.RemoveDate = DateTime.Now;
            cacheSet.RemoveCache(mail);

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.UserId;

    # 没有 Email
    def loadError():
        parent.ErrorCode = Lang.getLang("ErrorCode");
        #parent.ErrorInfo = Lang.getLang("St9302_NoMail");
        # 由客户端提示相关信息
        actionResult.Result = False;
        return actionResult;

    tjxMailService = TjxMailService(parent.Current.User);
    mailList =[]
    result = tjxMailService.GetMail();
    if not result:
        return loadError();

    mailList = result[0];
    if mailList:
        # 清空过期信件
        clearMail(mailList);

    tempList = []
    result = tjxMailService.GetMail();
    if not result:
        return loadError();

    mailList = result[0];
    # 删除多于 15 以上的邮件
    mailTypeNum = ShareCacheStruct[MailInfo]().FindAll(True).Count;
    for i in range(1,mailTypeNum+1):
        item = MathUtils.ToEnum[MailType](i)
        tempList = mailList.FindAll(lambda s:s.MailType == MathUtils.ToEnum[MailType](i) and s.UserId == userId);
        removeMail(tempList,len(tempList));

    # 取出所有信件
    result = tjxMailService.GetMail();
    # mailList = result[0];
    if result:
        mailList = result[0];
        allMail = mailList.FindAll(lambda s:s.UserId == userId);
        if urlParam.mailType:
            actionResult.userMail = mailList.FindAll(lambda s:s.MailType == urlParam.mailType and s.UserId == userId);
        else:
            actionResult.userMail = allMail;
        # 获取该玩家的所有信件，将其 IsRead 属性置为 True
        for item in allMail:
            item.IsRead = True;
    else:
        return loadError();
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    writer.PushIntoStack(len(actionResult.userMail));
    mailInfoCacheSet = ShareCacheStruct[MailInfo]();

    for item in actionResult.userMail:
        dsItem = DataStruct();
        mailType = item.MailType;
        dsItem.PushIntoStack(item.UserId);
        dsItem.PushShortIntoStack(MathUtils.ToShort(mailType));
        dsItem.PushIntoStack(item.FromUserId);
        dsItem.PushIntoStack(item.FromUserName if item.FromUserName else '');
        dsItem.PushIntoStack(item.Title);
        dsItem.PushIntoStack(item.Content);
        diffDays = (DateTime.Now-item.SendDate).Days;
        if diffDays < 1:
            diffHours = (DateTime.Now-item.SendDate).Hours;
            if diffHours < 1:
                diffMinutes = (DateTime.Now-item.SendDate).Minutes;
                minutesMsg = Lang.getLang("St9302_Minutes");
                dsItem.PushIntoStack('{0}{1}'.format(diffMinutes, minutesMsg));
            else:
                hourMsg = Lang.getLang("St9302_Hours");
                dsItem.PushIntoStack('{0}{1}'.format(diffHours, hourMsg));
        elif diffDays >= 1 and diffDays < 3:
            dayMsg = Lang.getLang("St9302_Days");
            dsItem.PushIntoStack('{0}{1}'.format(diffDays, dayMsg));
        else:
            dsItem.PushIntoStack(item.SendDate.ToString());
        #diffDays = (DateTime.Now-item.SendDate).TotalDays;
        #if diffDays < 1:
        #    diffHours = (DateTime.Now-item.SendDate).TotalHours;
        #    dsItem.PushIntoStack('{0}{1}'.format(diffHours, Lang.getLang("St9302_Hours")));
        #elif diffDays >= 1 and diffDays < 3:
        #    dsItem.PushIntoStack('{0}{1}'.format(diffDays, Lang.getLang("St9302_Days")));
        #else:
        #    dsItem.PushIntoStack(item.SendDate.ToString());
        #dsItem.PushByteIntoStack(item.IsRead);
        dsItem.PushByteIntoStack(item.IsGuide);

        if item.IsGuide == 1:
            mailInfo = mailInfoCacheSet.Find(lambda s:s.MailType == MathUtils.ToInt(mailType));
            if mailInfo:
                dsItem.PushIntoStack(mailInfo.GuideContent);
            else: # 如果获取不到相应类型的引导内容，下发空
                dsItem.PushIntoStack('');
        else:
            dsItem.PushIntoStack('');
        dsItem.PushByteIntoStack(item.IsReply);
        dsItem.PushShortIntoStack(MathUtils.ToShort(item.ReplyStatus));
        dsItem.PushIntoStack(MathUtils.ToNotNullString(item.MailID));
        dsItem.PushIntoStack(item.CounterattackUserID if item.CounterattackUserID else 0);
        dsItem.PushIntoStack(MathUtils.ToNotNullString(item.SendDate));
        writer.PushIntoStack(dsItem);
    return True;