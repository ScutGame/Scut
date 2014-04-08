------------------------------------------------------------------
-- RollScene.lua
-- Author     : Zonglin Liu
-- Version    : 1.0
-- Date       :   
-- Description: 转盘系统
------------------------------------------------------------------

module("RollScene", package.seeall)


mScene = nil 		-- 场景



-- 场景入口
function pushScene()
	initResource()
	createScene()
	
end

-- 退出场景
function popScene()
	if getLayer ~= nil then
		getLayer:getParent():removeChild(getLayer, true)
		getLayer = nil
	end
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil	
	end
	g_middleNode:stopAllActions()
	releaseResource()
	CCDirector:sharedDirector():popScene()
end

--

function startAction()
	if not isClick then
		isClick = true
		local ops =2--1：免费抽奖2：元宝抽奖3：确认抽奖 
		
		if mDataInfo.FreeNum > 0 then--免费次数
			ops = 1
		end
		sendAction(12002, ops)
	end
end

function askIsStart(index, content, tag)
	if index == 1 then
		sendAction(12002, 3)
	else
		isClick = false
	end
end

--显示获得的奖励
function showGet()
	if not mLayer then
		return
	end
	if getLayer ~= nil then
		getLayer:getParent():removeChild(getLayer, true)
		getLayer = nil
	end

	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	getLayer = layer
	
	--背景
--	local detailBg= CCSprite:create(P("common/panle_1014_1.png"));
--	detailBg:setAnchorPoint(PT(0,0))
--	detailBg:setPosition(PT((pWinSize.width-detailBg:getContentSize().width)/2,(pWinSize.height-detailBg:getContentSize().height)/2))
--	layer:addChild(detailBg,0)
	
	--背景
	local detailBg= CCSprite:create(P("common/panle_1015.9.png"));
	detailBg:setAnchorPoint(PT(0,0))
	detailBg:setScaleX(pWinSize.width*0.5/detailBg:getContentSize().width)
	detailBg:setScaleY(pWinSize.height*0.6/detailBg:getContentSize().height);
	detailBg:setPosition(PT((pWinSize.width-pWinSize.width*0.5)/2,(pWinSize.height-pWinSize.height*0.6)/2))
	layer:addChild(detailBg,0)

	--屏蔽
	local unTouch = UIHelper.createActionRect(SZ(pWinSize.width*0.5, pWinSize.height*0.6))
	unTouch:setPosition(detailBg:getPosition())
	layer:addChild(unTouch,0)

	--光背景
	local lightBg= CCSprite:create(P("roll/panle_1020_1.png"));
	lightBg:setAnchorPoint(PT(0.5,0.5))
	lightBg:setPosition(PT(pWinSize.width/2,detailBg:getPosition().y+pWinSize.height*0.5*0.7))
	layer:addChild(lightBg,0)	
	--恭喜你文字图片
	local labelBg= CCSprite:create(P("roll/panle_1020_2.png"));
	labelBg:setAnchorPoint(PT(0.5,0.5))
	labelBg:setPosition(PT(pWinSize.width/2,detailBg:getPosition().y+pWinSize.height*0.5*0.75))
	layer:addChild(labelBg,0)
	
	
	
	--奖励描述
	local priceLabel = CCLabelTTF:create(mRewardContent or "", FONT_NAME, FONT_FMM_SIZE)
	priceLabel:setAnchorPoint(PT(0.5,0))
	priceLabel:setColor(ccc3(241,176,63))
	priceLabel:setPosition(PT(pWinSize.width/2, detailBg:getPosition().y+pWinSize.height*0.5*0.42))
	layer:addChild(priceLabel, 0)	
	
	
	--确定按钮
	local OKBtn =ZyButton:new("button/button_1013.png","button/button_1014.png",nil, Language.IDS_OK,FONT_NAME,FONT_SM_SIZE)
	OKBtn:setAnchorPoint(PT(0.5,0))
	OKBtn:setPosition(PT(pWinSize.width/2,detailBg:getPosition().y+SY(15)))
	OKBtn:registerScriptTapHandler(RollScene.closeGetLayer)
