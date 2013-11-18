------------------------------------------------------------------
-- BagScene.lua
-- Author     :JUNM CHEN
-- Version    : 1.0
-- Date       :
-- Description: ,背包系统
------------------------------------------------------------------
module("BagScene", package.seeall)
require("layers.SynthesisLayer")--合成
require("scenes.GiftScene")--合成


-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer=nil
local mBagLayer=nil
local mServerData=nil
local mDetailLayer=nil
local m_listTable=nil
local topHeight=nil
local mGotoTopup=nil
userItemID = nil 
local mInputEdit=nil
local mInputLayer=nil
goodId = nil 
--物品类型
local itemType={
[1]=9,
[2]=7,
[3]=15,
}
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
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
	
	-- 添加背景
	if not  mCurrentTab then
		mCurrentTab=1
	end	

	topHeight=pWinSize.height*0.17
	local bgLayer=createContentLayer()
	bgLayer:setPosition(PT(0,0))
	mLayer:addChild(bgLayer,0)

	m_listTable={}
	m_listTable.currentPage=1
	m_listTable.gotoPage=1
	m_listTable.maxPage=1
	m_listTable.PageSize=300
	local tabStr={Language.BAG_PROPS,Language.BAG_MATERIAL,Language.BAG_PAPER}
	mTabBar=createTabbar(tabStr,mLayer)
	
	showLayer()	
	MainMenuLayer.init(1, mScene)
end

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


---创建背景层
function  createContentLayer()
	local layer=CCLayer:create()
	layer:setAnchorPoint(PT(0,0))
	--大背景	
	local bgSprite=CCSprite:create(P("common/list_1015.png"))
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setScaleX(pWinSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(pWinSize.height/bgSprite:getContentSize().height)
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(bgSprite,0)
	--中间层
	local midSprite=CCSprite:create(P("common/list_1040.png"))
	local boxSize=SZ(pWinSize.width, pWinSize.height*0.68)
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)

	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	layer:addChild(midSprite,0)
	return layer
end;

--创建tabbar
function  createTabbar(tabStr,layer)
	local tabBar=nil
	local tabBar=nil
	if tabStr then
	  	tabBar=ZyTabBar:new(nil,nil,tabStr,FONT_NAME,FONT_SM_SIZE,4)
		tabBar:selectItem(mCurrentTab)	
		local tabBar_X=pWinSize.width*0.04
		local tabBar_Y=pWinSize.height*0.77
		if posY then
			tabBar_Y=posY-tabBar:getContentSize().height-SY(4)
		end
		tabBar:setAnchorPoint(PT(0,0))
		tabBar:setPosition(PT(tabBar_X,tabBar_Y))
		tabBar:setCallbackFun(callbackTabBar)
		tabBar:addto(layer,1)
	end
	return tabBar
end;


----tabbar响应
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
		actionLayer.Action1101(mScene,false,1,m_listTable.PageSize,itemType[mCurrentTab])
	end
end;


function  releaseBagLayer()
	releaseNoLabel()
	 releaseDetailLayer()
	if mBagLayer then
		mBagLayer:getParent():removeChild(mBagLayer,true)
		mBagLayer=nil
	end
end;

function  createBagLayer(serverData)
	 releaseBagLayer()
	local layer=CCLayer:create()
	mLayer:addChild(layer,0)
	mBagLayer=layer
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	local startY=mTabBar:getPosition().y  

	 -------------------标题
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.55)
	local startX=pWinSize.width/2-listSize.width*0.45
	local titleSize=SZ(pWinSize.width*0.9,pWinSize.height*0.6)
--[[
	local tipLabel=CCLabelTTF:create(string.format(Language.BAG_TIP,serverData.GridNum,serverData.OccupyNum),FONT_NAME,FONT_SM_SIZE)
	tipLabel:setAnchorPoint(PT(0,0))
	tipLabel:setPosition(PT(startX,startY-tipLabel:getContentSize().height*1.5))
	layer:addChild(tipLabel,0)
	
	--扩充按钮
	local moreBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.BAG_MORE)
	moreBtn:setAnchorPoint(PT(0,0))
	moreBtn:setPosition(PT(pWinSize.width*0.95-moreBtn:getContentSize().width,
					tipLabel:getPosition().y+tipLabel:getContentSize().height/2-moreBtn:getContentSize().height/2))
--	moreBtn:addto(layer,0)	
	moreBtn:registerScriptHandler(moreBagAction)

	startY=moreBtn:getPosition().y
		--]]
	 ---LIST 控件
	local listX=(pWinSize.width-listSize.width)/2
	local listY=startY-listSize.height-SY(2)
	local mListRowHeight=pWinSize.height*0.17
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setTouchEnabled(true)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	list:setTouchEnabled(true)
	layer:addChild(list,1)
	mList=list
