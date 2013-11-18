
-- RelicArchaeology.lua.lua
-- Author     :Lysong
-- Version    : 1.0.0.0
-- Date       :
-- Description:
------------------------------------------------------------------

module("RelicArchaeology", package.seeall)

require("scenes.RbattleScene")

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mContentList=nil
local isCreateList=nil
local mHeadList=nil
local HeadListItems={}
local listTables = nil
local itemLayer = nil 
local tcontentLayer= nil 
local orTo = nil
local orOne = 0
local startX = nil
local startY = nil
local itemLayerTabel = {} 

local activeTabel = {
	eFeicui= 1,
	eJinzita = 2,
	eTongling = 3,
	eHeni = 4,
}
local listTables={}
local m_itemDataList={}
local index=nil
local order=nil
local plotid=nil
local list1=nil
local list2=nil
local mRecordTabel=nil
local mRecordTabel2=nil
local qualitypic=nil
local qualitypic2=nil
local diyi=nil
local list3=nil

local info={}


function initResource()
	HeadListItems={}
end;

function releaseResource()
	if listLayer ~= nil then
		listLayer:getParent():removeChild(listLayer, true)
		listLayer = nil
	end
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	if showLayer2 ~= nil then
		showLayer2:getParent():removeChild(showLayer2, true)
		showLayer2 = nil
	end
	rankList=nil
	mContentList=nil
	HeadListItems={}
 	closeAllLayer()
	listLayer = nil
	showLayer = nil
	showLayer2 = nil
	mLayer = nil
	mScene = nil
	
	noticeLabel=nil
	itemLayerTabel={}
	m_choiceImge = nil
	listTables=nil
	m_itemDataList=nil
	mlist=nil
	index=nil
	order=nil
	plotid=nil
	list1=nil
	list2=nil
	mRecordTabel=nil
	mRecordTabel2=nil
	qualitypic=nil
	qualitypic2=nil
	diyi=nil
	list3=nil
	info={}
	
end;


function init(mRecordTabel)
	local scene = ScutScene:new()
	-- 注册网络回调
    mScene = scene.root
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	scene:registerCallback(networkCallback)

	SlideInLReplaceScene(mScene,1)
	
	info=mRecordTabel

	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)
	--创建背景
	local midSprite=CCSprite:create(P(Image.image_halfbackground))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.855)
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)	
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	mLayer:addChild(midSprite,0)
	
	local bg1  = string.format("kaogu/%s.png",info[1].KgScene)
	imageBg = CCSprite:create(P(bg1));
	imageBg:setScaleX(pWinSize.width*0.92/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height*0.7/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.04,pWinSize.height*0.2))
	mLayer:addChild(imageBg,0)
	
	local label = CCLabelTTF:create("", FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(PT(0.5,0))
	label:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.6))
	mLayer:addChild(label, 0)
	noticeLabel = label	
	
	listTables={
    	 	{pic="mainUI/list_1179.png",type=1},
    	 	{pic="mainUI/list_1180.png",type=2},
    	 	{pic="mainUI/list_1177.png",type=3},
    	 }
    	 
    	 m_itemDataList={
    	 	{pic="smallitem/icon_4055.png"},
    	 	{pic="smallitem/icon_4055.png"},
    	 	{pic="smallitem/icon_4055.png"},
    	 	{pic="smallitem/icon_4055.png"},
    	 	{pic="smallitem/icon_4055.png"},
    	 	{pic="smallitem/icon_4055.png"},
    	 	{pic="smallitem/icon_4055.png"},
    	 	{pic="smallitem/icon_4055.png"},
    	 	{pic="smallitem/icon_4055.png"},
    	 }
    	 
    	 if PersonalInfo.getPersonalInfo().g_currentActiveID~=nil then
    	 	g_currentActiveID=PersonalInfo.getPersonalInfo().g_currentActiveID
    	 else
	    	g_currentActiveID=1
	    	PersonalInfo.getPersonalInfo().g_currentActiveID=g_currentActiveID
	 end
    	 
    diyi=1
    	 
	creatlist()	
	
	MainMenuLayer.init(2, mLayer)
	
	showHeroHead()
	
	if PersonalInfo.getPersonalInfo().plotid==nil then
		plotid=info[1].PlotID
		PersonalInfo.getPersonalInfo().plotid=plotid
		actionLayer.Action12051(mScene,nil,plotid)
	else
		plotid=PersonalInfo.getPersonalInfo().plotid
        actionLayer.Action12051(mScene,nil,PersonalInfo.getPersonalInfo().plotid)
	end
--	showContent()

-- ActiveTopUpScene.init(mScene,mLayer)	
  
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


