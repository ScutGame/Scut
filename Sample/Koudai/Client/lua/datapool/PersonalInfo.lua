------------------------------------------------------------------
-- PersonalInfo.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :   
-- Description: 角色信息类,
------------------------------------------------------------------
---保存用户基本的数据信息---
require("lib.lib")
module("PersonalInfo", package.seeall)
my_ID=10000

local mPersonalInfo = {
	-----角色信息
	_CityID=nil,--城镇ID
	_PointX=nil,
	_PointY=nil,
	_GeneralID=nil, --佣兵id
	_NickName      = nil,            --角色名称
	_GuideId=nil,
	_UserType=nil,           --用户类型
	_UserLv        =0,               --角色等级
	_CareerID=nil,--职业ID
	_Sex   =nil,     --性别0男1女
	_HeadID     =1,               --角色头像
	_GoldNum=0,                    --晶石
	_GameCoin=0  ,                 --金币
	_LifeNum=0 ,                   --当前生命
	_MaxLifeNum=0,                 --当前生命上限
	_CountryID=0,
	_CountryID  =0,    --国家ID
	_ItemLiveNum =0,       
	_ItemLiveMaxNum=0,   
	_WizardNum=0, --精灵数量
	
	_EnergyNum=0,
	_MaxEnergyNum=0,
	_CurrExperience=0,
	_MaxExperience=0,
	_VipLv=0,
	_userID=nil,
	equip_level_MAX=90,  --装备强化最大等级
	_sprite=nil,--角色精灵
	_speed=nil,--角色移动速度
	_GuildID=nil,--公会ID GuildID
	_DataTabel_3003=nil,--
	_UserLocation=nil,--玩家所在地  1：城市 2：集会所
	_BlessTable=nil,
	_UserExp = nil, --玩家阅历
	_UserStatus= nil,--玩家状态UserStatus
	_PlotID= nil,--副本ID
	_PictureID=nil,--变身卡变身图片
	_PictureTime =nil--变身卡剩余变身时间
	,_Pid,
	VipNextCold=0,
	_SurplusEnergy=nil,
	FunEnum = nil,--活动Id
	_Wings=nil,
	_PlotStatusID = nil,--副本id
	_MercenarySeq = nil,--出场次序
	_CardUserID=nil,--迎新卡	
	_IsChampion=nil,--是否跨服战冠军
	Shengjita=nil,--是否是圣吉塔返回
	Exchange=nil,--可兑换属性
	Receive=nil,--可领取奖励
	EffNum=nil,--4402进入单层界面要传的参数
	fightinfo=nil,--战斗信息，用于再来一次
	Score=nil,--
	StarNum=nil,--
	HasNextBoss=nil,--考古中是否有下个boss
	g_currentActiveID=nil,--考古中记录上次选中
	plotid=nil,--记住遗迹选的是哪个图
	fuben_1=nil,--记住是那个副本地图
}

npc_state_={}

function getPersonalInfo()
	return mPersonalInfo
end

function setPersonalInfo(info)
	--城市ID
	mPersonalInfo._CityID=info.CityID;	
	--佣兵id
	mPersonalInfo._GeneralID=info.GeneralID;	
	--公会ID
	mPersonalInfo._GuildID=info.GuildID;	  
	--昵称
	mPersonalInfo._NickName=info.NickName;
	--等级
	if mPersonalInfo._UserLv~=nil and 
		mPersonalInfo._UserLv~=0 and
		info.UserLv-mPersonalInfo._UserLv>0 
	then
