using ZyGames.Framework.Game.Lang;

namespace ZyGames.Tianjiexing.Lang
{
    public interface IGameLanguage : ILanguage
    {

        /// <summary>
        /// 随机姓数据
        /// </summary>
        string[] St_FirstNickNames { get; }
        /// <summary>
        /// 随机名数据
        /// </summary>
        string[] St_LastNickNames { get; }

        /// 获取注册通行证ID出现异常
        /// </summary>
        string St1002_GetRegisterPassportIDError { get; }

        string UserInfoError { get; }

        string LoadDataError { get; }

        string FestivalDataFormat { get; }

        string ShortDataFormat { get; }

        string DataFormat { get; }

        short shortInt { get; }

        /// <summary>
        /// 玩家佣兵固定ID
        /// </summary>
        int GameUserGeneralID { get; }

        /// <summary>
        /// 君主帐号
        /// </summary>
        int SystemUserId { get; }
        /// <summary>
        /// 玩家名称
        /// </summary>
        string KingName { get; }

        /// <summary>
        /// 客户端跳转充值页面的特殊值 10
        /// </summary>
        int RechargeError { get; }

        string Color_Gray { get; }
        string Color_Green { get; }
        string Color_Blue { get; }
        string Color_PurPle { get; }
        string Color_Yellow { get; }
        string Color_Orange { get; }

        string PaySuccessMsg { get; }

        string Date_Yesterday { get; }
        string Date_BeforeYesterday { get; }
        string Date_Day { get; }
        string NotLogin { get; }

        string GameMoney_Coin { get; }
        string GameMoney_Gold { get; }

        /// <summary>
        /// 跨服战-冠军奖励
        /// </summary>
        string St_Combat_FistPrize { get; }

        /// <summary>
        /// 您当前绷带已使用完，请及时补充
        /// </summary>
        string St_User_LiveMsg { get; }

        /// <summary>
        /// 您的背包已满，无法获得任何物品，确定要进入副本吗
        /// </summary>
        string St_User_BeiBaoMsg { get; }

        /// <summary>
        /// 您的灵件背包已满，无法获得任何物品，确定要进入副本吗
        /// </summary>
        string St_User_SpareBeiBaoMsg { get; }

        /// <summary>
        /// 加载用户出现异常
        /// </summary>
        string Load_User_Error { get; }
        /// <summary>
        /// 增加用户对象到缓存中失败
        /// </summary>
        string CacheUser_AddUserToCacheError { get; }

        /// <summary>
        /// 公用提示 - 精力不足
        /// </summary>
        string St_EnergyNotEnough { get; }

        /// <summary>
        /// 公用提示 - 晶石不足
        /// </summary>
        string St_GoldNotEnough { get; }

        /// <summary>
        /// 鼓舞失败
        /// </summary>
        string St_InspireFailed { get; }

        /// <summary>
        /// 公用提示 - VIP等级不足
        /// </summary>
        string St_VipNotEnough { get; }

        /// <summary>
        /// 等级不足
        /// </summary>
        string St_LevelNotEnough { get; }

        /// <summary>
        /// 公用提示 - 金币不足
        /// </summary>
        string St_GameCoinNotEnough { get; }

        /// <summary>
        /// 公用提示 - 阅历不足
        /// </summary>
        string St_ExpNumNotEnough { get; }

        /// <summary>
        /// 声望不足 
        /// </summary>
        string St_ObtainNumNotEnough { get; }

        /// <summary>
        /// 灵石不足 
        /// </summary>
        string St_LingshiNumNotEnough { get; }

        /// <summary>
        /// 功能未开启 
        /// </summary>
        string St_NoFun { get; }

        /// <summary>
        /// 公用提示 - VIP等级不足，未开启该功能
        /// </summary>
        string St_VipNotEnoughNotFuntion { get; }
        /// <summary>
        /// 您已经成功签到{0}次，完成了本月的微信签到活动，感谢您对掌游科技的支持！下个月可以继续参与哦
        /// </summary>
        string St1000_RegistrationNum { get; }
        /// <summary>
        /// 您今天已经签到成功了，请明天再来签到吧！
        /// </summary>
        string St1000_IsRegistration { get; }
        /// <summary>
        /// 帐号不存在或者您尚未绑定帐号，签到失败！
        /// </summary>
        string St1000_UserExistent { get; }
        /// <summary>
        /// 本次签到成功！恭喜您获得：晶石*{0}！
        /// </summary>
        string St1000_GetRegistrationGold { get; }
        /// <summary>
        /// 您输入的账号或密码不正确
        /// </summary>
        string St1004_PasswordMistake { get; }
        /// <summary>
        /// 您输入的密码出现异常
        /// </summary>
        string St1004_PasswordError { get; }

        /// <summary>
        /// 您的账号未登录或已过期
        /// </summary>
        string St1004_IDNoLogin { get; }
        /// <summary>
        /// 您的账号已在其它地方登录
        /// </summary>
        string St1004_IDLogined { get; }
        /// <summary>
        /// 账号ID出现异常
        /// </summary>
        string St1004_UserIDError { get; }

        /// <summary>
        /// 您的账号已被系统强制下线
        /// </summary>
        string St1004_UserOffline { get; }

        /// <summary>
        /// 该账号已被封禁，登陆失败！
        /// </summary>
        string St1004_IDDisable { get; }

        /// <summary>
        /// 您尚未创建角色
        /// </summary>
        string St1005_RoleCheck { get; }
        /// <summary>
        /// 您已创建了角色
        /// </summary>
        string St1005_RoleExist { get; }

        /// <summary>
        /// 职业未创建
        /// </summary>
        string St1005_Professional { get; }

        /// <summary>
        /// 用户名字不能为空
        /// </summary>
        string St1005_KingNameNotEmpty { get; }

        /// <summary>
        /// 通行证出错
        /// </summary>
        string St1005_PassportError { get; }

        /// <summary>
        /// 昵称应在1-{0}个字符
        /// </summary>
        string St1005_KingNameTooLong { get; }

        /// <summary>
        /// 昵称已被使用
        /// </summary>
        string St1005_Rename { get; }

        /// <summary>
        /// 昵称存在特殊字符
        /// </summary>
        string St1005_RegistNameExceptional { get; }

        /// <summary>
        /// 您输入的昵称包含敏感字，请重新输入！

        /// </summary>
        string St1005_RegistNameKeyWord { get; }

        /// <summary>
        /// 密码格式错误
        /// </summary>
        string St1006_PasswordError { get; }
        /// <summary>
        /// 修改密码失败
        /// </summary>
        string St1006_ChangePasswordError { get; }

        /// <summary>
        /// 输入错误，请输入4-12位数字或字母
        /// </summary>
        string St1006_PasswordTooLong { get; }
        /// <summary>
        /// 密码不能包含特殊字符！
        /// </summary>
        string St1006_PasswordExceptional { get; }
        /// <summary>
        /// 当前剩余经验加成次数
        /// </summary>
        string St1008_ExpressSurplusNum { get; }

        /// <summary>
        /// 当前剩余金币加成次数
        /// </summary>
        string St1008_GameCoinSurplusNum { get; }

        /// <summary>
        /// 当前剩余双倍材料掉落次数
        /// </summary>
        string St1008_DoubleItemSurplusNum { get; }

        /// <summary>
        ///  战力加成剩余时间
        /// </summary>
        string St1008_CombatNumDate { get; }

        /// <summary>
        /// 当前剩余血量
        /// </summary>
        string St1008_BloodBagSurplusNum { get; }

        /// <summary>
        /// 当前变身卡剩余时间
        /// </summary>
        string St1008_TransfigurationDate { get; }

        /// <summary>
        /// 充值失败
        /// </summary>
        string St1066_PayError { get; }

        /// <summary>
        /// 购买精力花费{0}晶石
        /// </summary>
        string St1010_PayEnergyUseGold { get; }

        /// <summary>
        /// 今日精力购买次数已用完
        /// </summary>
        string St1010_JingliFull { get; }

        /// <summary>
        /// 在线领取 - 获得{0}精力
        /// </summary>
        string St1013_JingliPrize { get; }

        /// <summary>
        /// 在线领取 - 每日获得{0}精力
        /// </summary>
        string St1013_DailyJingliPrize { get; }

        /// <summary>
        /// 登录领取 - 获得{0}晶石，{1}金币
        /// </summary>
        string St1014_JingshiPrize { get; }

        /// <summary>
        /// 登录领取 - 恭喜您获得{0}阅历，{0}声望
        /// </summary>
        string St1018_ExpObtainPrize { get; }

        /// <summary>
        /// 每日领取 - 恭喜您获得{0}俸禄
        /// </summary>
        string St1020_FengLu { get; }

        /// <summary>
        /// 花费{0}晶石获得{1}金币，剩余{2}次！
        /// </summary>
        string St1011_PayCoinUseGold { get; }
        /// <summary>
        /// 进入金矿洞可以获得大量金币，是否花费{0}晶石获得{1}金币，今日剩余{2}次。
        /// </summary>
        string St1011_PayUseGold { get; }

        /// <summary>
        /// 恭喜您进入金矿洞获得{0}金币
        /// </summary>
        string St1011_PayGainCoin { get; }

        /// <summary>
        /// 今日挖金矿次数已用完
        /// </summary>
        string St1011_WaJinKuangFull { get; }


        /// <summary>
        /// 花费晶石开启背包格子   是否花费XX晶石开启N个背包格子？
        /// </summary>
        string St1104_UseGold { get; }

        /// <summary>
        /// 该物品已卖出
        /// </summary> 
        string St1107_UserItemNotEnough { get; }

