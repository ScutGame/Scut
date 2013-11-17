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
	toumingBtn:registerScriptHandler(hide)


	
	--女子图
	local girlSprite=CCSprite:create(P("common/list_4000_2.png"))
	girlSprite:setAnchorPoint(PT(0,0))
	mLayer:addChild(girlSprite, 1)
	
	--背景框
	local bgSprite = CCSprite:create(P("common/list_4000_1.9.png"))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	mLayer:addChild(bgSprite, 0)
	
	--文字
	local labelWidth=pWinSize.width*0.95-girlSprite:getContentSize().width
	local contentStr=string.format("<label>%s</label>",strText )
	local contentLabel = ZyMultiLabel:new(contentStr,labelWidth,FONT_NAME,FONT_DEF_SIZE)
	contentLabel:addto(mLayer,0)
	--设置文字图大小	
	local bgHeight=contentLabel:getContentSize().height+pWinSize.height*0.03>pWinSize.height*0.22
							and contentLabel:getContentSize().height+pWinSize.height*0.03 or pWinSize.height*0.22
	local size =SZ(pWinSize.width,bgHeight) 
	
	--文字图拉伸
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(size.height/bgSprite:getContentSize().height)
	
	--女子图设置位置
	girlSprite:setPosition(PT(0, 
					bgSprite:getPosition().y-size.height/2+girlSprite:getContentSize().height*0.02))
	--文字设置位置
	local startY=bgSprite:getPosition().y+size.height/2-pWinSize.height*0.02 
	contentLabel:setPosition(PT(girlSprite:getContentSize().width+pWinSize.width*0.03, 
					startY-contentLabel:getContentSize().height))

	--居中显示
	mLayer:setAnchorPoint(PT(0,0))
	mLayer:setPosition(PT(0,0))

	local secondAction = CCSequence:createWithTwoActions(CCFadeOut:create(delayTime ),
			CCCallFuncN:create(ZyToast.hide))
	local  action = CCSequence:createWithTwoActions(
		CCDelayTime:create(apperTime),secondAction)
	mLayer:runAction(action)
		mLayer:registerScriptHandler(SpriteEase_onEnterOrExit)
	--mLayer:registerOnExit("ZyToast.onExit")	
end
function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        onExit()
    end
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

