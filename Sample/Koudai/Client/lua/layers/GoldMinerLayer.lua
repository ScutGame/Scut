------------------------------------------------------------------
-- GoldMinerLayer.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("GoldMinerLayer", package.seeall)


function setLayer(scene, layer)
	fatherLayer = layer
	mScene = scene
end;
--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()

end

function close()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	if mLayer then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end
	releaseResource()
end

-- 释放资源
function releaseResource()
	mLayer = nil
	showLayer=nil
end

-- 创建场景
function init()

	initResource()

	local layer = CCLayer:create()
	fatherLayer:addChild(layer, 0)
	
	mLayer= layer
		
	-- 此处添加场景初始内容                  
	local bg1 = "activeBg/list_1075.jpg"
	local imageBg = CCSprite:create(P(bg1));
	imageBg:setScaleX(pWinSize.width*0.92/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height*0.7/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.04,pWinSize.height*0.2))
	mLayer:addChild(imageBg,0)
	
	
	sendAction(1011, 1)
end

function releseShowLayer()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
end

function showContent()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	showLayer = layer
	
	--金矿洞图标
	local titleBg = CCSprite:create(P(Image.image_touxiang_beijing))
	titleBg:setAnchorPoint(PT(0,0))
	titleBg:setPosition(PT(pWinSize.width*0.2, pWinSize.height*0.65))
	showLayer:addChild(titleBg, 0)
	activeImg = CCSprite:create(P("mainUI/list_1177.png"))
	activeImg:setAnchorPoint(PT(0.5,0.5))
	activeImg:setPosition(PT(titleBg:getPosition().x+titleBg:getContentSize().width*0.5, titleBg:getPosition().y+titleBg:getContentSize().height*0.5))
	showLayer:addChild(activeImg, 0)
	

	--提示语
	local talkBox=SZ(pWinSize.width*0.4, pWinSize.height*0.15)
	local talkSprite=CCSprite:create(P("common/list_1077.9.png"))
	talkSprite:setScaleX(talkBox.width/talkSprite:getContentSize().width)
	talkSprite:setScaleY(talkBox.height/talkSprite:getContentSize().height)
	talkSprite:setAnchorPoint(PT(0,0))
	talkSprite:setPosition(PT(pWinSize.width*0.4,
								pWinSize.height*0.62))
	showLayer:addChild(talkSprite,0)
	
	local tipStr=string.format("<label>%s</label>",Language.GOLD_NOTICE)
	local tipLabel=ZyMultiLabel:new(tipStr,talkBox.width*0.9,FONT_NAME,FONT_SM_SIZE);
	tipLabel:setPosition(PT(talkSprite:getPosition().x+talkBox.width*0.1,
								talkSprite:getPosition().y+talkBox.height*0.9-tipLabel:getContentSize().height))
	tipLabel:addto(showLayer,0)
	

	local noticeBg = CCSprite:create(P("common/list_1038.9.png"))
	noticeBg:setAnchorPoint(PT(0,0))
	noticeBg:setPosition(PT(pWinSize.width*0.05, pWinSize.height*0.39))
	noticeBg:setScaleX(pWinSize.width*0.9/noticeBg:getContentSize().width)
	noticeBg:setScaleY(pWinSize.height*0.22/noticeBg:getContentSize().height)
	showLayer:addChild(noticeBg, 0)
	if m_showText~= nil then
		local noticeStr=string.format("<label>%s</label>", m_showText)
		local xmlLabel = ZyMultiLabel:new(noticeStr,pWinSize.width*0.8,FONT_NAME,FONT_SM_SIZE);
		xmlLabel:setPosition(PT(pWinSize.width*0.1, pWinSize.height*0.59-xmlLabel:getContentSize().height))
		xmlLabel:addto(showLayer, 0)
	end
	
	--使用购买
	local getBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.GOLD_IN,FONT_NAME,FONT_SM_SIZE)
	getBtn:setAnchorPoint(PT(0,0))
	getBtn:setPosition(PT(pWinSize.width/2-getBtn:getContentSize().width*0.5,pWinSize.height*0.42))
	getBtn:registerScriptHandler(confirmGetMoney)
--	getBtn:setColorNormal(ZyColor:colorYellow())
	getBtn:addto(showLayer, 0)   	
end

function confirmGetMoney()
	sendAction(1011, 2)
end;

--发送请求
function sendAction(actionId, ops)
	if actionId == 1011 then
		actionLayer.Action1011(mScene, false, 1, ops)
	end
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	local actionId = ZyReader:getActionID();
	if actionId == 1011 then--晶石购买金币
		local result = ZyReader:getResult()
		if result == eScutNetSuccess then--购买成功
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
			releseShowLayer()
			sendAction(1011, 1)
		elseif result == 1 then--询问价格
			m_showText = ZyReader:readErrorMsg()
			showContent()
		elseif result == 2  then--次数用完
			local notice = ZyReader:readErrorMsg()--Language.GOLD_TIMEOUT
			local gapString= CCLabelTTF:create(notice,FONT_NAME,FONT_SM_SIZE)
			mLayer:addChild(gapString,3)
			gapString:setAnchorPoint(PT(0.5,0.5))
			gapString:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.5))				
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
		end
	end
end