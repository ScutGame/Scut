
MB_STYLE_TITLE 		= 1 --标题
MB_STYLE_MESSAGE 	= 2  --内容
MB_STYLE_LBUTTON 	= 3
MB_STYLE_RBUTTON 	= 4
MB_STYLE_MODIFY		= 5
MB_STYLE_THEME		= 6
MB_STYLE_GOTO_PAGE	= 7
MB_STYLE_CLOSE		= 8
MB_STYLE_PROMPT		= 9
MB_STYLE_RENAME		= 10 --更名


ID_MBOK 		= 1   --左边
ID_MBCANCEL 	= 2
MB_THEME_NORMAL 	= 1

--资源
local BackGroundPath="common/black.png";
local BgBox="common/panle_1069.png"
local closeButton="button/list_1046.png"
local ButtonNor="button/button_1011.png"
local ButtonClk="button/button_1012.png"
local topHeight=SY(12)
local edgeWidth=SX(10)

ZyMessageBoxEx = {
	_parent 		= nil,
	_layerBox 		= nil,
	_layerBG 	    = nil,
	_funCallback 	= nil,
	_nTag 			= 0,
	_userInfo 		= {},
	_tableStyle 	= {[MB_STYLE_THEME] = MB_THEME_NORMAL},
	_tableParam		= {},
	_edit 			= nil,
	_size			= nil,
	_bShow          = nil,
	_editx			=nil,
	_edity			=nil,
	_titleColor     = nil,
	_message        = nil
}

-- 创建实例
function ZyMessageBoxEx:new()
	local instance = {}
	setmetatable(instance, self)
	self.__index = self
	instance:initStyle()
	return instance
end

-- 右按钮(取消)按下
function actionMessageboxRightButton(pSender)
      local bClose = true
      local box = gClassPool[pSender]
		if box._funCallback ~= nil then
			box._funCallback(ID_MBCANCEL,  nil,box._nTag)
		end

        if bClose then
		   box:onCloseMessagebox()
	    end
end

function actionMessageboxLeftButton(pNode)
    local bClose= true;
    local box = gClassPool[pNode]
    if box._tableStyle[MB_STYLE_MODIFY] == true then
        box._userInfo.content=box._edit:GetEditText()
    elseif box._tableStyle[MB_STYLE_RENAME] == true then
	box._userInfo.content=box._edit:GetEditText()
    end
    if box._funCallback ~= nil then
	   box._funCallback(ID_MBOK,  box._userInfo.content,box._nTag)
	end
	if bClose then
		box:onCloseMessagebox()
	end
end

-- 设置参数函数
function ZyMessageBoxEx:setTag(tag)
	self._nTag = tag
end

-- 提示框
-- 如果参数strButton为nil, 则在右上角显示一个退出按钮
function ZyMessageBoxEx:doPrompt(parent, strTitle, strMessage, strButton,funCallBack)
	if ZyMessageBoxEx == self and self._bShow then
		return
	end
	if strMessage==nil or string.len(strMessage)<=0 then
		return
	end
	if funCallBack~=nil then
	   	self._funCallback = funCallBack
	end
	self._parent = parent
	if strTitle then
		self._tableStyle[MB_STYLE_TITLE] = strTitle
	end

	if strMessage then
		self._tableStyle[MB_STYLE_MESSAGE] = strMessage
	end

	if strButton then
		self._tableStyle[MB_STYLE_RBUTTON] = strButton
	else
		self._tableStyle[MB_STYLE_CLOSE] = true
	end
	self:initMessageBox()
end


-- 询问框
function ZyMessageBoxEx:doQuery(parent, strTitle, strMessage, strButtonL, strButtonR, funCallBack,Color)
    if ZyMessageBoxEx == self and self._bShow then
		return
	end
    self._parent = parent
	self._funCallback = funCallBack

	if strTitle then
		self._tableStyle[MB_STYLE_TITLE] = strTitle
	end

	if strMessage then
		self._tableStyle[MB_STYLE_MESSAGE] = strMessage
	end

	if strButtonR then
		self._tableStyle[MB_STYLE_RBUTTON] = strButtonR
	end

	if strButtonL then
		self._tableStyle[MB_STYLE_LBUTTON] = strButtonL
	end

	if Color then
	   --   self._tableStyle[MB_STYLE_LBUTTON] = strButtonL
	end
	self:initMessageBox()
end

