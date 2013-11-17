------------------------------------------------------------------
-- CommonActiveLayer.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("CommonActiveLayer", package.seeall)


mScene = nil 		-- 场景


--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	isClick = nil
end

-- 释放资源
function releaseResource()
	if  g_layer  then
		g_layer:getParent():removeChild(g_layer,true)
		g_layer = nil 		
	end
	isClick = nil
end


function init(mScene,faherLayer,table, haveBtn)

	initResource()

	g_scene = mScene
	g_activeTable = table
	isHaveBtn = haveBtn
	-- 添加背景
	g_layer = CCLayer:create()
	faherLayer:addChild(g_layer, 0)


	-- 此处添加场景初始内容
	-- 此处添加场景初始内容
	
	
	local BgImgW  = pWinSize.width*0.92
	local BgImgH = pWinSize.height*0.69
	local BgImg = CCSprite:create(P("activeBg/list_1150.jpg"))
	BgImg:setScaleX(BgImgW/BgImg:getContentSize().width)
	BgImg:setScaleY(BgImgH/BgImg:getContentSize().height)
	g_layer:addChild(BgImg,0)
	BgImg:setAnchorPoint(PT(0,0))
	BgImg:setPosition(PT(pWinSize.width*0.5-BgImgW/2,pWinSize.height*0.21))
	
	
--	local strImg = CCSprite:create(P("title/list_1160.png"))
--	g_layer:addChild(strImg,0)
--	strImg:setAnchorPoint(PT(0,0))
--	titleHeight = BgImg:getPosition().y+BgImgH-SY(50)	
--	strImg:setPosition(PT((pWinSize.width-strImg:getContentSize().width)/2,titleHeight))

	local titleLabel = CCLabelTTF:create(g_activeTable.FestivalName, FONT_NAME, FONT_BIG_SIZE)
	g_layer:addChild(titleLabel,0)
	titleLabel:setAnchorPoint(PT(0,0))
	titleHeight = BgImg:getPosition().y+BgImgH-SY(50)	
	titleLabel:setPosition(PT((pWinSize.width-titleLabel:getContentSize().width)/2,titleHeight))
	--titleLabel:setColor(ZyColor:colorBlueLight())
 
	showLayer()
	
	
	--是否有领取按钮
	if not isHaveBtn then
		return
	end
	local buttonStr = Language.ACTIVE_BUTTONSTR2
	if g_activeTable.IsReceive == 2 then
		buttonStr = Language.ACTIVE_HAVEGET
	end
	local gotoButton  = ZyButton:new(Image.image_button_red_c_0, Image.image_button_red_c_1, Image.image_button_hui_c, buttonStr, FONT_NAME,FONT_SM_SIZE)
	gotoButton:setAnchorPoint(PT(0.5,0.5))
	gotoButton:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.3))
	gotoButton:addto(g_layer,0)
	gotoButton:registerScriptHandler(getReward)
	if g_activeTable.IsReceive ~= 1 then--0：否 1：是 2：已领取
		gotoButton:setEnabled(false)
	end	

	
	
	
	
end

function showLayer()
		--活动时间
		local DoubleStr = string.format(Language.Active_Date,g_activeTable.StartDate,g_activeTable.EndDate)
		local activeStr= CCLabelTTF:create(DoubleStr,FONT_NAME,FONT_SM_SIZE)
		g_layer:addChild(activeStr,0)
		activeStr:setAnchorPoint(PT(0,0))
		activeStr:setPosition(PT(pWinSize.width*0.05,titleHeight-SY(20)))
		
		--活动内容
		local cotentStr = string.format(Language.Active_ContentStr,g_activeTable.FestivalDesc)
		local str = string.format("<label>%s</label>",cotentStr)
		local activeStr1 = ZyMultiLabel:new(str,pWinSize.width*0.9,FONT_NAME,FONT_SM_SIZE)
		activeStr1:addto(g_layer,0)
		activeStr1:setAnchorPoint(PT(0,0))
		activeStr1:setPosition(PT(activeStr:getPosition().x,activeStr:getPosition().y-activeStr1:getContentSize().height-SY(10)))
		
--		--有效次数
--		local doubleStirng = string.format(Language.DoubleIncome_str2,g_activeTable.RestrainNum)
--		local activeStr2= CCLabelTTF:create(doubleStirng,FONT_NAME,FONT_SM_SIZE)
--		g_layer:addChild(activeStr2,0)
--		activeStr2:setAnchorPoint(PT(0,0))
--		activeStr2:setPosition(PT(activeStr:getPosition().x,activeStr1:getPosition().y-SY(20)))
		
end


function getReward()
	if not isClick then
		isClick=false	
		local FestivalID = g_activeTable.FestivalID 
		actionLayer.Action3014(g_scene, nil, FestivalID)
	end
end


--发送请求
function sendAction(actionId)
	if actionId == 1 then


	elseif actionId == 2 then


	end

end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1 then


	elseif actionId == 2 then


	end
end