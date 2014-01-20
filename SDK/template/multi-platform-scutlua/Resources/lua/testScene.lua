module("testScene", package.seeall)

function P(fileName)
    if fileName then
        return ScutDataLogic.CFileHelper:getPath(fileName)
    else
        return nil
    end
end

------文字资源-------------------------

IDS_JINDOU = "金豆"
IDS_JIFEN = "积分"
IDS_JINGYAN = "经验"
IDS_SORCE = "分数:"
IDS_NICKNAME = "昵称:"
IDS_SORCE1 = "分数"
IDS_NICKNAME1 = "昵称"
IDS_OK="确认"
IDS_SUBMIT = "提交成绩"
IDS_CANCLE="取消"
IDS_EMPTY_TIP = "输入不能为空"
IDS_ORDER = "名次"
IDS_TEST = "请按右下角排行按钮进行TCP连接"
IDS_TCP_CONNECTING = "TCP连接建立中"
IDS_TCP_CONNECTED = "TCP连接已建立，接受数据中"
IDS_CONNECT_COUNT = "收到服务器推送数据%d次"

-----图片资源路径-------------------------
image_background_sm="common/list_1002_1.9.png"--背景图 
image_list_txt="common/panle_1009_1.9.png"--文本输入框底图
image_list_txt_2="common/list_1004.9.png"--文本输入框底图
image_logo="Image/logo.png"--logo
image_logoSmall="Image/logo2.png" --小Logo

image_button_red_c_0="button/button_1012.png"--红色长按钮
image_button_red_c_1="button/button_1011.png"--红色长按钮

image_mainscene = "common/panel_1003_1.png" --背景

image_exit = "button/icon_1027.png" --返回按钮

image_roomBg="common/panel_1002_3.png" --房间背景
image_menuBg="common/panel_1006.png"--菜单背景

image_nameBg="common/panel_1002_1.9.png" --名字背景

image_shop_1="common/panel_1003_4.png" --头像商店
image_shop="common/panel_1003_5.png" --头像商店
---聊天
icon_1024="chat/icon_1024.png"--聊天按钮
tou_ming="common/tou_ming.9.png"--透明按钮
panle_1016_1="common/panle_1016_1.png"--下划线
button_1022="tabNameImg/button_1022.png"--即时聊天
button_1023="tabNameImg/button_1023.png"--聊天记录
--排行榜
button_1028="tabNameImg/button_1028.png"--金豆排行
button_1029="tabNameImg/button_1029.png"--胜率排行
button_1006="title/button_1006.png";--说明
panle_1019_1_9="button/panle_1019_1.9.png"--list中单个对应的背景框

panle_1014_1="common/panle_1014_1.png"--说明框背景框先用这张



MB_STYLE_TITLE      = 1
MB_STYLE_MESSAGE    = 2
MB_STYLE_LBUTTON    = 3
MB_STYLE_RBUTTON    = 4
MB_STYLE_MODIFY     = 5
MB_STYLE_THEME      = 6
MB_STYLE_GOTO_PAGE  = 7
MB_STYLE_CLOSE      = 8
MB_STYLE_PROMPT     = 9
MB_STYLE_RENAME     = 10


ID_MBOK         = 1
ID_MBCANCEL     = 2
MB_THEME_NORMAL     = 1
mConnectNum = 0;
pWinSize=CCDirector:sharedDirector():getWinSize()

function PT(x,y)
    return CCPoint(x,y)
end
function Half_Float(x)
    return x*0.5
end

function SZ(width, height)
    return CCSize(width, height)
end 

function SCALEX(x)
    return CCDirector:sharedDirector():getWinSize().width/480*x
end
function SCALEY(y)
    return CCDirector:sharedDirector():getWinSize().height /320*y
end

FONT_NAME     = "黑体"


FONT_DEF_SIZE = SCALEX(18)
FONT_SM_SIZE  = SCALEX(15)
FONT_BIG_SIZE = SCALEX(23)
FONT_M_BIG_SIZE = SCALEX(63)
FONT_SMM_SIZE  = SCALEX(13)
FONT_FM_SIZE=SCALEX(11)
FONT_FMM_SIZE=SCALEX(12)
FONT_FMMM_SIZE=SCALEX(9)

ccBLACK = ccc3(0,0,0)
ccWHITE = ccc3(255,255,255)
ccYELLOW = ccc3(255,255,0)
ccBLUE = ccc3(0,0,255)
ccGREEN = ccc3(0,255,0)
ccRED = ccc3(255,0,0)
ccMAGENTA = ccc3(255,0,255)
ccPINK = ccc3(228,56,214)      -- 粉色
ccORANGE = ccc3(206, 79, 2)   -- 橘红色
ccGRAY = ccc3(166,166,166)
ccC1=ccc3(45,245,250)
---通用颜色
ccRED1= ccc3(86,26,0)
ccYELLOW2=ccc3(241,176,63)

