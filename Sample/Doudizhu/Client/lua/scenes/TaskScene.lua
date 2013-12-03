
-- TaskScene.lua.lua
-- Author     :Lysong
-- Version    : 1.0.0.0
-- Date       :
-- Description:
------------------------------------------------------------------

module("TaskScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local layoutLayer=nil;--通用层
local tasktype=nil
local nowpage=nil
local pagemax=nil
local list1={}
local jindou=nil
--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释

-- 场景入口
function pushScene()
	initResource()
	createScene()
	CCDirector:sharedDirector():pushScene(_scene)
end
-- 退出场景
function popScene()
	releaseResource()
	CCDirector:sharedDirector():popScene()
end
function lingquAction(node)
	
	local tag=node
	jindou=list1[tag].GameCoin
	actionLayer.Action3004(mScene,nil,list1[tag].TaskID)
	
end;
function closeBtnActon()
    releaseResource()--释放资源
    SlideInRPopScene()
    if mLayer then
        mScene:removeChild(mLayer,true);
        mLayer=nil;
    end
	mScene=nil
end
--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	
end
function move_list(addIndex)
	
	nowpage=addIndex+1
	actionLayer.Action3003(mScene,nil,tasktype,nowpage,pagesize)
	
end
-- 释放资源
function releaseResource()
    if rankList then
        rankList:clear();
        rankList=nil;
    end
    removewu()
--    releaseLayer()--释放掉层
end
-- 创建场景
function createScene()
	initResource()
	local scene = ScutScene:new()
	-- 注册网络回调
	scene:registerCallback(networkCallback)
    mScene = scene.root
	-- 此处添加场景初始内容
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	----
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
	----
	local bgLayer = UIHelper.createUIBg(nil,nil,ZyColor:colorWhite(),nil,true);--没有背景框
	mLayer:addChild(bgLayer);	
	
	--返回按钮
	local closeBtn=ZyButton:new(P(Image.image_exit));
	closeBtn:setAnchorPoint(PT(1,0.5));
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width/2,pWinSize.height-closeBtn:getContentSize().height*0.8));
	closeBtn:registerScriptTapHandler(closeBtnActon);
	closeBtn:addto(mLayer);

--	SlideInLPushScene(mScene)
	
	local boxSize=SZ(pWinSize.width*0.88,pWinSize.height*0.71)
       local start_x=pWinSize.width*0.1
	local start_y=pWinSize.height*0.1
	
	rankList=ScutCxList:node(boxSize.height ,ccc4(25, 25, 25, 25),boxSize)
	mLayer:addChild(rankList,0)
	rankList:setAnchorPoint(PT(0,0))
	rankList:setPosition(PT(start_x*0.5,start_y*1))
--	rankList:setPageTurnEffect(true)--是否翻页
--	rankList:setRecodeNumPerPage(1)--设置页码
	rankList:setTouchEnabled(true)
	rankList:registerLoadEvent(move_list); --列表移动事件
--	rankList:registerItemClickListener("TaskScene.chooseid")
	rankList:setHorizontal(false)
	
	tasktype=1
	nowpage=1
	pagesize=2
	local tabName={"title/panle_1064.png","title/panle_1065.png"}
	createTabBar(tabName);
	actionLayer.Action3003(mScene,nil,tasktype,nowpage,pagesize)
--	refreshList()
    
	SlideInLPushScene(mScene)
	
end



function createTabBar(tabName)
	tabBar = ZyTabBar:new(Image.image_shop,Image.image_shop_1,tabName, FONT_NAME, FONT_SM_SIZE,2);
	tabBar:addto(mLayer)
	tabBar:setCallbackFun(callbackTabBar);  --点击响应的事件
	if not mCurrentTab then
		mCurrentTab=1
	end
	tabBar:selectItem(mCurrentTab);           --点击哪个按钮
	tabBar:setPosition(PT(SX(23),pWinSize.height*0.86-SY(1)))   
