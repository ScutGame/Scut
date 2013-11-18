------------------------------------------------------------------
-- ActiveBarLayer.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
  -- Description: 
------------------------------------------------------------------

module("ActiveBarLayer", package.seeall)
require("scenes.ActiveTopUpScene")
require("layers.GoldMinerLayer")
require("layers.CommonActiveLayer")require("scenes.ShengjitaScene")
require("scenes.DragonholeScene")
require("scenes.RelicScene")

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
local headImgLabel = {
	eManor = "mainUI/list_1174.png",
	eTodayTo = "mainUI/list_1175.png",

}
local activeTabel = {
	eManor = 14,
	eTodayTo = 15,
	ePray = 26,
	eTopUp = 13,
	eGoldMiner = 17,
	
	
	eToUp = 22, --- 首充翻倍
	eToUpRate = 18,---充值返利
	eRanger = 20, -----招募佣兵
	eDouble = 24,----副本双倍送礼活动
	eLv = 21,--- 升级送好礼活动
	eBussice = 23 ,---商城打折活动
	eScript = 27,----精灵祝福
	eSprite = 16,
	
	eDragon = 28,--龙穴
	eBoss = 30,--世界boss  假id
	
	eLogin = 19,--登录送好礼
}



function initResource()
	HeadListItems={}
end;

function releaseResource()
	if listLayer ~= nil then
		listLayer:getParent():removeChild(listLayer, true)
		listLayer = nil
	end
	mContentList=nil
	HeadListItems={}
 	closeAllLayer()
	listLayer = nil
	mLayer = nil
	mScene = nil
	TodayToScene.releaseResource()
	ManorScene.releaseResource()
	prayScene.releaseResource()
	GoldMinerLayer.releaseResource()

	TopUpRebate.releaseResource()
	UpGradeScene.releaseResource()
	ActiveTopUpScene.releaseResource()
	BSDiscountScene.releaseResource()
	DoubleIncome.releaseResource()
	RecruitScene.releaseResource()
	
	noticeLabel=nil
	itemLayerTabel={}
	m_choiceImge = nil
	listTables=nil
	
	MainMenuLayer.refreshWin()
end;


function init()
	local scene = ScutScene:new()
	-- 注册网络回调
			mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	scene:registerCallback(networkCallback)
    mScene = scene.root
	SlideInLReplaceScene(mScene,1)

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
	
	
	local label = CCLabelTTF:create("", FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(PT(0.5,0))
	label:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.6))
	mLayer:addChild(label, 0)
	noticeLabel = label	
	
	creatlist()	
	
	MainMenuLayer.init(2, mLayer)
	

--	ActiveTopUpScene.init(mScene,mLayer)	
    	
	sendAction(3012)
  
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
	list:setTouchEnabled(true)
	list:setPosition(PT(startX, startY))
	list:setHorizontal(true)
	layer:addChild(list, 0)
	mHeadList = list
end;

--佣兵头像
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
	if not listTables or #listTables == 0 then
		noticeLabel:setString(Language.ACTIVE_NOACTIVE)
		return
	else
		noticeLabel:setString("")
	end
	
	
	--开启上次活动
	local isHaveLast = nil--上一次查看的活动还存在
	for k,v in ipairs(listTables) do
		if v.FestivalType == g_currentActiveID then
			isHaveLast = true
		end
	end	
	if not isHaveLast then
		g_currentActiveID = listTables[1].FestivalType
	end

	
	for k,v in ipairs(listTables) do	
		local listItem = ScutCxListItem:itemWithColor(ccc3(42,28,13))
		listItem:setOpacity(0)	
		--头像 
		local image = nil
		if v.HeadID then
			image = string.format("mainUI/%s.png", v.HeadID)
		end
		local itemLayer = createMemberItem(image,k,mBgTecture)
		itemLayerTabel[k] = itemLayer
		listItem:addChildItem(itemLayer,layout)
		
		list:addListItem(listItem , false)
	end
	goSwitchScene(layer,g_currentActiveID)
end

