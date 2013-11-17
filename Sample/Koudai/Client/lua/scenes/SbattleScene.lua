
-- SbattleScene.lua.lua
-- Author     :Lysong
-- Version    : 1.0.0.0
-- Date       :
-- Description:
------------------------------------------------------------------

module("SbattleScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene= nil 
local mLayer=nil
local fightinfo={}

--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释



-- 场景入口
function init(info)
	 initResource()
	 createScene(info)
end


-- 退出场景
function closeScene()
	releaseResource()
	fightinfo={}
	CCDirector:sharedDirector():popScene()
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
end

-- 释放资源
function releaseResource()
end
-- 创建场景
function createScene(info)
	local scene = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	mScene = scene.root
	CCDirector:sharedDirector():pushScene(mScene)
	fightinfo=info
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	
	-- 此处添加场景初始内容
	--屏蔽按钮
	local actionBtn=UIHelper.createActionRect(pWinSize)
	actionBtn:setPosition(PT(0,0))
	mLayer:addChild(actionBtn,0)
	
	local image  = string.format("map/%s.jpg",fightinfo.BgScene)
	local imageBg = CCSprite:create(P(image))
	imageBg:setScaleX(pWinSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0,0))
	mLayer:addChild(imageBg, 0)	
	
	fightLayer = mLayer
	
	local battleType = 5
	local IsOverCombat = nil--1跳过
	PlotFightLayer.setFightInfo(fightinfo,IsOverCombat,battleType)
	
	PlotFightLayer.init(fightLayer)
	
end

--播放动画  胜利/失败
function playAnimation(skill, index, tag)
	local attribuAni=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(skill)
	if index then
		attribuAni:setCurAni(index)
	end
	if tag then
		attribuAni:setTag(tag)
	end
	attribuAni:play()
	attribuAni:registerFrameCallback("SbattleScene.finishAnimation")
	mLayer:addChild(attribuAni, 4)
	attribuAni:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.5))

end

--动画播放完成后 
function finishAnimation(pSprite, curAniIndex, curFrameIndex, nPlayingFlag)
	if nPlayingFlag == 2 then
		pSprite:registerFrameCallback("")
		delayRemove(pSprite)
--		battleOver()
	end
end

function delayRemove(sprite)
	if sprite ~= nil then
		local delayAct = CCDelayTime:create(0.1)
		local funcName = CCCallFuncN:create(SbattleScene.removeTmpSprite)
		local act = CCSequence:createWithTwoActions(delayAct,funcName)
		sprite:runAction(act)
		sprite = nil
	end
	
end

function removeTmpSprite(sprite)
	local tag = sprite:getTag()
	if sprite ~= nil then
		sprite:removeFromParentAndCleanup(true)
		sprite = nil
	end
    
    	
	if tag == 1 then
		closeFightLayer()
		PersonalInfo.getPersonalInfo().Shengjita=0
        	TrialScene.init()
	elseif tag == 2 then
		closeFightLayer()
		SBattleResult.createScene(PersonalInfo.getPersonalInfo().Score,PersonalInfo.getPersonalInfo().StarNum)
	end
end

function roundOver(info)
    PersonalInfo.getPersonalInfo().Score=info.Score
    PersonalInfo.getPersonalInfo().StarNum=info.StarNum
    isWin = false
	if info.IsWin == 0 then--失败
		playAnimation("donghua_1002_2", nil, 1)
	elseif info.IsWin == 1 then--成功
		isWin = true
		playAnimation("donghua_1002_1", nil, 2)
	end
end

function closeFightLayer()
	if fightLayer then
		fightLayer:getParent():removeChild(fightLayer, true)
		fightLayer = nil
	end
	closeScene()
	PlotFightLayer.releaseResource()
end;

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	
	
end