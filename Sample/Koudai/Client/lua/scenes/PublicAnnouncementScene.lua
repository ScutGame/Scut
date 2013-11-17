------------------------------------------------------------------
-- PublicAnnouncementScene.lua.lua
-- Author     : yeyq

-- Version    : 1.0
-- Date       :
-- Description: 
------------------------------------------------------------------

module("PublicAnnouncementScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

_scene = nil 		-- 场景
layerBG = nil
announceTable = nil 
Height = nil 
mList = nil 
pageCount = nil
isToday = nil
orTrue  = 0  -----  判断是否为刚开始登录的公告   0，登陆显示  1  设置进入
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	init()
end
-- 退出场景
function popScene()
	releaseResource()
	_scene = nil 		-- 场景
announceTable = nil 
Height = nil 
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	layerBG = nil 
	pageNum = 1
	list = nil 
	pageCount = nil
	isToday = nil
end


function  setNumt(num)
	orTrue = num 
end

-- 释放资源
function releaseResource()
	if  layerBG~=nil  then
		layerBG:getParent():removeChild(layerBG,true)	
		layerBG = nil
	end
	MainMenuLayer.setIsPublish(false)
	MainMenuLayer.judgeIsGuide()
end

-- 创建场景
function init(mScene) 
	initResource()
	if  layerBG~=nil  then
		layerBG:getParent():removeChild(layerBG,true)	
		layerBG = nil
	end
	
	_scene =mScene
	pageNum = pageNum
	
	
	
	-- 添加背景
	layerBG = CCLayer:create()
	_scene:addChild(layerBG,15)
	
	layerBGTo = CCLayer:create()
	layerBG:addChild(layerBGTo,5)
	


	-- 此处添加场景初始内容

	local bgButton=ZyButton:new(Image.image_toumingPath)
	bgButton:setScaleX(pWinSize.width/bgButton:getContentSize().width)
	bgButton:setScaleY(pWinSize.height/bgButton:getContentSize().height)
	bgButton:setAnchorPoint(PT(0,0))
	bgButton:setPosition(PT(0,0))
	bgButton:addto(layerBG,0)

	

	
	local backgroundImg = "common/list_1024.png" 
	local background = CCSprite:create(P(backgroundImg))
	background:setScaleX(pWinSize.width/background:getContentSize().width)
	background:setScaleY(pWinSize.height/background:getContentSize().height)
	background:setAnchorPoint(PT(0,0))
	background:setPosition(PT(0,0))
	layerBG:addChild(background,1)
	
	
	---- 公告标题
	local titleImg =  "title/list_1097.png"
	local titleName = CCSprite:create(P(titleImg))
	titleName:setAnchorPoint(PT(0.5,0))
	titleName:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.91))
	layerBG:addChild(titleName,1)
	
	
	
	local listSize = SZ(pWinSize.width*0.9,pWinSize.height*0.79)
	local listRowH=listSize.height*0.3
	local list = ScutCxList:node(listRowH,ccc4(24,24,24,0),listSize)
	list:setAnchorPoint(PT(0,0))
	list:setTouchEnabled(true)
	mList = list
	list:setPosition(PT(pWinSize.width*0.05,pWinSize.height*0.09))
	layerBG:addChild(list,1)

	
	local CloseButton = ZyButton:new("button/list_1046.png",nil,nil,nil,FONT_NAME,FONT_SM_SIZE)
	CloseButton:setAnchorPoint(PT(0,0))
	CloseButton:setPosition(PT(pWinSize.width*0.87,pWinSize.height*0.9))	
	CloseButton:addto(layerBG,1)
	CloseButton:registerScriptHandler(releaseResource)	
	
	showLayer()
end


function showLayer()

	local listSize = SZ(pWinSize.width*0.9,pWinSize.height*0.79)
	local listRowH=listSize.height*0.3
	local list = ScutCxList:node(listRowH,ccc4(24,24,24,0),listSize)
	list:setAnchorPoint(PT(0,0))
	mList = list
	list:setPosition(PT(pWinSize.width*0.05,pWinSize.height*0.09))
	layerBG:addChild(list,1)
	
	local stringPublic = nil 
	if  announceTable == nil  or   announceTable == 0  then
		stringPublic = CCLabelTTF:create(Language.Public_String,FONT_NAME,FONT_BIG_SIZE)	
		layerBG:addChild(stringPublic,10)
		stringPublic:setAnchorPoint(PT(0.5,0.5))
		stringPublic:setPosition(PT(pWinSize.width*0.5,pWinSize.height*0.5))
	else
		for i=1,#announceTable do 
			local item = ScutCxListItem:itemWithColor(ccc3(25,57,45))	
			item:setOpacity(0)
			local layout = CxLayout()
			layout.val_x.t = ABS_WITH_PIXEL
			layout.val_y.t = ABS_WITH_PIXEL	
			layout.val_x.val.pixel_val =0
			layout.val_y.val.pixel_val =0
			
			local itemLayer,rowH  = addItem(i)
			mList:setRowHeight(rowH)	
			item:addChildItem(itemLayer,layout)
			mList:addListItem(item,false)
		end
		
	end
