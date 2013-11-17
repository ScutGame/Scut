------------------------------------------------------------------
-- ActiveAllScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 
------------------------------------------------------------------

module("ActiveAllScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local g_scene = nil 		-- 场景
local g_Layer = nil 
local g_tListSize = nil 
local g_tList = nil 
local g_activeTabel = {}
local g_nowButtonNum = nil
local g_m_choiceImge = nil 
--[[
local activeType = {
	eToUp = 22, --- 首充奖励
	eToUpRate = 18,---充值返利
	eRanger = 20, -----招募佣兵
	eDouble = 24,----副本双倍送礼活动
	eLv = 21,--- 升级送好礼活动
	eBussice = 23 ,---商城打折活动
	eScript = 27,----精灵祝福
}
--]]
---假数据
local activeType = {
	eToUp = 1, --- 首充奖励
	eToUpRate = 2,---充值返利
	eRanger = 3, -----招募佣兵
	eDouble = 4,----副本双倍送礼活动
	eLv = 5,--- 升级送好礼活动
	eBussice = 6 ,---商城打折活动
	eScript = 7,----精灵祝福
}
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释


--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
end
-- 释放资源
function releaseResource()
	closeAllLayer()
	g_scene = nil 		-- 场景
	g_Layer = nil 
 	g_tListSize = nil 
 	g_tList = nil 
 	g_activeTabel = {}
 	g_nowButtonNum = nil
 	g_m_choiceImge = nil 
end
-- 创建场景
function init()
	local scene = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
    g_scene = scene.root
	mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	-- 添加背景
	local layer = CCLayer:create()
	g_Layer = layer
	SlideInLReplaceScene(g_scene,1)
	g_scene:addChild(layer, 0)
	-- 注册触屏事件
	layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "ActiveAllScene.touchBegan")
	layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "ActiveAllScene.touchMove")
	layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "ActiveAllScene.touchEnd")

	-- 此处添加场景初始内容
	local BgImg = CCSprite:create(P("common/list_1024.png"))
	layer:addChild(BgImg,0)
	BgImg:setScaleX(pWinSize.width/BgImg:getContentSize().width)
	BgImg:setScaleY(pWinSize.height*0.79/BgImg:getContentSize().width)
	BgImg:setAnchorPoint(PT(0,0))
	BgImg:setPosition(PT(0,pWinSize.height - pWinSize.height*0.86))
	
	
	--- 移动选择框
	local brListImgH = pWinSize.height*0.12
	local brListImg =  CCSprite:create(P("common/list_1052.9.png"))
	layer:addChild(brListImg,1)
	brListImg:setScaleY(brListImgH/brListImg:getContentSize().height)
	brListImg:setAnchorPoint(PT(0,0))
	brListImg:setPosition(PT((pWinSize.width+SX(3) - brListImg:getContentSize().width)/2,pWinSize.height*0.84))
	
	---移动框右图标
	local rightImg = CCSprite:create(P("button/list_1068.png"))
	layer:addChild(rightImg,1)
	rightImg:setAnchorPoint(PT(0,0))
	rightImg:setPosition(PT(brListImg:getPosition().x+brListImg:getContentSize().width
											,brListImg:getPosition().y+brListImgH/2-rightImg:getContentSize().height/2))
	---移动框左图标
	local LeftImg = CCSprite:create(P("button/list_1069.png"))
	layer:addChild(LeftImg,1)
	LeftImg:setAnchorPoint(PT(0,0))
	LeftImg:setPosition(PT(brListImg:getPosition().x-LeftImg:getContentSize().width
											,rightImg:getPosition().y))

	
	local  listSize = SZ(brListImg:getContentSize().width*0.98,pWinSize.height*0.11)
	local listY = brListImg:getPosition().y + brListImgH/2 - listSize.height/2
	local listX = brListImg:getPosition().x+brListImg:getContentSize().width/2 - listSize.width/2 +SX(1)
	g_tListSize = listSize
	g_tList = ScutCxList:node(listSize.width/4,ccc4(24,24,24,0),listSize)
	g_tList:setAnchorPoint(PT(0,0))
	g_tList:setPosition(PT(listX,listY))
	g_tList:setHorizontal(true)					
	layer:addChild(g_tList,1)
	g_tList:setTouchEnabled(true)
	showList()
	actionLayer.Action3012(g_scene,nil)
	MainMenuLayer.init(2, g_scene)
