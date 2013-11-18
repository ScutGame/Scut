------------------------------------------------------------------
-- BoxToScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 开宝箱界面
------------------------------------------------------------------

module("BoxToScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local g_mScene = nil 		-- 场景
local g_userItemID = nil  ---  宝箱的ID
local g_rollInfo = nil    --- 接收12001的返回
local g_startButton = nil   ----   
local g_mLayer = nil 
local g_mBox_X = pWinSize.width*0.92  -------- 格子的宽度( 总的) 
local g_mBox_Y = pWinSize.height*0.5  -------- 格子的高度( 总的)
local g_gapY = SY(5) ----- 空白高度
local g_gapX = SX(5) ----- 空白宽度
local g_box_X = (g_mBox_X-g_gapX*6)/5
local g_box_Y = (g_mBox_Y-g_gapY*3)/4


--左下点为坐标原点
startY = { 1, 2, 3, 4, 4, 4, 4,4, 3, 2, 1, 1, 1,1}
startX = { 1, 1, 1, 1, 2, 3, 4,5, 5, 5, 5, 4, 3,2}


-- 场景入口
function pushScene()
	initResource()
	init()
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
mLayer = nil 
rollInfo	= nil 
userItemID = nil  ---  宝箱的ID
mBox_X = pWinSize.width*0.92  -------- 格子的宽度( 总的) 
mBox_Y = pWinSize.height*0.5  -------- 格子的高度( 总的)

	m_Left_width = pWinSize.width*0.92
	m_Left_height = pWinSize.height
	m_Left_x = SX(20)
	m_Left_y = pWinSize.height*0.06
	
	m_right_width = pWinSize.width-m_Left_width-m_Left_x*2-SX(5)
	m_right_height = m_Left_height
	m_right_x = m_Left_x+m_Left_width+SX(5)
	m_right_y = m_Left_y+m_Left_height-m_right_height
	

	m_rightTop_width = m_right_width*0.94
	m_rightTop_height = m_right_height*0.35
	m_rightTop_x = (m_right_width-m_rightTop_width)*0.5
	m_rightTop_y = m_right_height*0.55

	m_rightEnd_width = m_rightTop_width
	m_rightEnd_height = m_right_height*0.35
	m_rightEnd_x = m_rightTop_x
	m_rightEnd_y = m_right_height*0.1

end

-- 释放资源
function releaseResource()

	rollInfo	= nil 
userItemID = nil  
mBox_X = nil
mBox_Y = nil
mScene = nil 
	m_Left_width = nil
	m_Left_height = nil
	m_Left_x =nil
	m_Left_y = nil
	
	m_right_width = nil
	m_right_height = nil
	m_right_x = nil
	m_right_y = nil
	


	m_rightTop_width = nil
	m_rightTop_height = nil
	m_rightTop_x = nil
	m_rightTop_y = nil

	m_rightEnd_width = nil
	m_rightEnd_height = nil
	m_rightEnd_x = nil
	m_rightEnd_y = nil
	priceNum = nil 
	userItemID = nil 
	closeRollLayers()
	if mLayer then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end
	
	
end

-- 创建场景
--function init(Mscense,Flayer,rollInfoF,userItemIDF)
function init(Mscense,userItemIDF)
	if mLayer then
		return
	end
	initResource()	
--	rollInfo	=rollInfoF
	mScene = Mscense
	userItemID = userItemIDF

	-- 添加主层
	mLayer= CCLayer:create()
	mScene:addChild(mLayer,4)

	
	local bgButton=ZyButton:new(Image.image_toumingPath)
	bgButton:setScaleX(pWinSize.width/bgButton:getContentSize().width)
	bgButton:setScaleY(pWinSize.height/bgButton:getContentSize().width)
	bgButton:setAnchorPoint(PT(0,0))
	bgButton:setPosition(PT(0,0))	
	bgButton:addto(mLayer,-1)
	
	
	--创建背景
	local background = CCSprite:create(P( "common/list_1076.png" ));
	
	background:setAnchorPoint(PT(0,0))
	background:setPosition(PT(0,0))
	mLayer:addChild(background,0)
	
	
	--背景图片	
	local midSprite = CCSprite:create(P( "activeBg/list_1078.jpg" ));
	midSprite:setScaleX(pWinSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(pWinSize.height/midSprite:getContentSize().height)
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setPosition(PT(pWinSize.width/2,0))
	mLayer:addChild(midSprite,0)
	
	
	local background2 = CCSprite:create(P( "common/list_1074.png" ));
	background2:setScaleX(pWinSize.width/background2:getContentSize().width)
	background2:setScaleY(pWinSize.height/background2:getContentSize().height)
	
	background2:setAnchorPoint(PT(0,0))
	background2:setPosition(PT(0,0))
	mLayer:addChild(background2,0)
	





	local button1 = "button/bottom_1002_1.9.png"
	local button2 = "button/bottom_1002_2.9.png"
	local button3  = "button/bottom_1002_3.9.png"
	startButton = ZyButton:new(button2,button1,button3,nil,FONT_NAME,FONT_SM_SIZE)
	startButton:setAnchorPoint(PT(0.5,0))
	startButton:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.22))
	startButton:addto(mLayer,1)
	startButton:registerScriptHandler(rollAction)
	local bg1 = Language.start_string
	if priceNum~=nil and rollInfo.RecordTabel[priceNum] ~=nil   then
		 
		startButton:setEnabled(false)
	
	end	
	local bg2 =Language.close_string
	local closeButton = ZyButton:new(button2,button1,button3,nil,FONT_NAME,FONT_SM_SIZE)
	closeButton:setAnchorPoint(PT(0.5,0))
	closeButton:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.12))
	closeButton:addto(mLayer,1)
	closeButton:registerScriptHandler(closeAction)
	
	
	local startImg1 = CCLabelTTF:create(bg2,FONT_NAME,FONT_SM_SIZE)
	mLayer:addChild(startImg1,1)
	startImg1:setColor(ccc3(250,250,250))
	startImg1:setAnchorPoint(PT(0.5,0))
	startImg1:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.14))
	
	local startImg = CCLabelTTF:create(bg1,FONT_NAME,FONT_SM_SIZE)
	mLayer:addChild(startImg,1)
	startImg:setColor(ccc3(250,250,250))
	startImg:setAnchorPoint(PT(0.5,0))
	startImg:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.24))
	
	actionLayer.Action12001(mScene,nil ,userItemID)
