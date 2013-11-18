------------------------------------------------------------------
-- GameSettingScene.lua
-- Author     :
-- Version    : 1.0
-- Date       :
-- Description: ,
------------------------------------------------------------------
module("GameSettingScene", package.seeall)

-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer=nil
local mSettingLayer=nil
local mSetState={}
local stateStr={[0]=Language.IDS_OPEN ,[1]=Language.IDS_COLSE}
----------
local mHelperLayer=nil
local mList=nil

local selecetBtnTable={}
local mPhyChoiceIndex=nil
local mBeAttackIndex=nil
local mInputEdit=nil 
function closeScene()
	releaseResource()
end

--初始化场景入口
function  init()
	if mScene then
		return
	end
	initResource()
	
	local scene = ScutScene:new()
	mScene = scene.root 
	mScene:registerScriptHandler(SpriteEase_onEnterOrExit)
	scene:registerCallback(networkCallback)
	SlideInLReplaceScene(mScene,1)
	
	mLayer = CCLayer:create()
	mScene:addChild(mLayer, 0)
	-- 添加背景
	if not  mCurrentTab then
		mCurrentTab=1
	end	

	showLayer()
	MainMenuLayer.init(3, mScene)
end;

function SpriteEase_onEnterOrExit (tag)
    if tag == "exit" then
        releaseResource()
    end
end


function onEnter()
releaseResource()

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
	if not type then
	layer:addChild(bgSprite,0)
	end
	--中间层
	local boxSize=SZ(pWinSize.width,pWinSize.height*0.76)
	local path="common/list_1043.png"
	if type then
		path="common/list_1024.png"
		 boxSize=SZ(pWinSize.width,pWinSize.height*0.855)
	end
	local midSprite=CCSprite:create(P(path))
	
	midSprite:setAnchorPoint(PT(0.5,0))
	midSprite:setScaleX(boxSize.width/midSprite:getContentSize().width)
	midSprite:setScaleY(boxSize.height/midSprite:getContentSize().height)
	local posY=pWinSize.height*0.145
	midSprite:setPosition(PT(pWinSize.width/2,posY))
	layer:addChild(midSprite,0)
	return layer
end;

--创建tabbar
function  createTabbar(tabStr,layer,posY)
	local tabBar=nil
	local midSprite=CCSprite:create(P("common/list_1024.png"))
	if tabStr then
		local titleBg=CCSprite:create(P("common/list_1047.png"))
	  	local titleSprite=CCSprite:create(P("common/list_1041.png"))
		titleSprite:setAnchorPoint(PT(0,0))
		tabBar=titleSprite
		local tabBar_X=pWinSize.width*0.04
		local tabBar_Y=pWinSize.height*0.84
		if posY then
			tabBar_Y=posY-tabBar:getContentSize().height-SY(4)
		end
		titleSprite:setPosition(PT(tabBar_X,tabBar_Y))
		layer:addChild(titleSprite,1)
		--
		local label=CCLabelTTF:create(tabStr[1],FONT_NAME,FONT_DEF_SIZE)
		label:setAnchorPoint(PT(0.5,0.5))
		label:setPosition(PT(tabBar_X+titleSprite:getContentSize().width/2,
								tabBar_Y+titleSprite:getContentSize().height/2))
		layer:addChild(label,1)
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

--层级管理器
function  showLayer()
	if mCurrentTab==1 then
		 createSettingLayer()
	end
end;

----释放设置层
function  releaseSettingLayer()
	mTabBar=nil
	mList=nil
	if mSettingLayer then
		mSettingLayer:getParent():removeChild(mSettingLayer,true)
		mSettingLayer=nil
	end
end;

--创建设置层
function  createSettingLayer()
	local layer=CCLayer:create()
	mSettingLayer=layer
	mLayer:addChild(layer,0)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
