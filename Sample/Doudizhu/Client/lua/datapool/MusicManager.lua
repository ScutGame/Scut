
-- MusicManager.lua
-- Author     :Lysong
-- Version    : 1.0.0.0
-- Date       :
-- Description:
------------------------------------------------------------------
require("datapool.SettingData")

EnumMusicType = {
	bgMusic 		= "sound/backgroundmusic.mp3",
	button 		= "sound/Button.mp3",
	buyao 		= "sound/buyao.mp3",--不要
	dani1  		= "sound/dani1.mp3",--大你1 接
	dani2  		= "sound/dani2.mp3",--大你2   接
	dani3  		= "sound/dani3.mp3",--大你3   接
	bujiao 		= "sound/fold.mp3",--不叫
	feiji   		= "sound/feiji.mp3",--飞机   接
	liandui 	= "sound/liandui.mp3",--连对   接
	lose 	= "sound/lose.mp3",--输   接
	menu 	= "sound/menu.mp3",--
	one 	= "sound/one.mp3",--一分
	pass 	= "sound/pass.mp3",--过   接
	rechoose 	= "sound/rechoose.mp3",--选牌  接
	sandaiyi 	= "sound/sandaiyi.mp3",--三带一  接
	sandaiyidui 	= "sound/sandaiyidui.mp3",--三带一对  接
	shunzi 	= "sound/shunzi.mp3",--顺子  接
	sidaier 	= "sound/sidaier.mp3",--四代二
	sidailiangdui 	= "sound/sidailiangdui.mp3",--四代两对
	start 	= "sound/start.mp3",--开始    接
	three 	= "sound/three.mp3",--三分
	two 	= "sound/two.mp3",--两分
	wangzha 	= "sound/wangzha.mp3",--王炸  接
	win 	= "sound/win.mp3",--赢  接
	xiaowang 	= "sound/xiaowang.mp3",--小王  接
	yaobuqi 	= "sound/yaobuqi.mp3",--要不起
	zhadan 	= "sound/zhadan.mp3",--炸弹  接
	chupai  = "sound/givecard.mp3",--出牌    接
}
local bInit = false
local mState={}
function resetMusicInit()
	stopMusic()
	bInit = false
end

function init()
	mState={}
	mState[2]={}
	mState[2].IsInUse=tonumber(accountInfo.getConfig("sys/config.ini", "SETTING2", "musicState2"))
	bInit = true
end

function playMusic(musicType)
	if  not bInit then
		init()
	end
	local setData = mState
	if setData[2].IsInUse == 1 then
--		CocosDenshion.SimpleAudioEngine:sharedEngine():playBackgroundMusic(P(musicType),true)
	end
end

function stopMusic()
--	CocosDenshion.SimpleAudioEngine:sharedEngine():stopBackgroundMusic(true)
end

function playEffect(effectType)
	if bInit == false then
		init()
	end
--	local setData = SettingData.getLocalData()
	local musicState1=tonumber(accountInfo.getConfig("sys/config.ini", "SETTING1", "musicState1"))
--	if setData[1].IsInUse == 1 then
	if  musicState1 ==nil or musicState1 == 1 then
--		CocosDenshion.SimpleAudioEngine:sharedEngine():playEffect(P(effectType), false);
	end
end