end
function nextFunc(item)
	local  age = item:getTag()
	if  age == 1    then
		 pageNum = pageNum +1 
	elseif   age == 2   then 	
		pageNum = pageNum - 1 
	end
	if pageNum >= 1 and  pageNum <= 4  then  
	mList:getParent():removeChild(mList,true)
		progressLayer.actionNum(pageNum)
		showLayer()
	elseif pageNum > 4  then 
			ZyToast.show(pScutScene,Language.Public_MaxPage,1.5,0.5)
	elseif pageNum < 1 then 
			ZyToast.show(pScutScene,Language.Public_SmallPage,1.5,0.5)	
	end
end

function addItem(i)	
	local itemLayer = CCLayer:create()
	
	local itemWidth = mList:getContentSize().width
	
	--背景
	local backgroundImg = CCSprite:create(P("common/list_1038.9.png"))
	backgroundImg:setAnchorPoint(PT(0,0))
	itemLayer:addChild(backgroundImg,0)
	
	local height = pWinSize.height*0.025
	if announceTable[i].NoticesType ~=  0 and announceTable[i].NoticesType ~= nil  and announceTable[i].NoticesType ~=""  then 
		local gotoButton = ZyButton:new(Image.image_button_red_c_0, Image.image_button_red_c_1, Image.image_button_hui_c, Language.Public_ButtonStr, FONT_NAME, FONT_SM_SIZE)
		gotoButton:setAnchorPoint(PT(0,0))
		gotoButton:setPosition(PT(itemWidth*0.5-gotoButton:getContentSize().width*0.5, height))	
		gotoButton:addto(itemLayer,5)
		gotoButton:setTag(announceTable[i].NoticesType)
		gotoButton:registerScriptHandler(gotoFunc)
		height = height+gotoButton:getContentSize().height
		height = height+pWinSize.height*0.05
	end	



	local aContentTo = string.format("<label >%s</label>",  announceTable[i].Content)
	local contentStr =ZyMultiLabel:new(aContentTo,pWinSize.width*0.88,FONT_NAME,FONT_SM_SIZE)
	contentStr:setPosition(PT(SX(5),height))
	contentStr:addto(itemLayer,1)
	height = height+contentStr:getContentSize().height+SY(5)
	
	
		
	local titleStr = string.format(Language.NOTICE_NAME,announceTable[i].Title)
	local titleName = CCLabelTTF:create(titleStr,FONT_NAME,FONT_DEF_SIZE)
	titleName:setAnchorPoint(PT(0.5,0))
	titleName:setPosition(PT(itemWidth*0.5,height))	
--	titleName:setColor(ccc3(250,250,0))
	itemLayer:addChild(titleName,1)

	height = height+titleName:getContentSize().height*1.5
	
	
	backgroundImg:setScaleX(itemWidth/backgroundImg:getContentSize().width)
	backgroundImg:setScaleY(height/backgroundImg:getContentSize().height)
	
	return itemLayer,height
end

function gotoFunc(item)
	 local  activeNum = item:getTag()
	 if activeNum == 1  then

	 elseif activeNum == 2 then 
	 	MainMenuLayer.funcAction(true,51)	
	 elseif activeNum == 3 then 
		MainMenuLayer.funcAction(true,14)
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
	local actionID =	ZyReader:getActionID()
	
	
	if  actionID == 9202 then
		local serverInfo = actionLayer._9202Callback(pScutScene, lpExternalData)
		if serverInfo~=nil then 
			pageCount = serverInfo.PageCount 
			announceTable = serverInfo.RecordTabel
			 isToday = serverInfo.IsToday			
		end
	end
	local userData = ScutRequestParam:getParamData(lpExternalData)
end