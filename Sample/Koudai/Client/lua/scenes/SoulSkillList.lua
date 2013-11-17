------------------------------------------------------------------
-- SoulSkillList.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 魂技列表
------------------------------------------------------------------

module("SoulSkillList", package.seeall)


mScene = nil 		-- 场景



-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end
--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	
end

-- 释放资源
function releaseResource()
	mLayer = nil
	mScene = nil
	heroLayer=nil
	heroList=nil
	noticeLabel=nil
	HeroAccessory.releaseResource()
	SoulSkillScene.releaseResource()
end

-- 创建场景
function createScene()

	local scene = ScutScene:new()
    mScene = scene.root 
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	
	scene:registerCallback(networkCallback)
	SlideInLReplaceScene(mScene,1)


	-- 注册网络回调
	
	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)

	--中间层
	local midSprite=CCSprite:create(P("common/list_1040.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.68)
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)	
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	mLayer:addChild(midSprite,0)
	
	--标签
	local tabName = { Language.SKILL_LIST}
	createTabBar(tabName)

	-- 此处添加场景初始内容
	MainMenuLayer.init(1, mLayer)
	
	showList()
	
	sendAction(1483)
end

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end



--Tab界面切换
function createTabBar(tabName)
	local tabBar = ZyTabBar:new(Image.image_top_button_0, Image.image_top_button_1, tabName, FONT_NAME, FONT_SM_SIZE, 4, Image.image_LeftButtonNorPath, Image.image_rightButtonNorPath);
	tabBar:addto(mLayer,0) ------添加
	tabBar:setColor(ZyColor:colorYellow())  ---设置颜色
	tabBar:setPosition(PT(SX(25),pWinSize.height*0.775))  -----设置坐标
end

function showList()
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	heroLayer = layer
	
	
	local list_width = pWinSize.width*0.9
	local list_height = pWinSize.height*0.52
	local list_x = pWinSize.width*0.5-list_width*0.5
	local list_y = pWinSize.height*0.24
	
	local listSize = SZ(list_width, list_height)
	list = ScutCxList:node(listSize.height/4, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(list_x, list_y))
	layer:addChild(list,0)
	list:setTouchEnabled(true)
	heroList = list
end

function showContent()
	local list = heroList
	heroList:clear()
	
	if not noticeLabel then
		noticeLabel = CCLabelTTF:create("", FONT_NAME, FONT_SM_SIZE)
		noticeLabel:setAnchorPoint(PT(0,0))
		noticeLabel:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.65))
		mLayer:addChild(noticeLabel, 0)
	end
	if #skillListInfo.RecordTabel == 0 then
		noticeLabel:setString(Language.SKILL_NOSKILL)
		return
	else
		noticeLabel:setString("")
	end 
	
	for k,v in ipairs(skillListInfo.RecordTabel) do
		local listItem=ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(0)	
		
		
		local item = creatItem(v,k)

		local layout=CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val =0
		layout.val_y.val.pixel_val =0

		listItem:addChildItem(item, layout)
		list:addListItem(listItem, false) 
	end
end;

