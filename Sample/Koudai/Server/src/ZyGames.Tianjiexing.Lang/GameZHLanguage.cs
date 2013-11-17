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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Tianjiexing.Lang
{
    class GameZHLanguage : BaseZHLanguage, IGameLanguage
    {

        #region IGameLanguage 成员

        public string St1002_GetRegisterPassportIDError
        {
            get { return "获取注册通行证ID失败!"; }
        }

        public string UserInfoError
        {
            get { return "获取用户信息失败!"; }
        }

        public string LoadDataError
        {
            get { return "加载数据失败!"; }
        }

        public string FestivalDataFormat
        {
            get { return "MM月dd日"; }
        }

        public string ShortDataFormat
        {
            get { return "yyyy年MM月dd日"; }
        }

        public string DataFormat
        {
            get { return "yyyy年MM月dd日 HH时mm分ss秒"; }
        }

        public short shortInt
        {
            get { return 0; }
        }

        public int GameUserGeneralID
        {
            get { return 10000; }
        }

        public int SystemUserId
        {
            get { return 1000000; }
        }

        public string KingName
        {
            get { return "系统"; }
        }

        public int RechargeError
        {
            get { return 10; }
        }

        public string Color_Gray
        {
            get { return "灰色"; }
        }

        public string Color_Green
        {
            get { return "绿色"; }
        }

        public string Color_Blue
        {
            get { return "蓝色"; }
        }

        public string Color_PurPle
        {
            get { return "紫色"; }
        }

        public string Color_Yellow
        {
            get { return "黄色"; }
        }

        public string Color_Orange
        {
            get { return "橙色"; }
        }

        public string PaySuccessMsg
        {
            get { return "您成功充值，获得晶石{0}，祝您游戏愉快！"; }
        }

        public string Date_Yesterday
        {
            get { return "昨天"; }
        }

        public string Date_BeforeYesterday
        {
            get { return "前天"; }
        }

        public string Date_Day
        {
            get { return "{0}天前"; }
        }

        public string NotLogin
        {
            get { return "未登录"; }
        }

        public string GameMoney_Coin
        {
            get { return "金币"; }
        }

        public string GameMoney_Gold
        {
            get { return "晶石"; }
        }

        public string St_Combat_FistPrize { get { return "20000声望，10000万金币"; } }

        public string St_User_LiveMsg
        {
            get { return "您当前绷带已使用完，请及时补充！"; }
        }

        public string St_User_BeiBaoMsg
        {
            get { return "您的背包已满，无法获得任何物品，确定要进入副本吗？"; }
        }

        public string St_User_SpareBeiBaoMsg
        {
            get { return "您的灵件背包已满，无法获得任何物品，确定要进入副本吗"; }
        }

        public string Load_User_Error
        {
            get { return "加载玩家信息失败!"; }
        }

        public string CacheUser_AddUserToCacheError
        {
            get { return "增加玩家缓存信息失败!"; }
        }

        public string St_EnergyNotEnough
        {
            get { return "精力不足!"; }
        }

        public string St_GoldNotEnough
        {
            get { return "晶石不足!"; }
        }

        public string St_InspireFailed
        {
            get { return "鼓舞失败"; }
        }

        public string St_VipNotEnough
        {
            get { return "VIP等级不足!"; }
        }

        public string St_LevelNotEnough
        {
            get { return "等级不足!"; }
        }

        public string St_GameCoinNotEnough
        {
            get { return "金币数量不足!"; }
        }

        public string St_ExpNumNotEnough
        {
            get { return "阅历不足!"; }
        }

        public string St_ObtainNumNotEnough
        {
            get { return "声望不足!"; }
        }

        public string St_LingshiNumNotEnough
        {
            get { return "灵石不足!"; }
        }

        public string St_NoFun
        {
            get { return "功能未开启"; }
        }

        public string St_VipNotEnoughNotFuntion
        {
            get { return "VIP等级不足，未开启该功能!"; }
        }
        public string St1000_RegistrationNum
        {
            get { return "您已经成功签到{0}次，完成了本月的微信签到活动，感谢您对掌游科技的支持！下个月可以继续参与哦"; }
        }
        public string St1000_IsRegistration
        {
            get { return "您今天已经签到成功了，请明天再来签到吧！"; }
        }
        public string St1000_UserExistent
        {
            get { return "帐号不存在或者您尚未绑定帐号，签到失败！"; }
        }
        public string St1000_GetRegistrationGold
        {
            get { return "本次签到成功！恭喜您获得：晶石*{0}！"; }
        }
        public string St1004_PasswordMistake
        {
            get { return "您输入的账号或密码不正确!"; }
        }

        public string St1004_PasswordError
        {
            get { return "您输入的密码有误!"; }
        }

        public string St1004_IDNoLogin
        {
            get { return "您的账号未登录或已过期!"; }
        }

        public string St1004_IDLogined
        {
            get { return "您的账号已在其它地方登录!"; }
        }

        /// <summary>
        /// 您的账号已被系统强制下线
        /// </summary>
        public string St1004_UserOffline { get { return "您的账号已被系统强制下线!"; } }

        public string St1004_UserIDError
        {
            get { return "账号ID获取失败!"; }
        }

        public string St1004_IDDisable
        {
            get { return "该账号已被封禁，登陆失败!"; }
        }

        public string St1005_RoleCheck
        {
            get { return "您尚未创建角色!"; }
        }

        public string St1005_RoleExist
        {
            get { return "您已创建了角色!"; }
        }

        public string St1005_Professional
        {
            get { return "职业未创建!"; }
        }

        public string St1005_KingNameNotEmpty
        {
            get { return "用户名称不能为空!"; }
        }

        public string St1005_PassportError
        {
            get { return "通行证帐号错误!"; }
        }

        public string St1005_KingNameTooLong
        {
            get { return "昵称应在1-{0}个字符!"; }
        }

        public string St1005_Rename
        {
            get { return "该名字已有玩家注册，请重新输入"; }
        }
        public string St1005_RegistNameExceptional
        {
            get { return "昵称存在特殊字符！"; }
        }
        public string St1005_RegistNameKeyWord
        {
            get { return "您输入的昵称包含敏感字，请重新输入！"; }
        }
        public string St1006_PasswordError
        {
            get { return "密码格式错误!"; }
        }

        public string St1006_ChangePasswordError
        {
            get { return "修改密码失败!"; }
        }

        public string St1006_PasswordTooLong
        {
            get { return "输入错误，请输入4-12位数字或字母!"; }
        }
        public string St1006_PasswordExceptional
        {
            get { return "密码不能包含特殊字符！"; }
        }
        /// <summary>
        /// 当前剩余经验加成次数
        /// </summary>
        public string St1008_ExpressSurplusNum
        {
            get { return "当前剩余经验加成次数{0}"; }
        }

        /// <summary>
        /// 当前剩余金币加成次数
        /// </summary>
        public string St1008_GameCoinSurplusNum
        {
            get { return "当前剩余金币加成次数{0}"; }
        }

        /// <summary>
        /// 当前剩余双倍材料掉落次数
        /// </summary>
        public string St1008_DoubleItemSurplusNum
        {
            get { return "当前剩余双倍材料掉落次数{0}"; }
        }

        /// <summary>
        /// 战力加成剩余时间
        /// </summary>
        public string St1008_CombatNumDate
        {
            get { return "战力加成剩余时间{0}秒"; }
        }

        /// <summary>
        /// 当前剩余血量
        /// </summary>
        public string St1008_BloodBagSurplusNum
        {
            get { return "当前剩余血量{0}"; }
        }

        /// <summary>
        /// 当前变身卡剩余时间
        /// </summary>
        public string St1008_TransfigurationDate
        {
            get { return "当前变身卡剩余时间{0}"; }
        }

        public string St1066_PayError
        {
            get { return "充值失败"; }
        }

        public string St1010_PayEnergyUseGold
        {
            get { return "是否花费{0}晶石购买{1}精力？"; }
        }

        public string St1010_JingliFull
        {
            get { return "今日精力购买次数已用完!"; }
        }

        public string St1013_JingliPrize
        {
            get { return "恭喜您获得{0}精力!"; }
        }

        public string St1013_DailyJingliPrize
        {
            get { return "恭喜您获得{0}精力!"; }
        }

        public string St1014_JingshiPrize
        {
            get { return "恭喜您获得{0}晶石，{1}金币!"; }
        }

        public string St1018_ExpObtainPrize
        {
            get { return "恭喜您获得{0}阅历，{0}声望!"; }
        }

        public string St1020_FengLu
        {
            get { return "恭喜您获得{0}俸禄!"; }
        }

        public string St1011_PayCoinUseGold
        {
            get { return "花费{0}晶石获得{1}金币，剩余{2}次！"; }
        }
        public string St1011_PayUseGold
        {
            get { return "进入金矿洞可以获得大量金币，是否花费{0}晶石获得{1}金币，今日剩余{2}次。"; }
        }
        public string St1011_PayGainCoin
        {
            get { return "恭喜您进入金矿洞获得{0}金币！"; }
        }

        public string St1011_WaJinKuangFull
        {
            get { return "今日挖金矿次数已用完！"; }
        }

        public string St1104_UseGold
        {
            get { return "是否花费{1}晶石开启{0}个背包格子？"; }
        }

        public string St1107_UserItemNotEnough
        {
            get { return "该物品已卖出"; }
        }

        public string St1107_WarehouseNumFull
        {
            get { return "仓库已满"; }
        }

        public string St1107_GridNumFull
        {
            get { return "背包已满"; }
        }

        public string St1108_WarehouseNumUseGold
        {
            get { return "是否花费{1}晶石开启{0}个格子？"; }
        }
        public string St1110_OverExpansion
        {
            get { return "当前扩容已达最大次数！"; }
        }

        public string St1213_GridNumFull
        {
            get { return "灵件背包已满"; }
        }

        /// <summary>
        /// 该位置已装备灵件
        /// </summary>
        public string St1213_GridPotionFull
        {
            get { return "该位置已装备灵件"; }
        }

        public string St1214_ResetUseGold
        {
            get { return "是否花费{0}晶石，{1}金币洗涤属性？"; }
        }

        public string St1214_ResetUseLingshi
        {
            get { return "是否花费{0}灵石，{1}金币洗涤属性？"; }
        }

        public string St1215_OpenGridNumUseGold
        {
            get { return "是否花费{1}晶石开启{0}个灵件格子？"; }
        }

        /// <summary>
        /// 开启的格子已被占满
        /// </summary>
        public string St1213_OpenNumNotEnough
        {
            get { return "开启的格子已被占满"; }
        }

        public string St1213_GridNumNotEnough
        {
            get { return "格子未开启"; }
        }

        public string St1217_NotPotential
        {
            get { return "潜能点不足！"; }
        }

        public string St1217_NotItem
        {
            get { return "培养药剂不足！"; }
        }
        public string St1217_NotBaiJin
        {
            get { return "VIP3级以上才能白金培养！"; }
        }
        public string St1217_NotZhiZhun
        {
            get { return "VIP5级以上才能至尊培养！"; }
        }
        public string St4002_NotChallengeNum
        {
            get { return "您当天最大挑战次数已用完!"; }
        }
        public string St4002_IsPlotNum
        {
            get { return "您当天最大挑战次数已用完，是否花费{0}晶石增加一次挑战次数（VIP3开启）?"; }
        }
        public string St4002_IsPlotEliteNotChallengeNum
        {
            get { return "已达到每日挑战次数！"; }
        }
        public string St4004_NoUseMagic
        {
            get { return "您未启用魔法阵!"; }
        }

        public string St4004_EmbattleEmpty
        {
            get { return "魔法阵未设置佣兵"; }
        }

        public string St4004_GeneralLiveNotEnough
        {
            get { return "佣兵血量不足!"; }
        }

        public string St4011_NoMonster
        {
            get { return "副本:{0}找不到怪物:{1}"; }
        }

        public string St1203_CareerError
        {
            get { return "佣兵职业不符!"; }
        }

        public string St1204_Message
        {
            get { return "装备强化{0}!"; }
        }

        public string St1204_ColdTime
        {
            get { return "装备强化冷却中!"; }
        }

        public string St1204_Colding
        {
            get { return "装备强化冷却中!"; }
        }

        public string St1216_EnableSpartProperty
        {
            get { return "是否花费{0}晶石开启灵件第{1}个属性？"; }
        }

        public string St1256_EnchantNotEnough
        {
            get { return "您当前背包没有附魔符！"; }
        }

        public string St1256_EnchantNumNotEnough
        {
            get { return "没有足够的附魔符"; }
        }

        public string St1256_OutMaxEnchantLv
        {
            get { return "附魔符已达最高级，无法继续合成！"; }
        }

        public string St1258_OutMaxEnchantMature
        {
            get { return "附魔符培养已达上限！"; }
        }

        public string St1258_ConsumeMagicCrystalUpEnhance
        {
            get { return "培养成功，消耗{0}魔晶提升{1}成长值！"; }
        }

        public string St1258_ConsumeGoldNumUpEnhance
        {
            get { return "培养成功，消耗{0}晶石提升{1}成长值！"; }
        }

        public string St1258_EnhanceCultureFailedMagicCrystal
        {
            get { return "培养失败！消耗{0}魔晶！"; }
        }

        public string St1258_EnhanceCultureFailedGold
        {
            get { return "培养失败！消耗{0}晶石！"; }
        }

        public string St1258_MagicCrystalNotEnough
        {
            get { return "魔晶不足，无法培养！"; }
        }

        public string St1259_EnchantOpenGridFull
        {
            get { return "该位置已装备附魔符！"; }
        }

        public string St1259_EnchantGridNumFull
        {
            get { return "附魔符背包已满！"; }
        }

        public string St1259_UserItemNotWuQi
        {
            get { return "当前装备不是武器！"; }
        }

        public string St1260_UseGoldOpenPackage
        {
            get { return " 您确定花费{0}晶石开启{1}个格子？"; }
        }

        public string St1261_EnchantEquipmentNotEnough
        {
            get { return "当前背包没有附魔符，无法装备！"; }
        }

        public string St1262_EnchantSynthesisNotEnough
        {
            get { return "当前背包没有可合成附魔符！"; }
        }

        public string St1305_FateBackpackFull
        {
            get { return "猎命空间已满"; }
        }

        public string St1305_BeiBaoBackpackFull
        {
            get { return "请清理背包，获得奖励！"; }
        }

        public string St1305_GainCrystalBackpack
        {
            get { return "恭喜您获得命运礼包！"; }
        }

        public string St1305_HuntingIDLight
        {
            get { return "当前人物已点亮"; }
        }

        public string St1305_HighQualityNotice
        {
            get { return "{0}人品大爆发，随手一摸发现竟然是{1}水晶{2}，看四下无人，赶紧藏入口袋！"; }
        }

        /// <summary>
        /// 天上掉馅饼了吗？玩家{0}伸手摸进宝箱，发现竟然是一个金光灿灿的{1}水晶{2}，元芳，此事你怎么看？
        /// </summary>
        public string St1305_GainQualityNotice
        {
            get { return "天上掉馅饼了吗？玩家{0}伸手摸进宝箱，发现竟然是一个金光灿灿的{1}水晶{2}，元芳，此事你怎么看？"; }
        }

        public string St1307_FateBackpackFull
        {
            get { return "命运水晶背包已满"; }
        }

        public string St1307_FateBackSpaceFull
        {
            get { return "命运背包空间不足"; }
        }

        public string St1308_CrystalNotEnough
        {
            get { return "水晶栏里没有水晶"; }
        }

        public string St1308_CrystalLvFull
        {
            get { return "此水晶已达最高级"; }
        }

        public string St1309_OpenNumNotEnough
        {
            get { return "佣兵开启的格子已被占满"; }
        }

        public string St1309_TheSameFate
        {
            get { return "该佣兵已装备了相同属性的水晶"; }
        }

        /// <summary>
        /// 该格子已装备水晶
        /// </summary>
        public string St1309_TheGridFullSameFate
        {
            get { return "该格子已装备水晶"; }
        }

        public string St1204_EquMaxLv
        {
            get { return "强化等级已达上限"; }
        }

        public string St1204_EquGeneralMaxLv
        {
            get { return "强化等级不能超过佣兵等级!"; }
        }

        public string St1310_UseCrystalGold { get { return "是否花费{1}晶石开启{0}个格子？"; } }

        public string St1404_RecruitNotFilter { get { return "未达到招募条件，不可招募!"; } }
        /// <summary>
        /// 佣兵数已满，不可邀请
        /// </summary>
        public string St1404_MaxGeneralNumFull { get { return "佣兵数已满，不可邀请"; } }

        /// <summary>
        /// 玩家角色不能离队
        /// </summary>
        public string St1405_LiDuiNotFilter { get { return "玩家角色不能离队"; } }

        /// <summary>
        /// 该品级药剂已服满
        /// </summary>
        public string St1407_MedicineNumFull { get { return "该品级药剂已服满"; } }

        /// <summary>
        /// 背包不存在该药剂
        /// </summary>
        public string St1407_MedicineNum { get { return "背包不存在该药剂"; } }

        /// <summary>
        /// 直接服用药剂需{0}晶石
        /// </summary>
        public string St1407_MedicineUseGold { get { return "是否花费{0}晶石直接使用药剂？"; } }

        /// <summary>
        /// 属性培养已达上限
        /// </summary>
        public string St1409_maxTrainingNum { get { return "属性培养已达上限"; } }

        /// <summary>
        /// 玩家没有此佣兵
        /// </summary>
        public string St1405_GeneralIDNotEnough { get { return "玩家没有此佣兵"; } }

        /// <summary>
        /// 上线未满半小时，不能修炼
        /// </summary>
        public string St1411_LessThanHalfAnHour { get { return "上线未满半小时，不能修炼"; } }

        /// <summary>
        /// 不存在该药剂
        /// </summary>
        public string St1415_MedicineNum { get { return "奇幻粉末不足"; } }

        public string St1415_GridNumNotEnough
        {
            get { return "背包已满，请清理背包后重新摘取"; }
        }

        /// <summary>
        /// 摘取{0}需消耗{1}个奇幻粉末，是否要摘取？摘取成功率为{2}%
        /// </summary>
        public string St11415_ClearMedicine { get { return "摘取{0}需消耗{1}个奇幻粉末，是否要摘取？| 摘取成功率为{2}%"; } }

        /// <summary>
        /// 摘取失败
        /// </summary>
        public string St11415_Clearfail { get { return "摘取失败"; } }

        /// <summary>
        /// 被传承佣兵等级低于传承佣兵，无法传承
        /// </summary>
        public string St1418_HeritageLvLow
        {
            get { return "被传承佣兵等级低于传承佣兵，无法传承"; }
        }

        /// <summary>
        /// 请选择传承佣兵
        /// </summary>
        public string St1418_HeritageNotEnough
        {
            get { return "请选择传承佣兵"; }
        }

        /// <summary>
        /// 请选择被传承佣兵
        /// </summary>
        public string St1419_IsHeritageNotEnough
        {
            get { return "请选择被传承佣兵"; }
        }

        /// <summary>
        /// 传承佣兵和被传承佣兵不能是同一人
        /// </summary>
        public string St1419_HeritageNotInIsHeritage
        {
            get { return "传承佣兵和被传承佣兵不能是同一人"; }
        }

        public string St1419_DanInsufficientHeritage
        {
            get { return "%s不足"; }
        }

        /// <summary>
        /// 此佣兵已传承过
        /// </summary>
        public string St1419_HeritageInUse
        {
            get { return "此佣兵已传承过"; }
        }

        public string St1419_HeritageSuccess
        {
            get { return "传承成功"; }
        }

        public string St1419_GoldHeritage
        {
            get { return "是否花费{0}晶石进行晶石传承"; }
        }

        public string St1419_ExtremeHeritage
        {
            get { return "是否花费%d晶石进行至尊传承"; }
        }

        public string St1422_PresentationUseGold
        {
            get { return "是否花费{0}晶石增加{1}好感度"; }
        }

        public string St1422_FeelMaxSatiationNum
        {
            get { return "当前饱食度已满，无法使用礼物"; }
        }

        public string St1422_PresentationGoldNum
        {
            get { return "今日晶石赠送次数已用完"; }
        }

        public string St1422_PresentationFeelNum
        {
            get { return "使用成功，增加好感度{0}"; }
        }

        public string St1422_MaxFeelFull
        {
            get { return "该佣兵好感度已达最高等级"; }
        }

        public string St1423_ClearCurrSatiation
        {
            get { return "是否花费1个{0}清除当前饱食度？"; }
        }

        public string St1423_UserItemNotEnough
        {
            get { return "背包不存在消除饱食度物品"; }
        }

        public string St1423_DragonHolyWater
        {
            get { return "该佣兵今日消除饱食度次数已用完"; }
        }

        public string St1484_OperateDefaultAbility
        {
            get { return "无法对默认附带魂技进行操作"; }
        }

        /// <summary>
        /// 魔术等级已达到最高级
        /// </summary>
        public string St1503_MaxMagicLv { get { return "魔术等级已达到最高级"; } }

        /// <summary>
        /// 魔术 - 等级不能超过玩家等级
        /// </summary>
        public string St1503_MagicLevel { get { return "魔术等级不能超过玩家等级"; } }

        /// <summary>
        /// 魔术 - 魔法阵等级不能超过需求等级
        /// </summary>
        public string St1503_MagicEmbattleLevel { get { return "魔法阵等级不能超过需求等级"; } }

        /// <summary>
        /// 魔术强化冷却中！
        /// </summary>
        public string St1503_MagicColding { get { return "魔术强化冷却中"; } }

        /// <summary>
        /// 阅历不足
        /// </summary>
        public string St1503_UpgradeExpNum { get { return "阅历不足"; } }

        /// <summary>
        /// 魔术不存在
        /// </summary>
        public string St1503_MagicIDNotEnough { get { return "魔术不存在"; } }

        /// <summary>
        /// 材料不足
        /// </summary>
        public string St1603_MaterialsNotEnough { get { return "材料不足"; } }

        /// <summary>
        /// 您当前缺少合成所需的装备，无法晶石合成
        /// </summary>
        public string St1603_EquNotEnough { get { return "您当前缺少合成所需的装备，无法晶石合成"; } }

        public string St1604_MaterialsCityID
        {
            get { return "您当前等级无法到达该副本"; }
        }

        /// <summary>
        /// 合成卷轴需{0}晶石
        /// </summary>
        public string St1603_SynthesisEnergyNum { get { return "是否花费{0}晶石直接进行制作？(注：部分物品无法使用晶石代替)"; } }

        /// <summary>
        /// 绷带已消耗是否补充 
        /// </summary>
        public string St1605_BandageNotEnough { get { return "绷带已消耗是否补充"; } }

        /// <summary>
        /// 绷带使用中 
        /// </summary>
        public string St1605_BandageUse { get { return "绷带使用中,无法继续使用。"; } }

        /// <summary>
        /// 是否使用2晶石打开道具商店购买绷带
        /// </summary>
        public string St1605_UseTwoGold { get { return "是否使用2晶石打开道具商店购买绷带？"; } }

        public string St1606_OpenPackLackItem
        {
            get { return "{0}不足，无法开启{1}！"; }
        }

        /// <summary>
        /// 背包格子不足
        /// </summary>
        public string St1606_GridNumNotEnough { get { return "背包格子不足"; } }

        /// <summary>
        /// 命格背包已满或背包格子不足
        /// </summary>
        public string St1606_BackpackFull { get { return "命运水晶背包已满或背包格子不足"; } }

        /// <summary>
        /// 队列加速 - 花费晶石消除该冷却时间
        /// </summary>
        public string St1702_UseGold { get { return "是否花费{0}晶石消除冷却时间？"; } }

        /// <summary>
        /// 开启队列 - 花费晶石开启队列
        /// </summary>
        public string St1703_UseGold { get { return "是否花费{0}晶石开启队列？"; } }

        /// <summary>
        /// 开启队列 - 队列已全部开启
        /// </summary>
        public string St1703_QueueNumFull { get { return "队列已全部开启"; } }

        /// <summary>
        /// 主角无法下阵
        /// </summary>
        public string St1902_UserGeneralUnable { get { return "主角无法下阵"; } }

        /// <summary>
        /// 领土战参战状态不允许改变阵法！
        /// </summary>
        public string St1902_CountryCombatNotUpEmbattle
        {
            get { return "领土战参战状态不允许改变阵法！"; }
        }

        /// <summary>
        /// 加入国家提示莫根马/哈斯德尔
        /// </summary>
        public string St2004_CountryName { get { return "恭喜您已加入{0}国家"; } }

        public string St2004_CountryM { get { return "莫根马"; } }
        public string St2004_CountryH { get { return "哈斯德尔"; } }

        /// <summary>
        /// 任务系统 - 等级不足，不能领取任务
        /// </summary>
        public string St3002_LvNotEnough { get { return "等级不足，不能领取任务!"; } }
        /// <summary>
        /// 任务系统 - 主线任务未完成
        /// </summary>
        public string St3002_MainNoCompleted { get { return "主线任务未完成!"; } }
        /// <summary>
        /// 任务系统 - "任务已完成，不能放弃
        /// </summary>
        public string St3002_Completed { get { return "任务已完成，不能放弃!"; } }
        /// <summary>
        /// 任务系统 - 未能找到任务
        /// </summary>
        public string St3002_NotFind { get { return "未能找到任务!"; } }
        /// <summary>
        /// 任务系统 - 任务不能领取
        /// </summary>
        public string St3002_NoAllowTaked { get { return "任务不能领取!"; } }
        /// <summary>
        /// 任务系统 - 任务未领取
        /// </summary>
        public string St3002_NoTaked { get { return "任务未领取!"; } }

        /// <summary>
        /// 任务系统 -刷新任务星级需{0}晶石
        /// </summary>
        public string St3005_RefreashUseGold { get { return "刷新任务星级需{0}晶石!"; } }

        /// <summary>
        /// 任务系统 -您当前的任务星级已满，无法刷新
        /// </summary>
        public string St3005_RefreashStarFull { get { return "您当前的任务星级已满，无法刷新"; } }

        /// <summary>
        /// 任务系统 -直接完成任务需{0}晶石
        /// </summary>
        public string St3005_CompletedUseGold { get { return "是否花费{0}晶石直接完成任务？"; } }

        /// <summary>
        /// 任务系统 - 任务已达到完成次数
        /// </summary>
        public string St3005_CompletedTimeout { get { return "任务已达到完成次数!"; } }
        /// <summary>
        /// 任务系统 - 任务未完成
        /// </summary>
        public string St3007_NoCompleted { get { return "任务未完成!"; } }

        /// <summary>
        /// 您的晶石不足，无法召唤
        /// </summary>
        public string St3203_GoldNotEnouht { get { return "您的晶石不足，无法召唤"; } }

        /// <summary>
        /// 您的VIP等級不足，無法召喚
        /// </summary>
        public string St3203_VipNotEnouht
        {
            get { return "您的VIP等级不足，无法召唤"; }
        }

        /// <summary>
        /// 是否花费{0}晶石刷新宠物！
        /// </summary>
        public string St3203_RefeshPet { get { return "是否花费{0}晶石刷新宠物！"; } }

        /// <summary>
        /// 是否花费{0}晶石直接召唤狮子！
        /// </summary>
        public string St3203_ZhaohuangPet { get { return "是否花费{0}晶石直接召唤狮子！"; } }

        /// <summary>
        /// 您的宠物已达到最高等级！
        /// </summary>
        public string St3203_MaxPet { get { return "您的宠物已达到最高等级！"; } }

        /// <summary>
        /// 您不能选择该宠物，宠物未开启！
        /// </summary>
        public string St3204_PetNoEnable { get { return "您不能选择该宠物，宠物未开启！"; } }

        /// <summary>
        /// 您的好友未通过邀请申请！
        /// </summary>
        public string St3204_PetYaoqingNoPass { get { return "您的好友未通过邀请申请！"; } }

        /// <summary>
        /// 您的宠物还在赛跑中，请等待！
        /// </summary>
        public string St3204_PetRunning { get { return "您的宠物还在赛跑中，请等待！"; } }

        /// <summary>
        /// 您今日宠物赛跑次数已用完！
        /// </summary>
        public string St3204_PetRunTimesOut { get { return "您今日宠物赛跑次数已用完！"; } }
        /// <summary>
        /// 您的好友今日护送宠物赛跑次数已用完！
        /// </summary>
        public string St3204_PetHelpeTimesOut { get { return "您的好友今日护送宠物赛跑次数已用完！"; } }

        /// <summary>
        /// 您不能拦截自己的宠物！
        /// </summary>
        public string St3206_PetInterceptError { get { return "您不能拦截自己的宠物！"; } }
        /// <summary>
        /// 您正在护送好友的宠物！
        /// </summary>
        public string St3206_PetFriendError { get { return "您正在护送好友的宠物！"; } }
        /// <summary>
        /// 您今日拦截宠物赛跑次数已用完！
        /// </summary>
        public string St3206_PetInterceptTimesOut { get { return "您今日拦截宠物赛跑次数已用完！"; } }
        /// <summary>
        /// 您拦截的宠物已经赛跑完！
        /// </summary>
        public string St3206_PetInterceptFaild { get { return "您拦截的宠物已经赛跑完！"; } }

        /// <summary>
        /// 您已经拦截过该宠物，不可重复拦截
        /// </summary>
        public string St3206_PetInterceptFull
        {
            get { return "您已经拦截过该宠物，不可重复拦截"; }
        }
        /// <summary>
        /// 您今天已祈祷过
        /// </summary>
        public string St3302_IsPray
        {
            get { return "您今天已祈祷过！"; }
        }
        /// <summary>
        /// 副本系统 - 您的绷带已使用完，请及时补充。
        /// </summary>
        public string St4002_PromptBlood { get { return "您的绷带已使用完，请及时补充。"; } }

        /// <summary>
        /// 副本系统 -  背包中没有绷带，请及时购买。
        /// </summary>
        public string St4002_UserItemPromptBlood { get { return "背包中没有绷带，请及时购买"; } }

        /// <summary>
        /// 副本系统 - 今日精英副本次数已用完
        /// </summary>
        public string St4002_EliteUsed { get { return "今日该精英副本次数已用完！"; } }

        public string St4002_HeroPlotNum
        {
            get { return "今日该英雄副本次数已用完"; }
        }


        /// <summary>
        /// 副本系统 - 正在扫荡中
        /// </summary>
        public string St4007_Saodanging { get { return "副本正在扫荡中!"; } }
        /// <summary>
        /// 副本系统 - 扫荡已结束
        /// </summary>
        public string St4007_SaodangOver { get { return "副本扫荡已结束!"; } }
        /// <summary>
        /// 副本系统 - 背包已满无法进行扫荡
        /// </summary>
        public string St4007_BeiBaoTimeOut { get { return "背包已满无法进行扫荡!"; } }
        /// <summary>
        /// 副本系统 - 是否花费{0}个晶石直接完成扫荡
        /// </summary>
        public string St4008_Tip { get { return "是否花费{0}个晶石直接完成扫荡？"; } }

        /// <summary>
        /// 副本系统 - 花费{0}个晶石重置精英副本
        /// </summary>
        public string St4012_JingYingPlot { get { return "花费{0}个晶石重置精英副本？"; } }

        /// <summary>
        /// 副本系统 - 今日重置精英副本次数已用完
        /// </summary>
        public string St4012_JingYingPlotFull { get { return "今日重置精英副本次数已用完"; } }

        public string St4014_HeroRefreshPlot
        {
            get { return "花费{0}个晶石重置英雄副本"; }
        }

        public string St4014_HeroRefreshPlotFull
        {
            get { return "当前城市今日重置英雄副本次数已用完"; }
        }

        /// <summary>
        /// 多人副本已结束
        /// </summary>
        public string St4202_OutMorePlotDate { get { return "多人副本已结束"; } }

        /// <summary>
        /// 今日已打过此副本
        /// </summary>
        public string St4205_PlotNotEnough { get { return "今日已打过此副本"; } }

        /// <summary>
        /// 已在队伍中
        /// </summary>
        public string St4205_InTeam { get { return "已在队伍中！"; } }

        /// <summary>
        /// 队伍中人数已满
        /// </summary>
        public string St4206_TeamPeopleFull { get { return "队伍中人数已满！"; } }

        /// <summary>
        /// 没有可加入的队伍
        /// </summary>
        public string St4206_NoTeam { get { return "没有可加入的队伍！"; } }

        ///<summary>
        ///队伍已开始战斗
        ///</summary>
        public string St4206_TeamPlotStart { get { return "队伍已开始战斗！"; } }

        ///<summary>
        ///队伍已解散
        ///</summary>
        public string St4206_TeamPlotLead { get { return "队伍已解散！"; } }

        ///<summary>
        ///队伍正在战斗中
        ///</summary>
        public string St4208_IsCombating { get { return "队伍正在战斗中"; } }

        ///<summary>
        ///队伍人数不足
        ///</summary>
        public string St4210_PeopleNotEnough { get { return "队伍人数不足"; } }

        ///<summary>
        ///不存在此副本
        ///</summary>
        public string St4210_PlotNotEnough { get { return "不存在此副本"; } }

        ///<summary>
        /// 您本次多人副本挑战胜利，奖励{0}阅历，{1}*{2}！
        ///</summary>
        public string St4211_MorePlotReward { get { return "您本次多人副本挑战胜利，奖励{0}阅历，{1}*{2}！"; } }

        ///<summary>
        /// 未加入队伍
        ///</summary>
        public string St4211_UserNotAddTeam { get { return "未加入队伍"; } }

        ///<summary>
        ///天地劫今日已刷新
        ///</summary>
        public string St4302_PlotRefresh { get { return "天地劫今日已刷新"; } }

        ///<summary>
        ///天地劫灵件掉落
        ///</summary>
        public string St4303_SparePartFalling { get { return "灵件背包已满，掉落灵件{0}"; } }

        /// <summary>
        /// 此操作将花费您{0}晶石并回到本层第一关，确定执行此操作
        /// </summary>
        public string St4302_SecondRefreshKalpa { get { return "此操作将花费您{0}晶石并回到本层第一关，确定执行此操作"; } }

        /// <summary>
        /// 此操作将花费您{0}晶石并回到上一层第一关，确定执行此操作
        /// </summary>
        public string St4302_LastRefreshKalpa { get { return "此操作将花费您{0}晶石并回到上层第一关，确定执行此操作"; } }

        /// <summary>
        /// 您当前位置无需返回本层！
        /// </summary>
        public string St4302_LastRefreshKalpaNotEnough
        {
            get { return "您当前位置无需返回本层！"; }
        }


        public string St4303_PlotNotEnable { get { return "天地劫下一关暂未开启！"; } }

        /// <summary>
        /// 天地劫下一层暂未开启
        /// </summary>
        public string St4303_PlotNotEnableLayerNum
        {
            get { return "天地劫下一层暂未开启"; }
        }

        /// <summary>
        /// 您
        /// </summary>
        public string St5101_JingJiChangMingCheng { get { return "您"; } }

        /// <summary>
        /// 今日挑战次数已用完
        /// </summary>
        public string St5103_ChallengeNotNum { get { return "今日挑战次数已用完"; } }

        /// <summary>
        /// 挑战时间冷却中！
        /// </summary>
        public string St5107_Colding { get { return "挑战时间冷却中"; } }

        /// <summary>
        /// 竞技场排名奖励{0}积分，{1}金币！
        /// </summary>
        public string St5106_JingJiChangRankReward { get { return "竞技场排名奖励{0}积分，{1}金币！"; } }

        /// <summary>
        /// 今日挑战次数已用完！
        /// </summary>
        public string St5107_ChallGeNumFull { get { return "今日挑战次数已用完！"; } }

        /// <summary>
        /// XX打败了XX，登上排行版第一的至尊宝座
        /// </summary>
        public string St5107_JingJiChangOneRank { get { return "{0}打败了{1}，登上排行榜第一的至尊宝座"; } }

        /// <summary>
        /// XX排名连续上升了N名，已经势如破竹，不可阻挡了。
        /// </summary>
        public string St5107_JingJiChangMoreNum { get { return "{0}排名连续上升了{1}名，已经势如破竹，不可阻挡了"; } }

        /// <summary>
        /// {0}霸气外露，突破纪录达到{1}连杀
        /// </summary>
        public string St5107_JingJiChangWinNum { get { return "{0}霸气外露，突破纪录达到{1}连杀"; } }

        /// <summary>
        /// XX（玩家名称）打破了XX（玩家名称）的最高连杀纪录，已经无法阻挡了
        /// </summary>
        public string St5107_ZuiGaoLianSha { get { return "{0}打破了{1}的最高连杀纪录，已经无法阻挡了"; } }

        /// <summary>
        /// XX（玩家名称）达到N连胜，奖励金币N，晶石N
        /// </summary>
        public string St5107_ArenaWinsNum { get { return "{0}达到{1}连胜，奖励金币{2}，晶石{3}"; } }

        /// <summary>
        /// 您还未加入国家阵营
        /// </summary>
        public string St5201_NoJoinCountry { get { return "您还未加入国家阵营！"; } }
        /// <summary>
        /// 国家领土战未开始
        /// </summary>
        public string St5201_CombatNoStart { get { return "国家领土战未开始"; } }
        /// <summary>
        /// 国家领土战已结束
        /// </summary>
        public string St5201_CombatOver { get { return "国家领土战已结束"; } }

        /// <summary>
        /// 消耗200阅历有几率增加20%战斗力
        /// </summary>
        public string St5202_InspireTip { get { return "消耗{0}阅历有几率增加20%战斗力"; } }
        /// <summary>
        /// 消耗20晶石增加20%战斗力
        /// </summary>
        public string St5202_InspireGoldTip { get { return "消耗{0}晶石增加20%战斗力"; } }
        /// <summary>
        /// 生命不足请补充血量
        /// </summary>
        public string St5204_LifeNotEnough { get { return "生命不足请补充血量"; } }

        /// <summary>
        /// 挑战还未开始
        /// </summary>
        public string St5402_CombatNoStart { get { return "挑战未开始"; } }

        /// <summary>
        /// 您本次领土战中胜利{0}场，失败{1}场。总共获得{2}金币，{3}声望，下次继续努力！
        /// </summary>
        public string St5204_CombatTransfusion { get { return "您本次领土战中胜利{0}场，失败{1}场。总共获得{2}金币，{3}声望，下次继续努力！"; } }

        /// <summary>
        /// 挑战还未开始
        /// </summary>
        public string St5204_CombatNoStart { get { return "挑战未开始"; } }

        /// <summary>
        /// 您还未复活，请等待！
        /// </summary>
        public string St5402_IsReliveError { get { return "会长您还在复活等待时间中呀"; } }

        /// <summary>
        /// 是否消耗{0}晶石直接进入战斗
        /// </summary>
        public string St5403_CombatGoldTip { get { return "是否消耗{0}晶石直接进入战斗？"; } }

        /// <summary>
        /// 您已经复活了5次，不能再使用浴火重生
        /// </summary>
        public string St5403_IsReLiveMaxNum { get { return "亲爱的会长，您立即复活的次数已用完！"; } }
        /// <summary>
        /// 您已经复活，不需要使用浴火重生
        /// </summary>
        public string St5403_IsLive { get { return "会长您还是满血状态呀，不需要使用立即复活！"; } }

        /// <summary>
        /// 挑战还在初始化数据，请等待
        /// </summary>
        public string St5405_CombatWait { get { return "挑战还在初始化数据，请等待"; } }
        /// <summary>
        /// Boss已被击杀
        /// </summary>
        public string St5405_BossKilled { get { return "Boss已被击杀"; } }
        /// <summary>
        /// 挑战已结束
        /// </summary>
        public string St5405_CombatOver { get { return "挑战已结束"; } }

        /// <summary>
        /// {0}玩家获得Boss战击杀奖，奖励{1}金币
        /// </summary>
        public string St5405_CombatKillReward { get { return "{0}玩家获得Boss战击杀奖，奖励{1}金币"; } }

        /// <summary>
        /// 参加Boss战获得伤害奖励金币：{0}，声望：{1}
        /// </summary>
        public string St5405_CombatHarmReward { get { return "参加Boss战获得伤害奖励金币：{0}，阅历{1}"; } }

        /// <summary>
        /// {0}玩家获得Boss战第{1}名，奖励{2}声望{3}
        /// </summary>
        public string St5405_CombatRankmReward { get { return "{0}玩家获得Boss战第{1}名，奖励{2}金币{3}"; } }

        /// <summary>
        /// 物品与数量
        /// </summary>
        public string St5405_CombatNum { get { return "{0}*{1}"; } }

        /// <summary>
        /// 已是会员不能申请
        /// </summary>
        public string St6006_AlreadyMember { get { return "已是会员不能申请"; } }

        /// <summary>
        /// 申请公会中
        /// </summary>
        public string St6006_ApplyGuild { get { return "申请公会中"; } }

        /// <summary>
        /// 已达申请上限
        /// </summary>
        public string St6006_ApplyMaxGuild { get { return "已达申请上限"; } }

        /// <summary>
        /// 已申请该公会
        /// </summary>
        public string St6006_ApplyMember { get { return "已申请该公会"; } }


        /// <summary>
        /// 退出工会未满8小时
        /// </summary>
        public string St6006_GuildMemberNotDate { get { return "退出公会未满8小时,无法继续加入公会"; } }

        /// <summary>
        /// 普通成员没权限
        /// </summary>
        public string St6007_AuditPermissions { get { return "普通成员没权限"; } }

        /// <summary>
        /// 只有公会的会长和副会长才有权限使用该道具
        /// </summary>
        public string St6024_AuditPermissions { get { return "只有公会的会长和副会长才有权限使用该道具"; } }

        /// <summary>
        /// 该玩家不是会长没有权限
        /// </summary>
        public string St6008_NotChairman { get { return "该玩家不是会长没有权限"; } }

        /// <summary>
        /// 副会长人数已满
        /// </summary>
        public string St6008_VicePresidentNum { get { return "副会长人数已满"; } }

        /// <summary>
        /// 该会员不是副会长不能转让
        /// </summary>
        public string St6008_NotVicePresident { get { return "该会员不是副会长不能转让"; } }

        /// <summary>
        /// 该会员不是副会长不能撤销
        /// </summary>
        public string St6008_NotVicePresidentCeXiao { get { return "该会员不是副会长不能撤销"; } }

        /// <summary>
        /// 内容不能为空
        /// </summary>
        public string St6009_ContentNotEmpty { get { return "内容不能为空!"; } }

        /// <summary>
        /// 内容应在100个字以内
        /// </summary>
        public string St6009_ContentTooLong { get { return "内容应在100个字以内!"; } }

        /// <summary>
        /// 您当前为公会会长，无法退出公会
        /// </summary>
        public string St6010_Chairman { get { return "您当前为公会会长，无法退出公会"; } }

        /// <summary>
        /// 您不是该工会成员
        /// </summary>
        public string St6011_GuildMemberNotMember { get { return "您不是该公会成员"; } }

        /// <summary>
        /// 今日已上香
        /// </summary>
        public string St6012_HasIncenseToday { get { return "今日已上香"; } }

        /// <summary>
        /// 您成功进行七星朝圣，获得声望：+300
        /// </summary>
        public string St6013_GainObtionNum { get { return "您成功进行七星朝圣，获得声望：{0}"; } }


        /// <summary>
        /// 工会上香已满级
        /// </summary>
        public string St6012_GuildShangXiang { get { return "公会上香已满级"; } }

        /// <summary>
        /// 加入公会当天无法进行朝圣！
        /// </summary>
        public string St6014_GuildFirstDateNotDevilNum
        {
            get { return "加入公会当天无法进行朝圣！"; }
        }

        /// <summary>
        /// 是否花费{0}晶石召唤散仙封魔
        /// </summary>
        public string St6015_SummonSanxian { get { return "是否花费{0}晶石召唤散仙朝圣？"; } }


        /// <summary>
        /// 已是工会成员不能创建工会
        /// </summary>
        public string St6017_UnionMembers { get { return "您已经加入公会，不能再次创建公会"; } }

        /// <summary>
        /// 已存在该公会
        /// </summary>
        public string St6017_Rename { get { return "该名字已有公会命名，请重新输入"; } }

        /// <summary>
        /// 公会名字不能为空
        /// </summary>
        public string St6017_GuildNameNotEmpty { get { return "公会名称不能为空!"; } }

        /// <summary>
        /// 公会名称应在4-12个字符以内
        /// </summary>
        public string St6017_GuildNameTooLong { get { return "公会名称应在4-12个字符以内"; } }

        /// <summary>
        /// 名称已被使用
        /// </summary>
        public string St6017_GuildRename { get { return "名称已被使用!"; } }

        /// <summary>
        /// 公会人数已满
        /// </summary>
        public string St6019_GuildMaxPeople { get { return "公会人数已满!"; } }

        /// <summary>
        /// 已加入其他公会
        /// </summary>
        public string St6019_AddGuild { get { return "已加入其他公会"; } }

        /// <summary>
        ///小李飞刀进行七星朝圣还需要N人，公会成员可以前往协助。
        /// </summary>
        public string St6022_GuildConvene { get { return "{0}进行七星朝圣还需要{1}人，公会成员可以前往协助。"; } }


        /// <summary>
        /// 公会成员数量已达上限，无法继续使用道具
        /// </summary>
        public string St6024_GuildAddMemberToLong { get { return "公会成员数量已达上限，无法继续使用道具"; } }

        /// <summary>
        /// 本周公会BOSS未开始
        /// </summary>
        public string St6101_GuildBossNotOpen { get { return "本周公会BOSS未开始"; } }

        /// <summary>
        /// 本周公会BOSS已结束
        /// </summary>
        public string St6101_GuildBossOver { get { return "本周公会BOSS已结束"; } }

        /// <summary>
        /// 本周公会BOSS挑战时间未设置
        /// </summary>
        public string St6101_GuildBossSet { get { return "本周公会BOSS挑战时间未设置"; } }


        /// <summary>
        /// {0}玩家获得Boss战击杀奖，奖励{1}金币
        /// </summary>
        public string St6105_CombatKillReward { get { return "{0}玩家获得公会Boss战击杀奖，奖励{1}金币"; } }

        /// <summary>
        /// 参加Boss战获得伤害奖励金币：{0}，声望：{1}
        /// </summary>
        public string St6105_CombatHarmReward { get { return "参加公会Boss战获得伤害奖励金币：{0}，声望：{1}"; } }

        /// <summary>
        /// {0}玩家获得Boss战第{1}名，奖励{2}声望{3}
        /// </summary>
        public string St6105_CombatRankmReward { get { return "{0}玩家获得公会Boss战第{1}名，奖励{2}声望{3}"; } }


        /// <summary>
        /// 本周公会BOSS挑战时间已设置
        /// </summary>
        public string St6109_GuildBossTime { get { return "本周公会BOSS挑战时间已设置"; } }

        /// <summary>
        /// 您不是公会成员，请先加入公会
        /// </summary>
        public string St6203_GuildMemberNotEnough { get { return "您不是公会成员，请先加入公会"; } }

        /// <summary>
        /// 捐献N金币获得NN声望和NN贡献度
        /// </summary>
        public string St6204_GuildMemberGameCoinDonate { get { return "捐献{0}金币获得{1}声望和{2}贡献度"; } }

        /// <summary>
        /// 捐献N晶石获得NN声望和NN贡献度
        /// </summary>
        public string St6204_GuildMemberGoldDonate { get { return "捐献{0}晶石获得{1}声望和{2}贡献度"; } }

        /// <summary>
        /// 您输入的数值大于当日可捐献最大金额，请重新输入
        /// </summary>
        public string St6204_OutMaxGuildMemberDonate
        {
            get { return "您输入的数值大于当日可捐献最大金额，请重新输入"; }
        }

        /// <summary>
        /// 您输入的数值大于当日可捐献最大晶石，请重新输入
        /// </summary>
        public string St6204_OutMaxGuildMemberDonateGold { get { return "您输入的数值大于当日可捐献最大晶石，请重新输入"; } }

        /// <summary>
        /// 今日捐献数量已达上限
        /// </summary>
        public string St6204_OutMaxGuildMemberNum
        {
            get { return "今日捐献数量已达上限"; }
        }

        /// <summary>
        /// 請輸入分配金額
        /// </summary>
        public string St6204_GuildMemberDonateNum { get { return "请输入分配金额"; } }

        /// <summary>
        /// 请输入分配晶石
        /// </summary>
        public string St6204_GuildMemberDonateNumGold
        {
            get { return "请输入分配晶石"; }
        }

        /// <summary>
        ///公会技能点不足
        /// </summary>
        public string St6205_GuildMemberDonateNotEnough { get { return "公会技能点不足"; } }

        /// <summary>
        /// {0}技能升到{1}级
        /// </summary>
        public string St6205_GuildMemberJiNengShengJi
        {
            get { return "{0}技能升到{1}级"; }
        }

        /// <summary>
        /// 公会晨练活动还没有开始！
        /// </summary>
        public string St6301_GuildExerciseNoOpen { get { return "公会晨练活动还没有开始！"; } }

        /// <summary>
        /// 公会晨练活动已开始，现在不能参加！
        /// </summary>
        public string St6301_GuildExerciseIsOpen { get { return "公会晨练活动已开始，现在不能参加！"; } }

        /// <summary>
        /// 公会晨练活动已结束！
        /// </summary>
        public string St6301_GuildExerciseClose { get { return "公会晨练活动已结束！"; } }

        /// <summary>
        /// 全员正确，等级提升！
        /// </summary>
        public string St6303_GuildExerciseAllAnswerTrue { get { return "全员正确，等级提升！"; } }

        /// <summary>
        /// 您超过5道未答题，退出公会
        /// </summary>
        public string St6301_GuildExerciseTimeOut { get { return "您超过5道未答题，退出公会晨练！"; } }

        /// <summary>
        /// 未能全对，从头开始
        /// </summary>
        public string St6303_GuildExerciseAllAnswerFalse { get { return "未能全对，从头开始！"; } }

        /// <summary>
        /// 该问题已回答过了
        /// </summary>
        public string St6305_GuildExerciseISAnswer { get { return "该问题已回答过了！"; } }


        /// <summary>
        /// 是否花费{0}晶石自动答对此题
        /// </summary>
        public string St6305_GuildExerciseGoldAnswer { get { return "是否花费{0}晶石自动答对此题？"; } }


        /// <summary>
        /// 是否花费{0}晶石自动回答并答对所有题目
        /// </summary>
        public string St6305_GuildExerciseAutoAnswer { get { return "是否花费{0}晶石自动回答并答对所有题目？"; } }

        /// <summary>
        /// 回答正确，获得{0}经验和{1}阅历
        /// </summary>
        public string St6305_GuildExerciseAnswerSuss { get { return "回答正确，获得{0}阅历和{1}经验！"; } }

        /// <summary>
        /// 回答错误!
        /// </summary>
        public string St6305_GuildExerciseAnswerFail { get { return "回答错误!"; } }

        /// <summary>
        /// {0}太过注意路上的“风景”，以至答错了题目。
        /// </summary>
        public string St6305_GuildExerciseGuildChat { get { return "{0}太过注意路上的“风景”，以至答错了题目。"; } }

        /// <summary>
        /// 天下第一正在报名中，请各位勇士报名参加。
        /// </summary>
        public string St6501_ServerCombatBroadcas { get { return "天下第一大会正在报名中，请各位勇士报名参加。"; } }

        /// <summary>
        /// 天下第一大会即将开始，请参赛各勇士做好准备。
        /// </summary>
        public string St6501_SyncBroadcas { get { return "天下第一大会即将开始，请参赛各勇士做好准备!"; } }

        /// <summary>
        /// 今日天下第一火爆下注，请没有下注的勇士抓紧时间了，金钱不等人！
        /// </summary>
        public string St6501_ServerCombatStakeBroadcas { get { return "今日天下第一火爆下注，请没有下注的勇士抓紧时间了，金钱不等人！"; } }

        /// <summary>
        /// 天下第一尚未开启
        /// </summary>
        public string St6501_DoesNotStart { get { return "天下第一尚未开启"; } }

        /// <summary>
        /// 您已经报名了
        /// </summary>
        public string St6502_YouHavesignedup { get { return "您已经报名了"; } }

        /// <summary>
        /// 当前阶段不能报名！
        /// </summary>
        public string St6502_Notsignup { get { return "当前阶段不能报名！"; } }

        /// <summary>
        /// 还在报名阶段！
        /// </summary>
        public string St6503_AlsoInTheStage { get { return "还在报名阶段！"; } }

        /// <summary>
        /// 当前没有您的相关战绩！
        /// </summary>
        public string St6504_NotHaveInfo { get { return "当前没有您的相关战绩！"; } }

        /// <summary>
        /// 已下注{0}{1}W
        /// </summary>
        public string St6506_HasBet { get { return "已下注[{0}]{1}W"; } }

        /// <summary>
        /// 金币不足
        /// </summary>
        public string St6507_GoldCoinShortage { get { return "金币不足"; } }

        /// <summary>
        /// 您已下注
        /// </summary>
        public string St6507_YouFaveBet { get { return "当前阶段您已下注"; } }

        /// <summary>
        /// 下注[{0}]{1}金币
        /// </summary>
        public string St6506_BetGold { get { return "下注[{0}]{1}W金币"; } }

        /// <summary>
        /// ,等待结果……
        /// </summary>
        public string St6506_WaitResults { get { return ",等待结果……"; } }

        /// <summary>
        /// ,获利{0}金币!
        /// </summary>
        public string St6506_ProfitGold { get { return ",获利{0}W金币!"; } }

        /// <summary>
        /// ,损失{0}金币!
        /// </summary>
        public string St6506_LossGold { get { return ",损失{0}W金币!"; } }

        /// <summary>
        ///  您已下注{0}场，总额{1}金币，获利{2}金币
        /// </summary>
        public string St6506_StakeDesc { get { return "您已下注{0}场，总额{1}W金币，获利{2}W金币"; } }

        public string St6512_CombatNoHistoricalRecord
        {
            get { return "暂无历史战绩"; }
        }

        /// <summary>
        ///  天下第一奖励金币{0}W!
        /// </summary>
        public string St65010_CombatPrizeGameCoins { get { return "天下第一奖励金币{0}W"; } }

        /// <summary>
        ///  天下第一奖励声望{0}!
        /// </summary>
        public string St65010_CombatPrizeObtainNum { get { return "天下第一奖励声望{0}"; } }

        /// <summary>
        ///  跨服战奖励:{0}
        /// </summary>
        public string St65010_CombatPrize { get { return "天下第一奖励:"; } }

        /// <summary>
        /// 天下第一下注获利{0}W
        /// </summary>
        public string St6501_StakePrizeWin { get { return "天下第一下注获利{0}W金币"; } }

        /// <summary>
        /// 天下第一下注返还金币{0}W
        /// </summary>
        public string St6501_StakePrizeLost { get { return "天下第一下注返还{0}W金币"; } }

        /// <summary>
        /// 天下第一阶段
        /// </summary>
        public string St_ServerCombatStage1 { get { return "淘汰赛"; } }

        /// <summary>
        /// 天下第一阶段
        /// </summary>
        public string St_ServerCombatStage2 { get { return "32强赛"; } }

        /// <summary>
        /// 天下第一阶段
        /// </summary>
        public string St_ServerCombatStage3 { get { return "16强赛"; } }

        /// <summary>
        /// 天下第一阶段
        /// </summary>
        public string St_ServerCombatStage4 { get { return "8强赛"; } }

        /// <summary>
        /// 天下第一阶段
        /// </summary>
        public string St_ServerCombatStage5 { get { return "半决赛"; } }

        /// <summary>
        /// 天下第一阶段
        /// </summary>
        public string St_ServerCombatStage6 { get { return "决赛"; } }

        /// <summary>
        /// 天下第一阶段
        /// </summary>
        public string St_ServerCombatCombatType1 { get { return "天榜"; } }

        /// <summary>
        /// 天下第一阶段
        /// </summary>
        public string St_ServerCombatCombatType2 { get { return "人榜"; } }

        /// <summary>
        /// 第{0}轮
        /// </summary>
        public string St_RoundNum { get { return "第{0}轮"; } }

        /// <summary>
        /// 商店系统 - 背包已满无法进行购买
        /// </summary>
        public string St7004_BeiBaoTimeOut { get { return "背包已满无法进行购买!"; } }

        /// <summary>
        /// 商店系统 - 奇石不足
        /// </summary>
        public string St7004_QiShiNotEnough { get { return "奇石不足!"; } }

        /// <summary>
        /// 黑市商店系统 - 此物品已购买
        /// </summary>
        public string St7005_HavePurchasedItem
        {
            get { return "此物品已购买"; }
        }

        public string St7006_UserItemHaveSpare
        {
            get { return "请先卸下装备上的灵件再出售装备"; }
        }

        /// <summary>
        /// 商店系统 - 神秘商店刷新花费晶石数
        /// </summary>
        public string St7007_UseSparRefreshGold { get { return "是否花费{0}晶石刷新？"; } }

        /// <summary>
        /// 礼包已领取
        /// </summary>
        public string St9003_AlreadyReceived { get { return "礼包已领取"; } }

        /// <summary>
        /// 不存在该用户
        /// </summary>
        public string St9103_DoesNotExistTheUser { get { return "不存在该用户"; } }

        /// <summary>
        /// 该用户已在好友中
        /// </summary>
        public string St9103_TheUserHasAFriendIn { get { return "该用户已在好友中"; } }

        /// <summary>
        /// 该用户已在粉丝中
        /// </summary>
        public string St9103_TheUserHasTheFansIn { get { return "该用户已在粉丝中"; } }

        /// <summary>
        /// 该用户已在关注中
        /// </summary>
        public string St9103_TheUserHasTheAttentIn { get { return "该用户已在关注中"; } }

        /// <summary>
        /// 该用户已在黑名单中
        /// </summary>
        public string St9103_TheUserHasTheBlacklist { get { return "该用户已在黑名单中"; } }

        /// <summary>
        /// 已达好友上限
        /// </summary>
        public string St9103_TheMaximumReachedAFriend { get { return "已达好友上限"; } }

        /// <summary>
        /// 不存在该玩家
        /// </summary>
        public string St9103_NotFriendsUserID { get { return "不存在该玩家"; } }

        /// <summary>
        /// 聊天内容不能为空
        /// </summary>
        public string St9201_contentNotEmpty { get { return "聊天内容不能为空"; } }


        /// <summary>
        /// 您当前背包中没有千里传音！亲，您可以到商城购买哦！
        /// </summary>
        public string St9203_ItemEmpty { get { return "您当前背包中没有千里传音！亲，您可以到商城购买哦！"; } }


        /// <summary>
        /// 输入文字过长
        /// </summary>
        public string St9201_TheInputTextTooLong { get { return "输入文字过长"; } }

        /// <summary>
        /// 不能频繁发送聊天内容
        /// </summary>
        public string St9203_ChatNotSend { get { return "您的发言过于频繁，10秒后可继续发言"; } }


        /// <summary>
        /// 未加入公会，不能发言
        /// </summary>
        public string St9203_ChaTypeNotGuildMember { get { return "未加入公会，不能发言"; } }

        /// <summary>
        /// 庄园种植未开启
        /// </summary>
        public string St10001_ManorPlantingNotOpen { get { return "庄园种植未开启"; } }

        /// <summary>
        /// 圣水不足
        /// </summary>
        public string St10004_DewNotEnough { get { return "圣水不足"; } }

        /// <summary>
        /// 佣兵等级不能超过玩家等级
        /// </summary>
        public string St10004_GeneralNotUserLv { get { return "佣兵等级不能超过玩家等级"; } }

        /// <summary>
        /// 庄园系统 - 是否花费{0}个晶石刷新
        /// </summary>
        public string St10005_Refresh { get { return " 是否花费{0}个晶石刷新？"; } }

        /// <summary>
        /// 庄园系统 - 当前为最高品质
        /// </summary>
        public string St10005_MaxQualityType { get { return "当前为最高品质!"; } }

        /// <summary>
        /// 恭喜您的佣兵{0}升至{1}级
        /// </summary>
        public string St10006_UserGeneralUpLv { get { return "恭喜您的佣兵{0}升至{1}级。"; } }


        /// <summary>
        /// 不存在该用户
        /// </summary>
        public string St10006_DoesNotExistTheGeneral { get { return "不存在该佣兵"; } }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石将冷却时间设为0!
        /// </summary>
        public string St10007_DoRefresh { get { return " 是否花费{0}个晶石将冷却时间设为0？"; } }

        /// <summary>
        /// 庄园系统 -土地已开启!
        /// </summary>
        public string St10008_LandPostionIsOpen { get { return " 土地已开启"; } }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石开启土地!
        /// </summary>
        public string St10008_OpenLandPostion { get { return " 是否花费{0}个晶石开启土地？"; } }

        /// <summary>
        /// 庄园系统 - 圣水数量已满!
        /// </summary>
        public string St10009_DewNumFull { get { return " 圣水数量已满!"; } }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石购买圣水!
        /// </summary>
        public string St10009_PayDewUseGold { get { return " 是否花费{0}个晶石购买圣水？"; } }
        /// <summary>
        /// VIP3级以上才能购买圣水!
        /// </summary>
        public string St10009_NotPayDew { get { return " VIP5级以上才能购买圣水!"; } }

        /// <summary>
        /// 庄园系统 - 土地未全部开启，不能升级红土地!
        /// </summary>
        public string St10010_LandNotEnough { get { return " 土地未全部开启，不能升级红土地!"; } }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石升级红土地!
        /// </summary>
        public string St10010_UpRedLandUseGold { get { return " 是否花费{0}个晶石升级红土地？"; } }

        /// <summary>
        /// 庄园系统 - 土地已升级!
        /// </summary>
        public string St10010_UpRedLandNotEnough { get { return " 土地已升级！"; } }

        /// <summary>
        /// 庄园系统 - 已是红土地，不需升级!
        /// </summary>
        public string St10010_RedLandFull { get { return " 已是红土地，不需升级"; } }

        /// <summary>
        /// 庄园系统 - 红土地未满，不能升级黑土地!
        /// </summary>
        public string St10011_RedLandNotEnough { get { return "红土地未满，不能升级黑土地!"; } }

        /// <summary>
        /// 庄园系统 - 花费{0}个晶石升级黑土地!
        /// </summary>
        public string St10011_UpBlackLandUseGold { get { return " 是否花费{0}个晶石升级黑土地？"; } }

        /// <summary>
        /// 庄园系统 - 不是红土地不能升级为黑土地!
        /// </summary>
        public string St10011_NotRedLand { get { return " 不是红土地不能升级为黑土地!"; } }

        /// <summary>
        /// 庄园系统 - 已是黑土地，不需升级!
        /// </summary>
        public string St10011_BlackLandFull { get { return " 已是黑土地，不需升级"; } }

        /// <summary>
        /// 每日探险答题冷却中！
        /// </summary>
        public string St11002_Colding { get { return "每日探险答题冷却中"; } }

        /// <summary>
        /// 您已经完成本日探险，请明天再来！
        /// </summary>
        public string St11002_ExpeditionFull { get { return "您已经完成本日探险，请明天再来！"; } }

        /// <summary>
        /// 庄园系统 - 是否花费{0}个晶石消除冷却时间
        /// </summary>
        public string St11003_DelCodeTime { get { return " 是否花费{0}个晶石消除冷却时间？"; } }

        /// <summary>
        /// 恭喜您获得父亲节奖励：30点精力，50点声望，20000金币，祝您游戏愉快！
        /// </summary>
        public string St_FathersDay { get { return "恭喜您获得父亲节奖励：{0}点精力，{1}点声望，{2}金币，祝您游戏愉快"; } }

        /// <summary>
        /// 恭喜您获得端午节奖励：20点精力，50点声望，200阅历，祝您游戏愉快！
        /// </summary>
        public string St_DragonBoatFestival { get { return "恭喜您获得端午节奖励：20点精力，50点声望，200阅历，祝您游戏愉快"; } }

        /// <summary>
        /// XXX无意中被不明物体砸中，定睛一看原来是白钻粽子，听说里面有传说中的紫色水晶，双手顿时颤抖不已。
        /// </summary>
        public string St_DragonBoatZongzi { get { return "{0}无意中被不明物体砸中，定睛一看原来是白钻粽子，听说里面有传说中的紫色水晶，双手顿时颤抖不已。"; } }

        /// <summary>
        /// 恭喜你获得端午节礼物XXX！
        /// </summary>
        public string St_DragonBoatPuTongZongzi { get { return "恭喜你获得端午节礼物{0}！"; } }

        /// <summary>
        /// 恭喜您获得竞技场幸运数字七奖励：声望50、阅历200、金币50W！
        /// </summary>
        public string St_HolidayFestival { get { return "恭喜您获得竞技场幸运数字七奖励：阅历200、金币50W"; } }

        /// <summary>
        /// 恭喜你获得假日狂欢季礼物XXX！
        /// </summary>
        public string St_HolidayFestivalGift { get { return "恭喜你获得假日狂欢季礼物{0}！"; } }

        /// <summary>
        /// XXX在村外闲逛，突然间眼前金光灿灿，顺手摸去，竟是一颗粉红水晶，里面包裹着沉甸甸的金子。于是高喊：发财啦,发财啦！
        /// </summary>
        public string St_HolidayFestivalGoinGift { get { return "{0}在村外闲逛，突然间眼前金光灿灿，顺手摸去，竟是一颗粉红水晶，里面包裹着沉甸甸的金子。于是高喊：发财啦,发财啦！"; } }


        /// <summary>
        /// 恭喜你获得活动奖励：40W金币、白奇石*4、神秘礼盒*1
        /// </summary>
        public string St_SummerSecondNotice1 { get { return "恭喜你获得活动奖励40W金币、白奇石*4、神秘礼盒*1"; } }

        /// <summary>
        /// 恭喜你获得活动奖励：40W金币、绿奇石*4、神秘礼盒*1
        /// </summary>
        public string St_SummerSecondNotice2 { get { return "恭喜你获得活动奖励：40W金币、绿奇石*4、神秘礼盒*1"; } }

        /// <summary>
        /// 恭喜你获得活动奖励：80W金币、绿奇石*4、神秘礼盒*1
        /// </summary>
        public string St_SummerSecondNotice3 { get { return "恭喜你获得活动奖励：80W金币、绿奇石*4、神秘礼盒*1"; } }

        /// <summary>
        /// 声望第一上线公告
        /// </summary>
        public string St_ObtionNumNotice { get { return "忽见遥远的西方风雨巨变，仔细一看，原来是本服声望之王{0}闪亮登场！"; } }

        /// <summary>
        /// 声望前三上线公告
        /// </summary>
        public string St_ObtionTopThreeNotice { get { return "当当当，当当当，远方传来了几声巨响，原来是本服三大声望巨贾之一的{0}上线了！"; } }

        /// <summary>
        /// 财富第一上线公告
        /// </summary>
        public string St_GameCoinTopOneNotice { get { return "忽见远方金光灿灿，银光闪闪，原来是本服财富第一，富可敌国的{0}粉墨登场了！"; } }

        /// <summary>
        /// 财富前三上线公告
        /// </summary>
        public string St_GameCoinThreeNotice { get { return "忽如一夜金币来，金币银币掉下来，原来是本服三大富豪之一的{0}闪亮登场了！"; } }

        /// <summary>
        /// 财富前十上线公告
        /// </summary>
        public string St_GameCoinTopTenNotice { get { return "本服十大富豪之一的{0}隆重出场了！"; } }

        /// <summary>
        /// 战力第一上线公告
        /// </summary>
        public string St_CombatNumTopOneNotice { get { return "大地在颤抖，高山在摇晃，原来是本服的战斗力之王战神{0}登录游戏了！"; } }

        /// <summary>
        /// 战力前三上线公告
        /// </summary>
        public string St_CombatNumTopThreeNotice { get { return "本服三大战神之一的{0}闪亮登场了，闲杂人等注意避让！"; } }

        /// <summary>
        /// 战力前十上线公告
        /// </summary>
        public string St_CombatNumTopTenNotice { get { return "本服十大战将之一的{0}闪亮登场了，果然气势不凡！"; } }

        /// <summary>
        /// 等级第一上线公告
        /// </summary>
        public string St_LvTopTenNotice { get { return "本服第一等级达人{0}粉墨登场，气宇轩昂迷倒万千粉丝！"; } }

        /// <summary>
        /// 精英副本奖励发聊天
        /// </summary>
        public string St_PlotRewardNotice { get { return "{0}神勇无比打败{1}，获得{2}"; } }

        /// <summary>
        /// 恭喜您获得金币
        /// </summary>
        public string St_SummerThreeGameCoinNotice { get { return "恭喜您获得金币：{0}"; } }

        /// <summary>
        /// 恭喜您获得声望
        /// </summary>
        public string St_SummerThreeObtionNotice { get { return "恭喜您获得声望：{0}"; } }

        /// <summary>
        /// 恭喜您获得精力
        /// </summary>
        public string St_SummerThreeEnergyNotice { get { return "恭喜您获得精力：{0}"; } }

        /// <summary>
        /// 恭喜您获得阅历
        /// </summary>
        public string St_SummerThreeExpNumNotice { get { return "恭喜您获得阅历：{0}"; } }

        /// <summary>
        /// 恭喜您获得晶石
        /// </summary>
        public string St_SummerThreeGoldNotice { get { return "恭喜您获得晶石：{0}"; } }

        /// <summary>
        /// 恭喜您获得经验
        /// </summary>
        public string St_SummerThreeExperienceNotice { get { return "恭喜您获得经验：{0}"; } }

        /// <summary>
        /// 恭喜您获得物品
        /// </summary>
        public string St_SummerThreeItemNotice { get { return "恭喜您获得物品：{1}*{0}"; } }

        /// <summary>
        /// 恭喜您获得水晶
        /// </summary>
        public string St_SummerCrystalNotice { get { return "恭喜您获得水晶：{0}"; } }

        /// <summary>
        /// 恭喜您，获得{0}
        /// </summary>
        public string St_SummerComradesItemNotice { get { return "恭喜您，获得{0}"; } }

        /// <summary>
        /// 天界大冲级-- 恭喜您，获得：xx、xx、xx,请继续努力！
        /// </summary>
        public string St_SummerLeveling { get { return "恭喜您，获得：{0}请继续努力！"; } }

        /// <summary>
        /// 金币*{0}
        /// </summary>
        public string St_GameCoin { get { return "金币*{0}"; } }

        /// <summary>
        /// 声望
        /// </summary>
        public string St_ObtionNum { get { return "{0}声望"; } }



        /// <summary>
        /// 聊天通知- 宠物赛跑,您当前成功护送{0}，获得金币{1}，声望{2}
        /// </summary>
        public string Chat_PetRunSucess { get { return "您当前成功护送{0}，获得金币{1}，声望{2}!"; } }

        /// <summary>
        /// 聊天通知- 宠物拦截赛跑,{0}成功拦截{1}的{2}，获得金币{3}，声望{4}!
        /// </summary>
        public string Chat_PetInterceptSucess { get { return "{0}成功拦截{1}的{2}，获得金币{3}，声望{4}!"; } }

        /// <summary>
        /// 私聊通知- 您的宠物XX在半路被玩家XX拦截，受到了惊吓，损失金币XXX，声望XXX。
        /// </summary>
        public string Chat_PetWasBlocked
        {
            get { return "您的宠物{0}在半路被玩家{1}拦截，受到了惊吓，损失金币{2}，声望{3}。"; }
        }

        public string St_Tanabata { get { return "恭喜你获得七夕节礼物{0}！"; } }

        public string St_UserNameTanabata { get { return "恭喜{0}获得七夕节礼物{1}！"; } }

        public string St_TanabataLoginFestival { get { return "恭喜您获得七夕节奖励：70点精力、700点声望、70W金币，祝您游戏愉快"; } }

        /// <summary>
        /// 恭喜您获得{0}奖励：{1}，祝您游戏愉快，祝您游戏愉快！
        /// </summary>
        public string St_FestivalRewardContent
        {
            get { return "恭喜您获得{0}奖励：{1}，祝您游戏愉快，祝您游戏愉快！"; }
        }

        public string st_FestivalInfoReward { get { return "恭喜您获得{0}{1}，请继续努力！"; } }

        public string St_AugustSecondWeek { get { return "狂欢号外活动奖励"; } }

        public string St_EnergyNum { get { return "{0}精力"; } }
        public string St_ExpNum { get { return "{0}阅历"; } }
        public string St_GiftGoldNum { get { return "晶石*{0}"; } }

        public string St_HonourNum
        {
            get { return "{0}荣誉值"; }
        }

        public string St_Experience { get { return "{0}经验"; } }
        public string St_Item { get { return "{0}*{1}"; } }
        public string St_ItemReward { get { return "{0}*{1}"; } }
        public string St_Crystal { get { return "{0}水晶{1}"; } }
        public string St_MonsterCard { get { return "{0}*{1}"; } }
        public string St_GeneralSoul { get { return "{0}*{1}"; } }
        public string St_Ability { get { return "{0}*{1}"; } }

        public string ZhongYuanHuodong
        {
            get { return "活动期间变身卡使用无效"; }
        }

        /// <summary>
        ///  恭喜XX获得竞技场X连胜奖励：XX
        /// </summary>
        public string SportVictoryReward
        {
            get { return " 恭喜{0}获得竞技场{1}连胜奖励：{2}"; }
        }

        /// <summary>
        /// 状态增幅药剂药性过于猛烈，不可连续食用！
        /// </summary>
        public string St1608_CombatPowerNotEnough
        {
            get { return "状态增幅药剂药性过于猛烈，不可连续食用！"; }
        }

        /// <summary>
        /// 爆炸发型，补丁衣裳，右手拿棍，左手拿碗，注定您要走运了，恭喜XX，获得：XX
        /// </summary>
        public string St_SparePackNotice
        {
            get { return "爆炸发型，补丁衣裳，右手拿棍，左手拿碗，注定您要走运了，恭喜{0}，获得：{1}"; }
        }

        /// <summary>
        /// 被合成装备上有灵件，请先放入灵件背包
        /// </summary>
        public string St_ItemEquIndexOfSpare
        {
            get { return "被合成装备上有灵件，请先放入灵件背包"; }
        }
        /// <summary>
        /// XXX物品无法使用晶石代替，合成失败！
        /// </summary>
        public string St_ItemIsGold
        {
            get { return "{0}物品无法使用晶石代替，合成失败！"; }
        }

        public string GiftType_Food
        {
            get { return "食物"; }
        }

        public string GiftType_Kitchenware
        {
            get { return "厨具"; }
        }

        public string GiftType_Mechanical
        {
            get { return "机械"; }
        }

        public string GiftType_Books
        {
            get { return "书籍"; }
        }

        public string GiftType_MusicalInstruments
        {
            get { return "乐器"; }
        }

        public string OldFriendPack { get { return "感谢您重新返回游戏，更多精彩等着你。恭喜您获得老友礼包*1"; } }

        public string NewHandPackage
        {
            get { return "恭喜您，获得{0}级拉新礼包一个！"; }
        }

        public string GainNewCard
        {
            get { return "恭喜您达到{0}级，获得金币{1}和两次邀请好友加入天界的机会，拉新卡号：{2}，可供两名20级以下的勇士与您一起分享多重惊喜！（注：新手勇士要在德亚兰的老村长处激活）"; }
        }

        public string St1024_NewHandFail
        {
            get { return "您的拉新卡号无效，或角色等级不符，激活失败！"; }
        }

        public string St1024_NewHandSuccess
        {
            get { return "拉新卡激活成功"; }
        }

        public string St1456_UpTrumpItemNotEnough
        {
            get { return "您当前没有灵魂碎片，无法升级法宝！"; }
        }

        public string St1457_UseLifeExtension
        {
            get { return "是否消耗一个延寿丹增加法宝寿命？"; }
        }

        public string St1457_LifeExtensionNotEnough
        {
            get { return "您当前背包没有延寿丹，可以选择到商城购买！"; }
        }

        public string St1457_MaxLifeExtension
        {
            get { return "当前法宝寿命已满，无需使用延寿丹！"; }
        }

        public string St1458_UseBackDaysOrb
        {
            get { return "是否消耗{0}个梦幻宝珠增加{1}点成长值（成功率{2}%）"; }
        }

        public string St1458_MaxMatrueNumFull
        {
            get { return "当前法宝成长值已达最高！"; }
        }

        public string St1458_XiLianSuccess
        {
            get { return "洗练成功"; }
        }

        public string St1458_XiLianFail
        {
            get { return "洗练失败"; }
        }

        public string St1458_BackDaysOrbNotEnough
        {
            get { return "您当前没有足够的梦幻宝珠，可以选择到商城购买！"; }
        }

        public string St1460_WashingSkills
        {
            get { return "确定要花费{0}晶石重新洗涤技能吗？"; }
        }

        public string St1460_SkillsNotEnough
        {
            get { return "法宝没有该技能"; }
        }

        public string St1460_WashingSkillsNotEnough
        {
            get { return "您当前晶石不足，无法洗涤！"; }
        }

        public string St1462_OutMaxTrumpLv
        {
            get { return "该技能已达最高等级！"; }
        }

        public string St1462_ItemNumNotEnough
        {
            get { return " 物品数量不足！"; }
        }

        public string St1464_UpgradeWasSsuccessful
        {
            get { return "升级成功"; }
        }

        public string St1466_WorshipPropertyNotEnough
        {
            get { return "当前没有空余凹槽！"; }
        }

        public string St1466_ItemPropertyNotEnough
        {
            get { return "当前没有属性技能书，无法学习！"; }
        }

        public string St1466_ItemPropertyExite
        {
            get { return "当前属性已存在！"; }
        }

        public string St1466_OutPropertyMaxLv
        {
            get { return "当前属性已达最高等级！"; }
        }

        public string St1467_WorshipGridNotEnough
        {
            get { return "确定要删除当前凹槽技能吗？"; }
        }

        public string St1471_ChangeZodiac
        {
            get { return "是否花费{0}晶石改变法宝属相？"; }
        }

        public string St1457_ChangeLifeNum
        {
            get { return "延寿成功，增加{0}寿命"; }
        }
        public string St1481_AbilityIsMaxLv
        {
            get { return "魂技等级已升到最高等级！"; }
        }
        public string St1481_AbilityIsGeneral
        {
            get { return "该魂技已装备在佣兵身上！"; }
        }
        public string St1481_AbilityEcho
        {
            get { return "被吞噬魂技卡與升​​級的魂技卡重複！"; }
        }
        public string St4303_EnchantingCharacterFalling
        {
            get { return "掉落附魔符{0}"; }
        }

        public string St1456_OutTrumpMaxLv
        {
            get { return "法宝等级已达上限"; }
        }

        public string St4301_RandomEnchant
        {
            get { return "随机1级附魔符"; }
        }

        public string St4002_EnchantPackageFull
        {
            get { return "您的附魔符背包已满，无法获得附魔符，确定要进入副本吗？"; }
        }

        public string St1460_WashingSuccess
        {
            get { return "洗涤成功"; }
        }

        public string St1466_LearningSuccess
        {
            get { return "学习成功"; }
        }

        public string St1464_WorshipSuccess
        {
            get { return "祭祀成功"; }
        }

        public string St1464_WorshipFail
        {
            get { return "祭祀失败"; }
        }

        public string St1433_StoryTaskGridNotEnough
        {
            get { return "您当前背包剩余空间不足，请整理背包后重新领奖！"; }
        }

        public string St1433_RewardAlreadyReceive
        {
            get { return "奖励已领取"; }
        }

        public string St1901_OpenGeneralReplace
        {
            get { return "恭喜您开启替补阵法，并获得替补佣兵礼包*1，您可以在阵法中设置替补出场的佣兵，享受非一般的战斗快感！"; }
        }

        public string St1902_PostionNotGeneral
        {
            get { return "该位置只能放置佣兵！"; }
        }

        public string St1434_RecruitmentErfolg
        {
            get { return "招募成功！"; }
        }

        public string St12004_FreeNotEnough
        {
            get { return "免费抽奖次数已用完"; }
        }

        public string St12004_FreeNumEnough
        {
            get { return "免费抽奖次数未用完"; }
        }

        public string St12004_SpendSparDraw
        {
            get { return "是否花费{0}晶石抽奖{1}次"; }
        }

        public string St12004_YouWheelOfFortune
        {
            get { return "您在幸运转盘中获得:{0}"; }
        }

        public string St12004_RewardSweepstakes
        {
            get { return "奖励一次抽奖次数"; }
        }

        public string St12004_DidNotAnyReward
        {
            get { return "没有获得任何奖励"; }
        }

        public string St12004_RechargeReturn
        {
            get { return "下一次充值时，享受{0}的返还。"; }
        }

        public string St12004_RechargeReturnGoldNum
        {
            get { return "您在大转盘中抽到充值返还{0}奖励，返还{1}晶石,祝您游戏愉快！"; }
        }

        public string St6404_HaveSignedUp
        {
            get { return "本周已报名"; }
        }

        public string St6404_CityABattleTime
        {
            get { return "公会城市争斗战战斗中，无法报名"; }
        }

        public string St6404_OrdinaryMemberNotCompetence
        {
            get { return "普通会员没有权限"; }
        }

        public string St6404_GuildLvNotEnough
        {
            get { return "公会等级不足，无法报名"; }
        }

        public string St6404_CurrDonateNumNotEnough
        {
            get { return "公会技能点不足，无法报名"; }
        }

        public string St6405_GuildBannerNotEnough
        {
            get { return "公会旗帜名称不能为空"; }
        }

        public string St6405_GuildNotEnterName
        {
            get { return "公会未报名，不能设置旗帜名称"; }
        }

        public string St6412_HaveSignedUp
        {
            get { return "未报名城市争斗战，不能参战"; }
        }

        public string St6412_FightWarDate
        {
            get { return "公会城市争斗战战斗中，不能参战"; }
        }

        public string St6404_GuildWarFirstPackID
        {
            get { return "恭喜您的公会在城市争夺战中获得第一名，所有参与成员获得{0}"; }
        }

        public string St6404_GuildWarSecondPackID
        {
            get { return "恭喜您的公会在城市争夺战中获得第二名，所有参与成员获得{0}"; }
        }

        public string St6404_GuildWarThirdPackID
        {
            get { return "恭喜您的公会在城市争夺战中获得第三名，所有参与成员获得{0}"; }
        }

        public string St6404_GuildWarParticipateID
        {
            get { return "恭喜您的公会在城市争夺战中获得名次，所有参与成员获得{0}"; }
        }

        public string St6401_GuildFightBroadCas
        {
            get { return "城市争夺战已打响，是否立即加入战场为公会而战?"; }
        }

        public string St6401_SuccessfulRegistration
        {
            get { return "报名成功"; }
        }

        public string St6405_SettingTheBannerSuccess
        {
            get { return "设置旗帜成功"; }
        }

        public string St6412_FightWarSuccess
        {
            get { return "参战成功"; }
        }

        public string St6404_OutRegistrationTime
        {
            get { return "报名时间已过，不能报名"; }
        }

        public string St6413_HaveBeenModified
        {
            get { return "冠军旗帜只能修改一次"; }
        }

        public string St6413_SantoVisit
        {
            get { return "忽见{0}城风云变幻，原来是城主{1}光临本城！"; }
        }

        public string St6405_FillInACharacter
        {
            get { return "旗帜只能填写一个字符"; }
        }

        public string St6411_FailedToExit
        {
            get { return "公会城市争斗战退出失败"; }
        }

        public string St6409_fatigueDesc
        {
            get { return "当前疲劳值{0}，减少战斗力{1}%"; }
        }

        public string ChampionWelfare
        {
            get { return "恭喜您获得公会占领{0}城市的福利:{1}金币，{2}晶石，{3}阅历。祝您游戏愉快！"; }
        }
        public string PackFull
        {
            get { return "背包格子已满！"; }
        }
        public string EquipFull
        {
            get { return "装备格子已满！"; }
        }
        public string AbilityFull
        {
            get { return "魂技格子已满！"; }
        }
        public string GeneralFull
        {
            get { return "装备格子已满！"; }
        }

        public string St12004_ChestKeyNotEnough
        {
            get { return "{0}不足"; }
        }

        public string St1442_SelectMercenaryUpgrade
        {
            get { return "请选择佣兵升级"; }
        }

        public string St1442_SelectTheExperienceCard
        {
            get { return "请选择经验卡"; }
        }

        public string St_SystemMailTitle
        {
            get { return "系统"; }
        }

        public string St_PayGoldNum
        {
            get { return "vip晶石{0}"; }
        }

        public string St_systemprompts
        {
            get { return "{0}历经千辛万苦，层层阻碍，终于闯过{1}的所有副本，一颗希望之星朗朗升起！"; }
        }
        public string St_UserGetGeneralQuality1
        {
            get { return "福星那个高照呀！%s在百里挑一中抽取到蓝色佣兵%s ，此佣兵天赋异禀，日后定成大器！"; }
        }
        public string St_UserGetGeneralQuality2
        {
            get { return "福星那个高照呀！%s在千载难逢中抽取到蓝色佣兵%s ，此佣兵天赋异禀，日后定成大器！"; }
        }
        public string St_UserGetGeneralQuality3
        {
            get { return "福星那个高照呀！%s在千载难逢中抽取到紫色佣兵%s ，此佣兵天赋异禀，日后定成大器！"; }
        }

        public string St_AskFirendMailTitle
        {
            get { return "好友请求"; }
        }

        public string St_AskFirendTip
        {
            get { return "{0}决定与您义结金兰，从此以后双方共同进退，结伴同行！"; }
        }

        public string St_FirendNotice
        {
            get { return "{0}已同意与您义结金兰，从此你们将共同进退！"; }
        }
        public string St_FirendNoticeTip
        {
            get { return "您已发送请求！请等待回复！"; }
        }

        public string St_ShengJiTaTip
        {
            get { return "您在{0}的“勇闯圣吉塔”活动中名列{1}榜第{2}，排名奖励{3}金币已经发送到您的账号中，请及时查收！"; }
        }

        public string St_ShengJiTaQintTong
        {
            get { return "青铜"; }
        }

        public string St_ShengJiTaBaiYin
        {
            get { return "白银"; }
        }
        public string St_ShengJiTaHuangJin
        {
            get { return "黄金"; }

        }
        public string SportsRankLetterForWin { get { return "{0}在竞技场中向会长您发起挑战，被您打的落荒而逃，您的排名保持不变!"; } }

        public string SportsRankLetterForFailure { get { return "{0}在竞技场中向会长您发起挑战，您不幸落败，排名降低至{1}名！"; } }
        public string SportsRankLetterForFailureRank { get { return "{0}在竞技场中向会长您发起挑战，您不幸落败，排名保持不变！"; } }

        #endregion

        #region 随机取名

        public string[] St_FirstNickNames
        {
            get
            {
                return new string[]{
   "赵","钱","孙","李","周","吴","郑","王","冯","陈","褚","卫","蒋","沈","韩","杨","朱","秦","尤","许",
   "何","吕","施","张","孔","曹","严","华","金","魏","陶","姜","戚","谢","邹","喻","柏","水","窦","章","云","苏","潘","葛","奚","范","彭","郎",
   "鲁","韦","昌","马","苗","凤","花","方","俞","任","袁","柳","酆","鲍","史","唐","费","廉","岑","薛","雷","贺","倪","汤","滕","殷",
   "罗","毕","郝","邬","安","常","乐","于","时","傅","皮","卞","齐","康","伍","余","元","卜","顾","孟","平","黄","和",
   "穆","萧","尹","姚","邵","湛","汪","祁","毛","禹","狄","米","贝","明","臧","计","伏","成","戴","谈","宋","茅","庞","熊","纪","舒",
   "屈","项","祝","董","梁","杜","阮","蓝","闵","席","季","麻","强","贾","路","娄","危","江","童","颜","郭","梅","盛","林","刁","钟",
   "徐","邱","骆","高","夏","蔡","田","樊","胡","凌","霍","虞","万","支","柯","昝","管","卢","莫","经","房","裘","缪","干","解","应",
   "宗","丁","宣","贲","邓","郁","单","杭","洪","包","诸","左","石","崔","吉","钮","龚","程","嵇","邢","滑","裴","陆","荣","翁","荀",
   "羊","于","惠","甄","曲","家","封","芮","羿","储","靳","汲","邴","糜","松","井","段","富","巫","乌","焦","巴","弓","牧","隗","山",
   "谷","车","侯","宓","蓬","全","郗","班","仰","秋","仲","伊","宫","宁","仇","栾","暴","甘","钭","厉","戎","祖","武","符","刘","景",
   "詹","束","龙","叶","幸","司","韶","郜","黎","蓟","溥","印","宿","白","怀","蒲","邰","从","鄂","索","咸","籍","赖","卓","蔺","屠",
   "蒙","池","乔","阴","郁","胥","能","苍","双","闻","莘","党","翟","谭","贡","劳","逄","姬","申","扶","堵","冉","宰","郦","雍","却",
   "璩","桑","桂","濮","牛","寿","通","边","扈","燕","冀","浦","尚","农","温","别","庄","晏","柴","瞿","阎","充","慕","连","茹","习",
   "宦","艾","鱼","容","向","古","易","慎","戈","廖","庾","终","暨","居","衡","步","都","耿","满","弘","匡","国","文","寇","广","禄",
   "阙","东","欧","殳","沃","利","蔚","越","夔","隆","师","巩","厍","聂","晁","勾","敖","融","冷","訾","辛","阚","那","简","饶","空",
   "曾","毋","沙","乜","养","鞠","须","丰","巢","关","蒯","相","查","后","荆","红","游","郏","竺","权","逯","盖","益","桓","公","仉",
   "督","岳","帅","缑","亢","况","郈","有","琴","归","海","晋","楚","闫","法","汝","鄢","涂","钦","商","牟","佘","佴","伯","赏","墨",
   "哈","谯","篁","年","爱","阳","佟","言","福","南","火","铁","迟","漆","官","冼","真","展","繁","檀","祭","密","敬","揭","舜","楼",
   "疏","冒","浑","挚","胶","随","高","皋","原","种","练","弥","仓","眭","蹇","覃","阿","门","恽","来","綦","召","仪","风","介","巨",
   "木","京","狐","郇","虎","枚","抗","达","杞","苌","折","麦","庆","过","竹","端","鲜","皇","亓","老","是","秘","畅","邝","还","宾",
   "闾","辜","纵","侴","万俟","司马","上官","欧阳","夏侯","诸葛","闻人","东方","赫连","皇甫","羊舌","尉迟","公羊","澹台","公冶","宗正",
   "濮阳","淳于","单于","太叔","申屠","公孙","仲孙","轩辕","令狐","钟离","宇文","长孙","慕容","鲜于","闾丘","司徒","司空","兀官","司寇",
   "南门","呼延","子车","颛孙","端木","巫马","公西","漆雕","车正","壤驷","公良","拓跋","夹谷","宰父","谷梁","段干","百里","东郭","微生",
   "梁丘","左丘","东门","西门","南宫","第五","公仪","公乘","太史","仲长","叔孙","屈突","尔朱","东乡","相里","胡母","司城","张廖","雍门",
   "毋丘","贺兰","綦毋","屋庐","独孤","南郭","北宫","王孙","羽", "芳","月", "若", "叱咤","魔","幽","呂","仙"};
            }
        }

        public string[] St_LastNickNames
        {
            get
            {
                return new string[] { "伟", "刚", "勇", "毅", "俊", "峰", "强", "军", "平", "保", "东", "文", "辉", "力", "明", "永", "健", "世", "广", "志", "义", "兴", "良", "海", "山", "仁", "波", "宁", "贵", "福", "生", "龙", "元", "全", "国", "胜", "学", "祥", "才", "发", "武", "新", "利", "清", "飞", "彬", "富", "顺", "信", "子", "杰", "涛", "昌", "成", "康", "星", "光", "天", "达", "安", "岩", "中", "茂", "进", "林", "有", "坚", "和", "彪", "博", "诚", "先", "敬", "震", "振", "壮", "会", "思", "群", "豪", "心", "邦", "承", "乐", "绍", "功", "松", "善", "厚", "庆", "磊", "民", "友", "裕", "河", "哲", "江", "超", "浩", "亮", "政", "谦", "亨", "奇", "固", "之", "轮", "翰", "朗", "伯", "宏", "言", "若", "鸣", "朋", "斌", "梁", "栋", "维", "启", "克", "伦", "翔", "旭", "鹏", "泽", "晨", "辰", "士", "以", "建", "家", "致", "树", "炎", "德", "行", "时", "泰", "盛", "雄", "琛", "钧", "冠", "策", "腾", "楠", "榕", "风", "航", "弘", "秀", "娟", "英", "华", "慧", "巧", "美", "娜", "静", "淑", "惠", "珠", "翠", "雅", "芝", "玉", "萍", "红", "娥", "玲", "芬", "芳", "燕", "彩", "春", "菊", "兰", "凤", "洁", "梅", "琳", "素", "云", "莲", "真", "环", "雪", "荣", "爱", "妹", "霞", "香", "月", "莺", "媛", "艳", "瑞", "凡", "佳", "嘉", "琼", "勤", "珍", "贞", "莉", "桂", "娣", "叶", "璧", "璐", "娅", "琦", "晶", "妍", "茜", "秋", "珊", "莎", "锦", "黛", "青", "倩", "婷", "姣", "婉", "娴", "瑾", "颖", "露", "瑶", "怡", "婵", "雁", "蓓", "纨", "仪", "荷", "丹", "蓉", "眉", "君", "琴", "蕊", "薇", "菁", "梦", "岚", "苑", "婕", "馨", "瑗", "琰", "韵", "融", "园", "艺", "咏", "卿", "聪", "澜", "纯", "毓", "悦", "昭", "冰", "爽", "琬", "茗", "羽", "希", "欣", "飘", "育", "滢", "馥", "筠", "柔", "竹", "霭", "凝", "晓", "欢", "霄", "枫", "芸", "菲", "寒", "伊", "亚", "宜", "可", "姬", "舒", "影", "荔", "枝", "丽", "阳", "妮", "宝", "贝", "初", "程", "梵", "罡", "恒", "鸿", "桦", "骅", "剑", "娇", "纪", "宽", "苛", "灵", "玛", "媚", "琪", "晴", "容", "睿", "烁", "堂", "唯", "威", "韦", "雯", "苇", "萱", "阅", "彦", "宇", "雨", "洋", "忠", "宗", "曼", "紫", "逸", "贤", "蝶", "菡", "绿", "蓝", "儿", "翠", "烟", "小", "惜", "霸", "主", "郡", "魔", "幽", "多", "仙" };
            }
        }

        #endregion
    }
}