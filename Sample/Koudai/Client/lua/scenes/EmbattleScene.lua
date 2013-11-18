------------------------------------------------------------------
-- EmbattleScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 布阵界面
------------------------------------------------------------------

module("EmbattleScene", package.seeall)


local mScene = nil 		-- 场景

-- 场景入口
function pushScene()
	initResource()
	createScene()
end

function getBtnTable()
	local btnTable = {}
	btnTable.saveBtn = _saveBtn
	btnTable.memberTable = mercenariesBtn
	btnTable.openArrayTable = openArrayTable
	return btnTable
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()

	personalInfo=PersonalInfo.getPersonalInfo()

	local listBg = CCSprite:create(P("common/list_1052.9.png"));
	magic_width = listBg:getContentSize().width
	magic_height = 	pWinSize.width*0.2
	magic_x = (pWinSize.width-magic_width)*0.5
	magic_y = pWinSize.height*0.76
	
	hero_width = pWinSize.width*0.2
	hero_height = pWinSize.height*0.5
	hero_x = pWinSize.width*0.05
	hero_y = magic_y-hero_height-SY(12)
end

-- 释放资源
function releaseResource()
	showLayer = nil
	midContentLayer=nil
	mLayer = nil
	mScene = nil
	infoTable = nil
	_saveBtn=nil
	_useBtn = nil
	isGuide=nil
	m_fucImageLable=nil
	mClickTag = nil
end


function onTouch(eventType , x, y)
    if eventType == "began" then 
        return touchBegan(x,y)
    elseif eventType == "moved" then
        return touchMove(x,y)
    elseif eventType == "ended" then 
        return touchEnd(x,y)
    elseif eventType == "cancelled" then 
        return touchEnd(x,y)
    end
end 

-- 创建场景
function createScene()
	local scene  = ScutScene:new()
	mScene = scene.root
	mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
		
	-- 注册网络回调
	scene:registerCallback(networkCallback)	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)
	SlideInLReplaceScene(mScene,1)

	-- 注册触屏事件
--	mLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "EmbattleScene.touchBegan")
--	mLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "EmbattleScene.touchMove")
--	mLayer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "EmbattleScene.touchEnd")
	mLayer:setTouchEnabled(true)
    mLayer:registerScriptTouchHandler(onTouch)
	--创建背景
	creatBg()
	
	-- 此处添加场景初始内容
	MainMenuLayer.init(2, mScene)
	showContent()
	
	infoTable = nil
	sendAction(1901)
end

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end



function creatBg()
	local midSprite=CCSprite:create(P(Image.image_halfbackground))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.855)
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)	
	midSprite:setAnchorPoint(PT(0,0))
	midSprite:setPosition(PT(0,pWinSize.height*0.145))
	mLayer:addChild(midSprite,0)
	
	local headTitleBg=CCSprite:create(P("title/list_1106.png"))
	headTitleBg:setAnchorPoint(PT(0.5, 1))
	headTitleBg:setPosition(CCPoint(pWinSize.width/2, pWinSize.height*0.96))
	mLayer:addChild(headTitleBg, 0)
end;

