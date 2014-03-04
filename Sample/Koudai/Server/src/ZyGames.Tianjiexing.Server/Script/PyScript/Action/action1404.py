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
from randomUtils import ZyRandomUtils;
from mathUtils import ZyMathUtils;
from ZyGames.Tianjiexing.Component.Chat import *
# 1404_佣兵招募接口
class UrlParam(HttpParam):
    def __init__(self):
        HttpParam.__init__(self)
        self.recruitType = None;
        self.soulID = 0
        self.IsLead = 0
        self.currSoulID = 0
        self.gainNum = 0
        self.general = None
        self.generalType = 0
        self.recruitType = None;
        self.soulID = 0;
        self.currSoulID = 0;
        self.gainNum = 0;
        self.general = None;
        self.generalType = 0;
        self.generalQuality = GeneralQuality.White;
        self.abilityInfo = None;

class ActionResult(DataResult):
    def __init__(self):
        DataResult.__init__(self)
        self.shengMing = 0;
        self.powerNum = 0;
        self.soulNum = 0;
        self.intelligenceNum = 0;
        self.potential = 0;
        self.user = None;

def getUrlElement(httpGet, parent):
    urlParam = UrlParam();
    if httpGet.Contains("RecruitType"):
        urlParam.recruitType = httpGet.GetEnum[RecruitType]("RecruitType");
        #urlParam.recruitType = httpGet.GetWordValue("RecruitType");        
        urlParam.soulID = httpGet.GetIntValue("SoulID");
        urlParam.IsLead = httpGet.GetIntValue("IsLead")
    else:
        urlParam.Result = False;
    return urlParam;