--头部按钮list
function creatlist()
	if listLayer ~= nil then
		listLayer:getParent():removeChild(listLayer, true)
		listLayer = nil
	end
	local layer = CCLayer:create()
	mLayer:addChild(layer, 2)
	listLayer = layer
	local m_showChoice_Height = pWinSize.height* 0.13
	local m_showChoice_StartY = pWinSize.height*0.98-m_showChoice_Height 	
	--背景
	local bgSprite = CCSprite:create(P("common/list_1052.9.png"))
	bgSprite:setScaleY(m_showChoice_Height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setPosition(PT(pWinSize.width*0.5,m_showChoice_StartY))
	layer:addChild(bgSprite, 0)
	local actionBox=SZ(bgSprite:getContentSize().width,m_showChoice_Height)
	local actionBtn=UIHelper. createActionRect(actionBox)
	layer:addChild(actionBtn)
	actionBtn:setPosition(PT(pWinSize.width*0.5-actionBox.width/2,m_showChoice_StartY))
	
	local list_width = bgSprite:getContentSize().width*0.95
       local list_height = m_showChoice_Height
	startX = pWinSize.width*0.5-list_width*0.5
	startY = m_showChoice_StartY
	local listSize = SZ(list_width, list_height)
	
	local lSprite=CCSprite:create(P("button/list_1069.png"))
	lSprite:setAnchorPoint(PT(1,0.5))
	lSprite:setPosition(PT((pWinSize.width-bgSprite:getContentSize().width)/2,startY+listSize.height/2))
	layer:addChild(lSprite,0)
	local rSprite=CCSprite:create(P("button/list_1068.png"))
	rSprite:setAnchorPoint(PT(0,0.5))
	rSprite:setPosition(PT(pWinSize.width-(pWinSize.width-bgSprite:getContentSize().width)/2,
						lSprite:getPosition().y))
	layer:addChild(rSprite,0)
	local list = ScutCxList:node(listSize.width/4, ccc4(24, 24, 24, 0), listSize)
	list:setPosition(PT(startX, startY))
	list:setHorizontal(true)
	layer:addChild(list, 0)
	list:setTouchEnabled(true)
	mHeadList = list
end;

--活动头像
function showHeroHead()
	closeAllLayer()
	releaseChoiceImge()
	local list = mHeadList
	list:clear()
	
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0
	local mBgTecture=IMAGE("common/list_1012.png")
	
	for k,v in ipairs(info) do	
		local listItem = ScutCxListItem:itemWithColor(ccc3(42,28,13))
		listItem:setOpacity(0)	
		--头像 
		local image = nil
		if v.BossHeadID then
--			image = string.format("mainUI/%s.png", v.pic)
			image = string.format("kaogu/%s.png", v.BossHeadID)
			name = v.PlotMapName
		end
		local itemLayer = createMemberItem(image,k,mBgTecture,name)
		itemLayerTabel[k] = itemLayer
		listItem:addChildItem(itemLayer,layout)
		
		list:addListItem(listItem , false)
	end
--	goSwitchScene(layer,g_currentActiveID)
end

--创建头部list 单个按钮
function  createMemberItem(headImage,tag,mBgTecture,name)
	local layer = CCLayer:create()
	local goodBg=CCSprite:createWithTexture(mBgTecture)	
	local pos_x = (mHeadList:getRowWidth()-goodBg:getContentSize().width)*0.5
	local pos_y = (mHeadList:getContentSize().height-goodBg:getContentSize().height)*0.5
	goodBg:setAnchorPoint(PT(0,0))
	goodBg:setPosition(PT(pos_x, pos_y))
	layer:addChild(goodBg,0)
	layer:setContentSize(goodBg:getContentSize())
	if headImage then
		headSprite=CCSprite:create(P(headImage))
		headSprite:setAnchorPoint(CCPoint(0.5, 0.5))
		headSprite:setPosition(PT(goodBg:getContentSize().width/2,goodBg:getContentSize().height/2))
		goodBg:addChild(headSprite,0)
		
		local actionBtn=UIHelper.createActionRect(goodBg:getContentSize(),key_head, tag)
		actionBtn:setAnchorPoint(PT(0,0))
		actionBtn:setPosition(PT(0,0))
		goodBg:addChild(actionBtn,0)
		HeadListItems[tag]=actionBtn
	end
	if name then
		--名字
		local jingli = CCLabelTTF:create(name,FONT_NAME,FONT_SMM_SIZE)
		goodBg:addChild(jingli,0)
		jingli:setAnchorPoint(PT(0,0))
		jingli:setPosition(PT(goodBg:getPosition().x+(goodBg:getContentSize().width-jingli:getContentSize().width)/2-SX(10),goodBg:getPosition().y))
	end
	--
	if PersonalInfo.getPersonalInfo().g_currentActiveID == diyi then
		releaseChoiceImge()
		m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		m_choiceImge:setPosition(PT(goodBg:getContentSize().width*0.5,goodBg:getContentSize().height*0.5))
		goodBg:addChild(m_choiceImge,1)
		diyi=diyi+1
	else
		diyi=diyi+1
	end
	return layer
end

--活动佣兵
function key_head(pNode, index)
	local tag = index
	if pNode then
		tag = pNode:getTag()
	end
	if info[tag].IsActive==1 then
		if tag ~= orTo then
			closeAllLayer()
			
			orTo = tag
			
			g_currentActiveID = tag
			
			goSwitchScene(nil,g_currentActiveID)
			PersonalInfo.getPersonalInfo().g_currentActiveID=g_currentActiveID
			releaseChoiceImge()
			m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
			m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
			m_choiceImge:setPosition(PT(pNode:getContentSize().width*0.5, pNode:getContentSize().height*0.5))
			pNode:addChild(m_choiceImge,1)
		end
	elseif info[tag].IsActive==0 then
		ZyToast.show(mScene,Language.TONGGUAN)
	end
end

function releaseChoiceImge()
	if m_choiceImge ~= nil  then
		m_choiceImge:getParent():removeChild(m_choiceImge,true)
		m_choiceImge=nil
	end
end;

function goSwitchScene(layer,activeId)
	local currentInfo = getCurrentInfo(activeId)
	plotid=info[activeId].PlotID
	PersonalInfo.getPersonalInfo().plotid=plotid
	releseShowLayer()
	releseShowLayer2()
	local IsActive=info[activeId].IsActive
	if IsActive==1 then
--		plotid=info[1].PlotID
		PersonalInfo.getPersonalInfo().HasNextBoss=nil
		actionLayer.Action12051(mScene,nil,plotid)
	end
end

function refreshgou()
	actionLayer.Action12051(mScene,nil,plotid)
end;

function closeAllLayer()

	ShengjitaScene.close()--圣吉塔
--	RelicScene.close()--遗迹
end;

function getCurrentInfo(activeId)
	for k,v in ipairs(listTables) do
		if v.FestivalType == activeId then
			return v
		end
	end
	return nil
end;

function releseShowLayer()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
end

function showContent()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil
	end
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	showLayer = layer
	
	local bg1  = string.format("kaogu/%s.png",info[g_currentActiveID].KgScene)
	imageBg = CCSprite:create(P(bg1));
	imageBg:setScaleX(pWinSize.width*0.92/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height*0.7/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(pWinSize.width*0.04,pWinSize.height*0.2))
	showLayer:addChild(imageBg,0)
	
	--已收集宝图碎片
	local tiaozhan=Language.YISHOUJI..list1.CurrentMapCount.."/"..list1.FragmentNum
	local yishouji = CCLabelTTF:create(tiaozhan,FONT_NAME,FONT_SMM_SIZE)
	showLayer:addChild(yishouji,1)
	yishouji:setAnchorPoint(PT(0,0))
	yishouji:setPosition(PT(pWinSize.width*0.05,pWinSize.height*0.3))
	
	--精力
	local tiaozhan=Language.TISHI_0..list1.ConsumeEnergy..Language.TISHI_1..list1.CurrentEnergy.."/"..list1.MaxEnergy
	local jingli = CCLabelTTF:create(tiaozhan,FONT_NAME,FONT_SMM_SIZE)
	showLayer:addChild(jingli,1)
	jingli:setAnchorPoint(PT(0,0))
	jingli:setPosition(PT(pWinSize.width*0.05,pWinSize.height*0.26))
	
	--介绍
	local content=string.format("<label>%s</label>",Language.TISHI_2)
	local multiWidth=pWinSize.width*0.64
	local ndMultiLabe=ZyMultiLabel:new(content,multiWidth,FONT_NAME,FONT_SMM_SIZE)
	ndMultiLabe:setAnchorPoint(PT(0,1))
	ndMultiLabe:setPosition(PT(pWinSize.width*0.05,pWinSize.height*0.2))
	ndMultiLabe:addto(showLayer,0)
	
	--离开考古按钮
	local getBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.LIKAIKAOGU,FONT_NAME,FONT_SM_SIZE)
	getBtn:setAnchorPoint(PT(0,0))
	getBtn:setPosition(PT(pWinSize.width*0.7,pWinSize.height*0.22))
	getBtn:registerScriptHandler(leave)
	getBtn:addto(showLayer, 0)
	
	local col,row=3,3
	local colW=pWinSize.width/col
	local rowH=pWinSize.height/row
	local startX=colW/2
	local startY=pWinSize.height*0.8-rowH/2
	for k, v in pairs(m_itemDataList) do
		button= ZyButton:new("kaogu/list_3026.png", "kaogu/list_3026.png", nil,nil,nil, FONT_SM_SIZE)
	 	button:setAnchorPoint(PT(0,0))
	 	local rowH=button:getContentSize().height*1.2
	 	button:setPosition(PT((k-1)%col*colW*0.8+startX*1.3-button:getContentSize().width/2,startY*1.2-math.floor((k-1)/col)*rowH-button:getContentSize().height/2))
	 	button:addto(showLayer,0)
