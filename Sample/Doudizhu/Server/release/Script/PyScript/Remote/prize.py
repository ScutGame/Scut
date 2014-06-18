""""prize.py 玩家奖励"""
import ReferenceLib
from System import *
from ZyGames.Framework.Common import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Cache import *
from ZyGames.Framework.Game.Com.Rank import *
from ZyGames.Framework.Game.Service import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Doudizhu.Bll import *
from ZyGames.Doudizhu.Bll.Logic import *

def send(httpGet, head, writer):
    op = httpGet.GetIntValue("op")
    opUserID = httpGet.GetIntValue("OpUserID")
    userList = httpGet.GetStringValue("UserID").split(',')
    gold = httpGet.GetIntValue("Gold")
    gameCoin = httpGet.GetIntValue("GameCoin")
    goodItems = httpGet.GetStringValue("GoodItem")
    content = httpGet.GetStringValue("MailContent")
    userCacheSet = GameDataCacheSet[GameUser]()
    cacheSet = ShareCacheStruct[UserTakePrize]()
    for userId in userList:
        isTasked = False
        user = userCacheSet.FindKey(userId)
        if user:
            user.GiftGold = MathUtils.Addition(user.GiftGold, gold)
            user.GameCoin = MathUtils.Addition(user.GameCoin, gameCoin)
            isTasked = True
            gameHall = GameHall(user)
            itemList = goodItems.split(',')
            for item in itemList:
                itemArr = item.split('=')
                if len(itemArr) == 2:
                    itemId = int(itemArr[0])
                    itemNum = int(itemArr[1])
                    gameHall.PutPackage(itemId, itemNum)

            GameTable.Current.NotifyUserChange(user.UserId)

        userPrize = UserTakePrize(Guid.NewGuid().ToString())
        userPrize.UserID = int(userId)
        userPrize.GameCoin = gameCoin
        userPrize.Gold = gold
        userPrize.ItemPackage = goodItems
        userPrize.MailContent = content
        userPrize.IsTasked = isTasked
        userPrize.TaskDate = MathUtils.SqlMinDate
        userPrize.OpUserID = opUserID
        userPrize.CreateDate = DateTime.Now
        cacheSet.Add(userPrize)
        
    cacheSet.Update()