end
--tabbar
function callbackTabBar(bar,pNode,tag)
	local index=pNode:getTag();
	tasktype=index
	nowpage=1
	if index==1 then--成长任务
		removewu()
		actionLayer.Action3003(mScene,nil,tasktype,nowpage,pagesize)
--		refreshList()
	elseif index==2 then--日常任务
		removewu()
		actionLayer.Action3003(mScene,nil,tasktype,nowpage,pagesize)
	end
end

function  refreshList()
	if rankList then
		rankList:clear()
		for i=1,pagemax do
			listItem=ScutCxListItem:itemWithColor(ccc3(42,28,13))
			listItem:setOpacity(0);
			listItem:setMargin(CCSize(0,0));
			rankList:addListItem(listItem, false);
		end
		local chay=(rankList:getContentSize().height/2-pWinSize.height*0.34)/2
		local mlayout = CxLayout()
		mlayout.val_x.t = ABS_WITH_PIXEL
		mlayout.val_y.t = ABS_WITH_PIXEL
		mlayout.val_x.val.pixel_val = 0
		mlayout.val_y.val.pixel_val = 0
		local layer = CCLayer:create();
		for k, v in pairs(list1) do
			local beijing= CCSprite:create(P("common/panel_1002_2.9.png"));
			beijing:setScaleX(pWinSize.width*0.8/beijing:getContentSize().width)
			beijing:setScaleY(pWinSize.height*0.34/beijing:getContentSize().height);
			beijing:setAnchorPoint(PT(0,1))
			beijing:setPosition(PT((rankList:getContentSize().width-pWinSize.width*0.8)/2,rankList:getContentSize().height-chay-(k-1)*(pWinSize.height*0.34+1.5*chay)))
			layer:addChild(beijing,0)
			
			--下面的框
			local kuang1= CCSprite:create(P("common/panle_1030_2.9.png"));
			kuang1:setScaleX(pWinSize.width*0.57/kuang1:getContentSize().width);
			kuang1:setAnchorPoint(PT(0,0))
			kuang1:setPosition(PT(pWinSize.width*0.07,beijing:getPosition().y-pWinSize.height*0.34+SY(5)))
			layer:addChild(kuang1,0)
			--任务奖励
			local titleName1=CCLabelTTF:create(Language.JIANGLI..list1[k].GameCoin..Language.JINDOU, FONT_NAME, FONT_SMM_SIZE)
		  	titleName1:setAnchorPoint(PT(0,0))
		  	titleName1:setPosition(PT(pWinSize.width*0.1,kuang1:getPosition().y+(kuang1:getContentSize().height-titleName1:getContentSize().height)/2))
		  	layer:addChild(titleName1,0)
			--上面的框
			local kuang2= CCSprite:create(P("common/panle_1030_2.9.png"));
			kuang2:setScaleX(pWinSize.width*0.57/kuang2:getContentSize().width);
			kuang2:setAnchorPoint(PT(0,0))
			kuang2:setPosition(PT(pWinSize.width*0.07,kuang1:getPosition().y+kuang1:getContentSize().height+SY(5)))
			layer:addChild(kuang2,0)
			--任务名称  进度
			local titleName1=CCLabelTTF:create(Language.TASKNAME..list1[k].TaskName.."  "..list1[k].CompleteNum.."/"..list1[k].TaskNum, FONT_NAME, FONT_SM_SIZE)
		  	titleName1:setAnchorPoint(PT(0,0))
		  	titleName1:setPosition(PT(pWinSize.width*0.1,kuang2:getPosition().y+kuang2:getContentSize().height+SY(3)))
		  	layer:addChild(titleName1,0)
		  	--任务描述
			local titleName1=CCLabelTTF:create(list1[k].TaskDesc, FONT_NAME, FONT_SMM_SIZE)
		  	titleName1:setAnchorPoint(PT(0,0))
		  	titleName1:setPosition(PT(pWinSize.width*0.1,kuang2:getPosition().y+(kuang2:getContentSize().height-titleName1:getContentSize().height)/2))
		  	layer:addChild(titleName1,0)
			--]]
			local yuan= CCSprite:create(P("common/panle_1030_1.png"));
			yuan:setAnchorPoint(PT(0,0.5))
			yuan:setPosition(PT(beijing:getPosition().x+pWinSize.width*0.8-yuan:getContentSize().width-SX(5),beijing:getPosition().y-pWinSize.height*0.34/2))
			layer:addChild(yuan,0)
			
			if list1[k].IsReceive==1 then
				islingqu=Language.WEIWANCHENG
				lingqu=ZyButton:new("button/panle_1026_2.png","button/panle_1026_2.png",nil,islingqu,FONT_NAME,FONT_SM_SIZE)
			elseif list1[k].IsReceive==2 then
				islingqu=Language.LINGQU
				lingqu=ZyButton:new("button/panle_1026_2.png","button/panle_1026_3.png",nil,islingqu,FONT_NAME,FONT_SM_SIZE)
			elseif list1[k].IsReceive==3 then
				islingqu=Language.YILINGQU
				lingqu=ZyButton:new("button/panle_1026_2.png","button/panle_1026_2.png",nil,islingqu,FONT_NAME,FONT_SM_SIZE)
			end