------获取资源的路径---------------------
function P(fileName)
    if fileName then
        return ScutDataLogic.CFileHelper:getPath(fileName)
    else
        return nil
    end
end

function SX(x)
    return SCALEX(x)
end 

function SY(y)
    return SCALEY(y)
end

local BackGroundPath="common/black.png";
local BgBox="common/panle_1069.png"
local closeButton="button/list_1046.png"
local ButtonNor="button/button_1011.png"
local ButtonClk="button/button_1012.png"
local topHeight=SY(12)
local edgeWidth=SX(10)

----------------------- ui control ----------------------------------------------
 ZyButton = {
    _menuItem = nil,
    _label = nil,
    _menu = nil,
    _colorNormal = nil,
    _colorSelected = nil,
    _isSelected = nil,
 }
 
 ZyImage = {
    _image = nil,
    _scaleX = 1,
    _scaleY = 1,
 }

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

function ZyImage:imageWithFile(fileName)
    local image =CCSprite:create(P(fileName)) 
    image:setAnchorPoint(CCPoint(0, 0))
    return image
end
function ZyImage:imageSize(fileName)
    local sprite = CCSprite:create(P(fileName))
    local size = sprite:getContentSize()
    return size
end
function ZyImage:resize(size)
    local oldSize = self._image:getContentSize()
    self._scaleX =  (size.width / oldSize.width)
    self._scaleY = (size.height / oldSize.height)
    self._image:setScaleX(self._scaleX)
    self._image:setScaleY(self._scaleY)
    self._image:setContentSize(size)
end
function ZyImage:image()
    return self._image
end
function ZyImage:getContentSize()
    return self._image:getContentSize()
end

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
            label1 = CCLabelTTF:create(title,fontName, fontSize, size, kCCTextAlignmentCenter )
            label1:setPosition(CCPoint(spriteNor:getContentSize().width / 2, spriteNor:getContentSize().height / 2))
            spriteNor:addChild(label1, 0)
            
            label2 = CCLabelTTF:create(title, fontName, fontSize, size, kCCTextAlignmentCenter)
            label2:setPosition(CCPoint(spriteDown:getContentSize().width / 2, spriteDown:getContentSize().height / 2))
            spriteDown:addChild(label2, 0)
            spriteDown:setPosition(CCPoint(0, SY(-1)))
        else
            spriteDown:setScale(0.94)
            spriteDown:setPosition(CCPoint(spriteNor:getContentSize().width * 0.03, spriteNor:getContentSize().height * 0.03))
        end
        menuItem = CCMenuItemSprite:create(spriteNor, spriteDown)
        addLabel = true
    else
        menuItem = CCMenuItemLabel:itemWithLabel(instance._label)
        addLabel = true
    end
    
    if addLabel and title then
        if title then
             label = CCLabelTTF:create(title, fontName, fontSize)
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
end

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
end

function ZyButton:menuItem()
    return self._menuItem
end

function ZyButton:menu()
    return self._menu
end

function ZyButton:registerScriptTapHandler(handler)
    self._menuItem:registerScriptTapHandler(handler)
end

function ZyButton:setIsEnabled(enabled)
    self._menuItem:setEnabled(enabled)
end

function ZyButton:getIsEnabled()
    return self._menuItem:getIsEnabled()
end

function ZyButton:setIsVisible(enabled)
    self._menuItem:setVisible(enabled)
end

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

function ZyButton:setTag(tag)
    self._menuItem:setTag(tag)
end

function ZyButton:getTag(tag)
    return self._menuItem:getTag()
end

function ZyButton:setPosition(position)
    self._menu:setPosition(position)
end

function ZyButton:getPosition()
    return self._menu:getPosition()
end

function ZyButton:setAnchorPoint(point)
    self._menu:setAnchorPoint(point)
    self._menuItem:setAnchorPoint(point)
end

function ZyButton:getAnchorPoint(ponint)
    return self._menu:getAnchorPoint(point)
end

function ZyButton:setContentSize(size)
    self._menu:setContentSize(size)
end

function ZyButton:getContentSize()
    return self._menu:getContentSize()
end

function ZyButton:setScale(scale)
    return self._menu:setScale(scale)
end

function ZyButton:setScaleX(scale)
    return self._menu:setScaleX(scale)
