------------------------------------------------------------------
-- HeroLvUp.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 佣兵升级
------------------------------------------------------------------

module("HeroLvUp", package.seeall)

require("layers.HeroLvUpChoiceLayer")


mScene = nil 		-- 场景

function setGeneralID(generalID)
	mGeneralID = generalID 
end

-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
	MainScene.init()
end


function getBtnTable()
	return btnTable
end;
--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	btnTable = {}
	
end

-- 释放资源
function releaseResource()
	HeroLvUpChoiceLayer.releaseResource()
	if manLayer then
		manLayer:getParent():removeChild(manLayer, true)
		manLayer = nil	
	end
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	mLayer = nil
	mScene = nil
	mGeneralID = nil
	currentInfo = nil
	heroListInfo = nil
	mCurrentLevel=nil
end

-- 创建场景
function createScene()
	local scene = ScutScene:new()
	mScene = scene.root 
	mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	scene:registerCallback(networkCallback)
	SlideInLReplaceScene(mScene,1)




	
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
	
	-- 此处添加场景初始内容

--	local tabName = { Language.ROLE_LVUP}
--	createTabBar(tabName)
	
	--返回按钮	
	local exitBtn = ZyButton:new(Image.image_button, nil, nil, Language.IDS_BACK, FONT_NAME, FONT_SM_SIZE)
	exitBtn:setAnchorPoint(PT(1,0))
	exitBtn:setPosition(PT(pWinSize.width*0.4, pWinSize.height*0.16))
	exitBtn:addto(mLayer, 0)
	exitBtn:registerScriptHandler(popScene)	
	

	--升级按钮	
	local button = ZyButton:new(Image.image_button, nil, nil, Language.ROLE_LVUP, FONT_NAME, FONT_SM_SIZE)
	button:setAnchorPoint(PT(0,0))
	button:setPosition(PT(pWinSize.width*0.6, pWinSize.height*0.16))
	button:addto(mLayer, 0)
	button:registerScriptHandler(LvUpAction)	
	
	btnTable.lvUpBtn = button
	
	MainMenuLayer.init(1, mScene)	
	
	showContent()
	


end



function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end



--Tab界面切换
function createTabBar(tabName)
	local tabBar = ZyTabBar:new(Image.image_top_button_0, Image.image_top_button_1, tabName, FONT_NAME, FONT_SM_SIZE, 4, Image.image_LeftButtonNorPath, Image.image_rightButtonNorPath);
	tabBar:setCallbackFun(topTabBarAction); -----点击启动函数
	tabBar:addto(mLayer,0) ------添加
	tabBar:setColor(ZyColor:colorYellow())  ---设置颜色
	tabBar:setPosition(PT(SX(25),pWinSize.height*0.775))  -----设置坐标
end