-- 	
	 end

	 for k,v in pairs(mRecordTabel) do
	 	if mRecordTabel[k].IsBox==0 then
		 	if mRecordTabel[k].Quality==1 then
		 		qualitypic2="common/icon_8015_1.png"
		 	elseif mRecordTabel[k].Quality==2 then
		 		qualitypic2="common/icon_8015_2.png"
		 	elseif mRecordTabel[k].Quality==3 then
		 		qualitypic2="common/icon_8015_3.png"
		 	elseif mRecordTabel[k].Quality==4 then
		 		qualitypic2="common/icon_8015_4.png"
		 	end
		 	local gempic= CCSprite:create(P(qualitypic2));
	     	 	gempic:setAnchorPoint(PT(0,0))
	     	 	local rowH=button:getContentSize().height*1.2
--	     	 	gempic:setPosition(PT((mRecordTabel[k].Position-1)%col*colW*0.8+startX*1.35-button:getContentSize().width/2,startY*1.25-math.floor((mRecordTabel[k].Position-1)/col)*rowH*0.52-button:getContentSize().height/2))
	     	 	gempic:setPosition(PT((mRecordTabel[k].Position-1)%col*colW*0.8+startX*1.35-button:getContentSize().width/2,startY*1.2-math.floor((mRecordTabel[k].Position-1)/col)*rowH-button:getContentSize().height/2+button:getContentSize().height-gempic:getContentSize().height))
	     	 	showLayer:addChild(gempic,0)
		 	
		 	local image  = string.format("smallitem/%s.png",mRecordTabel[k].HeadID)
		 	local gempic= ZyButton:new(image, image, nil,nil,nil, FONT_SM_SIZE)
	--	 	local gempic= ZyButton:new("smallitem/Icon_2501.png", "smallitem/Icon_2501.png", nil,nil,nil, FONT_SM_SIZE)
	     	 	gempic:setAnchorPoint(PT(0,0))