function showContent()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	showLayer = layer
	
	--魔法阵列表
	local listBg = CCSprite:create(P("common/list_1052.9.png"));
	listBg:setScaleY(magic_height/listBg:getContentSize().height)
	listBg:setAnchorPoint(PT(0,0))
	listBg:setPosition(PT(magic_x, magic_y))
	layer:addChild(listBg, 0)
	---左右按钮 
	local lSprite=CCSprite:create(P("button/list_1069.png"))
	lSprite:setAnchorPoint(PT(1,0.5))
	lSprite:setPosition(PT(magic_x, magic_y+magic_height/2))
	layer:addChild(lSprite, 0)
	local rSprite=CCSprite:create(P("button/list_1068.png"))
	rSprite:setAnchorPoint(PT(0,0.5))
	rSprite:setPosition(PT(magic_x+magic_width, lSprite:getPosition().y))
	layer:addChild(rSprite, 0)	
	
	--
	local magicSize = SZ(magic_width*0.96, magic_height*0.9)
	list = ScutCxList:node(magicSize.width/4, ccc4(24, 24, 24, 0), magicSize)
	list:setHorizontal(true)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(magic_x+magic_width*0.02, magic_y+magic_height*0.05))
	layer:addChild(list,0)
	list:registerItemClickListener(EmbattleScene.choiceClick)
	list:setTouchEnabled(true)
	magicList = list
	
	--魔法阵描述文字
	local decLabel=CCLabelTTF:create("",FONT_NAME,FONT_SM_SIZE)
	decLabel:setAnchorPoint(PT(0,0))
	decLabel:setPosition(PT(magic_x, magic_y-SY(10)))
	layer:addChild(decLabel, 0)	
	magicDes = decLabel
		
	--佣兵列表

	local listBg = CCSprite:create(P("common/list_1038.9.png"));
	listBg:setScaleX(hero_width/listBg:getContentSize().width)
	listBg:setScaleY(hero_height/listBg:getContentSize().height)
	listBg:setAnchorPoint(PT(0,0))
	listBg:setPosition(PT(hero_x, hero_y))
	layer:addChild(listBg, 0)
	
	local bSprite=CCSprite:create(P("button/list_1070.png"))
	bSprite:setAnchorPoint(PT(0.5,0))
	bSprite:setPosition(PT(hero_x+hero_width/2, hero_y))
	layer:addChild(bSprite, 0)

	local heroSize = SZ(hero_width, hero_height-bSprite:getContentSize().height*1.2)
	list = ScutCxList:node(heroSize.height/4, ccc4(24, 24, 24, 0), heroSize)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(hero_x, hero_y+bSprite:getContentSize().height*1.1))
	layer:addChild(list,0)
	heroList = list		
	list:setTouchEnabled(true)
	--布阵区域
	matrix_width = pWinSize.width*0.68
	matrix_height = hero_height
	matrix_x = pWinSize.width*0.265
	matrix_y =listBg:getPosition().y
	local listBg = CCSprite:create(P("common/list_1038.9.png"));
	listBg:setScaleX(matrix_width/listBg:getContentSize().width)
	listBg:setScaleY(matrix_height/listBg:getContentSize().height)
	listBg:setAnchorPoint(PT(0,0))
	listBg:setPosition(PT(matrix_x, matrix_y))
	layer:addChild(listBg, 0)


	--保存阵法
	local saveBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,Image.image_button_hui_c,
						Language.EMBATTLE_SAVE_ZHENFA,FONT_NAME,FONT_SM_SIZE)
	saveBtn:setAnchorPoint(PT(0,0))
	saveBtn:setColor(ccYELLOW)
	saveBtn:addto(layer,0)
	saveBtn:registerScriptHandler(saveArrayAction)
	saveBtn:setPosition(PT(matrix_x+matrix_width*0.6, matrix_y+matrix_height*0.05))	
	_saveBtn = saveBtn
	
	--启用阵法
	local useBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,Image.image_button_hui_c,
						Language.EMBATTLE_STAR_ZHENFA,FONT_NAME,FONT_SM_SIZE)
	useBtn:setAnchorPoint(PT(0,0))
	useBtn:setColor(ccYELLOW)
	useBtn:addto(layer,0)
	useBtn:registerScriptHandler(useArrayAction)
	useBtn:setPosition(PT(matrix_x+matrix_width*0.4-useBtn:getContentSize().width, matrix_y+matrix_height*0.05))
	
	
	_useBtn = useBtn
end

---- 按下
function  touchBegan(x, y)
   --[[
	local pointBegin=nil
 	for k,v in ipairs(e) do
 		pointBegin = v:locationInView(v:view())
		pointBegin=CCDirector:sharedDirector():convertToGL(pointBegin)
	end
	]]
	pointBegin = PT(x,y)
	if pointBegin.y<magic_y then
		craeteClickSprite(pointBegin)
	end
	return 1
end;


-----创建选中的佣兵  可移动图标 
function  craeteClickSprite(pos)
	local headId,generalId = checkIsClickIn(pos)
	
	if generalId and checkIsInArrary(generalId) then
		return
	end
	
	if headId then
		local path=string.format("smallitem/%s.png", checkIsClickIn(pos))
		if clickSprite then
			clickSprite:getParent():removeChild(clickSprite,true)
			clickSprite=nil
		end
		clickSprite=CCSprite:create(P(path))
		clickSprite:setPosition(pos)
		mScene:addChild(clickSprite,10)


		if isGuideMove then
			local btnTable = EmbattleScene.getBtnTable()
			local item = nil
			for k,v in pairs(btnTable.openArrayTable) do
				if v.IsStations==0 then
					item = v.sprite
				end
			end
			if ArowGuideLayer then
				ArowGuideLayer:getParent():removeChild(ArowGuideLayer, true)
				ArowGuideLayer = nil
			end
			createArowGuide(item)
		end

	end
end;

-----判断点击下的点    是否点击在9宫格或者是佣兵列表
function  checkIsClickIn(pos)
----clickType  用于判断是点中 佣兵表 还是9宫格内的佣兵 
	if pos.x<heroList:getPosition().x+heroList:getContentSize().width and pos.y > heroList:getPosition().y then
	-----------------点击佣兵列表
		if mercenariesBtn then
			for k , v in pairs(mercenariesBtn) do
				local pt=pos
				local Position = v:getParent():convertToWorldSpace(v:getPosition())
				local box=CCRectMake( Position.x,Position.y,
				v:getContentSize().width, v:getContentSize().height);
				if CCRect.containsPoint(box, pt) then
					clickType=2
					clickSpriteInfo={}
					clickSpriteInfo.GeneralID=infoTable.GeneralID_[k].GeneralID
					clickSpriteInfo.HeadID=infoTable.GeneralID_[k].HeadID   
					clickSpriteInfo.GeneralQuality=infoTable.GeneralID_[k].GeneralQuality
					return  clickSpriteInfo.HeadID,clickSpriteInfo.GeneralID
				end
			end
		end
	else
