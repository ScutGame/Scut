------------------------------------------------------------------
-- HotalLayer.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("HotalLayer", package.seeall)


mScene = nil 		-- 场景

function getBtnTable()
	return btnTable
end



--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	detail_width = pWinSize.width*0.92
	detail_height = pWinSize.height*0.535
	detail_x = pWinSize.width*0.5-detail_width*0.5
	detail_y = pWinSize.height*0.235
	isGuide = nil
end

function close()
	GuideLayer.close()
	if detailLayer ~= nil then
		detailLayer:getParent():removeChild(detailLayer, true)
		detailLayer = nil
	end
	releaseResource()
end

-- 释放资源
function releaseResource()
	detailLayer=nil
	timeTable = nil
	mScene = nil
	isGuide = nil
	newHeroLayer=nil
	isGuide = nil		
end

-- 创建场景
function init(fatherScene,fatherLayer)

	initResource()

	mScene = fatherScene
	
	local layer = CCLayer:create()
	fatherLayer:addChild(layer, 5)
	
	mLayer= layer

 	sendAction(1402)
end



function showDetail()
	if detailLayer ~= nil then
		detailLayer:getParent():removeChild(detailLayer, true)
		detailLayer = nil
	end
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(detail_x, detail_y))
	mLayer:addChild(layer, 0)

	detailLayer = layer


	local titleTable = {"title/list_1143.png", "title/list_1144.png", "title/list_1145.png",}--百里挑一，千里挑一，万里挑一
	local roleTable = {"smallitem/Icon_2530.png", "smallitem/Icon_2511.png", "smallitem/Icon_2500.png",}
	local firstTable = {nil, "mainUI/icon_7226.png", "mainUI/icon_7227.png"}
	
	timeTable = {}
	
	btnTable = {}
	for i=1,3 do
		if heroInfo.RecordTabel[i] then

			local coldTime = nil
			local str = nil
			if heroInfo.RecordTabel[i].ColdTime and heroInfo.RecordTabel[i].ColdTime > 0 then
				coldTime = heroInfo.RecordTabel[i].ColdTime
				str = formatTime(coldTime)..Language.HOTAL_FREE
			else
				str = Language.HOTAL_FREETIME..":"..heroInfo.RecordTabel[i].SurplusNum 
			end
			local tag = i
			local titleImg = titleTable[heroInfo.RecordTabel[i].RecruitType]--招募类型图片
			local roleImg = roleTable[heroInfo.RecordTabel[i].RecruitType]
			local Quality = heroInfo.RecordTabel[i].Quality
			local DemandGold = heroInfo.RecordTabel[i].DemandGold
			local firstImage = nil
			if  heroInfo.RecordTabel[i].IsFirst == 0 then
				firstImage = firstTable[heroInfo.RecordTabel[i].RecruitType]
			end
			
			local item,timeLabel = creatItem(str, titleImg, roleImg, Quality, DemandGold, tag, firstImage)			

			timeTable[i] = {}
			timeTable[i].time = coldTime
			timeTable[i].label = timeLabel
			
			local pos_x = detail_width/3*(i-0.5)-item:getContentSize().width*0.5
			local pos_y = detail_height*0.2
			
			item:setAnchorPoint(PT(0,0))
			item:setPosition(PT(pos_x, pos_y))
			layer:addChild(item, 0)
		end
	end
	
	detailLayer:stopAllActions()
	countDown()
	
	if GuideLayer.judgeIsGuide(4) then
			isGuide = true
		GuideLayer.setScene(mScene)	
		GuideLayer.init()
	else
		isGuide = nil
	end	

end

