--
-- WeaponScene.lua
-- Author     : JunMing Chen
-- Version    : 1.1.0.0
-- Date       : 2013-3-8
-- Description:
--

module("WeaponScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer = nil 		
local mCurrentTab=nil
local mContentLayer=nil
local personalInfo=nil
local mTabBar=nil
local m_listTable=nil
local mServerData=nil
local mStrenLayer=nil
local isSellLayer=nil
local weapType={
[1]=0,
[2]=2,
[3]=5,
[4]=1,
}


local mDetailInfo=nil
local isStrenAction=nil
local isChangeEquip=nil
local changeIndex=nil
local shicitype=nil

--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释
-- 退出场景
function closeScene()
	releaseResource()
end

-- 场景入口 -- 创建场景
function init()
	if mScene then
		return
	end	
	initResource()
	local scene = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	mScene = scene.root
	mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	SlideInLReplaceScene(mScene,1)
	
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	if not mCurrentTab then
		mCurrentTab=1
	end
	m_listTable={}
	m_listTable.currentPage=1
	m_listTable.gotoPage=1
	m_listTable.maxPage=1
	m_listTable.PageSize=100
	
	--创建背景
	local bgLayer=createBgLayer()
	bgLayer:setPosition(PT(0,0))
	mLayer:addChild(bgLayer,0)
	----创建公用层
	createContentLayer()
	
	MainMenuLayer.init(3, mScene)
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end




--创建背景层
function  createBgLayer()
	local layer=CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	--大背景
	local titleBg=CCSprite:create(P("common/list_1047.png"))
	local bgSprite=CCSprite:create(P("common/list_1015.png"))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(bgSprite,0)
	--中间层
	local path="common/list_1043.png"
	local midSprite=CCSprite:create(P(path))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.75)
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)
	local posY=pWinSize.height*0.145--pWinSize.height-boxSize.height-titleBg:getContentSize().height
	midSprite:setPosition(PT(pWinSize.width/2,posY))
	layer:addChild(midSprite,0)
	return layer
end;




--创建Tab
function createTabBar(tabName, layer,k)
	local tabBar = ZyTabBar:new(Image.image_top_button_0, Image.image_top_button_1, tabName, FONT_NAME, FONT_SM_SIZE, 4, Image.image_LeftButtonNorPath, Image.image_rightButtonNorPath);
	tabBar:setCallbackFun(callbackTabBar); -----点击启动函数
	if k~=1 then
	tabBar:addto(layer,0) ------添加
	end
	tabBar:setColor(ZyColor:colorYellow())  ---设置颜色
	
	tabBar:setPosition(PT(pWinSize.width*0.035,pWinSize.height*0.83))  -----设置坐标
	return tabBar
end


----tabbar响应
function callbackTabBar(bar,pNode)
    local index =pNode:getTag();
    if index ~=mCurrentTab then
	    mCurrentTab=index
	    showLayer()
    end
end;

--list翻页
function gotoListPage(page)
	if not isClick then
		isClick=true
    		actionLayer.Action1101(mScene,false,m_listTable.gotoPage,m_listTable.PageSize)
    	end
end

--
function rightAction()
m_listTable.gotoPage=m_listTable.currentPage+1
gotoListPage(m_listTable.gotoPage)
end;

--
function leftAction()
m_listTable.gotoPage=m_listTable.currentPage-1
gotoListPage(m_listTable.gotoPage)	
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

function releaseContentLayer()
	releaseNoLabel()
	if mContentLayer then
		mContentLayer:getParent():removeChild(mContentLayer,true)
		mContentLayer=nil
	end
end;

function createContentLayer()
	releaseContentLayer()
	local layer=CCLayer:create()
	mContentLayer=layer
	mLayer:addChild(layer,0)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	
	local titleBg=CCSprite:create(P("common/list_item_bg.9.png"))
	local tabStr={Language.EQUIP_ALL,Language.EQUIP_WEA,Language.EQUIP_CLO,Language.EQUIP_GUS}
	mTabBar=createTabBar(tabStr,layer)
	local posY=mTabBar:getPosition().y  	
	  -------------------标题
	local titleSize=SZ(pWinSize.width*0.9,pWinSize.height*0.08)
	local startX=pWinSize.width/2-titleSize.width*0.45
	local startY=posY-titleSize.height
	
	local tipLabel=CCLabelTTF:create(Language.EQUIP_TIP,FONT_NAME,FONT_DEF_SIZE)
	tipLabel:setAnchorPoint(PT(0,0.5))
	tipLabel:setPosition(PT(startX,startY+titleSize.height/2))
	layer:addChild(tipLabel,0)
	
	local sellBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.EQUIP_SELL)
	sellBtn:setAnchorPoint(PT(0,0))
	sellBtn:setPosition(PT(pWinSize.width/2+titleSize.width*0.45-sellBtn:getContentSize().width+SX(13),
								tipLabel:getPosition().y-sellBtn:getContentSize().height/2))
	sellBtn:addto(layer,0)
	sellBtn:registerScriptHandler(sellAction)
	
	  ---LIST 控件
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.53)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=startY-listSize.height
	local mListRowHeight=pWinSize.height*0.13
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	layer:addChild(list,1)
	list:setTouchEnabled(true)
	mList=list
	--创建页码
--[[
 	---页码
	local str=string.format("%d/%d",m_listTable.currentPage,m_listTable.maxPage)
	local pageLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	pageLabel:setAnchorPoint(PT(0.5,1))
	pageLabel:setPosition(PT(pWinSize.width/2,listY-pageLabel:getContentSize().height))
	layer:addChild(pageLabel,0)
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
	leftBtn:addto(layer,0)
	rightBtn:addto(layer,0)
	m_listTable.leftBtn=leftBtn
	m_listTable.rightBtn=rightBtn
	refreshPage()
	--]]
	showLayer()