----
	local boxSize=SZ(pWinSize.width*0.9,pWinSize.height*0.65)
	local rowH=boxSize.height/8
	local titleBg=CCSprite:create(P("common/list_item_bg.9.png"))

	local bgLayer=createContentLayer()
	bgLayer:setPosition(PT(0,0))
	layer:addChild(bgLayer,0)

	-- 此处添加场景初始内容
	local titleName={Language.GAME_SET}
	mTabBar=createTabbar(titleName,layer)
	  ---LIST 控件
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.6)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=pWinSize.height*0.22--mTabBar:getPosition().y-listSize.height-SY(2)
	local mListRowHeight=rowH
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setTouchEnabled(true)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	layer:addChild(list,1)
	mList=list
	
	 mSetState={}
	 local musicState=accountInfo.getConfig("sys/config.ini", "SETTING", "musicState")
	 musicState=tonumber(musicState)
	 mSetState.musicState=musicState
	 local chatState=accountInfo.getConfig("sys/config.ini", "SETTING", "chatState")
        chatState=tonumber(chatState)
	 mSetState.chatState=chatState
	 
	local titleNameStr={
	--音乐
	{name=Language.SET_MUSIC,btnName=Language.SET_MUSIC,state=musicState,tag=1},
	--聊天提示
	{name=Language.SET_CHATTIP,btnName=Language.SET_MUSIC,state=chatState,tag=2},
	--公告
	{name=Language.CHAT_TITLE5,btnName=Language.IDS_OPEN1,tag=3},
	--帮助
	{name=Language.SET_HELP,btnName=Language.IDS_OPEN1,tag=4},
	--设置通知
	{name=Language.SET_NOTICE,btnName=Language.IDS_SETING,tag=5},
	
	}
	
	local mMobileType ,mGameType,mRetailID=accountInfo. readMoble()
	local retailTable=nil
	
--	if mRetailID ~= 0004 then
--		--输入兑换礼品CD-KEY
--		retailTable = {name=Language.SET_CDKEY,btnName=Language.IDS_INPUT,tag=6},
--		ZyTable.push_back(titleNameStr,retailTable)
--	end
	
	
	if mRetailID=="0001"  or  mRetailID=="0036"  or  mRetailID=="0037"  then
		--账号设置
		retailTable={name=Language.SET_PERSONAL,btnName=Language.GAME_SET,tag=8}
		ZyTable.push_back(titleNameStr,retailTable)			
	elseif  mRetailID~="0021"  then
		retailTable={name=Language.SET_ACCOUNT,btnName=Language.GAME_SET,tag=7}
		ZyTable.push_back(titleNameStr,retailTable)		
	end

	
	--论坛专区
    	 if  mRetailID=="0001" then
		 local  retailTable={name=Language.IDS_NETDRAGON,btnName=Language.IDS_OPEN1,tag=9}
		 ZyTable.push_back(titleNameStr,retailTable)
	  end
	  
	  --问题反馈
    	 if  mRetailID=="0001" then
		 local  retailTable={name=Language.SET_FEEDBACK,btnName=Language.IDS_OPEN1,tag=10}
		 ZyTable.push_back(titleNameStr,retailTable)
	  end
	  		    	  
	----创建背景
	local bgTexture=IMAGE("common/list_1038.9.png")
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0
	mSetState.Btns={}
	for k, v in pairs(titleNameStr) do
	 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
		listItem:setOpacity(0)
		local itemLayer=createListItem(v,k,bgTexture)
		listItem:addChildItem(itemLayer, layout)
		mList:addListItem(listItem, false)
	end
end;

---
function  createListItem(info,index,bgTexture)
	local layer=CCLayer:create()
	local boxSize=SZ(mList:getContentSize().width,mList:getRowHeight())
	local bgSprite=createItemBg(bgTexture,boxSize.width,boxSize.height)
	bgSprite:setPosition(PT(0,0))
	layer:addChild(bgSprite,0)
	local startX=(pWinSize.width-boxSize.width)/2
	local titleLabel=CCLabelTTF:create(info.name,FONT_NAME,FONT_DEF_SIZE)
	titleLabel:setAnchorPoint(PT(0,0.5))
	titleLabel:setPosition(PT(startX,boxSize.height/2))
	layer:addChild(titleLabel,0)	
	
----按钮
	local nameStr=info.btnName
	if info.state then
		nameStr=stateStr[info.state]
	end
	 local actionBtn=ZyButton:new("button/list_1039.png",nil,nil,nameStr)
	 mSetState.Btns[index]=actionBtn
	 actionBtn:setAnchorPoint(PT(0,0))
	 actionBtn:setPosition(PT(boxSize.width*0.9-actionBtn:getContentSize().width,
	 							boxSize.height/2-actionBtn:getContentSize().height/2))
	 actionBtn:addto(layer,0)
	 actionBtn:setTag(info.tag)
	 actionBtn:registerScriptHandler(buttonActions) 
	 return layer
end;


