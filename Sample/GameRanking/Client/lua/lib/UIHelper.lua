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

--左边图标 + 右边文字 --
--offsetPixel: The Distance offset the icon right
--return icon sprite
function IconLabel(bgImage, iconName, text, fontSize, offsetPixel, color)

	local  sprite = nil
	if     bgImage ~= nil
	then
	       sprite = CCSprite:create(P(bgImage))
	else
	       sprite = CCNode:node()
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
	local bgPic = CCSprite:create(P(Image.image_mainscene))
	bgPic:setAnchorPoint(CCPoint(0,0))
	bgPic:setScaleX(pWinSize.width/bgPic:getContentSize().width)
	bgPic:setScaleY(pWinSize.height/bgPic:getContentSize().height)
	bgPic:setPosition(CCPoint(0,0))
	layer:addChild(bgPic, 0, 0)
	layer:setContentSize(pWinSize)
	
	--半透明背景
	if touming then
		local toumingBg = CCSprite:create(P("common/panel_1002_12.png"))
		toumingBg:setScaleX(pWinSize.width*0.92/toumingBg:getContentSize().width)
		toumingBg:setScaleY(pWinSize.height*0.8/toumingBg:getContentSize().height)
		toumingBg:setAnchorPoint(CCPoint(0,0))
		toumingBg:setPosition(CCPoint(pWinSize.width*0.04, pWinSize.height*0.06))
		layer:addChild(toumingBg, 0)		
	end
	
	--[[local fgPic = CCSprite:create(P(Image.image_frontground))
	fgPic:setAnchorPoint(CCPoint(0,0))
	fgPic:setScaleX(pWinSize.width/fgPic:getContentSize().width)
	fgPic:setScaleY(pWinSize.height/fgPic:getContentSize().height)
	fgPic:setPosition(CCPoint(0,0))
	layer:addChild(fgPic, 0, 0)
	layer:setContentSize(pWinSize)]]

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

-- 可点击的透明标签

-- 可点击的透明背景标签
function labelClickable(title, fontName, fontSize, listtener, fontColor, tag)
	local label = CCLabelTTF:create(title, fontName, fontSize)
	if fontColor ~= nil then
		label:setColor(fontColor)
	end

	local background = CCMenuItemLabel:itemWithLabel(label)
	background:setContentSize(label:getContentSize())
	background:setAnchorPoint(CCPoint(0, 0))
	background:setPosition(CCPoint(0, 0))
	background:registerScriptHandler(listtener)

	if tag ~= nil then
		background:setTag(tag)
	end	
	
	local menuItem = CCMenu:menuWithItem(background)
	menuItem:setPosition(CCPoint(0, 0))
	menuItem:setContentSize(label:getContentSize())
   	return menuItem
end





--关闭按钮
--[[
--layer 显示的层
--  CloseBtn CloseDown  可传入图片
-- closeBtnListener    按键监听
--]]
function BtnClose(layer,CloseBtn,CloseDown,closeBtnListener,tag)
        if CloseBtn==nil then
           CloseBtn=Image.image_close
        end
        if CloseDown ==nil then 
           CloseDown=Image.image_close
        end
        if  tag==nil then
            tag=0
        end
        local rightEdge=SX(8)
        local topEdge=SX(8)
		local norClose = CCSprite:create( P(CloseBtn))
		local norDown = CCSprite:create( P(CloseDown))
		      norDown:setScale(0.9)
		local menuItem = CCMenuItemSprite:itemFromNormalSprite(norClose, norDown)
		norClose:setAnchorPoint(CCPoint(0.5,0.5))
		norDown:setAnchorPoint(CCPoint(0.5, 0.5))
		norClose:setPosition(CCPoint(menuItem:getContentSize().width/2, menuItem:getContentSize().height/2))
		norDown:setPosition(CCPoint(menuItem:getContentSize().width/2, menuItem:getContentSize().height/2))

		local btn = CCMenu:menuWithItem(menuItem)
		menuItem:setAnchorPoint(CCPoint(0, 0))
		menuItem:setPosition(CCPoint(0, 0))
		
		if closeBtnListener ~= nil then
		   menuItem:registerScriptHandler(closeBtnListener)
		end
		btn:setContentSize(menuItem:getContentSize())
		if layer~=nil then 
		layer:addChild(btn, tag, 0)
		btn:setPosition(CCPoint(layer:getContentSize().width - btn:getContentSize().width-rightEdge ,
		                layer:getContentSize().height - btn:getContentSize().height-topEdge))
		end
        
       return btn,menuItem;	
end
--滚动条
--orgX ,orgY滚动layer的位置设置
--strroll 滚球图片
--strbar 滚轴图片
--width  滚轴宽度
--count  滚动轴个数
--height 2个滚轴间的高度间隔

