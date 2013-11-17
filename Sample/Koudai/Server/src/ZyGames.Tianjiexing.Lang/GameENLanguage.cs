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
using ZyGames.Base.BLL.Lang;

namespace ZyGames.Tianjiexing.Lang
{
    public class GameENLanguage : BaseENLanguage, IGameLanguage
    {
        #region IGameLanguage 成员

        public string St1002_GetRegisterPassportIDError
        {
            get { throw new NotImplementedException(); }
        }

        public string UserInfoError
        {
            get { throw new NotImplementedException(); }
        }

        public string LoadDataError
        {
            get { throw new NotImplementedException(); }
        }

        public short shortInt
        {
            get { throw new NotImplementedException(); }
        }

        public int GameUserGeneralID
        {
            get { throw new NotImplementedException(); }
        }

        public int SystemUserId
        {
            get { throw new NotImplementedException(); }
        }

        public string KingName
        {
            get { throw new NotImplementedException(); }
        }

        public string Color_Gray
        {
            get { throw new NotImplementedException(); }
        }

        public string Color_Green
        {
            get { throw new NotImplementedException(); }
        }

        public string Color_Blue
        {
            get { throw new NotImplementedException(); }
        }

        public string Color_PurPle
        {
            get { throw new NotImplementedException(); }
        }

        public string Color_Yellow
        {
            get { throw new NotImplementedException(); }
        }

        public string Color_Red
        {
            get { throw new NotImplementedException(); }
        }

        public string PaySuccessMsg
        {
            get { throw new NotImplementedException(); }
        }

        public string Date_Yesterday
        {
            get { throw new NotImplementedException(); }
        }

        public string Date_BeforeYesterday
        {
            get { throw new NotImplementedException(); }
        }

        public string Date_Day
        {
            get { throw new NotImplementedException(); }
        }

        public string NotLogin
        {
            get { throw new NotImplementedException(); }
        }

        public string GameMoney_Coin
        {
            get { throw new NotImplementedException(); }
        }

        public string GameMoney_Gold
        {
            get { throw new NotImplementedException(); }
        }

        public string St_User_LiveMsg
        {
            get { throw new NotImplementedException(); }
        }

        public string St_User_BeiBaoMsg
        {
            get { throw new NotImplementedException(); }
        }

        public string Load_User_Error
        {
            get { throw new NotImplementedException(); }
        }

        public string CacheUser_AddUserToCacheError
        {
            get { throw new NotImplementedException(); }
        }

        public string St_EnergyNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St_GoldNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St_InspireFailed
        {
            get { throw new NotImplementedException(); }
        }

        public string St_VipNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St_LevelNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St_GameCoinNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St_ExpNumNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St_ObtainNumNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St_LingshiNumNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St_NoFun
        {
            get { throw new NotImplementedException(); }
        }

        public string St_VipNotEnoughNotFuntion
        {
            get { throw new NotImplementedException(); }
        }

        public string St1004_PasswordMistake
        {
            get { throw new NotImplementedException(); }
        }

        public string St1004_PasswordError
        {
            get { throw new NotImplementedException(); }
        }

        public string St1004_IDNoLogin
        {
            get { throw new NotImplementedException(); }
        }

        public string St1004_IDLogined
        {
            get { throw new NotImplementedException(); }
        }

        public string St1004_UserIDError
        {
            get { throw new NotImplementedException(); }
        }

        public string St1004_IDDisable
        {
            get { throw new NotImplementedException(); }
        }

        public string St1005_RoleCheck
        {
            get { throw new NotImplementedException(); }
        }

        public string St1005_RoleExist
        {
            get { throw new NotImplementedException(); }
        }

        public string St1005_Professional
        {
            get { throw new NotImplementedException(); }
        }

        public string St1005_KingNameNotEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public string St1005_PassportError
        {
            get { throw new NotImplementedException(); }
        }

        public string St1005_KingNameTooLong
        {
            get { throw new NotImplementedException(); }
        }

        public string St1005_Rename
        {
            get { throw new NotImplementedException(); }
        }

        public string St1006_PasswordError
        {
            get { throw new NotImplementedException(); }
        }

        public string St1006_ChangePasswordError
        {
            get { throw new NotImplementedException(); }
        }

        public string St1006_PasswordTooLong
        {
            get { throw new NotImplementedException(); }
        }

        public string St1066_PayError
        {
            get { throw new NotImplementedException(); }
        }

