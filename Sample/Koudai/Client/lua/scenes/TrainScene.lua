------------------------------------------------------------------
-- TrainScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("TrainScene", package.seeall)


local mScene = nil 		-- 场景
local mGeneralInfo=nil
local personalInfo=nil
local trainLayer=nil
local mTabBar=nil
local mCharLayer=nil
local choiceLayer=nil
local buttonTabel=nil
local mChangeLayer=nil
local isChange=nil
local mGeneralID=nil

--
local trianType=nil
local trainLabel=nil
local trianTable={
{num=1},
{num=2},
{num=5},
{num=10}}
local mChoiceTabel=nil


-- 场景入口
function pushScene()
	initResource()
	createScene()
end


function  initScene(scene)
	if mScene then
		return 
	end
	mScene=scene
	initResource()
	createScene()
end;


function setGeneralInfo(info,id)
	mGeneralInfo=info
	mGeneralID=id
end;

function setTrainTabel(info, fatherType)
	mChoiceTabel=info.RecordTabel
	mGeneralInfo.ItemNum=info.ItemNum
	mFatherType = fatherType
end;
-- 退出场景
function popScene()
	releaseResource()
	
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	personalInfo=PersonalInfo.getPersonalInfo()
	if not trianType then	
		trianType=1
	end
end

-- 释放资源
function releaseResource()
	trainLayer = nil
	mLayer = nil
	mScene=nil
	mCharLayer=nil
	buttonTabel=nil
	mGeneralInfo=nil
	mChangeLayer=nil
	isChange=nil
	_setTimeBtn=nil
end

--退出培养界面
function  closeScene()
	 releaseChangeLayer()
	releaseContentLayer()
	if isChange then
		actionLayer.Action1403(mScene, nil, mGeneralID )
	end
	if mLayer ~= nil then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end
	releaseResource()
	
	if mFatherType == 1 then--1佣兵界面  2佣兵列表
		HeroScene.setBgIsVisible(false)
		MainMenuLayer.setIsShow(false, false)
		HeroScene.refreshWin()
	else
		MainMenuLayer.setIsShow(true, false)
	end
end;

-- 创建场景
function createScene()
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	
	--创建背景
	local bgLayer=createBgLayer()
	bgLayer:setPosition(PT(0,0))
	mLayer:addChild(bgLayer,0)
	
	local tabStr={Language.ROLE_TRAIN}
	mTabBar=createTabbar(tabStr,mLayer)	
	MainMenuLayer.setIsShow(false, true)
	if mFatherType == 1 then
		HeroScene.setBgIsVisible(true)
	elseif mFatherType == 2 then
	
	end
	
	trainContent()
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


--创建背景层
function  createBgLayer()
	local layer=CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	--大背景
	local titleBg=CCSprite:create(P("common/list_1047.png"))
	local bgSprite=CCSprite:create(P("common/list_1015.png"))
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
--	layer:addChild(bgSprite,0)
	
	--中间层
	local path="common/list_1043.png"
	local midSprite=CCSprite:create(P(path))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.78)
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)
	local posY=pWinSize.height*0.145
	midSprite:setPosition(PT(pWinSize.width/2,posY))
	bgSprite:setPosition(PT(pWinSize.width/2,posY))
	layer:addChild(midSprite,0)
	
	--
	local btnSize=SZ(pWinSize.width,pWinSize.height*0.855)
	local actionBtn=UIHelper.createActionRect(btnSize)
	actionBtn:setPosition(PT(0,pWinSize.height-btnSize.height))
	layer:addChild(actionBtn,0)
	return layer
	
end;

function  createTabbar(tabStr,layer,posY)
	local tabBar=nil
	if tabStr then
	  	local titleSprite=CCSprite:create(P("common/list_1041.png"))
		titleSprite:setAnchorPoint(PT(0,0))
		tabBar=titleSprite
		local tabBar_X=pWinSize.width*0.04
		local imageBg = CCSprite:create(P("common/list_1047.png"));
		local tabBar_Y=pWinSize.height*0.855
		if posY then
			tabBar_Y=posY-tabBar:getContentSize().height-SY(4)
		end
		titleSprite:setPosition(PT(tabBar_X,tabBar_Y))
--		layer:addChild(titleSprite,1)
		--
		local label=CCLabelTTF:create(tabStr[1],FONT_NAME,FONT_DEF_SIZE)
		label:setAnchorPoint(PT(0.5,0.5))
		label:setPosition(PT(tabBar_X+titleSprite:getContentSize().width/2,
								tabBar_Y+titleSprite:getContentSize().height/2))
