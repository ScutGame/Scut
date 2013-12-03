------------------------------------------------------------------
-- shopLayer.lua
-- Author     : chenjp

-- Version    : 1.0
-- Date       :
-- Description: 商店系统
------------------------------------------------------------------

module("ShopScene", package.seeall)

--require("scenes.MainScene")

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

mScene = nil 		-- 场景
local mLayer = nil 		-- 层
local shopLayer=nil	--商店层

local shopBg=nil  --商店背景
local headShopBtn=nil --头像商店
local coinShopBtn=nil --金豆商店
local yuanbao=nil
local backType=nil
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	createScene()
	CCDirector:sharedDirector():pushScene(mScene)
end
-- 退出场景
function popScene()
	releaseResource()
	CCDirector:sharedDirector():popScene()

end

--购买商品
function buyCommodity(index)
	local tab=index
	--[[
	if index then
		tab=index:getTag()
	end;
	]]
	send(7002,tab)
	
end;

function exitShop()
	releaseResource()
	if mLayer then
		mLayer:getParent():removeChild(mLayer, true)
		mLayer = nil
	end
	mLayer = nil
	CCDirector:sharedDirector():popScene()
end;

--查看详细
function seeDetail(index)
	    --透明背景
	local tag=index
	--[[
	if index~=nil then
		tag=index:getTag()
	end;
	]]
	local commodityDetailInfo=shopInfoTable[tag]
	
	shopLayer = CCLayer:create()
	mScene:addChild(shopLayer, 0)
	
	local transparentBg=Image.tou_ming;--背景图片
	local menuItem =CCMenuItemImage:create(P(transparentBg),P(transparentBg));
	local menuBg=CCMenu:createWithItem(menuItem);
	menuBg:setContentSize(menuItem:getContentSize());
	menuBg:setAnchorPoint(CCPoint(0,0));
	menuBg:setScaleX(pWinSize.width/menuBg:getContentSize().width*2)
	menuBg:setScaleY(pWinSize.height/menuBg:getContentSize().height*2);
	menuBg:setPosition(CCPoint(0,0));
	shopLayer:addChild(menuBg,0);
	
	--背景
	local detailBg= CCSprite:create(P("common/panle_1015.9.png"));
	detailBg:setAnchorPoint(PT(0,0))
	detailBg:setScaleX(pWinSize.width*0.5/detailBg:getContentSize().width)
	detailBg:setScaleY(pWinSize.height*0.62/detailBg:getContentSize().height);
	detailBg:setPosition(PT((pWinSize.width-pWinSize.width*0.5)/2,(pWinSize.height-pWinSize.height*0.62)/2))
	shopLayer:addChild(detailBg,0)
	
	--物品图标框
	local detailImg= ZyButton:new("common/panle_1014_2.png","common/panle_1014_2.png",nil, nil,FONT_NAME,FONT_SM_SIZE)
	detailImg:setAnchorPoint(PT(0.5,0))
	detailImg:setPosition(PT(pWinSize.width/2,
		detailBg:getPosition().y+pWinSize.height*0.57-detailImg:getContentSize().height))
	detailImg:addto(shopLayer)
	
	if commodityDetailInfo.HeadID then
		local headStr=string.format("headImg/%s.png",commodityDetailInfo.HeadID)
		detailImg:addImage(headStr)
	end;
	
	--商品名称
    local nameLabel=CCLabelTTF:create(commodityDetailInfo.ItemName,FONT_NAME,FONT_FMM_SIZE);
    nameLabel:setAnchorPoint(PT(0.5,0));
    nameLabel:setPosition(PT(detailImg:getPosition().x,detailImg:getPosition().y-SY(2)-nameLabel:getContentSize().height*1.1))
    shopLayer:addChild(nameLabel,0)
    nameLabel:setColor(ccYELLOW2)
	--离开按钮
	local exitBtn=ZyButton:new("button/panle_1014_3.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	exitBtn:setAnchorPoint(PT(1,1))
	exitBtn:setPosition(PT(detailBg:getPosition().x+pWinSize.width*0.48,
		detailBg:getPosition().y+pWinSize.height*0.58))
	exitBtn:registerScriptTapHandler(ShopScene.exitDetail)
	exitBtn:addto(shopLayer)
	--..commodityDetailInfo.ItemPrice
	--商品价格
	local priceLabel=CCLabelTTF:create(Language.SHOP_PRICE..commodityDetailInfo.ItemPrice..Language.SHOP_GOLD,FONT_NAME,FONT_FMM_SIZE);
	priceLabel:setAnchorPoint(PT(0.5,0))
	priceLabel:setPosition(PT(detailImg:getPosition().x,
			nameLabel:getPosition().y-nameLabel:getContentSize().height*1.1))
    priceLabel:setColor(ccYELLOW2)
	shopLayer:addChild(priceLabel)
	
	
	if commodityDetailInfo.GainGameCoin ~=0 then
		--金豆数量
	--	local coinStr=string.format("<label fontsize='15'>%s</label>",Language.SHOP_GAINGAMECOIN..commodityDetailInfo.GainGameCoin )
		local coinLabel=CCLabelTTF:create(Language.SHOP_GAINGAMECOIN..commodityDetailInfo.GainGameCoin,FONT_NAME,FONT_FMM_SIZE);
		coinLabel:setAnchorPoint(PT(0.5,0))
		coinLabel:setPosition(PT(priceLabel:getPosition().x,
				priceLabel:getPosition().y-nameLabel:getContentSize().height*1.1))
		shopLayer:addChild(coinLabel)
		    coinLabel:setColor(ccYELLOW2)
		
	end;
	
	--购买按钮
	local buyBtn=ZyButton:new("button/button_1013.png","button/button_1014.png",nil, nil,FONT_NAME,FONT_SM_SIZE)
	buyBtn:setAnchorPoint(PT(0.5,0))
	buyBtn:setPosition(PT(pWinSize.width/2,detailBg:getPosition().y+pWinSize.height*0.0375))
	buyBtn:registerScriptTapHandler(ShopScene.buyCommodity)
	buyBtn:setTag(tag)
	buyBtn:addImage("title/button_1027.png")
	buyBtn:addto(shopLayer)
	
end;

--头像商店
function headShop()
	coinShopBtn:unselected()
	headShopBtn:selected()
	send(7001,1)
end;

function coinShop()
	headShopBtn:unselected()
	coinShopBtn:selected()
	send(7001,2)
end;
--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
end
-- 释放资源
function releaseResource()
	if list then
		list:clear();
		list=nil;
	end
end
-- 创建场景
function createScene(type,back)
	local scene = ScutScene:new()
	backType=back
	-- 注册网络回调
	scene:registerCallback(networkCallback)
	mScene = scene.root
	
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, -2)
	-- 注册触屏事件

	-- 此处添加场景初始内容
	
	--透明背景
	local transparentBg=Image.tou_ming;--背景图片
	local menuItem =CCMenuItemImage:create(P(transparentBg),P(transparentBg));
	local menuBg=CCMenu:createWithItem(menuItem);
	menuBg:setContentSize(menuItem:getContentSize());
	menuBg:setAnchorPoint(CCPoint(0,0));
	menuBg:setScaleX(pWinSize.width/menuBg:getContentSize().width*2)
	menuBg:setScaleY(pWinSize.height/menuBg:getContentSize().height*2);
	menuBg:setPosition(CCPoint(0,0));
	mLayer:addChild(menuBg,0);
	
	--背景图
	local bgLayer = UIHelper.createUIBg(nil,nil,ZyColor:colorWhite(),nil,true);--没有背景框
	mLayer:addChild(bgLayer);

	--头像商店
	headShopBtn=ZyButton:new(Image.image_shop,Image.image_shop_1,nil,nil ,FONT_NAME,FONT_SM_SIZE)
	headShopBtn:setAnchorPoint(PT(0,0))
	headShopBtn:setPosition(PT(SX(23),pWinSize.height*0.86-SY(1)))
	headShopBtn:registerScriptTapHandler(headShop)
	headShopBtn:addImage("title/button_1024.png")
	headShopBtn:addto(mLayer)
	
	--金豆商店
	coinShopBtn=ZyButton:new(Image.image_shop,Image.image_shop_1,nil, nil,FONT_NAME,FONT_SM_SIZE)
	coinShopBtn:setAnchorPoint(PT(0,0))
	coinShopBtn:setPosition(PT(headShopBtn:getPosition().x+headShopBtn:getContentSize().width,pWinSize.height*0.86-SY(1)))
	coinShopBtn:registerScriptTapHandler(coinShop)
	coinShopBtn:addImage("title/button_1025.png")
	coinShopBtn:addto(mLayer)

	--返回按钮
	local exitBtn=ZyButton:new(Image.image_exit,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)	
	local pos_Y = pWinSize.height-exitBtn:getContentSize().height*0.82
	

	local exitText=ZyButton:new("title/panle_1039.png")
	exitText:setAnchorPoint(PT(0,0.5))
	exitText:setPosition(PT(pWinSize.width-exitText:getContentSize().width-SY(25), pos_Y))
	exitText:addto(mLayer, 0)
	exitText:registerScriptTapHandler(exitShop)
	
	exitBtn:setAnchorPoint(PT(0,0.5))
	exitBtn:setPosition(PT(exitText:getPosition().x-exitText:getContentSize().width-SY(5), pos_Y))
	exitBtn:registerScriptTapHandler(exitShop)
	exitBtn:addto(mLayer)
  
	--元宝
	local gameCoinImg= CCSprite:create(P("common/panle_1067.png"));
	gameCoinImg:setAnchorPoint(PT(0,0.5))
	gameCoinImg:setPosition(PT(pWinSize.width*0.45, pos_Y))
	mLayer:addChild(gameCoinImg,0)
	
	local gameGoldnum=PersonalInfo.getPersonalInfo()._Gold
	gameGold=CCLabelTTF:create(gameGoldnum,FONT_NAME,FONT_SM_SIZE);
	gameGold:setAnchorPoint(PT(0,0.5))
	gameGold:setPosition(PT(gameCoinImg:getPosition().x+gameCoinImg:getContentSize().width+SX(3), pos_Y))
	mLayer:addChild(gameGold)	
	
	
	layer_1= CCLayer:create()
	mScene:addChild(layer_1, 0)
	if not type then
	headShop()
	else
	coinShop()
	end
	SlideInLPushScene(mScene)
