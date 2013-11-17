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
	_UserType=nil, --用户状态
	_Pid=nil,--通行证ID
	_HeadIcon=nil,--玩家icon
	_NickName=nil,--玩家昵称
	_GameCoin      = 0,            --金豆
       _Gold=0,--元宝
       _VipLv=0,           --VIP等级
        _WinNum       =0,               --胜利局数
     	 _FailNum=0,--失败局数
    	_TitleName   =nil,     --称号
    	_ScoreNum     =0,               --积分
    	_WinRate=0,                    --胜率
	_roomTabel={},--房间信息
	_userID=nil,
_chatTable=nil;--存放9001聊天列表信息
}

npc_state_={}

function getPersonalInfo()
	return mPersonalInfo
end

function setPersonalInfo(info)
	mPersonalInfo._Pid=info.Pid
	mPersonalInfo._HeadIcon=info.HeadIcon
	mPersonalInfo._NickName=info.NickName
	mPersonalInfo._GameCoin=info.GameCoin
	mPersonalInfo._Gold=info.Gold
	mPersonalInfo._VipLv=info.VipLv
	mPersonalInfo._WinNum=info.WinNum
	mPersonalInfo._FailNum=info.FailNum
	mPersonalInfo._TitleName=info.TitleName
	mPersonalInfo._ScoreNum=info.ScoreNum
	mPersonalInfo._WinRate=info.WinRate
	mPersonalInfo._roomTabel=info.roomTabel
	mPersonalInfo._RoomId =info.RoomId 
end

