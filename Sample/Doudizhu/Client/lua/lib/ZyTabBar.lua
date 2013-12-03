

local strModuleName = "ZyTabBar";
CCLuaLog("Module ".. strModuleName.. " loaded.");
strModuleName = nil;

local n
 ZyTabBar = {
	_menuItems = nil,
	_labels = nil,
	_menu = nil,
	_callbackFun = nil,
	_picNor = nil,
	_picDown = nil,
	_titles = nil,
	_fontName = nil,
	_fontSize = nil,
	_picLArrow = nil;
	_picRArrow = nil;
	_nNumOfVisibleTab = nil,
	_bDisplayArrows = false,
	_nCurrentFirstTab = 1,
	_nCurrentTab = 1,
	_color = nil,
	_list=nil,
	_nextBtn=nil
 }
function gZytableHandler(pNode)
	local bar = gClassPool[pNode];
	local bSel = true;
	
	if bar and bar._callbackFun then
		local sel = bar._callbackFun(bar, pNode)
		if sel == false then
			bSel = false
		end
	end
	
	if bSel then
		bar._nCurrentTab = pNode:getTag();
		for key, value in pairs(bar._menuItems) do
			value:unselected()
		end
		bar._menuItems[bar._nCurrentTab]:selected();
	end
end
-- 刷新Tabbar按钮
function ZyTabBarRefresh(this)
	local nMin = this._nCurrentFirstTab;
	local nMax = this._nCurrentFirstTab + this._nNumOfVisibleTab - 1;
	local titles = {}
	for k, v in pairs(this._titles) do
	    titles[#titles+1]=v
	end
	
	local width = 0;
	local height = 0;
	local menuItems = {};
	local LButton;
	local RButton;
	local menu = this._menu;
	local sprite=CCSprite:create(P(this._picNor))
	local spriteDown=CCSprite:create(P(this._picDown))
	
	local listWidth = sprite:getContentSize().width
	--* this._nNumOfVisibleTab
	local showHeight=spriteDown:getContentSize().height
	local width=0
	if this._nNumOfVisibleTab< #titles then
		width=sprite:getContentSize().width/6
	end
		
	local listSize=SZ(listWidth*this._nNumOfVisibleTab+width,showHeight)
	local list = ScutCxList:node(listWidth,ccc4(124, 124, 124, 255),listSize)
	this._list=list
	list:setTouchEnabled(true)
	--list:registerLoadEvent(gZytableTurnPage)
	list:setAnchorPoint(PT(0,0))
	list:setHorizontal(true)
--	list:setPageTurnEffect(true)--是否翻页
--	list:setRecodeNumPerPage(1)--设置页码
--	list:setSelectedItemColor(ccc3(0, 0, 0), ccc3(0, 0, 0))	
       local itemTable={}
	local index=0
	local titleIndex=0
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val =0
	layout.val_y.val.pixel_val =0
	 for k, v in pairs(this._titles) do
            titleIndex=titleIndex+1
	       		index=index+1
	       		local listItem=ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
	       		listItem:setOpacity(0)
	              --	local Btn=ZyButton:new(this._picNor, this._picDown,nil,v,FONT_NAME,FONT_SM_SIZE)
                    local Btn=ZyButton:new(this._picNor, this._picDown,nil,nil,FONT_NAME,FONT_SM_SIZE)
	              	Btn:setColor(ccC1)
	              	Btn:addto(listItem,layout)
	              	Btn:setTag(k)
	              	menuItems[k]=Btn
	              	gClassPool[Btn._menuItem] = this; 
                     ----tab名字用图标
                    local BtnName=CCSprite:create(P(v));
                    BtnName:setAnchorPoint(PT(0.5,0.5));
                    BtnName:setPosition(PT(Btn:getContentSize().width/2,Btn:getContentSize().height/2));
                    Btn:addChild(BtnName,1);
                    -------
	              	if (this._nCurrentTab ==  k) then
					Btn:selected();
				end
	              	Btn:registerScriptTapHandler(function () gZytableHandler(Btn._menuItem) end ); 
              		 list:addListItem(listItem, false) 
		end
		this._menuItems = menuItems
		if #titles>this._nNumOfVisibleTab then
			local layer=CCLayer:create()		
			local sprite1=CCSprite:create(P(this._picLArrow))
			local sprite2=CCSprite:create(P(this._picRArrow))
			sprite1:setAnchorPoint(CCPoint(0, 0));
			sprite2:setAnchorPoint(CCPoint(0, 0));
			sprite1:setPosition(PT(0,0))
			sprite2:setPosition(PT(sprite1:getContentSize().width,0))
			layer:addChild(sprite1,0)
			layer:addChild(sprite2,0)
			--this._nextBtn=layer
		end
		
end


function  ZyTabBar:GotoTag(index)
	self._nCurrentTab = index;

	for key, value in pairs(self._menuItems) do
		if value then
		if(value:getTag() == index) then
			value:selected();
		else
			value:unselected();
		end
		end
	end

    if (index < (#(self._titles) - self._nNumOfVisibleTab+1)) then
		self._nCurrentFirstTab =index;
    else
        self._nCurrentFirstTab=(#(self._titles) - self._nNumOfVisibleTab+1)
	end
  --      ZyTabBarRefresh(self);
end;

-- 创建实例
function ZyTabBar:new(picNor, picDown, names, fontName, fontSize, nNumOfVisibleTab, picLArrow, picRArrow)
	local instance = {};
	setmetatable(instance, self)
	self.__index = self
	
	local labels = {};
	local menuItems = {};
	local menu;
       local TmpMenuItem =nil
       if  picLArrow==nil then
           picLArrow="button/list_1069.png"
           picRArrow="button/list_1068.png"
       end
       
	if fontName == nil then
		fontName = FONT_NAME
	end
	
	if fontSize == nil then
		fontSize = FONT_SM_SIZE
	end
	
	local width = 0
	local height = 0
	local titles={}
	for k, v in pairs(names) do
	    titles[#titles+1]=v 
	end

	-- 可视标签数量设置
	if (not nNumOfVisibleTab) or (nNumOfVisibleTab <= 0 ) or (nNumOfVisibleTab > #titles) then
		nNumOfVisibleTab = #titles;
	end
	instance._picNor ="common/list_1042.png"
	instance._picDown ="common/list_1041.png"
	if picNor~=nil then
	instance._picNor = picNor;
	end
	if picDown~=nil then
	instance._picDown = picDown;
	end
	instance._titles = names;
	instance._fontName = fontName;
	instance._fontSize = fontSize;
	instance._nNumOfVisibleTab = nNumOfVisibleTab;
	instance._picLArrow = picLArrow;
	instance._picRArrow = picRArrow;

	--可视标签数量少于总标签数量时才显示箭头
	if (nNumOfVisibleTab ~= #titles) then
		instance._bDisplayArrows = true;
	end
	
	ZyTabBarRefresh(instance);

	return instance;
end
--

function ZyTabBar:addto(parent, param1, param2)
	local child=self._menu
	if self._list then
		child=self._list 
	end
	if type(param1) == "userdata" then
		parent:addChildItem(child, param1)
	else
		if param2 then
			parent:addChild(child, param1, param2)
		elseif param1 then
			parent:addChild(child, param1)
		else
			parent:addChild(child, 0)
		end
		if self._nextBtn then
			parent:addChild(self._nextBtn, 0)
		end
	end
end


function ZyTabBar:setWidth(width)
	local size = self._menu:getContentSize()
	if size.width < width then
		local num = #self._menuItems
		local space = (width - size.width) / (num - 1)
		for key, value in pairs(self._menuItems) do
			if value then
			local position = value:getPosition()
			position.x = position.x + space * (key - 1)
			value:setPosition(position)
			end
		end
	else 
		local num = #self._menuItems
		local space = (size.width - width) / num
		local W1 = self._menuItems[1]:getContentSize().width
		local W2 = W1 - space
		local scale = W2 / W1
		local offsetX = 0
		for key, value in pairs(self._menuItems) do
			if value then
			value:setScaleX(scale)
			value:setPosition(CCPoint(offsetX, 0))		
			offsetX = offsetX + W2
			end
		end
	end
	size.width = width
	self._menu:setContentSize(size)
end

----]]
function ZyTabBar:selectItem(index)

	self._nCurrentTab = index;

	for key, value in pairs(self._menuItems) do
		if value then
		if(value:getTag() == index) then
			value:selected();
		else
			value:unselected();
		end
		end
	end
		--]]
end

function ZyTabBar:setIsEnabled(isEnable)

	for key, value in pairs(self._menuItems) do
            value:setIsEnabled(isEnable)
	end
	
end
function ZyTabBar:setIsVisible(isEnable)

	for key, value in pairs(self._menuItems) do
            value:setIsVisible(isEnable)
	end
	
end
-- 回调原型 void callback(bar, node)
function ZyTabBar:setCallbackFun(fun)
	self._callbackFun = fun
end
--
function ZyTabBar:setString(index, label)
	self._labels[index]:setString(label)
end
--
function ZyTabBar:getString(index)
	return self._labels[index]:getString()
end
--
function ZyTabBar:setColor(color)
end
--
function ZyTabBar:setTag(index, tag)
	self._menuItems[index]:setTag(tag)
end
--
function ZyTabBar:setPosition(position)
	if self._list then
			self._list:setPosition(position)
	else
			self._menu:setPosition(position)
	end

	if self._nextBtn then
		self._nextBtn:setPosition(PT(position.x+self._list:getContentSize().width,position.y))
	end
end
--
function ZyTabBar:getPosition()
	return self._list:getPosition()
end
--

function ZyTabBar:setAnchorPoint(point)
	if 	 self._list then	
		self._list:setAnchorPoint(point)
	else
		self._menu:setAnchorPoint(point)
	end	
end

--
function ZyTabBar:getAnchorPoint(ponint)
	return self._list:getAnchorPoint(point)
end

--
function ZyTabBar:setContentSize(size)
	self._list:setContentSize(size)
end
--
function ZyTabBar:getContentSize()
	
	if 	 self._list then	
		return self._list:getContentSize()
	else
		return self._menu:getContentSize()
	end
	
end
--

function ZyTabBar:remove()
	if self._list then
    self._list:getParent():removeChild(self._list,true)
    else
      self._menu:getParent():removeChild(self._menu,true)
    end
end



function ZyTabBar:SetSilence(value)
	 self._list:SetSilence(value)
end;


