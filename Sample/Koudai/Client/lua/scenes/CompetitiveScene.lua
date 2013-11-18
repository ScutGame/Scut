------------------------------------------------------------------
-- CompetitiveScene.lua
-- Author     :JUNM CHEN
-- Version    : 1.0
-- Date       :
-- Description: 竞技场
------------------------------------------------------------------
module("CompetitiveScene", package.seeall)
require("scenes.CompetitiveBattle")
require("scenes.CompetitiveStore")

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer=nil
local mTabBar=nil

local mServerData=nil
local personalInfo=nil
local mList=nil
local topten=nil
local jige=nil


--
---------------公有接口(以下两个函数固定不用改变)--------
--

-- 函数名以小写字母开始，每个函数都应注释
function  init()
	if mScene then
	return
	end
	initResource()
	local scene = ScutScene:new()
	mScene = scene.root
	-- 注册网络回调

			mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	scene:registerCallback(networkCallback)
	SlideInLReplaceScene(mScene,1)
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	-- 添加背景
	if not  mCurrentTab then
		mCurrentTab=1
	end	
	local bgLayer=createContentLayer()
	bgLayer:setPosition(PT(0,0))
	mLayer:addChild(bgLayer,0)

	----
	local titleBg=CCSprite:create(P("common/list_item_bg.9.png"))

	local tabStr={Language.COMPETI_TITLE,Language.COMPETI_GETREWARD}
	mTabBar=createTabbar(tabStr,mLayer)
	showLayer()	
	
	MainMenuLayer.init(3, mScene)	
end;

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end

---创建背景层
function  createContentLayer(type)
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
	if type then
		path="common/list_1024.png"
	end
	local midSprite=CCSprite:create(P(path))
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.76)
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)	
--	local posY=pWinSize.height-boxSize.height-titleBg:getContentSize().height
--	if type then
--		posY=pWinSize.height-boxSize.height-SY(2)
--	end
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height*0.145))
	layer:addChild(midSprite,0)
	return layer
end;

--创建tabbar
function  createTabbar(tabStr,layer)
	local tabBar=nil
	local midSprite=CCSprite:create(P("common/list_1024.png"))
	if tabStr then
	  	tabBar=ZyTabBar:new(nil,nil,tabStr,FONT_NAME,FONT_SM_SIZE,4)
		tabBar:selectItem(mCurrentTab)	
		local tabBar_X=pWinSize.width*0.04
		local tabBar_Y=pWinSize.height*0.84
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
--    mServerData=nil
    if index ~=mCurrentTab then
	    mCurrentTab=index
	    showLayer()
    end
end;

--层管理器
function  showLayer()
	 releaseCompetitiveLayer()
	 releaseRewardLayer()
	if mCurrentTab ==1 then
--		if mServerData then
--			createCompetitiveLayer(mServerData)
--		else
			sendAction(5101)
--		end
	elseif mCurrentTab==2 then
		sendAction(7011)
	--	 createRewarLayer(mServerData)
	end
end;

---释放佣兵层
function  releaseCompetitiveLayer()
	mList=nil
	if mCompetitiveLayer then
		mCompetitiveLayer:getParent():removeChild(mCompetitiveLayer,true)
		mCompetitiveLayer=nil
	end
end;

