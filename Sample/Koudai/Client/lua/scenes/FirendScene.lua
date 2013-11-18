--
-- FireScutScene.lua
-- Author     : JunMing Chen
-- Version    : 1.1.0.0
-- Date       : 2013-3-8
-- Description:
--

module("FireScutScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer = nil 		
local mCurrentTab=nil
local mInputEdit=nil
local mInputLayer=nil
local topHeight=nil
local mFriendCache={}
local mChoiceIndex=nil

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
	local scene  = ScutScene:new()
	mScene = scene.root
	-- 注册网络回调
	scene:registerCallback(networkCallback)
		mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	SlideInLReplaceScene(mScene,1)
	-- 添加背景
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	if not mCurrentTab then
		mCurrentTab=1
	end
	local topSprite=CCSprite:create(P("mainUI/list_1000.png"))
	topHeight=topSprite:getContentSize().height
	
	local bgLayer=createBgSprite()
	bgLayer:setPosition(PT(0,0))
	mLayer:addChild(bgLayer,0)
	

 	createContentLayer()
 	MainMenuLayer.init(1, mScene)
end


function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


--创建通用层
function releaseContentLayer()
	releaseNoLabel()
	if mContentLayer then
		mContentLayer:getParent():removeChild(mContentLayer,true)
		mContentLayer=nil
	end
end;

function  createContentLayer()
	local layer=CCLayer:create()
	mContentLayer=layer
	mLayer:addChild(layer,0)
---
	local tabStr={Language.FIREND_TITLE,Language.FIREND_ENEMY,Language.FIREND_CARE}
	mTabBar=createTabbar(tabStr,layer)
	
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.52)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=mTabBar:getPosition().y-listSize.height-SY(2)
	local mListRowHeight=pWinSize.height*0.08
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setTouchEnabled(true)
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	layer:addChild(list,0)
	mList=list
	m_listTable={}
	m_listTable.currentPage=1
	m_listTable.gotoPage=1
	m_listTable.maxPage=1
	m_listTable.PageSize=100

	local addFirendBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.FIREND_ADD)
	addFirendBtn:setAnchorPoint(PT(0,0))
	addFirendBtn:setPosition(PT(pWinSize.width*0.95-addFirendBtn:getContentSize().width,
								mTabBar:getPosition().y))
	addFirendBtn:addto(layer,0)
	addFirendBtn:registerScriptHandler(addFriendAction)
	showLayer()
end;

function addFriendAction()
    if not isClick then
    isClick=true
    actionLayer.Action9102(mScene,nil,1,10)
    end
end

--添加好友界面
function  createAddFriendLayer()
	local layer=CCLayer:create()
	mContentLayer=layer
	mLayer:addChild(layer,0)	
---
	local tabStr={Language.FIREND_ADD}
	mTabBar=createTabbar(tabStr,layer)
	
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.52)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=mTabBar:getPosition().y-listSize.height-SY(2)
	local mListRowHeight=pWinSize.height*0.08
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setTouchEnabled(true)
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	layer:addChild(list,0)
	mList=list
	
	local backBtn=ZyButton:new("button/list_1039.png",nil,nil,Language.IDS_BACK)
	backBtn:setAnchorPoint(PT(0,0))
	backBtn:setPosition(PT(pWinSize.width*0.95-backBtn:getContentSize().width,
								mTabBar:getPosition().y))
	backBtn:addto(layer,0)
	backBtn:registerScriptHandler(backToFirend)
	
	if #mServerData>0 then
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
		for k, v in pairs(mServerData) do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local itemLayer=createListItem(v,k,4)
			listItem:addChildItem(itemLayer, layout)
			mList:addListItem(listItem, false)
		end
	else
			 createNoLabel(4)
	end

end;

function backToFirend()
	mServerData=nil
	releaseContentLayer()
	createContentLayer()
end;

---创建背景层
function  createBgSprite()
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
--
--创建tabbar
function  createTabbar(tabStr,layer)
	local tabBar=nil
	local midSprite=CCSprite:create(P("common/list_1024.png"))
	if tabStr then
	  	tabBar=ZyTabBar:new(nil,nil,tabStr,FONT_NAME,FONT_SM_SIZE,3)
		tabBar:selectItem(mCurrentTab)	
		local tabBar_X=pWinSize.width*0.04
		local tabBar_Y=pWinSize.height*0.77
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
    	    m_listTable.gotoPage=1
	    mCurrentTab=index
	    showLayer()
    end
end;