end;

function moreBagAction()
	if not isClick then
		isClick=true
		actionLayer.Action1110(mScene,nil,4,1)
	end
end;

--刷新列表
function  refreshItemList()
	releaseNoLabel()
	
	if mList  and mServerData  and #mServerData>0 then
	--	refreshPage()
		mList:clear()
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
		local mBgTecture=IMAGE(Image.Image_normalItemBg)
		for k, v in pairs(mServerData) do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local itemLayer=createListItem(v,k,mBgTecture)
			listItem:addChildItem(itemLayer, layout)
			mList:addListItem(listItem, false)
		end
	else
		 createNoLabel()
	end
	
	if GuideLayer.judgeIsGuide(13) then
		isGuide = true
		GuideLayer.setScene(mScene)	
		GuideLayer.init()
	end
end;

--------没有好友提示
function  releaseNoLabel()
	if mNoneLabel then
		mNoneLabel:getParent():removeChild(mNoneLabel,true)
		mNoneLabel=nil
	end
end;

---
function  createNoLabel()
	releaseNoLabel()
	local noneLabel=CCLabelTTF:create(Language.BAG_TIP1,FONT_NAME,FONT_DEF_SIZE)
	mNoneLabel=noneLabel
	noneLabel:setAnchorPoint(PT(0.5,0.5))
	noneLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.5))
	mBagLayer:addChild(noneLabel,2)
end;

function createListItem(info ,index,mBgTecture)
	local layer=CCLayer:create()
	local boxSize=SZ(mList:getContentSize().width,mList:getRowHeight())
	local startX=SX(5)
	local startY=boxSize.height*0.9
	local colW=boxSize.width/5
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
	goodBg:setPosition(PT(startX,boxSize.height*0.55))
	layer:addChild(goodBg,0)	
	---
	local actionBtn=UIHelper.createActionRect(goodBg:getContentSize(),BagScene.checkAction,index)
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
	local goodName=CCLabelTTF:create(info.ItemName,FONT_NAME,FONT_SM_SIZE)
	goodName:setAnchorPoint(PT(0,0))
	local rowH=goodName:getContentSize().height
	goodName:setPosition(PT(startX,startY-rowH))
	layer:addChild(goodName,0)
	--说明文字
	local contentStr=string.format("<label>%s</label>",info.ItemDesc or Language.IDS_NONE)
	local contentLabel=ZyMultiLabel:new(contentStr,boxSize.width*0.7,FONT_NAME,FONT_SM_SIZE);
	contentLabel:setAnchorPoint(PT(0,0))
	contentLabel:setPosition(PT(goodName:getPosition().x,
								goodName:getPosition().y-contentLabel:getContentSize().height))
	contentLabel:addto(layer,0)
	--物品数量
	local str=Language.IDS_NUM .. ":" .. info.Num
	local changeNum=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	changeNum:setAnchorPoint(PT(0,0))
	changeNum:setPosition(PT(goodName:getPosition().x,
								contentLabel:getPosition().y-rowH))
	layer:addChild(changeNum,0)
	--价格
	if info.SalePrice and info.SalePrice > 0 then
		local priceStr = Language.BAG_PRICE..":"..info.SalePrice
		local priceNum=CCLabelTTF:create(priceStr,FONT_NAME,FONT_SM_SIZE)
		priceNum:setAnchorPoint(PT(0,0))
		priceNum:setPosition(PT(startX,
									changeNum:getPosition().y-rowH))
		layer:addChild(priceNum,0)
	end	

	--使用按钮
	local startX=boxSize.width*0.98
	
	if info.IsUse ==1 then
		local useBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.IDS_USE)
		useBtn:setAnchorPoint(PT(0,0))
		useBtn:setPosition(PT(startX-useBtn:getContentSize().width,
											useBtn:getContentSize().height*0.1))
		useBtn:addto(layer,0)	
		useBtn:setTag(index)
		useBtn:registerScriptHandler(useAction)
		startX=useBtn:getPosition().x-SX(5)
	end
	
	local sellBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_SELL)
	sellBtn:setAnchorPoint(PT(0,0))
	sellBtn:setPosition(PT(startX-sellBtn:getContentSize().width,
										sellBtn:getContentSize().height*0.1))
	sellBtn:addto(layer,0)	
	sellBtn:setTag(index)
	sellBtn:registerScriptHandler(sellItemAction)
	return layer