--	     	 	gempic:setPosition(PT((mRecordTabel[k].Position-1)%col*colW*0.8+startX*1.41-button:getContentSize().width/2,startY*1.265-math.floor((mRecordTabel[k].Position-1)/col)*rowH*0.52-button:getContentSize().height/2))
	     	 	gempic:setPosition(PT((mRecordTabel[k].Position-1)%col*colW*0.8+startX*1.41-button:getContentSize().width/2,startY*1.185-math.floor((mRecordTabel[k].Position-1)/col)*rowH-button:getContentSize().height/2+button:getContentSize().height-gempic:getContentSize().height))
	     	 	gempic:setTag(k)
	     	 	gempic:registerScriptHandler(ButtonListAction1)
	     	 	gempic:addto(showLayer,0)

	     	 	
	     	 	--名字
			local jingli = CCLabelTTF:create(mRecordTabel[k].Name,FONT_NAME,FONT_SMM_SIZE)
			showLayer:addChild(jingli,0)
			jingli:setAnchorPoint(PT(0,0))
			jingli:setPosition(PT(gempic:getPosition().x+(gempic:getContentSize().width-jingli:getContentSize().width)/2,gempic:getPosition().y))
	     	 	

			local keyItem = showKey(mRecordTabel[k].Quality , mRecordTabel[k].HasMapCount  , startX*0.18)
			keyItem:setAnchorPoint(PT(0,0))
	     	 	keyItem:setPosition(PT(gempic:getPosition().x+gempic:getContentSize().width/2-keyItem:getContentSize().width*0.5, startY*1.2-math.floor((mRecordTabel[k].Position-1)/col)*rowH-button:getContentSize().height/2+SY(5)))			
			showLayer:addChild(keyItem,0)
			
	     	 	--]]
	     	 else
	     	 	button= ZyButton:new("kaogu/list_3028.png", "kaogu/list_3028.png", nil,nil,nil, FONT_SM_SIZE)
		 	button:setAnchorPoint(PT(0,0))
		 	local rowH=button:getContentSize().height*1.2
		 	button:setPosition(PT((mRecordTabel[k].Position-1)%col*colW*0.8+startX*1.3-button:getContentSize().width/2,startY*1.2-math.floor((mRecordTabel[k].Position-1)/col)*rowH*0.96-button:getContentSize().height/2))
		 	button:setTag(k)
		 	button:registerScriptHandler(ButtonListAction1)
		 	button:addto(showLayer,0)
	     	 end
	     	 --]]
	 end;
	--]]
	local order=1
	
end

