------------------------------------------------------------------
-- UpGradeScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 升级奖励领取
------------------------------------------------------------------

module("UpGradeScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local g_scene = nil 		-- 场景
local g_layer = nil
local g_tList= nil 
local g_tListSize = nil
local g_VipNum = nil 
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释


function setData(info)
	g_DataInfo = info
end;
--
-------------------------私有接口------------------------

function close()
	if g_layer then
		g_layer:getParent():removeChild(g_layer, nil)
		g_layer = nil
	end
	releaseResource()
end
--
-- 初始化资源、成员变量
function initResource()
	g_VipNum = 0
end
-- 释放资源
function releaseResource()
	g_scene = nil 		-- 场景
	g_layer = nil
	g_VipNum= nil
	g_tList= nil 
	g_tListSize = nil
	g_DataInfo=nil
end
-- 创建场景
function init(Scene,FLayer)
	g_scene = Scene
	initResource()
	-- 添加背景
	local layer = CCLayer:create()
	FLayer:addChild(layer,0)
	g_layer = layer

	-- 美术标语  升级有精喜
	local HeadTitleImg = CCSprite:create(P("title/list_1148.png"))
	layer:addChild(HeadTitleImg,0)
	HeadTitleImg:setAnchorPoint(PT(0.5,0))
	HeadTitleImg:setPosition(PT(pWinSize.width/2,pWinSize.height*0.76))
	
	
	----语句：亲爱的会长，您在游戏中达到指定的等级时可以领取晶石,并享受VIP待遇哦
	local UpGradeStrW = pWinSize.width*0.84
	local str = Language.UpGrade_Str1
	if accountInfo.getRetailId() == "0004" then
		str = Language.UpGrade_Str1NOVIP
	end
	local UpGradeStr = ZyMultiLabel:new(str,UpGradeStrW,FONT_NAME,FONT_SM_SIZE)
	UpGradeStr:addto(layer,0)
	UpGradeStr:setAnchorPoint(PT(0,0))
	UpGradeStr:setPosition(PT((pWinSize.width - UpGradeStrW)/2,pWinSize.height*0.76-HeadTitleImg:getContentSize().height))
	

	-- list
	local  listSize = SZ(pWinSize.width*0.84,UpGradeStr:getPosition().y-pWinSize.height*0.21)
	g_tListSize = listSize
	g_tList = ScutCxList:node(listSize.height/5,ccc4(24,24,24,0),listSize)
	g_tList:setAnchorPoint(PT(0,0))
	g_tList:setPosition(PT((pWinSize.width - listSize.width)/2,pWinSize.height*0.21))
	layer:addChild(g_tList,0)
		g_tList:setTouchEnabled(true)
	showList()
end

function delayExec(funName,nDuration)
	local action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFunc:create(funName));
	if g_layer then
		g_layer:runAction(action)
	end
end

function showList()
	if not g_DataInfo then
		return
	end

	for k,v in ipairs(g_DataInfo.RecordTabel) do 
			local listItem = ScutCxListItem:itemWithColor(ccc3(255,255,255))	--25,57,45
			listItem:setOpacity(0)
			local layout = CxLayout()
			layout.val_x.t = ABS_WITH_PIXEL
			layout.val_y.t = ABS_WITH_PIXEL	
			layout.val_x.val.pixel_val =0
			layout.val_y.val.pixel_val =0
			
			local item = creatItem(v,k)		

			listItem:addChildItem(item,layout)
			g_tList:addListItem(listItem,false)		
	end
end

function creatItem(info, tag)
	local listH = g_tList:getRowHeight()
	local listW = pWinSize.width*0.84
	local itemLayer = CCLayer:create()
	
		--- list 背景
	local listBgImgH = listH*0.9
	local listBgImg = CCSprite:create(P("common/list_1038.9.png"))
	listBgImg:setScaleX(listW/listBgImg:getContentSize().width)
	listBgImg:setScaleY(listBgImgH/listBgImg:getContentSize().height)
	listBgImg:setAnchorPoint(PT(0,0))
	listBgImg:setPosition(PT(0,listH/2 - listBgImgH/2))
	itemLayer:addChild(listBgImg,0)

	
	---那个等级背景图标
	local LvBgImg = CCSprite:create(P("mainUI/list_1147.png"))
	LvBgImg:setAnchorPoint(PT(0,0))
	LvBgImg:setPosition(PT(SX(15),listH/2-LvBgImg:getContentSize().height/2))
	itemLayer:addChild(LvBgImg,0)
	-- 等级
	local nowLV = info.RestrainNum 
	local lvNum = CCLabelTTF:create(nowLV,FONT_NAME,FONT_DEF_SIZE)
	LvBgImg:addChild(lvNum,0)
	lvNum:setAnchorPoint(PT(0.5,0.5))
	lvNum:setPosition(PT(LvBgImg:getContentSize().width/2,LvBgImg:getContentSize().height*0.6))
	
	local itemStr1 = string.format("<label>%s</label>", info.FestivalDesc)
	local itemStrW = listW*0.56
	local itemStr = ZyMultiLabel:new(itemStr1,itemStrW,FONT_NAME,FONT_SM_SIZE)
	itemStr:addto(itemLayer,0)
	itemStr:setAnchorPoint(PT(0,0))
	itemStr:setPosition(PT(LvBgImg:getPosition().y+LvBgImg:getContentSize().width+SX(15)
									,listH/2 - itemStr:getContentSize().height/2))
									

		----充值按钮
		local czButton  = ZyButton:new("button/list_1039.png","button/list_1039.png","button/list_1039_1.png",Language.UpGrade_ButtonStr)
		czButton:addto(itemLayer,0)
		czButton:setAnchorPoint(PT(0,0))
		czButton:setPosition(PT(listW - czButton:getContentSize().width - SX(15)
								,listH/2-czButton:getContentSize().height/2))
		czButton:registerScriptHandler(isClickFunc)
		czButton:setTag(tag)
		czButton:setEnabled(false)
		if info.IsReceive == 0 or info.IsReceive == 2 then--0：否 1：是 2：已领取
			if info.IsReceive == 2 then
				czButton:setString(Language.ACTIVE_HAVERECEIVE)
			end
		elseif info.IsReceive == 1 then
			czButton:setEnabled(true)
		end

	return itemLayer
end

function closeLayer()
	if g_layer then
		g_layer:getParent():removeChild(g_layer,true)
	end
end

--领取按钮
function isClickFunc(pNode)
	if not isClick then
		isClick=false	
		local tag = pNode:getTag()
		local FestivalID = g_DataInfo.RecordTabel[tag].FestivalID 
		actionLayer.Action3014(g_scene, nil, FestivalID)
	end
end

function setIsClick(value)
	isClick=value	
end


-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = NdReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
end