end

function ZyButton:setScaleY(scale)
    return self._menu:setScaleY(scale)
end

function ZyButton:getScale()
    return self._menu:getScale()
end

function ZyButton:selected()
    self._isSelected = true
    self._menuItem:selected()
    if self._colorSelected then
        self._label:setColor(self._colorSelected)
    elseif self._colorNormal then
        self._label:setColor(self._colorNormal)
    end
end

function ZyButton:unselected()
    self._isSelected = false
    self._menuItem:unselected()
    if self._colorNormal then
        self._label:setColor(self._colorNormal)
    end
end


ZyMessageBoxEx = {
    _parent         = nil,
    _layerBox       = nil,
    _layerBG        = nil,
    _funCallback    = nil,
    _nTag           = 0,
    _userInfo       = {},
    _tableStyle     = {[MB_STYLE_THEME] = MB_THEME_NORMAL},
    _tableParam     = {},
    _edit           = nil,
    _size           = nil,
    _bShow          = nil,
    _editx          =nil,
    _edity          =nil,
    _titleColor     = nil,
    _message        = nil
}

function ZyMessageBoxEx:new()
    local instance = {}
    setmetatable(instance, self)
    self.__index = self
    instance:initStyle()
    return instance
end

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
       box._funCallback(ID_MBOK, box._userInfo.content,box._nTag)
    end
    if bClose then
        box:onCloseMessagebox()
    end
end

function ZyMessageBoxEx:setTag(tag)
    self._nTag = tag
end

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

function ZyMessageBoxEx:autoHide(fInterval)
    if fInterval == nil then
        fInterval = 3
    end
    gClassPool[1] = self
    CCScheduler:sharedScheduler():scheduleScriptFunc("timerMBAutoHide", fInterval, false)
end

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
    self._editx  =nil
    self._edity =nil
end

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

function MenuItem(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, bCheck, disablePic)
    local strNor     = nil
    local strDown    = nil
    local strDisable = nil
    if normalPic ~= nil then
        strNor = (normalPic)
    end

    if downPic ~= nil then
        strDown = (downPic)
    end

    if disablePic ~= nil then
        strDisable = (disablePic)
    end

    local menuItem
    if disablePic == nil then
        menuItem = CCMenuItemImage:create(strNor, strDown)
    else
        menuItem = CCMenuItemImage:create(strNor, strDown, strDisable)
    end
    
    if bCheck then
        menuItem:selected()
    else
        menuItem:unselected()
    end

    local pLable = nil
    if strText ~= nil then
        pLable  = CCLabelTTF:create(strText, FONT_NAME, (fontSize))
    end

    local szMenu = menuItem:getContentSize()


    if listtener ~= nil then
        menuItem:registerScriptTapHandler(function () listtener(menuItem) end )
    end
    menuItem:setAnchorPoint(CCPoint(0, 0))


    --TextAlign--
    if pLable ~= nil then
        if TextAlign == 0 then  --AlignLeft
            pLable:setAnchorPoint(CCPoint(0, 0.5))
            pLable:setPosition(CCPoint(0, szMenu.height/2))
        elseif TextAlign == 1 then --AlignCenter
            pLable:setPosition(CCPoint(szMenu.width/2, szMenu.height/2))
        else --ALignRight
           pLable:setAnchorPoint(CCPoint(1, 0.5))
           pLable:setPosition(CCPoint(szMenu.width/2, szMenu.height/2))
        end
        if color ~= nil then
            pLable:setColor(color)
        end
        menuItem:addChild(pLable, 0, 0)
    end
    return menuItem, pLable
end

function Button(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, tag, bCheck, disablePic)

    menuItem = MenuItem(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, bCheck, disablePic)
    if tag ~= nil then
        menuItem:setTag(tag)
    end

    local Btn = CCMenu:createWithItem(menuItem)
    Btn:setContentSize(menuItem:getContentSize())
    return Btn, menuItem
end

