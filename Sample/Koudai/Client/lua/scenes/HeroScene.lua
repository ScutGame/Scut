------------------------------------------------------------------
-- HeroScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 上阵佣兵详情信息
------------------------------------------------------------------

module("HeroScene", package.seeall)

require("scenes.TrainScene")
require("scenes.InheritScene")
require("layers.ItemListLayer")
require("scenes.HeroAccessory")
mScene = nil 		-- 场景


local weapType={
[4]=2,
[5]=5,
[6]=1,
}
local isClick=nil
local m_playerId = nil
m_current_GerenalIndex = 1
--背包点击小图标查看佣兵信息
function isRoleBagLook(generalId,info)
	initResource()
	mGeneralId = generalId
	m_current_Info = info
	isLookSingelHeroInfo = true
	if playerId then
		MainMenuLayer.setCurrentScene(105)
	end
	createScene()

	
	heroInfo()
	HeroAccessory.setData(m_current_Info)			
	HeroAccessory.refreshPlayerDate(mLayer)
	if not m_playerId then
		buttonContent()
	end
	sendAction(1401)--头部佣兵			
end


-- 场景入口
function pushScene(generalId,playerId)
	mGeneralId = generalId
	m_playerId=playerId
	if playerId then
		MainMenuLayer.setCurrentScene(105)
	end
	
	initResource()
	createScene()
	if isFirst == true and not m_playerId then
		isFirst = false
		sendAction(1403)
	end	
	
	sendAction(1401)--头部佣兵	
end

function getBtnTable()
	return btnTable
end;


--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	m_current_GerenalIndex=nil
	isFirst = true
	btnTable = {}
	
	showType = 1--1显示装备 2显示魂技
end

-- 释放资源
function releaseResource()
	m_current_GerenalIndex = nil
	mGeneralInfo=nil
	mGeneralId = nil
	dataLayer=nil
	mDetailLayer = nil
	m_choiceImge=nil
	choiceLayer = nil
	m_heroLayer = nil
	m_buttonLayer = nil	
	mLayer = nil	
	_headList = nil
	mScene = nil
	isClick=nil
	HeroAccessory.releaseResource()
	SoulSkillScene.releaseResource()
	ItemListLayer.releaseResource()
	btnTable=nil
	bgLayer=nil
	m_playerId=nil
	isLookSingelHeroInfo=nil
end

function setBgIsVisible(value)
	if bgLayer then
		bgLayer:setVisible(value)
	end
end;

-- 创建场景
function createScene()

	local scene = ScutScene:new()
    mScene = scene.root 
	-- 注册网络回调	
			
	mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	scene:registerCallback(networkCallback)
	SlideInLReplaceScene(mScene,1)

	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)


	--创建背景
	creatBg()



	-- 此处添加场景初始内容
	MainMenuLayer.init(4, mScene)
	
	
	headList()


end

function creatBg()
	local imageBg = CCSprite:create(P(Image.ImageBackground))
	imageBg:setScaleX(pWinSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height*0.855/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, pWinSize.height*0.145))
	mLayer:addChild(imageBg, 0)
	
	
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

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end

--顶部佣兵头像显示区
function headList()
	if choiceLayer ~= nil then