end;

--检查按钮
function checkAction(node)
	local tag=node:getTag()
	local itemInfo=mServerData[tag]
	if  itemInfo  then
		itemInfo.index=tag
		 createDetailLayer(itemInfo)
	end
end;

--物品卖出
function sellItemAction(node)
	local tag=node:getTag()
	local itemInfo=mServerData[tag]
	if  itemInfo  then
		if itemInfo.IsCostly==1 then
			local box = ZyMessageBoxEx:new()
			box:setTag(tag)
			box:doQuery(mScene, nil,Language.BAG_TIP7 , Language.IDS_SURE, Language.IDS_CANCEL,makeSellConfirm) 
		--
		else
			if not isClick then
			isClick=true
			actionLayer.Action7006(mScene,nil ,itemInfo.UserItemID, itemInfo.SalePrice)
			end	
		end

	end
end;



function makeSellConfirm(clickedButtonIndex,content,tag)
	if clickedButtonIndex == 1 then--确认
	local itemInfo=mServerData[tag]
	if not isClick then
		isClick=true
		actionLayer.Action7006(mScene,nil ,itemInfo.UserItemID)
	end
	end
end;

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
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height-boxSize.height))
	
	layer:addChild(bgSprite,0)
	
	--标题
	local titleLabel=CCSprite:create(P("title/list_1101.png"))
	titleLabel:setAnchorPoint(PT(0.5,0))
	titleLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.965-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)
	
	
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width*1.2,
							pWinSize.height-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(releaseDetailLayer)
	local startY=closeBtn:getPosition().y-titleLabel:getContentSize().height
	

	--简介
	local sizeBox=SZ(boxSize.width*0.9,pWinSize.height*0.5)
	local contentBg=CCSprite:create(P("common/list_1052.9.png"))
	contentBg:setAnchorPoint(PT(0.5,0))
	contentBg:setScaleY(sizeBox.height/contentBg:getContentSize().height)
	contentBg:setPosition(PT(pWinSize.width/2,
								startY-sizeBox.height))
	layer:addChild(contentBg,0)
	local startX=pWinSize.width/2-contentBg:getContentSize().width*0.48
	
	---	物品图片
	local goodBg=CCSprite:create(P("common/icon_8017_3.png"))
	goodBg:setAnchorPoint(PT(0,0))
	goodBg:setPosition(PT(startX,
				contentBg:getPosition().y+sizeBox.height*0.95-goodBg:getContentSize().height))
	layer:addChild(goodBg,0)
	local imgPath=string.format("bigitem/%s.png",info.MaxHeadID)
	local goodSprite=CCSprite:create(P(imgPath))
	goodSprite:setAnchorPoint(PT(0.5,0.5))
	goodSprite:setPosition(PT(goodBg:getContentSize().width/2,
									goodBg:getContentSize().height/2))
	goodBg:addChild(goodSprite,0)
	
	--增加攻击力
	--[[
	local attackLabel=CCLabelTTF:create(Language.BAG_INTRO .. ":",FONT_NAME,FONT_SM_SIZE)
	attackLabel:setAnchorPoint(PT(0,0))
	attackLabel:setPosition(PT(goodBg:getContentSize().width*0.15,SY(4)))
	goodBg:addChild(attackLabel,0)
	--]]
	
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
	if info.SalePrice and info.SalePrice>0 then
		--售价
		local sellLabel=CCLabelTTF:create(Language.BAG_PRICE .. ":",FONT_NAME,FONT_BIG_SIZE)
		sellLabel:setAnchorPoint(PT(0,0))
		sellLabel:setPosition(PT(titleLabel:getPosition().x,
							contentLabel:getPosition().y-sellLabel:getContentSize().height*3))
		layer:addChild(sellLabel,0)
		local priceLabel=CCLabelTTF:create(info.SalePrice .. Language.IDS_GOLD,FONT_NAME,FONT_DEF_SIZE)
		priceLabel:setAnchorPoint(PT(0,0))
		priceLabel:setPosition(PT(titleLabel:getPosition().x,
							sellLabel:getPosition().y-sellLabel:getContentSize().height*1.1))
		layer:addChild(priceLabel,0)
	end
	--使用按钮
	local colW=pWinSize.width/2

	local useBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.IDS_USE)
	useBtn:setAnchorPoint(PT(0,0))
	useBtn:setPosition(PT(colW*1.5-useBtn:getContentSize().width/2 ,
							contentBg:getPosition().y-useBtn:getContentSize().height*1.5))
	useBtn:addto(layer,0)	
	useBtn:setTag(info.index)
	useBtn:registerScriptHandler(useAction)
	useBtn:setVisible(false)
	if info.IsUse ==1 then
		useBtn:setVisible(true)	
	end
	
	--------
	local sellBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.EQUIP_SELL)
	sellBtn:setAnchorPoint(PT(0,0))
	sellBtn:setPosition(PT(pWinSize.width/2-sellBtn:getContentSize().width/2,
							useBtn:getPosition().y))
	sellBtn:addto(layer,0)
	sellBtn:setTag(info.index)	
	sellBtn:registerScriptHandler(sellItemAction)
	if info.IsUse ==1  then
		sellBtn:setPosition(PT(colW/2-sellBtn:getContentSize().width/2,
							useBtn:getPosition().y))
	end