function rollbar(parent,orgX,orgY,strroll,strbar,width,count,height)
    if strroll==nil then
        strroll="rollbar/menu_1001_22.png"
    end
    if strbar==nil then
        strbar=Image.image_button_red_c_0
    end
    rolllayer=CCLayer:create()
    local offsetY=-SY(10)
    mBar={}
    sBar={}
    value=count
    if height==nil then
        height=0
    end
    for index=1 ,value ,1 do
        local menuItem = CCSprite:create(P(strroll))
        menuItem:setAnchorPoint(CCPoint(0.2, 0.5))
        mBar[index]=menuItem
       
        local bgBar=CCSprite:create(P(strbar))
        bgBar:setAnchorPoint(CCPoint(0,0))
        bgBar:setPosition(CCPoint(0,offsetY))
        local sizeOld = bgBar:getContentSize()
        bgBar:setScaleX(width / sizeOld.width)
        rolllayer:addChild(bgBar,0)
               
        bgBar:setContentSize(sizeOld)
        local sbar=CCSprite:create(P(Image.image_button_red_c_0))
        sbar:setAnchorPoint(CCPoint(0,0))
        sbar:setPosition(CCPoint(SX(6),offsetY+SY(5)))
        local sizeOld = sbar:getContentSize()
        sbar:setScaleY(sizeOld.height / sizeOld.height)
        sbar:setContentSize(sizeOld)
        sBar[index]=sbar
        menuItem:setPosition(CCPoint(0,bgBar:getPosition().y+sizeOld.height/10*8))
        rolllayer:addChild(menuItem,1)
        
        rolllayer:addChild(sbar,0)
        rolllayer:setPosition(CCPoint(orgX,orgY))
        offsetY=offsetY+height
	end
	rolllayer:setContentSize(CCSize(width,height))
    rolllayer:setIsTouchEnabled(true)
	parent:addChild(rolllayer,0)
	return rolllayer,mBar
end

function tBegan(e,nowvalue,max,nowindex,moveheight)
    if e ~= nil then
        if moveheight==nil then
            moveheight=0
        end
        for k,v in ipairs(e) do
            local pointBegin = v:locationInView(v:view())
            pointBegin = CCDirector:sharedDirector():convertToGL(pointBegin)
            pointBegin=ccpSub(pointBegin,rolllayer:getPosition())
            local spriteBarsz={}
            local spriteSize={}
            local positionCurrent={}
            local long={}
            for index=1,value,1 do
                long[index]={}
                spriteBarsz[index]=mBar[index]:getTextureRect();
                spriteSize[index]=spriteBarsz[index].size;
                positionCurrent[index] = mBar[index]:getPosition()   
                if pointBegin.x>positionCurrent[index].x-SX(5) and pointBegin.x<positionCurrent[index].x+spriteSize[index].width+SX(5) and
                    pointBegin.y+moveheight<positionCurrent[index].y+spriteSize[index].height+SY(5) and  pointBegin.y+moveheight>positionCurrent[index].y-SY(10) then           
                    mMouseBeginCCPoint = pointBegin  
                    curindex=index
                elseif pointBegin.y+moveheight<=positionCurrent[index].y+spriteSize[index].height+SY(5)  and  pointBegin.y+moveheight>=positionCurrent[index].y-SY(10) then
                    local pos=pointBegin
                    if pos.x>-mBar[index]:getContentSize().width and pos.x <rolllayer:getContentSize().width then
                        if pos.x>rolllayer:getContentSize().width-mBar[index]:getContentSize().width/2-SX(3) then
                            pos.x=rolllayer:getContentSize().width-mBar[index]:getContentSize().width/2-SX(3)
                        end
                        if pos.x<0 then
                            pos.x=0
                        end
                        sBar[index]:setScaleX(pos.x/ sBar[index]:getContentSize().width)
                        mBar[index]:setPosition(CCPoint(pos.x,positionCurrent[index].y))
                        long[index].value=pos.x
                        long[index].isvalue=true
                        return long,rolllayer:getContentSize().width- mBar[index]:getContentSize().width/2-SX(3)                       
                    end
                end                 
            end
        end
    elseif value~=nil then
        if mBar[nowindex]~=nil then
            if max~=0 then
                local pos=nowvalue/max*(rolllayer:getContentSize().width- mBar[nowindex]:getContentSize().width/2-SX(3))
                sBar[nowindex]:setScaleX(pos/ (sBar[nowindex]:getContentSize().width))
                mBar[nowindex]:setPosition(CCPoint(pos,mBar[nowindex]:getPosition().y))
            end
        end
    end