def takeAction(urlParam, parent):
    actionResult = ActionResult();
    userId = parent.Current.User.PersonalId;
    contextUser = parent.Current.User;
    actionResult.user = parent.Current.User;

    cacheSet = GameDataCacheSet[UserGeneral]();
    if urlParam.recruitType == RecruitType.SoulRecruit:
        soulGeneral = cacheSet.FindKey(userId, urlParam.soulID);
        if soulGeneral is None:
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            actionResult.Result = False;
            return actionResult;
        urlParam.general = ConfigCacheSet[GeneralInfo]().Find(lambda s:s.SoulID == urlParam.soulID);
        generalInfo = urlParam.general;
        if urlParam.general == None:
            urlParam.general = ConfigCacheSet[GeneralInfo]().FindKey(urlParam.soulID);
        if urlParam.general == None:
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            actionResult.Result = False;
            return actionResult;
        if soulGeneral.AtmanNum < urlParam.general.DemandNum:
            parent.ErrorCode = Lang.getLang("ErrorCode");
            parent.ErrorInfo = Lang.getLang("St1404_OverfulfilNumNotEnough");
            actionResult.Result = False;
            return actionResult;
        urlParam.generalQuality = urlParam.general.GeneralQuality;
        userGeneral = cacheSet.FindKey(userId, urlParam.general.GeneralID);
        if userGeneral != None:
            if userGeneral.GeneralStatus == GeneralStatus.YinCang:
                userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
                userGeneral.GeneralLv = MathUtils.ToShort(urlParam.general.GeneralLv);
                userGeneral.LifeNum = urlParam.general.LifeNum;
                userGeneral.PowerNum = urlParam.general.PowerNum;
                userGeneral.SoulNum = urlParam.general.SoulNum;                
                userGeneral.IntellectNum = urlParam.general.IntellectNum;
                userGeneral.CurrExperience = 0;    
                userGeneral.HeritageType = HeritageType.Normal;               
            else :
                PotentialArry = ConfigEnvSet.GetString("User.GeneralCardPotential").Split(',');
                potential = MathUtils.ToInt(PotentialArry[MathUtils.ToInt(urlParam.general.GeneralQuality)-1]);
                userGeneral.Potential = MathUtils.Addition(userGeneral.Potential,potential);
        else:
            userGeneral = UserGeneral();
            userGeneral.UserID = userId;           
            userGeneral.GeneralID = urlParam.general.GeneralID;
            userGeneral.GeneralName = urlParam.general.GeneralName;
            userGeneral.GeneralName = urlParam.general.GeneralName;
            userGeneral.HeadID = urlParam.general.HeadID;
            userGeneral.PicturesID = urlParam.general.PicturesID;
            userGeneral.GeneralLv = MathUtils.ToShort(urlParam.general.GeneralLv);
            userGeneral.LifeNum = urlParam.general.LifeNum;
            userGeneral.GeneralType = GeneralType.YongBing;
            userGeneral.CareerID = urlParam.general.CareerID;
            userGeneral.PowerNum = urlParam.general.PowerNum;
            userGeneral.SoulNum = urlParam.general.SoulNum;
            userGeneral.IntellectNum = urlParam.general.IntellectNum;
            userGeneral.TrainingPower = 0;
            userGeneral.TrainingSoul = 0;
            userGeneral.TrainingIntellect = 0;
            userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
            userGeneral.AbilityID = urlParam.general.AbilityID;
            userGeneral.Momentum = 0;
            userGeneral.Description = urlParam.general.Description;
            userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
            userGeneral.CurrExperience = 0;
            userGeneral.Experience1 = 0;
            userGeneral.Experience2 = 0;
            userGeneral.AbilityNum = 3;
            cacheSet.Add(userGeneral);
            UserAbilityHelper.AddUserAbility(urlParam.general.AbilityID, MathUtils.ToInt(userId), urlParam.general.GeneralID,1);
            # 添加集邮卡牌
            UserAlbumHelper.AddUserAlbum(userId, AlbumType.General, urlParam.general.GeneralID);
        soulGeneral.AtmanNum = MathUtils.Subtraction(soulGeneral.AtmanNum,urlParam.general.DemandNum);
    else:        
        UserHelper.ChechDailyRestrain(userId)
        isFirst = False;
        recruitRule = ConfigCacheSet[RecruitRule]().FindKey(MathUtils.ToInt(urlParam.recruitType));
        if recruitRule == None:
            parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
            actionResult.Result = False;
            return actionResult;
        surplusNum = GeneralHelper.SurplusNum(userId, recruitRule.FreeNum,MathUtils.ToEnum[RecruitType](recruitRule.RecruitType));
        if urlParam.IsLead == 1:
            GeneralHelper.UpdateDailyRecruitNum(userId, urlParam.recruitType);
            GeneralHelper.UpdateRecruitColdTime(userId, recruitRule);
            quality = GeneralQuality.Blue;
        else:
            mallPrice =  MathUtils.ToInt(FestivalHelper.StoreDiscount() * recruitRule.GoldNum);
            if mallPrice <= contextUser.GoldNum:
                if urlParam.recruitType == RecruitType.BaiLiTiaoYi and not GeneralHelper.IsFirstRecruit(actionResult.user,RecruitType.BaiLiTiaoYi):
                    quality = GeneralQuality.Blue;
                    isFirst = True;
                if urlParam.recruitType == RecruitType.Golden and not GeneralHelper.IsFirstRecruit(actionResult.user,RecruitType.Golden):
                    quality = GeneralQuality.Purple;
                    isFirst = True;

            if not isFirst and surplusNum > 0 and not GeneralHelper.GeneralRecruitColdTime(userId, urlParam.recruitType):
                GeneralHelper.UpdateDailyRecruitNum(userId, urlParam.recruitType);
                GeneralHelper.UpdateRecruitColdTime(userId, recruitRule);
            else:                
                if contextUser.GoldNum < mallPrice:
                    parent.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    parent.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    actionResult.Result = False;
                    return actionResult;
                contextUser.UseGold = MathUtils.Addition(contextUser.UseGold, mallPrice);        
        if not isFirst and urlParam.IsLead == 0:
            recruitInfos = recruitRule.GeneralQuality;
            probability = [];
            for i in range(0,recruitInfos.Count):
                probability.append(MathUtils.ToDouble(recruitInfos[i].Probability));
            random = ZyRandomUtils()
            index2 =  random.getHitIndex(probability);
            quality = recruitInfos[index2].Quality;

        generalList = ConfigCacheSet[GeneralInfo]().FindAll(lambda s:s.GeneralQuality == quality and s.IsConscribe == True,True);
        if generalList.Count > 0:
            indexradom = RandomUtils.GetRandom(0, generalList.Count);
            if indexradom < 0 or indexradom >= generalList.Count:
                actionResult.Result = False;
                return actionResult;
            urlParam.general = generalList[indexradom];
            general = urlParam.general;

            # 获取魂技
            urlParam.abilityInfo = ConfigCacheSet[AbilityInfo]().FindKey(general.AbilityID);

            # 获取加成
            shengMing = general.Mature.Find(lambda s:s.AbilityType == AbilityType.ShengMing);
            powerNum = general.Mature.Find(lambda s:s.AbilityType == AbilityType.PowerNum);
            soulNum = general.Mature.Find(lambda s:s.AbilityType == AbilityType.SoulNum);
            intelligenceNum = general.Mature.Find(lambda s:s.AbilityType == AbilityType.IntelligenceNum);
            potential = general.Mature.Find(lambda s:s.AbilityType == AbilityType.Potential);
            if shengMing and powerNum and soulNum and intelligenceNum and potential:
                actionResult.shengMing = shengMing.AbilityValue;
                actionResult.powerNum = powerNum.AbilityValue;
                actionResult.soulNum = soulNum.AbilityValue;
                actionResult.intelligenceNum = intelligenceNum.AbilityValue;
                actionResult.potential = potential.AbilityValue;

            urlParam.generalQuality = general.GeneralQuality;
            userGeneral = cacheSet.FindKey(userId, general.GeneralID);
            if userGeneral != None and userGeneral.GeneralStatus != GeneralStatus.YinCang:
                urlParam.currSoulID = general.SoulID;
                urlParam.gainNum = general.DemandNum;
                userGeneral = cacheSet.FindKey(userId, urlParam.currSoulID);
                if userGeneral != None:
                    userGeneral.AtmanNum = MathUtils.Addition(userGeneral.AtmanNum, urlParam.gainNum);
                    urlParam.generalType = GeneralType.Soul;
                else:
                    userGeneral = UserGeneral();
                    userGeneral.UserID = userId;
                    userGeneral.GeneralID = urlParam.currSoulID;
                    urlParam.generalType = GeneralType.Soul;
                    userGeneral.AtmanNum = MathUtils.Addition(userGeneral.AtmanNum, urlParam.gainNum);
                    cacheSet.Add(userGeneral);
                    UserAbilityHelper.AddUserAbility(general.AbilityID,  MathUtils.ToInt(userId), general.GeneralID,1);
                    # 添加集邮卡牌
                    UserAlbumHelper.AddUserAlbum(userId, AlbumType.General, urlParam.general.GeneralID);
            elif userGeneral == None:
                userGeneral = UserGeneral();
                userGeneral.UserID = userId;
                userGeneral.GeneralID = general.GeneralID;
                urlParam.generalType = GeneralType.YongBing;
                cacheSet.Add(userGeneral);
                UserAbilityHelper.AddUserAbility(urlParam.general.AbilityID, MathUtils.ToInt(userId), urlParam.general.GeneralID,1);
                # 添加集邮卡牌
            UserAlbumHelper.AddUserAlbum(userId, AlbumType.General, urlParam.general.GeneralID);
            userGeneral.GeneralName = urlParam.general.GeneralName
            userGeneral.HeadID = urlParam.general.HeadID
            userGeneral.PicturesID = urlParam.general.PicturesID
            userGeneral.GeneralLv = MathUtils.ToShort(urlParam.general.GeneralLv);
            userGeneral.LifeNum = urlParam.general.LifeNum;
            #UserAbilityHelper.AddUserAbility(general.AbilityID, MathUtils.ToInt(userId), general.GeneralID,1);
            userGeneral.GeneralType = urlParam.generalType;
            userGeneral.CareerID = general.CareerID;
            userGeneral.PowerNum = general.PowerNum;
            userGeneral.SoulNum = general.SoulNum;
            userGeneral.IntellectNum = general.IntellectNum;
            userGeneral.TrainingPower = 0;
            userGeneral.TrainingSoul = 0;
            userGeneral.TrainingIntellect = 0;
            userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
            userGeneral.AbilityID = general.AbilityID;
            userGeneral.Momentum = 0;
            userGeneral.Description = general.Description;
            userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
            userGeneral.CurrExperience = 0;
            userGeneral.Experience1 = 0;
            userGeneral.AbilityNum = 3;
            userGeneral.Experience2 = 0;
            userGeneral.RefreshMaxLife();
            userGeneral.LifeNum =userGeneral.LifeMaxNum;
            #玩家抽取到蓝色和紫色佣兵时，给予系统频道提示
            if urlParam.recruitType != RecruitType.SoulRecruit:
                if MathUtils.ToEnum[GeneralQuality](general.GeneralQuality) == GeneralQuality.Blue or MathUtils.ToEnum[GeneralQuality](general.GeneralQuality) == GeneralQuality.Purple:
                    if urlParam.recruitType==RecruitType.BaiLiTiaoYi and MathUtils.ToEnum[GeneralQuality](general.GeneralQuality) == GeneralQuality.Blue:
                        content=LanguageManager.GetLang().St_UserGetGeneralQuality1 %(contextUser.NickName,general.GeneralName)
                    elif urlParam.recruitType==RecruitType.Golden and MathUtils.ToEnum[GeneralQuality](general.GeneralQuality) == GeneralQuality.Blue:
                        content=LanguageManager.GetLang().St_UserGetGeneralQuality2 %(contextUser.NickName,general.GeneralName)
                    elif urlParam.recruitType==RecruitType.Golden and MathUtils.ToEnum[GeneralQuality](general.GeneralQuality) == GeneralQuality.Purple:
                        content=LanguageManager.GetLang().St_UserGetGeneralQuality3 %(contextUser.NickName,general.GeneralName)
                    TjxChatService().SystemSend(ChatType.World, content)
    if isFirst:
        GeneralHelper.FirstRecruitComplete(actionResult.user,urlParam.recruitType);
    rtype = MathUtils.ToEnum[RecruitType](urlParam.recruitType)
    priContent = FestivalHelper.RecruitGeneral(userId,rtype);   
    if priContent:
        parent.ErrorCode = 1;
        parent.ErrorInfo = priContent;
    return actionResult;

