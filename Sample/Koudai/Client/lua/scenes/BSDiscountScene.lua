------------------------------------------------------------------
-- BSDiscountScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 商城打折
------------------------------------------------------------------

module("BSDiscountScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local g_scene = nil 		-- 场景
local g_layer = nil 
local g_activeTable = nil



-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
end
-- 释放资源
function releaseResource()
	if  g_layer  then
		g_layer:getParent():removeChild(g_layer,true)
		g_layer = nil 		
	end
end
-- 创建场景
function init(mScene,Flayer,table)
	g_scene = mScene
	initResource()
	g_activeTable= table
	-- 添加背景
	g_layer = CCLayer:create()
	Flayer:addChild(g_layer, 0)
	-- 注册触屏事件
	g_layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "BSDiscountScene.touchBegan")
	g_layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "BSDiscountScene.touchMove")
	g_layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "BSDiscountScene.touchEnd")

	-- 此处添加场景初始内容
	local BgImgW  = pWinSize.width*0.92
	local BgImgH = pWinSize.height*0.69
	local  BgImg = CCSprite:create(P("activeBg/list_1157.jpg"))
	BgImg:setScaleX(BgImgW/BgImg:getContentSize().width)
	BgImg:setScaleY(BgImgH/BgImg:getContentSize().height)
	g_layer:addChild(BgImg,0)
	BgImg:setAnchorPoint(PT(0,0))
	BgImg:setPosition(PT(pWinSize.width*0.5-BgImgW/2,pWinSize.height*0.21))
	
	
	
	local strImg = CCSprite:create(P("title/list_1158.png"))
	g_layer:addChild(strImg,0)
	strImg:setAnchorPoint(PT(0,0))
	titleHeight = BgImg:getPosition().y+BgImgH-SY(50)	
	strImg:setPosition(PT((pWinSize.width-strImg:getContentSize().width)/2,titleHeight))

	
	local gotoButton  = ZyButton:new("button/list_1023.png",nil,nil,Language.RecruitScene_buttonStr,FONT_NAME,FONT_SM_SIZE)
	gotoButton:setAnchorPoint(PT(0.5,0.5))
	gotoButton:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.3))
	gotoButton:addto(g_layer,0)
	gotoButton:registerScriptHandler(gotoAction)

	showLayer()
	
	
end
function showLayer()
		local  BSDiscount_Str =   string.format(Language.Active_Date,g_activeTable.StartDate,g_activeTable.EndDate)
		local activeStr= CCLabelTTF:create(BSDiscount_Str,FONT_NAME,FONT_SM_SIZE)
		g_layer:addChild(activeStr,0)
		activeStr:setAnchorPoint(PT(0,0))
		activeStr:setPosition(PT(pWinSize.width*0.05,titleHeight-SY(20)))
		
		local contentStr= string.format(Language.Active_ContentStr,g_activeTable.FestivalDesc )
		local str = string.format("<label>%s</label>",contentStr)
		local activeStr1 = ZyMultiLabel:new(str,pWinSize.width*0.9,FONT_NAME,FONT_SM_SIZE)
		activeStr1:addto(g_layer,0)
		activeStr1:setAnchorPoint(PT(0,0))
		activeStr1:setPosition(PT(activeStr:getPosition().x,activeStr:getPosition().y-SY(20)))
end
function gotoAction()
	MainMenuLayer.funcAction(true,37)
end


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
	local actionID = NdReader:getActionID()

	local userData = ScutRequestParam:getParamData(lpExternalData)
end