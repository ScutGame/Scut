
------------------------------------------------------------------
-- Human.lua
-- Author     : ChenJM
-- Version    : 1.15
-- Date       :
-- Description: 精灵类 创建人物,
------------------------------------------------------------------



Human = {
	_parent=nil,
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
	_SpriteState=4,
	_frames = nil,--当前用第几个动画
	_nameLabel=nil,
	_hpLabel=nil,
	_ememy=nil,
	_isPicture=nil,--是否使用了变身卡，1有（0没有，）
 }

 ---------0 攻击 1 被攻击  2 走 3 备战

 --------------------------------------------------------------------
 -- 创建实例
function Human:new()
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	return instance
end

function Human:setSpeed (speed)
		self._speed=speed
end;
function  Human:createPlayer(parent,spr_name_string,speed,name,layerTag,Tag)
	self.SpritePath=spr_name_string
	self._parent=parent
	self._speed=speed
	self._layer_num=layerTag
	self._name=name
	self._tag=Tag
	self:init()
end;

function  Human:createNpc(parent,spr_name_string,name,headSpritePath,StartX,StartY,layerTag)
	self._parent=parent
	self._Npc=true
	self.SpritePath=spr_name_string
	self._name=name
	self._headSpritePath=headSpritePath
	self._layer_num=layerTag
	self._taskState=0
	self._startX=StartX
	self._startY=StartY
	self:init()
end;

function  Human:createEnemy(parent,spr_name_string,name,layerTag,state)
	self.SpritePath=spr_name_string
	self._parent=parent
	self._layer_num=layerTag
	self._name=name
	self._SpriteState=state
	self._ememy=true
	self:init()
end;

--设置帧序列
function Human:setCurAni(tag)
 	self._sprite:setCurAni(tag)
	self._sprite:play()
	self._frames=tag
end;


--添加到层
function Human:addto(parent, param1)
	if param1==nil then
		param1=0
	end
	parent:addChild(self._sprite,param1)
end;

--移除
function Human:remove()
	if self._morningSprite~=nil then
		self._morningSprite:getParent():removeChild(self._morningSprite,true)
		self._morningSprite=nil
	end
    if self._sprite ~= nil then
        self._sprite:removeFromParentAndCleanup(true)
    end
end;


function  Human:getParent()
	return self._sprite:getParent()
end;
--获取精灵标签
function  Human:getZOrder()
	return self._sprite:getZOrder()
end;

--设置第几层
function Human:reorderChild(layer,param1)
	if self._layer_num~=param1 and param1~=nil then
		layer:reorderChild(self._sprite,param1)
		self._layer_num=param1
	end;
end;

function Human:setPosition(xx,yy)
	self._sprite:setPosition(xx,yy)
end;

function  Human:setOpacity(value)
	--self._sprite:setOpacity(value)
end;

--当前坐标
function Human:getPosition()
	return self._sprite:getPosition()
end;
--------------------------------------------------------------------------------

----------------初始化玩家
function  Human:init()
----动画精灵
	if self.SpritePath~=nil then
		------------非NPC
		if not self._Npc then
			local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(self.SpritePath)
			self._sprite=spr
			self._frames=0
			self:setCurAni(self._SpriteState)
			spr:play()
	--名字
			if self._name~=nil then
				self:setLabel(self._name)
			end
			if self._parent~=nil then
				self:addto(self._parent, self._layer_num)
			end
			if self._startX~=nil and self._startY~=nil then
				self:setPosition(self._startX,self._startY)
			end
		else
			local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(self.SpritePath)
			spr:setCurAni(0)
			spr:play()
		      self._sprite=spr
		      self:addto(self._parent, self._layer_num)
		      if self._startX~=nil and self._startY~=nil then
				self:setPosition(self._startX,self._startY)
			end
			if self._name~=nil then
				if not self._headSpritePath then
					self:setLabel(self._name)
		       	end
			end
		end
	end
 --玩家头像是否有图片
	if self._headSpritePath~=nil then
		local sprite=CCSprite:create(P(self._headSpritePath))
		self._headSprite=sprite
		sprite:setAnchorPoint(PT(0.5,0))
		sprite:setPosition(PT(self._sprite:getPosition().x,
								self._sprite:getPosition().y+self._sprite:getContentSize().height+SY(10)))
		self._parent:addChild(sprite,self._layer_num)
	end
end;

--攻击
function Human:attack()
	self:setCurAni(0)
	self._state=0;
end;

--挨打
function Human:hit()
	self:setCurAni(1)
	self._state=1;
end;

--走
function Human:run()
	self:setCurAni(2)
	self._state=2;
end;