--	showLeft()
	
end
function closeAction()
	priceNum = nil 
	BagScene.showLayer()
	popScene()
end

----------------------------
--关闭摇奖界面
function closeRollLayers()
	closeLeft()
end


--关闭左
function closeLeft()
	if choiceImage then
		choiceImage:getParent():removeChild(choiceImage, true)
		choiceImage = nil
	end
	if leftLayer ~= nil then
		leftLayer:getParent():removeChild(leftLayer, true)
		leftLayer = nil
	end
end;


--左边
function showLeft()
	closeLeft()

	
	
	local layer = CCLayer:create()

	
	--免费抽奖次数
	
	local backgroundX = pWinSize.width*0.92
	local backgorundY = pWinSize.height*0.92

	
	
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT((pWinSize.width-backgroundX)/2, pWinSize.height*0.06))	
	mLayer:addChild(layer,0)

	leftLayer = layer
	
	
		----- 宝箱抽奖标题背景
	local bg1 = "common/list_1080.png"
	local background3  =  CCSprite:create(P(bg1))
	background3:setAnchorPoint(PT(0.5,0))
	
	background3:setPosition(PT(backgroundX/2,backgorundY-SY(45)))
	layer:addChild(background3,2)
	
	-- 宝箱抽奖标题图片
	local bg1 = "title/list_1109.png"
	local background4 =  CCSprite:create(P(bg1))
	background4:setAnchorPoint(PT(0.5,0.5))
	background4:setPosition(PT(background3:getPosition().x,
										background3:getPosition().y+background3:getContentSize().height/2))
	layer:addChild(background4,2)
		
	local rollBg = CCSprite:create(P(Image.image_zhenfa_beijing));
	
	local colW=backgroundX/6
	local roll_x =colW/2
	local roll_y = background4:getPosition().y - pWinSize.height*0.5 - SY(20)
	

	
	
	priceBox = {}
	for i=1,14 do
		local image = nil
		local Desc = nil
		if rollInfo.RecordTabel[i] then
			--smallitem
			image = string.format("smallitem/%s.png", rollInfo.RecordTabel[i].HeadID)
			Desc = rollInfo.RecordTabel[i].ItemDesc
		end
		local item = createItemButton(image,i,Desc,nil)
		local posX, posY = startX[i], startY[i]
		posX =colW*(posX-1) +roll_x
		posY = g_mBox_Y/4*(posY-1)+roll_y
		item:setAnchorPoint(PT(0,0))
		item:setPosition(PT(posX, posY))
		layer:addChild(item, 0)
		
		priceBox[i] = item
	
	end
	
	
	----中间的中奖框
	local item = CCSprite:create(P(Image.image_zhenfa_beijing));
	item:setAnchorPoint(PT(0,0))
	item:setPosition(PT(m_Left_width*0.5-item:getContentSize().width*0.5, m_Left_height*0.5-item:getContentSize().height*0.2))
	layer:addChild(item, 0)
	
	--获得奖励	
	if priceNum ~= nil and rollInfo.RecordTabel[priceNum].HeadID ~= nil  then
		local image = string.format("smallitem/%s.png", rollInfo.RecordTabel[priceNum].HeadID )
		item = createItemButton(image,nil,nil,nil)
		item:setAnchorPoint(PT(0,0))
		item:setPosition(PT(m_Left_width*0.5-item:getContentSize().width*0.5, m_Left_height*0.5-item:getContentSize().height*0.2))
		layer:addChild(item, 0)
		
		delayExec("BoxToScene.reshbox",1.5)
		
		if rollInfo.RecordTabel[priceNum].ItemDesc  then
			local prayStr=rollInfo.RecordTabel[priceNum].ItemDesc
			prayLabel = CCLabelTTF:create(prayStr, FONT_NAME, FONT_SM_SIZE)
			prayLabel:setAnchorPoint(PT(0.5,0))
			prayLabel:setPosition(PT(item:getPosition().x+item:getContentSize().width*0.5, item:getPosition().y-prayLabel:getContentSize().height*1.2))
			layer:addChild(prayLabel, 0)
		end
	
	end	
	
