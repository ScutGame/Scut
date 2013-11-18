
 ZyButton = {
	_menuItem = nil,
	_label = nil,
	_menu = nil,
	_colorNormal = nil,
	_colorSelected = nil,
	_isSelected = nil,
 }




-- ´´½¨ÊµÀý
function ZyButton:new(picNor, picDown, picDis, title, fontName, fontSize)
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	
	local addLabel = true
	local label
	local label1
	local label2
	local menuItem
	local menu
	
	if fontName == nil then
		fontName = FONT_NAME
	end
	
	if fontSize == nil then
		fontSize = FONT_SM_SIZE
	end
	
	if picNor and picDown and picDis then
		menuItem = CCMenuItemImage:create(P(picNor), P(picDown), P(picDis))
	elseif picNor and picDown then
		menuItem = CCMenuItemImage:create(P(picNor), P(picDown))
	elseif picNor then
		local spriteNor = CCSprite:create(P(picNor))
		local spriteDown = CCSprite:create(P(picNor)) 
		if title then
			local size = ZyFont.stringSize(title, spriteNor:getContentSize().width, fontName, fontSize)
			label1 = CCLabelTTF:create(title, fontName, fontSize, size, kCCTextAlignmentCenter)
			label1:setPosition(CCPoint(spriteNor:getContentSize().width / 2, spriteNor:getContentSize().height / 2))
			spriteNor:addChild(label1, 0)
			
			label2 = CCLabelTTF:create(title , fontName, fontSize, size, kCCTextAlignmentCenter)
			label2:setPosition(CCPoint(spriteDown:getContentSize().width / 2, spriteDown:getContentSize().height / 2))
			spriteDown:addChild(label2, 0)
			spriteDown:setPosition(CCPoint(0, SY(-1)))
		else
			spriteDown:setScale(0.94)
			spriteDown:setPosition(CCPoint(spriteNor:getContentSize().width * 0.03, spriteNor:getContentSize().height * 0.03))
		end
		menuItem = CCMenuItemSprite:create(spriteNor, spriteDown)
		addLabel = false
	else
		menuItem = CCMenuItemLabel:create(instance._label)
		addLabel = false
	end
	
	if addLabel and title then
		if title then
			local size = ZyFont.stringSize(title, menuItem:getContentSize().width, fontName, fontSize)
			label = CCLabelTTF:create(title, fontName, fontSize, size, kCCTextAlignmentCenter)
		end
		label:setPosition(CCPoint(menuItem:getContentSize().width / 2, menuItem:getContentSize().height / 2))
		menuItem:addChild(label, 0)
	end

	menuItem:setAnchorPoint(CCPoint(0, 0))
	menu = CCMenu:createWithItem(menuItem)
	menu:setContentSize(menuItem:getContentSize())
	menu:setAnchorPoint(CCPoint(0, 0))
	
	instance._menuItem = menuItem
	instance._menu = menu
	instance._label = label
	instance._label1 = label1
	instance._label2 = label2
	instance._isSelected = false
	return instance
end

function  setScaleXY(frame,scaleX,scaleY)
		 if scaleX~=nil then
		 	frame:setScaleX(scaleX/frame:getContentSize().width)
		 end
		 if scaleY~=nil then
		 	frame:setScaleX(scaleY/frame:getContentSize().height)
		 end
end;




--
function ZyButton:addto(parent, param1, param2)
	if type(param1) == "userdata" then
		parent:addChildItem(self._menu, param1)
	else
		if param2 then
			parent:addChild(self._menu, param1, param2)
		elseif param1 then
			parent:addChild(self._menu, param1)
		else
			parent:addChild(self._menu, 0)
		end
	end
end

function  ZyButton:addChild(item,tag)
	self._menuItem:addChild(item,tag)
end;

function ZyButton:menuItem()
	return self._menuItem
end
--
function ZyButton:menu()
	return self._menu
end
--
function ZyButton:registerScriptHandler(handler)
    local fun=function() handler(self) end
	self._menuItem:registerScriptTapHandler(fun)
		
end
--
function ZyButton:setEnabled(enabled)
	self._menuItem:setEnabled(enabled)
end

function ZyButton:getIsEnabled()
	return self._menuItem:getIsEnabled()
end

function ZyButton:setVisible(enabled)
	self._menuItem:setVisible(enabled)
end
--
function ZyButton:setString(label)
    if self._label1 then
	    self._label1:setString(label)
    end
    if self._label2 then
	    self._label2:setString(label)
    end
    if self._label then
	    self._label:setString(label)
	end
end
--
function ZyButton:getString()
	return self._label:getString()
end
--
function ZyButton:setColor(color)
	if  self._label then
		self._label:setColor(color)
	end
    if self._label1 then
	    self._label1:setColor(color)
    end
    if self._label2 then
	    self._label2:setColor(color)
    end
end
function ZyButton:setColorNormal(color)
	self._colorNormal = color
	if not self._isSelected then
		self._label:setColor(color)
	end
end

function ZyButton:setColorSelected(color)
	self._colorSelected = color
	if self._isSelected then
		self._label:setColor(color)
	end
end
--
function ZyButton:setTag(tag)
	self._menuItem:setTag(tag)
end
--
function ZyButton:getTag(tag)
	return self._menuItem:getTag()
end
--
function ZyButton:setPosition(position)
	self._menu:setPosition(position)
end
--
function ZyButton:getPosition()
	return self._menu:getPosition()
end
--
function ZyButton:setAnchorPoint(point)
	self._menu:setAnchorPoint(point)
    self._menuItem:setAnchorPoint(point)
end
--
function ZyButton:getAnchorPoint(ponint)
	return self._menu:getAnchorPoint(point)
end

--
function ZyButton:setContentSize(size)
	self._menu:setContentSize(size)
end
--
function ZyButton:getContentSize()
	return self._menu:getContentSize()
end

--
function ZyButton:setScale(scale)
	return self._menu:setScale(scale)
end

--
function ZyButton:setScaleX(scale)
	return self._menu:setScaleX(scale)
end

--
function ZyButton:setScaleY(scale)
	return self._menu:setScaleY(scale)
end

--
function ZyButton:getScale()
	return self._menu:getScale()
end

--
function ZyButton:selected()
	self._isSelected = true
	self._menuItem:selected()
	if self._colorSelected then
		self._label:setColor(self._colorSelected)
	elseif self._colorNormal then
		self._label:setColor(self._colorNormal)
	end
end

--
function ZyButton:unselected()
	self._isSelected = false
	self._menuItem:unselected()
	if self._colorNormal then
		self._label:setColor(self._colorNormal)
	end
end

function ZyButton:remove()
	self._menu:getParent():removeChild(self._menu,true)
end;

function ZyButton:getParent()
       return self._menu:getParent()
end