-------------------------------------------------点击9宫格
		if openGeneralBtn then
			for k , v in pairs(openGeneralBtn) do
				local pt=pos
				local Position =v:getPosition()
				local box=CCRectMake( Position.x,Position.y,
				v:getContentSize().width, v:getContentSize().height);
				if CCRect.containsPoint(box, pt) then
					clickType=1
					clickSpriteInfo={}
					clickSpriteInfo.GeneralID=openArrayTable[k].EGeneralID
					clickSpriteInfo.HeadID=openArrayTable[k].GeneralHeadID
					clickSpriteInfo.index=openArrayTable[k].index
					clickSpriteInfo.GeneralQuality=openArrayTable[k].GeneralQuality
					clickSpriteInfo.position=	k				
					return   clickSpriteInfo.HeadID
				end
			end
		end
	end
	return false
end;

--移动
function  touchMove(x,y)
	local pointBegin=PT(x,y)
	--[[
 	for k,v in ipairs(e) do
 		pointBegin = v:locationInView(v:view())
		pointBegin=CCDirector:sharedDirector():convertToGL(pointBegin)
	end
	]]
	if clickSprite then
		clickSprite:setPosition(PT(pointBegin.x,pointBegin.y))
	end
end;

--是否可以放到格子里
function checkIsReplace(position)
	local nextIndex=openArrayTable[position].index
	local clickIndex = openArrayTable[clickSpriteInfo.position].index
	
	local isReplace = nil
	if (currentArray[nextIndex].IsReplace == 1 and clickSpriteInfo.GeneralID == personalInfo._GeneralID ) then--目标格子是替补  自己是主角
		isReplace = true
	end
	if currentArray[clickIndex].IsReplace == 1 and currentArray[nextIndex].EGeneralID == personalInfo._GeneralID then	--移动的是替补 目标是主角
		isReplace = true
	end
	if isReplace then
		return true
	else
		return false
	end
end

---松开 
function   touchEnd(x,y)
	local pointBegin=PT(x,y)
	--[[
 	for k,v in ipairs(e) do
 		pointBegin = v:locationInView(v:view())
		pointBegin=CCDirector:sharedDirector():convertToGL(pointBegin)
	end
	]]
	---如果当前没有选中的佣兵 不做操作
	if clickSprite==nil then
		return
	end
	---[[
	---clickType 1 从9宫格佣兵选中的 2 从佣兵列表中选中得
	if  pointBegin.y < magic_y then
		if clickType==1 then
			----是否放进开启的格子中
			if  checkPlaceInBox(pointBegin) then
				local position= checkPlaceInBox(pointBegin)
				----是否格子中已经有佣兵
				if checkIsReplace(position) then--替补佣兵判断
					ZyToast.show(mScene, Language.REPLACE_NOTICE, 1.5, 0.35)
				elseif openArrayTable[position].IsStations ==1 then
					pushInfoInBox(3,position)
					refreshMidLayer()
				else
				------------放进新位置
					pushInfoInBox(4,position)
					refreshMidLayer()
				end
				------------------没有放入则释放该位置
			else
				local num = 0
				for k,v in ipairs(currentArray) do
					if v.EGeneralID ~= 0 then
						num = num+1
					end
				
				end
				if num > 1 then
					pushInfoInBox(1)
					refreshMidLayer()
				end
			end
		-----从佣兵列表中选择出的佣兵  
		else
			 ---判断是否放置在box中， 
			if  checkPlaceInBox(pointBegin) then
				local position= checkPlaceInBox(pointBegin) 
				---box中是否已经有了其他佣兵
				if openArrayTable[position].IsStations ==1 then
					---如果是主角的话 是不能替换
--					if openArrayTable[position].EGeneralID~=personalInfo._GeneralID then
						pushInfoInBox(2,position)
						--如果有进行替换位置则进行刷新
						refreshMidLayer()
--					end
					--其他就能替换掉
				else
					pushInfoInBox(2,position)
					refreshMidLayer()
				end
				if isGuide then
					local _IsGuide,_GuideId, mTaskStep = GuideLayer.getIsGuide()	
					if _GuideId == 1003 and mTaskStep == 2 then
					
						GuideLayer.setIsGuide(0, 1003, 3)
						
						if GuideLayer.judgeIsGuide(5) then
							GuideLayer.setScene(mScene)	
							GuideLayer.init()
						end
						closeGuideActioin()
					end
				end
			end
		end
	end
	--]]
	---如果存在选中精灵 释放 
	if clickSprite then
		clickSprite:getParent():removeChild(clickSprite,true)
		clickSprite=nil
		clickSpriteInfo=nil
		clickType=nil
	end
	