--	OKBtn:addImage("title/button_1027.png")
	OKBtn:addto(layer, 0)

end;

function rollAction(num)

	if currentIndex == 10 then
		currentIndex = 0
	end
	
	local aciton1 = CCRotateBy:create(2, 1080+(num-currentIndex)*36)
	local action2 = CCCallFuncN:create(showGet)
	local action = CCSequence:createWithTwoActions(aciton1, action2)
	
	
	g_middleNode:runAction(action)
	
	
	currentIndex = num
end

--个人档案
function toPersonalFile()
	PersonalFileScene.createScene()
end;
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	currentIndex = 1
	middlePoint = PT(pWinSize.width*0.5, pWinSize.height*0.5)
	mRewardContent = nil
	mDataInfo = nil
	isClick = nil	
end

-- 释放资源
function releaseResource()
	mLayer = nil
	mScene = nil
	middlePoint=nil
	showLayer = nil
	getLayer = nil
	mRewardContent = nil
	mDataInfo = nil
	g_freeLabel = nil
	g_middleNode = nil
	isClick = nil
	gameCoinText=nil
	goldText=nil
end

-- 创建场景
function createScene()
	local scene = ScutScene:new()
	mScene = scene.root
--	mScene:registerOnExit("RollScene.releaseResource")	
	CCDirector:sharedDirector():pushScene(mScene)

	-- 注册网络回调
	scene:registerCallback(networkCallback)
	
	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer, 0)
	--创建背景
	-- 此处添加场景初始内容
	background()

	head()
	
	goldNum()
	
		--开始按钮
	local startBtn = ZyButton:new("roll/panle_1020_3.png",nil,"roll/panle_1020_4.png")
	startBtn:setAnchorPoint(PT(0.5,0.5))
	startBtn:setPosition(middlePoint)	
	startBtn:addto(mLayer, 0)
	startBtn:registerScriptTapHandler(startAction)	
	
	--返回按钮
	local backBtn = ZyButton:new("button/icon_1027.png")
	backBtn:setAnchorPoint(PT(0.5,0.5))
	backBtn:setPosition(PT(pWinSize.width*0.98-backBtn:getContentSize().width/2, pWinSize.height*0.93))	
	backBtn:addto(mLayer, 0)
	backBtn:registerScriptTapHandler(popScene)
	sendAction(12001)	
end


function background()
	local layer = mLayer
	
	--大背景
	local imageBg = CCSprite:create(P("common/beijing_1.jpg"))
	imageBg:setScaleX(pWinSize.width/imageBg:getContentSize().width)
	imageBg:setScaleY(pWinSize.height/imageBg:getContentSize().height)
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(0,0))
	layer:addChild(imageBg, 0)


	--转盘背景
	local rollBg = CCSprite:create(P("roll/panle_1020_7.png"))
	rollBg:setAnchorPoint(PT(0.5,0.5))
	rollBg:setPosition(middlePoint)
	layer:addChild(rollBg, 0)
	
	--中间背景
	local middleSprite = CCSprite:create(P("roll/panle_1020_6.png"))

	
	--转动中心
	local item = CCNode:create()	
	item:setAnchorPoint(PT(0.5,0.5))
	item:setPosition(middlePoint)
	layer:addChild(item, 0)
	
	--转动框
	local imageBg = CCSprite:create(P("roll/panle_1020_5.png"))
	imageBg:setAnchorPoint(PT(0,0))
	imageBg:setPosition(PT(-middleSprite:getContentSize().width*0.03, middleSprite:getContentSize().height*0.35))
	item:addChild(imageBg, 0)
	
	
	middleSprite:setAnchorPoint(PT(0.5,0.5))
	middleSprite:setPosition(middlePoint)
	layer:addChild(middleSprite, 0)
	
	
	--剩余免费次数
	local middleLabel = CCLabelTTF:create(Language.ROLL_FREE, FONT_NAME, FONT_FMMM_SIZE)
	middleLabel:setAnchorPoint(PT(0.5,1))
	middleLabel:setPosition(PT(middlePoint.x, middlePoint.y-imageBg:getContentSize().height*0.43))
	layer:addChild(middleLabel, 0)

	g_freeLabel = middleLabel

	g_middleNode = item
	
