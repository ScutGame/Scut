------------------------------------------------------------------
-- TrialScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 
------------------------------------------------------------------

module("TrialScene", package.seeall)


mScene = nil 		-- 场景


acitveId = {
	eBoss = 19,
	eShengjita = 57,
	eRelic=58,

}


-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
	releaseResource()
	
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()

end

-- 释放资源
function releaseResource()
	WorldBossToScene.releaseResource()
	ShengjitaScene.releaseResource()
	RelicScene.close()
	listLayer=nil
	_noticeLabel=nil
	mLayer = nil
	mScene = nil
	m_choiceImge=nil
	mHeadList=nil
	listTables=nil
	MainMenuLayer.refreshWin()
end

-- 创建场景
function init()
	local scene = ScutScene:new()
	mScene = scene.root
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	
	SlideInLReplaceScene(mScene,1)	
--	
--	   CCDirector:sharedDirector():pushScene(mScene)
	
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
	
	
	--提示
	local label = CCLabelTTF:create("", FONT_NAME, FONT_SM_SIZE)
	label:setAnchorPoint(PT(0.5,0))
	label:setPosition(PT(pWinSize.width*0.5, pWinSize.height*0.6))
	mLayer:addChild(label, 0)
	_noticeLabel = label	
	
	MainMenuLayer.init(2, mLayer)
	
	creatlist()
	sendAction(3009)	
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
	list:setTouchEnabled(true)
	list:setHorizontal(true)
	layer:addChild(list, 0)
	mHeadList = list
end;

--佣兵头像
function showActiceHead()
      --mHeadList:clear()
	local list = mHeadList
	list:clear()
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0

	if not listTables or #listTables == 0 then
		_noticeLabel:setString(Language.ACTIVE_NOACTIVE)
		return
	else
		_noticeLabel:setString("")
	end
	
	
	--开启上次活动
	local index = nil--上一次查看的活动还存在
	for k,v in ipairs(listTables) do
		if v.ActiveType == mActiveType then
			index = k
		end
	end	
	if not index then
		mActiveType = listTables[1].ActiveType
	end
	if not index then
		index = 1
	end
	_itemTabel = {}
	local mBgTecture=IMAGE("common/list_1012.png")
	for k,v in ipairs(listTables) do	
		local listItem = ScutCxListItem:itemWithColor(ccc3(42,28,13))
		listItem:setOpacity(0)	
		--头像 
		local image = nil
		if v.HeadID then
			image = string.format("mainUI/%s.png", v.HeadID)
		end
		local item = createMemberItem(image,k,mBgTecture)
		_itemTabel[k] = item
		listItem:addChildItem(item,layout)
		
		list:addListItem(listItem , false)
	end

	key_head(nil, index)
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
		
		local actionBtn=UIHelper.createActionRect(goodBg:getContentSize(),TrialScene.key_head, tag)
		actionBtn:setPosition(PT(0,0))
		goodBg:addChild(actionBtn,0)
	end
	
	if listTables[tag].ActiveType == mActiveType then
		releaseChoiceImge()
		m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		m_choiceImge:setPosition(PT(goodBg:getContentSize().width*0.5,goodBg:getContentSize().height*0.5))
		goodBg:addChild(m_choiceImge,1)
	end
	

	
	return layer
end

function key_head(pNode, index)
	local tag = nil
	if index then
		tag = index
	else
		tag = pNode:getTag()
	end


	
	local activeInfo = listTables[tag] 
	local ActiveType = activeInfo.ActiveType
	if pNode and ActiveType == mActiveType then
	else
		mActiveType = ActiveType
		
	if pNode then
		releaseChoiceImge()
		m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		m_choiceImge:setPosition(PT(pNode:getContentSize().width*0.5, pNode:getContentSize().height*0.5))
		pNode:addChild(m_choiceImge,1)
	end
	
		--mActiveType = activeInfo.ActiveId
		closeAllLayer()
		if ActiveType == acitveId.eBoss then
			WorldBossToScene.setData(activeInfo)
			WorldBossToScene.init(mScene, mLayer)
		elseif ActiveType==acitveId.eShengjita then
			ShengjitaScene.setLayer(mScene, mLayer)
			ShengjitaScene.init()
	elseif ActiveType==acitveId.eRelic then
		RelicScene.setLayer(mScene, mLayer)
		RelicScene.init()
		end		
		
	end

end;

function releaseChoiceImge()
	if m_choiceImge ~= nil  then
		m_choiceImge:getParent():removeChild(m_choiceImge,true)
		m_choiceImge=nil
	end
end;

function refreshWin()
	sendAction(3009)
end

function closeAllLayer()
WorldBossToScene.close()
ShengjitaScene.close()
RelicScene.close()
end


--发送请求
function sendAction(actionId)
	if actionId == 3009 then
		actionLayer.Action3009(mScene, nil)
	elseif actionId == 2 then
	


	end

end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionId == 3009 then
		local serverInfo = actionLayer._3009Callback(pScutScene, lpExternalData)
		if serverInfo then
			listTables = serverInfo.RecordTabel
			showActiceHead()
		end
	elseif actionId == 2 then

	elseif actionId >= 5401 and actionId <= 5408  then
		WorldBossToScene.networkCallback(pScutScene, lpExternalData)
	elseif actionId == 4401 or actionId == 4402 or actionId == 4403 or actionId == 4404 or actionId == 4411 or actionId==4405 or actionId==4407 or actionId==4408 or actionId==4409 or actionId==4410 then
--		if g_currentActiveID ==activeTabel.eGoldMiner then
			ShengjitaScene.networkCallback(pScutScene, lpExternalData)
--		end
	elseif actionId==12057 then
		RelicScene.networkCallback(pScutScene, lpExternalData)	
	end
end