        /// <summary>
        /// 仓库已满
        /// </summary>
        string St1107_WarehouseNumFull { get; }

        /// <summary>
        /// 背包已满
        /// </summary>
        string St1107_GridNumFull { get; }

        /// <summary>
        /// 花费{0}晶石开启仓库格子
        /// </summary>
        string St1108_WarehouseNumUseGold { get; }
        /// <summary>
        /// 超出扩容次数
        /// </summary>
        string St1110_OverExpansion { get; }

        /// <summary>
        /// 灵件背包已满
        /// </summary>
        string St1213_GridNumFull { get; }

        /// <summary>
        /// 该位置已装备灵件
        /// </summary>
        string St1213_GridPotionFull { get; }

        /// <summary>
        /// 是否花费{0}晶石，{1}金币洗涤属性？
        /// </summary>
        string St1214_ResetUseGold { get; }

        /// <summary>
        /// 是否花费{0}灵石，{1}金币洗涤属性？
        /// </summary>
        string St1214_ResetUseLingshi { get; }

        /// <summary>
        /// 是否花费{1}晶石开启{0}个灵件格子？
        /// </summary>
        string St1215_OpenGridNumUseGold { get; }

        /// <summary>
        /// 开启的格子已被占满
        /// </summary>
        string St1213_OpenNumNotEnough { get; }

        /// <summary>
        /// 格子未开启
        /// </summary>
        string St1213_GridNumNotEnough { get; }

        /// <summary>
        /// 潜能点不足
        /// </summary>
        string St1217_NotPotential { get; }

        /// <summary>
        /// 培养药剂不足
        /// </summary>
        string St1217_NotItem { get; }
        /// <summary>
        /// VIP3级以上才能白金培养
        /// </summary>
        string St1217_NotBaiJin { get; }
        /// <summary>
        /// VIP5级以上才能至尊培养
        /// </summary>
        string St1217_NotZhiZhun { get; }
        /// <summary>
        /// 您当天最大挑战次数已用完
        /// </summary>
        string St4002_NotChallengeNum { get; }
        /// <summary>
        /// 您当天最大挑战次数已用完，是否花费X晶石增加一次挑战次数（VIP3开启）?
        /// </summary>
        string St4002_IsPlotNum { get; }
        /// <summary>
        /// 已达到每日挑战次数！
        /// </summary>
        string St4002_IsPlotEliteNotChallengeNum { get; }
        /// <summary>
        /// 副本战斗 您未启用魔法阵
        /// </summary>
        string St4004_NoUseMagic { get; }

        /// <summary>
        /// 副本战斗 魔法阵未设置佣兵
        /// </summary>
        string St4004_EmbattleEmpty { get; }

        /// <summary>
        /// 副本战斗 佣兵血量不足！
        /// </summary>
        string St4004_GeneralLiveNotEnough { get; }

        /// <summary>
        /// 副本找不到怪物
        /// </summary>
        string St4011_NoMonster { get; }

        /// <summary>
        /// 佣兵职业不符
        /// </summary>
        string St1203_CareerError { get; }

        /// <summary>
        /// 装备强化
        /// </summary>
        string St1204_Message { get; }

        /// <summary>
        /// 冷却时间超上限，请稍等或用晶石消除！
        /// </summary>
        string St1204_ColdTime { get; }

        /// <summary>
        /// 装备强化冷却中！
        /// </summary>
        string St1204_Colding { get; }


        /// <summary>
        /// 是否花费{0}晶石开启灵件第{1}个属性？
        /// </summary>
        string St1216_EnableSpartProperty { get; }

        /// <summary>
        /// 此附魔符不存在
        /// </summary>
        string St1256_EnchantNotEnough { get; }

        /// <summary>
        /// 没有足够的附魔符
        /// </summary>
        string St1256_EnchantNumNotEnough { get; }

        /// <summary>
        /// 附魔符已达最高级
        /// </summary>
        string St1256_OutMaxEnchantLv { get; }

        /// <summary>
        /// 附魔符培养已达上限
        /// </summary>
        string St1258_OutMaxEnchantMature { get; }

        /// <summary>
        /// 培养成功，消耗{0}点魔晶提升{1}点成长值！
        /// </summary>
        string St1258_ConsumeMagicCrystalUpEnhance { get; }

        /// <summary>
        /// 培养成功，消耗{0}点晶石提升{1}点成长值！
        /// </summary>
        string St1258_ConsumeGoldNumUpEnhance { get; }

        /// <summary>
        /// 培养失败！消耗{0}点魔晶！
        /// </summary>
        string St1258_EnhanceCultureFailedMagicCrystal { get; }

        /// <summary>
        /// 培养失败！消耗{0}点晶石！
        /// </summary>
        string St1258_EnhanceCultureFailedGold { get; }

        /// <summary>
        /// 魔晶不足
        /// </summary>
        string St1258_MagicCrystalNotEnough { get; }

        /// <summary>
        /// 该位置已装备附魔符
        /// </summary>
        string St1259_EnchantOpenGridFull { get; }

        /// <summary>
        /// 附魔符背包已满
        /// </summary>
        string St1259_EnchantGridNumFull { get; }

        /// <summary>
        /// 当前装备不是武器
        /// </summary>
        string St1259_UserItemNotWuQi { get; }

        /// <summary>
        /// 您确定花费{0}晶石开启{1}个格子？
        /// </summary>
        string St1260_UseGoldOpenPackage { get; }

        /// <summary>
        /// 背包没有可装备附魔符
        /// </summary>
        string St1261_EnchantEquipmentNotEnough { get; }

        /// <summary>
        /// 背包没有可合成附魔符
        /// </summary>
        string St1262_EnchantSynthesisNotEnough { get; }

        /// <summary>
        /// 猎命空间已满
        /// </summary>
        string St1305_FateBackpackFull { get; }


        /// <summary>
        /// 请清理背包，获得奖励！
        /// </summary>
        string St1305_BeiBaoBackpackFull { get; }

        /// <summary>
        /// 恭喜您获得命运礼包！
        /// </summary>
        string St1305_GainCrystalBackpack { get; }

        /// <summary>
        /// 当前人物已点亮
        /// </summary>
        string St1305_HuntingIDLight { get; }

        /// <summary>
        /// {0}人品大爆发，随手一摸发现竟然是{1}水晶{2}，看四下无人，赶紧藏入口袋！
        /// </summary>
        string St1305_HighQualityNotice { get; }

        /// <summary>
        /// 天上掉馅饼了吗？玩家{0}伸手摸进宝箱，发现竟然是一个金光灿灿的橙色水晶{1}，元方，此事你怎么看？
        /// </summary>
        string St1305_GainQualityNotice { get; }

        /// <summary>
        /// 命运水晶背包已满
        /// </summary>
        string St1307_FateBackpackFull { get; }

        /// <summary>
        /// 命运背包空间不足
        /// </summary>
        string St1307_FateBackSpaceFull { get; }

        /// <summary>
        ///此水晶不存在
        /// </summary>
        string St1308_CrystalNotEnough { get; }

        /// <summary>
        ///此水晶已达最高级
        /// </summary>
        string St1308_CrystalLvFull { get; }

        /// <summary>
        /// 佣兵开启的格子已被占满
        /// </summary>
        string St1309_OpenNumNotEnough { get; }

        /// <summary>
        /// 该佣兵已装备了相同属性的水晶
        /// </summary>
        string St1309_TheSameFate { get; }

        /// <summary>
        /// 该格子已装备水晶
        /// </summary>
        string St1309_TheGridFullSameFate { get; }

        /// <summary>
        /// 装备强化等级已达上限
        /// </summary>
        string St1204_EquMaxLv { get; }

        /// <summary>
        /// 装备强化等级不能超过佣兵等级
        /// </summary>
        string St1204_EquGeneralMaxLv { get; }

        /// <summary>
        /// 花费晶石开启命运背包格子
        /// </summary>
        string St1310_UseCrystalGold { get; }

        /// <summary>
        /// 未达到招募条件，不可招募
        /// </summary>
        string St1404_RecruitNotFilter { get; }

        /// <summary>
        /// 佣兵数已满，不可邀请
        /// </summary>
        string St1404_MaxGeneralNumFull { get; }

        /// <summary>
        /// 玩家角色不能离队
        /// </summary>
        string St1405_LiDuiNotFilter { get; }

        /// <summary>
        /// 该品级药剂已服满
        /// </summary>
        string St1407_MedicineNumFull { get; }

        /// <summary>
        /// 背包不存在该药剂
        /// </summary>
        string St1407_MedicineNum { get; }

        /// <summary>
        /// 直接服用药剂需{0}晶石
        /// </summary>
        string St1407_MedicineUseGold { get; }

        /// <summary>
        /// 属性培养已达上限
        /// </summary>
        string St1409_maxTrainingNum { get; }

        /// <summary>
        /// 玩家没有此佣兵
        /// </summary>
        string St1405_GeneralIDNotEnough { get; }

        /// <summary>
        /// 上线未满半小时，不能修炼
        /// </summary>
        string St1411_LessThanHalfAnHour { get; }

        /// <summary>
        /// 奇幻粉末不足
        /// </summary>
        string St1415_MedicineNum { get; }

        /// <summary>
        /// 背包已满，请清理背包后重新摘取
        /// </summary>
        string St1415_GridNumNotEnough { get; }

        /// <summary>
        /// 摘取{0}需消耗{1}个奇幻粉末，是否要摘取？摘取成功率为{2}%
        /// </summary>
        string St11415_ClearMedicine { get; }