--	g_middleNode:setRotation(36*(currentIndex-1))	

end;

function head()
	local m_personInfo=PersonalInfo.getPersonalInfo()
	if m_personInfo._Pid~=nil then
		--头像背景框
		local headBg=CCSprite:create(P("common/panel_1001_6.png"))
		headBg:setAnchorPoint(PT(0,0))
		headBg:setPosition(PT(0,pWinSize.height-headBg:getContentSize().height))
		mLayer:addChild(headBg)

		
		
		local myHead=string.format("headImg/%s.png",m_personInfo._HeadIcon)
		local headSprite=CCSprite:create(P(myHead))
		headSprite:setAnchorPoint(PT(0.5,0.5))
		headSprite:setPosition(PT(headBg:getContentSize().width/2,headBg:getContentSize().height/2))
		headBg:addChild(headSprite)
		
		
		local actionBtn=UIHelper.createActionRect(headBg:getContentSize(),toPersonalFile)
		actionBtn:setPosition(PT(0,0))
		headBg:addChild(actionBtn)

		
		--玩家名称
		local nameBg= CCSprite:create(P(Image.image_nameBg));
		nameBg:setAnchorPoint(PT(0,0))
		nameBg:setScaleX(pWinSize.width*0.3/nameBg:getContentSize().width)
		nameBg:setPosition(PT(headBg:getPosition().x+headBg:getContentSize().width,
						pWinSize.height-nameBg:getContentSize().height))
		mLayer:addChild(nameBg,0)
		
		local nameText=CCLabelTTF:create(m_personInfo._NickName,FONT_NAME,FONT_SM_SIZE);
		nameText:setAnchorPoint(PT(0.5,0.5))
		nameText:setPosition(PT(nameBg:getPosition().x+pWinSize.width*0.13,
						nameBg:getPosition().y+nameBg:getContentSize().height/2))
		mLayer:addChild(nameText)
		if nameText:getContentSize().width>pWinSize.width*0.3 then
			nameBg:setScaleX(nameText:getContentSize().width/nameBg:getContentSize().width)	
		end
	end
end


function goldNum()
		local layer = mLayer
		local posY = pWinSize.height*0.045

	----
		local bottomSprite=CCSprite:create(P("common/panel_1001_4.png"))
		bottomSprite:setAnchorPoint(PT(0,0))
		bottomSprite:setScaleX(pWinSize.width/bottomSprite:getContentSize().width)
		bottomSprite:setPosition(PT(0,pWinSize.height*0.05))
		layer:addChild(bottomSprite,0)
	 
		--金豆
		local gameCoinImg= CCSprite:create(P("common/icon_1031.png"));
		gameCoinImg:setAnchorPoint(PT(0,0.5))
		gameCoinImg:setPosition(PT(pWinSize.width*0.2, posY))
		layer:addChild(gameCoinImg,0)
		
		local gameCoinStr=PersonalInfo.getPersonalInfo()._GameCoin
		gameCoinText=CCLabelTTF:create(gameCoinStr,FONT_NAME,FONT_SM_SIZE);
		gameCoinText:setAnchorPoint(PT(0,0.5))
		gameCoinText:setPosition(PT(gameCoinImg:getPosition().x+gameCoinImg:getContentSize().width+SX(1),
				gameCoinImg:getPosition().y))
		layer:addChild(gameCoinText, 0)
		

		
		--元宝
		local gameCoinImg= CCSprite:create(P("common/panle_1067.png"));
		gameCoinImg:setAnchorPoint(PT(0,0.5))
		gameCoinImg:setPosition(PT(pWinSize.width*0.65, posY))
		layer:addChild(gameCoinImg,0)
		
		local goldStr=PersonalInfo.getPersonalInfo()._Gold
		goldText=CCLabelTTF:create(goldStr,FONT_NAME,FONT_SM_SIZE);
		goldText:setAnchorPoint(PT(0,0.5))
		goldText:setPosition(PT(gameCoinImg:getPosition().x+gameCoinImg:getContentSize().width+SX(1),
				gameCoinImg:getPosition().y))
		layer:addChild(goldText, 0)		
		
		