--创建头部list 单个按钮
function  createMemberItem(headImage,tag,mBgTecture)
	local layer = CCLayer:create()
	local goodBg=CCSprite:createWithTexture(mBgTecture)	
	local pos_x = (mHeadList:getRowWidth()-goodBg:getContentSize().width)*0.5
	local pos_y = (mHeadList:getContentSize().height-goodBg:getContentSize().height)*0.5
	goodBg:setAnchorPoint(PT(0,0))
	goodBg:setPosition(PT(pos_x, pos_y))
	layer:addChild(goodBg,0)
	layer:setContentSize(goodBg:getContentSize())
	if headImage then
		local headSprite=CCSprite:create(P(headImage))
		headSprite:setAnchorPoint(CCPoint(0.5, 0.5))
		headSprite:setPosition(PT(goodBg:getContentSize().width/2,
						goodBg:getContentSize().height/2))
		goodBg:addChild(headSprite,0)
		
		local actionBtn=UIHelper.createActionRect(goodBg:getContentSize(),ActiveBarLayer.key_head, tag)
		actionBtn:setAnchorPoint(PT(0,0))
		actionBtn:setPosition(PT(0,0))
		goodBg:addChild(actionBtn,0)
		HeadListItems[tag]=actionBtn
	end
	if listTables[tag].FestivalType == g_currentActiveID then
		releaseChoiceImge()
		m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		m_choiceImge:setPosition(PT(goodBg:getContentSize().width*0.5,goodBg:getContentSize().height*0.5))
		goodBg:addChild(m_choiceImge,1)
	end
	return layer
end

--选择佣兵
function key_head(pNode, index)
	local tag = index
	if pNode then
		tag = pNode:getTag()
	end
	if tag ~= orTo then
		closeAllLayer()

		
		orTo = tag
		
		g_currentActiveID = listTables[tag].FestivalType
		goSwitchScene(nil,g_currentActiveID)
		
		releaseChoiceImge()
		m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		m_choiceImge:setPosition(PT(pNode:getContentSize().width*0.5, pNode:getContentSize().height*0.5))
		pNode:addChild(m_choiceImge,1)
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
	if activeId ==activeTabel.eManor  then
		ManorScene.initUI(mScene,mLayer)
	elseif activeId ==activeTabel.eTodayTo then
		TodayToScene.setLayer(mLayer)
		TodayToScene.init(mScene)
	elseif activeId ==activeTabel.ePray then 
		prayScene.init(mScene, mLayer)	
	elseif activeId ==activeTabel.eTopUp  then
		ActiveTopUpScene.setData(currentInfo)
		ActiveTopUpScene.init(mScene,mLayer)
	elseif activeId ==activeTabel.eGoldMiner then
		GoldMinerLayer.setLayer(mScene, mLayer)
		GoldMinerLayer.init()
--		DragonholeScene.setLayer(mScene, mLayer)
--		DragonholeScene.init()
--		RelicScene.setLayer(mScene, mLayer)
--		RelicScene.init()
	elseif activeId == activeTabel.eDragon then
		DragonholeScene.setLayer(mScene, mLayer)
		DragonholeScene.init()
	elseif activeId  == activeTabel.eToUpRate then
		TopUpRebate.setData(currentInfo)	
		TopUpRebate.init(mScene,mLayer)
	elseif  activeId  == activeTabel.eLv then
		UpGradeScene.setData(currentInfo)	
		UpGradeScene.init(mScene,mLayer)
	elseif activeId ==activeTabel.eToUp then-- 
	--	ActiveTopUpScene.init(mScene,mLayer)
	elseif activeId == activeTabel.eBussice then--商城大作战 
		BSDiscountScene.init(mScene,mLayer, currentInfo)	
	elseif activeId == activeTabel.eDouble then--双倍收益 
		DoubleIncome.init(mScene,mLayer,currentInfo)	
	elseif activeId == activeTabel.eRanger then--招募送灵魂
		RecruitScene.init(mScene,mLayer,currentInfo)
	elseif activeId == activeTabel.eBoss then
		WorldBossToScene.setData(currentInfo)
		WorldBossToScene.init(mScene, mLayer)
	elseif activeId == activeTabel.eSprite then
--		sendToScene.init()	
	elseif activeId == activeTabel.eLogin then
		local haveBtn = true--是否有；领取按钮
		CommonActiveLayer.init(mScene,mLayer,currentInfo, haveBtn)
	else
		CommonActiveLayer.init(mScene,mLayer,currentInfo)--通用活动	界面  无领取按钮
	end	
end

function closeAllLayer()
	TodayToScene.close()
	ManorScene.close()
	prayScene.close()
	ActiveTopUpScene.closeAction()
	GoldMinerLayer.close()