        /// <summary>
        /// 摘取失败
        /// </summary>
        string St11415_Clearfail { get; }

        /// <summary>
        /// 被传承佣兵等级低于传承佣兵，无法传承
        /// </summary>
        string St1418_HeritageLvLow { get; }

        /// <summary>
        /// 请选择传承佣兵
        /// </summary>
        string St1418_HeritageNotEnough { get; }

        /// <summary>
        /// 请选择被传承佣兵
        /// </summary>
        string St1419_IsHeritageNotEnough { get; }

        /// <summary>
        /// 传承佣兵和被传承佣兵不能是同一人
        /// </summary>
        string St1419_HeritageNotInIsHeritage { get; }

        /// <summary>
        /// %s不足
        /// </summary>
        string St1419_DanInsufficientHeritage { get; }

        /// <summary>
        /// 此佣兵已传承过
        /// </summary>
        string St1419_HeritageInUse { get; }

        /// <summary>
        /// 传承成功
        /// </summary>
        string St1419_HeritageSuccess { get; }

        /// <summary>
        /// 是否花费{0}晶石进行晶石传承
        /// </summary>
        string St1419_GoldHeritage { get; }

        /// <summary>
        /// 是否花费%d晶石进行至尊传承
        /// </summary>
        string St1419_ExtremeHeritage { get; }

        /// <summary>
        /// 是否花费{0}晶石增加{1}好感度
        /// </summary>
        string St1422_PresentationUseGold { get; }

        /// <summary>
        /// 当前饱食度已满，无法使用礼物
        /// </summary>
        string St1422_FeelMaxSatiationNum { get; }

        /// <summary>
        /// 今日晶石赠送次数已用完
        /// </summary>
        string St1422_PresentationGoldNum { get; }

        /// <summary>
        /// 使用成功，增加好感度{0}
        /// </summary>
        string St1422_PresentationFeelNum { get; }

        /// <summary>
        /// 该佣兵好感度已达最高等级
        /// </summary>
        string St1422_MaxFeelFull { get; }

        /// <summary>
        ///是否花费1个{0}清除当前饱食度？
        /// </summary>
        string St1423_ClearCurrSatiation { get; }

        /// <summary>
        /// 背包不存在消除饱食度物品
        /// </summary> 
        string St1423_UserItemNotEnough { get; }

        /// <summary>
        /// 该佣兵今日消除饱食度次数已用完
        /// </summary> 
        string St1423_DragonHolyWater { get; }

        /// <summary>
        /// 不能对默认魂技进行操作
        /// </summary>
        string St1484_OperateDefaultAbility { get; }

        /// <summary>
        /// 魔术等级已达到最高级
        /// </summary>
        string St1503_MaxMagicLv { get; }

        /// <summary>
        /// 魔术 - 等级不能超过玩家等级
        /// </summary>
        string St1503_MagicLevel { get; }

        /// <summary>
        /// 魔术 - 魔法阵等级不能超过需求等级
        /// </summary>
        string St1503_MagicEmbattleLevel { get; }

        /// <summary>
        /// 魔术强化冷却中！
        /// </summary>
        string St1503_MagicColding { get; }

        /// <summary>
        /// 阅历不足
        /// </summary>
        string St1503_UpgradeExpNum { get; }

        /// <summary>
        /// 魔术不存在
        /// </summary>
        string St1503_MagicIDNotEnough { get; }

        /// <summary>
        /// 材料不足
        /// </summary>
        string St1603_MaterialsNotEnough { get; }

        /// <summary>
        ///  您当前缺少合成所需的装备，无法晶石合成
        /// </summary>
        string St1603_EquNotEnough { get; }

        /// <summary>
        /// 您当前等级无法到达该副本
        /// </summary>
        string St1604_MaterialsCityID { get; }

        /// <summary>
        /// 合成卷轴需{0}晶石
        /// </summary>
        string St1603_SynthesisEnergyNum { get; }

        /// <summary>
        /// 绷带已消耗是否补充 
        /// </summary>
        string St1605_BandageNotEnough { get; }

        /// <summary>
        /// 绷带使用中 
        /// </summary>
        string St1605_BandageUse { get; }

        /// <summary>
        /// 是否使用2晶石打开道具商店购买绷带
        /// </summary>
        string St1605_UseTwoGold { get; }

        /// <summary>
        /// {0}不足，无法开启{1}！
        /// </summary>
        string St1606_OpenPackLackItem { get; }

        /// <summary>
        /// 背包格子不足
        /// </summary>
        string St1606_GridNumNotEnough { get; }

        /// <summary>
        /// 命格背包已满或背包格子不足
        /// </summary>
        string St1606_BackpackFull { get; }

        /// <summary>
        /// 队列加速 - 花费晶石消除该冷却时间
        /// </summary>
        string St1702_UseGold { get; }

        /// <summary>
        /// 开启队列 - 花费晶石开启队列
        /// </summary>
        string St1703_UseGold { get; }

        /// <summary>
        /// 开启队列 - 队列已全部开启
        /// </summary>
        string St1703_QueueNumFull { get; }

        /// <summary>
        /// 主角无法下阵
        /// </summary>
        string St1902_UserGeneralUnable { get; }

        /// <summary>
        /// 领土战参战状态不允许改变阵法！
        /// </summary>
        string St1902_CountryCombatNotUpEmbattle { get; }

        /// <summary>
        /// 加入国家提示莫根马/哈斯德尔
        /// </summary>
        string St2004_CountryName { get; }

        string St2004_CountryM { get; }
        string St2004_CountryH { get; }

        /// <summary>
        /// 任务系统 - 等级不足，不能领取任务
        /// </summary>
        string St3002_LvNotEnough { get; }
        /// <summary>
        /// 任务系统 - 主线任务未完成
        /// </summary>
        string St3002_MainNoCompleted { get; }
        /// <summary>
        /// 任务系统 - "任务已完成，不能放弃
        /// </summary>
        string St3002_Completed { get; }
        /// <summary>
        /// 任务系统 - 未能找到任务
        /// </summary>
        string St3002_NotFind { get; }
        /// <summary>
        /// 任务系统 - 任务不能领取
        /// </summary>
        string St3002_NoAllowTaked { get; }
        /// <summary>
        /// 任务系统 - 任务未领取
        /// </summary>
        string St3002_NoTaked { get; }

        /// <summary>
        /// 任务系统 -刷新任务星级需{0}晶石
        /// </summary>
        string St3005_RefreashUseGold { get; }

        /// <summary>
        /// 任务系统 -您当前的任务星级已满，无法刷新
        /// </summary>
        string St3005_RefreashStarFull { get; }

        /// <summary>
        /// 任务系统 -直接完成任务需{0}晶石
        /// </summary>
        string St3005_CompletedUseGold { get; }

        /// <summary>
        /// 任务系统 - 任务已达到完成次数
        /// </summary>
        string St3005_CompletedTimeout { get; }
        /// <summary>
        /// 任务系统 - 任务未完成
        /// </summary>
        string St3007_NoCompleted { get; }

        /// <summary>
        /// 您的晶石不足，无法召唤
        /// </summary>
        string St3203_GoldNotEnouht { get; }

        /// <summary>
        /// 您的VIP等级不足，无法召唤
        /// </summary>
        string St3203_VipNotEnouht { get; }

        /// <summary>
        /// 是否花费{0}晶石刷新宠物！
        /// </summary>
        string St3203_RefeshPet { get; }

        /// <summary>
        /// 是否花费{0}晶石直接召唤狮子！
        /// </summary>
        string St3203_ZhaohuangPet { get; }

        /// <summary>
        /// 您的宠物已达到最高等级！
        /// </summary>
        string St3203_MaxPet { get; }

        /// <summary>
        /// 您不能选择该宠物，宠物未开启！
        /// </summary>
        string St3204_PetNoEnable { get; }

        /// <summary>
        /// 您的好友未通过邀请申请！
        /// </summary>
        string St3204_PetYaoqingNoPass { get; }

        /// <summary>
        /// 您的宠物还在赛跑中，请等待！
        /// </summary>
        string St3204_PetRunning { get; }

        /// <summary>
        /// 您今日宠物赛跑次数已用完！
        /// </summary>
        string St3204_PetRunTimesOut { get; }
        /// <summary>
        /// 您的好友今日护送宠物赛跑次数已用完！
        /// </summary>
        string St3204_PetHelpeTimesOut { get; }

        /// <summary>
        /// 您不能拦截自己的宠物！
        /// </summary>
        string St3206_PetInterceptError { get; }
        /// <summary>
        /// 您正在护送好友的宠物！
        /// </summary>
        string St3206_PetFriendError { get; }
        /// <summary>
        /// 您今日拦截宠物赛跑次数已用完！
        /// </summary>
        string St3206_PetInterceptTimesOut { get; }
        /// <summary>
        /// 您拦截的宠物已经赛跑完！
        /// </summary>
        string St3206_PetInterceptFaild { get; }
        /// <summary>
        /// 您已经拦截过该宠物，不可重复拦截！
        /// </summary>
        string St3206_PetInterceptFull { get; }
        /// <summary>
        /// 您今天已祈祷过！
        /// </summary>
        string St3302_IsPray { get; }

        /// <summary>
        /// 副本系统 - 您的绷带已使用完，请及时补充。
        /// </summary>
        string St4002_PromptBlood { get; }

        /// <summary>
        /// 副本系统 -  背包中没有绷带，请及时购买。
        /// </summary>
        string St4002_UserItemPromptBlood { get; }

        /// <summary>
        /// 副本系统 - 今日精英副本次数已用完
        /// </summary>
        string St4002_EliteUsed { get; }