end



--展示商品
function showCommodity()
	local listSize = SZ(pWinSize.width*0.9, pWinSize.height*0.72)
	local list_x=(SX(15)+pWinSize.width*0.92/2-listSize.width/2)
	local list_y=SY(32)
	local listRowH=listSize.height/2
	
	if  layer_1 then
		layer_1:getParent():removeChild(layer_1, true)
		layer_1 = nil
	end;
	
	layer_1= CCLayer:create()
	mScene:addChild(layer_1, 0)
	
	gameGold:setString(yuanbao)
	list = ScutCxList:node(listRowH, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0, 0))
	list:setPosition(PT(list_x, list_y))
	list:setTouchEnabled(true)
	layer_1:addChild(list,0)
	
	local layout=CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val =0
	layout.val_y.val.pixel_val =0	
	local length=#shopInfoTable
	local row=nil
	local col=3
	if length%3==0 then
		row = math.floor(length/3)
	else
		row=math.floor(length/3)+1
	end;
	local boxSize=SZ(listSize.width*0.3,pWinSize.height*0.25)
	local startX=listSize.width/6
	local colW=listSize.width/3
	for index=1,row do
		local listItem = ScutCxListItem:itemWithColor(ccc3(32, 24, 3))
		listItem:setOpacity(0)
		if index==row then
			if length%3==0 then
				col=3
			else	
				col=length%3
			end;
		else
			col=3
		end;	
		for  k=1 ,col do
			local layer=CCLayer:create()	
			local i=k+(index-1)*3
			
			--物品背景
			local commodityBg= CCSprite:create(P("common/panel_1002_2.9.png"));
			commodityBg:setScaleX(boxSize.width/commodityBg:getContentSize().width)
			commodityBg:setScaleY(boxSize.height/commodityBg:getContentSize().height)
	  		commodityBg:setAnchorPoint(PT(0.5,0))
	  		local headColW=startX+(k-1)%col*colW
	  		local headRowH=math.floor((k-1)/col)*(pWinSize.height*0.22*2)
			commodityBg:setPosition(PT(headColW,0))
			layer:addChild(commodityBg,0)
			
			local midSprite=CCSprite:create(P(Image.image_roomBg));
			midSprite:setScaleX(boxSize.width*0.85/midSprite:getContentSize().width)
			midSprite:setScaleY(boxSize.height*0.85/midSprite:getContentSize().height)
	  		midSprite:setAnchorPoint(PT(0.5,0))
			midSprite:setPosition(PT(commodityBg:getPosition().x, commodityBg:getPosition().y + boxSize.height*0.07))
			layer:addChild(midSprite,0)
			
			local actionBtn=UIHelper.createActionRect(boxSize,seeDetail,i)
			actionBtn:setPosition(PT(commodityBg:getPosition().x-boxSize.width/2,
									commodityBg:getPosition().y))
			layer:addChild(actionBtn,0)
			
			--物品图标框
			local headBgImg= CCSprite:create(P("common/panle_1014_2.png"));
	  		headBgImg:setAnchorPoint(PT(0,0.5))
	  		headBgImg:setScale(0.8)
			headBgImg:setPosition(PT(commodityBg:getPosition().x-boxSize.width*0.4,
				commodityBg:getPosition().y+boxSize.height/2))
	  		layer:addChild(headBgImg,0)
	  		
	  		if shopInfoTable[i].HeadID then
	  			local headStr=string.format("headImg/%s.png",shopInfoTable[i].HeadID)
		  		local headImg= CCSprite:create(P(headStr));
		  		headImg:setAnchorPoint(PT(0.5,0.5))
				headImg:setPosition(PT(headBgImg:getContentSize().width/2,
					headBgImg:getContentSize().height/2))
		  		headBgImg:addChild(headImg,0)
	  		end;
			
	
			local priceStr=string.format("<label >%s</label>",shopInfoTable[i].ItemPrice..Language.SHOP_GOLD )
			local priceLabel=ZyMultiLabel:new(priceStr,pWinSize.width*0.13,FONT_NAME,FONT_FM_SIZE);
			priceLabel:setPosition(PT(headBgImg:getPosition().x+headBgImg:getContentSize().width*0.82,
					headBgImg:getPosition().y+headBgImg:getContentSize().height*0.2-priceLabel:getContentSize().height))
			priceLabel:addto(layer,1)
						
			local nameStr=string.format("<label >%s</label>",shopInfoTable[i].ItemName )
			local nameLabel=ZyMultiLabel:new(nameStr,pWinSize.width*0.1,FONT_NAME,FONT_FM_SIZE);
			nameLabel:setPosition(PT(priceLabel:getPosition().x,
					priceLabel:getPosition().y-nameLabel:getContentSize().height-SY(5)))
			nameLabel:addto(layer,1)

			listItem:addChildItem(layer, layout)
			list:setRowHeight(pWinSize.height*0.22+SY(15))
			
		end
		list:addListItem(listItem, false)
	end;
