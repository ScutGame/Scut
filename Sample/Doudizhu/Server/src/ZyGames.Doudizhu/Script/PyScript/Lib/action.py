
class HttpParam():
    """httpGet param"""

    def __init__(self):
        self.Result = True

class DataResult(object):
    """Action process result"""
    
    def __init__(self):
        self.Result = True

def getGbLen(str):
    """get length"""
    return len(str.encode('gb2312'))
