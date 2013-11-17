------------------------------------------------------------------
-- TopUpRebate.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 
------------------------------------------------------------------

module("TopUpRebate", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local g_scene = nil 		-- 场景
local g_Layer = nil 
local g_personalInfo = nil 
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

function setData(currentInfo)
	g_activeTable = currentInfo
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
end
-- 释放资源
function releaseResource()
	close()
	g_scene = nil 		-- 场景
	g_Layer = nil 
	g_personalInfo = nil
	g_activeTable=nil	
end
-- 创建场景
function init(Scene,fLayer)
	g_scene = Scene
	
	
	-- 添加背景
	local layer = CCLayer:create()
	g_Layer = layer
	fLayer:addChild(layer, 0)
	-- 注册触屏事件
	layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHBEGAN, "TopUpRebate.touchBegan")
	layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHMOVED, "TopUpRebate.touchMove")
	layer.__CCTouchDelegate__:registerScriptTouchHandler(CCTOUCHENDED, "TopUpRebate.touchEnd")
	-- list
	local  listSize = SZ(pWinSize.width*0.84,pWinSize.height*0.45)
	g_tListSize = listSize
	g_tList = ScutCxList:node(listSize.height/4,ccc4(24,24,24,0),listSize)
	g_tList:setAnchorPoint(PT(0,0))
	g_tList:setPosition(PT((pWinSize.width - listSize.width)/2,pWinSize.height*0.21))
	layer:addChild(g_tList,1)
	g_tList:setTouchEnabled(true)
	
	----充值返利语句
	local HeadTitleImg = CCSprite:create(P("title/list_1146.png"))
	layer:addChild(HeadTitleImg,1)
	HeadTitleImg:setAnchorPoint(PT(0.5,0))
	HeadTitleImg:setPosition(PT(pWinSize.width/2,pWinSize.height*0.76))
	
	---帐户信息
	g_personalInfo=PersonalInfo.getPersonalInfo()
	
	--vip提示语句
	local VIPRecharge = Language.VIP_YUJU -------VIP再充值升级
	local nowValue =g_personalInfo._VipLv
	local jingshi = g_personalInfo.VipNextCold
	local nextValue = nil 
	local titleRecharge =nil
	local StringY = pWinSize.height*0.21+listSize.height
	if  nowValue == 10   then 
		nextValue = nowValue
		titleRecharge = CCLabelTTF:create(VIPRecharge,FONT_NAME,FONT_DEF_SIZE)
		layer:addChild(titleRecharge,0)
		titleRecharge:setAnchorPoint(PT(0,0))
		titleRecharge:setPosition(PT(pWinSize.width*0.07,StringY))
		
		
		local VipNowImg = CCSprite:create(P("button/vip_10.png"))
		VipNowImg:setAnchorPoint(PT(0,0))
		VipNowImg:setPosition(PT(pWinSize.width*0.07+titleRecharge:getContentSize().width,StringY))
		layer:addChild(VipNowImg,0)
		
		
	elseif  nowValue <10  and  nowValue > 0   then  
		nextValue = nowValue+1		
			----您当前是
			 titleRecharge = CCLabelTTF:create(VIPRecharge,FONT_NAME,FONT_DEF_SIZE)
			layer:addChild(titleRecharge,0)
			titleRecharge:setAnchorPoint(PT(0,0))
			titleRecharge:setPosition(PT(pWinSize.width*0.07,StringY))
			local ImgNum = 0 
			local VIPImg_Num = "button/vip_%s.png"
			local VIPImg = ""
			if  nowValue<10 and nowValue>0  then 
			VIPImg = string.format(VIPImg_Num,nowValue) 
			end
			----VIP几美术字
			local VipNowImg = CCSprite:create(P(VIPImg))
			VipNowImg:setAnchorPoint(PT(0,0))
			VipNowImg:setPosition(PT(pWinSize.width*0.07+titleRecharge:getContentSize().width,StringY))
			layer:addChild(VipNowImg,0)
		
			
			----再充值多少
			local  jingshiStr = string.format(Language.VIP_Str,jingshi)
			local titleNum = CCLabelTTF:create(jingshiStr,FONT_NAME,FONT_DEF_SIZE)
			layer:addChild(titleNum,0)
			titleNum:setAnchorPoint(PT(0,0))
			titleNum:setPosition(PT(VipNowImg:getPosition().x+VipNowImg:getContentSize().width,
			StringY))
			
			
			------下一个VIp美术字
			if  nowValue<10 and nowValue>0  then 
			VIPImg = string.format(VIPImg_Num,nextValue) 
			end
			local VipNextImg = CCSprite:create(P(VIPImg))
			VipNextImg:setAnchorPoint(PT(0,0))
			VipNextImg:setPosition(PT(titleNum:getPosition().x+titleNum:getContentSize().width,StringY))
			layer:addChild(VipNextImg,0)
			
	elseif  	nowValue == 0 then 
		nextValue = nowValue+1	
		local titleStr = string.format(Language.VIP_OStr,jingshi)
		titleRecharge = CCLabelTTF:create(titleStr,FONT_NAME,FONT_DEF_SIZE)
		layer:addChild(titleRecharge,0)
		titleRecharge:setAnchorPoint(PT(0,0))
		titleRecharge:setPosition(PT(pWinSize.width*0.07,StringY))
		
		local VipNowImg = CCSprite:create(P("button/vip_1.png"))
		VipNowImg:setAnchorPoint(PT(0,0))
		VipNowImg:setPosition(PT(pWinSize.width*0.07+titleRecharge:getContentSize().width,StringY))
		layer:addChild(VipNowImg,0)
				
	end
	

	
	local titleStr = CCLabelTTF:create(Language.TopUpRebate_TitleStr,FONT_NAME,FONT_DEF_SIZE)
	layer:addChild(titleStr,0)
	titleStr:setAnchorPoint(PT(0,0))
	titleStr:setPosition(PT(pWinSize.width*0.07,StringY + titleStr:getContentSize().height + SY(3)))
	
	local vipButton  = ZyButton:new("button/bottom_1002_2.9.png",nil,nil,Language.TopUpRebate_ButtonStr)
	vipButton:addto(layer,0)
	vipButton:setAnchorPoint(PT(0,0))
	vipButton:setPosition(PT(g_tList:getPosition().x+listSize.width - vipButton:getContentSize().width,
								StringY + titleStr:getContentSize().height + SY(3)))
	vipButton:registerScriptHandler(goVIPFunc)
	
	
	
	showList()