def buildPacket(writer, urlParam, actionResult):
    general = urlParam.general;
    writer.PushIntoStack(MathUtils.ToNotNullString(general.GeneralName) if general else '');
    writer.PushIntoStack(MathUtils.ToNotNullString(general.PicturesID) if general else '');
    writer.PushShortIntoStack(general.GeneralLv if general else 0);
    writer.PushShortIntoStack(MathUtils.ToShort(urlParam.generalType));
    writer.PushIntoStack(general.LifeNum if general else 0);
    writer.PushShortIntoStack(MathUtils.ToShort(general.PowerNum) if general else 0);
    writer.PushShortIntoStack(MathUtils.ToShort(general.SoulNum) if general else 0);
    writer.PushShortIntoStack(MathUtils.ToShort(general.IntellectNum) if general else 0);
    writer.PushIntoStack(urlParam.currSoulID);
    writer.PushIntoStack(urlParam.gainNum);
    writer.PushShortIntoStack(MathUtils.ToShort(urlParam.generalQuality));
    writer.PushShortIntoStack(0);  # 潜力点初始值为0

    # 下发魂技相关信息
    abilityInfo = urlParam.abilityInfo;
    writer.PushIntoStack(abilityInfo.AbilityID);
    writer.PushIntoStack(MathUtils.ToNotNullString(abilityInfo.AbilityName));
    writer.PushIntoStack(MathUtils.ToNotNullString(abilityInfo.HeadID));
    writer.PushIntoStack(MathUtils.ToNotNullString(abilityInfo.AbilityDesc));
    writer.PushShortIntoStack(MathUtils.ToShort(abilityInfo.AbilityQuality));

    # 成长值
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.shengMing));
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.powerNum));
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.soulNum));
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.intelligenceNum));
    writer.PushIntoStack(MathUtils.ToNotNullString(actionResult.potential));

    writer.PushShortIntoStack(MathUtils.ToShort(general.CareerID));
    return True;