--list翻页
function gotoListPage(page)
	if not isClick then
		isClick=true
    		actionLayer.Action9101(mScene,false,mCurrentTab,m_listTable.gotoPage,m_listTable.PageSize)
    	end
end

function rightAction()
m_listTable.gotoPage=m_listTable.currentPage+1
gotoListPage(m_listTable.gotoPage)
end;

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

--------没有好友提示
function  releaseNoLabel()
	if mNoneLabel then
		mNoneLabel:getParent():removeChild(mNoneLabel,true)
		mNoneLabel=nil
	end
end;
---
function  createNoLabel(type)
	releaseNoLabel()
	local friendStr={
	[1]=Language.FIREND_NONE1,[2]=Language.FIREND_NONE2,
	[3]=Language.FIREND_NONE3,[4]=Language.FIREND_NONE4,}
	local noneLabel=CCLabelTTF:create(friendStr[type],FONT_NAME,FONT_DEF_SIZE)
	mNoneLabel=noneLabel
	noneLabel:setAnchorPoint(PT(0.5,0.5))
	noneLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.5))
	mContentLayer:addChild(noneLabel,2)
end;


function inviteAction(node)
	local tag=node:getTag()
	if mServerData[tag] then
	if not isClick then
	isClick=true
	mChoiceIndex=tag
	--if mServerData[tag].FriendType==2 then
	--		local box = ZyMessageBoxEx:new()
	--		box:doQuery(pScutScene,nil, Language.FIREND_FIGHT2, Language.IDS_SURE, Language.IDS_CANCEL, askIsAdd)
	--else
		actionLayer.Action9103(mScene,nil,mServerData[tag].FriendID,mServerData[tag].FriendName)
	--end
	end
	end
end;


function askIsAdd(clickedButtonIndex,content,tag)
	if clickedButtonIndex == 1 then--确认
		actionLayer.Action9103(mScene,nil,mServerData[mChoiceIndex].FriendID,mServerData[mChoiceIndex].FriendName)
	end
end;

--层管理器
function showLayer()
	releaseNoLabel()
	actionLayer.Action9101(mScene,false,mCurrentTab,m_listTable.gotoPage,m_listTable.PageSize)
	--[[
	if  not mFriendCache[mCurrentTab] then
	actionLayer.Action9101(mScene,false,mCurrentTab,m_listTable.gotoPage,m_listTable.PageSize)
	else
	mServerData=mFriendCache[mCurrentTab]
	refreshMailList()
	end
	--]]
end

--刷新list
function  refreshMailList()
	if mList and mServerData then
		mList:clear()
	--	refreshPage()
		if #mServerData>0 then
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
		for k, v in pairs(mServerData) do
		 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local itemLayer=createListItem(v,k,mCurrentTab)
			listItem:addChildItem(itemLayer, layout)
			mList:addListItem(listItem, false)
		end
		else
			 createNoLabel(mCurrentTab)
		end
	end
end;