end;


--------没有装备
function  releaseNoLabel()
	if mNoneLabel then
		mNoneLabel:getParent():removeChild(mNoneLabel,true)
		mNoneLabel=nil
	end
end;

function  createNoLabel()
	releaseNoLabel()
	local noneLabel=CCLabelTTF:create(Language.EQUIP_STRENTIP1,FONT_NAME,FONT_DEF_SIZE)
	mNoneLabel=noneLabel
	noneLabel:setAnchorPoint(PT(0.5,0.5))
	noneLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.6))
	mContentLayer:addChild(noneLabel,2)
end;


--刷新列表
function  refreshItemList()
	releaseNoLabel()
	if mList  and mServerData then
		mList:clear()
		if #mServerData>0 then
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0

		for k, v in pairs(mServerData) do
			local mBgTecture= IMAGE(getQualityBg(v.QualityType, 1))
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local itemLayer=createListItem(v,k,mBgTecture)
			listItem:addChildItem(itemLayer, layout)
			mList:addListItem(listItem, false)
		end
		else
			createNoLabel()
		end
	end
end;

function createListItem(info ,index,mBgTecture)
	local layer=CCLayer:create()
	local boxSize=SZ(mList:getContentSize().width,mList:getRowHeight())
	local startX=SX(5)
	local startY=boxSize.height*0.9
	local rowH=boxSize.height/5
	local colW=boxSize.width/4
	-----------

	local bgSprite=CCSprite:create(P("common/list_1038.9.png"))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setPosition(PT(0,0))
	layer:addChild(bgSprite,0)
	
	--物品图片
	local goodBg=CCSprite:createWithTexture(mBgTecture)
	goodBg:setAnchorPoint(PT(0,0.5))
	goodBg:setPosition(PT(startX,boxSize.height/2))
	layer:addChild(goodBg,0)
	
	--
	local fun=checkAction
	if isSellLayer  or isChangeEquip then
		fun=nil
	end
	local actionBtn=UIHelper.createActionRect(goodBg:getContentSize(),fun,index)
	actionBtn:setPosition(PT(0,0))
	goodBg:addChild(actionBtn,0)	
	local imgPath=string.format("smallitem/%s.png",info.HeadID)
	local goodSprite=CCSprite:create(P(imgPath))
	goodSprite:setAnchorPoint(PT(0.5,0.5))
	goodSprite:setPosition(PT(goodBg:getContentSize().width/2,
									goodBg:getContentSize().height/2))
	goodBg:addChild(goodSprite,0)
	startX=startX+goodBg:getContentSize().width+SX(10)
	
	--物品名字
	local goodName=CCLabelTTF:create(info.ItemName ,FONT_NAME,FONT_SM_SIZE)
	goodName:setAnchorPoint(PT(0,0))
	goodName:setPosition(PT(startX,startY-rowH))
	layer:addChild(goodName,0)
	
	---装备于谁身上
	if info.GeneralName  then
	local quipName=CCLabelTTF:create(string.format(Language.EQUIP_EQUIPMAN,info.GeneralName ),FONT_NAME,FONT_SMM_SIZE)
	quipName:setAnchorPoint(PT(0,0))
	quipName:setPosition(PT(startX+boxSize.width*0.4,goodName:getPosition().y))
	layer:addChild(quipName,0)
	end
	
	--品质
	local str=Language.EQUIP_PIN .. ":" 
	if  genrealQuality[info.QualityType ] then
		str=str .. genrealQuality[info.QualityType ]
	end	
	local qualityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
	qualityLabel:setAnchorPoint(PT(0,0))
	qualityLabel:setPosition(PT(goodName:getPosition().x,
								goodName:getPosition().y-rowH))
	layer:addChild(qualityLabel,0)
	
	--等级
	local str=Language.IDS_LEVEL .. ":" .. string.format("Lv.%d",info.CurLevel  or 1)
	local levelLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
	levelLabel:setAnchorPoint(PT(0,0))
	levelLabel:setPosition(PT(qualityLabel:getPosition().x+qualityLabel:getContentSize().width+SX(10),
								qualityLabel:getPosition().y))
	layer:addChild(levelLabel,0)
	
	local startY=qualityLabel:getPosition().y-rowH
	local tableInfo=info.AbilityList or {}
	for k, v in pairs(tableInfo) do
		local str=Language.BAG_TYPE_[v.AbilityType] .. ":+" .. v.BaseNum
		local lAttribute=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
		lAttribute:setAnchorPoint(PT(0,0))
		local posX=startX+((k-1)%2)*colW
		local posY=startY-math.floor((k-1)/2)*rowH
		lAttribute:setPosition(PT(posX,posY))
		layer:addChild(lAttribute,0)
	end
	local startY=startY-math.floor((#tableInfo-1)/2)*rowH
	
	--售价
	local str=Language.EQUIP_PRICE .. ":" .. (info.Sellprice or 0 )
	local priceLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
	priceLabel:setAnchorPoint(PT(0,0))
	priceLabel:setPosition(PT(startX,startY-rowH))
	layer:addChild(priceLabel,0)
	
	if isSellLayer  then
		local actionBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_SELL)
		actionBtn:setAnchorPoint(PT(0,0))
		actionBtn:setPosition(PT(boxSize.width*0.98-actionBtn:getContentSize().width,
										boxSize.height/2-actionBtn:getContentSize().height/2))
		actionBtn:addto(layer,0)	
		actionBtn:setTag(index)
		actionBtn:registerScriptHandler(sellWeaponAction)	
		--[[
		local actionBtn=ZyButton:new("button/main_3001_14.png","button/main_3001_15.png")
		actionBtn:setAnchorPoint(PT(0,0))
		actionBtn:setPosition(PT(boxSize.width*0.95-actionBtn:getContentSize().width,
										boxSize.height/2-actionBtn:getContentSize().height/2))
		actionBtn:addto(layer,0)	
		actionBtn:setTag(index)
		actionBtn:registerScriptHandler(choiceEquipAction)
		info.actionBtn=actionBtn
		--]]
	elseif isChangeEquip then
		local actionBtn=ZyButton:new("button/list_1044.png","button/list_1045.png")
		actionBtn:setAnchorPoint(PT(0,0))
		actionBtn:setPosition(PT(boxSize.width*0.95-actionBtn:getContentSize().width,
										boxSize.height/2-actionBtn:getContentSize().height/2))
		actionBtn:addto(layer,0)	
		actionBtn:setTag(index)
		actionBtn:registerScriptHandler(choiceEquipAction)
		info.actionBtn=actionBtn
	else
		--强化按钮
		local actionBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_STREN)
		actionBtn:setAnchorPoint(PT(0,0))
		actionBtn:setPosition(PT(boxSize.width*0.98-actionBtn:getContentSize().width,
										boxSize.height/2-actionBtn:getContentSize().height/2))
		actionBtn:addto(layer,0)	
		actionBtn:setTag(index)
		actionBtn:registerScriptHandler(strenAction)
	end
	return layer