end;

------放进9宫格 判断
function  pushInfoInBox(type,position)
--------1  释放位置 2 将佣兵放入空位置 3 两个佣兵进行位置调换 4 佣兵移动到新位置
	if type==2  then
		local index=openArrayTable[position].index
		currentArray[index].IsStations=1
		currentArray[index].Location=position
		currentArray[index].EGeneralID=clickSpriteInfo.GeneralID
		currentArray[index].GeneralHeadID=clickSpriteInfo.HeadID
		currentArray[index].GeneralQuality=clickSpriteInfo.GeneralQuality
------------------
	elseif type==1  then
		local index=clickSpriteInfo.index
		currentArray[index].IsStations=0
		currentArray[index].Location=clickSpriteInfo.position
		currentArray[index].EGeneralID=0
		currentArray[index].GeneralHeadID=0
		currentArray[index].GeneralQuality=0
--------------两个人的位置进行调换
	elseif type==3  then
		local oldIndex=clickSpriteInfo.index
		local newIndex=openArrayTable[position].index
		local table=ZyTable.th_table_dup(currentArray[newIndex])	  
		---
		currentArray[newIndex].IsStations=1
		currentArray[newIndex].EGeneralID=clickSpriteInfo.GeneralID
		currentArray[newIndex].GeneralHeadID=clickSpriteInfo.HeadID
		currentArray[newIndex].GeneralQuality=clickSpriteInfo.GeneralQuality
		---
		currentArray[oldIndex].IsStations=table.IsStations
		currentArray[oldIndex].EGeneralID=table.EGeneralID
		currentArray[oldIndex].GeneralHeadID=table.GeneralHeadID
		currentArray[oldIndex].GeneralQuality=table.GeneralQuality		
---------------原来就在位置上进行空位置调换
	elseif type==4  then
		local index=clickSpriteInfo.index
		currentArray[index].IsStations=0
		currentArray[index].Location=clickSpriteInfo.position
		currentArray[index].EGeneralID=0
		currentArray[index].GeneralHeadID=0
		currentArray[index].GeneralQuality=0
		
		local index=openArrayTable[position].index
		currentArray[index].IsStations=1
		currentArray[index].Location=position
		currentArray[index].EGeneralID=clickSpriteInfo.GeneralID
		currentArray[index].GeneralHeadID=clickSpriteInfo.HeadID
		currentArray[index].GeneralQuality=clickSpriteInfo.GeneralQuality
	end	
end;

---判断松开的时候 时候选中点是否在9宫格开启位置的上面
function  checkPlaceInBox(pt)
	if openArrayTable then
		for k , v in pairs(openArrayTable) do
			if k <= 6 then
				local sprite=v.sprite
				local Position =sprite:getPosition()
				local box=CCRectMake( Position.x,Position.y,
				sprite:getContentSize().width, sprite:getContentSize().height);
				if CCRect.containsPoint(box, pt) then
					if not isGuideMove or ( isGuideMove and m_GuideNum == k ) then
						return k
					end
				end
			end
		end
	end
	return false
end;

---刷新阵法列表
function refreshMagicList()

	if infoTable~=nil and magicList then
		if m_fucImageLable then
			m_fucImageLable:getParent():removeChild(m_fucImageLable, true)
			m_fucImageLable = nil
		end
		magicList:clear()
		local layout=CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val =0
		layout.val_y.val.pixel_val =0
		if  infoTable.EmbattleID_ then	
			for k, v in pairs( infoTable.EmbattleID_) do	
				local listItem=ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
				listItem:setOpacity(0)
				local layer = CCLayer:create()
				local fun=choiceClick
				local item,useTag = createMagicItem(v, nil, nil, k,fun)
				item:setAnchorPoint(PT(0,0))
				local pos_x = (magicList:getRowWidth()-item:getContentSize().width)*0.5
				local pos_y = (magicList:getContentSize().height-item:getContentSize().height)*0.5
				item:setPosition(PT(pos_x, pos_y))
				layer:addChild(item, 0)
				
				v.useTag = useTag
				
				listItem:addChildItem(layer, layout)
				magicList:addListItem(listItem, false) 		
				--判断是否当前启用的阵法
				if v.IsEnabled == 1 then
					mClickTag = k
					showCurrentEmbattle(v)
				end
			end				
			--阵型都没开启，默认显示第一个
			local isOpen = false
			for k, v in pairs( infoTable.EmbattleID_) do
				if v.IsEnabled == 1 then
					isOpen = true
				end
			end
			if isOpen == false and infoTable.EmbattleID_[1] then
				showCurrentEmbattle(infoTable.EmbattleID_[1])
			end
			
		end
	end
	if  not mClickTag then
		mClickTag=1
	end
	
	magicList:turnToPage (mClickTag-1)
	--]]
end;