--			local lingqu=ZyButton:new("button/panle_1026_2.png","button/panle_1026_3.png",nil,islingqu,FONT_NAME,FONT_SM_SIZE)
			lingqu:setAnchorPoint(PT(0,0));
			lingqu:setPosition(PT(yuan:getPosition().x+(yuan:getContentSize().width-lingqu:getContentSize().width)/2,yuan:getPosition().y-yuan:getContentSize().height/2+(yuan:getContentSize().height-lingqu:getContentSize().height)/2));
			lingqu:setTag(k)
			if list1[k].IsReceive==2 then
				lingqu:registerScriptTapHandler(lingquAction);
			end
			lingqu:addto(layer);
		end
	rankList:getChild(nowpage-1):removeAllChildrenWithCleanup(true)
	rankList:getChild(nowpage-1):addChildItem(layer,mlayout)	

	end
	
end



function removewu()
    if wuLayer ~= nil then
        wuLayer:getParent():removeChild(wuLayer,true)
        wuLayer = nil
    end
end

function initwu()
    removewu()
    wuLayer=CCLayer:create()
    wuLayer:setAnchorPoint(PT(0,0));
    wuLayer:setPosition(PT(0,0));
    mLayer:addChild(wuLayer,0)
end

function taskfull()
	
	--暂无任务
	initwu()
	local titleName1=CCLabelTTF:create(Language.ZANWURENWU, FONT_NAME, FONT_SM_SIZE)
	titleName1:setAnchorPoint(PT(0,0))
	titleName1:setPosition(PT((pWinSize.width-titleName1:getContentSize().width)/2,(pWinSize.height-titleName1:getContentSize().height)/2))
	wuLayer:addChild(titleName1,0)
	
end;



-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ZyRequestParam:getParamData(lpExternalData)
	
	if actionID ==3003 then
		local serverInfo=actionLayer._3003Callback(pScutScene, lpExternalData)
 		if serverInfo~=nil then
			pagemax=serverInfo.PageCount
			list1 = serverInfo.RecordTabel
		end;
		if #list1>0 then
			refreshList()
		else
			taskfull()
		end
	elseif actionID==3004 then
		if ZyReader:getResult()== eScutNetSuccess then
			rankList:turnToPage(nowpage-1)
			ZyToast.show(mScene,Language.HUODE_1..jindou..Language.HUODE_2)
		else
			ZyToast.show(mScene,ZyReader:readErrorMsg())
		end
	end
	
end