--	ShengjitaScene.close()--圣吉塔
	DragonholeScene.close()--龙穴

	TopUpRebate.releaseResource()
	UpGradeScene.close()
	
	ActiveTopUpScene.releaseResource()
	BSDiscountScene.releaseResource()
	DoubleIncome.releaseResource()
	RecruitScene.releaseResource()
	CommonActiveLayer.releaseResource()
end;

function getCurrentInfo(activeId)
	if activeId == eToUpRate or activeId == UpGradeScene then
		for k,v in ipairs(listTables) do
			if v.FestivalType == activeId then
				return v
			end
		end	
	else
		if listTables[orTo] then
			return listTables[orTo]
		end
	end
	return nil
end;

--[[
function refresCurrentWin()
	sendAction(3012)
end
--]]
--发送请求
function sendAction(actionId)
	if actionId == 3012 then
		actionLayer.Action3012(mScene,nil)
	end
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if  actionId == 3012    then
		local serverInfo=actionLayer._3012Callback(pScutScene, lpExternalData)
		if  serverInfo ~= nil and serverInfo.activeTable ~= " "  then
		--	serverInfo.activeTable=addVirtualData(serverInfo.activeTable)
		
			listTables = resetData(serverInfo.activeTable)
			if listTables ~= nil and listTables ~=" " then
				showHeroHead(serverInfo.activeTable)					
			else
				ZyToast.show(pScutScene,Language._activeStr,1,0.5)
			end
		else
			ZyToast.show(pScutScene,Language._activeStr,1,0.5)						
		end	
	elseif actionId == 11001 or actionId == 11002 or actionId == 11003 then
		if g_currentActiveID ==activeTabel.eTodayTo then
			TodayToScene.networkCallback(pScutScene, lpExternalData)
		end
	elseif (actionId>=10001 and actionId<=10011)	then
		if g_currentActiveID ==activeTabel.eManor then
			ManorScene.networkCallback(pScutScene, lpExternalData)
		end
	elseif actionId ==  3301 or actionId == 3302	 then 
		if g_currentActiveID ==activeTabel.ePray then
			prayScene.networkCallback(pScutScene, lpExternalData)
		end
	elseif actionId == 9006 then
		if g_currentActiveID ==activeTabel.eTopUp then
			ActiveTopUpScene.networkCallback(pScutScene, lpExternalData)
		end
	elseif actionId == 1011 then
		if g_currentActiveID ==activeTabel.eGoldMiner then
			GoldMinerLayer.networkCallback(pScutScene, lpExternalData)
		end
	elseif actionId == 4401 or actionId == 4402 or actionId == 4403 or actionId == 4404 or actionId == 4411 or actionId==4405 or actionId==4407 or actionId==4408 or actionId==4409 or actionId==4410 then
		if g_currentActiveID ==activeTabel.eGoldMiner then
			ShengjitaScene.networkCallback(pScutScene, lpExternalData)
		end
	elseif actionId == 12101 or actionId==12102 then
		if g_currentActiveID ==activeTabel.eDragon then
			DragonholeScene.networkCallback(pScutScene, lpExternalData)
		end
	elseif actionId == 3009 or ( actionId >= 5401 and actionId <= 5408 ) then
		WorldBossToScene.networkCallback(pScutScene, lpExternalData)
	elseif actionId == 3014 then
		if ZyReader:getResult() == eScutNetSuccess then
			ZyToast.show(pScutScene,Language.ACTIVE_GETNOTICE, 1.5, 0.35)	
				refresCurrentWin()
		elseif ZyReader:getResult() == 1 then	
			local showStr = string.format(Language.ACTIVE_SuccessStr, ZyReader:readErrorMsg())
			ZyToast.show(pScutScene,showStr,1.5,0.35)
			refresCurrentWin()
		else          
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1.5,0.35)
		end
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)	
	end
	
	UpGradeScene.setIsClick(false)
end

function resetData(info)
	local mRecordTabel = {}

	for k,v in ipairs(info) do

		local isHave = false
		for m,n in ipairs(mRecordTabel) do
			if v.FestivalType == n.FestivalType and ( v.FestivalType == 18 or  v.FestivalType == 21 ) then
--				n.RecordTabel[#n.RecordTabel+1] = v
				isHave = true
				if not n.RecordTabel then
					n.RecordTabel = {}
					n.RecordTabel[#n.RecordTabel+1] = n
				end
				n.RecordTabel[#n.RecordTabel+1] = v
			end
		end
		
		if not isHave then
			mRecordTabel[#mRecordTabel+1] = v
		end
	end
	return mRecordTabel
end