--创建单个
function creatItem(labelStr, image, roleImage, Quality, DemandGold, tag, firstImage)
	
	local layer = CCLayer:create()
	
	ItemSize = SZ(detail_width*0.3, detail_height*0.7)--imageBg:getContentSize()
	
	local imageBg = CCSprite:create(P("common/List_2009_3.9.png"))
	imageBg:setScaleX(ItemSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(ItemSize.height/imageBg:getContentSize().height)	
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0,0))
	layer:addChild(imageBg, 0)
	
	layer:setContentSize(ItemSize)
	
	--标题
	if image then
		local titleImg = CCSprite:create(P(image))
		titleImg:setAnchorPoint(PT(0.5,1))
		titleImg:setPosition(PT(ItemSize.width*0.5, ItemSize.height*0.95))
		layer:addChild(titleImg, 0)
	end
	
	local middleBg = CCSprite:create(P("common/list_1012.png"))
	middleBg:setAnchorPoint(PT(0.5,0.5))
	middleBg:setPosition(PT(ItemSize.width*0.5, ItemSize.height*0.6))
	layer:addChild(middleBg, 0)
	--佣兵头像  固定

	if roleImage then
		local roleImg = CCSprite:create(P(roleImage))
		roleImg:setAnchorPoint(PT(0.5,0.5))
		roleImg:setPosition(PT(ItemSize.width*0.5, ItemSize.height*0.6))
		layer:addChild(roleImg, 0)	
	end
	
	if firstImage then
		local firstImg = CCSprite:create(P(firstImage))
		firstImg:setAnchorPoint(PT(0.5,0.5))
		firstImg:setPosition(PT(ItemSize.width*0.5, ItemSize.height*0.6))
		layer:addChild(firstImg, 0)		
	end
	
	--可发现的佣兵品质 文字
	local descLabel = CCLabelTTF:create(Language.HOTAL_DESC..":", FONT_NAME, FONT_SM_SIZE)
	descLabel:setAnchorPoint(PT(0,0))
	descLabel:setPosition(PT(ItemSize.width*0.05, ItemSize.height*0.33))
	layer:addChild(descLabel, 0)
	
	--品质
	if Quality then
		local qualityLayer = CCLayer:create()
		
		local table = Split(Quality,",")
		local startX=0
		for k,v in ipairs(table) do
			v = tonumber(v)
			if qualityText[v] then
				local color = ZyColor:getRoleQualityColor(v)
				local str = ""
				if k ~= 1 then
					str = str.." "
				end
				str = str..qualityText[v]
				
				local label = CCLabelTTF:create(str, FONT_NAME, FONT_SM_SIZE)
				label:setAnchorPoint(PT(0,0))
				label:setPosition(PT(startX, 0))
				label:setColor(color)
				qualityLayer:addChild(label, 0)
				startX = startX+label:getContentSize().width
			end
		end
	
	
		qualityLayer:setAnchorPoint(PT(0,0))
		qualityLayer:setPosition(PT(ItemSize.width*0.5-startX*0.5, ItemSize.height*0.25))
		layer:addChild(qualityLayer, 0)

	end
	
	--剩余次数，倒计时
	local timeLabel = nil
	if labelStr then
		timeLabel = CCLabelTTF:create(labelStr, FONT_NAME, FONT_SM_SIZE)
		timeLabel:setAnchorPoint(PT(0.5,0))
		timeLabel:setPosition(PT(ItemSize.width*0.5, ItemSize.height*0.17))
		layer:addChild(timeLabel, 0)
	end
	
	
	--价格
	if DemandGold and DemandGold > 0 then
		local item,labelSize = imageMoney("mainUI/list_1006.png", DemandGold)
	--	local costLabel = CCLabelTTF:create(DemandGold, FONT_NAME, FONT_SM_SIZE)
		item:setAnchorPoint(PT(0,0))
		item:setPosition(PT(ItemSize.width*0.5-labelSize.width*0.5, ItemSize.height*0.05))
		layer:addChild(item, 0)	
	end
	
	
	
	
	--招募按钮
	local button = ZyButton:new("button/list_1039.png", nil, nil, Language.PUB_RECRIUIT, FONT_NAME, FONT_SM_SIZE)
	button:setAnchorPoint(PT(0,0))
	button:setPosition(PT(ItemSize.width*0.5-button:getContentSize().width*0.5, -button:getContentSize().height*1.25))
	button:addto(layer, 0)
	button:setTag(tag)
	button:registerScriptHandler(key_recruitBtn)
	
	btnTable[#btnTable+1] = button
	
	
	return layer,timeLabel
end

--图片做单位  + 价格
function imageMoney(image, num)

	local layer = CCLayer:create()
		local sprite = CCSprite:create(P(image));
		sprite:setAnchorPoint(PT(0,0))
		sprite:setPosition(PT(0,0))
		layer:addChild(sprite, 0)

		local numLabel = CCLabelTTF:create(num, FONT_NAME, FONT_SM_SIZE)
		numLabel:setAnchorPoint(PT(0,0))
		numLabel:setPosition(PT(sprite:getPosition().x+sprite:getContentSize().width*1.25, (sprite:getContentSize().height-numLabel:getContentSize().height)*0.5))
		layer:addChild(numLabel, 0)

		local itemSize = SZ(numLabel:getPosition().x+numLabel:getContentSize().width, sprite:getContentSize().height)

	return layer,itemSize
end



function countDown()
	local isRefresh = nil
	if timeTable then
		for k,v in ipairs(timeTable) do
			if v.time then
				v.time = v.time-1
				if v.time > 0 and v.label then
					v.label:setString(formatTime(v.time)..Language.HOTAL_FREE)
				else
					isRefresh = true
				end
			end
		end
	end
	if isRefresh then
		refresh()
	end

	delayExec(countDown, 1, nil)
end;


---延迟进行方法
function delayExec(funName,nDuration,parent)
	local  action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFuncN:create(funName));
	local layer = detailLayer
	if parent then
		layer=parent
	end
	if layer then
		layer:runAction(action)
	end	