end;

function sellWeaponAction(node)
	local tag=node:getTag()
	if mServerData[tag] then
		if not isClick then
		isClick=true
		actionLayer.Action7006(mScene,nil,mServerData[tag].UserItemID)
		end
	end
end;

--进去出售界面
function sellAction()
	releaseContentLayer()
	isSellLayer=true
	if not isClick then
	isClick=true
	actionLayer.Action1208(mScene,nil)
	end
end;

--释放出售列表
function  releaseSellLayer()
	isSellLayer=false
	mServerData=nil
	if mSellLayer then
		mSellLayer:getParent():removeChild(mSellLayer,true)
		mSellLayer=nil
	end
end;

--创建出售列表
function  createSellLayer(info,type)
	 releaseStrenLayer()
	if mSellLayer then
		mSellLayer:getParent():removeChild(mSellLayer,true)
		mSellLayer=nil
	end
	local layer=CCLayer:create()
	mSellLayer=layer
	mLayer:addChild(layer,2)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	
	local titleBg=CCSprite:create(P("common/list_item_bg.9.png"))
	local tabStr={Language.EQUIP_SELLTILE}
	mTabBar=createTabBar(tabStr,layer)
	local posY=mTabBar:getPosition().y 

	  -------------------标题
	local titleSize=SZ(pWinSize.width*0.9,pWinSize.height*0.08)
	local startX=pWinSize.width/2-titleSize.width*0.45
	local startY=posY-titleSize.height
	
	local str=Language.EQUIP_SELLTIP
	if type then
	 	str=""
	end
	local tipLabel=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
	tipLabel:setAnchorPoint(PT(0,0.5))
	tipLabel:setPosition(PT(startX,startY+titleSize.height/2))
	layer:addChild(tipLabel,0)
	
	local str=Language.IDS_BACK
	local fun=sellEquipAction
	if type then
	 	str=Language.IDS_SURE
	 	fun=changeEquipAction
	end
	local sureBtn=ZyButton:new("button/list_1039.png",nil,nil,str)
	sureBtn:setAnchorPoint(PT(0,0))
	sureBtn:setPosition(PT(pWinSize.width/2+titleSize.width*0.45-sureBtn:getContentSize().width,
								tipLabel:getPosition().y-sureBtn:getContentSize().height/2))
	sureBtn:addto(layer,0)
	sureBtn:registerScriptHandler(fun)
	
	  ---LIST 控件
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.5)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=startY-listSize.height
	local mListRowHeight=pWinSize.height*0.13
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	layer:addChild(list,1)
	mList=list
	--创建页码
	
	local data = {}
	for k,v in ipairs(info) do
		if not v.GeneralName then
			data[#data+1]=v
		end
	end			
	if #data>0 then
		refreshItemList(data)
	else
		local tipLabel=CCLabelTTF:create(Language.EQUIP_NONE,FONT_NAME,FONT_DEF_SIZE)
		tipLabel:setAnchorPoint(PT(0.5,0.5))
		tipLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.6))
		layer:addChild(tipLabel,0)
	end	
end;


function changeEquipAction(node)
	if changeIndex and mServerData[changeIndex] then
		local  itemInfo=mServerData[tag]
		
		
	else
		releaseSellLayer()
		createContentLayer()
	end
end;





--选择更换装备
function choiceEquipAction(node)
	local tag=node:getTag()
	changeIndex=tag
	for k, v in pairs(mServerData) do
		if k==tag then
			v.actionBtn:selected()
		else
			v.actionBtn:unselected()
		end
	end
	--[[
	local itemInfo=mServerData[tag]
	if  itemInfo  then
		itemInfo.isSelected= not itemInfo.isSelected
		itemInfo.actionBtn:unselected()
		if itemInfo.isSelected then
		itemInfo.actionBtn:selected()
		end
	end
--]]	
end;


