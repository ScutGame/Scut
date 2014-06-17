import ReferenceLib
import types

from ZyGames.Framework.Common.Log import *
from System.Collections.Generic import *
from ZyGames.Doudizhu.Model import *
from ZyGames.Framework.Cache.Generic import *
from ZyGames.Framework.Game.Cache import *

class CardVector():
    """牌储存结构,ex:2:[202,102],3:[203,103]"""
    def __init__(self, type):
        """@type:结构类型wang,bomb,shun,three,two,one"""
        self.__type = type
        self.__data = {}

    def addChild(self, vector):
        key = vector.__type
        if key:
            self.__data[key] = vector

    def len(self):
        return len(self.__data.keys())

    def add(self, key, cards):
        self.__data[key] = cards
        
    def hasKey(self, key):
        return self.__data.has_key(key)

    def get(self, key):
        return self.__data[key]

    def remove(self, key):
        del self.__data[key]

    def clear(self):
        return self.__data.clear()
    
    def getItems(self):
        return self.__data.items()

    def getKeys(self):
        keys = self.__data.keys()
        keys.sort()
        return keys

    def getFirstVal(self):
        keys = self.getKeys()
        if len(keys) > 0:
            return self.get(keys[0])
        return None

    def getLasttVal(self):
        keys = self.getKeys()
        if len(keys) > 0:
            return self.get(keys[len(keys)-1])
        return None

    def getGreaterThan(self, k):
        """取出大于k的牌->obj或[]"""
        keys = self.getKeys()
        for key in keys:
            if key > k:
                return self.get(key)
        return []
    
    def getMaxGreaterThan(self, k):
        """取出大于k的最大的牌->obj或[]"""
        keys = self.getKeys()
        for i in range(len(keys)-1, -1,-1):
            key = keys[i]
            if key > k:
                return self.get(key)
        return []

    def getList(self):
        list = []
        keys = self.getKeys()
        for key in keys:
            val = self.get(key)
            if type(val) == types.ListType:
                for t in val:
                    list.append(t)
            else:
                list.append(val)
        return list
    
    def getIndexVal(self, index):
        keys = self.getKeys()
        if len(keys) > index:
            return self.get(keys[index])
        return None

class AIConfig():
    @staticmethod
    def getConfig(name):
        return AIConfig.__config[name]
    
    __config = {
        "nickName": ['雪舞№枫红','夜舞＆倾城','魅影い冰绝','残恋々ら','匰身ァ饚З','走鐹菂蕗_','无语づ肴','传じ☆ve说','恋☆鵷:鶵','≮梦★羽≯','莞镁主题曲〃','o┢┦apΡy','幽魂邀絮】','So丶滚吧','無話可說。','花╮开一夏','▋潶禮菔▍','倾世恋流年','訫侑所属丶','命里缺沵','柔情似水似','淫大代表','钢茎混凝凸','∫安雅轩 *','查 无 此 人','上帝是个妞','_她入他心','___髅ㄦ〤','三分淑女范','﹋落花伊人','吃货不痴货','〃逢床作戏╮','屌丝先森 ぃ','资格让皒哭','╰︶情兽〤','得瑟的尛孩纸','好小伙中国造','情非得已り','夏兮兮°','等着你来宠','安小夕 ▽','堇色流年丿','如歌亦如梦','墨染锦年╮','独自沉沦 ∞','乱世°妖娆','夏沫＂Smile','七分笑三分真','沫尐诺⌒_⌒','___大苹果','德玛→西亚','Oo草丛伦oO','菊★花№信','_提莫必须死','嘉文四阿哥_','轮子┱妈','稻__草人','乌┣┰┝鸦','┣死歌┰┝','爱~。~射','炮-，-娘','oO小萝莉','狗头人Oo','大々‖嘴','大虫子','扇子Oo妈','冰鸟','船长','女剑','女枪','男枪','风女','卡萨丁','阿卡丽','卡特琳娜','伊泽瑞尔','戴安娜','安妮','凯特琳','贾克斯','卡萨丁','拉克丝','易大师','莫甘娜','奈德丽','索拉卡','提莫','潘森','泰达米尔','佛拉基米尔','崔斯塔娜','(=@__@=)哪里','丽桑桌','奎因','扎克','维嘉','墨菲特','索菲娅','阿狸','影流之主'],
        "head": ['head_1001','head_1002','head_1003','head_1004','head_1005','head_1006','head_1007','head_1008','head_1009','head_1010','head_1011','head_1012','head_1013','head_1014','head_1015','head_1016','head_1017','head_1018','head_1019','head_1020']
    }

