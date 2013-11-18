------------------------------------------------------------------
-- InheritScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.25
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("InheritScene", package.seeall)


mScene = nil 		-- 场景

function setGeneralId(generalId)
	mGeneralId = generalId
end;


-- 退出场景
function closeAction()
	if mFatherType == 1 then--1 佣兵阵营   2佣兵列表
		HeroScene.pushScene(mGeneralId)
	elseif mFatherType == 2 then
		RoleBagScene.pushScene()
	end
end


function close()
	closeChoiceLayer()
	if showLayer then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil	
	end
	if mLayer then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end
end;

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	inheritType = 1
end

-- 释放资源
function releaseResource()
	showLayer=nil
	mLayer=nil
	mGeneralId=nil
	heroLayer=nil
	choiceLayer=nil
end

-- 创建场景
function init(fatherType)
	mFatherType = fatherType--1 佣兵阵营   2佣兵列表

	initResource()

	local scene  = ScutScene:new()
	scene:registerCallback(networkCallback)
	mScene = scene.root 
	mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	SlideInLReplaceScene(mScene,1)

	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)
	
	--创建背景
	--大背景
	local bgSprite=CCSprite:create(P("common/list_1015.png"))
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	mLayer:addChild(bgSprite,0)
	
	--中间背景
--	local tabStr={Language.INHERIT_TITLE}
	local bgLayer = creatBg()
	mLayer:addChild(bgLayer, 0)	



	-- 此处添加场景初始内容
	MainMenuLayer.init(3, mScene)


	
	--showContent()
	btnTable = {Language.IDS_BACK,  Language.ROLE_INHERIT}
	for i=1,2 do
		local button = ZyButton:new(Image.image_button, nil, nil, btnTable[i], FONT_NAME,FONT_SM_SIZE)
		button:setAnchorPoint(PT(0,0))
		local pos_x = pWinSize.width*0.3+pWinSize.width*0.4*(i-1)-button:getContentSize().width*0.5
		local pos_y = pWinSize.height*0.245
		button:setPosition(PT(pos_x, pos_y))	
		button:registerScriptHandler(key_button)
--		button:setColorNormal(ZyColor:colorYellow())
		button:setTag(i)
		button:addto(mLayer, 0)		
	end
	
	
	sendAction(1446)
	--PubLayer.init(mScene, mLayer)
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


--创建背景
function creatBg(tabStr)
	local layer = CCLayer:create()
	

	
	local boxSize = SZ(pWinSize.width, pWinSize.height*0.78)
	local pos_x = 0
	local pos_y = pWinSize.height*0.145
	
	local bgSprite=CCSprite:create(P("common/list_1043.png"))
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setPosition(PT(pos_x,pos_y))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	layer:addChild(bgSprite,0)

	layer:setContentSize(boxSize)

	--屏蔽按钮ceng
	local unTouchBtn =  ZyButton:new(Image.image_transparent)
	unTouchBtn:setScaleX(boxSize.width/unTouchBtn:getContentSize().width)
	unTouchBtn:setScaleY(boxSize.height/unTouchBtn:getContentSize().height)
	unTouchBtn:setAnchorPoint(PT(0,0))
	unTouchBtn:setPosition(PT(0,bgSprite:getPosition().y))
	unTouchBtn:addto(layer, 0)
	
	if tabStr then
		createTabBar(tabStr,layer)
	end
	
	return layer
end;

--Tab界面切换
function createTabBar(tabName, layer)
	local tabBar = ZyTabBar:new(Image.image_top_button_0, Image.image_top_button_1, tabName, FONT_NAME, FONT_SM_SIZE, 4, Image.image_LeftButtonNorPath, Image.image_rightButtonNorPath);
	tabBar:setCallbackFun(topTabBarAction); -----点击启动函数
	tabBar:addto(layer,0) ------添加
	tabBar:setColor(ZyColor:colorYellow())  ---设置颜色
	tabBar:setPosition(PT(SX(25),pWinSize.height*0.855))  -----设置坐标
end