---创建佣兵层
function  createCompetitiveLayer(info)
	  releaseCompetitiveLayer()
	  local layer=CCLayer:create()
	  mLayer:addChild(layer,0)
	  mCompetitiveLayer=layer
	  layer:setAnchorPoint(PT(0,0))
	  layer:setPosition(PT(0,0))
	  local posY=mTabBar:getPosition().y  
	  -------------------标题
	
	  local titleBg=CCSprite:create(P("common/list_1052.9.png"))
		local titleSize=SZ(titleBg:getContentSize().width,pWinSize.height*0.12)
	  titleBg:setAnchorPoint(PT(0.5,0))
	  titleBg:setScaleY(titleSize.height/titleBg:getContentSize().height)
	  titleBg:setPosition(PT(pWinSize.width/2,posY-titleSize.height))
	  layer:addChild(titleBg,0)
	  
	  local startX=pWinSize.width/2-titleSize.width*0.45
	  local startY=titleBg:getPosition().y+pWinSize.height*0.11
	  local colW=titleSize.width*0.3
	  local rowH=pWinSize.height*0.1/3
	  
	  local tipLabel=CCLabelTTF:create(Language.COMPETI_TIP,FONT_NAME,FONT_SM_SIZE)
	  tipLabel:setAnchorPoint(PT(0.5,0))
	  tipLabel:setPosition(PT(pWinSize.width/2,startY-rowH))
	  layer:addChild(tipLabel,0)
	  ----
	  local secTipLabel=CCLabelTTF:create(Language.COMPETI_TIP1,FONT_NAME,FONT_SM_SIZE)
	  secTipLabel:setAnchorPoint(PT(0.5,0))
	  secTipLabel:setPosition(PT(pWinSize.width/2,tipLabel:getPosition().y-rowH))
	  layer:addChild(secTipLabel,0)
	  
	  --当前排名 
	  local str=Language.COMPETI_RANK .. ":" .. info.Ranking
	  local rankLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	  rankLabel:setAnchorPoint(PT(0,0))
	  rankLabel:setPosition(PT(startX,secTipLabel:getPosition().y-rowH))
	  layer:addChild(rankLabel,0)
	  
	  ---剩余挑战次数
	  local str=Language.COMPETI_TIMES .. ":" .. info.ChallengeNum
	  local timesLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	  timesLabel:setAnchorPoint(PT(0,0))
	  timesLabel:setPosition(PT(startX+colW,rankLabel:getPosition().y))
	  layer:addChild(timesLabel,0)  
	  --vip
	  local vipSprite = nil
	  if personalInfo._VipLv>0 then
	  	 local vipPath=string.format("button/vip_%d.png",personalInfo._VipLv)
	  	 vipSprite=CCSprite:create(P(vipPath))
		  vipSprite:setAnchorPoint(PT(0,0.5))
		  vipSprite:setPosition(PT(timesLabel:getPosition().x+timesLabel:getContentSize().width+SX(5),
		  							timesLabel:getPosition().y+timesLabel:getContentSize().height/2))
		  layer:addChild(vipSprite,0)
	  end
	  ---LIST 控件
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.5)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=titleBg:getPosition().y-listSize.height-SY(2)
	local mListRowHeight=listSize.height/4
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	list:setTouchEnabled(true)
	layer:addChild(list,1)
	mList=list
	
	if info.TopTenTabel then
		local recordTable=info.TopTenTabel 
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0	
	--	local mBgTecture=IMAGE(Image.Image_normalItemBg)
		for k, v in pairs(recordTable) do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local isTopTen = true
			local itemLayer,listHeight=createListItem(v,k, isTopTen)
			listItem:addChildItem(itemLayer, layout)
			mList:setRowHeight(listHeight)
			mList:addListItem(listItem, false)
		end
	end
	
	if info._ToUserID then
		local recordTable=info._ToUserID 
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0	
	--	local mBgTecture=IMAGE(Image.Image_normalItemBg)
		for k, v in pairs(recordTable) do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local itemLayer,listHeight=createListItem(v,k)
			listItem:addChildItem(itemLayer, layout)
			mList:setRowHeight(listHeight)
			mList:addListItem(listItem, false)
		end
	end
	
	--跳到玩家所在的项
	--list:turnToPage(2)
	
	--购买次数按钮
	local addTimeBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.COMPETI_ADD)
	addTimeBtn:setAnchorPoint(PT(0,0))
	addTimeBtn:setPosition(PT(pWinSize.width*0.5-titleSize.width*0.5+titleSize.width*0.98-addTimeBtn:getContentSize().width,timesLabel:getPosition().y))
	addTimeBtn:addto(layer,0)	
	addTimeBtn:registerScriptHandler(addTimeAction)

	if vipSprite and vipSprite:getPosition().x+vipSprite:getContentSize().width > addTimeBtn:getPosition().x then
		addTimeBtn:setPosition(PT(addTimeBtn:getPosition().x, timesLabel:getPosition().y+vipSprite:getContentSize().height))
	end