--		choiceLayer:getParent():removeChild(choiceLayer, true)
--		choiceLayer = nil
	end
	
	local m_showChoice_Width = pWinSize.width
	local m_showChoice_Height = pWinSize.height* 0.18
	local m_showChoice_StartX = 0
	local m_showChoice_StartY = pWinSize.height-m_showChoice_Height
	local list_width = m_showChoice_Width* 0.9    ---- list width
       local list_height = m_showChoice_Height *0.7

	local layer = CCLayer:create()
	layer:setContentSize(SZ(pWinSize.width,pWinSize.height))
	layer:setAnchorPoint(PT(0, 0))
	layer:setPosition(PT(m_showChoice_StartX ,m_showChoice_StartY))
	mLayer:addChild(layer, 0)
	
	choiceLayer = layer
	

	
	local background = CCSprite:create(P("common/list_1052.9.png"));--背景--common/transparentBg.png
	background:setScaleY(list_height/background:getContentSize().height)
	background:setAnchorPoint(PT(0,0))
	background:setPosition(PT(pWinSize.width*0.5-background:getContentSize().width*0.5,list_height*0.15))
	layer:addChild(background,0)
	

	list_width = background:getContentSize().width*0.95
	
	local list = ScutCxList:node(list_width/5, ccc4(124, 124, 124, 255),CCSize(list_width, list_height))
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(pWinSize.width*0.5-list_width*0.5, list_height*0.15))
	list:setHorizontal(true)
	list:setTouchEnabled(true)
	list:setPageTurnEffect(true)
	list:setRecodeNumPerPage(1)	
	list:registerLoadEvent(moveList)
	layer:addChild(list,0)
	_headList = list


	---左右按钮 
	local lSprite=CCSprite:create(P("button/list_1069.png"))
	lSprite:setAnchorPoint(PT(1,0.5))
	lSprite:setPosition(PT(background:getPosition().x, background:getPosition().y+list_height/2))
	layer:addChild(lSprite, 0)
	local rSprite=CCSprite:create(P("button/list_1068.png"))
	rSprite:setAnchorPoint(PT(0,0.5))
	rSprite:setPosition(PT(background:getPosition().x+background:getContentSize().width, background:getPosition().y+list_height/2))
	layer:addChild(rSprite, 0)	
	
end

--佣兵头像
function showHeroHead(info)
	local list = _headList
	mHeadTable = {}
	for k,v in ipairs(info) do
			
		local listItem = ScutCxListItem:itemWithColor(ccc3(42,28,13))
		listItem:setAnchorPoint(PT(0,0))
		listItem:setPosition(PT(0, 0))
		listItem:setOpacity(0)
		list:addListItem(listItem , false)
			
		local image = string.format("smallitem/%s.png", v.HeadID)
		local quality = v.GeneralQuality  

		local wlayer = CCLayer:create()
		local SimpleItem = nil
		SimpleItem = createMemberItem(image, quality,k, key_head)
		SimpleItem:setAnchorPoint(PT(0,0))

		local pos_x = (list:getRowWidth()-SimpleItem:getContentSize().width)*0.5
		local pos_y = (list:getContentSize().height-SimpleItem:getContentSize().height)*0.5
		SimpleItem:setPosition(PT(pos_x, pos_y))
		wlayer:addChild(SimpleItem,0)
				
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
	
		listItem:addChildItem(wlayer,layout)
		
	end
	
end

--移动佣兵头像  list
function moveList(page)
	currentPage = page+1

end;

--选择佣兵
function key_head(pNode, index)
	local tag = nil
	if index then
		tag = index
		pNode = mHeadTable[index]
	else
		tag =pNode:getTag()
	end
	
	
	
	
	m_current_GerenalIndex = tag
	
	if m_choiceImge ~= nil then
		m_choiceImge:getParent():removeChild(m_choiceImge,true)
		m_choiceImge=nil
	end
	if m_choiceImge == nil then
		m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		m_choiceImge:setPosition(PT(pNode:getContentSize().width*0.5,pNode:getContentSize().height*0.5))
		pNode:addChild(m_choiceImge,0)
	end
	
	sendAction(1403)
end

