
ZyArmy = {

	_parent=nil,
	_node=nil,
	_sprite = nil,
	_speed = 140,
	_layer_num = nil,--在第几层
	_name="",
	_tag = nil,
	_state = nil,--4站立2跑
	_move_x = nil,--目标坐标
	_move_y = nil,
	_direction = nil,--方向
	_headSpritePath=nil,
	_headSprite=nil,    --头像的图片
	_taskState=nil    ,   --任务状态
	_NameLabel=nil,
	_startX=nil,
	_startY=nil,
	_SpriteState=0,
	_frames = nil,--当前用第几个动画
	_nameLabel=nil,
	_hpLabel=nil,
	_ememy=nil,
	callbackfun="MoveOver"
 }

 ---------0 攻击 1 被攻击  2 走 3 备战

 --------------------------------------------------------------------
 -- 创建实例
function ZyArmy:new()
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	return instance
end


function  ZyArmy:createArmy(parent,spr_name_string,speed,armyName,armyNum,armyType,layerTag)
	self.SpritePath=spr_name_string
	self._parent=parent
	self._speed=speed
	self.armyName=armyName
	self.armyNum=armyNum
	self.armyType=armyType
	self._layer_num=layerTag
	self:init()
end;

--设置帧序列
function ZyArmy:setCurAni(tag)
	self._sprite:setCurAni(tag)
	self._sprite:play()
	self._frames=tag
end;


--添加到层
function ZyArmy:addto(parent, param1)
	if param1==nil then
		param1=0
	end
	parent:addChild(self._node,param1)
end;

--移除
function ZyArmy:remove()
    if self._node ~= nil then
        self._node:removeFromParentAndCleanup(true)
    end
end;


function  ZyArmy:getParent()
	return self._node:getParent()
end;

--获取精灵标签
function  ZyArmy:getZOrder()
	return self._node:getZOrder()
end;

--设置第几层
function ZyArmy:reorderChild(layer,param1)
	if self._layer_num~=param1 and param1~=nil  and self._node then
		layer:reorderChild(self._node,param1)
		self._layer_num=param1
	end;
end;

--------------
function ZyArmy:setPosition(xx,yy)
	self._node:setPosition(xx,yy)
end;

function  ZyArmy:setOpacity(value)
	self._node:setOpacity(value)
end;

--当前坐标
function ZyArmy:getPosition()
	return self._node:getPosition()
end;
--------------------------------------------------------------------------------

----------------初始化玩家
function  ZyArmy:init()
	if self.SpritePath~=nil then
			local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(self.SpritePath)
			local node=CCNode:node()
			node:setContentSize(spr:getContentSize())
			node:addChild(spr,0)

			self._sprite=spr
			self._node=node
			self._frames=0
			self:setCurAni(self._SpriteState)
			spr:play()
			if self.armyType then
				self:createArmyEffect()
			end

			if self.armyName then
				self:setArmyLayer()
			end

			if self._parent~=nil then
				self:addto(self._parent, self._layer_num)
			end

			if self._startX~=nil and self._startY~=nil then
				self:setPosition(self._startX,self._startY)
			end
	end
end;

function  ZyArmy:createArmyEffect()
	local bgPath={"plot/list_1099_1.png","plot/list_1099_2.png","plot/list_1099_3.png","plot/list_1099_3.png","plot/list_1099_4.png",}
	local imgePath=bgPath[self.armyType]
	if imgePath then
		local boxBg=CCSprite:spriteWithTexture(IMAGE(imgePath))
		boxBg:setAnchorPoint(PT(0,0))
		boxBg:setPosition(PT(-self._sprite:getContentSize().width*0.5,-self._sprite:getContentSize().height*0.1))
		self._node:addChild(boxBg,-1)	
	end
end;


function MoveOver(sprite)
sprite:play(false)
end

function ZyArmy:setRotation(value)
self._sprite:setRotation(value)
end;

function ZyArmy:setSkewX (value)
self._sprite:setSkewX (value)
end;