function showKey(totalNum, currentNum, space)
	local layer = CCLayer:create()
	
	local startX = 0
	local height = 1
	
	if totalNum > 0  then
		for k=1, totalNum do
			local gempic= CCSprite:create(P("kaogu/list_3030.png"));
			gempic:setAnchorPoint(PT(0,0))
			gempic:setPosition(PT(startX, 0))
			layer:addChild(gempic,0)
			
			height = gempic:getContentSize().height
			if k== totalNum then
				startX = startX+gempic:getContentSize().width
			else
				startX = startX+space
			end
		end
	end
	
	local width = startX
	
	
	local pos_x=0
	if currentNum > 0 then
		for k3=1 ,currentNum do
			local gempic= CCSprite:create(P("kaogu/list_3029.png"));
			gempic:setAnchorPoint(PT(0,0))
			gempic:setPosition(PT(pos_x,0))
			layer:addChild(gempic,0)
			
			
			if k== currentNum then
				pos_x = pos_x+gempic:getContentSize().width
			else
				pos_x = pos_x+space
			end				
		end
	end
	
	layer:setContentSize(SZ(width, height))
	return layer
end

function leave()
	TrialScene.init()
end;

function ButtonListAction1(pNode)
	local tag= pNode:getTag()
	index=tag
--	if tag==3 or tag==4 or tag==8 then
--		send()
--	end
	if mRecordTabel[tag].IsBox==0 then
--		SbattleScene.init(fightInfo)
		actionLayer.Action12053(mScene,nil,mRecordTabel[tag].PlotNpcID,mRecordTabel[tag].Position)
	else
		actionLayer.Action12056(mScene,nil,plotid,mRecordTabel[tag].PlotNpcID)
	end
	
end;

function removesend()
    if sendLayer ~= nil then
        sendLayer:getParent():removeChild(sendLayer,true)
        sendLayer = nil
    end
end

function initsend()
    removesend()
    sendLayer=CCLayer:create()
    sendLayer:setAnchorPoint(PT(0,0));
    sendLayer:setPosition(PT(0,0));
    mLayer:addChild(sendLayer,0)
end

function send()

	local posX=pWinSize.width
    	local posY=pWinSize.height
	initsend()
	--透明
	local touming=ZyButton:new("common/list_1020.9.png","common/list_1020.9.png", nil,nil,nil, FONT_SM_SIZE)
	touming:setScaleX(posX/touming:getContentSize().width)
	touming:setScaleY(posY/touming:getContentSize().height)
	touming:setAnchorPoint(PT(0,0))
	touming:setPosition(PT(0,0));
	touming:addto(sendLayer)
	
	local background= CCSprite:create(P("common/List_2008.9.png"));
	background:setScaleX(posX/background:getContentSize().width*0.4)
	background:setScaleY(posY/background:getContentSize().height*0.4)
    	background:setAnchorPoint(PT(0,0))
    	background:setPosition(PT(posX*0.3,posY*0.3))
    	sendLayer:addChild(background,0)
    	
    	--恭喜
    	local qbxiaohao = CCLabelTTF:create(Language.GONGXI,FONT_NAME,FONT_SMM_SIZE)
	sendLayer:addChild(qbxiaohao,0)
	qbxiaohao:setAnchorPoint(PT(0,0))
	qbxiaohao:setPosition(PT(posX*0.45,posY*0.65))
    	
    	--恭喜获得：
    	local qbxiaohao = CCLabelTTF:create(Language.GONGXIHUODE..list3.RewardInfo.."*"..list3.Num,FONT_NAME,FONT_SMM_SIZE)
	sendLayer:addChild(qbxiaohao,0)
	qbxiaohao:setAnchorPoint(PT(0,0))
	qbxiaohao:setPosition(PT(posX*0.32,posY*0.6))
    	
    	local huode= CCSprite:create(P("common/icon_8015_4.png"));
    	huode:setAnchorPoint(PT(0,0))
    	huode:setPosition(PT(posX*0.41,posY*0.45))
    	sendLayer:addChild(huode,0)
    	local image  = string.format("smallitem/%s.png",list3.Picture)
    	local huode= CCSprite:create(P(image));
    	huode:setAnchorPoint(PT(0,0))
    	huode:setPosition(PT(posX*0.42,posY*0.458))
    	sendLayer:addChild(huode,0)
    	--数量
    	local qbxiaohao = CCLabelTTF:create(list3.Num,FONT_NAME,FONT_SMM_SIZE)
	sendLayer:addChild(qbxiaohao,0)
	qbxiaohao:setAnchorPoint(PT(0,0))
	qbxiaohao:setPosition(PT(background:getPosition().x+(posX*0.4-qbxiaohao:getContentSize().width)/2,posY*0.42))
	
	--确定按钮
	local quedingBtn= ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1, nil,Language.IDS_SURE,nil, FONT_SM_SIZE)
	quedingBtn:setAnchorPoint(PT(0,0))
	quedingBtn:setPosition(PT(posX*0.38,posY*0.33))
	quedingBtn:registerScriptHandler(suresend)
	quedingBtn:addto(sendLayer,1)