end;

function addTimeAction()
	sendAction(5104, 1)
end;

function askIsAdd(index, content, tag)
	if index == 1 then
		sendAction(5104, 2)		
	end
end;

--创建一个项目
function createListItem(info, index, isTopTen)
	local layer=CCLayer:create()
	local layerHeight=0
	local boxSize=SZ(mList:getContentSize().width,pWinSize.height*0.16)
	layerHeight=boxSize.height
	local startX=SX(8)
	local startY=boxSize.height*0.9
	local rowH=boxSize.height/4
	local colW=boxSize.width*0.25
	-----------
	local bgSprite=CCSprite:create(P("common/list_1038.9.png"))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setPosition(PT(0,0))
	layer:addChild(bgSprite,0)
	
	--玩家名字
	--可按动的文字
	local contentStr=info.UserName
	if isTopTen then
		xmlContent=string.format("<label class='CompetitiveScene.checkAction' tag='%d' >%s</label>",index,contentStr)
	else
		xmlContent=string.format("<label class='CompetitiveScene.checkAction2' tag='%d' >%s</label>",index,contentStr)
	end
	local ndMultiLabe=ZyMultiLabel:new(xmlContent,pWinSize.width*0.75,FONT_NAME,FONT_SM_SIZE,nil,nil,true)
	ndMultiLabe:setAnchorPoint(PT(0,1));
	ndMultiLabe:setPosition(PT(startX,startY-ndMultiLabe:getContentSize().height*1.2))
	ndMultiLabe:addto(layer,1)
	--透明按钮
	--[[
	local contentSize=ZyFont.stringSize(contentStr,ndMultiLabe:getContentSize().width,FONT_NAME,FONT_SM_SIZE)
	local xilianBtn00=ZyButton:new("common/tou_ming.9.png")
	xilianBtn00:setScaleX(contentSize.width/xilianBtn00:getContentSize().width)--设置缩放比例
	xilianBtn00:setScaleY(ndMultiLabe:getContentSize().height/xilianBtn00:getContentSize().height)
	if isTopTen then
		xilianBtn00:registerScriptHandler(checkAction)
	else
		xilianBtn00:registerScriptHandler(checkAction2)
	end
	xilianBtn00:setAnchorPoint(PT(0,0))--设置对其坐标
	xilianBtn00:setTag(index)
	xilianBtn00:setPosition(PT(ndMultiLabe:getPosition().x,ndMultiLabe:getPosition().y))
	xilianBtn00:addto(layer,0)
	--]]
	--[[
	--玩家名字
	local userName=CCLabelTTF:create(info.UserName,FONT_NAME,FONT_DEF_SIZE)
	userName:setAnchorPoint(PT(0,0))
	userName:setPosition(PT(startX,startY-userName:getContentSize().height*1.2))
	layer:addChild(userName,0)
	--]]
	--排名
	local rankLabel=CCLabelTTF:create(info.UserRank,FONT_NAME,FONT_SM_SIZE)
	rankLabel:setAnchorPoint(PT(0,0))
	rankLabel:setPosition(PT(startX,ndMultiLabe:getPosition().y-rowH))
	layer:addChild(rankLabel,0)
	--积分
	local sroceStr=Language.COMPETI_SORCE .. ":+" .. info.Reward
	local sorceLabel=CCLabelTTF:create(sroceStr,FONT_NAME,FONT_SM_SIZE)
	sorceLabel:setAnchorPoint(PT(0,0))
	sorceLabel:setPosition(PT(startX,rankLabel:getPosition().y-rowH))
	layer:addChild(sorceLabel,0)
	--等级
	local levelLabel=CCLabelTTF:create(string.format(Language.IDS_LVSTR,info.UserLv),FONT_NAME,FONT_SM_SIZE)
	levelLabel:setAnchorPoint(PT(0,0))
	levelLabel:setPosition(PT(boxSize.width*0.23,
								startY-levelLabel:getContentSize().height))
	layer:addChild(levelLabel,0)
	---上阵佣兵
	local soldierStr=Language.COMPETI_SOIDIER .. ":" .. string.format("%d/%d", info.embattleListCount, info.embatListCount)
	local soldierLabel=CCLabelTTF:create(soldierStr,FONT_NAME,FONT_SM_SIZE)
	soldierLabel:setAnchorPoint(PT(0,0))
	soldierLabel:setPosition(PT(boxSize.width*0.55,
								levelLabel:getPosition().y))
	layer:addChild(soldierLabel,0)
	---创建佣兵表
		---按钮
	local fun="CompetitiveScene.attackAction"
	local name=Language.COMPETI_ATTACK
	
	if info.ToUserID~=personalInfo._userID or isTopTen then
		local rows=3;
		local soidierTable=info.RecordTabel or {}
		local num = 1
