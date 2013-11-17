module("UIHelper", package.seeall)

--return button and LableText
function Button(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, tag, bCheck, disablePic)

	menuItem = MenuItem(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, bCheck, disablePic)
	if tag ~= nil then
		menuItem:setTag(tag)
	end

	local Btn = CCMenu:createWithItem(menuItem)
	Btn:setContentSize(menuItem:getContentSize())
	return Btn, menuItem
end


function MenuItem(normalPic, downPic, listtener, strText, TextAlign, fontSize, color, bCheck, disablePic)
	local strNor     = nil
	local strDown    = nil
	local strDisable = nil
	if normalPic ~= nil then
		strNor = P(normalPic)
	end

	if downPic ~= nil then
		strDown = P(downPic)
	end

	if disablePic ~= nil then
		strDisable = P(disablePic)
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
		menuItem:registerScriptTapHandler(listtener)
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
--左边图标 + 右边文字 --
--offsetPixel: The Distance offset the icon right
--return icon sprite
function IconLabel(bgImage, iconName, text, fontSize, offsetPixel, color)

	local  sprite = nil
	if     bgImage ~= nil
	then
	       sprite = CCSprite:create(P(bgImage))
	else
	       sprite = CCNode:create()
	end
	local  size = sprite:getContentSize()
	       sprite:setIsRelativeAnchorPoint(false);
           sprite:setAnchorPoint(CCPoint(0, 0.5))

	local  icon = CCSprite:create(P(iconName))
	       icon:setAnchorPoint(CCPoint(0, 0.5))
	       icon:setPosition(CCPoint(0, size.height/2))
	       sprite:addChild(icon, 0, 0)
	local  iconSize = icon:getContentSize()

	       pLable  = CCLabelTTF:create(text, FONT_NAME, (fontSize))
	       pLable:setAnchorPoint(CCPoint(0, 0.5))
	       pLable:setPosition(CCPoint(icon:getPosition().x + iconSize.width + offsetPixel, size.height/2))
	if     color ~= nil
	then
		   pLable:setColor(color)
	end
	       sprite:addChild(pLable, 0, 0)

	return sprite, icon, pLable

end

--通用的UI背景 背景图加标题--
--添加关闭按钮图片
function createUIBg(titleImagePath,titleStr,textColor,closeBtnActionPath, touming)
	local layer = CCLayer:create()
	layer:setAnchorPoint(CCPoint(0,0))
	layer:setPosition(CCPoint(0,0))
	
	--大背景
	local bgPic = CCSprite:create(P(Image.image_background))
	bgPic:setAnchorPoint(CCPoint(0,0))
	bgPic:setScaleX(pWinSize.width/bgPic:getContentSize().width)
	bgPic:setScaleY(pWinSize.height/bgPic:getContentSize().height)
	bgPic:setPosition(CCPoint(0,0))
	layer:addChild(bgPic, 0, 0)
	layer:setContentSize(pWinSize)
	
	--半透明背景
	if touming then
		local toumingBg = CCSprite:create(P("common/List_2009_3.9.png"))
		toumingBg:setScaleX(pWinSize.width*0.92/toumingBg:getContentSize().width)
		toumingBg:setScaleY(pWinSize.height*0.915/toumingBg:getContentSize().height)
		toumingBg:setAnchorPoint(PT(0,0))
		toumingBg:setPosition(PT(pWinSize.width*0.04, pWinSize.height*0.06))
		layer:addChild(toumingBg, 0)		
	end
	
	local fgPic = CCSprite:create(P(Image.image_frontground))
	fgPic:setAnchorPoint(CCPoint(0,0))
	fgPic:setScaleX(pWinSize.width/fgPic:getContentSize().width)
	fgPic:setScaleY(pWinSize.height/fgPic:getContentSize().height)
	fgPic:setPosition(CCPoint(0,0))
	layer:addChild(fgPic, 0, 0)
	layer:setContentSize(pWinSize)	

	local closeBtn = nil
	if closeBtnActionPath ~= nil then
	
	
	--[[
		local norClose = CCSprite:create(P(Image.image_close))
		local norDown = CCSprite:create(P(Image.image_close))
		norDown:setScale(0.9)
		local menuItem = CCMenuItemSprite:create(norClose, norDown)
		norClose:setAnchorPoint(PT(0.5,0.5))
		norDown:setAnchorPoint(PT(0.5, 0.5))
		norClose:setPosition(PT(menuItem:getContentSize().width/2, menuItem:getContentSize().height/2))
		norDown:setPosition(PT(menuItem:getContentSize().width/2, menuItem:getContentSize().height/2))
		
		closeBtn = CCMenu:createWithItem(menuItem)
		menuItem:setAnchorPoint(PT(0, 0))
		menuItem:setPosition(PT(0, 0))
		menuItem:registerScriptHandler(closeBtnActionPath)
		closeBtn:setContentSize(menuItem:getContentSize())
		layer:addChild(closeBtn, 0, 0)
		
		--]]
		
		closeBtn=ZyButton:new(Image.image_close)
		closeBtn:addto(layer,0)
		
		closeBtn:registerScriptHandler(closeBtnActionPath)
		closeBtn:setPosition(PT(layer:getContentSize().width - closeBtn:getContentSize().width,layer:getContentSize().height - closeBtn:getContentSize().height-SY(3)))
		
		
		
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

