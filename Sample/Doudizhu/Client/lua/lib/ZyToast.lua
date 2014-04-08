--提示的Toast 自动消息
require("lib.Common")
module("ZyToast", package.seeall)
local mLayer 			= nil

--设置默认的值
local mDefApperTimeSec  = 1.5
local mDefHideTimeSec  = 0.8

function getDefApperTime()
    return mDefApperTimeSec
end

function getDefHideTime()
    return mDefHideTimeSec
end

function show(parent, strText, apperTimeSec, hideTimeSec)
	if mLayer ~= nil then
	    hide()
	end
	if strText==nil or string.len(strText)<=0 then
		return
	end
	
	local showSec = mDefApperTimeSec
	if apperTimeSec ~= nil then
        showSec = apperTimeSec
    end
    local hideTime = mDefHideTimeSec
    if hideTimeSec ~= nil then
        hideTime = hideTimeSec
    end
	createToast(parent, showSec, hideTime, strText)
end

function createToast(parent, apperTime, delayTime, strText)

	mLayer = CCLayer:create()
	parent:addChild(mLayer, 9)
	local toumingBtn=ZyButton:new("common/tou_ming.9.png")
	
	toumingBtn:setScaleX(pWinSize.width/toumingBtn:getContentSize().width)
	toumingBtn:setScaleY(pWinSize.height/toumingBtn:getContentSize().height)
	toumingBtn:setAnchorPoint(PT(0,0))
	toumingBtn:setPosition(PT(0,0))
	toumingBtn:addto(mLayer,0)
	toumingBtn:registerScriptTapHandler(ZyToast.hide)


	--女子图
	--[[
	local girlSprite=CCSprite:create(P("common/list_4000_2.png"))
	girlSprite:setAnchorPoint(PT(0,0))
	mLayer:addChild(girlSprite, 1)
	--]]
	--背景框
	local bgSprite = CCSprite:create(P("common/panle_1069.png"))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	mLayer:addChild(bgSprite, 0)
	
	--文字
	
	local size =bgSprite:getContentSize()
	
--	
	local labelWidth=size.width*0.8
	local contentStr=string.format("<label>%s</label>",strText )
	local contentLabel = ZyMultiLabel:new(contentStr,labelWidth,FONT_NAME,FONT_SM_SIZE)
	local label=CCLabelTTF:create(strText,FONT_NAME,FONT_SM_SIZE)
	if label:getContentSize().width<labelWidth then
		labelWidth= label:getContentSize().width
	end
	contentLabel:addto(mLayer,0)
	--设置文字图大小	

	--[[
	local bgHeight=contentLabel:getContentSize().height+pWinSize.height*0.03>pWinSize.height*0.42
							and contentLabel:getContentSize().height+pWinSize.height*0.03 or pWinSize.height*0.42
	local size =SZ(pWinSize.width*0.55,bgHeight) 
	--]]
	--文字图拉伸
	bgSprite:setScaleX(size.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(size.height/bgSprite:getContentSize().height)
	
	--女子图设置位置
	--[[
	girlSprite:setPosition(PT(0, 
					bgSprite:getPosition().y-size.height/2+girlSprite:getContentSize().height*0.02))
	--]]
	
	--文字设置位置
	local startY=bgSprite:getPosition().y-size.height*0.1-contentLabel:getContentSize().height/2
	contentLabel:setPosition(PT(pWinSize.width/2-labelWidth/2, 
					startY))

	--居中显示
	mLayer:setAnchorPoint(PT(0,0))
	mLayer:setPosition(PT(0,0))

	local secondAction = CCSequence:createWithTwoActions(CCFadeOut:create(delayTime ),
			CCCallFuncN:create(ZyToast.hide))
	local  action = CCSequence:createWithTwoActions(CCDelayTime:create(apperTime),secondAction)
	mLayer:runAction(action)
	--mLayer:registerOnExit("ZyToast.onExit")	
	
end

function hide()
	if mLayer~= nil then
	    mLayer:getParent():removeChild(mLayer, true)
	    mLayer=nil
	end
end

function onExit()
    if mLayer~= nil then
        mLayer:stopAllActions()
        mLayer = nil
	end
end
---------------------
function show2(parent, strText, apperTimeSec, hideTimeSec)
	if mLayer ~= nil then
	    hide()
	end
	if strText==nil or string.len(strText)<=0 then
		return
	end
	
	local showSec = mDefApperTimeSec
	if apperTimeSec ~= nil then
        showSec = apperTimeSec
    end
    local hideTime = mDefHideTimeSec
    if hideTimeSec ~= nil then
        hideTime = hideTimeSec
    end
	createToast2(parent, showSec, hideTime, strText)
end
function createToast2(parent, apperTime, delayTime, strText,posState)
	if mLayer ~= nil then
	    hide()
	end
	mLayer = CCLayer:create()	
	--女子图
	--[[local girlSprite=CCSprite:create(P("common/list_4000_2.png"))
	girlSprite:setAnchorPoint(PT(0,0))
	mLayer:addChild(girlSprite, 1)]]
	
	--背景框
	local bgSprite = CCSprite:create(P("chat/panle_1045.png"))
	bgSprite:setScaleX(pWinSize.width*0.25/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height*0.4/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setPosition(PT(0,0))
	mLayer:addChild(bgSprite, 0)
	
	--文字
	local labelWidth=pWinSize.width*0.25*0.88
	local contentStr=string.format("<label color='%d,%d,%d' >%s</label>",0,0,0,strText )
	local contentLabel = ZyMultiLabel:new(contentStr,labelWidth,FONT_NAME,FONT_FM_SIZE)
	contentLabel:addto(mLayer,0)
	--设置文字图大小	
	local bgHeight=contentLabel:getContentSize().height+pWinSize.height*0.03>pWinSize.height*0.2
							and contentLabel:getContentSize().height+pWinSize.height*0.03 or pWinSize.height*0.2
	local size =SZ(pWinSize.width*0.3,bgHeight) 
	
	--文字图拉伸
--	bgSprite:setScaleX(size.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(size.height/bgSprite:getContentSize().height)

	
	--文字设置位置
--	local startY=bgSprite:getPosition().y+size.height-pWinSize.height*0.02 
	local startY=bgSprite:getPosition().y+size.height*0.75
	contentLabel:setPosition(PT(bgSprite:getPosition().x+pWinSize.width*0.037, 
					startY-contentLabel:getContentSize().height))
     if posState then
     	bgSprite:setScaleX(-1)
     		contentLabel:setPosition(PT(bgSprite:getPosition().x-bgSprite:getContentSize().width+pWinSize.width*0.01, 
					startY-contentLabel:getContentSize().height))
     end
	--居中显示
	mLayer:setAnchorPoint(PT(0,0))
	mLayer:setPosition(PT(0,0))

	local secondAction = CCSequence:createWithTwoActions(CCFadeOut:create(delayTime ),
			CCCallFuncN:create(ZyToast.hide))
	local  action = CCSequence:createWithTwoActions(
		CCDelayTime:create(apperTime),secondAction)
	mLayer:runAction(action)
	--mLayer:registerOnExit("ZyToast.onExit")	
	return mLayer
end