        public string St1010_PayEnergyUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1010_JingliFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1013_JingliPrize
        {
            get { throw new NotImplementedException(); }
        }

        public string St1013_DailyJingliPrize
        {
            get { throw new NotImplementedException(); }
        }

        public string St1014_JingshiPrize
        {
            get { throw new NotImplementedException(); }
        }

        public string St1018_ExpObtainPrize
        {
            get { throw new NotImplementedException(); }
        }

        public string St1020_FengLu
        {
            get { throw new NotImplementedException(); }
        }

        public string St1011_PayCoinUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1011_PayGainCoin
        {
            get { throw new NotImplementedException(); }
        }

        public string St1011_WaJinKuangFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1104_UseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1107_UserItemNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1107_WarehouseNumFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1107_GridNumFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1108_WarehouseNumUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1214_ResetUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1214_ResetUseLingshi
        {
            get { throw new NotImplementedException(); }
        }

        public string St1215_OpenGridNumUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St4004_NoUseMagic
        {
            get { throw new NotImplementedException(); }
        }

        public string St4004_EmbattleEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public string St4004_GeneralLiveNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St4011_NoMonster
        {
            get { throw new NotImplementedException(); }
        }

        public string St1203_CareerError
        {
            get { throw new NotImplementedException(); }
        }

        public string St1204_Message
        {
            get { throw new NotImplementedException(); }
        }

        public string St1204_ColdTime
        {
            get { throw new NotImplementedException(); }
        }

        public string St1204_Colding
        {
            get { throw new NotImplementedException(); }
        }

        public string St1216_EnableSpartProperty
        {
            get { throw new NotImplementedException(); }
        }

        public string St1305_FateBackpackFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1305_BeiBaoBackpackFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1305_GainCrystalBackpack
        {
            get { throw new NotImplementedException(); }
        }

        public string St1305_HuntingIDLight
        {
            get { throw new NotImplementedException(); }
        }

        public string St1305_HighQualityNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St1307_FateBackpackFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1308_CrystalNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1308_CrystalLvFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1309_OpenNumNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1309_TheSameFate
        {
            get { throw new NotImplementedException(); }
        }

        public string St1204_EquMaxLv
        {
            get { throw new NotImplementedException(); }
        }

        public string St1204_EquGeneralMaxLv
        {
            get { throw new NotImplementedException(); }
        }

        public string St1310_UseCrystalGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1404_RecruitNotFilter
        {
            get { throw new NotImplementedException(); }
        }

        public string St1404_MaxGeneralNumFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1405_LiDuiNotFilter
        {
            get { throw new NotImplementedException(); }
        }

        public string St1407_MedicineNumFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1407_MedicineNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St1407_MedicineUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1409_maxTrainingNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St1405_GeneralIDNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1411_LessThanHalfAnHour
        {
            get { throw new NotImplementedException(); }
        }

        public string St1503_MaxMagicLv
        {
            get { throw new NotImplementedException(); }
        }

        public string St1503_MagicLevel
        {
            get { throw new NotImplementedException(); }
        }

        public string St1503_MagicEmbattleLevel
        {
            get { throw new NotImplementedException(); }
        }

        public string St1503_MagicColding
        {
            get { throw new NotImplementedException(); }
        }

        public string St1503_UpgradeExpNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St1503_MagicIDNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1603_MaterialsNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1603_EquNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1603_SynthesisEnergyNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St1605_BandageNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1605_BandageUse
        {
            get { throw new NotImplementedException(); }
        }

        public string St1605_UseTwoGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1606_GridNumNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St1606_BackpackFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1702_UseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1703_UseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St1703_QueueNumFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St1902_UserGeneralUnable
        {
            get { throw new NotImplementedException(); }
        }

        public string St2004_CountryName
        {
            get { throw new NotImplementedException(); }
        }

        public string St2004_CountryM
        {
            get { throw new NotImplementedException(); }
        }

        public string St2004_CountryH
        {
            get { throw new NotImplementedException(); }
        }

        public string St3002_LvNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St3002_MainNoCompleted
        {
            get { throw new NotImplementedException(); }
        }

        public string St3002_Completed
        {
            get { throw new NotImplementedException(); }
        }

        public string St3002_NotFind
        {
            get { throw new NotImplementedException(); }
        }

        public string St3002_NoAllowTaked
        {
            get { throw new NotImplementedException(); }
        }

        public string St3002_NoTaked
        {
            get { throw new NotImplementedException(); }
        }