--创建阵法名字子项
function createMagicItem(info, nScale,actionPath,tag,fun)
	-- 背景
	local menuItem = CCMenuItemImage:create(P(Image.image_zhenfa_beijing), P(Image.image_zhenfa_beijing))
	local btn = CCMenu:createWithItem(menuItem)
	btn:setAnchorPoint(PT(0,0))
	menuItem:setAnchorPoint(PT(0,0))
	if tag~= nil and tag~=-1 then
		menuItem:setTag(tag)
	end	
	--设置缩放
	if nScale == nil then
		nScale = 1
	end
	menuItem:setScale(nScale)
	btn:setContentSize(SZ(menuItem:getContentSize().width*nScale,menuItem:getContentSize().height*nScale))
    	if  not info.empty then
    	if fun then
    		menuItem:registerScriptTapHandler(function() fun(menuItem) end )
    	end
	--设置回调
	if actionPath ~= nil then
		menuItem:registerScriptTapHandler(function () actionPath(menuItem) end )
	end
    	if info == nil then
 		return btn   		
    	end
    	
	-- 缩略图
	local image = string.format("smallitem/%s.png", MagicConfig.getMagicInfo(info.EmbattleID).HeadID)
	if image ~= nil and image ~= "" then
		local imageLabel = CCSprite:create(P(image))
		imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5,menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end   
	--等级
	local level = info.EmbattleLv 
	if level ~= nil then
		local levelLabel = CCLabelTTF:create(level..Language.EMBATTLE_CLASS, FONT_NAME, FONT_SM_SIZE-2)
		levelLabel:setAnchorPoint(CCPoint(1, 0))
		levelLabel:setPosition(PT(menuItem:getContentSize().width-SX(3),SY(3)))
		menuItem:addChild(levelLabel, 0)
	end
	local name = info.EmbattleName    
	if name ~= nil then
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_SM_SIZE-2)
		nameLabel:setAnchorPoint(CCPoint(0, 0))
		nameLabel:setPosition(PT(SX(3),menuItem:getContentSize().height-nameLabel:getContentSize().height-SY(3)))
		menuItem:addChild(nameLabel,1)
	end
	if info.IsEnabled==1 then
		if m_fucImageLable then
			m_fucImageLable:getParent():removeChild(m_fucImageLable, true)
			m_fucImageLable = nil
		end
		m_fucImageLable = CCSprite:create(P(Image.Image_choicebox))
		m_fucImageLable:setAnchorPoint(CCPoint(0.5, 0.5))
		m_fucImageLable:setPosition(PT(menuItem:getContentSize().width*0.5,menuItem:getContentSize().height*0.5))
		menuItem:addChild(m_fucImageLable,0)
	end
	end
	
	--启用标志
	local label=CCLabelTTF:create(Language.EMBATTLE_MESSAGE1,FONT_NAME,FONT_SM_SIZE)
	label:setAnchorPoint(PT(0.5,0.5))
	label:setColor(ccGREEN)
	label:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.5))
	menuItem:addChild(label,1)
	
	label:setVisible(fasle)
	--启用标志
	if info.IsEnabled==1 then
		label:setVisible(true)
	end
	
	return btn
end

--显示当前启用的阵法
function showCurrentEmbattle(info)
	currentArray={}
	currentArray=ZyTable.th_table_dup(info.Location_)
	local str=info.EffectDesc
	refreshDec(str)
	refreshMidLayer()
end

-------------------------------------
----释放中间布阵信息
function releaseMidLayer()
	if midContentLayer~=nil then
		midContentLayer:getParent():removeChild(midContentLayer,true)
		midContentLayer=nil
	end
end;

--刷新中间布阵信息
function refreshMidLayer()
	if currentArray then
		releaseMidLayer()
		midContentLayer=CCLayer:create()
		mLayer:addChild(midContentLayer,1)
		openArrayTable={}
		for k,v in pairs(currentArray) do
			openArrayTable[v.Location]=v
			openArrayTable[v.Location].index=k
		end
		--出手次序索引
		local tipIndex=1
		
		local line = 2
		local row = 3
		
		openGeneralBtn={}
		---绘制9宫格
		for index=1,line*row do
			local tipPath=nil--先手图标
			local isReplace=nil--是否替补佣兵
			
			local bgImg = Image.image_zhenfa_beijing
			if openArrayTable[index] and openArrayTable[index].IsStations and openArrayTable[index].GeneralQuality and openArrayTable[index].GeneralQuality  > 0 then
				bgImg = getQualityBg(openArrayTable[index].GeneralQuality, 1)
			end
			local spriteBg=CCSprite:create(P(bgImg))
			spriteBg:setAnchorPoint(PT(0,0))


			local pos_x = matrix_x+matrix_width/row*((index-1)%row+0.5)-spriteBg:getContentSize().width*0.5
			local pos_y = matrix_y+matrix_height-matrix_height*0.3*(math.ceil(index/row))-spriteBg:getContentSize().height*0.5
			
			spriteBg:setPosition(PT(pos_x, pos_y))
			midContentLayer:addChild(spriteBg,0)
			
			local path="common/list_1016.png"
			if openArrayTable[index] then
				openArrayTable[index].sprite=spriteBg
				if openArrayTable[index].IsStations==1 then
					path=string.format("smallitem/%s.png", openArrayTable[index].GeneralHeadID)

					
