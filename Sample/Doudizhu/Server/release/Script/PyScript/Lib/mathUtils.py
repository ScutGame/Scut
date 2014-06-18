import ReferenceLib
from ZyGames.Framework.Common import MathUtils

class ZyMathUtils():
    """提供类型转换等操作"""
    VERSION = 1
    _math = None

    def __init__(self):
        self._math = MathUtils;
        VERSION = 1;
        

    def toString(self, str):
        """转换成string类型"""
        return self._math.ToNotNullString(str)
    
    def toBool(self, str):
        """将对象转换成布尔值"""
        return self._math.ToBool(str)
    
    def toByte(self, str):
        """将对象转换成字节值"""
        return self._math.ToByte(str)
    
    def toDateTime(self, str):
        """将对象转换成时间值"""
        return self._math.ToDateTime(str)

    def toDouble(self, str):
        """将对象转化为双精度"""
        return self._math.ToDouble(str)
    
    def toDecimal(self, str):
        """将对象转换成单精度值"""
        return self._math.ToDecimal(str)
    
    def toInt(self, str):
        """将对象转换成整型值"""
        return self._math.ToInt(str)

    def toShort(self, str):
        """将对象转换成短整型值"""
        return self._math.ToShort(str)
    
    def Now(self):
        """获取当前系统时间"""
        return self._math.Now