--	if info.SalePrice<=0 then
--		sellBtn:setVisible(false)
--	end
end;

---使用响应
function useAction(node)
	local tag=node:getTag()
	local itemInfo=mServerData[tag]
	
	if  itemInfo  then
			--使用图纸
		if itemInfo.ItemType == 4 or itemInfo.ItemType == 5 or itemInfo.ItemType == 15  then--图纸
			if not isClick then
			isClick=true
			SynthesisLayer.setItemID(itemInfo.UserItemID)
			actionLayer.Action1601(mScene,nil ,itemInfo.ItemID)
			end
			--使用道具
		elseif itemInfo.ProType== 13 then
			 userItemID = itemInfo.UserItemID
			actionLayer.Action1113(mScene,nil ,userItemID,0)
		elseif itemInfo.ProType== 15 then
			if itemInfo.IsKey ==1 then
				userItemID = itemInfo.UserItemID
				BoxToScene.init(mScene,itemInfo.UserItemID)
--				actionLayer.Action12001(mScene,nil ,itemInfo.UserItemID)
			else
				local tipStr=string.format(Language.BAG_TIP12,itemInfo.ItemName)
				ZyToast.show(mScene,tipStr)
			end
		elseif itemInfo.ProType== 18 then
			createInputLayer()	
		elseif itemInfo.ItemType == 9 then
			if not isClick then
				isClick=true
				goodId = itemInfo.UserItemID
				GiftScene.init(mScene,goodId)	
--				actionLayer.Action1114(mScene,nil ,itemInfo.UserItemID)	
			end
		
			
		end
	end	
end;

function GiftFtoBagFunc(id)
	if not isClick then
		isClick=true
		actionLayer.Action1606(mScene,nil ,id)	
	end
end

--释放输入框
function  releaseInputLayer()
	if mInputEdit then
		mInputEdit:release()
		mInputEdit=nil
	end
	if mInputLayer then
		mInputLayer:getParent():removeChild(mInputLayer,true)
		mInputLayer=nil
	end
end;

--创建输入框
function  createInputLayer()
	releaseInputLayer()
	local layer=CCLayer:create()
	mInputLayer=layer
	mScene:addChild(layer,2)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
