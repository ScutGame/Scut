------------------------------------------------------------------
-- showHeroDetailLayer.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 佣兵详情界面
------------------------------------------------------------------

module("showHeroDetailLayer", package.seeall)


mScene = nil 		-- 场景

function close()
	if headLayer then
		headLayer:getParent():removeChild(headLayer, true)
		headLayer = nil
	end
 	releaseDataLayer()
	if mLayer then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end

end;

function setData(info, fatherType, scene)
	mDataTabel = info
	mFatherType = fatherType--1 佣兵阵营   2佣兵列表
	mScene = scene
end

function initResource()

end;
---接入佣兵详细信息
function init(father)
	local index = nil
	mIndex = index	
	if  father.Scene  then 
		index = father.index
		father  = father.Scene 
	end
	mLayer = CCLayer:create()
	father:addChild(mLayer, 5)
	
	--屏蔽按钮 
	local unTouchBtn =  ZyButton:new(Image.image_transparent)
	unTouchBtn:setScaleX(pWinSize.width/unTouchBtn:getContentSize().width)
	unTouchBtn:setScaleY(pWinSize.height/unTouchBtn:getContentSize().height)
	unTouchBtn:setAnchorPoint(PT(0,0))
	unTouchBtn:setPosition(PT(0,0))
	unTouchBtn:addto(mLayer, 0)

	--创建背景
	local bgLayer = UIHelper.createUIBg("title/list_1113.png", nil, ZyColor:colorYellow(), nil,true)
	mLayer:addChild(bgLayer,0)	

	
	
	btnTable = {
		Language.INHERIT_TITLE,
		Language.TRAIN_TITLE,
		Language.IDS_BACK,
	}
	for i=1,3 do
		local button = ZyButton:new(Image.image_button, nil, nil, btnTable[i], FONT_NAME,FONT_SM_SIZE)
		button:setAnchorPoint(PT(0,0))
		local pos_x = pWinSize.width*0.75-pWinSize.width*0.25*(3-i)-button:getContentSize().width*0.5
		local pos_y = pWinSize.height*0.1
		button:setPosition(PT(pos_x, pos_y))	
		button:registerScriptHandler(key_button)
		--button:setColorNormal(ZyColor:colorYellow())
		button:setTag(i)
		button:addto(mLayer, 0)	
		if   index == 1    then
			button:setVisible(false)
					--关闭按钮 
					if i == 3 then
						local name=Language.IDS_BACK
						local fun=key_button
						local popBtn=ZyButton:new("button/list_1023.png",nil,nil,name)
						popBtn:setAnchorPoint(PT(0,0))
						popBtn:setTag(i)
						popBtn:setPosition(PT(pWinSize.width*0.5-popBtn:getContentSize().width/2,pos_y))
						if  pWinSize.height == 800  then
							popBtn:setPosition(PT(pWinSize.width*0.5-popBtn:getContentSize().width/2,pWinSize.height*0.12))
						end
						popBtn:addto(mLayer,0)
						popBtn:registerScriptHandler(fun)
					end
		end
	end

	
	
	headImage()
	
	refreshPlayerDate()
end;