--		rows=rows>#soidierTable and rows or #soidierTable;
--		if rows>#soidierTable then
--			for i=1,rows-#soidierTable do
--			        soidierTable[#soidierTable+1]={empty=true}
--			end
--		end
		local startX=levelLabel:getPosition().x
		local startY=levelLabel:getPosition().y
		for k , v in pairs(soidierTable) do
			if num < 4 then
				num = num+1
				jige=num
				if v.GeneralName then
					local BgTecture = IMAGE(getQualityBg(v.GeneralQuality, 1))
					bgSprite=CCSprite:createWithTexture(BgTecture)
					bgSprite:setAnchorPoint(PT(0,0))
					bgSprite:setPosition(PT(startX+(k-1)*(bgSprite:getContentSize().width+SX(2)),startY-bgSprite:getContentSize().height))
					layer:addChild(bgSprite,0)
					
					local heroImg = string.format("smallitem/%s.png",v.PicturesID)
					local heroSprite = CCSprite:create(P(heroImg))
					heroSprite:setAnchorPoint(PT(0.5, 0.5))
					heroSprite:setPosition(PT(bgSprite:getContentSize().width*0.5,bgSprite:getContentSize().height*0.5))
					bgSprite:addChild(heroSprite,0)
					
				end
			end
		end
	else
		fun=refreshAction
	 	name=Language.IDS_REFRESH
		local str=Language.COMPETI_CURRENTSORCE .. ":" ..  mServerData.sportsIntegral 
		local currentSorce=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
		currentSorce:setAnchorPoint(PT(0,0))
		currentSorce:setPosition(PT(levelLabel:getPosition().x,
								levelLabel:getPosition().y-rowH))
		layer:addChild(currentSorce,0)
		
		local str=string.format(Language.COMPETI_GETSORCE, info.Reward)
		local getSorce=CCLabelTTF:create(str,FONT_NAME,FONT_SMM_SIZE)
		getSorce:setAnchorPoint(PT(0,0))
		getSorce:setPosition(PT(currentSorce:getPosition().x,
								currentSorce:getPosition().y-rowH))
		layer:addChild(getSorce,0)
	end

--	local contentSize=ZyFont.stringSize(contentStr,ndMultiLabe:getContentSize().width,FONT_NAME,FONT_SM_SIZE)
	local image = CCSprite:create(P("smallitem/Icon_1000.png"))
	local actionBtn=ZyButton:new("common/tou_ming.9.png")
	actionBtn:setScaleX(image:getContentSize().width*(jige-1)*1.1/actionBtn:getContentSize().width)--设置缩放比例
	actionBtn:setScaleY(bgSprite:getContentSize().height/actionBtn:getContentSize().height)
	if isTopTen then
		actionBtn:registerScriptHandler(checkAction)
	else
		actionBtn:registerScriptHandler(checkAction2)
	end
	actionBtn:setAnchorPoint(PT(0,0))--设置对其坐标
	actionBtn:setTag(index)
	actionBtn:setPosition(PT(levelLabel:getPosition().x+SX(2),levelLabel:getPosition().y-bgSprite:getContentSize().height))
	actionBtn:addto(layer,0)
	
	local actionBtn=ZyButton:new("button/list_1039.png",nil,nil,name)
	actionBtn:setAnchorPoint(PT(0,0))
	actionBtn:setPosition(PT(boxSize.width*0.98-actionBtn:getContentSize().width,boxSize.height/2-actionBtn:getContentSize().height/2))
	if not isTopTen then
	actionBtn:addto(layer,0)	
	end
	actionBtn:setTag(index)
	actionBtn:registerScriptHandler(fun)
	
	topten=isTopTen
	
	return layer,layerHeight	
end;

--挑战按钮
function  attackAction(node)
	local tag=node:getTag()
	local soldierInfo=mServerData._ToUserID
	local itemInfo=soldierInfo[tag]
	if  itemInfo and not itemInfo.empty then
		if not isClick then
		isClick =true
	--		actionLayer.Action5107(mScene,nil,itemInfo.ToUserID)
			sendAction(5107, itemInfo.ToUserID)
		end
	--	ZyToast.show(mScene,itemInfo.UserName)
	end
	
end;

--刷新按钮
function  refreshAction()
	if not isClick then
	isClick =true
		actionLayer.Action5101(mScene,false)
	end
end;

--查看
function  checkAction(node)
	local tag=node:getTag()
	local soldierInfo=mServerData.TopTenTabel
	local playerId = mServerData.TopTenTabel[tag].ToUserID   
	HeroScene.pushScene(nil, playerId)
end;
function  checkAction2(node)
	local tag=node:getTag()
	local soldierInfo=mServerData._ToUserID
	local itemInfo=soldierInfo[tag]
	HeroScene.pushScene(nil, itemInfo.ToUserID)
end;


-----------------释放奖励层
function  releaseRewardLayer()
	mList=nil
	_sorceLabel=nil
	if mRewardLayer then
		mRewardLayer:getParent():removeChild(mRewardLayer,true)
		mRewardLayer=nil
	end
end;

function  createRewarLayer()
	releaseRewardLayer()
	local layer=CCLayer:create()
	mLayer:addChild(layer,0)
	mRewardLayer=layer
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	local posY=mTabBar:getPosition().y  
	-------------------标题
	local titleBg=CCSprite:create(P("common/list_1052.9.png"))
	local titleSize=SZ(titleBg:getContentSize().width,pWinSize.height*0.08)  
	titleBg:setAnchorPoint(PT(0.5,0))
	titleBg:setScaleY(titleSize.height/titleBg:getContentSize().height)
	titleBg:setPosition(PT(pWinSize.width/2,posY-titleSize.height))
	layer:addChild(titleBg,0)
	local rowH=titleSize.height/3
	local startX=pWinSize.width/2-titleSize.width*0.48
	local startY=titleBg:getPosition().y+titleSize.height
	local colW=titleSize.width*0.45
	
	local str=Language.COMPETI_CURRENTSORCE .. ":"
	if m_goodTable.SportsIntegral then
		str = str..m_goodTable.SportsIntegral
	end		
	local sorceLabel=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
	sorceLabel:setAnchorPoint(PT(0,0))
	sorceLabel:setPosition(PT(startX,startY-rowH*1.2))
	layer:addChild(sorceLabel,0)
	_sorceLabel = sorceLabel
	----
	local tipLabel=CCLabelTTF:create(Language.COMPETI_RETIP,FONT_NAME,FONT_SM_SIZE)
	tipLabel:setAnchorPoint(PT(0,0))
	tipLabel:setPosition(PT(startX,sorceLabel:getPosition().y-rowH))
	layer:addChild(tipLabel,0)

	  ---LIST 控件
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.5)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=titleBg:getPosition().y-listSize.height-SY(2)
	local mListRowHeight=listSize.height/3
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	layer:addChild(list,1)
	mList=list
	--
	local info = m_goodTable
	if info.RecordTabel then
		local recordTable=info.RecordTabel 
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0	
		for k, v in pairs(recordTable) do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local itemLayer,listHeight=createRewarItem(v,k)
			listItem:addChildItem(itemLayer, layout)
			mList:setRowHeight(listHeight)
			mList:addListItem(listItem, false)
		end
	end