-------
	for k=1 ,2 do
		local pingbiBtn=UIHelper.createActionRect(pWinSize)
		pingbiBtn:setPosition(PT(0,0))
		layer:addChild(pingbiBtn,0)
	end

	local blackSprite=CCSprite:create(P("common/transparentBg.png"))
	blackSprite:setScaleX(pWinSize.width/blackSprite:getContentSize().width)
	blackSprite:setScaleY(pWinSize.height/blackSprite:getContentSize().height)
	blackSprite:setAnchorPoint(PT(0.5,0.5))
	blackSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(blackSprite,0)
	
	local bgSize=SZ(pWinSize.width*0.8,pWinSize.height*0.25)
	
	local bgSprite=CCSprite:create(P("common/list_1054.png"))
	local bgSize=SZ(pWinSize.width,bgSprite:getContentSize().height)
	bgSprite:setScaleX(bgSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(bgSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(bgSprite,0)
	
	local startX=pWinSize.width/2-bgSize.width*0.4
	local startY=pWinSize.height/2+bgSize.height*0.3
	
	--标题
	
	local titleTip=CCLabelTTF:create(Language.BAG_TIP5,FONT_NAME,FONT_DEF_SIZE)
	titleTip:setAnchorPoint(PT(0.5,0))
	titleTip:setPosition(PT(pWinSize.width/2,startY-titleTip:getContentSize().height*1.5))
	layer:addChild(titleTip,0)	

	--输入框
	local editSize=SZ(bgSize.width*0.8, titleTip:getContentSize().height*5)
	local startY=pWinSize.height-titleTip:getPosition().y+titleTip:getContentSize().height
	mInputEdit= CScutEdit:new();
	mInputEdit:init(true, false)
	mInputEdit:setRect(CCRect(pWinSize.width/2-bgSize.width*0.4,startY,editSize.width,editSize.height))
	
	--取消 确定按钮
	local cancelBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.IDS_CANCEL)
	cancelBtn:setAnchorPoint(PT(0,0))
	cancelBtn:setPosition(PT(startX,pWinSize.height/2-bgSize.height*0.4))
	cancelBtn:addto(layer,0)
	cancelBtn:registerScriptHandler(releaseInputLayer)
	
	local sureBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.IDS_SURE)
	sureBtn:setAnchorPoint(PT(0,0))
	sureBtn:setPosition(PT(pWinSize.width/2+bgSize.width*0.4-cancelBtn:getContentSize().width,
								cancelBtn:getPosition().y))
	sureBtn:addto(layer,0)
	sureBtn:registerScriptHandler(makeSureAction)
end;

--设置输入框可见
function  setEditVisible(value)
	if mInputEdit then
		mInputEdit:setVisible(value)
	end
end;

function  setEditSee()
	 setEditVisible(true)
end;
---
function makeSureAction()
	local content=mInputEdit:GetEditText()
	if content and string.len(content)>0 then
		if not isClick then
			isClick=true
			actionLayer.Action1023(mScene, nil, content)
		end
	end

end;

-- 退出场景
function closeScene()
	releaseDetailLayer()
	releaseBagLayer()
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
	if not mGotoTopup then
	mList=nil
	mNoneLabel=nil
	mBagLayer=nil
	mDetailLayer=nil
	mServerData=nil
	mLayer=nil
	mScene=nil
	end
	mGotoTopup=nil
end

function refreshWin()
	actionLayer.Action1101(mScene,false,m_listTable.gotoPage,m_listTable.PageSize,itemType[mCurrentTab])
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	--物品列表
	if actionID==1101 then
		
		local serverData=actionLayer._1101Callback(pScutScene,lpExternalDate)
		if serverData then		
			mServerData=serverData.RecordTabel
		      if serverData.PageCount>0 then
				m_listTable.maxPage=serverData.PageCount
			end
			m_listTable.currentPage=m_listTable.gotoPage
			createBagLayer(serverData)
			refreshItemList()
			MainMenuLayer.refreshWin()	
		end

		--使用人物卡
	elseif  actionID==1113 then
    		if ZyReader:getResult() == eScutNetSuccess then
    			local str=ZyReader:readString()
    			local potential=ZyReader:getInt()
    			
    			local labelStr = nil
    			if str then
    				labelStr = Language.BAG_TIP9 .. str
    			elseif potential then
    				labelStr=Language.BAG_TIP10 .. potential
    			end
    			ZyToast.show(pScutScene,labelStr)
			actionLayer.Action1101(mScene,false,1,100,itemType[mCurrentTab])
    		elseif ZyReader:getResult() == 1 then
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene, nil, Language.BAG_TIP8 , Language.IDS_SURE, Language.IDS_CANCEL,makeUseConfirm) 		
		else
        		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)	
		end

		--使用物品
	elseif  actionID==1606 then