--内容显示区
function showContent()
	if showLayer then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil	
	end
	
	show_x = 0
	show_y = pWinSize.height*0.17
	show_width = pWinSize.width
	hero_height = pWinSize.height*0.4

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(show_x, show_y))
	mLayer:addChild(layer, 0)	
	showLayer = layer

	--说明
	local contentStr=string.format("<label>%s</label>", Language.INHERIT_NOTICE)
	local noticLabel=ZyMultiLabel:new(contentStr,pWinSize.width*0.8,FONT_NAME,FONT_SM_SIZE);
	noticLabel:setAnchorPoint(PT(0,0))
	noticLabel:setPosition(PT(pWinSize.width*0.1, pWinSize.height*0.64-noticLabel:getContentSize().height))
	noticLabel:addto(layer,0)
	
	local isHave = false
	for i=1,2 do--1：传承人 2：被传承人
		local manInfo = nil
		for m,n in pairs(inheritInfo.RecordTabel) do
			if n.HeritageType == i then
				manInfo = n
				if i ==1 then
					isHave = true
				end
			end
		
		end
		
		local image,name,level,quality = nil
		if manInfo ~= nil then
			image = string.format("battleHead/%s.png", manInfo.Head)
			name = manInfo.GeneralName
			level = manInfo.GeneralLv
			quality = manInfo.GeneralQuality 
		end
		
		local memberCallBack = nil
		if i == 1 then
			memberCallBack = key_to_choice
		end

		local item = creatItem(image, name, quality, level, i, memberCallBack)
		item:setAnchorPoint(PT(0,0))
		
		local pos_x = pWinSize.width*0.15+(i-1)*pWinSize.width*0.5
		local pos_y = pWinSize.height*0.38
		item:setPosition(PT(pos_x, pos_y))
		layer:addChild(item, 0)
	end
	
	--箭头图片
	local bgSprite=CCSprite:create(P("common/list_1048.png"))
	local pos_x = pWinSize.width*0.525
	local pos_y = pWinSize.height*0.485
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pos_x, pos_y))
	layer:addChild(bgSprite,0)	
	
	
	local start_X = pWinSize.width*0.15
	local start_Y = pWinSize.height*0.38
	--请选择佣兵 文字
	local nameLabel = CCLabelTTF:create(Language.INHERIT_CHOICE, FONT_NAME, FONT_SM_SIZE)
	nameLabel:setAnchorPoint(PT(0, 0))
	nameLabel:setPosition(PT(start_X, start_Y-nameLabel:getContentSize().height*1.5))
	layer:addChild(nameLabel, 0)
	
	--佣兵传承之后会消失 文字
	local nameLabel = CCLabelTTF:create(Language.INHERIT_DISAPPEAR, FONT_NAME, FONT_SM_SIZE)
	nameLabel:setAnchorPoint(PT(0, 0))
	nameLabel:setPosition(PT(start_X, start_Y-nameLabel:getContentSize().height*3))
	layer:addChild(nameLabel, 0)	
	
	
	--传承类型
	inheritType_buttonTable = {}
	for i=1,2 do	
		local choiceBtn=ZyButton:new(Image.image_button_hook_0,Image.image_button_hook_1)
		choiceBtn:setAnchorPoint(PT(0,0))
		local posX = pWinSize.width*0.1+pWinSize.width*0.4*(i-1)
		local posY = pWinSize.height*0.255
		choiceBtn:setPosition(PT(posX, posY))
		choiceBtn:addto(layer, 0)
		choiceBtn:setTag(i)
		choiceBtn:registerScriptHandler(typeChoiceAction)
		if i == inheritType then
			choiceBtn:selected()
		end
		
		
		local str = ""
		if inheritInfo.RecordTabel2[i].OpsType == 1 then
			str = Language.INHERIT_NORMAL
		elseif inheritInfo.RecordTabel2[i].OpsType == 2 then
			str = Language.INHERIT_TOP.."("
			if not isShowVip() then
				str = str.."VIP"..inheritInfo.RecordTabel2[i].VipLv ..","
			end
			str = str..inheritInfo.RecordTabel2[i].UseGold ..Language.IDS_JINGSHI..")"
		end
		
		local label = CCLabelTTF:create(str, FONT_NAME, FONT_SM_SIZE)
		label:setAnchorPoint(PT(0,0.5))
		label:setPosition(PT(choiceBtn:getPosition().x+choiceBtn:getContentSize().width*1.2, 
						choiceBtn:getPosition().y+choiceBtn:getContentSize().height*0.5))
		layer:addChild(label, 0)	
		inheritType_buttonTable[i]=choiceBtn
		
		if not isShowVip() and inheritInfo.RecordTabel2[i].OpsType == 2 and PersonalInfo.getPersonalInfo()._VipLv < inheritInfo.RecordTabel2[i].VipLv  then
			label:setVisible(false)
			choiceBtn:setVisible(false)
		end
	end

	--传承结果
	local nameLabel = CCLabelTTF:create(Language.INHERIT_RESULT..":", FONT_NAME, FONT_SM_SIZE)
	nameLabel:setAnchorPoint(PT(0, 0))
	nameLabel:setPosition(PT(pWinSize.width*0.1, pWinSize.height*0.21))
	layer:addChild(nameLabel, 0)	
	
	local str = ""
	if inheritInfo.HeritageName and inheritInfo.HeritageLv and  inheritInfo.DisGeneralName then
		str = string.format("<label>%s\n</label>", inheritInfo.HeritageName..Language.INHERIT_UP..inheritInfo.HeritageLv..Language.IDS_LEV)
		str = str..string.format("<label>%s\n</label>", inheritInfo.DisGeneralName..Language.INHERIT_ISAWAY)
	elseif not isHave then
		str = string.format("<label>%s</label>", Language.INHERIT_CHOICE)
	end
	local noticeLabel = ZyMultiLabel:new(str, pWinSize.width*0.8, FONT_NAME, FONT_SM_SIZE, nil, nil)
	noticeLabel:setPosition(PT(pWinSize.width*0.1, nameLabel:getPosition().y-pWinSize.height*0.01-noticeLabel:getContentSize().height))
	noticeLabel:addto(layer, 0)
		

	--]]