function buttonActions(node)
	local tag=node:getTag()
	if tag==1 then
		 musicAction()
	elseif  tag==2 then
		chatAction()
	elseif tag==3 then
		activityAction()
	elseif tag==4 then
		 helpAction()
	elseif tag==5 then
		ZyToast.show(mScene,Language.IDS_NOFUNCTION)
	elseif tag==6 then
		ZyToast.show(mScene,Language.IDS_NOFUNCTION)
	elseif tag==7 then
		ChangPassword.pushScene()
	elseif tag==8 then
		netdragonCenter()
	elseif tag==9 then
		gotoBBS()
	elseif tag==10 then
		netdragonFAQ()
	end
end;


function  gotoBBS()
    	local nType = ScutUtility.ScutUtils:GetPlatformType();
	if nType == ScutUtility.ptAndroid then
        	channelEngine.command("openForum")	
	end
end;



function netdragonCenter()
    	local nType = ScutUtility.ScutUtils:GetPlatformType();
	if nType == ScutUtility.ptAndroid then
        		channelEngine.enterUserCenter()
	else
        		LanScenes.getLogin91Sdk():showMessageBox()
	end
end;

function netdragonFAQ()
    	local nType = ScutUtility.ScutUtils:GetPlatformType();
	if nType == ScutUtility.ptAndroid then
        	channelEngine.sendSeed("",1)
	else
        	LanScenes.getLogin91Sdk():LoginEnv()
	end
end;

--音乐设置
function musicAction()
	if  mSetState.musicState==1 then
		mSetState.musicState=0
		accountInfo.saveConfig("sys/config.ini", "SETTING", "musicState"  , mSetState.musicState)
		 resetMusicInit() 	
	else	 
		mSetState.musicState=1	
		accountInfo.saveConfig("sys/config.ini", "SETTING", "musicState"  , mSetState.musicState)
		local index=EnumMusicType.bgMusic
		resetMusicInit()
	    	playMusic(index)
	end	
	if mSetState.Btns[1] then
		mSetState.Btns[1]:setString(stateStr[mSetState.musicState])
	end
end;


--聊天提示
function chatAction()
	if  mSetState.chatState==1 then
		mSetState.chatState=0	 
	else		 
		mSetState.chatState=1	
	end	
	if mSetState.Btns[2] then
	mSetState.Btns[2]:setString(stateStr[mSetState.chatState])
	end
	accountInfo.saveConfig("sys/config.ini", "SETTING", "chatState"  , mSetState.chatState)
end;

--打开活动
function activityAction()
	actionLayer.Action9202(mScene,nil,1,20)	
end;

--打开帮助
function helpAction()
	releaseSettingLayer()
	createHelpLayer()
end;

--打开通知设置
function noticeAction()
	releaseSettingLayer()
	createNoticeLayer()
end;

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

--打开输入框
function  createInputLayer()
	releaseInputLayer()
	local layer=CCLayer:create()
	mInputLayer=layer
	mLayer:addChild(layer,2)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
-------
	for k=1 ,2 do
		local pingbiBtn=UIHelper.createActionRect(pWinSize)
		pingbiBtn:setPosition(PT(0,0))
		layer:addChild(pingbiBtn,0)
	end
	local bgSize=SZ(pWinSize.width*0.8,pWinSize.height*0.25)
	local bgSprite=CCSprite:create(P("common/list_1016.9.png"))
	bgSprite:setScaleX(bgSize.width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(bgSize.height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0.5,0.5))
	bgSprite:setPosition(PT(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(bgSprite,0)
	
	local startX=pWinSize.width/2-bgSize.width*0.45
	local startY=pWinSize.height/2+bgSize.height*0.48
	--x按钮
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width/2+bgSize.width*0.48-closeBtn:getContentSize().width,
							startY-closeBtn:getContentSize().height))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(releaseInputLayer)
	
	--标题
	local topTip=CCLabelTTF:create(Language.SET_CDTILE,FONT_NAME,FONT_DEF_SIZE)
	topTip:setAnchorPoint(PT(0.5,0))
	topTip:setPosition(PT(pWinSize.width/2,startY-topTip:getContentSize().height*1.5))
	layer:addChild(topTip,0)	
	--提示框
	local titleTip=CCLabelTTF:create(Language.SET_INPUTTITLE,FONT_NAME,FONT_SM_SIZE)
	titleTip:setAnchorPoint(PT(0,0))
	titleTip:setPosition(PT(startX,topTip:getPosition().y-titleTip:getContentSize().height*2))
	layer:addChild(titleTip,0)	
	--输入框
	local editSize=SZ(bgSize.width*0.8, titleTip:getContentSize().height*1.5)
	local startY=pWinSize.height-titleTip:getPosition().y+editSize.height
	mInputEdit= CScutEdit:new();
	mInputEdit:init(false, false)
	mInputEdit:setRect(CCRect(pWinSize.width/2-bgSize.width*0.4,startY,editSize.width,editSize.height))
	--取消 确定按钮
	local cancelBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.IDS_CANCEL)
	cancelBtn:setAnchorPoint(PT(0,0))
	cancelBtn:setPosition(PT(startX,
								pWinSize.height/2-bgSize.height/2+SY(8)))
	cancelBtn:addto(layer,0)
	cancelBtn:registerScriptHandler(releaseInputLayer)
	
	local sureBtn=ZyButton:new("button/list_1039.png",nil,nil,
						Language.IDS_SURE)
	sureBtn:setAnchorPoint(PT(0,0))
	sureBtn:setPosition(PT(pWinSize.width/2+bgSize.width*0.45-cancelBtn:getContentSize().width,
								cancelBtn:getPosition().y))
	sureBtn:addto(layer,0)
	sureBtn:registerScriptHandler(bindingAction)

