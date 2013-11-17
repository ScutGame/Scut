class HttpParam():
    """httpGet参数"""

    def __init__(self):
        self.Result = True

class DataResult(object):
    """Action处理结果"""
    
    def __init__(self):
        self.Result = True

def getGbLen(str):
    """获得中英文长度"""
    return len(str.encode('gb2312'))