        public string St3005_RefreashUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St3005_RefreashStarFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St3005_CompletedUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St3005_CompletedTimeout
        {
            get { throw new NotImplementedException(); }
        }

        public string St3007_NoCompleted
        {
            get { throw new NotImplementedException(); }
        }

        public string St3203_GoldNotEnouht
        {
            get { throw new NotImplementedException(); }
        }

        public string St3203_VipNotEnouht
        {
            get { throw new NotImplementedException(); }
        }

        public string St3203_RefeshPet
        {
            get { throw new NotImplementedException(); }
        }

        public string St3203_ZhaohuangPet
        {
            get { throw new NotImplementedException(); }
        }

        public string St3203_MaxPet
        {
            get { throw new NotImplementedException(); }
        }

        public string St3204_PetNoEnable
        {
            get { throw new NotImplementedException(); }
        }

        public string St3204_PetYaoqingNoPass
        {
            get { throw new NotImplementedException(); }
        }

        public string St3204_PetRunning
        {
            get { throw new NotImplementedException(); }
        }

        public string St3204_PetRunTimesOut
        {
            get { throw new NotImplementedException(); }
        }

        public string St3204_PetHelpeTimesOut
        {
            get { throw new NotImplementedException(); }
        }

        public string St3206_PetInterceptError
        {
            get { throw new NotImplementedException(); }
        }

        public string St3206_PetFriendError
        {
            get { throw new NotImplementedException(); }
        }

        public string St3206_PetInterceptTimesOut
        {
            get { throw new NotImplementedException(); }
        }

        public string St3206_PetInterceptFaild
        {
            get { throw new NotImplementedException(); }
        }

        public string St4002_PromptBlood
        {
            get { throw new NotImplementedException(); }
        }

        public string St4002_UserItemPromptBlood
        {
            get { throw new NotImplementedException(); }
        }

        public string St4002_EliteUsed
        {
            get { throw new NotImplementedException(); }
        }

        public string St4007_Saodanging
        {
            get { throw new NotImplementedException(); }
        }

        public string St4007_SaodangOver
        {
            get { throw new NotImplementedException(); }
        }

        public string St4007_BeiBaoTimeOut
        {
            get { throw new NotImplementedException(); }
        }

        public string St4008_Tip
        {
            get { throw new NotImplementedException(); }
        }

        public string St4012_JingYingPlot
        {
            get { throw new NotImplementedException(); }
        }

        public string St4012_JingYingPlotFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St4205_PlotNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St4205_InTeam
        {
            get { throw new NotImplementedException(); }
        }

        public string St4206_TeamPeopleFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St4206_NoTeam
        {
            get { throw new NotImplementedException(); }
        }

        public string St4206_TeamPlotStart
        {
            get { throw new NotImplementedException(); }
        }

        public string St4206_TeamPlotLead
        {
            get { throw new NotImplementedException(); }
        }

        public string St4208_IsCombating
        {
            get { throw new NotImplementedException(); }
        }

        public string St4210_PeopleNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St4210_PlotNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St4211_MorePlotReward
        {
            get { throw new NotImplementedException(); }
        }

        public string St4211_UserNotAddTeam
        {
            get { throw new NotImplementedException(); }
        }

        public string St4302_PlotRefresh
        {
            get { throw new NotImplementedException(); }
        }

        public string St5101_JingJiChangMingCheng
        {
            get { throw new NotImplementedException(); }
        }

        public string St5103_ChallengeNotNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St5107_Colding
        {
            get { throw new NotImplementedException(); }
        }

        public string St5106_JingJiChangRankReward
        {
            get { throw new NotImplementedException(); }
        }

        public string St5107_ChallGeNumFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St5107_JingJiChangOneRank
        {
            get { throw new NotImplementedException(); }
        }

        public string St5107_JingJiChangMoreNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St5107_JingJiChangWinNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St5107_ZuiGaoLianSha
        {
            get { throw new NotImplementedException(); }
        }

        public string St5107_ArenaWinsNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St5201_NoJoinCountry
        {
            get { throw new NotImplementedException(); }
        }

        public string St5201_CombatNoStart
        {
            get { throw new NotImplementedException(); }
        }

        public string St5201_CombatOver
        {
            get { throw new NotImplementedException(); }
        }

        public string St5202_InspireTip
        {
            get { throw new NotImplementedException(); }
        }

        public string St5202_InspireGoldTip
        {
            get { throw new NotImplementedException(); }
        }

        public string St5204_LifeNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St5402_CombatNoStart
        {
            get { throw new NotImplementedException(); }
        }

