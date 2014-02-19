import random;

class ZyRandomUtils():
    """提供获取随机操作方法"""
    VERSION = 1

    def __init__(self):
        VERSION = 1;

    def random(self):
        """获取0.0-1随机值"""
        return random.random()

    def nextBool(self, rate):
        """获取下一个随机布尔值"""
        return random.random() < rate
    
    def getInt(self, min=1, max=100):
        """获取随机范围[min<=r<=max]的值"""
        return random.randint(min, max)
     
    def isHit(self, rate):
        """命中概率rate:float"""
        return self.nextBool(rate)

    def isHitInt(self, rate):
        """命中概率,rate:百分比概率值"""
        if rate > 100:
            rate = 100
        rate = rate * 0.01
        return self.nextBool(rate)
    
    def getHitIntIndex(self, list=[]):
        """获取命中的索引百分比,list:Int数组"""
        l = len(list);
        if l <= 0:
            raise ValueError, "arg is list type"

        index = -1;
        num = 0;
        p = self.random();
        for i in range(len(list)):
            num = num + (list[i] * 0.01)
            if p <= num:
                index = i;
                break;
        return index;

    def getHitIndex(self, list=[]):
        """获取命中的索引,list:float数组"""
        l = len(list);
        if l <= 0:
            raise ValueError, "arg is list type"

        index = -1;
        num = 0;
        p = self.random();
        for i in range(len(list)):
            num = num + list[i]
            if p <= num:
                index = i;
                break;
        return index;

    def getArray(self, list, cout):
        """从list数组中随机取cout个项"""
        return random.sample(list, cout)