end;

function bindingAction()
	if not isClick then
	isClick=true
	end
	ZyToast.show(mScene,Language.IDS_NOFUNCTION)
end;

--释放设置通知层
function  releaseNoticeLayer()
	mTabBar=nil
	if mNoticeLayer then
		mNoticeLayer:getParent():removeChild(mNoticeLayer,true)
		mNoticeLayer=nil
	end
end;

--创建设置通知层
function  createNoticeLayer()
	 releaseHelpLayer()
	local layer=CCLayer:create()
	mNoticeLayer=layer
	mLayer:addChild(layer,1)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))


	local bgLayer=createContentLayer(1)
	bgLayer:setPosition(PT(0,0))
	layer:addChild(bgLayer,0)
	
	--local tilteLabel=CCLabelTTF:create(Language.SET_NOTICE,FONT_NAME,FONT_BIG_SIZE)
	local tilteLabel=CCSprite:create(P("title/list_1103.png"))
	tilteLabel:setAnchorPoint(PT(0.5,0))
	layer:addChild(tilteLabel,0)
	tilteLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.96-tilteLabel:getContentSize().height))
	
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width*1.2,
							pWinSize.height-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(backAction)
	
	
	  ---LIST 控件
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.7)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=tilteLabel:getPosition().y-listSize.height-SY(5)
	local mListRowHeight=pWinSize.height*0.2
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setTouchEnabled(true)
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	layer:addChild(list,1)
	mList=list
	--从本地读取状态
	mPhyChoiceIndex=tonumber(accountInfo.getConfig("sys/config.ini", "SETTING", "phyNotice"))
	mBeAttackIndex=tonumber(accountInfo.getConfig("sys/config.ini", "SETTING", "attackNotice"))
	local titleTable={
	{name=Language.SET_MAXPHY,type=mPhyChoiceIndex},
	{name=Language.SET_BEATTACK,type=mBeAttackIndex},
	}
	local layout = CxLayout()
	layout.val_x.t = ABS_WITH_PIXEL
	layout.val_y.t = ABS_WITH_PIXEL
	layout.val_x.val.pixel_val = 0
	layout.val_y.val.pixel_val = 0
	local mBgTecture=IMAGE("common/list_5001.9.png")
	selecetBtnTable={}
	for k, v in pairs(titleTable) do
	 	local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
		listItem:setOpacity(0)
		local itemLayer=createSetItem(v,k,mBgTecture)
		listItem:addChildItem(itemLayer, layout)
		mList:addListItem(listItem, false)
	end
end;

