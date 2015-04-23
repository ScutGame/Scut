class HttpParam():
    """httpGet参数"""

    def __init__(self):
        self.Result = True

class DataResult(object):
    """Action处理结果"""
    
    def __init__(self):
        self.Result = True

class JsonDataResult():
    """Json处理结果"""

    def __init__(self, urlParam):
        self.MsgId = urlParam.MsgId
        self.ActionId = urlParam.ActionId
        self.ErrorCode = 0
        self.ErrorInfo = ''
        self.Data = None

    def setBody(self, value):
        self.Data = value


def getGbLen(str):
    """获得中英文长度"""
    return len(str.encode('gb2312'))