--出售物品
function  sellEquipAction()
	local content=""
	local isFirst=true
	for k , v in pairs(mServerData) do
		if v.isSelected and  v.UserItemID then
			if isFirst then
				isFirst=false
				content=content .. v.UserItemID
			else
				content=content .. "," ..   v.UserItemID
			end
		end
	end
	if string.len(content)>1 then
			ZyToast.show(mScene,content)
	else
		releaseSellLayer()
		createContentLayer()
	end
end;

--查看按钮
function checkAction(node)
	local tag=node
 	local data = {}
 	data.type = 1
 	data.UserItemID =mServerData[tag].UserItemID
 	data.index = tag
	actionLayer.Action1202(mScene,false,mServerData[tag].UserItemID, nil, data)
end;

--
-------------------------私有接口------------------------
--
--tabbar响应
function callbackTabBar(bar,pNode)
    local index =pNode:getTag();
    if index ~=mCurrentTab then
	    mCurrentTab=index
	    showLayer()
    end
end;

--层管理器
function showLayer()
	if not isClick then
		isClick =true
		actionLayer.Action1205(mScene,true,weapType[mCurrentTab], 1)
	end
end

function setScene(scene, fatherLayer)
	mScene = scene
	mLayer = fatherLayer
end

--释放详细层
function  releaseDetailLayer()
	if mDetailLayer then
		mDetailLayer:getParent():removeChild(mDetailLayer,true)
		mDetailLayer=nil
	end
end;

--创建详细层
function  createDetailLayer(info)
	releaseDetailLayer()
	local layer=CCLayer:create()
	mScene:addChild(layer,2)
	mDetailLayer=layer
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	-----------------
-----------------
	local sprite=CCSprite:create(P("mainUI/list_1014.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.855)
	for k=1 ,2 do
		local pingBiBtn=UIHelper.createActionRect(boxSize)
		pingBiBtn:setPosition(PT(0,pWinSize.height-boxSize.height))
		layer:addChild(pingBiBtn,0)
	end
	
	local bgSprite=CCSprite:create(P("common/list_1024.png"))
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	local posY=pWinSize.height*0.145--pWinSize.height-boxSize.height-titleBg:getContentSize().height
	bgSprite:setPosition(PT(pWinSize.width/2,posY))
	layer:addChild(bgSprite,0)
	
	--标题
	local titleLabel=CCSprite:create(P("title/list_1101.png"))
	titleLabel:setAnchorPoint(PT(0.5,0))
	titleLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.97-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)
	
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width*1.2,
							pWinSize.height-closeBtn:getContentSize().height*1.2))
--	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(releaseDetailLayer)
	local startY=closeBtn:getPosition().y-titleLabel:getContentSize().height
	

	--简介
	local sizeBox=SZ(boxSize.width*0.9,pWinSize.height*0.55)
	local contentBg=CCSprite:create(P("common/list_1052.9.png"))
	contentBg:setAnchorPoint(PT(0.5,0))
	contentBg:setScaleY(sizeBox.height/contentBg:getContentSize().height)
	contentBg:setPosition(PT(pWinSize.width/2,
								startY-sizeBox.height))
	layer:addChild(contentBg,0)
	local startX=pWinSize.width/2-contentBg:getContentSize().width*0.48
	
	---	物品图片
	local bgPic = getQualityBg(info.QualityType, 3)	
	local goodBg=CCSprite:create(P(bgPic))
	goodBg:setAnchorPoint(PT(0,0))
	goodBg:setPosition(PT(startX,
				contentBg:getPosition().y+sizeBox.height*0.9-goodBg:getContentSize().height))
	layer:addChild(goodBg,0)
	local imgPath=string.format("bigitem/%s.png",info.MaxHeadID)
	local goodSprite=CCSprite:create(P(imgPath))
	goodSprite:setAnchorPoint(PT(0.5,0.5))
	goodSprite:setPosition(PT(goodBg:getContentSize().width/2,
									goodBg:getContentSize().height/2))
	goodBg:addChild(goodSprite,0)
	
	---名称
	local nameLabel=CCLabelTTF:create(info.ItemName,FONT_NAME,FONT_SM_SIZE)
	nameLabel:setAnchorPoint(PT(0,0))
	nameLabel:setPosition(PT(goodBg:getContentSize().width*0.15,
				goodBg:getContentSize().height*0.99-nameLabel:getContentSize().height))
	goodBg:addChild(nameLabel,0)

	--品质
	local str=""
	--Language.EQUIP_PIN .. ":" 
	if  genrealQuality[info.QualityType ] then
		str=str .. genrealQuality[info.QualityType ]
	end	
	local qualityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
	qualityLabel:setAnchorPoint(PT(0,0))
	qualityLabel:setPosition(PT(goodBg:getContentSize().width*0.65,
								nameLabel:getPosition().y))
	goodBg:addChild(qualityLabel,0)
	
	--增加属性值
	local tableInfo=info.AbilityList or {}
	local startX=goodBg:getContentSize().width*0.08
	local startY=SY(8)+qualityLabel:getContentSize().height
	local colW=goodBg:getContentSize().width/2
	local rowH=nil
	local col=1
	for k, v in pairs(tableInfo) do
		local str=Language.BAG_TYPE_[v.AbilityType] .. ":+" .. v.BaseNum
		local lAttribute=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
		if not rowH then
			rowH=lAttribute:getContentSize().height+SY(1)
		end
		lAttribute:setAnchorPoint(PT(0,0))
		local posX=startX+((k-1)%col)*colW
		local posY=rowH+SY(7)+math.floor((k-1)/col)*rowH
		lAttribute:setPosition(PT(posX,posY))
		goodBg:addChild(lAttribute,0)
	end
		
	--说明文字
	local titleLabel=CCLabelTTF:create(Language.BAG_INTRO .. ":",FONT_NAME,FONT_BIG_SIZE)
	titleLabel:setAnchorPoint(PT(0,0))
	titleLabel:setPosition(PT(goodBg:getPosition().x+goodBg:getContentSize().width+SX(5),
						goodBg:getPosition().y+goodBg:getContentSize().height-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)
	local contentStr=string.format("<label>%s</label>",info.ItemDesc or Language.IDS_NONE)
	local contentLabel=ZyMultiLabel:new(contentStr,sizeBox.width*0.35,FONT_NAME,FONT_DEF_SIZE);
	contentLabel:setAnchorPoint(PT(0,0))
	contentLabel:setPosition(PT(titleLabel:getPosition().x,
						titleLabel:getPosition().y-contentLabel:getContentSize().height-SY(2)))
	contentLabel:addto(layer,0)
	
	--售价
	local sellLabel=CCLabelTTF:create(Language.EQUIP_PRICE .. ":",FONT_NAME,FONT_BIG_SIZE)
	sellLabel:setAnchorPoint(PT(0,0))
	sellLabel:setPosition(PT(titleLabel:getPosition().x,
						contentLabel:getPosition().y-sellLabel:getContentSize().height*2))
	layer:addChild(sellLabel,0)
	local priceLabel=CCLabelTTF:create(info.Sellprice .. Language.IDS_GOLD,FONT_NAME,FONT_DEF_SIZE)
	priceLabel:setAnchorPoint(PT(0,0))
	priceLabel:setPosition(PT(titleLabel:getPosition().x,
						sellLabel:getPosition().y-sellLabel:getContentSize().height*1.1))
	layer:addChild(priceLabel,0)
	
	--强化按钮
	local colW=pWinSize.width/2
	local strenBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_STREN)
	strenBtn:setAnchorPoint(PT(0,0))
	strenBtn:setPosition(PT(colW*1.5-strenBtn:getContentSize().width/2,
							contentBg:getPosition().y-strenBtn:getContentSize().height*1.2))
	strenBtn:addto(layer,0)	
	strenBtn:setTag(info.index)
	strenBtn:registerScriptHandler(strenAction)
	
	--
	local name=Language.IDS_BACK
	local fun=releaseDetailLayer
	local closeBtn=ZyButton:new("button/list_1039.png",nil,nil,name)
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setTag(info.index)
	closeBtn:setPosition(PT(colW/2-strenBtn:getContentSize().width/2 ,
							strenBtn:getPosition().y))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(fun)
