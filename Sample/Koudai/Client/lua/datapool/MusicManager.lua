------------------------------------------------------------------
-- lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :   
-- Description: “Ù¿÷π‹¿Ì,
------------------------------------------------------------------
require("datapool.SettingData")

EnumMusicType = {
	bgMusic 		= "sound/backgroundmusic.mp3",
	fightMusic 		= "sound/FightMusic.mp3",
	loseEffect 		= "sound/lose.mp3",
	winEffect  		= "sound/win.mp3",
	openBox  		= "sound/openbox.mp3",
	upgradeLose 	= "sound/upgrade_lose.mp3",
	upgradesuccess 	= "sound/upgrade_success.mp3",
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
	mState[2].IsInUse=tonumber(accountInfo.getConfig("sys/config.ini", "SETTING", "musicState"))
	bInit = true
end

function playMusic(musicType)
	if  not bInit then
		init()
	end
	local setData = mState
	if setData[2].IsInUse == 1 then
		--CocosDenshion.SimpleAudioEngine:sharedEngine():playBackgroundMusic(P(musicType),true)
	end
end

function stopMusic()
	--CocosDenshion.SimpleAudioEngine:sharedEngine():stopBackgroundMusic(true)
end

function playEffect(effectType)
	if bInit == false then
		init()
	end
	local setData = SettingData.getLocalData()
	if setData[1].IsInUse == 1 then
		--CocosDenshion.SimpleAudioEngine:sharedEngine():playEffect(P(effectType), false);
	end
end