--备战
function Human:ready()
	self:setCurAni(3)
	self._state=3;
end;


--停止
function Human:stop(tag)
	if tag==nil then
		tag=4
	end
	self:setCurAni(tag)
	self._state=tag;
end;

--
function Human:skillAttack(tag)
	if tag==nil then
		tag=5
	end
	self:setCurAni(tag)
	self._state=tag;
end;

function  Human:stopActionByTag(tag)
	self._sprite:stopActionByTag(tag)
end;


--播放
function Human:play()
	self._sprite:play()
end

--移动
function Human:move(moveX,moveY,callbackfun)
	local spriteX=self._sprite:getPosition().x
	local spriteY=self._sprite:getPosition().y
	if moveX~=spriteX or moveY~=spriteY then
		self._move_x=moveX
		self._move_y=moveY
		local distance=ccpDistance(CCPointMake(moveX, moveY), CCPointMake(spriteX, spriteY))
		local move_time=distance/getPlatFormScale(self._speed)
		local MoveTo = PT(moveX,moveY)
		local moveAct = CCMoveTo:actionWithDuration(move_time,MoveTo);
		if callbackfun~=nil then
			moveAct=CCSequence:actionOneTwo(moveAct, CCCallFuncN:actionWithScriptFuncName(callbackfun))
		end
		self:run()
		self:play()
		self._sprite:runAction(moveAct)
		local temp=-1
		if moveX>spriteX then
			temp=1
		end

				----------翅膀
			if self._wings then
				if temp==1 then
					self:setWingDrect(1)
				else
					self:setWingDrect(1)
				end
			end


		if temp~=self._direction then
			self._direction=temp
			self:setScaleX(self._direction)
		----------翅膀
			if self._wings then
				if temp==1 then
					self:setWingDrect(1)
				else
					self:setWingDrect(1)
				end
			end

			if self._nameLabel then
				self._nameLabel:setScaleX(self._direction)
			end
		end;
	end
end;


function Human:setTag(tag)
	self._tag=tag
end;

--设置第几层
function Human:set_Layer_num(num)
	self._layer_num=num
end;

--设置精灵方向
function Human:setleft()
	self._sprite:setScaleX(1)
	self._direction=1
end;

--设置精灵方向
function Human:setright()
	self._sprite:setScaleX(-1)
	self._direction=-1
end


function  Human:stopAction()
    if self._sprite then
	self._sprite:stopAllActions()
	--self._sprite:play(false)
	end
end;

--给出编号
function Human:getTag()
	return self._tag
end;

--设置是否显示
function Human:setIsVisible(parent)
	self._sprite:setIsVisible(parent)
end;

function Human:getPosition()
	return self._sprite:getPosition()
end;

--给出尺寸
function Human:getContentSize()
	return self._sprite:getContentSize()
end;

--在第几层
function Human:getLayer_num()
	return self._layer_num
end;

------
function Human:getstate()
	return self._state
end;

function Human:setScaleX (value)
	self._sprite:setScaleX(value)
	if self._nameLabel then
				self._nameLabel:setScaleX(value)
	end
	if self._direction~= value then
	    self._direction=value
	end
end;

--设置精灵方向
function Human:setScale(scale)
	self._sprite:setScale(scale)
end

function Human:setScaleY (value)
	self._sprite:setScaleY(value)
	if self._sprite._nameLabel then
		self._sprite._nameLabel:setScaleY(value)
	end
end;

--------------Npc点击
function Human:Rect(size,type)
    	local 	Position = self._sprite:getPosition()
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

----------------------------
function Human:Rect1()
    local Position = self._sprite:getPosition()
	local	ContentSize = self._sprite:getContentSize()
	local	AnchorPoint = self._sprite:getAnchorPoint()
    return CCRectMake( Position.x - ContentSize.width/4*3,
						Position.y - ContentSize.height * AnchorPoint.y,
						ContentSize.width*1.5, ContentSize.height);
end

-------------------
function Human:isTouchInNpc(touchLocation,type,size)
	local pt= self._sprite:getParent():convertToNodeSpace(touchLocation)
	local rc = self:Rect(size,type)
	if CCRect:CCRectContainsPoint(rc, pt) then
		return true
	end
	return false
end;

----------------------
function Human:getAnchorPoint()
    return self._sprite:getAnchorPoint()
end

---------------
function Human:getContentSize()
    return self._sprite:getContentSize()
end


---------执行动作
function  Human:runAction(act)
	self._sprite:runAction(act)
end;

--------------------
function  Human:setScale(scale)
	self._sprite:setScale(scale)
end;

function Human:setAnchorPoint(x,y)
	self._sprite:setAnchorPoint(PT(x,y))
end