--		layer:addChild(label,1)
	end
	return tabBar
end;

--顶部缩略内容
function topMiniInfo()
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	mLayer:addChild(layer, 0)
	local imageBg = CCSprite:create(P("common/list_1047.png"));
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0, 0))
	imageBg:setScaleX(pWinSize.width/imageBg:getContentSize().width)
	layer:addChild(imageBg, 0)
	local miniContent_height = imageBg:getContentSize().height
	layer:setPosition(PT(0,  pWinSize.height-miniContent_height))
	--姓名背景
	local startX = pWinSize.width*0.015
	local nameBg = CCSprite:create(P("mainUI/list_1001_2.png"));
	nameBg:setAnchorPoint(PT(0,0))
	local height = miniContent_height*0.5-nameBg:getContentSize().height*0.5
	nameBg:setPosition(PT(startX, height))
	layer:addChild(nameBg, 0)
	if personalInfo._NickName then
		local nameLabel = CCLabelTTF:create(personalInfo._NickName, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0,0))
		nameLabel:setPosition(PT(nameBg:getPosition().x+(nameBg:getContentSize().width-nameLabel:getContentSize().width)*0.5, nameBg:getPosition().y+(nameBg:getContentSize().height-nameLabel:getContentSize().height)*0.5))
		layer:addChild(nameLabel, 0)
	end
	--等级背景
	local levelBg = CCSprite:create(P("mainUI/list_1001_1.png"));
	levelBg:setAnchorPoint(PT(0,0))
	levelBg:setPosition(PT(nameBg:getPosition().x+nameBg:getContentSize().width*0.9, height))
	layer:addChild(levelBg, 0)
	if personalInfo._UserLv then
		local levelStr = Language.ROLE_LEVEL..":"..personalInfo._UserLv
		local levelLabel = CCLabelTTF:create(levelStr, FONT_NAME, FONT_SM_SIZE)
		levelLabel:setAnchorPoint(PT(0,0))
		levelLabel:setPosition(PT(levelBg:getPosition().x+levelBg:getContentSize().width*0.25, levelBg:getPosition().y+(levelBg:getContentSize().height-levelLabel:getContentSize().height)*0.5))
		layer:addChild(levelLabel, 0)
	end

	--晶石背景
	local goldBg = CCSprite:create(P("mainUI/list_1051.png"));
	goldBg:setAnchorPoint(PT(0,0))
	goldBg:setPosition(PT(pWinSize.width*0.6, height))
	layer:addChild(goldBg, 0)
		local item,itemSize = imageMoney("mainUI/list_1006.png", personalInfo._GoldNum)
		item:setPosition(PT(goldBg:getPosition().x+goldBg:getContentSize().width*0.1, goldBg:getPosition().y+(goldBg:getContentSize().height-itemSize.height)*0.5))
		layer:addChild(item, 0)
	--金币背景
	local item,itemSize = imageMoney("mainUI/list_1007.png", personalInfo._GameCoin)
	item:setPosition(PT(goldBg:getPosition().x+pWinSize.width*0.2, goldBg:getPosition().y+(goldBg:getContentSize().height-itemSize.height)*0.5))
	layer:addChild(item, 0)
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

--培养界面
function  releaseContentLayer()
	 relaseShowLayer()
	if trainLayer ~= nil then
		trainLayer:getParent():removeChild(trainLayer, true)
		trainLayer = nil
	end
end;