end;

function  createRewarItem(info, index)
	local layer=CCLayer:create()
	local layerHeight=0
	local boxSize=SZ(mList:getContentSize().width,pWinSize.height*0.15)
	local startX=SX(8)
	local startY=boxSize.height*0.9
	local rowH=boxSize.height/6
	local colW=boxSize.width/5
	layerHeight=boxSize.height
	-----------
	local bgSprite=CCSprite:create(P("common/list_1038.9.png"))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setPosition(PT(0,0))
	layer:addChild(bgSprite,0)

	--物品图片
	local BgTecture = getQualityBg(info.QualityType,1)
	local goodBg=CCSprite:create(P(BgTecture))
	goodBg:setAnchorPoint(PT(0,0.5))
	goodBg:setPosition(PT(startX,boxSize.height/2))
	layer:addChild(goodBg,0)	
	
	if info.HeadID then
		local goodImg = string.format("smallitem/%s.png", info.HeadID)
		local goodSprite=CCSprite:create(P(goodImg))
		goodSprite:setAnchorPoint(PT(0.5,0.5))
		goodSprite:setPosition(PT(goodBg:getContentSize().width/2,
										goodBg:getContentSize().height/2))
		goodBg:addChild(goodSprite,0)
		
		startX=startX+goodBg:getContentSize().width+SX(10)
	
	end
	
	
	--物品名字
	if info.ItemName then
		local goodName=CCLabelTTF:create(info.ItemName ,FONT_NAME,FONT_DEF_SIZE)
		goodName:setAnchorPoint(PT(0,0))
		goodName:setPosition(PT(startX,startY-rowH*1.2))
		layer:addChild(goodName,0)
	end
	
	--兑换积分
	if info.Athletics then
		local str=Language.COMPETI_CHANGE .. ":" .. info.Athletics
		local changeNum=CCLabelTTF:create(str,FONT_NAME,FONT_SM_SIZE)
		changeNum:setAnchorPoint(PT(0,0))
		changeNum:setPosition(PT(startX,
									startY-rowH*2.2))
		layer:addChild(changeNum,0)
	end
	
	--说明文字
	if info.ItemDesc then
		local contentStr=string.format("<label>%s</label>", info.ItemDesc )
		local contentLabel=ZyMultiLabel:new(contentStr,boxSize.width*0.55,FONT_NAME,FONT_SM_SIZE);
		contentLabel:setAnchorPoint(PT(0,0))
		contentLabel:setPosition(PT(startX,
									startY-rowH*2.2-contentLabel:getContentSize().height))
		contentLabel:addto(layer,0)
	end
	
	--兑换按钮
	local changeBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.COMPETI_GET)
	changeBtn:setAnchorPoint(PT(0,0))
	changeBtn:setPosition(PT(boxSize.width*0.98-changeBtn:getContentSize().width,
								boxSize.height/2-changeBtn:getContentSize().height/2))
	changeBtn:addto(layer,0)	
	changeBtn:setTag(index)
	changeBtn:registerScriptHandler(getRewardAction)
	return layer, layerHeight
