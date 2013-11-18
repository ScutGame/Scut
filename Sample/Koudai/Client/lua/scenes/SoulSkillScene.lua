------------------------------------------------------------------
-- SoulSkillScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 魂技升级界面
------------------------------------------------------------------

module("SoulSkillScene", package.seeall)


mScene = nil 		-- 场景


function setAbilityID(abilityID, userItemID, scene, fatherType)
	mAbilityID = abilityID
	mUserItemID = userItemID
	mScene = scene
	mFatherType = fatherType
end

-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end

-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	train_x = 0
	train_y = pWinSize.height*0.145
	
	choiceTable = {}
end

-- 释放资源

function closeAllLayer()
	if costLayer ~= nil then
		costLayer:getParent():removeChild(costLayer, true)
		costLayer = nil
	end
	if trainLayer ~= nil then
		trainLayer:getParent():removeChild(trainLayer, true)
		trainLayer = nil
	end	
	if mLayer ~= nil then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end	
	choiceTable = nil
end;

function releaseResource()
	costLayer = nil
	trainLayer = nil
	mLayer = nil
	choiceTable = nil
	m_currentLevel = nil
	heroLayer = nil
	choiceLayer = nil
end

-- 创建场景
function init()

	 initResource()
 
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)


	--创建背景
	local midSprite=CCSprite:create(P("common/list_1040.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.68)
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)	
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	mLayer:addChild(midSprite,0)

	--屏蔽按钮ceng
	local unTouchBtn =  ZyButton:new(Image.image_transparent)
	unTouchBtn:setScaleX(boxSize.width/unTouchBtn:getContentSize().width)
	unTouchBtn:setScaleY(boxSize.height/unTouchBtn:getContentSize().height)
	unTouchBtn:setAnchorPoint(PT(0,0))
	unTouchBtn:setPosition(PT(0,midSprite:getPosition().y))
	unTouchBtn:addto(mLayer, 0)

	-- 此处添加场景初始内容
	--MainMenuLayer.init(3, mScene)

	local tabStr={Language.SKILL_LEVELUP}
	createTabBar(tabStr, mLayer)
	
	--返回按钮
	local exitBtn =ZyButton:new(Image.image_button, nil, nil, Language.IDS_BACK, FONT_NAME, FONT_SM_SIZE)
	exitBtn:setAnchorPoint(PT(0,0))
	exitBtn:setPosition(PT(pWinSize.width*0.4-exitBtn:getContentSize().width, pWinSize.height*0.25))
	exitBtn:registerScriptHandler(backAction)
	exitBtn:addto(mLayer, 1)
	
	--升级按钮
	local LevelUpBtn =ZyButton:new(Image.image_button, nil, nil, Language.IDS_LEVELUP, FONT_NAME, FONT_SM_SIZE)
	LevelUpBtn:setAnchorPoint(PT(0,0))
	LevelUpBtn:setPosition(PT(pWinSize.width*0.6, pWinSize.height*0.25))
	LevelUpBtn:registerScriptHandler(levelUPAction)
	LevelUpBtn:addto(mLayer, 1)	
	

	showContent()
	
	costContent()
	
	sendAction(1482)
	
end


--Tab界面切换
function createTabBar(tabName, fatherLayer)
	local tabBar = ZyTabBar:new(Image.image_top_button_0, Image.image_top_button_1, tabName, FONT_NAME, FONT_SM_SIZE, 4, Image.image_LeftButtonNorPath, Image.image_rightButtonNorPath);
--	tabBar:setCallbackFun(topTabBarAction); -----点击启动函数
	tabBar:addto(fatherLayer,0) ------添加
	tabBar:setColor(ZyColor:colorYellow())  ---设置颜色
	tabBar:setPosition(PT(SX(25),pWinSize.height*0.775))  -----设置坐标
end

--
function showContent()
	if trainLayer ~= nil then
		trainLayer:getParent():removeChild(trainLayer, true)
		trainLayer = nil
	end
	
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(train_x, train_y))
	mLayer:addChild(layer, 0)
	trainLayer = layer

	
	
	--头像
	local image = nil
	if skillInfo and skillInfo.HeadID then
		image = string.format("smallitem/%s.png", skillInfo.HeadID)
	end
	local item = creatItem(image, memberCallBack)
	item:setAnchorPoint(PT(0,0))
	local pos_x = pWinSize.width*0.2
	local pos_y = pWinSize.height*0.5
	item:setPosition(PT(pos_x, pos_y))
	layer:addChild(item, 0)		
	
	--显示文字
	showLabel(layer)
	
	
end;


