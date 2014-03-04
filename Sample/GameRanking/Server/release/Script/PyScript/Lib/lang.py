#静态类
class Lang:
    """语言包配置
    >>Demo:
    from lang import Lang
    print Lang.getLang("ErrorCode")
    """
    @staticmethod
    def getLang(name):
        """获取语言包配置属性,参数name：属性名"""
        return Lang.__langconfig[name]

    __langconfig = {
        "ErrorCode": 10000,
        "ErrorInfo": "系统繁忙"
    }