--创建单个佣兵头像
function  createMemberItem(headImage,quality,tag,menberCallBack)

	local bgPic =  getQualityBg(quality, 1)

	local menuItem = CCMenuItemImage:create(P(bgPic), P(bgPic))
	local btn = CCMenu:createWithItem(menuItem)
	
	menuItem:setAnchorPoint(PT(0,0))
	menuItem:registerScriptTapHandler(function () menberCallBack(menuItem) end)
	if tag then
		menuItem:setTag(tag)
	end
	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))
	
	mHeadTable[#mHeadTable+1] = menuItem
	
	
	--佣兵头像
	if headImage then
		local imageLabel = CCMenuItemImage:create(P(headImage),P(headImage))
		if imageLabel == nil then
			 return btn 
		end
		imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5,menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end

	if m_current_GerenalIndex == nil then
		m_current_GerenalIndex = 1
	end
	--[[
	if tag == m_current_GerenalIndex then
		m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		m_choiceImge:setPosition(PT(btn:getContentSize().width*0.5,btn:getContentSize().height*0.5))
		btn:addChild(m_choiceImge,0)
	end
    ]]

	
	return  btn
end
------------------------------

--佣兵信息显示区
function heroInfo()
	if m_heroLayer then
		m_heroLayer:getParent():removeChild(m_heroLayer, true)
		m_heroLayer = nil	
	end
	
				
	hero_x = 0
	hero_y = pWinSize.height*0.45
	hero_width = pWinSize.width
	hero_height = pWinSize.height*0.4

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(hero_x, hero_y))
	mLayer:addChild(layer, 0)
	
	m_heroLayer = layer
	
	--佣兵大图片背景
	local bgPic = getQualityBg(m_current_Info.GeneralQuality, 3)
	
	local imageBg = ZyButton:new(bgPic,bgPic,nil)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(hero_width*0.5-imageBg:getContentSize().width*0.5, pWinSize.height*0.16-imageBg:getContentSize().height*0.5))
	imageBg:addto(layer, 0)
	imageBg:registerScriptHandler(key_hero)
	
	local image = string.format("bigitem/%s.png", m_current_Info.PicturesID)
	
	local headImage = CCSprite:create(P(image))
	headImage:setAnchorPoint(PT(0,0))
	local pos_x = imageBg:getContentSize().width*0.5-headImage:getContentSize().width*0.5
	local pos_y = imageBg:getContentSize().height*0.09
	headImage:setPosition(PT(pos_x, pos_y))
	imageBg:addChild(headImage, 0)
	
	--佣兵名称
	local secondLabel=CCLabelTTF:create(m_current_Info.GeneralName,FONT_NAME,FONT_SM_SIZE)
	secondLabel:setAnchorPoint(PT(0,0))
	secondLabel:setPosition(PT(imageBg:getContentSize().width*0.15, imageBg:getContentSize().height*0.92))
	imageBg:addChild(secondLabel,0)
	
	--佣兵等级
	local levelLabel=CCLabelTTF:create(m_current_Info.GeneralLv ..Language.IDS_LEV,FONT_NAME,FONT_SM_SIZE)
	levelLabel:setAnchorPoint(PT(1,0))
	levelLabel:setPosition(PT(imageBg:getContentSize().width*0.85, imageBg:getContentSize().height*0.92))
	imageBg:addChild(levelLabel,0)

	--职业背景
	local pos_x = imageBg:getContentSize().width*0.8
	local pos_y = imageBg:getContentSize().height*0.8
	local careerBg = CCSprite:create(P("common/list_1156.png"))
	careerBg:setAnchorPoint(PT(0.5,0.5))
	careerBg:setPosition(PT(pos_x, pos_y))
	imageBg:addChild(careerBg, 0)	

	--职业
	if m_current_Info.CareerID and carrerImg[m_current_Info.CareerID] then
		local careerImage = string.format("common/%s.png", carrerImg[m_current_Info.CareerID])
	
		local careerImgSprite=CCSprite:create(P(careerImage))
		careerImgSprite:setAnchorPoint(PT(0.5,0.5))
		careerImgSprite:setPosition(PT(pos_x, pos_y))
		imageBg:addChild(careerImgSprite,0)
	end

	btnTable.box = {}
	
	local equipTable = {"list_1192", "list_1191", "list_1190", "list_1199", "list_1193",}
	local partTable = {"list_1194", "list_1199", "list_1195", "list_1197", "list_1196", "list_1198",}

	local equiptInfo = m_current_Info.Equipt
	local skillInfo = m_current_Info.Skill
	for i=1,6 do
		local image, name, level,IsSynthesis = nil
		local tag = i
		local menberCallBack = HeroScene.choiceAction
		local bgImg = Image.image_touxiang_beijing	
		if showType == 1 then
			if equiptInfo[i] then
				image = string.format("smallitem/%s.png", equiptInfo[i].HeadPic) 
				name = equiptInfo[i].ItemName 
				level = equiptInfo[i].ItemLv
				if not m_playerId then
					IsSynthesis = equiptInfo[i].IsSynthesis
				end
				bgImg = getQualityBg(equiptInfo[i].QualityType,1)
			else
				if i == 2 then
					image = string.format("common/%s.png", equipTable[m_current_Info.CareerID]) 
				else
					image = string.format("common/%s.png", partTable[i]) 
				end				
			end
		elseif showType == 2 then
			if i > m_current_Info.AbilityNum then
				image = Image.image_lock
			else
				if skillInfo[i] then
					image = string.format("smallitem/%s.png",skillInfo[i].AbilityHead)
					name = skillInfo[i].AbilityName 
					level = skillInfo[i].AbilityLv
					bgImg =  getQualityBg(skillInfo[i].AbilityQuality,1)
				end
			end
		end
		
		local item = creatCardItem(image, name, level, tag, menberCallBack, IsSynthesis,bgImg)
		local pos_x = nil
		local pos_y = pWinSize.height*0.25-pWinSize.height*0.135*((i-1)%3)
		
		if math.floor((i-1)/3) == 0 then
			pos_x = pWinSize.width*0.08
		elseif math.floor((i-1)/3) == 1  then
			pos_x = pWinSize.width*0.92-item:getContentSize().width
		end
		item:setAnchorPoint(PT(0,0))
		item:setPosition(PT(pos_x, pos_y))
		layer:addChild(item, 0)
		
		btnTable.box[#btnTable.box+1] = item
	end

	if GuideLayer.judgeIsGuide(6) and showType == 1 then
		GuideLayer.close()
		local layerRank=0		
		GuideLayer.setScene(mScene,layerRank)
		GuideLayer.init()
	else
		GuideLayer.close()
	end
	
end

--创建技能，装备 卡片   小
function creatCardItem(image, name, level, tag, menberCallBack, IsSynthesis,bgImg)
	local menuItem = CCMenuItemImage:create(P(bgImg), P(bgImg))
	local btn = CCMenu:createWithItem(menuItem)
	menuItem:setAnchorPoint(PT(0,0))
	
	--响应函数
	if menberCallBack then
		menuItem:registerScriptTapHandler(function () menberCallBack(menuItem) end )
	end
	
	--索引
	if tag then
		menuItem:setTag(tag)
	end
	

	-- 图片
	if image then
		local imageLabel = CCMenuItemImage:create(P(image),P(image))
		if imageLabel == nil then
			 return btn 
		end
		imageLabel:setAnchorPoint(CCPoint(0.5, 0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end	
	
	--名称
	if name then
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0.5, 1))
		nameLabel:setPosition(PT(menuItem:getContentSize().width*0.5, 0))
		menuItem:addChild(nameLabel, 0)
	end
	
	--合成按钮
	if IsSynthesis == 1 then
		local button = ZyButton:new("button/list_1030_1.png")
		button:setAnchorPoint(PT(0,0))
		local pos_x = menuItem:getContentSize().width-button:getContentSize().width
		local pos_y = 0
		button:setPosition(PT(pos_x, pos_y))	
		button:registerScriptTapHandler(HeroScene.SynthesisAction)
		button:setTag(tag)
		button:addto(menuItem, 0)	
	end
	




	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))
	return  btn
