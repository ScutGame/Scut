import clr, sys
import random
import time
import datetime
clr.AddReference('ZyGames.Framework.Common');
clr.AddReference('ZyGames.Framework');
clr.AddReference('ZyGames.Framework.Game');
clr.AddReference('ZyGames.Tianjiexing.Model');
clr.AddReference('ZyGames.Tianjiexing.BLL');
clr.AddReference('ZyGames.Tianjiexing.Lang');
clr.AddReference('ZyGames.Tianjiexing.BLL.Combat');
clr.AddReference('ZyGames.Tianjiexing.Component');

from mathUtils import MathUtils
from action import *
from System import *
from System.Collections.Generic import *
from ZyGames.Framework.Common.Log import *
from ZyGames.Tianjiexing.Model import *
from ZyGames.Tianjiexing.BLL import *
from ZyGames.Tianjiexing.BLL.Base import *
from ZyGames.Tianjiexing.Lang import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Tianjiexing.Model.Config import *
from ZyGames.Tianjiexing.BLL.Combat import *
from ZyGames.Tianjiexing.Model.Enum import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Tianjiexing.Component.Chat import *

class FestivalBll():

    def __init__(self):
        #self.__result = True
        pass   
    
    def UpdateArcheologyRestrain(self,user, npcinfoID):  

        """通关考古副本活动,更新通关次数与奖励"""
        def IsArcheologyPlot(self,plotID,npcInfoID):  
            """是否当前考古副本的npc"""    
            npcinfoList = ConfigCacheSet[PlotNPCInfo]().FindAll(match = lambda s:s.PlotID == plotID and s.NpcSeqNo>0);
            for npcInfo in npcinfoList:
                if npcInfoID == npcInfo.PlotNpcID:
                    return True;
            return False
        festivalInfosList = ShareCacheStruct[FestivalInfo]().FindAll(match = lambda m:m.FestivalType == FestivalType.Archeology);
        for info in festivalInfosList:
            if not NoviceHelper.IsFestivalOpen(info.FestivalID) or info.FestivalExtend == None or info.FestivalExtend.PlotID == 0:
                continue;
            if not IsArcheologyPlot(self,info.FestivalExtend.PlotID,npcinfoID):
                continue
            cacheSet = GameDataCacheSet[FestivalRestrain]();
            restrain = cacheSet.FindKey(user.UserID,info.FestivalID);
            if restrain != None and restrain.IsReceive:
                continue;
            if restrain == None :
                restrain = FestivalRestrain();
                restrain.FestivalID = info.FestivalID;
                restrain.UserID = user.UserID;
                restrain.RestrainNum = 0;
                restrain.RefreashDate = DateTime.Now;
                restrain.IsReceive = False;
                cacheSet.Add(restrain);
            userExtend = restrain.UserExtend.Find(lambda s:s.ID == npcinfoID);
            if userExtend:
                userExtend.Num = MathUtils.Addition(userExtend.Num, 1);
            else :
                userExtend = RestrainExtend();
                userExtend.ID = npcinfoID;
                userExtend.Num = 1;
                restrain.UserExtend.Add(userExtend);
            isGo = True;
            if restrain ==None or restrain.UserExtend.Count == 0 or info.FestivalExtend == None or info.FestivalExtend.PlotID == 0:
                isGo = False;
            npcinfoList = ConfigCacheSet[PlotNPCInfo]().FindAll(match = lambda s:s.PlotID == info.FestivalExtend.PlotID and s.NpcSeqNo>0);
            for npcInfo in npcinfoList:
                userExtend = restrain.UserExtend.Find(lambda s:s.ID == npcInfo.PlotNpcID)
                if userExtend and userExtend.Num >= info.RestrainNum:
                    continue;
                isGo = False;  
                break;         

            if isGo:
                restrain.IsReceive = True;
                content = PrizeHelper.GetPrizeUserTake(user, info.Reward.ToList());
                if content and info.FestivalExtend.Desc :
                    content = content.Replace(",","")
                    TjxChatService().SystemSendWhisper(user, info.FestivalExtend.Desc %content)   

                      
   
                