-- 修改
function ZyMessageBoxEx:doModify(parent, title, prompt,strButtonR, strButtonL,funCallback)
	self._parent = parent
	self._funCallback = funCallback

	self._tableParam.prompt = prompt
	self._tableStyle[MB_STYLE_MODIFY] = true
    if title then
		self._tableStyle[MB_STYLE_TITLE] = title
	end

    if strButtonR then
		self._tableStyle[MB_STYLE_RBUTTON] = strButtonR
	end

	if strButtonL then
		self._tableStyle[MB_STYLE_LBUTTON] = strButtonL
	end

	self:initMessageBox()
end

--改名
function ZyMessageBoxEx:doRename(parent,title,oldName,oldNameStr,newName,strButtonR,strButtonL,funCallback)
	self._parent = parent
	self._funCallback = funCallback

	self._tableParam.oldName = oldName
	self._tableParam.oldNameStr = oldNameStr
	self._tableParam.newName = newName

	self._tableStyle[MB_STYLE_RENAME] = true

	if title then
		self._tableStyle[MB_STYLE_TITLE] = title
	end

	if strButtonR then
		self._tableStyle[MB_STYLE_RBUTTON] = strButtonR
	end

	if strButtonL then
		self._tableStyle[MB_STYLE_LBUTTON] = strButtonL
	end

	self:initMessageBox()
end

--－－－－－－－－－－－－－－－－－－－ 以下为私有接口 －－－－－－－－－－－－－－－－－－－－－－－
--
-- 自动隐藏
function ZyMessageBoxEx:autoHide(fInterval)
	if fInterval == nil then
		fInterval = 3
	end
	gClassPool[1] = self
	CCScheduler:sharedScheduler():scheduleScriptFunc("timerMBAutoHide", fInterval, false)
end

-- 初始化一些参数
function ZyMessageBoxEx:initStyle()
    self._parent   = nil
    self._layerBox = nil
    self._layerBG  = nil
    self._funCallback = nil
	self._nTag = 0
	self._userInfo = {}
	self._tableStyle = {[MB_STYLE_THEME] = MB_THEME_NORMAL}
	self._tableParam = {}
	self._edit = nil
	self._size = nil
	self._bShow = false
       self._editx	=nil
	self._edity	=nil
end

-- 关闭对话框
function ZyMessageBoxEx:onCloseMessagebox()
	if self._edit ~= nil then
		self._edit:release()
		self._edit = nil
	end

    if self._funCallback==nil then
	   	isNetCall=false
	end

	if self._tableStyle[MB_STYLE_CONTRIBUTE] == true then
		for key, value in ipairs(self._tableParam.edit) do
			value:release()
		end
	end
	self._parent:removeChild(self._layerBox, true)
	self._parent:removeChild(self._layerBG, true)
	self:initStyle()
end


function ZyMessageBoxEx:isShow()
	return self._bShow
end

-- 初始化界面
function ZyMessageBoxEx:initMessageBox()
      self._bShow = true
	--大背景
	local winSize = CCDirector:sharedDirector():getWinSize()
	local menuBG = ZyButton:new(BackGroundPath, BackGroundPath)
	menuBG:setScaleX(winSize.width / menuBG:getContentSize().width)
	menuBG:setScaleY(winSize.height  / menuBG:getContentSize().height)
--	menuBG:registerScriptTapHandler(handlerMessageboxBGClick)
	menuBG:setPosition(CCPoint(0, 0))
	menuBG:addto(self._parent,9)
	self._layerBG = menuBG:menu()