--攻击
function ZyArmy:attack()
	self:setCurAni(2)
	self._state=2;
end;

function ZyArmy:stop()
	self:setCurAni(0)
	self._state=0;
end;

--走
function ZyArmy:run()
	self:setCurAni(1)
	self._state=1;
end;

function  ZyArmy:stopActionByTag(tag)
	self._node:stopActionByTag(tag)
end;

--播放
function ZyArmy:play()
	self._sprite:play()
end

--移动
function ZyArmy:move(moveX,moveY,callbackfun,type)
	local spriteX=self._node:getPosition().x
	local spriteY=self._node:getPosition().y
	--if moveX~=spriteX or moveY~=spriteY then
		self._move_x=moveX
		self._move_y=moveY
		local distance=ccpDistance(CCPointMake(moveX, moveY), CCPointMake(spriteX, spriteY))
		local move_time=distance/getPlatFormScale(self._speed)
		local MoveTo = PT(moveX,moveY)

		local moveAct = CCMoveTo:actionWithDuration(move_time,MoveTo);
		local fun=self.callbackfun
		if callbackfun then
			fun= callbackfun
		end
		moveAct=CCSequence:actionOneTwo(moveAct, CCCallFuncN:actionWithScriptFuncName(fun))
		self:run()
		self:play()
		self._node:runAction(moveAct)
		local temp=-1
		if moveX>spriteX then
			temp=1
		end
		if temp~=self._direction then
			self._direction=temp
			self:setScaleX(self._direction)
			if self._nameLabel then
				self._nameLabel:setScaleX(self._direction)
			end
		end;
--	end
end;

function ZyArmy:setTag(tag)
	self._tag=tag
	self._sprite:stTag(tag)
end;

--设置第几层
function ZyArmy:set_Layer_num(num)
	self._layer_num=num
end;



function  ZyArmy:stopAction()
	self._node:stopAllActions()
end;

--给出编号
function ZyArmy:getTag()
	return self._tag
end;

--设置是否显示
function ZyArmy:setIsVisible(parent)
	self._sprite:setIsVisible(parent)
end;

function ZyArmy:getPosition()
	return self._node:getPosition()
end;

--给出尺寸
function ZyArmy:getContentSize()
	return self._sprite:getContentSize()
end;

--在第几层
function ZyArmy:getLayer_num()
	return self._layer_num
end;

------
function ZyArmy:getstate()
	return self._state
end;

function ZyArmy:setScaleX (value)
	self._sprite:setScaleX(value)
	if self._nameLabel then
				self._nameLabel:setScaleX(value)
	end
	if self.armyLayer then
	--	self.armyLayer:setScaleX(value)
	end
	if self._direction~= value then
	    self._direction=value
	end
end;

--设置精灵方向
function ZyArmy:setScale(scale)
	self._sprite:setScale(scale)
end

function ZyArmy:setScaleY (value)
	self._sprite:setScaleY(value)
	if self._sprite._nameLabel then
		self._sprite._nameLabel:setScaleY(value)
	end
end;

--------------Npc点击
function ZyArmy:Rect(size,type)
    local Position = self._sprite:getPosition()
	local	ContentSize = self._sprite:getContentSize()
	local	AnchorPoint = self._sprite:getAnchorPoint()
	if size~=nil then
		ContentSize=size
	end
	if self._headSprite~=nil then
		ContentSize.height=ContentSize.height+SY(40)
	end
	if type then
		    return CCRectMake( Position.x,
						Position.y - ContentSize.height * AnchorPoint.y,
						ContentSize.width, ContentSize.height);
	else
		    return CCRectMake( Position.x-ContentSize.width/4,
						Position.y - ContentSize.height * AnchorPoint.y,
						ContentSize.width/2, ContentSize.height);
	end

end

function ZyArmy:Rect1()
    local Position = self._sprite:getPosition()
	local	ContentSize = self._sprite:getContentSize()
	local	AnchorPoint = self._sprite:getAnchorPoint()
    return CCRectMake( Position.x - ContentSize.width/4*3,
						Position.y - ContentSize.height * AnchorPoint.y,
						ContentSize.width*1.5, ContentSize.height);
