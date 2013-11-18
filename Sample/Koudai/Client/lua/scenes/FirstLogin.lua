------------------------------------------------------------------
-- FirstLogin.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("FirstLogin", package.seeall)




function releaseResource()
_layer = nil
_scene = nil
_num = nil
picLayer=nil
end

function initResource()
	_timeTable = { 5, 15, 15, 5, 10, 5}
	_picTable = { "donghua_1003", "donghua_1004", "donghua_1005", "donghua_1006", "donghua_1007", "donghua_1008" }
	_num = 1
	
end;
function init()
	initResource()
	local scene = ScutScene:new()
	_scene = scene.root
		_scene:registerScriptHandler(SpriteEase_onEnterOrExit)
	SlideInLReplaceScene(_scene)	

	_layer = CCLayer:create()
	_scene:addChild(_layer, 0)
	
	
	
	local toumingBtn=ZyButton:new("common/tou_ming.9.png","common/tou_ming.9.png")
	toumingBtn:setScaleX(pWinSize.width/toumingBtn:getContentSize().width)
	toumingBtn:setScaleY(pWinSize.height/toumingBtn:getContentSize().height)
	toumingBtn:addto(_layer,0)
	toumingBtn:setAnchorPoint(PT(0,0))
	toumingBtn:setPosition(PT(0,0))
	toumingBtn:registerScriptHandler(nextPicture)	
	
	showPicture()	
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end



function showPicture()
	if picLayer then
		picLayer:getParent():removeChild(picLayer, true)
		picLayer = nil
	end
	
	local layer = CCLayer:create()
	_layer:addChild(layer, 0)
	
	picLayer = layer
	
	local image = string.format("story/%s.jpg", _picTable[_num])
	local bgImage = CCSprite:create(P(image))
	bgImage:setAnchorPoint(PT(0,0))
	bgImage:setPosition(PT(0,0))
	bgImage:setScaleX(pWinSize.width/bgImage:getContentSize().width)	
	bgImage:setScaleY(pWinSize.height/bgImage:getContentSize().height)
	layer:addChild(bgImage, 0)

	layer:setTag(_num)

	local action= CCSequence:createWithTwoActions(CCDelayTime:create(_timeTable[_num]),
	CCCallFuncN:create(FirstLogin.keyAction))
	layer:runAction(action)		
end

function keyAction(pNode)
	local tag = pNode:getTag()
	if _num <= _num then
		nextPicture()
	end

end;

function nextPicture()
	_num = _num+1
	if _num <= #_picTable then
		showPicture()
	else
		MainScene.init()
	end
	
end;