end;

---
function changeEquip(node)
	local tag=node:getTag()
	local itemInfo=mServerData[tag]
	actionLayer.Action1205(mScene,true,itemInfo.EquParts, 3)	
--	actionLayer.Action1205(mScene,nil,)
end;


--进入强化界面
function strenAction(node)
	local tag=node:getTag()
	local itemInfo=mServerData[tag]
	if  itemInfo  then
		 if not isClick then
		 	isClick =true
		 	local data = {}
		 	data.type = 2
		 	data.UserItemID = itemInfo.UserItemID
		 	actionLayer.Action1202(mScene, nil, itemInfo.UserItemID, nil, data)
		 end
	end	
end;

--释放强化层
function  releaseStrenLayer()
	if mStrenLayer then
		mStrenLayer:getParent():removeChild(mStrenLayer,true)
		mStrenLayer=nil
	end
end;

--创建强化层
function createStrenLayer(info)
	mDetailInfo = info
	 releaseStrenLayer()
	local layer=CCLayer:create()
	mStrenLayer=layer
	mLayer:addChild(layer,2)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	
	local sprite=CCSprite:create(P("mainUI/list_1014.png"))
	local boxSize=SZ(pWinSize.width,pWinSize.height-sprite:getContentSize().height)
	for k=1 ,2 do
		local pingBiBtn=UIHelper.createActionRect(boxSize)
		pingBiBtn:setPosition(PT(0,pWinSize.height-boxSize.height))
		layer:addChild(pingBiBtn,0)
	end	
	
	local titleBg=CCSprite:create(P("common/list_1047.png"))
	
	
	local path="common/list_1043.png"
	local bgSprite=CCSprite:create(P(path))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.75)
	bgSprite:setAnchorPoint(PT(0.5,0))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	local posY=pWinSize.height*0.145--pWinSize.height-boxSize.height-titleBg:getContentSize().height
	bgSprite:setPosition(PT(pWinSize.width/2,posY))
	layer:addChild(bgSprite,0)
	
	
	local tabStr={Language.EQUIP_STREN}
	local tabBar=createTabBar(tabStr,layer,1)
	local startY=tabBar:getPosition().y  

	--背景图片
	local sizeBox=SZ(pWinSize.width*0.8,pWinSize.height*0.5)

	local contentBg=CCSprite:create(P("common/list_1052.9.png"))
	contentBg:setAnchorPoint(PT(0.5,0))
	contentBg:setScaleY(sizeBox.height/contentBg:getContentSize().height)
	contentBg:setPosition(PT(pWinSize.width/2,
								startY-sizeBox.height-SY(5)))
	layer:addChild(contentBg,0)
	local startX=pWinSize.width*0.5-contentBg:getContentSize().width*0.48
	---	物品图片
	local bgPic = getQualityBg(info.QualityType, 3)
	local goodBg=CCSprite:create(P(bgPic))
	goodBg:setAnchorPoint(PT(0,0))
	goodBg:setPosition(PT(startX,
				contentBg:getPosition().y+(sizeBox.height-goodBg:getContentSize().height)/2))
	layer:addChild(goodBg,0)
	local imgPath=string.format("bigitem/%s.png",info.MaxHeadID )
	local goodSprite=CCSprite:create(P(imgPath))   
	goodSprite:setAnchorPoint(PT(0.5,0.5))
	goodSprite:setPosition(PT(goodBg:getContentSize().width/2,
									goodBg:getContentSize().height/2))
	goodBg:addChild(goodSprite,0)
	
	--等级
	local goodLv=CCLabelTTF:create(info.CurLevel .. Language.IDS_LEV,FONT_NAME,FONT_SMM_SIZE)
	goodLv:setAnchorPoint(PT(0,0))
	goodLv:setPosition(PT(goodBg:getContentSize().width*0.08,goodBg:getContentSize().height*0.09-goodLv:getContentSize().height))
	goodBg:addChild(goodLv,0)
	--属性
	local tableInfo=info.AbilityList or {}
	local startX=goodLv:getPosition().x
	local startY=goodBg:getContentSize().height*0.1
	local colW=goodBg:getContentSize().width/2
	local rowH=nil
	local col=1
	for k, v in pairs(tableInfo) do
		local str=Language.BAG_TYPE_[v.AbilityType] .. "+" .. v.BaseNum
		local lAttribute=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
		if not rowH then
			rowH=lAttribute:getContentSize().height+SY(1)
		end
		lAttribute:setAnchorPoint(PT(0,0))
		local posX=startX+((k-1)%col)*colW
		local posY=startY+math.floor((k-1)/col)*rowH
		lAttribute:setPosition(PT(posX,posY))
		goodBg:addChild(lAttribute,0)
	end		
	---名称
	local nameLabel=CCLabelTTF:create(info.ItemName,FONT_NAME,FONT_SM_SIZE)
	nameLabel:setAnchorPoint(PT(0,0))
	nameLabel:setPosition(PT(goodBg:getContentSize().width*0.15,
				goodBg:getContentSize().height*0.99-nameLabel:getContentSize().height))
	goodBg:addChild(nameLabel,0)
	--品质
	local str="" 
	if  genrealQuality[info.QualityType ] then
		str=str .. genrealQuality[info.QualityType ]
	end	
	local qualityLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
	qualityLabel:setAnchorPoint(PT(0,0))
	qualityLabel:setPosition(PT(goodBg:getContentSize().width*0.65,
								nameLabel:getPosition().y))
	goodBg:addChild(qualityLabel,0)
	
	--强化标题
	local titleLabel=CCLabelTTF:create(Language.EQUIP_AFTER .. ":",FONT_NAME,FONT_DEF_SIZE)
	titleLabel:setAnchorPoint(PT(0,0))
	titleLabel:setPosition(PT(goodBg:getPosition().x+goodBg:getContentSize().width*1.075,
								goodBg:getPosition().y+goodBg:getContentSize().height-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)
		
	--增加的属性
	local tableInfo=info.AbilityList or {}
	local rowH=titleLabel:getContentSize().height
	local startX=titleLabel:getPosition().x
	local startY=titleLabel:getPosition().y-rowH*1.5
	local colW=pWinSize.width*0.2
	local afterInfo=info.AbilityList1 or {}
	for k, v in pairs(tableInfo) do
		local str=Language.BAG_TYPE_[v.AbilityType] .. ":"
		local lAttribute=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
		lAttribute:setAnchorPoint(PT(0,0))
		lAttribute:setPosition(PT(startX,startY-(k-1)*rowH*2))
		layer:addChild(lAttribute,0)
		local baseNum=CCLabelTTF:create(v.BaseNum,FONT_NAME,FONT_SM_SIZE)
		baseNum:setAnchorPoint(PT(0,0))
		baseNum:setPosition(PT(lAttribute:getPosition().x,
								lAttribute:getPosition().y-rowH))
		layer:addChild(baseNum,0)
		---
		
		local toSprite=CCSprite:create(P("common/list_1189.png"))
		toSprite:setAnchorPoint(PT(0,0.5))
		toSprite:setPosition(PT(baseNum:getPosition().x+baseNum:getContentSize().width,
								baseNum:getPosition().y+baseNum:getContentSize().height/2))	
		layer:addChild(toSprite,0)
		
		local afterNum=CCLabelTTF:create( afterInfo[k].BaseNum,FONT_NAME,FONT_SM_SIZE)
		afterNum:setAnchorPoint(PT(0,0))
		afterNum:setPosition(PT(toSprite:getPosition().x+toSprite:getContentSize().width,
								baseNum:getPosition().y))
		layer:addChild(afterNum,0)
	end
	startY=startY-(#tableInfo-1)*rowH*2
	
	--等级变化
	local levelTile=CCLabelTTF:create(Language.IDS_LEVEL,FONT_NAME,FONT_DEF_SIZE)
	levelTile:setAnchorPoint(PT(0,0))
	levelTile:setPosition(PT(startX,startY-rowH*3))
	layer:addChild(levelTile,0)
	local levelNum=info.CurLevel
	local baseNum=CCLabelTTF:create(string.format(Language.IDS_LVSTR,levelNum),FONT_NAME,FONT_SM_SIZE)
	baseNum:setAnchorPoint(PT(0,0))
	baseNum:setPosition(PT(levelTile:getPosition().x,
							levelTile:getPosition().y-rowH))
	layer:addChild(baseNum,0)
	---

	if info.IsMaxLv == 0 then--是否已经强化到最大等级   0：否1：是 
		local levAfter=levelNum+1
		local toSprite=CCSprite:create(P("common/list_1189.png"))
		toSprite:setAnchorPoint(PT(0,0.5))
		toSprite:setPosition(PT(baseNum:getPosition().x+baseNum:getContentSize().width,
								baseNum:getPosition().y+baseNum:getContentSize().height/2))	
		layer:addChild(toSprite,0)
								
		local afterNum=CCLabelTTF:create(string.format(Language.IDS_LVSTR,levAfter),
													FONT_NAME,FONT_SM_SIZE)
		afterNum:setAnchorPoint(PT(0,0))
		afterNum:setPosition(PT(toSprite:getPosition().x+toSprite:getContentSize().width,
								baseNum:getPosition().y))
		layer:addChild(afterNum,0)
		
		---需求
		startY=baseNum:getPosition().y-rowH*2
		local needLabel=CCLabelTTF:create(Language.EQUIP_YICI .. ":" .. info.StrongMoney..Language.IDS_GOLD ,FONT_NAME,FONT_SM_SIZE)
		needLabel:setAnchorPoint(PT(0,0))
		needLabel:setPosition(PT(startX,startY))
		layer:addChild(needLabel,0)	
		
		local needLabel1=CCLabelTTF:create(Language.EQUIP_SHICI .. ":" .. info.TenTimesStrongMoney..Language.IDS_GOLD ,FONT_NAME,FONT_SM_SIZE)
		needLabel1:setAnchorPoint(PT(0,0))
		needLabel1:setPosition(PT(startX,needLabel:getPosition().y-needLabel1:getContentSize().height-SY(5)))
		layer:addChild(needLabel1,0)		
		
	else
		local afterNum=CCLabelTTF:create(Language.ROLE_LVMAX,
													FONT_NAME,FONT_SM_SIZE)
		afterNum:setColor(ZyColor:colorRed())
		afterNum:setAnchorPoint(PT(0,0))
		afterNum:setPosition(PT(baseNum:getPosition().x,
								baseNum:getPosition().y-baseNum:getContentSize().height))
		layer:addChild(afterNum,0)		
	end
	

	
---关闭按钮
	local colW=pWinSize.width/2
	local closeBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.IDS_BACK)
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(colW/2-closeBtn:getContentSize().width/2,
							contentBg:getPosition().y-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(gobackList)
	
	--强化按钮一次
	local strenBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_YICI_1,nil,FONT_SMM_SIZE)
	strenBtn:setAnchorPoint(PT(0,0))
	strenBtn:setPosition(PT(colW-strenBtn:getContentSize().width/2,
							closeBtn:getPosition().y))
	strenBtn:addto(layer,0)	
	strenBtn:registerScriptHandler(strenWeaponAction)
	
	--强化按钮十次
	local strenBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_SHICI_1,nil,FONT_SMM_SIZE)
	strenBtn:setAnchorPoint(PT(0,0))
	strenBtn:setPosition(PT(colW*1.5-strenBtn:getContentSize().width/2,
							closeBtn:getPosition().y))
	strenBtn:addto(layer,0)	
	strenBtn:registerScriptHandler(strenWeaponAction10)
	