end

--点击招募按钮
function key_recruitBtn(pNode, index)
	if isGuide then
		GuideLayer.close()
	end
	local tag = nil
	if index then
		tag = index
	else
		tag = pNode:getTag()
	end
	local recruitType = heroInfo.RecordTabel[tag].RecruitType 
	sendAction(1404, recruitType)
end

--刷新界面
function refresh()
	sendAction(1402)
end;

function sendAction(actionId, data)
	if actionId == 1402 then--1402_招募佣兵界面接口
		actionLayer.Action1402(mScene, nil)
	elseif actionId == 1404 then--        1404_佣兵邀请接口（ID=1404）
		local recruitType = data
		local IsLead  = 0
		local IsGuide,GuideId, mTaskStep = GuideLayer.getIsGuide()
		if IsGuide == 0 and GuideId == 1002 and mTaskStep == 3 then
			IsLead = 1
		end
		actionLayer.Action1404(mScene, nil, recruitType, nil, IsLead)--1十里挑一  2百里挑一   3千载难逢  4灵魂招募
	end
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 1402 then--1402_招募佣兵界面接口
		local serverInfo = actionLayer._1402Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			heroInfo = serverInfo
			PersonalInfo.setMoney(heroInfo.GameCoin, heroInfo.GoldNum)--金币，晶石
			MainMenuLayer.topShow()
			showDetail()
		end
	elseif actionId == 1404 then-- 1404_佣兵邀请接口（ID=1404）
		local serverInfo = actionLayer._1404Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then	
			newHeroInfo = serverInfo
			
			showNewHero()

		end
	end
end

--动画播放完成后  显示新佣兵
function finishAnimation(pSprite, curAniIndex, curFrameIndex, nPlayingFlag)
	if nPlayingFlag == 2 then
		pSprite:registerFrameCallback("")
		delayRemove(pSprite)
	end
end

function delayRemove(sprite)
	if sprite ~= nil then
		local delayAct = CCDelayTime:create(0.1)
		local funcName = CCCallFuncN:create(HotalLayer.removeTmpSprite)
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
    
    
    showContent()
end


