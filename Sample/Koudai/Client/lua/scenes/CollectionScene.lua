------------------------------------------------------------------
-- CollectionScene.lua
-- Author     :JUNM CHEN
-- Version    : 1.0
-- Date       :集邮系统
-- Description: ,
------------------------------------------------------------------
module("CollectionScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer=nil
local m_listTable=nil
local mList=nil
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

function init()
	if mScene then
		return
	end
	initResource()
	local scene  = ScutScene:new()
    mScene = scene.root
			mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	SlideInLReplaceScene(mScene,1)
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	MainMenuLayer.init(2, mScene)
	-- 此处添加场景初始内容
	if not  mCurrentTab then
		mCurrentTab=1
	end	
	--创建背景
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.9)
	createBgSprite(mLayer,boxSize)
	local tabStr={Language.COLLECTION_SOLDIER,Language.COLLECTION_EQUIP,Language.COLLECTION_SKILL}
	mTabBar=createTabbar(tabStr,mLayer)
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.65)
	local startY=mTabBar:getPosition().y  
	local startX=pWinSize.width/2-listSize.width*0.45
	 ---LIST 控件
	local listX=(pWinSize.width-listSize.width)/2
	local listY=startY-listSize.height-SY(2)
	local mListRowHeight=listSize.height/3
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	list:setTouchEnabled(true)
	mLayer:addChild(list,1)
	mList=list
	list:setHorizontal(true)	 
	m_listTable={}
	m_listTable.currentPage=1
	m_listTable.gotoPage=1
	m_listTable.maxPage=1
	m_listTable.PageSize=20
	
	 ---页码
	local str=string.format("%d/%d",m_listTable.currentPage,m_listTable.maxPage)
	local pageLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	pageLabel:setAnchorPoint(PT(0.5,1))
	pageLabel:setPosition(PT(pWinSize.width/2,list:getPosition().y-SY(10)))
	mLayer:addChild(pageLabel,0)
	m_listTable.pageLabel=pageLabel
	----------------
	local leftBtn=ZyButton:new("button/list_1069.png")
	local rightBtn=ZyButton:new("button/list_1068.png")
	leftBtn:setAnchorPoint(PT(1,0.5))
	rightBtn:setAnchorPoint(PT(0,0.5))
	rightBtn:registerScriptHandler(rightAction)
	leftBtn:registerScriptHandler(leftAction)
	leftBtn:setPosition(PT(pageLabel:getPosition().x-SX(10)-pageLabel:getContentSize().width/2,
				pageLabel:getPosition().y-pageLabel:getContentSize().height/2))
	rightBtn:setPosition(PT(pageLabel:getPosition().x+pageLabel:getContentSize().width/2+SX(10),
						leftBtn:getPosition().y))
	leftBtn:addto(mLayer,0)
	rightBtn:addto(mLayer,0)
	m_listTable.leftBtn=leftBtn
	m_listTable.rightBtn=rightBtn
	refreshPage()
	showLayer()
	
end

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


--list翻页
function callbackListview(page)
	if tonumber(page) <0 then
		page=0
	end
	m_listTable.gotoPage=tonumber(page)+1	
	if m_listTable.gotoPage	~=m_listTable.currentPage then

      end
end


function rightAction()
m_listTable.gotoPage=m_listTable.currentPage+1
mList:turnToPage(m_listTable.gotoPage-1)
end;

function leftAction()
m_listTable.gotoPage=m_listTable.currentPage-1
mList:turnToPage(m_listTable.gotoPage-1)	
end;



--刷新页码
function refreshPage()
	if m_listTable.currentPage< m_listTable.maxPage then
		m_listTable.rightBtn:setVisible(true)
	else
		m_listTable.rightBtn:setVisible(false)	
	end	
	if m_listTable.currentPage> 1 then
		m_listTable.leftBtn:setVisible(true)
	else
		m_listTable.leftBtn:setVisible(false)	
	end	
	local str=string.format("%d/%d",m_listTable.currentPage,m_listTable.maxPage)
	m_listTable.pageLabel:setString(str)
end;

--创建背景
function  createBgSprite(layer,boxSize)	
	--大背景
	local bgSprite=CCSprite:create(P("common/list_1024.png"))
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height-bgSprite:getContentSize().height))
	layer:addChild(bgSprite,0)
end;

