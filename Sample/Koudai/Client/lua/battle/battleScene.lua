------------------------------------------------------------------
-- battleScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 战斗场景
------------------------------------------------------------------

module("battleScene", package.seeall)


mScene = nil 		-- 场景



function setPlotMapInfo(info, PlotID)
	_plotMapInfo=info
	_plotId=PlotID
	BattleResult.setData(PlotID, #_plotMapInfo.DemenInfo)
end;


-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
	releaseResource()
	if GuideLayer.judgeIsGuide(3) then--新手引导战斗退到主界面
		local _IsGuide,_GuideId, mTaskStep = GuideLayer.getIsGuide()
		GuideLayer.setIsGuide(_IsGuide, _GuideId, 1)
		MainScene.init()
	else
		PlotListScene:initResource()
		PlotListScene.init(PlotListScene.getPlotType())
	end
	
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	currentEnemyId=1  --当前的怪物索引
	isQuick = nil
	IsOverCombat = _plotMapInfo.IsOverCombat
end

-- 释放资源
function releaseResource()
	fightLayer=nil
	roundLabel = nil
	mLayer = nil
	isQuick = nil	isSend = nil
	playMusic(EnumMusicType.bgMusic)	
end

-- 创建场景
function createScene()
	local scene = ScutScene:new()


	scene:registerCallback(networkCallback)
	mScene = scene.root;
--	mScene:registerOnExit("battleScene.releaseResource")	
	SlideInLReplaceScene(mScene,1)

	playMusic(EnumMusicType.fightMusic)
	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)

	--大背景
	if _plotMapInfo and _plotMapInfo.BgScene  then
		bgImge =  string.format("map/%s.jpg", _plotMapInfo.BgScene )
	end
	
	local bgSprite=CCSprite:create(P(bgImge))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	mLayer:addChild(bgSprite,0)
	

	
	
	local label = CCLabelTTF:create(Language.PLOT_ROUND.."1/1", FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(PT(1,0))
	label:setPosition(PT(pWinSize.width*0.98, pWinSize.height-label:getContentSize().height*1.1))
	mLayer:addChild(label, 0)
	
	roundLabel = label
	
	
	shart()
	
end

function shart()
	if currentEnemyId <= #_plotMapInfo.DemenInfo then
		roundLabel:setString(Language.PLOT_ROUND..currentEnemyId.."/"..#_plotMapInfo.DemenInfo)
		closeFightLayer()
		if currentEnemyId <  #_plotMapInfo.DemenInfo then
			playAnimation("donghua_1003", currentEnemyId-1)
		else
			playAnimation("donghua_1003", 2)		
		end
	else
		playAnimation("donghua_1002_1", nil, 1)
	end
end

function sendAction()
	isWin = nil
	local plotNpcID  = _plotMapInfo.DemenInfo[currentEnemyId].MercenaryID
	actionLayer.Action4004(mScene, nil, plotNpcID)

	currentEnemyId = currentEnemyId+1
	
	isSend = true
	delayExec(battleScene.noBack, 10)
end


---延迟进行方法
function delayExec(funName,nDuration)
	local  action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFuncN:create(funName));
	if mLayer then
		mLayer:runAction(action)
	end
end

function noBack()
	if isSend then
		popScene()
	end
end

--一回合战斗播放结束
function roundOver(info)
	isWin = false
	if info.IsWin == 0 then--失败
		playAnimation("donghua_1002_2", nil, 2)
	elseif info.IsWin == 1 then--成功
		isWin = true
		closeFightLayer()
		shart()
	end
end

function isQuickEnd(info)
	isQuick = true
	isWin = false
	local string = nil
	local memberCallback = nil
	if info.IsWin == 0 then--失败
		playAnimation("donghua_1002_2",nil, 2)
	elseif info.IsWin == 1 and currentEnemyId <= #_plotMapInfo.DemenInfo then--成功
		sendAction()	
	else
		isWin = true
		playAnimation("donghua_1002_1",nil, 1)
	end
	


end



function nextFight()
	closeFightLayer()
	shart()
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
	attribuAni:registerFrameCallback("battleScene.finishAnimation")
	mLayer:addChild(attribuAni, 1)
	attribuAni:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.5))

end

--动画播放完成后 
function finishAnimation(pSprite, curAniIndex, curFrameIndex, nPlayingFlag)
	if nPlayingFlag == 2 then
		pSprite:registerFrameCallback("")
		delayRemove(pSprite)
	end
end

function delayRemove(sprite)
	if sprite ~= nil then
		local delayAct = CCDelayTime:create(0.1)
		local funcName = CCCallFuncN:create(battleScene.removeTmpSprite)
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
		BattleResult.init()
	elseif tag == 2 then
		popScene()
	else
		sendAction()
	end
end


----------------------------------





--------------------------------




-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 4004 then
		isSend = false
		local serverInfo=actionLayer._4004Callback(pScutScene, lpExternalData)
		if serverInfo~=nil then
			if isQuick then
				isQuickEnd(serverInfo)
			else
				startFight(serverInfo)
			end
		else
			local CloseButton = ZyButton:new("button/list_1046.png")
			CloseButton:setAnchorPoint(PT(1,0))
			CloseButton:setPosition(PT(pWinSize.width*0.975,pWinSize.height*0.9))	
			CloseButton:addto(mLayer,1)
			CloseButton:registerScriptHandler(popScene)			
		end	
	end
	
end


----------------开始战斗
function startFight(info)


   	 ScutAnimation.CScutAnimationManager:GetInstance():ReleaseAllAniGroup()

--	_layer:setIsTouchEnabled(false)
	
	if fightLayer then
		fightLayer:getParent():removeChild(fightLayer, true)
		fightLayer = nil
	end
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	fightLayer = layer

	PlotFightLayer.setFightInfo(info,IsOverCombat)

	PlotFightLayer.init(fightLayer)

end;

function closeFightLayer()
	if fightLayer then
		fightLayer:getParent():removeChild(fightLayer, true)
		fightLayer = nil
	end
	PlotFightLayer.releaseResource()
end;




