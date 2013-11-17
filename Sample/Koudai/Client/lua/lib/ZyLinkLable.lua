
ZyLinkLable={
	_menu=nil,
	_menuItem=nil,
	_label=nil,
	_userData=nil,
}

--创建一个有下划线的文本按钮

--text文本内容(必填) color 颜色 fontname 字体 fontsize 字号,是否有下划线
function ZyLinkLable:new(text,color,fontname,fontsize,width,isLine)
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	
	if fontname == nil then	
		fontname = FONT_NAME;
	end
	if fontsize == nil then	
		fontsize = FONT_SM_SIZE;
	end
	
	if color == nil then
		color=NdColor:colorWhite();
	end
    local subStr = text
	local nLine = nil
	local nRow = 0
	local lengthList = {}
	while true do
	    nLine,subStr = ZyFont.subString(subStr,width,fontname,fontsize)
	    if nLine ~= nil then
	        nRow = nRow + 1
	        lengthList[nRow] = ZyFont.stringWidth(nLine,fontname,fontsize)
	        if lengthList[nRow] < fontsize then
	            lengthList[nRow] = fontsize
            end
	    end
	    if subStr == nil then
	        break;
	    end
    end
    if nRow < 1 then
	    nRow = 1
    end
	local labelChange = nil
	if nRow > 1 then
	 local lable=CCLabelTTF:create(Language.IDS_SURE,fontname,fontsize)
        local szContent  = ZyFont.stringSize(text,width, fontname, fontsize);
        szContent.height=nRow*lable:getContentSize().height
        labelChange = CCLabelTTF:create(text,szContent,CCTextAlignmentLeft, FONT_NAME,fontsize)    
   else
        labelChange = CCLabelTTF:create(text, fontname, fontsize);
    end
    
	labelChange:setColor(color);
	local ChangeMenuItem = CCMenuItemLabel:create(labelChange);
	ChangeMenuItem:setContentSize(SZ(labelChange:getContentSize().width,labelChange:getContentSize().height))
	ChangeMenuItem:setPosition(PT(ChangeMenuItem:getContentSize().width/2,ChangeMenuItem:getContentSize().height/2))
	ChangeMenuItem:setAnchorPoint(PT(0.5, 0.5))	
    
	color = ccc4(color.r, color.g, color.b,255)
	local nHeight = labelChange:getContentSize().height/nRow
	if isLine then
		for i=1,nRow do
		        local lineNode = ScutLineNode:lineWithPoint(PT(0,nHeight*(nRow-i)), PT(lengthList[i], nHeight*(nRow-i)), 1, color)
	        	ChangeMenuItem:addChild(lineNode, 0)
		end
	end
	
	local changeMenu = CCMenu:createWithItem(ChangeMenuItem)
	changeMenu:setContentSize(ChangeMenuItem:getContentSize())
	changeMenu:setAnchorPoint(PT(0, 0))
	
	instance._menu = changeMenu
	instance._menuItem = ChangeMenuItem
	instance._label = labelChange
	
	return instance
end

function ZyLinkLable:setUserData(data)
	self._userData=data
end

function ZyLinkLable:getUserData()
	return self._userData
end

function ZyLinkLable:getParent()
	return self._menu:getParent();
end
function ZyLinkLable:registerScriptHandler(event)
	gClassPool[self._menuItem] = self
	self._menuItem:registerScriptHandler(event)
end

function ZyLinkLable:setPosition(point)
	self._menu:setPosition(point)
end

function ZyLinkLable:getPosition()
	return self._menu:getPosition()
end

function ZyLinkLable:setTag(tag)
	self._menuItem:setTag(tag)
end

function ZyLinkLable:getTag()
	return self._menuItem:getTag()
end

function ZyLinkLable:getContentSize()
	return self._menu:getContentSize()
end

function ZyLinkLable:addto(parent, param1, param2)
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

function ZyLinkLable:setVisible(visible)
	self._menu:setVisible(visible)
end

function ZyLinkLable:getIsVisible()
	return self._menu:getIsVisible()
end
-------------
--触发事件中取到linklabel本身
function ZyLinkLable.getLingLabel(node)
	return gClassPool[node]
end