end
function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


---显示list
function showList()
	for  i =1 ,9 do 
		local item = ScutCxListItem:itemWithColor(ccc3(255,255,255))	--25,57,45
			item:setOpacity(50)
			local layout = CxLayout()
			layout.val_x.t = ABS_WITH_PIXEL
			layout.val_y.t = ABS_WITH_PIXEL	
			layout.val_x.val.pixel_val =0
			layout.val_y.val.pixel_val =0
			local itemLayer = CCLayer:create()
			local listW = g_tList:getRowWidth()
			local listH = pWinSize.height*0.11
			local itemImg =  CCSprite:create(P("common/list_1012.png"))
			itemLayer:addChild(itemImg,0)
			itemImg:setAnchorPoint(PT(0,0))
			itemImg:setPosition(PT(listW/2 - itemImg:getContentSize().width/2 ,(listH- itemImg:getContentSize().height)/2))
			
			
			local actionBtn=UIHelper.createActionRect(itemImg:getContentSize(),ActiveAllScene.key_head,i)
			actionBtn:setAnchorPoint(PT(0,0))
			actionBtn:setPosition(PT(0,0))
			itemImg:addChild(actionBtn,0)
			g_activeTabel[i] = actionBtn
		
			item:addChildItem(itemLayer,layout)
			g_tList:addListItem(item,false)		
	end
	if  g_nowButtonNum == nil then 
		key_head(nil,1)
	end
			
end
----头像点击
function key_head(pNode, index)
	local tag = index
	if pNode then
		tag = pNode:getTag()		
	end
	pNode = g_activeTabel[tag]
	if tag ~= g_nowButtonNum then
		releaseChoiceImge()
		g_nowButtonNum = tag
		g_m_choiceImge = CCSprite:create(P(Image.Image_choicebox))
		g_m_choiceImge:setAnchorPoint(CCPoint(0.5, 0.5))
		g_m_choiceImge:setPosition(PT(pNode:getContentSize().width*0.5, pNode:getContentSize().height*0.5))
		pNode:addChild(g_m_choiceImge,1)
		closeAllLayer()
		goAction(tag)
	end
end
---清楚选中框
function releaseChoiceImge()
	if g_m_choiceImge ~= nil  then
		g_m_choiceImge:getParent():removeChild(g_m_choiceImge,true)
		g_m_choiceImge=nil
	end
end;
--- 点击响应
function goAction(item)
	if item  == activeType.eToUpRate then 
		TopUpRebate.init(g_scene,g_Layer)
	elseif  item  == activeType.eLv then 
		UpGradeScene.init(g_scene,g_Layer)
	elseif item ==activeType.eToUp then 
		ActiveTopUpScene.init(g_scene,g_Layer)
	elseif item == activeType.eBussice then 
		BSDiscountScene.init(g_scene,g_Layer)	
	elseif item == activeType.eDouble then 
		DoubleIncome.init(g_scene,g_Layer)	
	elseif item == activeType.eRanger then
		RecruitScene.init(g_scene,g_Layer)		
	end
end

function closeAllLayer()
	TopUpRebate.releaseResource()
	UpGradeScene.releaseResource()
	ActiveTopUpScene.releaseResource()
	BSDiscountScene.releaseResource()
	DoubleIncome.releaseResource()
	RecruitScene.releaseResource()
end;
-- 触屏按下
function touchBegan(e)
end
-- 触屏移动
function touchMove(e)
end
-- 触屏弹起
function touchEnd(e)
end
-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	if  actionID == 9006  then
		ActiveTopUpScene.networkCallback(pScutScene, lpExternalData)
	if  actionID == 3012  then
		local serverInfo = actionLayer._3012Callback(pScutScene, lpExternalData)	
		if   serverInfo ~= nil  and    serverInfo ~= {}   then 
			local activeTable = serverInfo.activeTable
			
		end		
	end	
	end

end