--设置坐标
function Human:setPosition(xx,yy)
	self._sprite:setPosition(PT(xx,yy))
end;

--设置是否显示
function Human:setIsVisible(value)
	self._sprite:setIsVisible(value)
end;

function Human:play_stop()
	self._sprite:play(false)
end

--当前第几个动画序列
function Human:getCurAni()
	return self._frames
end;

--给出尺寸
function Human:getContentSize()
	return self._sprite:getContentSize()
end;

--给出精灵对象
function Human:get_spr()
	return self._sprite
end

-----修炼状态的动画
function  Human:createExperienceSprite()
	local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("list_3000_4")
	spr:play()
	self._experienceSprite=spr
	self._sprite:getParent():addChild(spr,self._sprite:getZOrder()-1)
	spr:setPosition(PT(self._sprite:getPosition().x,self._sprite:getPosition().y))
end;

function  Human:releaseExperienceSprite()
	if self._experienceSprite~=nil then
		self._experienceSprite:getParent():removeChild(self._experienceSprite,true)
		self._experienceSprite=nil
	end
end;

----------设置名字
function  Human:setLabel(name,x,y)
	local label=CCLabelTTF:create(name,FONT_NAME,FONT_SM_SIZE)
	--local sprite=CCSprite:create(P("common/task_7.png"))
	self._nameLabel=label
	self._sprite:addChild(label,0)
	label:setAnchorPoint(PT(0.5,0))
	local startX=0
	local startY=self._sprite:getContentSize().height+label:getContentSize().height/2
	if x~=nil or y~=nil then
		startX=startX+x
		startY=startY+y
	end
	label:setPosition(PT(0,startY))
end;

function  Human:registerFrameCallback(fun)
	self._sprite:registerFrameCallback(fun)
end;

----------------设置任务图标
function  Human:setTaskImg(state)
------------  0 原来的图标 1 感叹号 2灰色的问号 3 黄色的问号
	if self._headSprite~=nil then
		self._headSprite:removeFromParentAndCleanup(true)
		self._headSprite=nil
	end
	if state==0 then
		if self._headSpritePath~=nil then
			local sprite=CCSprite:create(P(self._headSpritePath))
			self._headSprite=sprite
			sprite:setAnchorPoint(PT(0.5,0))
			sprite:setPosition(PT(self._sprite:getPosition().x,
									self._sprite:getPosition().y+self._sprite:getContentSize().height+SY(10)))
			self._parent:addChild(sprite,self._layer_num)
		end
	elseif state~=nil then
		local taskImgTable={[1]="list_3000_1",[2]="list_3000_3",[3]="list_3000_2"}
		local imgPath=taskImgTable[state]
		local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(imgPath)
		spr:play()
		self._headSprite=spr
		self._sprite:getParent():addChild(spr,self._sprite:getZOrder())
		spr:setPosition(PT(self._sprite:getPosition().x,self._sprite:getPosition().y+self._sprite:getContentSize().height+SY(10)))
	end
end;

--------------
---------------------------------怪物说话dialogue
function  Human:setDialogue(str)
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

function  Human:releaseDialogue()
	if self._dialogBox~=nil then
		self._dialogBox:getParent():removeChild(self._dialogBox,true)
		self._dialogBox=nil
	end
	if self._dialogContent~=nil then
		self._dialogContent:getParent():removeChild(self._dialogContent,true)
		self._dialogContent=nil
	end
end;

-------------------------------气势满了
function  Human:createQishiAnimation()
    if self._QishiAnimation==nil then
	local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("icon_1411")
	spr:play()
	self._QishiAnimation=spr
	self._sprite:addChild(spr,0)
	spr:setPosition(PT(0,0))
    end
end;

function  Human:releaseQishiAnimation()
	if self._QishiAnimation~=nil then
		self._QishiAnimation:getParent():removeChild(self._QishiAnimation,true)
		self._QishiAnimation=nil
	end
end;

--15 眩晕16 昏睡 17冰冻 18迷失 19定身 20中毒21 出血  23混乱 24	绝对防御
function  Human:setEffectAnimation(state,direction)
	local stateEffectTable={[15]="list_2000_21",[16]="list_2000_21",[17]="list_2000_22",[18]="list_2000_21"
		,[19]="list_2000_21",[20]="list_2000_23",[21]="list_2000_24",[23]="list_2000_27",[24]="skill_3019"}
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
			local posY=self._sprite:getContentSize().height/2-spr:getContentSize().height/2
			spr:setPosition(PT(spr:getContentSize().width/2,posY))
			if direction==0 then
				spr:setScaleX(1)
			end
	end
end;