function ZyMessageBoxEx:initMessageBox()
    self._bShow = true
    local winSize = CCDirector:sharedDirector():getWinSize()
    local menuBG = ZyButton:new(BackGroundPath, BackGroundPath)
    menuBG:setScaleX(winSize.width / menuBG:getContentSize().width)
    menuBG:setScaleY(winSize.height  / menuBG:getContentSize().height)
    menuBG:setPosition(CCPoint(0, 0))
    menuBG:addto(self._parent,9)
    self._layerBG = menuBG:menu()

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

    if self._tableStyle[MB_STYLE_CLOSE] ~= nil then
        local button = ZyButton:new(closeButton)
        offsetX = boxSize.width - button:getContentSize().width - SPACE_X
        offsetY = boxSize.height - button:getContentSize().height - SPACE_Y
        button:setPosition(CCPoint(offsetX, offsetY))
        button:setTag(1)
        button:registerScriptHandler(actionMessageboxRightButton)
        button:addto(messageBox)
        gClassPool[button:menuItem()] = self
    end

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

    if self._tableStyle[MB_STYLE_MESSAGE] ~= nil then
        local size = CCSize(boxSize.width - edgeWidth * 2, offsetY - topHeight * 2)
        if self._tableStyle[MB_STYLE_RBUTTON] == nil and self._tableStyle[MB_STYLE_LBUTTON] == nil then
            size.height = offsetY - topHeight * 2
        else
            size.height = offsetY - topHeight * 3 - ZyImage:imageSize((image_button_red_c_0)).height
        end
        
        local labelWidth= boxSize.width*0.9 - edgeWidth * 2
        --local contentStr=string.format("<label>%s</label>",self._tableStyle[MB_STYLE_MESSAGE] )
        local contentStr=self._tableStyle[MB_STYLE_MESSAGE] 
        contentLabel= CCLabelTTF:create(contentStr,FONT_NAME,FONT_SMM_SIZE)
        --contentLabel:addto(messageBox,0)
        messageBox:addChild(contentLabel,0);
        contentLabel:setAnchorPoint(PT(0.5,0.5));
        local posX=boxSize.width/2-contentLabel:getContentSize().width/2
        local posY=boxSize.height*0.42-contentLabel:getContentSize().height/2
        contentLabel:setPosition(PT(posX,posY))
    end
    
        if self._tableStyle[MB_STYLE_RBUTTON] ~= nil and self._tableStyle[MB_STYLE_LBUTTON] == nil then
            local button, item = Button(P(ButtonNor), P(ButtonClk), actionMessageboxRightButton,
                 self._tableStyle[MB_STYLE_RBUTTON], kCCTextAlignmentCenter, FONT_SMM_SIZE)
            offsetX = (boxSize.width - button:getContentSize().width) / 2
            button:setPosition(CCPoint(offsetX, topHeight))
            messageBox:addChild(button, 0, 0)
            gClassPool[item] = self
        elseif self._tableStyle[MB_STYLE_RBUTTON] ~= nil and self._tableStyle[MB_STYLE_LBUTTON] ~= nil then
            local button, item = Button(P(ButtonNor), P(ButtonClk), actionMessageboxLeftButton,
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

    if self._tableStyle[MB_STYLE_MODIFY] ~= nil then
        local offsetX =edgeWidth+SX(4)
        local offsetY =boxSize.height/2+SY(6)
        local label = CCLabelTTF:labelWithString(self._tableParam.prompt, FONT_NAME, FONT_SM_SIZE)
        label:setAnchorPoint(CCPoint(0, 1))
        label:setPosition(CCPoint(offsetX, offsetY))
        messageBox:addChild(label, 0)
    --  offsetY = offsetY + label:getContentSize().height/2
        local size = CCSize(boxSize.width/3, SY(22))
        local edit = CScutEdit:new()
        edit:init(false, false)

        offsetX = messageBox:getPosition().x + edgeWidth+label:getContentSize().width+SY(6)
        offsetY =pWinSize.height-messageBox:getPosition().y-boxSize.height/2-SY(6)-size.height+label:getContentSize().height
        edit:setRect(CCRect(offsetX, offsetY, size.width, size.height))
        self._edit = edit
    end

    if self._tableStyle[MB_STYLE_PROMPT] ~= nil then
        offsetX = parentSize.width/4
        offsetY = parentSize.height/5
        local prompt = CCLabelTTF:labelWithString(self._message, FONT_NAME, FONT_SM_SIZE)
        prompt:setAnchorPoint(CCPoint(0.5, 1))
        prompt:setPosition(CCPoint(offsetX, offsetY))
        messageBox:addChild(prompt, 0)
    end

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



function ZyMessageBoxEx:setCallbackFun(fun)
    self._funCallback = fun
end


local mRankLayer
local mLayer
local tip1 
local tip2 
local tip3 
local bgLayer
local submitLayer
local allTable={};
local mList;
local mNameEdit;
local mScoreEdit;
local isCanSubmit = true
local isCanGetRank = true 
local mNameStr;
local mScoreStr;

-- close the ranking layer
function closeBtnActon()
    if bgLayer then 
        mScene:removeChild(bgLayer,true);
		bgLayer = nil 
    end 
end

function showRank()
    if isCanGetRank == false then 
        return 
    end 
	-- if addressPath is not nil   use socket connect 
    local addressPath="ph.scutgame.com:9001"
    ScutDataLogic.CNetWriter:getInstance():writeString("ActionId",1001)
    ScutDataLogic.CNetWriter:getInstance():writeString("PageIndex",1  )
    ScutDataLogic.CNetWriter:getInstance():writeString("PageSize",30)
    ZyExecRequest(mScene, nil,false,addressPath )
	if labelIds1 then 
	    labelIds1:setVisible(false)
	end
	labelIds2 = CCLabelTTF:create(IDS_TCP_CONNECTING, "fsfe", FONT_SM_SIZE);
	labelIds2:setPosition(labelIds1:getPosition());
	mScene:addChild(labelIds2, 99);
	
end

function submitOK()    
        local name= mNameEdit:getText()
        local sorce= mScoreEdit:getText()
        if  name==""  or sorce == "" then
            local box = ZyMessageBoxEx:new()
            box:doPrompt(mScene, nil,IDS_EMPTY_TIP,IDS_OK,messageCallback)
            mNameEdit:setVisible(false);
            mScoreEdit:setVisible(false);
        else
	    local addressPath="ph.scutgame.com:9001"
        ScutDataLogic.CNetWriter:getInstance():writeString("ActionId",1000)
        ScutDataLogic.CNetWriter:getInstance():writeString("UserName",name )
        ScutDataLogic.CNetWriter:getInstance():writeString("Score",sorce)    
        ZyExecRequest(mScene, nil,false,addressPath) 
        end
end

function createUIBg(titleImagePath,titleStr,textColor,closeBtnActionPath, touming)
    local layer = CCLayer:create()
    layer:setAnchorPoint(CCPoint(0,0))
    layer:setPosition(CCPoint(0,0))

    local bgPic = CCSprite:create(P(image_mainscene))
    bgPic:setAnchorPoint(CCPoint(0,0))
    bgPic:setScaleX(pWinSize.width/bgPic:getContentSize().width)
    bgPic:setScaleY(pWinSize.height/bgPic:getContentSize().height)
    bgPic:setPosition(CCPoint(0,0))
    layer:addChild(bgPic, 0, 0)
    layer:setContentSize(pWinSize)

    if touming then
        local toumingBg = CCSprite:create(P("common/panel_1002_12.png"))
        toumingBg:setScaleX(pWinSize.width*0.92/toumingBg:getContentSize().width)
        toumingBg:setScaleY(pWinSize.height*0.8/toumingBg:getContentSize().height)
        toumingBg:setAnchorPoint(CCPoint(0,0))
        toumingBg:setPosition(CCPoint(pWinSize.width*0.04, pWinSize.height*0.06))
        layer:addChild(toumingBg, 0)
    end

    local closeBtn = nil
    if closeBtnActionPath ~= nil then
        local norClose = CCSprite:create(P(Image.image_close))
        local norDown = CCSprite :create(P(Image.image_close))
        norDown:setScale(0.9)
        local menuItem = CCMenuItemSprite:itemFromNormalSprite(norClose, norDown)
        norClose:setAnchorPoint(CCPoint(0.5,0.5))
        norDown:setAnchorPoint(CCPoint(0.5, 0.5))
        norClose:setPosition(CCPoint(menuItem:getContentSize().width/2, menuItem:getContentSize().height/2))
        norDown:setPosition(CCPoint(menuItem:getContentSize().width/2, menuItem:getContentSize().height/2))
        
        closeBtn = CCMenu:menuWithItem(menuItem)
        menuItem:setAnchorPoint(CCPoint(0, 0))
        menuItem:setPosition(CCPoint(0, 0))
        menuItem:registerScriptHandler(closeBtnActionPath)
        closeBtn:setContentSize(menuItem:getContentSize())
        layer:addChild(closeBtn, 0, 0)
        closeBtn:setPosition(CCPoint(layer:getContentSize().width - closeBtn:getContentSize().width,layer:getContentSize().height - closeBtn:getContentSize().height-SY(3)))
    end

    local titleImageLabel = nil
    local titleStrLabel   = nil
    if titleImagePath then
        titleImageLabel = CCSprite:create(P(titleImagePath))
        titleImageLabel:setAnchorPoint(CCPoint(0,0))
        titleImageLabel:setPosition(CCPoint(pWinSize.width*0.5-titleImageLabel:getContentSize().width*0.5,pWinSize.height*0.95-titleImageLabel:getContentSize().height))
        layer:addChild(titleImageLabel,0)
    end
    
    
    if titleStr then
        titleStrLabel = CCLabelTTF:create(titleStr, FONT_NAME, FONT_BIG_SIZE)
        titleStrLabel:setAnchorPoint(CCPoint(0.5,0))
        titleStrLabel:setPosition(CCPoint(pWinSize.width*0.5,pWinSize.height-titleStrLabel:getContentSize().height-SY(15)))
        layer:addChild(titleStrLabel,0)
        if textColor ~= nil then
            titleStrLabel:setColor(textColor)
        end 
    end
    
    return layer
    
end

function messageCallback()
    mNameEdit:setVisible(true);
    mScoreEdit:setVisible(true);
end

function submitCancle()
    closeSubmitLaye()
end 

function submit()
    if isCanSubmit == false then
        return 
    end 
    isCanSubmit = false
    isCanGetRank = false
    local aa=nil
    local ww=288
    local hh=0
    local xx=(pWinSize.width-ww)/2
    local imgSprite=CCSprite:create(P(image_list_txt));
    local txt_h= imgSprite:getContentSize().height
    submitLayer = CCLayer:create();
    submitLayer:setContentSize(CCSize(SX(240),SY(160)));
    mScene:addChild(submitLayer);
    submitLayer:setAnchorPoint(PT(0.5,0.5));
    submitLayer:setPosition(PT(mScene:getContentSize().width/2, mScene:getContentSize().height/2));
    local sprite = CCSprite:create(P("common/panel_1002_12.png"))
    sprite:setScaleX(SX(240)/sprite:getContentSize().width);
    sprite:setScaleY(SY(160)/sprite:getContentSize().height);
    submitLayer:addChild(sprite,0);
    local startY = 0
    local titleName1=CCLabelTTF:create(IDS_NICKNAME,FONT_NAME, FONT_DEF_SIZE);
    titleName1:setAnchorPoint(CCPoint(0,0))
    titleName1:setPosition(CCPoint(SX(-100),
        startY+titleName1:getContentSize().height - SY(30)))
    submitLayer:addChild(titleName1)
    titleName1:setColor(ccc3(0,0,0))

    local txt_x=titleName1:getPositionX()+SX(8)+titleName1:getContentSize().width
    local txt_ww=xx+ww-txt_x-SX(44)
    
    local bgEmCCPointy1= CCSprite:create(P(image_list_txt))
  
    mNameEdit =  CCEditBox:create(CCSize(SX(120),bgEmCCPointy1:getContentSize().height), CCScale9Sprite:create(P(image_list_txt)))
    mNameEdit:setPosition(CCPoint(titleName1:getPositionX()+ titleName1:getContentSize().width +  SX(60) ,titleName1:getPositionY()+SY(5)))
    mNameEdit:setFontColor(ccc3(0,0,0))
    submitLayer:addChild(mNameEdit)

    local titleName=CCLabelTTF:create(IDS_SORCE, "sfeew", FONT_DEF_SIZE);
    titleName:setColor(ccc3(0,0,0))
    titleName:setAnchorPoint(CCPoint(0,0))
    aa=(hh/2-titleName:getContentSize().height)/2
    titleName:setPosition(CCPoint(titleName1:getPositionX(),titleName1:getPositionY()+txt_h+SY(10)))
    submitLayer:addChild(titleName)

    mScoreEdit = CCEditBox:create(CCSize(SX(120),bgEmCCPointy1:getContentSize().height), CCScale9Sprite:create(P(image_list_txt)))
    mScoreEdit:setFontColor(ccc3(0,0,0))
    mScoreEdit:setPosition(CCPoint(titleName:getPositionX() + titleName:getContentSize().width + SX(60) ,titleName:getPositionY()+SY(5)))
    submitLayer:addChild(mScoreEdit)
    mScoreEdit:setVisible(true)
    
    local button2 = ZyButton:new("button/button_1011.png", "button/button_1012.png",nil,IDS_OK)
    button2:setPosition(PT(SX(-30) -button2:getContentSize().width,SY(-50)));
    button2:addto(submitLayer,0)
    button2:registerScriptTapHandler(submitOK);
    
    local button3 = ZyButton:new("button/button_1011.png", "button/button_1012.png",nil,IDS_CANCLE)
    button3:setPosition(PT(SX(30) ,SY(-50)));
    button3:addto(submitLayer,0)
    button3:registerScriptTapHandler(submitCancle);
end 

function closeSubmitLaye()
     mScene:removeChild(submitLayer,true)
     isCanSubmit = true 
     isCanGetRank = true 
end 

function init()
    if mScene then
        return
    end
    local scene = ScutScene:new()
    mScene = scene.root
    scene:registerCallback(netCallback)

    CCDirector:sharedDirector():pushScene(mScene)
    pWinSize = mScene:getContentSize()
    
    mLayer = CCLayer:create()
    mLayer:setAnchorPoint(CCPoint(0,0))
    mLayer:setPosition(CCPoint(0,0))
    mScene:addChild(mLayer, 0)
	
	
    
    mRankLayer = CCLayer:create();
    mRankLayer:setAnchorPoint(PT(0.5, 0.5));
    mRankLayer:setPosition(PT(pWinSize.width/2, pWinSize.height/2));

    CCDirector:sharedDirector():pushScene(mScene)

    local bgSprite=CCSprite:create(P("beijing.jpg"))
    bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
    bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
    bgSprite:setAnchorPoint(CCPoint(0.5,0.5))
    bgSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2));
    mScene:addChild(bgSprite);
    --ScutDataLogic.CNetWriter:setUrl("http://ph.scutgame.com/service.aspx")
    

    local button = ZyButton:new("icon_1011.png");
    button:addto(mScene,0);
    button:setPosition(PT(pWinSize.width - button:getContentSize().width - SX(10), SY(10)));
    button:registerScriptTapHandler(showRank)
    
    local button2 = ZyButton:new("button/button_1011.png", "button/button_1012.png",nil,IDS_SUBMIT)
    button2:setPosition(PT(pWinSize.width/2 - button2:getContentSize().width/2 ,SY(10)));
    button2:addto(mScene,0)
    button2:registerScriptTapHandler(submit);
	
	
	-- 请按右下角排行按钮进行TCP连接
	labelIds1 = CCLabelTTF:create(IDS_TEST,"sfeew", FONT_SM_SIZE);
	labelIds1:setPosition(PT(SX(20) + labelIds1:getContentSize().width/2 , pWinSize.height - SY(18)));
	labelIds1:setAnchorPoint(PT(0.5,0.5));
	mScene:addChild(labelIds1,99);