function showNewHero()
	if newHeroLayer ~= nil then
		newHeroLayer:getParent():removeChild(newHeroLayer, true)
		newHeroLayer = nil
	end
	
	local layer = CCLayer:create()
	mScene:addChild(layer, 5)
	
	newHeroLayer = layer
	
	
 	creatBgImage()	
 	
 	
	local unTouchBtn =  ZyButton:new(Image.image_transparent,Image.image_transparent)
	unTouchBtn:setScaleX(pWinSize.width/unTouchBtn:getContentSize().width)
	unTouchBtn:setScaleY(pWinSize.height/unTouchBtn:getContentSize().height)
	unTouchBtn:setAnchorPoint(PT(0,0))
	unTouchBtn:setPosition(PT(0,0))
	unTouchBtn:addto(layer, 0)



	--播放动画
	local attribuAni=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite("donghua_1000")
	attribuAni:play()
	attribuAni:registerFrameCallback("HotalLayer.finishAnimation")
	layer:addChild(attribuAni, 1)
	attribuAni:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.5))    	
	
end

function showContent()
	local layer = newHeroLayer

	--按钮放前面防止后面出错，没有退出按钮
	--佣兵列表
	local heroBtn = ZyButton:new("button/list_1039.png", nil, nil, Language.BAG_HERO, FONT_NAME, FONT_SM_SIZE)
	heroBtn:setAnchorPoint(PT(1,0))
	heroBtn:setPosition(PT(pWinSize.width*0.5-heroBtn:getContentSize().width, pWinSize.height*0.2))
	heroBtn:addto(layer, 0)
	heroBtn:registerScriptHandler(gotoHeroList)
	btnTable[#btnTable+1]=heroBtn
	
	--继续
	local continueBtn = ZyButton:new("button/list_1039.png", nil, nil, Language.IDS_GOON, FONT_NAME, FONT_SM_SIZE)
	continueBtn:setAnchorPoint(PT(0,0))
	continueBtn:setPosition(PT(pWinSize.width*0.5+continueBtn:getContentSize().width, pWinSize.height*0.2))
	continueBtn:addto(layer, 0)
	continueBtn:registerScriptHandler(closeNewHero)
	btnTable[#btnTable+1]=continueBtn

	
	--佣兵大图片背景
	local bgPic = getQualityBg(newHeroInfo.GeneralQuality, 3) 	
	local imageBg = ZyButton:new(bgPic,bgPic,nil)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.05, pWinSize.height*0.35))
	imageBg:addto(layer, 0)
	imageBg:registerScriptHandler(key_hero)
	

	--佣兵图片
	if newHeroInfo.PicturesID and newHeroInfo.PicturesID ~= "" then
		local image = string.format("bigitem/%s.png", newHeroInfo.PicturesID)
		local headImage = CCSprite:create(P(image))
		headImage:setAnchorPoint(PT(0,0))
		local pos_x = imageBg:getContentSize().width*0.5-headImage:getContentSize().width*0.5
		local pos_y = imageBg:getContentSize().height*0.09
		headImage:setPosition(PT(pos_x, pos_y))
		imageBg:addChild(headImage, 0)
	end
	

	
	--佣兵名称
	if newHeroInfo.GeneralName then
		local secondLabel=CCLabelTTF:create(newHeroInfo.GeneralName,FONT_NAME,FONT_SM_SIZE)
		secondLabel:setAnchorPoint(PT(0.5,0))
		secondLabel:setPosition(PT(imageBg:getContentSize().width*0.5, imageBg:getContentSize().height*0.92))
		imageBg:addChild(secondLabel,0)
		
		local color=ZyColor:getRoleQualityColor(newHeroInfo.GeneralQuality)		
		secondLabel:setColor(color)		
	end
	
	
	--职业
	if newHeroInfo.CareerID and carrerImg[newHeroInfo.CareerID] then	
		--职业背景
		local pos_x = imageBg:getContentSize().width*0.8
		local pos_y = imageBg:getContentSize().height*0.8
		local careerBg = CCSprite:create(P("common/list_1156.png"))
		careerBg:setAnchorPoint(PT(0.5,0.5))
		careerBg:setPosition(PT(pos_x, pos_y))
		imageBg:addChild(careerBg, 0)	

		local careerImage = string.format("common/%s.png", carrerImg[newHeroInfo.CareerID])
		local careerImgSprite=CCSprite:create(P(careerImage))
		careerImgSprite:setAnchorPoint(PT(0.5,0.5))
		careerImgSprite:setPosition(PT(pos_x, pos_y))
		imageBg:addChild(careerImgSprite,0)
	end
	
	--是否灵魂碎片
	if newHeroInfo.GeneralType == 2 then
		local width = pWinSize.width*0.8
		local str = Language.ROLE_SOULNOTICE..newHeroInfo.GainNum
		str = string.format("<label>%s</label>",str)
		local noticeLabel = ZyMultiLabel:new(str, width, FONT_NAME, FONT_SM_SIZE, nil, nil)
	--	local secondLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
		noticeLabel:setPosition(PT(pWinSize.width*0.1, pWinSize.height*0.3-noticeLabel:getContentSize().height))
		noticeLabel:addto(layer, 0)
	end
	
	--文字背景
	local bgBox=SZ(pWinSize.width*0.4,imageBg:getContentSize().height)
	local labelBg = CCSprite:create(P("common/List_2009_3.9.png"))
	labelBg:setScaleX(bgBox.width/labelBg:getContentSize().width)
	labelBg:setScaleY(bgBox.height/labelBg:getContentSize().height)
	labelBg:setAnchorPoint(PT(0,0))
	labelBg:setPosition(PT(pWinSize.width*0.525,pWinSize.height*0.35))
	layer:addChild(labelBg, 0)
	
	local colW=pWinSize.width/100*24
	local rowH=nil
	local posX=pWinSize.width*0.15
	local posY=pWinSize.height*0.185
	