function trainContent()
 	releaseContentLayer()
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0, 0))
	mLayer:addChild(layer, 0)
	trainLayer = layer
	--说明
	local contentStr=string.format("<label>%s</label>", Language.TRAIN_NOTICE)
	local noticLabel=ZyMultiLabel:new(contentStr,pWinSize.width*0.8,FONT_NAME,FONT_DEF_SIZE);
	noticLabel:setAnchorPoint(PT(0,0))
	noticLabel:setPosition(PT(pWinSize.width*0.1,
							pWinSize.height*0.855-noticLabel:getContentSize().height-SY(2)))
	noticLabel:addto(layer,0)	
	--头像
	local image=nil
	if mGeneralInfo.BattleHeadID and mGeneralInfo.BattleHeadID ~= "" then
		image = string.format("battleHead/%s.png",mGeneralInfo.BattleHeadID)
	end
	local item = creatItem(image)
	item:setAnchorPoint(PT(0,0))
	local pos_x = pWinSize.width*0.1
	local pos_y =noticLabel:getPosition().y-item:getContentSize().height-SY(5)
	item:setPosition(PT(pos_x, pos_y))
	layer:addChild(item, 0)
	
	--昵称
	local nameLabel=CCLabelTTF:create(mGeneralInfo.GeneralName,FONT_NAME,FONT_DEF_SIZE)
	nameLabel:setAnchorPoint(PT(0,0))
	nameLabel:setPosition(PT(pos_x+item:getContentSize().width+SX(2),
							pos_y+item:getContentSize().height*0.7	))
	layer:addChild(nameLabel, 0)
	--------品质
	--1	白色2	绿色3	蓝色4	紫色
	local str=Language.EQUIP_PIN .. ":"
	if genrealQuality[mGeneralInfo.GeneralQuality] then
		str=str .. genrealQuality[mGeneralInfo.GeneralQuality]
	end
	local qulityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
	qulityLabel:setAnchorPoint(PT(0,0))
	qulityLabel:setPosition(PT(nameLabel:getPosition().x,
							nameLabel:getPosition().y-nameLabel:getContentSize().height*1.5	))
	layer:addChild(qulityLabel, 0)
	
	---等级
	local levelStr=Language.IDS_LEVEL .. ":" .. (mGeneralInfo.GeneralLv or 0)
	local levelLabel=CCLabelTTF:create(levelStr,FONT_NAME,FONT_DEF_SIZE)
	levelLabel:setAnchorPoint(PT(0,0))
	levelLabel:setPosition(PT(qulityLabel:getPosition().x,
							qulityLabel:getPosition().y-qulityLabel:getContentSize().height*1.5	))
	layer:addChild(levelLabel, 0)


	--显示文字
	showLabel(layer)
	--潜能点
	pos_x =item:getPosition().x+item:getContentSize().width/2
	pos_y =item:getPosition().y-SY(10)
	local addNumStr=Language.TRAIN_COUNT .. ":" .. (mGeneralInfo.Potential or 0)
	local addNumLabel=CCLabelTTF:create(addNumStr,FONT_NAME,FONT_DEF_SIZE)
	addNumLabel:setAnchorPoint(PT(0,0))
	addNumLabel:setPosition(PT(pos_x,pos_y-addNumLabel:getContentSize().height	))
	layer:addChild(addNumLabel, 0)
	pos_x=pWinSize.width/2

	local boxNumStr=Language.TRAIN_DRUG .. ":" .. (mGeneralInfo.ItemNum or 0)
	local boxNumLabel=CCLabelTTF:create(boxNumStr,FONT_NAME,FONT_DEF_SIZE)
	boxNumLabel:setAnchorPoint(PT(0,0))
	boxNumLabel:setPosition(PT(pos_x,addNumLabel:getPosition().y	))
	layer:addChild(boxNumLabel, 0)
	
--]]

	--培养选择
	pos_y=boxNumLabel:getPosition().y-SY(5)
	choiceBox(pos_y)	
	--退出按钮
	local exitBtn =ZyButton:new(Image.image_button_red_c_0, Image.image_button_red_c_1, nil, Language.TRAIN_EXIT, FONT_NAME, FONT_SM_SIZE)
	exitBtn:setColorNormal(ZyColor:colorYellow())
	exitBtn:setAnchorPoint(PT(0,0))
	exitBtn:setPosition(PT(pWinSize.width*0.4-exitBtn:getContentSize().width, 
										pWinSize.height*0.22))
	exitBtn:registerScriptHandler(closeScene)
	exitBtn:addto(layer, 0)

	--培养按钮
	local trainBtn =ZyButton:new(Image.image_button_red_c_0, Image.image_button_red_c_1, nil, Language.TRAIN_TITLE, FONT_NAME, FONT_SM_SIZE)
	trainBtn:setColorNormal(ZyColor:colorYellow())
	trainBtn:setAnchorPoint(PT(0,0))
	trainBtn:setPosition(PT(pWinSize.width*0.6, exitBtn:getPosition().y))
	trainBtn:registerScriptHandler(trainAction)
	trainBtn:addto(layer, 0)	
end;

--显示文字
function  relaseShowLayer()
	if mCharLayer then
		mCharLayer:getParent():removeChild(mCharLayer,true)
		mCharLayer=nil
	end