--[[		local serverInfo=actionLayer._1606Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			local HasNextGift=serverInfo.HasNextGift
			local Content=serverInfo.Content
			if HasNextGift==1 then
				GiftScene.releaseResource()
				actionLayer.Action1114(mScene,nil ,goodId)
				ZyToast.show(mScene,Content,1,0.35)
			else
				GiftScene.releaseResource()
				MainMenuLayer.refreshWin()
				actionLayer.Action1101(mScene,false,m_listTable.gotoPage,m_listTable.PageSize,itemType[mCurrentTab])
				ZyToast.show(mScene,Content,1,0.35)
			end
		end;--]]
		GiftScene.networkCallback(pScutScene, lpExternalData)
    		
    		--物品合成
	elseif  actionID==1601 then
		local serverData=actionLayer._1601Callback(pScutScene,lpExternalDate)
		if serverData then
			releaseDetailLayer()
			SynthesisLayer.setDataInfo(serverData)
			SynthesisLayer.init(pScutScene)
		end
		--扩充
	elseif actionID==1110 then
    		if ZyReader:getResult() == eScutNetSuccess then
    			actionLayer.Action1101(mScene,false,1,100,itemType[mCurrentTab])
		elseif  ZyReader:getResult() ==1 then
			local box = ZyMessageBoxEx:new()
			local price=ZyReader:getInt()
			box:doQuery(pScutScene, nil, string.format(Language.BAG_TIP2,price) , Language.IDS_SURE, Language.IDS_CANCEL,makeConfirm) 
		elseif  ZyReader:getResult() ==2 then
				MainMenuLayer.refreshWin()
				actionLayer.Action1101(mScene,false,1,100,itemType[mCurrentTab])
		elseif  ZyReader:getResult() ==3 then
			local box = ZyMessageBoxEx:new()
			local contentStr=Language.BAG_TIP3

		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg() )
		end

		--出售
	elseif actionID==7006 then
    		if ZyReader:getResult() == eScutNetSuccess then
    		
    			releaseDetailLayer()
    			local price=ZyReader:getInt()
    			actionLayer.Action1101(mScene,false,1,100,itemType[mCurrentTab])
			--local pirce = userData
			if price and price>0 then
				ZyToast.show(pScutScene,string.format(Language.BAG_TIP11,price),1.5,0.25)	
			else
				ZyToast.show(pScutScene,Language.BAG_TIP4,1.5,0.25)	
			end
    			MainMenuLayer.refreshWin()
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg() )
		end
	--	MainMenuLayer.refreshWin()
	elseif actionID==1023 then
    		if ZyReader:getResult() == eScutNetSuccess then
    			releaseInputLayer()
    			MainMenuLayer.refreshWin()
			ZyToast.show(pScutScene,Language.BAG_TIP6 )
		else
			ZyToast.show(pScutScene,ZyReader:readErrorMsg() )
			releaseInputLayer()
		end
	elseif actionID==12001 then
--[[		local serverInfo = actionLayer._12001Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			local rollInfo = serverInfo
			BoxToScene.init(mScene,mLayer,rollInfo ,userItemID)
		end--]]
		BoxToScene.networkCallback(pScutScene, lpExternalData)
	elseif actionID == 12004 then--12004_抽奖接口
--[[		local serverInfo=actionLayer._12004Callback(pScutScene, lpExternalData)
		if serverInfo ~= nil then
			local priceNum = serverInfo.Postion 
			local HasNextBox = serverInfo.HasNextBox
			BoxToScene.startAction(priceNum,HasNextBox)
			
		else
			BoxToScene.setBtnState(true)
		end
		MainMenuLayer.refreshWin()
		--]]
		BoxToScene.networkCallback(pScutScene, lpExternalData)
	elseif 	actionID == 1114 then 
--[[			if mDetailLayer~=nil  then 
				mDetailLayer:getParent():removeChild(mDetailLayer,true)
				mDetailLayer = nil 
			end
			GiftScene.networkCallback(pScutScene, lpExternalData)
			GiftScene.init(mScene,mLayer,goodId)	--]]
			GiftScene.networkCallback(pScutScene, lpExternalData)
	elseif actionID == 1603 then
		SynthesisLayer.networkCallback(pScutScene, lpExternalData)
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)	
	end	

	isClick=false

end




function makeConfirm(clickedButtonIndex,content,tag)
	if clickedButtonIndex == 1 then--确认
	if not isClick then
		isClick=true
		actionLayer.Action1110(mScene,false,4,2)
	end
	end
end;


function makeUseConfirm(clickedButtonIndex,content,tag)
	if clickedButtonIndex == 1 then--确认
	if not isClick then
		isClick=true	
		actionLayer.Action1113(mScene,false,userItemID,1)
	end
	end
end;

---延迟进行方法
function delayExec(funName,nDuration)
    local  action = CCSequence:createWithTwoActions(
    CCDelayTime:create(nDuration),
    CCCallFunc:create(funName));
    mLayer:runAction(action)
end

--合成成功提示
function showCreatSuccess()
    ZyToast.show(mScene, Language.HUNT_FUSIONTIP, 1.5, 0.35)
end