end

function netCallback(pZyScene, lpExternalData, isTcp)
    local actionID = ZyReader:getActionID()
    local lpExternalData = lpExternalData or 0
    local userData = ZyRequestParam:getParamData(lpExternalData)
    if actionID==1001 then
        local table = _1001Callback(pZyScene, lpExternalData);
		if labelIds2 then 
			labelIds2:setVisible(false)
		end
		labelIds3 = CCLabelTTF:create(IDS_TCP_CONNECTED, "xxxx", FONT_SM_SIZE);
		labelIds3:setPosition(labelIds2:getPosition());
		mScene:addChild(labelIds3);
		if isTcp == true then 
			mConnectNum = mConnectNum + 1 ;
		end 
		if labelIds4 == nil  then 
			labelIds4 = CCLabelTTF:create(string.format(IDS_CONNECT_COUNT,mConnectNum), "xxxx", FONT_SM_SIZE);
			labelIds4:setPosition(PT(labelIds3:getPositionX() , labelIds3:getPositionY() - SY(15)));
			mScene:addChild(labelIds4, 99);
		else
			labelIds4:setString(string.format(IDS_CONNECT_COUNT,mConnectNum));
		end
		
        if table then
		    if bgLayer == nil then 
				bgLayer= createUIBg(nil,nil,ccc3(255,255,255),nil,true)
				mScene:addChild(bgLayer)
				local closeBtn=ZyButton:new(image_exit, image_exit);
				closeBtn:setPosition(PT(bgLayer:getContentSize().width-closeBtn:getContentSize().width - SX(15),bgLayer:getContentSize().height-closeBtn:getContentSize().height - SY(5)));
				closeBtn:registerScriptTapHandler(closeBtnActon);
				closeBtn:addto(bgLayer,99);
				showLayout(table.RecordTabel)
            end 
        end
    elseif actionID == 1000 then
          _1000Callback(pZyScene, lpExternalData);
    end