---------------------------------------------
function Human:releaseEffectAnimation()
	if self._EffectAnimation~=nil then
		self._EffectAnimation:getParent():removeChild(self._EffectAnimation,true)
		self._EffectAnimation=nil
	end
end;

function Human:setSpeed (speed)
		self._speed=speed
end;


------------
function Human:createaWings()
      if self._wings==nil then
      
	local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("animation_6668")
	spr:setCurAni(1)
	spr:play()
      local head=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("list_4000_6")
	head:play()
	
	self._wings=spr
	self._headPic=head
	local tag=self._sprite:getZOrder()
	self._sprite:addChild(spr,-1)
	self._sprite:addChild(head,0)
	spr:setPosition(PT(0,self._sprite:getContentSize().height*0.4))
	head:setPosition(PT(0,self._sprite:getContentSize().height))
    end
end


---------------------------
function Human:setWingDrect(dirc)
	if self._wings then
	local career=PersonalInfo.getPersonalInfo()._CareerID
	local sex=PersonalInfo.getPersonalInfo()._Sex+1
	--1	格斗家2	神枪手3	鬼剑士
	--1男2女
	local rolePosition={
	[1]={[1]=PT(-self._sprite:getContentSize().width*0.1,self._sprite:getContentSize().height*0.4),
		 [2]=PT(-self._sprite:getContentSize().width*0.2,self._sprite:getContentSize().height*0.45)},
		 
	[2]={[1]=PT(-self._sprite:getContentSize().width*0.13,self._sprite:getContentSize().height*0.4),
		 [2]=PT(-self._sprite:getContentSize().width*0.02,self._sprite:getContentSize().height*0.4)},	
		 
	[3]={[1]=PT(-self._sprite:getContentSize().width*0.05,self._sprite:getContentSize().height*0.4),
		 [2]=PT(-self._sprite:getContentSize().width*0.15,self._sprite:getContentSize().height*0.4)}, 

	}
	
	local headPosition={
	[1]={[1]=PT(self._headPic:getContentSize().width*0.7,self._sprite:getContentSize().height*0.95),
		 [2]=PT(self._headPic:getContentSize().width*0.3,self._sprite:getContentSize().height*0.95)},
		 
	[2]={[1]=PT(self._headPic:getContentSize().width*0.5,self._sprite:getContentSize().height*0.9),
		 [2]=PT(self._headPic:getContentSize().width*0.9,self._sprite:getContentSize().height*0.9)},	
		 
	[3]={[1]=PT(self._headPic:getContentSize().width*0.7,self._sprite:getContentSize().height*0.9),
		 [2]=PT(self._headPic:getContentSize().width*0.5,self._sprite:getContentSize().height*0.9)}, 
	}
	
	local positions={
	rolePosition[career][sex],
	PT(-rolePosition[career][sex].x,rolePosition[career][sex].y),
	PT(0,self._sprite:getContentSize().height*0.4)
	}
	local headPositions={
	headPosition[career][sex],
	PT(-headPosition[career][sex].x,headPosition[career][sex].y),
	PT(0,self._sprite:getContentSize().height)
	}
	
	local indexs={3,3,1,1}
	self._wings:setPosition(positions[dirc])
	self._headPic:setPosition(headPositions[dirc])
	if self._direction==1 then
		dirc=dirc+1
	end
	self._wings:setCurAni(indexs[dirc])
	self._wings:play()
	end
end

function Human:setSpriteTag(tag)
	 self._sprite:setTag(tag)
end;

------------
function Human:setCrown()
	if self.crown==nil then
		local sprite=CCSprite:create(P("mainUI/list_5008.png"))
		self.crown = sprite
		self._sprite:addChild(sprite,0)
		sprite:setAnchorPoint(PT(0.5,0))
		sprite:setPosition(PT(0,self._nameLabel:getPosition().y+self._nameLabel:getContentSize().height))
	end
end

-----公会晨练状态光效（临时）
function  Human:createMoriningSprite()
	if self._morningSprite==nil then
		local spr=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("icon_3005")
		spr:play()
		self._morningSprite=spr
		self._sprite:getParent():addChild(spr,self._sprite:getZOrder()-1)
		spr:setAnchorPoint(PT(0,0))
		spr:setPosition(PT(self._sprite:getPosition().x,self._sprite:getPosition().y+SY(10)))
	end
end;

function  Human:releaseMoriningSprite()
	if self._morningSprite~=nil then
		self._morningSprite:getParent():removeChild(self._morningSprite,true)
		self._morningSprite=nil
	end
end;

--是否使用了变身卡
function Human:setIsPicture(isPicture)
	self._isPicture = isPicture
end

function Human:getIsPicture()
	return self._isPicture
end;