end










function showPrices()
	if showLayer ~= nil then
		showLayer:getParent():removeChild(showLayer, true)
		showLayer = nil	
	end
	local layer = CCLayer:create()
	mLayer:addChild(layer, 0)
	
	showLayer = layer
	
	local rollBg = CCSprite:create(P("roll/panle_1020_7.png"))
	local height = rollBg:getContentSize().height*0.375
	
	for k,v in ipairs(mDataInfo.RecordTabel) do
		priceItem(v, layer, height)
	end

	g_freeLabel:setString(Language.ROLL_FREE..Language.IDS_COLON..mDataInfo.FreeNum )
end

function priceItem(info, layer, height)
	local item = CCNode:create()
	item:setAnchorPoint(PT(0,0))
	item:setPosition(middlePoint)
	layer:addChild(item, 0)	
	
	
	local picture = CCSprite:create(P("common/icon_1031.png"))
	picture:setAnchorPoint(PT(0.5,0))
	picture:setPosition(PT(0, height))
	item:addChild(picture, 0)
	
	
	local coinStr = info.GameCoin or ""
	local coinNum = CCLabelTTF:create(coinStr, FONT_NAME, FONT_SMM_SIZE)
	coinNum:setAnchorPoint(PT(0.5,0))
	coinNum:setPosition(PT(0, height-coinNum:getContentSize().height))
	item:addChild(coinNum, 0)	
	

	
	local num = info.Postion 
	item:setRotation(36*(num-1)+18)	
--	return item
end



function closeGetLayer()
	if getLayer ~= nil then
		getLayer:getParent():removeChild(getLayer, true)
		getLayer = nil
	end
	isClick = false
	if mDataInfo then
		gameCoinText:setString(mDataInfo.UserCoin)--金豆
	end
end




function setNewManInput(clickedButtonIndex)
	if clickedButtonIndex ==1 then
		popScene()
		TopUpScene.init()
	else
		isClick = false
	end
end;

--发送请求
function sendAction(actionId, ops)
	if actionId == 12001 then
		actionLayer.Action12001(mScene, false)
	elseif actionId == 12002 then
		actionLayer.Action12002(mScene, false, ops)
	end

end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID()
	local userData = ZyRequestParam:getParamData(lpExternalData)
	if actionId == 12001 then
		local serverInfo = actionLayer._12001Callback(pScutScene, lpExternalData)
		if serverInfo then
			mDataInfo = serverInfo
			showPrices()
			gameCoinText:setString(serverInfo.UserCoin)--金豆
			goldText:setString(serverInfo.UserGold)--元宝
		end
	elseif actionId == 12002 then
		if ZyReader:getResult() == 1 or ZyReader:getResult() == 3 then
		--	sendAction(12001)
		local DataTabel={}
		DataTabel.Postion = ZyReader:getWORD()
		DataTabel.RewardContent = ZyReader:readString()
		DataTabel.FreeNum = ZyReader:getInt()
		DataTabel.UserCoin= ZyReader:getInt()
		DataTabel.UserGold= ZyReader:getInt()	
		
		mRewardContent = DataTabel.RewardContent
		mDataInfo = DataTabel
		
		rollAction(DataTabel.Postion)
		
		g_freeLabel:setString(Language.ROLL_FREE..Language.IDS_COLON..DataTabel.FreeNum )
		
		goldText:setString(mDataInfo.UserGold)--元宝
		
		PersonalInfo.getPersonalInfo()._GameCoin = DataTabel.UserCoin
		PersonalInfo.getPersonalInfo()._Gold = DataTabel.UserGold
		
		elseif ZyReader:getResult() == 2 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene,nil, ZyReader:readErrorMsg(),Language.IDS_SURE,Language.IDS_CANCEL,askIsStart)
		elseif ZyReader:getResult() == 4 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene,nil, Language.SHOP_BUZUCHONGZHI,Language.IDS_OK,Language.IDS_CANCEL,setNewManInput)
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg(),0.8,0.35)

			isClick = false
		end
	end
end