end;

--兑换按钮响应
function  getRewardAction(node)
	local tag=node:getTag()
	local itemInfo=m_goodTable.RecordTabel[tag]
--	if  itemInfo and not itemInfo.empty then
--		if not isClick then
--			isClick =true
--			sendAction(7012,itemInfo.ItemID )
--		end
--	end
	CompetitiveStore.setInfo(itemInfo, mScene)
	CompetitiveStore.numChoice()
end;

---------------------------------------




function refreshWin()
	showLayer()
end

--------------------------------------
--发送请求
function sendAction(actionID, data)
	if actionID == 5101 then
		actionLayer.Action5101(mScene,false)
	elseif actionID == 5104 then
		local ops = data 
		actionLayer.Action5104(mScene,false, ops)		
	elseif actionID == 5107 then
		local ToUserID = data 
		actionLayer.Action5107(mScene,false, ToUserID)
	elseif actionID == 7011 then
		actionLayer.Action7011(mScene,false,1,199)
	elseif actionID == 7012 then
		local ItemId = data 
		actionLayer.Action7012(mScene, nil, ItemId, 1 )	
	end
end


-- 退出场景
function closeScene()
	releaseRewardLayer()
	releaseCompetitiveLayer()
	releaseResource()
end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	personalInfo=PersonalInfo.getPersonalInfo()
end