--		MainMapLayer.playerLevelUp()
	end
	mPersonalInfo._UserLv=info.UserLv;
	---职业id
	mPersonalInfo._CareerID=info.CareerID;
	mPersonalInfo._Sex=info.Sex;
	mPersonalInfo._HeadID=info.HeadID;
	--晶石
	mPersonalInfo._GoldNum=info.GoldNum;
	--金币
	mPersonalInfo._GameCoin=info.GameCoin;
	--当前生命
	mPersonalInfo._LifeNum=info.LifeNum		
	--当前生命上限
	mPersonalInfo._MaxLifeNum=info.MaxLifeNum;
	--当前精力
	mPersonalInfo._EnergyNum=info.EnergyNum;
	--当前精力上限
	mPersonalInfo._MaxEnergyNum=info.MaxEnergyNum;
	--	当前经验
	mPersonalInfo._CurrExperience=info.CurrExperience;
	--	当级经验上限
	mPersonalInfo._MaxExperience=info.MaxExperience;
	--	Vip等级
	mPersonalInfo._VipLv=info.VipLv;
	--	所在国家
	mPersonalInfo._CountryID=info.CountryID;
	--	血量包剩余值
	mPersonalInfo._ItemLiveNum=info.ItemLiveNum;
	--	血量包最大值
	mPersonalInfo._ItemLiveMaxNum=info.ItemLiveMaxNum;
	--祝福
	mPersonalInfo._BlessTable=info.BlessTable;
	---玩家所在地
	mPersonalInfo._UserLocation=info.UserLocation
	--	玩家阅历
	mPersonalInfo._UserExp = info.UserExp
	--	玩家状态
	mPersonalInfo._UserStatus = info.UserStatus
	--	扫荡副本ID
	mPersonalInfo._PlotID = info.PlotID
	--	变身卡
	mPersonalInfo._PictureID=info.PictureID	
	--变身卡,时间是否用完
	mPersonalInfo._PictureTime=info.PictureTime
	-----	
	mPersonalInfo.VipNextCold=info.DemandGold
	mPersonalInfo._SurplusEnergy=info.SurplusEnergy
	if info.IsHelper and info.IsHelper~=0  then
		accountInfo.setHelpShow(info.IsHelper)
	end
	---
	--副本id
	mPersonalInfo._PlotStatusID=info.PlotStatusID
	--副本出场次序
	mPersonalInfo._MercenarySeq=info.MercenarySeq
	
	--拉新卡号
	mPersonalInfo._CardUserID=info.CardUserID
	
	--是否跨服战冠军
	mPersonalInfo._IsChampion = info.IsChampion
	
	--上阵佣兵
	mPersonalInfo._BattleNum = info.BattleNum
	
	--总上阵佣兵
	mPersonalInfo._TotalBattle = info.TotalBattle
	

	--体力恢复时间 
	mPersonalInfo._Rstore  = info.Rstore 
	
	--全部体力恢复时间
	mPersonalInfo._TotalRstore  = info.TotalRstore

	mPersonalInfo._HonourNum = info.HonourNum
	
	mPersonalInfo._NextHonourNum = info.NextHonourNum
	
	
	mPersonalInfo._CombatNum = info.CombatNum
	
	mPersonalInfo._TalPriority = info.TalPriority
	
	mPersonalInfo._IsLv = info.IsLv

	mPersonalInfo._EnergyNum = info.EnergyNum
	
	mPersonalInfo._unReadCount = info.unReadCount
	
	mPersonalInfo._WizardNum = info.WizardNum
	
	
	mPersonalInfo.Shengjita = info.Shengjita
	
	mPersonalInfo.Exchange = info.Exchange
	
	mPersonalInfo.Receive = info.Receive
	
	mPersonalInfo.EffNum = info.EffNum
	
	mPersonalInfo.fightinfo = info.fightinfo
	
	mPersonalInfo.Score=info.Score
	
	mPersonalInfo.StarNum=info.StarNum
	
	mPersonalInfo.HasNextBoss=info.HasNextBoss
	
	mPersonalInfo.g_currentActiveID=info.g_currentActiveID
	
	mPersonalInfo.plotid=info.plotid
	
end


function  setTaskBackValue(serverInfo)
    if serverInfo~=nil then
	if  serverInfo.UserLv-mPersonalInfo._UserLv>0 
	then
			MainMapLayer.playerLevelUp()
	end
	mPersonalInfo._UserLv=serverInfo.UserLv;
	mPersonalInfo._LifeNum=serverInfo.LifeNum;
	mPersonalInfo._MaxLifeNum=serverInfo.MaxLifeNum;
	mPersonalInfo._CurrExperience=serverInfo.CurrExperience;
	mPersonalInfo._MaxExperience=serverInfo.MaxExperience;
	mPersonalInfo._GameCoin=serverInfo.GameCoin;
	end
end;

function setUserStatus(nStatus)
    if mPersonalInfo~= nil then
        mPersonalInfo._UserStatus = nStatus
    end
    
end

function setFunEnum(nActiveId)
    if mPersonalInfo~= nil then
        mPersonalInfo.FunEnum = nActiveId
    end
end

function setNickName(nNickName)
    if mPersonalInfo~= nil then
        mPersonalInfo._NickName = nNickName
    end
end;

function setPictureId(nPictureID)
    if mPersonalInfo~= nil then
        mPersonalInfo._PictureID = nPictureID
    end
end;

function setUserLocation(nUserLocation)
    if mPersonalInfo~= nil then
        mPersonalInfo._UserLocation = nUserLocation
    end
end;

function setPlotInfo(nPlotStatusID, nMercenarySeq)
	if mPersonalInfo~= nil then
		mPersonalInfo._PlotStatusID = nPlotStatusID
		--副本出场次序
		mPersonalInfo._MercenarySeq = nMercenarySeq
	end
end

function  getCityXY(CityId)
     if mPersonalInfo._Citys~=nil and #mPersonalInfo._Citys>0 then
         for k, v in ipairs(mPersonalInfo._Citys) do
              if v.CityID==CityId then
                 return v.PointX,v.PointY
              end
         end
     end
end;

--小人的Event事件
local mRandomEvent = {}

function getRendomEventlst()
	return mRandomEvent
end

function pushEvent(EventItem)
	ZyTable.push_back(mRandomEvent, EventItem)
end

function removeEvent(pos)
	ZyTable.remove(pos)
end

function setMoney(goin, gold)--金币，晶石
	--晶石
	mPersonalInfo._GoldNum=gold
	--金币
	mPersonalInfo._GameCoin=goin

end