end;

function showLabel()
	--姓名
	local pos_x = pWinSize.width*0.57
	local pos_y =  pWinSize.height*0.73
	 relaseShowLayer()
	local layer=CCLayer:create()
	mCharLayer=layer
	trainLayer:addChild(layer,0)	
---------------------------
	local charTable={
	[1]={name=mGeneralInfo.PowerNum,image="mainUI/list_3037.png"},
	[2]={name=mGeneralInfo.IntellectNum,image="mainUI/list_3038.png"},
	[3]={name=mGeneralInfo.SoulNum,image="mainUI/list_3039.png"},
	}
	local col=1
	local colW=pWinSize.width/5
	local rowH=pWinSize.height*0.06
	for k, v in pairs(charTable) do
		local posX=pos_x+(k-1)%col*colW
		local posY=pos_y-math.floor((k-1)/col)*rowH
		creatImageLabel(layer,v.image, v.name,nil,posX,posY,k)
	end

end
--创建文字
function creatImageLabel(parent,image, content,color,posX,posY,type)
	local titleImage=nil
	local startX=posX
	local startY=posY
	if image then
		titleImage=CCSprite:create(P(image))
		titleImage:setAnchorPoint(PT(0.5,0))
		titleImage:setPosition(PT(posX,posY))
		parent:addChild(titleImage,0)
	end
	
	if titleImage then
		startX=posX+titleImage:getContentSize().width
		startY=posY+titleImage:getContentSize().height/2
	end
	if content then
		local contentLabel=CCLabelTTF:create(content,FONT_NAME,FONT_DEF_SIZE)
		contentLabel:setAnchorPoint(PT(0,0.5))
	--	local color = ZyColor:colorYellow()
		if color~=nil then
			contentLabel:setColor(color)
		end
		if type==3 then
			startX=startX-SX(13)
		end
		contentLabel:setPosition(PT(startX, startY))
		parent:addChild(contentLabel,0)
	end
	
end

--创建头像
function creatItem(image, name)
	local menuItem = CCMenuItemImage:create(P("common/list_1032.png"), P("common/list_1032.png"))
	local btn = CCMenu:createWithItem(menuItem)
	menuItem:setAnchorPoint(PT(0,0))
--	menuItem:registerScriptTapHandler(menberCallBack)
	if tag then
		menuItem:setTag(tag)
	end
	btn:setContentSize(SZ(menuItem:getContentSize().width, menuItem:getContentSize().height))
	-- 商品图片
	if image then
		local sprite=CCSprite:create(P(image))
		sprite:setAnchorPoint(CCPoint(0.5, 0.5))
		sprite:setPosition(PT(menuItem:getContentSize().width*0.5, menuItem:getContentSize().height*0.55))
		menuItem:addChild(sprite,0)
	end
	--名称
	if name then
		local nameLabel = CCLabelTTF:create(name, FONT_NAME, FONT_SM_SIZE)
		nameLabel:setAnchorPoint(PT(0.5, 0))
		nameLabel:setPosition(PT(menuItem:getContentSize().width*0.5, nameLabel:getContentSize().heigth*1.2))
		menuItem:addChild(nameLabel, 0)
	end
	return  btn
end;


--选择界面
function releaseChoiceBox()
	if choiceLayer ~= nil then
		choiceLayer:getParent():removeChild(choiceLayer, true)
		choiceLayer = nil
	end
end;

