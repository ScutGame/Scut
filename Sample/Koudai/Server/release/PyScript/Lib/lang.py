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
        "ErrorInfo": "系统繁忙！",
        "LoadError": "加载数据失败！",
        "St1404_OverfulfilNumNotEnough":"该佣兵灵魂数量不足，无法招募！",
        "St1114_IsNotGiftBag":"此物品不是礼包！",
        "St1425_MercenarySoulNotEnough":"佣兵灵魂为空！",
        "St1425_MercenaryOverfulfil":"是否对该佣兵进行突破潜能？",
        "St1425_MercenaryNoRecruitment":"该佣兵未招募！",
        "St1425_OverfulfilNumNotEnough":"该佣兵灵魂数量不足，无法突破！",
        "St1484_GeneralAndAbilityIsExist":"佣兵已经存在此魂技！",
        "St1484_OperateDefaultAbilityError":"不允许对默认魂技进行操作！",
        "St1442_GeneralLvNotUserLv":"佣兵等级不能高于会长等级3倍！",
        "St1442_GeneralLvIsMax":"佣兵等级已达到上限！",
        "St5108_CombatReplayFail":"战斗回放失败！",
        "St9302_IsNotFriend":"该用户不是您的好友,请重新选择好友",
        "St9302_OverMaxLength":"信件内容过长",
        "St9302_NoMail":"尊敬的会长，暂时没有收到伊妹儿！",
        "St9302_Minutes":"分钟前",
        "St9302_Hours":"小时前",
        "St9302_Days":"天前",
        "St7012_IntegralNotEnough":"积分不足",
        "St1449_GeneralHaveEqu":"请先卸下传承佣兵身上的装备",
        "St1449_GeneralHaveCrystal":"请先卸下传承佣兵身上的水晶",
        "St1449_GeneralHaveAbility":"请先卸下传承佣兵身上的魂技",
        "St1425_OverfulfilNumNotEnough":"该佣兵灵魂数量不足，无法突破",
        "St3013_NoWizard":"精灵已使用完！",
        "St13002_gameUserLv":"玩家等级不够，不能挑战！",
        "St13002_BattleRount":"玩家挑战次数已满，今日不能再挑战！",
        "St4405_UserLvNotEnough":"玩家未达到10级，不能进行挑战！",
        "St4405_ChallengeChanceNotEnough":"今日已无挑战机会！",
        "St4407_FightFail":"战斗失败，无法获得加强属性！",
        "St4407_NoAddProperty":"该层无加强属性！",
        "St4408_ProNotAvailable":"该属性加成暂不可用！",
        "St12057_UserLvNotEnough":"玩家未达到20级，暂未开启‘遗迹考古’活动！",
        "St12053_HasEnoughMapCount":"该怪物的所有碎片已集齐，无需继续挑战！",
        "St12053_EnergyNotEnough":"精力不足，请等待精力恢复后继续战斗！",
        "St12053_BuyOneChallenge":"挑战次数达到上限,是否花费10晶石增加一次挑战机会?",
        "St12101_NotLairTreasure":"龙穴获取奖励表失败",
        "St12102_GameCoinNotEnough":"金币不足",
        "St12102_PayGoldNotEnough":"晶石不足，是否立即充值？",
        "St12102_LairNumNot":"剩余次数不足！",
        "Gold":"晶石",
        "GameGoin":"金币",
        "Today":"今天",
        "Tomorrow":"明天",
        "DateFormatMMdd":"MM月dd日",
        "GetAccessFailure":"获取受权失败！"


    }