--[[	生命  力量 魂力
	智力	  潜力
 --]] 	
	local dateTable={Language.ROLE_LIFE,Language.ROLE_POWER,Language.ROLE_SOUL,
		Language.ROLE_INTELLECT,Language.ROLE_POTENTIAL,
	}

	local posX=labelBg:getPosition().x
	local posY=labelBg:getPosition().y+bgBox.height*0.9
	
	--
	local titleTable={
	Language.PUB_INITIAL,Language.PUB_GROW,
	}
	local col=3
	local colW=bgBox.width/3
	local rowH=bgBox.height/12
	
	for k, v in pairs(titleTable) do
		if k==1 then 
			color=ccc3(255,0,0)
		else
		    color=ccc3(19, 247, 31)
		end 
		local label=createLabel(v,layer,color)
		local labelX=k*colW
		label:setPosition(PT(posX+colW*k,posY))
		rowH = label:getContentSize().height
	end
	
	
	--初始值
	-- 生命 力量 魂技 智力 潜力
	local numDate={
		newHeroInfo.LifeNum,newHeroInfo.PowerNum,newHeroInfo.SoulNum,newHeroInfo.IntellectNum,newHeroInfo.Potential,
	}
	--成长值
	local addNum={
		newHeroInfo.AddLifeNum,newHeroInfo.AddPowerNum,newHeroInfo.AddSoulNum,newHeroInfo.AddIntellectNum,newHeroInfo.AddPotential,
	}
	
	for k , v in ipairs(dateTable) do	
	    local titleLabel=createLabel(v,layer)
	    local valueLabel=createLabel(numDate[k],layer)
	    local addLabel=createLabel(addNum[k],layer,ccc3(19, 247, 31))	
	    local pos_x=posX
	    local pos_y=posY-k*rowH
	    titleLabel:setPosition(PT(pos_x+pWinSize.width*0.025,pos_y))
	    valueLabel:setPosition(PT(pos_x+colW,pos_y))
	    addLabel:setPosition(PT(pos_x+colW*2,pos_y))
	end
	
	--天赋魂技图标
 	  
 	local skillImage = string.format("smallitem/%s.png",  newHeroInfo.HeadID) 
	local skillItem = creatCardItem(skillImage, nil, nil, nil, nil,newHeroInfo.AbilityQuality)
	skillItem:setAnchorPoint(PT(0,0))
	posY = posY-rowH*5 - skillItem:getContentSize().height
	skillItem:setPosition(PT(posX+bgBox.width*0.5-skillItem:getContentSize().width*0.5,posY))
	layer:addChild(skillItem, 0)	
	
	--天赋魂技名字
	local skillNameLabel = CCLabelTTF:create(newHeroInfo.AbilityName, FONT_NAME, FONT_SM_SIZE)
	skillNameLabel:setAnchorPoint(PT(0.5,0))
	posY = posY	-rowH
	skillNameLabel:setPosition(PT(posX+bgBox.width*0.5,posY))
	layer:addChild(skillNameLabel, 0)
	
	--魂技简介
	local list_x=labelBg:getPosition().x+pWinSize.width*0.02
	local list_y=labelBg:getPosition().y+bgBox.height*0.03	
	local listSize = SZ(pWinSize.width*0.85, posY-list_y)
	local listRowH=listSize.height/3
	
	local list = ScutCxList:node(listRowH, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(list_x, list_y))
	layer:addChild(list,0)
	list:setTouchEnabled(true)
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val =0
	layout.val_y.val.pixel_val =0
	
	local listItem = ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
	listItem:setOpacity(0)
	local skillLayer=CCLayer:create()
	local skillStr = string.format("<label >%s</label>",newHeroInfo.AbilityDesc)

	local width = pWinSize.width*0.35
	local label = ZyMultiLabel:new(skillStr, width, FONT_NAME, FONT_SM_SIZE, nil, nil)
	label:setPosition(PT(0,0))
	listItem:addChildItem(skillLayer, layout)
	list:setRowHeight(label:getContentSize().height+SY(1))
	list:addListItem(listItem, false)
	label:addto(skillLayer, 0)
	
	
	if GuideLayer.judgeIsGuide(14) then
		isGuide = true
		GuideLayer.setScene(mScene)	
		GuideLayer.init()
	else
		isGuide = nil
	end