        /// <summary>
        /// 今日该英雄副本次数已用完
        /// </summary>
        string St4002_HeroPlotNum { get; }

        /// <summary>
        /// 副本系统 - 正在扫荡中
        /// </summary>
        string St4007_Saodanging { get; }
        /// <summary>
        /// 副本系统 - 扫荡已结束
        /// </summary>
        string St4007_SaodangOver { get; }
        /// <summary>
        /// 副本系统 - 背包已满无法进行扫荡
        /// </summary>
        string St4007_BeiBaoTimeOut { get; }
        /// <summary>
        /// 副本系统 - 是否花费{0}个晶石直接完成扫荡
        /// </summary>
        string St4008_Tip { get; }

        /// <summary>
        /// 副本系统 - 花费{0}个晶石重置精英副本
        /// </summary>
        string St4012_JingYingPlot { get; }

        /// <summary>
        /// 副本系统 - 今日重置精英副本次数已用完
        /// </summary>
        string St4012_JingYingPlotFull { get; }

        /// <summary>
        /// 副本系统 - 花费{0}个晶石重置英雄副本
        /// </summary>
        string St4014_HeroRefreshPlot { get; }

        /// <summary>
        /// 副本系统 - 当前城市今日重置英雄副本次数已用完
        /// </summary>
        string St4014_HeroRefreshPlotFull { get; }

        /// <summary>
        /// 多人副本已结束
        /// </summary>
        string St4202_OutMorePlotDate { get; }

        /// <summary>
        /// 今日已打过此副本
        /// </summary>
        string St4205_PlotNotEnough { get; }

        /// <summary>
        /// 已在队伍中
        /// </summary>
        string St4205_InTeam { get; }

        /// <summary>
        /// 队伍中人数已满
        /// </summary>
        string St4206_TeamPeopleFull { get; }

        /// <summary>
        /// 没有可加入的队伍
        /// </summary>
        string St4206_NoTeam { get; }

        ///<summary>
        ///队伍已开始战斗
        ///</summary>
        string St4206_TeamPlotStart { get; }

        ///<summary>
        ///队伍已解散
        ///</summary>
        string St4206_TeamPlotLead { get; }

        ///<summary>
        ///队伍正在战斗中
        ///</summary>
        string St4208_IsCombating { get; }

        ///<summary>
        ///队伍人数不足
        ///</summary>
        string St4210_PeopleNotEnough { get; }

        ///<summary>
        ///不存在此副本
        ///</summary>
        string St4210_PlotNotEnough { get; }

        ///<summary>
        /// 您本次多人副本挑战胜利，奖励{0}阅历，{1}*{2}！
        ///</summary>
        string St4211_MorePlotReward { get; }

        ///<summary>
        /// 未加入队伍
        ///</summary>
        string St4211_UserNotAddTeam { get; }

        ///<summary>
        ///天地劫今日已刷新
        ///</summary>
        string St4302_PlotRefresh { get; }

        ///<summary>
        ///天地劫灵件掉落
        ///</summary>
        string St4303_SparePartFalling { get; }

        /// <summary>
        /// 此操作将花费您{0}晶石并回到本层第一关，确定执行此操作
        /// </summary>
        string St4302_SecondRefreshKalpa { get; }

        /// <summary>
        /// 此操作将花费您{0}晶石并回到上一层第一关，确定执行此操作
        /// </summary>
        string St4302_LastRefreshKalpa { get; }

        /// <summary>
        /// 您当前位置无需返回本层！
        /// </summary>
        string St4302_LastRefreshKalpaNotEnough { get; }

        /// <summary>
        /// 天地劫下一关暂未开启
        /// </summary>
        string St4303_PlotNotEnable { get; }

        /// <summary>
        /// 天地劫下一层暂未开启
        /// </summary>
        string St4303_PlotNotEnableLayerNum { get; }

        /// <summary>
        /// 您
        /// </summary>
        string St5101_JingJiChangMingCheng { get; }

        /// <summary>
        /// 今日挑战次数已用完
        /// </summary>
        string St5103_ChallengeNotNum { get; }

        /// <summary>
        /// 挑战时间冷却中！
        /// </summary>
        string St5107_Colding { get; }

        /// <summary>
        /// 竞技场排名奖励{0}声望，{1}金币！
        /// </summary>
        string St5106_JingJiChangRankReward { get; }

        /// <summary>
        /// 今日挑战次数已用完！
        /// </summary>
        string St5107_ChallGeNumFull { get; }

        /// <summary>
        /// XX打败了XX，登上排行版第一的至尊宝座
        /// </summary>
        string St5107_JingJiChangOneRank { get; }

        /// <summary>
        /// XX排名连续上升了N名，已经势如破竹，不可阻挡了。
        /// </summary>
        string St5107_JingJiChangMoreNum { get; }

        /// <summary>
        /// {0}霸气外露，突破纪录达到{1}连杀
        /// </summary>
        string St5107_JingJiChangWinNum { get; }

        /// <summary>
        /// XX（玩家名称）打破了XX（玩家名称）的最高连杀纪录，已经无法阻挡了
        /// </summary>
        string St5107_ZuiGaoLianSha { get; }

        /// <summary>
        /// XX（玩家名称）达到N连胜，奖励金币N，晶石N
        /// </summary>
        string St5107_ArenaWinsNum { get; }

        /// <summary>
        /// 您还未加入国家阵营
        /// </summary>
        string St5201_NoJoinCountry { get; }
        /// <summary>
        /// 国家领土战未开始
        /// </summary>
        string St5201_CombatNoStart { get; }
        /// <summary>
        /// 国家领土战已结束
        /// </summary>
        string St5201_CombatOver { get; }

        /// <summary>
        /// 消耗200阅历有几率增加20%战斗力
        /// </summary>
        string St5202_InspireTip { get; }
        /// <summary>
        /// 消耗20晶石增加20%战斗力
        /// </summary>
        string St5202_InspireGoldTip { get; }
        /// <summary>
        /// 生命不足请补充血量
        /// </summary>
        string St5204_LifeNotEnough { get; }

        /// <summary>
        /// 挑战还未开始
        /// </summary>
        string St5402_CombatNoStart { get; }

        /// <summary>
        /// 您本次领土战中胜利{0}场，失败{1}场。总共获得{2}金币，{3}声望，下次继续努力！
        /// </summary>
        string St5204_CombatTransfusion { get; }

        /// <summary>
        /// 挑战还未开始
        /// </summary>
        string St5204_CombatNoStart { get; }

        /// <summary>
        /// 您还未复活，请等待！
        /// </summary>
        string St5402_IsReliveError { get; }

        /// <summary>
        /// 是否消耗{0}晶石直接进入战斗
        /// </summary>
        string St5403_CombatGoldTip { get; }

        /// <summary>
        /// 您已经复活了5次，不能再使用浴火重生
        /// </summary>
        string St5403_IsReLiveMaxNum { get; }
        /// <summary>
        /// 您已经复活，不需要使用浴火重生
        /// </summary>
        string St5403_IsLive { get; }

        /// <summary>
        /// 挑战还在初始化数据，请等待
        /// </summary>
        string St5405_CombatWait { get; }
        /// <summary>
        /// Boss已被击杀
        /// </summary>
        string St5405_BossKilled { get; }
        /// <summary>
        /// 挑战已结束
        /// </summary>
        string St5405_CombatOver { get; }

        /// <summary>
        /// {0}玩家获得Boss战击杀奖，奖励{1}金币
        /// </summary>
        string St5405_CombatKillReward { get; }

        /// <summary>
        /// 参加Boss战获得伤害奖励金币：{0}，声望：{1}
        /// </summary>
        string St5405_CombatHarmReward { get; }

        /// <summary>
        /// {0}玩家获得Boss战第{1}名，奖励{2}声望{3}
        /// </summary>
        string St5405_CombatRankmReward { get; }

        /// <summary>
        /// 物品与数量
        /// </summary>
        string St5405_CombatNum { get; }

        /// <summary>
        /// 已是会员不能申请
        /// </summary>
        string St6006_AlreadyMember { get; }

        /// <summary>
        /// 申请公会中
        /// </summary>
        string St6006_ApplyGuild { get; }

        /// <summary>
        /// 已达申请上限
        /// </summary>
        string St6006_ApplyMaxGuild { get; }

        /// <summary>
        /// 已申请该公会
        /// </summary>
        string St6006_ApplyMember { get; }


        /// <summary>
        /// 退出工会未满8小时
        /// </summary>
        string St6006_GuildMemberNotDate { get; }

        /// <summary>
        /// 普通成员没权限
        /// </summary>
        string St6007_AuditPermissions { get; }

        /// <summary>
        /// 只有公会的会长和副会长才有权限使用该道具
        /// </summary>
        string St6024_AuditPermissions { get; }

        /// <summary>
        /// 该玩家不是会长没有权限
        /// </summary>
        string St6008_NotChairman { get; }

        /// <summary>
        /// 副会长人数已满
        /// </summary>
        string St6008_VicePresidentNum { get; }

        /// <summary>
        /// 该会员不是副会长不能转让
        /// </summary>
        string St6008_NotVicePresident { get; }

        /// <summary>
        /// 该会员不是副会长不能撤销
        /// </summary>
        string St6008_NotVicePresidentCeXiao { get; }

        /// <summary>
        /// 内容不能为空
        /// </summary>
        string St6009_ContentNotEmpty { get; }

        /// <summary>
        /// 内容应在100个字以内
        /// </summary>
        string St6009_ContentTooLong { get; }

        /// <summary>
        /// 您当前为公会会长，无法退出公会
        /// </summary>
        string St6010_Chairman { get; }