-- 可点击的透明标签



function createGuideMessage(type,text,fontSize)

    local imagePath = nil
    if type == 2 then--竖指
        imagePath = "common/icon_1411.9.png"
    else
        imagePath = "common/icon_1408.9.png"--横指
    end
	local  sprite = CCNode:create()
	local  icon = CCSprite:create(P(imagePath))
    icon:setAnchorPoint(CCPoint(0, 0))
    icon:setPosition(CCPoint(0, 0))
    sprite:addChild(icon,0)
	local  iconSize = icon:getContentSize()
	local textLable  = CCLabelTTF:create(text,FONT_NAME,(fontSize))
	
    textLable:setAnchorPoint(CCPoint(0, 0))
    textLable:setColor(ZyColor:colorRed())
    sprite:addChild(textLable,1)
    local textPx = SX(5)
	local textPy = icon:getContentSize().height/2
	
    
    local nScaleX  = (textLable:getContentSize().width+SX(20))/icon:getContentSize().width
    if type == 2  then
        if nScaleX > 1 then
            textPx = (icon:getContentSize().width*nScaleX-textLable:getContentSize().width)/2
        else
            textPx = (icon:getContentSize().width-textLable:getContentSize().width)/2
        end
        textPy = icon:getContentSize().height*0.2+(icon:getContentSize().height*0.8-textLable:getContentSize().height)/2
    else
        textLable:setAnchorPoint(CCPoint(0, 0.5))
        if nScaleX < 1 then
            textLable:setAnchorPoint(CCPoint(0.5, 0.5))
            textPx = (icon:getContentSize().width)/2-SX(5)
        end
    end
    icon:setScaleX(nScaleX)
    textLable:setPosition(PT(textPx,textPy))
    
    if nScaleX < 1 then
        nScaleX = 1
    end
      sprite:setContentSize(SZ(icon:getContentSize().width*nScaleX,icon:getContentSize().height))
	return sprite
end

--创建一个没有图片
function  createActionRect(rect,fun,tag)
   	local pMenuNode = CCNode:create()
	pMenuNode:setContentSize(rect)
	pMenuNode:setPosition(PT(0,0))
	pMenuNode:setAnchorPoint(PT(0,0))
	local menuItemHead = CCMenuItemSprite:create(pMenuNode, nil)
	menuItemHead:setAnchorPoint(PT(0,0))
	menuItemHead:setPosition(PT(0,0))
	if tag then
		menuItemHead:setTag(tag)
	end
	if fun~=nil then
		menuItemHead:registerScriptTapHandler(function ()fun(menuItemHead) end )
	end
   	local menu = CCMenu:createWithItem(menuItemHead)
   	menu:setAnchorPoint(PT(0,0))
	menu:setContentSize(pMenuNode:getContentSize())
	return menu,menuItemHead
end;


--口袋天界通用小背景
function createSmallBg(strtitlepic,strtitle, spriteBg)
	local layer=CCLayer:create()
	local bgImg = Image.image_halfbackground
	if spriteBg then
		bgImg = spriteBg
	end
	
	local bg=CCSprite:create(P(bgImg))
	bg:setScaleX(pWinSize.width/bg:getContentSize().width)
	bg:setScaleY(pWinSize.height*0.855/bg:getContentSize().height)
	bg:setAnchorPoint(PT(0,0))
	bg:setPosition(CCPoint(0, pWinSize.height*0.145))	
	layer:addChild(bg, 0, 0)
	layer:setContentSize(bg:getContentSize())


	--headPic
	if strtitlepic and strtitlepic ~= "" then
		local headTitleBg=CCSprite:create(P(strtitlepic))
		headTitleBg:setAnchorPoint(CCPoint(0.5, 1))
		headTitleBg:setPosition(CCPoint(bg:getContentSize().width/2, bg:getContentSize().height*0.95))
		bg:addChild(headTitleBg, 0, 0)

		--head label
	elseif strtitle and strtitle ~= "" then
		local label=CCLabelTTF:create(strtitle,FONT_NAME,FONT_SM_SIZE)
		label:setColor(ccc3(255,255,255))
		label:setAnchorPoint(CCPoint(0.5, 1))
		label:setPosition(CCPoint(bg:getContentSize().width/2, bg:getContentSize().height*0.95))
		bg:addChild(label, 0, 0)
	end
	return layer
end