--					if openArrayTable[index].IsReplace ~= 1 then
--						if tipIndex <= 5 then
--							tipPath=string.format("mainUI/list_1029_%d.png",tipIndex)
--						end
--						tipIndex=tipIndex+1
--					end
					openGeneralBtn[index]=spriteBg
				else
					path=nil
				end
				if openArrayTable[index].IsReplace == 1 then
					isReplace = 1
				end
			end
			
			--佣兵头像
			if path then
				local headSprite=CCSprite:create(P(path))
				headSprite:setAnchorPoint(PT(0,0))
				headSprite:setPosition(PT(spriteBg:getPosition().x+spriteBg:getContentSize().width*0.5-headSprite:getContentSize().width*0.5,spriteBg:getPosition().y+spriteBg:getContentSize().height*0.5-headSprite:getContentSize().height*0.5))
				midContentLayer:addChild(headSprite,0)
			end
			
--			--先手图标
--			if tipPath then
--				local tipSprite=CCSprite:create(P(tipPath))
--				tipSprite:setAnchorPoint(PT(1,1))
--				tipSprite:setPosition(PT(spriteBg:getPosition().x+spriteBg:getContentSize().width/2,
--						spriteBg:getPosition().y+spriteBg:getContentSize().height/2))
--				midContentLayer:addChild(tipSprite,0)
--			end
				
		end
	end
	refreshMenberList()
	
			
	if _useBtn then
		if  infoTable.EmbattleID_[mClickTag].IsEnabled ==1 then
			_useBtn:setEnabled(false)
		elseif  infoTable.EmbattleID_[mClickTag].IsEnabled == 0 then
			_useBtn:setEnabled(true)
		end
	end	
		
end;

-----刷新左边的佣兵列表
function refreshMenberList()
	mercenariesBtn = {}
	local arraryNum = 0
	if infoTable.GeneralID_~=nil and heroList then
		heroList:clear()
		for k,v in ipairs(infoTable.GeneralID_) do
			-----检查是否在布阵中 是否出战

				local listItem=ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
				listItem:setOpacity(0)
				local layer = CCLayer:create()
				local image=string.format("smallitem/%s.png", v.HeadID)
				local isSeason = nil
				if checkIsInArrary(v.GeneralID) ==true then
					isSeason = true
					arraryNum = arraryNum+1
				end
				local item = createMemberItem(image, nil, nil, k,isSeason, v.GeneralQuality)
				item:setAnchorPoint(PT(0,0))
				local pos_x = (heroList:getContentSize().width-item:getContentSize().width)*0.5
				local pos_y = (heroList:getRowHeight()-item:getContentSize().height)*0.5
				item:setPosition(PT(pos_x, pos_y))
				layer:addChild(item, 0)
							
				mercenariesBtn[k] = item
				local layout=CxLayout()
				layout.val_x.t = ABS_WITH_PIXEL
				layout.val_y.t = ABS_WITH_PIXEL
				layout.val_x.val.pixel_val =0
				layout.val_y.val.pixel_val =0
				listItem:addChildItem(layer, layout)
				heroList:addListItem(listItem, false) 		              
	
		end
	end
	
	local _IsGuide,_GuideId, mTaskStep = GuideLayer.getIsGuide()
	if _GuideId == 1003 and 	mTaskStep == 2 then
		isGuide = true
		creatGuideAction()
	end
	
end

--佣兵列表中的单个佣兵
function createMemberItem(image, nScale,actionPath,tag, isSeason, quality)

	-- 背景
	local bgImg = getQualityBg(quality, 1)
	local menuItem = CCMenuItemImage:create(P(bgImg), P(bgImg))
	local btn = CCMenu:createWithItem(menuItem)
	btn:setAnchorPoint(PT(0,0))
	menuItem:setAnchorPoint(PT(0,0))
	if tag~= nil and tag~=-1 then
		menuItem:setTag(tag)
	end
	--设置缩放
	if nScale == nil then
		nScale = 1
	end
	menuItem:setScale(nScale)
    
	--设置回调
	if actionPath ~= nil then
		menuItem:registerScriptHandler(actionPath)
	end
    
	-- 缩略图
	if image ~= nil and image ~= "" then
		local imageLabel = CCSprite:create(P(image))
		imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5,menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end
    
	--等级
	if name ~= nil then
		local nameLabel22 = CCLabelTTF:create(level, FONT_NAME, FONT_SM_SIZE-2)
		nameLabel22:setAnchorPoint(CCPoint(1, 0))
		nameLabel22:setPosition(PT(menuItem:getContentSize().width-SX(3),SY(3)))
		menuItem:addChild(nameLabel22,1)
		
		labelList[tag]=nameLabel22
	end
    
	if name ~= nil then
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_SM_SIZE-2)
		nameLabel:setAnchorPoint(CCPoint(0.5, 0))
		nameLabel:setPosition(PT(menuItem:getContentSize().width/2,-nameLabel:getContentSize().height-SY(1)))
		menuItem:addChild(nameLabel,1)
	end
	
	--出战图标
	if isSeason == true then
		local seasonImg = CCSprite:create(P("common/list_1071.png"))
		seasonImg:setAnchorPoint(CCPoint(1, 1))
		seasonImg:setPosition(PT(menuItem:getContentSize().width,menuItem:getContentSize().height))
		menuItem:addChild(seasonImg,0)		
	end
    

	btn:setContentSize(SZ(menuItem:getContentSize().width*nScale,menuItem:getContentSize().height*nScale))
	return btn