end

function suresend()
	removesend()
	actionLayer.Action12051(mScene,nil,plotid)
end

function releseShowLayer2()
	if showLayer2 ~= nil then
		showLayer2:getParent():removeChild(showLayer2, true)
		showLayer2 = nil
	end
end

function showContent2()
	if showLayer2 ~= nil then
		showLayer2:getParent():removeChild(showLayer2, true)
		showLayer2 = nil
	end
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	showLayer2 = layer
	
	--精力
	local tiaozhan=Language.TISHI_0..list2.ConsumeEnergy..Language.TISHI_1..list2.CurrentEnergy.."/"..list2.MaxEnergy
	local yishouji = CCLabelTTF:create(tiaozhan,FONT_NAME,FONT_SMM_SIZE)
	showLayer2:addChild(yishouji,1)
	yishouji:setAnchorPoint(PT(0,0))
	yishouji:setPosition(PT(pWinSize.width*0.05,pWinSize.height*0.34))
	
	--剩余挑战次数
	local tiaozhan=Language.SHENGYUTIAOZHAN..list2.ChallengeCount.."/"..list2.MaxChallengeCount
	local cishu = CCLabelTTF:create(tiaozhan,FONT_NAME,FONT_SMM_SIZE)
	showLayer2:addChild(cishu,1)
	cishu:setAnchorPoint(PT(0,0))
	cishu:setPosition(PT(pWinSize.width*0.65,pWinSize.height*0.34))
	
	--掉落
	local cishu = CCLabelTTF:create(Language.DIAOLUO,FONT_NAME,FONT_SMM_SIZE)
	showLayer2:addChild(cishu,1)
	cishu:setAnchorPoint(PT(0,0))
	cishu:setPosition(PT(pWinSize.width*0.05,pWinSize.height*0.27))
	
	local boxSize=SZ(pWinSize.width*0.55,pWinSize.height*0.15)
       local start_x=pWinSize.width*0.1
	local start_y=pWinSize.height*0.1
	rankList=ScutCxList:node(boxSize.height ,ccc4(25, 25, 25, 25),boxSize)
	showLayer2:addChild(rankList,0)
	rankList:setAnchorPoint(PT(0,0))
	rankList:setPosition(PT(start_x*1.3,start_y*2))
	rankList:setHorizontal(true)
	
	refreshList()
	--[[
	for k,v in pairs(mRecordTabel2) do
		local huode= CCSprite:create(P("common/icon_8015_4.png"));
	    	huode:setAnchorPoint(PT(0,0))
	    	huode:setPosition(PT(pWinSize.width*0.15+(k-1)*pWinSize.width*0.16,pWinSize.height*0.22))
	    	showLayer2:addChild(huode,0)
	    	
	    	local image  = string.format("smallitem/%s.png",mRecordTabel2[k].ItemHead)
		local huode= CCSprite:create(P(image));
	    	huode:setAnchorPoint(PT(0,0))
	    	huode:setPosition(PT(pWinSize.width*0.16+(k-1)*pWinSize.width*0.16,pWinSize.height*0.228))
	    	showLayer2:addChild(huode,0)
	    	
	    	local cishu = CCLabelTTF:create(mRecordTabel2[k].ItemName,FONT_NAME,FONT_SMM_SIZE)
		showLayer2:addChild(cishu,0)
		cishu:setAnchorPoint(PT(0,0))
		cishu:setPosition(PT(pWinSize.width*0.18+(k-1)*pWinSize.width*0.16,pWinSize.height*0.2))
		
	end
	--]]
	--离开考古按钮
	local getBtn=ZyButton:new(Image.image_button_red_c_0,Image.image_button_red_c_1,nil,Language.LIKAIKAOGU,FONT_NAME,FONT_SM_SIZE)
	getBtn:setAnchorPoint(PT(0,0))
	getBtn:setPosition(PT(pWinSize.width*0.7,pWinSize.height*0.22))
	getBtn:registerScriptHandler(leave)
	getBtn:addto(showLayer2, 0)
	
	if list2.Quality==1 then
		qualitypic="common/icon_8017_1.png"
	elseif list2.Quality==1 then
		qualitypic="common/icon_8017_2.png"
	elseif list2.Quality==3 then
		qualitypic="common/icon_8017_3.png"
	elseif list2.Quality==4 then
		qualitypic="common/icon_8017_4.png"
	end
	local background= CCSprite:create(P(qualitypic));
    	background:setAnchorPoint(PT(0,0))
    	background:setPosition(PT((pWinSize.width-background:getContentSize().width)/2,pWinSize.height*0.4))
    	showLayer2:addChild(background,0)
	
	if list2.IsWin==0 then
	--	local background= CCSprite:create(P(list2.Picture));
	    local image  = string.format("bigitem/%s.png",list2.Picture)
		local background= ZyButton:new(image,image, nil,nil,nil, FONT_SM_SIZE)
	    	background:setAnchorPoint(PT(0,0))
	    	background:registerScriptHandler(boss)
	    	background:setPosition(PT((pWinSize.width-background:getContentSize().width)/2,pWinSize.height*0.44))
	    	background:addto(showLayer2,0)
	else
	    local image  = string.format("bigitem/%s.png",list2.Picture)
		local background= ZyButton:new(image,image, nil,nil,nil, FONT_SM_SIZE)
	    	background:setAnchorPoint(PT(0,0))
	    	background:setPosition(PT((pWinSize.width-background:getContentSize().width)/2,pWinSize.height*0.44))
	    	background:addto(showLayer2,0)
	    	
	    	local zuihou= CCSprite:create(P("longxue/list_3036.png"));
	    	zuihou:setAnchorPoint(PT(0,0))
	    	zuihou:setPosition(PT((pWinSize.width-zuihou:getContentSize().width)/2,pWinSize.height*0.65))
	    	showLayer2:addChild(zuihou,0)
	end
	
	local bossname = CCLabelTTF:create(list2.Name,FONT_NAME,FONT_SMM_SIZE)
	showLayer2:addChild(bossname,0)
	bossname:setAnchorPoint(PT(0,0))
	bossname:setPosition(PT((pWinSize.width-bossname:getContentSize().width)/2,pWinSize.height*0.8))
	
	--boss编号
	local bossname = CCLabelTTF:create(list2.NpcSeqNo..Language.BIANHAO,FONT_NAME,FONT_SMM_SIZE)
	showLayer2:addChild(bossname,0)
	bossname:setAnchorPoint(PT(0,0))
	bossname:setPosition(PT(background:getPosition().x+SX(10),background:getPosition().y+SX(10)))
	
	--boss等级
	local bosslv = CCLabelTTF:create("Lv"..list2.Level,FONT_NAME,FONT_SMM_SIZE)
	showLayer2:addChild(bosslv,0)
	bosslv:setAnchorPoint(PT(0,0))
	bosslv:setPosition(PT(background:getPosition().x+background:getContentSize().width-bosslv:getContentSize().width*1.5,background:getPosition().y+SX(10)))
	