end

--创建单个玩家
function creatItem(image, name, quality, level, type, memberCallBack)
	local menuItem = CCMenuItemImage:create(P("common/list_1032.png"), P("common/list_1032.png"))
	local btn = CCMenu:createWithItem(menuItem)
	
	menuItem:setAnchorPoint(PT(0,0))
	if memberCallBack then 
	    menuItem:registerScriptTapHandler(memberCallBack)
    end 
	if tag then
		menuItem:setTag(tag)
	end
	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))

	-- 商品图片
	if image then
		local imageLabel = CCMenuItemImage:create(P(image),P(image))
		if imageLabel == nil then
			 return btn 
		end
		imageLabel:setAnchorPoint(CCPoint(0.5, 0))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.2))
		menuItem:addChild(imageLabel,0)
	end


	--名称
	if name then
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0.5, 0))
		nameLabel:setPosition(PT(menuItem:getContentSize().width*0.5, nameLabel:getContentSize().height*0.5))
--		menuItem:addChild(nameLabel, 0)
	end
	
	if type == 2 and name then
			--昵称
		local nameLabel=CCLabelTTF:create(name, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		local rowH=nameLabel:getContentSize().height*1.1
		nameLabel:setPosition(PT(0, -rowH))
		menuItem:addChild(nameLabel, 0)
		--------品质
		--1	白色2	绿色3	蓝色4	紫色
		local str=Language.EQUIP_PIN .. ":"
		if quality and genrealQuality[quality] then
			str=str .. genrealQuality[quality]
		end
		local qulityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
		qulityLabel:setAnchorPoint(PT(0,0))
		qulityLabel:setPosition(PT(nameLabel:getPosition().x,
								nameLabel:getPosition().y-rowH))
		menuItem:addChild(qulityLabel, 0)
		
		---等级
		local levelStr=Language.IDS_LEVEL .. ":" .. (level or 0)
		local levelLabel=CCLabelTTF:create(levelStr,FONT_NAME,FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(qulityLabel:getPosition().x,
								qulityLabel:getPosition().y-rowH	))
		menuItem:addChild(levelLabel, 0)	
	
	end
	

	return  btn
end;

--传承类型选择
function typeChoiceAction(pNode)
	local tag = pNode:getTag()
	local currentType = inheritType
	
	if inheritInfo.RecordTabel2[tag].VipLv <= PersonalInfo.getPersonalInfo()._VipLv then
		inheritType = tag
	end

	for k,v in ipairs(inheritType_buttonTable) do
		if k == inheritType then
			v:selected()
		else
			v:unselected()
		end
	end
	
	if currentType ~= inheritType then
		refreshWin()
	end
	
	
end;
-------------------------------------------------------
--点击去选择传承佣兵
function key_to_choice()
	sendAction(1447)
end;

---------------------------------

--关闭   被传承佣兵选择界面
function closeChoiceLayer()
	if heroLayer ~= nil then
		heroLayer:getParent():removeChild(heroLayer, true)
		heroLayer = nil
	end
	if choiceLayer ~= nil then
		choiceLayer:getParent():removeChild(choiceLayer, true)
		choiceLayer = nil
	end
end

--被传承佣兵选择界面
function choiceContent()
	if choiceLayer ~= nil then
		choiceLayer:getParent():removeChild(choiceLayer, true)
		choiceLayer = nil
	end

	local layer = CCLayer:create()
	mScene:addChild(layer, 3)
	
	choiceLayer = layer
	
	--创建背景
	local tabStr={Language.BAG_HERO}
	local bgLayer = creatBg(tabStr)
	choiceLayer:addChild(bgLayer, 0)	


	
	local tabName = {Language.ROLE_EQUIPCHOICE}

	
	--确认按钮	
	local button = ZyButton:new(Image.image_button, nil, nil, Language.IDS_SURE, FONT_NAME, FONT_SM_SIZE)
	button:setAnchorPoint(PT(0,0))
	button:setPosition(PT(pWinSize.width*0.8, pWinSize.height*0.855))
	button:addto(choiceLayer, 0)
	button:registerScriptHandler(key_sure)	
	
	creatList()
end

--创建选择列表
function creatList()
	if heroLayer ~= nil then
		heroLayer:getParent():removeChild(heroLayer, true)
		heroLayer = nil
	end

	local layer = CCLayer:create()
	choiceLayer:addChild(layer, 0)
	
	heroLayer = layer
	
	
	local list_width = pWinSize.width*0.9
	local list_height = pWinSize.height*0.6
	local list_x = pWinSize.width*0.5-list_width*0.5
	local list_y = pWinSize.height*0.22
	local line = 4
	
	local listSize = SZ(list_width, list_height)
	list = ScutCxList:node(listSize.height/line, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(list_x, list_y))
	layer:addChild(list,0)
	list:setTouchEnabled(true)
	skillList = list

--	local ITEM = list
--	
--	local itemBg = CCSprite:create(P("common/list_1020.9.png"))
--	itemBg:setAnchorPoint(PT(0,0))
--	itemBg:setPosition(PT(ITEM:getPosition().x,ITEM:getPosition().y))
--	itemBg:setScaleX(ITEM:getContentSize().width/itemBg:getContentSize().width)
--	itemBg:setScaleY(ITEM:getContentSize().height/itemBg:getContentSize().height)
--	layer:addChild(itemBg, -1)
	
	
	local label = CCLabelTTF:create("", FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(PT(0.5,0))
	label:setPosition(PT(list_x+list_width*0.5, list_y+list_height*0.5))
	layer:addChild(label, 0)
	noticeLabel = label
	
	choiceTable = {}
--		local listItem=ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
--		listItem:setOpacity(50)
--	list:addListItem(listItem, false) 
end

--显示可选项
function showList()
	local list = skillList
	list:clear()
	
	local infoTabel = choiceInfo.RecordTabel

	if infoTabel and #infoTabel == 0 then
		noticeLabel:setString(Language.INHERIT_NOCHOICE)
		return
	else
		noticeLabel:setString("")
	end
	
	buttonTable = {}
	for k,v in ipairs(infoTabel) do
		local listItem=ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(0)	
		
		
		
		local item,choiceBtn = creatManItem(v,k)
		v.choiceBtn = choiceBtn
		
--
		local layout=CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val =0
		layout.val_y.val.pixel_val =0

		listItem:addChildItem(item, layout)
		list:addListItem(listItem, false) 
	end
end;


--创建佣兵信息
function creatManItem(info, tag)
	local layer = CCLayer:create()
	
	local layerSize = SZ(list:getContentSize().width, list:getRowHeight())
	layer:setContentSize(layerSize)

	--背景
	local itemBox=SZ((layerSize.width-SX(4)),(layerSize.height-SY(4)))	
	local itemBg = CCSprite:create(P("common/list_1020.9.png"))
	itemBg:setScaleX(itemBox.width/itemBg:getContentSize().width)
	itemBg:setScaleY(itemBox.height/itemBg:getContentSize().height)
	itemBg:setAnchorPoint(PT(0,0))
	itemBg:setPosition(PT(SX(2), SY(2)))	
	layer:addChild(itemBg, 0)
	
	
	--	--佣兵头像
	local image = nil
	if info.HeadID then
 		image = string.format("smallitem/%s.png", info.HeadID)
	end
	local headItem = creatHeadItem(image, tag, nil)
	headItem:setAnchorPoint(PT(0,0))
	headItem:setPosition(PT(layerSize.width*0.05, layerSize.height*0.5-headItem:getContentSize().height*0.5))
	layer:addChild(headItem, 0)


	local startX = layerSize.width*0.25

	--佣兵姓名
	local nameStr = info.GeneralName 
	if nameStr then
		local nameLabel = CCLabelTTF:create(nameStr, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(startX, layerSize.height*0.6))
		layer:addChild(nameLabel, 0)
	end
	
	--品质
	if info.CurLevel and genrealQuality[info.CurLevel] then
		local qualityStr = Language.BAG_QUALITY..":"..genrealQuality[info.CurLevel]
		local qualityLabel = CCLabelTTF:create(qualityStr, FONT_NAME, FONT_SM_SIZE)
		qualityLabel:setAnchorPoint(PT(0,0))
		qualityLabel:setPosition(PT(startX, layerSize.height*0.4))
		layer:addChild(qualityLabel, 0)
	end
	
	--等级
	local level = info.GeneralLv  
	if level then
		local levelStr = Language.ROLE_LEVEL..":"..level
		local levelLabel = CCLabelTTF:create(levelStr, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(layerSize.width*0.45, layerSize.height*0.4))
		layer:addChild(levelLabel, 0)	
	end

	--属性
	if info.AbilityList and #info.AbilityList > 0 then
		local AbilityStr = ""
		for k,v in ipairs(info.AbilityList) do
			AbilityStr = AbilityStr..Language.BAG_TYPE_[v.AbilityType].."+"..v.BaseNum.." "
		end
		--Language.ROLE_LEVEL..":"..info.CurLevel
		local AbilityLabel = CCLabelTTF:create(AbilityStr, FONT_NAME, FONT_SM_SIZE)
		AbilityLabel:setAnchorPoint(PT(0,0))
		AbilityLabel:setPosition(PT(startX, layerSize.height*0.2))
		layer:addChild(AbilityLabel, 0)		
	end

	--选择按钮	
	local button = ZyButton:new(Image.image_button_hook_0, Image.image_button_hook_1, nil)
	button:setAnchorPoint(PT(0,0))
	button:setPosition(PT(layerSize.width*0.85, layerSize.height*0.5-button:getContentSize().height*0.5))
	button:addto(layer, 0)
	button:setTag(tag)
	
	local actionPath = choiceInheritHero
	
	local actionBtn=UIHelper.createActionRect(itemBox,InheritScene.choiceInheritHero,tag)
	actionBtn:setPosition(itemBg:getPosition())
	layer:addChild(actionBtn, 0)
	return layer,button	
end;

--创建头像
function creatHeadItem(image, tag, actionPath)

	-- 背景
	local menuItem = CCMenuItemImage:create(P(Image.image_zhenfa_beijing), P(Image.image_zhenfa_beijing))
	local btn = CCMenu:createWithItem(menuItem)
	btn:setAnchorPoint(PT(0,0))
	menuItem:setAnchorPoint(PT(0,0))
	if tag~= nil and tag~=-1 then
		menuItem:setTag(tag)
	end

	btn:setContentSize(SZ(menuItem:getContentSize().width,menuItem:getContentSize().height))
	
	

    
	-- 缩略图
	if image ~= nil and image ~= "" then
		local imageLabel = CCSprite:create(P(image))
		imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5,menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end	

	return btn
end

--选择传承佣兵
function choiceInheritHero(pNode)
	local tag = pNode:getTag()
	for k,v in ipairs(choiceInfo.RecordTabel) do
		if k == tag and not v.choiceBtn._isSelected then
 			v.choiceBtn:selected()
		else
			v.choiceBtn:unselected()
		end
	end

end;


--选择界面确认按钮
function key_sure()
	local generalId = nil
	for k,v in ipairs(choiceInfo.RecordTabel) do
		if v.choiceBtn._isSelected then
			generalId = v.GeneralID 
		end
	end
	
	if generalId then
		sendAction(1448, generalId)
	--	closeChoiceLayer()
	else
		closeChoiceLayer()
	end
end;



---------------------------

--返回--传承 按钮响应
function key_button(pNode)
	local tag = pNode:getTag()
	if tag == 1 then--返回
		closeAction()
	elseif tag == 2 then--传承
		sendAction(1449)
	end
end;


--刷新屏幕
function refreshWin()
	sendAction(1446)
end;



function sendAction(actionId, data)
	if actionId == 1446 then
		actionLayer.Action1446(mScene, nil, inheritType)	
	elseif actionId == 1447 then
		actionLayer.Action1447(mScene, nil, 1)
	elseif actionId == 1448 then
		local userData = {}
		userData.generalId = data
		userData.inheritType = 1
		actionLayer.Action1448(mScene, nil, data, 1, userData)--1：传承人 2：被传承人
	elseif actionId == 1449 then
		actionLayer.Action1449(mScene, nil, inheritType)--1：普通传承2：至尊传承3：确认至尊传承 
	end
end;


-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1446 then--1446_传承界面接口（ID=1446）
		local serverInfo = actionLayer._1446Callback(pScutScene, lpExternalData)
		if serverInfo then
			inheritInfo = serverInfo
			showContent()
		end
	elseif actionId == 1447 then--1447_传承与被传承人列表接口（ID=1447）1446_传承界面接口（ID=1446）
		local serverInfo = actionLayer._1447Callback(pScutScene, lpExternalData)
		if serverInfo then
			choiceInfo = serverInfo

			choiceContent()
			showList()
		end
	elseif actionId == 1448 then-- 1448_传承人与被传承人选择接口
		local serverInfo = actionLayer._1448Callback(pScutScene, lpExternalData)
		if serverInfo then
			refreshWin()
			closeChoiceLayer()
		end		
	elseif actionId == 1449 then--_传承接口
		if ZyReader:getResult() == 1 or ZyReader:getResult() == 3 then
			refreshWin()
			MainMenuLayer.refreshWin()
		elseif ZyReader:getResult() == 2 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene, nil, ZyReader:readErrorMsg(), Language.IDS_SURE, Language.IDS_CANCEL,askIsInherit);
		else          
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)				
		end
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)	
	end

end

function askIsInherit(index, contentStr, tag)
	if index == 1 then
		actionLayer.Action1449(mScene, nil, 3)
	end
end