---设置界面一个项目
function  createSetItem(info,index,bgTexture)
	local layer=CCLayer:create()
	local boxSize=SZ(mList:getContentSize().width,mList:getRowHeight())
	local bgSprite=createItemBg(bgTexture,boxSize.width,boxSize.height)
	bgSprite:setPosition(PT(0,0))
	layer:addChild(bgSprite,0)
	local startY=boxSize.height*0.9
	--标题
	local titleLabel=CCLabelTTF:create(info.name,FONT_NAME,FONT_DEF_SIZE)
	titleLabel:setAnchorPoint(PT(0.5,0))
	titleLabel:setPosition(PT(boxSize.width/2,startY-titleLabel:getContentSize().height))
	layer:addChild(titleLabel,0)

	local startX=(pWinSize.width-boxSize.width)/2
	local rowH=boxSize.height*0.3
	local colW=boxSize.width*0.5
	startY=titleLabel:getPosition().y-boxSize.height*0.2
	local itemTable={
	{name=Language.SET_NOTICE1,fun=allNoticeAction},
	{name=Language.SET_NOTICE2,fun=dailyNoticeAction},
	{name=Language.SET_NOTICE3,fun=weekNoticeAction},
	{name=Language.SET_NOTICE4,fun=noNoticeAction},
	}	
	selecetBtnTable[index]={}
	if not info.type or info.type==0 then
			info.type=1
	end
------- 创建不同的选择框
	local btns={}
	for  n , m in  pairs(itemTable) do
		local choiceBtn=ZyButton:new("button/list_1044.png","button/list_1045.png")
		choiceBtn:setAnchorPoint(PT(0,0))
		local posX=startX+((n-1)%2)*colW
		local posY=startY-math.floor((n-1)/2)*rowH
		choiceBtn:setPosition(PT(posX,posY-choiceBtn:getContentSize().height/2))
		choiceBtn:addto(layer,0)
		choiceBtn:setTag(index)
		btns[n]={}
		btns[n].choiceBtn=choiceBtn
		if n==info.type then
			choiceBtn:selected()
		end
		if m.fun then
			choiceBtn:registerScriptHandler(m.fun)
		end
		local nameLabel=CCLabelTTF:create(m.name,FONT_NAME,FONT_DEF_SIZE)
		nameLabel:setAnchorPoint(PT(0,0.5))
		nameLabel:setPosition(PT(choiceBtn:getPosition().x+choiceBtn:getContentSize().width*1.5,
								choiceBtn:getPosition().y+choiceBtn:getContentSize().height/2))
		layer:addChild(nameLabel,0)
	end
	selecetBtnTable[index]=btns

	 return layer
end;

---设置界面选项按钮
function allNoticeAction(node)
local tag=node:getTag()
local key=1
local btnsTable=selecetBtnTable[tag]
for k, v in pairs(btnsTable) do
	v.choiceBtn:unselected()
	if k==key then
	v.choiceBtn:selected()
	end
end
 saveLoclData(tag,key)
end;

function dailyNoticeAction(node)
local tag=node:getTag()
local key=2
local btnsTable=selecetBtnTable[tag]
for k, v in pairs(btnsTable) do
	v.choiceBtn:unselected()
	if k==key then
	v.choiceBtn:selected()
	end
end
 saveLoclData(tag,key)
end;

function weekNoticeAction(node)
local tag=node:getTag()
local key=3
local btnsTable=selecetBtnTable[tag]
for k, v in pairs(btnsTable) do
	v.choiceBtn:unselected()
	if k==key then
	v.choiceBtn:selected()
	end
end
 saveLoclData(tag,key)
end;
--
function noNoticeAction(node)
local tag=node:getTag()
local key=4
local btnsTable=selecetBtnTable[tag]
for k, v in pairs(btnsTable) do
	v.choiceBtn:unselected()
	if k==key then
	v.choiceBtn:selected()
	end
end
 saveLoclData(tag,key)
end;

--保存设置到本地 
function  saveLoclData(value,key)
	local valueStr="phyNotice"
	if value==2 then
		valueStr="attackNotice"
	end
	accountInfo.saveConfig("sys/config.ini", "SETTING", valueStr , key)
end;

--释放帮助界面
function  releaseHelpLayer()
	mList=nil
	mTabBar=nil
	if mHelperLayer then
		mHelperLayer:getParent():removeChild(mHelperLayer,true)
		mHelperLayer=nil
	end
end;

