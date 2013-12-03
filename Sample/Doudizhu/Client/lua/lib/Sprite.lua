------------------------------------------------------------------
-- Sprite.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :   
-- Description: 精灵类,
------------------------------------------------------------------
Sprite = {
	_sprite = nil,
	_frames = nil,--当前用第几个动画
	_nameLabel=nil,
	_hpLabel=nil
 }
 
function Sprite:new(spr_name_string)
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	--------------
	local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(spr_name_string)
	spr:play()
	instance._sprite=spr
	instance._frames=0
	return instance
end;

--添加到层
function Sprite:addto(parent, param1)
	if param1 then
		parent:addChild(self._sprite,param1)
	else
		parent:addChild(self._sprite)
	end;
end;

function  Sprite:setScale(scale)
	self._sprite:setScale(scale)
end;

--设置坐标
function Sprite:setPosition(xx,yy)
	self._sprite:setPosition(PT(xx,yy))
end;

--设置是否显示
function Sprite:setIsVisible(parent)
	self._sprite:setIsVisible(parent)
end;

--设置帧序列
function Sprite:setCurAni(parent)
	self._sprite:setCurAni(parent)
	self._sprite:play()
	self._frames=parent
end;

function Sprite:setTag(tag)
	self._sprite:setTag(tag)
end;

function Sprite:play()
	self._sprite:play()
end

function Sprite:play_stop()
	self._sprite:play(false)
end

--设置精灵方向
function Sprite:setScaleX(parent)
	self._sprite:setScaleX(parent)
end;

function Sprite:setScaleY(parent)
	self._sprite:setScaleY(parent)
end;

--设置精灵方向
function Sprite:setFilpX(value)
	self._sprite:setFlipX(value)
end;

function Sprite:setFilpY(parent)
	self._sprite:setFilpY(parent)
end;


--当前第几个动画序列
function Sprite:getCurAni(parent)
	return self._frames
end;

--当前坐标
function Sprite:getPosition()
	return self._sprite:getPosition()
end;

--给出尺寸
function Sprite:getContentSize()
	return self._sprite:getContentSize()
end;

--给出精灵对象
function Sprite:get_spr()
	return self._sprite
end

--移动到哪里
function Sprite:move(xx,yy,sp,callbackfun)
	local x1=self._sprite:getPosition().x
	local y1=self._sprite:getPosition().y
	if xx==x1 and yy==y1 then
		return
	end
	-------
	local ww=nil
	local hh=nil
	if xx>x1 then
		ww=xx-x1
	else
		ww=x1-xx
	end
	if yy>y1 then
		hh=yy-y1
	else
		hh=y1-yy
	end;
	local speed= math.sqrt(ww*ww+hh*hh)
	--------
	local MoveTo = PT(xx,yy)
	local move_l=speed/sp
	local moveAct = CCMoveTo:actionWithDuration(move_l,MoveTo);
	if callbackfun~=nil then
		moveAct=CCSequence:actionOneTwo(moveAct, CCCallFuncN:actionWithScriptFuncName(callbackfun))
	end
	self._sprite:runAction(moveAct)
	return move_l
end;

function Sprite:setAnchorPoint(x,y)
        self._sprite:setAnchorPoint(PT(x,y))
end

function  Sprite:RunAction(act)
		self._sprite:runAction(act)
end;


function  Sprite:stopAction()
	self._sprite:stopAllActions()
end;

function  Sprite:registerFrameCallback(fun)
	self._sprite:registerFrameCallback(fun)
end;


--清除精灵
function Sprite:removeSprite()
    if self._sprite ~= nil then
        self._sprite:removeFromParentAndCleanup(true)
    end
end;
----------设置名字
function  Sprite:setLabel(name,x,y)
	local label=CCLabelTTF:create(name,FONT_NAME,FONT_FMM_SIZE)
	--local sprite=CCSprite:create(P("common/task_7.png"))
	self._nameLabel=label
	self._sprite:addChild(label,0)
	label:setAnchorPoint(PT(0.5,0))
	local startX=0
	local startY=self._sprite:getContentSize().height
	if x~=nil or y~=nil then
		startX=startX+x
		startY=startY+y
	end
	label:setPosition(PT(0,startY))
end;



--------------Npc点击
function Sprite:Rect()
    local Position = self._sprite:getPosition()
	local	ContentSize = self._sprite:getContentSize()
	local	AnchorPoint = self._sprite:getAnchorPoint()
    return CCRectMake( Position.x - ContentSize.width /4,
						Position.y - ContentSize.height * AnchorPoint.y,
						ContentSize.width/2, ContentSize.height);
end


function Sprite:isTouchInNpc(touchLocation)
	local pt= self._sprite:getParent():convertToNodeSpace(touchLocation)
	local rc = self:Rect()
	if CCRect:CCRectContainsPoint(rc, pt) then
		return true
	end
	return false
end;