--创建选择框
function choiceBox(poxY)
	releaseChoiceBox()
	local choiceBox=SZ( pWinSize.width*0.85,pWinSize.width*0.28)
	local choiceBox_x = (pWinSize.width-choiceBox.width)/2
	local choiceBox_y = poxY-choiceBox.height
	
	local layer = CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(choiceBox_x, choiceBox_y))
	trainLayer:addChild(layer, 0)
	--背景图
	local imageBg = CCSprite:create(P("common/list_1038.9.png"))
	imageBg:setScaleX(choiceBox.width/imageBg:getContentSize().width)
	imageBg:setScaleY(choiceBox.height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0,0))
	layer:addChild(imageBg, 0)
	local strTable={Language.ROLE_TIP1,Language.ROLE_TIP2,Language.ROLE_TIP3,Language.ROLE_TIP4}
	local useType={[1]=Language.ROLE_TIP10,[2]=Language.ROLE_TIP9}
	local colW=choiceBox.width*0.5
	local rowH=choiceBox.height*0.25
	local startX=choiceBox.width*0.05
	local startY=choiceBox.height*0.95-rowH
	local col=2
	if  not m_choiceType  then
		m_choiceType = 1
	end
	
	local num = 4
	if not isShowVip() then
		if PersonalInfo.getPersonalInfo()._VipLv < 5 then
			num = 3
		end
		if PersonalInfo.getPersonalInfo()._VipLv < 5 then
			num = 2
		end
	end
	
	for i=1,num do
		local v = mChoiceTabel[i]
		local k = i
		local choiceBtn=ZyButton:new(Image.image_button_hook_0,Image.image_button_hook_1)
		choiceBtn:setAnchorPoint(PT(0,0))
		local posX=startX+(k-1)%col*colW
		local posY=startY-math.floor((k-1)/col)*rowH
		choiceBtn:setPosition(PT(posX, posY))
		choiceBtn:addto(layer, 0)
		choiceBtn:setTag(k)
		choiceBtn:registerScriptHandler(typeChoiceAction)
		if k==m_choiceType then
			choiceBtn:selected()
		end
		local str =strTable[v.BringUpType] ..string.format(useType[v.UseUpType],v.UseUpNum)
		local label = CCLabelTTF:create(str, FONT_NAME, FONT_SM_SIZE)
		label:setAnchorPoint(PT(0,0.5))
		label:setPosition(PT(choiceBtn:getPosition().x+choiceBtn:getContentSize().width*1.2, 
						choiceBtn:getPosition().y+choiceBtn:getContentSize().height*0.5))
		layer:addChild(label, 0)	
		v.choiceBtn=choiceBtn
	end
	
	local setTimeStr = string.format(Language.TRAIN_TIME, trianTable[trianType].num)
	local setTimeBtn =ZyButton:new(Image.image_button_red_c_0, Image.image_button_red_c_1, nil,
						setTimeStr, FONT_NAME, FONT_SMM_SIZE)
	setTimeBtn:setColorNormal(ZyColor:colorYellow())
	setTimeBtn:setAnchorPoint(PT(0,0))
	setTimeBtn:setPosition(PT(choiceBox.width*0.5-setTimeBtn:getContentSize().width*0.5, 
								choiceBox.height*0.05))
	setTimeBtn:registerScriptHandler(createTimeLayer)
	setTimeBtn:addto(layer, 0)
	
	_setTimeBtn = setTimeBtn
end

--
function  releaseTimeLayer()
	if mTimeLayer then
		mTimeLayer:getParent():removeChild(mTimeLayer,true)
		mTimeLayer=nil
	end
end;