end
function goVIPFunc()
	VIPScene.pushScene(g_scene)
end
function close()
	if  g_Layer   then
		g_Layer:getParent():removeChild(g_Layer,true)
		g_Layer= nil
	end
end
function showList()
	for k,v in ipairs(g_activeTable.RecordTabel) do
		local item = ScutCxListItem:itemWithColor(ccc3(255,255,255))	--25,57,45
		item:setOpacity(0)
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL	
		layout.val_x.val.pixel_val =0
		layout.val_y.val.pixel_val =0
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
		

		
		if v.IsReceive == 2 then--0：否 1：是 2：已领取
			czButton:setEnabled(false)
			czButton:setString(Language.ACTIVE_HAVERECEIVE)
		elseif v.IsReceive == 1 then
			czButton:setString(Language.ACTIVE_BUTTONSTR2)
			czButton:registerScriptHandler(isClickFunc)
		end		
		
		
		local itemStr1 = string.format("<label>%s</label>", v.FestivalDesc)
		local itemStrW = listW*0.7
		local itemStr = ZyMultiLabel:new(itemStr1,itemStrW,FONT_NAME,FONT_SM_SIZE)
		itemStr:addto(itemLayer,0)
		itemStr:setAnchorPoint(PT(0,0))
		itemStr:setPosition(PT(listW*0.05,listH/2 - itemStr:getContentSize().height/2))
									
			
		item:addChildItem(itemLayer,layout)
		g_tList:addListItem(item,false)		
	
	end
end

--领取按钮
function isClickFunc(pNode)
	if not isClick then
		isClick=false	
		local tag = pNode:getTag()
		local FestivalID = g_activeTable.RecordTabel[tag].FestivalID 
		actionLayer.Action3014(g_scene, nil, FestivalID)
	end
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
	local actionID = ZyReader:getActionID()

end