end


function ZyArmy:isTouchInNpc(touchLocation,type,size)

	local pt= self._sprite:getParent():convertToNodeSpace(touchLocation)
	local rc = self:Rect(size,type)
	if CCRect:CCRectContainsPoint(rc, pt) then
		return true
	end
	return false
end;

function ZyArmy:getAnchorPoint()
    return self._sprite:getAnchorPoint()
end

function ZyArmy:getContentSize()
    return self._sprite:getContentSize()
end

---------执行动作
function  ZyArmy:runAction(act)
	self._node:runAction(act)
end;

function  ZyArmy:setScale(scale)
	self._sprite:setScale(scale)
end;

--设置坐标
function ZyArmy:setPosition(xx,yy)
	self._node:setPosition(PT(xx,yy))
end;

--设置是否显示
function ZyArmy:setIsVisible(value)
	self._node:setIsVisible(value)
end;


function ZyArmy:getIsVisible()
	return self._node:getIsVisible()
end;


function ZyArmy:setTag(tag)
	 self._tag=tag
	 self._sprite:setTag(tag)
end;


function ZyArmy:play_stop()
	self._sprite:play(false)
end

--当前第几个动画序列
function ZyArmy:getCurAni()
	return self._frames
end;

--给出尺寸
function ZyArmy:getContentSize()
	return self._sprite:getContentSize()
end;

--给出精灵对象
function ZyArmy:get_spr()
	return self._sprite
end

----------设置名字
function  ZyArmy:setLabel(name,x,y)
	local label=CCLabelTTF:create(name,FONT_NAME,FONT_SM_SIZE)
	self._nameLabel=label
	self._sprite:addChild(label,0)
	label:setAnchorPoint(PT(0.5,0))
	local startX=0
	local startY=self._sprite:getContentSize().height/3*2
	if x~=nil or y~=nil then
		startX=startX+x
		startY=startY+y
	end
	label:setPosition(PT(0,startY))
end;


--部队信息
function  ZyArmy:setArmyLayer(x,y)
	local layer=CCLayer:create()
	local nameLabel=CCLabelTTF:create(self.armyName,FONT_NAME,FONT_FMM_SIZE)
	nameLabel:setColor(ccleginoGreen)
	self.armyNameLabel=nameLabel
	layer:addChild(nameLabel,0)
	layer:setContentSize(SZ(self._sprite:getContentSize().width,nameLabel:getContentSize().height*2))
	self.armyLayer=layer
	local numLabel=CCLabelTTF:create(self.armyNum,FONT_NAME,FONT_FMM_SIZE)
	self.numLabel=numLabel
	layer:addChild(numLabel,0)
	numLabel:setAnchorPoint(PT(0.5,0))
	numLabel:setPosition(PT(self._sprite:getContentSize().width/2,0))
	nameLabel:setAnchorPoint(PT(0.5,0))
	nameLabel:setPosition(PT(self._sprite:getContentSize().width/2,numLabel:getContentSize().height))
	self._node:addChild(layer,0)
	layer:setAnchorPoint(PT(0,0))
	local startX=0
	local startY=self._sprite:getContentSize().height/3
	if x~=nil or y~=nil then
		startX=startX+x
		startY=startY+y
	end
	layer:setPosition(PT(-self._sprite:getContentSize().width/2,startY))
end;

function  ZyArmy:setArmyNum(num)
	if self.numLabel and num then
		self.numLabel:setString(num)
	end
end;

function  ZyArmy:registerFrameCallback(fun)
	self._sprite:registerFrameCallback(fun)
end;