end;

--请求强化接口
function strenWeaponAction()
	if not isClick then
	isClick =true		
	shicitype=1
	actionLayer.Action1204(mScene,nil,mDetailInfo.UserItemID,1 )
	end
end;
--十次
function strenWeaponAction10()
	if not isClick then
		isClick =true		
		shicitype=10		
		actionLayer.Action1204(mScene,nil,mDetailInfo.UserItemID,10 )
	end
end;

--返回列表
function gobackList()
	releaseStrenLayer()
	--如果有执行强化接口则刷新列表
	if isStrenAction then
		isStrenAction=nil
		showLayer()
	end
end;

-- 初始化资源、成员变量
function initResource()
	personalInfo=PersonalInfo.getPersonalInfo()
end

-- 释放资源
function releaseResource()
 	mScene = nil 		-- 场景
 	mLayer = nil 	
	mNoneLabel=nil
 	mContentLayer=nil
 	mDetailLayer=nil
 	mStrenLayer=nil
 	isSellLayer=nil
 	mTopLayer=nil
 	isStrenAction=nil
 	isClick=nil
 	mSellLayer=nil
end

function releaseAnimationLayer()
	if mAnimationLayer then
		mAnimationLayer:getParent():removeChild(mAnimationLayer,true)
		mAnimationLayer=nil
	end
