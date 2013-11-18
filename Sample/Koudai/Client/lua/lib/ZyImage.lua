
 ZyImage = {
	_image = nil,
	_scaleX = 1,
	_scaleY = 1,
 }

-- 创建实例
function ZyImage:new(param)
	local instance = {}
	if type(param) == "string" then
		instance._image = self:imageWithFile(param)
	elseif type(param) == "userdata" then
		instance._image = param
	end
	instance._image:setAnchorPoint(CCPoint(0, 0))
	setmetatable(instance, self)
	self.__index = self
	return instance
end


function ZyImage:resize(size)
	local oldSize = self._image:getContentSize()
	self._scaleX =  (size.width / oldSize.width)
	self._scaleY = (size.height / oldSize.height)
	self._image:setScaleX(self._scaleX)
	self._image:setScaleY(self._scaleY)
	self._image:setContentSize(size)
end

function ZyImage:scale(scale)
	local size = self:getContentSize()
	size.width = size.width * scale
	size.height = size.height * scale
	self:resize(size)
end

function ZyImage:scaleX(scale)
	local size = self:getContentSize()
	size.width = size.width * scale
	self:resize(size)
end

function ZyImage:image()
	return self._image
end

function ZyImage:setPosition(point)
	self._image:setPosition(point)
end

function ZyImage:getPosition()
	return self._image:getPosition()
end

function ZyImage:getContentSize()
	return self._image:getContentSize()
end

function ZyImage:setAnchorPoint(point)
	self._image:setAnchorPoint(point)
end

function ZyImage:getAnchorPoint()
	return self._image:getAnchorPoint()
end
function ZyImage:setOpacity(num)
	self._image:setOpacity(num)
end
--
function ZyImage:addChild(child)
	self._image:addChild(child, 0)
end
--
function ZyImage:addChild(child, v, tag)
	if tag then
		self._image:addChild(child, v, tag)
	elseif v then
		self._image:addChild(child, v)
	else 
		self._image:addChild(child, 0)
	end
end
--
function ZyImage:setImage(param)
	local parent = self._image:getParent()
	local position = self._image:getPosition()
	parent:removeChild(self._image, true)
	if type(param) == "string" then
		self._image = self:imageWithFile(param)
	elseif type(param) == "userdata" then
		self._image = param
	end
	self._image:setPosition(position)
	parent:addChild(self._image)
end
--
function ZyImage:addto(parent, param1, param2)
	if type(param1) == "userdata" then
		parent:addChildItem(self._image, param1)
	else
		if param2 then
			parent:addChild(self._image, param1, param2)
		elseif param1 then
			parent:addChild(self._image, param1)
		else 
			parent:addChild(self._image, 0)
		end
	end
end
--
--------------------------静态方法-------------------------------
--
function ZyImage:imageWithFile(fileName)
	local image =CCSprite:create(P(fileName)) 
	--CCMenuItemImage:create(P(fileName), nil)
	image:setAnchorPoint(CCPoint(0, 0))
	return image
end

-- 获取图片大小
function ZyImage:imageSize(fileName)
	local sprite = CCSprite:create(P(fileName))
	local size = sprite:getContentSize()
	sprite:delete()
	return size
end