end

function  refreshList()
	if rankList~=nil then
		rankList:clear()
	end
	for k, v in pairs(mRecordTabel2) do
--	for k, v in pairs(tasklist) do
		local listItem=ScutCxListItem:itemWithColor(ccc3(42,28,13))
		listItem:setOpacity(0)
		listItem:setMargin(CCSize(0,0));
		local mlayout=CxLayout()
		mlayout.val_x.t = ABS_WITH_PIXEL
		mlayout.val_y.t = ABS_WITH_PIXEL
		mlayout.val_x.val.pixel_val =0
		mlayout.val_y.val.pixel_val =0
		local layer = CCLayer:create();
		
		local huode= CCSprite:create(P("common/icon_8015_4.png"));
	    	huode:setAnchorPoint(PT(0,0))
	    	huode:setPosition(PT(0,pWinSize.height*0.02))
	    	layer:addChild(huode,0)
	    	
	    	local image  = string.format("smallitem/%s.png",mRecordTabel2[k].ItemHead)
		local huode= CCSprite:create(P(image));
	    	huode:setAnchorPoint(PT(0,0))
	    	huode:setPosition(PT(pWinSize.width*0.01,pWinSize.height*0.028))
	    	layer:addChild(huode,0)
	    	
	    	local cishu = CCLabelTTF:create(mRecordTabel2[k].ItemName,FONT_NAME,FONT_SMM_SIZE)
		layer:addChild(cishu,0)
		cishu:setAnchorPoint(PT(0,0))
--		cishu:setPosition(PT(pWinSize.width*0.03,0))
		cishu:setPosition(PT(huode:getPosition().x+(huode:getContentSize().width-cishu:getContentSize().width)/2,0))
	    	
	       rankList:setRowWidth(huode:getContentSize().height*1.2)
	       --]]
	       listItem:addChildItem(layer)
	       rankList:addListItem(listItem, false)
	end
	
end

function boss()
	if list2.CurrentEnergy==0 then
		ZyToast.show(mScene,Language.JINLIBUZU)
	else
		if list2.ChallengeCount==0 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(mScene, nil,Language.TIAOZHANBUZU , Language.IDS_SURE, Language.IDS_CANCEL,addAction) 
		else
			actionLayer.Action12053(mScene,nil,list2.PlotNpcID,0)
		end
	end
	
end;

function addAction(clickedButtonIndex)
	if clickedButtonIndex == 1 then--确认
		actionLayer.Action12055(mScene,nil,list2.PlotNpcID)
	end
end;