-----------------------
	--小背景
	local messageBox = CCNode:create()
	local bg

	bg=ZyImage:new(BgBox)
	self._size = bg:getContentSize()
	bg:resize(self._size)
	topHeight=self._size.height*0.1
	messageBox:addChild(bg:image(), 0, 0)
	messageBox:setContentSize(bg:getContentSize())
	messageBox:setPosition(PT((self._parent:getContentSize().width - messageBox:getContentSize().width) / 2,
             (self._parent:getContentSize().height - messageBox:getContentSize().height) / 2))
	local parentSize = self._parent:getContentSize()
	local boxSize = messageBox:getContentSize()
	local offsetY = boxSize.height
	local offsetX = 0
	
	-- 退出按钮
	if self._tableStyle[MB_STYLE_CLOSE] ~= nil then
		local button = ZyButton:new(closeButton)
		offsetX = boxSize.width - button:getContentSize().width - SPACE_X
		offsetY = boxSize.height - button:getContentSize().height - SPACE_Y
		button:setPosition(CCPoint(offsetX, offsetY))
		button:setTag(1)
		button:registerScriptTapHandler(actionMessageboxRightButton)
		button:addto(messageBox)
		gClassPool[button:menuItem()] = self
	end

	-- 标题
	if self._tableStyle[MB_STYLE_TITLE] ~= nil then
		local label = CCLabelTTF:labelWithString(self._tableStyle[MB_STYLE_TITLE], FONT_NAME, FONT_SM_SIZE)
		if boxSize.height >= parentSize.height * 0.8 then
			offsetY = boxSize.height - SPACE_Y * 5 - label:getContentSize().height
		else
			offsetY = boxSize.height - SPACE_Y * 3.6 - label:getContentSize().height
		end
		label:setPosition(CCPoint(boxSize.width * 0.5, offsetY))
		label:setAnchorPoint(CCPoint(0.5, 0))
		messageBox:addChild(label, 0, 0)
	end

	-- 内容消息
	if self._tableStyle[MB_STYLE_MESSAGE] ~= nil then
		local size = CCSize(boxSize.width - edgeWidth * 2, offsetY - topHeight * 2)
		if self._tableStyle[MB_STYLE_RBUTTON] == nil and self._tableStyle[MB_STYLE_LBUTTON] == nil then
			size.height = offsetY - topHeight * 2
		else
			size.height = offsetY - topHeight * 3 - ZyImage:imageSize(P(Image.image_button_red_c_0)).height
		end
		
		--文字	
		local labelWidth= boxSize.width*0.9 - edgeWidth * 2
		local contentStr=string.format("<label>%s</label>",self._tableStyle[MB_STYLE_MESSAGE] )
		contentLabel= ZyMultiLabel:new(contentStr,labelWidth,FONT_NAME,FONT_SMM_SIZE)
		contentLabel:addto(messageBox,0)
		local posX=boxSize.width/2-contentLabel:getContentSize().width/2
		local posY=boxSize.height*0.42-contentLabel:getContentSize().height/2
		contentLabel:setPosition(PT(posX,posY))
	end
	
    --左右按钮
    	if self._tableStyle[MB_STYLE_RBUTTON] ~= nil and self._tableStyle[MB_STYLE_LBUTTON] == nil then
            local button, item = UIHelper.Button(P(ButtonNor), P(ButtonClk), actionMessageboxRightButton,
                 self._tableStyle[MB_STYLE_RBUTTON], kCCTextAlignmentCenter, FONT_SMM_SIZE)
            offsetX = (boxSize.width - button:getContentSize().width) / 2
            button:setPosition(CCPoint(offsetX, topHeight))
            messageBox:addChild(button, 0, 0)
            gClassPool[item] = self
        elseif self._tableStyle[MB_STYLE_RBUTTON] ~= nil and self._tableStyle[MB_STYLE_LBUTTON] ~= nil then
            local button, item = UIHelper.Button(P(ButtonNor), P(ButtonClk), actionMessageboxLeftButton,
                self._tableStyle[MB_STYLE_LBUTTON], kCCTextAlignmentCenter, FONT_SMM_SIZE);
            offsetX = boxSize.width*0.9-button:getContentSize().width-edgeWidth+SX(5)
            button:setPosition(CCPoint(offsetX, topHeight))
            messageBox:addChild(button, 0, 0)
            gClassPool[item] = self
            button, item = UIHelper.Button(P(ButtonNor), P(ButtonClk), actionMessageboxRightButton,
            self._tableStyle[MB_STYLE_RBUTTON], kCCTextAlignmentCenter, FONT_SMM_SIZE);
            offsetX =edgeWidth -SX(5)+boxSize.width*0.1
            button:setPosition(CCPoint(offsetX, topHeight));
            messageBox:addChild(button, 0, 0)
            gClassPool[item] = self
        end
    -- 修改

	if self._tableStyle[MB_STYLE_MODIFY] ~= nil then
		local offsetX =edgeWidth+SX(4)
		local offsetY =boxSize.height/2+SY(6)
		local label = CCLabelTTF:labelWithString(self._tableParam.prompt, FONT_NAME, FONT_SM_SIZE)
		label:setAnchorPoint(CCPoint(0, 1))
		label:setPosition(CCPoint(offsetX, offsetY))
		messageBox:addChild(label, 0)
	--	offsetY = offsetY + label:getContentSize().height/2
	-- 编辑框
		local size = CCSize(boxSize.width/3, SY(22))
		local edit = CScutEdit:new()
		edit:init(false, false)

		offsetX = messageBox:getPosition().x + edgeWidth+label:getContentSize().width+SY(6)
		offsetY =pWinSize.height-messageBox:getPosition().y-boxSize.height/2-SY(6)-size.height+label:getContentSize().height
		edit:setRect(CCRect(offsetX, offsetY, size.width, size.height))
		self._edit = edit
	end

	
	
    -- 带提示的输入框
	if self._tableStyle[MB_STYLE_PROMPT] ~= nil then
	--提示
		offsetX = parentSize.width/4
		offsetY = parentSize.height/5
		local prompt = CCLabelTTF:labelWithString(self._message, FONT_NAME, FONT_SM_SIZE)
		prompt:setAnchorPoint(CCPoint(0.5, 1))
		prompt:setPosition(CCPoint(offsetX, offsetY))
		messageBox:addChild(prompt, 0)
	end

	--改名输入框
	if self._tableStyle[MB_STYLE_RENAME] ~= nil then
		local offsetX = nil
		local offsetY =boxSize.height*0.5     --+SY(6)
		local nameW = 0

		local oldLabel = CCLabelTTF:labelWithString(self._tableParam.oldName..": ", FONT_NAME, FONT_SM_SIZE)
		oldLabel:setAnchorPoint(CCPoint(0, 1))
		messageBox:addChild(oldLabel, 0)

		local newLabel = CCLabelTTF:labelWithString(self._tableParam.newName..": ", FONT_NAME, FONT_SM_SIZE)
		newLabel:setAnchorPoint(CCPoint(0, 1))
		messageBox:addChild(newLabel, 0)

		if oldLabel:getContentSize().width > newLabel:getContentSize().width then
			nameW = oldLabel:getContentSize().width
		else
			nameW = newLabel:getContentSize().width
		end
		offsetX = (boxSize.width/2-nameW)/2
		offsetY = offsetY+oldLabel:getContentSize().height+SY(5)
		oldLabel:setPosition(CCPoint(offsetX, offsetY))
		offsetY =boxSize.height*0.5
		newLabel:setPosition(CCPoint(offsetX, offsetY))

		local oldStr = CCLabelTTF:labelWithString(self._tableParam.oldNameStr, FONT_NAME, FONT_SM_SIZE)
		oldStr:setPosition(CCPoint(offsetX+nameW, oldLabel:getPosition().y))
		oldStr:setAnchorPoint(CCPoint(0, 1))
		messageBox:addChild(oldStr, 0)

	-- 编辑框
		local size = CCSize(boxSize.width/2, newLabel:getContentSize().height)
		local edit = CScutEdit:new()
		edit:init(false, false)
		offsetX = messageBox:getPosition().x + offsetX+nameW
		offsetY = parentSize.height/2--size.height/2+oldLabel:getContentSize().height-SY(5)
		edit:setRect(CCRect(offsetX, offsetY, size.width, size.height))
		self._edit = edit
	end

        self._layerBox = messageBox
        self._parent:addChild(messageBox, 9, 0)
end


function ZyMessageBoxEx:setEditTextSize(size)
	self._edit:setMaxText(size)
end
-- 带提示的输入框
function ZyMessageBoxEx:doModifyWithPrompt(parent, title, prompt,message,strButtonR, strButtonL,funCallback)
	self._parent = parent
	self._funCallback = funCallback
	self._message=message
	self._tableStyle[MB_STYLE_PROMPT] = true
    if title then
		self._tableStyle[MB_STYLE_TITLE] = title
	end

	if prompt then
		self._tableStyle[MB_STYLE_MODIFY] = true
		self._tableParam.prompt = prompt
	end

    if strButtonR then
		self._tableStyle[MB_STYLE_RBUTTON] = strButtonR
	end

	if strButtonL then
		self._tableStyle[MB_STYLE_LBUTTON] = strButtonL
	end

	self:initMessageBox()
end





-- 回调原型 function funCallback(int clickedButtonIndex, void userInfo, int tag)
function ZyMessageBoxEx:setCallbackFun(fun)
	self._funCallback = fun
end