--创建单个listItem
function createListItem(info,index,friendType)
	local layer=CCLayer:create()
	
	

	local boxSize=SZ(mList:getContentSize().width,mList:getRowHeight())
	local startX=SX(5)
	local startY=boxSize.height*0.9
	
	-----------
	local bgSprite=CCSprite:create(P("common/list_1038.9.png"))
	bgSprite:setScaleX(boxSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(boxSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setPosition(PT(0,0))
	layer:addChild(bgSprite,0)

	------玩家名字
	local userName=CCLabelTTF:create(info.FriendName or "",FONT_NAME,FONT_DEF_SIZE)
	userName:setAnchorPoint(PT(0,0.5))
	userName:setPosition(PT(startX,boxSize.height/2))
	layer:addChild(userName,0)
	
	--等级 
	local lvLabel=CCLabelTTF:create(string.format(Language.IDS_LVSTR,info.FriendLv or 0),
											FONT_NAME,FONT_DEF_SIZE)
	lvLabel:setAnchorPoint(PT(0,0.5))
	lvLabel:setPosition(PT(startX+boxSize.width*0.3,
							userName:getPosition().y))
	layer:addChild(lvLabel,0)
	
	if  friendType ==2 then 
		local tipLabel=CCLabelTTF:create(Language.FIREND_FIGHT1,FONT_NAME,FONT_SM_SIZE)
		tipLabel:setColor(ccRED)
		tipLabel:setAnchorPoint(PT(0,0.5))
		tipLabel:setPosition(PT(lvLabel:getPosition().x+boxSize.width*0.2,
							userName:getPosition().y))
		layer:addChild(tipLabel,0)
	end
	local btnStr=Language.FIREND_MESSAGE
	local fun=messageAction
	actionFun=FireScutScene.selectAction
	if friendType ==4 then
		btnStr=Language.FIREND_INVITE
		fun=inviteAction
		actionFun=nil
	elseif  friendType ==2 then
		btnStr=Language.FIREND_FIGHT
		fun=gotoCompetive
	elseif  friendType ==3 then
		btnStr=nil
	end
	
	if actionFun then
		local actionBtn=UIHelper.createActionRect(SZ(boxSize.width*0.7,boxSize.height),actionFun,index)
		actionBtn:setPosition(PT(0,0))
		layer:addChild(actionBtn,0)
	end
	
	---按钮
	if btnStr then
		local messageBtn=ZyButton:new("button/list_1039.png",nil,nil,btnStr)
		messageBtn:setAnchorPoint(PT(0,0))
		messageBtn:setPosition(PT(boxSize.width*0.95-messageBtn:getContentSize().width,
						lvLabel:getPosition().y-messageBtn:getContentSize().height/2))
		messageBtn:addto(layer,0)
		messageBtn:setTag(index)
		messageBtn:registerScriptHandler(fun)
		info.actionBtn=messageBtn
	end
	--

	return layer
end;

--前往竞技场
function gotoCompetive(node)
	local tag=node:getTag()
	if mServerData[tag] then
		actionLayer.Action5107(mScene,nil ,mServerData[tag].FriendID)
	end
end;

--选择一条信息 
function selectAction(node)
	local tag=node
	if mServerData[tag] then
		 createOprationLayer(tag)
	end
end;

---释放操作界面
function releaseOprationLayer()
	if mOprationLayer then
		mOprationLayer:getParent():removeChild(mOprationLayer,true)
		mOprationLayer=nil
	end
end;

--创建操作界面 
function  createOprationLayer(index)
	 releaseOprationLayer()
	 local layer=CCLayer:create()
	 mOprationLayer=layer
	 mScene:addChild(layer,2)
	for k=1 ,2 do
		local pingBiBtn=UIHelper.createActionRect(pWinSize)
		pingBiBtn:setPosition(PT(0,0))
		layer:addChild(pingBiBtn,0)
	end
	local blackSprite=CCSprite:create(P("common/transparentBg.png"))
	blackSprite:setScaleX(pWinSize.width/blackSprite:getContentSize().width)
	blackSprite:setScaleY(pWinSize.height/blackSprite:getContentSize().height)
	blackSprite:setAnchorPoint(PT(0.5,0.5))
	blackSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(blackSprite,0)
	
	
	local midSprite=CCSprite:create(P("common/list_1054.png"))
	local bgSize=midSprite:getContentSize()
	midSprite:setAnchorPoint(PT(0.5,0.5))
	midSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(midSprite,0)
	
	local boxSize=SZ(bgSize.width,bgSize.height*0.8)
	local btnBg=CCSprite:create(P("common/list_1052.9.png"))
	btnBg:setScaleY(boxSize.height/btnBg:getContentSize().height)
	btnBg:setAnchorPoint(PT(0,0.5))
	btnBg:setPosition(PT((pWinSize.width-btnBg:getContentSize().width)/2,pWinSize.height/2-bgSize.height*0.08))
	layer:addChild(btnBg,0)
	
	
	local rowH=bgSize.height/5
	local startY=pWinSize.height/2+bgSize.height/2-rowH*1.5
	local deleteStr=Language.IDS_DELETE
	if mCurrentTab==2 then
		deleteStr=Language.FIREND_CANVEL2
	elseif  mCurrentTab==3 then
		deleteStr=Language.FIREND_CANVEL3
	end
	local btnsTable={{name=deleteStr,fun=deleteAction},
	{name=Language.IDS_CHECK,fun=checkAction},
	{name=Language.FIREND_MESSAGE,fun=messageAction},
	{name=Language.IDS_COLSE,fun=releaseOprationLayer},}
	for k, v in pairs(btnsTable) do
		local actionBtn=ZyButton:new("button/list_1023.png",nil,nil,v.name)
		actionBtn:setAnchorPoint(PT(0,0))
		actionBtn:setPosition(PT(pWinSize.width*0.5-actionBtn:getContentSize().width/2,
						startY-(k-1)*rowH-actionBtn:getContentSize().height/2))
		actionBtn:addto(layer,0)	
		actionBtn:setTag(index)
		actionBtn:registerScriptHandler(v.fun)
	end
end;

---删除按钮
function deleteAction(node)
	local tag=node:getTag()
	if mServerData[tag] then
		if not isClick then
		isClick=true
		actionLayer.Action9104(mScene,nil ,mServerData[tag].FriendID)
		end
	end
end;

--查看玩家
function checkAction(node)
	local tag=node:getTag()
	if mServerData[tag] then
		releaseOprationLayer()

		local playerId = mServerData[tag].FriendID 
		HeroScene.pushScene(nil, playerId)

	end
end;

--留言按钮
function messageAction(node)
	local tag=node:getTag()
	
	if mServerData[tag] then
		mChoiceIndex=tag
		releaseOprationLayer()
		createInputLayer(mServerData[tag])
	end
end;

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
function  createInputLayer(info)
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
	local str= string.format(Language.FIREND_MESSAGE1,info.FriendName)
	local titleTip=CCLabelTTF:create(str,FONT_NAME,FONT_DEF_SIZE)
	titleTip:setAnchorPoint(PT(0.5,0))
	titleTip:setPosition(PT(pWinSize.width/2,startY-titleTip:getContentSize().height*1.5))
	layer:addChild(titleTip,0)	

	--输入框
	local editSize=SZ(bgSize.width*0.8, bgSize.height*0.35)
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
	sureBtn:registerScriptHandler(sendMessageAction)
end;

---确定发送按钮
function sendMessageAction()
	local content=mInputEdit:GetEditText()
	if content and string.len(content)>0 then
		if not isClick then
		isClick=true
		local friendInfo=mServerData[mChoiceIndex]
		local mailTitle = string.format(Language.MAILL_LEAVESTRING,friendInfo.FriendName)
		actionLayer.Action9301(mScene,nil,mailTitle,content,friendInfo.UserID,friendInfo.FriendName)
 		releaseInputLayer()		
		end
	end
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

-- 初始化资源、成员变量
function initResource()

end

-- 释放资源
function releaseResource()
 	mScene = nil 		-- 场景
 	mLayer = nil 	
	mNoneLabel=nil
	mInputLayer=nil
	mList=nil	
	mFriendCache={}
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	local userData = ScutRequestParam:getParamData(lpExternalData)
	if actionID==9101 then
		local serverData=actionLayer._9101Callback(pScutScene,lpExternalDate)
		if serverData then
		mServerData=serverData.RecordTabel
		if serverData.PageCount>0 then
				m_listTable.maxPage=serverData.PageCount
		end
		mFriendCache[userData]={}	
		mFriendCache[userData]=mServerData
		m_listTable.currentPage=m_listTable.gotoPage
		refreshMailList()
		end
	elseif actionID==9104 then
		    if ZyReader:getResult() == eScutNetSuccess then
		    		releaseOprationLayer()
		    		ZyToast.show(pScutScene,Language.FIREND_DELETET)
    				actionLayer.Action9101(pScutScene,false,mCurrentTab,m_listTable.gotoPage,m_listTable.PageSize)
		    else
		    		ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
		    end
	elseif actionID==9102 then
		local serverData=actionLayer._9102Callback(pScutScene,lpExternalDate)
		if serverData.RecordTabel then
			mServerData=serverData.RecordTabel
			 releaseContentLayer()
			 createAddFriendLayer()
		end
	elseif actionID==9103 then
		    if ZyReader:getResult() == eScutNetSuccess then	
		    		if mServerData[mChoiceIndex].actionBtn then
		    			mServerData[mChoiceIndex].actionBtn:setVisible(false)
		    		end
				mFriendCache={}	
		    else
				ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
		    end
		    mChoiceIndex=nil
	elseif  actionID==9301 then
		    if ZyReader:getResult() == eScutNetSuccess then    		
       			ZyToast.show(pScutScene,Language.MAILL_goLetterStr,1,0.5)
		    else
				ZyToast.show(pScutScene,ZyReader:readErrorMsg(),1,0.35)
		    end
	elseif actionID == 5107 then
		local serverInfo=actionLayer._5107Callback(pScutScene, lpExternalData)
		if  serverInfo ~= nil then
			local battleType = 4
			CompetitiveBattle.setBattleInfo(serverInfo, mScene, battleType)
			CompetitiveBattle.pushScene()					
		end		
	else
		commonCallback.networkCallback(pScutScene, lpExternalData)	
	end
	isClick=false

end

