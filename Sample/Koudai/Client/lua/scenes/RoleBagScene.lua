------------------------------------------------------------------
-- RoleBagScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 佣兵背包
------------------------------------------------------------------

module("RoleBagScene", package.seeall)
require("layers.showHeroDetailLayer")


mScene = nil 		-- 场景

local mGeneralInfo=nil
local isClick=nil


function setBgIsVisible(value)
	if bgLayer then
		bgLayer:setVisible(value)
	end
end;


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
	mCurrentTab = 1
	_isInherit = MenuBtnConfig.getMenuItem(38)
	_isTrain = MenuBtnConfig.getMenuItem(5)
	_isInherit = 1
	_isTrain = 1

end

-- 释放资源
function releaseResource()
	mLayer=nil
	mScene=nil
	heroLayer=nil
	bgLayer=nil
--	HeroAccessory.releaseResource()	
--	SoulSkillScene.releaseResource()	
end

-- 创建场景
function createScene()
	local scene  = ScutScene:new()
	scene:registerCallback(networkCallback)
    mScene = scene.root 
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	SlideInLReplaceScene(mScene,1)




	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)
	
	--中间层
	creatBg()
	
	
	--当前容量
	local label = CCLabelTTF:create("", FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(PT(0,0))
	label:setPosition(PT(pWinSize.width*0.05, pWinSize.height*0.75))
	mLayer:addChild(label, 0)
	
	_numLabel = label

	
	
	local tabName = { Language.BAG_HERO, Language.ROLE_SOUL}
	createTabBar(tabName)

	-- 此处添加场景初始内容
	MainMenuLayer.init(4, mScene)
	MainMenuLayer.setIsShow(true, false)
	
	showHeroList()
	
	sendAction(1401)
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end



function creatBg()
	local midSprite=CCSprite:create(P("common/list_1040.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.68)
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)	
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	mLayer:addChild(midSprite,0)
	
	bgLayer = CCLayer:create()
	mScene:addChild(bgLayer, 0)	
	local imageBg = CCSprite:create(P(Image.image_mainscene))
	imageBg:setScaleX(pWinSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, 0))
	bgLayer:addChild(imageBg, 0)
	bgLayer:setVisible(false)
end;

--创建Tab
function createTabBar(tabName)
	local tabBar = ZyTabBar:new(Image.image_top_button_0, Image.image_top_button_1, tabName, FONT_NAME, FONT_SM_SIZE, 4, Image.image_LeftButtonNorPath, Image.image_rightButtonNorPath);
	tabBar:setCallbackFun(topTabBarAction); -----点击启动函数
	tabBar:addto(mLayer,0) ------添加
	tabBar:setColor(ZyColor:colorYellow())  ---设置颜色
	tabBar:setPosition(PT(SX(25),pWinSize.height*0.775))  -----设置坐标
end

--Tab界面切换
function topTabBarAction(bar,pNode)
	local index  = pNode:getTag();
	if mCurrentTab ~= index then
		mCurrentTab =index
		releaseAllLayer()
		if index == 1 then--佣兵列表
			sendAction(1401)
		elseif index == 2 then--碎片列表
			sendAction(1401)
		end
	end
end

--关闭所有层
function releaseAllLayer()
	
end;

--创建佣兵列表
function showHeroList()
	if heroLayer then
		heroLayer:getParent():removeChild(heroLayer, true)
		heroLayer = nil
	end
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

	local ITEM = list
	
	local itemBg = CCSprite:create(P("common/list_1020.9.png"))
	itemBg:setAnchorPoint(PT(0,0))
	itemBg:setPosition(PT(ITEM:getPosition().x,ITEM:getPosition().y))
	itemBg:setScaleX(ITEM:getContentSize().width/itemBg:getContentSize().width)
	itemBg:setScaleY(ITEM:getContentSize().height/itemBg:getContentSize().height)
--	layer:addChild(itemBg, -1)
	
	--提示文字
	local label = CCLabelTTF:create("", FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(PT(0.5,0))
	label:setPosition(PT(list_x+list_width*0.5, list_y+list_height*0.5))
	layer:addChild(label, 0)
	noticeLabel = label
	

end

--佣兵列表内容
function showContent()
	local list = heroList
	heroList:clear()
	
	if heroTable.RecordTabel and #heroTable.RecordTabel == 0 then
		if mCurrentTab == 1 then
			noticeLabel:setString(Language.ROLE_NOHEROCARD)
		elseif mCurrentTab == 2 then
			noticeLabel:setString(Language.ROLE_NOSOULCARD)
		end
		return
	else
		noticeLabel:setString("")
	end
	
	for k,v in ipairs(heroTable.RecordTabel) do
		local listItem=ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(0)	
		
		
		
		local item = nil
		if mCurrentTab == 1 then--佣兵列表
			item = creatHreoItem(v,k)
		elseif mCurrentTab == 2 then--灵魂 列表
			item  = creatSoulItem(v,k)
		end

		local layout=CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val =0
		layout.val_y.val.pixel_val =0

		listItem:addChildItem(item, layout)
		list:addListItem(listItem, false) 
	end
end;

--创建单条佣兵信息
function creatHreoItem(info, tag)
	local layer = CCLayer:create()
	
	local layerSize = SZ(list:getContentSize().width, list:getRowHeight())
	layer:setContentSize(layerSize)

	--背景
	local itemBg = CCSprite:create(P("common/list_1038.9.png"))
	itemBg:setScaleX((layerSize.width-SX(4))/itemBg:getContentSize().width)
	itemBg:setScaleY((layerSize.height-SY(2))/itemBg:getContentSize().height)
	itemBg:setAnchorPoint(PT(0,0))
	itemBg:setPosition(PT(SX(2), SY(2)))	
	layer:addChild(itemBg, 0)

	--佣兵头像
	local image = string.format("smallitem/%s.png", info.HeadID)
	local quality = info.GeneralQuality 
	local actionPath = key_head
	local headItem = creatHeadItem(image, quality, tag, actionPath)
	headItem:setAnchorPoint(PT(0,0))
	headItem:setPosition(PT(layerSize.width*0.05, layerSize.height*0.5-headItem:getContentSize().height*0.5))
	layer:addChild(headItem, 0)
	

	local startX = layerSize.width*0.25
	
	--佣兵姓名
	if info.GeneralName then
		local nameStr = info.GeneralName
		local nameLabel = CCLabelTTF:create(nameStr, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(startX, layerSize.height*0.6))
		layer:addChild(nameLabel, 0)
	end
	
	--品质
	if info.GeneralQuality and genrealQuality[info.GeneralQuality] then
		local qualityStr = Language.BAG_QUALITY..":"..genrealQuality[info.GeneralQuality]
		local qualityLabel = CCLabelTTF:create(qualityStr, FONT_NAME, FONT_SM_SIZE)
		qualityLabel:setAnchorPoint(PT(0,0))
		qualityLabel:setPosition(PT(startX, layerSize.height*0.4))
		layer:addChild(qualityLabel, 0)
	end
	
	--等级
	if info.GeneralLv and mCurrentTab == 1 then
		local levelStr = Language.ROLE_LEVEL..":"..info.GeneralLv
		local levelLabel = CCLabelTTF:create(levelStr, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(startX, layerSize.height*0.2))
		layer:addChild(levelLabel, 0)	
	end
	

	if info.IsBattle == 1 then
		local label = CCLabelTTF:create(Language.BAG_INGROUP, FONT_NAME, FONT_SM_SIZE)
		label:setAnchorPoint(PT(0,0))
		label:setPosition(PT( layerSize.width*0.45, layerSize.height*0.6))
		layer:addChild(label, 0)	

	end
	
	

	--培养按钮
	
	local startX = layerSize.width*0.8
	
	if _isTrain then
		local buttonStr = Language.ROLE_TRAIN
		local buttonCallback = trainAction	
		local button = ZyButton:new(Image.image_button, nil, nil, buttonStr, FONT_NAME, FONT_SM_SIZE)
		button:setAnchorPoint(PT(0,0))
		button:setPosition(PT(startX, layerSize.height*0.5-button:getContentSize().height*0.5))
		button:addto(layer, 0)
		if tag then
			button:setTag(tag)
		end
		button:registerScriptHandler(buttonCallback)	
		
		startX = startX-button:getContentSize().width*1.1
	end
	
	
	
	
	
	--传承按钮
	if _isInherit then
		local buttonStr = Language.ROLE_INHERIT
		local buttonCallback = inheritAction	
		local  button = ZyButton:new(Image.image_button, nil, nil, buttonStr, FONT_NAME, FONT_SM_SIZE)
		button:setAnchorPoint(PT(0,0))
		button:setPosition(PT(startX, layerSize.height*0.5-button:getContentSize().height*0.5))
		button:addto(layer, 0)
		if tag then
			button:setTag(tag)
		end
		button:registerScriptHandler(buttonCallback)		
	end
	

	return layer
end;


--创建单个灵魂信息
function creatSoulItem(info, tag)
	local layer = CCLayer:create()
	
	local layerSize = SZ(list:getContentSize().width, list:getRowHeight())
	layer:setContentSize(layerSize)

	--背景
	local itemBg = CCSprite:create(P("common/list_1038.9.png"))
	itemBg:setScaleX((layerSize.width-SX(4))/itemBg:getContentSize().width)
	itemBg:setScaleY((layerSize.height-SY(2))/itemBg:getContentSize().height)
	itemBg:setAnchorPoint(PT(0,0))
	itemBg:setPosition(PT(SX(2), SY(2)))	
	layer:addChild(itemBg, 0)

	--佣兵头像
	local image = string.format("smallitem/%s.png", info.HeadID)
	local quality = info.GeneralQuality 
	local actionPath = key_head
	local headItem = creatHeadItem(image, quality, tag, actionPath)
	headItem:setAnchorPoint(PT(0,0))
	headItem:setPosition(PT(layerSize.width*0.05, layerSize.height*0.5-headItem:getContentSize().height*0.5))
	layer:addChild(headItem, 0)
	

	local startX = layerSize.width*0.25
	
	--佣兵姓名
	if info.GeneralName then
		local nameStr = info.GeneralName
		local nameLabel = CCLabelTTF:create(nameStr, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(startX, layerSize.height*0.6))
		layer:addChild(nameLabel, 0)
	end
	
	--品质
	if info.GeneralQuality and genrealQuality[info.GeneralQuality] then
		local qualityStr = Language.BAG_QUALITY..":"..genrealQuality[info.GeneralQuality]
		local qualityLabel = CCLabelTTF:create(qualityStr, FONT_NAME, FONT_SM_SIZE)
		qualityLabel:setAnchorPoint(PT(0,0))
		qualityLabel:setPosition(PT(startX, layerSize.height*0.4))
		layer:addChild(qualityLabel, 0)
	end
	
	--等级
	if info.GeneralLv and mCurrentTab == 1 then
		local levelStr = Language.ROLE_LEVEL..":"..info.GeneralLv
		local levelLabel = CCLabelTTF:create(levelStr, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(startX, layerSize.height*0.2))
		layer:addChild(levelLabel, 0)	
	end

	--灵魂数量
	if info.CurrNum and info.DemandNum then
		local str = Language.BAG_CURRNUM..":"..info.CurrNum.."/"..info.DemandNum 
		local levelLabel = CCLabelTTF:create(str, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(startX, layerSize.height*0.2))
		layer:addChild(levelLabel, 0)	
	end

	--差多少可以招募，突破
	if info.WorseNum and info.WorseNum > 0 and ( info.IsRecruit == 0 or info.IsRecruit == 2 )then
		local str = ""
		if info.IsRecruit == 2 then--突破--0：不可招募1：可招募 2：不可突破 3：可突破 
			str = string.format(Language.BAG_BREAKLACK, info.WorseNum)		
		elseif info.IsRecruit == 0 then--招募
			str = string.format(Language.BAG_LACK, info.WorseNum)				
		end

		local levelLabel = CCLabelTTF:create(str, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(1,0.5))
		levelLabel:setPosition(PT(layerSize.width*0.75, layerSize.height*0.5))
		layer:addChild(levelLabel, 0)	
	end
	
	--按钮  灵魂招募

	local buttonStr,buttonCallback = nil
	if info.IsRecruit == 3 then--突破
		buttonStr = Language.BAG_BREAK
		buttonCallback = breakAction	
	elseif info.IsRecruit == 1 then--招募
		buttonStr = Language.PUB_RECRIUIT
		buttonCallback = recuitAction		
	end
	if buttonStr then
		local  button = ZyButton:new(Image.image_button, nil, nil, buttonStr, FONT_NAME, FONT_SM_SIZE)
		button:setAnchorPoint(PT(0,0))
		button:setPosition(PT(layerSize.width*0.8, layerSize.height*0.5-button:getContentSize().height*0.5))
		button:addto(layer, 0)
		if tag then
			button:setTag(tag)
		end
		button:registerScriptHandler(buttonCallback)		

	end
	
	
	return layer
end;


--创建头像
function creatHeadItem(image, quality, tag, actionPath)

	-- 背景
	local bgPic =  getQualityBg(quality, 1)
	local menuItem = CCMenuItemImage:create(P(bgPic), P(bgPic))
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

--点击头像响应
function key_head(pNode)
	if mCurrentTab == 1 then
		local tag = pNode
		m_currentGeneralID = heroTable.RecordTabel[tag].GeneralID 
		sendAction(1403)
	end
end

--灵魂招募按钮响应
function recuitAction(pNode)
	local tag = pNode:getTag()
	if tag then 
		local generalID = heroTable.RecordTabel[tag].GeneralID
		sendAction(1404, generalID)		
	end
end;

--突破按钮响应
function breakAction(pNode)
	local tag = pNode:getTag()
	if tag then
		local generalID = heroTable.RecordTabel[tag].GeneralID
		sendAction(1425, generalID)			
	end
end


function askIsBreak(index, content, tag)
	if index == 1 then
		actionLayer.Action1425(mScene,nil, tag, 2)	
	end
end

--佣兵升级按钮响应  转跳到佣兵升级界面
function lvupAction(pNode)
	local tag = pNode:getTag()
	if tag then 
		local generalID = heroTable.RecordTabel[tag].GeneralID
		HeroLvUp.setGeneralID(generalID)
		HeroLvUp.pushScene()
	end
	
end;

--培养按钮响应  转跳到培养界面
function trainAction(pNode)
	local tag = pNode:getTag()
	if tag then 
		m_currentGeneralID = heroTable.RecordTabel[tag].GeneralID
		
		if  not isClick then
			isClick=true
			sendAction(14031)
		end	
	end
end;

--传承按钮响应  转跳到传承界面
function inheritAction(pNode)
	local tag = pNode:getTag()
	if tag then 
		local generalID = heroTable.RecordTabel[tag].GeneralID
		sendAction(1448, generalID)
	end
end;

--跳转到培养界面
function gotoTrain()
	TrainScene.setGeneralInfo(mGeneralInfo,m_currentGeneralID)
	sendAction(1443)
end

--跳转到传承界面
function gotoInherit()
	sendAction(1448,m_currentGeneralID)
end;


--刷新界面
function refreshWin()
	sendAction(1401)
end



function sendAction(actionId, data)
	if actionId == 1401 then
		local GeneralType = mCurrentTab--0玩家   1职业佣兵  2魂魄  3上阵佣兵
		actionLayer.Action1401(mScene, nil, ToUserID, GeneralType)
	elseif actionId == 1448 then
		local userData = {}
		userData.generalId = data
		userData.inheritType = 2
		actionLayer.Action1448(mScene, nil, data , 2, userData)--1：传承人 2：被传承人	
	elseif actionId == 1403 then
		actionLayer.Action1403(mScene, nil, m_currentGeneralID, ToUserID,m_currentGeneralID)
	elseif actionId == 14031 then
		actionLayer.Action1403(mScene, nil, m_currentGeneralID, ToUserID,1)
	elseif actionId == 1404 then--招募
		actionLayer.Action1404(mScene, nil, 4, data)		
	elseif actionId == 1443 then
		actionLayer.Action1443(mScene,nil, data)
	elseif actionId == 1425 then--突破
		local GeneralID = data
		actionLayer.Action1425(mScene,nil, GeneralID, 1)	
	end
end;

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1401 then----        1401_玩家佣兵列表接口（ID=1401）
		local serverInfo = actionLayer._1401Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			heroTable = serverInfo
			showContent()
		end
	elseif actionId == 1403 then
		local serverInfo = actionLayer._1403Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
		
			mGeneralInfo=serverInfo

			local Equipt = {}
			for k,v in ipairs(serverInfo.RecordTabel) do
				local postion = v.Position
				Equipt[postion] = v
			end
			mGeneralInfo.Equipt = Equipt
			
			local Skill = {}
			for k,v in ipairs(serverInfo.RecordTabel2) do
				local postion = v.Position
				Skill[postion] = v
			end
			mGeneralInfo.Skill = Skill
			
			if userData == 1 then
				TrainScene.setGeneralInfo(mGeneralInfo,m_currentGeneralID)
				sendAction(1443,1)
			else
				local generalId = userData
--				local layer = creatHeroInfoBg()
--				HeroScene.setHeroInfo(serverInfo,generalId,mScene, layer)
				
				HeroScene. isRoleBagLook(generalId,serverInfo)
--				showHeroDetailLayer.setData(serverInfo, 2, mScene)
--				showHeroDetailLayer.init(mScene)
			end
		end
	elseif actionId == 1404 then
		local serverInfo = actionLayer._1404Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then	
			ZyToast.show(pScutScene, Language.BAG_RECRUITSUCCESS,1.5,0.3)
			refreshWin()
		end
	elseif actionId == 1448 then--传承
		if userData.inheritType == 2 then
			local serverInfo = actionLayer._1448Callback(pScutScene, lpExternalData)
			if serverInfo then
				InheritScene.init(2)
			end
		elseif  userData.inheritType == 1 then
			InheritScene.networkCallback(pScutScene, lpExternalData)
		end	
--	elseif actionId == 1417 or actionId == 1416 or actionId == 1419 then--传承
--		InheritScene.networkCallback(pScutScene, lpExternalData)		
	elseif actionId == 1217 then--培养
		TrainScene._1217Callback(pScutScene, lpExternalData)
	elseif actionId==1443 then--培养
		local serverInfo = actionLayer._1443Callback(pScutScene, lpExternalData)
		if serverInfo then
			TrainScene.setTrainTabel(serverInfo, 2)
			TrainScene.initScene(mScene)
		end	
	elseif actionId == 1485 then
		local serverInfo = actionLayer._1485Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			local currentGeneralID = userData.currentGeneralID
			local position = userData.Position
			
			HeroAccessory.setScene(mScene)
			HeroAccessory.setData(serverInfo)
			local mOpenType = 1----   1佣兵列表界面  2阵营				
			HeroAccessory.showSkillDetailLayer(1,mOpenType)
		end	
	elseif actionId == 1425 then		
		if ZyReader:getResult() == 1 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene, nil, ZyReader:readErrorMsg(), Language.IDS_SURE, Language.IDS_CANCEL,askIsBreak);
			box:setTag(userData)			
		elseif ZyReader:getResult() == 2 or ZyReader:getResult() == 0 then
			DataTabel={}
			DataTabel.GeneralName= ZyReader:readString()
			DataTabel.Potential= ZyReader:getInt()
			local str = string.format(Language.BAG_BREAKNUM, DataTabel.GeneralName, DataTabel.Potential)
			ZyToast.show(pScutScene, str,1,0.35)
			refreshWin()
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
		end
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)
	end
	isClick=false
	

end

function creatHeroInfoBg()
	local layer = CCLayer:create()
	mScene:addChild(layer, 0)

	local imageBg = CCSprite:create(P(Image.ImageBackground))
	imageBg:setScaleX(pWinSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height*0.855/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, pWinSize.height*0.145))
	layer:addChild(imageBg, 0)
	
	local actionBtn=UIHelper.createActionRect(SZ(pWinSize.width, pWinSize.height*0.855))
	actionBtn:setPosition(imageBg:getPosition())
	layer:addChild(actionBtn, 0)	

	return layer
end