--显示文字
function showLabel(layer)
	
	local pos_x = pWinSize.width*0.4
	local pos_y = pWinSize.height*0.6
	
	if skillInfo then
		--姓名
		creatLabel(layer, Language.MAGIC_NAME,  skillInfo.AbilityName , pos_x, pos_y)
		
		--等级
		pos_y = pos_y-pWinSize.height*0.05
		creatLabel(layer, Language.IDS_LEVEL,  skillInfo.AbilityLv , pos_x, pos_y)
	
		--经验
		pos_y = pos_y-pWinSize.height*0.05
		creatLabel(layer, Language.SKILL_EXP,  skillInfo.IsExperienceNum.."/".. skillInfo.NextExperienceNum, pos_x, pos_y)
	end
end


--创建文字
function creatLabel(parent, fitstName, secondName, x, y)
	if fitstName == nil then
		fitstName = ""
	end
	local firstLabel=CCLabelTTF:create(fitstName..":",FONT_NAME,FONT_SM_SIZE)
	firstLabel:setAnchorPoint(PT(0,0))
	local color = ZyColor:colorYellow()
	if color~=nil then
		firstLabel:setColor(color)
	end
	firstLabel:setPosition(PT(x, y))
	parent:addChild(firstLabel,0)
	
	if secondName == nil then
		secondName = ""
	end
	local secondLabel=CCLabelTTF:create(secondName,FONT_NAME,FONT_SM_SIZE)
	secondLabel:setAnchorPoint(PT(0,0))
	secondLabel:setPosition(PT(firstLabel:getPosition().x+firstLabel:getContentSize().width, firstLabel:getPosition().y))
	parent:addChild(secondLabel,0)	
end

--退出按钮响应
function backAction()
	closeAllLayer()
	if mFatherType == 1 then--佣兵列表界面
		MainMenuLayer.setIsShow(true, false)
		RoleBagScene.setBgIsVisible(false)
	elseif mFatherType == 2 then--阵营
		MainMenuLayer.setIsShow(false, false)
		HeroScene.setBgIsVisible(false)
	else--魂技列表界面
		SoulSkillList.refreshWin()
	end
	
end

--升级按钮响应
function levelUPAction()
	local itemString = nil
	if choiceTable then
		for k,v in ipairs(choiceTable) do
			if itemString == nil then
				itemString = v.UserItemID 
			else
				itemString = itemString..","..v.UserItemID 
			end
		end
	end
	if itemString then
		sendAction(1481, itemString)
	else
		ZyToast.show(mScene, Language.SKILL_CHOICE,1,0.35)		
	end
end;

--消耗
function costContent()
	if costLayer ~= nil then
		costLayer:getParent():removeChild(costLayer, true)
		costLayer = nil
	end
	
	local boxSize = SZ(pWinSize.width*0.9, pWinSize.height*0.15)
	
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(pWinSize.width*0.5-boxSize.width*0.5, pWinSize.height*0.4))
	mLayer:addChild(layer, 0)
	
	costLayer = layer

	--背景图
	local imageBg = CCSprite:create(P("common/list_1038.9.png"))
	imageBg:setScaleX(boxSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(boxSize.height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0,0))
	layer:addChild(imageBg, 0)
	
	--消耗魂技文字
	local costLabel=CCLabelTTF:create(Language.SKILL_COST, FONT_NAME, FONT_SM_SIZE)
	costLabel:setAnchorPoint(PT(0.5,0))
	costLabel:setPosition(PT(boxSize.width*0.5, boxSize.height))
	layer:addChild(costLabel,0)	


	--请选择消耗魂技
	local costLabel=CCLabelTTF:create(Language.SKILL_CHOICE, FONT_NAME, FONT_SM_SIZE)
	costLabel:setAnchorPoint(PT(0.5, 0))
	costLabel:setPosition(PT(boxSize.width*0.5, -costLabel:getContentSize().height))
	layer:addChild(costLabel,0)
	
	local exp = nil
	if choiceTable then
		for k,v in ipairs(choiceTable) do
			if exp == nil then
				exp = v.ExperienceNum 
			else
				exp = exp+v.ExperienceNum 
			end
			
		end
	end
	
	
	--增加经验值
	if exp then
		local costLabel=CCLabelTTF:create(Language.SKILL_ADD..exp, FONT_NAME, FONT_SM_SIZE)
		costLabel:setAnchorPoint(PT(0.5, 1))
		costLabel:setPosition(PT(boxSize.width*0.5, -costLabel:getContentSize().height*2))
		layer:addChild(costLabel,0)
	end	
	
	
	for i=1,5 do
		local memberCallBack = SoulSkillScene.skill_choice
		
		local image = nil
		if choiceTable and choiceTable[i] and choiceTable[i].HeadID then
			image = string.format("smallitem/%s.png", choiceTable[i].HeadID)
		end		

		local item= creatItem(image, memberCallBack)
		local pos_x = boxSize.width/5*(i-0.5)-item:getContentSize().width*0.5
		local pos_y = boxSize.height*0.5-item:getContentSize().width*0.5
		item:setAnchorPoint(PT(0,0))
		item:setPosition(PT(pos_x, pos_y))
		layer:addChild(item, 0)
		
	
	end
	
	
