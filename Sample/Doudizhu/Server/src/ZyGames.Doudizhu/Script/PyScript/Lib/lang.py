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
        "ErrorInfo": "系统繁忙",
        "LoadError": "加载数据失败！",
        "St1010_ChangeHeadError": "更新头像失败！",
        "St2001_CoinNotEnough": "您的金豆余额为{0}，进入房间至少需要{1}金豆，您的金豆不足！是否到商城购买金豆？",
        "St2001_GiffCoin": "您今日首次登陆游戏，获得{0}每日赠豆！",
        "St2001_ConnectError": "网路连接失败！",
        "St2001_InGaming": "您上一局游戏正处于托管中，请等待游戏结束！",
        "St2005_CalledIsEnd": "抢地主已经结束！",
        "St2009_OutCardError": "您出牌已违反规则！",
        "St2009_OutCardNoExist": "您出牌无效！",
        "St2009_ReOutCardError": "不能过牌，已轮到您重新出牌！",
        "St2009_OutCardExitPos": "您已离开桌面！",
        "St2011_Gameover": "您上一局游戏已结束！",

        "St_1011RealNameRangeOut": "您输入的真实姓名不能超过8个字符！",
        "St_1011BirthdayError": "您输入的出生日期错误！",
        "St_1011HobbyRangeOut": "您输入的爱好不能超过8个字符！",
        "St_1011ProfessionRangeOut": "您输入的职业字不能超过8个字符！",

        "St_1011RealNameExistKeyWord": "您输入的真实姓名存在非法字符！",
        "St_1011HobbyExistKeyWord": "您输入的爱好字存在非法字符！",
        "St_1011ProfessionExistKeyWord": "您输入的职业存在非法字符！",
        "St_2912UserActiveNumNotEnough":"活跃度未达到要求！",
        "St_2908_StHonorNullEnough":"荣誉值不足！", 
        "St_2909_StGoldNumNullEnough":"您的元宝余额不足，是否进行充值？",
        "St_2909_StAddNumTooEnough":"增加挑战次数已达到10次！",
        "St_2911_AllDay":"全天",
        "St3004_CurrentTaskIsNotCompleted":"当前任务未完成！",
        "St3004_CurrentTaskrewardHasReceived":"当前任务奖励已领取！",
        "St_7002_ShopOffTheShelf":"该商品已下架！",
        "St_7002_PurchasedHeadID":"您已购买过该头像！",
        "St9002_NotJoinedTheRoom":"未加入房间，不能聊天！",
        "St12002_FreeNotEnough":"免费转盘次数已用完！",
        "St12002_FreeEnough":"免费转盘次数未用完！",
        "St12002_UseGoldTurntable":"您将消耗%d元宝再次转盘？",
        "St12002_GoldenBeanAwards":"您获得%d金豆奖励！",        
    }
