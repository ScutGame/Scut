------------------------------------------------------------------
--MainScene.lua
-- Author     : JMChen
-- Version    : 1.0.0.0
-- Date       : 2011-10-15
-- DescriCCPointion:
------------------------------------------------------------------


module("MainScene", package.seeall)

require("scenes.ShopScene")
require("layers.ChatLayer")--聊天
require("layers.BroadcastLayer")
require("layers.HelperLayer")
require("scenes.Ranking")--排行榜
require("scenes.PersonalFileScene")
require("scenes.TaskScene")
require("scenes.SetScene")
require("scenes.MainDesk")
require("scenes.PersonalFileScene")
require("datapool.MainSceneNetCallback");
require("scenes.RollScene")--转盘
require("layers.ActivityNoticLayer")--系统公告

--require("scenes.PersonalFileScene") 
-- 场景中用到的成员变量要在文件头部先申明，并附加必要的说明
-- 成员变量统一以下划线开始，紧接的首字母要小写

local mScene = nil 		-- 场景
local mLayer = nil 		-- 层
local m_personInfo=nilc
local isFirstClick=nil
local menuBtn=nil
local mSetState={}
---聊天
local isClick=nil;
local roomID=nil
--
local  haveWhere=nil;--在哪里
local mTogoType
---------------公有接口(以下两个函数固定不用改变)--------
--



-- 退出场景
function closeScene()
    haveWhere=nil;--在哪里
	BroadcastLayer.releaseResource()
	releaseResource()

end

--
-------------------------私有接口------------------------
--
-- 初始化资源、成员变量
function initResource()
	m_personInfo=PersonalInfo.getPersonalInfo()
end

-- 释放资源
function releaseResource()
    haveWhere=nil;--在哪里
	BroadcastLayer.releaseResource()
	mScene=nil
	gameCoinText=nil
	mLayer=nil
    ---聊天
    isClick=nil;
    heartbeatExit()
end

--在哪里
function getWherer()
    return haveWhere;
end
-- 创建场景
function initScene(gotype)
 	--releaseResource()
-- 	initResource()
	if mScene then
		return 
	end
    haveWhere=true;--在哪里
	local scene = ScutScene:new()	
	mScene = scene.root
	CCDirector:sharedDirector():replaceScene(mScene)
	-- 注册网络回调
	scene:registerCallback(MainSceneNetCallback.netCallBack)	
--	mScene:registerOnEnter("MainScene.onEnter")
--	mScene:registerOnExit("MainScene.onExit")
	--SlideInLReplaceScene(mScene)

	--mScene:setMultiTouchSupported(true)
	-- 添加背景
	mLayer = CCLayer:create()
    mScene:addChild(mLayer, -2)
	initResource()

	creatBgImage()
	createScene()
	touxiang()
	setjindou()
	
	heartbeat()
	
--	BroadcastLayer.init(mScene)
	if gotype then
		mTogoType=gotype
	end
	--gobackToGameDesk()
	if mTogoType==1 then
		MainHelper.delayExec("MainScene.gotoShop",0.1, mScene)	
	elseif mTogoType==2 then
		gobackToGameDesk()
	end
	mTogoType=nil
end

function time()
	actionLayer.Action100(mScene,false,1)
end
function heartbeat()
--	CCScheduler.scheduleScriCCPointFunc(time,10, false)
end;
function exit()
	LoginScene.releaseResource()
	LoginScene.setFirstLogin()
	LoginScene.init(3)
end;

function topUp()
	TopUpScene.init()
end;



function exitRoomInfo()
	if  roomInfoLayer then
		mScene:removeChild(roomInfoLayer,true)
	end;
end;

----排行榜按钮想要事件
function rankBtnAction()
   Ranking.createScene();
end




function getRoomID()
 return roomID
end;

--商店系统
function toShop()
	ShopScene.createScene()
end;

--任务
function taskBtnAction()
	TaskScene.createScene()
end;

--设置
function setAction()
	local setLayer = SetScene.createScene()
	mScene:addChild(setLayer,1)