end;

--创建佣兵信息卡片  大
function creatHeroCard()
	local menuItem = CCMenuItemImage:create(P(Image.image_touxiang_beijing), P(Image.image_touxiang_beijing))
	local btn = CCMenu:createWithItem(menuItem)
	
	menuItem:setAnchorPoint(PT(0,0))
	
	if menberCallBack then
		menuItem:registerScriptTapHandler(menberCallBack)
	end
	
	--佣兵名称
	if name then
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(0,0))
		menuItem:addChild(nameLabel, 0)
	end
	
	if tag then
		menuItem:setTag(tag)
	end
	-- 商品图片
	if image then
		
		local imageLabel = CCMenuItemImage:create(P(image),P(image))
		if imageLabel == nil then
			 return btn 
		end
		imageLabel:setAnchorPoint(CCPoint(0.5,0.5))
		imageLabel:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.5))
		menuItem:addChild(imageLabel,0)
	end	


	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))
	return  btn	
end

--点击佣兵大卡片
function key_hero()
	if not m_playerId then
		showHeroDetailLayer.setData(m_current_Info, 1, mScene)
		showHeroDetailLayer.init(mScene)
	end
end;
------------------

--佣兵装备选择  / 魂技选择 
function choiceAction(pNode, index)
if not m_playerId then
	local tag = nil
	if index then
		tag = index
	else
		tag = pNode:getTag()
	end
	local general = nil
	if isLookSingelHeroInfo then
		general = mGeneralId
	else
	    m_current_GerenalIndex = 1
		general = mGeneralInfo[m_current_GerenalIndex].GeneralID
	end
	
	mPosition = tag
	if showType == 1 then--武器点击响应
		currentEquipInfo = m_current_Info.Equipt[tag]
		if m_current_Info.Equipt[tag]  then--有武器
			
			showEquipDetail()
		else--没有武器
			ItemListLayer.setInfo(mScene)
			local equipType = tag
			ItemListLayer.init(1, equipType, nil, general)
		end
	
	elseif showType == 2 then--魂技点击响应
		if tag <= m_current_Info.AbilityNum then
			currentSkillInfo = m_current_Info.Skill[tag]
			if currentSkillInfo then
				showSkillDetail()
			else
				local skillType = tag
				ItemListLayer.setInfo(mScene)
				ItemListLayer.init(2, skillType, nil, general)
			end
		else
			ZyToast.show(mScene, Language.ROLE_SKILLNOTICE,1,0.2)
		end	
	end