end

function showLayout(data)
    --[[
    if layoutLayer then
        mLayer:removeChild(layoutLayer,true)
        layoutLayer=nil;
		return
    end
	]]
    layoutLayer=CCLayer:create()
    bgLayer:addChild(layoutLayer,1)
    local simpleW=(pWinSize.width*0.8)/3
    local Bg=CCSprite:create(P(panle_1019_1_9))
    local scalex=simpleW/Bg:getContentSize().width
    local scaleList={0.7,1.1,1.2}
    local cursor=pWinSize.width*0.1
    local listBgStartY=pWinSize.height*0.75
    allTable.ranKingTitlePos={}
    allTable.biliSize={}

    local table=nil
    table={IDS_ORDER,IDS_NICKNAME1,IDS_SORCE1}
   allTable.title={}
   for i,v in ipairs(table) do
        local temp=scaleList[i]
        local textlabel=CCLabelTTF:create(v,FONT_NAME,FONT_SM_SIZE)
        textlabel:setAnchorPoint(PT(0.5,0))
        textlabel:setColor(ccRED1)
        textlabel:setPosition(PT(cursor+simpleW*temp/2,listBgStartY+SY(2)))
        allTable.ranKingTitlePos[i]=(textlabel:getPositionX())
        allTable.biliSize[i]=simpleW*scaleList[i]
        cursor=cursor+(simpleW*scaleList[i])
        layoutLayer:addChild(textlabel,1)
        allTable.title[i]=textlabel
    end
   mScrollView = CCScrollView:create()
   mScrollSize = SZ(pWinSize.width*0.8 , pWinSize.height*0.63)
   if nil ~= mScrollView then
      mScrollView:setViewSize(mScrollSize)
      mScrollView:setPosition(PT(pWinSize.width*0.1,listBgStartY - pWinSize.height*0.63))
      mScrollView:setScale(1.0)
      mScrollView:ignoreAnchorPointForPosition(true)
      mScrollView:setDirection(kCCScrollViewDirectionVertical)
    end
    layoutLayer:addChild(mScrollView)
	update(data)
