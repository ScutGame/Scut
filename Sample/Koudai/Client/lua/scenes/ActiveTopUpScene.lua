------------------------------------------------------------------
-- ActiveTopUpScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 首充奖励界面
------------------------------------------------------------------

module("ActiveTopUpScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local g_scene = nil 		-- 场景
local g_imgTabel = {}
local g_mListColWidth = nil 
local choiceLayer = nil 
local g_LayerBG = nil 
--
---------------公有接口(以下两个函数固定不用改变)--------

function setData(info)
	g_DataInfo = info
end;
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	createScene()
---	CCDirector:sharedDirector():pushScene(_scene)
end
-- 退出场景
function popScene()
	releaseResource()
--	CCDirector:sharedDirector():popScene()
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	g_scene = nil 		-- 场景
	 g_imgTabel = {}
	 g_mListColWidth = nil 
	 choiceLayer = nil 
end
-- 释放资源
function releaseResource()
	closeAction()
	g_scene = nil 		-- 场景
	g_imgTabel = {}
	g_mListColWidth = nil 
	choiceLayer = nil
	g_gainButton=nil
	g_DataInfo=nil
end
-- 创建场景
function init(mScene,mLayer)
	g_scene = mScene
	-- 注册网络回调
	-- 添加背景
	g_LayerBG = CCLayer:create()
	mLayer:addChild(g_LayerBG,1)
	-- 注册触屏事件
--	g_LayerBG.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "ActiveTopUpScene.touchBegan")
--	g_LayerBG.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "ActiveTopUpScene.touchMove")
--	g_LayerBG.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "ActiveTopUpScene.touchEnd")
	
	local tBgImg = CCSprite:create(P("common/list_1038.9.png"))
	tBgImg:setScaleX(pWinSize.width*0.92/tBgImg:getContentSize().width)
	tBgImg:setScaleY(pWinSize.height*0.6/tBgImg:getContentSize().height)
	tBgImg:setAnchorPoint(PT(0.5,0))
	tBgImg:setPosition(PT(pWinSize.width*0.5,pWinSize.height-pWinSize.height*0.6-SY(32)))
	g_LayerBG:addChild(tBgImg,0)
	
	local titleImg = CCSprite:create(P("title/list_1149.png"))
	titleImg:setAnchorPoint(PT(0.5,0))
	titleImg:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.75))
	g_LayerBG:addChild(titleImg,0)
	
	local contentWidth = pWinSize.width*0.8
	local contentSize = SZ(pWinSize.width*0.8,pWinSize.height*0.3)
	local topUpStr = ZyMultiLabel:new(Language.ACTIVE_TOPUPSTR,contentWidth,FONT_NAME,FONT_SM_SIZE,nil,nil)
	topUpStr:setAnchorPoint(PT(0,0))
	topUpStr:setPosition(PT((pWinSize.width-contentWidth)/2,titleImg:getPosition().y-SY(30)))
	topUpStr:addto(g_LayerBG,0)
	
	
	local m_showChoice_Height = pWinSize.height* 0.2
	local m_showChoice_StartY = (topUpStr:getPosition().y-tBgImg:getPosition().y-m_showChoice_Height)/2+tBgImg:getPosition().y
	
	local brImg = CCSprite:create(P("common/list_1052.9.png"))
	brImg:setScaleY(m_showChoice_Height/brImg:getContentSize().height)
	brImg:setAnchorPoint(PT(0.5,0))
	brImg:setPosition(PT(pWinSize.width*0.5,m_showChoice_StartY))
	g_LayerBG:addChild(brImg,0)

	local col=4
	local listSize=SZ(brImg:getContentSize().width*0.96,pWinSize.height*0.2)		
	g_mListColWidth= listSize.width/col
	local list = ScutCxList:node(g_mListColWidth, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setHorizontal(true)	 
	list:setTouchEnabled(true)
	list:setPosition(PT(brImg:getPosition().x-listSize.width/2+SX(1),brImg:getPosition().y))
	g_LayerBG:addChild(list,1)
	mList=list

	--领取按钮
	local gainButton=ZyButton:new(Image.image_button_red_c_0, Image.image_button_red_c_1, Image.image_button_hui_c, Language.ACTIVE_BUTTONSTR2)
	gainButton:setAnchorPoint(PT(0.5,0.5))
	gainButton:setPosition(PT(pWinSize.width*0.5,m_showChoice_StartY-SY(37)))
	gainButton:addto(g_LayerBG,0)
	gainButton:registerScriptHandler(gotoAction)
	gainButton:setEnabled(false)
--	
	g_gainButton = gainButton

	
	
	actionLayer.Action9006(g_scene, false)
end



function gotoAction()
	actionLayer.Action3014(g_scene, nil, g_DataInfo.FestivalID )
end


function closeAction()
	if choiceLayer ~= nil then
		choiceLayer:getParent():removeChild(choiceLayer,true)
		choiceLayer = nil 
	end
	if g_LayerBG ~= nil  then 
		g_LayerBG:getParent():removeChild(g_LayerBG,true)
		g_LayerBG = nil 
	end
end


---
function showListImg(table)
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0
	for k, v in pairs(table) do
			v.HeadID=v.HeadID or "icon_4109"
			local picPath=string.format("smallitem/%s.png",v.HeadID)
			local name =v.ItemName	 or Language.DAILY_REWARDTYPE_[v.Type]
			--1	金币 2	声望 3	阅历 4	精力 5	经验 6	晶石 7	物品
			local picPathTable={
			[1]="smallitem/icon_8012.png",
			[3]="smallitem/icon_8011.png",
			[4]="smallitem/icon_4094.png",
			[6]="smallitem/icon_8010.png",
			}
			picPath=picPathTable[v.Type] or picPath
			local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local itemLayer = createImg(picPath,name,v.Num,k)
			
			listItem:addChildItem(itemLayer, layout)
			mList:addListItem(listItem, false)
	end
	
	if g_DataInfo and g_DataInfo.IsReceive == 1 then
		g_gainButton:setEnabled(true)
	end
end

--
function createImg(img,str,num,k)
		local layer = CCLayer:create()
		
		local itemColW=mList:getRowWidth() 
		local itemHeight=mList:getContentSize().height 

		local bgSmallImg = CCSprite:create(P(Image.Image_normalItemBg))
		bgSmallImg:setAnchorPoint(PT(0.5,0))
		bgSmallImg:setPosition(PT(g_mListColWidth/2,
							(itemHeight-bgSmallImg:getContentSize().height)/2))
		layer:addChild(bgSmallImg,0)

		
		local smallImg = CCSprite:create(P(img))
		smallImg:setAnchorPoint(PT(0.5, 0.5))
		smallImg:setPosition(PT(bgSmallImg:getContentSize().width/2,
					bgSmallImg:getContentSize().height/2))
		bgSmallImg:addChild(smallImg,0)
		
		
		local imgStr=CCLabelTTF:create(str..  "*" .. num,FONT_NAME,FONT_FM_SIZE)
		imgStr:setAnchorPoint(PT(0.5,0))
		imgStr:setPosition(PT(bgSmallImg:getContentSize().width/2,-imgStr:getContentSize().height))
		bgSmallImg:addChild(imgStr,0)

		return layer
end



-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	if  actionID == 9006  then
		local serverInfo = actionLayer._9006Callback(pScutScene, lpExternalData)	
		if  serverInfo ~= nil and serverInfo ~= ""  then 
			local info = serverInfo
			showListImg(info)
		end	
	end

end

