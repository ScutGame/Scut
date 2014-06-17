"""CacheClear.py刷新缓存"""
import ReferenceLib
from action import *
from System import *
from mathUtils import MathUtils
from lang import Lang

from System.Collections.Generic import *
from ZyGames.Framework.SyncThreading import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Logic import *
from ZyGames.Doudizhu.Bll.Com.Chat import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Runtime import *
from ZyGames.Framework.Cache import *
from ZyGames.Framework.Common.Log import *

#class cache():
def refresh(cacheSet, personalId = ""):
    """刷新缓存,personalId可为空"""
    cacheSet.ReLoad(personalId)

def refreshConfig(httpGet, head, writer):
    """刷新Config"""
    refresh(ConfigCacheSet[AchievementInfo]())
    refresh(ConfigCacheSet[ChatInfo]())
    refresh(ConfigCacheSet[ConfigEnvSet]())
    refresh(ConfigCacheSet[DialInfo]())
    refresh(ConfigCacheSet[PokerInfo]())
    refresh(ConfigCacheSet[RoomInfo]())
    refresh(ConfigCacheSet[ShopInfo]())
    refresh(ConfigCacheSet[TaskInfo]())
    refresh(ConfigCacheSet[TitleInfo]())    

def refreshData(httpGet, head, writer):
    """刷新Data"""
    refresh(ShareCacheStruct[GameNotice]())
    refresh(ShareCacheStruct[UserTakePrize]())
    
def refreshUser(httpGet, head, writer):
    personalId = httpGet.GetStringValue("personalId")
    """刷新User"""    
    gameuser = GameDataCacheSet[GameUser]().FindKey(personalId)    
    if not gameuser:
        TraceLog.ReleaseWriteFatal("刷新玩家缓存失败") 
        return
    TraceLog.ReleaseWriteFatal("刷新玩家成功{0}",personalId)
    refresh(GameDataCacheSet[GameUser](), personalId)
    refresh(GameDataCacheSet[UserAchieve](), personalId)
    refresh(GameDataCacheSet[UserDailyRestrain](), personalId)
    refresh(GameDataCacheSet[UserItemPackage](), personalId)
    refresh(ShareCacheStruct[UserNickName](), personalId)
    refresh(GameDataCacheSet[UserTask](), personalId)