--------------
---------------------------------怪物说话dialogue
function  ZyArmy:setDialogue(str)
    self:releaseDialogue()
	local sprite=CCSprite:create(P("common/list_3002.9.png"))
	local size=ZyFont.stringSize(str,sprite:getContentSize().width*0.9,FONT_NAME, FONT_FMM_SIZE)
	local spriteSize=sprite:getContentSize()
	local lb = CCLabelTTF:create(str, size, CCTextAlignmentLeft, FONT_NAME, FONT_FMM_SIZE)
	lb:setAnchorPoint(PT(0,1))
	local dialogueHeight=size.height+SY(20)
	if dialogueHeight>sprite:getContentSize().height then
			sprite:setScaleY(dialogueHeight/sprite:getContentSize().height)
			spriteSize.height=dialogueHeight
	end
	sprite:setAnchorPoint(PT(1,0))
	sprite:setPosition(PT(self._sprite:getPosition().x-self._sprite:getContentSize().width/2,
	self._sprite:getPosition().y+self._sprite:getContentSize().height))
	lb:setPosition(PT(sprite:getPosition().x-spriteSize.width+SX(5),
						sprite:getPosition().y+spriteSize.height-SY(5)))
	self._dialogBox=sprite
	self._dialogContent=lb
	self._parent:addChild(sprite,self._layer_num)
	self._parent:addChild(lb,self._layer_num+1)
end


function  ZyArmy:releaseDialogue()
	if self._dialogBox~=nil then
		self._dialogBox:getParent():removeChild(self._dialogBox,true)
		self._dialogBox=nil
	end
	if self._dialogContent~=nil then
		self._dialogContent:getParent():removeChild(self._dialogContent,true)
		self._dialogContent=nil
	end
end;

function ZyArmy:showSkillEffect(id,fun,type)
	
	local  spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(id)
	self._skillSprite =spr
	spr:play()
	self._sprite:addChild(spr,0)
	spr:registerFrameCallback(fun)
	local index=1
	if type then
		index=type
	end
	local pos={
	[1]=PT(spr:getContentSize().width/8,0),
	[2]=PT(spr:getContentSize().width/16,-self._sprite:getContentSize().width/4),
	[3]=PT(-self._sprite:getContentSize().width/2,self._sprite:getContentSize().width/8),
	[4]=PT(0,self._sprite:getContentSize().width/32),
	[5]=PT(-self._sprite:getContentSize().width/16,-self._sprite:getContentSize().width/2),
	}
	spr:setPosition(pos[index])
end;


-------------------------------气势满了
function  ZyArmy:createQishiAnimation()
    if self._QishiAnimation==nil then
	local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("icon_1411")
	spr:play()
	self._QishiAnimation=spr
	self._sprite:addChild(spr,0)
	spr:setPosition(PT(0,0))
	end
end;

function  ZyArmy:releaseQishiAnimation()
	if self._QishiAnimation~=nil then
		self._QishiAnimation:getParent():removeChild(self._QishiAnimation,true)
		self._QishiAnimation=nil
	end
end;


--15 眩晕16 昏睡 17冰冻 18迷失 19定身 20中毒21 出血  23混乱 24	绝对防御
function  ZyArmy:setEffectAnimation(state,direction)
	local stateEffectTable={[15]="list_2000_21",[16]="list_2000_21",[17]="list_2000_22",[18]="list_2000_21"
	,[19]="list_2000_21",[20]="list_2000_23",[21]="list_2000_24",[23]="list_2000_21",[24]="skill_3019"}
	local path=stateEffectTable[state]
	if path==nil then
		return
	end
	self:releaseEffectAnimation()
	local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(path)
	spr:play()
	self._EffectAnimation=spr
	self._sprite:addChild(spr,0)
	spr:setPosition(PT(0,self._sprite:getContentSize().height))
	if state==24 then
			local posY=self._sprite:getContentSize().height/4
			spr:setPosition(PT(0,posY))
			if direction==0 then
				spr:setScaleX(1)
			end
	end
end;

function ZyArmy:releaseEffectAnimation()
	if self._EffectAnimation~=nil then
		self._EffectAnimation:getParent():removeChild(self._EffectAnimation,true)
		self._EffectAnimation=nil
	end
end;