end
end


------------------
--武器详情
function showEquipDetail()
	sendAction(1202, 1)


end


--魂技详情
function showSkillDetail()
	sendAction(1485)
end

--装备合成
function SynthesisAction(pNode)
	local tag = pNode:getTag()
	
	sendAction(1601, tag)
end

--获取当前点击的位置
function getItemPosition()
	return mPosition
end

--获取当前佣兵id
function getGeneralId()
	return mGeneralInfo[m_current_GerenalIndex].GeneralID
end;

---------------------------
--按钮
function buttonContent()
	if m_buttonLayer then
--		m_buttonLayer:getParent():removeChild(m_buttonLayer, true)
--		m_buttonLayer = nil	
	end

	btn_width = pWinSize.width*0.8
	btn_height = pWinSize.height*0.1
	btn_x = (pWinSize.width-btn_width)*0.5
	btn_y = pWinSize.height*0.165

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(btn_x, btn_y))
	mLayer:addChild(layer, 0)
	
	m_buttonLayer = layer
	
	btnId = {
		eChuanChen = 1,
		ePeiYang = 2,
		eHunJi = 3,

	}

	local funcTable = {
		{ Id = btnId.eChuanChen, btnName = Language.ROLE_INHERIT, },
		{ Id = btnId.ePeiYang, btnName = Language.ROLE_TRAIN, },
		{ Id = btnId.eHunJi, btnName = Language.ROLE_SOULSKILL, },
	}

	
	for k,v in ipairs(funcTable) do
		local button = ZyButton:new(Image.image_button, nil, nil, v.btnName, FONT_NAME,FONT_SM_SIZE)
		button:setAnchorPoint(PT(0,0))
		local pos_x = btn_width*0.25*k-button:getContentSize().width*0.5
		local pos_y = 0
		button:setPosition(PT(pos_x, pos_y))	
		button:registerScriptHandler(key_button)
		button:setTag(v.Id)
		button:addto(layer, 0)
		if v.Id == btnId.eHunJi then
			m_changeBtn = button
		end
	end
	if showType == 2 then
		m_changeBtn:setString(Language.COLLECTION_EQUIP)
	end	