end

-----检查是否在布阵中 是否出战
function  checkIsInArrary(id)
	if openArrayTable then
		for k, v in pairs(openArrayTable) do
			if v.IsStations==1 and v.EGeneralID==id then
				return true
			end
		end
	end
	return false
end;




-------阵法选择点击
function  choiceClick(pNode)
	local messageIndex = pNode

	if messageIndex~=mClickTag then
		----当前选中的阵法列表id 用于启用阵法
		mClickTag=messageIndex
		----当前选中的阵法  编辑后用于保存阵法数据
		currentArray={}
		local info= infoTable.EmbattleID_[mClickTag].Location_
		currentArray=ZyTable.th_table_dup(info)	  
		local str=infoTable.EmbattleID_[mClickTag].EffectDesc


		if m_fucImageLable then
			m_fucImageLable:getParent():removeChild(m_fucImageLable, true)
			m_fucImageLable = nil
		end
		m_fucImageLable = CCSprite:create(P(Image.Image_choicebox))
		m_fucImageLable:setAnchorPoint(CCPoint(0.5, 0.5))
		m_fucImageLable:setPosition(PT(pNode:getContentSize().width*0.5,pNode:getContentSize().height*0.5))
		pNode:addChild(m_fucImageLable,0)

		

		--阵法描述文字刷新
		refreshDec(str)	
		--刷新阵法内容
		refreshMidLayer()
	end
	
end;

----刷新阵法描述
function refreshDec(str)
	if magicDes then
		magicDes:setString(str)
	end
end;   
 
--保存阵法
function  saveArrayAction()
	if currentArray ~=nil and not isGuideMove then
		local MagicID=infoTable.EmbattleID_[mClickTag].EmbattleID
		local GeneralID=""
		local Location=""
		for k, v in pairs(currentArray) do
			if v.EGeneralID   then
				GeneralID=GeneralID .. v.EGeneralID
			else
				GeneralID=GeneralID .. 0
			end
			GeneralID=GeneralID..","
			Location=Location .. v.Location
			Location=Location..","
		end
		
		local oldLocation = ZyTable.th_table_dup(infoTable.EmbattleID_[mClickTag].Location_)
		oldLocation = ZyTable.orderBy(oldLocation, "EGeneralID" );
		
		local nowLocation = ZyTable.th_table_dup(currentArray)
		nowLocation = ZyTable.orderBy(nowLocation, "EGeneralID" );
		
		local ops = 0
		for k,v in ipairs(oldLocation) do
			if v.EGeneralID ~= nowLocation[k].EGeneralID then
			ops = 1
			end
		end
		
		
		--1902_魔法阵设置接口（ID=1902）
		actionLayer.Action1902(mScene, nil, MagicID, ops, GeneralID, Location)
	end
end;

--启用阵法
function useArrayAction()
	if mClickTag~=nil then
		local MagicID=infoTable.EmbattleID_[mClickTag].EmbattleID
		actionLayer.Action1903(mScene,nil,MagicID)
	end
end
------------------------------------------------
--发送请求
function sendAction(actionId)
	if actionId == 1901 then----1901_魔法阵列表接口（ID=1901）
		actionLayer.Action1901(mScene,nil)	
	end

end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1901 then----1901_魔法阵列表接口（ID=1901）
		local serverInfo=actionLayer._1901Callback(pScutScene, lpExternalData)
		if serverInfo then
			infoTable=serverInfo
			refreshMagicList()
				
		end
	elseif actionId == 1902 then--1902_魔法阵设置接口（ID=1902）
		local serverInfo=actionLayer._1903Callback(pScutScene, lpExternalData)
		if serverInfo~=nil then
			ZyToast.show(mScene,Language.EMBATTLE_SAVESUC)
			actionLayer.Action1901(mScene,nil)
			MainMenuLayer.refreshWin()
		end
	elseif actionId == 1903 then
		local serverInfo=actionLayer._1903Callback(pScutScene, lpExternalData)
		if serverInfo~=nil then
			ZyToast.show(mScene,Language.EMBATTLE_OPENSUC)
			MainMenuLayer.refreshWin()
			actionLayer.Action1901(mScene,nil)
		end	
	else	
		commonCallback.networkCallback(pScutScene, lpExternalData)
	end