--创建tabbar
function  createTabbar(tabStr,layer)
	local tabBar=nil
	local midSprite=CCSprite:create(P("common/list_1024.png"))
	if tabStr then
	  	tabBar=ZyTabBar:new(nil,nil,tabStr,FONT_NAME,FONT_SM_SIZE,4)
		tabBar:selectItem(mCurrentTab)	
		local tabBar_X=pWinSize.width*0.04
		local tabBar_Y=pWinSize.height/2+midSprite:getContentSize().height/2-tabBar:getContentSize().height*0.3
		tabBar:setAnchorPoint(PT(0,0))
		tabBar:setPosition(PT(tabBar_X,tabBar_Y))
		tabBar:setCallbackFun(callbackTabBar)
		tabBar:addto(layer,1)
	end
	return tabBar
end;


function callbackTabBar(bar, pNode)
    local index =pNode:getTag();
    if index ~= mCurrentTab then
        mCurrentTab=index;
        showLayer()
    end
end

---
function showLayer()
	mServerData={}
	if mCurrentTab==1 then
		mServerData={
		{type=1},{type=1},{type=1},{type=1},{type=1},
		{},{},{},{},{},
		{},{},{},{},{},
		{},{},{},{},{},
		}
	elseif mCurrentTab==2 then
		mServerData={		
		{},{},{},{},{},
		{},{},{},{},{},
		{type=1},{type=1},{type=1},{type=1},{type=1},
		{},{},{},{},{},
		}
	elseif mCurrentTab==3 then
	
	end
 	createSinglePage()
end;

------
function  createSinglePage()
	if mList then
		refreshPage()
		 mList:clear()
		 for k=1, m_listTable.maxPage do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			mList:addListItem(listItem, false)
		 end
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
		local row=5
		local col=4
		local rowH=mList:getContentSize().height/row	
		local startY=mList:getContentSize().height-rowH/2
		local colW=mList:getContentSize().width/col
		local startX=colW/2
		local layer=CCLayer:create()
		local bgSprite=CCSprite:createWithTexture(IMAGE("common/list_1038.9.png"))
		bgSprite:setScaleX(mList:getContentSize().width/bgSprite:getContentSize().width)
		bgSprite:setScaleY(mList:getContentSize().height/bgSprite:getContentSize().height)
		bgSprite:setAnchorPoint(PT(0,0))
		bgSprite:setPosition(PT(0,0))
		layer:addChild(bgSprite,0)
		if mServerData and #mServerData>0 then
			local mBgTecture=IMAGE(Image.Image_normalItemBg)
			for k , v in pairs(mServerData) do
				local itemLayer=createImage(v,k,mBgTecture)
				local posX=startX+((k-1)%col)*colW
				local posY=startY-math.floor((k-1)/col)*rowH
				itemLayer:setPosition(PT(posX-itemLayer:getContentSize().width/2,
										posY-itemLayer:getContentSize().height/2))
				layer:addChild(itemLayer,0)	
			end
		else
			local label=CCLabelTTF:create(Language.IDS_NONE,FONT_NAME,FONT_SM_SIZE)
			label:setAnchorPoint(PT(0.5,0.5))
			label:setPosition(PT(mList:getContentSize().width/2,mList:getContentSize().height/2))
			layer:addChild(label,0)
		end
		mList:getChild(m_listTable.currentPage-1):removeAllChildrenWithCleanup(true)
		mList:getChild(m_listTable.currentPage-1):addChildItem(layer,layout)
	end
end;


function  createImage(info,index,mBgTecture)
		local layer=CCLayer:create()
		local bgSprite=CCSprite:createWithTexture(mBgTecture)
		bgSprite:setAnchorPoint(PT(0,0))
		bgSprite:setPosition(PT(0,0))
		layer:addChild(bgSprite,0)
		local imagePath="smallitem/icon_3000.png"
		local fun
		if info.type==1 then
			 imagePath="smallitem/icon_3001.png"
			 fun=CollectionScene.selectAction
		end
		local imageSprite=CCSprite:create(P(imagePath))
		imageSprite:setAnchorPoint(PT(0.5,0.5))
		imageSprite:setPosition(PT(bgSprite:getContentSize().width/2,
								bgSprite:getContentSize().height/2))
		bgSprite:addChild(imageSprite,0)
---------
		
		local actionBtn=UIHelper.createActionRect(bgSprite:getContentSize(),fun,index)
		actionBtn:setPosition(PT(0,0))
		bgSprite:addChild(actionBtn,0)
		layer:setContentSize(bgSprite:getContentSize())
		return layer
end;

--选中响应 
function selectAction(node)
	local tag=node:getTag()
	if mServerData[tag] then
		
	end
end;

-- 退出场景
function closeScene()
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
 mScene = nil 		-- 场景
 mLayer=nil
 m_listTable=nil
 mList=nil
end


-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	
	
end