--播放动画  胜利/失败
function playAnimation(skill, index, tag)
	local attribuAni=ScutAnimation.CScutAnimationManager:GetInstance():LoadSprite(skill)
	if index then
		attribuAni:setCurAni(index)
	end
	if tag then
		attribuAni:setTag(tag)
	end
	attribuAni:play()
	attribuAni:registerFrameCallback("RelicArchaeology.finishAnimation")
	mLayer:addChild(attribuAni, 1)
	attribuAni:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.5))

end

--动画播放完成后 
function finishAnimation(pSprite, curAniIndex, curFrameIndex, nPlayingFlag)
	if nPlayingFlag == 2 then
		pSprite:registerFrameCallback("")
		delayRemove(pSprite)
--		battleOver()
	end
end

function delayRemove(sprite)
	if sprite ~= nil then
		local delayAct = CCDelayTime:create(0.3)
		local funcName = CCCallFuncN:create(RelicArchaeology.removeTmpSprite)
		local act = CCSequence:createWithTwoActions(delayAct,funcName)
		sprite:runAction(act)
		sprite = nil
	end
	
end

function removeTmpSprite(sprite)
	local tag = sprite:getTag()
	if sprite ~= nil then
		sprite:removeFromParentAndCleanup(true)
		sprite = nil
	end
    
	if tag == 1 then
		releseShowLayer()
		removetm()
		actionLayer.Action12052(mScene,nil,plotid)
--		showContent2()
	end
end



--透明层
function removetm()
    if tmLayer ~= nil then
        tmLayer:getParent():removeChild(tmLayer,true)
        tmLayer = nil
    end
end

function inittm()
    removetm()
    tmLayer=CCLayer:create()
    tmLayer:setAnchorPoint(PT(0,0));
    tmLayer:setPosition(PT(0,0));
    mLayer:addChild(tmLayer,0)
end

function tmceng()

	local posX=pWinSize.width
    	local posY=pWinSize.height
	inittm()
	--透明
	local touming=ZyButton:new("common/list_1020.9.png","common/list_1020.9.png", nil,nil,nil, FONT_SM_SIZE)
	touming:setScaleX(posX/touming:getContentSize().width)
	touming:setScaleY(posY/touming:getContentSize().height)
	touming:setAnchorPoint(PT(0,0))
	touming:setPosition(PT(0,0));
	touming:addto(tmLayer)
	
end;

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	if actionId==12051 then
		local serverInfo=actionLayer._12051Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			list1=serverInfo
			mRecordTabel=serverInfo.RecordTabel
		end
	       if PersonalInfo.getPersonalInfo().HasNextBoss~=nil and PersonalInfo.getPersonalInfo().HasNextBoss==1 then
	              actionLayer.Action12052(mScene,nil,plotid)
	       else
		       if list1.CurrentMapCount==list1.FragmentNum then
		             tmceng()
		             if list1.PlayAnimat==0 then
					playAnimation("donghua_1011", nil ,1)
				else
					releseShowLayer()
					removetm()
					actionLayer.Action12052(mScene,nil,plotid)
				end
		       else
		             showContent()
		       end
	       end
	elseif actionId==12052 then
		local serverInfo=actionLayer._12052Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			list2=serverInfo
			mRecordTabel2=serverInfo.RecordTabel
			PersonalInfo.getPersonalInfo().HasNextBoss=list2.HasNextBoss
		end
		showContent2()
	elseif actionId==12053 then
		if ZyReader:getResult()== eScutNetSuccess then
			local serverInfo=actionLayer._12053Callback(pScutScene, lpExternalData)
			if serverInfo~=nil then			
				RbattleScene.init(serverInfo)
			end
		else
			ZyToast.show(mScene,ZyReader:readErrorMsg())
		end
	elseif actionId==12055 then
		if ZyReader:getResult()== eScutNetSuccess then
			actionLayer.Action12052(mScene,nil,plotid)
			MainMenuLayer.refreshWin()
		else
			if ZyReader:readErrorMsg()==1 then

			else
				ZyToast.show(mScene,ZyReader:readErrorMsg())
			end
		end
	elseif actionId==12056 then
		if ZyReader:getResult()== eScutNetSuccess then
			local serverInfo=actionLayer._12056Callback(pScutScene, lpExternalData)
			if serverInfo~=nil then			
				list3=serverInfo
			end
			send()
		else
			ZyToast.show(mScene,ZyReader:readErrorMsg())
		end
	elseif actionId==1091 then
		local serverInfo=actionLayer._1091Callback(pScutScene, lpExternalData)
		if serverInfo then
			AddSpriteLayer.setInfo(mScene)
			AddSpriteLayer.createEnergyLayer(serverInfo,1)
		end
	elseif actionId==1010 then
		AddSpriteLayer.networkCallback(pScutScene, lpExternalData)
	end
	
	UpGradeScene.setIsClick(false)
end