function showContent()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	showLayer = layer
	

	--佣兵大图片背景]
	
	local imageBg = ZyButton:new("common/list_1085.png","common/list_1085.png",nil)
	imageBg:setAnchorPoint(PT(0,0))
	local pos_y = pWinSize.height*0.34+pWinSize.height*0.475*0.5-imageBg:getContentSize().height*0.5 --pWinSize.height*0.825*0.95-imageBg:getContentSize().height
	imageBg:setPosition(PT(pWinSize.width*0.5-imageBg:getContentSize().width*0.5, pos_y))
	imageBg:addto(layer, 0)
	imageBg:registerScriptHandler(key_hero)
	
	
	btnTable.imageBg = imageBg
	


	local image = nil
	if currentInfo and currentInfo.CurrHead then
		image = string.format("bigitem/%s.png", currentInfo.CurrHead)
	end
	if image then	
		local headImage = CCSprite:create(P(image))
		headImage:setAnchorPoint(PT(0,0))
		local pos_x = imageBg:getContentSize().width*0.5-headImage:getContentSize().width*0.5
		local pos_y = imageBg:getContentSize().height*0.09
		headImage:setPosition(PT(pos_x, pos_y))
		imageBg:addChild(headImage, 0)
	else
		local noticeLabel = CCLabelTTF:create(Language.ROLE_POINTCHOICE, FONT_NAME, FONT_SM_SIZE)
		noticeLabel:setAnchorPoint(PT(0.5, 0.5))
		noticeLabel:setPosition(PT(imageBg:getContentSize().width*0.5, imageBg:getContentSize().height*0.5))
		imageBg:addChild(noticeLabel, 0)	
	end
	
	local cardInfo = {}
	if currentInfo then
		cardInfo = currentInfo.RecordTabel
	end
	btnTable.cardTable = {}
	for i=1,6 do
		local image, name, level,num = nil
		local tag = i
		local menberCallBack = key_to_choiceCost
		if cardInfo[i] then
			image = string.format("smallitem/%s.png", cardInfo[i].HeadID) 
			name = cardInfo[i].ItemName
			num = cardInfo[i].ItemNum 		
		--	level = equiptInfo[i].ItemLv 
		end
		
		
	
		local item = creatCardItem(image, name, level, tag, menberCallBack, num)
		local pos_x = pWinSize.width*0.1+pWinSize.width*0.65*(math.ceil(i/3)-1)
		local pos_y = imageBg:getPosition().y+imageBg:getContentSize().height-item:getContentSize().height*1.3*((i-1)%3+1)
		item:setAnchorPoint(PT(0,0))
		item:setPosition(PT(pos_x, pos_y))
		layer:addChild(item, 0)
		
		btnTable.cardTable [#btnTable.cardTable +1] = item
	end
	
	
	

	manInfo()

	if GuideLayer.judgeIsGuide(10) then
		GuideLayer.setScene(mScene)	
		GuideLayer.init()
	end	
end;

--选择升级佣兵
function  key_hero()
	mCurrentLevel = nil
	sendAction(1401)
end;

--创建技能，装备 卡片   小
function creatCardItem(image, name, level, tag, menberCallBack, num)
	local menuItem = CCMenuItemImage:create(P(Image.image_touxiang_beijing), P(Image.image_touxiang_beijing))
	local btn = CCMenu:createWithItem(menuItem)
	menuItem:setAnchorPoint(PT(0,0))
	
	--响应函数
	if menberCallBack then
		menuItem:registerScriptTapHandler(menberCallBack)
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

	if num then
		local nameLabel = CCLabelTTF:create("x"..num, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(1, 0))
		nameLabel:setPosition(PT(menuItem:getContentSize().width*0.9, menuItem:getContentSize().height*0.1))
		menuItem:addChild(nameLabel, 0)	
	end

	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))
	return  btn
end;

-----------------------------------------------------
--信息
function manInfo()
	if manLayer then
		manLayer:getParent():removeChild(manLayer, true)
		manLayer = nil	
	end

	lot_width = pWinSize.width*0.8
	lot_height = pWinSize.height*0.12
	lot_x = (pWinSize.width-lot_width)*0.5
	lot_y = pWinSize.height*0.22

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(lot_x, lot_y))
	mLayer:addChild(layer, 0)
	
	manLayer = layer
	


	
	local background = CCSprite:create(P("common/list_1052.9.png"));--背景--common/transparentBg.png
--	background:setScaleX(lot_width/background:getContentSize().width)
	background:setScaleY(lot_height/background:getContentSize().height)
	background:setAnchorPoint(PT(0.5,0))
	background:setPosition(PT(lot_width*0.5,0))
	layer:addChild(background,0)

	local pos_x = background:getPosition().x-background:getContentSize().width*0.48
	local pos_y = lot_height*0.7
	
	--策划案版本

	if currentInfo then
		--佣兵名称
		creatLabel(layer, Language.HERO_NAME, currentInfo.CurrGeneralName  , pos_x, pos_y)
		
	
		--获得经验
		pos_y = lot_height*0.45
		creatLabel(layer, Language.HERO_GETEXP, currentInfo.Experience , pos_x, pos_y)
	
		--升级经验
		pos_y = lot_height*0.2
		if currentInfo.Percent~=0 then
			creatLabel(layer, Language.ROLE_UPEXP, currentInfo.UpExperience.."("..currentInfo.Percent..")"  , pos_x, pos_y)
		else
			creatLabel(layer, Language.ROLE_UPEXP, currentInfo.UpExperience  , pos_x, pos_y)
		end
		
		
		--当前等级
		pos_y = lot_height*0.7
		pos_x = lot_width*0.52
		creatLabel(layer, Language.HERO_CURRLV,  currentInfo.CurrLv , pos_x, pos_y)
	
		--可提升等级至
		pos_y = lot_height*0.45
		if currentInfo.CurrLv==100 or currentInfo.NextLv==0 then
			creatLabel(layer, Language.HERO_NEXTLV, Language.HERO_SHANGXIAN , pos_x, pos_y)
		else
			creatLabel(layer, Language.HERO_NEXTLV,  currentInfo.NextLv , pos_x, pos_y)
		end
		
		--天赋魂技
		pos_y = lot_height*0.2
		local skillInfo = SkillInfoConfig.getSkillInfo(currentInfo.AbilityID)
		local skillName = nil		
		if skillInfo then
			skillName = skillInfo.AbilityName
		end
		creatLabel(layer, Language.PUB_ABILITY,  skillName , pos_x, pos_y)	
	
	end

