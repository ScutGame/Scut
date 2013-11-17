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
    class GameBig5Language : BaseBIG5Language, IGameLanguage
    {
        #region IGameLanguage 成員

        public string St1002_GetRegisterPassportIDError
        {
            get { return "獲取註冊通行證ID失敗!"; }
        }

        public string UserInfoError
        {
            get { return "獲取使用者資訊失敗!"; }
        }

        public string LoadDataError
        {
            get { return "載入數據失敗!"; }
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
            get { return "yyyy年MM月dd日 HH時mm分ss秒"; }
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
            get { return "系統"; }
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
            get { return "綠色"; }
        }

        public string Color_Blue
        {
            get { return "藍色"; }
        }

        public string Color_PurPle
        {
            get { return "紫色"; }
        }

        public string Color_Yellow
        {
            get { return "黃色"; }
        }

        public string Color_Orange
        {
            get { return "橙色"; }
        }

        public string PaySuccessMsg
        {
            get { return "您成功充值，獲得晶石{0}，祝您遊戲愉快！"; }
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
            get { return "未登錄"; }
        }

        public string GameMoney_Coin
        {
            get { return "金幣"; }
        }

        public string GameMoney_Gold
        {
            get { return "晶石"; }
        }

        public string St_Combat_FistPrize { get { return "20000聲望，10000萬金幣"; } }

        public string St_User_LiveMsg
        {
            get { return "您當前繃帶已使用完，請及時補充！"; }
        }

        public string St_User_BeiBaoMsg
        {
            get { return "您的背包已滿，無法獲得任何物品，確定要進入副本嗎？"; }
        }

        public string St_User_SpareBeiBaoMsg
        {
            get { return "您的靈件背包已滿，無法獲得任何物品，確定要進入副本嗎"; }
        }

        public string Load_User_Error
        {
            get { return "載入玩家資訊失敗!"; }
        }

        public string CacheUser_AddUserToCacheError
        {
            get { return "增加玩家緩存資訊失敗!"; }
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
            get { return "鼓舞失敗"; }
        }

        public string St_VipNotEnough
        {
            get { return "VIP等級不足!"; }
        }

        public string St_LevelNotEnough
        {
            get { return "等級不足!"; }
        }

        public string St_GameCoinNotEnough
        {
            get { return "金幣數量不足!"; }
        }

        public string St_ExpNumNotEnough
        {
            get { return "閱歷不足!"; }
        }

        public string St_ObtainNumNotEnough
        {
            get { return "聲望不足!"; }
        }

        public string St_LingshiNumNotEnough
        {
            get { return "靈石不足!"; }
        }

        public string St_NoFun
        {
            get { return "功能未開啟"; }
        }

        public string St_VipNotEnoughNotFuntion
        {
            get { return "VIP等級不足，未開啟該功能!"; }
        }
        public string St1000_RegistrationNum
        {
            get { return "您已經成功簽到{0}次，完成了本月的微信簽到活動，感謝您對掌遊科技的支持！下個月可以繼續參與哦"; }
        }
        public string St1000_IsRegistration
        {
            get { return "您今天已經簽到成功了，請明天再來簽到吧！"; }
        }
        public string St1000_UserExistent
        {
            get { return "帳號不存在或者您尚未綁定帳號，簽到失敗！"; }
        }
        public string St1000_GetRegistrationGold
        {
            get { return "本次簽到成功！恭喜您獲得：晶石*{0}！"; }
        }
        public string St1004_PasswordMistake
        {
            get { return "您輸入的帳號或密碼不正確!"; }
        }

        public string St1004_PasswordError
        {
            get { return "您輸入的密碼有誤!"; }
        }

        public string St1004_IDNoLogin
        {
            get { return "您的帳號未登錄或已過期!"; }
        }

        public string St1004_IDLogined
        {
            get { return "您的帳號已在其它地方登錄!"; }
        }

        /// <summary>
        /// 您的帳號已被系統強制下線
        /// </summary>
        public string St1004_UserOffline { get { return "您的帳號已被系統強制下線!"; } }

        public string St1004_UserIDError
        {
            get { return "帳號ID獲取失敗!"; }
        }

        public string St1004_IDDisable
        {
            get { return "該帳號已被封禁，登陸失敗!"; }
        }

        public string St1005_RoleCheck
        {
            get { return "您尚未創建角色!"; }
        }

        public string St1005_RoleExist
        {
            get { return "您已創建了角色!"; }
        }

        public string St1005_Professional
        {
            get { return "職業未創建!"; }
        }

        public string St1005_KingNameNotEmpty
        {
            get { return "用戶名稱不能為空!"; }
        }

        public string St1005_PassportError
        {
            get { return "通行證帳號錯誤!"; }
        }

        public string St1005_KingNameTooLong
        {
            get { return "昵稱應在1-{0}個字元!"; }
        }

        public string St1005_Rename
        {
            get { return "該名字已有玩家註冊，請重新輸入"; }
        }
        public string St1005_RegistNameExceptional
        {
            get { return "暱稱存在特殊字符！"; }
        }
        public string St1005_RegistNameKeyWord
        {
            get { return "您輸入的暱稱包含敏感字，請重新輸入！"; }
        }
        public string St1006_PasswordError
        {
            get { return "密碼格式錯誤!"; }
        }

        public string St1006_ChangePasswordError
        {
            get { return "修改密碼失敗!"; }
        }

        public string St1006_PasswordTooLong
        {
            get { return "輸入錯誤，請輸入4-12位數字或字母!"; }
        }
        public string St1006_PasswordExceptional
        {
            get { return "密碼不能包含特殊字符！"; }
        }
        /// <summary>
        /// 當前剩餘經驗加成次數
        /// </summary>
        public string St1008_ExpressSurplusNum
        {
            get { return "當前剩餘經驗加成次數{0}"; }
        }

        /// <summary>
        /// 當前剩餘金幣加成次數
        /// </summary>
        public string St1008_GameCoinSurplusNum
        {
            get { return "當前剩餘金幣加成次數{0}"; }
        }

        /// <summary>
        /// 當前剩餘雙倍材料掉落次數
        /// </summary>
        public string St1008_DoubleItemSurplusNum
        {
            get { return "當前剩餘雙倍材料掉落次數{0}"; }
        }

        /// <summary>
        /// 戰力加成剩餘時間
        /// </summary>
        public string St1008_CombatNumDate
        {
            get { return "戰力加成剩餘時間{0}秒"; }
        }

        /// <summary>
        /// 當前剩餘血量
        /// </summary>
        public string St1008_BloodBagSurplusNum
        {
            get { return "當前剩餘血量{0}"; }
        }

        /// <summary>
        /// 當前變身卡剩餘時間
        /// </summary>
        public string St1008_TransfigurationDate
        {
            get { return "當前變身卡剩餘時間{0}"; }
        }

        public string St1066_PayError
        {
            get { return "充值失敗"; }
        }

        public string St1010_PayEnergyUseGold
        {
            get { return "是否花費{0}晶石購買{1}體力？"; }
        }

        public string St1010_JingliFull
        {
            get { return "今日精力購買次數已用完!"; }
        }

        public string St1013_JingliPrize
        {
            get { return "恭喜您獲得{0}精力!"; }
        }

        public string St1013_DailyJingliPrize
        {
            get { return "恭喜您獲得{0}精力!"; }
        }

        public string St1014_JingshiPrize
        {
            get { return "恭喜您獲得{0}晶石，{1}金幣!"; }
        }

        public string St1018_ExpObtainPrize
        {
            get { return "恭喜您獲得{0}閱歷，{0}聲望!"; }
        }

        public string St1020_FengLu
        {
            get { return "恭喜您獲得{0}俸祿!"; }
        }

        public string St1011_PayCoinUseGold
        {
            get { return "花費{0}晶石獲得{1}金幣，剩餘{2}次！"; }
        }
        public string St1011_PayUseGold
        {
            get { return "進入金礦洞可以獲得大量金幣，是否花費{0}晶石獲得{1}金幣，今日剩餘{2}次。"; }
        }
        public string St1011_PayGainCoin
        {
            get { return "恭喜您進入金礦洞獲得{0}金幣！"; }
        }

        public string St1011_WaJinKuangFull
        {
            get { return "今日挖金礦次數已用完！"; }
        }

        public string St1104_UseGold
        {
            get { return "是否花費{1}晶石開啟{0}個背包格子？"; }
        }

        public string St1107_UserItemNotEnough
        {
            get { return "該物品已賣出"; }
        }

        public string St1107_WarehouseNumFull
        {
            get { return "倉庫已滿"; }
        }

        public string St1107_GridNumFull
        {
            get { return "背包已滿"; }
        }

        public string St1108_WarehouseNumUseGold
        {
            get { return "是否花費{1}晶石開啟{0}個格子？"; }
        }

        public string St1110_OverExpansion
        {
            get { return "超出擴容次數！"; }
        }

        public string St1213_GridNumFull
        {
            get { return "靈件背包已滿"; }
        }

        /// <summary>
        /// 該位置已裝備靈件
        /// </summary>
        public string St1213_GridPotionFull
        {
            get { return "該位置已裝備靈件"; }
        }

        public string St1214_ResetUseGold
        {
            get { return "是否花費{0}晶石，{1}金幣洗滌屬性？"; }
        }

        public string St1214_ResetUseLingshi
        {
            get { return "是否花費{0}靈石，{1}金幣洗滌屬性？"; }
        }

        public string St1215_OpenGridNumUseGold
        {
            get { return "是否花費{1}晶石開啟{0}個靈件格子？"; }
        }

        /// <summary>
        /// 開啟的格子已被占滿
        /// </summary>
        public string St1213_OpenNumNotEnough
        {
            get { return "開啟的格子已被占滿"; }
        }

        public string St1213_GridNumNotEnough
        {
            get { return "格子未開啟"; }
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
            get { return "VIP3級以上才能白金培養！"; }
        }
        public string St1217_NotZhiZhun
        {
            get { return "VIP5級以上才能至尊培養！"; }
        }
        public string St4002_NotChallengeNum
        {
            get { return "您當天最大挑戰次數已用完!"; }
        }
        public string St4002_IsPlotNum
        {
            get { return "您當天最大挑戰次數已用完，是否花費{0}晶石增加一次挑戰次數（VIP3開啟）?"; }
        }
        public string St4002_IsPlotEliteNotChallengeNum
        {
            get { return "已達到每日挑戰次數！"; }
        }
        public string St4004_NoUseMagic
        {
            get { return "您未啟用魔法陣!"; }
        }

        public string St4004_EmbattleEmpty
        {
            get { return "魔法陣未設置傭兵"; }
        }

        public string St4004_GeneralLiveNotEnough
        {
            get { return "傭兵血量不足!"; }
        }

        public string St4011_NoMonster
        {
            get { return "副本:{0}找不到怪物:{1}"; }
        }

        public string St1203_CareerError
        {
            get { return "傭兵職業不符!"; }
        }

        public string St1204_Message
        {
            get { return "裝備強化{0}!"; }
        }

        public string St1204_ColdTime
        {
            get { return "裝備強化冷卻中!"; }
        }

        public string St1204_Colding
        {
            get { return "裝備強化冷卻中!"; }
        }

        public string St1216_EnableSpartProperty
        {
            get { return "是否花費{0}晶石開啟靈件第{1}個屬性？"; }
        }

        public string St1256_EnchantNotEnough
        {
            get { return "您當前背包沒有附魔符！"; }
        }

        public string St1256_EnchantNumNotEnough
        {
            get { return "沒有足夠的附魔符"; }
        }

        public string St1256_OutMaxEnchantLv
        {
            get { return "附魔符已達最高級，無法繼續合成！"; }
        }

        public string St1258_OutMaxEnchantMature
        {
            get { return "附魔符培養已達上限！"; }
        }

        public string St1258_ConsumeMagicCrystalUpEnhance
        {
            get { return "培養成功，消耗{0}魔晶提升{1}成長值！"; }
        }

        public string St1258_ConsumeGoldNumUpEnhance
        {
            get { return "培養成功，消耗{0}晶石提升{1}成長值！"; }
        }

        public string St1258_EnhanceCultureFailedMagicCrystal
        {
            get { return "培養失敗！消耗{0}魔晶！"; }
        }

        public string St1258_EnhanceCultureFailedGold
        {
            get { return "培養失敗！消耗{0}晶石！"; }
        }

        public string St1258_MagicCrystalNotEnough
        {
            get { return "魔晶不足，無法培養！"; }
        }

        public string St1259_EnchantOpenGridFull
        {
            get { return "該位置已裝備附魔符！"; }
        }

        public string St1259_EnchantGridNumFull
        {
            get { return "附魔符背包已滿！"; }
        }

        public string St1259_UserItemNotWuQi
        {
            get { return "當前裝備不是武器！"; }
        }

        public string St1260_UseGoldOpenPackage
        {
            get { return " 您確定花費{0}晶石開啟{1}個格子？"; }
        }

        public string St1261_EnchantEquipmentNotEnough
        {
            get { return "當前背包沒有附魔符，無法裝備！"; }
        }

        public string St1262_EnchantSynthesisNotEnough
        {
            get { return "當前背包沒有可合成附魔符！"; }
        }

        public string St1305_FateBackpackFull
        {
            get { return "獵命空間已滿"; }
        }

        public string St1305_BeiBaoBackpackFull
        {
            get { return "請清理背包，獲得獎勵！"; }
        }

        public string St1305_GainCrystalBackpack
        {
            get { return "恭喜您獲得命運禮包！"; }
        }

        public string St1305_HuntingIDLight
        {
            get { return "當前人物已點亮"; }
        }

        public string St1305_HighQualityNotice
        {
            get { return "{0}人品大爆發，隨手一摸發現竟然是{1}水晶{2}，看四下無人，趕緊藏入口袋！"; }
        }

        /// <summary>
        /// 天上掉餡餅了嗎？玩家{0}伸手摸進寶箱，發現竟然是一個金光燦燦的{1}水晶{2}，元芳，此事你怎麼看？
        /// </summary>
        public string St1305_GainQualityNotice
        {
            get { return "天上掉餡餅了嗎？玩家{0}伸手摸進寶箱，發現竟然是一個金光燦燦的{1}水晶{2}，元芳，此事你怎麼看？"; }
        }

        public string St1307_FateBackpackFull
        {
            get { return "命運水晶背包已滿"; }
        }

        public string St1307_FateBackSpaceFull
        {
            get { return "命運背包空間不足"; }
        }

        public string St1308_CrystalNotEnough
        {
            get { return "水晶欄裡沒有水晶"; }
        }

        public string St1308_CrystalLvFull
        {
            get { return "此水晶已達最高級"; }
        }

        public string St1309_OpenNumNotEnough
        {
            get { return "傭兵開啟的格子已被占滿"; }
        }

        public string St1309_TheSameFate
        {
            get { return "該傭兵已裝備了相同屬性的水晶"; }
        }

        /// <summary>
        /// 該格子已裝備水晶
        /// </summary>
        public string St1309_TheGridFullSameFate
        {
            get { return "該格子已裝備水晶"; }
        }

        public string St1204_EquMaxLv
        {
            get { return "強化等級已達上限"; }
        }

        public string St1204_EquGeneralMaxLv
        {
            get { return "強化等級不能超過傭兵等級!"; }
        }

        public string St1310_UseCrystalGold { get { return "是否花費{1}晶石開啟{0}個格子？"; } }

        public string St1404_RecruitNotFilter { get { return "未達到招募條件，不可招募!"; } }
        /// <summary>
        /// 傭兵數已滿，不可邀請
        /// </summary>
        public string St1404_MaxGeneralNumFull { get { return "傭兵數已滿，不可邀請"; } }

        /// <summary>
        /// 玩家角色不能離隊
        /// </summary>
        public string St1405_LiDuiNotFilter { get { return "玩家角色不能離隊"; } }

        /// <summary>
        /// 該品級藥劑已服滿
        /// </summary>
        public string St1407_MedicineNumFull { get { return "該品級藥劑已服滿"; } }

        /// <summary>
        /// 背包不存在該藥劑
        /// </summary>
        public string St1407_MedicineNum { get { return "背包不存在該藥劑"; } }

        /// <summary>
        /// 直接服用藥劑需{0}晶石
        /// </summary>
        public string St1407_MedicineUseGold { get { return "是否花費{0}晶石直接使用藥劑？"; } }

        /// <summary>
        /// 屬性培養已達上限
        /// </summary>
        public string St1409_maxTrainingNum { get { return "屬性培養已達上限"; } }

        /// <summary>
        /// 玩家沒有此傭兵
        /// </summary>
        public string St1405_GeneralIDNotEnough { get { return "玩家沒有此傭兵"; } }

        /// <summary>
        /// 上線未滿半小時，不能修煉
        /// </summary>
        public string St1411_LessThanHalfAnHour { get { return "上線未滿半小時，不能修煉"; } }

        /// <summary>
        /// 不存在該藥劑
        /// </summary>
        public string St1415_MedicineNum { get { return "奇幻粉末不足"; } }

        public string St1415_GridNumNotEnough
        {
            get { return "背包已滿，請清理背包後重新摘取"; }
        }

        /// <summary>
        /// 摘取{0}需消耗{1}個奇幻粉末，是否要摘取？摘取成功率為{2}%
        /// </summary>
        public string St11415_ClearMedicine { get { return "摘取{0}需消耗{1}個奇幻粉末，是否要摘取？| 摘取成功率為{2}%"; } }

        /// <summary>
        /// 摘取失敗
        /// </summary>
        public string St11415_Clearfail { get { return "摘取失敗"; } }

        /// <summary>
        /// 被傳承傭兵等級低於傳承傭兵，無法傳承
        /// </summary>
        public string St1418_HeritageLvLow
        {
            get { return "被傳承傭兵等級低於傳承傭兵，無法傳承"; }
        }

        /// <summary>
        /// 請選擇傳承傭兵
        /// </summary>
        public string St1418_HeritageNotEnough
        {
            get { return "請選擇傳承傭兵"; }
        }

        /// <summary>
        /// 請選擇被傳承傭兵
        /// </summary>
        public string St1419_IsHeritageNotEnough
        {
            get { return "請選擇被傳承傭兵"; }
        }

        /// <summary>
        /// 傳承傭兵和被傳承傭兵不能是同一人
        /// </summary>
        public string St1419_HeritageNotInIsHeritage
        {
            get { return "傳承傭兵和被傳承傭兵不能是同一人"; }
        }

        public string St1419_DanInsufficientHeritage
        {
            get { return "%s不足"; }
        }

        /// <summary>
        /// 此傭兵已傳承過
        /// </summary>
        public string St1419_HeritageInUse
        {
            get { return "此傭兵已傳承過"; }
        }

        public string St1419_HeritageSuccess
        {
            get { return "傳承成功"; }
        }

        public string St1419_GoldHeritage
        {
            get { return "是否花費{0}晶石進行晶石傳承"; }
        }

        public string St1419_ExtremeHeritage
        {
            get { return "是否花費%d晶石進行至尊傳承"; }
        }

        public string St1422_PresentationUseGold
        {
            get { return "是否花費{0}晶石增加{1}好感度"; }
        }

        public string St1422_FeelMaxSatiationNum
        {
            get { return "當前飽食度已滿，無法使用禮物"; }
        }

        public string St1422_PresentationGoldNum
        {
            get { return "今日晶石贈送次數已用完"; }
        }

        public string St1422_PresentationFeelNum
        {
            get { return "使用成功，增加好感度{0}"; }
        }

        public string St1422_MaxFeelFull
        {
            get { return "該傭兵好感度已達最高等級"; }
        }

        public string St1423_ClearCurrSatiation
        {
            get { return "是否花費1個{0}清除當前飽食度？"; }
        }

        public string St1423_UserItemNotEnough
        {
            get { return "背包不存在消除飽食度物品"; }
        }

        public string St1423_DragonHolyWater
        {
            get { return "該傭兵今日消除飽食度次數已用完"; }
        }

        public string St1484_OperateDefaultAbility
        {
            get { return "無法對默認附帶魂技進行操作"; }
        }

        /// <summary>
        /// 魔術等級已達到最高級
        /// </summary>
        public string St1503_MaxMagicLv { get { return "魔術等級已達到最高級"; } }

        /// <summary>
        /// 魔術 - 等級不能超過玩家等級
        /// </summary>
        public string St1503_MagicLevel { get { return "魔術等級不能超過玩家等級"; } }

        /// <summary>
        /// 魔術 - 魔法陣等級不能超過需求等級
        /// </summary>
        public string St1503_MagicEmbattleLevel { get { return "魔法陣等級不能超過需求等級"; } }

        /// <summary>
        /// 魔術強化冷卻中！
        /// </summary>
        public string St1503_MagicColding { get { return "魔術強化冷卻中"; } }

        /// <summary>
        /// 閱歷不足
        /// </summary>
        public string St1503_UpgradeExpNum { get { return "閱歷不足"; } }

        /// <summary>
        /// 魔術不存在
        /// </summary>
        public string St1503_MagicIDNotEnough { get { return "魔術不存在"; } }

        /// <summary>
        /// 材料不足
        /// </summary>
        public string St1603_MaterialsNotEnough { get { return "材料不足"; } }

        /// <summary>
        /// 您當前缺少合成所需的裝備，無法晶石合成
        /// </summary>
        public string St1603_EquNotEnough { get { return "您當前缺少合成所需的裝備，無法晶石合成"; } }

        public string St1604_MaterialsCityID
        {
            get { return "您當前等級無法到達該副本"; }
        }

        /// <summary>
        /// 合成卷軸需{0}晶石
        /// </summary>
        public string St1603_SynthesisEnergyNum { get { return "是否花費{0}晶石直接進行製作？(注：部分物品無法使用晶石代替)"; } }

        /// <summary>
        /// 繃帶已消耗是否補充 
        /// </summary>
        public string St1605_BandageNotEnough { get { return "繃帶已消耗是否補充"; } }

        /// <summary>
        /// 繃帶使用中 
        /// </summary>
        public string St1605_BandageUse { get { return "繃帶使用中,無法繼續使用。"; } }

        /// <summary>
        /// 是否使用2晶石打開道具商店購買繃帶
        /// </summary>
        public string St1605_UseTwoGold { get { return "是否使用2晶石打開道具商店購買繃帶？"; } }

        public string St1606_OpenPackLackItem
        {
            get { return "{0}不足，無法開啟{1}！"; }
        }

        /// <summary>
        /// 背包格子不足
        /// </summary>
        public string St1606_GridNumNotEnough { get { return "背包格子不足"; } }

        /// <summary>
        /// 命格背包已滿或背包格子不足
        /// </summary>
        public string St1606_BackpackFull { get { return "命運水晶背包已滿或背包格子不足"; } }

        /// <summary>
        /// 佇列加速 - 花費晶石消除該冷卻時間
        /// </summary>
        public string St1702_UseGold { get { return "是否花費{0}晶石消除冷卻時間？"; } }

        /// <summary>
        /// 開啟佇列 - 花費晶石開啟佇列
        /// </summary>
        public string St1703_UseGold { get { return "是否花費{0}晶石開啟佇列？"; } }

        /// <summary>
        /// 開啟佇列 - 佇列已全部開啟
        /// </summary>
        public string St1703_QueueNumFull { get { return "佇列已全部開啟"; } }

        /// <summary>
        /// 主角無法下陣
        /// </summary>
        public string St1902_UserGeneralUnable { get { return "主角無法下陣"; } }

        /// <summary>
        /// 領土戰參戰狀態不允許改變陣法！
        /// </summary>
        public string St1902_CountryCombatNotUpEmbattle
        {
            get { return "領土戰參戰狀態不允許改變陣法！"; }
        }

        /// <summary>
        /// 加入國家提示莫根馬/哈斯德爾
        /// </summary>
        public string St2004_CountryName { get { return "恭喜您已加入{0}國家"; } }

        public string St2004_CountryM { get { return "莫根馬"; } }
        public string St2004_CountryH { get { return "哈斯德爾"; } }

        /// <summary>
        /// 任務系統 - 等級不足，不能領取任務
        /// </summary>
        public string St3002_LvNotEnough { get { return "等級不足，不能領取任務!"; } }
        /// <summary>
        /// 任務系統 - 主線任務未完成
        /// </summary>
        public string St3002_MainNoCompleted { get { return "主線任務未完成!"; } }
        /// <summary>
        /// 任務系統 - "任務已完成，不能放棄
        /// </summary>
        public string St3002_Completed { get { return "任務已完成，不能放棄!"; } }
        /// <summary>
        /// 任務系統 - 未能找到任務
        /// </summary>
        public string St3002_NotFind { get { return "未能找到任務!"; } }
        /// <summary>
        /// 任務系統 - 任務不能領取
        /// </summary>
        public string St3002_NoAllowTaked { get { return "任務不能領取!"; } }
        /// <summary>
        /// 任務系統 - 任務未領取
        /// </summary>
        public string St3002_NoTaked { get { return "任務未領取!"; } }

        /// <summary>
        /// 任務系統 -刷新任務星級需{0}晶石
        /// </summary>
        public string St3005_RefreashUseGold { get { return "刷新任務星級需{0}晶石!"; } }

        /// <summary>
        /// 任務系統 -您當前的任務星級已滿，無法刷新
        /// </summary>
        public string St3005_RefreashStarFull { get { return "您當前的任務星級已滿，無法刷新"; } }

        /// <summary>
        /// 任務系統 -直接完成任務需{0}晶石
        /// </summary>
        public string St3005_CompletedUseGold { get { return "是否花費{0}晶石直接完成任務？"; } }

        /// <summary>
        /// 任務系統 - 任務已達到完成次數
        /// </summary>
        public string St3005_CompletedTimeout { get { return "任務已達到完成次數!"; } }
        /// <summary>
        /// 任務系統 - 任務未完成
        /// </summary>
        public string St3007_NoCompleted { get { return "任務未完成!"; } }

        /// <summary>
        /// 您的晶石不足，無法召喚
        /// </summary>
        public string St3203_GoldNotEnouht { get { return "您的晶石不足，無法召喚"; } }

        /// <summary>
        /// 您的VIP等級不足，無法召喚
        /// </summary>
        public string St3203_VipNotEnouht
        {
            get { return "您的VIP等級不足，無法召喚"; }
        }

        /// <summary>
        /// 是否花費{0}晶石刷新寵物！
        /// </summary>
        public string St3203_RefeshPet { get { return "是否花費{0}晶石刷新寵物！"; } }

        /// <summary>
        /// 是否花費{0}晶石直接召喚獅子！
        /// </summary>
        public string St3203_ZhaohuangPet { get { return "是否花費{0}晶石直接召喚獅子！"; } }

        /// <summary>
        /// 您的寵物已達到最高等級！
        /// </summary>
        public string St3203_MaxPet { get { return "您的寵物已達到最高等級！"; } }

        /// <summary>
        /// 您不能選擇該寵物，寵物未開啟！
        /// </summary>
        public string St3204_PetNoEnable { get { return "您不能選擇該寵物，寵物未開啟！"; } }

        /// <summary>
        /// 您的好友未通過邀請申請！
        /// </summary>
        public string St3204_PetYaoqingNoPass { get { return "您的好友未通過邀請申請！"; } }

        /// <summary>
        /// 您的寵物還在賽跑中，請等待！
        /// </summary>
        public string St3204_PetRunning { get { return "您的寵物還在賽跑中，請等待！"; } }

        /// <summary>
        /// 您今日寵物賽跑次數已用完！
        /// </summary>
        public string St3204_PetRunTimesOut { get { return "您今日寵物賽跑次數已用完！"; } }
        /// <summary>
        /// 您的好友今日護送寵物賽跑次數已用完！
        /// </summary>
        public string St3204_PetHelpeTimesOut { get { return "您的好友今日護送寵物賽跑次數已用完！"; } }

        /// <summary>
        /// 您不能攔截自己的寵物！
        /// </summary>
        public string St3206_PetInterceptError { get { return "您不能攔截自己的寵物！"; } }
        /// <summary>
        /// 您正在護送好友的寵物！
        /// </summary>
        public string St3206_PetFriendError { get { return "您正在護送好友的寵物！"; } }
        /// <summary>
        /// 您今日攔截寵物賽跑次數已用完！
        /// </summary>
        public string St3206_PetInterceptTimesOut { get { return "您今日攔截寵物賽跑次數已用完！"; } }
        /// <summary>
        /// 您攔截的寵物已經賽跑完！
        /// </summary>
        public string St3206_PetInterceptFaild { get { return "您攔截的寵物已經賽跑完！"; } }

        /// <summary>
        /// 您已經攔截過該寵物，不可重複攔截
        /// </summary>
        public string St3206_PetInterceptFull
        {
            get { return "您已經攔截過該寵物，不可重複攔截"; }
        }
        /// <summary>
        /// 您今天已祈禱過
        /// </summary>
        public string St3302_IsPray
        {
            get { return "您今天已祈禱過！"; }
        }
        /// <summary>
        /// 副本系統 - 您的繃帶已使用完，請及時補充。
        /// </summary>
        public string St4002_PromptBlood { get { return "您的繃帶已使用完，請及時補充。"; } }

        /// <summary>
        /// 副本系統 -  背包中沒有繃帶，請及時購買。
        /// </summary>
        public string St4002_UserItemPromptBlood { get { return "背包中沒有繃帶，請及時購買"; } }

        /// <summary>
        /// 副本系統 - 今日精英副本次數已用完
        /// </summary>
        public string St4002_EliteUsed { get { return "今日該精英副本次數已用完！"; } }

        public string St4002_HeroPlotNum
        {
            get { return "今日該英雄副本次數已用完"; }
        }


        /// <summary>
        /// 副本系統 - 正在掃蕩中
        /// </summary>
        public string St4007_Saodanging { get { return "副本正在掃蕩中!"; } }
        /// <summary>
        /// 副本系統 - 掃蕩已結束
        /// </summary>
        public string St4007_SaodangOver { get { return "副本掃蕩已結束!"; } }
        /// <summary>
        /// 副本系統 - 背包已滿無法進行掃蕩
        /// </summary>
        public string St4007_BeiBaoTimeOut { get { return "背包已滿無法進行掃蕩!"; } }
        /// <summary>
        /// 副本系統 - 是否花費{0}個晶石直接完成掃蕩
        /// </summary>
        public string St4008_Tip { get { return "是否花費{0}個晶石直接完成掃蕩？"; } }

        /// <summary>
        /// 副本系統 - 花費{0}個晶石重置精英副本
        /// </summary>
        public string St4012_JingYingPlot { get { return "花費{0}個晶石重置精英副本？"; } }

        /// <summary>
        /// 副本系統 - 今日重置精英副本次數已用完
        /// </summary>
        public string St4012_JingYingPlotFull { get { return "今日重置精英副本次數已用完"; } }

        public string St4014_HeroRefreshPlot
        {
            get { return "花費{0}個晶石重置英雄副本"; }
        }

        public string St4014_HeroRefreshPlotFull
        {
            get { return "當前城市今日重置英雄副本次數已用完"; }
        }

        /// <summary>
        /// 多人副本已結束
        /// </summary>
        public string St4202_OutMorePlotDate { get { return "多人副本已結束"; } }

        /// <summary>
        /// 今日已打過此副本
        /// </summary>
        public string St4205_PlotNotEnough { get { return "今日已打過此副本"; } }

        /// <summary>
        /// 已在隊伍中
        /// </summary>
        public string St4205_InTeam { get { return "已在隊伍中！"; } }

        /// <summary>
        /// 隊伍中人數已滿
        /// </summary>
        public string St4206_TeamPeopleFull { get { return "隊伍中人數已滿！"; } }

        /// <summary>
        /// 沒有可加入的隊伍
        /// </summary>
        public string St4206_NoTeam { get { return "沒有可加入的隊伍！"; } }

        ///<summary>
        ///隊伍已開始戰鬥
        ///</summary>
        public string St4206_TeamPlotStart { get { return "隊伍已開始戰鬥！"; } }

        ///<summary>
        ///隊伍已解散
        ///</summary>
        public string St4206_TeamPlotLead { get { return "隊伍已解散！"; } }

        ///<summary>
        ///隊伍正在戰鬥中
        ///</summary>
        public string St4208_IsCombating { get { return "隊伍正在戰鬥中"; } }

        ///<summary>
        ///隊伍人數不足
        ///</summary>
        public string St4210_PeopleNotEnough { get { return "隊伍人數不足"; } }

        ///<summary>
        ///不存在此副本
        ///</summary>
        public string St4210_PlotNotEnough { get { return "不存在此副本"; } }

        ///<summary>
        /// 您本次多人副本挑戰勝利，獎勵{0}閱歷，{1}*{2}！
        ///</summary>
        public string St4211_MorePlotReward { get { return "您本次多人副本挑戰勝利，獎勵{0}閱歷，{1}*{2}！"; } }

        ///<summary>
        /// 未加入隊伍
        ///</summary>
        public string St4211_UserNotAddTeam { get { return "未加入隊伍"; } }

        ///<summary>
        ///天地劫今日已刷新
        ///</summary>
        public string St4302_PlotRefresh { get { return "天地劫今日已刷新"; } }

        ///<summary>
        ///天地劫靈件掉落
        ///</summary>
        public string St4303_SparePartFalling { get { return "靈件背包已滿，掉落靈件{0}"; } }

        /// <summary>
        /// 此操作將花費您{0}晶石並回到本層第一關，確定執行此操作
        /// </summary>
        public string St4302_SecondRefreshKalpa { get { return "此操作將花費您{0}晶石並回到本層第一關，確定執行此操作"; } }

        /// <summary>
        /// 此操作將花費您{0}晶石並回到上一層第一關，確定執行此操作
        /// </summary>
        public string St4302_LastRefreshKalpa { get { return "此操作將花費您{0}晶石並回到上層第一關，確定執行此操作"; } }

        /// <summary>
        /// 您當前位置無需返回本層！
        /// </summary>
        public string St4302_LastRefreshKalpaNotEnough
        {
            get { return "您當前位置無需返回本層！"; }
        }


        public string St4303_PlotNotEnable { get { return "天地劫下一關暫未開啟！"; } }

        /// <summary>
        /// 天地劫下一層暫未開啟
        /// </summary>
        public string St4303_PlotNotEnableLayerNum
        {
            get { return "天地劫下一層暫未開啟"; }
        }

        /// <summary>
        /// 您
        /// </summary>
        public string St5101_JingJiChangMingCheng { get { return "您"; } }

        /// <summary>
        /// 今日挑戰次數已用完
        /// </summary>
        public string St5103_ChallengeNotNum { get { return "今日挑戰次數已用完"; } }

        /// <summary>
        /// 挑戰時間冷卻中！
        /// </summary>
        public string St5107_Colding { get { return "挑戰時間冷卻中"; } }

        /// <summary>
        /// 競技場排名獎勵{0}積分，{1}金幣！
        /// </summary>
        public string St5106_JingJiChangRankReward { get { return "競技場排名獎勵{0}積分，{1}金幣！"; } }

        /// <summary>
        /// 今日挑戰次數已用完！
        /// </summary>
        public string St5107_ChallGeNumFull { get { return "今日挑戰次數已用完！"; } }

        /// <summary>
        /// XX打敗了XX，登上排行版第一的至尊寶座
        /// </summary>
        public string St5107_JingJiChangOneRank { get { return "{0}打敗了{1}，登上排行榜第一的至尊寶座"; } }

        /// <summary>
        /// XX排名連續上升了N名，已經勢如破竹，不可阻擋了。
        /// </summary>
        public string St5107_JingJiChangMoreNum { get { return "{0}排名連續上升了{1}名，已經勢如破竹，不可阻擋了"; } }

        /// <summary>
        /// {0}霸氣外露，突破紀錄達到{1}連殺
        /// </summary>
        public string St5107_JingJiChangWinNum { get { return "{0}霸氣外露，突破紀錄達到{1}連殺"; } }

        /// <summary>
        /// XX（玩家名稱）打破了XX（玩家名稱）的最高連殺紀錄，已經無法阻擋了
        /// </summary>
        public string St5107_ZuiGaoLianSha { get { return "{0}打破了{1}的最高連殺紀錄，已經無法阻擋了"; } }

        /// <summary>
        /// XX（玩家名稱）達到N連勝，獎勵金幣N，晶石N
        /// </summary>
        public string St5107_ArenaWinsNum { get { return "{0}達到{1}連勝，獎勵金幣{2}，晶石{3}"; } }

        /// <summary>
        /// 您還未加入國家陣營
        /// </summary>
        public string St5201_NoJoinCountry { get { return "您還未加入國家陣營！"; } }
        /// <summary>
        /// 國家領土戰未開始
        /// </summary>
        public string St5201_CombatNoStart { get { return "國家領土戰未開始"; } }
        /// <summary>
        /// 國家領土戰已結束
        /// </summary>
        public string St5201_CombatOver { get { return "國家領土戰已結束"; } }

        /// <summary>
        /// 消耗200閱歷有幾率增加20%戰鬥力
        /// </summary>
        public string St5202_InspireTip { get { return "消耗{0}閱歷有幾率增加20%戰鬥力"; } }
        /// <summary>
        /// 消耗20晶石增加20%戰鬥力
        /// </summary>
        public string St5202_InspireGoldTip { get { return "消耗{0}晶石增加20%戰鬥力"; } }
        /// <summary>
        /// 生命不足請補充血量
        /// </summary>
        public string St5204_LifeNotEnough { get { return "生命不足請補充血量"; } }

        /// <summary>
        /// 挑戰還未開始
        /// </summary>
        public string St5402_CombatNoStart { get { return "挑戰未開始"; } }

        /// <summary>
        /// 您本次領土戰中勝利{0}場，失敗{1}場。總共獲得{2}金幣，{3}聲望，下次繼續努力！
        /// </summary>
        public string St5204_CombatTransfusion { get { return "您本次領土戰中勝利{0}場，失敗{1}場。總共獲得{2}金幣，{3}聲望，下次繼續努力！"; } }

        /// <summary>
        /// 挑戰還未開始
        /// </summary>
        public string St5204_CombatNoStart { get { return "挑戰未開始"; } }

        /// <summary>
        /// 您還未復活，請等待！
        /// </summary>
        public string St5402_IsReliveError { get { return "您還未復活，請等待！"; } }

        /// <summary>
        /// 是否消耗{0}晶石直接進入戰鬥
        /// </summary>
        public string St5403_CombatGoldTip { get { return "是否消耗{0}晶石直接進入戰鬥？"; } }

        /// <summary>
        /// 您已經復活了5次，不能再使用浴火重生
        /// </summary>
        public string St5403_IsReLiveMaxNum { get { return "您已經復活了5次，不能再使用浴火重生"; } }
        /// <summary>
        /// 您已經復活，不需要使用浴火重生
        /// </summary>
        public string St5403_IsLive { get { return "您已經復活，不需要使用浴火重生"; } }

        /// <summary>
        /// 挑戰還在初始化數據，請等待
        /// </summary>
        public string St5405_CombatWait { get { return "挑戰還在初始化數據，請等待"; } }
        /// <summary>
        /// Boss已被擊殺
        /// </summary>
        public string St5405_BossKilled { get { return "Boss已被擊殺"; } }
        /// <summary>
        /// 挑戰已結束
        /// </summary>
        public string St5405_CombatOver { get { return "挑戰已結束"; } }

        /// <summary>
        /// {0}玩家獲得Boss戰擊殺獎，獎勵{1}金幣
        /// </summary>
        public string St5405_CombatKillReward { get { return "{0}玩家獲得Boss戰擊殺獎，獎勵{1}金幣"; } }

        /// <summary>
        /// 參加Boss戰獲得傷害獎勵金幣：{0}，聲望：{1}
        /// </summary>
        public string St5405_CombatHarmReward { get { return "參加Boss戰獲得傷害獎勵金幣：{0}，聲望：{1}"; } }

        /// <summary>
        /// {0}玩家獲得Boss戰第{1}名，獎勵{2}聲望{3}
        /// </summary>
        public string St5405_CombatRankmReward { get { return "{0}玩家獲得Boss戰第{1}名，獎勵{2}聲望{3}"; } }

        /// <summary>
        /// 物品與數量
        /// </summary>
        public string St5405_CombatNum { get { return "{0}*{1}"; } }

        /// <summary>
        /// 已是會員不能申請
        /// </summary>
        public string St6006_AlreadyMember { get { return "已是會員不能申請"; } }

        /// <summary>
        /// 申請公會中
        /// </summary>
        public string St6006_ApplyGuild { get { return "申請公會中"; } }

        /// <summary>
        /// 已達申請上限
        /// </summary>
        public string St6006_ApplyMaxGuild { get { return "已達申請上限"; } }

        /// <summary>
        /// 已申請該公會
        /// </summary>
        public string St6006_ApplyMember { get { return "已申請該公會"; } }


        /// <summary>
        /// 退出工會未滿8小時
        /// </summary>
        public string St6006_GuildMemberNotDate { get { return "退出公會未滿8小時,無法繼續加入公會"; } }

        /// <summary>
        /// 普通成員沒許可權
        /// </summary>
        public string St6007_AuditPermissions { get { return "普通成員沒許可權"; } }

        /// <summary>
        /// 只有公會的會長和副會長才有許可權使用該道具
        /// </summary>
        public string St6024_AuditPermissions { get { return "只有公會的會長和副會長才有許可權使用該道具"; } }

        /// <summary>
        /// 該玩家不是會長沒有許可權
        /// </summary>
        public string St6008_NotChairman { get { return "該玩家不是會長沒有許可權"; } }

        /// <summary>
        /// 副會長人數已滿
        /// </summary>
        public string St6008_VicePresidentNum { get { return "副會長人數已滿"; } }

        /// <summary>
        /// 該會員不是副會長不能轉讓
        /// </summary>
        public string St6008_NotVicePresident { get { return "該會員不是副會長不能轉讓"; } }

        /// <summary>
        /// 該會員不是副會長不能撤銷
        /// </summary>
        public string St6008_NotVicePresidentCeXiao { get { return "該會員不是副會長不能撤銷"; } }

        /// <summary>
        /// 內容不能為空
        /// </summary>
        public string St6009_ContentNotEmpty { get { return "內容不能為空!"; } }

        /// <summary>
        /// 內容應在100個字以內
        /// </summary>
        public string St6009_ContentTooLong { get { return "內容應在100個字以內!"; } }

        /// <summary>
        /// 您當前為公會會長，無法退出公會
        /// </summary>
        public string St6010_Chairman { get { return "您當前為公會會長，無法退出公會"; } }

        /// <summary>
        /// 您不是該工會成員
        /// </summary>
        public string St6011_GuildMemberNotMember { get { return "您不是該公會成員"; } }

        /// <summary>
        /// 今日已上香
        /// </summary>
        public string St6012_HasIncenseToday { get { return "今日已上香"; } }

        /// <summary>
        /// 您成功進行七星朝聖，獲得聲望：+300
        /// </summary>
        public string St6013_GainObtionNum { get { return "您成功進行七星朝聖，獲得聲望：{0}"; } }


        /// <summary>
        /// 工會上香已滿級
        /// </summary>
        public string St6012_GuildShangXiang { get { return "公會上香已滿級"; } }

        /// <summary>
        /// 加入公會當天無法進行朝聖！
        /// </summary>
        public string St6014_GuildFirstDateNotDevilNum
        {
            get { return "加入公會當天無法進行朝聖！"; }
        }

        /// <summary>
        /// 是否花費{0}晶石召喚散仙封魔
        /// </summary>
        public string St6015_SummonSanxian { get { return "是否花費{0}晶石召喚散仙朝聖？"; } }


        /// <summary>
        /// 已是工會成員不能創建工會
        /// </summary>
        public string St6017_UnionMembers { get { return "您已經加入公會，不能再次創建公會"; } }

        /// <summary>
        /// 已存在該公會
        /// </summary>
        public string St6017_Rename { get { return "該名字已有公會命名，請重新輸入"; } }

        /// <summary>
        /// 公會名字不能為空
        /// </summary>
        public string St6017_GuildNameNotEmpty { get { return "公會名稱不能為空!"; } }

        /// <summary>
        /// 公會名稱應在4-12個字元以內
        /// </summary>
        public string St6017_GuildNameTooLong { get { return "公會名稱應在4-12個字元以內"; } }

        /// <summary>
        /// 名稱已被使用
        /// </summary>
        public string St6017_GuildRename { get { return "名稱已被使用!"; } }

        /// <summary>
        /// 公會人數已滿
        /// </summary>
        public string St6019_GuildMaxPeople { get { return "公會人數已滿!"; } }

        /// <summary>
        /// 已加入其他公會
        /// </summary>
        public string St6019_AddGuild { get { return "已加入其他公會"; } }

        /// <summary>
        ///小李飛刀進行七星朝聖還需要N人，公會成員可以前往協助。
        /// </summary>
        public string St6022_GuildConvene { get { return "{0}進行七星朝聖還需要{1}人，公會成員可以前往協助。"; } }


        /// <summary>
        /// 公會成員數量已達上限，無法繼續使用道具
        /// </summary>
        public string St6024_GuildAddMemberToLong { get { return "公會成員數量已達上限，無法繼續使用道具"; } }

        /// <summary>
        /// 本周公會BOSS未開始
        /// </summary>
        public string St6101_GuildBossNotOpen { get { return "本周公會BOSS未開始"; } }

        /// <summary>
        /// 本周公會BOSS已結束
        /// </summary>
        public string St6101_GuildBossOver { get { return "本周公會BOSS已結束"; } }

        /// <summary>
        /// 本周公會BOSS挑戰時間未設置
        /// </summary>
        public string St6101_GuildBossSet { get { return "本周公會BOSS挑戰時間未設置"; } }


        /// <summary>
        /// {0}玩家獲得Boss戰擊殺獎，獎勵{1}金幣
        /// </summary>
        public string St6105_CombatKillReward { get { return "{0}玩家獲得公會Boss戰擊殺獎，獎勵{1}金幣"; } }

        /// <summary>
        /// 參加Boss戰獲得傷害獎勵金幣：{0}，聲望：{1}
        /// </summary>
        public string St6105_CombatHarmReward { get { return "參加公會Boss戰獲得傷害獎勵金幣：{0}，聲望：{1}"; } }

        /// <summary>
        /// {0}玩家獲得Boss戰第{1}名，獎勵{2}聲望{3}
        /// </summary>
        public string St6105_CombatRankmReward { get { return "{0}玩家獲得公會Boss戰第{1}名，獎勵{2}聲望{3}"; } }


        /// <summary>
        /// 本周公會BOSS挑戰時間已設置
        /// </summary>
        public string St6109_GuildBossTime { get { return "本周公會BOSS挑戰時間已設置"; } }

        /// <summary>
        /// 您不是公會成員，請先加入公會
        /// </summary>
        public string St6203_GuildMemberNotEnough { get { return "您不是公會成員，請先加入公會"; } }

        /// <summary>
        /// 捐獻N金幣獲得NN聲望和NN貢獻度
        /// </summary>
        public string St6204_GuildMemberGameCoinDonate { get { return "捐獻{0}金幣獲得{1}聲望和{2}貢獻度"; } }

        /// <summary>
        /// 捐獻N晶石獲得NN聲望和NN貢獻度
        /// </summary>
        public string St6204_GuildMemberGoldDonate { get { return "捐獻{0}晶石獲得{1}聲望和{2}貢獻度"; } }

        /// <summary>
        /// 您輸入的數值大於當日可捐獻最大金額，請重新輸入
        /// </summary>
        public string St6204_OutMaxGuildMemberDonate
        {
            get { return "您輸入的數值大於當日可捐獻最大金額，請重新輸入"; }
        }

        /// <summary>
        /// 您輸入的數值大於當日可捐獻最大晶石，請重新輸入
        /// </summary>
        public string St6204_OutMaxGuildMemberDonateGold { get { return "您輸入的數值大於當日可捐獻最大晶石，請重新輸入"; } }

        /// <summary>
        /// 今日捐獻數量已達上限
        /// </summary>
        public string St6204_OutMaxGuildMemberNum
        {
            get { return "今日捐獻數量已達上限"; }
        }

        /// <summary>
        /// 請輸入分配金額
        /// </summary>
        public string St6204_GuildMemberDonateNum { get { return "請輸入分配金額"; } }

        /// <summary>
        /// 請輸入分配晶石
        /// </summary>
        public string St6204_GuildMemberDonateNumGold
        {
            get { return "請輸入分配晶石"; }
        }

        /// <summary>
        ///公會技能點不足
        /// </summary>
        public string St6205_GuildMemberDonateNotEnough { get { return "公會技能點不足"; } }

        /// <summary>
        /// {0}技能升到{1}級
        /// </summary>
        public string St6205_GuildMemberJiNengShengJi
        {
            get { return "{0}技能升到{1}級"; }
        }

        /// <summary>
        /// 公會晨練活動還沒有開始！
        /// </summary>
        public string St6301_GuildExerciseNoOpen { get { return "公會晨練活動還沒有開始！"; } }

        /// <summary>
        /// 公會晨練活動已開始，現在不能參加！
        /// </summary>
        public string St6301_GuildExerciseIsOpen { get { return "公會晨練活動已開始，現在不能參加！"; } }

        /// <summary>
        /// 公會晨練活動已結束！
        /// </summary>
        public string St6301_GuildExerciseClose { get { return "公會晨練活動已結束！"; } }

        /// <summary>
        /// 全員正確，等級提升！
        /// </summary>
        public string St6303_GuildExerciseAllAnswerTrue { get { return "全員正確，等級提升！"; } }

        /// <summary>
        /// 您超過5道未答題，退出公會
        /// </summary>
        public string St6301_GuildExerciseTimeOut { get { return "您超過5道未答題，退出公會晨練！"; } }

        /// <summary>
        /// 未能全對，從頭開始
        /// </summary>
        public string St6303_GuildExerciseAllAnswerFalse { get { return "未能全對，從頭開始！"; } }

        /// <summary>
        /// 該問題已回答過了
        /// </summary>
        public string St6305_GuildExerciseISAnswer { get { return "該問題已回答過了！"; } }


        /// <summary>
        /// 是否花費{0}晶石自動答對此題
        /// </summary>
        public string St6305_GuildExerciseGoldAnswer { get { return "是否花費{0}晶石自動答對此題？"; } }


        /// <summary>
        /// 是否花費{0}晶石自動回答並答對所有題目
        /// </summary>
        public string St6305_GuildExerciseAutoAnswer { get { return "是否花費{0}晶石自動回答並答對所有題目？"; } }

        /// <summary>
        /// 回答正確，獲得{0}經驗和{1}閱歷
        /// </summary>
        public string St6305_GuildExerciseAnswerSuss { get { return "回答正確，獲得{0}閱歷和{1}經驗！"; } }

        /// <summary>
        /// 回答錯誤!
        /// </summary>
        public string St6305_GuildExerciseAnswerFail { get { return "回答錯誤!"; } }

        /// <summary>
        /// {0}太過注意路上的“風景”，以至答錯了題目。
        /// </summary>
        public string St6305_GuildExerciseGuildChat { get { return "{0}太過注意路上的“風景”，以至答錯了題目。"; } }

        /// <summary>
        /// 天下第一正在報名中，請各位勇士報名參加。
        /// </summary>
        public string St6501_ServerCombatBroadcas { get { return "天下第一大會正在報名中，請各位勇士報名參加。"; } }

        /// <summary>
        /// 天下第一大会即将开始，请参赛各勇士做好准备。
        /// </summary>
        public string St6501_SyncBroadcas { get { return "天下第一大會即將開始，請參賽各勇士做好準備!"; } }


        /// <summary>
        /// 今日天下第一火爆下注，请没有下注的勇士抓紧时间了，金钱不等人！
        /// </summary>
        public string St6501_ServerCombatStakeBroadcas { get { return "今日天下第一火爆下注，請沒有下注的勇士抓緊時間了，金錢不等人！"; } }

        /// <summary>
        /// 天下第一尚未開啟
        /// </summary>
        public string St6501_DoesNotStart { get { return "天下第一尚未開啟"; } }

        /// <summary>
        /// 您已經報名了
        /// </summary>
        public string St6502_YouHavesignedup { get { return "您已經報名了"; } }

        /// <summary>
        /// 當前階段不能報名！
        /// </summary>
        public string St6502_Notsignup { get { return "當前階段不能報名！"; } }

        /// <summary>
        /// 還在報名階段！
        /// </summary>
        public string St6503_AlsoInTheStage { get { return "還在報名階段！"; } }

        /// <summary>
        /// 當前沒有您的相關戰績！
        /// </summary>
        public string St6504_NotHaveInfo { get { return "當前沒有您的相關戰績！"; } }

        /// <summary>
        /// 已下注{0}{1}W
        /// </summary>
        public string St6506_HasBet { get { return "已下注[{0}]{1}W"; } }

        /// <summary>
        /// 金幣不足
        /// </summary>
        public string St6507_GoldCoinShortage { get { return "金幣不足"; } }

        /// <summary>
        /// 您已下注
        /// </summary>
        public string St6507_YouFaveBet { get { return "當前階段您已下注"; } }

        /// <summary>
        /// 下注[{0}]{1}金幣
        /// </summary>
        public string St6506_BetGold { get { return "下注[{0}]{1}W金幣"; } }

        /// <summary>
        /// ,等待結果……
        /// </summary>
        public string St6506_WaitResults { get { return ",等待結果……"; } }

        /// <summary>
        /// ,獲利{0}金幣!
        /// </summary>
        public string St6506_ProfitGold { get { return ",獲利{0}W金幣!"; } }

        /// <summary>
        /// ,損失{0}金幣!
        /// </summary>
        public string St6506_LossGold { get { return ",損失{0}W金幣!"; } }

        /// <summary>
        ///  您已下注{0}場，總額{1}金幣，獲利{2}金幣
        /// </summary>
        public string St6506_StakeDesc { get { return "您已下注{0}場，總額{1}W金幣，獲利{2}W金幣"; } }

        public string St6512_CombatNoHistoricalRecord
        {
            get { return "暫無歷史戰績"; }
        }

        /// <summary>
        ///  天下第一獎勵金幣{0}W!
        /// </summary>
        public string St65010_CombatPrizeGameCoins { get { return " 天下第一獎勵金幣{0}W"; } }

        /// <summary>
        ///  天下第一獎勵聲望{0}!
        /// </summary>
        public string St65010_CombatPrizeObtainNum { get { return " 天下第一獎勵聲望{0}"; } }

        /// <summary>
        ///  跨服战奖励:{0}
        /// </summary>
        public string St65010_CombatPrize { get { return "天下第一獎勵:"; } }

        /// <summary>
        /// 天下第一下注獲利{0}W
        /// </summary>
        public string St6501_StakePrizeWin { get { return "天下第一下注獲利{0}W金幣"; } }

        /// <summary>
        /// 天下第一下注返還金幣{0}W
        /// </summary>
        public string St6501_StakePrizeLost { get { return "天下第一下注返還{0}W金幣"; } }

        /// <summary>
        /// 天下第一階段
        /// </summary>
        public string St_ServerCombatStage1 { get { return "淘汰賽"; } }

        /// <summary>
        /// 天下第一階段
        /// </summary>
        public string St_ServerCombatStage2 { get { return "32強賽"; } }

        /// <summary>
        /// 天下第一階段
        /// </summary>
        public string St_ServerCombatStage3 { get { return "16強賽"; } }

        /// <summary>
        /// 天下第一階段
        /// </summary>
        public string St_ServerCombatStage4 { get { return "8強賽"; } }

        /// <summary>
        /// 天下第一階段
        /// </summary>
        public string St_ServerCombatStage5 { get { return "半決賽"; } }

        /// <summary>
        /// 天下第一階段
        /// </summary>
        public string St_ServerCombatStage6 { get { return "決賽"; } }

        /// <summary>
        /// 天下第一階段
        /// </summary>
        public string St_ServerCombatCombatType1 { get { return "天榜"; } }

        /// <summary>
        /// 天下第一階段
        /// </summary>
        public string St_ServerCombatCombatType2 { get { return "人榜"; } }

        /// <summary>
        /// 第{0}輪
        /// </summary>
        public string St_RoundNum { get { return "第{0}輪"; } }

        /// <summary>
        /// 商店系統 - 背包已滿無法進行購買
        /// </summary>
        public string St7004_BeiBaoTimeOut { get { return "背包已滿無法進行購買!"; } }

        /// <summary>
        /// 商店系統 - 奇石不足
        /// </summary>
        public string St7004_QiShiNotEnough { get { return "奇石不足!"; } }

        /// <summary>
        /// 黑市商店系統 - 此物品已購買
        /// </summary>
        public string St7005_HavePurchasedItem
        {
            get { return "此物品已購買"; }
        }

        public string St7006_UserItemHaveSpare
        {
            get { return "請先卸下裝備上的靈件再出售裝備"; }
        }

        /// <summary>
        /// 商店系統 - 神秘商店刷新花費晶石數
        /// </summary>
        public string St7007_UseSparRefreshGold { get { return "是否花費{0}晶石刷新？"; } }

        /// <summary>
        /// 禮包已領取
        /// </summary>
        public string St9003_AlreadyReceived { get { return "禮包已領取"; } }

        /// <summary>
        /// 不存在該用戶
        /// </summary>
        public string St9103_DoesNotExistTheUser { get { return "不存在該用戶"; } }

        /// <summary>
        /// 該用戶已在好友中
        /// </summary>
        public string St9103_TheUserHasAFriendIn { get { return "該用戶已在好友中"; } }

        /// <summary>
        /// 該用戶已在粉絲中
        /// </summary>
        public string St9103_TheUserHasTheFansIn { get { return "該用戶已在粉絲中"; } }


        /// <summary>
        /// 該用戶已在关注中
        /// </summary>
        public string St9103_TheUserHasTheAttentIn { get { return "該用戶已在关注中"; } }


        /// <summary>
        /// 該用戶已在黑名單中
        /// </summary>
        public string St9103_TheUserHasTheBlacklist { get { return "該用戶已在黑名單中"; } }

        /// <summary>
        /// 已達好友上限
        /// </summary>
        public string St9103_TheMaximumReachedAFriend { get { return "已達好友上限"; } }

        /// <summary>
        /// 不存在該玩家
        /// </summary>
        public string St9103_NotFriendsUserID { get { return "不存在該玩家"; } }

        /// <summary>
        /// 聊天內容不能為空
        /// </summary>
        public string St9201_contentNotEmpty { get { return "聊天內容不能為空"; } }



        /// <summary>
        /// 您當前背包中沒有千里傳音！親，您可以到商城購買哦！
        /// </summary>
        public string St9203_ItemEmpty { get { return "您當前背包中沒有千里傳音！親，您可以到商城購買哦！"; } }

        /// <summary>
        /// 輸入文字過長
        /// </summary>
        public string St9201_TheInputTextTooLong { get { return "輸入文字過長"; } }

        /// <summary>
        /// 不能頻繁發送聊天內容
        /// </summary>
        public string St9203_ChatNotSend { get { return "您的發言過於頻繁，10秒後可繼續發言"; } }


        /// <summary>
        /// 未加入公會，不能發言
        /// </summary>
        public string St9203_ChaTypeNotGuildMember { get { return "未加入公會，不能發言"; } }

        /// <summary>
        /// 莊園種植未開啟
        /// </summary>
        public string St10001_ManorPlantingNotOpen { get { return "莊園種植未開啟"; } }

        /// <summary>
        /// 聖水不足
        /// </summary>
        public string St10004_DewNotEnough { get { return "聖水不足"; } }

        /// <summary>
        /// 傭兵等級不能超過玩家等級
        /// </summary>
        public string St10004_GeneralNotUserLv { get { return "傭兵等級不能超過玩家等級"; } }

        /// <summary>
        /// 莊園系統 - 是否花費{0}個晶石刷新
        /// </summary>
        public string St10005_Refresh { get { return " 是否花費{0}個晶石刷新？"; } }

        /// <summary>
        /// 莊園系統 - 當前為最高品質
        /// </summary>
        public string St10005_MaxQualityType { get { return "當前為最高品質!"; } }

        /// <summary>
        /// 恭喜您的傭兵{0}升至{1}級
        /// </summary>
        public string St10006_UserGeneralUpLv { get { return "恭喜您的傭兵{0}升至{1}級。"; } }


        /// <summary>
        /// 不存在該用戶
        /// </summary>
        public string St10006_DoesNotExistTheGeneral { get { return "不存在該傭兵"; } }

        /// <summary>
        /// 莊園系統 - 花費{0}個晶石將冷卻時間設為0!
        /// </summary>
        public string St10007_DoRefresh { get { return " 是否花費{0}個晶石將冷卻時間設為0？"; } }

        /// <summary>
        /// 莊園系統 -土地已開啟!
        /// </summary>
        public string St10008_LandPostionIsOpen { get { return " 土地已開啟"; } }

        /// <summary>
        /// 莊園系統 - 花費{0}個晶石開啟土地!
        /// </summary>
        public string St10008_OpenLandPostion { get { return " 是否花費{0}個晶石開啟土地？"; } }

        /// <summary>
        /// 莊園系統 - 聖水數量已滿!
        /// </summary>
        public string St10009_DewNumFull { get { return " 聖水數量已滿!"; } }

        /// <summary>
        /// 莊園系統 - 花費{0}個晶石購買聖水!
        /// </summary>
        public string St10009_PayDewUseGold { get { return " 是否花費{0}個晶石購買聖水？"; } }
        /// <summary>
        /// VIP3級以上才能購買聖水!
        /// </summary>
        public string St10009_NotPayDew { get { return " VIP5級以上才能購買聖水!"; } }

        /// <summary>
        /// 莊園系統 - 土地未全部開啟，不能升級紅土地!
        /// </summary>
        public string St10010_LandNotEnough { get { return " 土地未全部開啟，不能升級紅土地!"; } }

        /// <summary>
        /// 莊園系統 - 花費{0}個晶石升級紅土地!
        /// </summary>
        public string St10010_UpRedLandUseGold { get { return " 是否花費{0}個晶石升級紅土地？"; } }

        /// <summary>
        /// 莊園系統 - 土地已升級!
        /// </summary>
        public string St10010_UpRedLandNotEnough { get { return " 土地已升級！"; } }

        /// <summary>
        /// 莊園系統 - 已是紅土地，不需升級!
        /// </summary>
        public string St10010_RedLandFull { get { return " 已是紅土地，不需升級"; } }

        /// <summary>
        /// 莊園系統 - 紅土地未滿，不能升級黑土地!
        /// </summary>
        public string St10011_RedLandNotEnough { get { return "紅土地未滿，不能升級黑土地!"; } }

        /// <summary>
        /// 莊園系統 - 花費{0}個晶石升級黑土地!
        /// </summary>
        public string St10011_UpBlackLandUseGold { get { return " 是否花費{0}個晶石升級黑土地？"; } }

        /// <summary>
        /// 莊園系統 - 不是紅土地不能升級為黑土地!
        /// </summary>
        public string St10011_NotRedLand { get { return " 不是紅土地不能升級為黑土地!"; } }

        /// <summary>
        /// 莊園系統 - 已是黑土地，不需升級!
        /// </summary>
        public string St10011_BlackLandFull { get { return " 已是黑土地，不需升級"; } }

        /// <summary>
        /// 每日探險答題冷卻中！
        /// </summary>
        public string St11002_Colding { get { return "每日探險答題冷卻中"; } }

        /// <summary>
        /// 您已經完成本日探險，請明天再來！
        /// </summary>
        public string St11002_ExpeditionFull { get { return "您已經完成本日探險，請明天再來！"; } }

        /// <summary>
        /// 莊園系統 - 是否花費{0}個晶石消除冷卻時間
        /// </summary>
        public string St11003_DelCodeTime { get { return " 是否花費{0}個晶石消除冷卻時間？"; } }

        /// <summary>
        /// 恭喜您獲得父親節獎勵：30點精力，50點聲望，20000金幣，祝您遊戲愉快！
        /// </summary>
        public string St_FathersDay { get { return "恭喜您獲得父親節獎勵：{0}點精力，{1}點聲望，{2}金幣，祝您遊戲愉快"; } }

        /// <summary>
        /// 恭喜您獲得端午節獎勵：20點精力，50點聲望，200閱歷，祝您遊戲愉快！
        /// </summary>
        public string St_DragonBoatFestival { get { return "恭喜您獲得端午節獎勵：20點精力，50點聲望，200閱歷，祝您遊戲愉快"; } }

        /// <summary>
        /// XXX無意中被不明物體砸中，定睛一看原來是白鑽粽子，聽說裡面有傳說中的紫色水晶，雙手頓時顫抖不已。
        /// </summary>
        public string St_DragonBoatZongzi { get { return "{0}無意中被不明物體砸中，定睛一看原來是白鑽粽子，聽說裡面有傳說中的紫色水晶，雙手頓時顫抖不已。"; } }

        /// <summary>
        /// 恭喜你獲得端午節禮物XXX！
        /// </summary>
        public string St_DragonBoatPuTongZongzi { get { return "恭喜你獲得端午節禮物{0}！"; } }

        /// <summary>
        /// 恭喜您獲得競技場幸運數字七獎勵：聲望50、閱歷200、金幣50W！
        /// </summary>
        public string St_HolidayFestival { get { return "恭喜您獲得競技場幸運數字七獎勵：聲望50、閱歷200、金幣50W"; } }

        /// <summary>
        /// 恭喜你獲得假日狂歡季禮物XXX！
        /// </summary>
        public string St_HolidayFestivalGift { get { return "恭喜你獲得假日狂歡季禮物{0}！"; } }

        /// <summary>
        /// XXX在村外閒逛，突然間眼前金光燦燦，順手摸去，竟是一顆粉紅水晶，裡面包裹著沉甸甸的金子。於是高喊：發財啦,發財啦！
        /// </summary>
        public string St_HolidayFestivalGoinGift { get { return "{0}在村外閒逛，突然間眼前金光燦燦，順手摸去，竟是一顆粉紅水晶，裡面包裹著沉甸甸的金子。於是高喊：發財啦,發財啦！"; } }


        /// <summary>
        /// 恭喜你獲得活動獎勵：40W金幣、白奇石*4、神秘禮盒*1
        /// </summary>
        public string St_SummerSecondNotice1 { get { return "恭喜你獲得活動獎勵40W金幣、白奇石*4、神秘禮盒*1"; } }

        /// <summary>
        /// 恭喜你獲得活動獎勵：40W金幣、綠奇石*4、神秘禮盒*1
        /// </summary>
        public string St_SummerSecondNotice2 { get { return "恭喜你獲得活動獎勵：40W金幣、綠奇石*4、神秘禮盒*1"; } }

        /// <summary>
        /// 恭喜你獲得活動獎勵：80W金幣、綠奇石*4、神秘禮盒*1
        /// </summary>
        public string St_SummerSecondNotice3 { get { return "恭喜你獲得活動獎勵：80W金幣、綠奇石*4、神秘禮盒*1"; } }

        /// <summary>
        /// 聲望第一上線公告
        /// </summary>
        public string St_ObtionNumNotice { get { return "忽見遙遠的西方風雨巨變，仔細一看，原來是本服聲望之王{0}閃亮登場！"; } }

        /// <summary>
        /// 聲望前三上線公告
        /// </summary>
        public string St_ObtionTopThreeNotice { get { return "當當當，當當當，遠方傳來了幾聲巨響，原來是本服三大聲望巨賈之一的{0}上線了！"; } }

        /// <summary>
        /// 財富第一上線公告
        /// </summary>
        public string St_GameCoinTopOneNotice { get { return "忽見遠方金光燦燦，銀光閃閃，原來是本服財富第一，富可敵國的{0}粉墨登場了！"; } }

        /// <summary>
        /// 財富前三上線公告
        /// </summary>
        public string St_GameCoinThreeNotice { get { return "忽如一夜金幣來，金幣銀幣掉下來，原來是本服三大富豪之一的{0}閃亮登場了！"; } }

        /// <summary>
        /// 財富前十上線公告
        /// </summary>
        public string St_GameCoinTopTenNotice { get { return "本服十大富豪之一的{0}隆重出場了！"; } }

        /// <summary>
        /// 戰力第一上線公告
        /// </summary>
        public string St_CombatNumTopOneNotice { get { return "大地在顫抖，高山在搖晃，原來是本服的戰鬥力之王戰神{0}登錄遊戲了！"; } }

        /// <summary>
        /// 戰力前三上線公告
        /// </summary>
        public string St_CombatNumTopThreeNotice { get { return "本服三大戰神之一的{0}閃亮登場了，閒雜人等注意避讓！"; } }

        /// <summary>
        /// 戰力前十上線公告
        /// </summary>
        public string St_CombatNumTopTenNotice { get { return "本服十大戰將之一的{0}閃亮登場了，果然氣勢不凡！"; } }

        /// <summary>
        /// 等級第一上線公告
        /// </summary>
        public string St_LvTopTenNotice { get { return "本服第一等級達人{0}粉墨登場，氣宇軒昂迷倒萬千粉絲！"; } }

        /// <summary>
        /// 精英副本獎勵發聊天
        /// </summary>
        public string St_PlotRewardNotice { get { return "{0}神勇無比打敗{1}，獲得{2}"; } }

        /// <summary>
        /// 恭喜您獲得金幣
        /// </summary>
        public string St_SummerThreeGameCoinNotice { get { return "恭喜您獲得金幣：{0}"; } }

        /// <summary>
        /// 恭喜您獲得聲望
        /// </summary>
        public string St_SummerThreeObtionNotice { get { return "恭喜您獲得聲望：{0}"; } }

        /// <summary>
        /// 恭喜您獲得精力
        /// </summary>
        public string St_SummerThreeEnergyNotice { get { return "恭喜您獲得精力：{0}"; } }

        /// <summary>
        /// 恭喜您獲得閱歷
        /// </summary>
        public string St_SummerThreeExpNumNotice { get { return "恭喜您獲得閱歷：{0}"; } }

        /// <summary>
        /// 恭喜您獲得晶石
        /// </summary>
        public string St_SummerThreeGoldNotice { get { return "恭喜您獲得晶石：{0}"; } }

        /// <summary>
        /// 恭喜您獲得經驗
        /// </summary>
        public string St_SummerThreeExperienceNotice { get { return "恭喜您獲得經驗：{0}"; } }

        /// <summary>
        /// 恭喜您獲得物品
        /// </summary>
        public string St_SummerThreeItemNotice { get { return "恭喜您獲得物品：{1}*{0}"; } }

        /// <summary>
        /// 恭喜您獲得水晶
        /// </summary>
        public string St_SummerCrystalNotice { get { return "恭喜您獲得水晶：{0}"; } }

        /// <summary>
        /// 恭喜您，獲得{0}
        /// </summary>
        public string St_SummerComradesItemNotice { get { return "恭喜您，獲得{0}"; } }

        /// <summary>
        /// 天界大沖級-- 恭喜您，獲得：xx、xx、xx,請繼續努力！
        /// </summary>
        public string St_SummerLeveling { get { return "恭喜您，獲得：{0}請繼續努力！"; } }

        /// <summary>
        /// 金幣*{0}
        /// </summary>
        public string St_GameCoin { get { return "金幣*{0}"; } }

        /// <summary>
        /// 聲望
        /// </summary>
        public string St_ObtionNum { get { return "{0}聲望"; } }



        /// <summary>
        /// 聊天通知- 寵物賽跑,您當前成功護送{0}，獲得金幣{1}，聲望{2}
        /// </summary>
        public string Chat_PetRunSucess { get { return "您當前成功護送{0}，獲得金幣{1}，聲望{2}!"; } }

        /// <summary>
        /// 聊天通知- 寵物攔截賽跑,{0}成功攔截{1}的{2}，獲得金幣{3}，聲望{4}!
        /// </summary>
        public string Chat_PetInterceptSucess { get { return "{0}成功攔截{1}的{2}，獲得金幣{3}，聲望{4}!"; } }

        /// <summary>
        /// 私聊通知- 您的寵物XX在半路被玩家XX攔截，受到了驚嚇，損失金幣XXX，聲望XXX。
        /// </summary>
        public string Chat_PetWasBlocked
        {
            get { return "您的寵物{0}在半路被玩家{1}攔截，受到了驚嚇，損失金幣{2}，聲望{3}。"; }
        }

        public string St_Tanabata { get { return "恭喜你獲得七夕節禮物{0}！"; } }

        public string St_UserNameTanabata { get { return "恭喜{0}獲得七夕節禮物{1}！"; } }

        public string St_TanabataLoginFestival { get { return "恭喜您獲得七夕節獎勵：70點精力、700點聲望、70W金幣，祝您遊戲愉快"; } }

        /// <summary>
        /// 恭喜您獲得{0}獎勵：{1}，祝您遊戲愉快，祝您遊戲愉快！
        /// </summary>
        public string St_FestivalRewardContent
        {
            get { return "恭喜您獲得{0}獎勵：{1}，祝您遊戲愉快，祝您遊戲愉快！"; }
        }

        public string st_FestivalInfoReward { get { return "恭喜您獲得{0}{1}，請繼續努力！"; } }

        public string St_AugustSecondWeek { get { return "狂歡號外活動獎勵"; } }

        public string St_EnergyNum { get { return "{0}精力"; } }
        public string St_ExpNum { get { return "{0}閱歷"; } }
        public string St_GiftGoldNum { get { return "晶石*{0}"; } }

        public string St_HonourNum
        {
            get { return "{0}榮譽值"; }
        }

        public string St_Experience { get { return "{0}經驗"; } }
        public string St_Item { get { return "{0}*{1}"; } }
        public string St_ItemReward { get { return "{0}*{1}"; } }
        public string St_Crystal { get { return "{0}水晶{1}"; } }
        public string St_MonsterCard { get { return "{0}*{1}"; } }
        public string St_GeneralSoul { get { return "{0}*{1}"; } }
        public string St_Ability { get { return "{0}*{1}"; } }

        public string ZhongYuanHuodong
        {
            get { return "活動期間變身卡使用無效"; }
        }

        /// <summary>
        ///  恭喜XX獲得競技場X連勝獎勵：XX
        /// </summary>
        public string SportVictoryReward
        {
            get { return " 恭喜{0}獲得競技場{1}連勝獎勵：{2}"; }
        }

        /// <summary>
        /// 狀態增幅藥劑藥性過於猛烈，不可連續食用！
        /// </summary>
        public string St1608_CombatPowerNotEnough
        {
            get { return "狀態增幅藥劑藥性過於猛烈，不可連續食用！"; }
        }

        /// <summary>
        /// 爆炸髮型，補丁衣裳，右手拿棍，左手拿碗，註定您要走運了，恭喜XX，獲得：XX
        /// </summary>
        public string St_SparePackNotice
        {
            get { return "爆炸髮型，補丁衣裳，右手拿棍，左手拿碗，註定您要走運了，恭喜{0}，獲得：{1}"; }
        }

        /// <summary>
        /// 被合成裝備上有靈件，請先放入靈件背包
        /// </summary>
        public string St_ItemEquIndexOfSpare
        {
            get { return "被合成裝備上有靈件，請先放入靈件背包"; }
        }
        /// <summary>
        /// XXX物品無法使用晶石代替，合成失敗！
        /// </summary>
        public string St_ItemIsGold
        {
            get { return "{0}物品無法使用晶石代替，合成失敗！"; }
        }

        public string GiftType_Food
        {
            get { return "食物"; }
        }

        public string GiftType_Kitchenware
        {
            get { return "廚具"; }
        }

        public string GiftType_Mechanical
        {
            get { return "機械"; }
        }

        public string GiftType_Books
        {
            get { return "書籍"; }
        }

        public string GiftType_MusicalInstruments
        {
            get { return "樂器"; }
        }

        public string OldFriendPack { get { return "感謝您重新返回遊戲，更多精彩等著你。恭喜您獲得老友禮包*1"; } }

        public string NewHandPackage
        {
            get { return "恭喜您，獲得{0}級拉新禮包一個！"; }
        }

        public string GainNewCard
        {
            get { return "恭喜您達到{0}級，獲得金幣{1}和兩次邀請好友加入天界的機會，拉新卡號：{2}，可供兩名20級以下的勇士與您一起分享多重驚喜！（注：新手勇士要在德亞蘭的老村長處啟動）"; }
        }

        public string St1024_NewHandFail
        {
            get { return "您的拉新卡號無效，或角色等級不符，啟動失敗！"; }
        }

        public string St1024_NewHandSuccess
        {
            get { return "拉新卡啟動成功"; }
        }

        public string St1456_UpTrumpItemNotEnough
        {
            get { return "您當前沒有靈魂碎片，無法升級法寶！"; }
        }

        public string St1457_UseLifeExtension
        {
            get { return "是否消耗一個延壽丹增加法寶壽命？"; }
        }

        public string St1457_LifeExtensionNotEnough
        {
            get { return "您當前背包沒有延壽丹，可以選擇到商城購買！"; }
        }

        public string St1457_MaxLifeExtension
        {
            get { return "當前法寶壽命已滿，無需使用延壽丹！"; }
        }

        public string St1458_UseBackDaysOrb
        {
            get { return "是否消耗{0}個夢幻寶珠增加{1}點成長值（成功率{2}%）"; }
        }

        public string St1458_MaxMatrueNumFull
        {
            get { return "當前法寶成長值已達最高！"; }
        }

        public string St1458_BackDaysOrbNotEnough
        {
            get { return "您當前沒有足夠的夢幻寶珠，可以選擇到商城購買！"; }
        }

        public string St1458_XiLianSuccess
        {
            get { return "洗練成功"; }
        }

        public string St1458_XiLianFail
        {
            get { return "洗練失敗"; }
        }

        public string St1460_WashingSkills
        {
            get { return "確定要花費{0}晶石重新洗滌技能嗎？"; }
        }

        public string St1460_SkillsNotEnough
        {
            get { return "法寶沒有該技能"; }
        }

        public string St1460_WashingSkillsNotEnough
        {
            get { return "您當前晶石不足，無法洗滌！"; }
        }

        public string St1462_OutMaxTrumpLv
        {
            get { return "該技能已達最高等級！"; }
        }

        public string St1462_ItemNumNotEnough
        {
            get { return " 物品數量不足！"; }
        }

        public string St1464_UpgradeWasSsuccessful
        {
            get { return "升級成功"; }
        }

        public string St1466_WorshipPropertyNotEnough
        {
            get { return "當前沒有空餘凹槽！"; }
        }

        public string St1466_ItemPropertyNotEnough
        {
            get { return "當前沒有屬性技能書，無法學習！"; }
        }

        public string St1466_ItemPropertyExite
        {
            get { return "當前屬性已存在！"; }
        }

        public string St1466_OutPropertyMaxLv
        {
            get { return "當前屬性已達最高等級！"; }
        }

        public string St1467_WorshipGridNotEnough
        {
            get { return "確定要刪除當前凹槽技能嗎？"; }
        }

        public string St1471_ChangeZodiac
        {
            get { return "是否花費{0}晶石改變法寶屬相？"; }
        }

        public string St1457_ChangeLifeNum
        {
            get { return "延壽成功，增加{0}壽命"; }
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
            get { return "该魂技已装备在佣兵身上！"; }
        }
        public string St4303_EnchantingCharacterFalling
        {
            get { return "掉落附魔符{0}"; }
        }

        public string St1456_OutTrumpMaxLv
        {
            get { return "法寶等級已達上限"; }
        }

        public string St4301_RandomEnchant
        {
            get { return "隨機1級附魔符"; }
        }

        public string St4002_EnchantPackageFull
        {
            get { return "您的附魔符背包已滿，無法獲得附魔符，確定要進入副本嗎？"; }
        }

        public string St1460_WashingSuccess
        {
            get { return "洗滌成功"; }
        }

        public string St1466_LearningSuccess
        {
            get { return "學習成功"; }
        }

        public string St1464_WorshipSuccess
        {
            get { return "祭祀成功"; }
        }

        public string St1464_WorshipFail
        {
            get { return "祭祀失敗"; }
        }

        public string St1433_StoryTaskGridNotEnough
        {
            get { return "您當前背包剩餘空間不足，請整理背包後重新領獎！"; }
        }

        public string St1433_RewardAlreadyReceive
        {
            get { return "獎勵已領取"; }
        }

        public string St1901_OpenGeneralReplace
        {
            get { return "恭喜您開啟替補陣法，並獲得替補傭兵禮包*1，您可以在陣法中設置替補出場的傭兵，享受非一般的戰鬥快感！"; }
        }

        public string St1902_PostionNotGeneral
        {
            get { return "該位置只能放置傭兵！"; }
        }
        public string St1434_RecruitmentErfolg
        {
            get { return "招募成功！"; }
        }

        public string St12004_FreeNotEnough
        {
            get { return "免費抽獎次數已用完"; }
        }

        public string St12004_FreeNumEnough
        {
            get { return "免費抽獎次數未用完"; }
        }

        public string St12004_SpendSparDraw
        {
            get { return "是否花費{0}晶石抽獎{1}次"; }
        }

        public string St12004_YouWheelOfFortune
        {
            get { return "您在幸運轉盤中獲得:{0}"; }
        }

        public string St12004_RewardSweepstakes
        {
            get { return "獎勵一次抽獎次數"; }
        }

        public string St12004_DidNotAnyReward
        {
            get { return "沒有獲得任何獎勵"; }
        }

        public string St12004_RechargeReturn
        {
            get { return "下一次充值時，享受{0}的返還。"; }
        }

        public string St12004_RechargeReturnGoldNum
        {
            get { return "您在大轉盤中抽到充值返還{0}獎勵，返還{1}晶石,祝您遊戲愉快！"; }
        }

        public string St6404_HaveSignedUp
        {
            get { return "本周已報名"; }
        }

        public string St6404_CityABattleTime
        {
            get { return "公會城市爭鬥戰戰鬥中，無法報名"; }
        }

        public string St6404_OrdinaryMemberNotCompetence
        {
            get { return "普通會員沒有許可權"; }
        }

        public string St6404_GuildLvNotEnough
        {
            get { return "公會等級不足，無法報名"; }
        }

        public string St6404_CurrDonateNumNotEnough
        {
            get { return "公會技能點不足，無法報名"; }
        }

        public string St6405_GuildBannerNotEnough
        {
            get { return "公會旗幟名稱不能為空"; }
        }

        public string St6405_GuildNotEnterName
        {
            get { return "公會未報名，不能設置旗幟名稱"; }
        }

        public string St6412_HaveSignedUp
        {
            get { return "未報名城市爭鬥戰，不能參戰"; }
        }

        public string St6412_FightWarDate
        {
            get { return "公會城市爭鬥戰戰鬥中，不能參戰"; }
        }

        public string St6404_GuildWarFirstPackID
        {
            get { return "恭喜您的公會在城市爭奪戰中獲得第一名，所有參與成員獲得{0}"; }
        }

        public string St6404_GuildWarSecondPackID
        {
            get { return "恭喜您的公會在城市爭奪戰中獲得第二名，所有參與成員獲得{0}"; }
        }

        public string St6404_GuildWarThirdPackID
        {
            get { return "恭喜您的公會在城市爭奪戰中獲得第三名，所有參與成員獲得{0}"; }
        }

        public string St6404_GuildWarParticipateID
        {
            get { return "恭喜您的公會在城市爭奪戰中獲得名次，所有參與成員獲得{0}"; }
        }

        public string St6401_GuildFightBroadCas
        {
            get { return "城市爭奪戰已打響，是否立即加入戰場為公會而戰?"; }
        }

        public string St6401_SuccessfulRegistration
        {
            get { return "報名成功"; }
        }

        public string St6405_SettingTheBannerSuccess
        {
            get { return "設置旗幟成功"; }
        }

        public string St6412_FightWarSuccess
        {
            get { return "參戰成功"; }
        }

        public string St6404_OutRegistrationTime
        {
            get { return "報名時間已過，不能報名"; }
        }

        public string St6413_HaveBeenModified
        {
            get { return "冠軍旗幟只能修改一次"; }
        }

        public string St6413_SantoVisit
        {
            get { return "忽見{0}城風雲變幻，原來是城主{1}光臨本城！"; }
        }

        public string St6405_FillInACharacter
        {
            get { return "旗幟只能填寫一個字元"; }
        }

        public string St6411_FailedToExit
        {
            get { return "公會城市爭鬥戰退出失敗"; }
        }

        public string St6409_fatigueDesc
        {
            get { return "當前疲勞值{0}，減少戰鬥力{1}%"; }
        }

        public string ChampionWelfare
        {
            get { return "恭喜您獲得公會佔領{0}城市的福利:{1}金幣，{2}晶石，{3}閱歷。祝您遊戲愉快！"; }
        }
        public string PackFull
        {
            get { return "背包格子已滿！"; }
        }
        public string EquipFull
        {
            get { return "裝備格子已滿！"; }
        }
        public string AbilityFull
        {
            get { return "魂技格子已滿！"; }
        }
        public string GeneralFull
        {
            get { return "裝備格子已滿！"; }
        }
        public string St12004_ChestKeyNotEnough
        {
            get { return "{0}不足"; }
        }

        public string St1442_SelectMercenaryUpgrade
        {
            get { return "請選擇傭兵升級"; }
        }

        public string St1442_SelectTheExperienceCard
        {
            get { return "請選擇經驗卡"; }
        }

        public string St_SystemMailTitle
        {
            get { return "系統"; }
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
            get { return "好友請求"; }
 
        }

        public string St_AskFirendTip
        {
            get { return "{0} 決定與您義結金蘭，從此以後雙方共同進退，結伴同行！"; }
        }


        public string St_FirendNotice
        {
            get { return "{0}已同意與您義結金蘭，從此你們將共同進退！"; }
        }

        public string St_FirendNoticeTip
        {
            get { return "您已發送請求！請等待回复！"; }
        }

        public string St_ShengJiTaTip
        {
            get { return "您在{0}的“勇闖聖吉塔”活動中名列{1}榜第{2}，排名獎勵{3}金幣已經發送到您的賬號中，請及時查收！"; }
        }

        public string St_ShengJiTaQintTong
        {
            get { return "青銅"; }
        }

        public string St_ShengJiTaBaiYin
        {
            get { return "白銀"; }
        }
        public string St_ShengJiTaHuangJin
        {
            get { return "黃金"; }
        }
        public string SportsRankLetterForWin { get; private set; }
        public string SportsRankLetterForFailure { get; private set; }


        #endregion


        #region 随机取名

        public string[] St_FirstNickNames
        {
            get
            {
                return new string[]{
   "趙","錢","孫","李","周","吳","鄭","王","馮","陳","褚","衛","蔣","沈","韓","楊","朱","秦","尤","許",
   "何","呂","施","張","孔","曹","嚴","華","金","魏","陶","姜","戚","謝","鄒","喻","柏","水","竇","章","雲","蘇","潘","葛","奚","范","彭","郎",
   "魯","韋","昌","馬","苗","鳳","花","方","俞","任","袁","柳","酆","鮑","史","唐","費","廉","岑","薛","雷","賀","倪","湯","滕","殷",
   "羅","畢","郝","鄔","安","常","樂","于","時","傅","皮","卞","齊","康","伍","餘","元","卜","顧","孟","平","黃","和",
   "穆","蕭","尹","姚","邵","湛","汪","祁","毛","禹","狄","米","貝","明","臧","計","伏","成","戴","談","宋","茅","龐","熊","紀","舒",
   "屈","項","祝","董","梁","杜","阮","藍","閔","席","季","麻","強","賈","路","婁","危","江","童","顏","郭","梅","盛","林","刁","鐘",
   "徐","邱","駱","高","夏","蔡","田","樊","胡","淩","霍","虞","萬","支","柯","昝","管","盧","莫","經","房","裘","繆","幹","解","應",
   "宗","丁","宣","賁","鄧","郁","單","杭","洪","包","諸","左","石","崔","吉","鈕","龔","程","嵇","邢","滑","裴","陸","榮","翁","荀",
   "羊","于","惠","甄","曲","家","封","芮","羿","儲","靳","汲","邴","糜","松","井","段","富","巫","烏","焦","巴","弓","牧","隗","山",
   "谷","車","侯","宓","蓬","全","郗","班","仰","秋","仲","伊","宮","甯","仇","欒","暴","甘","鈄","厲","戎","祖","武","符","劉","景",
   "詹","束","龍","葉","幸","司","韶","郜","黎","薊","溥","印","宿","白","懷","蒲","邰","從","鄂","索","鹹","籍","賴","卓","藺","屠",
   "蒙","池","喬","陰","鬱","胥","能","蒼","雙","聞","莘","党","翟","譚","貢","勞","逄","姬","申","扶","堵","冉","宰","酈","雍","卻",
   "璩","桑","桂","濮","牛","壽","通","邊","扈","燕","冀","浦","尚","農","溫","別","莊","晏","柴","瞿","閻","充","慕","連","茹","習",
   "宦","艾","魚","容","向","古","易","慎","戈","廖","庾","終","暨","居","衡","步","都","耿","滿","弘","匡","國","文","寇","廣","祿",
   "闕","東","歐","殳","沃","利","蔚","越","夔","隆","師","鞏","厙","聶","晁","勾","敖","融","冷","訾","辛","闞","那","簡","饒","空",
   "曾","毋","沙","乜","養","鞠","須","豐","巢","關","蒯","相","查","後","荊","紅","遊","郟","竺","權","逯","蓋","益","桓","公","仉",
   "督","岳","帥","緱","亢","況","郈","有","琴","歸","海","晉","楚","閆","法","汝","鄢","塗","欽","商","牟","佘","佴","伯","賞","墨",
   "哈","譙","篁","年","愛","陽","佟","言","福","南","火","鐵","遲","漆","官","冼","真","展","繁","檀","祭","密","敬","揭","舜","樓",
   "疏","冒","渾","摯","膠","隨","高","皋","原","種","練","彌","倉","眭","蹇","覃","阿","門","惲","來","綦","召","儀","風","介","巨",
   "木","京","狐","郇","虎","枚","抗","達","杞","萇","折","麥","慶","過","竹","端","鮮","皇","亓","老","是","秘","暢","鄺","還","賓",
   "閭","辜","縱","侴","萬俟","司馬","上官","歐陽","夏侯","諸葛","聞人","東方","赫連","皇甫","羊舌","尉遲","公羊","澹台","公冶","宗正",
   "濮陽","淳于","單于","太叔","申屠","公孫","仲孫","軒轅","令狐","鐘離","宇文","長孫","慕容","鮮於","閭丘","司徒","司空","兀官","司寇",
   "南門","呼延","子車","顓孫","端木","巫馬","公西","漆雕","車正","壤駟","公良","拓跋","夾谷","宰父","谷梁","段幹","百里","東郭","微生",
   "梁丘","左丘","東門","西門","南宮","第五","公儀","公乘","太史","仲長","叔孫","屈突","爾朱","東鄉","相裡","胡母","司城","張廖","雍門",
   "毋丘","賀蘭","綦毋","屋廬","獨孤","南郭","北宮","王孫","羽", "芳","月", "若", "叱吒","魔","幽","呂","仙"};
            }
        }

        public string[] St_LastNickNames
        {
            get
            {
                return new string[] { "偉", "剛", "勇", "毅", "俊", "峰", "強", "軍", "平", "保", "東", "文", "輝", "力", "明", "永", "健", "世", "廣", "志", "義", "興", "良", "海", "山", "仁", "波", "寧", "貴", "福", "生", "龍", "元", "全", "國", "勝", "學", "祥", "才", "發", "武", "新", "利", "清", "飛", "彬", "富", "順", "信", "子", "傑", "濤", "昌", "成", "康", "星", "光", "天", "達", "安", "岩", "中", "茂", "進", "林", "有", "堅", "和", "彪", "博", "誠", "先", "敬", "震", "振", "壯", "會", "思", "群", "豪", "心", "邦", "承", "樂", "紹", "功", "松", "善", "厚", "慶", "磊", "民", "友", "裕", "河", "哲", "江", "超", "浩", "亮", "政", "謙", "亨", "奇", "固", "之", "輪", "翰", "朗", "伯", "宏", "言", "若", "鳴", "朋", "斌", "梁", "棟", "維", "啟", "克", "倫", "翔", "旭", "鵬", "澤", "晨", "辰", "士", "以", "建", "家", "致", "樹", "炎", "德", "行", "時", "泰", "盛", "雄", "琛", "鈞", "冠", "策", "騰", "楠", "榕", "風", "航", "弘", "秀", "娟", "英", "華", "慧", "巧", "美", "娜", "靜", "淑", "惠", "珠", "翠", "雅", "芝", "玉", "萍", "紅", "娥", "玲", "芬", "芳", "燕", "彩", "春", "菊", "蘭", "鳳", "潔", "梅", "琳", "素", "雲", "蓮", "真", "環", "雪", "榮", "愛", "妹", "霞", "香", "月", "鶯", "媛", "豔", "瑞", "凡", "佳", "嘉", "瓊", "勤", "珍", "貞", "莉", "桂", "娣", "葉", "璧", "璐", "婭", "琦", "晶", "妍", "茜", "秋", "珊", "莎", "錦", "黛", "青", "倩", "婷", "姣", "婉", "嫻", "瑾", "穎", "露", "瑤", "怡", "嬋", "雁", "蓓", "紈", "儀", "荷", "丹", "蓉", "眉", "君", "琴", "蕊", "薇", "菁", "夢", "嵐", "苑", "婕", "馨", "瑗", "琰", "韻", "融", "園", "藝", "詠", "卿", "聰", "瀾", "純", "毓", "悅", "昭", "冰", "爽", "琬", "茗", "羽", "希", "欣", "飄", "育", "瀅", "馥", "筠", "柔", "竹", "靄", "凝", "曉", "歡", "霄", "楓", "芸", "菲", "寒", "伊", "亞", "宜", "可", "姬", "舒", "影", "荔", "枝", "麗", "陽", "妮", "寶", "貝", "初", "程", "梵", "罡", "恒", "鴻", "樺", "驊", "劍", "嬌", "紀", "寬", "苛", "靈", "瑪", "媚", "琪", "晴", "容", "睿", "爍", "堂", "唯", "威", "韋", "雯", "葦", "萱", "閱", "彥", "宇", "雨", "洋", "忠", "宗", "曼", "紫", "逸", "賢", "蝶", "菡", "綠", "藍", "兒", "翠", "煙", "小", "惜", "霸", "主", "郡", "魔", "幽", "多", "仙" };
            }
        }


        #endregion

        #region IGameLanguage 成员




        #endregion

        #region IGameLanguage 成员


        public string SportsRankLetterForFailureRank
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}