end



function closeGuideActioin()
	if ArowGuideLayer then
		ArowGuideLayer:getParent():removeChild(ArowGuideLayer, true)
		ArowGuideLayer = nil
	end
	if m_guideLayer then
		m_guideLayer:getParent():removeChild(m_guideLayer, true)
		m_guideLayer = nil
	end

	isGuideMove = false
	magicList:SetSilence(false)
	heroList:SetSilence(false)
	MainMenuLayer.endList:SetSilence(false)
	local btnTable = MainMenuLayer.getBtnTable()
	for k,v in ipairs(btnTable.menu) do
		v:setEnabled(true)
	end
	
end;

function creatGuideAction()
	
	if m_guideLayer then
		return
	end
	
	isGuideMove = true
	local btnTable = EmbattleScene.getBtnTable()
	local item = nil
	for k,v in pairs(btnTable.openArrayTable) do
		if v.IsStations==0 then
			m_GuideNum = k
		end
	end

	local layer = CCLayer:create()
	mScene:addChild(layer, 5)
	m_guideLayer = layer

	local bgImg = CCSprite:create(P("common/transparentBg.png"))
	bgImg:setScaleX(pWinSize.width/bgImg:getContentSize().width)
	bgImg:setScaleY(pWinSize.height/bgImg:getContentSize().height)
	bgImg:setAnchorPoint(PT(0,0))
	bgImg:setPosition(PT(0,0))
	layer:addChild(bgImg, 0)
	
	
	local item = mercenariesBtn[#mercenariesBtn]	
	local position = item:getParent():convertToWorldSpace(item:getPosition())


	local heroDetail = infoTable.GeneralID_[#infoTable.GeneralID_]
	local qualityType = heroDetail.GeneralQuality
	local picBg = getQualityBg(qualityType, 1)
	local image = string.format("smallitem/%s.png", heroDetail.HeadID)
	local imageBg = CCSprite:create(P(picBg))
	imageBg:setAnchorPoint(CCPoint(0, 0))
	imageBg:setPosition(position)
	layer:addChild(imageBg,0)

	local imageLabel = CCSprite:create(P(image))
	imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
	imageLabel:setPosition(PT(imageBg:getContentSize().width*0.5,imageBg:getContentSize().height*0.5))
	imageBg:addChild(imageLabel,0)
	
	
	createArowGuide( item)
		
	magicList:SetSilence(true)
	heroList:SetSilence(true)
	MainMenuLayer.endList:SetSilence(true)
	
	
	local btnTable = MainMenuLayer.getBtnTable()
	for k,v in ipairs(btnTable.menu) do
		v:setEnabled(false)
	end
	
end;

--箭头
function createArowGuide(item)
	local layer = CCLayer:create()
	m_guideLayer:addChild(layer, 0)
	
	ArowGuideLayer = layer


	local boxSize = item:getContentSize()
	local position = item:getParent():convertToWorldSpace(item:getPosition())
	

	--箭头
	local crowSprite=CCSprite:createWithTexture(IMAGE("common/list_1170_1.png"))
	crowSprite:setAnchorPoint(PT(0.5,0))
	layer:addChild(crowSprite,0)
	

	--光圈
	local lightBtn = CCSprite:create(P("common/list_1171.png"))
	lightBtn:setAnchorPoint(PT(0,0))
	lightBtn:setPosition(PT(position.x+boxSize.width*0.5-lightBtn:getContentSize().width*0.5, position.y+boxSize.height*0.5-lightBtn:getContentSize().height*0.5))
	layer:addChild(lightBtn, 0)		


	local olditem = mercenariesBtn[#mercenariesBtn]	
	oldPos = olditem:getParent():convertToWorldSpace(olditem:getPosition())
	oldPos = PT(oldPos.x+olditem:getContentSize().width*0.5, oldPos.y+olditem:getContentSize().height*0.5)

	local newItem = nil
	for k,v in pairs(openArrayTable) do
		if v.IsStations==0 then
			newItem = v.sprite
		end
	end
	movePos = newItem:getParent():convertToWorldSpace(newItem:getPosition())
	movePos = PT(movePos.x+newItem:getContentSize().width*0.5, movePos.y+newItem:getContentSize().height*0.5)
	
	
	m_crowSprite = crowSprite

	setPos()
	local action1 = CCCallFuncN:create(EmbattleScene.setPos)
	local action2 = CCMoveTo:create(1.5,movePos )
	local action3 = CCSequence:createWithTwoActions(action1, action2)
	crowSprite:runAction(CCRepeatForever:create(action3))
end;

function setPos()
	m_crowSprite:setPosition(oldPos)
end