end;




function send(actionID,num)
	if  actionID==7001 then
		    mCurrentTab=num;
    		actionLayer.Action7001(mScene,false,num,1,100)
   	elseif actionID==7002 then
   		local ItemID=shopInfoTable[num].ItemID
   		actionLayer.Action7002(mScene,false,ItemID)
  	 end
end;

--离开详细界面
function exitDetail()
	if  shopLayer then
		mScene:removeChild(shopLayer,true)
	end;
end;

function refreshWin()
	--actionLayer.Action1008(mScene,nil)
end;

function setNewManInput(clickedButtonIndex)
	if clickedButtonIndex ==1 then
--		TopUpScene.init()
	end
end;

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID();
	 local userData = ZyRequestParam:getParamData(lpExternalData)
	
	 if actionId ==1008 then
      	local personalInfo=actionLayer._1008Callback(pScutScene, lpExternalData)
  	 	if personalInfo~=nil then
  	 		PersonalInfo.getPersonalInfo()._Wings=nil
  	 		PersonalInfo.setPersonalInfo(personalInfo)
  	 	end
	elseif actionId == 7001 then
		shopInfo =actionLayer._7001Callback(pScutScene, lpExternalData)
		if shopInfo~=nil then
			shopInfoTable=shopInfo.RecordTabel
			yuanbao=shopInfo.GoldNum
			showCommodity()
		else
			ZyToast.show(mScene, ZyReader:readErrorMsg(), 1, 0.35)
		end
	elseif actionId==7002 then
	   if ZyReader:getResult() == eScutNetSuccess then
			exitDetail();
			ZyToast.show(mScene,Language.SHOP_BUYSUCCESS, 1, 0.35)	
    		actionLayer.Action7001(mScene,false,mCurrentTab,1,100)
		elseif ZyReader:getResult()==1 then   
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene,nil, Language.SHOP_BUZUCHONGZHI,Language.IDS_OK,Language.IDS_CANCEL,setNewManInput)  
        else	
			ZyToast.show(mScene, ZyReader:readErrorMsg(), 1, 0.35)		
		end

	end
end