--设置次数
function createTimeLayer()
	releaseTimeLayer()
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	mTimeLayer = layer
	
	for k=1 ,2 do
		local pingBiBtn=UIHelper.createActionRect(pWinSize)
		pingBiBtn:setPosition(PT(0,0))
		layer:addChild(pingBiBtn,0)
	end
	
	local blackSprite=CCSprite:create(P("common/transparentBg.png"))
	blackSprite:setScaleX(pWinSize.width/blackSprite:getContentSize().width)
	blackSprite:setScaleY(pWinSize.height/blackSprite:getContentSize().height)
	blackSprite:setAnchorPoint(PT(0.5,0.5))
	blackSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(blackSprite,0)
	
	
	local midSprite=CCSprite:create(P("common/list_1054.png"))
	local bgSize=midSprite:getContentSize()
	midSprite:setAnchorPoint(PT(0.5,0.5))
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(midSprite,0)
	
	local boxSize=SZ(bgSize.width,bgSize.height*0.8)
	local btnBg=CCSprite:create(P("common/list_1052.9.png"))
	btnBg:setScaleY(boxSize.height/btnBg:getContentSize().height)
	btnBg:setAnchorPoint(PT(0,0.5))
	btnBg:setPosition(PT((pWinSize.width-btnBg:getContentSize().width)/2,pWinSize.height/2-bgSize.height*0.08))
	layer:addChild(btnBg,0)
	
	local startX=pWinSize.width/2-bgSize.width*0.4
	local startY=pWinSize.height/2+bgSize.height*0.3
	
	--标题
	local titleTip=CCLabelTTF:create(Language.ROLE_TIP5,FONT_NAME,FONT_DEF_SIZE)
	titleTip:setAnchorPoint(PT(0.5,0))
	titleTip:setPosition(PT(pWinSize.width/2,startY-titleTip:getContentSize().height*1.5))
	layer:addChild(titleTip,0)	
	
	
	startY=titleTip:getPosition().y-titleTip:getContentSize().height
	local str=string.format(Language.ROLE_TIP6,trianTable[trianType].num)	
	local titleTip1=CCLabelTTF:create(str ,FONT_NAME,FONT_DEF_SIZE)
	titleTip1:setAnchorPoint(PT(0,0))
	titleTip1:setPosition(PT(startX,startY-titleTip1:getContentSize().height*1.5))
	layer:addChild(titleTip1,0)	
	trainLabel=titleTip1
	local colW=bgSize.width/5
	local startX=midSprite:getPosition().x-midSprite:getContentSize().width*0.5+bgSize.width*0.14
	local posY=titleTip1:getPosition().y-titleTip1:getContentSize().height*3
	for k, v in pairs(trianTable) do
		local choiceBtn=ZyButton:new(Image.image_button_hook_0,Image.image_button_hook_1)
		choiceBtn:setAnchorPoint(PT(0,0))
		local posX=startX+(k-1)*colW
		choiceBtn:setPosition(PT(posX, posY))
		choiceBtn:addto(layer, 0)
		choiceBtn:setTag(k)
		choiceBtn:registerScriptHandler(trainTpyeChoice)
		if k==trianType then
			choiceBtn:selected()
		end
		local str =v.num .. Language.IDS_TIMES
		local label = CCLabelTTF:create(str, FONT_NAME, FONT_SM_SIZE)
		label:setAnchorPoint(PT(0,0.5))
		label:setPosition(PT(choiceBtn:getPosition().x+choiceBtn:getContentSize().width*1.2, 
						choiceBtn:getPosition().y+choiceBtn:getContentSize().height*0.5))
		layer:addChild(label, 0)	
		v.choiceBtn=choiceBtn
	end
	
	--取消 确定按钮
	local sureBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.IDS_SURE)
	sureBtn:setAnchorPoint(PT(0,0))
	sureBtn:setPosition(PT(pWinSize.width/2-sureBtn:getContentSize().width/2,
							pWinSize.height/2-bgSize.height*0.4))
	sureBtn:addto(layer,0)
	sureBtn:registerScriptHandler(releaseTimeLayer)	
end;

function trainTpyeChoice(pNode)
	local tag = pNode:getTag()
	for k,v in ipairs(trianTable) do
		if k ~= tag then
			v.choiceBtn:unselected()
		else
			v.choiceBtn:selected()
			trianType = tag
		end
	end
	local str=string.format(Language.ROLE_TIP6,trianTable[trianType].num)	
	trainLabel:setString(str)
	
	local setTimeStr = string.format(Language.TRAIN_TIME, trianTable[trianType].num)
	_setTimeBtn:setString(setTimeStr)
end;

--培养类型选择
function typeChoiceAction(pNode)
	local tag = pNode:getTag()
	for k,v in ipairs(mChoiceTabel) do
		if k ~= tag then
			v.choiceBtn:unselected()
		else
			v.choiceBtn:selected()
			m_choiceType = tag
		end
	end
end

--培养按钮响应
function trainAction()
	if not isClick then
		isClick=true
		actionLayer.Action1217(mScene,nil,1,trianType,mGeneralID,m_choiceType)
	end
end


function releaseChangeLayer()
	if mChangeLayer then
		mChangeLayer:getParent():removeChild(mChangeLayer,true)
		mChangeLayer=nil
	end
end;