        /// <summary>
        /// 您不是该工会成员
        /// </summary>
        string St6011_GuildMemberNotMember { get; }

        /// <summary>
        /// 今日已上香
        /// </summary>
        string St6012_HasIncenseToday { get; }

        /// <summary>
        /// 您成功进行七星朝圣，获得声望：+300
        /// </summary>
        string St6013_GainObtionNum { get; }

        /// <summary>
        /// 工会上香已满级
        /// </summary>
        string St6012_GuildShangXiang { get; }

        /// <summary>
        /// 加入公会当天无法进行朝圣！
        /// </summary>
        string St6014_GuildFirstDateNotDevilNum { get; }

        /// <summary>
        /// 是否花费{0}晶石召唤散仙封魔
        /// </summary>
        string St6015_SummonSanxian { get; }


        /// <summary>
        /// 已是工会成员不能创建工会
        /// </summary>
        string St6017_UnionMembers { get; }

        /// <summary>
        /// 該名字已有公會命名，請重新輸入
        /// </summary>
        string St6017_Rename { get; }

        /// <summary>
        /// 公会名字不能为空
        /// </summary>
        string St6017_GuildNameNotEmpty { get; }

        /// <summary>
        /// 公会名称应在4-12个字符以内
        /// </summary>
        string St6017_GuildNameTooLong { get; }

        /// <summary>
        /// 名称已被使用
        /// </summary>
        string St6017_GuildRename { get; }

        /// <summary>
        /// 公会人数已满
        /// </summary>
        string St6019_GuildMaxPeople { get; }

        /// <summary>
        /// 已加入其他公会
        /// </summary>
        string St6019_AddGuild { get; }

        /// <summary>
        ///小李飞刀进行七星朝圣还需要N人，公会成员可以前往协助。
        /// </summary>
        string St6022_GuildConvene { get; }

        /// <summary>
        /// 公会添加人数已达上限
        /// </summary>
        string St6024_GuildAddMemberToLong { get; }

        /// <summary>
        /// 本周公会BOSS未开始
        /// </summary>
        string St6101_GuildBossNotOpen { get; }

        /// <summary>
        /// 本周公会BOSS已结束
        /// </summary>
        string St6101_GuildBossOver { get; }

        /// <summary>
        /// 本周公会BOSS挑战时间未设置
        /// </summary>
        string St6101_GuildBossSet { get; }


        /// <summary>
        /// {0}玩家获得Boss战击杀奖，奖励{1}金币
        /// </summary>
        string St6105_CombatKillReward { get; }

        /// <summary>
        /// 参加Boss战获得伤害奖励金币：{0}，声望：{1}
        /// </summary>
        string St6105_CombatHarmReward { get; }

        /// <summary>
        /// {0}玩家获得Boss战第{1}名，奖励{2}声望{3}
        /// </summary>
        string St6105_CombatRankmReward { get; }

        /// <summary>
        /// 本周公会BOSS挑战时间已设置
        /// </summary>
        string St6109_GuildBossTime { get; }

        /// <summary>
        /// 您不是公会成员，请先加入公会
        /// </summary>
        string St6203_GuildMemberNotEnough { get; }

        /// <summary>
        /// 捐献N金币获得NN声望和NN贡献度
        /// </summary>
        string St6204_GuildMemberGameCoinDonate { get; }

        /// <summary>
        /// 捐献N晶石获得NN声望和NN贡献度
        /// </summary>
        string St6204_GuildMemberGoldDonate { get; }


        /// <summary>
        /// 您输入的数值大于当日可捐献最大金额，请重新输入
        /// </summary>
        string St6204_OutMaxGuildMemberDonate { get; }

        /// <summary>
        /// 您输入的数值大于当日可捐献最大晶石，请重新输入
        /// </summary>
        string St6204_OutMaxGuildMemberDonateGold { get; }

        /// <summary>
        /// 今日捐献数量已达上限
        /// </summary>
        string St6204_OutMaxGuildMemberNum { get; }

        /// <summary>
        /// 請輸入分配金額
        /// </summary>
        string St6204_GuildMemberDonateNum { get; }

        /// <summary>
        /// 请输入分配晶石
        /// </summary>
        string St6204_GuildMemberDonateNumGold { get; }

        /// <summary>
        /// 公会晨练活动还没有开始！
        /// </summary>
        string St6301_GuildExerciseNoOpen { get; }

        /// <summary>
        /// 公会晨练活动已开始，现在不能参加！
        /// </summary>
        string St6301_GuildExerciseIsOpen { get; }

        /// <summary>
        /// 公会晨练活动已结束！
        /// </summary>
        string St6301_GuildExerciseClose { get; }

        /// <summary>
        /// 您超过5道未答题，退出公会
        /// </summary>
        string St6301_GuildExerciseTimeOut { get; }

        /// <summary>
        /// 全员正确，等级提升！
        /// </summary>
        string St6303_GuildExerciseAllAnswerTrue { get; }

        /// <summary>
        /// 未能全对，从头开始
        /// </summary>
        string St6303_GuildExerciseAllAnswerFalse { get; }

        /// <summary>
        /// 该问题已回答过了
        /// </summary>
        string St6305_GuildExerciseISAnswer { get; }

        /// <summary>
        /// 是否花费{0}晶石自动答对此题
        /// </summary>
        string St6305_GuildExerciseGoldAnswer { get; }

        /// <summary>
        /// 是否花费{0}晶石自动回答并答对所有题目
        /// </summary>
        string St6305_GuildExerciseAutoAnswer { get; }

        /// <summary>
        /// 回答正确，获得{0}经验和{1}阅历
        /// </summary>
        string St6305_GuildExerciseAnswerSuss { get; }

        /// <summary>
        /// 回答错误!
        /// </summary>
        string St6305_GuildExerciseAnswerFail { get; }

        /// <summary>
        /// {0}太过注意路上的“风景”，以至答错了题目。
        /// </summary>
        string St6305_GuildExerciseGuildChat { get; }

        /// <summary>
        /// 公会技能点不足
        /// </summary>
        string St6205_GuildMemberDonateNotEnough { get; }

        /// <summary>
        /// {0}技能升到{1}级
        /// </summary>
        string St6205_GuildMemberJiNengShengJi { get; }

        /// <summary>
        /// 天下第一正在报名中，请各位勇士报名参加。
        /// </summary>
        string St6501_ServerCombatBroadcas { get; }

        /// <summary>
        /// 今日天下第一火爆下注，请没有下注的勇士抓紧时间了，金钱不等人！
        /// </summary>
        string St6501_ServerCombatStakeBroadcas { get; }

        /// <summary>
        /// 天下第一大会即将开始，请参赛各勇士做好准备。
        /// </summary>
        string St6501_SyncBroadcas { get; }

        /// <summary>
        /// 跨服战尚未开启
        /// </summary>
        string St6501_DoesNotStart { get; }

        /// <summary>
        /// 您已经报名了
        /// </summary>
        string St6502_YouHavesignedup { get; }

        /// <summary>
        /// 当前阶段不能报名！
        /// </summary>
        string St6502_Notsignup { get; }

        /// <summary>
        /// 还在报名阶段！
        /// </summary>
        string St6503_AlsoInTheStage { get; }

        /// <summary>
        /// 当前没有您的相关战绩！
        /// </summary>
        string St6504_NotHaveInfo { get; }

        /// <summary>
        /// 已下注{0}{1}W
        /// </summary>
        string St6506_HasBet { get; }

        /// <summary>
        /// 金币不足
        /// </summary>
        string St6507_GoldCoinShortage { get; }

        /// <summary>
        /// 您已下注
        /// </summary>
        string St6507_YouFaveBet { get; }

        /// <summary>
        /// 下注[{0}]{1}金币
        /// </summary>
        string St6506_BetGold { get; }

        /// <summary>
        /// ,等待结果……
        /// </summary>
        string St6506_WaitResults { get; }

        /// <summary>
        /// ,获利{0}金币!
        /// </summary>
        string St6506_ProfitGold { get; }

        /// <summary>
        /// ,损失{0}金币
        /// </summary>
        string St6506_LossGold { get; }

        /// <summary>
        ///  您已下注{0}场，总额{1}金币，获利{2}金币
        /// </summary>
        string St6506_StakeDesc { get; }

        /// <summary>
        /// 跨服战下注获利{0}W
        /// </summary>
        string St6501_StakePrizeWin { get; }

        /// <summary>
        /// 跨服战下注返还金币{0}W
        /// </summary>
        string St6501_StakePrizeLost { get; }

        /// <summary>
        ///  暂无历史战绩
        /// </summary>
        string St6512_CombatNoHistoricalRecord { get; }

        /// <summary>
        ///  跨服战奖励金币{0}W!
        /// </summary>
        string St65010_CombatPrizeGameCoins { get; }

        /// <summary>
        ///  跨服战奖励声望{0}!
        /// </summary>
        string St65010_CombatPrizeObtainNum { get; }