--佣兵头像
function headImage()
	if headLayer then
		headLayer:getParent():removeChild(headLayer, true)
		headLayer = nil
	end

	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0, pWinSize.height*0.45))
	
	mLayer:addChild(layer, 0)
	headLayer = layer
	

	
	
	--人物背景
	local bgPic = getQualityBg(mDataTabel.GeneralQuality, 3) 		
	local imageBg = CCSprite:create(P(bgPic))
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.05,0))
	layer:addChild(imageBg, 0)

	local image = string.format("bigitem/%s.png", mDataTabel.PicturesID)
	local headImage = CCSprite:create(P(image))
	headImage:setAnchorPoint(PT(0,0))
	local pos_x = imageBg:getContentSize().width*0.5-headImage:getContentSize().width*0.5
	local pos_y = imageBg:getContentSize().height*0.09
	headImage:setPosition(PT(pos_x, pos_y))
	imageBg:addChild(headImage, 0)
	
	--品质
	local quality=genrealQuality[mDataTabel.GeneralQuality]
	if quality then
		local qualityLabel = CCLabelTTF:create(quality, FONT_NAME, FONT_SM_SIZE)
		qualityLabel:setAnchorPoint(PT(0,1))
		qualityLabel:setPosition(PT(imageBg:getContentSize().width*0.2, imageBg:getContentSize().height*0.99))
		imageBg:addChild(qualityLabel, 0)
	end
	
	--等级
	local level = mDataTabel.GeneralLv..Language.IDS_LEV 
	if level then
		local levelLabel = CCLabelTTF:create(level, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(1,1))
		levelLabel:setPosition(PT(imageBg:getContentSize().width*0.75, imageBg:getContentSize().height*0.99))
		imageBg:addChild(levelLabel, 0)	
	end
	
	
	
	if mDataTabel.CareerID and carrerImg[mDataTabel.CareerID] then	
		--职业背景
		local pos_x = imageBg:getContentSize().width*0.8
		local pos_y = imageBg:getContentSize().height*0.8
		local careerBg = CCSprite:create(P("common/list_1156.png"))
		careerBg:setAnchorPoint(PT(0.5,0.5))
		careerBg:setPosition(PT(pos_x, pos_y))
		imageBg:addChild(careerBg, 0)	

	--职业
		local careerImage = string.format("common/%s.png", carrerImg[mDataTabel.CareerID])
	
		local careerImgSprite=CCSprite:create(P(careerImage))
		careerImgSprite:setAnchorPoint(PT(0.5,0.5))
		careerImgSprite:setPosition(PT(pos_x, pos_y))
		imageBg:addChild(careerImgSprite,0)
	end
	
	
	--文字背景
	local labelBg = CCSprite:create(P("common/List_2009_3.9.png"))
	labelBg:setScaleX(pWinSize.width*0.4/labelBg:getContentSize().width)
	labelBg:setScaleY(imageBg:getContentSize().height/labelBg:getContentSize().height)
	labelBg:setAnchorPoint(PT(0,0))
	labelBg:setPosition(PT(pWinSize.width*0.525,0))
	layer:addChild(labelBg, 0)

	local labelStr = string.format("<label >%s</label>", Language.BAG_DES..":")
	if mDataTabel.GeneralDesc then
		labelStr = labelStr..string.format("<label >%s</label>", mDataTabel.GeneralDesc)
	else
		labelStr = labelStr..string.format("<label >%s</label>", Language.IDS_NONE)
	end
	local width = pWinSize.width*0.35
	local label = ZyMultiLabel:new(labelStr, width, FONT_NAME, FONT_SM_SIZE, nil, nil)
	label:setPosition(PT(pWinSize.width*0.025, imageBg:getContentSize().height*0.95-label:getContentSize().height))
	label:addto(labelBg, 0)

	--天赋魂技文字
	local levelLabel = CCLabelTTF:create(Language.PUB_ABILITY..":", FONT_NAME, FONT_SM_SIZE)
	levelLabel:setAnchorPoint(PT(0,0))
	levelLabel:setPosition(PT(pWinSize.width*0.025, pWinSize.height*0.18))
	labelBg:addChild(levelLabel, 0)
	
	--天赋魂技图标
	local skillImage = nil
	if mDataTabel.Skill and mDataTabel.Skill[1]then
		skillImage = string.format("smallitem/%s.png", mDataTabel.Skill[1].AbilityHead) 
	end
	local menberCallBack = showSkillDetail
	
	if mDataTabel.Skill and #mDataTabel.Skill>0 then
	
	local skillItem = creatCardItem(skillImage, nil, nil, nil, menberCallBack,mDataTabel.Skill[1].AbilityQuality)
	skillItem:setAnchorPoint(PT(0,0))
	skillItem:setPosition(PT(imageBg:getContentSize().width*0.5-skillItem:getContentSize().width*0.5, pWinSize.height*0.05))
	labelBg:addChild(skillItem, 0)	
	
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

