------------------------------------------------------------------
-- MainScene.lua
-- Author     :
-- Version    : 1.0.0.0
-- Date       :
-- Description:
------------------------------------------------------------------
local strModuleName = "MainScene";
module(strModuleName, package.seeall);
CCLuaLog("Module " .. strModuleName .. " loaded.");
strModuleName = nil;
require("layers.MainMenuLayer")
require("datapool.commonCallback")
require("layers.spriteBlessLayer")--精灵祝福界面

pMainScene=nil
local mAnimationLayer=nil

---
function setNotice(value)
	_isNotice = value
end;

function  getMainScene()
	return pMainScene
end;


---------------------------------------公有方法

function init()
	if pMainScene then
		return
	end
	 releaseResource()
	ScutScene:registerNetErrorFunc("MainScene.netConnectError");
	local scene =ScutScene:new();
	pMainScene = scene.root 
	scene:registerCallback(networkCallback)

	pWinSize=CCDirector:sharedDirector():getWinSize();
	SlideInLReplaceScene(pMainScene)
	 

--	pMainScene:setMultiTouchSupported(true)

	-- 添加主层
	mLayer= CCLayer:create()
	pMainScene:addChild(mLayer, -2)
	
	
	creatBgImage()--背景层
	
	createAnimationLayer()----飘动的光点
	

	
			

--	if PersonalInfo:getPersonalInfo()._WizardNum>0  then
--		createAccelerometer()--小精灵 摇一摇
--	end;
--[[
	if  _isNotice  then
		_isNotice = false
		PublicAnnouncementScene.init(pMainScene)
		MainMenuLayer.setIsPublish(true)
	end
--]]	
	MainMenuLayer.init(nil, pMainScene)	
	MainMenuLayer.setMainSceneId()
	MainMenuLayer.createMessageLayer()

	sendAction(1401)--玩家上阵佣兵    假的接口
--[[	
--]]

end

function createAccelerometer()
	mLayer:setIsAccelerometerEnabled(true)
	mLayer:registerDidAccelerate("MainScene.shakeItOff")
	isShake = true
end

function GMbutton()
	local topBtn=ZyButton:new("button/list_1161.png",nil,nil, "GM",FONT_NAME, FONT_SM_SIZE)
	topBtn:registerScriptHandler("MainScene.GMAction")
	topBtn:setAnchorPoint(PT(0,0))
	topBtn:setPosition(PT(pWinSize.width*0.85,pWinSize.height*0.2))
	topBtn:addto(mLayer, 0)
end

function GMAction()
	GMCtrl.init(mLayer)
end

--通用底图
function creatBgImage()
	local m_bgImage= CCSprite:create(P(Image.image_mainscene));
	m_bgImage:setScaleX(pWinSize.width/m_bgImage:getContentSize().width)
	m_bgImage:setScaleY(pWinSize.height/m_bgImage:getContentSize().height)
	m_bgImage:setAnchorPoint(PT(0.5,0.5))
	m_bgImage:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	mLayer:addChild(m_bgImage,-1)
end;

function onEnter()

end


--
function onExit()
 
end

function onPause()
	local type=ScutUtility.ScutUtils:GetPlatformType();--手机类型

end


function  onResume()
	local type=ScutUtility.ScutUtils:GetPlatformType();--手机类型
	if type~=5 then
--		ScutUtility.ScutUtils:cancelLocalNotifications()
	end
end

function releaseResource()
       spriteBlessLayer.releaseResource()
	MainMenuLayer.releaseResource()
	if mLayer then
--		mLayer:setIsAccelerometerEnabled(false)
		mLayer=nil
	end
	mAnimationLayer=nil
	pMainScene=nil

	
end

--确认退出游戏
function makeSureExitGame()
    CCDirector:sharedDirector():endToLua()
end

function closeApp()

	local  mMobileType ,mGameType,mRetailID=accountInfo. readMoble()
	local nType = ScutUtility.ScutUtils:GetPlatformType();
	if mRetailID=="0001"  and nType == ScutUtility.ptAndroid then
    		channelEngine.command("endGame")		
	else
		if not _isShowMsgBox then
			ZyLoading.releaseAll()
			_isShowMsgBox = true
			local runningScene = CCDirector:sharedDirector():getRunningScene()
			local box = ZyMessageBoxEx:new()
			box:doQuery(runningScene, nil,  Language.IDS_CLOSED_APP, Language.IDS_SURE, Language.IDS_CANCEL,closeAppMessageCallback);	
		end
	end