end

function update(mtable)
    if bgLayer == nil then 
		return 
	end 
    local bgEmpty  = CCSprite:create(P(panle_1019_1_9))
    local layer = CCLayer:create()
    layer:setContentSize(CCSize(mScrollSize.width, bgEmpty:getContentSize().height*#mtable))
    layer:setPosition(PT(0, -bgEmpty:getContentSize().height*#mtable + mScrollSize.height))
    if layer and mScrollView then
        mScrollView:setContainer(layer)
        mScrollView:updateInset()
    end
    
    for i, v in ipairs(mtable) do
        local bgEmpty  = CCSprite:create(P(panle_1019_1_9))
        local bgLayer =  CCLayer:create()
        bgLayer:setContentSize(CCSize(mScrollSize.width, bgEmpty:getContentSize().height))
        layer:addChild(bgLayer,0)
        bgLayer:setPosition(PT(-SX(50),layer:getContentSize().height-i*bgEmpty:getContentSize().height))
        local table={i,v.UserName,v.Score }
        for k, v in ipairs(table) do
            local bgEmpty= CCScale9Sprite:create(P(panle_1019_1_9));
                bgEmpty:setContentSize(CCSize(allTable.biliSize[k]*0.95,bgEmpty:getContentSize().height));
                bgEmpty:setAnchorPoint(PT(0.5,0.5))
                bgEmpty:setPosition(PT(allTable.ranKingTitlePos[k]-layer:getPositionX(),pWinSize.height*0.63/4/2))
                bgLayer:addChild(bgEmpty,0)
                    local value=CCLabelTTF:create(v,FONT_NAME,FONT_SM_SIZE)
                    value:setAnchorPoint(PT(0.5,0.5))
                    value:setPosition(PT(allTable.ranKingTitlePos[k]-layer:getPositionX(),pWinSize.height*0.63/4/2))
                    bgLayer:addChild(value,0)
        end
    end
end

function _1001Callback(pZyScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == 0 then
        DataTabel={}
        DataTabel.PageCount= ZyReader:getInt()
        local RecordNums_1=ZyReader:getInt()
         local RecordTabel_1={}
          if RecordNums_1~=0 then
            for k=1,RecordNums_1 do
             local mRecordTabel_1={}
             ZyReader:recordBegin()
                mRecordTabel_1.UserName= ZyReader:readString()
                mRecordTabel_1.Score= ZyReader:getInt()
                ZyReader:recordEnd()
                table.insert(RecordTabel_1,mRecordTabel_1)
              end
        end
        DataTabel.RecordTabel = RecordTabel_1;
    else
          local box = ZyMessageBoxEx:new()
          box:doPrompt(pZyScene, nil, ZyReader:readErrorMsg(),IDS_OK)
    end
    return DataTabel
end

function _1000Callback(pZyScene, lpExternalData)
    local DataTabel=nil
    if ZyReader:getResult() == 0 then
        DataTabel={}
       closeSubmitLaye()
    else
          local box = ZyMessageBoxEx:new()
          box:doPrompt(pZyScene, nil, ZyReader:readErrorMsg(), IDS_OK)
    end
    return DataTabel
end