end;

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
	
--	return firstLabel,secondLabel
end

------------------------------------------
--------------------------

--点击选择消耗卡片
function key_to_choiceCost()
	if currentInfo then
		sendAction(1444)
	else
		ZyToast.show(mScene, Language.HERO_PLEASECHOICE,1,0.35)
	end
end;

---------------------------
function refreshWin()
	sendAction(1441)
end


----点击升级按钮响应
function LvUpAction()
	if mGeneralID then
		if currentInfo.Status==0 then
			sendAction(1442)
		elseif currentInfo.Status==1 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(mScene, nil,Language.HERO_TISHI , Language.IDS_SURE, Language.IDS_CANCEL,isAction) 
		end
	else
		ZyToast.show(mScene, Language.HERO_PLEASECHOICE,1,0.35)
	end
end

function isAction(clickedButtonIndex)
	if clickedButtonIndex == 1 then--确认
		sendAction(1442)
	end
end;

--------------------
--播放佣兵升级动画
function playAnimation()
	local attribuAni=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("donghua_1006")
	attribuAni:play()
	attribuAni:registerFrameCallback("HeroLvUp.finishAnimation")
	mLayer:addChild(attribuAni, 1)
	attribuAni:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.5))

end

--动画播放完成后 
function finishAnimation(pSprite, curAniIndex, curFrameIndex, nPlayingFlag)
	if nPlayingFlag == 2 then
		pSprite:registerFrameCallback("")
		delayRemove(pSprite)
	end
end

function delayRemove(sprite)
	if sprite ~= nil then
		local delayAct = CCDelayTime:create(0.1)
		local funcName = CCCallFuncN:create(HeroLvUp.removeTmpSprite)
		local act = CCSequence:createWithTwoActions(delayAct,funcName)
		sprite:runAction(act)
		sprite = nil
	end
	
end

function removeTmpSprite(sprite)
	if sprite ~= nil then
		sprite:removeFromParentAndCleanup(true)
		sprite = nil
	end
end



--------------

--发送请求
function sendAction(actionId, data,num)
	if actionId == 1441 then
		actionLayer.Action1441(mScene, nil, mGeneralID)
	elseif actionId == 1401 then
		actionLayer.Action1401(mScene, nil, nil, 1)
	elseif actionId == 1442 then
		actionLayer.Action1442(mScene, nil, mGeneralID)
	elseif actionId == 1444 then
		actionLayer.Action1444(mScene, nil, 1, 200)
	elseif actionId == 1445 then
		actionLayer.Action1445(mScene, nil, data,mGeneralID,num)
	end

end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1441 then--1441_佣兵升级界面接口（ID=1441）
		local serverInfo = actionLayer._1441Callback(pScutScene, lpExternalData)
		if serverInfo then
			currentInfo = serverInfo
			
			if mCurrentLevel and mCurrentLevel < currentInfo.CurrLv then
				playAnimation()
			end
			mCurrentLevel = currentInfo.CurrLv
			showContent()
		end
	elseif actionId == 1401 then
		local serverInfo = actionLayer._1401Callback(pScutScene, lpExternalData)
		if serverInfo then
			heroListInfo = serverInfo

			
			local type = 1
			HeroLvUpChoiceLayer.init(mScene, type)
			HeroLvUpChoiceLayer.showList(serverInfo.RecordTabel)
		end
	elseif actionId == 1442 then
		local serverInfo = actionLayer._1442Callback(pScutScene, lpExternalData)
		if serverInfo then		
			refreshWin()
		end
	elseif actionId == 1444 then
		local serverInfo = actionLayer._1444Callback(pScutScene, lpExternalData)
		if serverInfo then	
			local type = 2
			
			HeroLvUpChoiceLayer.setCardTable(currentInfo.RecordTabel)
			HeroLvUpChoiceLayer.init(mScene, type)
			HeroLvUpChoiceLayer.showList(serverInfo.RecordTabel)
		end
	elseif actionId == 1445 then
		local serverInfo = actionLayer._1445Callback(pScutScene, lpExternalData)
		if serverInfo then
			refreshWin()
		end
	end
	
	commonCallback.networkCallback(pScutScene, lpExternalData)
end