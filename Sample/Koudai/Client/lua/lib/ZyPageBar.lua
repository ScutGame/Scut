

ZyPageBar = {
	mnCurrentPage = 1,
	mnTotalPage = 1,
	MOST_PAGE = 10,
	mCallbackPageChange = nil,
	mLayerBar = nil,
	mParent = nil,
	contentSize = nil,
	mTableItems = nil,
	mListView = nil,
	mSceen = nil,
	mLabelPage = nil,
}

--
---------------------------------------------公有接口----------------------------------------
--
-- 回调原型 fun callback(pagebar, int page)
function ZyPageBar:new(parent, nCurrentPage, nTotalPage, callbackPageChange)
	--[[
	if parent == nil or nTotalPage < 2 or nCurrentPage < 1 or nCurrentPage > nTotalPage or callbackPageChange == nil then
		LogFile("ZyPageBar.txt", "a", "bad argument!")
		return nil
	end
	--]]
	
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	
	if nTotalPage == 0 then
		nTotalPage = 1
	end
	
	instance.mParent = parent
	instance.mnCurrentPage = nCurrentPage
	instance.mnTotalPage = nTotalPage
	instance.mSceen = math.floor((nCurrentPage - 1) / instance.MOST_PAGE)
	instance.mCallbackPageChange = callbackPageChange
	instance:initBar()
	
	return instance
end

-- 更新当前页数
function ZyPageBar:setCurrentPage(page)
	if page >= 1 and page <= self.mnTotalPage then
		self.mnCurrentPage = page
		self.mSceen = math.floor((page - 1) / self.MOST_PAGE)
		self:selectPage(page)
	end
end
--
function ZyPageBar:setListView(list)
	self.mListView = list
end

-- 获取当前页数
function ZyPageBar:getCurrentPage()
	return self.mnCurrentPage
end

-- 更新总页数
function ZyPageBar:setTotalPage(page)
	if page >= 0 then
	    if page == 0 then
	        page = 1
        end
		self.mnTotalPage = page
		local position = self:getPosition()
		if self.mLayerBar ~= nil then
			self.mParent:removeChild(self.mLayerBar, true)
			self.mLayerBar = nil
		end
        self:initBar()
        self:setPosition(position)
		
	end
end

-- 获取总页数
function ZyPageBar:getTotalPage()
	return self.mnTotalPage
end

-- 设置位置，AnchorPoint默认为(0.5, 0.5)
function ZyPageBar:setPosition(position)
	self.mLayerBar:setPosition(position)
end

-- 获取位置
function ZyPageBar:getPosition()
	return self.mLayerBar:getPosition()
end

-- 获取大小
function ZyPageBar:getContentSize()
   return self.mLayerBar:getContentSize();
end
--
function ZyPageBar:setVisible(visible)
	self.mLayerBar:setVisible(visible)