end

function reshbox()
	if has==1 then
		closeRollLayers()
--		if mLayer then
--			mLayer:getParent():removeChild(mLayer, true)
--			mLayer = nil
--		end
		has=nil
		priceNum=nil
		startButton:setEnabled(true)
		actionLayer.Action12001(mScene,nil ,userItemID)
--		closeAction()
	end
end;

--创建带背景框的图片
function createItemButton(image,tag,Desc,nScale)
	-- 背景
	
	local bgSprite=CCSprite:create(P(Image.image_zhenfa_beijing))
	
	local imageSprite = CCSprite:create(P(image))
	imageSprite:setAnchorPoint(CCPoint(0.5, 0.5))
	imageSprite:setPosition(PT(bgSprite:getContentSize().width*0.5,bgSprite:getContentSize().height*0.5))
	bgSprite:addChild(imageSprite,0)
		
	--描述
	if Desc ~= nil then
		local DescLabel = CCLabelTTF:create(Desc, FONT_NAME, FONT_FM_SIZE)
		DescLabel:setAnchorPoint(PT(0.5,1))
		DescLabel:setPosition(PT(bgSprite:getContentSize().width*0.5, 0))
		bgSprite:addChild(DescLabel,0)	
	end
	
	return bgSprite
end


-----------------------------------
function rollAction()
	
	setBtnState(false)
	actionLayer.Action12004(mScene, nil, 1)
end

function startAction(priceNumF,HasNextBox)
	priceNum = priceNumF
	time = 0
	coldTime = 0.03
	
	has=HasNextBox
--	if has==1 then
--		startButton:setIsEnabled(true)
--	end
	selectAI()
end

--奖励选中AI
function selectAI()
	if choiceImage == nil then
		choiceImage = CCSprite:create(P(Image.Image_choicebox))
		choiceImage:setAnchorPoint(PT(0.5, 0.5))
		leftLayer:addChild(choiceImage,0)	        
		choiceNum = 0
	end
	
	if choiceNum == 14 then
		choiceNum = 1
		time = time+1
	else
		choiceNum = choiceNum+1
	end

	local pos = PT(priceBox[choiceNum]:getPosition().x+priceBox[choiceNum]:getContentSize().width*0.5, priceBox[choiceNum]:getPosition().y+priceBox[choiceNum]:getContentSize().height*0.5)	
	choiceImage:setPosition(PT(pos.x, pos.y))

	if time >= 4 and choiceNum == priceNum then
		delayExec("BoxToScene.getNext",1)
	else
		delayExec("BoxToScene.selectAI", coldTime, nil)
	end
end



function getNext()
	
	showLeft()
end;

---延迟进行方法

function delayExec(funName,nDuration,parent)
	local  action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFunc:create(funName));
	local layer = mLayer
	if parent then
		layer=parent
	end
	if layer then
		layer:runAction(action)
	end
end

-----------------------------------------------
--发送请求
function sendAction(actionId)
	if actionId == 12001 then--12001_幸运转盘界面接口
		actionLayer.Action12001(mScene, false,userItemID)
		
	elseif actionId == 12004 then--12004_抽奖接口
		actionLayer.Action12004(mScene, false, 1)
	
	end
end

function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	if actionID==12001 then
		local serverInfo = actionLayer._12001Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			rollInfo = serverInfo
			showLeft()
		end
	elseif actionID == 12004 then--12004_抽奖接口
		local serverInfo=actionLayer._12004Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			local priceNum = serverInfo.Postion 
			local HasNextBox = serverInfo.HasNextBox
			BoxToScene.startAction(priceNum,HasNextBox)
			
		else
			BoxToScene.setBtnState(true)
		end
		MainMenuLayer.refreshWin()
	end
	
end


function setBtnState(value)
	if value == true then
		
			startButton:setEnabled(true)
		
		
		if rollInfo.FreeNum <= 0 then
			startButton:setEnabled(false)
		end
	else	
			
			startButton:setEnabled(false)
				
	end
end