--变化框
function createChangeLayer()
	 releaseChangeLayer()
	 local layer=CCLayer:create()
	 mLayer:addChild(layer,0)
	 mChangeLayer=layer
	 
	--说明
	local contentStr=string.format("<label>%s</label>", Language.TRAIN_NOTICE)
	local noticLabel=ZyMultiLabel:new(contentStr,pWinSize.width*0.8,FONT_NAME,FONT_DEF_SIZE);
	noticLabel:setAnchorPoint(PT(0,0))
	noticLabel:setPosition(PT(pWinSize.width*0.1,
							pWinSize.height*0.855-noticLabel:getContentSize().height-SY(2)))
	noticLabel:addto(layer,0)	

	--头像
	local image=string.format("battleHead/%s.png",mGeneralInfo.BattleHeadID)
	local item = creatItem(image)
	item:setAnchorPoint(PT(0,0))
	local pos_x = pWinSize.width*0.4-item:getContentSize().width/2
	local pos_y =noticLabel:getPosition().y-item:getContentSize().height-SY(5)
	item:setPosition(PT(pos_x, pos_y))
	layer:addChild(item, 0)
	
	--昵称
	local nameLabel=CCLabelTTF:create(mGeneralInfo.GeneralName,FONT_NAME,FONT_DEF_SIZE)
	nameLabel:setAnchorPoint(PT(0,0))
	nameLabel:setPosition(PT(pos_x+item:getContentSize().width+SX(2),
							pos_y+item:getContentSize().height*0.7	))
	layer:addChild(nameLabel, 0)
	--------品质
	--1	白色2	绿色3	蓝色4	紫色
	local str=Language.EQUIP_PIN .. ":"
	if genrealQuality[mGeneralInfo.GeneralQuality] then
		str=str .. genrealQuality[mGeneralInfo.GeneralQuality]
	end
	local qulityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
	qulityLabel:setAnchorPoint(PT(0,0))
	qulityLabel:setPosition(PT(nameLabel:getPosition().x,
							nameLabel:getPosition().y-nameLabel:getContentSize().height*1.5	))
	layer:addChild(qulityLabel, 0)
	
	---等级
	local levelStr=Language.IDS_LEVEL .. ":" .. (mGeneralInfo.GeneralLv or 0)
	local levelLabel=CCLabelTTF:create(levelStr,FONT_NAME,FONT_DEF_SIZE)
	levelLabel:setAnchorPoint(PT(0,0))
	levelLabel:setPosition(PT(qulityLabel:getPosition().x,
							qulityLabel:getPosition().y-qulityLabel:getContentSize().height*1.5	))
	layer:addChild(levelLabel, 0)
	pos_y=item:getPosition().y
	pos_x=pWinSize.width*0.06
	local boxSize=SZ(pWinSize.width*0.3,pWinSize.height*0.3)
	--左边的框
	local table={
	{name=Language.ROLE_POWER,num=mGeneralInfo.PowerNum},
	{name=Language.ROLE_SOULPOWER,num=mGeneralInfo.SoulNum},
	{name=Language.ROLE_INTELLECT,num=mGeneralInfo.IntellectNum},
	}	
	local lBgSprite=createSingleItem(boxSize,Language.ROLE_TIP7,table)
	lBgSprite:setPosition(PT(pos_x,pos_y-boxSize.height))
	layer:addChild(lBgSprite, 0)
	
	--中间的潜能点
	local midSprite=CCSprite:create(P("common/list_1048.png"))
	midSprite:setAnchorPoint(PT(0.5,0.5))
	midSprite:setPosition(PT(pWinSize.width/2,
							lBgSprite:getPosition().y+boxSize.height/2))
	layer:addChild(midSprite, 0)
	
	local str=Language.TRAIN_COUNT .. ":" .. mGeneralInfo.Potential1
	local countLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	countLabel:setAnchorPoint(PT(0.5,0))
	countLabel:setPosition(PT(pWinSize.width/2,
							midSprite:getPosition().y+midSprite:getContentSize().height/2	))
	layer:addChild(countLabel, 0)
	
	--右边的框
	local table={
	{name=Language.ROLE_POWER,num=mGeneralInfo.PowerNum,addNum=mGeneralInfo.PotenceNum-mGeneralInfo.PowerNum},
	{name=Language.ROLE_SOULPOWER,num=mGeneralInfo.SoulNum,addNum=mGeneralInfo.ThoughtNum-mGeneralInfo.SoulNum},
	{name=Language.ROLE_INTELLECT,num=mGeneralInfo.IntellectNum,addNum=mGeneralInfo.IntelligenceNum-mGeneralInfo.IntellectNum},
	}	
	local rBgSprite=createSingleItem(boxSize,Language.ROLE_TIP8,table)
	rBgSprite:setPosition(PT(pWinSize.width*0.94-boxSize.width,
							lBgSprite:getPosition().y))
	layer:addChild(rBgSprite, 0)	
	---
	--取消 确定按钮
	local cancelBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.IDS_CANCEL)
	cancelBtn:setAnchorPoint(PT(0,0))
	cancelBtn:setPosition(PT(pWinSize.width*0.4-cancelBtn:getContentSize().width,
								pWinSize.height*0.21))
	cancelBtn:addto(layer,0)
	cancelBtn:registerScriptHandler(backToMainTrain)
	
	local sureBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.IDS_SURE)
	sureBtn:setAnchorPoint(PT(0,0))
	sureBtn:setPosition(PT(pWinSize.width*0.6,
								cancelBtn:getPosition().y))
	sureBtn:addto(layer,0)
	sureBtn:registerScriptHandler(makeSureChange)