end;
--
function animationLayer(serverData)
	releaseAnimationLayer()
	local layer=CCLayerColor:create(ccc4(0,0,0,255))
	--local layer=CCLayer:create()
	mAnimationLayer=layer
	for k=1 ,2 do
		local actionBtn=UIHelper.createActionRect(pWinSize)
		actionBtn:setPosition(PT(0,0))
		layer:addChild(actionBtn,0)
	end
	 mScene:addChild(layer,2)
	 local sprite=Sprite:new("donghua_1001")
	 sprite:setPosition(pWinSize.width/2,pWinSize.height/2)
	 sprite:addto(layer,0)
	if shicitype==1 then
		 bgPath = "common/list_8006_1.png"
		 if serverData.StrongLv > 1 then
		 	bgPath = "common/list_8006.png"
		 end	
	elseif shicitype==10 then
		bgPath = "common/list_3040.png"
		 if serverData.StrongLv > 10 then
		 	bgPath = "common/list_3041.png"
		 end
	end		
	 local str=string.format(Language.EQUIP_TIP2,mDetailInfo.CurLevel+1 or 1)
	 local bgSprite=CCSprite:create(P(bgPath))
	 bgSprite:setAnchorPoint(PT(0.5,0.5))
	 bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2)) 
	 layer:addChild(bgSprite,0)
	 local numTable={
	 "list_8000", "list_8001","list_8002", "list_8003","list_8004", "list_8005", }