end



--创建技能卡片   小
function creatCardItem(image, name, level, tag, menberCallBack,quality)
	local bgImg = getQualityBg(quality,1)
	local menuItem = CCMenuItemImage:create(P(bgImg), P(bgImg))
	local btn = CCMenu:createWithItem(menuItem)
	menuItem:setAnchorPoint(PT(0,0))
	
	--响应函数
	if menberCallBack then
		menuItem:registerScriptHandler(menberCallBack)
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

	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))
	return  btn
end;

function  createLabel(name,parent,color)
	local label=CCLabelTTF:create(name,FONT_NAME,FONT_SM_SIZE)
	label:setAnchorPoint(PT(0,0))
	if color~=nil then
		label:setColor(color)
	end
	parent:addChild(label,0)
	return label
end;

--通用底图
function creatBgImage()
	local m_bgImage= CCSprite:create(P(Image.image_mainscene));
	m_bgImage:setScaleX(pWinSize.width/m_bgImage:getContentSize().width)
	m_bgImage:setScaleY(pWinSize.height/m_bgImage:getContentSize().height)
	m_bgImage:setAnchorPoint(PT(0.5,0.5))
	m_bgImage:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	newHeroLayer:addChild(m_bgImage,0)
end;

--跳转到佣兵列表
function gotoHeroList()
	if newHeroLayer ~= nil then
		newHeroLayer:getParent():removeChild(newHeroLayer, true)
		newHeroLayer = nil
	end
	RoleBagScene.pushScene()
	MainMenuLayer.setCurrentScene(101)
end

--关闭新佣兵展示界面
function closeNewHero()
	if newHeroLayer ~= nil then
		newHeroLayer:getParent():removeChild(newHeroLayer, true)
		newHeroLayer = nil
	end
	refresh()
end;