        public string St5204_CombatTransfusion
        {
            get { throw new NotImplementedException(); }
        }

        public string St5204_CombatNoStart
        {
            get { throw new NotImplementedException(); }
        }

        public string St5402_IsReliveError
        {
            get { throw new NotImplementedException(); }
        }

        public string St5403_CombatGoldTip
        {
            get { throw new NotImplementedException(); }
        }

        public string St5403_IsReLiveMaxNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St5403_IsLive
        {
            get { throw new NotImplementedException(); }
        }

        public string St5405_CombatWait
        {
            get { throw new NotImplementedException(); }
        }

        public string St5405_BossKilled
        {
            get { throw new NotImplementedException(); }
        }

        public string St5405_CombatOver
        {
            get { throw new NotImplementedException(); }
        }

        public string St5405_CombatKillReward
        {
            get { throw new NotImplementedException(); }
        }

        public string St5405_CombatHarmReward
        {
            get { throw new NotImplementedException(); }
        }

        public string St5405_CombatRankmReward
        {
            get { throw new NotImplementedException(); }
        }

        public string St5405_CombatNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St6006_AlreadyMember
        {
            get { throw new NotImplementedException(); }
        }

        public string St6006_ApplyGuild
        {
            get { throw new NotImplementedException(); }
        }

        public string St6006_ApplyMaxGuild
        {
            get { throw new NotImplementedException(); }
        }

        public string St6006_ApplyMember
        {
            get { throw new NotImplementedException(); }
        }

        public string St6006_GuildMemberNotDate
        {
            get { throw new NotImplementedException(); }
        }

        public string St6007_AuditPermissions
        {
            get { throw new NotImplementedException(); }
        }

        public string St6008_NotChairman
        {
            get { throw new NotImplementedException(); }
        }

        public string St6008_VicePresidentNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St6008_NotVicePresident
        {
            get { throw new NotImplementedException(); }
        }

        public string St6008_NotVicePresidentCeXiao
        {
            get { throw new NotImplementedException(); }
        }

        public string St6009_ContentNotEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public string St6009_ContentTooLong
        {
            get { throw new NotImplementedException(); }
        }

        public string St6010_Chairman
        {
            get { throw new NotImplementedException(); }
        }

        public string St6011_GuildMemberNotMember
        {
            get { throw new NotImplementedException(); }
        }

        public string St6012_HasIncenseToday
        {
            get { throw new NotImplementedException(); }
        }

        public string St6013_GainObtionNum
        {
            get { throw new NotImplementedException(); }
        }

        public string St6012_GuildShangXiang
        {
            get { throw new NotImplementedException(); }
        }

        public string St6015_SummonSanxian
        {
            get { throw new NotImplementedException(); }
        }

        public string St6017_UnionMembers
        {
            get { throw new NotImplementedException(); }
        }

        public string St6017_Rename
        {
            get { throw new NotImplementedException(); }
        }

        public string St6017_GuildNameNotEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public string St6017_GuildNameTooLong
        {
            get { throw new NotImplementedException(); }
        }

        public string St6017_GuildRename
        {
            get { throw new NotImplementedException(); }
        }

        public string St6019_GuildMaxPeople
        {
            get { throw new NotImplementedException(); }
        }

        public string St6019_AddGuild
        {
            get { throw new NotImplementedException(); }
        }

        public string St6022_GuildConvene
        {
            get { throw new NotImplementedException(); }
        }

        public string St6101_GuildBossNotOpen
        {
            get { throw new NotImplementedException(); }
        }

        public string St6101_GuildBossOver
        {
            get { throw new NotImplementedException(); }
        }

        public string St6101_GuildBossSet
        {
            get { throw new NotImplementedException(); }
        }

        public string St6105_CombatKillReward
        {
            get { throw new NotImplementedException(); }
        }

        public string St6105_CombatHarmReward
        {
            get { throw new NotImplementedException(); }
        }

        public string St6105_CombatRankmReward
        {
            get { throw new NotImplementedException(); }
        }

        public string St6109_GuildBossTime
        {
            get { throw new NotImplementedException(); }
        }

        public string St7004_BeiBaoTimeOut
        {
            get { throw new NotImplementedException(); }
        }

        public string St7004_QiShiNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St7005_HavePurchasedItem
        {
            get { throw new NotImplementedException(); }
        }

        public string St7007_UseSparRefreshGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St9003_AlreadyReceived
        {
            get { throw new NotImplementedException(); }
        }