--创建单条魂技信息
function creatItem(info, tag)
	local layer = CCLayer:create()
	
	local layerSize = SZ(list:getContentSize().width, list:getRowHeight())
	layer:setContentSize(layerSize)

	--背景
	local itemBg = CCSprite:create(P("common/list_1038.9.png"))
	itemBg:setScaleX((layerSize.width-SX(4))/itemBg:getContentSize().width)
	itemBg:setScaleY((layerSize.height-SY(4))/itemBg:getContentSize().height)
	itemBg:setAnchorPoint(PT(0,0))
	itemBg:setPosition(PT(SX(2), SY(2)))	
	layer:addChild(itemBg, 0)

	--魂技图片
	local actionPath = key_head
	local image = string.format("smallitem/%s.png", info.HeadID) 
	local headItem = creatHeadItem(image, tag, actionPath, info.AbilityQuality)
	headItem:setAnchorPoint(PT(0,0))
	headItem:setPosition(PT(layerSize.width*0.05, layerSize.height*0.5-headItem:getContentSize().height*0.5))
	layer:addChild(headItem, 0)
		

	local startX = layerSize.width*0.25
	
	--魂技名称 
	local nameStr = info.AbilityName 
	if nameStr then	
		local nameLabel = CCLabelTTF:create(nameStr, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(startX, layerSize.height*0.7+SY(1)))
		layer:addChild(nameLabel, 0)
	end
	
	--品质
	if info.AbilityQuality and genrealQuality[info.AbilityQuality] then
		local qualityStr = Language.BAG_QUALITY..":"..genrealQuality[info.AbilityQuality]
		local qualityLabel = CCLabelTTF:create(qualityStr, FONT_NAME, FONT_SM_SIZE)
		qualityLabel:setAnchorPoint(PT(0,0))
		qualityLabel:setPosition(PT(startX, layerSize.height*0.5+SY(1)))
		layer:addChild(qualityLabel, 0)
	end
	
	--魂技等级 
	if info.AbilityLv  then
		local levelStr = Language.ROLE_LEVEL..":"..info.AbilityLv
		local levelLabel = CCLabelTTF:create(levelStr, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(startX, layerSize.height*0.3+SY(1)))
		layer:addChild(levelLabel, 0)	
	end
	  ----  魂技威力
	  if  info.RatioNum  then 
	  	local ratioNumStr = Language.SKILL_PROWER..":"..info.RatioNum
		local ratioNum = CCLabelTTF:create(ratioNumStr, FONT_NAME, FONT_SM_SIZE)
		ratioNum:setAnchorPoint(PT(0,0))
		ratioNum:setPosition(PT(startX, layerSize.height*0.1+SY(1)))
		layer:addChild(ratioNum, 0)	
	  end
	
	--装备于
	local GeneralName = info.GeneralName--info.AbilityName 	
	if GeneralName  then
		local generalStr = Language.SKILL_EQUIP..GeneralName
		local generalLabel = CCLabelTTF:create(generalStr, FONT_NAME, FONT_SM_SIZE)
		generalLabel:setAnchorPoint(PT(0,0))
		generalLabel:setPosition(PT(layerSize.width*0.5, layerSize.height*0.6))
		layer:addChild(generalLabel, 0)	
	end
	
	--攻击类型	
	local AttackType = info.AttackType
	if AttackType and AttackTypetTable[AttackType] then
		local typeStr = AttackTypetTable[AttackType]
		local typeLabel = CCLabelTTF:create(typeStr, FONT_NAME, FONT_SM_SIZE)
		typeLabel:setAnchorPoint(PT(0,0))
		typeLabel:setPosition(PT(layerSize.width*0.5, layerSize.height*0.2))
		layer:addChild(typeLabel, 0)	
	end	
	 
	
	
	
	
	local button = ZyButton:new(Image.image_button, nil, nil, Language.IDS_LEVELUP, FONT_NAME, FONT_SM_SIZE)
	button:setAnchorPoint(PT(0,0))
	button:setPosition(PT(layerSize.width*0.8, layerSize.height*0.5-button:getContentSize().height*0.5))
	button:addto(layer, 0)
	button:registerScriptHandler(key_LvUp)
	button:setTag(tag)
	
	
	
	return layer
end;

--创建头像
function creatHeadItem(image, tag, actionPath, abilityQuality)

	-- 背景
	
	local bgImg = getQualityBg(abilityQuality,1)
	local menuItem = CCMenuItemImage:create(P(bgImg), P(bgImg))
	local btn = CCMenu:createWithItem(menuItem)
	btn:setAnchorPoint(PT(0,0))
	menuItem:setAnchorPoint(PT(0,0))
	if tag~= nil and tag~=-1 then
		menuItem:setTag(tag)
	end

	btn:setContentSize(SZ(menuItem:getContentSize().width,menuItem:getContentSize().height))
	
	
	--设置回调
	if actionPath ~= nil then
		menuItem:registerScriptTapHandler(actionPath)
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

function key_head(pNode)
	local tag = pNode
	local UserItemID = skillListInfo.RecordTabel[tag].UserItemID
	
	sendAction(1485, UserItemID)
	
end

--点击升级按钮
function key_LvUp(pNode)
	local tag = pNode:getTag()
	local AbilityID = skillListInfo.RecordTabel[tag].AbilityID--技能id
	local UserItemID = skillListInfo.RecordTabel[tag].UserItemID
	--	
	SoulSkillScene.setAbilityID(AbilityID, UserItemID, mScene)
	SoulSkillScene.init()
end


function refreshWin()
	sendAction(1483)
end

--发送请求
function sendAction(actionId, data)
	if actionId == 1483 then--        1483_魂技列表接口（ID=1483）
		actionLayer.Action1483(mScene, nil,1,199, 0)--PageIndex,PageSize,Ops  --0：全部：1未装在佣兵身上 
	elseif actionId == 1485 then
		actionLayer.Action1485(mScene,nil, data,data)
	end

end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1483 then--        1483_魂技列表接口（ID=1483）
		local serverInfo = actionLayer._1483Callback(pScutScene, lpExternalData)
		if serverInfo then
			skillListInfo = serverInfo
			showContent()
		end
	elseif actionId == 1481 or actionId == 1482 then
		SoulSkillScene.networkCallback(pScutScene, lpExternalData)
	elseif actionId == 1485 then
		local serverInfo = actionLayer._1485Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
	
			serverInfo.UserItemID = userData
			HeroAccessory.setScene(mScene)
			HeroAccessory.setData(serverInfo)
			local isNochangeBtn = true--是否没有更换按钮		
			HeroAccessory.showSkillDetailLayer(isNochangeBtn)
		end
	end
	commonCallback.networkCallback(pScutScene, lpExternalData)
end