end;



function rollBtnAction()
	RollScene.pushScene()
end

function explain(index)
	local tag=nil
	if index~=nil then
		tag=index:getTag()
	end;
	local roomInfoTable=RoomConfig.getRoomInfo()
	
	roomInfoLayer = CCLayer:create()
	mScene:addChild(roomInfoLayer, 0)
	
	local transparentBg=Image.tou_ming;--背景图片
	local menuItem =CCMenuItemImage:itemFromNormalImage(P(transparentBg),P(transparentBg));
	local menuBg=CCMenu:menuWithItem(menuItem);
	menuBg:setContentSize(menuItem:getContentSize());
	menuBg:setAnchorPoint(CCPoint(0,0));
	menuBg:setScaleX(pWinSize.width/menuBg:getContentSize().width*2)
	menuBg:setScaleY(pWinSize.height/menuBg:getContentSize().height*2);
	menuBg:setPosition(CCPoint(0,0));
	roomInfoLayer:addChild(menuBg,0);
	
	--背景
	local detailBg= CCSprite:create(P("common/panle_1014_1.png"));
	detailBg:setAnchorPoint(CCPoint(0,0))
	detailBg:setPosition(CCPoint((pWinSize.width-detailBg:getContentSize().width)/2,(pWinSize.height-detailBg:getContentSize().height)/2))
	roomInfoLayer:addChild(detailBg,0)
	
	local roomInfo=roomInfoTable[tag].content
		
	--房间说明
	local roomInfoLabel=ZyMultiLabel:new(roomInfo,detailBg:getContentSize().width*0.8,FONT_NAME,FONT_SM_SIZE,nil,nil,true);
	roomInfoLabel:setAnchorPoint(CCPoint(0,0.5))
	roomInfoLabel:setPosition(CCPoint(detailBg:getPosition().x+(detailBg:getContentSize().width-roomInfoLabel:getContentSize().width)/2,
		detailBg:getPosition().y+(detailBg:getContentSize().height-roomInfoLabel:getContentSize().height)/2))
	roomInfoLabel:addto(roomInfoLayer,1)
	
	--离开按钮
	local exitBtn=ZyButton:new("button/panle_1014_3.png",nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
	exitBtn:setAnchorPoint(CCPoint(0,0))
	exitBtn:setPosition(CCPoint(detailBg:getPosition().x+detailBg:getContentSize().width-exitBtn:getContentSize().width-SY(15),
		detailBg:getPosition().y+detailBg:getContentSize().height-exitBtn:getContentSize().height-SY(15)))
	exitBtn:registerScriptTapHandler(MainScene.exitRoomInfo)
	exitBtn:addto(roomInfoLayer)
end;

function heartbeatExit()
--	CCScheduler.unscheduleScriCCPointEntry(time)
end
--进入房间
function gotoRoom(node)
	local tag=node
	if m_personInfo._roomTabel[tag] then
		roomID=m_personInfo._roomTabel[tag].RoomId
		actionLayer.Action2001(mScene,nil,roomID)
	end
end;
function gobackToGameDesk()
	local layer=CCLayer:create()
	--
	local sprite=CCSprite:create(P("common/black.png"))
	sprite:setScaleX(pWinSize.width/sprite:getContentSize().width)
	sprite:setScaleY(pWinSize.height/sprite:getContentSize().height)
	sprite:setAnchorPoint(CCPoint(0.5,0.5))
	sprite:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(sprite,0)
	local actionBtn=UIHelper.createActionRect(pWinSize)
	actionBtn:setPosition(CCPoint(0,0))
	layer:addChild(actionBtn,0)
	
	local label=CCLabelTTF:create(Language.COMEBACK_TIP,FONT_NAME,FONT_DEF_SIZE)
	label:setAnchorPoint(CCPoint(0.5,0.5))
	label:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	layer:addChild(label,0)
	mScene:addChild(layer,5)
	
	roomID=m_personInfo._RoomId
	actionLayer.Action2001(mScene,nil,roomID)	
end;


function setGotoType(value)
	mTogoType=value
end;

function gotoShop()
	ShopScene.createScene(2)
end;

--通用底图
function creatBgImage()
	local m_bgImage= CCSprite:create(P(Image.image_mainscene));
	m_bgImage:setScaleX(pWinSize.width/m_bgImage:getContentSize().width)
	m_bgImage:setScaleY(pWinSize.height/m_bgImage:getContentSize().height)
	m_bgImage:setAnchorPoint(CCPoint(0.5,0.5))
	m_bgImage:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
	mLayer:addChild(m_bgImage,-2)
end;

function touxiang()
	--头像
	newhead=PersonalInfo.getPersonalInfo()._HeadIcon
	local Head=string.format("headImg/%s.png",newhead)
	local myHead= CCSprite:create(P(Head));
	myHead:setAnchorPoint(CCPoint(0.5,0.5))
	local pos=mMyHead:getPosition()
	myHead:setPosition(pos)			
	mMyHead:getParent():addChild(myHead,0)
	mMyHead:getParent():removeChild(mMyHead,true)
	mMyHead=myHead
end;

--个人档案
function toPersonalFile()
	PersonalFileScene.initResource()
	PersonalFileScene.createScene()
end;

function gotoHelperLayer()
	HelperLayer.initScene(mScene)
end;
--游戏大厅
function createScene()	
	if m_personInfo._Pid~=nil then
		--头像背景框
		local headBg=CCSprite:create(P("common/panel_1001_6.png"))
		headBg:setAnchorPoint(CCPoint(0,0))
		headBg:setPosition(CCPoint(0,pWinSize.height-headBg:getContentSize().height))
		mLayer:addChild(headBg)
	
		local myHead=string.format("headImg/%s.png",m_personInfo._HeadIcon)
		local headSprite=CCSprite:create(P(myHead))
		mMyHead=headSprite
		headSprite:setAnchorPoint(CCPoint(0.5,0.5))
		headSprite:setPosition(CCPoint(headBg:getContentSize().width/2,headBg:getContentSize().height/2))
		headBg:addChild(headSprite)		
		
		local actionBtn=UIHelper.createActionRect(headBg:getContentSize(),toPersonalFile)
		actionBtn:setPosition(CCPoint(0,0))
		headBg:addChild(actionBtn)
	
		--玩家名称
		local nameBg= CCSprite:create(P(Image.image_nameBg));
		nameBg:setAnchorPoint(CCPoint(0,0))
		nameBg:setScaleX(pWinSize.width*0.3/nameBg:getContentSize().width)
		nameBg:setPosition(CCPoint(headBg:getPosition().x+headBg:getContentSize().width,
						pWinSize.height-nameBg:getContentSize().height))
		mLayer:addChild(nameBg,0)
		
	--	local nameText=CCLabelTTF:create(m_personInfo._NickName,FONT_NAME,FONT_SM_SIZE);
		local nameText=CCLabelTTF:create(m_personInfo._NickName,FONT_NAME,FONT_SM_SIZE);
		nameText:setAnchorPoint(CCPoint(0.5,0.5))
		nameText:setPosition(CCPoint(nameBg:getPosition().x+pWinSize.width*0.13,
						nameBg:getPosition().y+nameBg:getContentSize().height/2))
		mLayer:addChild(nameText)
		if nameText:getContentSize().width>pWinSize.width*0.3 then
			nameBg:setScaleX(nameText:getContentSize().width/nameBg:getContentSize().width)	
		end
		--帮助按钮
		local helpSprite=CCSprite:create(P(Image.image_help))
		helpSprite:setAnchorPoint(CCPoint(0,0))
		helpSprite:setPosition(CCPoint(pWinSize.width-helpSprite:getContentSize().width*1.4,
										pWinSize.height-helpSprite:getContentSize().height-SY(7)))
		mLayer:addChild(helpSprite)
		
		local boxSize=CCSize(helpSprite:getContentSize().width*1.6,helpSprite:getContentSize().height*1.6)		
		local actionBtn=UIHelper.createActionRect(boxSize,gotoHelperLayer)
		actionBtn:setPosition(CCPoint(helpSprite:getPosition().x-helpSprite:getContentSize().width*0.3,
										helpSprite:getPosition().y-helpSprite:getContentSize().height*0.3));
		mLayer:addChild(actionBtn,0);
	
		
		--房间背景
		local roomBg= CCSprite:create(P("common/panel_1002_12.png"));
		roomBg:setScaleX(pWinSize.width*0.95/roomBg:getContentSize().width)
		roomBg:setScaleY(pWinSize.height*0.77/roomBg:getContentSize().height)
		roomBg:setAnchorPoint(CCPoint(0.5,0.5))
		roomBg:setPosition(CCPoint(pWinSize.width/2,pWinSize.height/2))
		mLayer:addChild(roomBg,-1)
		
--		--游戏房间
		local roomMultipleTable={"title/panel_1002_4.png","title/panel_1002_5.png","title/panel_1002_6.png","title/panel_1002_7.png"}
		local roomTitleTable={"common/panle_1071_1.png","common/panle_1071_2.png","common/panle_1071_3.png","common/panle_1071_4.png"}
		local inTable={"common/panle_1073.png","common/panle_1074.png","common/panle_1075.png","common/panle_1076.png"}
		
		if  #m_personInfo._roomTabel>0   then
			local rowH=15
			local col=2
			for k,v in pairs(m_personInfo._roomTabel) do			
				--房间背景
				local roomImg= CCSprite:create(P("common/panel_1002_2.9.png"));
				roomImg:setScaleX(pWinSize.width*0.4/roomImg:getContentSize().width)
				roomImg:setScaleY(pWinSize.height*0.3/roomImg:getContentSize().height)
		  		roomImg:setAnchorPoint(CCPoint(0,0))
		  		local headColW=(k-1)%col*(pWinSize.width*0.4)*1.08
		  		local headRowH=math.floor((k-1)/col)*(pWinSize.height*0.3)*1.05
				roomImg:setPosition(CCPoint(SX(35)+headColW,roomBg:getPosition().y-SY(8)-headRowH))
	  			mLayer:addChild(roomImg,0)
		  		
		  		local roomImg_1=ZyButton:new(Image.image_roomBg)
				roomImg_1:setScaleX(pWinSize.width*0.4/roomImg_1:getContentSize().width)
				roomImg_1:setScaleY(pWinSize.height*0.28/roomImg_1:getContentSize().height)
				roomImg_1:registerScriptTapHandler(gotoRoom)
		  		roomImg_1:setAnchorPoint(CCPoint(0,0))
		  		roomImg_1:setTag(k)
				roomImg_1:setPosition(CCPoint(roomImg:getPosition().x+SX(1),roomImg:getPosition().y+SY(3)))
		  		roomImg_1:addto(mLayer,0)
		  		
		  		--房间倍数
		  		local roomMultipleImg= CCSprite:create(P(roomMultipleTable[k]));
		  		roomMultipleImg:setAnchorPoint(CCPoint(0,0))
				roomMultipleImg:setPosition(CCPoint(roomImg:getPosition().x+SX(5),
					roomImg_1:getPosition().y+pWinSize.height*0.28-roomMultipleImg:getContentSize().height))
		  		mLayer:addChild(roomMultipleImg,0)
		  		
		  		--房间标题
		  		local roomTitleImg= CCSprite:create(P(roomTitleTable[k]));
		  		roomTitleImg:setAnchorPoint(CCPoint(1,0))
				roomTitleImg:setPosition(CCPoint(roomImg:getPosition().x+pWinSize.width*0.39,
					roomImg:getPosition().y+ SY(3)))
		  		mLayer:addChild(roomTitleImg,1)
		  		
		  		local roomTitleImg1= CCSprite:create(P("common/panle_1071.png"));
		  		roomTitleImg1:setAnchorPoint(CCPoint(1,0.5))
				roomTitleImg1:setPosition(CCPoint(roomTitleImg:getPosition().x-roomTitleImg:getContentSize().width-2,
					roomTitleImg:getPosition().y+roomTitleImg:getContentSize().height/2))
		  		mLayer:addChild(roomTitleImg1,1)
		  					  		
		  		--金豆数限制
		  		local minCionText=CCSprite:create(P(inTable[k]));
				minCionText:setAnchorPoint(CCPoint(0.5,0.5))
				minCionText:setPosition(CCPoint(roomImg:getPosition().x+pWinSize.width*0.2,
						roomImg:getPosition().y+pWinSize.height*0.15))
				mLayer:addChild(minCionText)		
				
				--说明按钮
				local explainBtn=ZyButton:new(Image.image_explain,Image.image_explain_1)
				explainBtn:setAnchorPoint(CCPoint(0,0))
				explainBtn:setPosition(CCPoint(roomImg_1:getPosition().x+pWinSize.width*0.36-pWinSize.width*0.1-SX(20),
					roomImg:getPosition().y+explainBtn:getContentSize().height-7))
				explainBtn:registerScriptTapHandler(explain)
				explainBtn:setTag(k)
				explainBtn:addImage("title/button_1006.png")
			--	explainBtn:addto(mLayer)
				
				if k==1 then
					--房间标题
			  		local coinImg= CCSprite:create(P("common/icon_1033.png"));
			  		coinImg:setAnchorPoint(CCPoint(0,0))
					coinImg:setPosition(CCPoint(roomImg:getPosition().x+pWinSize.width*0.4-coinImg:getContentSize().width-SX(2),
						roomImg:getPosition().y+pWinSize.height*0.3-coinImg:getContentSize().height-SY(2)))
			  		mLayer:addChild(coinImg,0)
				end;
				
			end;
		end;

		
		--金豆
		gameCoinImg= CCSprite:create(P("common/icon_1031.png"));
		gameCoinImg:setAnchorPoint(CCPoint(0,0.5))
		gameCoinImg:setPosition(CCPoint(SX(10),SY(10)))
		mLayer:addChild(gameCoinImg,0)
		
		local gameCoinStr=PersonalInfo.getPersonalInfo()._GameCoin
		gameCoinText=CCLabelTTF:create(gameCoinStr,FONT_NAME,FONT_SM_SIZE);
		gameCoinText:setAnchorPoint(CCPoint(0,0.5))
		gameCoinText:setPosition(CCPoint(gameCoinImg:getPosition().x+gameCoinImg:getContentSize().width,
				gameCoinImg:getPosition().y))
		mLayer:addChild(gameCoinText)
	
		--转盘按钮
		local posY=gameCoinText:getPosition().y - SY(10)
		rollBtn=ZyButton:new(Image.image_rollBtn,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
		rollBtn:setAnchorPoint(CCPoint(0,0))
		rollBtn:setPosition(CCPoint(pWinSize.width-rollBtn:getContentSize().width-SX(15),posY))
		rollBtn:registerScriptTapHandler(rollBtnAction)
		rollBtn:addto(mLayer)
		
		--设置按钮
		settingBtn=ZyButton:new(Image.image_settingBtn,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
		settingBtn:setAnchorPoint(CCPoint(0,0))
		settingBtn:setPosition(CCPoint(rollBtn:getPosition().x-settingBtn:getContentSize().width-SX(15),posY))
		settingBtn:registerScriptTapHandler(setAction)
		settingBtn:addto(mLayer)
		
		--排行按钮
		rankBtn=ZyButton:new(Image.image_rankBtn,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
		rankBtn:setAnchorPoint(CCPoint(0,0))
		rankBtn:setPosition(CCPoint(settingBtn:getPosition().x-rankBtn:getContentSize().width-SX(15),posY))
		rankBtn:registerScriptTapHandler(rankBtnAction)
		rankBtn:addto(mLayer)
		
		--任务按钮
		taskBtn=ZyButton:new(Image.image_taskBtn,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
		taskBtn:setAnchorPoint(CCPoint(0,0))
		taskBtn:setPosition(CCPoint(rankBtn:getPosition().x-taskBtn:getContentSize().width-SX(15),posY))
		taskBtn:registerScriptTapHandler(taskBtnAction)
		taskBtn:addto(mLayer)
		
		--商店按钮
		shopBtn=ZyButton:new(Image.image_shopBtn,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
		shopBtn:setAnchorPoint(CCPoint(0,0))
		shopBtn:setPosition(CCPoint(taskBtn:getPosition().x-shopBtn:getContentSize().width-SX(15),posY))
		shopBtn:registerScriptTapHandler(toShop)
		shopBtn:addto(mLayer)
		---公告按钮
		local noticBttn=ZyButton:new(Image.image_icon_1048,nil,nil, nil,FONT_NAME,FONT_SM_SIZE)
		noticBttn:setAnchorPoint(CCPoint(0,0))
		noticBttn:setPosition(CCPoint(shopBtn:getPosition().x-noticBttn:getContentSize().width-SX(15),posY))
		noticBttn:registerScriptTapHandler(noticBtnAction)
		noticBttn:addto(mLayer)
		
		
		isFirstClick=true
	end
end;



function setjindou()
	if mLayer~=nil then
		local jindou=PersonalInfo.getPersonalInfo()._GameCoin
        gameCoinText:setString(jindou)
	end
end;

function onEnter()
 -- local runningScene = CCDirector:sharedDirector():getRunningScene()
	actionLayer.Action1008(mScene,nil)
end

function onExit()
end
---公告按钮想要时间
function noticBtnAction()
    local runningScene = CCDirector:sharedDirector():getRunningScene()
    ActivityNoticLayer.createScene(runningScene);--公告活动
end




--菜单
function menu()
	if isFirstClick then
		menuBg:setIsVisible(true)
		shopBtn:setIsVisible(true)
		taskBtn:setIsVisible(true)
		rankBtn:setIsVisible(true)
		settingBtn:setIsVisible(true)
		rollBtn:setIsVisible(true)	
		isFirstClick=false
	else
		menuBg:setIsVisible(false)
		shopBtn:setIsVisible(false)
		taskBtn:setIsVisible(false)
		rankBtn:setIsVisible(false)
		settingBtn:setIsVisible(false)
		rollBtn:setIsVisible(false)
		
		isFirstClick=true
	end
end;

function closeApp()
	if TopUpScene.mWebVies then
		TopUpScene.mWebVies:close()
		TopUpScene.mWebVies=nil
		return
	end	
	local  mMobileType ,mGameType,mRetailID=accountInfo. readMoble()
	local nType = ScutUtility.ScutUtils:GetPlatformType();
	if mRetailID=="0001"  and nType == ScutUtility.CCPointAndroid then
    		channelEngine.command("endGame")		
	else
		if not _isShowMsgBox then
			ZyLoading.releaseAll()
			_isShowMsgBox = true
			local runningScene = CCDirector:sharedDirector():getRunningScene()
			local box = ZyMessageBoxEx:new()
			box:doQuery(runningScene, nil,  Language.IDS_CLOSED_APP, Language.IDS_SURE, Language.IDS_CANCEL,closeAppMessageCallback);	
		end
	end
end
-------------------------------------
function closeAppMessageCallback(clickedButtonIndex, userInfo, tag)
    if clickedButtonIndex == ID_MBOK then
   	 	local runningScene = CCDirector:sharedDirector():getRunningScene()
	--	actionLayer.Action1017(runningScene,false)
		if accountInfo.getRetailId()=="0036" and accountInfo.getMobileType()==5 then
        		channelEngine.sendSeed("0036", 1)
	      end
	      CCDirector:sharedDirector():endToLua();      	 
   end
       _isShowMsgBox = false
end


function netConnectError()
end;

-- 网络回调
function netCallBack(pScutScene, lpExternalData)
	local actionId = ZyReader:getActionID();
	 local userData = ZyRequestParam:getParamData(lpExternalData)
	 MainSceneNetCallback.netCallBack(pScutScene, lpExternalData)
end