end;

function  backToMainTrain()
	mGeneralInfo.PotenceNum=0
	mGeneralInfo.ThoughtNum=0
	mGeneralInfo.IntelligenceNum=0
	releaseContentLayer()
	releaseChangeLayer()
	trainContent()
end;

function makeSureChange()
	if not isClick then
		isClick=true
		actionLayer.Action1217(mScene,nil,2,trianType,mGeneralID,m_choiceType)
	end
end;

--属性栏
function  createSingleItem(boxSize,titleName,table)
	local layer=CCLayer:create()
	local lBgSprite=CCSprite:create(P("common/list_1038.9.png"))
	lBgSprite:setScaleX(boxSize.width/lBgSprite:getContentSize().width)
	lBgSprite:setScaleY(boxSize.height/lBgSprite:getContentSize().height)
	lBgSprite:setAnchorPoint(PT(0,0))
	lBgSprite:setPosition(PT(0,0))
	layer:addChild(lBgSprite, 0)
	
	local titleName=CCLabelTTF:create(titleName,FONT_NAME,FONT_SM_SIZE)
	titleName:setAnchorPoint(PT(0.5,0))
	titleName:setPosition(PT(boxSize.width/2,boxSize.height-titleName:getContentSize().height*1.5))
	layer:addChild(titleName, 0)
	
	local startX=boxSize.width*0.1
	local startY=titleName:getPosition().y
	local rowH=boxSize.height/6
	if table then
	for k, v in pairs(table) do
		local titleLabel=CCLabelTTF:create(v.name.. ":" .. v.num,FONT_NAME,FONT_SM_SIZE)
		titleLabel:setAnchorPoint(PT(0,0))
		titleLabel:setPosition(PT(startX,startY-k*rowH))
		layer:addChild(titleLabel, 0)
		if v.addNum and  v.addNum~=0 then
			local color=	ccGREEN
			local str="+" .. math.abs(v.addNum)
			if v.addNum<0 then
				color=ccRED
			 	str="-" .. math.abs(v.addNum)
			end
			local addLabel=CCLabelTTF:create( str,FONT_NAME,FONT_SM_SIZE)
			addLabel:setColor(color)
			addLabel:setAnchorPoint(PT(0,0))
			addLabel:setPosition(PT(titleLabel:getPosition().x+titleLabel:getContentSize().width+SX(10),
										titleLabel:getPosition().y))
			layer:addChild(addLabel, 0)
		end	
	end
	end
	return layer
end;

function _1217Callback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if ZyReader:getResult() == eScutNetSuccess then
		--培养
	--	
		if userData==1 then--1：培养
			mGeneralInfo.PotenceNum=ZyReader:getInt()
			mGeneralInfo.ThoughtNum=ZyReader:getInt()
			mGeneralInfo.IntelligenceNum=ZyReader:getInt()
			mGeneralInfo.Potential1=ZyReader:getInt()
			if m_choiceType==1 then
				mGeneralInfo.ItemNum=mGeneralInfo.ItemNum -trianTable[trianType].num
			end
			releaseContentLayer()
			createChangeLayer()
			MainMenuLayer.refreshWin()
		elseif userData==2 then--2：保存
			releaseChangeLayer()
			
			local num = mGeneralInfo.PowerNum+mGeneralInfo.SoulNum+mGeneralInfo.IntellectNum-mGeneralInfo.PotenceNum-mGeneralInfo.ThoughtNum-mGeneralInfo.IntelligenceNum	
			
			mGeneralInfo.PowerNum=mGeneralInfo.PotenceNum
			mGeneralInfo.SoulNum=mGeneralInfo.ThoughtNum
			mGeneralInfo.IntellectNum=mGeneralInfo.IntelligenceNum
			mGeneralInfo.Potential=mGeneralInfo.Potential1+num
			
			trainContent()
			isChange=true	
			ZyToast.show(trainLayer, Language.TRAIN_SUCCESS,1,0.35)
		end
	else
		ZyToast.show(pScutScene, ZyReader:readErrorMsg(),1,0.35)
	end
	isClick=false
end;