end;

--功能按钮点击响应
function key_button(pNode)
	local tag = pNode:getTag()
	if tag == btnId.eChuanChen then--传承
		gotoInherit()
	elseif tag == btnId.ePeiYang then--培养
		gotoTrain()
	elseif tag == btnId.eHunJi then--魂技
		if showType == 1 then
			showType = 2
			m_changeBtn:setString(Language.COLLECTION_EQUIP)
		else
			showType = 1
			m_changeBtn:setString(Language.ROLE_SOULSKILL)
		end
		heroInfo()
	end
end


--跳转到培养界面
function gotoTrain()
	local generalId = nil
	if isLookSingelHeroInfo then
		generalId = mGeneralId
	else
		generalId = mGeneralInfo[m_current_GerenalIndex].GeneralID	
	end
	TrainScene.setGeneralInfo(m_current_Info,generalId)
	if  not isClick then
		isClick=true
		sendAction(1443,1)
	end
	
	if isLookSingelHeroInfo then
		closeLookSingelInfo()
	end

end

--跳转到传承界面
function gotoInherit()
	sendAction(1448)
end;


--刷新当前佣兵信息
function refreshWin()
	sendAction(1403)
end;

--合成成功提示
function showCreatSuccess()
    ZyToast.show(mScene, Language.HUNT_FUSIONTIP, 1.5, 0.35)
end

function getCurrentHeroInfo()
	return m_current_Info
end
--------------------------------------------