class CardAILogic():
    """斗地主AI逻辑"""
    def __init__(self, roomId, tableId, positionId):
        self.__table = None
        self.__pos = None
        self.__nextPos = None
        self.__prePos = None
        self.__playerNum = 3
        self.__Landlord = None
        self.__CK = 13
        self.__CA = 14
        self.__C2 = 15#2子
        self.__CW1 = 18
        self.__CW2 = 19
        self.__role = 0#处于地主的位置
        self.__largestCardSize = self.__CW2

        roomStruct = MemoryCacheStruct[RoomData]()
        key = str(roomId)
        roomData = None
        result = roomStruct.TryGet(key)
        if result[0]:
            roomData = result[1]
            resultTable = roomData.Tables.TryGetValue(tableId)
            if resultTable[0]:
                self.__table = resultTable[1]
                self.__playerNum = self.__table.PlayerNum

        if self.__table and positionId < self.__table.Positions.Length:
            tempCard = 0
            for pos in self.__table.Positions:
                if pos.IsLandlord:
                    self.__Landlord = pos
                if pos.Id == positionId:
                    self.__pos = pos
                #计算最大牌
                if pos.CardData and pos.CardData.Count > 0:
                   lastCardVal = pos.CardData[pos.CardData.Count-1] % 100
                   if lastCardVal > tempCard:
                       tempCard = lastCardVal
            if tempCard > 0:
                self.__largestCardSize = tempCard

        if self.__Landlord and self.__pos:
            index = (self.__pos.Id + 1) % self.__playerNum 
            self.__nextPos  = self.__table.Positions[index]
            preindex = (self.__pos.Id + self.__playerNum - 1) % self.__playerNum 
            self.__prePos = self.__table.Positions[preindex]
            #计算role处在0:地主,-1:上家,1:下家位置
            if self.__Landlord.Id == self.__pos.Id:
                self.__role = 0
            elif self.__Landlord.Id == self.__nextPos.Id:
                self.__role = -1
            else:
                self.__role = 1

    def writeLog(self, msg):
        #todo test
        TraceLog.WriteComplement(msg)

    def getCardSize(self, card):
        """获取牌大小->int"""
        return card % 100

    def getOutCardData(self):
        """出牌对象->CardData"""
        return self.__table.PreCardData
            
    def getOutCardResult(self):
        """获取已出牌记录->List<CardData>"""
        return self.__table.OutCardList

    def getUserCard(self):
        """获取玩家手上的牌->List<int>"""
        return self.__pos.CardData

    def getUserRole(self):
        """玩家角色，0:地主,-1:上家,1:下家位置"""
        return self.__role
    
    def getNextPosCardCount(self):
        """获取玩家下家牌数->int"""
        return self.__nextPos.CardData.Count
    
    def getPrePosCardCount(self):
        """获取玩家上家牌数->int"""
        return self.__prePos.CardData.Count

    def checkCall(self):
        """检查能否叫地主->bool"""
        #火箭为8分，炸弹为6分，大王4分，小王3分，一个2为2分
        bigCardValue = 0#大牌权值
        cards = self.getUserCard()
        times = self.__table.MultipleNum / 2
        bomCards = []
        bombvct = CardVector('bomb')
        self.getBombCard(cards, bombvct)
        bomCards.append(bombvct.getList())
        wangvct = CardVector('wang')
        self.getWangBombCard(cards, wangvct)
        bomCards.append(wangvct.getList())
        c2 = CardVector('c2')
        self.getCard(cards, self.__C2, c2)
        c2Count = len(c2.getList())

        for bom in bomCards:
            if len(bom) == 4:
                bigCardValue = bigCardValue + 6
            elif len(bom) == 2:
                bigCardValue = bigCardValue + 8
        if len(bomCards) == 0:
            if wangvct.hasKey(self.__CW2):
                bigCardValue = bigCardValue + 4
            if wangvct.hasKey(self.__CW1):
                bigCardValue = bigCardValue + 3
        if c2Count < 4:
            bigCardValue = bigCardValue +(c2Count * 2)
        if(bigCardValue >= 7 and times < 3)\
           or (bigCardValue >= 5 and times < 2)\
           or (bigCardValue >= 2 and times < 1):
            return True
        return False

    def searchOutCard(self):
        """出牌搜索,牌组0:为空,1:单牌,2:对牌,3:王炸,4:三张,5:三带一,6:三带二,7:炸弹,8:顺子,9:四带二,10:连对,11:飞机,12:飞机带二,13:二连飞机带二对"""
        resultCards = []
        handCard = self.getUserCard()
        if not handCard or len(handCard) == 0:
            return resultCards
        myCardCount = len(handCard)
        handleTimes = self.getCardHandleTimes(handCard)
        preOutCard = self.getOutCardData()#当前已的出牌
        outCardResult = self.getOutCardResult()
        role = self.getUserRole()
        nextPosCardCount = self.getNextPosCardCount()
        prePosCardCount = self.getPrePosCardCount()
        nextIsLand = self.__nextPos.IsLandlord
        landlord = self.__Landlord
        landlordCardCount = self.__prePos.CardData.Count

        if not preOutCard or preOutCard.PosId == self.__pos.Id:
            #任意出牌
            outVct = CardVector('')
            resultCards = self.freeOutCard(handCard)
            cardCount = len(resultCards)
            if (role==0 and (nextPosCardCount == 1 or prePosCardCount ==1) and cardCount==1)\
                or (role!=0 and nextIsLand and nextPosCardCount == 1 and cardCount==1)\
                or (role!=0 and landlord.CardData.Count == 1 and cardCount==1):
                #不能出比最大牌小的单牌
                resultCards = []
                self.getTwoCard(handCard, outVct)
                if outVct.len() > 0:
                    self.copyList(outVct.getFirstVal(), resultCards)
                else:
                    #最大单牌
                    cards = self.getLargeSingleCard(handCard, self.__CA)
                    if len(cards) > 0:
                        self.copyList(cards, resultCards)
                    else:
                        self.copyList(handCard[len(handCard)-1], resultCards)

            elif (role==0 and (nextPosCardCount == 2 or prePosCardCount ==2) and cardCount==2 and myCardCount > 2)\
                or (role!=0 and nextIsLand and nextPosCardCount == 2 and cardCount==2 and myCardCount > 2)\
                or (role!=0 and landlord.CardData.Count == 2 and cardCount==2 and myCardCount > 2):
                #不能出小于最大牌的对子
                cardval = self.getCardSize(resultCards[0])
                if cardval < self.__CA and cardval < self.__largestCardSize:
                    resultCards = []
                    bombvct =  CardVector('')
                    lastCards = self.getBombCard(handCard, bombvct)
                    if myCardCount < 6 and bombvct.len() > 0:
                        self.copyList(bombvct.getFirstVal(), resultCards)
                    self.getSingleCard(lastCards, outVct)
                    if outVct.len() > 0:
                        self.copyList(outVct.getFirstVal(), resultCards)
                    else:
                        resultCards.append(handCard[0])
            elif role!=0 and nextIsLand==False and handleTimes > 3 and nextPosCardCount == 1:
                #自己手上大于6张，且同家剩1张时出单牌
                resultCards = []
                resultCards.append(handCard[0])
            elif role!=0 and nextIsLand==False and handleTimes > 3 and nextPosCardCount == 2 and self.__pos.OutTwoTimes == 0:
                #自己手上大于6张，且同家剩2张配合对家出对次数为0时出对子
                self.getTwoCard(handCard, outVct)
                if outVct.len() > 0:
                    resultCards = []
                    self.__pos.OutTwoTimes = self.__pos.OutTwoTimes + 1
                    self.copyList(outVct.getFirstVal(), resultCards)
            else:
                pass
            #self.writeLog('%s:%s' % ('任意出牌' ,resultCards))

        elif preOutCard.Type == 3:
            #王炸
            return resultCards
        else:
            outcardType = preOutCard.Type
            islandlordOut = preOutCard.PosId == landlord.Id
            if role == 0:
                #地主压牌
                if handleTimes < 3 or (nextPosCardCount == 2 and outcardType == DeckType.Double)\
                    or (nextPosCardCount == 1 and outcardType == DeckType.Single):
                    #压牌
                    resultCards = self.enforceOutCard(preOutCard, handCard)
                elif nextPosCardCount < 7 or prePosCardCount < 7:
                    #压牌不打炸
                    resultCards = self.enforceOutCard(preOutCard, handCard, True)
                else:
                    #跟牌
                    resultCards = self.followOutCard(preOutCard, handCard)
            elif role < 0:
                #地主上家出牌
                #a)当自己只差2手时压牌，或者地主小于3张且出对时压牌；或者地主小于6张并且打单或对且对家不是最大牌时压牌
                #b)农民同家出牌大于等于A时不跟
                if handleTimes < 3 or\
                    (landlordCardCount <= 2 and outcardType == DeckType.Double and (islandlordOut or preOutCard.CardSize < self.__largestCardSize)) or\
                    (landlordCardCount == 1 and outcardType == DeckType.Single and (islandlordOut or preOutCard.CardSize < self.__largestCardSize)) or\
                    landlordCardCount <= 2:
                    #压牌
                    resultCards = self.enforceOutCard(preOutCard, handCard)
                elif (landlordCardCount < 7 and islandlordOut and (outcardType == DeckType.Double or outcardType == DeckType.Single) ):
                    #压牌不打炸
                    resultCards = self.enforceOutCard(preOutCard, handCard, True)
                else:
                    if (not islandlordOut) and (preOutCard.CardSize >= self.__CA \
                        or (outcardType >= DeckType.Double and preOutCard.CardSize >= self.__CK)\
                        or outcardType == DeckType.Single and preOutCard.CardSize > self.__C2):
                        #不跟
                        return resultCards
                    #跟牌
                    resultCards = self.followOutCard(preOutCard, handCard)
            else:
                #地主下家出牌，当地主小于3张
                if handleTimes < 3 or (islandlordOut and landlordCardCount <= 2):
                    #压牌
                    resultCards = self.enforceOutCard(preOutCard, handCard)
                elif (landlordCardCount <= 2 and outcardType == DeckType.Double and (islandlordOut or preOutCard.CardSize < self.__largestCardSize)) or\
                    (landlordCardCount == 1 and outcardType == DeckType.Single and (islandlordOut or preOutCard.CardSize < self.__largestCardSize)):
                    #压牌不打炸
                    resultCards = self.enforceOutCard(preOutCard, handCard, True)
                else:
                    if (not islandlordOut) and (preOutCard.CardSize >= self.__CA \
                        or (outcardType >= DeckType.Double and preOutCard.CardSize >= self.__CK)\
                        or outcardType == DeckType.Single and preOutCard.CardSize > self.__C2):
                        #不跟
                        return resultCards
                    #跟牌
                    resultCards = self.followOutCard(preOutCard, handCard)

        return resultCards
    
    def copyList(self, list, clist, index=0, count=0):
        """@list: copy源,\n@clist: copy目标\@index:开始位置\n@count:数量"""
        if type(list) == types.ListType:
            i = 0
            for val in list:
                if count > 0 and len(clist) == count:
                    break
                if i >= index:
                    clist.append(val)
                i = i+1
        elif list:
            clist.append(list)

    def getLargeSingleCard(self, handCard, minval):
        """单牌倒打->[]"""
        result = []
        #先打2
        verctor = CardVector('')
        handTime = self.getCardHandleTimes(handCard)
        self.getCard(handCard, self.__C2, verctor)
        if handTime > 3 and verctor.len() > 0 and minval < self.__C2:
            result.append(verctor.getFirstVal())
        else:
            verctor = CardVector('')
            wangvct =  CardVector('')
            bombvct =  CardVector('')
            #排除炸
            lastCards = self.getBombCard(handCard, bombvct)
            lastCards = self.getWangBombCard(lastCards, wangvct)
            self.convertVerctor(lastCards, verctor)
            if verctor.len() > 0:
                tempCard = verctor.getMaxGreaterThan(minval)
                if tempCard:
                    self.copyList(tempCard, result)
                    return result
            #self.copyList(bombvct.getFirstVal(), result)
            #if len(result) == 0:
            #    self.copyList(wangvct.getList(), result)
        return result
    
    def freeOutCard(self, handCard):
        """自由出牌,先飞机,连对,顺子,三张,对子,单牌->[]"""
        vector = self.splitCard(handCard)
        val = 0
        resultCards = []
        #判断剩余2手牌时是否是有炸或王炸
        handTimes = self.getVectorHandleTimes(vector)
        if handTimes < 3:
            #王炸
            resultCards = self.outWangBombCard(handCard)
            if len(resultCards) == 0:
                #取炸
                resultCards = self.outBombCard(handCard)
            if len(resultCards) == 0:
                #先出最大的对或单
                vectorMax = CardVector('Max')
                self.getCard(handCard, self.__largestCardSize, vectorMax)
                if vectorMax.len() > 0:
                    if vector.get('two').hasKey(self.__largestCardSize):
                        self.copyList(vector.get('two').get(self.__largestCardSize), resultCards)
                    if len(resultCards) == 0 and vector.get('one').hasKey(self.__largestCardSize):
                        self.copyList(vector.get('one').get(self.__largestCardSize), resultCards)
            if len(resultCards) > 0:
                return resultCards

        resultCards = self.processOutCard(vector, DeckType.FlyAndTwo, val, 8)
        if len(resultCards) > 0:
            return resultCards
        resultCards = self.processOutCard(vector, DeckType.FlyAndTwoDouble, val, 10)
        if len(resultCards) > 0:
            return resultCards
        resultCards = self.processOutCard(vector, DeckType.Fly, val, 6)
        if len(resultCards) > 0:
            return resultCards
        resultCards = self.processOutCard(vector, DeckType.Liandui, val, 6)
        if len(resultCards) > 0:
            return resultCards
        resultCards = self.processOutCard(vector, DeckType.Shunzi, val, 5)
        if len(resultCards) > 0:
            return resultCards
        #三张是A以上的牌时，先出其它类型牌
        c2Cards = []
        resultCards = self.processOutCard(vector, DeckType.ThreeAndOne, val, 3)
        if len(resultCards) > 0:
            if handTimes > 2 and self.getCardSize(resultCards[0]) >= self.__CA:
               c2Cards = resultCards
               resultCards = []
            else:
               return resultCards
        if len(c2Cards) == 0:
            resultCards = self.processOutCard(vector, DeckType.ThreeAndTwo, val, 3)
            if len(resultCards) > 0:
                return resultCards
            resultCards = self.processOutCard(vector, DeckType.Three, val, 3)
            if len(resultCards) > 0:
                return resultCards
            resultCards = []
        #只差一对
        if len(handCard) == 2:
            resultCards = self.processOutCard(vector, DeckType.Double, val, 2)
            if len(resultCards) > 0:
                return resultCards
        #是否有四带2
        bombCards = self.processOutCard(vector, DeckType.Bomb, val, 4)
        if handTimes < 5 and len(bombCards) > 0:
            resultCards = []
            self.copyList(bombCards, resultCards)
            oneCount = self.getSingleCount(vector)
            twoCount = self.getTwoCount(vector)
            if oneCount > 1:
                self.copyList(vector.get('one').getIndexVal(0), resultCards)
                self.copyList(vector.get('one').getIndexVal(1), resultCards)
                return resultCards
            elif twoCount == 1 and oneCount ==1:
                onetemp = vector.get('one').getFirstVal()
                twotemp = vector.get('two').getFirstVal()
                if self.getCardSize(onetemp) > self.getCardSize(twotemp[0]):
                    self.copyList(twotemp, resultCards)
                    return resultCards
                else:
                    self.copyList(onetemp, resultCards)
                    self.copyList(twotemp[0], resultCards)
                    return resultCards

            elif twoCount > 0:
                self.copyList(vector.get('two').getList(), resultCards)
                return resultCards
            else:
                resultCards = []

        #是否出对或单
        singleCards = self.processOutCard(vector, DeckType.Single, val, 1)
        twoCards = self.processOutCard(vector, DeckType.Double, val, 2)
        if len(twoCards) > 0 and len(singleCards) > 0\
           and self.getCardSize(singleCards[0]) > self.getCardSize(twoCards[0]):
            return twoCards
        elif len(singleCards) > 0:
            return singleCards
        elif len(twoCards) > 0:
            return twoCards

        resultCards = self.processOutCard(vector, DeckType.Bomb, val, 4)
        if len(resultCards) > 0:
            return resultCards

        if len(c2Cards) == 0:
            resultCards = c2Cards
        return resultCards

    def followOutCard(self, preOutCard, handCard):
        """跟对方的出牌，不出对2或3张2->[]"""
        type = preOutCard.Type
        val = preOutCard.CardSize
        count = preOutCard.Cards.Length
        vector = self.splitCard(handCard)
        resultCards = self.processOutCard(vector, type, val, count, 0)
        if len(resultCards) > 1:
            if type != DeckType.Single\
                and self.getCardSize(resultCards[0]) == self.__C2:
                resultCards = []#不出
        return resultCards
    
    def processOutCard(self, vector, type, minval, mincount, maxcount=20):
        """处理牌组出牌规则->[]\n@vector:vector对象集合,@type:出牌类型,\n@minval:,\n@mincount:,\n@maxcount:为0时压牌，否则任意出"""
        resultCards = []
        cards = []
        if type == DeckType.Single:
            cards = self.getMoreThanCardVal(vector.get('one'), minval, mincount)
            if len(cards) > 0:
                resultCards.append(cards[0])
        elif type == DeckType.Double:
            cards = self.getMoreThanCardVal(vector.get('two'), minval, mincount/2)
            if len(cards) > 0:
                self.copyList(cards, resultCards)
        elif type == DeckType.WangBomb:
            return resultCards
        elif type == DeckType.Three:
            cards = self.getMoreThanCardVal(vector.get('three'), minval, mincount/3)
            if len(cards) > 0:
                self.copyList(cards, resultCards)
        elif type == DeckType.ThreeAndOne:
            cards = self.getMoreThanCardVal(vector.get('three'), minval, mincount/3)
            onevct = vector.get('one')
            if len(cards) > 0 and onevct.len() > 0:
                self.copyList(cards, resultCards)
                resultCards.append(onevct.getFirstVal())
        elif type == DeckType.ThreeAndTwo:
            cards = self.getMoreThanCardVal(vector.get('three'), minval, mincount/3)
            twovct = vector.get('two')
            if len(cards) > 0 and twovct.len() > 0:
                self.copyList(cards, resultCards)
                self.copyList(twovct.getFirstVal(), resultCards)
        elif type == DeckType.Bomb:
            cards = self.getMoreThanCardVal(vector.get('bomb'), minval, mincount/4)
            if len(cards) > 0:
                self.copyList(cards, resultCards)
        elif type == DeckType.Shunzi:
            shunvct = vector.get('shun')
            keys = shunvct.getKeys()
            for key in keys:
                cards = shunvct.get(key)
                resultCards = self.getMoreThanShunCard(cards, minval, mincount, maxcount)
                if len(resultCards) > 0:
                    break
        elif type == DeckType.FourAndTwo:
            cards = self.getMoreThanCardVal(vector.get('bomb'), minval, mincount/4)
            onevct = vector.get('one')
            twovct = vector.get('two')
            if len(cards) > 0 and (onevct.len() > 1 or twovct.len() > 0):
                self.copyList(cards, resultCards)
                if onevct.len() > 1:
                    self.copyList(onevct.getIndexVal(0), resultCards)
                    self.copyList(onevct.getIndexVal(1), resultCards)
                else:
                    for i in range(0,2):
                        self.copyList(twovct.getIndexVal(i), resultCards)
        elif type == DeckType.Liandui:
            twovct = vector.get('two')
            resultCards = self.getMoreThanCardVal(twovct, minval, mincount/2, maxcount)
        elif type == DeckType.Fly\
            or type == DeckType.FlyAndTwo\
            or type == DeckType.FlyAndTwoDouble:
            #处理飞机
            threevct = vector.get('three')
            if threevct.len() > 1:
                mcount = 0
                attrVector = None
                if type == DeckType.FlyAndTwo:
                    attrVector = vector.get('one')
                    mcount = mincount / (3+1)
                elif type == DeckType.FlyAndTwoDouble:
                    attrVector = vector.get('two')
                    mcount = mincount / (3+2)
                else:
                    mcount = mincount / 3

                if mcount > 1:
                    threeCards = self.getMoreThanCardVal(threevct, minval, mcount, maxcount)
                    if len(threeCards) > 0 and attrVector and attrVector.len() >= mcount:
                        attrcount = 0
                        tempArr = []
                        attrkeys = attrVector.getKeys()
                        for key in attrkeys:
                            if attrcount == mcount:
                                break
                            self.copyList(attrVector.get(key),tempArr)
                            attrcount = attrcount + 1
                        if (maxcount==0 and attrcount == mcount) or ((maxcount>0 and attrcount >= mcount)):
                            self.copyList(threeCards, resultCards)
                            self.copyList(tempArr, resultCards)
                    else:
                        pass
        return resultCards
    
    def enforceOutCard(self, preOutCard, handCard, ignoreBomb=False):
        """压对方的牌->list\n@ignoreBomb:是否排除炸"""
        resultCards = []
        type = preOutCard.Type
        minval = preOutCard.CardSize
        cardCount = preOutCard.Cards.Length
        return self.processEnforceOutCard(handCard, type, minval, cardCount, ignoreBomb)

    def processEnforceOutCard(self, handCard, type, minval, cardCount, ignoreBomb):
        """处理压对方的牌->list"""
        resultCards = []
        vector = CardVector('')

        #判断剩余牌是否是炸或王炸
        handTimes = self.getCardHandleTimes(handCard)
        if handTimes < 3:
            #取炸
            resultCards = self.outBombCard(handCard)
        if len(resultCards) == 0:
            #王炸
            resultCards = self.outWangBombCard(handCard)
        if len(resultCards) > 0:
            return resultCards

        if type == DeckType.Single:
            #最大单牌
            self.copyList(self.getLargeSingleCard(handCard, minval),resultCards)
        elif type == DeckType.Double:
            self.getTwoCard(handCard, vector)
            if vector.len()>0:
                self.copyList(vector.getGreaterThan(minval),resultCards)
            else:
                self.getTwoCard(handCard, vector, True)
                if vector.len()>0:
                    self.copyList(vector.getGreaterThan(minval),resultCards)
        elif type == DeckType.WangBomb:
            return resultCards
        elif type == DeckType.Three:
            self.getThreeCard(handCard, vector)
            if vector.len()>0:
                self.copyList(vector.getGreaterThan(minval),resultCards)
        elif type == DeckType.ThreeAndOne:
            lessCards = self.getThreeCard(handCard, vector)
            if vector.len()>0:
                vct = CardVector('')
                self.getSingleCard(lessCards, vct)
                threearr = vector.getGreaterThan(minval)
                if vct.len() > 0 and threearr and len(threearr) > 0:
                    self.copyList(threearr, resultCards)
                    resultCards.append(vct.getFirstVal())
        elif type == DeckType.ThreeAndTwo:
            lessCards = self.getThreeCard(handCard, vector)
            if vector.len()>0:
                vct = CardVector('')
                self.getTwoCard(lessCards, vct)
                threearr = vector.getGreaterThan(minval)
                if vct.len() > 0 and threearr and len(threearr) > 0:
                    self.copyList(threearr,resultCards)
                    self.copyList(vct.getFirstVal(),resultCards)
        elif type == DeckType.Bomb:
            pass
        elif type == DeckType.Shunzi:
            lessCards = self.getShunCard(handCard, vector)
            if vector.len()>0:
               resultCards = self.getMoreThanShunCard(vector.getFirstVal(), minval, cardCount)
        elif type == DeckType.FourAndTwo:
            lessCards = self.getBombCard(handCard, vector)
            tempcard = vector.getGreaterThan(minval)
            if tempcard:
                onevct = CardVector('')
                twovct = CardVector('')
                lessCards = self.getSingleCard(lessCards, onevct)
                lessCards = self.getTwoCard(lessCards, twovct)
                if (onevct.len() > 1 or twovct.len() > 0):
                    self.copyList(tempcard, resultCards)
                    if onevct.len() > 1:
                        self.copyList(onevct.getIndexVal(0), resultCards)
                        self.copyList(onevct.getIndexVal(1), resultCards)
                    else:
                        for i in range(0,2):
                            self.copyList(twovct.getIndexVal(i), resultCards)
        elif type == DeckType.Liandui:
            self.getTwoCard(handCard, vector)
            cnum = cardCount / 2
            if vector.len() >= cnum:
               resultCards = self.getMoreThanCardVal(vector, minval, cnum)
            pass
        elif type == DeckType.Fly\
            or type == DeckType.FlyAndTwo\
            or type == DeckType.FlyAndTwoDouble:
            #处理飞机
            lessCards = self.getThreeCard(handCard, vector)
            threevct = vector
            if threevct.len() > 1:
                mcount = 0
                attrVector = None
                if type == DeckType.FlyAndTwo:
                    attrVector = CardVector('')
                    self.getSingleCard(lessCards, attrVector)
                    mcount = mincount / (3+1)
                elif type == DeckType.FlyAndTwoDouble:
                    attrVector = CardVector('')
                    self.getTwoCard(lessCards, attrVector)
                    mcount = mincount / (3+2)
                else:
                    mcount = mincount / 3

                if mcount > 1:
                    threeCards = self.getMoreThanCardVal(threevct, minval, mcount, maxcount)
                    if len(threeCards) > 0 and attrVector and attrVector.len() >= mcount:
                        attrcount = 0
                        tempArr = []
                        attrkeys = attrVector.getKeys()
                        for key in attrkeys:
                            if attrcount == mcount:
                                break
                            self.copyList(attrVector.get(key),tempArr)
                            attrcount = attrcount + 1
                        if (maxcount==0 and attrcount == mcount) or ((maxcount>0 and attrcount >= mcount)):
                            self.copyList(threeCards, resultCards)
                            self.copyList(tempArr, resultCards)
                    else:
                        pass

        if ignoreBomb:
            if len(resultCards) == 0:
                #取炸
                resultCards = self.outBombCard(handCard)
            if len(resultCards) == 0:
                #王炸
                resultCards = self.outWangBombCard(handCard)
        return resultCards
        
    def outBombCard(self, handCard):
        bombvct = CardVector('bomb')
        self.getBombCard(handCard, bombvct)
        if bombvct.len() > 0:
            return bombvct.getFirstVal()
        return []

    def outWangBombCard(self, handCard):
        wangvct = CardVector('wang')
        self.getWangBombCard(handCard, wangvct)
        if wangvct.len() == 2:
            return wangvct.getList()
        return []

    #以下是拆牌算法
    def splitCard(self, handCard):
        """拆牌->CardVector"""
        vector = CardVector('all')
        vector.addChild(CardVector('wang'))
        vector.addChild(CardVector('bomb'))
        vector.addChild(CardVector('shun'))
        vector.addChild(CardVector('three'))
        vector.addChild(CardVector('two'))
        vector.addChild(CardVector('one'))
        lastCards = []
        if len(handCard) == 2:
            if handCard[0] == self.__CW1 and handCard[1] == self.__CW2:
                vector.get('wang').add(val, handCard[0])
                vector.get('wang').add(val, handCard[1])

        if len(handCard) > 1:
            lastCards = self.getUnrelatedCard(handCard, vector)
            #剩余关联牌
            lastCards = self.getBombCard(lastCards, vector.get('bomb'))
            #检查飞机
            lastCards = self.getFlyCard(lastCards, vector.get('three'))
            lastCards = self.getShunTwoCard(lastCards, vector.get('two'))
            lastCards = self.getShunCard(lastCards, vector.get('shun'))
            lastCards = self.getThreeCard(lastCards, vector.get('three'))
            lastCards = self.getTwoCard(lastCards, vector.get('two'))
            lastCards = self.checkSingleAsShun(lastCards, vector.get('shun'))
        else:
            lastCards = handCard
        lastCards = self.getSingleCard(lastCards, vector.get('one'))
        return vector
    
    def addVector(self, vector, val, list):
        """增加到vector集合以wang,bomb,three,two,one分类->void"""
        l = len(list)
        if l > 0 and val >= self.__CW1:
            vector.get('wang').add(val, list[0])
        elif l == 4:
            vector.get('bomb').add(val, list)
        elif l == 3:
            vector.get('three').add(val, list)
        elif l == 2:
            vector.get('two').add(val, list)
        elif l == 1:
            vector.get('one').add(val, list[0])
    
    def getUnrelatedCard(self, handCard, vector):
        """取出无关联的牌,返回剩余关联牌\n@vector:是wang,bomb,shun等的集合->[]"""
        arr = []
        lastCards = []
        eqVal = 0
        count = len(handCard)
        if count < 2:
            return handCard

        for i in range(0, count):
            card = handCard[i]
            val = self.getCardSize(card)
            if i == 0:
                arr.append(card)
                continue
            precard = handCard[i-1]
            preval = self.getCardSize(precard)

            if val == preval:
                arr.append(card)
            else:
                #之间是否有联系
                if preval >= self.__C2 or ((eqVal==0 or eqVal != preval-1) and (val==self.__C2 or preval < val-1)):
                    self.addVector(vector, preval, arr)
                else:
                    self.copyList(arr, lastCards)

                arr=[]
                eqVal = preval
                arr.append(card)
            #结尾
            if i == count-1:
                if val >= self.__C2 or eqVal < val-1:
                    if (val == self.__CW1 or (preval < self.__CW1 and val == self.__CW2)):
                        #单张大小王
                        vector.get('one').add(val, card)
                    else:
                        self.addVector(vector, val, arr)
                else:
                    self.copyList(arr, lastCards)
        return lastCards
    


    #以下是取牌组逻辑
    def getShunCard(self, handCard, vector):
        """取出顺子，返回剩余牌->[]"""
        lastCards = []
        count = len(handCard)
        if count < 2:
            return handCard
        arr = []
        arrignore = []
        arrlog = []
        for i in range(0, count):
            card = handCard[i]
            if i == 0:
                arr.append(card)
                arrlog.append(card)
                continue
            precard = handCard[i-1]
            preval = self.getCardSize(precard)
            val = self.getCardSize(card)
            if preval == val-1 and val < self.__C2:
                arr.append(card)
                arrlog.append(card)
            elif preval == val:
                arrignore.append(card)
                arrlog.append(card)
            else:
                if len(arr) >= 5:
                    vector.add(self.getCardSize(arr[0]), arr)
                    self.copyList(arrignore, lastCards)
                else:
                    self.copyList(arrlog, lastCards)
                arr = []
                arr.append(card)
                arrlog = []
                arrlog.append(card)
                arrignore = []

            if i == count-1:
                if len(arr) >= 5:
                    vector.add(self.getCardSize(arr[0]), arr)
                    self.copyList(arrignore, lastCards)
                    arrignore = []
                else:
                    self.copyList(arrlog, lastCards)
        return lastCards
   
    def getMoreThanShunCard(self, cards, minval, mincount, maxcount=0):
        """从顺子cards对象中获取顺子大于minval的牌->[]\n@cards:手上的牌,\n@minval:最小牌面,\n@mincount:顺子个数,\n@maxcount:0时固定匹配"""
        result = []
        length = len(result)
        for card in cards:
            val = self.getCardSize(card)
            if val > minval and val < self.__C2\
               and ((maxcount==0 and length < mincount)\
                or(maxcount > 0 and length < maxcount-1)):
                result.append(card)
        if (maxcount == 0 and len(result) == mincount)\
            or(maxcount > 0 and len(result) >= mincount) :
            return result
        return []
    
    def getWangBombCard(self, handCard, vector):
        """取出王炸牌至vector对象，返回剩余牌->[]"""
        arr = []
        temp = []
        count = len(handCard)
        for i in range(0, count):
            card = handCard[i]
            val = self.getCardSize(card)
            if val == self.__CW1 or val == self.__CW2:
                temp.append(card)
            else:
                arr.append(card)
        if len(temp) == 2:
            for card in temp:
                vector.add(self.getCardSize(card), card)
        else:
            self.copyList(temp, arr)
        return arr
    
    def getBombCard(self, handCard, vector):
        """取出炸至vector对象，返回剩余牌->[]"""
        return self.getSameCard(handCard, 4, vector)
    
    def getFlyCard(self, handCard, vector):
        """取出飞机至vector对象，返回剩余牌->[]"""
        if len(handCard) > 5:
            threevct = CardVector('')
            self.getThreeCard(handCard, threevct)
            if threevct.len() > 1:
                arr = self.getMoreThanCardVal(threevct, 0, 2, 20)
                self.getSameCard(arr, 3, vector)
                for card in arr:
                    handCard.remove(card)
        return handCard;

    def getShunTwoCard(self, handCard, vector):
        """取出大于3连对至vector对象，返回剩余牌->[]"""
        if len(handCard) > 6:
            twovct = CardVector('')
            self.getTwoCard(handCard, twovct)
            if twovct.len() > 3:
                arr = self.getMoreThanCardVal(twovct, 0, 4, 20)
                self.getSameCard(arr, 2, vector)
                for card in arr:
                    handCard.remove(card)
        return handCard;

    def getThreeCard(self, handCard, vector):
        """取出3条至vector对象，返回剩余牌->[]"""
        return self.getSameCard(handCard, 3, vector)
    
    def getTwoCard(self, handCard, vector, matchThree=False):
        """取出对子至vector对象，返回剩余牌->[]\n@matchThree:是否匹配三张"""
        lastCards = self.getSameCard(handCard, 2, vector)
        if matchThree:
            threevct = CardVector('')
            lastCards = self.getSameCard(handCard, 3, threevct)
            if threevct.len() > 0:
                keys = threevct.getKeys()
                for key in keys:
                    val = threevct.get(key)
                    arr = []
                    self.copyList(val, arr, 0, 2)
                    vector.add(key, arr)
        return lastCards
    
    def getMoreThanCardVal(self, vector, minval, mincount, maxcount=0):
        """从vector对象取出大于minval的连续牌列表->[]\n@vector:vector对象,\n@minval:牌面值,\n@mincount:匹配个数,\n@maxcount:0时固定匹配"""
        arr = []
        resultCards = []
        keys = vector.getKeys()
        for i in range(0, len(keys)):
            key = keys[i]
            if i == 0 and key > minval and key < self.__C2:
                arr.append(key)
            elif key > minval:
                prekey = keys[i-1]
                if prekey == key-1 and key < self.__C2:
                    arr.append(key)
                    if maxcount==0 and len(arr) == mincount:
                        break
                else:
                    if len(arr) >= mincount:
                        break
                    else:
                        arr = []
                        arr.append(key)
        length = len(arr)
        if length >= mincount:
            for i in range(0, length):
                if (maxcount==0 and i == mincount) or (maxcount > 0 and i >= maxcount-1):
                    break
                val = arr[i]
                self.copyList(vector.get(val), resultCards)
        return resultCards

    def checkSingleAsShun(self, handCard, vector):
        """检查单牌是否可加到顺子中，返回剩余牌->[]"""
        shunvct = vector
        keys = shunvct.getKeys()
        if len(keys) == 0:
            return handCard;
        lastCards = []
        for card in handCard:
            suc = False
            for key in keys:
                items = shunvct.get(key)
                if len(items) == 0:
                    continue
                min =  self.getCardSize(items[0])
                max =  self.getCardSize(items[len(items)-1])
                val = self.getCardSize(card)
                if val == min-1:
                    items.insert(0, val)
                    suc = True
                    break
                elif val == max+1:
                    items.append(val)
                    suc = True
                    break
            if not suc:
                lastCards.append(card)
        return lastCards
    
    def convertVerctor(self, handCard, vector):
        """将牌转至vector对象->void"""
        for card in handCard:
            vector.add(self.getCardSize(card), card)

    def getSingleCard(self, handCard, vector):
        """取出单牌至vector对象，返回剩余牌->[]"""
        return self.getSameCard(handCard, 1, vector)
           
         
    def getSameCard(self, handCard, num, vector):
        """取相同的牌，返回剩余牌\n@num:相同的个数->[]"""
        lastCards = []
        arr = []
        count = len(handCard)
        if count < 2:
            if count > 0 and num == 1:
                card = handCard[0]
                vector.add(self.getCardSize(card), card)
                return arr;
            else:
               return handCard
        for i in range(0, count):
            card = handCard[i]
            if i == 0:
                arr.append(card)
                continue
            precard = handCard[i-1]
            val = self.getCardSize(card)
            preval = self.getCardSize(precard)
            if val == preval:
                arr.append(card)
            else:
                if len(arr) == num:
                    if num == 1:
                        vector.add(preval, arr[0])
                    else:
                        vector.add(preval, arr)
                else:
                    self.copyList(arr, lastCards)
                arr = []
                arr.append(card)

            if count-1 == i:
                if len(arr) == num:
                    if num == 1:
                        vector.add(val, arr[0])
                    else:
                        vector.add(val, arr)
                else:
                    self.copyList(arr, lastCards)
        return lastCards

    def getCard(self, handCard, cardVal, vector):
        """取出指定大小的牌至vector对象，返回剩余牌->[]\n@cardId:搜索指定牌大小"""
        arr = []
        count = len(handCard)
        for i in range(0, count):
            card = handCard[i]
            val = self.getCardSize(card)
            if val == cardVal:
                vector.add(val, card)
            else:
                arr.append(card)
        return arr
    
    def getCardHandleTimes(self, handCard):
        """计算出牌手数->int"""
        vector = self.splitCard(handCard)
        return self.getVectorHandleTimes(vector)

    def getVectorHandleTimes(self, vector):
        """计算出牌手数->int\n@vector:拆牌后的vector对象"""
        times = 0
        if vector.get('wang').len() > 0:
            times = times + 1
        bombLen = vector.get('bomb').len()
        if bombLen > 0:
            times = times + bombLen #四带2
        times = times + vector.get('shun').len()
        threeLen = vector.get('three').len()
        if threeLen > 0:
            times = times + threeLen
        twoLen = vector.get('two').len()
        if twoLen > 0:
            times = times + twoLen
        oneLen = vector.get('one').len()
        if oneLen > 0:
            times = times + oneLen
        times = times - threeLen * 1
        return times

    def getSingleCount(self, vector):
        """计算单牌数->int"""
        return vector.get('one').len()
    
    def getTwoCount(self, vector):
        """计算对牌数->int"""
        return vector.get('two').len()