--魂技详情
function showSkillDetail()
	close()
	local Data = {}
	if mIndex ~= 1 and  mDataTabel.Skill then
		Data.currentGeneralID=mDataTabel.GeneralID
		Data.UserItemID = mDataTabel.Skill[1].UserItemID	
		Data.Position=1
	end
	if  mDataTabel.Skill then
		actionLayer.Action1485(mScene,nil, mDataTabel.Skill[1].UserItemID, Data)
	else
		actionLayer.Action1611(mScene,nil,3,mDataTabel.RecordTabel2[1].UserItemID)
	end
end;



function  releaseDataLayer()
	if dataLayer~=nil then
		dataLayer:getParent():removeChild(dataLayer,true)
		dataLayer=nil
	end
end;

function  refreshPlayerDate()
	releaseDataLayer()
	dataLayer=CCLayer:create()
	dataLayer:setAnchorPoint(PT(0,0))
	dataLayer:setPosition(PT(0, pWinSize.height*0.2))	
	
	
	mLayer:addChild(dataLayer,0)
	
	
	--文字背景
	local labelBg = CCSprite:create(P("common/List_2009_3.9.png"))
	labelBg:setScaleX(pWinSize.width*0.85/labelBg:getContentSize().width)
	labelBg:setScaleY(pWinSize.height*0.2/labelBg:getContentSize().height)
	labelBg:setAnchorPoint(PT(0,0))
	labelBg:setPosition(PT(pWinSize.width*0.075,0))
	dataLayer:addChild(labelBg, 0)	
	
	
	
	local colW=pWinSize.width*0.8
	local rowH=nil
	
	--缘分系统
	local listSize = SZ(pWinSize.width*0.85, pWinSize.height*0.18)
	local list_x=labelBg:getPosition().x+pWinSize.width*0.02
	local list_y=labelBg:getPosition().y+pWinSize.height*0.01
	local listRowH=listSize.height/5
	
	local list = ScutCxList:node(listRowH, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(list_x, list_y))
	dataLayer:addChild(list,0)
	
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val =0
	layout.val_y.val.pixel_val =0

	local YFInforTabel=mDataTabel.YFInforTabel
	for k,v in ipairs(YFInforTabel) do
		local listItem = ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(0)
		local layer=CCLayer:create()
		
		local str=v.KarmaName..":"..v.KarmaDesc
		local color
		if v.IsActive==0 then 
			 color=ccc3(120,120,120)
		else
			color=ccc3(255,255,255)
		end 
		local label=createRowLabel(str,layer,color,pWinSize.width*0.8)
		label:setPosition(PT(0,0))
		listItem:addChildItem(layer, layout)
		list:setRowHeight(label:getContentSize().height+SY(1))
		list:addListItem(listItem, false)
	end
end;



--

function  createRowLabel(content,parent,color,width)
	if not color then
		color=ccc3(255,255,255)
	end

	local contentStr=string.format("<label color='%d,%d,%d'>%s</label>",
							color.r,color.g,color.b,content)
	local label= ZyMultiLabel:new(contentStr,width,FONT_NAME,FONT_SM_SIZE)
	label:addto(parent,0)
	return label
end;

function  percentStr(num)
	if num~=0 then
	return num*100 ..  "%"
	end
	return num
end;

function  getDataNum(id)
	for k , v in pairs(mDataTabel.RecordTabel4) do
		if v.SkillID ==id then
			return v.SkillNum
		end
	end
	return 0
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


function key_button(pNode)
	local index = pNode:getTag()
	if index == 1 then--传承
		close()
		if mFatherType == 1 then
			HeroScene.gotoInherit()
		else
			RoleBagScene.gotoInherit()
		end
	elseif index == 2 then--培养
		close()
		if mFatherType == 1 then
			HeroScene.gotoTrain()
		elseif mFatherType == 2 then
			RoleBagScene.gotoTrain()
		end
	elseif index == 3 then--退出
		close()
	end
end