end;

--创建单个佣兵头像
function  creatItem(headImage,menberCallBack)
	local menuItem = CCMenuItemImage:create(P(Image.image_touxiang_beijing), P(Image.image_touxiang_beijing))
	local btn = CCMenu:createWithItem(menuItem)
	
	menuItem:setAnchorPoint(PT(0,0))
	if menberCallBack then 
	    menuItem:registerScriptTapHandler(menberCallBack)
	end 
	if tag then
		menuItem:setTag(tag)
	end
	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))
	
	-- 商品图片
	if headImage then
		local imageLabel = CCMenuItemImage:create(P(headImage),P(headImage))
		if imageLabel == nil then
			 return btn 
		end
		imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end
	
	return  btn
end

---------------------------------------------------------------------

--点击消耗卡片区域
function skill_choice()
	choiceContent()
end

function choiceContent()
	if choiceLayer ~= nil then
		choiceLayer:getParent():removeChild(choiceLayer, true)
		choiceLayer = nil
	end


	--黑底
	local layer = CCLayer:create()--Color:layerWithColor(ccc4(0,0,0,255))
	mScene:addChild(layer, 0)
	
	choiceLayer = layer
	
	--创建背景
	local midSprite=CCSprite:create(P("common/list_1040.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.68)
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)	
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	layer:addChild(midSprite,0)

	--屏蔽按钮ceng
	local unTouchBtn =  ZyButton:new(Image.image_transparent)
	unTouchBtn:setScaleX(boxSize.width/unTouchBtn:getContentSize().width)
	unTouchBtn:setScaleY(boxSize.height/unTouchBtn:getContentSize().height)
	unTouchBtn:setAnchorPoint(PT(0,0))
	unTouchBtn:setPosition(PT(0,midSprite:getPosition().y))
	unTouchBtn:addto(layer, 0)	
	
	local tabName = {Language.SKILL_LIST}
	createTabBar(tabName, choiceLayer)
	

	
--	MainMenuLayer.init(3, choiceLayer)
	
	--确认按钮	
	local button = ZyButton:new(Image.image_button, nil, nil, Language.IDS_SURE, FONT_NAME, FONT_SM_SIZE)
	button:setAnchorPoint(PT(0,0))
	button:setPosition(PT(pWinSize.width*0.75, pWinSize.height*0.775))
	button:addto(choiceLayer, 0)
	button:registerScriptHandler(key_sure)
	
	choiceNum = 0
	
	creatList()	
	
end;


function creatList()
	if heroLayer ~= nil then
		heroLayer:getParent():removeChild(heroLayer, true)
		heroLayer = nil
	end

	local layer = CCLayer:create()
	choiceLayer:addChild(layer, 0)
	
	heroLayer = layer
	
	
	local list_width = pWinSize.width*0.9
	local list_height = pWinSize.height*0.52
	local list_x = pWinSize.width*0.5-list_width*0.5
	local list_y = pWinSize.height*0.24
	
	local listSize = SZ(list_width, list_height)
	list = ScutCxList:node(listSize.height/4, ccc4(24, 24, 24, 0), listSize)
--	list:setHorizontal(true)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(list_x, list_y))
	layer:addChild(list,0)
--	list:registerItemClickListener("EmbattleScene.choiceClick")
	list:setTouchEnabled(true)
	skillList = list

	local ITEM = list
	
	local itemBg = CCSprite:create(P("common/list_1020.9.png"))
	itemBg:setAnchorPoint(PT(0,0))
	itemBg:setPosition(PT(ITEM:getPosition().x,ITEM:getPosition().y))
	itemBg:setScaleX(ITEM:getContentSize().width/itemBg:getContentSize().width)
	itemBg:setScaleY(ITEM:getContentSize().height/itemBg:getContentSize().height)