end

function tMoved(e)
    if e ~= nil then
        local long
        if mMouseBeginCCPoint ~=nil and curindex~=nil then
            for k,v in ipairs(e) do
                local long={}
                local index=nil
                local touchLocation =  v:locationInView(v:view() )
                local pointBegin = CCDirector:sharedDirector():convertToGL(touchLocation)
                local prevLocation = v:previousLocationInView(v:view())
                for i=1,value,1 do
                    long[i]={}
                end
                local diff = ccpSub(touchLocation, prevLocation)
                local currentPos = mBar[curindex]:getPosition()
                local pos = ccpAdd(currentPos, diff)
                if pos.x< 0 then
                    pos.x = 0
                end
                if pos.x > rolllayer:getContentSize().width-mBar[curindex]:getContentSize().width/2-SX(3) then
                    pos.x = rolllayer:getContentSize().width-mBar[curindex]:getContentSize().width/2-SX(3)
                end
                sBar[curindex]:setScaleX(pos.x/ sBar[curindex]:getContentSize().width)
                mBar[curindex]:setPosition(CCPoint(pos.x,mBar[curindex]:getPosition().y))
                long[curindex].value=pos.x
                long[curindex].isvalue=true
                return long,rolllayer:getContentSize().width- mBar[curindex]:getContentSize().width/2-SX(3)
            end
        end
    end
end

function tEnd(e)
    if e ~= nil then
        mMouseBeginCCPoint=nil
        curindex=nil
    end
end


function createGuideMessage(type,text,fontSize)

    local imagePath = nil
    if type == 2 then--竖指
        imagePath = "common/icon_1411.9.png"
    else
        imagePath = "common/icon_1408.9.png"--横指
    end
	local  sprite = CCNode:node()
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
    textLable:setPosition(CCPoint(textPx,textPy))
    
    if nScaleX < 1 then
        nScaleX = 1
    end
      sprite:setContentSize(SZ(icon:getContentSize().width*nScaleX,icon:getContentSize().height))
	return sprite
end

--创建一个没有图片
function  createActionRect(rect,fun,tag)
   	local pMenuNode =CCNode:create()
	pMenuNode:setContentSize(rect)
	pMenuNode:setPosition(CCPoint(0,0))
	pMenuNode:setAnchorPoint(CCPoint(0,0))
	local menuItemHead = CCMenuItemSprite:create(pMenuNode, nil)
	menuItemHead:setAnchorPoint(CCPoint(0,0))
	menuItemHead:setPosition(CCPoint(0,0))
	if tag then
		menuItemHead:setTag(tag)
	end
	if fun~=nil then
		menuItemHead:registerScriptTapHandler(fun)
	end
   	local menu = CCMenu:createWithItem(menuItemHead)
   	menu:setAnchorPoint(CCPoint(0,0))
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
	bg:setAnchorPoint(CCPoint(0,0))
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



--
function createMainDestBg(bHeight)
	local layer=CCLayer:create()
	local bottomHeight=bHeight or pWinSize.height*0.08 
	-- 此处添加场景初始内容
	local topSprite=CCSprite:create(P("common/panel_1001_1.png"))
	topSprite:setScaleX(pWinSize.width/topSprite:getContentSize().width)
	topSprite:setAnchorPoint(CCPoint(0.5,0))
	topSprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height-topSprite:getContentSize().height))
	layer:addChild(topSprite,0)
	local midHeight=pWinSize.height
	
	----
	 local bottomSprite=CCSprite:create(P("common/panel_1001_4.png"))
	 bottomSprite:setAnchorPoint(CCPoint(0,1))
	 bottomSprite:setScaleX(pWinSize.width/bottomSprite:getContentSize().width)
	  bottomSprite:setPosition(CCPoint(0,bottomHeight-SY(2)))
	 layer:addChild(bottomSprite,1)

	local bgSprite=CCSprite:create(P("common/beijing.jpg"))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(midHeight/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(CCPoint(0.5,0))
	bgSprite:setPosition(CCPoint(pWinSize.width/2,0))
	layer:addChild(bgSprite,0)
	
	local midBox=CCSize(pWinSize.width*0.94,midHeight*0.9)
	local midSprite=CCSprite:create(P("common/panel_1001_2.png"))
	midSprite:setAnchorPoint(CCPoint(0.5,0.5))
	midSprite:setScaleX(midBox.width/midSprite:getContentSize().width)
	midSprite:setScaleY(midBox.height/midSprite:getContentSize().height)
	midSprite:setPosition(CCPoint(pWinSize.width/2,bottomHeight+midHeight/2))
--	layer:addChild(midSprite,0)
	return layer
end;