end
--
-----------------------------------------------------私有接口----------------------------------
--
function ZyPageBar:initBar()
	local bMore = false

	local nPage = self.mnTotalPage - self.mSceen * self.MOST_PAGE
	if nPage > self.MOST_PAGE then
	    nPage = self.MOST_PAGE
	end
	if self.mnTotalPage > self.MOST_PAGE then
		bMore = true
	end

	if self.mTableItems == nil then
		self.mTableItems = {}
	end

	for key in pairs(self.mTableItems) do
		self.mTableItems[key] = nil
	end

	self.mLayerBar = CCNode:create()
	self.mLayerBar:setAnchorPoint(CCPoint(0.5, 0.5))
	
	local height = ZyImage:imageSize("button/button9_normal.png").height
	local spaceX = SPACE_X
	local offsetX = spaceX
	local offsetY = 0
	local button, item
	local strfun = "ZyPageBar:actionPage"
	local nc = self.mnCurrentPage % self.MOST_PAGE
	if nc == 0 then
		nc = self.MOST_PAGE
	end
	for i = 1, nPage do
		button, item = UIHelper.Button(P("button/checkbtnnomral.png"), P("button/checkbtnclick.png"), "ZyPageBarItemPageHandler", "", CCTextAlignmentCenter, FONT_SM_SIZE, nil, i, i == nc)
		button:setPosition(CCPoint(offsetX, offsetY + (height - button:getContentSize().height) / 2))
		self.mLayerBar:addChild(button, 0, 0)
		self.mTableItems[i] = item
		gClassPool[item] = self
		offsetX = offsetX + button:getContentSize().width + spaceX
	end

	if bMore then
		button = ZyButton:new("button/button9_normal.png")
		button:registerScriptHandler("ZyPageBarItemGotoPageHandler")
		button:setPosition(CCPoint(offsetX, offsetY))
		button:addto(self.mLayerBar)
		gClassPool[button:menuItem()] = self
		offsetX = offsetX + button:getContentSize().width + spaceX
	end
	
	local label = CCLabelTTF:create(string.format("%d/%d", self.mnCurrentPage, self.mnTotalPage), FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(CCPoint(0, 0))
	label:setPosition(CCPoint(offsetX, offsetY + (height - label:getContentSize().height) / 2))
	self.mLayerBar:addChild(label, 0)
	self.mLabelPage = label

	self.mLayerBar:setContentSize(CCSize(offsetX, height))
	self.mParent:addChild(self.mLayerBar, 0, 0)
end

function ZyPageBar:showPage(page)
	if page then
		self:selectPage(page)
		self.mCallbackPageChange(self, page)
	end
end
--
function ZyPageBar:selectPage(page)
	if page then
		self.mLabelPage:setString(string.format("%d/%d", page, self.mnTotalPage))
		page = page % self.MOST_PAGE
		if page == 0 then
			page = self.MOST_PAGE
		end
		for k, v in ipairs(self.mTableItems) do
			if k == page then
				v:selected()
			else
				v:unselected()
			end
		end
	end
end

--
----------------------------------------全局函数-------------------------------
--
function ZyPageBarItemPageHandler(pNode)
	local pagebar = gClassPool[pNode]
	local page = pagebar.mSceen * pagebar.MOST_PAGE + pNode:getTag()
	if page > 0 and page <= pagebar.mnTotalPage then
	    pagebar:showPage(page)
	end
end

function ZyPageBarItemGotoPageHandler(pNode)
	local pagebar = gClassPool[pNode]
	local parent = pagebar.mParent
	local winsize = CCDirector:sharedDirector():getWinSize()
	while true do
		local size = parent:getContentSize()
		if winsize.width == size.width and winsize.height == size.height then
			break
		end
		parent = parent:getParent()
	end
	
	if pagebar.mListView then
		for key, value in ipairs(pagebar.mListView) do
			value:SetSilence(false)
		end
	end

	local box = ZyMessageBoxEx:new()
	box:doGotoPage(parent, pagebar.mnCurrentPage, pagebar.mnTotalPage, ZyPageBarMessageBoxCallback)
	box:setUserInfo({pagebar = pagebar})
end

function ZyPageBarMessageBoxCallback(buttonIndex, userInfo, tag)
	local pagebar = userInfo.pagebar
	if buttonIndex == ID_MBOK then
		pagebar.mSceen = math.floor((userInfo.page - 1) / pagebar.MOST_PAGE)
		local leftPage = pagebar.mnTotalPage - pagebar.mSceen * pagebar.MOST_PAGE
		pagebar.mnCurrentPage = userInfo.page
		if leftPage > pagebar.MOST_PAGE then
		    leftPage = pagebar.MOST_PAGE
		end
		if leftPage ~= #pagebar.mTableItems then
			local position = pagebar:getPosition()
			pagebar.mParent:removeChild(pagebar.mLayerBar, true)
			pagebar:initBar()
			pagebar:setPosition(position)
			pagebar:showPage(userInfo.page)
		else
			pagebar:showPage(userInfo.page)
		end
	end
	if pagebar.mListView then
		for key, value in ipairs(pagebar.mListView) do
			value:SetSilence(true)
		end
	end
end