-- 释放资源
function releaseResource()
	mList=nil
	mServerData=nil
	mCompetitiveLayer=nil
	mRewardLayer=nil
	mLayer=nil
	mScene=nil
	isClick=nil
	_sorceLabel=nil
end


-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionID==5101 then
		local serverData=actionLayer._5101Callback(pScutScene, lpExternalData)
		if serverData then
			mServerData=serverData
			 createCompetitiveLayer(serverData)
		end
	elseif actionID == 5104 then
		if ZyReader:getResult() == 2 then	
			sendAction(5101)
			MainMenuLayer.refreshWin()
		elseif ZyReader:getResult() == 1 then
			local str = string.format(Language.COMPETI_ADDTIMESMSG, ZyReader:readErrorMsg())
			local box = ZyMessageBoxEx:new()
			box:doQuery(pScutScene, Language.TIP_TIP, str, Language.IDS_SURE, Language.IDS_CANCEL, askIsAdd)
		end			
	elseif  actionID==5107 then
	--战斗数据返回
		local attackData=actionLayer._5107Callback(pScutScene, lpExternalData)
		if attackData then
			local battleType = 1
			CompetitiveBattle.setBattleInfo(attackData, mScene, battleType)
			CompetitiveBattle.pushScene()
		end
	elseif actionID == 7011 then
		local serverData=actionLayer._7011Callback(pScutScene, lpExternalData)
		if serverData then
			m_goodTable = serverData
			createRewarLayer()
		end
	elseif actionID == 7012 then
		local serverData=actionLayer._7012Callback(pScutScene, lpExternalData)
		if serverData then
			ZyToast.show(pScutScene, Language.COMPETI_EXCHAGNESUCCESS,1.5,0.35)
			local str = Language.COMPETI_CURRENTSORCE .. ":"..serverData.SportsIntegral 
			_sorceLabel:setString(str)
		end
	elseif actionID == 1401 then
		local serverInfo = actionLayer._1401Callback(pScutScene, lpExternalData)	
		if   serverInfo ~= nil then 
			local headTabel = serverInfo.RecordTabel
			HeroScene.createScene()
			HeroScene.showHeroHead(headTabel)
		end		
	elseif  actionID == 1403 then 
		local serverInfo = actionLayer._1403Callback(pScutScene, lpExternalData)	
		if   serverInfo ~= nil  and    serverInfo ~= {}   then 
			local headTabel = serverInfo.RecordTabel
			HeroScene.createScene()
			HeroScene.showHeroHead(headTabel)
		end
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)
	end
	isClick=false
end