--	layer:addChild(itemBg, -1)
	
	
	local label = CCLabelTTF:create("", FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(PT(0.5,0))
	label:setPosition(PT(list_x+list_width*0.5, list_y+list_height*0.5))
	layer:addChild(label, 0)
	noticeLabel = label
	
--	if choiceTable then
--		choiceTable = {}
--	end

	showList()
	
	
end

function showList()
	local list = skillList
	list:clear()
	buttonTable = {}

	if not skillInfo then
		return
	end
	if skillInfo.RecordTabel and #skillInfo.RecordTabel == 0 then
		noticeLabel:setString(Language.ROLE_NOEQUIP)
	else
		noticeLabel:setString("")
	end
	
	for k,v in ipairs(skillInfo.RecordTabel) do
		local listItem=ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(0)	
		
		
		
		local item,choiceBtn = creatSkillItem(v,k)
		buttonTable[k] = choiceBtn
		
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


--创建魂技信息
function creatSkillItem(info, tag)
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
	
	
	--技能图片
	local image = nil
	if info.HeadID then
 		image = string.format("smallitem/%s.png", info.HeadID)
	end
	local headItem = creatHeadItem(image, tag, actionPath)
	headItem:setAnchorPoint(PT(0,0))
	headItem:setPosition(PT(layerSize.width*0.05, layerSize.height*0.5-headItem:getContentSize().height*0.5))
	layer:addChild(headItem, 0)


	local startX = layerSize.width*0.25

	--技能姓名
	local nameStr = info.AbilityName 
	if nameStr then
		local nameLabel = CCLabelTTF:create(nameStr, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(startX, layerSize.height*0.6))
		layer:addChild(nameLabel, 0)
	end
	
	--品质
	if genrealQuality[info.CurLevel] then
		local qualityStr = Language.BAG_QUALITY..":"..genrealQuality[info.CurLevel]
		local qualityLabel = CCLabelTTF:create(qualityStr, FONT_NAME, FONT_SM_SIZE)
		qualityLabel:setAnchorPoint(PT(0,0))
		qualityLabel:setPosition(PT(startX, layerSize.height*0.4))
		layer:addChild(qualityLabel, 0)
	end
	
	--等级
	if info.CurLevel then
		local levelStr = Language.ROLE_LEVEL..":"..info.CurLevel
		local levelLabel = CCLabelTTF:create(levelStr, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(layerSize.width*0.45, layerSize.height*0.4))
		layer:addChild(levelLabel, 0)	
	end



	--选择按钮	
	local button = ZyButton:new(Image.image_button_hook_0, Image.image_button_hook_1, nil)
	button:setAnchorPoint(PT(0,0))
	button:setPosition(PT(layerSize.width*0.85, layerSize.height*0.5-button:getContentSize().height*0.5))
	button:addto(layer, 0)
	button:setTag(tag)
--	button:registerScriptHandler(key_choicebtn)
	
	local actionBtn=UIHelper.createActionRect(itemBox,SoulSkillScene.key_choicebtn,tag)
	actionBtn:setAnchorPoint(PT(0,0))
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

	return btn
end

--点击选择按钮
function key_choicebtn(pNode)
	local tag = pNode
	if buttonTable[tag]._isSelected == true then
		buttonTable[tag]:unselected()
		choiceNum = choiceNum-1
	else
		if choiceNum < 5 then
			buttonTable[tag]:selected()
			choiceNum = choiceNum+1
		else
			ZyToast.show(mScene, Language.SKILL_FULL,1,0.35)
		end
	end	

end;

--确认按钮
function key_sure()

	choiceTable = {}
	for k,v in ipairs(buttonTable) do
		if v._isSelected == true then
			choiceTable[#choiceTable+1] = skillInfo.RecordTabel[k] 
		end
	end
	closeChoiceLayer()
	
	costContent()	
	
end

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
------------------------
function refreshWin()
	sendAction(1482)
end;

-----------------------
--发送请求
function sendAction(actionId, data)
	if actionId == 1482 then--魂技升级详细接口
		actionLayer.Action1482(mScene, nil, mUserItemID )
	elseif actionId == 1481 then--魂技升级接口（ID=1481）
		actionLayer.Action1481(mScene, nil, mAbilityID, mUserItemID, data)
	end
end


-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1481 then
		local serverInfo = actionLayer._1481Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			choiceTable = {}
			refreshWin()
			local str = string.format(Language.SKILL_ADDEXP, serverInfo.ExperienceNum)
			
			if serverInfo.AbilityLv and serverInfo.AbilityLv > m_currentLevel then
				str = string.format(Language.SKILL_UP, serverInfo.ExperienceNum, serverInfo.AbilityLv)
			end
			ZyToast.show(pScutScene, str,1,0.2)
		end	
	elseif actionId == 1482 then
		local serverInfo = actionLayer._1482Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			skillInfo = serverInfo
			
			m_currentLevel = serverInfo.AbilityLv 
			showContent()
			costContent()			
		end
	end	
end