end

--
function  releaseAnimationLayer()
	if mAnimationLayer then
		mAnimationLayer:getParent():removeChild(mAnimationLayer,true)
		mAnimationLayer=nil
	end
end;
function stopMove(node)
	local moveX=math.random(0,pWinSize.width)
	local moveY=math.random(pWinSize.height*0.05,pWinSize.height*0.5)
	node:setPosition(PT(moveX,moveY))
	local moveX=math.random(0,pWinSize.width)
	local moveY=pWinSize.height*0.9
	local scale=math.random(2,10)
	node:setScale(scale*0.1)
	local move_time=math.random(5,20)
	local moveAct = CCMoveTo:create(move_time,PT(moveX,moveY));
	local fun=CCCallFuncN:create(stopMove)
	local action=CCSequence:createWithTwoActions(moveAct,fun)
	node:runAction(action)	
end;
--飘动的光点
function  createAnimationLayer()
	local layer=CCLayer:create()
	mAnimationLayer=layer
	pMainScene:addChild(layer,0)
	local num=40
	for k=1 , num do
		local sprite=CCSprite:create(P("mainUI/list_1141.png"))
		sprite:setAnchorPoint(PT(0,0))
		local scale=math.random(2,10)
		local startX=math.random(0,pWinSize.width)
		local startY=math.random(0,pWinSize.height)
		sprite:setScale(scale*0.1)
		sprite:setPosition(PT(startX,startY))
		layer:addChild(sprite,0)
		local moveX=math.random(0,pWinSize.width)
		local moveY=pWinSize.height*0.9
		local move_time=math.random(5,20)
		local moveAct = CCMoveTo:create(move_time,PT(moveX,moveY));
		local fun=CCCallFuncN:create(stopMove)
		local action=CCSequence:createWithTwoActions(moveAct,fun)
		sprite:runAction(action)
	end
end;



---------------------------------

--摇一摇
function shakeItOff(pAccelerationValue)
    local ggDefault =  (0.49 * 4.5) * (0.49 * 4.5)
    local gg = pAccelerationValue.x*pAccelerationValue.x +pAccelerationValue.y*pAccelerationValue.y+pAccelerationValue.z*pAccelerationValue.z
    if gg > ggDefault and isShake then
        drawCurioCabinet()
    end
end

function drawCurioCabinet()
	isShake = false
	local action = CCSequence:createWithTwoActions(
	CCDelayTime:create(0.1),
	CCCallFunc:create("MainScene.getRewards"))
	pMainScene:runAction(action)
end



-------------------------------------
function closeAppMessageCallback(clickedButtonIndex, userInfo, tag)
    if clickedButtonIndex == ID_MBOK then
   	 	local runningScene = CCDirector:sharedDirector():getRunningScene()
		actionLayer.Action1017(runningScene,false)
		if accountInfo.getRetailId()=="0036" and accountInfo.getMobileType()==5 then
        		channelEngine.sendSeed("0036", 1)
        	elseif accountInfo.getRetailId()=="0037" then
        		channelEngine.command("endGame")
	      end
	      CCDirector:sharedDirector():endToLua();      	 
   end
       _isShowMsgBox = false
end

function sendAction(actionId)
	if actionId == 1401 then
		local GeneralType = 3
		actionLayer.Action1401(pMainScene, nil, nil, GeneralType)
	elseif actionId==3013 then
		actionLayer.Action3013(pMainScene, nil)
	end
end;

--精灵祝福奖励
function getRewards()
	sendAction(3013)
end;


--网络连接失败
function netConnectError(pScutScene)
	ZyLoading.releaseAll()
--	if LoginScene.getEdit()~=nil then
--		LoginScene.getEdit():setVisible(true)
--	end
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1401 or actionId == 3009 then
		MainMenuLayer.networkCallback(pScutScene, lpExternalData)
	elseif actionId ==9202  then 
		PublicAnnouncementScene.networkCallback(pScutScene, lpExternalData)
	elseif actionId ==9204  then
		ChatScene._9204Callback(pScutScene, lpExternalData)	
	elseif actionId ==3013  then
		spriteBlessLayer._3013Callback(pScutScene, lpExternalData)
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)
	end
end