--发送请求
function sendAction(actionId, data)
	if actionId == 1401 then
		local ops = 3
		if isLookSingelHeroInfo then
			ops = 1
		end
		
		actionLayer.Action1401(mScene, nil, m_playerId,ops)-- 1401_玩家佣兵列表接口
	elseif actionId == 1403 then--佣兵详情
		local GeneralID = nil
		if not mGeneralInfo then
			GeneralID = mGeneralId
		else
			GeneralID = mGeneralInfo[m_current_GerenalIndex].GeneralID
		end
		actionLayer.Action1403(mScene, nil, GeneralID, m_playerId,GeneralID)
	elseif actionId == 1448 then--  1448_传承人与被传承人选择接口（ID=1448）
		local userData = {}
		if isLookSingelHeroInfo then
			userData.generalId = mGeneralId
		else
			userData.generalId = mGeneralInfo[m_current_GerenalIndex].GeneralID	
		end
		userData.inheritType = 2
		actionLayer.Action1448(mScene, nil, userData.generalId, 2,userData)--1：传承人 2：被传承人
	elseif actionId == 1443 then
	
		actionLayer.Action1443(mScene,nil)
	elseif actionId == 1202 then
		local type = data
		actionLayer.Action1202(mScene,nil, currentEquipInfo.UserItemID, nil, type )--1详情， 2强化
	elseif actionId == 1485 then	
		local Data = {}
		Data.currentGeneralID=mGeneralId 
		Data.Position=mPosition
		Data.UserItemID = currentSkillInfo.UserItemID
		
		actionLayer.Action1485(mScene,nil, currentSkillInfo.UserItemID, Data)

	elseif actionId == 1601 then
		local tag = data
		local itemId = m_current_Info.Equipt[tag].ItemID
		local userItemId=m_current_Info.Equipt[tag].UserItemID
		SynthesisLayer.setItemID(nil,userItemId)
		actionLayer.Action1601(mScene,nil, itemId, userItemId)
	end
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1401 then
		local serverInfo = actionLayer._1401Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			mGeneralInfo = serverInfo.RecordTabel
			
			for k,v in ipairs(serverInfo.RecordTabel) do
				if v.GeneralID == mGeneralId then
					m_current_GerenalIndex = k
				end
			end
		
			showHeroHead(serverInfo.RecordTabel)		
			
			if m_playerId and isFirst then
				isFirst = false
				key_head(nil, 1)
			end
			
		end
			
	elseif actionId == 1403 then
		local serverInfo = actionLayer._1403Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			m_current_Info = serverInfo
			local Equipt = {}
			for k,v in ipairs(serverInfo.RecordTabel) do
				local postion = v.Position
				Equipt[postion] = v
			end
			m_current_Info.Equipt = Equipt
			
			local Skill = {}
			for k,v in ipairs(serverInfo.RecordTabel2) do
				local postion = v.Position
				Skill[postion] = v
			end
			m_current_Info.Skill = Skill
			
			heroInfo()
			HeroAccessory.setData(m_current_Info)			
			HeroAccessory.refreshPlayerDate(mLayer)
			if not m_playerId then
				buttonContent()
			end

			
		end

	elseif actionId==1202 or actionId==1204 then--装备详情
		HeroAccessory.setScene(mScene)
		HeroAccessory.networkCallback(pScutScene, lpExternalData)
		
	elseif actionId == 1203 or actionId == 1209 then
		ItemListLayer.networkCallback(pScutScene, lpExternalData)
	elseif  actionId == 1217 then
		TrainScene._1217Callback(pScutScene, lpExternalData)
	elseif actionId==1443 then
		local serverInfo = actionLayer._1443Callback(pScutScene, lpExternalData)
		if serverInfo then
			TrainScene.setTrainTabel(serverInfo, 1)
			TrainScene.initScene(mScene)
		end
	elseif actionId == 1448 then
		if userData.inheritType == 2 then
			local serverInfo = actionLayer._1448Callback(pScutScene, lpExternalData)
			if serverInfo then
				InheritScene.setGeneralId(mGeneralId)
				InheritScene.init(1)
			end
		elseif  userData.inheritType == 1 then
			InheritScene.networkCallback(pScutScene, lpExternalData)
		end
--	elseif actionId == 1417 or actionId == 1416 or actionId == 1419 then
--		InheritScene.networkCallback(pScutScene, lpExternalData)
	elseif actionId == 1483 or actionId == 1484 then
		ItemListLayer.networkCallback(pScutScene, lpExternalData)
	elseif actionId == 1485 then
		local serverInfo = actionLayer._1485Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then

			serverInfo.UserItemID = userData.UserItemID
			serverInfo.GeneralID = userData.currentGeneralID
			serverInfo.Position = userData.Position
			
			HeroAccessory.setScene(mScene)
			HeroAccessory.setData(serverInfo)
			local mOpenType = 2----   1佣兵列表界面  2阵营		
			HeroAccessory.showSkillDetailLayer(nil, mOpenType)
		end
	elseif actionId == 1601 then
		local serverData=actionLayer._1601Callback(pScutScene,lpExternalDate)
		if serverData then
			local bgType = 2
			SynthesisLayer.setDataInfo(serverData, bgType)
			SynthesisLayer.init(pScutScene)
		end
	elseif actionId == 1603 then
		SynthesisLayer.networkCallback(pScutScene, lpExternalData)		
	elseif actionId == 1481 or actionId == 1482 then
		SoulSkillScene.networkCallback(pScutScene, lpExternalData)
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)	
	end
	isClick=false
	

end