        public string St9103_DoesNotExistTheUser
        {
            get { throw new NotImplementedException(); }
        }

        public string St9103_TheUserHasAFriendIn
        {
            get { throw new NotImplementedException(); }
        }

        public string St9103_TheUserHasTheFansIn
        {
            get { throw new NotImplementedException(); }
        }

        public string St9103_TheUserHasTheBlacklist
        {
            get { throw new NotImplementedException(); }
        }

        public string St9103_TheMaximumReachedAFriend
        {
            get { throw new NotImplementedException(); }
        }

        public string St9103_NotFriendsUserID
        {
            get { throw new NotImplementedException(); }
        }

        public string St9201_contentNotEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public string St9201_TheInputTextTooLong
        {
            get { throw new NotImplementedException(); }
        }

        public string St9203_ChatNotSend
        {
            get { throw new NotImplementedException(); }
        }

        public string St9203_ChaTypeNotGuildMember
        {
            get { throw new NotImplementedException(); }
        }

        public string St10001_ManorPlantingNotOpen
        {
            get { throw new NotImplementedException(); }
        }

        public string St10004_DewNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St10004_GeneralNotUserLv
        {
            get { throw new NotImplementedException(); }
        }

        public string St10005_Refresh
        {
            get { throw new NotImplementedException(); }
        }

        public string St10005_MaxQualityType
        {
            get { throw new NotImplementedException(); }
        }

        public string St10006_UserGeneralUpLv
        {
            get { throw new NotImplementedException(); }
        }

        public string St10006_DoesNotExistTheGeneral
        {
            get { throw new NotImplementedException(); }
        }

        public string St10007_DoRefresh
        {
            get { throw new NotImplementedException(); }
        }

        public string St10008_LandPostionIsOpen
        {
            get { throw new NotImplementedException(); }
        }

        public string St10008_OpenLandPostion
        {
            get { throw new NotImplementedException(); }
        }

        public string St10009_DewNumFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St10009_PayDewUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St10010_LandNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St10010_UpRedLandUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St10010_UpRedLandNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St10010_RedLandFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St10011_RedLandNotEnough
        {
            get { throw new NotImplementedException(); }
        }

        public string St10011_UpBlackLandUseGold
        {
            get { throw new NotImplementedException(); }
        }

        public string St10011_NotRedLand
        {
            get { throw new NotImplementedException(); }
        }

        public string St10011_BlackLandFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St11002_Colding
        {
            get { throw new NotImplementedException(); }
        }

        public string St11002_ExpeditionFull
        {
            get { throw new NotImplementedException(); }
        }

        public string St11003_DelCodeTime
        {
            get { throw new NotImplementedException(); }
        }

        public string St_FathersDay
        {
            get { throw new NotImplementedException(); }
        }

        public string St_DragonBoatFestival
        {
            get { throw new NotImplementedException(); }
        }

        public string St_DragonBoatZongzi
        {
            get { throw new NotImplementedException(); }
        }

        public string St_DragonBoatPuTongZongzi
        {
            get { throw new NotImplementedException(); }
        }

        public string St_HolidayFestival
        {
            get { throw new NotImplementedException(); }
        }

        public string St_HolidayFestivalGift
        {
            get { throw new NotImplementedException(); }
        }

        public string St_HolidayFestivalGoinGift
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerSecondNotice1
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerSecondNotice2
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerSecondNotice3
        {
            get { throw new NotImplementedException(); }
        }

        public string St_ObtionNumNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_ObtionTopThreeNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_GameCoinTopOneNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_GameCoinThreeNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_GameCoinTopTenNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_CombatNumTopOneNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_CombatNumTopThreeNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_CombatNumTopTenNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_LvTopTenNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_PlotRewardNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerThreeGameCoinNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerThreeObtionNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerThreeEnergyNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerThreeExpNumNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerThreeGoldNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerThreeExperienceNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerThreeItemNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerCrystalNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerComradesItemNotice
        {
            get { throw new NotImplementedException(); }
        }

        public string St_SummerLeveling
        {
            get { throw new NotImplementedException(); }
        }

        public string St_GameCoin
        {
            get { throw new NotImplementedException(); }
        }

        public string St_ObtionNum
        {
            get { throw new NotImplementedException(); }
        }

        public string Chat_PetRunSucess
        {
            get { throw new NotImplementedException(); }
        }

        public string Chat_PetInterceptSucess
        {
            get { throw new NotImplementedException(); }
        }

        public string Chat_PetWasBlocked
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}