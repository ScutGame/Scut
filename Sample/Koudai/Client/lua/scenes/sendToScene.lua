------------------------------------------------------------------
-- sendToScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description:  送精力
------------------------------------------------------------------

module("sendToScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

_scene = nil 		-- 场景
loveLayer = nil    -- 显示层
layerBG = nil  ---- 主层
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	createScene()
	CCDirector:sharedDirector():pushScene(_scene)
end
-- 退出场景
function popScene()
	releaseResource()
CCDirector:sharedDirector():popScene()
	releaseResource()
	MainScene.init()
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	_scene = nil 		-- 场景
	layerBG = nil  ---- 主层
	loveLayer =  nil  ----显示层
end
-- 释放资源
function releaseResource()
	_scene = nil 		-- 场景
layerBG = nil  ---- 主层

if layerBG then
		layerBG:getParent():removeChild(layerBG, true)
		layerBG = nil
	end
end
-- 创建场景
function createScene()
	local scene  = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	_scene = scene.root
	-- 添加背景
	layerBG = CCLayer:create()
	_scene:addChild(layerBG, 0)
	-- 注册触屏事件
	layerBG.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "sendToScene.touchBegan")
	layerBG.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "sendToScene.touchMove")
	layerBG.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "sendToScene.touchEnd")

	-- 此处添加场景初始内容
	local bigBgImg = CCSprite:create(P(Image.image_BIG));
	bigBgImg:setAnchorPoint(PT(0,0))
	bigBgImg:setPosition(PT(0,0))
	layerBG:addChild(bigBgImg, 0)
	
	local borderImg = CCSprite:create(P(Image.image_BIGBorder));
--	borderImg:setScaleX(pWinSize.width/sendBgImg:getContentSize().width)
	borderImg:setAnchorPoint(PT(0,0))
	borderImg:setPosition(PT(0,0))
	layerBG:addChild(borderImg, 0)
	
	

	
	--关闭按钮
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:registerScriptHandler(closeAction)						
	closeBtn:setAnchorPoint(PT(1,0))
	closeBtn:setPosition(PT(pWinSize.width-SX(2),
							pWinSize.height-closeBtn:getContentSize().height*1.1))	
	closeBtn:addto(layerBG,2)
	
	
	
	showLayer()
	
end
function showLayer()
	if loveLayer~=nil  then
			loveLayer:getParent():removeChild(loveLayer,true)
	end
	loveLayer = CCLayer:create()
	layerBG:addChild(loveLayer,1)
	
	
	
	local sendBgImgX =pWinSize.width*0.92
	local sendBgImg = CCSprite:create(P(Image.image_bgSendImg));
	sendBgImg:setScaleX(sendBgImgX/sendBgImg:getContentSize().width)
	sendBgImg:setScaleY(pWinSize.height*0.91/sendBgImg:getContentSize().height)
	
	sendBgImg:setAnchorPoint(PT(0,0))
	sendBgImg:setPosition(PT((pWinSize.width-sendBgImgX)/2,pWinSize.height*0.06))
	loveLayer:addChild(sendBgImg, 0)
	
	
	
	
	local loveImg = Image.image_loveHert
	--	local lovedImg = 
	
	local loveStr = Language.SEND_LOVE_ONE
	local loveButton = ZyButton:new(loveImg,nil,lovedImg,nil,FONT_NAME,FONT_SM_SIZE);
	loveButton:addto(loveLayer,0)
	loveButton:setTag(1)  																			
	loveButton:setAnchorPoint(PT(0,0))
	loveButton:setPosition(PT( pWinSize.width*0.07,pWinSize.height*0.42))
	loveButton:registerScriptHandler(onclick)
--	if  numberTime > 0 then 
	--	loveButton:setIsEnabled(false) ---是否点击
--	end	
	local loveStr = CCLabelTTF:create(loveStr,FONT_NAME,FONT_SM_SIZE)
	loveLayer:addChild(loveStr,1)
	loveStr:setAnchorPoint(PT(0.5,0))
	loveStr:setPosition(PT(loveButton:getPosition().x+loveButton:getContentSize().width*0.5,
						loveButton:getPosition().y+loveButton:getContentSize().height*0.65))
		


	
	local loveStr1 = Language.SEND_LOVE_TO
	local loveButtonTo = ZyButton:new(loveImg,nil,lovedImg,nil,FONT_NAME,FONT_SM_SIZE);
	loveButtonTo:addto(loveLayer,0)
	loveButton:setTag(2)  																			
	loveButtonTo:setAnchorPoint(PT(0,0))
	loveButtonTo:setPosition(PT(pWinSize.width*0.54,pWinSize.height*0.38))
	loveButtonTo:registerScriptHandler(onclick)
--	if  numberTime > 0 then 
--		loveButtonTo:setIsEnabled(false) ---是否点击
--	end	
	
	local loveStrTo= CCLabelTTF:create(loveStr1,FONT_NAME,FONT_SM_SIZE)
	loveLayer:addChild(loveStrTo,1)
	loveStrTo:setAnchorPoint(PT(0.5,0))
	loveStrTo:setPosition(PT(loveButtonTo:getPosition().x+loveButtonTo:getContentSize().width*0.5,
	loveButtonTo:getPosition().y+loveButtonTo:getContentSize().height*0.65))
	
	
	

end
function onclick()
	
end
function closeAction()
	popScene()
end
-- 触屏按下
function touchBegan(e)
end
-- 触屏移动
function touchMove(e)
end
-- 触屏弹起
function touchEnd(e)
end
-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = NdReader:getActionID()
	
	if actionID == 11001 then 
		local serverInfo=actionLayer._11001Callback(pScutScene, lpExternalData)
		if serverInfo~=nil then
			
		
		end
	end 
	if actionID == 11002 then
		local serverInfo1 = actionLayer._11002Callback(pScutScene, lpExternalData)
		if serverInfo1 ~= nil then 
		
		end 
	end
	if actionID == 11003 then
	    if ZyReader:getResult() == 1  then
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene, nil, ZyReader:readErrorMsg() , Language.doString_OK, Language.IDS_BACK,refreshButton) 
	    elseif     ZyReader:getResult() == 0 then
	    		actionLayer.Action11001(_scene,nil)	
	    else
		       ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
	    end
	end   
	
	
	local userData = ScutRequestParam:getParamData(lpExternalData)
end