        /// <summary>
        ///  跨服战奖励:{0}
        /// </summary>
        string St65010_CombatPrize { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_ServerCombatStage1 { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_ServerCombatStage2 { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_ServerCombatStage3 { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_ServerCombatStage4 { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_ServerCombatStage5 { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_ServerCombatStage6 { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_ServerCombatCombatType1 { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_ServerCombatCombatType2 { get; }

        /// <summary>
        /// 跨服战阶段
        /// </summary>
        string St_RoundNum { get; }

        /// <summary>
        /// 商店系统 - 背包已满无法进行购买
        /// </summary>
        string St7004_BeiBaoTimeOut { get; }

        /// <summary>
        /// 商店系统 - 奇石不足
        /// </summary>
        string St7004_QiShiNotEnough { get; }

        /// <summary>
        /// 黑市商店系统 - 此物品已购买
        /// </summary>
        string St7005_HavePurchasedItem { get; }

        /// <summary>
        /// 商店系统 - 该装备上已镶嵌灵件，确定出售吗？
        /// </summary>
        string St7006_UserItemHaveSpare { get; }

        /// <summary>
        /// 商店系统 - 神秘商店刷新花费晶石数
        /// </summary>
        string St7007_UseSparRefreshGold { get; }

        /// <summary>
        /// 礼包已领取
        /// </summary>
        string St9003_AlreadyReceived { get; }

        /// <summary>
        /// 不存在该用户
        /// </summary>
        string St9103_DoesNotExistTheUser { get; }

        /// <summary>
        /// 该用户已在好友中
        /// </summary>
        string St9103_TheUserHasAFriendIn { get; }

        /// <summary>
        /// 该用户已在粉丝中
        /// </summary>
        string St9103_TheUserHasTheFansIn { get; }


        /// <summary>
        /// 该用户已在关注中
        /// </summary>
        string St9103_TheUserHasTheAttentIn { get; }


        /// <summary>
        /// 该用户已在黑名单中
        /// </summary>
        string St9103_TheUserHasTheBlacklist { get; }

        /// <summary>
        /// 已达好友上限
        /// </summary>
        string St9103_TheMaximumReachedAFriend { get; }

        /// <summary>
        /// 不存在该玩家
        /// </summary>
        string St9103_NotFriendsUserID { get; }

        /// <summary>
        /// 聊天内容不能为空
        /// </summary>
        string St9201_contentNotEmpty { get; }


        /// <summary>
        /// 您当前背包中没有千里传音！亲，您可以到商城购买哦！
        /// </summary>
        string St9203_ItemEmpty { get; }


        /// <summary>
        /// 输入文字过长
        /// </summary>
        string St9201_TheInputTextTooLong { get; }

        /// <summary>
        /// 不能频繁发送聊天内容
        /// </summary>
        string St9203_ChatNotSend { get; }


        /// <summary>
        /// 未加入公会，不能发言
        /// </summary>
        string St9203_ChaTypeNotGuildMember { get; }

        /// <summary>
        /// 庄园种植未开启
        /// </summary>
        string St10001_ManorPlantingNotOpen { get; }

        /// <summary>
        /// 圣水不足
        /// </summary>
        string St10004_DewNotEnough { get; }

        /// <summary>
        /// 佣兵等级不能超过玩家等级
        /// </summary>
        string St10004_GeneralNotUserLv { get; }

        /// <summary>
        /// 庄园系统 - 是否花费{0}个晶石刷新
        /// </summary>
        string St10005_Refresh { get; }

        /// <summary>
        /// 庄园系统 - 当前为最高品质
        /// </summary>
        string St10005_MaxQualityType { get; }

        /// <summary>
        /// 恭喜您的佣兵{0}升至{1}级
        /// </summary>
        string St10006_UserGeneralUpLv { get; }


        /// <summary>
        /// 不存在该用户
        /// </summary>
        string St10006_DoesNotExistTheGeneral { get; }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石将冷却时间设为0!
        /// </summary>
        string St10007_DoRefresh { get; }

        /// <summary>
        /// 庄园系统 -土地已开启!
        /// </summary>
        string St10008_LandPostionIsOpen { get; }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石开启土地!
        /// </summary>
        string St10008_OpenLandPostion { get; }

        /// <summary>
        /// 庄园系统 - 圣水数量已满!
        /// </summary>
        string St10009_DewNumFull { get; }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石购买圣水!
        /// </summary>
        string St10009_PayDewUseGold { get; }

        /// <summary>
        /// VIP5级以上才能购买圣水!
        /// </summary>
        string St10009_NotPayDew { get; }

        /// <summary>
        /// 庄园系统 - 土地未全部开启，不能升级红土地!
        /// </summary>
        string St10010_LandNotEnough { get; }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石升级红土地!
        /// </summary>
        string St10010_UpRedLandUseGold { get; }

        /// <summary>
        /// 庄园系统 - 土地已升级!
        /// </summary>
        string St10010_UpRedLandNotEnough { get; }

        /// <summary>
        /// 庄园系统 - 已是红土地，不需升级!
        /// </summary>
        string St10010_RedLandFull { get; }

        /// <summary>
        /// 庄园系统 - 红土地未满，不能升级黑土地!
        /// </summary>
        string St10011_RedLandNotEnough { get; }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石升级黑土地!
        /// </summary>
        string St10011_UpBlackLandUseGold { get; }

        /// <summary>
        /// 庄园系统 - 不是红土地不能升级为黑土地!
        /// </summary>
        string St10011_NotRedLand { get; }

        /// <summary>
        /// 庄园系统 - 已是黑土地，不需升级!
        /// </summary>
        string St10011_BlackLandFull { get; }

        /// <summary>
        /// 每日探险答题冷却中！
        /// </summary>
        string St11002_Colding { get; }

        /// <summary>
        /// 您已经完成本日探险，请明天再来！
        /// </summary>
        string St11002_ExpeditionFull { get; }

        /// <summary>
        /// 庄园系统 - 是否花费{0}个晶石消除冷却时间
        /// </summary>
        string St11003_DelCodeTime { get; }

        /// <summary>
        /// 恭喜您获得父亲节奖励：30点精力，50点声望，20000金币，祝您游戏愉快！
        /// </summary>
        string St_FathersDay { get; }

        /// <summary>
        /// 恭喜您获得端午节奖励：20点精力，50点声望，200阅历，祝您游戏愉快！
        /// </summary>
        string St_DragonBoatFestival { get; }

        /// <summary>
        /// XXX无意中被不明物体砸中，定睛一看原来是白钻粽子，听说里面有传说中的紫色水晶，双手顿时颤抖不已。
        /// </summary>
        string St_DragonBoatZongzi { get; }

        /// <summary>
        /// 恭喜你获得端午节礼物XXX！
        /// </summary>
        string St_DragonBoatPuTongZongzi { get; }

        /// <summary>
        /// 恭喜您获得竞技场幸运数字七奖励：声望50、阅历200、金币50W！
        /// </summary>
        string St_HolidayFestival { get; }

        /// <summary>
        /// 恭喜你获得假日狂欢季礼物XXX！
        /// </summary>
        string St_HolidayFestivalGift { get; }

        /// <summary>
        /// XXX在村外闲逛，突然间眼前金光灿灿，顺手摸去，竟是一颗粉红水晶，里面包裹着沉甸甸的金子。于是高喊：发财啦,发财啦！
        /// </summary>
        string St_HolidayFestivalGoinGift { get; }


        /// <summary>
        /// 恭喜你获得活动奖励：40W金币、白奇石*4、神秘礼盒*1
        /// </summary>
        string St_SummerSecondNotice1 { get; }

        /// <summary>
        /// 恭喜你获得活动奖励：40W金币、绿奇石*4、神秘礼盒*1
        /// </summary>
        string St_SummerSecondNotice2 { get; }

        /// <summary>
        /// 恭喜你获得活动奖励：80W金币、绿奇石*4、神秘礼盒*1
        /// </summary>
        string St_SummerSecondNotice3 { get; }

        /// <summary>
        /// 声望第一上线公告
        /// </summary>
        string St_ObtionNumNotice { get; }

        /// <summary>
        /// 声望前三上线公告
        /// </summary>
        string St_ObtionTopThreeNotice { get; }

        /// <summary>
        /// 财富第一上线公告
        /// </summary>
        string St_GameCoinTopOneNotice { get; }

        /// <summary>
        /// 财富前三上线公告
        /// </summary>
        string St_GameCoinThreeNotice { get; }

        /// <summary>
        /// 财富前十上线公告
        /// </summary>
        string St_GameCoinTopTenNotice { get; }

        /// <summary>
        /// 战力第一上线公告
        /// </summary>
        string St_CombatNumTopOneNotice { get; }

        /// <summary>
        /// 战力前三上线公告
        /// </summary>
        string St_CombatNumTopThreeNotice { get; }

        /// <summary>
        /// 战力前十上线公告
        /// </summary>
        string St_CombatNumTopTenNotice { get; }

        /// <summary>
        /// 等级第一上线公告
        /// </summary>
        string St_LvTopTenNotice { get; }

        /// <summary>
        /// 精英副本奖励发聊天
        /// </summary>
        string St_PlotRewardNotice { get; }

        /// <summary>
        /// 恭喜您获得金币
        /// </summary>
        string St_SummerThreeGameCoinNotice { get; }

        /// <summary>
        /// 恭喜您获得声望
        /// </summary>
        string St_SummerThreeObtionNotice { get; }

        /// <summary>
        /// 恭喜您获得精力
        /// </summary>
        string St_SummerThreeEnergyNotice { get; }

        /// <summary>
        /// 恭喜您获得阅历
        /// </summary>
        string St_SummerThreeExpNumNotice { get; }

        /// <summary>
        /// 恭喜您获得晶石
        /// </summary>
        string St_SummerThreeGoldNotice { get; }

        /// <summary>
        /// 恭喜您获得经验
        /// </summary>
        string St_SummerThreeExperienceNotice { get; }

        /// <summary>
        /// 恭喜您获得物品
        /// </summary>
        string St_SummerThreeItemNotice { get; }

        /// <summary>
        /// 恭喜您获得水晶
        /// </summary>
        string St_SummerCrystalNotice { get; }

        /// <summary>
        /// 恭喜您，获得{0}礼包
        /// </summary>
        string St_SummerComradesItemNotice { get; }

        /// <summary>
        /// 天界大冲级-- 恭喜您，获得：xx、xx、xx,请继续努力！
        /// </summary>
        string St_SummerLeveling { get; }

        /// <summary>
        /// 金币*{0}
        /// </summary>
        string St_GameCoin { get; }

        /// <summary>
        /// 声望
        /// </summary>
        string St_ObtionNum { get; }

        /// <summary>
        /// 私聊通知- 宠物赛跑,您当前成功护送的{0}，获得金币{1}，声望{2}
        /// </summary>
        string Chat_PetRunSucess { get; }

        /// <summary>
        /// 私聊通知- 宠物拦截赛跑,{0}成功拦截{1}的{2}，获得金币{3}，声望{4}!
        /// </summary>
        string Chat_PetInterceptSucess { get; }

        /// <summary>
        /// 私聊通知- 您的宠物XX在半路被玩家XX拦截，受到了惊吓，损失金币XXX，声望XXX。
        /// </summary>
        string Chat_PetWasBlocked { get; }

        /// <summary>
        /// 恭喜你获得七夕节礼物XXX！
        /// </summary>
        string St_Tanabata { get; }
        /// <summary>
        /// 恭喜你获得七夕节礼物XXX！
        /// </summary>
        string St_UserNameTanabata { get; }
        /// <summary>
        /// 恭喜您获得七夕节奖励：70点精力、700点声望、70W金币，祝您游戏愉快，祝您游戏愉快！
        /// </summary>
        string St_TanabataLoginFestival { get; }

        /// <summary>
        /// 恭喜您获得{0}奖励：{1}，祝您游戏愉快，祝您游戏愉快！
        /// </summary>
        string St_FestivalRewardContent { get; }

        /// <summary>
        /// 活动奖励提示 --恭喜您，获得xx活动奖励：xx、xx、xx,请继续努力！
        /// </summary>
        string st_FestivalInfoReward { get; }

        /// <summary>
        /// 狂欢号外活动奖励---八月第二周奖励
        /// </summary>
        string St_AugustSecondWeek { get; }

        /// <summary>
        /// 精力
        /// </summary>
        string St_EnergyNum { get; }
        /// <summary>
        /// 阅历
        /// </summary>
        string St_ExpNum { get; }
        /// <summary>
        /// 晶石*{0}
        /// </summary>
        string St_GiftGoldNum { get; }
        /// <summary>
        /// 荣誉值
        /// </summary>
        string St_HonourNum { get; }
        /// <summary>
        /// 经验
        /// </summary>
        string St_Experience { get; }
        /// <summary>
        /// 物品{0}*{1}
        /// </summary>
        string St_Item { get; }
        /// <summary>
        ///  {0}*{1}
        /// </summary>
        string St_ItemReward { get; }
        /// <summary>
        /// {0}水晶{1}
        /// </summary>
        string St_Crystal { get; }

        /// <summary>
        /// {0}怪物卡*{1}
        /// </summary>
        string St_MonsterCard { get; }

        /// <summary>
        /// {0}佣兵魂魄*{1}
        /// </summary>
        string St_GeneralSoul { get; }

        /// <summary>
        /// {0}技能*{1}
        /// </summary>
        string St_Ability { get; }

        /// <summary>
        /// 活动期间变身卡使用无效
        /// </summary>
        string ZhongYuanHuodong { get; }

        /// <summary>
        /// 恭喜XX获得竞技场X连胜奖励：XX
        /// </summary>
        string SportVictoryReward { get; }
        /// <summary>
        /// 状态增幅药剂药性过于猛烈，不可连续食用！
        /// </summary>
        string St1608_CombatPowerNotEnough { get; }

        /// <summary>
        /// 爆炸发型，补丁衣裳，右手拿棍，左手拿碗，注定您要走运了，恭喜XX，获得：XX
        /// </summary>
        string St_SparePackNotice { get; }

        /// <summary>
        /// 被合成装备上有灵件，请先放入灵件背包
        /// </summary>
        string St_ItemEquIndexOfSpare { get; }

        /// <summary>
        /// XXX物品无法使用晶石代替，合成失败！
        /// </summary>
        string St_ItemIsGold { get; }

        /// <summary>
        /// 食物
        /// </summary>
        string GiftType_Food { get; }

        /// <summary>
        /// 厨具
        /// </summary>
        string GiftType_Kitchenware { get; }

        /// <summary>
        /// /机械
        /// </summary>
        string GiftType_Mechanical { get; }

        /// <summary>
        /// 书籍
        /// </summary>
        string GiftType_Books { get; }

        /// <summary>
        /// 乐器
        /// </summary>
        string GiftType_MusicalInstruments { get; }

        /// <summary>
        /// 感谢您重新返回游戏，更多精彩等着你。恭喜您获得老友礼包*1
        /// </summary>
        string OldFriendPack { get; }

        /// <summary>
        /// 恭喜您，获得{0}级拉新礼包一个！
        /// </summary>
        string NewHandPackage { get; }

        /// <summary>
        /// 恭喜您达到{0}级，获得金币{1}和两次邀请好友加入天界的机会，拉新卡号：{2}，可供两名20级以下的勇士与您一起分享多重惊喜！（注：新手勇士要在德亚兰的老村长处激活）
        /// </summary>
        string GainNewCard { get; }

        /// <summary>
        /// 您的拉新卡号无效，或角色等级不符，激活失败！
        /// </summary>
        string St1024_NewHandFail { get; }

        /// <summary>
        /// 拉新卡激活成功
        /// </summary>
        string St1024_NewHandSuccess { get; }

        /// <summary>
        /// 当前没有升级法宝物品
        /// </summary>
        string St1456_UpTrumpItemNotEnough { get; }

        /// <summary>
        /// 是否消耗一个延寿丹增加法宝寿命？
        /// </summary>
        string St1457_UseLifeExtension { get; }

        /// <summary>
        /// 您当前背包没有延寿丹，可以选择到商城购买！
        /// </summary>
        string St1457_LifeExtensionNotEnough { get; }

        /// <summary>
        /// 当前法宝未损耗寿命
        /// </summary>
        string St1457_MaxLifeExtension { get; }

        /// <summary>
        /// 是否消耗{0}个回天宝珠增加{1}点成长值（成功率{2}%）
        /// </summary>
        string St1458_UseBackDaysOrb { get; }

        /// <summary>
        /// 您当前没有没有足够的回天宝珠，可以选择到商城购买！
        /// </summary>
        string St1458_BackDaysOrbNotEnough { get; }

        /// <summary>
        /// 当前法宝成长值已达最高
        /// </summary>
        string St1458_MaxMatrueNumFull { get; }

        /// <summary>
        /// 洗练成功
        /// </summary>
        string St1458_XiLianSuccess { get; }

        /// <summary>
        /// 洗练失败
        /// </summary>
        string St1458_XiLianFail { get; }

        /// <summary>
        /// 确定要花费{0}晶石重新洗涤技能吗？
        /// </summary>
        string St1460_WashingSkills { get; }

        /// <summary>
        /// 法宝没有该技能
        /// </summary>
        string St1460_SkillsNotEnough { get; }

        /// <summary>
        /// 您当前晶石不足，无法洗涤！
        /// </summary>
        string St1460_WashingSkillsNotEnough { get; }

        /// <summary>
        /// 该技能已达最高等级
        /// </summary>
        string St1462_OutMaxTrumpLv { get; }

        /// <summary>
        /// 物品数量不足
        /// </summary>
        string St1462_ItemNumNotEnough { get; }

        /// <summary>
        /// 升级成功
        /// </summary>
        string St1464_UpgradeWasSsuccessful { get; }

        /// <summary>
        /// 当前没有空余凹槽！
        /// </summary>
        string St1466_WorshipPropertyNotEnough { get; }

        /// <summary>
        /// 当前没有属性类技能书，无法学习！
        /// </summary>
        string St1466_ItemPropertyNotEnough { get; }

        /// <summary>
        /// 该属性已存在
        /// </summary>
        string St1466_ItemPropertyExite { get; }

        /// <summary>
        /// 该属性已达最高等级
        /// </summary>
        string St1466_OutPropertyMaxLv { get; }

        /// <summary>
        /// 确定要删除当前凹槽技能吗？
        /// </summary>
        string St1467_WorshipGridNotEnough { get; }

        /// <summary>
        /// 是否花费{0}晶石随机改变法宝属相？
        /// </summary>
        string St1471_ChangeZodiac { get; }

        /// <summary>
        /// 延寿成功，增加{0}寿命
        /// </summary>
        string St1457_ChangeLifeNum { get; }
        /// <summary>
        /// 魂技等级已升到最高等级！
        /// </summary>
        string St1481_AbilityIsMaxLv { get; }
        /// <summary>
        /// 该魂技已装备在佣兵身上！
        /// </summary>
        string St1481_AbilityIsGeneral { get; }
        /// <summary>
        /// 被吞噬魂技卡与升级的魂技卡重复！
        /// </summary>
        string St1481_AbilityEcho { get; }
        /// <summary>
        /// 掉落附魔符
        /// </summary>
        string St4303_EnchantingCharacterFalling { get; }

        /// <summary>
        /// 法宝等级已达上限
        /// </summary>
        string St1456_OutTrumpMaxLv { get; }

        /// <summary>
        /// 随机1级附魔符
        /// </summary>
        string St4301_RandomEnchant { get; }

        /// <summary>
        /// 您的附魔符背包已满，无法获得附魔符，确定要进入副本吗？
        /// </summary>
        string St4002_EnchantPackageFull { get; }

        /// <summary>
        /// 洗涤成功
        /// </summary>
        string St1460_WashingSuccess { get; }

        /// <summary>
        /// 学习成功
        /// </summary>
        string St1466_LearningSuccess { get; }

        /// <summary>
        /// 祭祀成功
        /// </summary>
        string St1464_WorshipSuccess { get; }

        /// <summary>
        /// 祭祀失败
        /// </summary>
        string St1464_WorshipFail { get; }

        /****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
        /// <summary>
        /// 您当前背包剩余空间不足，请整理背包后重新领奖！
        /// </summary>
        string St1433_StoryTaskGridNotEnough { get; }

        /// <summary>
        /// 奖励已领取
        /// </summary>
        string St1433_RewardAlreadyReceive { get; }

        /// <summary>
        /// 恭喜您开启替补阵法，并获得替补佣兵礼包*1，您可以在阵法中设置替补出场的佣兵，享受非一般的战斗快感！
        /// </summary>
        string St1901_OpenGeneralReplace { get; }

        /// <summary>
        /// 该位置只能放置佣兵！
        /// </summary>
        string St1902_PostionNotGeneral { get; }

        /// <summary>
        /// 招募成功
        /// </summary>
        string St1434_RecruitmentErfolg { get; }

        /// <summary>
        /// 免费抽奖次数已用完
        /// </summary>
        string St12004_FreeNotEnough { get; }

        /// <summary>
        /// 免费抽奖次数未用完
        /// </summary>
        string St12004_FreeNumEnough { get; }

        /// <summary>
        /// 是否花费{0}晶石抽奖{1}次
        /// </summary>
        string St12004_SpendSparDraw { get; }

        /// <summary>
        /// 您在幸运转盘中获得:{0}
        /// </summary>
        string St12004_YouWheelOfFortune { get; }

        /// <summary>
        /// 奖励一次抽奖次数
        /// </summary>
        string St12004_RewardSweepstakes { get; }

        /// <summary>
        /// 没有获得任何奖励
        /// </summary>
        string St12004_DidNotAnyReward { get; }

        /// <summary>
        /// 下一次充值时，享受{0}的返还。
        /// </summary>
        string St12004_RechargeReturn { get; }

        /// <summary>
        /// 您在大转盘中抽到充值返还{0}奖励，返还{1}晶石,祝您游戏愉快！
        /// </summary>
        string St12004_RechargeReturnGoldNum { get; }

        /// <summary>
        /// 本周已报名
        /// </summary>
        string St6404_HaveSignedUp { get; }

        /// <summary>
        /// 公会城市争斗战战斗中，无法报名
        /// </summary>
        string St6404_CityABattleTime { get; }

        /// <summary>
        /// 普通会员没有权限
        /// </summary>
        string St6404_OrdinaryMemberNotCompetence { get; }

        /// <summary>
        /// 公会等级不足，无法报名
        /// </summary>
        string St6404_GuildLvNotEnough { get; }

        /// <summary>
        /// 公会技能点不足，无法报名
        /// </summary>
        string St6404_CurrDonateNumNotEnough { get; }

        /// <summary>
        /// 公会旗帜名称不能为空
        /// </summary>
        string St6405_GuildBannerNotEnough { get; }

        /// <summary>
        /// 公会未报名，不能设置旗帜名称
        /// </summary>
        string St6405_GuildNotEnterName { get; }

        /// <summary>
        /// 未报名城市争斗战，不能参战
        /// </summary>
        string St6412_HaveSignedUp { get; }

        /// <summary>
        /// 公会城市争斗战战斗中，不能参战
        /// </summary>
        string St6412_FightWarDate { get; }

        /// <summary>
        /// 恭喜您的公会在城市争夺战中获得第一名，所有参与成员获得{0}
        /// </summary>
        string St6404_GuildWarFirstPackID { get; }

        /// <summary>
        /// 恭喜您的公会在城市争夺战中获得第二名，所有参与成员获得{0}
        /// </summary>
        string St6404_GuildWarSecondPackID { get; }

        /// <summary>
        /// 恭喜您的公会在城市争夺战中获得第三名，所有参与成员获得{0}
        /// </summary>
        string St6404_GuildWarThirdPackID { get; }

        /// <summary>
        /// 恭喜您的公会在城市争夺战中获得名次，所有参与成员获得{0}
        /// </summary>
        string St6404_GuildWarParticipateID { get; }

        /// <summary>
        /// 城市争夺战已打响，是否立即加入战场为公会而战?
        /// </summary>
        string St6401_GuildFightBroadCas { get; }

        /// <summary>
        /// 报名成功
        /// </summary>
        string St6401_SuccessfulRegistration { get; }

        /// <summary>
        /// 设置旗帜成功
        /// </summary>
        string St6405_SettingTheBannerSuccess { get; }

        /// <summary>
        /// 参战成功
        /// </summary>
        string St6412_FightWarSuccess { get; }

        /// <summary>
        /// 报名时间已过，不能报名
        /// </summary>
        string St6404_OutRegistrationTime { get; }

        /// <summary>
        /// 冠军旗帜只能修改一次
        /// </summary>
        string St6413_HaveBeenModified { get; }

        /// <summary>
        /// 忽见{0}城风云变幻，原来是城主{1}光临本城！
        /// </summary>
        string St6413_SantoVisit { get; }

        /// <summary>
        /// 旗帜只能填写一个字符
        /// </summary>
        string St6405_FillInACharacter { get; }

        /// <summary>
        /// 公会城市争斗战退出失败
        /// </summary>
        string St6411_FailedToExit { get; }

        /// <summary>
        /// 当前疲劳值{0}，减少战斗力{1}%
        /// </summary>
        string St6409_fatigueDesc { get; }

        /// <summary>
        /// 恭喜您获得公会占领{0}城市的福利:{1}金币，{2}晶石，{3}阅历。祝您游戏愉快！
        /// </summary>
        string ChampionWelfare { get; }
        /// <summary>
        /// 背包格子已满
        /// </summary>
        string PackFull { get; }
        /// <summary>
        /// 装备格子已满
        /// </summary>
        string EquipFull { get; }
        /// <summary>
        /// 魂技格子已满
        /// </summary>
        string AbilityFull { get; }
        /// <summary>
        /// 装备格子已满
        /// </summary>
        string GeneralFull { get; }

        /// <summary>
        /// {0}不足
        /// </summary>
        string St12004_ChestKeyNotEnough { get; }

        /// <summary>
        /// 请选择佣兵升级
        /// </summary>
        string St1442_SelectMercenaryUpgrade { get; }

        /// <summary>
        /// 请选择经验卡
        /// </summary>
        string St1442_SelectTheExperienceCard { get; }

        /// <summary>
        /// 系统信件标题
        /// </summary>
        string St_SystemMailTitle { get; }

        /// <summary>
        /// vip晶石
        /// </summary>
        string St_PayGoldNum { get; }

        /// <summary>
        /// {0}（玩家名称）历经千辛万苦，层层阻碍，终于闯过{1}（城市名称）的所有副本，一颗希望之星朗朗升起！
        /// </summary>
        string St_systemprompts { get; }
        /// <summary>
        /// 福星那个高照呀！{0}在百里挑一中抽取到蓝色佣兵{1}（佣兵名称），此佣兵天赋异禀，日后定成大器！
        /// </summary>
        string St_UserGetGeneralQuality1 { get; }

        /// <summary>
        /// 福星那个高照呀！{0}在千载难逢中抽取到蓝色佣兵{1}（佣兵名称），此佣兵天赋异禀，日后定成大器！
        /// </summary>
        string St_UserGetGeneralQuality2 { get; }
        /// <summary>
        /// 福星那个高照呀！{0}在千载难逢中抽取到紫色佣兵{1}（佣兵名称），此佣兵天赋异禀，日后定成大器！
        /// </summary>
        string St_UserGetGeneralQuality3 { get; }

        /// <summary>
        /// 好友請求
        /// </summary>
        string St_AskFirendMailTitle { get; }

        /// <summary>
        /// 好友請求提示
        /// </summary>
        string St_AskFirendTip { get; }

        /// <summary>
        /// 好友請求提示
        /// </summary>
        string St_FirendNotice { get; }

        /// <summary>
        /// 好友请求提示
        /// </summary>
        string St_FirendNoticeTip { get; }

        /// <summary>
        /// XXX在竞技场中像会长您发起挑战，被您打的落荒而逃，您的排名保持不变!
        /// </summary>
        string SportsRankLetterForWin { get; }
        /// <summary>
        /// XXX在竞技场中像会长您发起挑战，您不幸落败，排名降低至N名！
        /// </summary>
        string SportsRankLetterForFailure { get; }
        /// <summary>
        /// XXX在竞技场中像会长您发起挑战，您不幸落败，排名保持不变！
        /// </summary>
        string SportsRankLetterForFailureRank { get; }

        /// <summary>
        /// 圣吉塔排名奖励提示:您在{0}的“勇闯圣吉塔”活动中名列{1}榜第{2}，排名奖励{3}金币已经发送到您的账号中，请及时查收！
        /// </summary>
        string St_ShengJiTaTip { get; }

        /// <summary>
        /// 青铜
        /// </summary>
        string St_ShengJiTaQintTong { get; }

        /// <summary>
        /// 白银
        /// </summary>
        string St_ShengJiTaBaiYin { get; }

        /// <summary>
        /// 黄金
        /// </summary>
        string St_ShengJiTaHuangJin { get; }

    }
}