--创建帮助界面
function  createHelpLayer()
	 releaseHelpLayer()
	local layer=CCLayer:create()
	mHelperLayer=layer
	mScene:addChild(layer,1)
	layer:setAnchorPoint(PT(0,0))
	layer:setPosition(PT(0,0))
	
	local bgLayer=createContentLayer(1)
	bgLayer:setPosition(PT(0,0))
	layer:addChild(bgLayer,0)
	
	--local tilteLabel=CCLabelTTF:create(Language.SET_HELP,FONT_NAME,FONT_BIG_SIZE)
	local tilteLabel=CCSprite:create(P("title/list_1102.png"))
	tilteLabel:setAnchorPoint(PT(0.5,0))
	layer:addChild(tilteLabel,0)
	tilteLabel:setPosition(PT(pWinSize.width/2,pWinSize.height*0.96-tilteLabel:getContentSize().height))
	
	local closeBtn=ZyButton:new("button/list_1046.png")
	closeBtn:setAnchorPoint(PT(0,0))
	closeBtn:setPosition(PT(pWinSize.width-closeBtn:getContentSize().width*1.2,
							pWinSize.height-closeBtn:getContentSize().height*1.2))
	closeBtn:addto(layer,0)
	closeBtn:registerScriptHandler(backAction)
	
	local listSize=SZ(pWinSize.width*0.9,pWinSize.height*0.66)
	local listX=(pWinSize.width-listSize.width)/2
	local listY=tilteLabel:getPosition().y-listSize.height-SY(5)
	local mListRowHeight=listSize.height/3
	local list = ScutCxList:node(mListRowHeight, ccc4(24, 24, 24, 0), listSize)
	list:setAnchorPoint(PT(0,0))
	list:setPosition(PT(listX,listY))
	list:setRecodeNumPerPage(1)
	list:setTouchEnabled(true)
	layer:addChild(list,0)
	mList=list
	refreshHelpList()
end;

--刷新帮助列表
function  refreshHelpList()
	if mList then
		mList:clear()
		local layout = CxLayout()
		layout.val_x.t = ABS_WITH_PIXEL
		layout.val_y.t = ABS_WITH_PIXEL
		layout.val_x.val.pixel_val = 0
		layout.val_y.val.pixel_val = 0
		local helpInfo=HelpInfoConfig.getHelpInfo()
		for k, v in pairs(helpInfo) do
			local listItem = ScutCxListItem:itemWithColor(ccc3(24,24,24))
			listItem:setOpacity(0)
			local itemLayer,listHeight=createHelpItem(v)
			listItem:addChildItem(itemLayer, layout)
			mList:setRowHeight(listHeight)
			mList:addListItem(listItem, false)
		end
	end
end;

--创建帮助一个项目
function  createHelpItem(info)
	local layer=CCLayer:create()
	local bgSprite=CCSprite:create(P("common/list_1038.9.png"))
	bgSprite:setAnchorPoint(PT(0,0))
	bgSprite:setScaleX(pWinSize.width*0.9/bgSprite:getContentSize().width)
	bgSprite:setPosition(PT(0,0))
	layer:addChild(bgSprite,0)
	--内容
	local contentStr=info.content
	local contentWidth=pWinSize.width*0.8
	local startX=pWinSize.width*0.05
	local contentLabel=ZyMultiLabel:new(contentStr,contentWidth,FONT_NAME,FONT_SM_SIZE);
	contentLabel:setAnchorPoint(PT(0,0))
	contentLabel:addto(layer,1)
	local itemHeight=contentLabel:getContentSize().height+pWinSize.height*0.06
	contentLabel:setPosition(PT(startX,itemHeight-contentLabel:getContentSize().height-SY(5)))
	bgSprite:setScaleY(itemHeight/bgSprite:getContentSize().height)
	return layer,itemHeight
end;

--返回
function  backAction()
 releaseNoticeLayer()
releaseHelpLayer()
createSettingLayer()
end;

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function  createItemBg(bgTexture,width,height)
	local bgSprite=CCSprite:createWithTexture(bgTexture)
	bgSprite:setScaleX(width/bgSprite:getContentSize().width)
	bgSprite:setScaleY(height/bgSprite:getContentSize().height)
	bgSprite:setAnchorPoint(PT(0,0))
	return bgSprite
end;

function initResource()
	personalInfo=PersonalInfo.getPersonalInfo()
	mSetState={}
end

-- 释放资源
function releaseResource()
	mLayer=nil
	mList=nil
	mTabBar=nil
	mScene=nil
	mHelperLayer=nil
end

-- 网络回调
function networkCallback(pScutScene, lpExternalData)
	local actionID = ZyReader:getActionID()
	if  actionID == 9202 then
		PublicAnnouncementScene.networkCallback(pScutScene, lpExternalData)
		PublicAnnouncementScene.init(mScene)
	end
	
	local userData = ScutRequestParam:getParamData(lpExternalData)
	
	commonCallback.networkCallback(pScutScene, lpExternalData)
end