--	 local path=string.format("common/%s.png",numTable[serverData.StrongLv])
--	 local numSprite=CCSprite:create(P(path))
	 numSprite=WeaponScene.getNumberSprite(serverData.StrongLv)
	 numSprite:setAnchorPoint(PT(0,0.5))
	 numSprite:setPosition(PT(bgSprite:getContentSize().width*0.72,bgSprite:getContentSize().height/2)) 
	 bgSprite:addChild(numSprite,0)

	 bgSprite:setScale(0.5)
	local point=PT(pWinSize.width/2,pWinSize.height*0.3)
	local action=CCScaleTo:create(0.5,1)
	local funcName = CCCallFuncN:create(WeaponScene.animationOver)
	local action2 = CCSequence:createWithTwoActions(CCDelayTime:create(0.5),funcName);		 
	local action1 = CCMoveTo:create(0.3, point) 
	local actionHarm=CCSequence:createWithTwoActions(action1,action2)
	local lastAction=CCSpawn:createWithTwoActions(action,actionHarm)
	bgSprite:runAction(lastAction)
end;

--获取数字图片
function getNumberSprite(nNumber,type)
	local imageFile = "common/list_3042.png"
	local texture = IMAGE(imageFile)
	if texture == nil then
		return nil
	end
	local txSize = texture:getContentSize()
	local strNumber = tostring(nNumber)
	strNumber=strNumber or 0
	strNumber=math.abs(strNumber)
	local nLength = string.len(strNumber)
	local pNode = CCNode:create()
	local nWidth = txSize.width/3
	local nHeight = txSize.height/4
	local subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake(0, 3*nHeight,nWidth,nHeight))
	local nLeft =-nWidth
	if type ~=1 then
		local subSprite = CCSprite:createWithSpriteFrame(subFrame)
		pNode:setPosition(PT(0, 0))
		pNode:addChild(subSprite, 0)
		subSprite:setAnchorPoint(PT(0,0))
		subSprite:setPosition(PT(0, 0 ))
		nLeft =0
	end
	for i = 1,nLength do
		local nDig = tonumber(string.sub(strNumber, i, i))
		if nDig == 0 then
			subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake(nWidth, 3*nHeight,nWidth,nHeight))
		else
			subFrame = CCSpriteFrame:createWithTexture(texture, CCRectMake((nDig- 1)%3*nWidth,math.floor((nDig -1)/3)*nHeight,nWidth,nHeight))
		end
		subSprite = CCSprite:createWithSpriteFrame(subFrame)
		pNode:addChild(subSprite, 0)
		subSprite:setAnchorPoint(PT(0,0))
		nLeft = nLeft + nWidth
		subSprite:setPosition(PT(nLeft, 0 ))

	end
	pNode:setContentSize(SZ(nLeft, subSprite:getContentSize().height))
	return pNode
end

function animationOver()
	releaseAnimationLayer()
	local data = {}
	data.type = 2
	actionLayer.Action1202(mScene,false,mDetailInfo.UserItemID, nil, data)
end;

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	--装备列表
	if actionID==1205 then
		local serverData=actionLayer._1205Callback(pScutScene,lpExternalDate)
		if serverData then
			if userData == 1 then
				mServerData=serverData.RecordTabel 
				refreshItemList()
			elseif userData == 2 then
				mServerData=serverData.RecordTabel
				createSellLayer(mServerData)
			elseif userData == 3 then
				releaseDetailLayer()
				releaseContentLayer()
				isChangeEquip=true
				mServerData=serverData.RecordTabel 
				createSellLayer(mServerData,1)
			end
		end
	--装备详情
	elseif actionID==1202 then
		local serverData=actionLayer._1202Callback(pScutScene,lpExternalDate)
		local type = userData.type
		if userData.index then
			serverData.index = userData.index
		end
		if serverData then
			if type == 1 then--详情
				createDetailLayer(serverData)
			elseif type == 2 then--强化
				mDetailInfo=serverData
				releaseDetailLayer() 
				createStrenLayer(serverData)
			end			
		end
	--强化接口
	elseif actionID==1204 then
    		if ZyReader:getResult() == eScutNetSuccess then	
    			isStrenAction=true
		--	ZyToast.show(pScutScene, Language.EQUIP_STRENTIP,0.4,0.2)
			local serverData=actionLayer._1204Callback(pScutScene,lpExternalDate)
			animationLayer(serverData)
			MainMenuLayer.refreshWin()
    		else
			ZyToast.show(pScutScene, ZyReader:readErrorMsg())
    		end	
    	 --未装备
	elseif actionID==1208 then
		local serverData=actionLayer._1208Callback(pScutScene,lpExternalDate)
		if serverData then
			mServerData=serverData.GeneralEquiptList or {}
			createSellLayer(mServerData)
		end
	--出售装备
	elseif actionID==7006 then
    		if ZyReader:getResult() == eScutNetSuccess then	
    			local pirce=ZyReader:getInt()
    			ZyToast.show(pScutScene, string.format(Language.EQUIP_TIP3,pirce),0.8,0.2)
			MainMenuLayer.refreshWin()
    		--	actionLayer.Action1205(mScene,nil,weapType[mCurrentTab], 2)
    			actionLayer.Action1208(mScene,nil)
    		else
    			ZyToast.show(pScutScene, ZyReader:readErrorMsg())
    		end
	end
	isClick=false
	
	commonCallback.networkCallback(pScutScene, lpExternalData)
end

---延迟进行方法
function delayExec(funName,nDuration)
	local action = CCSequence:createWithTwoActions(
	CCDelayTime:create(nDuration),
	CCCallFunc:create(funName));
	mLayer:runAction(action)
end