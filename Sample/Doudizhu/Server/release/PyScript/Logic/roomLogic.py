import clr, sys
clr.AddReference('ZyGames.Framework')
clr.AddReference('ZyGames.Framework.Game')
clr.AddReference('ZyGames.Doudizhu.Model')
from ZyGames.Doudizhu.Model import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Cache import *

class RoomLogic():
    """斗地主房间逻辑"""
    def getRoomInfo():
        return []